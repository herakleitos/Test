import tensorflow as tf
from models import filehelper
import numpy as np
import os
import tpu_config
TEXT_FEATURE_SIZE = 320
FLAGS = tf.flags.FLAGS
def get_feature_columns(mode):
  feature_columns = []
  feature_columns.append(tf.contrib.layers.real_valued_column(
    column_name="context", dimension=TEXT_FEATURE_SIZE, dtype=tf.int64))
  feature_columns.append(tf.contrib.layers.real_valued_column(
      column_name="context_len", dimension=1, dtype=tf.int64))
  feature_columns.append(tf.contrib.layers.real_valued_column(
      column_name="utterance", dimension=TEXT_FEATURE_SIZE, dtype=tf.int64))
  feature_columns.append(tf.contrib.layers.real_valued_column(
      column_name="utterance_len", dimension=1, dtype=tf.int64))

  if mode == tf.contrib.learn.ModeKeys.TRAIN:
    # During training we have a label feature
    feature_columns.append(tf.contrib.layers.real_valued_column(
      column_name="label", dimension=1, dtype=tf.int64))

  if mode == tf.contrib.learn.ModeKeys.EVAL:
    # During evaluation we have distractors
    for i in range(9):
      feature_columns.append(tf.contrib.layers.real_valued_column(
        column_name="distractor_{}".format(i), dimension=TEXT_FEATURE_SIZE, dtype=tf.int64))
      feature_columns.append(tf.contrib.layers.real_valued_column(
        column_name="distractor_{}_len".format(i), dimension=1, dtype=tf.int64))

  return set(feature_columns)

def parser(record):
    feature_map = tf.parse_single_example(
    record,
    features=tf.contrib.layers.create_feature_spec_for_parsing(
        get_feature_columns(tf.contrib.learn.ModeKeys.TRAIN))
    )
    target = feature_map.pop("label")
    return feature_map, target
def parser_test(record):
    feature_map = tf.parse_single_example(
    record,
    features=tf.contrib.layers.create_feature_spec_for_parsing(
        get_feature_columns(tf.contrib.learn.ModeKeys.EVAL))
    )
      # In evaluation we have 10 classes (utterances).
      # The first one (index 0) is always the correct one
    target = tf.zeros([1], dtype=tf.int64)
    return feature_map, target

def parser_predict(record):
    feature_map = tf.parse_single_example(
    record,
    features=tf.contrib.layers.create_feature_spec_for_parsing(
        get_feature_columns(tf.contrib.learn.ModeKeys.INFER))
    )
    return feature_map

def create_input_fn(mode, input_files,batch_size, num_epochs):
  def input_fn(params):
    batch_size = params["batch_size"]
    features = tf.contrib.layers.create_feature_spec_for_parsing(
        get_feature_columns(mode))

    feature_map = tf.contrib.learn.io.read_batch_features(
        file_pattern=input_files,
        batch_size=batch_size,
        features=features,
        reader=tf.TFRecordReader,
        randomize_input=True,
        num_epochs=num_epochs,
        queue_capacity=200000 + batch_size * 10,
        name="read_batch_features_{}".format(mode))

    # This is an ugly hack because of a current bug in tf.learn
    # During evaluation TF tries to restore the epoch variable which isn't defined during training
    # So we define the variable manually here
    if mode == tf.contrib.learn.ModeKeys.TRAIN:
      tf.get_variable(
        "read_batch_features_eval/file_name_queue/limit_epochs/epochs",
        initializer=tf.constant(0, dtype=tf.int64))

    if mode == tf.contrib.learn.ModeKeys.TRAIN:
      target = feature_map.pop("label")
    else:
      # In evaluation we have 10 classes (utterances).
      # The first one (index 0) is always the correct one
      target = tf.zeros([batch_size,1], dtype=tf.int64)
    return feature_map, target
  return input_fn

def create_input_fn_test(mode, input_files,batch_size, num_epochs):
  def input_fn(params):
    batch_size = params["batch_size"]
    dataset =  tf.data.TFRecordDataset(input_files)
    if mode == tf.contrib.learn.ModeKeys.TRAIN:
      dataset = dataset.map(parser)
      return dataset.cache().repeat().shuffle(
      buffer_size=200000 + batch_size * 10).batch(batch_size, drop_remainder=True)
    else:
      dataset = dataset.map(parser_test)
      return dataset.batch(batch_size, drop_remainder=True)
  return input_fn

def transform_sentence(sequence, vocab_processor):
  """
  Maps a single sentence into the integer vocabulary. Returns a python array.
  """
  return next(vocab_processor.transform([sequence])).tolist()

def create_example_predict(row, vocab):
  """
  Creates a training example for the Ubuntu Dialog Corpus dataset.
  Returnsthe a tensorflow.Example Protocol Buffer object.
  """
  context, utterance = row
  context_transformed = transform_sentence(context, vocab)
  utterance_transformed = transform_sentence(utterance, vocab)
  context_len = len(next(vocab._tokenizer([context])))
  utterance_len = len(next(vocab._tokenizer([utterance])))
  # New Example
  example = tf.train.Example()
  example.features.feature["context"].int64_list.value.extend(context_transformed)
  example.features.feature["utterance"].int64_list.value.extend(utterance_transformed)
  example.features.feature["context_len"].int64_list.value.extend([context_len])
  example.features.feature["utterance_len"].int64_list.value.extend([utterance_len])
  return example

def create_input_fn_predict(vp, batch_size,hparams, num_epochs):
  predict_file_path= os.path.abspath(os.path.join(FLAGS.result_dir, "pre.csv"))
  canned_file_path= os.path.abspath(os.path.join(FLAGS.result_dir, "canned.csv"))
  if tpu_config.use_tpu()==True:
    tf_file_path = "gs://comm100testdata/data/predict.tfrecords"
  else:
    tf_file_path = os.path.abspath(os.path.join(FLAGS.result_dir, "predict.tfrecords"))
  INPUT_CONTEXTS,POTENTIAL_RESPONSES = filehelper.read_predict_file(predict_file_path)
  SET_RESPONSES = filehelper.read_csv_file(canned_file_path)
  writer = tf.python_io.TFRecordWriter(tf_file_path)
  for i, context in enumerate(INPUT_CONTEXTS):
    row1 = [context,POTENTIAL_RESPONSES[i]]
    x1 = create_example_predict(row1,vp)
    writer.write(x1.SerializeToString())
    for response in SET_RESPONSES:
      row2 = [context,response]
      x2 = create_example_predict(row2,vp)
      writer.write(x2.SerializeToString())
  writer.close()
  def input_fn(params):
    batch_size = params["batch_size"]
    dataset =  tf.data.TFRecordDataset(tf_file_path)
    dataset = dataset.map(parser_predict)
    return dataset.batch(batch_size)
  return input_fn
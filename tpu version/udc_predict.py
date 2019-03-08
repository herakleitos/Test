import os
import time
import itertools
import sys
import numpy as np
import tensorflow as tf
import udc_model
import udc_hparams
import udc_metrics
import udc_inputs
from models import filehelper
from models.dual_encoder import dual_encoder_model
from models.helpers import load_vocab
import tpu_config

tf.flags.DEFINE_integer("num_epochs", None, "Number of training Epochs. Defaults to indefinite.")
tf.flags.DEFINE_string("result_dir", "./preData", "Directory containing input data files 'train.tfrecords' and 'validation.tfrecords'")
tf.flags.DEFINE_string("model_dir", "gs://comm100testdata/runs", "Directory to load model checkpoints from")
tf.flags.DEFINE_string("vocab_processor_file", None, "Saved vocabulary processor file")
FLAGS = tf.flags.FLAGS

def tokenizer_fn(iterator):
  return (x.split(" ") for x in iterator)

if tpu_config.use_tpu() ==False:
  FLAGS.model_dir = os.path.abspath("./runs")
  FLAGS.vocab_processor_file = os.path.abspath(os.path.join("./data", 'vocab_processor.bin'))
else:
  FLAGS.vocab_processor_file = "gs://comm100testdata/data/vocab_processor_protocol2.bin"
# Load vocabulary
vp = tf.contrib.learn.preprocessing.VocabularyProcessor.restore(FLAGS.vocab_processor_file)

def get_sentence(array_data):
  sentence = ''
  for item in array_data:
    if item<=0:
      continue
    sentence+= vp.vocabulary_._reverse_mapping[item]
    sentence+=' '
  return sentence
# Load your own data here
def get_features(context, utterance):
  context_matrix = np.array(list(vp.transform([context])))
  utterance_matrix = np.array(list(vp.transform([utterance])))
  context_len = len(context.split(" "))
  utterance_len = len(utterance.split(" "))
  features = {
    "context": tf.convert_to_tensor(context_matrix, dtype=tf.int64),
    "context_len": tf.constant(context_len, shape=[1,1], dtype=tf.int64),
    "utterance": tf.convert_to_tensor(utterance_matrix, dtype=tf.int64),
    "utterance_len": tf.constant(utterance_len, shape=[1,1], dtype=tf.int64),
  }
  return features, None

if __name__ == "__main__":
  hparams = udc_hparams.create_hparams()
  model_fn = udc_model.create_model_fn(hparams, model_impl=dual_encoder_model)
  if tpu_config.use_tpu() ==True:
    tpu_cluster_resolver = tf.contrib.cluster_resolver.TPUClusterResolver(
      'virtueares',
      zone='us-central1-b',
      project='virtue-d2a48'
    )
  else:
    tpu_cluster_resolver=None

  run_config = tf.contrib.tpu.RunConfig(
      cluster=tpu_cluster_resolver,
      model_dir=FLAGS.model_dir,
      session_config=tf.ConfigProto(
          allow_soft_placement=True, log_device_placement=True),
      tpu_config=tf.contrib.tpu.TPUConfig(50, 8),
  ) 
  estimator = tf.contrib.tpu.TPUEstimator(
    model_fn=model_fn,
    config=run_config,
    train_batch_size=hparams.batch_size,
    eval_batch_size=hparams.eval_batch_size,
    predict_batch_size=FLAGS.batch_size,
    use_tpu= tpu_config.use_tpu()
  )

  input_fn_pre = udc_inputs.create_input_fn_predict(
    vp =vp,
    batch_size=hparams.batch_size,
    hparams = hparams,
    num_epochs=FLAGS.num_epochs)

  result_file_path= os.path.abspath(os.path.join(FLAGS.result_dir, "result.txt"))
  predictions = estimator.predict(input_fn = input_fn_pre)
  i =0
  result_dic = {}
  for pred_dict in predictions:
    i+=1
    context_array = pred_dict['context']
    utterance_array =pred_dict['utterance']
    context = get_sentence(context_array)
    utterance = get_sentence(utterance_array)
    prob = pred_dict['probabilities']
    temp_result =[prob,utterance.replace('__eou__','')]
    if context in result_dic.keys():
      result_dic[context].append(temp_result)
    else:
      item = {context:[temp_result]}
      result_dic.update(item)
  result = []
  for ctx in result_dic.keys():
    temp_dialog='{}\n'.format(filehelper.explan_context(ctx))
    i =0
    result_dic[ctx].sort(key=lambda x:x[0],reverse=True)
    for item in result_dic[ctx]:
      i+=1
      temp_dialog+="{}, {} -> {}\n".format(i, item[0],item[1])
    result.append(temp_dialog)
  filehelper.write_lines(result_file_path,result)
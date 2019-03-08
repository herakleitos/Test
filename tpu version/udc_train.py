import os
import time
import itertools
import tensorflow as tf
import udc_model
import udc_hparams
import udc_metrics
import udc_inputs
import tpu_config
from models.dual_encoder import dual_encoder_model

tf.flags.DEFINE_string("model_dir", "gs://comm100testdata/runs", "Directory to store model checkpoints (defaults to ./runs)")
tf.flags.DEFINE_integer("loglevel", 20, "Tensorflow log level")
tf.flags.DEFINE_integer("num_epochs", None, "Number of training Epochs. Defaults to indefinite.")
tf.flags.DEFINE_integer("eval_every", 100, "Evaluate after this many train steps")
tf.flags.DEFINE_string("input_dir", "./data", "Directory containing input data files 'train.tfrecords' and 'validation.tfrecords'")
FLAGS = tf.flags.FLAGS

TIMESTAMP = int(time.time())

tf.logging.set_verbosity(FLAGS.loglevel)

def tokenizer_fn(iterator):
  return (x.split(" ") for x in iterator)

def main(unused_argv):
  use_tpu = tpu_config.use_tpu()
  hparams = udc_hparams.create_hparams()
  if use_tpu:
    TRAIN_FILE = "gs://comm100testdata/data/train.tfrecords"
    VALIDATION_FILE = "gs://comm100testdata/data/validation.tfrecords" 
  else:
    FLAGS.model_dir = os.path.abspath(os.path.join("./runs", str(TIMESTAMP)))
    TRAIN_FILE = os.path.abspath(os.path.join(FLAGS.input_dir, "train.tfrecords"))
    VALIDATION_FILE = os.path.abspath(os.path.join(FLAGS.input_dir, "validation.tfrecords"))

  if use_tpu:
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
  model_fn = udc_model.create_model_fn(
    hparams,
    model_impl=dual_encoder_model)

  estimator = tf.contrib.tpu.TPUEstimator(
    model_fn=model_fn,
    config=run_config,
    train_batch_size=hparams.batch_size,
    eval_batch_size=hparams.eval_batch_size,
    predict_batch_size=hparams.batch_size,
    use_tpu= use_tpu
  )

  input_fn_train = udc_inputs.create_input_fn_test(
    mode=tf.contrib.learn.ModeKeys.TRAIN,
    input_files=[TRAIN_FILE],
    batch_size=hparams.batch_size,
    num_epochs=FLAGS.num_epochs)

  input_fn_eval = udc_inputs.create_input_fn_test(
    mode=tf.contrib.learn.ModeKeys.EVAL,
    input_files=[VALIDATION_FILE],
    batch_size=hparams.eval_batch_size,
    num_epochs=1)

  estimator.train(input_fn=input_fn_train,  max_steps=1000)
  if 100:
    estimator.evaluate(input_fn=input_fn_eval, steps=100)

if __name__ == "__main__":
  tf.app.run()

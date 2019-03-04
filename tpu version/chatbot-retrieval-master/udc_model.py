import tensorflow as tf
import sys
import tpu_config
def get_id_feature(features, key, len_key, max_len):
  ids = features[key]
  ids_len = tf.squeeze(features[len_key], [1])
  ids_len = tf.minimum(ids_len, tf.constant(max_len, dtype=tf.int64))
  return ids, ids_len

def create_train_op(mode,loss, hparams):
  """   train_op = tf.contrib.layers.optimize_loss(
      loss=loss,
      global_step=tf.train.get_global_step(),
      learning_rate=hparams.learning_rate,
      clip_gradients=10.0,
      optimizer=hparams.optimizer) """
  learning_rate = tf.train.exponential_decay(
        hparams.learning_rate,
        tf.train.get_global_step(),
        decay_steps=100000,
        decay_rate=0.96)
  optimizer = tf.train.GradientDescentOptimizer(learning_rate=learning_rate)
  if tpu_config.use_tpu()==True:
    optimizer = tf.contrib.tpu.CrossShardOptimizer(optimizer)
  train_op = optimizer.minimize(loss, tf.train.get_global_step())
  return train_op

def metric_fn(labels, logits):
  accuracy = tf.metrics.accuracy(
      labels=labels, predictions=tf.argmax(logits, axis=1))
  return {"accuracy": accuracy}

def create_model_fn(hparams, model_impl):

  def model_fn(features, labels, mode,params):
    context, context_len = get_id_feature(
        features, "context", "context_len", hparams.max_context_len)

    utterance, utterance_len = get_id_feature(
        features, "utterance", "utterance_len", hparams.max_utterance_len)

    if (labels!=None):
      batch_size = labels.get_shape().as_list()[0]
    else:
      batch_size = hparams.eval_batch_size
        
    if mode == tf.contrib.learn.ModeKeys.TRAIN:
      probs, loss = model_impl(
          hparams,
          mode,
          context,
          context_len,
          utterance,
          utterance_len,
          labels)
      train_op = create_train_op(mode,loss, hparams)
      """ return probs, loss, train_op """
      return tf.contrib.tpu.TPUEstimatorSpec(
        mode=mode,
        loss=loss,
        train_op=train_op)

    if mode == tf.contrib.learn.ModeKeys.INFER:
      probs, loss = model_impl(
          hparams,
          mode,
          context,
          context_len,
          utterance,
          utterance_len,
          None)
      return tf.contrib.tpu.TPUEstimatorSpec(mode, predictions=probs)

    if mode == tf.contrib.learn.ModeKeys.EVAL:
      # We have 10 exampels per record, so we accumulate them
      all_contexts = [context]
      all_context_lens = [context_len]
      all_utterances = [utterance]
      all_utterance_lens = [utterance_len]
      all_targets = [tf.ones([batch_size, 1], dtype=tf.int64)]

      for i in range(9):
        distractor, distractor_len = get_id_feature(features,
            "distractor_{}".format(i),
            "distractor_{}_len".format(i),
            hparams.max_utterance_len)
        all_contexts.append(context)
        all_context_lens.append(context_len)
        all_utterances.append(distractor)
        all_utterance_lens.append(distractor_len)
        all_targets.append(
          tf.zeros([batch_size, 1], dtype=tf.int64)
        )

      probs,losses, logits = model_impl(
          hparams,
          mode,
          tf.concat(all_contexts,0),
          tf.concat(all_context_lens,0),
          tf.concat(all_utterances,0),
          tf.concat(all_utterance_lens,0),
          tf.concat(all_targets,0))

      split_probs = tf.split(probs,10, 0)
      shaped_probs = tf.concat(split_probs,1)
      # Add summaries
      """   tf.summary.histogram("eval_correct_probs_hist", split_probs[0])
      tf.summary.scalar("eval_correct_probs_average", tf.reduce_mean(split_probs[0]))
      tf.summary.histogram("eval_incorrect_probs_hist", split_probs[1])
      tf.summary.scalar("eval_incorrect_probs_average", tf.reduce_mean(split_probs[1])) """
      """ return shaped_probs, loss, None """
      return tf.contrib.tpu.TPUEstimatorSpec(
        mode=mode, loss=losses, eval_metrics=(metric_fn, [tf.concat(all_targets,0), logits]))

  return model_fn

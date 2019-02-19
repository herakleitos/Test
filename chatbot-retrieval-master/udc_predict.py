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
from scripts import filehelper
from models.dual_encoder import dual_encoder_model
from models.helpers import load_vocab

tf.flags.DEFINE_string("model_dir", r'E:/Demo Code/Tensorflow/chatbot-retrieval-master/runs/1550474684', "Directory to load model checkpoints from")
tf.flags.DEFINE_string("vocab_processor_file", "./data/vocab_processor.pickle", "Saved vocabulary processor file")
FLAGS = tf.flags.FLAGS

def tokenizer_fn(iterator):
  return (x.split(" ") for x in iterator)

# Load vocabulary
vp = tf.contrib.learn.preprocessing.VocabularyProcessor.restore(
  FLAGS.vocab_processor_file)

# Load your own data here
""" INPUT_CONTEXT = "Hello Asgeir, this is Norman with Comm100! How may I help you today? __eou__ __eot__ I dont get the verificaton sms __eou__ __eot__ I am sorry I think you might be on the wrong website. We do not send out any sms. __eou__ This is Comm100, a live chat software provider. Our website is www.comm100.com. Is it possible that you confused us with another company? __eou__ __eot__ Your link is in playerauctions website __eou__ __eot__ They are using our chat software and it is likely that their service is not active at the moment __eou__ __eot__ ok __eou__ __eot__ "
POTENTIAL_RESPONSES = ["Please try other means to contact them. Thanks! __eou__ Sorry for the trouble __eou__",
"Could you please elaborate that a bit? __eou__",
"that is correct __eou__ I apologize Zachary, could you please send me a screen shot of where you have the code placed __eou__ Hi Zachary are you still with me? __eou__",
"thats all there should be right - it rang me to chat - I just want tit to  knock to let me know I have a visitor and I can manually invite __eou__",
"Do you have an account with Comm100? __eou__ Are we still connected, Bradley? __eou__",
"Alright thanks again. __eou__"] """

""" INPUT_CONTEXT="hello"
POTENTIAL_RESPONSES =["hello","hello, how arr you?","Could you please elaborate that a bit?","Alright thanks again"] """
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
  estimator = tf.contrib.learn.Estimator(model_fn=model_fn, model_dir=FLAGS.model_dir)

  # Ugly hack, seems to be a bug in Tensorflow
  # estimator.predict doesn't work without this line
  estimator._targets_info = tf.zeros([hparams.eval_batch_size, 1], dtype=tf.int64)

  predict_file_path= r'E:\Demo Code\Tensorflow\chatbot-retrieval-master\data\pre.csv'
  result_file_path=r'E:\Demo Code\Tensorflow\chatbot-retrieval-master\predict_result\result.txt'
  INPUT_CONTEXTS,POTENTIAL_RESPONSES = filehelper.read_predict_file(predict_file_path)
  """   INPUT_CONTEXTS = ["Dear Robson Oliveira, thank you for contacting us! How may I help you? __eou__ __eot__ Hi __eou__ i need to acess my account __eou__ __eot__ Could you please share what error message you are getting, when trying to log in? __eou__ __eot__ I'm not getting access __eou__ rsoliveira200*****mail.com __eou__ __eot__ Please use Forget my password option to get password reset. __eou__ You will get an e-mail with the log in details __eou__ __eot__ already I tried, but not enough nothing __eou__ __eot__ Let me get it checked with the technical team and we will get back to you via e-mail at rsoliveira200*****mail.com ASAP __eou__ __eot__ I need urgent access __eou__ __eot__ Have you checked the Spam and junk folders to see if the e-mail has arrived therein __eou__ __eot__ yes __eou__ __eot__ "]
  POTENTIAL_RESPONSES =["Could you please share the login e-mail and the link you are using? __eou__",
  "We will surely get back to you ASAP. __eou__ As your request has been escalated, you will be contacted couple of hours for sure. __eou__",
  "I guess i'll wait for Rob to get back __eou__ that wont work __eou__",
  "i wait for two days....waste all time. __eou__"] """
  result =[]
  i = 0
  for context in INPUT_CONTEXTS:
    i+=1
    j=0
    temp_result='{}\n'.format(filehelper.explan_context(context))
    temp_response =[]
    for r in POTENTIAL_RESPONSES:
      j+=1
      prob = estimator.predict(input_fn=lambda: get_features(context, r))
      for p in list(prob):
        if p >0.6 or i==j:
          score_response=[]
          if i==j:
            score_response=[100,"Setted Response: {}-{}\n".format(r.replace('__eou__','.'),p)]
            temp_response.append(score_response)
          else:
            score_response=[p,"{}-{}\n".format(r.replace('__eou__','.'),p)]
            temp_response.append(score_response)
    temp_response.sort(key=lambda x:x[0],reverse=True)
    for line in temp_response:
      temp_result+=line[1]
      temp_result+='\n'
    temp_result+='\n\n***********************end*****************************\n\n'
    result.append(temp_result)
    print(temp_result)
  filehelper.write_txt_file(result_file_path,result)
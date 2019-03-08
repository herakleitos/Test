import os
import random
from six.moves import urllib
import tarfile
import csv
import filehelper
import nltk
from nltk.stem import SnowballStemmer, WordNetLemmatizer

__author__ = 'rkadlec'

"""
Script for generation of train, test and valid datasets from Ubuntu Corpus 1 on 1 dialogs.
Copyright IBM Corporation 2016
LICENSE: Apache License 2.0  URL: ttp://www.apache.org/licenses/LICENSE-2.0
Contact: Rudolf Kadlec (rudolf_kadlec@cz.ibm.com)
"""

dialog_end_symbol = "__dialog_end__"
end_of_utterance_symbol = "__eou__"
end_of_turn_symbol = "__eot__"



def translate_dialog_to_lists(dialog_filename):
    """
    Translates the dialog to a list of lists of utterances. In the first
    list each item holds subsequent utterances from the same user. The second level
    list holds the individual utterances.
    :param dialog_filename:
    :return:
    """
    # go through the dialog
    first_turn = True
    dialog = []
    same_user_utterances = []
    #last_user = None
    dialog.append(same_user_utterances)
    with open(dialog_filename, 'r',encoding='utf-16') as f:
        for dialog_line in f:
            if len(dialog_line.split(':'))<=1:
                continue
            line_apart= dialog_line.split(']')
            if len(line_apart)<=1:
                continue
            dialog_info = line_apart[1].split(':')
            if len(dialog_info)<=1:
                continue
            user = dialog_info[0].strip()
            line = dialog_info[1].strip()
            if first_turn:
                last_user = user
                first_turn = False
            if last_user != user:
                # user has changed
                same_user_utterances = []
                dialog.append(same_user_utterances)
            same_user_utterances.append(line)
            last_user = user
        dialog.append([dialog_end_symbol])
    return dialog


def get_random_utterances_from_corpus(candidate_dialog_paths,rng,utterances_num=9,min_turn=3,max_turn=20):
    """
    Sample multiple random utterances from the whole corpus.
    :param candidate_dialog_paths:
    :param rng:
    :param utterances_num: number of utterances to generate
    :param min_turn: minimal index of turn that the utterance is selected from
    :return:
    """
    utterances = []
    dialogs_num = len(candidate_dialog_paths)

    for i in range(0,utterances_num):
        # sample random dialog
        dialog_path = candidate_dialog_paths[rng.randint(0,dialogs_num-1)]
        # load the dialog
        dialog = translate_dialog_to_lists(dialog_path)

        # we do not count the last  _dialog_end__ urn
        dialog_len = len(dialog) - 1
        if(dialog_len<min_turn):
            """  print("Dialog {} was shorter than the minimum required lenght {}".format(dialog_path,dialog_len)) """
            continue
        # sample utterance, exclude the last round that is always "dialog end"
        max_ix = min(max_turn, dialog_len) - 1

        # this code deals with corner cases when dialogs are too short
        if min_turn -1 == max_ix:
            turn_index = max_ix
        else:
            turn_index = rng.randint(min_turn,max_ix)

        utterance = singe_user_utterances_to_string(dialog[turn_index])
        utterances.append(utterance)
    return utterances

def singe_user_utterances_to_string(utterances_list):
    """
    Concatenates multiple user's utterances into a single string.
    :param utterances_list:
    :return:
    """
    return " ".join(map(lambda x: x+" "+ end_of_utterance_symbol, utterances_list))

def dialog_turns_to_string(dialog):
    """
    Translates the whole dialog (list of lists) to a string
    :param dialog:
    :return:
    """
    # join utterances
    turns_as_strings = map(singe_user_utterances_to_string,dialog)
    # join turns
    return "".join(map(lambda x : x + " " + end_of_turn_symbol + " ", turns_as_strings))

def create_random_context(dialog,rng,minimum_context_length=2,max_context_length=20):
    """
    Samples random context from a dialog. Contexts are uniformly sampled from the whole dialog.
    :param dialog:
    :param rng:
    :return: context, index of next utterance that follows the context
    """
    # sample dialog context
    #context_turns = rng.randint(minimum_context_length,len(dialog)-1)
    max_len = min(max_context_length, len(dialog)) - 2
    if max_len <= minimum_context_length:
        context_turns = max_len
    else:
        context_turns = rng.randint(minimum_context_length,max_len)
    if context_turns%2 !=0:
        context_turns -=1
    # create string
    return dialog_turns_to_string(dialog[:context_turns]),context_turns



def create_single_dialog_train_example(context_dialog_path, candidate_dialog_paths, rng, positive_probability,
                                       minimum_context_length=2,max_context_length=20):
    """
    Creates a single example for training set.
    :param context_dialog_path:
    :param candidate_dialog_paths:
    :param rng:
    :param positive_probability:
    :return:
    """

    dialog = translate_dialog_to_lists(context_dialog_path)

    context_str, next_utterance_ix = create_random_context(dialog, rng,
                                                           minimum_context_length=minimum_context_length,
                                                           max_context_length=max_context_length)

    if positive_probability > rng.random():
        # use the next utterance as positive example
        response = singe_user_utterances_to_string(dialog[next_utterance_ix])
        label = 1.0
    else:
        response_set = get_random_utterances_from_corpus(candidate_dialog_paths,rng,1,
                                                     min_turn=minimum_context_length+1,
                                                     max_turn=max_context_length)
        if len(response_set)>=1:
            response =response_set[0]
        else:
            response=''
        label = 0.0
    return context_str, response, label


def create_single_dialog_test_example(context_dialog_path, candidate_dialog_paths, rng, distractors_num, max_context_length):
    """
    Creates a single example for testing or validation. Each line contains a context, one positive example and N negative examples.
    :param context_dialog_path:
    :param candidate_dialog_paths:
    :param rng:
    :param distractors_num:
    :return: triple (context, positive response, [negative responses])
    """

    dialog = translate_dialog_to_lists(context_dialog_path)

    context_str, next_utterance_ix = create_random_context(dialog, rng, max_context_length=max_context_length)

    # use the next utterance as positive example
    positive_response = singe_user_utterances_to_string(dialog[next_utterance_ix])

    negative_responses = get_random_utterances_from_corpus(candidate_dialog_paths,rng,distractors_num)
    return context_str, positive_response, negative_responses


def create_examples_train(candidate_dialog_paths, rng, positive_probability=0.5, max_context_length=20):
    """
    Creates single training example.
    :param candidate_dialog_paths:
    :param rng:
    :param positive_probability: probability of selecting positive training example
    :return:
    """
    i = 0
    examples = []
    for context_dialog in candidate_dialog_paths:
        if i % 1000 == 0:
            print(str(i))
        dialog_path = candidate_dialog_paths[i]
        examples.append(create_single_dialog_train_example(dialog_path, candidate_dialog_paths, rng, positive_probability,
                                                           max_context_length=max_context_length))
        i+=1
    #return map(lambda dialog_path : create_single_dialog_train_example(dialog_path, candidate_dialog_paths, rng, positive_probability), candidate_dialog_paths)

def create_examples(candidate_dialog_paths, examples_num, creator_function):
    """
    Creates a list of training examples from a list of dialogs and function that transforms a dialog to an example.
    :param candidate_dialog_paths:
    :param creator_function:
    :return:
    """
    i = 0
    examples = []
    unique_dialogs_num = len(list(candidate_dialog_paths))

    while i < examples_num:
        context_dialog = candidate_dialog_paths[i % unique_dialogs_num]
        # counter for tracking progress
        if i % 1000 == 0:
            print(str(i))
        i+=1

        examples.append(creator_function(context_dialog, candidate_dialog_paths))

    return examples

def convert_csv_with_dialog_paths(csv_file):
    """
    Converts CSV file with comma separated paths to filesystem paths.
    :param csv_file:
    :return:
    """
    def convert_line_to_path(line):
        file, dir = map(lambda x : x.strip(), line.split(","))
        return os.path.join(dir, file)

    return map(convert_line_to_path, csv_file)


def prepare_data_maybe_download(directory):
  """
  Download and unpack dialogs if necessary.
  """
  filename = 'ubuntu_dialogs.tgz'
  url = 'http://cs.mcgill.ca/~jpineau/datasets/ubuntu-corpus-1.0/ubuntu_dialogs.tgz'
  dialogs_path = os.path.join(directory, 'dialogs')

  # test it there are some dialogs in the path
  if not os.path.exists(os.path.join(directory,"10","1.tst")):
    # dialogs are missing
    archive_path = os.path.join(directory,filename)
    if not os.path.exists(archive_path):
        # archive missing, download it
        print("Downloading %s to %s" % (url, archive_path))
        filepath, _ = urllib.request.urlretrieve(url, archive_path)
        print("Successfully downloaded " + filepath)

    # unpack data
    if not os.path.exists(dialogs_path):
          print("Unpacking dialogs ...")
          with tarfile.open(archive_path) as tar:
                tar.extractall(path=directory)
          print("Archive unpacked.")

    return

def remove_no_valid_file(folder):
    root = r'E:\Demo Code\Tensorflow\ubuntu-ranking-dataset-creator-master\rawdata'
    dialog_paths =  filehelper.get_files_path(os.path.join(root,folder))
    for path in dialog_paths:
        if os.path.exists(path):
            dialog = translate_dialog_to_lists(path)
            if len(dialog)-2<3:
                os.remove(path)
                print("file deleted {}".format(path))

def create_predict_context(args):
    rng = random.Random(args.seed)
    dialog = translate_dialog_to_lists(args.dialog_file_path)
    predict_context,context_turn = create_random_context(dialog, rng, max_context_length=args.max_context_length)
    return predict_context
#####################################################################################
# Command line script related code
#####################################################################################

def create_eval_dataset(args, folder,output_file_name):
    rng = random.Random(args.seed)
    # training dataset
    root = args.rowData
    dialog_paths =  filehelper.get_files_path(os.path.join(root,folder))

    data_set = create_examples(dialog_paths,
                                   len(list(dialog_paths)),
                                   lambda context_dialog, candidates : create_single_dialog_test_example(context_dialog, candidates, rng,
                                                                     args.n, args.max_context_length))
    # output the dataset
    w = csv.writer(open(os.path.join(args.output,output_file_name), 'w', encoding='utf-8',newline=''))
    # header
    header = ["Context", "Ground Truth Utterance"]
    header.extend(map(lambda x: "Distractor_{}".format(x), range(args.n)))
    w.writerow(header)

    stemmer = SnowballStemmer("english")
    lemmatizer = WordNetLemmatizer()

    for row in data_set:
        translated_row = [row[0], row[1]]
        translated_row.extend(row[2])
            
        if args.tokenize:
            translated_row = map(nltk.word_tokenize, translated_row)
            if args.stem:
                translated_row = map(lambda sub: map(stemmer.stem, sub), translated_row)
            if args.lemmatize:
                translated_row = map(lambda sub: map(lambda tok: lemmatizer.lemmatize(tok, pos='v'), sub), translated_row)
                    
            translated_row = map(lambda x: " ".join(x), translated_row)
        if len(translated_row)<3:
            continue
        if len(translated_row[0])<1 or len(translated_row[1])<1:
            continue
        w.writerow(translated_row)
    print("Dataset stored in: {}".format(args.output))


def train_cmd(args):
    rng = random.Random(args.seed)
    # training dataset

    """     f = open(r'E:/Demo Code/Tensorflow/ubuntu-ranking-dataset-creator-master/meta/train.csv', 'r')
    dialog_paths = map(lambda path: os.path.join(args.data_root, "dialogs", path), convert_csv_with_dialog_paths(f)) """
    dialog_paths =[]
    row_data_path =os.path.join(args.rowData,'train')
    dialog_paths =  filehelper.get_files_path(row_data_path)
    train_set = create_examples(dialog_paths,
                                args.examples,
                                    lambda context_dialog, candidates :
                                    create_single_dialog_train_example(context_dialog, candidates, rng,
                                                                       args.p, max_context_length=args.max_context_length))

    stemmer = SnowballStemmer("english")
    lemmatizer = WordNetLemmatizer()

    # output the dataset
    w = csv.writer(open(os.path.join(args.output,'train.csv'), 'w',encoding='utf-8',newline=''))
    # header
    w.writerow(["Context", "Utterance", "Label"])
    for row in train_set:
        translated_row = row

        if args.tokenize:
            translated_row = [nltk.word_tokenize(row[i]) for i in [0,1]]

            if args.stem:
                translated_row = map(lambda sub: map(stemmer.stem, sub), translated_row)
            if args.lemmatize:
                translated_row = map(lambda sub: map(lambda tok: lemmatizer.lemmatize(tok, pos='v'), sub), translated_row)

            translated_row = map(lambda x: " ".join(x), translated_row)
            translated_row.append(int(float(row[2])))
        if len(translated_row)<3:
            continue
        if len(translated_row[0])<1 or len(translated_row[1])<1:
            continue
        w.writerow(translated_row)
    print("Train dataset stored in: {}".format(args.output))

def valid_cmd(args):
    create_eval_dataset(args, "valid","valid.csv")

def test_cmd(args):
    create_eval_dataset(args, "test","test.csv")

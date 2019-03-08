import argparse
import utils as create_dataset

parser = argparse.ArgumentParser(formatter_class=argparse.ArgumentDefaultsHelpFormatter,
                                     description="Script that creates train, valid and test set from 1 on 1 dialogs in Ubuntu Corpus. " +
                                                 "The script downloads 1on1 dialogs from internet and then it randomly samples all the datasets with positive and negative examples.")

parser.add_argument('--data_root', default= '.',
                        help='directory where 1on1 dialogs will be downloaded and extracted, the data will be downloaded from cs.mcgill.ca/~jpineau/datasets/ubuntu-corpus-1.0/ubuntu_dialogs.tgz')

parser.add_argument('--seed', type=int, default=1234,
                        help='seed for random number generator')

parser.add_argument('--max_context_length', type=int, default=20,
                        help='maximum number of dialog turns in the context')

parser.add_argument('-o', '--output', default= r'E:\Demo Code\Tensorflow\tpu version\chatbot-retrieval-master\data',
                        help='output csv')

parser.add_argument('--rowData', default= r'E:\Demo Code\Tensorflow\ubuntu-ranking-dataset-creator-master\rawdata',
                        help='output csv')

parser.add_argument('-t', '--tokenize', action='store_true',
                        help='tokenize the output')

parser.add_argument('-l', '--lemmatize', action='store_true',
                        help='lemmatize the output by nltk.stem.WorldNetLemmatizer (applied only when -t flag is present)')

parser.add_argument('-s', '--stem', action='store_true',
                        help='stem the output by nltk.stem.SnowballStemmer (applied only when -t flag is present)')

""" subparsers = parser.add_subparsers(help='sub-command help') """

""" parser = subparsers.add_parser('train', help='trainset generator') """
parser.add_argument('-p', type=float, default=0.5, help='positive example probability')
parser.add_argument('-e', '--examples', type=int, default=1000, help='number of examples to generate')
""" parser_train.set_defaults(func=train_cmd) """

""" parser_test = subparsers.add_parser('test', help='testset generator') """
""" parser.add_argument('-n', type=int, default=9, help='number of distractor examples for each context') """
""" parser_test.set_defaults(func=test_cmd) """

""" parser_valid = subparsers.add_parser('valid', help='validset generator') """
parser.add_argument('-n', type=int, default=9, help='number of distractor examples for each context')
""" parser_valid.set_defaults(func=valid_cmd) """

args = parser.parse_args()

# download and unpack data if necessary
"""     prepare_data_maybe_download(args.data_root) """

# create dataset
create_dataset.train_cmd(args)
create_dataset.test_cmd(args)
create_dataset.valid_cmd(args)
""" create_dataset.remove_no_valid_file("train")
create_dataset.remove_no_valid_file("test")
create_dataset.remove_no_valid_file("valid") """
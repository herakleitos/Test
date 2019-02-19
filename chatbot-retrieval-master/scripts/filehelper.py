import os
import csv
def get_files_path(root):
    filepaths=[]
    def get_files(root):
        for root, dirs, files in os.walk(root):
            if len(files) >0: 
                filepaths.extend(list(map(lambda path: os.path.join(root,path),files)))
            for p  in  dirs:
                get_files(os.path.join(root,p))
    get_files(root)
    return filepaths
def read_predict_file(path):
    questions=[]
    answers=[]
    with open(path,'r',encoding='utf-8-sig') as csvfile:
        reader = csv.reader(csvfile)
        for row in reader:
            question, answer =row
            questions.append(question)
            answers.append(answer)
    return questions,answers
def write_txt_file(file_path, contents):
    with open(file_path,'w',encoding='utf-8') as txtfile:
        for line in contents:
            txtfile.write(line+'\n')
        txtfile.close()
def explan_context(context):
    user ="A say:"
    i =0
    result =''
    for item in context.split('__eot__'):
        if i%2==0:
            user="A say: "
        else:
            user="B say: "
        i+=1
        result+=user
        result  += item.replace('__eou__','.')
        result +='\n'
    return result


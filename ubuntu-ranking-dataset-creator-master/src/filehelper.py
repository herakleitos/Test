import os
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
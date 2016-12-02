import os
import shutil
from sys import argv

path = argv[1]
fileToAddPath = argv[2]

print "Start File Copy From:" + fileToAddPath
print "Start File Copy To:" + path

root_src_dir = fileToAddPath
root_dst_dir = path

for src_dir, dirs, files in os.walk(root_src_dir):
    dst_dir = src_dir.replace(root_src_dir, root_dst_dir)
    if not os.path.exists(dst_dir):
        os.mkdir(dst_dir)
    for file_ in files:
        if not file_.endswith(".meta"):
            print "Copying File: " + file_
            src_file = os.path.join(src_dir, file_)
            dst_file = os.path.join(dst_dir, file_)
            if os.path.exists(dst_file):
                os.remove(dst_file)
            shutil.copy2(src_file, dst_dir)

print "Copying Complete"

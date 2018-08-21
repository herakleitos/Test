using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace DialogflowTest
{
    public static class Myfile
    {
        public static void Write(string path,string fileName,string content)
        {
            string filepath
                = string.Format("{0}{1}", path, fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite))
            {
                fs.Position = fs.Length;
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    writer.Write(content);
                    writer.Write("\r\n");
                    writer.Flush();
                    writer.Dispose();
                }
                fs.Dispose();
            }
        }
        public static string Read(string path,string fileName)
        {
            string filepath
                = string.Format("{0}{1}", path, fileName);
            if (!File.Exists(filepath))
            {
                return string.Empty;
            }
            string content = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                   FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                {
                    content = reader.ReadToEnd();
                }
                fs.Dispose();
            }
            return content;
        }
    }
}

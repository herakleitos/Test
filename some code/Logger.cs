using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common
{
    public static class Logger
    {
        public static void Log(string content)
        {
            string fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
            string folderPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Logs");
            string filePath = System.IO.Path.Combine(folderPath, fileName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string msg = string.Format("{0}->{1} \r\n",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), content);
            FileHelper.Write(filePath, msg);
        }
    }
}

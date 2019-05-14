using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
public class FileHelper
{
    public static void Write(string filepath, string content, bool isOverWrite = false)
    {
        FileMode mode = isOverWrite ? FileMode.Create : FileMode.OpenOrCreate;
        using (FileStream fs = new FileStream(filepath, mode, FileAccess.ReadWrite,
                FileShare.ReadWrite))
        {
            fs.Position = isOverWrite ? 0 : fs.Length;
            using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.Write(content);
                writer.Flush();
                writer.Dispose();
            }
            fs.Dispose();
        }
    }
    public static string Read(string filepath)
    {
        string result = string.Empty;
        using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.ReadWrite))
        {
            fs.Position = 0;
            using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
                reader.Dispose();
            }
            fs.Dispose();
        }
        return result;
    }
}
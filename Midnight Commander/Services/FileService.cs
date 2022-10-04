using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Midnight_Commander.Services
{
    public class FileService
    {
        public static List<string> FileContent(string name)
        {
            List<string> content = new List<string>();
            StreamReader reader = new StreamReader(name);
            while(!reader.EndOfStream)
            {
                content.Add(reader.ReadLine());
            }
            reader.Close();
            return content;
        }
        public static void SaveMe(string path, List<string> content)
        {
            StreamWriter writer = new StreamWriter(path);
            foreach (string item in content)
            {
                writer.WriteLine(item);
            }
            writer.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCrawler
{
    class FileTypes
    {
        List<String> fileList = new List<string>();
        static StreamReader reader;
        
        public List<String> GetFileTypesToDownlaod(String p)
        {
            string[] files = Directory.GetFiles(p);
            foreach (String filePath in files)
            {
                ReadCSVToOne(filePath);
            }
            return fileList;
        }

        private void ReadCSVToOne(String filePath)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            reader = new System.IO.StreamReader(filePath);
            while ((line = reader.ReadLine()) != null)
            {
                String fileName = Path.GetFileName(filePath).ToString().Substring(0, Path.GetFileName(filePath).ToString().Length-4);
                fileList.Add(line+","+fileName);
                counter++;
            }

            reader.Close();
        }
    }
}

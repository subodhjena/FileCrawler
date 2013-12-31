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

        public String GetConnectionString(String p)
        {
            int counter = 0;
            string line;
            String returnVal = "Please Update the app data file.";

            // Read the file and display it line by line.
            reader = new System.IO.StreamReader(p);
            while ((line = reader.ReadLine()) != null)
            {
                String[] connectionString = line.Trim().Split(',');
                if (connectionString[0].ToString() == "ConnectionString")
                {
                    returnVal = connectionString[1];
                }
                counter++;
            }

            reader.Close();
            return returnVal;
        }
        public String GetHostToCrawlString(String p)
        {
            int counter = 0;
            string line;
            String returnVal = "Please Update the app data file.";

            // Read the file and display it line by line.
            reader = new System.IO.StreamReader(p);
            while ((line = reader.ReadLine()) != null)
            {
                String[] connectionString = line.Trim().Split(',');
                if (connectionString[0].ToString() == "WebsiteName")
                {
                    returnVal = connectionString[1];
                }
                counter++;
            }

            reader.Close();
            return returnVal;
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

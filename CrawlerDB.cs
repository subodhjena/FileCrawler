using System;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
namespace FileCrawler
{
    public class CrawlerDB
    {
        public const string DB_CONN_STRING = "data source=SUBODH;initial catalog=DownloadCrawler;Integrated security=true";

        public void SaveFileURLToDB(String hostName, string fileType, string fileUrl)
        {
            using (SqlConnection con = new SqlConnection(DB_CONN_STRING))
            {
                con.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO [DownloadCrawler].[dbo].[CrawledFiles] VALUES(@host, @file, @url, @credt,@crename,@upddt,@updnm)", con))
                    {
                        command.Parameters.Add(new SqlParameter("host", hostName));
                        command.Parameters.Add(new SqlParameter("file", fileType));
                        command.Parameters.Add(new SqlParameter("url", fileUrl));
                        command.Parameters.Add(new SqlParameter("credt", DateTime.Now.ToString()));
                        command.Parameters.Add(new SqlParameter("crename", "Subodh"));
                        command.Parameters.Add(new SqlParameter("upddt", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("updnm", DBNull.Value));
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("**************************************");
                    System.Console.WriteLine("WRITING: ERROR DESCRIPTION");
                    System.Console.WriteLine("  Error Message:" + ex.Message);
                    System.Console.WriteLine("  Stack Trace  :" + ex.StackTrace);
                    System.Console.WriteLine("  Connection String.:" + DB_CONN_STRING);
                    System.Console.WriteLine("**************************************");
                }
            }
        }

    }
}


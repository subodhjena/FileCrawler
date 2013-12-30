using System;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
namespace FileCrawler
{
    public class CrawlerDB
    {
        public const string DB_CONN_STRING = "data source=SUBODH;initial catalog=FileCrawler;Integrated security=true";

        public void SaveFileURLToDB(String tableName, String hostName, string fileType, string fileDescription,string fileUrl)
        {
            using (SqlConnection con = new SqlConnection(DB_CONN_STRING))
            {
                con.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO [FileCrawler].[dbo].[" + tableName + "] VALUES(@hostName,@fileType,@fileDescription,@url,@createdDt,@createdByName,@updatedDt,@updatedByName)", con))
                    {
                        command.Parameters.Add(new SqlParameter("hostName", hostName));
                        command.Parameters.Add(new SqlParameter("fileType", fileType));
                        command.Parameters.Add(new SqlParameter("fileDescription", fileDescription));
                        command.Parameters.Add(new SqlParameter("url", fileUrl));
                        command.Parameters.Add(new SqlParameter("createdDt", DateTime.Now.ToString()));
                        command.Parameters.Add(new SqlParameter("createdByName", "Subodh"));
                        command.Parameters.Add(new SqlParameter("updatedDt", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("updatedByName", DBNull.Value));
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


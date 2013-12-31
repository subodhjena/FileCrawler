using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abot.Crawler;
using Abot.Poco;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace FileCrawler
{
    class Program
    {
        #region "Fields and Object Declaration"
        static String connectionString;
        static String webURL;
        static string appDataPath = @"C:\Users\sjena\Documents\GitHub\FileCrawler\AppData.csv";
        static String fileTypePath = @"C:\Users\sjena\Documents\GitHub\FileCrawler\FileTypes";

        static CrawlerDB crawalerDatabase = new CrawlerDB(connectionString);
        static FileTypes fileTyp = new FileTypes();
        static List<String> filters;
        #endregion

        static void Main(string[] args)
        {
            //Read Configuration from File
            connectionString = fileTyp.GetConnectionString(appDataPath);
            webURL = fileTyp.GetHostToCrawlString(appDataPath);

            //Will Get the FileTypes to Download
            filters = fileTyp.GetFileTypesToDownlaod(fileTypePath);
            //Will use app.config for confguration
            PoliteWebCrawler crawler = new PoliteWebCrawler();

            #region "Crawler Events"
            crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
            crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
            crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;
            #endregion

            CrawlResult result = crawler.Crawl(new Uri(webURL));
            if (result.ErrorOccurred)
                Console.WriteLine("Crawl of {0} completed with error: {1}", result.RootUri.AbsoluteUri, result.ErrorException.ToString());
            else
                Console.WriteLine("Crawl of {0} completed without error.", result.RootUri.AbsoluteUri);
        }

        #region "Crawler Event Delegates"
        static void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri);
        }
        static void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                SaveURLFail(crawledPage.Uri.AbsoluteUri.ToString());
            else
                SaveURLSuccess(crawledPage.Uri.AbsoluteUri.ToString());

            if (string.IsNullOrEmpty(crawledPage.RawContent))
                SaveURLNoContent(crawledPage.Uri.AbsoluteUri.ToString());
        }
        static void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            Console.WriteLine("Did not crawl the links on page {0} due to {1}", crawledPage.Uri.AbsoluteUri, e.DisallowedReason);
            SavePageLinksCrawlDisallowed(crawledPage.Uri.AbsoluteUri.ToString());
        }
        static void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason);
            SavePageCrawlDisallowed(pageToCrawl.Uri.AbsoluteUri);
        }
        #endregion

        //Saving the file links
        private static void SaveURLSuccess(string p)
        {
            Console.WriteLine("Shubh :Crawl of page succeeded {0}", p);
            WriteToDB(p);
        }
        private static void SaveURLFail(string p)
        {
            Console.WriteLine("Shubh :Crawl of page Failed {0}", p);
            WriteToDB(p);
        }
        private static void SavePageLinksCrawlDisallowed(string p)
        {
            Console.WriteLine("Shubh :Page Links Craw lDisallowed {0}", p);
            WriteToDB(p);
        }
        private static void SaveURLNoContent(string p)
        {
            Console.WriteLine("Shubh :Crawl of page had no content {0}", p);
            WriteToDB(p);
        }
        private static void SavePageCrawlDisallowed(string p)
        {
            Console.WriteLine("Shubh :Crawl of page Not allowed {0}", p);
            WriteToDB(p);
        }

        #region "Inserts new url to database"
        private static void WriteToDB(string p)
        {
            try
            {

                foreach (String filter in filters)
                {
                    String[] splitFilters = filter.Split(',');
                    String filterFileType = splitFilters[0];
                    String filterFileDescription = splitFilters[1];
                    String filterFileTable = splitFilters[2];

                    if (p.Trim().EndsWith(filterFileType) && Uri.IsWellFormedUriString(p, UriKind.RelativeOrAbsolute))
                    {
                        crawalerDatabase.SaveFileURLToDB(filterFileTable, webURL, filterFileType, filterFileDescription, p);
                        Console.WriteLine("Wrote :" + p + " to :" + filterFileTable);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("**************************************");
                System.Console.WriteLine("WRITING: ERROR DESCRIPTION");
                System.Console.WriteLine("  Error Message:" + ex.Message);
                System.Console.WriteLine("  Stack Trace  :" + ex.StackTrace);
                System.Console.WriteLine("**************************************");
            }
        }
        #endregion

    }
}
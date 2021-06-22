using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DirectoryCrawler.Logic;
using File = DirectoryCrawler.Logic.File;

namespace DirectoryCrawler.Presentation
{
    class Program
    {
        private static string crawlResult = "No results yet";
        static void Main()
        {
            Console.WriteLine("Bonjour, Welcome to the most advanced calculator. Right?!\n\n");
            
            // This is the thread for crawling the folders. You need to keep the rest of the application responsive
            Thread crawlThread = new Thread(StartCrawl);
            crawlThread.Start();

            while (true)
            {
                Console.WriteLine("I can addition\nGive a the first number");
                int? x = Convert.ToInt32(Console.ReadLine());
                if (x == -1) Console.WriteLine(crawlResult);
                if (x == -2) break;
                Console.WriteLine("What is the second number?");
                int y = Convert.ToInt32(Console.ReadLine());
                if (x == -1) Console.WriteLine(crawlResult);
                if (y == -2) break;
                Console.WriteLine($"Answer: {x + y}");
            }
        }

        private static void StartCrawl()
        {
            string currentFolder = "C:/Users/woute/Desktop";
            Crawler crawler = new Crawler();
            crawler.GetDirectorieContent(currentFolder, out List<File> files, out List<string> folders);
            /*
            foreach (File file in files)
            {
                Console.WriteLine($"File location: {file.ToString()}");
            }*/
            crawlResult = $"Crawling report:\n  Accessed {folders.Count} folders\n  Detected {files.Count} files";
        }
        
        // If you want to crawl the whole device call this function.
        // It will return the root folder (usually the "C:" disk)
        private static string GetRootFolder()
        {
            string location = Directory.GetCurrentDirectory();
            location = location.Replace("\\", "/");
            return location.Split("/")[0];
        }
    }
}
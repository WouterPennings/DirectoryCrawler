﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using DirectoryCrawler.Logic;
using WebSocketSharp;
using File = DirectoryCrawler.Logic.File;

namespace DirectoryCrawler.Presentation
{
    class Program
    {
        private static string crawlResult = "No results yet";
        private static List<File> _files = new List<File>();
        private static List<string> _folders = new List<string>();
        private static WebSocket ws;
        static void Main()
        {
            Console.WriteLine("Bonjour, Welcome to the most advanced calculator. Right?!\n\n");
            ws = new WebSocket("ws://localhost:3000");
            ws.Connect();
            // This is the thread for crawling the folders. You need to keep the rest of the application responsive
            Thread crawlThread = new Thread(StartCrawl);
            crawlThread.Start();

            while (true)
            {
                Console.WriteLine("I can addition\nGive a the first number");
                int x = Convert.ToInt32(Console.ReadLine());
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
            string currentFolder = "C:/Users/woute/Desktop/restapi";
            Crawler crawler = new Crawler();
            crawler.GetDirectorieContent(currentFolder, out _files, out _folders);
            /*
            foreach (File file in files)
            {
                Console.WriteLine($"File location: {file.ToString()}");
            }*/
            crawlResult = $"Crawling report:  Accessed {_folders.Count} folders // Detected {_files.Count} files";
            
            List<string> properties = _files.Select(o => o.ToString()).ToList();
            ws.Send(JsonSerializer.Serialize(properties));
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
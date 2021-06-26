using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DirectoryCrawler.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DirectoryCrawler.Presentation
{
    class Program
    {
        private static string crawlResult = "No results yet";
        private static List<CFile> _files = new List<CFile>();
        private static List<string> _folders = new List<string>();
        private static WebSocket ws;
        private static Client client;
        
        static void Main()
        {
            bool run = true;
            Crawler crawler = new Crawler(true);
            Console.Write($"\n\n\n\n\n\n{crawler.ToString()}> ");
            while (run)
            {
                string input = Console.ReadLine();
                List<string> list = CommonFunctions.SeperateCommands(input);
                try
                {
                    if (list[0].Length > 0)
                    {
                        switch (list[0])
                        {
                            case "exit":
                                run = false;
                                break;
                            case "cd":
                                if (list.Count > 2) Console.WriteLine("Command 'cd' only takes one parameter.");
                                else if (list.Count > 1) Console.WriteLine("\n" + crawler.ChangeDirectory(list[1]));
                                else Console.WriteLine("No parameter was given.");
                                break;
                            case "dir":
                                if (list.Count == 1)
                                {
                                    crawler.DirectorieContent(out List<CDirectory> dirs, out List<CFile> files);
                                    Console.WriteLine($"\ndirectory of {crawler.ToString()}");
                                    foreach (CDirectory dir in dirs)   Console.WriteLine($"<dir>     {dir.GetName()}");
                                    foreach (CFile file in files)      Console.WriteLine($"<file>    {file.GetName()}");
                                }
                                else Console.WriteLine("'dir' does not take any parameter.");
                                break;
                            default:
                                Console.WriteLine($"\n'{list[0]}' is not a command.");
                                break;
                        }
                    }
                }
                catch (ChangeDirectoryException exc)
                {
                    Console.WriteLine($"\nException: {exc.Message}");
                    Console.WriteLine($"ExceptionCode: {exc.ExceptionCode}\n");
                }
                Console.Write($"{crawler.ToString()}> ");
            }
        }

        private static void StartCrawl()
        {
            string currentFolder = "C:/Users/woute/Desktop/restapi";
            Crawler crawler = new Crawler();
            crawler.CrawlDirectorie(currentFolder, out _files, out _folders);
            crawlResult = $"Crawling report:  Accessed {_folders.Count} folders // Detected {_files.Count} files";
            
            client.Files = _files.Select(o => o.ToString()).ToList();
            ws.Send(JsonSerializer.Serialize(client));
        }
        
        // If you want to crawl the whole device call this function.
        // It will return the root folder (usually the "C:" disk)
        private static string GetRootFolder()
        {
            string location = Directory.GetCurrentDirectory();
            location = location.Replace("\\", "/");
            return location.Split("/")[0];
        }

        private static void Ws_OnMessage(object sender, MessageEventArgs message)
        {
            JObject json = JObject.Parse(message.Data);
            Console.WriteLine(json["mes"]);
        }
    }
}
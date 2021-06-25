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
            //Console.WriteLine("Bonjour, Custom FTP protocol");
            //client = JsonConvert.DeserializeObject<Client>(File.ReadAllText(@"../../../Files/info.json"));
            //client?.MaybeSetId();

            Crawler crawler = new Crawler();
            Console.WriteLine($"\n\n\n\n\n\nCurrent Directory: {crawler.ToString()} ->");
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit") break;
                List<string> list = CommonFunctions.SeperateCommands(input);
                try
                {
                    string firstCommand = list[0];
                    if (firstCommand.Length > 0)
                    {
                        switch (firstCommand)
                        {
                            case "cd":
                                Console.WriteLine("\n" + crawler.ChangeDirectory(list[1]));
                                break;
                            case "dir":
                                if (list.Count == 1)
                                {
                                    crawler.DirectorieContent(out List<string> dirs, out List<string> files);
                                    Console.WriteLine($"\ndirectory of {crawler.ToString()}");
                                    foreach (string dir in dirs)   Console.WriteLine($"<dir>     {CommonFunctions.PathToList(dir)[^1]}");
                                    foreach (string file in files) Console.WriteLine($"<file>    {CommonFunctions.PathToList(file)[^1]}");
                                }
                                else Console.WriteLine("'dir' does not take any parameter.");
                                break;
                            default:
                                Console.WriteLine($"\n'{firstCommand}' is not a command.");
                                break;
                        }
                    }
                    Console.WriteLine($"\nCurrent Directory: {crawler.ToString()} ->");
                }
                catch (ChangeDirectoryException exc)
                {
                    Console.WriteLine($"Exception: {exc.Message}");
                    Console.WriteLine($"ExceptionCode: {exc.ExceptionCode}");
                }
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
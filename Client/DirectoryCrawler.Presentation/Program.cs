using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
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
            Console.WriteLine("Bonjour, Custom FTP protocol");
            client = JsonConvert.DeserializeObject<Client>(File.ReadAllText(@"../../../Files/info.json"));
            client?.MaybeSetId();

            try
            {
                Crawler crawler = new Crawler("C:\\Users\\woute\\Desktop\\restapi");
                Console.WriteLine("Current Dir: " + crawler.ChangeDirectory("."));
                Console.WriteLine("Current Dir: " + crawler.ChangeDirectory(".."));
                Console.WriteLine("Current Dir: " + crawler.ChangeDirectory("restapi"));
                crawler.DirectorieContent(out List<string> dirs, out List<string> files);
                foreach (string dir in dirs)   Console.WriteLine($"<dir>     {dir}");
                foreach (string file in files) Console.WriteLine($"          {file}");
            }
            catch (ChangeDirectoryException exc)
            {
                Console.WriteLine($"Exception: {exc.Message}");
                Console.WriteLine($"ExceptionCode: {exc.ExceptionCode}");
            }
            
            /*List<string> x = crawler.SubDirectories();
            foreach (string dir in x)
            {
                Console.WriteLine(dir);
            }*/
            
            /*
            string uri = "ws://localhost:3000";
            using (ws = new WebSocket(uri))
            {
                ws = new WebSocket(uri);
                ws.OnMessage += Ws_OnMessage;
                ws.Connect ();
                
                Console.ReadKey();
            }*/
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
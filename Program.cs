using System;
using System.Collections.Generic;
using System.IO;

namespace FolderCrawler
{
    class Program
    {
        static void Main()
        {
            string currentFolder = "C:/Users/woute/Desktop/test";
            Crawler crawler = new Crawler();
            List<string> folders = crawler.GetDirectories(currentFolder, out List<string> files);
            foreach (string file in files)
            {
                Console.WriteLine($"Filename: {file}");
            }
            Console.WriteLine($"============\nAccessed {folders.Count} folders");
            Console.WriteLine($"Detected {files.Count} files");

            File fileNew = new File(files[0]);
            Console.WriteLine(fileNew.ToString());
        }
    }
}

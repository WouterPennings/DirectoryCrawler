using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace DirectoryCrawler.Logic
{
    public class Crawler
    {
        public void GetDirectorieContent(string startDirectory, out List<CFile> files, out List<string> folders)
        {
            List<string> newFolders = new List<string>();
            List<CFile> newFiles = new List<CFile>();
            newFolders.Add(startDirectory);
            newFiles.AddRange(ConvertToFiles(Directory.GetFiles(startDirectory).ToList()));
            foreach (string sub in Directory.GetDirectories(startDirectory))
            {
                newFolders.Add(sub);
                newFolders.AddRange(GetSubDirectoriesContent(sub, out List<CFile> newFiles2));
                newFiles.AddRange(newFiles2);
            }

            files = newFiles;
            folders = newFolders;
        }

        private List<string> GetSubDirectoriesContent(string dir, out List<CFile> files)
        {
            Convert.ToString(5);
            List<string> folders = new List<string>();
            List<CFile> newFiles = new List<CFile>();
            newFiles.AddRange(ConvertToFiles(Directory.GetFiles(dir).ToList()));
            try
            {
                string[] subDirs = Directory.GetDirectories(dir);
                foreach (string sub in subDirs)
                {
                    folders.Add(sub);
                    folders.AddRange(GetSubDirectoriesContent(sub, out List<CFile> newFiles2));
                    newFiles.AddRange(newFiles2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Herres: {e}");
            }

            files = newFiles;
            return folders;
        }

        private List<CFile> ConvertToFiles(List<string> files)
        {
            List<CFile> newFiles = new List<CFile>();
            foreach (string file in files)
            {
                newFiles.Add(new CFile(file));
            }

            return newFiles;
        }
    }
}
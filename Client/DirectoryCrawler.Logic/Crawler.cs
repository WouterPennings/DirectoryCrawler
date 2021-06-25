using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace DirectoryCrawler.Logic
{
    public class Crawler
    {
        private string _currentDirectory;

        public Crawler()
        {
            string location = Directory.GetCurrentDirectory();
            _currentDirectory = location.Replace("\\", "/");
        }

        public Crawler(string directory)
        {
            _currentDirectory = directory;
        }
        
        public void CrawlDirectorie(string startDirectory, out List<CFile> files, out List<string> folders)
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

        public string ChangeDirectory(string parameter)
        {
            if (parameter.Length == 0) throw new ChangeDirectoryException("No parameter was given", 0);
            if (OnlyContainsDot(parameter) && parameter.Length > 1)
            {
                for (int i = 0; i < parameter.Length - 1; i++)
                {
                    _currentDirectory = Directory.GetParent(_currentDirectory).ToString();
                }
            }
            else if(parameter != ".")
            {
                List<string> subDirectories = Directory.GetDirectories(_currentDirectory).ToList();
                string x = subDirectories.Find(dir => dir == $"{_currentDirectory}\\{parameter}");
                if (x != null) _currentDirectory = x;
                else throw new ChangeDirectoryException("Directory given did not exist", 1);
            }
            return _currentDirectory;
        }

        public void DirectorieContent(out List<string> dirs, out List<string> files)
        {
            dirs = Directory.GetDirectories(_currentDirectory).ToList();
            files = Directory.GetFiles(_currentDirectory).ToList();
        }

        private bool OnlyContainsDot(string str)
        {
            foreach(char c in str) if (c != '.') return false;
            return true;
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
                List<string> path = CommonFunctions.PathToList(file);
                newFiles.Add(new CFile(path.GetRange(0, path.Count - 1), path[^1]));
            }

            return newFiles;
        }
    }
}
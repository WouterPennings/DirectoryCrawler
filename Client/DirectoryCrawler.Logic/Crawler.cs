using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryCrawler.Logic
{
    public class Crawler
    {
        private CDirectory _currentDirectory;

        public Crawler(bool root = false)
        {
            if (!root)
            {
                List<string> path = CommonFunctions.PathToList(Directory.GetCurrentDirectory());
                _currentDirectory = new CDirectory(path.GetRange(0, path.Count - 1), path[^1]);
            }
            else
            {
                _currentDirectory = new CDirectory(new List<string>(), "C:");
            }
            
        }

        public Crawler(CDirectory directory)
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
            if (parameter.Length == 0) 
                throw new ChangeDirectoryException("No parameter was given", 0);
            if (_currentDirectory.Getpath().GetPath().Count == 0) 
                return _currentDirectory.ToString();
            if (OnlyContainsDot(parameter) && parameter.Length > 1) 
                _currentDirectory = GetParentDirectory(parameter);
            else if(parameter != ".") 
                _currentDirectory = GetSubDirectory(parameter);
            return _currentDirectory.ToString();
        }

        public void DirectorieContent(out List<CDirectory> newDirs, out List<CFile> newFils)
        {
            List<string> dirs = Directory.GetDirectories(_currentDirectory.ToString()).ToList();
            newDirs = GetDirectoriesInDirectory(dirs);
            List<string> files = Directory.GetFiles(_currentDirectory.ToString()).ToList();
            newFils = GetFilesInDirectory(files);
        }

        public override string ToString()
        {
            return _currentDirectory.ToString();
        }

        private bool OnlyContainsDot(string str)
        {
            foreach(char c in str) if (c != '.') return false;
            return true;
        }

        private CDirectory GetSubDirectory(string directoryName)
        {
            List<string> subDirectories = Directory.GetDirectories(_currentDirectory.ToString()).ToList();
            string newDir = subDirectories.Find(dir => dir == $"{_currentDirectory}\\{directoryName}");
            if (newDir != null)
            {
                List<string> x = CommonFunctions.PathToList(newDir);
                return new CDirectory(x.GetRange(0, x.Count - 1), x[^1]);
            }
            throw new ChangeDirectoryException("Directory given did not exist", 1);
        }

        private CDirectory GetParentDirectory(string parameter)
        {
            CDirectory newDir = _currentDirectory;
            for (int i = 0; i < parameter.Length - 1; i++)
            {
                List<string> dir = CommonFunctions.PathToList(Directory.GetParent(newDir.ToString()).ToString());
                newDir = new CDirectory(dir.GetRange(0, dir.Count - 1), dir[^1]);
            }
            return newDir;
        }
        
        private List<CDirectory> GetDirectoriesInDirectory(List<string> dirs)
        {
            List<CDirectory> newDirs = new List<CDirectory>();
            foreach (string dir in dirs)
            {
                List<string> x = CommonFunctions.PathToList(dir);
                newDirs.Add(new CDirectory(x.GetRange(0, x.Count - 1), x[^1]));
            }

            return newDirs;
        }
        
        private List<CFile> GetFilesInDirectory(List<string> files)
        {
            List<CFile> newFiles = new List<CFile>();
            foreach (string file in files)
            {
                List<string> x = CommonFunctions.PathToList(file);
                newFiles.Add(new CFile(x.GetRange(0, x.Count - 1), x[^1]));
            }

            return newFiles;
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
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
        
        // Command: "cd <parameter>"
        public string ChangeDirectory(string parameter)
        {
            if (parameter.Length == 0) 
                // If the parameter has no value
                throw new ChangeDirectoryException("No parameter was given", 0); 
            if (OnlyContainsDot(parameter) && parameter.Length > 1) 
                // Example command: "cd .." or "cd ...."
                _currentDirectory = GetParentDirectory(parameter.Length - 1); 
            else if(parameter != ".") 
                // Example command: "cd Programming"
                _currentDirectory = GetSubDirectory(parameter); 
            return _currentDirectory.ToString();
        }

        // Command: "dir"
        public void DirectorieContent(out List<CDirectory> newDirs, out List<CFile> newFils)
        {
            string directory = _currentDirectory.ToString() + "\\";
            List<string> dirs = Directory.GetDirectories(directory).ToList();
            newDirs = GetDirectoriesInDirectory(dirs);
            List<string> files = Directory.GetFiles(directory).ToList();
            newFils = GetFilesInDirectory(files);
        }
        
        // Command: "cd .."
        private CDirectory GetParentDirectory(int count)
        {
            CDirectory newDir = _currentDirectory;
            if (_currentDirectory.Getpath().GetPath().Count == 0) return _currentDirectory;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    if (_currentDirectory.Getpath().GetPath().Count > 0)
                    {
                        _currentDirectory.SetName(_currentDirectory.Getpath().GetPath()[^1].GetName());
                        _currentDirectory.Getpath().RemoveLastDirectory();
                    }
                    else
                    {
                        break;
                    }

                    
                    // If you are in the root folder, you can not go into subfolders.
                    // This is because the name of the root folder is in the path prop, instead of in the name prop
                    //List<string> dir = CommonFunctions.PathToList(Directory.GetParent("C:").ToString());
                    //if (dir[0].Length == 2 && dir[1].Length == 0) newDir = new CDirectory(new List<string>(), dir[0]);
                    //else newDir = new CDirectory(dir.GetRange(0, dir.Count - 1), dir[^1]);
                }
                catch(SystemException)
                {
                    Console.WriteLine("Something went wrong with changing directories");
                    return _currentDirectory;
                }
            }
            return newDir;
        }
        
        // Command: "cd Programming"
        private CDirectory GetSubDirectory(string directoryName)
        {
            List<string> subDirectories = Directory.GetDirectories(_currentDirectory.ToString() + "\\").ToList();
            string directory = _currentDirectory.ToString()+ "\\" + directoryName;
            string newDir = subDirectories.Find(dir => dir == directory);
            if (newDir != null)
            {
                List<string> x = CommonFunctions.PathToList(newDir);
                return new CDirectory(x.GetRange(0, x.Count - 1), x[^1]);
            }
            Console.WriteLine(directory);
            throw new ChangeDirectoryException("Directory given did not exist", 1);
        }
        
        // Gets all the directories in the directory, when command: "dir"
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
        
        // Gets all the files in the directory, when command: "dir"
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
        
        // Checks is parameter only had '.'s
        private bool OnlyContainsDot(string str)
        {
            foreach(char c in str) if (c != '.') return false;
            return true;
        }
        
        // THIS IS THE CRAWLER
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
        
        public override string ToString()
        {
            return _currentDirectory.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace DirectoryCrawler.Logic
{
    public class File
    {
        private string _name;
        private List<Folder> _Path;

        public File(string filePath)
        {
            _Path = new List<Folder>();
            filePath = filePath.Replace("\\", "/");
            List<string> path = filePath.Split('/').ToList();
            for (int i = 0; i < path.Count - 1; i++)
            {
                _Path.Add(new Folder(path[i]));
            }
            _name = path[^1];
        }

        public string GetName()
        {
            return _name;
        }
        
        public IEnumerable<Folder> GetPath()
        {
            return _Path;
        }

        public override string ToString()
        {
            string path = "";
            foreach (Folder x in _Path)
            {
                path += x.GetName() + '/';
            }
            return path + _name; 
        }
    }
}
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace DirectoryCrawler.Logic
{
    public class CFile
    {
        private string _name;
        private CPath _cPath;

        public CFile(string filePath)
        {
            filePath = filePath.Replace("\\", "/");
            List<string> path = filePath.Split('/').ToList();
            _cPath = new CPath(path.Take(path.Count - 2).ToList());
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
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace FolderCrawler
{
    public class File
    {
        private List<string> _Path;

        public File(string filePath)
        {
            filePath = filePath.Replace("\\", "/");
            _Path = filePath.Split('/').ToList();
        }

        public IEnumerable<string> GetPath()
        {
            return _Path;
        }

        public override string ToString()
        {
            string path = "";
            foreach (string x in _Path)
            {
                path += x + '/';
            }
            return path.Remove(path.Length - 1);
        }
    }
}
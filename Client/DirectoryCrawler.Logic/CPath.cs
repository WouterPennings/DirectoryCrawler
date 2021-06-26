using System.Collections.Generic;
using System.Linq;

namespace DirectoryCrawler.Logic
{
    public class CPath
    {
        private List<CDirectory> _path;

        public CPath(List<string> directories)
        {
            _path = new List<CDirectory>();
            for (int i = 0; i < directories.Count; i++)
            {
                _path.Add(new CDirectory(directories.GetRange(0, i).ToList(), directories[i]));
            }
        }

        public List<CDirectory> GetPath()
        {
            return _path;
        }
        
        public override string ToString()
        {
            string path = "";
            foreach (CDirectory dir in _path)
            {
                path += dir.GetName() + "\\";
            }

            return path;
        }
    }
}
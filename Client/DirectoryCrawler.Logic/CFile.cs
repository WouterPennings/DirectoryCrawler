using System.Collections.Generic;
using System.Linq;

namespace DirectoryCrawler.Logic
{
    public class CFile
    {
        private string _name;
        private CPath _cPath;
        
        public CFile(List<string> path, string name)
        {
            _cPath = new CPath(path.GetRange(0, path.Count - 1).ToList());
            _name = name;
        }
        
        public string GetName()
        {
            return _name;
        }
        
        public CPath GetPath()
        {
            return _cPath;
        }
        
        public override string ToString()
        {
            return _cPath.ToString() + _name;
        }
    }
}
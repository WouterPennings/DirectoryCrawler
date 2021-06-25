using System.Collections.Generic;

namespace DirectoryCrawler.Logic
{
    public class CDirectory
    {
        private string _name;
        private CPath _path;

        public CDirectory(List<string> parentDirectories, string name)
        {
            _name = name;
            _path = new CPath(parentDirectories);
        }

        public string GetName()
        {
            return _name;
        }   
        
        public CPath Getpath()
        {
            return _path;
        }

        public override string ToString()
        {
            return _path + _name;
        }
    }
}
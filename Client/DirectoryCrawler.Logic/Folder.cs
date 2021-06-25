using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace DirectoryCrawler.Logic
{
    public class Folder
    {
        private string _name;
        private List<string> _parentFolders;

        public Folder(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }
    }
}
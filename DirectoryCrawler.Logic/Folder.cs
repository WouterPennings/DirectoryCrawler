using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace DirectoryCrawler.Logic
{
    public class Folder
    {
        private string _name;

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
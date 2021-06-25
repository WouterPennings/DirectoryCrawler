using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DirectoryCrawler.Logic
{
    public class CPath
    {
        private List<CDirectory> _path;

        public CPath(List<string> directories)
        {
            _path = new List<CDirectory>();
            foreach (string dir in directories)
            {
                
            }
        }
    }
}
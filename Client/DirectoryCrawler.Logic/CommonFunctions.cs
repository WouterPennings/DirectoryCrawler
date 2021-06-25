using System.Collections.Generic;
using System.Linq;

namespace DirectoryCrawler.Logic
{
    public static class CommonFunctions
    {
        public static List<string> PathToList(string path)
        {
            string file = path.Replace("\\", "/");
            return file.Split('/').ToList();
        }
    }
}
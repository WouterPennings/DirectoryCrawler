using System.Collections.Generic;
using System.Linq;

namespace DirectoryCrawler.Logic
{
    public static class CommonFunctions
    {
        public static List<string> PathToList(string path)
        {
            string str = path.Replace("\\", "/");
            return str.Split('/').ToList();
        }

        public static List<string> SeperateCommands(string input)
        {
            return input.Split(' ').ToList();
        } 
    }
}
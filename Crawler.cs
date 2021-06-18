using System;
using System.Collections.Generic;
using System.IO;

namespace FolderCrawler
{
    public class Crawler
    {
        public List<string> GetSubDirectories(string dir, out List<string> files){
            List<string> folders = new List<string>();
            List<string> newFiles = new List<string>();
            newFiles.AddRange(Directory.GetFiles(dir));
            try{
                string[] subDirs = Directory.GetDirectories(dir);
                foreach(string sub in subDirs){
                    folders.Add(sub);
                    folders.AddRange(GetSubDirectories(sub, out List<string> newFiles2));
                    newFiles.AddRange(newFiles2);
                }  
            }
            catch(Exception e){
                Console.WriteLine($"Herres: {e}");
            }
            files = newFiles;
            return folders;
        }

        public List<string> GetDirectories(string startDirectory, out List<string> files){
            List<string> folders = new List<string>();
            List<string> newFiles = new List<string>();
            folders.Add(startDirectory);
            newFiles.AddRange(Directory.GetFiles(startDirectory));
            foreach(string sub in Directory.GetDirectories(startDirectory)){
                folders.Add(sub);
                folders.AddRange(GetSubDirectories(sub, out List<string> newFiles2)); 
                newFiles.AddRange(newFiles2);
            }
            files = newFiles;
            return folders;
        }
    }
}
﻿using System.Collections.Generic;

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
    }
}
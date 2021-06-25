﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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
    }
}
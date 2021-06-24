using System;

namespace DirectoryCrawler.Logic
{
    public class ChangeDirectoryException : Exception
    {
        public int ExceptionCode { get; set; }
        public ChangeDirectoryException(string message, int code)
            : base(message)
        {
            ExceptionCode = code;
        }
    }
}
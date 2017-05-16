using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CheckersHost.Log
{
    public static class LogManager
    {
        private static string _logFileName;

        public static void SetLogFile(string fileName)
        {
            _logFileName = fileName;
        }

        public static void Log(string message)
        {
            using (StreamWriter w = File.AppendText(_logFileName))
            {
                w.WriteLine(message);
            }
        }
    }
}
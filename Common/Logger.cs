using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public static class Logger
    {
        public static string LogPath { get; set; } = "Log.txt";
        public static string ErrorPath { get; set; } = "Error.txt";
        public static void WriteLine(string content, bool inError=false,bool inLog = true)
        {
            DateTime dt = DateTime.Now;
            string s = $"{dt.ToStringLog()}\t{content}";
            Console.WriteLine(s);
            if (inLog)
                File.AppendAllLines(LogPath, s.ToSequence());
            if (inError)
                File.AppendAllLines(ErrorPath, s.ToSequence());
        }
    }
}

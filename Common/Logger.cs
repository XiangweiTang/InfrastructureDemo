using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Common
{
    /// <summary>
    /// The log system.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// The log file path.
        /// </summary>
        public static string LogPath { get; set; } = "Log.txt";
        /// <summary>
        /// The error file path.
        /// </summary>
        public static string ErrorPath { get; set; } = "Error.txt";
        /// <summary>
        /// The status file path.
        /// </summary>
        public static string StatusPath { get; set; } = "Status.txt";
        /// <summary>
        /// Write the log.
        /// </summary>
        /// <param name="content">The log content.</param>
        /// <param name="inLog">Whether output to log file or not.</param>
        public static void WriteLog(string content, bool inLog=true)
        {
            LogLine l = new LogLine(LogLineType.Log, content);
            Console.WriteLine(l.Output());
            if (inLog)
                File.AppendAllLines(LogPath, l.Output().ToSequence());
        }
        /// <summary>
        /// Write the error.
        /// </summary>
        /// <param name="error">The error content.</param>
        /// <param name="inError">Whether output to error file or not.</param>
        /// <param name="inLog">Whether output to log file or not.</param>
        public static void WriteError(string error, bool inError=true, bool inLog=true)
        {
            LogLine l = new LogLine(LogLineType.InternalError, error);
            var contents = l.Output().ToSequence();
            Console.WriteLine(l.Output());
            if (inError)
                File.AppendAllLines(ErrorPath, contents);
            if (inLog)
                File.AppendAllLines(LogPath, contents);
        }
        /// <summary>
        /// Write the status.
        /// </summary>
        /// <param name="status">The status content.</param>
        /// <param name="inStatus">Whether output to status file or not.</param>
        /// <param name="inLog">Whether output to log file or not.</param>
        public static void WriteStatus(string status, bool inStatus=true,bool inLog = false)
        {
            LogLine l = new LogLine(LogLineType.Status, status);
            var content = l.Output().ToSequence();
            if (inStatus)
                File.AppendAllLines(StatusPath, content);
            if (inLog)
                File.AppendAllLines(LogPath, content);
        }
    }
}

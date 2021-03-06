﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Common
{
    /// <summary>
    /// Class for run a certain file.
    /// </summary>
    public static class RunFile
    {
        /// <summary>
        /// Run a file.
        /// </summary>
        /// <param name="fileName">The name of the file to be run.</param>
        /// <param name="arg">The argument string.</param>
        /// <param name="showWindow">Whether pop out new cmd window.</param>
        /// <param name="workDirectory">The work directory.</param>
        public static void Run(string fileName, string arg, bool showWindow=false,string workDirectory = "", bool redirectOutput=true)
        {
            Process proc = new Process();
            proc.StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arg,
                UseShellExecute = !showWindow,
                WorkingDirectory = workDirectory,
                RedirectStandardOutput = redirectOutput
            };
            proc.Start();
            if (redirectOutput)
            {
                using(StreamReader sr = proc.StandardOutput)
                {
                    string line;
                    while((line=sr.ReadLine())!=null)
                        Console.WriteLine(line);
                }
            }
            proc.WaitForExit();
        }

        /// <summary>
        /// Run a python script.
        /// </summary>
        /// <param name="pythonPath">The name of the python.exe</param>
        /// <param name="scriptPath">The file path of the python script.</param>
        /// <param name="pythonArg">The argument string for python.</param>
        /// <param name="showWindow">Whether pop out new cmd window.</param>
        /// <param name="workDirectory">The work directory.</param>
        public static void RunPython(string pythonPath, string scriptPath, string pythonArg, bool showWindow=true,string workDirectory = "")
        {
            Run(pythonPath, $"{scriptPath} {pythonArg}", showWindow, workDirectory, true);
        }
    }
}

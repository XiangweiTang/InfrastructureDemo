using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo
{
    abstract class Feature
    {
        /// <summary>
        /// The work folder of the task. Typically in the tmp folder.
        /// </summary>
        public string WorkFolder { get; set; } = "";
        protected TaskStatusLine StatusLine { get; set; } = new TaskStatusLine();
        public Feature()
        {
            StatusLine.UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
        /// <summary>
        /// Main thread for the feature.
        /// </summary>
        /// <param name="arg">The argument.</param>
        public void LoadAndRun(Argument arg)
        {
            StatusLine.StartTime = DateTime.Now;
            Logger.LogPath = Path.Combine(WorkFolder, "Log.txt");
            Logger.ErrorPath = Path.Combine(WorkFolder, "Error.txt");

            Logger.WriteLog("Start to load the argument.");
            Load(arg);
            Logger.WriteLog("Argument is loaded.");

            Logger.WriteLog("Start to run.");
            Run();
            Logger.WriteLog("Task is done.");
            StatusLine.EndTime = DateTime.Now;
            OutputStatusLine();
        }

        /// <summary>
        /// This is for call the Run directly.
        /// </summary>
        public void TestRun()
        {
            Run();
        }

        /// <summary>
        /// Load arguments.
        /// </summary>
        /// <param name="arg"></param>
        abstract protected void Load(Argument arg);
        /// <summary>
        /// Run main procedure.
        /// </summary>
        abstract protected void Run();
        /// <summary>
        /// Set the item count and feature name of the status line.
        /// </summary>
        abstract protected void SetStatusLine();
        /// <summary>
        /// Output the status to a certain file.
        /// </summary>
        private void OutputStatusLine()
        {
            string taskStatusFilePath = Path.Combine(WorkFolder, "TaskStatus.txt");
            File.WriteAllText(taskStatusFilePath, StatusLine.Output());
        }
    }
}

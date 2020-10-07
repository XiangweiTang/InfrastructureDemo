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
        /// <summary>
        /// Main thread for the feature.
        /// </summary>
        /// <param name="arg">The argument.</param>
        public void LoadAndRun(Argument arg)
        {
            Logger.LogPath = Path.Combine(WorkFolder, "Log.txt");
            Logger.ErrorPath = Path.Combine(WorkFolder, "Error.txt");

            Logger.WriteLog("Start to load the argument.");
            Load(arg);
            Logger.WriteLog("Argument is loaded.");

            Logger.WriteLog("Start to run.");
            Run();
            Logger.WriteLog("Task is done.");
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
    }
}

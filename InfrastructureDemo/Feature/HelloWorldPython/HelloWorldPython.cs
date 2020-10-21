using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.HelloWorldPython
{
    class HelloWorldPython : Feature
    {
        ConfigHelloWorldPython Cfg = new ConfigHelloWorldPython();
        string HelloWorldPythonScriptPath;
        protected override void Load(Argument arg)
        {
            Cfg.Load(arg);
        }

        protected override void Run()
        {
            Validation.Requires(File.Exists(Cfg.PythonPath), "The python.exe doesn't exist.");
            Logger.WriteLog("Start to prepare python script.");
            PrintPythonScript();
            Logger.WriteLog("Python script is ready.");
            Logger.WriteLog("Start to run python.");
            RunPythonScript();
            Logger.WriteLog("Python execution is done.");
        }

        /// <summary>
        /// Print the python script for the HelloWorldPython
        /// </summary>
        private void PrintPythonScript()
        {
            // Output the python script to the tmp folder.
            HelloWorldPythonScriptPath = Path.Combine(WorkFolder, "HelloWorld.py");
            // Read the content from the embeded resource.
            var pythonContent = IO.ReadEmbed("InfrastructureDemo.Internal.HlloWorldPython.HelloWorld.py", "InfrastructureDemo");
            // Print the python script.
            File.WriteAllLines(HelloWorldPythonScriptPath, pythonContent);
        }

        /// <summary>
        /// Run the python script for the HelloWorldPython.
        /// </summary>
        private void RunPythonScript()
        {
            // Set the python script.
            string pythongArg = Cfg.Name;
            // Run the python script.
            RunFile.RunPython(Cfg.PythonPath, HelloWorldPythonScriptPath, pythongArg);
        }

        /// <summary>
        /// The post setting of the task status.
        /// </summary>
        protected override void SetStatusLine()
        {
            StatusLine.FeatureName = Cfg.FeatureName;
            StatusLine.ItemCount = 1;
        }
    }
}

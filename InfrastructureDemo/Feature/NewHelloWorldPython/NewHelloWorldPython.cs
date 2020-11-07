using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.NewHelloWorldPython
{
    class NewHelloWorldPython : Feature
    {
        ConfigNewHelloWorldPython Cfg = new ConfigNewHelloWorldPython();
        string PythonScriptPath;
        protected override void Load(Argument arg)
        {
            Cfg.Load(arg);
        }

        protected override void Run()
        {
            PrintPythonScript();
            RunPythonScript();
        }

        private void PrintPythonScript()
        {
            PythonScriptPath = Path.Combine(WorkFolder, "NewHelloWorld.py");
            var list = IO.ReadEmbed("InfrastructureDemo.Internal.NewHelloWorldPython.NewHelloWorld.py", "InfrastructureDemo");
            File.WriteAllLines(PythonScriptPath, list);
        }

        private void RunPythonScript()
        {
            string pythonArg = $"{Cfg.Name} {Cfg.RepeatCount}";
            RunFile.RunPython(Cfg.PythonPath, PythonScriptPath, pythonArg);
        }

        protected override void SetStatusLine()
        {
            StatusLine.FeatureName = Cfg.FeatureName;
            StatusLine.ItemCount = Cfg.RepeatCount;
        }
    }
}

using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.HelloWorld
{
    class HelloWorld : Feature
    {
        ConfigHelloWorld Cfg = new ConfigHelloWorld();
        protected override void Load(Argument arg)
        {
            Cfg.Load(arg);
        }

        protected override void Run()
        {
            Logger.WriteLog("This is the HelloWorld for InfrastuctureDemo.");
            Console.WriteLine($"Hello world {Cfg.Name}!");
        }

        protected override void SetStatusLine()
        {
            StatusLine.FeatureName = Cfg.FeatureName;
            StatusLine.ItemCount = 1;
        }
    }
}

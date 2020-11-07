using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.NewHelloWorld
{
    class NewHelloWorld : Feature
    {
        ConfigNewHelloWorld Cfg = new ConfigNewHelloWorld();
        protected override void Load(Argument arg)
        {
            Cfg.Load(arg);
        }

        protected override void Run()
        {
            for (int i = 0; i < Cfg.RepeatCount; i++)
                Console.WriteLine($"{i} Hello world {Cfg.Name}!");
        }

        protected override void SetStatusLine()
        {
            StatusLine.FeatureName = Cfg.FeatureName;
            StatusLine.ItemCount = Cfg.RepeatCount;
        }
    }
}

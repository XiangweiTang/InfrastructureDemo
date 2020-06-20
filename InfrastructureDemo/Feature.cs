using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo
{
    abstract class Feature
    {
        public void LoadAndRun(Argument arg)
        {
            Load(arg);
            Run();
        }
        public void TestRun()
        {
            Run();
        }
        abstract protected void Load(Argument arg);
        abstract protected void Run();
    }
}

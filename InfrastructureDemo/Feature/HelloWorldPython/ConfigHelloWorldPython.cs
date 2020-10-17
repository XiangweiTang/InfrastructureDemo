using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace InfrastructureDemo.HelloWorldPython
{
    class ConfigHelloWorldPython : Config
    {
        public string Name { get; private set; } = "";
        protected override void LoadExtraArg(List<string> freeArgList, Dictionary<string, string> constrainedArgDict)
        {
            // DO NOTHING HERE.
        }

        protected override void LoadTaskNode()
        {
            Name = TaskNode.GetXmlValue("Name");
        }
    }
}

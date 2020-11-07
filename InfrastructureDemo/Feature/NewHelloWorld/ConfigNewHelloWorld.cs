using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace InfrastructureDemo.NewHelloWorld
{
    class ConfigNewHelloWorld : Config
    {
        public string Name { get; private set; }
        public int RepeatCount { get; private set; }
        protected override void LoadTaskNode()
        {
            Name = TaskNode.GetXmlValue("Name");
            RepeatCount = TaskNode.GetXmlValueInt32("Repeat");
        }
    }
}

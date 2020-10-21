using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace InfrastructureDemo.UpdateConfig
{
    class ConfigUpdateConfig : Config
    {
        public string InputConfigPath { get; private set; }
        public string OutputConfigPath { get; private set; }
        protected override void LoadExtraArg(List<string> freeArgList, Dictionary<string, string> constrainedArgDict)
        {
            // DO NOTHING.
        }

        protected override void LoadTaskNode()
        {
            InputConfigPath = TaskNode.GetXmlValue("InputConfig", "Path");
            OutputConfigPath = TaskNode.GetXmlValue("OutputConfig", "Path");
        }
    }
}

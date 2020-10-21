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
        /// <summary>
        /// The input config, or the old config path.
        /// </summary>
        public string InputConfigPath { get; private set; }
        /// <summary>
        /// The output config, or the new config path.
        /// </summary>
        public string OutputConfigPath { get; private set; }
        /// <summary>
        /// Load the task node.
        /// </summary>
        protected override void LoadTaskNode()
        {
            InputConfigPath = TaskNode.GetXmlValue("InputConfig", "Path");
            OutputConfigPath = TaskNode.GetXmlValue("OutputConfig", "Path");
        }
    }
}

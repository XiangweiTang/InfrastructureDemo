using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace InfrastructureDemo.CostSaving
{
    class ConfigCostSaving : Config
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string OutputFolderPath { get; private set; }
        protected override void LoadExtraArg(List<string> freeArgList, Dictionary<string, string> constrainedArgDict)
        {
            // Do nothing.
        }

        protected override void LoadTaskNode()
        {
            StartTime = DateTime.ParseExact(TaskNode.GetXmlValue("StartTime"), "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(TaskNode.GetXmlValue("EndTime"), "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            OutputFolderPath = TaskNode.GetXmlValue("OutputFolder", "Path");
        }
    }
}

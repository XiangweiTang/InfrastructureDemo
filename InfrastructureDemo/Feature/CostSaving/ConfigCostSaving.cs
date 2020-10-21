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
        /// <summary>
        /// The start time of the cost saving statistic time span.
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// The end time of the cost saving statistic time span.
        /// </summary>
        public DateTime EndTime { get; private set; }
        /// <summary>
        /// The output folder of the cost saving summarize.
        /// </summary>
        public string OutputFolderPath { get; private set; }

        /// <summary>
        /// Load the task node.
        /// </summary>
        protected override void LoadTaskNode()
        {
            StartTime = DateTime.ParseExact(TaskNode.GetXmlValue("StartTime"), "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(TaskNode.GetXmlValue("EndTime"), "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            OutputFolderPath = TaskNode.GetXmlValue("OutputFolder", "Path");
        }
    }
}

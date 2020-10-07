using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class CostSavingLine : Line
    {
        public string FeatureName { get; set; } = "";
        public double Rate { get; set; } = 0;
        public string FeatureDescription { get; set; } = "";
        public string ReportTemplate { get; set; } = "";
        protected override IEnumerable<object> GetLine()
        {
            yield return FeatureName;
            yield return Rate;
            yield return FeatureDescription;
            yield return ReportTemplate;
        }

        protected override void SetLine(string[] split)
        {
            FeatureName = split[0];
            Rate = double.Parse(split[1]);
            FeatureDescription = split[2];
            ReportTemplate = split[3];
        }
    }
}

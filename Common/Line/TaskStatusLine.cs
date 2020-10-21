using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class TaskStatusLine : Line
    {
        public string FeatureName { get; set; }
        public double ItemCount { get; set; } = 0;
        public string UserName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TaskStatusLine() { }
        public TaskStatusLine(string line) : base(line) { }
        protected override IEnumerable<object> GetLine()
        {
            yield return FeatureName;
            yield return UserName;
            yield return ItemCount;
            yield return StartTime.ToStringPathMedium();
            yield return EndTime.ToStringPathMedium();
        }
        protected override void SetLine(string[] split)
        {
            FeatureName = split[0];
            UserName = split[1];
            ItemCount = double.Parse(split[2]);
            StartTime = split[3].FromStringPathMedium();
            EndTime = split[4].FromStringPathMedium();
        }
    }
}

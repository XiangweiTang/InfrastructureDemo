using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public abstract class Line
    {
        public string OriginalString { get; }
        public Line() { }
        public Line(string lineStr)
        {
            SetLine(lineStr.Split('\t'));
            OriginalString = lineStr;
        }
        public string Output()
        {
            return string.Join("\t", GetLine());
        }
        protected abstract void SetLine(string[] split);
        protected abstract IEnumerable<object> GetLine();
    }
}

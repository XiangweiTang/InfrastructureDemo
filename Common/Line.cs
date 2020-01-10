using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public abstract class Line
    {
        public string OriginalString { get; private set; } = "";
        string[] Split = { };
        public Line() { }
        public Line(string lineStr)
        {
            OriginalString = lineStr;
            Split = lineStr.Split('\t');
            Set();
        }
        public string Output()
        {
            return string.Join("\t", Get());
        }
        abstract protected void Set();
        abstract protected IEnumerable<object> Get();
        public string Update()
        {
            OriginalString = Output();
            return OriginalString;
        }
    }
}

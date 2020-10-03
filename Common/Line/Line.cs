using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// The base class of all lines(tab separated).
    /// The tab seperated line is one of the main data structure in the InfrastructureDemo.
    /// </summary>
    public abstract class Line
    {
        /// <summary>
        /// This string is to keep the original string from the input.
        /// </summary>
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
        /// <summary>
        /// Break the line into pieces, then set the respective attributes.
        /// </summary>
        /// <param name="split">The split parts.</param>
        protected abstract void SetLine(string[] split);
        /// <summary>
        /// Merge the parts together.
        /// </summary>
        /// <returns>The sequence of the parts.</returns>
        protected abstract IEnumerable<object> GetLine();
    }
}

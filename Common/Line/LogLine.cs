using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// The Line structure for log.
    /// </summary>
    public class LogLine:Line
    {
        /// <summary>
        /// The type of the log.
        /// </summary>
        public LogLineType LogType { get; set; } = LogLineType.NA;
        /// <summary>
        /// The timestamp of the log.
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// The content of the log.
        /// </summary>
        public string Content { get; set; } = "";
        /// <summary>
        /// Create a new instance of the LogLine.
        /// </summary>
        /// <param name="t">The type of the log.</param>
        /// <param name="content">The content of the log.</param>
        public LogLine(LogLineType t, string content):base()
        {
            LogType = t;
            TimeStamp = new DateTime();
            Content = content;
        }
        /// <summary>
        /// Create a new instance of the LogLine.
        /// </summary>
        /// <param name="line">The full log line string.</param>
        public LogLine(string line) : base(line) { }
        /// <summary>
        /// Implement the abstract function GetLine().
        /// </summary>
        /// <returns>The sequence of LogLine parts.</returns>
        protected override IEnumerable<object> GetLine()
        {
            yield return LogType;
            yield return TimeStamp;
            yield return Content;
        }
        /// <summary>
        /// Implement the abstract function SetLine().
        /// </summary>
        /// <param name="split">The split parts of the input line string.</param>
        protected override void SetLine(string[] split)
        {
            LogType = EnumParse.ParseLogLineString(split[0]);
            TimeStamp = DateTime.Parse(split[1]);
            Content = split[2];
        }
    }    
    /// <summary>
    /// The differnet types of LogLine.
    /// </summary>
    public enum LogLineType
    {
        /// <summary>
        /// Default type.
        /// </summary>
        NA=0,
        /// <summary>
        /// Plain log.
        /// </summary>
        Log=1,
        /// <summary>
        /// Internal error.
        /// </summary>
        InternalError=2,
        /// <summary>
        /// Status of the current memory/CPU...
        /// </summary>
        Status=3,
    }
}

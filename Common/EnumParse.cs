using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// This class is for parse the enum from strings.
    /// </summary>
    public static class EnumParse
    {
        /// <summary>
        /// This class is for parsing enum from the LogLine type.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static LogLineType ParseLogLineString(string s)
        {
            switch (s.ToLower())
            {
                case "na":
                    return LogLineType.NA;
                case "log":
                    return LogLineType.Log;
                case "internalerror":
                    return LogLineType.InternalError;
                case "status":
                    return LogLineType.Status;
                default:
                    throw new InfException($"Invalid LogLineType: {s}");
            }
        }
    }
}

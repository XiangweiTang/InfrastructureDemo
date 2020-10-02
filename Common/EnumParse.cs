using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class EnumParse
    {
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

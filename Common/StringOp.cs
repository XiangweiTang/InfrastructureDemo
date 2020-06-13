using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class StringOp
    {
        public static string ToStringLog(this DateTime dt)
        {
            return dt.ToString("yyyyMMdd hh:mm:ss");
        }
    }
}

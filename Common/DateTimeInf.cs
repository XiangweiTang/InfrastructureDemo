using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class DateTimeInf
    {
        /// <summary>
        /// Output the date in the following format
        ///     yyyy/MM/dd hh:mm:ss.
        /// This is mainly for log.
        /// </summary>
        /// <param name="dt">The datatime.</param>
        /// <returns>The datetime string.</returns>
        public static string ToStringLog(this DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd hh:mm:ss");
        }
        /// <summary>
        /// Output the date time in the following format
        ///     yyyyMMddhhmmssfff
        /// This is mainly for path.
        /// </summary>
        /// <param name="dt">The datetime.</param>
        /// <returns>The datetime string.</returns>
        public static string ToStringPathLong(this DateTime dt)
        {
            return dt.ToString("yyyyMMddhhmmssfff");
        }
        /// <summary>
        /// Output the date time in the following(midium) format
        ///     yyyyMMddhhmmss
        /// This is mainly for path.
        /// </summary>
        /// <param name="dt">The datetime.</param>
        /// <returns>The datetime string.</returns>
        public static string ToStringPathMedium(this DateTime dt)
        {
            return dt.ToString("yyyyMMddhhmmss");
        }
        /// <summary>
        /// Output the date time in the following(short) format
        ///     yyyyMMdd
        /// This is mainly for path.
        /// </summary>
        /// <param name="dt">The datetime.</param>
        /// <returns>The datetime string.</returns>
        public static string ToStringFileShort(this DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace Common
{
    public static class ML
    {
        public static TItem ArgMax<TItem, TResult>(this IEnumerable<TItem> list, Func<TItem,TResult> f) where TResult : IComparable<TResult>
        {
            return list.Aggregate((x, y) => f(x).CompareTo(f(y)) > 0 ? x : y);
        }
        public static TItem ArgMin<TItem, TResult>(this IEnumerable<TItem> list, Func<TItem, TResult> f) where TResult : IComparable<TResult>
        {
            return list.Aggregate((x, y) => f(x).CompareTo(f(y)) < 0 ? x : y);
        }
    }
}

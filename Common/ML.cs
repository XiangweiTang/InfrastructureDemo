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
            return ArgM(list, f, true);
        }
        public static TItem ArgMin<TItem, TResult>(this IEnumerable<TItem> list,Func<TItem,TResult> f)where TResult : IComparable<TResult>
        {
            return ArgM(list, f, false);
        }
        private static TItem ArgM<TItem, TResult>(this IEnumerable<TItem> list, Func<TItem, TResult> f, bool max) where TResult : IComparable<TResult>
        {
            var bestItem = max
            ? list.Aggregate((x, y) => f(x).CompareTo(f(y)) > 0 ? x : y)
            : list.Aggregate((x, y) => f(x).CompareTo(f(y)) < 0 ? x : y);
            return bestItem;
        }
    }
}

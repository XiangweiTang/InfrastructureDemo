using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Common
{
    public static class ML
    {
        public static TItem ArgMax<TItem, TResult>(this IList<TItem> list, Func<TItem,TResult> f) where TResult : IComparable
        {
            return ArgPeek(list, f, 1);
        }
        public static TItem ArgMin<TItem, TResult>(this IList<TItem> list,Func<TItem,TResult> f)where TResult : IComparable
        {
            return ArgPeek(list, f, -1);
        }
        private static TItem ArgPeek<TItem, TResult>(this IList<TItem> list, Func<TItem, TResult> f, int sig) where TResult : IComparable
        {
            TItem bestItem = list[0];
            TResult bestResult = f(list[0]);
            foreach (TItem item in list)
            {
                TResult result = f(item);
                if (result.CompareTo(bestResult)*sig > 0)
                {
                    bestResult = result;
                    bestItem = item;
                }
            }
            return bestItem;
        }
    }
}

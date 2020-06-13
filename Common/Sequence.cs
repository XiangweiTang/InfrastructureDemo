using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common
{
    public static class Sequence
    {
        public static IEnumerable<T> ToSequence<T>(this T t)
        {
            yield return t;
        }
        public static IEnumerable<T> Concat<T>(this T t, IEnumerable<T> sequence)
        {
            return t.ToSequence().Concat(sequence);
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> sequence, T t)
        {
            return sequence.Concat(t.ToSequence());
        }

    }
}

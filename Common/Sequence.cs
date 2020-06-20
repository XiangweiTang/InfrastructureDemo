using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Common
{
    public static class Sequence
    {
        private static Random R = new Random();

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
        public static T[] Shuffle<T>(this IEnumerable<T> sequence)
        {
            var array = sequence.ToArray();
            for(int i = array.Length - 1; i > 0; i--)
            {
                int j = R.Next(i);
                array.Swap(i, j);
            }
            return array;
        }

        private static void Swap<T>(this IList<T> list, int i, int j)
        {
            T t = list[i];
            list[i] = list[j];
            list[j] = t;
        }

        /// <summary>
        /// Random sample some of the element from the sequence, and the sample count is tiny.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="k"></param>
        /// <param name="maxK"></param>
        /// <param name="maxRatio"></param>
        /// <returns></returns>
        public static T[] RandomSampleTiny<T>(this IList<T> list, int k, int maxK = 10, double maxRatio = 0.15)
        {
            return RandomSampleTinyIndex(list, k, maxK, maxRatio).Select(x => list[x]).ToArray();
        }

        public static int[] RandomSampleTinyIndex<T>(this IList<T> list, int k, int maxK = 10, double maxRatio = 0.15)
        {
            Sanity.Requires(k <= maxK, $"The required k(${k}) is more than the upperbound of k({maxK}).");
            Sanity.Requires(k <= list.Count * maxRatio, $"The required k(${k}) is more than the upperbound of sequence count({list.Count})*max ratio({maxRatio}).");
            
            HashSet<int> sampledSet = new HashSet<int>();
            for (int i = 0; i < k; i++)
            {
                while (true)
                {
                    int index = R.Next(list.Count);
                    if (!sampledSet.Contains(index))
                    {
                        sampledSet.Add(index);                        
                        break;
                    }
                }
            }
            return sampledSet.ToArray();
        }
    }
}

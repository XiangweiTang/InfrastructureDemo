using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common
{
    public static class Sequence
    {
        /// <summary>
        /// Internal Random.
        /// </summary>
        private static Random R = new Random();
        /// <summary>
        /// Convert an item to a sequence(lazy).
        /// </summary>
        /// <typeparam name="T">The input item type.</typeparam>
        /// <param name="item">The item need to be converted to a sequence.</param>
        /// <returns>The converted sequence.</returns>
        public static IEnumerable<T> ToSequence<T>(this T item)
        {
            yield return item;
        }
        /// <summary>
        /// Concatenate an item on the end of a sequence(lazy).
        /// </summary>
        /// <typeparam name="T">The input item type.</typeparam>
        /// <param name="sequence"></param>
        /// <param name="item"></param>
        /// <returns>The concated sequence.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> sequence, T item)
        {
            foreach (T t in sequence)
                yield return t;
            yield return item;
        }
        /// <summary>
        /// Concatenate a sequence to an item(lazy).
        /// </summary>
        /// <typeparam name="T">The input item type.</typeparam>
        /// <param name="item">The item to be concatenated.</param>
        /// <param name="sequence">The sequence to be concatenated.</param>
        /// <returns>The concated sequence.</returns>
        public static IEnumerable<T> Concat<T>(this T item, IEnumerable<T> sequence)
        {
            yield return item;
            foreach (T t in sequence)
                yield return t;
        }
        /// <summary>
        /// Concatenate two items together(lazy).
        /// </summary>
        /// <typeparam name="T">The input item type.</typeparam>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <returns>The concated sequence.</returns>
        public static IEnumerable<T> Concat<T>(this T item1, T item2)
        {
            yield return item1;
            yield return item2;
        }
        /// <summary>
        /// Suffle a sequence.
        /// </summary>
        /// <typeparam name="T">The input item type.</typeparam>
        /// <param name="sequence">The sequence needs to be shuffled.</param>
        /// <returns>The shuffled array.</returns>
        public static T[] Shuffle<T>(this IEnumerable<T> sequence)
        {
            var array = sequence.ToArray();
            for(int i = array.Length - 1; i >= 1; i--)
            {
                int j = R.Next(i);
                array.Swap(i, j);
            }
            return array;
        }
        /// <summary>
        /// Random sampling a sequence(reservoir sampling)
        /// </summary>
        /// <typeparam name="T">The input item type.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="k">The item needs to be sampled.</param>
        /// <param name="allowTruncate">If the sampled number is less than the length of the sequence, truncate the output or not.</param>
        /// <returns>The random sampled result</returns>
        public static T[] RandomSample<T>(this IEnumerable<T> sequence, int k, bool allowTruncate=true)
        {
            if (k <= 0)
                return new T[0];
            T[] array = new T[k];
            int index = 0;
            foreach(T t in sequence)
            {
                if (index < k)
                    array[index] = t;
                else
                {
                    int r = R.Next(index);
                    if (r < k)
                        array[r] = t;
                }
                index++;
            }
            if (allowTruncate && index < k)
            {
                T[] truncatedArray = new T[index];
                Array.Copy(array, 0, truncatedArray, 0, index);
                return truncatedArray;
            }
            return array;
        }
        /// <summary>
        /// Swap two items in an array.
        /// </summary>
        /// <typeparam name="T">The input item type.</typeparam>
        /// <param name="indexedSequence">The indexed sequence.</param>
        /// <param name="i">The first index to be swapped.</param>
        /// <param name="j">The second index to be swapped.</param>
        public static void Swap<T>(this IList<T> indexedSequence, int i, int j)
        {
            T t = indexedSequence[i];
            indexedSequence[i] = indexedSequence[j];
            indexedSequence[j] = t;
        }
    }
}

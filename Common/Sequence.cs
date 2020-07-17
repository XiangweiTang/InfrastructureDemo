using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections;

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

        public static T[] ReservoirSampling<T>(this IEnumerable<T> sequence, int k)
        {
            T[] reservoir = new T[k];
            int i = 0;
            foreach(T t in sequence)
            {
                if (i < k)
                    reservoir[i] = t;
                else
                {
                    int j = R.Next(i);
                    if (j < k)
                        reservoir[j] = t;
                }
                i++;
            }
            if (i < k)
                return reservoir.Take(i).ToArray();
            return reservoir;
        }        

        public static void Partition<TInput, TClassifier>(this IEnumerable<TInput> sequence, Func<TInput,TClassifier> classifier, Dictionary<TClassifier,Action<IEnumerable<TInput>>> actionDict, int bufferSize = 10_000)
        {
            Dictionary<TClassifier, List<TInput>> bufferDict = actionDict.ToDictionary(x => x.Key, x => new List<TInput>(bufferSize));
            foreach(TInput input in sequence)
            {
                TClassifier key = classifier(input);
                bufferDict[key].Add(input);
                if (bufferDict.Count >= bufferSize)
                {
                    actionDict[key](bufferDict[key]);
                    bufferDict[key].Clear();
                    bufferDict[key].Capacity = bufferSize;
                }
            }
            foreach(TClassifier key in bufferDict.Keys)
            {
                if (bufferDict[key].Count > 0)
                    actionDict[key](bufferDict[key]);
            }
        }

        public static void PartitionByInteger<T>(this IEnumerable<T> sequence, Func<T,int> classifier,  Action<int,IEnumerable<T>> action, HashSet<int> classifierValues, int bufferSize = 10_000)
        {
            Dictionary<int, Action<IEnumerable<T>>> actionDict = classifierValues.ToDictionary(x => x, x => new Action<IEnumerable<T>>((IEnumerable<T> l) => action(x, l)));
            sequence.Partition(classifier, actionDict, bufferSize);
        }

        public static void PartitionByHash<T>(this IEnumerable<T> sequence, Action<int, IEnumerable<T>> action, int maskBits = 6, int bufferSize = 10_000)
        {
            int mask = 1 << maskBits;
            Func<T, int> classifier = x => x.GetHashCode() & (mask - 1);
            HashSet<int> classifierValues = new HashSet<int>(Enumerable.Range(0, mask));
            sequence.PartitionByInteger(classifier, action, classifierValues, bufferSize);
        }
    }
}

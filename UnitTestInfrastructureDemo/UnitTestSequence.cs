using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using System.Linq;
using System.Collections.Generic;

namespace UnitTestInfrastructureDemo
{
    [TestClass]
    public class UnitTestSequence
    {
        [TestMethod]
        public void TestSwap()
        {
            int[] originalArray = { 0, 1, 2, 3, 4, 5, 6 };
            int[] swappedArray = { 0, 4, 2, 3, 1, 5, 6 };
            originalArray.Swap(1, 4);
            Assert.IsTrue(originalArray.SequenceEqual(swappedArray));
        }

        [TestMethod]
        public void TestShuffle()
        {
            const int UPPER_BOUND = 20;
            int[] originalArray = Enumerable.Range(0, UPPER_BOUND).ToArray();            
            var shuffledArray = originalArray.Shuffle();
            // Two sequence should be a permutation to each other.
            Assert.IsTrue(shuffledArray.OrderBy(x => x).SequenceEqual(originalArray));
            // Not all the element are exactly the same.
            // There is a 1/(UPPER_BOUND!) chance to fail this test, even if it is a valid shuffle.
            // However we may safely ignore this.
            Assert.IsFalse(originalArray.Zip(shuffledArray, (x, y) => x == y).All(x => x));
        }

        [TestMethod]
        public void TestRandomSample1()
        {
            const int UPPER_BOUND = 20;
            const int SHORT_SIZE = 10;
            int[] originalArray = Enumerable.Range(0, UPPER_BOUND).ToArray();

            var sample = originalArray.RandomSample(SHORT_SIZE);
            var permutationArray = GeneratePermutationArray(UPPER_BOUND);
            foreach (int i in sample)
            {
                Assert.IsTrue(permutationArray[i]);
                permutationArray[i] = false;
            }
        }

        [TestMethod]
        public void TestRandomSample2()
        {
            const int UPPER_BOUND = 20;
            const int LONG_SIZE = 30;
            int[] originalArray = Enumerable.Range(0, UPPER_BOUND).ToArray();

            var sample = originalArray.RandomSample(LONG_SIZE, true);
            var permutationArray = GeneratePermutationArray(UPPER_BOUND);
            Assert.AreEqual(permutationArray.Length, UPPER_BOUND);
            foreach (int i in sample)
            {
                Assert.IsTrue(permutationArray[i]);
                permutationArray[i] = false;
            }
        }
        [TestMethod]
        public void TestRandomSample3()
        {
            const int UPPER_BOUND = 20;
            const int LONG_SIZE = 30;
            int[] originalArray = Enumerable.Range(0, UPPER_BOUND).ToArray();


            var sample3 = originalArray.RandomSample(LONG_SIZE, false);
            var permutationArray = GeneratePermutationArray(UPPER_BOUND);
            Assert.AreEqual(sample3.Length, LONG_SIZE);
            for (int index = 0; index < LONG_SIZE; index++)
            {
                if (index < UPPER_BOUND)
                {
                    Assert.IsTrue(permutationArray[sample3[index]]);
                }
                else
                    Assert.AreEqual(sample3[index], 0);
            }
        }
        private bool[] GeneratePermutationArray(int upperBound)
        {
            return Enumerable.Range(0, upperBound).Select(x => true).ToArray();
        }
    }
}

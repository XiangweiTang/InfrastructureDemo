using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using System.Linq;

namespace UnitTestInfrastructureDemo
{
    [TestClass]
    public class UnitTestIO
    {
        [TestMethod]
        public void TestReadEmbed()
        {
            var commonResult = IO.ReadEmbed("Common.Internal.SampleData.txt");
            string[] commonExpected = { "0\tabc", "1\tpqr", "2\txyz" };
            Assert.IsTrue(commonResult.SequenceEqual(commonExpected));

            string commonStringResult=IO.ReadEmbedAll("Common.Internal.SampleData.txt");
            string commonStringExpected = string.Join("\r\n", commonExpected);
            Assert.AreEqual(commonStringResult, commonStringExpected);

            var demoResult = IO.ReadEmbed("InfrastructureDemo.Internal.SampleData.txt", "InfrastructureDemo");
            string[] demoExpected= { "1\tabc", "2\tpqr", "3\txyz" };
            Assert.IsTrue(demoResult.SequenceEqual(demoExpected));

            string demoStringResult=IO.ReadEmbedAll("InfrastructureDemo.Internal.SampleData.txt", "InfrastructureDemo");
            string demoStringExpected= string.Join("\r\n", demoExpected);
            Assert.AreEqual(demoStringResult, demoStringExpected);
        }
    }
}

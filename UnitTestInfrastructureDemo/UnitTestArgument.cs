using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfrastructureDemo;
using System.Collections.Generic;

namespace UnitTestInfrastructureDemo
{
    [TestClass]
    public class UnitTestArgument
    {
        [TestMethod]
        public void TestMethodArgument()
        {
            string[][] argStringArrays = {
                new string[] { },
                new string[] { "abc.xml" },
                new string[] { "-config", "abc.xml" },
                new string[] { "abc.xml", "-config", "xyz.xml" },
                new string[] { "abc.xml", "arg" },
                new string[] { "-key" },
                new string[] { "-key", "value" },
                new string[] { "-key", "value0", "value1" },
                new string[] { "abc.xml", "-key", "value" },
                new string[] { "magictest" },
                new string[] { "arg", "magictest" },
                new string[] { "magictest", "-key" },
                new string[] { "magictest", "abc.xml" },
                new string[] { "magictest", "-config", "abc.xml" },
                new string[] { "magictest", "arg", "abc.xml", "-key", "value", "-config", "xyz.xml" },
                new string[]{"-key0","value0","-Key1","Value1", "-config","abc.xml","-kEy2","value3","Value4"}
            };
            Argument[] setArgs =
            {
                new Argument(ArgumentCategory.ConfigFile,new List<string>(),new Dictionary<string, string>(),"config.xml"),
                new Argument(ArgumentCategory.ConfigFile,new List<string>(),new Dictionary<string, string>(),"abc.xml"),
                new Argument(ArgumentCategory.ConfigFile,new List<string>(),new Dictionary<string, string>(),"abc.xml"),
                new Argument(ArgumentCategory.ConfigFile|ArgumentCategory.ExternalArg,new List<string>{"abc.xml"},new Dictionary<string, string>(),"xyz.xml"),
                new Argument(ArgumentCategory.ExternalArg,new List<string>{"abc.xml","arg"},new Dictionary<string, string>(),""),
                new Argument(ArgumentCategory.ExternalArg,new List<string>(),new Dictionary<string, string>{{"key",""}},""),
                new Argument(ArgumentCategory.ExternalArg,new List<string>(),new Dictionary<string, string>{{"key","value"}},""),
                new Argument(ArgumentCategory.ExternalArg,new List<string>(),new Dictionary<string, string>{ { "key", "value0 value1" } },""),
                new Argument(ArgumentCategory.ExternalArg,new List<string>{"abc.xml"},new Dictionary<string, string>{{"key","value"}},""),
                new Argument(ArgumentCategory.Test,new List<string>(),new Dictionary<string, string>(),""),
                new Argument(ArgumentCategory.Test|ArgumentCategory.ExternalArg,new List<string>{"arg" },new Dictionary<string, string>(),""),
                new Argument(ArgumentCategory.Test|ArgumentCategory.ExternalArg,new List<string>(),new Dictionary<string, string>{ { "key",""} },""),
                new Argument(ArgumentCategory.Test|ArgumentCategory.ExternalArg,new List<string>{"abc.xml" },new Dictionary<string, string>(),""),
                new Argument(ArgumentCategory.Test|ArgumentCategory.ConfigFile,new List<string>(),new Dictionary<string, string>(),"abc.xml"),
                new Argument(ArgumentCategory.Test|ArgumentCategory.ConfigFile|ArgumentCategory.ExternalArg,new List<string>{"arg","abc.xml"},new Dictionary<string, string>{{"key","value"} },"xyz.xml"),
                new Argument(ArgumentCategory.ConfigFile|ArgumentCategory.ExternalArg,new List<string>()
                ,new Dictionary<string, string>{{"key0","value0"},{ "key1","Value1"}, {"key2","value3 Value4" } },"abc.xml")

            };
            for (int i = 0; i < argStringArrays.Length; i++)
                Assert.IsTrue(Argument.ArgEqual(new Argument(argStringArrays[i]), setArgs[i]));
        }
    }
}

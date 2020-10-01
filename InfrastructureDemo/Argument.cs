using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo
{
    /// <summary>
    /// Argument for InfrastructureDemo
    /// Usaage:
    ///     >xx.exe
    ///         Config file mod, config path is "config.xml"(in the same directory).
    ///     >xx.exe magictest
    ///         Test mod, or debug mod.
    ///     >xx.exe abc.xml
    ///         Config file mod, config path is "abc.xml"
    ///     >xx.exe arg0 arg1
    ///         External argument mod, the no-dash arguments are in the free arg list.
    ///     >xx.exe -key0 value0 -key1 value1
    ///         External argument mod, the dash arguments are keys, the following non-dash arguments are the values. They're in the constrained arg dict.
    ///     >xx.exe -config abc.xml
    ///         Config file mod, config path is "abc.xml"
    ///     >xx.exe magictest arg0 -config abc.xml -arg0 value0
    ///         Test/config file/external arg mod
    /// </summary>
    public class Argument
    {
        /// <summary>
        /// Defines which category does this argument belong to.
        /// </summary>
        public ArgumentCategory Category { get; set; } = ArgumentCategory.NA;
        /// <summary>
        /// The dictionary for the constrained arguments.
        /// </summary>
        public Dictionary<string, string> ConstrainedArgDict { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// The list for the free arguments.
        /// </summary>
        public List<string> FreeArgList { get; set; } = new List<string>();
        /// <summary>
        /// The config file path.
        /// </summary>
        public string ConfigFilePath { get; set; } = "";
        /// <summary>
        /// The main entrance for the argument class.
        /// </summary>
        /// <param name="args"></param>
        public Argument(string[] args)
        {
            DeployArg(args);
            SetArgType();
        }

        /// <summary>
        /// Deploy args into FreeArgList and ConstrainedArgDict
        /// FreeArg first, ConstrainedArg later.
        /// </summary>
        /// <param name="args">The string style arg</param>
        private void DeployArg(string[] args)
        {
            bool constrainedArgFlag = false;
            string key = null;
            StringBuilder valueSb = new StringBuilder();
            foreach(string arg in args)
            {
                if (!constrainedArgFlag)
                {
                    if(arg[0]=='-')
                    {
                        constrainedArgFlag = true;
                        key = arg.Substring(1).ToLower();
                        continue;
                    }
                    FreeArgList.Add(arg);
                }
                else
                {
                    if (arg[0] == '-')
                    {
                        if (key != null)
                        {
                            ConstrainedArgDict.Add(key, valueSb.ToString());
                            valueSb.Clear();
                        }
                        key = arg.Substring(1).ToLower();
                    }
                    else
                    {
                        if (valueSb.Length != 0)
                            valueSb.Append(' ');
                        valueSb.Append(arg);
                    }
                }
            }
            if (key != null)
                ConstrainedArgDict.Add(key, valueSb.ToString());
        }

        /// <summary>
        /// Set the argument type.
        /// </summary>
        private void SetArgType()
        {
            if(FreeArgList.Count==0&&ConstrainedArgDict.Count==0)
            {
                Category = ArgumentCategory.ConfigFile;
                ConfigFilePath = "Config.xml";
                return;
            }
            if (FreeArgList.Count == 1&&FreeArgList[0].ToLower()!="magictest" &&ConstrainedArgDict.Count==0)
            {       
                Category = ArgumentCategory.ConfigFile;
                ConfigFilePath = FreeArgList[0];
                FreeArgList.Clear();
                return;
            }
            if (ConstrainedArgDict.ContainsKey("config"))
            {
                Category |= ArgumentCategory.ConfigFile;
                ConfigFilePath = ConstrainedArgDict["config"];
            }
            ConstrainedArgDict.Remove("config");
            int index = -1;
            for(int i=0;i<FreeArgList.Count;i++)
            {
                if (FreeArgList[i].ToLower() == "magictest")
                {
                    index = i;
                    Category |= ArgumentCategory.Test;
                }
            }
            if (index != -1)
                FreeArgList.RemoveAt(index);
            if (ConstrainedArgDict.Count > 0 || FreeArgList.Count > 0)
                Category |= ArgumentCategory.ExternalArg;
        }
        /// <summary>
        /// Test whether two arguments are equal or not.
        /// Mainly for unit test.
        /// </summary>
        /// <param name="arg1">The first argument</param>
        /// <param name="arg2">The secnod argument</param>
        /// <returns>If the two arguments are equal, then true, otherwise, false</returns>
        public static bool ArgEqual(Argument arg1, Argument arg2)
        {
            if (arg1.Category != arg2.Category)
                return false;
            if (arg1.ConfigFilePath.ToLower() != arg2.ConfigFilePath.ToLower())
                return false;
            if (arg1.FreeArgList.Count != arg2.FreeArgList.Count)
                return false;
            if (arg1.ConstrainedArgDict.Count != arg2.ConstrainedArgDict.Count)
                return false;
            if (!arg1.FreeArgList.OrderBy(x => x).SequenceEqual(arg2.FreeArgList.OrderBy(y => y)))
                return false;
            foreach(string key in arg1.ConstrainedArgDict.Keys)
            {
                if (!arg2.ConstrainedArgDict.ContainsKey(key))
                    return false;
                if (arg1.ConstrainedArgDict[key] != arg2.ConstrainedArgDict[key])
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Override argument setting.
        /// It is not used in practice, mainly for unit test.
        /// </summary>
        /// <param name="cat">The argument category</param>
        /// <param name="argList">The free argument list</param>
        /// <param name="argDict">The constrained argument dict</param>
        /// <param name="configFilePath">The config file path</param>
        public Argument(ArgumentCategory cat, List<string> argList, Dictionary<string,string> argDict, string configFilePath)
        {
            Category = cat;
            FreeArgList = argList;
            ConstrainedArgDict = argDict;
            ConfigFilePath = configFilePath;
        }
    }

    /// <summary>
    /// Defines the cateogory of the argument.
    /// </summary>
    [Flags]
    public enum ArgumentCategory
    {
        /// <summary>
        /// Init argument mode.
        /// </summary>
        NA=0,
        /// <summary>
        /// The test mode.
        /// </summary>
        Test=1,
        /// <summary>
        /// The config file mod.
        /// </summary>
        ConfigFile=2,
        /// <summary>
        /// The external argument mod.
        /// </summary>
        ExternalArg=4,
    }
}

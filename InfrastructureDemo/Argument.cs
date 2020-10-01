using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo
{
    public class Argument
    {
        public ArgumentCategory Category { get; set; } = ArgumentCategory.NA;
        public Dictionary<string, string> ConstrainedArgDict { get; set; } = new Dictionary<string, string>();
        public List<string> FreeArgList { get; set; } = new List<string>();
        public string ConfigFilePath { get; set; } = "";
        public Argument(string[] args)
        {
            DeployArg(args);
            SetArgType();
        }

        /// <summary>
        /// Put args into FreeArgList and ConstrainedArgDict
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
        public Argument(ArgumentCategory cat, List<string> argList, Dictionary<string,string> argDict, string configFilePath)
        {
            Category = cat;
            FreeArgList = argList;
            ConstrainedArgDict = argDict;
            ConfigFilePath = configFilePath;
        }
    }

    [Flags]
    public enum ArgumentCategory
    {
        NA=0,
        Test=1,
        ConfigFile=2,
        ExternalArg=4,
    }
}

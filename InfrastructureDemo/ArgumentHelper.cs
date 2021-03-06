﻿using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace InfrastructureDemo
{
    class ArgumentHelper
    {
        Argument Arg = null;
        string TmpFolder = "Tmp";
        private bool NeedConfirm = true;
        public ArgumentHelper(Argument arg)
        {
            Arg = arg;
        }

        public void RunMainProcess()
        {            
            Directory.CreateDirectory(TmpFolder);
            string mainLogName = DateTime.Now.ToStringPathMedium();
            Logger.LogPath = Path.Combine(TmpFolder, mainLogName + ".log");
            Logger.ErrorPath =Path.Combine(TmpFolder, mainLogName + ".error.txt");
            try
            {
                RunBranches();
            }
            catch(InfException infe)
            {
                Logger.WriteError(infe.Message);
            }
            catch(Exception e)
            {
                Logger.WriteError(e.Message);
            }
        }

        private void RunBranches()
        {
            if ((Arg.Category & ArgumentCategory.Test) != 0)
            {
                // Test mod.
                _ = new Test(Arg);
            }
            else if ((Arg.Category & ArgumentCategory.ConfigFile) == 0)
            {
                // Arg only mod.
                Validation.Requires(Arg.ConstrainedArgDict.ContainsKey("feature"), "Missing feature name.");
                var feature = GetFeature(Arg.ConstrainedArgDict["feature"]);
                feature.LoadAndRun(Arg);
            }
            else
            {
                RunConfigFileMod(Arg);
            }
        }

        private void RunConfigFileMod(Argument arg)
        {
            if(!File.Exists(arg.ConfigFilePath))
            {
                OutputInitConfig(arg.ConfigFilePath);
                return;
            }
            DummyConfig cfg = new DummyConfig();
            cfg.Load(arg);
            // Here we allow the multiple task names in a single run.
            // Each of the features are seperated by a comma.
            Logger.WriteLog("You're going to run the following task(s): ");
            Logger.WriteLog(cfg.FeatureName);
            if (NeedConfirm)
            {
                Logger.WriteLog("Press any key to continue.", false);
                Console.ReadKey();
            }
            var subTaskNames = cfg.FeatureName.Split(',');

            // Split the main task into small subtasks.
            foreach (string subTaskName in subTaskNames)
            {                
                try
                {
                    var feature = GetFeature(subTaskName);
                    if (feature == null)
                    {
                        Logger.WriteLog("The feature is NA, skip.");
                        continue;
                    }
                    var subArg = new Argument(Arg);
                    string workFolder = Path.Combine(TmpFolder, $"{DateTime.Now.ToStringPathMedium()}_{subTaskName}");
                    feature.WorkFolder = workFolder;
                    Directory.CreateDirectory(workFolder);
                    string subConfigPath = Path.Combine(workFolder, "Config.xml");
                    subArg.ConfigFilePath = subConfigPath;
                    cfg.ExtractSubXDoc().Save(subConfigPath);
                    feature.LoadAndRun(subArg);
                }
                catch (InfException e)
                {
                    Logger.WriteError(e.Message);
                    continue;
                }
            }
        }

        private void OutputInitConfig(string configPath)
        {
            var l = IO.ReadEmbed("InfrastructureDemo.Config.xml", "InfrastructureDemo");
            File.WriteAllLines(configPath, l);
        }

        /// <summary>
        /// Get the feature name.
        /// </summary>
        /// <param name="featureName">The name of the feature.</param>
        /// <returns>The feature withe the respective name.</returns>
        private Feature GetFeature(string featureName)
        {
            if (featureName.ToUpper() == Constants.NA)
                return null;
            var dict = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == "InfrastructureDemo")
                .GetTypes()
                .Where(x=>x.IsSubclassOf(typeof(Feature)))
                .ToDictionary(x => x.Name, x => x);
            Validation.Requires(dict.ContainsKey(featureName), $"{featureName} is not a valid feature name.");
            return (Feature)dict[featureName].GetConstructor(new Type[0]).Invoke(new object[0]);
        }
    }
}

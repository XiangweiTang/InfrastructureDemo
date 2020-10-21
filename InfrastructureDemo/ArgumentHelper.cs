using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Feature feature = GetFeature(subTaskName);
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

        /// <summary>
        /// Get the feature name.
        /// </summary>
        /// <param name="featureName">The name of the feature.</param>
        /// <returns>The feature withe the respective name.</returns>
        private Feature GetFeature(string featureName)
        {
            switch (featureName.ToLower())
            {
                case "helloworld":
                    return new HelloWorld.HelloWorld();
                case "helloworldpython":
                    return new HelloWorldPython.HelloWorldPython();
                case "costsaving":
                    return new CostSaving.CostSaving();                    
                case "NA":
                    // NA is a safty exit, do nothing but continue without error.
                    return null;
                default:
                    throw new InfException($"Invalid feature name: {featureName}.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Common;

namespace InfrastructureDemo
{
    abstract class Config
    {
        protected XmlNode TaskNode { get; private set; } = null;
        public string FeatureName { get; protected set; }
        public string PythonPath { get; private set; }
        protected XmlDocument XDoc = new XmlDocument();
        public void Load(Argument arg)
        {
            if ((arg.Category & ArgumentCategory.ConfigFile) != 0)
            {
                Logger.WriteLog("Xml argument is enabled.");
                LoadXmlArg(arg.ConfigFilePath);
            }
            if ((arg.Category & ArgumentCategory.ExternalArg) != 0)
            {
                Logger.WriteLog("Extra argument is enabled.");
                LoadExtraArg(arg.FreeArgList, arg.ConstrainedArgDict);
            }
        }
        protected virtual void LoadXmlArg(string xmlPath)
        {
            XmlReaderSettings settings = new XmlReaderSettings { IgnoreComments = true };
            using (XmlReader xReader = XmlReader.Create(xmlPath, settings))
            {
                XDoc.Load(xReader);
                FeatureName = XDoc.GetXmlValue("Root", "FeatureName");
                TaskNode = XDoc["Root"][FeatureName];
                LoadTaskNode();
                LoadCommonNode();
            }            
        }
        protected abstract void LoadTaskNode();
        protected abstract void LoadExtraArg(List<string> freeArgList, Dictionary<string, string> constrainedArgDict);
        private void LoadCommonNode()
        {
            PythonPath = XDoc.GetXmlValue("Root/Common/Python", "Path");
        }
        public XmlDocument ExtractSubXDoc()
        {
            XDoc["Root"].KeepNodeInXmlDoc(FeatureName, "Common");
            return XDoc;
        }
    }

    class DummyConfig : Config
    {
        protected override void LoadXmlArg(string xmlPath)
        {
            XDoc.Load(xmlPath);
            FeatureName = XDoc["Root"].Attributes["FeatureName"].Value;
        }
        protected override void LoadExtraArg(List<string> freeArgList, Dictionary<string, string> constrainedArgDict)
        {            
        }

        protected override void LoadTaskNode()
        {            
        }
    }
}

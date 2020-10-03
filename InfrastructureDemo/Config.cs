using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InfrastructureDemo
{
    abstract class Config
    {
        protected XmlNode TaskNode { get; private set; } = null;
        public abstract string TaskName { get; protected set; }        
        public void Load(Argument arg)
        {
            if ((arg.Category & ArgumentCategory.ConfigFile) != 0)
            {
                LoadXmlArg(arg.ConfigFilePath);
            }
            if ((arg.Category & ArgumentCategory.ExternalArg) != 0)
                LoadExtraArg(arg.FreeArgList, arg.ConstrainedArgDict);
        }
        private void LoadXmlArg(string xmlPath)
        {
            XmlReaderSettings settings = new XmlReaderSettings { IgnoreComments = true };
            using (XmlReader xReader = XmlReader.Create(xmlPath, settings))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xReader);
                TaskNode = xDoc["Root"][TaskName];
                LoadXmlNode();
            }            
        }
        protected abstract void LoadXmlNode();
        protected abstract void LoadExtraArg(List<string> freeArgList, Dictionary<string, string> constrainedArgDict);
    }
}

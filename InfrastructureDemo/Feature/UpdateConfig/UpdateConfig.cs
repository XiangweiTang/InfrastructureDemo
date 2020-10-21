using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InfrastructureDemo.UpdateConfig
{
    class UpdateConfig : Feature
    {
        ConfigUpdateConfig Cfg = new ConfigUpdateConfig();
        protected override void Load(Argument arg)
        {
            Cfg.Load(arg);
        }

        protected override void Run()
        {
            Validation.Requires(File.Exists(Cfg.InputConfigPath), "Input config doesn't exist.");
            Update();
        }

        private void Update()
        {
            XmlDocument inputXdoc = new XmlDocument();
            inputXdoc.Load(Cfg.InputConfigPath);

            XmlDocument outputXdoc = new XmlDocument();
            string newXmlString = IO.ReadEmbedAll("InfrastructureDemo.Config.xml", "InfrastructureDemo");
            outputXdoc.LoadXml(newXmlString);

            outputXdoc.XmlMerge(inputXdoc);

            outputXdoc.Save(Cfg.OutputConfigPath);
        }

        protected override void SetStatusLine()
        {
            StatusLine.FeatureName = Cfg.FeatureName;
            StatusLine.ItemCount = 1;
        }
    }
}

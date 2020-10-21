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

        /// <summary>
        /// Update the config.
        /// Use the structure of the old config.
        /// Use the value of the new config.
        /// </summary>
        private void Update()
        {
            // The input config, used as value config.
            XmlDocument inputXdoc = new XmlDocument();
            inputXdoc.Load(Cfg.InputConfigPath);

            // The output config, load from internal data.
            // Used as structure config.
            XmlDocument outputXdoc = new XmlDocument();
            string newXmlString = IO.ReadEmbedAll("InfrastructureDemo.Config.xml", "InfrastructureDemo");
            outputXdoc.LoadXml(newXmlString);

            // Merge the config.
            outputXdoc.XmlMerge(inputXdoc);

            // Save the config to the required path.
            outputXdoc.Save(Cfg.OutputConfigPath);
        }

        protected override void SetStatusLine()
        {
            StatusLine.FeatureName = Cfg.FeatureName;
            StatusLine.ItemCount = 1;
        }
    }
}

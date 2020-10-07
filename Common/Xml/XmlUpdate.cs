using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Common
{
    /// <summary>
    /// Update related operations.
    /// </summary>
    public static partial class Xml
    {
        /// <summary>
        /// Merge two xml documents. The first one provides xml structure, the second on provides xml values.
        /// e.g.:
        ///     We have one old version of xml config and a new version of xml config.
        ///     In the new config we have more nodes/attributes(new feature).
        ///     We would like the output config contains everything in the new config, but the old features have the same config settings.
        ///     Then we use the new as the structure node, the old as the value node.
        /// </summary>
        /// <param name="structureXmlNode">The node provides xml structure.</param>
        /// <param name="valueXmlNode">The node provides xml value.</param>        
        public static void XmlMerge(this XmlNode structureXmlNode, XmlNode valueXmlNode)
        {
            Validation.Requires(structureXmlNode != null, "The structure xml node is null.");
            Validation.Requires(valueXmlNode != null, "The value xml node is null.");

            if (structureXmlNode.Attributes != null)
            {
                foreach (XmlAttribute attrib in structureXmlNode.Attributes)
                {
                    // If one attribute exists in the structure node, but no in value node, then do nothing.
                    // Or, keep the default structure attribute.
                    if (valueXmlNode.Attributes[attrib.Name] == null)
                        continue;
                    attrib.Value = valueXmlNode.Attributes[attrib.Name].Value;
                }
            }
            // HACK: If the value node and strucuture node has different structures, there might be error here.
            if (structureXmlNode.InnerText == structureXmlNode.ChildNodes[0].Value)
                structureXmlNode.InnerText = valueXmlNode.InnerText;
            for(int i = 0; i < structureXmlNode.ChildNodes.Count; i++)
            {
                var childStructureNode = structureXmlNode.ChildNodes[i];
                if (childStructureNode.NodeType == XmlNodeType.Comment)
                    continue;
                // If one node exists in the structure node, but not value node, then do nothing.
                // Or, keep the default structure node.
                if (valueXmlNode[childStructureNode.Name] == null)
                    continue;
                XmlMerge(structureXmlNode.ChildNodes[i], valueXmlNode[childStructureNode.Name]);
            }
        }
    }
}

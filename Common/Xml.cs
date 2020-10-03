using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Common
{
    /// <summary>
    /// Xml related operations.
    /// </summary>
    public static class Xml
    {
        /// <summary>
        /// Get a value from certain xpath and attribute name.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="xPath">The xpath.</param>
        /// <param name="attribute">The attribute name. Leave empty if not attribute.</param>
        /// <exception cref="InfException">Thrown when the given node is null</exception>
        /// <returns>The required value</returns>
        public static string GetXmlValue(this XmlNode node, string xPath, string attribute = "")
        {
            Validation.Requires(node != null, "The root node is null.");
            var valueNode = node.SelectSingleNode(xPath);
            Validation.Requires(valueNode != null, $"The xpath {xPath} doesn't exist.");
            if (string.IsNullOrWhiteSpace(attribute))
                return valueNode.InnerText;
            else
                return valueNode.Attributes[attribute].Value;
        }

        public static IEnumerable<string> GetXmlValues(this XmlNode node, string xPath, string attribute = "")
        {
            Validation.Requires(node != null, "The root node is null.");
            var valueNodes = node.SelectNodes(xPath);
            Validation.Requires(valueNodes.Count > 0, $"The xpath {xPath} doesn't exist.");
            bool getInnerText = string.IsNullOrWhiteSpace(attribute);
            foreach(XmlNode subNode in valueNodes)
            {
                if (getInnerText)
                    yield return subNode.InnerText;
                else
                    yield return subNode.Attributes[attribute].Value;
            }
        }
    }
}

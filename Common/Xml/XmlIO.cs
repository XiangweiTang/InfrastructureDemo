using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Common
{
    /// <summary>
    /// IO from Xml related operations.
    /// </summary>
    public static partial class Xml
    {
        /// <summary>
        /// Get a string value from certain xpath and attribute name.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="xPath">The xpath.</param>
        /// <param name="attribute">The attribute name. Leave empty if not attribute.</param>
        /// <exception cref="InfException">Thrown when the given node is null</exception>
        /// <returns>The required string value</returns>
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

        /// <summary>
        /// Get string values from certain xpath and attibute name.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="xPath">The xpath.</param>
        /// <param name="attribute">The attribute name, leave empty if not attribute.</param>
        /// <returns>The required values.</returns>
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


        /// <summary>
        /// Get a int value from certain xpath and attribute name.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="xPath">The xpath.</param>
        /// <param name="attribute">The attribute name. Leave empty if not attribute.</param>
        /// <returns>The required int value</returns>
        public static int GetXmlValueInt32(this XmlNode node, string xPath, string attribute = "")
        {
            return int.Parse(GetXmlValue(node, xPath, attribute));
        }
        /// <summary>
        /// Get int values from certain xpath and attribute name.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="xPath">The xpath.</param>
        /// <param name="attribute">The attribute name. Leave empty if not attribute.</param>
        /// <returns>The required int values</returns>
        public static IEnumerable<int> GetXmlValuesInt32(this XmlNode node, string xPath, string attribute = "")
        {
            return GetXmlValues(node, xPath, attribute).Select(x => int.Parse(x));
        }

        /// <summary>
        /// Get a double value from certain xpath and attribute name.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="xPath">The xpath.</param>
        /// <param name="attribute">The attribute name. Leave empty if not attribute.</param>
        /// <returns>The required double value</returns>
        public static double GetXmlValueDouble(this XmlNode node, string xPath, string attribute = "")
        {
            return double.Parse(GetXmlValue(node, xPath, attribute));
        }

        /// <summary>
        /// Get int values from certain xpath and attribute name.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="xPath">The xpath.</param>
        /// <param name="attribute">The attribute name. Leave empty if not attribute.</param>
        /// <returns>The required double values</returns>
        public static IEnumerable<double> GetXmlValuesDouble(this XmlNode node, string xPath, string attribute = "")
        {
            return GetXmlValues(node, xPath, attribute).Select(x => double.Parse(x));
        }

        /// <summary>
        /// Get a bool value from certain xpath and attribute name.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="xPath">The xpath.</param>
        /// <param name="attribute">The attribute name. Leave empty if not attribute.</param>
        /// <returns>The required bool value</returns>
        public static bool GetXmlValueBool(this XmlNode node, string xPath, string attribute = "")
        {
            return bool.Parse(GetXmlValue(node, xPath, attribute));
        }

        /// <summary>
        /// Get bool values from certain xpath and attribute name.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="xPath">The xpath.</param>
        /// <param name="attribute">The attribute name. Leave empty if not attribute.</param>
        /// <returns>The required bool values</returns>
        public static IEnumerable<bool> GetXmValuesBool(this XmlNode node, string xPath, string attribute = "")
        {
            return GetXmlValues(node, xPath, attribute).Select(x => bool.Parse(x));
        }
    }
}

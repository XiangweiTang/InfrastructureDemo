using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

namespace Common
{
    /// <summary>
    /// This is the add/remove parts of the xml.
    /// It is mainly for operations in configs, and other xml documents in our daily works.
    /// </summary>
    public static partial class Xml
    {
        /// <summary>
        /// Remove nodes with certain names from an xml document.
        /// </summary>
        /// <param name="rootNode">The xml document.</param>
        /// <param name="discardNodeNames">The node names to be removed.</param>
        /// <returns>The new xml document after the removal</returns>
        public static XmlNode RemoveNodeFromXmlDoc(this XmlNode rootNode, params string[] discardNodeNames)
        {
            var dupe = rootNode.Clone();
            for(int i = dupe.ChildNodes.Count - 1; i >= 0; i--)
            {
                var child = dupe.ChildNodes[i];
                if (discardNodeNames.Contains(child.Name))
                    dupe.RemoveChild(child);
            }
            return dupe;
        }

        /// <summary>
        /// Keep nodes with cerain names in an xml document.
        /// </summary>
        /// <param name="rootNode">The xml document.</param>
        /// <param name="keptNodeNames">The node names to be kept.</param>
        /// <returns>The new xml document after the kept.</returns>
        public static void KeepNodeInXmlDoc(this XmlNode rootNode, params string[] keptNodeNames)
        {
            for(int i = rootNode.ChildNodes.Count - 1; i >= 0; i--)
            {
                var child = rootNode.ChildNodes[i];
                if (!keptNodeNames.Contains(child.Name))
                    rootNode.RemoveChild(child);
            }
        }
        /// <summary>
        /// Remove a node with certain name.
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        /// <param name="nodeName">The name of the node to be removed.</param>
        public static void RemoveChild(this XmlNode rootNode, string nodeName)
        {
            var node = rootNode[nodeName];
            if (node != null)
                rootNode.RemoveChild(node);
        }
        /// <summary>
        /// Remove all nodes with certain name.
        /// </summary>
        /// <param name="rootnode">The root node.</param>
        /// <param name="nodeName">The name of the nodes to be removed.</param>
        public static void RemoveAllChild(this XmlNode rootnode, string nodeName)
        {
            XmlNode removedNode;
            while ((removedNode = rootnode[nodeName]) != null)
                rootnode.RemoveChild(removedNode);
        }

        /// <summary>
        /// Insert a sequence of duplicated nodes as the child node of the root node.
        /// </summary>
        /// <param name="rootNode">The rootnode to be inserted.</param>
        /// <param name="subNodeName">The name of the subnode.</param>
        /// <param name="iDupe">The interface of generating duplication nodes.</param>
        /// <param name="removeOriginal">Whether to remove the original nodes or not.</param>
        public static void DupeNode(this XmlNode rootNode, string subNodeName, IDupeXml iDupe, bool removeOriginal = true)
        {
            Validation.Requires(rootNode[subNodeName] != null, $"The subnode {subNodeName} doesn't exist.");
            int index = -1;
            for (int i = rootNode.ChildNodes.Count - 1; i >= 0; i--)
            {
                // Get the last of the required node.
                if (rootNode.ChildNodes[i].Name == subNodeName)
                {
                    index = i;
                    break;
                }
            }
            var templateNode = rootNode.ChildNodes[index];
            var dupeNodeList = iDupe.GenerateSubNodes(templateNode);
            InsertXmlNodesAfter(rootNode, dupeNodeList, index);
            if (removeOriginal)
            {
                // The index-th node is the last one.
                // So remove every nodes with the certain names before(inclusive) it.
                for (int i = index; i >= 0; i--)
                {
                    if (rootNode.ChildNodes[i].Name == subNodeName)
                        rootNode.RemoveChild(rootNode.ChildNodes[i]);
                }
            }
        }

        /// <summary>
        /// Insert a sequence of nodes before a certain child nodes index.
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        /// <param name="nodeList">The node sequence to be inserted.</param>
        /// <param name="i">The index of the inserted node.</param>
        public static void InsertXmlNodesBefore(this XmlNode rootNode, IEnumerable<XmlNode> nodeList, int i)
        {
            foreach (var node in nodeList)
                rootNode.InsertBefore(node, rootNode.ChildNodes[i]);
        }

        /// <summary>
        /// Insert a sequence of nodes after a certain child nodes index.
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        /// <param name="nodeList">The node sequence to be inserted.</param>
        /// <param name="i">The index of the inserted node.</param>
        public static void InsertXmlNodesAfter(this XmlNode rootNode, IEnumerable<XmlNode> nodeList, int i)
        {
            foreach (var node in nodeList)
                rootNode.InsertAfter(node, rootNode.ChildNodes[i++]);
        }
    }

    /// <summary>
    /// Interface for generate a sequence of xml nodes.
    /// </summary>
    public interface IDupeXml
    {
        IEnumerable<XmlNode> GenerateSubNodes(XmlNode templateNode);
    }
}

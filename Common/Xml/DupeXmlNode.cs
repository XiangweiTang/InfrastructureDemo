using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Common
{
    /// <summary>
    /// This interface is for generate duplication of xml nodes.
    /// </summary>
    public interface IDupeXml
    {
        /// <summary>
        /// Generate a sequence of node based on template node.
        /// </summary>
        /// <param name="templateNode">The template node to be duplicated.</param>
        /// <returns>The sequence of the duplication of node.</returns>
        IEnumerable<XmlNode> GenerateSubNodes(XmlNode templateNode);
    }

    public static partial class Xml
    {
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
            for(int i = rootNode.ChildNodes.Count-1; i >=0; i--)
            {
                // Get the last of the required node.
                if(rootNode.ChildNodes[i].Name==subNodeName)
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
                for(int i = index; i >= 0; i--)
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
            foreach(var node in nodeList)            
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
            foreach(var node in nodeList)
                rootNode.InsertAfter(node, rootNode.ChildNodes[i++]);
        }
    }
}

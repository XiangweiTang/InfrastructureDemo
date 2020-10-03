using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace InfrastructureDemo.HelloWorld
{
    class ConfigHelloWorld : Config
    {
        public string Name { get; private set; } = "";

        public override string TaskName { get => "HelloWorld"; protected set { } }

        /// <summary>
        /// Override the load extra arg.
        /// So HelloWorld feature can be called in this way:
        ///     >xx.exe -feature helloworld -name MyName
        /// </summary>
        /// <param name="freeArgList">The free arguments list.</param>
        /// <param name="constrainedArgDict">The constrained arugments dictionary.</param>
        protected override void LoadExtraArg(List<string> freeArgList, Dictionary<string, string> constrainedArgDict)
        {
            if (constrainedArgDict.ContainsKey("name"))
                Name = constrainedArgDict["name"];
        }

        /// <summary>
        /// Override the load xml node.
        /// </summary>
        protected override void LoadXmlNode()
        {
            Name = TaskNode.GetXmlValue("Name");
        }
    }
}

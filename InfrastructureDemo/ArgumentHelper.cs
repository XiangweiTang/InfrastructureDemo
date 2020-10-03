using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo
{
    class ArgumentHelper
    {
        Argument Arg = null;
        public ArgumentHelper(Argument arg)
        {
            Arg = arg;
        }

        public void Run()
        {
            if((Arg.Category&ArgumentCategory.Test)!=0)
            {
                _ = new Test(Arg);
            }
            else
            {
            }
        }

        private Feature GetFeature(string featureName)
        {
            switch (featureName.ToLower())
            {
                case "helloworld":
                    return new HelloWorld.HelloWorld();
                case "NA":
                    return null;
                default:
                    throw new InfException($"Invalid feature name: {featureName}.");
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

namespace InfrastructureDemo
{
    class Test
    {
        /// <summary>
        /// This is the test mod. For advance user(programer) use.
        /// By using the Test class, the user can call many internal functions to boost program efficiency.
        /// </summary>
        /// <param name="arg">The argument.</param>
        public Test(Argument arg)
        {
            // Add your test code here.

            // This is a test for HelloWorld feature.            
            /*
            HelloWorld.HelloWorld helloWorld = new HelloWorld.HelloWorld();
            helloWorld.TestRun();
            */

            // This is a call of internal class Matrix.
            /*
            Matrix<string, int, double> m = new Matrix<string, int, double>();
            m["a", 1] = 0.1;
            m["b", 2] = 0.3;
            m["c", 2] = 0.5;
            var list = m.ToText();
            foreach (string s in list)
                Console.WriteLine(s);
            */
            XmlSerializer xs = new XmlSerializer(typeof(HelloWorld.ConfigHelloWorld));
            using (StreamWriter sw = new StreamWriter("1.xml"))
            {
                xs.Serialize(sw, new HelloWorld.ConfigHelloWorld());
            }
        }
    }
}

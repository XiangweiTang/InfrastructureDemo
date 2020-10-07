using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Argument arg = new Argument(args);
            ArgumentHelper ah = new ArgumentHelper(arg);
            ah.Run();
        }
    }
}

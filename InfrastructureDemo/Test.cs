using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace InfrastructureDemo
{
    class Test
    {
        public Test()
        {
            Wave w = new Wave();
            w.DeepParse(@"D:\Tmp\Audio\Work\Audios\1\0000_20191001093522.wav");
        }
    }
}

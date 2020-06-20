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
            Features.KMeans.KMeansSample kms = new Features.KMeans.KMeansSample();
            kms.LoadAndRun(null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.Features.KMeans
{
    class ConfigKMeans:Config
    {
        public int K { get; private set; } = 3;
        public int RoundThreashold { get; private set; } = 10;
        public double DistDiffThreshold { get; private set; } = -1;
    }
}
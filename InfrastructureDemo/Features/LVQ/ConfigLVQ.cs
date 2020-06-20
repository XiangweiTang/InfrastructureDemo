using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.Features.LVQ
{
    class ConfigLVQ:Config
    {
        public int RoundThreshold { get; private set; } = 400;
        public double DiffThreshold { get; private set; } = -1;
        public int CanvasSize { get; private set; } = 50;
        public bool PrintVector { get; private set; } = true;
        public bool PrintMeans { get; private set; } = true;
        public double LearningRate { get; private set; } = 0.1;
    }
}

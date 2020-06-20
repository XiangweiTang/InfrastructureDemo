using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace InfrastructureDemo.Features.KMeans
{
    abstract class KMeans<Vector>:Feature
    {
        protected ConfigKMeans Cfg = new ConfigKMeans();
        protected Vector[] VectorSequence = null;
        protected int[] ClusterDistribute = null;
        protected Vector[] ClusterMeans = null;        
        protected int K = -1;
        bool ClusterChange = false;
        protected override void Load(Argument arg)
        {
            Cfg.Load(arg);
        }
        protected override void Run()
        {
            for (int i = 0; i < 5; i++)
            {
                Init();
                Iteration();
            }
        }

        private void Init()
        {
            SetSequence();
            K = Cfg.K;
            ClusterDistribute = new int[VectorSequence.Length];
            Vector[] bookSample= { VectorSequence[5], VectorSequence[11], VectorSequence[23] };
            ClusterMeans = VectorSequence.RandomSampleTinyIndex(3).Select(x => VectorSequence[x]).ToArray();            
        }
        
        private void Iteration()
        {
            int round = 0;
            double diff;
            do
            {
                ClusterChange = false;
                round++;
                ResetCluster();
                diff = ResetClusterMean();
            } while (!(!ClusterChange || (round > Cfg.RoundThreashold) || diff < Cfg.DistDiffThreshold));
            OutputMean();
            Plot();
            Console.WriteLine("------------------------------");
        }
        private void ResetCluster()
        {
            for (int i = 0; i < VectorSequence.Length; i++)
            {
                int minIndex = Enumerable.Range(0, K).ArgMin((index) => VectorDistance(VectorSequence[i], ClusterMeans[index]));
                if (ClusterDistribute[i] != minIndex)
                {
                    ClusterChange = true;
                    ClusterDistribute[i] = minIndex;
                }
            }
        }
        private double ResetClusterMean()
        {
            double totalDiff = 0;
            var newMeans = OverallVectorMean().ToArray();
            Sanity.Requires(newMeans.Length == K, $"Reset cluster mean error, #(mean)={newMeans.Length}, expected K={K}.");
            for(int i = 0; i < K; i++)
            {
                totalDiff += VectorDistance(newMeans[i], ClusterMeans[i]);
                ClusterMeans[i] = newMeans[i];
            }
            return totalDiff;
        }
        private void OutputMean()
        {
            foreach (var vector in ClusterMeans)
                Console.WriteLine(OutputVector(vector));
        }
        protected abstract void SetSequence();
        protected abstract double VectorDistance(Vector v1, Vector v2);
        protected abstract IEnumerable<Vector> OverallVectorMean();
        protected abstract string OutputVector(Vector v);
        protected virtual void Plot() { }
        
    }

    class KMeansSample : KMeans<PlainVector>
    {
        protected override string OutputVector(PlainVector v)
        {
            return $"X={v.X:0.000} Y={v.Y:0.000}";
        }

        protected override void SetSequence()
        {
            string path = "InfrastructureDemo.Internal.KMeans.KMeansInput1.txt";
            VectorSequence = IO.ReadEmbedClean(path).Select(x => new PlainVector { X = double.Parse(x[1]), Y = double.Parse(x[2]) }).ToArray();
        }

        protected override double VectorDistance(PlainVector v1, PlainVector v2)
        {
            double dx = v1.X - v2.X;
            double dy = v1.Y - v2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }


        protected override IEnumerable<PlainVector> OverallVectorMean()
        {
            int[] vectorCountDict = new int[K];
            double[] vectorXDict = new double[K];
            double[] vectorYDict = new double[K];
            for(int i = 0; i < ClusterDistribute.Length; i++)
            {
                vectorCountDict[ClusterDistribute[i]]++;
                vectorXDict[ClusterDistribute[i]] += VectorSequence[i].X;
                vectorYDict[ClusterDistribute[i]] += VectorSequence[i].Y;
            }
            for (int i = 0; i < K; i++)
                yield return new PlainVector { X = vectorXDict[i] / vectorCountDict[i], Y = vectorYDict[i] / vectorCountDict[i] };
        }

        protected override void Plot()
        {
            char[] Dot = { '@', '#', 'X' };
            int canvasSize = Cfg.CanvasSize;
            char[,] canvas = new char[canvasSize, canvasSize];
            for (int i = 0; i < canvasSize; i++)
                for (int j = 0; j < canvasSize; j++)
                    canvas[i, j] = ' ';
            if (Cfg.PrintVectors)
            {
                for (int i = 0; i < VectorSequence.Length; i++)
                {
                    int x = (int)(canvasSize * VectorSequence[i].X);
                    int y = (int)(canvasSize * VectorSequence[i].Y);
                    canvas[x, y] = Dot[ClusterDistribute[i]];
                }
            }
            if (Cfg.PrintMeans)
            {
                foreach (var vector in ClusterMeans)
                {
                    int x = (int)(canvasSize * vector.X);
                    int y = (int)(canvasSize * vector.Y);
                    canvas[x, y] = '*';
                }
            }
            if (Cfg.PrintVectors || Cfg.PrintMeans)
                canvas.DrawPlain();
        }
    }

    struct PlainVector
    {
        public double X;
        public double Y;
    }
}

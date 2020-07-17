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
    abstract class KMeans<TVector>:Feature
    {
        protected ConfigKMeans Cfg = new ConfigKMeans();
        protected TVector[] VectorSequence = null;
        protected int[] ClusterDistribute = null;
        protected TVector[] ClusterMeans = null;        
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
            TVector[] bookSample= { VectorSequence[5], VectorSequence[11], VectorSequence[23] };
            ClusterMeans = VectorSequence.ReservoirSampling(3).ToArray();            
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
        protected abstract double VectorDistance(TVector v1, TVector v2);
        protected abstract IEnumerable<TVector> OverallVectorMean();
        protected abstract string OutputVector(TVector v);
        protected virtual void Plot() { }
        
    }

    abstract class KMeansPlainVector : KMeans<PlainVector>
    {
        protected override string OutputVector(PlainVector v)
        {
            return v.OutputVector();
        }
        protected override double VectorDistance(PlainVector v1, PlainVector v2)
        {
            return v1.GetDistance(v2);
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
    class KMeansSample : KMeansPlainVector
    {
        protected override void SetSequence()
        {
            string path = "InfrastructureDemo.Internal.Sample.Sample1.txt";
            VectorSequence = IO.ReadEmbedClean(path).Select(x => new PlainVector { X = double.Parse(x[1]), Y = double.Parse(x[2]) }).ToArray();
        }
    }
    class KMeansVerify : KMeansPlainVector
    {
        Random R = new Random();
        protected override void SetSequence()
        {
            K = 3;
            PlainVector K1 = new PlainVector { X = 0.3, Y = 0.2 };
            PlainVector K2 = new PlainVector { X = 0.5, Y = 0.8 };
            PlainVector K3 = new PlainVector { X = 0.7, Y = 0.4 };
            var list1 = GeneratePlainVectors(K1, 500, 5, 0.05);
            var list2 = GeneratePlainVectors(K2, 500, 5, 0.05);
            var list3 = GeneratePlainVectors(K3, 500, 5, 0.05);
            VectorSequence = list1.Concat(list2).Concat(list3).Shuffle();
        }
        private IEnumerable<PlainVector> GeneratePlainVectors(PlainVector center, int n, int k, double step)
        {
            for (int i = 0; i < n; i++)
                yield return GeneratePlainVector(center, k, step);
        }
        private PlainVector GeneratePlainVector(PlainVector center, int k, double step)
        {
            double x = center.X;
            double y = center.Y;
            for(int i = 0; i < k; i++)
            {
                x += (R.NextDouble() * 2 - 1) * step;
                y += (R.NextDouble() * 2 - 1) * step;
            }
            if (x < 0) x = 0;
            if (x > 1) x = 0.99;
            if (y < 0) y = 0;
            if (y > 1) y = 0.99;
            return new PlainVector { X = x, Y = y };
        }
    }
}

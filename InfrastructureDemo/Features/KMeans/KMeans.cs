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
        protected HashSet<int>[] Clusters = null;
        protected Vector[] ClusterMeans = null;
        int[] KSeq = null;
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
            Clusters = new HashSet<int>[K];
            for (int i = 0; i < K; i++)
                Clusters[i] = new HashSet<int>();
            //Vector[] bookSample= { VectorSequence[5], VectorSequence[11], VectorSequence[23] };
            ClusterMeans = VectorSequence.RandomSampleTiny(K);
            KSeq = Enumerable.Range(0, K).ToArray();
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
                //PrintCurrentResult(round);
            } while (!(!ClusterChange || (round > Cfg.RoundThreashold) || diff < Cfg.DistDiffThreshold));
            Plot();
        }
        private void ResetCluster()
        {
            for (int i = 0; i < VectorSequence.Length; i++)
            {
                int minIndex = KSeq.ArgMin((index) => VectorDist(VectorSequence[i], ClusterMeans[index]));
                if(!Clusters[minIndex].Contains(i))
                {
                    ClusterChange = true;
                    foreach (var cluster in Clusters)
                    { 
                        if(cluster.Contains(i))
                        {
                            cluster.Remove(i);
                            break;
                        }
                    }
                    Clusters[minIndex].Add(i);
                }    
            }
        }
        private double ResetClusterMean()
        {
            double totalDiff = 0;
            for(int i = 0; i < K; i++)
            {
                Vector mean = VectorMean(Clusters[i].Select(x => VectorSequence[x]));
                totalDiff += VectorDist(mean, ClusterMeans[i]);
                ClusterMeans[i] = mean;
            }
            return totalDiff;
        }
        protected abstract void SetSequence();
        protected abstract double VectorDist(Vector v1, Vector v2);
        protected abstract Vector VectorMean(IEnumerable<Vector> vectors);
        protected abstract string OutputVector(Vector v);

        protected virtual void Plot() { }
        private void PrintCurrentResult(int i)
        {
            Console.WriteLine($"Round {i}");
            for(int j = 0; j < K; j++)
            {
                Console.WriteLine(OutputVector(ClusterMeans[j]));
            }
            for(int j = 0; j < K; j++)
            {
                Console.WriteLine($"\tCluster {j}");
                foreach (int index in Clusters[j])
                    Console.WriteLine($"\t\t{OutputVector(VectorSequence[index])}");
            }
        }

        
    }

    class KMeansSample : KMeans<PlainVector>
    {
        char[] Dot = { '@', '#', 'X' };
        protected override string OutputVector(PlainVector v)
        {
            return $"{v.X} {v.Y}";
        }

        protected override void SetSequence()
        {
            string path = "InfrastructureDemo.Internal.KMeans.KMeansInput1.txt";
            VectorSequence = IO.ReadEmbedClean(path).Select(x => new PlainVector { X = double.Parse(x[1]), Y = double.Parse(x[2]) }).ToArray();
        }

        protected override double VectorDist(PlainVector v1, PlainVector v2)
        {
            double dx = v1.X - v2.X;
            double dy = v1.Y - v2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        protected override PlainVector VectorMean(IEnumerable<PlainVector> vectors)
        {
            double sumX = 0;
            double sumY = 0;
            int n = 0;
            foreach(var v in vectors)
            {
                sumX += v.X;
                sumY += v.Y;
                n++;
            }
            return new PlainVector { X = sumX / n, Y = sumY / n };
        }

        protected override void Plot()
        {
            const int CANVAS_SIZE = 50;
            char[,] Canvas = new char[CANVAS_SIZE, CANVAS_SIZE];
            Console.WriteLine("------------------------------");
            for (int i = 0; i < CANVAS_SIZE; i++)
                for (int j = 0; j < CANVAS_SIZE; j++)
                    Canvas[i, j] = ' ';
            for(int i = 0; i < K; i++)
            {
                Console.WriteLine(Clusters[i].Count);
                foreach(int index in Clusters[i])
                {
                    int x = (int)(CANVAS_SIZE * VectorSequence[index].X);
                    int y = (int)(CANVAS_SIZE * VectorSequence[index].Y);
                    Canvas[x, y] = Dot[i];
                }
                int meanX = (int)(CANVAS_SIZE * ClusterMeans[i].X);
                int meanY = (int)(CANVAS_SIZE * ClusterMeans[i].Y);
                Canvas[meanX, meanY] = '*';
            }
            Canvas.DrawPlain();
        }
    }

    struct PlainVector
    {
        public double X;
        public double Y;
    }
}

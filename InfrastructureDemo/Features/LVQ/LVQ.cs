using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace InfrastructureDemo.Features.LVQ
{
    /// <summary>
    /// Learning Vector Quantization
    /// </summary>
    abstract class LVQ<TVector, TLabel> : Feature
    {
        protected TVector[] VectorSequence = null;
        protected TLabel[] LabelSequence = null;
        protected double LearningRate = 0.1;
        protected TLabel[] TargetLabel = null;
        protected TVector[] TargetVector = null;
        protected int Q = 0;
        Random R = new Random();

        protected ConfigLVQ Cfg = new ConfigLVQ();
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
                Plot();
            }
        }

        private void Init()
        {
            SetSequence();
            Q = TargetLabel.Length;
            TargetVector = VectorSequence.Shuffle();
        }
        private void Iteration()
        {
            int round = 0;
            double diff;
            do
            {
                round++;
                diff = Update();
            } while ((round <= Cfg.RoundThreshold) || (diff < Cfg.DiffThreshold));
            Plot();
        }
        private double Update()
        {
            int index = R.Next(VectorSequence.Length);
            TVector v = VectorSequence[index];
            TLabel l = LabelSequence[index];
            int min = Enumerable.Range(0, Q).ArgMin((x) => GetDistance(TargetVector[x], v));
            return UpdateTargetVector(min, v, l);
        }
        
        protected abstract void SetSequence();
        protected abstract double GetDistance(TVector v1, TVector v2);
        protected abstract double UpdateTargetVector(int min, TVector v, TLabel l);
        protected virtual void Plot() { }
    }
    abstract class LVQPlainText : LVQ<PlainVector, int>
    {
        protected char[] Dot = null;
        protected override double GetDistance(PlainVector v1, PlainVector v2)
        {
            return v1.GetDistance(v2);
        }
        protected override double UpdateTargetVector(int min, PlainVector v, int l)
        {
            var vCurrent = TargetVector[min];
            var vInterp = LearningRate * (v - vCurrent);
            PlainVector vResult = TargetLabel[min] == l
                ? vCurrent + vInterp
                : vCurrent - vInterp;
            if (vResult.X < 0) vResult.X = 0;
            if (vResult.X >= 1) vResult.X = 0.99;
            if (vResult.Y < 0) vResult.Y = 0;
            if (vResult.Y >= 1) vResult.Y = 0.99;
            TargetVector[min] = vResult;
            return vInterp.Length();
        }
        protected override void Plot()
        {
            SetDot();
            int canvasSize = Cfg.CanvasSize;
            char[,] canvas = new char[canvasSize, canvasSize];
            for (int i = 0; i < canvasSize; i++)
                for (int j = 0; j < canvasSize; j++)
                    canvas[i, j] = ' ';
            if (Cfg.PrintVector)
            {
                for(int i = 0; i < VectorSequence.Length; i++)
                {
                    int x = (int)(canvasSize * VectorSequence[i].X);
                    int y = (int)(canvasSize * VectorSequence[i].Y);
                    canvas[x, y] = Dot[LabelSequence[i]];
                }
            }
            if (Cfg.PrintMeans)
            {
                for(int i = 0; i < Q; i++)
                {
                    int x = (int)(canvasSize * TargetVector[i].X);
                    int y = (int)(canvasSize * TargetVector[i].Y);
                    canvas[x, y] = '*';
                }
            }
            if (Cfg.PrintVector || Cfg.PrintMeans)
                canvas.DrawPlain();
        }
        protected abstract void SetDot();
    }
    class LVQSample : LVQPlainText
    {
        protected override void SetSequence()
        {
            string path = "InfrastructureDemo.Internal.Sample.Sample1.txt";
            VectorSequence = IO.ReadEmbedClean(path).Select(x => new PlainVector { X = double.Parse(x[1]), Y = double.Parse(x[2]) }).ToArray();
            LabelSequence = Enumerable.Repeat(1, VectorSequence.Length).ToArray();
            for (int i = 8; i <= 20; i++)
                LabelSequence[i] = 0;
            TargetVector = new PlainVector[] { VectorSequence[4], VectorSequence[11], VectorSequence[17], VectorSequence[22], VectorSequence[28] };
            TargetLabel = new int[] { 1, 0, 0, 1, 1 };
        }
        protected override void SetDot()
        {
            Dot = new char[] { '@', 'O' };
        }
    }
}

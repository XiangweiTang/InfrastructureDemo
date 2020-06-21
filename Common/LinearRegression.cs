using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class LinearRegression
    {
        public double SumX { get; private set; } = 0;
        public double SumY { get; private set; } = 0;
        public double SumXY { get; private set; } = 0;
        public double SumX2 { get; private set; } = 0;
        public double MeanX { get; private set; } = 0;
        public double W { get; private set; } = 0;
        public double B { get; private set; } = 0;
        public double M { get; private set; } = 0;
        private IEnumerable<PlainVector> Vectors = null;
        public LinearRegression(IEnumerable<PlainVector> vectors)
        {
            Vectors = vectors;
        }
        public void Calc()
        {
            SumX = 0;
            SumY = 0;
            SumXY = 0;
            SumX2 = 0;
            foreach(var vector in Vectors)
            {
                SumX += vector.X;
                SumY += vector.Y;
                SumXY += vector.X * vector.Y;
                SumX2 += vector.X * vector.X;
                M++;
            }

            MeanX = SumX / M;
            W = (SumXY - MeanX * SumY) / (SumX2 - SumX * SumX / M);
            B = (SumY - W * SumX) / M;
        }
    }
}

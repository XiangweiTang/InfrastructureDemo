using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Common
{

    public partial class PlainVector
    {
        public double X;
        public double Y;
    }

    public static class PlainVectorProcess
    {
        public static double GetDistance(this PlainVector v1, PlainVector v2)
        {
            double dx = v1.X - v2.X;
            double dy = v1.Y - v2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public static string OutputVector(this PlainVector v)
        {
            return $"X={v.X:0.000} Y={v.Y:0.000}";
        }
        public static double Length(this PlainVector v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }
    }
    public partial class PlainVector
    {
        public static PlainVector operator +(PlainVector v1, PlainVector v2)
        {
            return new PlainVector { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }
        public static PlainVector operator *(double r, PlainVector v)
        {
            return new PlainVector { X = r * v.X, Y = r * v.Y };
        }
        public static PlainVector operator*(PlainVector v,double r)
        {
            return r * v;
        }
        public static double operator*(PlainVector v1, PlainVector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }        
        public static PlainVector operator -(PlainVector v)
        {
            return new PlainVector { X = -v.X, Y = -v.Y };
        }
        public static PlainVector operator -(PlainVector v1, PlainVector v2)
        {
            return v1 + (-v2);
        }
    }
}

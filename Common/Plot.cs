using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class Plot
    {
        public static void DrawPlain(this char[,] canvas)
        {
            int x = canvas.GetLength(0);
            int y = canvas.GetLength(1);
            
            for(int i = 0; i < y; i++)
            {
                StringBuilder sb = new StringBuilder(x);
                for (int j = 0; j < x; j++)
                {
                    sb.Append(canvas[j, y - 1 - i]);
                    sb.Append(' ');
                }
                Console.WriteLine(sb.ToString());
            }
        }
    }
}

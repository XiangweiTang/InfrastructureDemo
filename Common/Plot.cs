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
            for(int i = y - 1; i >= 0; i--)
            {
                for(int j = 0; j < x; j++)                
                    Console.Write($"{canvas[j, i]} ");
                Console.WriteLine();
            }
        }
    }
}

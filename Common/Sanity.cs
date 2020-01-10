using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class Sanity
    {
        public static void Requires(bool valid)
        {
            if (!valid)
                throw new InfException();
        }
        public static void Requires(bool valid, string errorMessage)
        {
            if (!valid)
                throw new InfException(errorMessage);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class InfException:Exception
    {
        public InfException() : base() { }
        public InfException(string message) : base(message) { }
    }
}

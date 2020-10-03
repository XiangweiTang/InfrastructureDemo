using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// The self define exception of the InfrastructureDemo.
    /// Most of the time, these errors will be dealt differently.
    /// </summary>
    public class InfException:Exception
    {
        public InfException() : base() { }
        public InfException(string message) : base(message) { }
    }
}

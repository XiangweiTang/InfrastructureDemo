using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo
{
    class Argument
    {
        public ArgumentCategory Category { get; private set; } = ArgumentCategory.NA;
    }

    enum ArgumentCategory
    {
        NA=0,
        Test=1,
        ConfigFile=2,
        ExternalArg=3,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Utils.Filter
{
    public enum Operation
    {
        AndEquals,
        OrEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
}

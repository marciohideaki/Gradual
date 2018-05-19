using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Utils.Filter
{
    public class Filter
    {
        public string PropertyName { get; set; }
        public Operation Operation { get; set; }
        public object Value { get; set; }
    }
}

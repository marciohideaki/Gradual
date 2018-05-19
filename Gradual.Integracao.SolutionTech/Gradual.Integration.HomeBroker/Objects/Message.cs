using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Integration.HomeBroker
{
    public class Response
    {
        public bool         Valid       { get; set; }
        public string       Message     { get; set; }
        public User         User        { get; set; }
        public Exception    Except      { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Integration.HomeBroker
{
    public class Request
    {
        public string Token { get; set; }
        public string Host { get; set; }
        public string TokenType { get; set; }
        public string PublicKeyPath { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Integration.HomeBroker
{
    [Serializable]
    public class User
    {
        public Int32 Codigo         { get; set; }
        public String Nome          { get; set; }
        public String Login         { get; set; }
        public DateTime DataHora    { get; set; }
        public int ValidadeToken    { get; set; }
    }
}

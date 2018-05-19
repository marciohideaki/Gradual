using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Ordens
{
    public class OrdemReferenciaNaoEncontradaException : Exception
    {
        public string ClOrdID { get; set; }
        public object MensagemOriginal { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundosDia01Info
    {
        public string CodigoFundo { get; set; }

        public DateTime Data { get; set; }

        public string CotasEmitidas { get; set; }

        public string CotasResgCliente { get; set; }

        public string CotasResgIR { get; set; }

        public string NumeroCotistas { get; set; }

        public DateTime DataHora { get; set; }
    }
}

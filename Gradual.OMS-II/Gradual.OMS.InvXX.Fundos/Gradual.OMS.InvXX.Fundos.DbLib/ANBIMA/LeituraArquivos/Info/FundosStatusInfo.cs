using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundosStatusInfo
    {
        public string CodigoFundo { get; set; }

        public string Status { get; set; }

        public string OpcoesStatus { get; set; }

        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }

        public DateTime DataHora { get; set; }
    }
}

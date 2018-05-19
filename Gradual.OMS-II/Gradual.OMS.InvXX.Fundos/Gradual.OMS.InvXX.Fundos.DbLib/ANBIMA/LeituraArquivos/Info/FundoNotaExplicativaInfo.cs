using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundoNotaExplicativaInfo
    {
        public string CodigoFundo { get; set; }

        public string Ano { get; set; }

        public string CodigoSequencia { get; set; }

        public DateTime DataHora { get; set; }
    }
}

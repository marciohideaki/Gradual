using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundosTipoInfo
    {
        public int CodigoTipo { get; set; }

        public string Descricao { get; set; }

        public string Sigla { get; set; }

        public DateTime DataInicioVigencia { get; set; }

        public DateTime DataFimVigencia { get; set; }

        public DateTime DataHora { get; set; }
    }
}

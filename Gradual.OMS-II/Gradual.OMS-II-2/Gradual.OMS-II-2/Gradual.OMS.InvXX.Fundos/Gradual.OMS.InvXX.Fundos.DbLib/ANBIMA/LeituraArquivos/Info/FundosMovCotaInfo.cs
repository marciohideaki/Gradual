using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundosMovCotaInfo
    {
        public int IdProduto { get; set; }

        public string CodigoFundo { get; set; }

        public DateTime Data { get; set; }

        public double ValorMinimoAplicacaoInicial { get; set; }

        public double ValorMinimoAplicacaoAdicional { get; set; }

        public double ValorMinimoResgate { get; set; }

        public double ValorMinimoAplicacao { get; set; }

        public string Identificador { get; set; }

        public DateTime DataHora { get; set; }
    }
}

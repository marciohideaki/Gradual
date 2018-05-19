using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundosMesInfo
    {
        public int IdProduto { get; set; }

        public string CodigoFundo { get; set; }

        public int DataMes { get; set; }

        public int DataAno { get; set; }

        public double ValorPL { get; set; }

        public double ValorCota { get; set; }

        public double RentabilidadeMes { get; set; }

        public double RentabilidadeAno { get; set; }

        public string CodigoTipo { get; set; }

        public DateTime DataHora { get; set; }
    }
}

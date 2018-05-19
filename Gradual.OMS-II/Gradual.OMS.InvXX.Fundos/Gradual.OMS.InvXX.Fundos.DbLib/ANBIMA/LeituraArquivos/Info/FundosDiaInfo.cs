using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundosDiaInfo
    {
        public string CodigoFundo { get; set; }

        public DateTime Data { get; set; }

        public double Pl { get; set; }

        public double ValorCota { get; set; }

        public double RentabilidadeDia { get; set; }

        public double RentabilidadeMes { get; set; }

        public double RentabilidadeAno { get; set; }
    }
}

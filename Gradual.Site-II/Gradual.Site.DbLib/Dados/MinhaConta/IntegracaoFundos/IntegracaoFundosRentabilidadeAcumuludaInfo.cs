using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosRentabilidadeAcumuludaInfo
    {
        public DateTime Data                { get; set; }

        public string CodigoANBIMA          { get; set; }

        public IntegracaoFundosInfo Produto { get; set; }

        public decimal PatrimonioLiquido    { get; set; }

        public decimal ValorCota            { get; set; }

        public decimal RentabilidadeMes     { get; set; }

        public decimal RentabilidadeAno     { get; set; }

        public decimal Rentabilidade12Meses { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class RentabilidadeIntegracaoFundosResponse : MensagemResponseBase
    {
        public List<IntegracaoFundosRentabilidadeAcumuludaInfo> Resultado { get; set; }

        public RentabilidadeIntegracaoFundosResponse()
        {
            this.Resultado = new List<IntegracaoFundosRentabilidadeAcumuludaInfo>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class CompararRentabilidadeIntegracaoFundosResponse : MensagemResponseBase
    {
        public List<List<IntegracaoFundosSimulacaoInfo>> FundosSimulados
        {
            get;
            set;
        }

        public List<List<IntegracaoFundosComparacaoInfo>> FundosComparados
        {
            get;
            set;
        }

        public CompararRentabilidadeIntegracaoFundosResponse()
        {
            this.FundosSimulados = new List<List<IntegracaoFundosSimulacaoInfo>>();
            this.FundosComparados = new List<List<IntegracaoFundosComparacaoInfo>>();
        }
    }
}

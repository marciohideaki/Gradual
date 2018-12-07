using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PosicaoConsolidadaIntegracaoFundosResponse : MensagemResponseBase
    {
        public List<IntegracaoFundosPosicaoClienteInfo> ResultadoBusca { get; set; }

        public PosicaoConsolidadaIntegracaoFundosResponse()
        {
            this.ResultadoBusca = new List<IntegracaoFundosPosicaoClienteInfo>();
        }
    }
}

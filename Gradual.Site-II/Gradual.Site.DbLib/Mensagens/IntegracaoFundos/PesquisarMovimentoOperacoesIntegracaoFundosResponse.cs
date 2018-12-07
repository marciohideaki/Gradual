using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PesquisarMovimentoOperacoesIntegracaoFundosResponse : MensagemResponseBase
    {
        public List<IntegracaoFundosMovimentoOperacoesInfo> Resultado { get; set; }

        public PesquisarMovimentoOperacoesIntegracaoFundosResponse()
        {
            Resultado = new List<IntegracaoFundosMovimentoOperacoesInfo>();
        }
    }
}

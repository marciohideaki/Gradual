using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using Gradual.Site.DbLib.Dados.MinhaConta;


namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class SolicitarIntegracaoFundosOperacaoRequest : MensagemRequestBase
    {
        public IntegracaoFundosOperacaoInfo Operacao    { get; set; }
        public string TipoAcesso                        { get; set; }
        public List<FundoInfo> PosicaoCotista           { get; set; }
    }
}

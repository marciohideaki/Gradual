using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PosicaoConsolidadaIntregacaoFundosRequest : MensagemRequestBase
    {
        public OpcaoBuscaClienteIntegracaoFundosEnum BuscarPor { get; set; }

        public string TermoDeBusca { get; set; }

        public Nullable<OpcaoTipoClienteIntegracaoFundosEnum> TipoCliente { get; set; }

        public int CodigoProduto { get; set; }

        public string PerfisProduto { get; set; }

        public Nullable<int> CodigoAssessor { get; set; }

        public Nullable<int> CodigoLogin { get; set; }

        public PosicaoConsolidadaIntregacaoFundosRequest()
        {
            this.TermoDeBusca = string.Empty;
            this.PerfisProduto = string.Empty;
        }

        
    }
}

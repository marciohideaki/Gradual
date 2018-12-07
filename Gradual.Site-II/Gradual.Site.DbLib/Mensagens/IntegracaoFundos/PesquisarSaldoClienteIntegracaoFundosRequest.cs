using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PesquisarSaldoClienteIntegracaoFundosRequest : MensagemRequestBase
    {
        public IntegracaoFundosOpcaoBuscaClienteEnum BuscarPor { get; set; }

        public string TermoDeBusca { get; set; }

        public Nullable<IntegracaoFundosOpcaoTipoClienteEnum> TipoCliente { get; set; }

        public int CodigoProduto { get; set; }

        public bool TemSaldoBloqueado { get; set; }
    }
}

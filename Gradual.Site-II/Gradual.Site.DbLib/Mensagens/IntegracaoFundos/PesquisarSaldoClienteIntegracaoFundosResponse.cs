using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using Gradual.OMS.ContaCorrente.Lib;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PesquisarSaldoClienteIntegracaoFundosResponse : MensagemResponseBase
    {
        public List<SaldoIntegracaoFundosInfo> Resultado { get; set; }

        public List<ContaCorrenteInfo> ListaContaCorrente { get; set; }

        public PesquisarSaldoClienteIntegracaoFundosResponse()
        {
            this.Resultado = new List<SaldoIntegracaoFundosInfo>();
        }
    }
}

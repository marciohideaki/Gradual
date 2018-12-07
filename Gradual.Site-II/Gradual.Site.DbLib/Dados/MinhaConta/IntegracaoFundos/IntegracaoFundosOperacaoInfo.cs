using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosOperacaoInfo
    {
        public IntegracaoFundosTipoOperacaoEnum TipoOperacao { get; set; }

        public int IdCliente { get; set; }

        public IntegracaoFundosInfo Produto { get; set; }

        public decimal ValorSolicitado { get; set; }

        public Nullable<DateTime> DataAgendamento { get; set; }

        public bool AplicacaoProgramada { get; set; }

        public bool Aprovada { get; set; }

        public IntegracaoFundosStatusOperacaoEnum Status { get; set; }

        public int IdOperacao { get; set; }

        public int DiaAplicacaoProgramada { get; set; }

        public bool AntecipaAplicacao { get; set; }

        public bool ResgateTotal { get; set; }

        public IntegracaoFundosOperacaoInfo()
        {
            this.Produto = new  IntegracaoFundosInfo();
        }
    }
}

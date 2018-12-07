using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PesquisarMovimentoOperacoesIntegracaoFundosRequest : MensagemResponseBase
    {
        public string DataDe { get; set; }

        public string DataAte { get; set; }

        public int IdProduto { get; set; }

        public string Status { get; set; }

        public string TipoOperacao { get; set; }

        public string HorarioSolicitacaoIni { get; set; }

        public string HorarioSolicitacaoFim { get; set; }

        public PosicaoConsolidadaIntregacaoFundosRequest BuscaDeCliente { get; set; }

        public int IdCasa { get; set; }

        public DateTime ObterDataInicio()
        {
            if (!String.IsNullOrWhiteSpace(this.DataDe))
            {
                try
                {
                    return Convert.ToDateTime(this.DataDe, new CultureInfo("pt-BR"));
                }
                catch { }
            }
            return DateTime.Now; ;
        }

        public DateTime ObterDataFim()
        {
            if (!String.IsNullOrWhiteSpace(this.DataAte))
            {
                try
                {
                    return Convert.ToDateTime(this.DataAte, new CultureInfo("pt-BR"));
                }
                catch { }
            }
            return DateTime.Now;
        }
    }
}

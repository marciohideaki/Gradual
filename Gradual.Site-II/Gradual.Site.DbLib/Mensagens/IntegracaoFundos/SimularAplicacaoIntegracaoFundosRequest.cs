using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class SimularAplicacaoIntegracaoFundosRequest : MensagemRequestBase
    {
        public List<int> Produtos { get; set; }

        public List<int> Indexadores { get; set; }

        public int Periodo { get; set; }

        public decimal Valor { get; set; }

        public string ProdutosParaSimular { get; set; }

        public SimularAplicacaoIntegracaoFundosRequest()
        {
            this.Produtos    = new List<int>();
            this.Indexadores = new List<int>();
        }
    }
}

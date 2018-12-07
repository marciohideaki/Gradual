using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class CompararRentabilidadeIntegracaoFundosRequest : MensagemRequestBase
    {
        public List<int> Produtos { get; set; }

        public List<int> Indexadores { get; set; }

        public int Periodo { get; set; }

        public CompararRentabilidadeIntegracaoFundosRequest()
        {
            this.Produtos = new List<int>();
            this.Indexadores = new List<int>();
        }
    }
}

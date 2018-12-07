using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class RentabilidadeIntegracaoFundosRequest : MensagemRequestBase
    {
        public List<int> Produtos { get; set; }
    }
}

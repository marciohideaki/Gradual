using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class IndicePeriodoIntegracaoFundosRequest : MensagemRequestBase
    {
        public string NomeIndexador { get; set; }
        
        public int IdIndexador      { get; set; }
    }
}

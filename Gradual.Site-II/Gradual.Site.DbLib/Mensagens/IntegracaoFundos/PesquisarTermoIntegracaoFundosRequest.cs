using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PesquisarTermoIntegracaoFundosRequest : MensagemRequestBase
    {
        public int CodigoFundo { get; set; }
        public int CodigoCliente { get; set; }
    }
}

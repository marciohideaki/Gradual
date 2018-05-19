using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Fundos;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class FundoResponse : MensagemResponseBase
    {

        public List<ClienteFundosInfo> ListaFundo { get; set; }

        
        public FundosInfo Fundo { get; set; }
    }
}

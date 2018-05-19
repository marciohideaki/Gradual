using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Fundos;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class FundoRequest : MensagemRequestBase
    {
        
        public string CpfDoCliente { get; set; }


        public ClienteFundosInfo Fundo { get; set; }
    }
}

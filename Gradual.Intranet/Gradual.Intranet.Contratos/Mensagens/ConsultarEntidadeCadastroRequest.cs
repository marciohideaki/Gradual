using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class ConsultarEntidadeCadastroRequest<T> : MensagemRequestBase
    {

        public T EntidadeCadastro { get; set; }

        public List<CondicaoInfo> Condicoes { get; set; }        
        
        public ConsultarEntidadeCadastroRequest()
        {
            this.Condicoes = new List<CondicaoInfo>();
        }
        
        public ConsultarEntidadeCadastroRequest(T pEntidade) : this()
        {
            this.EntidadeCadastro = pEntidade;
        }

    }
}

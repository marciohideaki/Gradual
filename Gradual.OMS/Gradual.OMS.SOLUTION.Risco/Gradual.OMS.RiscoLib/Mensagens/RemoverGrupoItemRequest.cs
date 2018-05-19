using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.OMS.Risco.RegraLib.Mensagens
{
    public class RemoverGrupoItemRequest  : MensagemRequestBase
    {
        public int CodigoGrupoItem { get; set; }
    }
}

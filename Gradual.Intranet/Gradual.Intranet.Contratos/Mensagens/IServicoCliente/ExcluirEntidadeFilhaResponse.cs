#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class ExcluirEntidadeFilhaResponse : BaseResponse
    {
        public bool blnExcluido { get; set; }
    }
}

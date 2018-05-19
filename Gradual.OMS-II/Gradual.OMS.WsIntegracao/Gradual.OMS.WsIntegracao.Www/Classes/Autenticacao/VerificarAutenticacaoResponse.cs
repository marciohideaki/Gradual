using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Library;

namespace Gradual.OMS.WsIntegracao
{
    public class VerificarAutenticacaoResponse : RespostaBase
    {
        public bool AutenticacaoVerificada { get; set; }
    }
}
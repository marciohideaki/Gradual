using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class AutenticarUsuarioRequest
    {
        #region Propriedades

        public string CodigoOuEmailDoUsuario { get; set; }

        public string Senha { get; set; }

        public string Token { get; set; }

        #endregion
    }
}
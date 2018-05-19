using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class AutenticarUsuarioResponse : RespostaBase
    {
        #region Propriedades

        public string CodigoDaSessao { get; set; }

        public string CodigoDeAcessoDoUsuario { get; set; }
        
        public string IdLogin { get; set; }

        #endregion
    }
}
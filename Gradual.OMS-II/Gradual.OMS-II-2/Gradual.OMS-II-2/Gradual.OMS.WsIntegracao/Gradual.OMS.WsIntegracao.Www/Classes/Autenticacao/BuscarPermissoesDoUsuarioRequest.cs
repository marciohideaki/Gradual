using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class BuscarPermissoesDoUsuarioRequest
    {
        #region Propriedades

        public string CodigoDoUsuario { get; set; }

        public string CodigoDaSessao { get; set; }

        #endregion
    }
}
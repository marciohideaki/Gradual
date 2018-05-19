using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class BuscarAvisosParaUsuariosDoHomebrokerRequest
    {
        #region Propriedades

        public string CBLC { get; set; }

        public bool BuscarSomenteAvisosAtivos { get; set; }

        public int IdSistema { get; set; }

        #endregion
    }
}
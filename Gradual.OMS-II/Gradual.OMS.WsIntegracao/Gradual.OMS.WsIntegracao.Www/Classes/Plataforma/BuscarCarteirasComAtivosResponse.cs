using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class BuscarCarteirasComAtivosResponse : RespostaBase
    {
        #region Propriedades

        public List<string> Carteiras { get; set; }

        public List<string> Ativos { get; set; }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class RespostaBase
    {
        #region Propriedades

        public string StatusResposta { get; set; }

        public string DescricaoResposta { get; set; }

        #endregion
    }
}
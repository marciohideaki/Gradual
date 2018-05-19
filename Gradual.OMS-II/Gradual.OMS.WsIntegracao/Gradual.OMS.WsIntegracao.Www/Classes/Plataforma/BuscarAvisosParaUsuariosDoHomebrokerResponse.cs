using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class BuscarAvisosParaUsuariosDoHomebrokerResponse : RespostaBase
    {
        #region Propriedades
        
        public List<AvisoHomeBrokerInfo> Avisos { get; set; }

        #endregion
    }
}
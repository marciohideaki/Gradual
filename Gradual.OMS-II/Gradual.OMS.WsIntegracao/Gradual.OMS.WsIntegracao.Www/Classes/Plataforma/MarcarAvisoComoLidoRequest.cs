using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class MarcarAvisoComoLidoRequest
    {
        #region Propriedades

        /// <summary>
        /// ID do aviso, em formato string: nomeDoArquivo-NumeroDaLinha-CBLCdoCliente
        /// </summary>
        public string IdDoAviso { get; set; }

        #endregion
    }
}
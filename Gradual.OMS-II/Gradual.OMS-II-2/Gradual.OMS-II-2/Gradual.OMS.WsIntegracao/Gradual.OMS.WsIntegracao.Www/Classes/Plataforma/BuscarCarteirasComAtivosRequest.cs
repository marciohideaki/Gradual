﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class BuscarCarteirasComAtivosRequest
    {
        #region Propriedades

        public string CodigoDoUsuario { get; set; }

        #endregion
    }
}
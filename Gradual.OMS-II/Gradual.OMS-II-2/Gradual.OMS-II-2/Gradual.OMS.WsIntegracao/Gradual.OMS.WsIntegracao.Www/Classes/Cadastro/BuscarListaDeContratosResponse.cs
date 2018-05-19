using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.OMS.WsIntegracao
{
    public class BuscarListaDeContratosResponse : RespostaBase
    {
        #region Propriedades

        public List<ClienteContratoInfo> Contratos { get; set; }

        //public List<int> IDsDosContratos { get; set; }

        #endregion
    }
}
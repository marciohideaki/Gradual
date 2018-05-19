using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class BuscarPermissoesDoUsuarioResponse : RespostaBase
    {
        #region Propriedades

        public List<PermissaoAssociadaInfo> Permissoes { get; set; }

        #endregion
    }
}
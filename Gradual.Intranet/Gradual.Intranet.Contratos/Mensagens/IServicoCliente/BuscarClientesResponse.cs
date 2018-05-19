using System;
using System.Collections.Generic;

using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class BuscarClientesResponse : BaseResponse
    {
        #region Propriedades

        public List<ClienteResumidoInfo> ClientesEncontrados { get; set; }

        #endregion

        #region Construtor

        public BuscarClientesResponse()
        {
            this.ClientesEncontrados = new List<ClienteResumidoInfo>();
        }

        #endregion

    }
}

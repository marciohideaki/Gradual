using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao
{
    public class CadastrarAdesaoAoContratoRequest
    {
        #region Propriedades

        public int CodigoBovespaDoCliente { get; set; }

        public int IdDoContratoNoCadastro { get; set; }
        
        public string NomeDoCliente { get; set; }

        public string EmailDoCliente { get; set; }

        public string IdDoClienteNoBanco { get; set; }

        #endregion
    }
}
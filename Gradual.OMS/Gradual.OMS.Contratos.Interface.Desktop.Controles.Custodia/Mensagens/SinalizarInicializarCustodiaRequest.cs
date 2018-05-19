using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.Custodia.Mensagens
{
    /// <summary>
    /// Mensagem para informar inicializacao de custodia
    /// </summary>
    public class SinalizarInicializarCustodiaRequest : MensagemInterfaceRequestBase
    {
        /// <summary>
        /// Custodia sendo inicializada
        /// </summary>
        public CustodiaInfo Custodia { get; set; }
    }
}

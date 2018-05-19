using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.Custodia.Mensagens
{
    /// <summary>
    /// Mensagem de sinalização de salvar o perfil de risco
    /// </summary>
    public class SinalizarSalvarCustodiaRequest : MensagemInterfaceRequestBase
    {
        /// <summary>
        /// Custodia que esta sendo salva
        /// </summary>
        public CustodiaInfo Custodia { get; set; }
    }
}

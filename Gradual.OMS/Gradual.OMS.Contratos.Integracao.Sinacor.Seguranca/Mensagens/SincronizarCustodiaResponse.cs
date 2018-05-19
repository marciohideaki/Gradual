using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Custodia.Dados;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de sincronização de custodia
    /// </summary>
    public class SincronizarCustodiaResponse : MensagemResponseBase
    {
        /// <summary>
        /// Informações da custodia salva
        /// </summary>
        public CustodiaInfo Custodia { get; set; }
    }
}

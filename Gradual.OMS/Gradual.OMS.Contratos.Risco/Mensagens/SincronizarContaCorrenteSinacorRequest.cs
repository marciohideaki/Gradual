using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.ContaCorrente.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitacao de sincronizacao de conta corrente com o
    /// sinacor
    /// </summary>
    public class SincronizarContaCorrenteSinacorRequest : MensagemRequestBase
    {
        /// <summary>
        /// Opcionalmente pode-se fazer o sicronismo atraves do código do usuário
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Referencia direta para o objeto usuario
        /// </summary>
        public UsuarioInfo Usuario { get; set; }

        /// <summary>
        /// Opcionalmente pode-se passar a conta corrente para não precisar carregar da persistencia
        /// </summary>
        public ContaCorrenteInfo ContaCorrente { get; set; }

        /// <summary>
        /// Indica se deve realizar a sincronizacao de conta corrente
        /// </summary>
        public bool SincronizarContaCorrente { get; set; }

        /// <summary>
        /// Indica se deve realizar a sincronizacao de conta investimento
        /// </summary>
        public bool SincronizarContaInvestimento { get; set; }

        /// <summary>
        /// Indica se deve realizar a sincronizacao de conta margem
        /// </summary>
        public bool SincronizarContaMargem { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de inicialização de usuário
    /// </summary>
    public class InicializarUsuarioRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do usuário a ser inicializado
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Caso o usuário não tenha o contexto com o código cblc informado,
        /// precisa passar o cblc para realizar o sincronismo
        /// </summary>
        public string CodigoCBLC { get; set; }

        /// <summary>
        /// Opcionalmente pode-se informar o objeto do usuário
        /// </summary>
        public UsuarioInfo Usuario { get; set; }

        /// <summary>
        /// Indica se deve tentar inferir o código cblc da conta investimento
        /// </summary>
        public bool InferirCBLCInvestimento { get; set; }
        
        /// <summary>
        /// Indica se deve sincronizar conta corrente
        /// </summary>
        public bool SincronizarContaCorrente { get; set; }

        /// <summary>
        /// Indica se deve sincronizar conta investimento
        /// </summary>
        public bool SincronizarContaInvestimento { get; set; }

        /// <summary>
        /// Indica se deve sincronizar conta margem
        /// </summary>
        public bool SincronizarContaMargem { get; set; }

        /// <summary>
        /// Indica se deve sincronizar custodia
        /// </summary>
        public bool SincronizarCustodia { get; set; }
    }
}

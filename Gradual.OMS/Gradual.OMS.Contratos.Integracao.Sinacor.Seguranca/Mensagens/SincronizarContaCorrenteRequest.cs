using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de sincronização de conta corrente
    /// </summary>
    public class SincronizarContaCorrenteRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do cliente cblc para realizar a consulta no sinacor.
        /// Caso informado, irá fazer a consulta diretamente por este valor.
        /// </summary>
        public string CodigoClienteCBLC { get; set; }

        /// <summary>
        /// Código do cliente cblc de investimento para realizar a consulta no sinacor.
        /// Caso informado, irá fazer a consulta diretamente por este valor.
        /// </summary>
        public string CodigoClienteCBLCInvestimento { get; set; }

        /// <summary>
        /// Código do usuário para localizar o cliente cblc.
        /// Caso informado, irá localizar o usuário e procurar o código cblc dele.
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Opcionalmente pode-se informar o usuário diretamente
        /// </summary>
        public UsuarioInfo Usuario { get; set; }

        /// <summary>
        /// Código da conta corrente, caso não seja informado a propria conta corrente
        /// </summary>
        public string CodigoContaCorrente { get; set; }
        
        /// <summary>
        /// Conta corrente a ser sincronizada
        /// </summary>
        public ContaCorrenteInfo ContaCorrente { get; set; }

        /// <summary>
        /// Indica se deve sincronizar a conta corrente
        /// </summary>
        public bool SincronizarContaCorrente { get; set; }

        /// <summary>
        /// Indica se deve sincronizar a conta investimento
        /// </summary>
        public bool SincronizarContaInvestimento { get; set; }

        /// <summary>
        /// Indica se deve salvar a conta corrente ao final do processo
        /// </summary>
        public bool SalvarEntidades { get; set; }
    }
}

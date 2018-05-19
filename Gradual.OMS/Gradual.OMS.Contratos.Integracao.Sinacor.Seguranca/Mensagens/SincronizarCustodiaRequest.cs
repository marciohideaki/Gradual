using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Custodia.Dados;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de sincronização de custódia
    /// </summary>
    public class SincronizarCustodiaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo da custodia que será atualizada
        /// </summary>
        public string CodigoCustodia { get; set; }

        /// <summary>
        /// Opcionalmente pode informar diretamente a custodia
        /// </summary>
        public CustodiaInfo Custodia { get; set; }

        /// <summary>
        /// Código do cliente cblc para realizar a consulta no sinacor.
        /// Caso informado, irá fazer a consulta diretamente por este valor.
        /// </summary>
        public string CodigoClienteCBLC { get; set; }

        /// <summary>
        /// Código do usuário para localizar o cliente cblc.
        /// Caso informado, irá localizar o usuário e procurar o código cblc dele.
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Opcionalmente pode informar o usuário diretamente
        /// </summary>
        public UsuarioInfo Usuario { get; set; }

        /// <summary>
        /// Indica se deve salvar as entidades alteradas.
        /// No caso, usuário e custodia
        /// </summary>
        public bool SalvarEntidades { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public SincronizarCustodiaRequest()
        {
        }
    }
}

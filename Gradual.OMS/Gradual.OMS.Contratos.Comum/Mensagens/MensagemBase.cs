using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Classe base para implementação de mensagens.
    /// Fornece a identificação única da mensagem através do CodigoMensagem e faz controle de sessão 
    /// através do CodigoSessao.
    /// </summary>
    [Serializable]
    public abstract class MensagemBase : ICodigoEntidade
    {
        /// <summary>
        /// BMF / Bovespa:
        /// Identificador único da mensagem
        /// </summary>
        [Category("MensagemBase")]
        [Description("BMF / Bovespa: Identificador único da mensagem")]
        public string CodigoMensagem { get; set; }

        /// <summary>
        /// Indica a sessao correspondente da mensagem.
        /// Em casos de solicitação, indica a sessão que está fazendo a solicitação.
        /// Em casos de resposta, indica a sessão que deve ser avisada da resposta.
        /// </summary>
        [Category("MensagemRequestBase")]
        public string CodigoSessao { get; set; }

        /// <summary>
        /// Data de referencia da mensagem. Geralmente irá indicar a data de criação
        /// da mensagem.
        /// </summary>
        [Category("MensagemRequestBase")]
        public DateTime DataReferencia { get; set; }

        /// <summary>
        /// Construtor. Gera um ClOrdID para a mensagem
        /// </summary>
        public MensagemBase()
        {
            // Gera new Guid para o ClOrdID
            this.CodigoMensagem = Guid.NewGuid().ToString();
        }
        
        #region ICodigoEntidade Members

        /// <summary>
        /// Retorna o código desta entidade
        /// </summary>
        /// <returns></returns>
        public string ReceberCodigo()
        {
            return this.CodigoMensagem;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de processamento de operação
    /// </summary>
    public class ProcessarOperacaoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do usuário a quem pertence a operação
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Opcionalmente pode-se informar o objeto do usuário
        /// </summary>
        public UsuarioInfo Usuario { get; set; }

        /// <summary>
        /// Código de referencia para a operacao.
        /// No caso de operacoes de bolsa, aqui vai o código 
        /// da operacao
        /// </summary>
        public string CodigoReferenciaOperacao { get; set; }

        /// <summary>
        /// Código para indicar a origem da operacao
        /// </summary>
        public string CodigoOrigemOperacao { get; set; }

        /// <summary>
        /// Tipo do evento a processar
        /// </summary>
        public ProcessarOperacaoTipoEventoEnum TipoEvento { get; set; }

        /// <summary>
        /// Valor da operação
        /// </summary>
        public double? ValorOperacao { get; set; }
    }
}

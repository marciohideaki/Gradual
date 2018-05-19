using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Lista de status de resposta de mensagens.
    /// </summary>
    public enum MensagemResponseStatusEnum
    {
        /// <summary>
        /// Indica que a solicitação foi executada sem problemas
        /// e a resposta não contem nenhum erro.
        /// </summary>
        OK = 0,

        /// <summary>
        /// Indica que foram encontrados erros de negócio, provavelmente
        /// devido a alguma validação efetuada.
        /// </summary>
        ErroNegocio = 1,

        /// <summary>
        /// Indica que foram encontrados erros de programa, lógica, ou 
        /// qualquer outro erro inexperado.
        /// </summary>
        ErroPrograma = 2,

        /// <summary>
        /// Indica que não passou na camada de validação.
        /// Neste caso a mensagem deverá carregar uma lista de críticas
        /// </summary>
        ErroValidacao = 3,

        /// <summary>
        /// Indica que o processamento da mensagem não foi permitido por
        /// motivos de segurança
        /// </summary>
        AcessoNaoPermitido = 4,

        /// <summary>
        /// Indica que o processamento não poderá ocorrer por problemas
        /// na sessão. Sessão expirada ou número incorreto
        /// </summary>
        SessaoInvalida = 5
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Servico para persistencia de mensagens.
    /// Permite que as mensagens sejam, salvas e recuperadas por código ou algum outro filtro.
    /// </summary>
    public interface IServicoPersistenciaMensagens
    {
        /// <summary>
        /// Solicitação para salvar uma mensagem
        /// </summary>
        /// <param name="parametros">Parâmetros da solicitação</param>
        /// <returns></returns>
        SalvarMensagemResponse SalvarMensagem(SalvarMensagemRequest parametros);

        /// <summary>
        /// Solicitação para receber uma mensagem
        /// </summary>
        /// <param name="parametros">Parâmetros da solicitação</param>
        /// <returns></returns>
        ReceberMensagemResponse ReceberMensagem(ReceberMensagemRequest parametros);

        /// <summary>
        /// Solicitação de lista de mensagens
        /// </summary>
        /// <param name="parametros">Parâmetros da solicitação</param>
        /// <returns></returns>
        ListarMensagensResponse ListarMensagens(ListarMensagensRequest parametros);
    }
}

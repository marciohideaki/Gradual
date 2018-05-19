using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para receber a visao de funcionalidades de um usuario ou grupo
    /// </summary>
    public class ReceberVisaoFuncionalidadesRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo do usuário no qual montar a lista de funcionalidades
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Codigo do grupos de usuários no qual montar a lista de funcionalidades
        /// </summary>
        public string CodigoUsuarioGrupo { get; set; }

        /// <summary>
        /// Código do grupo de funcionalidades para gerar a visão
        /// </summary>
        public string CodigoGrupoFuncionalidade { get; set; }
    }
}

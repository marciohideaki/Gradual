using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar usuario grupo
    /// </summary>
    [Serializable]
    public class SalvarUsuarioGrupoRequest : MensagemRequestBase
    {
        /// <summary>
        /// UsuarioGrupo a ser salvo
        /// </summary>
        public UsuarioGrupoInfo UsuarioGrupo { get; set; }
    }
}

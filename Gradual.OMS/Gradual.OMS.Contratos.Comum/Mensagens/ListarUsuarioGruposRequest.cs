using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de consulta de usuarios grupo de acordo com o filtro informado
    /// </summary>
    [Serializable]
    public class ListarUsuarioGruposRequest : MensagemRequestBase
    {
        /// <summary>
        /// Permite filtro pelo nome do grupo
        /// </summary>
        public string FiltroNomeUsuarioGrupo { get; set; }

        /// <summary>
        /// Permite filtro pelo código do grupo
        /// </summary>
        public string FiltroCodigoUsuarioGrupo { get; set; }
    }
}

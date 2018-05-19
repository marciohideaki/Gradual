using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de usuario grupo
    /// </summary>
    [Serializable]
    public class ReceberUsuarioGrupoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do UsuarioGrupo desejado
        /// </summary>
        public string CodigoUsuarioGrupo { get; set; }

        /// <summary>
        /// Indica se as coleções Pefis2 e o detalhe
        /// de relações de permissões devem ser preenchidos
        /// </summary>
        public bool PreencherColecoesCompletas { get; set; }
    }
}

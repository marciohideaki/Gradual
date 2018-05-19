using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de usuário
    /// </summary>
    [Serializable]
    public class ReceberUsuarioRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do usuário a recuperar
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Email do usuário a recuperar
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Indica se as coleções UsuarioGrupos2, Pefis2 e o detalhe
        /// de relações de permissões devem ser preenchidos
        /// </summary>
        public bool PreencherColecoesCompletas { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de perfil
    /// </summary>
    [Serializable]
    public class ReceberPerfilRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do perfil desejado
        /// </summary>
        public string CodigoPerfil { get; set; }

        /// <summary>
        /// Indica se o detalhe de relações de permissões devem ser preenchidos
        /// </summary>
        public bool PreencherColecoesCompletas { get; set; }
    }
}

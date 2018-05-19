using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    /// <summary>
    /// Classe de info de vínculo de usuário e suas permissões
    /// </summary>
    public class UsuarioPermissaoInfo
    {
        /// <summary>
        /// Código do Usuário da intranet e sistemas da gradual
        /// Esse é o código de id_login da tabela de login do banco de dados do cadastro
        /// </summary>
        public int CodigoUsuario { get; set; }

        /// <summary>
        /// Lista guids (Códigos) de permissões que usuário têm para acessar nos 
        /// sistemas da gradual
        /// </summary>
        public List<string> ListaPermissoes { get; set; }

        #region Construtores
        /// <summary>
        /// Construtor da classe de Permissões
        /// </summary>
        public UsuarioPermissaoInfo()
        {
            ListaPermissoes = new List<string>();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Classe com informações de uma sessão.
    /// </summary>
    [Serializable]
    public class SessaoInfo : ICodigoEntidade
    {
        /// <summary>
        /// Chave primária da sessão
        /// </summary>
        public string CodigoSessao { get; set; }

        /// <summary>
        /// Código do sistema cliente autenticado na sessão.
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        /// <summary>
        /// Código do usuário autenticado na sessão.
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Data da criação da sessão.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Informa a data em que a sessão foi consultada pela última vez.
        /// </summary>
        public DateTime DataUltimaConsulta { get; set; }

        /// <summary>
        /// Indica se a sessao é do usuário administrador
        /// </summary>
        public bool EhSessaoDeAdministrador { get; set; }
        
        /// <summary>
        /// Construtor default
        /// </summary>
        public SessaoInfo()
        {
            this.CodigoSessao = Guid.NewGuid().ToString();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoSessao;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe com informações sobre uma instância de sessão
    /// </summary>
    public class Sessao
    {
        /// <summary>
        /// Informações do usuário
        /// </summary>
        public Usuario Usuario { get; set; }

        /// <summary>
        /// Informações da sessão
        /// </summary>
        public SessaoInfo SessaoInfo { get; set; }

        /// <summary>
        /// Permite que sejam adicionados objetos de contexto
        /// para a sessão.
        /// </summary>
        public ColecaoTipoInstancia Contexto { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public Sessao(SessaoInfo sessaoInfo)
        {
            this.SessaoInfo = sessaoInfo;
            this.Contexto = new ColecaoTipoInstancia();
        }
    }
}

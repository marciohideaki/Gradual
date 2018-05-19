using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Classe de dados que contem informações de uma sessão.
    /// Uma sessão é um acesso de uma aplicação cliente ao sistema de ordens.
    /// </summary>
    public class SessaoOrdensInfo
    {
        /// <summary>
        /// Chave primaria da sessao
        /// </summary>
        public string CodigoSessao { get; set; }

        /// <summary>
        /// Construtor. Cria código default para a sessao.
        /// </summary>
        public SessaoOrdensInfo()
        {
            this.CodigoSessao = Guid.NewGuid().ToString();
        }
    }
}

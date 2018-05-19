using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Classe de dados para realizar agrupamento de regras por 
    /// tipo de mensagem
    /// </summary>
    public class RegrasPorTipoInfo
    {
        /// <summary>
        /// Tipo da mensagem
        /// </summary>
        public Type TipoMensagem { get; set; }

        /// <summary>
        /// Campo auxiliar para serializar o tipo da mensagem
        /// </summary>
        public string TipoMensagemString { get; set; }

        /// <summary>
        /// Lista de regras para este tipo de mensagem
        /// </summary>
        public List<RegraBase> Regras { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public RegrasPorTipoInfo()
        {
            this.Regras = new List<RegraBase>();
        }
    }
}

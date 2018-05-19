using System;
using System.Collections.Generic;


namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Classe de configurações para o serviço de config
    /// </summary>
    [Serializable]
    public class ServicoValidacaoConfig
    {
        /// <summary>
        /// Coleção de regras por tipo de mensagem
        /// </summary>
        public List<RegrasPorTipoInfo> RegrasPorTipo { get; set; }

        /// <summary>
        /// Coleção de regras a serem aplicadas a todos os tipos
        /// </summary>
        public List<RegraInfo> RegrasGerais { get; set; }

        /// <summary>
        /// Coleção de geradores de regra a serem utilizados
        /// </summary>
        public List<GeradorRegraInfo> GeradoresRegra { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoValidacaoConfig()
        {
            this.RegrasGerais = new List<RegraInfo>();
            this.RegrasPorTipo = new List<RegrasPorTipoInfo>();
            this.GeradoresRegra = new List<GeradorRegraInfo>();
        }
    }
}

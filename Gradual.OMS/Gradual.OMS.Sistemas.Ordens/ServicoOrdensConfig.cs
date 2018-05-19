using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Sistemas.Ordens
{
    /// <summary>
    /// Classe de configuração do serviço de ordens.
    /// </summary>
    public class ServicoOrdensConfig
    {
        /// <summary>
        /// Diretório onde será gravado ou lido o arquivo de repositório de instrumentos.
        /// </summary>
        public string DiretorioRepositorioInstrumentos { get; set; }

        /// <summary>
        /// Prefixo do arquivo de repositório de instrumentos que será gravado ou lido. Ao prefixo é 
        /// adicionado o nome do canal e a extensão para compor o nome do arquivo.
        /// </summary>
        public string PrefixoRepositorioInstrumentos { get; set; }

        /// <summary>
        /// Regra para tradução da combinação Sistema + Bolsa + ClienteTipo para Canais.
        /// </summary>
        public List<RelacaoBolsaCanalInfo> RelacaoCanais { get; set; }

        public ServicoOrdensConfig()
        {
            this.DiretorioRepositorioInstrumentos = "";
            this.PrefixoRepositorioInstrumentos = "";
            this.RelacaoCanais = new List<RelacaoBolsaCanalInfo>();
        }
    }
}

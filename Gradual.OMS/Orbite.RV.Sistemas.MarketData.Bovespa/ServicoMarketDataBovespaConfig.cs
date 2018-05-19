using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.MarketData.Bovespa
{
    /// <summary>
    /// Classe de configuração do serviço de market data bovespa
    /// </summary>
    [Serializable]
    public class ServicoMarketDataBovespaConfig
    {
        /// <summary>
        /// Diretório que contém os arquivos bovespa (cotação, proventos, etc)
        /// </summary>
        public string DiretorioArquivosBovespa { get; set; }

        /// <summary>
        /// Indica se a inicialização deve ser feita de forma assincrona
        /// </summary>
        public bool InicializarAssincrono { get; set; }

        /// <summary>
        /// Meses iniciais a carregar
        /// </summary>
        public int MesesIniciaisACarregar { get; set; }
    }
}

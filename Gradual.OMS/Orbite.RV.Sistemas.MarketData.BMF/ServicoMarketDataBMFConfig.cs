using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.MarketData.BMF
{
    /// <summary>
    /// Configurações do serviço de market data BMF
    /// </summary>
    public class ServicoMarketDataBMFConfig
    {
        /// <summary>
        /// Diretório que contém os arquivos bmf (cotação, ativos, etc)
        /// </summary>
        public string DiretorioArquivosBMF { get; set; }
    }
}

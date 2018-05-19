using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Sistemas.MarketData
{
    /// <summary>
    /// Classe de configuração para o serviço de MarketData.
    /// </summary>
    public class ServicoMarketDataConfig
    {
        /// <summary>
        /// Lista de canais de market data a serem carregados
        /// </summary>
        public List<CanalInfo> Canais { get; set; }

        /// <summary>
        /// Indica se a inicialização do serviço deve ser feito de forma separada.
        /// Se sim, a inicialização é feita em uma thread separada. Deve-se tomar cuidado
        /// de dar o tempo suficiente para que a inicialização ocorra antes de utilizar 
        /// os serviços do market data. Como implementação, deve-se colocar um sinalizador
        /// indicando quando o serviço estiver totalmente inicializado.
        /// </summary>
        public bool InicializarEmThreadSeparada { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ServicoMarketDataConfig()
        {
            this.Canais = new List<CanalInfo>();
        }
    }
}

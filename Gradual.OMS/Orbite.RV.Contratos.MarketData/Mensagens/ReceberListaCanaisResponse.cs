using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.MarketData.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de canais
    /// </summary>
    public class ReceberListaCanaisResponse
    {
        /// <summary>
        /// Lista de canais
        /// </summary>
        public List<CanalInfo> Canais { get; set; }
    }
}

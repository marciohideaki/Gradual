using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.Comum;
using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.MarketData.Mensagens
{
    /// <summary>
    /// Mensagem para requisição de histórico de uma série.
    /// É uma mensagem base para as implementações dos canais.
    /// </summary>
    public abstract class ReceberSerieItensRequest
    {
    }
}

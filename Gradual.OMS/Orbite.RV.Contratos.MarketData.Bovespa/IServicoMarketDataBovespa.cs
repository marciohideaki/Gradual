using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library.Servicos;

using Orbite.RV.Contratos.MarketData.Bovespa.Mensagens;

namespace Orbite.RV.Contratos.MarketData.Bovespa
{
    /// <summary>
    /// Interface para o serviço de market data bovespa
    /// </summary>
    public interface IServicoMarketDataBovespa : IServicoControlavel
    {
        /// <summary>
        /// Solicita histórico de cotações de um instrumento bovespa
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberHistoricoCotacaoBovespaResponse ReceberHistoricoCotacaoBovespa(ReceberHistoricoCotacaoBovespaRequest parametros);

        /// <summary>
        /// Solicita lista de custos bovespa
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberCustosBovespaResponse ReceberCustosBovespa(ReceberCustosBovespaRequest parametros);

        /// <summary>
        /// Solicita detalhe de um instrumento bovespa
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberDetalheInstrumentoBovespaResponse ReceberDetalheInstrumentoBovespa(ReceberDetalheInstrumentoBovespaRequest parametros);

        /// <summary>
        /// Solicita a lista de instrumentos bovespa
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarInstrumentosBovespaResponse ListarInstrumentosBovespa(ListarInstrumentosBovespaRequest parametros);

        /// <summary>
        /// Solicita uma série bovespa. Pode ser série de desdobramentos, dividendos, juros, etc.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberSerieBovespaResponse ReceberSerieBovespa(ReceberSerieBovespaRequest parametros);

        /// <summary>
        /// Solicita ultima cotação de ativos
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberUltimaCotacaoBovespaResponse ReceberUltimaCotacaoBovespa(ReceberUltimaCotacaoBovespaRequest parametros);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library.Servicos;

using Orbite.RV.Contratos.MarketData.BMF.Mensagens;

namespace Orbite.RV.Contratos.MarketData.BMF
{
    /// <summary>
    /// Interface para o serviço de market data BMF
    /// </summary>
    public interface IServicoMarketDataBMF : IServicoControlavel
    {
        /// <summary>
        /// Solicita a lista de instrumentos BMF
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarInstrumentosBMFResponse ReceberListaInstrumentosBMF(ListarInstrumentosBMFRequest parametros);

        /// <summary>
        /// Solicita detalhe de um instrumento BMF
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberDetalheInstrumentoBMFResponse ReceberDetalheInstrumentoBMF(ReceberDetalheInstrumentoBMFRequest parametros);

        /// <summary>
        /// Solicita histórico de cotações de um instrumento BMF
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberHistoricoCotacaoBMFResponse ReceberHistoricoCotacaoBMF(ReceberHistoricoCotacaoBMFRequest parametros);

        /// <summary>
        /// Solicita ultima cotação de ativos
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberUltimaCotacaoBMFResponse ReceberUltimaCotacaoBMF(ReceberUltimaCotacaoBMFRequest parametros);
    }
}

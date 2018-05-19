using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Mensagens;

namespace Orbite.RV.Contratos.MarketData
{
    /// <summary>
    /// Interface para o serviço de MarketData.
    /// Este serviço é responsável por:
    /// - fornecer séries históricas de ações, opções, futuros, indicadores, etc
    /// - fornecer informações online
    /// - fornecer detalhes sobre ativos
    /// - etc
    /// </summary>
    public interface IServicoMarketData
    {
        /// <summary>
        /// Faz a assinatura de um book de ofertas.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        AssinarBookResponse AssinarBook(AssinarBookRequest parametros);

        /// <summary>
        /// Faz a assinatura de uma série online.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        AssinarSerieResponse AssinarSerie(AssinarSerieRequest parametros);

        /// <summary>
        /// Recebe o histórico de uma série, de acordo com os parametros informados.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberSerieHistoricoResponse ReceberSerieHistorico(ReceberSerieHistoricoRequest parametros);

        /// <summary>
        /// Consulta lista de instrumentos disponíveis. Permite listar todos ou aplicar filtros.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberListaInstrumentosResponse ReceberListaInstrumentos(ReceberListaInstrumentosRequest parametros);

        /// <summary>
        /// Consulta lista de séries disponíveis. Permite listar todos ou aplicar filtros.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberListaSeriesResponse ReceberListaSeries(ReceberListaSeriesRequest parametros);

    }
}

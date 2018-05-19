using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    [ServiceContract(Namespace = "http://gradual")]
    //[ServiceKnownType("RetornarTipos", typeof(MarketDataTiposHelper))]
    public interface IServicoMarketData
    {
        /// <summary>
        /// Faz a assinatura de uma série online.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        AssinarEventoMarketDataResponse AssinarEvento(AssinarEventoMarketDataRequest parametros);

        /// <summary>
        /// Recebe a lista de eventos disponíveis, de acordo com os parametros informados
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        ReceberListaEventosResponse ReceberListaEventos(ReceberListaEventosRequest parametros);

        /// <summary>
        /// Recebe o histórico de uma série, de acordo com os parametros informados.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        ReceberSerieItensResponse ReceberSerieItens(ReceberSerieItensRequest parametros);

        /// <summary>
        /// Consulta lista de séries disponíveis. Permite listar todos ou aplicar filtros.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        ReceberListaSeriesResponse ReceberListaSeries(ReceberListaSeriesRequest parametros);

        /// <summary>
        /// Recebe a lista de canais registrados no market data
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        ReceberListaCanaisResponse ReceberListaCanais(ReceberListaCanaisRequest parametros);
    }
}

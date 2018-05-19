using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

namespace Orbite.RV.Contratos.MarketData
{
    /// <summary>
    /// Classe base para a implementação de um canal de market data.
    /// </summary>
    public abstract class CanalMarketDataBase
    {
        /// <summary>
        /// Aponta para objeto que contém informações do canal.
        /// </summary>
        public CanalInfo Info { get; set; }

        /// <summary>
        /// Assinatura de evento
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AssinarEventoMarketDataRequest AssinarEvento(AssinarEventoMarketDataResponse parametros)
        {
            return OnAssinarEvento(parametros);
        }

        /// <summary>
        /// Método virtual para assinatura de evento
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected virtual AssinarEventoMarketDataRequest OnAssinarEvento(AssinarEventoMarketDataResponse parametros)
        {
            return null;
        }

        /// <summary>
        /// Receber lista de séries
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberListaSeriesResponse ReceberListaSeries(ReceberListaSeriesRequest parametros)
        {
            return OnReceberListaSeries(parametros);
        }

        /// <summary>
        /// Método virtual para receber lista de séries
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected virtual ReceberListaSeriesResponse OnReceberListaSeries(ReceberListaSeriesRequest parametros)
        {
            return null;
        }

        /// <summary>
        /// Recebe os dados da série solicitada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberSerieItensResponse ReceberSerieItens(ReceberSerieItensRequest parametros) 
        {
            return OnReceberSerieItens(parametros);
        }

        /// <summary>
        /// Método virtual para receber os dados da série
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected virtual ReceberSerieItensResponse OnReceberSerieItens(ReceberSerieItensRequest parametros) 
        {
            return null;
        }

        /// <summary>
        /// Evento para sinalizar chegada de evento
        /// </summary>
        public event EventHandler<SinalizarEventoRecebidoEventArgs> EventoSinalizarEventoRecebido;
    }
}

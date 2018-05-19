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
        public CanalMarketDataInfo Info { get; set; }

        /// <summary>
        /// Assinatura de livro de ofertas
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AssinarBookResponse AssinarBook(AssinarBookRequest parametros)
        {
            return OnAssinarBook(parametros);
        }

        /// <summary>
        /// Método virtual para assinatura de book
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected virtual AssinarBookResponse OnAssinarBook(AssinarBookRequest parametros)
        {
            return null;
        }

        /// <summary>
        /// Assinatura de série
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AssinarSerieRequest AssinarSerie(AssinarSerieResponse parametros)
        {
            return OnAssinarSerie(parametros);
        }

        /// <summary>
        /// Método virtual para assinatura série
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected virtual AssinarSerieRequest OnAssinarSerie(AssinarSerieResponse parametros)
        {
            return null;
        }

        /// <summary>
        /// Receber lista de instrumentos
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberListaInstrumentosResponse ReceberListaInstrumentos(ReceberListaInstrumentosRequest parametros)
        {
            return OnReceberListaInstrumentos(parametros);
        }

        /// <summary>
        /// Método virtual para receber lista de instrumentos
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected virtual ReceberListaInstrumentosResponse OnReceberListaInstrumentos(ReceberListaInstrumentosRequest parametros)
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
        public ReceberSerieHistoricoResponse ReceberSerieHistorico(ReceberSerieHistoricoRequest parametros) 
        {
            return OnReceberSerieHistorico(parametros);
        }

        /// <summary>
        /// Método virtual para receber os dados da série
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected virtual ReceberSerieHistoricoResponse OnReceberSerieHistorico(ReceberSerieHistoricoRequest parametros) 
        {
            return null;
        }

        /// <summary>
        /// Evento para sinalizar chegada de item de book
        /// </summary>
        public event EventHandler<SinalizarBookItemRecebidoEventArgs> EventoSinalizarBookItemRecebido;

        /// <summary>
        /// Evento para sinalizar chegada de item de série
        /// </summary>
        public event EventHandler<SinalizarSerieItemRecebidoEventArgs> EventoSinalizarSerieItemRecebido;

    }
}

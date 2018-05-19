using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.PositionClient.Monitor.Lib.Message
{
    /// <summary>
    /// Classe de request do socket para efetuar o filtro 
    /// da descida dos objeto no socket da aplicação conectada
    /// </summary>
    public class BuscarOperacoesIntradaySocketRequest
    {
        /// <summary>
        /// Código do cliente
        /// </summary>
        public int CodigoCliente { get; set; }

        /// <summary>
        /// Filtro do Código Instrumento
        /// </summary>
        public string CodigoInstrumento { get; set; }

        /// <summary>
        /// Filtro de Todos os mercados
        /// </summary>
        public bool OpcaoMarketTodosMercados { get; set; }

        /// <summary>
        /// Filtro de Mercado a vista
        /// </summary>
        public bool OpcaoMarketAVista { get; set; }

        /// <summary>
        /// Filtro de Mercado BMF
        /// </summary>
        public bool OpcaoMarketFuturos { get; set; }

        /// <summary>
        /// Filtro de Opção
        /// </summary>
        public bool OpcaoMarketOpcao { get; set; }

        /// <summary>
        /// Filtro de Ofertas na Pedra
        /// </summary>
        public bool OpcaoParametroIntradayOfertasPedra { get; set; }

        /// <summary>
        /// Filtro de Net Negativo
        /// </summary>
        public bool OpcaoParametroIntradayNetNegativo { get; set; }

        /// <summary>
        /// Filtro de PL Negativo
        /// </summary>
        public bool OpcaoParametroIntradayPLNegativo { get; set; }

    }
}

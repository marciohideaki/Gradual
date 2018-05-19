using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Dados
{
    /// <summary>
    /// Contem informações sobre uma série.
    /// Utilizado tanto para retornar informações de uma série, quanto 
    /// fazer a solicitação de informações. 
    /// Nem todos os campos são obrigatórios.
    /// </summary>
    public class SerieInfo
    {
        /// <summary>
        /// Contem a lista de canais que disponibilizam esta série. 
        /// Para cada canal, indica as características da série oferecida.
        /// </summary>
        public Dictionary<CanalMarketDataInfo, SerieInfo> Canais { get; set; }
        
        /// <summary>
        /// Indica se esta série é default no contexto informado.
        /// Por exemplo, quando este item estiver na coleção de canais de uma 
        /// determinada série, indica se este item é o default, ou seja, elegido 
        /// quando não é especificado o canal a ser utilizado.
        /// </summary>
        public bool? EhDefault { get; set; }
        
        /// <summary>
        /// Contém informações sobre o instrumento da série.
        /// </summary>
        public InstrumentoInfo Instrumento { get; set; }
        
        /// <summary>
        /// Quando estiver retornando informações sobre uma série, indica se a 
        /// série tem histórico.
        /// </summary>
        public bool? TemHistorico { get; set; }
        
        /// <summary>
        /// Quando estiver retornando informações sobre uma série, indica se a 
        /// série tem online.
        /// </summary>
        public bool? TemOnLine { get; set; }
        
        /// <summary>
        /// Informa o tipo da série, de ticks, de negócios, de notícias (futuramente).
        /// </summary>
        public SerieTipoEnum Tipo { get; set; }

        /// <summary>
        /// Caso o tipo da série seja outros, especifica o tipo da série
        /// </summary>
        public string EspecificacaoOutros { get; set; }
        
        /// <summary>
        /// Indica o tipo de dados dos itens retornados pela série.
        /// </summary>
        public Type TipoDados { get; set; }

        /// <summary>
        /// Indica o tipo da série retornada.
        /// </summary>
        public Type TipoSerie { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public SerieInfo()
        {
            this.Tipo = SerieTipoEnum.Desconhecido;
            this.Canais = new Dictionary<CanalMarketDataInfo, SerieInfo>();
        }
    }
}

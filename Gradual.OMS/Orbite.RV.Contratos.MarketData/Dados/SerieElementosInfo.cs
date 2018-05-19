using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.Comum;

namespace Orbite.RV.Contratos.MarketData.Dados
{
    /// <summary>
    /// Contém uma lista de elementos de uma série. Indica as condições que estes elementos estão 
    /// carregados, por exemplo, faixa de datas, se é online, etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SerieElementosInfo<T>
    {
        /// <summary>
        /// Indica o canal preencheu esta coleção de elementos.
        /// </summary>
        public string Canal { get; set; }
        
        /// <summary>
        /// Indica a data final dos elementos carregados.
        /// </summary>
        public DateTime DataFinal { get; set; }
        
        /// <summary>
        /// Indica a data inicial dos elementos carregados.
        /// </summary>
        public DateTime DataInicial { get; set; }
        
        /// <summary>
        /// Contém a coleção de elementos.
        /// </summary>
        public List<T> Elementos { get; set; }
        
        /// <summary>
        /// Indica se a série é online.
        /// </summary>
        public bool OnLine { get; set; }
        
        /// <summary>
        /// Indica o período que os elementos estão sumarizados
        /// </summary>
        public PeriodoEnum Periodo { get; set; }
        
        /// <summary>
        /// Caso o período seja personalizado, indica o intervalo período.
        /// </summary>
        public TimeSpan PeriodoPersonalizado { get; set; }

        /// <summary>
        /// Indica a série a que estes elementos pertencem.
        /// </summary>
        public SerieDescricaoInfo Serie { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public SerieElementosInfo()
        {
            this.Elementos = new List<T>();
        }
    }
}

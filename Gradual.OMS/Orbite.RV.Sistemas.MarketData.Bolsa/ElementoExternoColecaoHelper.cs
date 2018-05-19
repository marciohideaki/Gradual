using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.MarketData.Bolsa
{
    /// <summary>
    /// Coleção baseada em ElementoExternoHelper que mantem dois indices, uma pela chave interna e 
    /// outro pela chave externa, permitindo que os itens sejam acessados tanto pela chave interna
    /// quanto pela externa.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ElementoExternoColecaoHelper<T>
    {
        /// <summary>
        /// Dicionario com indice pela chave externa
        /// </summary>
        public Dictionary<string, ElementoExternoHelper<T>> DicionarioChaveExterno { get; set; }

        /// <summary>
        /// Dicionario com indice pela chave interna
        /// </summary>
        public Dictionary<T, ElementoExternoHelper<T>> DicionarioChaveInterno { get; set; }

        /// <summary>
        /// Construtor. Inicializa as coleções
        /// </summary>
        public ElementoExternoColecaoHelper()
        {
            this.DicionarioChaveExterno = new Dictionary<string, ElementoExternoHelper<T>>();
            this.DicionarioChaveInterno = new Dictionary<T, ElementoExternoHelper<T>>();
        }

        /// <summary>
        /// Adiciona um novo elemento nas duas coleções
        /// </summary>
        /// <param name="elementoExterno"></param>
        /// <param name="elementoInterno"></param>
        public void Adicionar(string elementoExterno, T elementoInterno)
        {
            ElementoExternoHelper<T> elemento = new ElementoExternoHelper<T>();
            elemento.ElementoExterno = elementoExterno;
            elemento.ElementoInterno = elementoInterno;

            this.DicionarioChaveExterno.Add(elemento.ElementoExterno, elemento);
            this.DicionarioChaveInterno.Add(elemento.ElementoInterno, elemento);
        }

        /// <summary>
        /// Remove um elemento pela chave externa
        /// </summary>
        /// <param name="elementoExterno"></param>
        public void RemoverPorChaveExterna(string elementoExterno)
        {
            ElementoExternoHelper<T> elemento = this.DicionarioChaveExterno[elementoExterno];
            this.DicionarioChaveExterno.Remove(elemento.ElementoExterno);
            this.DicionarioChaveInterno.Remove(elemento.ElementoInterno);
        }

        /// <summary>
        /// Remove um elemento pela chave interna
        /// </summary>
        /// <param name="elementoInterno"></param>
        public void RemoverPorChaveInterna(T elementoInterno)
        {
            ElementoExternoHelper<T> elemento = this.DicionarioChaveInterno[elementoInterno];
            this.DicionarioChaveExterno.Remove(elemento.ElementoExterno);
            this.DicionarioChaveInterno.Remove(elemento.ElementoInterno);
        }
    }
}

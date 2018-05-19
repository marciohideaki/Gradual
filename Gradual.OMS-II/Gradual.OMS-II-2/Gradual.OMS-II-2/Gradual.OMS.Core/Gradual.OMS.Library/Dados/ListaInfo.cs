using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Representa uma lista de elementos.
    /// O objetivo desta lista é servir de metadados para entidades 
    /// bancos de dados. Geralmente essas listas serão itens mapeados
    /// de enumeradores, ou classes, etc, ou seja, artefatos fixos
    /// usados nos programas. Mas nada impede que estas listas tenham
    /// elementos de outras origens.
    /// </summary>
    [Serializable]
    public class ListaInfo : ICodigoEntidade
    {
        /// <summary>
        /// Descrição da lista
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Mnemonico da lista.
        /// Geralmente um código que poderá ser referenciado através de 
        /// algum tipo de rotina. Seja stored procedures, programas intermediários,
        /// etc.
        /// </summary>
        public string Mnemonico { get; set; }

        /// <summary>
        /// Itens da lista
        /// </summary>
        public List<ListaItemInfo> Itens { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListaInfo()
        {
            this.Itens = new List<ListaItemInfo>();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

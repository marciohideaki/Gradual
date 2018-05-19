using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum
{
    /// <summary>
    /// Contem informacoes de um item de persistencia do 
    /// layout de controles devexpress.
    /// Carrega o nome do controle, o nome do controle pai, 
    /// caso exista, etc.
    /// </summary>
    [Serializable]
    public class LayoutDevExpressItemInfo
    {
        /// <summary>
        /// Nome do controle alvo. Pode ser uma view dentro de um
        /// grid.
        /// </summary>
        public string NomeControle { get; set; }

        /// <summary>
        /// Nome do controle pai. No caso de views, contem o nome
        /// do grid.
        /// </summary>
        public string NomeControlePai { get; set; }

        /// <summary>
        /// Tipo do controle. Indica se é uma view ou um layoutMananger
        /// </summary>
        public LayoutDevExpressTipoEnum TipoControle { get; set; }

        /// <summary>
        /// Serialização do layout
        /// </summary>
        public string Layout { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// EventArgs para inicialização do nó
    /// </summary>
    public class ArvoreNoInicializarEventArgs<T, U> : EventArgs
    {
        /// <summary>
        /// Referencia para o nó que está sendo inicializado
        /// </summary>
        public ArvoreNo<T, U> ArvoreNo { get; set; }
    }
}

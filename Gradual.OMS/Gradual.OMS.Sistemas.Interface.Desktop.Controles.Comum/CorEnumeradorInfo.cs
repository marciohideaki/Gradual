using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum
{
    /// <summary>
    /// Classe para armazenar a cor de um determinado enumerador
    /// para colorir linhas de grid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class CorEnumeradorInfo<T>
    {
        /// <summary>
        /// Valor do item a ser colorido
        /// </summary>
        public T Valor { get; set; }

        /// <summary>
        /// Cor do item
        /// </summary>
        public Color Cor{ get; set; }

        /// <summary>
        /// Indica se o estilo deve ser aplicado na linha inteira
        /// </summary>
        public bool LinhaInteira { get; set; }

        /// <summary>
        /// Representa o objeto em string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Valor.ToString();
        }
    }
}

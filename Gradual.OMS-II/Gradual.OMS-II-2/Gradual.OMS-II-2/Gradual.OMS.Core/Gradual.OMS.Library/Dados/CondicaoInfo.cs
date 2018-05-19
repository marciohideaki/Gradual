using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Classe para representar condições flexíveis de pesquisa
    /// de objetos. 
    /// </summary>
    public class CondicaoInfo
    {
        #region Propriedades

        /// <summary>
        /// Indica a propriedade em que será verificada a condição
        /// </summary>
        public string Propriedade { get; set; }

        /// <summary>
        /// Indica o tipo da condição
        /// </summary>
        public CondicaoTipoEnum TipoCondicao { get; set; }

        /// <summary>
        /// Valor ou valores a serem testados
        /// </summary>
        public object[] Valores { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public CondicaoInfo()
        {
        }

        /// <summary>
        /// Construtor que aceita diretamente as propriedades e apenas 1 valor.
        /// </summary>
        /// <param name="propriedade"></param>
        /// <param name="tipoCondicao"></param>
        /// <param name="valor"></param>
        public CondicaoInfo(string propriedade, CondicaoTipoEnum tipoCondicao, object valor)
        {
            this.Propriedade = propriedade;
            this.TipoCondicao = tipoCondicao;
            this.Valores = new object[] { valor };
        }

        /// <summary>
        /// Construtor que aceita diretamente as propriedades e diversos valores.
        /// </summary>
        /// <param name="propriedade"></param>
        /// <param name="tipoCondicao"></param>
        /// <param name="valores"></param>
        public CondicaoInfo(string propriedade, CondicaoTipoEnum tipoCondicao, params object[] valores)
        {
            this.Propriedade = propriedade;
            this.TipoCondicao = tipoCondicao;
            if (valores != null)
                this.Valores = valores;
            else
                this.Valores = new object[] { null };
        }

        #endregion

        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}

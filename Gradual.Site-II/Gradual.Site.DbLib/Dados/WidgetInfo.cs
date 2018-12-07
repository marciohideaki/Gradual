using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Dados
{
    [Serializable]
    [DataContract]
    public class WidgetInfo
    {
        
        /// <summary>
        /// ID da tabela TB_Widget
        /// </summary>
        [DataMember]
        public int CodigoWidget { get; set; }

        /// <summary>
        /// ID da tabela TB_ESTRUTURA
        /// </summary>
        [DataMember]
        public int CodigoEstrutura { get; set; }

        /// <summary>
        /// ID_CONTEUDO
        /// </summary>
        [DataMember]
        public int CodigoListaConteudo { get; set; }
            
        /// <summary>
        /// Objeto Listaconteudo
        /// </summary>
        [DataMember]
        public ListaConteudoInfo ObjetoListaConteudo { get; set; }

        /// <summary>
        /// Descrição widget Json
        /// </summary>
        [DataMember]
        public string WidgetJson { get; set; }

        /// <summary>
        /// Numero de ordem na pagina
        /// </summary>
        [DataMember]
        public int OrdemPagina { get; set; }

    }
}

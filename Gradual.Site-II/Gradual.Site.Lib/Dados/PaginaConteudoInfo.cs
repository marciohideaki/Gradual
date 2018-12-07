using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.Lib.Mensagens;

namespace Gradual.Site.Lib.Dados
{
    [Serializable]
    [DataContract]
    public class PaginaConteudoInfo
    {
        /// <summary>
        /// ID da tabela
        /// </summary>
        [DataMember]
        public int CodigoPagina { get; set; }

        /// <summary>
        /// Nome da Pagina
        /// </summary>
        [DataMember]
        public string NomePagina { get; set; }

        /// <summary>
        /// Descrição da URL
        /// </summary>
        [DataMember]
        public string DescURL { get; set; }


        /// <summary>
        /// Json Widget 
        /// </summary>
        [DataMember]
        public string WidgetJson { get; set; }

        /// <summary>
        /// Json Conteudo 
        /// </summary>
        [DataMember]
        public string ConteudoJson { get; set; }

        /// <summary>
        /// Conteudo HTML 
        /// </summary>
        [DataMember]
        public string ConteudoHTML { get; set; }

        /// <summary>
        /// Conteudo usado para o filtro
        /// </summary>
        [DataMember]
        public string ConteudoTermo { get; set; }
    }
}

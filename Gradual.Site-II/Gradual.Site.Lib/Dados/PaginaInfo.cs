using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.Lib.Mensagens;

namespace Gradual.Site.Lib.Dados
{
    [Serializable]
    [DataContract]
    public class PaginaInfo
    {
        /// <summary>
        /// ID da tabela
        /// </summary>
        [DataMember]
        public Nullable<int> CodigoPagina { get; set; }

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
        /// Tipo de estrutura
        /// </summary>
        [DataMember]
        public string TipoEstrutura { get; set; }

        /// <summary>
        /// Lista de estrutura
        /// </summary>
        [DataMember]
        public List<EstruturaInfo> ListaEstrutura { get; set; }

    }
}

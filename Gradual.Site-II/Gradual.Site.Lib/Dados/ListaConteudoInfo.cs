using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.Lib.Mensagens;

namespace Gradual.Site.Lib.Dados
{
    [Serializable]
    [DataContract]
    public class ListaConteudoInfo
    {
        /// <summary>
        /// ID da tabela
        /// </summary>
        [DataMember]
        public int CodigoLista { get; set; }

        /// <summary>
        /// ID do conteudo
        /// </summary>
        [DataMember]
        public int CodigoTipoConteudo { get; set; }

        /// <summary>
        /// Descrição da Regra
        /// </summary>
        [DataMember]
        public string Regra { get; set; }

        /// <summary>
        /// Descrição da Lista
        /// </summary>
        [DataMember]
        public string DescricaoLista { get; set; }

        /// <summary>
        /// Objeto conteudo
        /// </summary>
        [DataMember]
        public TipoDeConteudoInfo ObjetoTipoConteudo { get; set; }
    }
}

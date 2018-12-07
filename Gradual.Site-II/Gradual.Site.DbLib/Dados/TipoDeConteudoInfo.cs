using System;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Dados
{
    [Serializable]
    [DataContract]
    public class TipoDeConteudoInfo
    {
        /// <summary>
        /// ID do tipo de conteudo
        /// </summary>
        [DataMember]
        public int IdTipoConteudo { get; set; }
        
        /// <summary>
        /// Nome / Descrição do tipo de conteúdo
        /// </summary>
        [DataMember]
        public string Descricao { get; set; }
        
        /// <summary>
        /// Nome propriedade1
        /// </summary>
        [DataMember]
        public string NomePropriedade1 { get; set; }

        /// <summary>
        /// Nome propriedade2
        /// </summary>
        [DataMember]
        public string NomePropriedade2 { get; set; }

        /// <summary>
        /// Nome propriedade3
        /// </summary>
        [DataMember]
        public string NomePropriedade3 { get; set; }

        /// <summary>
        /// Nome propriedade4
        /// </summary>
        [DataMember]
        public string NomePropriedade4 { get; set; }

        /// <summary>
        /// Nome propriedade5
        /// </summary>
        [DataMember]
        public string NomePropriedade5 { get; set; }

        /// <summary>
        /// JSON descritivo das propriedades do conteúdo
        /// </summary>
        [DataMember]
        public string TipoDeConteudoJson { get; set; }

    }
}

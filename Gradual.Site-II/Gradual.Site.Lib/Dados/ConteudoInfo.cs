using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.Lib.Mensagens;
using Newtonsoft.Json;

namespace Gradual.Site.Lib.Dados
{
    [Serializable]
    [DataContract]
    public class ConteudoInfo
    {
        /// <summary>
        /// ID da tabela
        /// </summary>
        [DataMember]
        public int CodigoConteudo { get; set; }

        /// <summary>
        /// ID do tipo de conteudo
        /// </summary>
        [DataMember]
        public int CodigoTipoConteudo { get; set; }

        /// <summary>
        /// Data de Criação
        /// </summary>
        [DataMember]
        public DateTime? DtCriacao { get; set; }

        [DataMember]
        public DateTime? DtInicio { get; set; }

        [DataMember]
        public DateTime? DtFim { get; set; }


        [DataMember]
        public string ValorTag { get; set; }

        /// <summary>
        /// Nome propriedade1
        /// </summary>
        [DataMember]
        public string ValorPropriedade1 { get; set; }

        /// <summary>
        /// Nome propriedade2
        /// </summary>
        [DataMember]
        public string ValorPropriedade2 { get; set; }

        /// <summary>
        /// Nome propriedade3
        /// </summary>
        [DataMember]
        public string ValorPropriedade3 { get; set; }

        /// <summary>
        /// Nome propriedade4
        /// </summary>
        [DataMember]
        public string ValorPropriedade4 { get; set; }

        /// <summary>
        /// Nome propriedade5
        /// </summary>
        [DataMember]
        public string ValorPropriedade5 { get; set; }

        [DataMember]
        public string DataConsulta { get; set; }
        
        /// <summary>
        /// Conteudo do JSON
        /// </summary>
        [DataMember]
        public string ConteudoJson { get; set; }
        
        /// <summary>
        /// Conteudo do JSON
        /// </summary>
        [DataMember]
        public string ConteudoHtml { get; set; }

        [JsonIgnore]
        public string ConteudoJsonComPropriedadesExtras
        {
            get
            {
                string lConteudoJson = this.ConteudoJson;

                lConteudoJson = lConteudoJson.TrimStart('{');

                lConteudoJson = string.Format(" CodigoConteudo: \"{0}\", {1}"
                                                , this.CodigoConteudo
                                                , lConteudoJson);

                lConteudoJson = "{" + lConteudoJson;

                return lConteudoJson;
            }
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Dados
{
    [Serializable]
    [DataContract]
    public class VersaoInfo
    {
        #region Propriedades

        /// <summary>
        /// ID da versão no banco
        /// </summary>
        [DataMember]
        public Nullable<int> CodigoVersao { get; set; }

        [DataMember]
        public int CodigoPagina { get; set; }

        [DataMember]
        public string CodigoDeIdentificacao { get; set; }

        [DataMember]
        public DateTime DataCriacao { get; set; }

        [DataMember]
        public bool Publicada { get; set; }

        /// <summary>
        /// Lista de estruturas dessa versão
        /// </summary>
        [DataMember]
        public List<EstruturaInfo> ListaEstrutura { get; set; }

        #endregion

        #region Constructor

        public VersaoInfo()
        {
            this.ListaEstrutura = new List<EstruturaInfo>();
        }

        #endregion
    }
}

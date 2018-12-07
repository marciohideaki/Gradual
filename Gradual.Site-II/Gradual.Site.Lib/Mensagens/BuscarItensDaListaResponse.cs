using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.Lib.Dados;

namespace Gradual.Site.Lib.Mensagens
{
    public class BuscarItensDaListaResponse : MensagemResponseBase
    {
        [DataMember]
        public List<ConteudoInfo> Itens { get; set; }
        
        [DataMember]
        public int IdTipoConteudo { get; set; }
        
        [DataMember]
        public string DescricaoDaLista { get; set; }

        #region Construtor

        public BuscarItensDaListaResponse()
        {
            this.Itens = new List<ConteudoInfo>();
        }

        #endregion
    }
}

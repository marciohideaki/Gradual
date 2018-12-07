using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.Comercial;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class BuscarDadosDosProdutosResponse : MensagemResponseBase
    {
        [DataMember]
        public List<ProdutoInfo> DadosDosProdutos { get; set; }
    }
}

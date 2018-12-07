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
    public class InserirProdutoPorTransacaoRequest : MensagemRequestBase
    {
        [DataMember]
        public ClienteProdutoInfo ClienteProduto { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.Lib.Mensagens;

namespace Gradual.Site.Lib.Dados.MinhaConta
{
    [Serializable]
    [DataContract]
    public class AtivoInfo
    {
        public string CodigoAtivo { get; set; }

        public int CodigoProduto { get; set; }
    }
}

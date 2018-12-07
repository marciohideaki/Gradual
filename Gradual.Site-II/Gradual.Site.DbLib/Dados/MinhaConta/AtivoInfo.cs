using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Dados.MinhaConta
{
    [Serializable]
    [DataContract]
    public class AtivoInfo
    {
        public string CodigoAtivo { get; set; }

        public int CodigoProduto { get; set; }
    }
}

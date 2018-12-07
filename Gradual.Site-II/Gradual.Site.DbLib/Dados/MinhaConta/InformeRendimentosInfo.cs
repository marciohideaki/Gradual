using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Dados.MinhaConta
{
    [Serializable]
    [DataContract]
    public class InformeRendimentosInfo
    {
        public string Data { get; set; }

        public Nullable<decimal> Rendimento { get; set; }

        public Nullable<decimal> Imposto { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public Int64 CPF { get; set; }

        public DateTime DataNascimento { get; set; }

        public int CondicaoDependente { get; set; }

        public int CondicaoRetencao { get; set; }
    }
}

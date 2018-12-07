using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.Lib.Mensagens;

namespace Gradual.Site.Lib.Dados.MinhaConta
{
    [Serializable]
    [DataContract]
    public class InformeRendimentosTesouroInfo
    {
        public string Posicao { get; set; }

        public Nullable<decimal> QuantidadeAnoAnterior { get; set; }

        public Nullable<decimal> ValorAnoAnterior { get; set; }

        public Nullable<decimal> Quantidade { get; set; }

        public Nullable<decimal> Valor { get; set; }

        public DateTime AnoAtual { get; set; }
        
        public DateTime AnoAnterior { get; set; }

        public Int64 CPF { get; set; }

        public DateTime DataNascimento { get; set; }

        public int CondicaoDependente { get; set; }
    }
}

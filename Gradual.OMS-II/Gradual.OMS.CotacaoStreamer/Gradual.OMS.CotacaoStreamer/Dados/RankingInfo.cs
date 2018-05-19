using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    [Serializable]
    public class RankingInfo : IComparable<RankingInfo>
    {
        /// <summary>
        /// Valor para a ordenação do Ranking
        /// </summary>
        [DataMember]
        public double Valor { get; set; }

        /// <summary>
        /// Código do Instrumento ou Código da Corretora
        /// </summary>
        [DataMember]
        public string Adicional { get; set; }

        public RankingInfo(double Valor, string Adicional)
        {
            this.Valor = Valor;
            this.Adicional = Adicional;
        }

        public int CompareTo(RankingInfo obj)
        {
            int comparaValor = Valor.CompareTo(obj.Valor);
            if (comparaValor != 0)
                return comparaValor;

            return Adicional.CompareTo(obj.Adicional);
        }
    }
}

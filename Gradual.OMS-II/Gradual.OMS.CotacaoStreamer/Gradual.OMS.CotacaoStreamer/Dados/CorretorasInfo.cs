using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    [Serializable]
    public class CorretorasInfo : IComparable<CorretorasInfo>
    {
        /// <summary>
        /// Ordenar Ranking de Corretoras por Volume Bruto (Volume de Compras + Volume de Vendas)
        /// </summary>
        [DataMember]
        public double VolumeBruto { get; set; }

        /// <summary>
        /// Código da Corretora
        /// </summary>
        [DataMember]
        public string Corretora { get; set; }

        /// <summary>
        /// Volume de Compras
        /// </summary>
        [DataMember]
        public double VolumeCompra { get; set; }

        /// <summary>
        /// Volume de Vendas
        /// </summary>
        [DataMember]
        public double VolumeVenda { get; set; }

        /// <summary>
        /// Volume Liquido (Volume de Compras - Volume de Vendas)
        /// </summary>
        [DataMember]
        public double VolumeLiquido { get; set; }

        public CorretorasInfo()
        {
        }

        public CorretorasInfo(double VolumeBruto, string Corretora, double VolumeCompra, double VolumeVenda, double VolumeLiquido)
        {
            this.VolumeBruto = VolumeBruto;
            this.Corretora = Corretora;
            this.VolumeCompra = VolumeCompra;
            this.VolumeVenda = VolumeVenda;
            this.VolumeLiquido = VolumeLiquido;
        }

        public int CompareTo(CorretorasInfo obj)
        {
            int comparaVolumeBruto = VolumeBruto.CompareTo(obj.VolumeBruto);
            if (comparaVolumeBruto != 0)
                return comparaVolumeBruto;

            return Corretora.CompareTo(obj.Corretora);
        }
    }
}

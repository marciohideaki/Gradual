using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoDoClienteCorretagemInfo : ICodigoEntidade
    {
        public int? ConsultaCdCliente { get; set; }

        public string ConsultaCodigoAssessor { get; set; }

        public string ConsultaNmCliente { get; set; }

        public decimal VlCorretagemMes { get; set; }

        public decimal VlCorretagemMediaAno { get; set; }

        public decimal VlCorretagemEm12Meses { get; set; }

        public decimal VlVolumeMes { get; set; }

        public decimal VlVolumeMediaAno { get; set; }

        public decimal VlVolumeEm12Meses { get; set; }

        public decimal VlDisponivel { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}

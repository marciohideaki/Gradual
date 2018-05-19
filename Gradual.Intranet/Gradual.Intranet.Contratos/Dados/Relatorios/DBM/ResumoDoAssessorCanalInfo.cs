using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoDoAssessorCanalInfo : ICodigoEntidade
    {
        public int? ConsultaCodigoAssessor { get; set; }

        public DateTime ConsultaDataFinal { get; set; }

        public DateTime ConsultaDataInicial { get; set; }

        public decimal QtHbValor { get; set; }

        public decimal VlHbPercentual { get; set; }

        public decimal QtRepassadorValor { get; set; }

        public decimal VlRepassadorPercentual { get; set; }

        public decimal QtMesaValor { get; set; }

        public decimal VlMesaPercentual { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}

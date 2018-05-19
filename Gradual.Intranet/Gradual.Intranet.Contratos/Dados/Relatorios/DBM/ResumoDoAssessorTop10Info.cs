using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoDoAssessorTop10Info : ICodigoEntidade
    {
        public DateTime ConsultaDataInicial { get; set; }

        public DateTime ConsultaDataFinal { get; set; }

        public int? ConsultaCodigoAssessor { get; set; }

        public string DsNomeCliente { get; set; }

        public decimal VlCorretagem { get; set; }

        public decimal VlPercentualTotal { get; set; }

        public decimal VlPercentualAcumulado { get; set; }

        public decimal VlPercentualDevMedia { get; set; }

        public decimal VlCustodia { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoDoAssessorCadastroInfo : ICodigoEntidade
    {
        public int? ConsultaCodigoAssessor { get; set; }

        public int QtTotalClientes { get; set; }

        public int QtClientesAtivos { get; set; }

        public int QtClientesInativos { get; set; }

        public int QtClientesNoVarejo { get; set; }

        public int QtClientesInstitucional { get; set; }

        public int QtClientesNovos { get; set; }

        public decimal VlPercentOperouNoMes { get; set; }

        public decimal VlPercenturalComCustodia { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}

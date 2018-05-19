using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoDoAssessorMetricasInfo : ICodigoEntidade
    {
        public int? ConsultaCdAssessor { get; set; }
        public DateTime ConsultaDataInicio { get; set; }
        public DateTime ConsultaDataFim { get; set; }
        public decimal VlCorretagemDia { get; set; }
        public decimal VlCorretagemMes { get; set; }
        public decimal VlCorretagemAno { get; set; }
        public decimal VlCorretagemMesAnterior { get; set; }
        public decimal QtCadastrosMediaAno { get; set; }
        public int QtCadastrosDia { get; set; }
        public int QtCadastrosMes { get; set; }
        public int QtCadastrosMesAnterior { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}

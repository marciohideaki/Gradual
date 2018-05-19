using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Cliente
{
    public class PosicaoConsolidadaPorPapelRelInfo : ICodigoEntidade
    {
        public string ConsultaInstrumento { get; set; }

        public string ConsultaCodigoAssessor { get; set; }

        public int ClienteCodigo { get; set; }

        public string ClienteNome { get; set; }

        public string ClienteTipo { get; set; }

        public int AssessorCodigo { get; set; }

        public string AssessorNome { get; set; }

        public string CodigoNegocio { get; set; }

        public string DescricaoCarteira { get; set; }

        public int Locador { get; set; }

        public int QuantidadeDisponivel { get; set; }

        public int QuantidadeTotal { get; set; }

        public int QuantidadeD1 { get; set; }

        public int QuantidadeD2 { get; set; }

        public int QuantidadeD3 { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}

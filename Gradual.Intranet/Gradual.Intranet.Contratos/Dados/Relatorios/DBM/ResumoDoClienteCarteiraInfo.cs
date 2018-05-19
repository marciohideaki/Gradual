using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoDoClienteCarteiraInfo : ICodigoEntidade
    {
        public int? ConsultaCodigoCliente { get; set; }

        public string ConsultaCodigoAssessor { get; set; }

        public string ConsultaNomeCliente { get; set; }

        public string DsCarteira { get; set; }

        public decimal VlCotacao { get; set; }

        public int QtQuantidade { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}

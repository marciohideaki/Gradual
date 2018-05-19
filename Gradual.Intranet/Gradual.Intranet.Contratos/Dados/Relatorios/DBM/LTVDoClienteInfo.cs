using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class LTVDoClienteInfo : ICodigoEntidade
    {
        public int ConsultaCodigoCliente { get; set; }

        public string ConsultaCodigoAssessor { get; set; }

        public DateTime ConsultaDataDe { get; set; }

        public DateTime ConsultaDataAte { get; set; }

        public string CodigoCliente { get; set; }

        public string NomeCliente { get; set; }

        public string MesNegocio { get; set; }

        public decimal ValorCorretagemPorPeriodo { get; set; }

        public decimal ValorVolumePorPeriodo { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}

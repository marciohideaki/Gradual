using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class MovimentoDeContaCorrenteInfo : ICodigoEntidade
    {
        public string ConsultaCodigoAssessor { get; set; }

        public DateTime ConsultaDataLancamento { get; set; }

        public int CdCliente { get; set; }

        public string NmCliente { get; set; }

        public DateTime DtLancamento { get; set; }

        public DateTime DtReferencia { get; set; }

        public DateTime DtLiquidacao { get; set; }

        public string DsLancamento { get; set; }

        public decimal VlLancamento { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

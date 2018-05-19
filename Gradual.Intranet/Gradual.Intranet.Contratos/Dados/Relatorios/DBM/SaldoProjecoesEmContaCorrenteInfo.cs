using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class SaldoProjecoesEmContaCorrenteInfo : ICodigoEntidade
    {
        public string ConsultaCdAssessor { get; set; }

        public DateTime ConsultaDataOperacao { get; set; }

        public int CdAssessor { get; set; }

        public string NmAssessor { get; set; }

        public int CdCliente { get; set; }

        public string NmCliente { get; set; }

        public decimal VlTotal { get; set; }

        public decimal VlALiquidar { get; set; }

        public decimal VlDisponivel { get; set; }

        public decimal VlProjetado1 { get; set; }

        public decimal VlProjetado2 { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

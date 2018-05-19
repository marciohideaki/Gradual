using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class TotalClientePorAssessorInfo : ICodigoEntidade
    {
        public string ConsultaCdAssessor { get; set; }

        public DateTime ConsultaDtNegocioInicio { get; set; }

        public DateTime ConsultaDtNegocioFim { get; set; }

        public int CdAssessor { get; set; }

        public string NmAssessor { get; set; }

        public int CdCliente { get; set; }

        public string NmCliente { get; set; }

        public string DsBolsa { get; set; }

        public decimal VlCorretagemLiquida { get; set; }

        public decimal VlCorretagemBruta { get; set; }

        public decimal VlDescontoDv { get; set; }

        public decimal PcDescontoDv { get; set; }

        public decimal VlVc { get; set; }

        public decimal VlPc { get; set; }

        public decimal VlFg { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

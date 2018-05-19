using Gradual.OMS.Library;
using System;

namespace Gradual.Intranet.Contratos.Dados
{
    public class UltimasNegociacoesInfo : ICodigoEntidade
    {
        public DateTime DtUltimasNegociacoes { get; set; }

        public string TipoBolsa { get; set; }

        public Int32 CdCliente { get; set; }

        public Int32 CdClienteBmf { get; set; }

        public DateTime DataDe { get; set; }

        public DateTime DataAte { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

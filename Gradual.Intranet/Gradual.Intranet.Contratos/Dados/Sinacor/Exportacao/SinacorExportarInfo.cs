using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Contrato de Exportação
    /// </summary>
    public class SinacorExportarInfo : ICodigoEntidade
    {
        public SinacorExportacaoEntradaInfo Entrada { get; set; }

        public SinacorExportacaoRetornoInfo Retorno { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

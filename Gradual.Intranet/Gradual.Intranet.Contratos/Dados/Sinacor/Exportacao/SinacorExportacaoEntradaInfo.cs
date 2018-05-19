using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Entrada do Contrato de Exportação
    /// </summary>
    public class SinacorExportacaoEntradaInfo : ICodigoEntidade
    {
        /// <summary>
        /// Id do Cliente
        /// </summary>
        public int IdCliente { get; set; }
        /// <summary>
        /// Informação se é a primeira vez que o cliente está sendo Exportado
        /// </summary>
        public Boolean PrimeiraExportacao { get; set; }
        /// <summary>
        /// Código exportado caso não seja a primeira exportação
        /// </summary>
        public Nullable<int> CdCodigo { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

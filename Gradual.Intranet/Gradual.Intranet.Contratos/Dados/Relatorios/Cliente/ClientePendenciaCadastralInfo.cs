using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClientePendenciaCadastralRelInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código do Assessor
        /// </summary>
        public Nullable<int> CodigoAssessor { get; set; }

        /// <summary>
        /// Filtro de data "De" 
        /// </summary>
        public Nullable<DateTime> DtDe { get; set; }

        /// <summary>
        /// Filtro de data "Ate" 
        /// </summary>
        public Nullable<DateTime> DtAte { get; set; }

        /// <summary>
        /// Código da bolsa
        /// </summary>
        public Nullable<int> CodigoBolsa { get; set; }

        /// <summary>
        /// Código do cliente 
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string DsNomeCliente { get; set; }

        /// <summary>
        /// Número do Cpf e Cnpj do cliente
        /// </summary>
        public string DsCpfCnpj { get; set; }

        /// <summary>
        /// Código do Tipo de pendencia cadastral
        /// </summary>
        public Nullable<int> IdTipoPendenciaCadastral { get; set; }

        /// <summary>
        /// Código do tipo de documento
        /// </summary>
        public Nullable<int> IdTipoDocumento { get; set; }

        /// <summary>
        /// Data da pendencia cadastral
        /// </summary>
        public DateTime DtPendenciaCadastral { get; set; }

        /// <summary>
        /// Data da Resolução
        /// </summary>
        public DateTime DtResolucao { get; set; }

        /// <summary>
        /// Descrição da pendencia cadastral
        /// </summary>
        public string DsPendenciaCadastral { get; set; }

        /// <summary>
        /// Descrição do tipo de pendencia cadastral
        /// </summary>
        public string DsTipoPendenciaCadastral { get; set; }

        /// <summary>
        /// Tipo de pessoa F = física, J = Jurídica
        /// </summary>
        public string TipoPessoa { get; set; }

        /// <summary>
        /// Para o filtro
        /// </summary>
        public System.Nullable<Boolean> StResolvido { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

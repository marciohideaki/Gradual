using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteSemLoginInfo : ICodigoEntidade
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
        /// Código do cliente
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Nome do Cliente
        /// </summary>
        public string DsNomeCliente { get; set; }

        /// <summary>
        /// Tipo de Pessoa
        /// </summary>
        public string TipoPessoa { get; set; }

        /// <summary>
        /// Data de cadastro
        /// </summary>
        public DateTime DtCadastro { get; set; }

        /// <summary>
        /// CPF ou CNPJ
        /// </summary>
        public string DsCpfCnpj { get; set; }

        /// <summary>
        /// Código bovespa do cliente.
        /// </summary>
        public string CdBovespa { get; set; }

        /// <summary>
        /// Nome do Cliente
        /// </summary>
        public string DsEmail { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

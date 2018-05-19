using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteInativoInfo : ICodigoEntidade
    {

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
        /// Nome do Cliente
        /// </summary>
        public string DsEmail { get; set; }

        /// <summary>
        /// Id do Assessor
        /// </summary>
        public System.Nullable<int> IdAssessor { get; set; }

        /// <summary>
        /// Número da Conta
        /// </summary>
        public string CdConta { get; set; }

        /// <summary>
        /// Tipo da Conta - CliGer, Bovespa, BMF, CUS, CC
        /// </summary>
        public string DsConta { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}

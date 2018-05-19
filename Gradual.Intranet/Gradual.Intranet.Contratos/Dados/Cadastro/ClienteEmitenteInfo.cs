#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
#endregion

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Classe para preenchimento do Emitente
    /// </summary>
    public class ClienteEmitenteInfo : ICodigoEntidade
    {
        #region Propriedades

        /// <summary>
        /// Código da pessoa autorizada
        /// </summary>
        public Nullable<int> IdPessoaAutorizada { get; set; }

        /// <summary>
        /// Código do Cliente
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Nome da Pessoa autorizada
        /// </summary>
        public string DsNome { get; set; }

        /// <summary>
        /// Cpf ou CNPJ da pessoa autorizada
        /// </summary>
        public string NrCpfCnpj { get; set; }

        /// <summary>
        /// Data de nascimento da pessoa autorizada
        /// </summary>
        public Nullable<DateTime> DtNascimento { get; set; }

        /// <summary>
        /// Número de Documento da pessoa autorizada
        /// </summary>
        public string DsNumeroDocumento { get; set; }

        /// <summary>
        /// Código do sistema Bol - Bovespa, BMF, etc..
        /// </summary>
        public string CdSistema { get; set; }

        /// <summary>
        /// Flag para Emitente principal do cliente
        /// </summary>
        public bool StPrincipal { get; set; }

        /// <summary>
        /// E-mail do Emitente
        /// </summary>
        public string DsEmail { get; set; }

        /// <summary>
        /// Data de cadastro do Emitente
        /// </summary>
        public Nullable<DateTime> DsData { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

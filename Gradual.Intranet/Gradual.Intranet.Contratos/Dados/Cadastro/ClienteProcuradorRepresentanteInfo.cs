#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
#endregion

namespace Gradual.Intranet.Contratos.Dados
{
    /// <summary>
    /// Classe para preenchimento de Procurador/Representante
    /// </summary>
    public class ClienteProcuradorRepresentanteInfo : ICodigoEntidade
    {
        #region Propriedades

        /// <summary>
        /// Código do Procurador ou representante
        /// </summary>
        public Nullable<int> IdProcuradorRepresentante { get; set; }

        /// <summary>
        /// Código do Cliente
        /// </summary>
        public Nullable<int> IdCliente { get; set; }

        /// <summary>
        /// Tipo de procurador/representante
        /// </summary>
        public TipoProcuradorRepresentante TpProcuradorRepresentante { get; set; }

        /// <summary>
        /// Nome do Produrador ou representante
        /// </summary>
        public string DsNome { get; set; }

        /// <summary>
        /// Cpf ou CNPJ do Procurador/representante
        /// </summary>
        public string NrCpfCnpj { get; set; }

        /// <summary>
        /// Data de nascimento do Procurador/Representante 
        /// </summary>
        public DateTime? DtNascimento { get; set; }

        /// <summary>
        /// Documento do Procurador/representante
        /// </summary>
        public string DsNumeroDocumento { get; set; }

        /// <summary>
        /// Tipo de situação legal
        /// </summary>
        public int TpSituacaoLegal { set; get; }

        /// <summary>
        /// Orgão emissor do documento 
        /// </summary>
        public string CdOrgaoEmissor { get; set; }

        /// <summary>
        /// Código da UF do orgão emissor
        /// </summary>
        public string CdUfOrgaoEmissor { get; set; }

        /// <summary>
        /// Tipo de Documento
        /// </summary>
        public string TpDocumento { get; set; }

        /// <summary>
        /// Data de validade da Procuração
        /// </summary>
        public System.Nullable<DateTime> DtValidade { get; set; }

        #endregion

        #region Construtor

        public ClienteProcuradorRepresentanteInfo() { }
        
        public ClienteProcuradorRepresentanteInfo(string pIdcliente) 
        {
            this.IdCliente = int.Parse(pIdcliente);
        }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

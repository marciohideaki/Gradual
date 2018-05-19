#region includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
#endregion

namespace Gradual.Intranet.Contratos.Dados
{
    public class EmailDisparadoPeriodoInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código Bovespa do cliente.
        /// </summary>
        public string CdCodigo { get; set; }

        /// <summary>
        /// Cnpj do cliente.
        /// </summary>
        public string DsCpfCnpj { get; set; }

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
        /// TIpo de Pessoa
        /// </summary>
        public string TipoPessoa { get; set; }

        /// <summary>
        /// Data de envio do email
        /// </summary>
        public DateTime DtEnvio { get; set; }

        /// <summary>
        /// E-mail do remetente.
        /// </summary>
        public string DsEmailRemetente { get; set; }

        /// <summary>
        /// E-mail do destinatario
        /// </summary>
        public string DsEmailDestinatario { get; set; }

        /// <summary>
        /// Cliente exportado true/false
        /// </summary>
        public bool BlnExportado { get; set; }

        /// <summary>
        /// Corpo do email
        /// </summary>
        public string DsCorpoEmail { get; set; }

        /// <summary>
        /// Tipo de E-mail que foi disparado 
        /// 1 - Disparado para assessor 
        /// 2 - Disparado para compliance;
        /// </summary>
        public eTipoEmailDisparo ETipoEmailDisparo { get; set; }

        /// <summary>
        /// Assunto do email 
        /// </summary>
        public string DsAssuntoEmail { get; set; }

        /// <summary>
        /// Nome 
        /// </summary>
        public string DsNome { get; set; }

        /// <summary>
        /// Perfil 
        /// </summary>
        public string DsPerfil { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

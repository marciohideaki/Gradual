using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteSuspeitoInfo : ICodigoEntidade
    {
        /// <summary>
        /// Filtro de data "De" 
        /// </summary>
        public Nullable<DateTime> DtDe { get; set; }

        /// <summary>
        /// Filtro de data "Ate" 
        /// </summary>
        public Nullable<DateTime> DtAte { get; set; }

        /// <summary>
        /// Código do assessor
        /// </summary>
        public string CdAssessor { get; set; }

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
        /// CPF ou CNPJ
        /// </summary>
        public string DsCpfCnpj { get; set; }

        /// <summary>
        /// Código do país na blacklist
        /// </summary>
        public int IdPaisBlackList { get; set; }

        /// <summary>
        /// Código sinacor do País
        /// </summary>
        public string CdPais { get; set; }

        /// <summary>
        /// Nome do País do cliente
        /// </summary>
        public string NmPaisBlackList { get; set; }

        /// <summary>
        /// Cliente exportado Sim/Não
        /// </summary>
        public bool blnExportado { get; set; }

        /// <summary>
        /// Código da atividade ilícita
        /// </summary>
        public Nullable<int> IdAtividadeIlicita { get; set; }

        /// <summary>
        /// Código sinacor da atividade
        /// </summary>
        public int CdAtividade { get; set; }

        /// <summary>
        /// Descrição da atividade ilícita
        /// </summary>
        public string DsAtividadeIlicita { get; set; }

        /// <summary>
        /// Código de bolsa do cliente Bovespa/Bmf
        /// </summary>
        public Int64 CodigoBolsa { get; set; }

        /// <summary>
        /// Endereco principal do cliente sim/não
        /// </summary>
        public bool BlEnderecoPrincipal { get; set; }

        /// <summary>
        /// Código do endereço do cliente
        /// </summary>
        public Int64 IdEndereco { get; set; }

        /// <summary>
        /// Data de cadastro do Cliente
        /// </summary>
        public DateTime DtCadastro { get; set; }

        public string CdBovespa { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteCadastradoPeriodoInfo : ICodigoEntidade
    {

        /// <summary>
        /// Data de do filtro
        /// </summary>
        public Nullable<DateTime> DtDe { get; set; }

        /// <summary>
        /// Data "Até" do filtro
        /// </summary>
        public Nullable<DateTime> DtAte { get; set; }

        /// <summary>
        /// Código do Cliente
        /// </summary>
        public Nullable<int> IdCliente { get; set; }

        /// <summary>
        /// Nome do Cliente
        /// </summary>
        public string DsNomeCliente { get; set; }

        /// <summary>
        /// Cpf ou CNPJ do cliente
        /// </summary>
        public string DsCpfCnpj { get; set; }

        /// <summary>
        /// Tipo de pessoa se é física ou Jurídica
        /// </summary>
        public string TipoPessoa { get; set; }

        /// <summary>
        /// Data do cadastro
        /// </summary>
        public DateTime DtCadastro { get; set; }

        /// <summary>
        /// Passo Atual
        /// </summary>
        public String PassoAtual { get; set; }

        /// <summary>
        /// Flag de exportado
        /// </summary>
        public Boolean BlnExportado { get; set; }

        /// <summary>
        /// Código Bovespa 
        /// </summary>
        public Nullable<int> CodigoBovespa { get; set; }

        /// <summary>
        /// Código Bmf
        /// </summary>
        public Nullable<int> CodigoBmf { get; set; }

        /// <summary>
        /// Código do Assessor
        /// </summary>
        public int CodigoAssessor { get; set; }

        /// <summary>
        /// Data da ultima atualização
        /// </summary>
        public Nullable<DateTime> DtUltimaAtualizacao { get; set; }

        /// <summary>
        /// Telefone Principal
        /// </summary>
        public string DsTelefone { get; set; }

        /// <summary>
        /// DDD Principal
        /// </summary>
        public string DsDDD { get; set; }

        /// <summary>
        /// Ramal Principal
        /// </summary>
        public string DsRamal { get; set; }

        /// <summary>
        /// E-mail do Cliente
        /// </summary>
        public string DsEmail { get; set; }


        /// <summary>
        /// Deseja operar em 
        /// </summary>
        public string DsDesejaOperarEm { get; set; }

        /// <summary>
        /// Codigo do tipo de operação
        /// </summary>
        public string CodigoTipoOperacao { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

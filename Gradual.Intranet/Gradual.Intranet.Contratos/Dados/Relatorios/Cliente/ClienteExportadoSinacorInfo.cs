using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClientesExportadosSinacorInfo : ICodigoEntidade
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
        /// Código assessor do filtro
        /// </summary>
        public Nullable<int> CdAssessor { get; set; }

        /// <summary>
        /// Cpf ou CNPJ do cliente do filtro
        /// </summary>
        public string DsCpfCnpj { get; set; }

        /// <summary>
        /// Código Bmf ou Bovespa do filtro
        /// </summary>
        public string CdBolsa { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string DsNomeCliente { get; set; }

        /// <summary>
        /// Tipo de pessoa se é física ou Jurídica
        /// </summary>
        public string TipoPessoa { get; set; }

        /// <summary>
        /// Data do cadastro
        /// </summary>
        public DateTime DtCadastro { get; set; }


        /// <summary>
        /// Código Bovespa do cliente
        /// </summary>
        public Nullable<int> CodigoBovespa { get; set; }


        /// <summary>
        /// Código do Assessor
        /// </summary>
        public Nullable<Int32> CodigoAssessor { get; set; }


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

        public DateTime DtPrimeiraExportacao { get; set; }

        public DateTime DtUltimaExportacao { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

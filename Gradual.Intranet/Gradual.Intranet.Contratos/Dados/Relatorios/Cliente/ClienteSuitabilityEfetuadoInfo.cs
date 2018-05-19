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
    /// 
    /// </summary>
    public class ClienteSuitabilityEfetuadoInfo : ICodigoEntidade
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
        /// Para o Filtro
        /// </summary>
        public Nullable<Boolean> StRealizado { get; set; }

        /// <summary>
        /// Código do Cliente
        /// </summary>
        public Nullable<int> IdCliente { get; set; }


        /// <summary>
        /// Nome de quam realizou
        /// </summary>
        public string DsNomeCliente { get; set; }

        /// <summary>
        /// Nome de quam realizou
        /// </summary>
        public string DsLoginRealizado { get; set; }

        public string DsArquivoCiencia { get; set; }

        public string DtArquivoCiencia { get; set; }

        /// <summary>
        /// Descrição do perfil
        /// </summary>
        public string DsPerfil { get; set; }

        /// <summary>
        /// Descrição da fonte (de onde foi preenchido o suitability)
        /// </summary>
        public string DsFonte { get; set; }

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
        public DateTime DtRealizacao { get; set; }

        /// <summary>
        /// Código Bovespa 
        /// </summary>
        public Nullable<int> CodigoBovespa { get; set; }

        /// <summary>
        /// Código do Assessor
        /// </summary>
        public Nullable<int> CodigoAssessor { get; set; }

        public Nullable<Boolean> StPreenchidoPeloCliente { get; set; }

        public string DsStatus { get; set; }

        public string DsRespostas { get; set; }
        public string Peso { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

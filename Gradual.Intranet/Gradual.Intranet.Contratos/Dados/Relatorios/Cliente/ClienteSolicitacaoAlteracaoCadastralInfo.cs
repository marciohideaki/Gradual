using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteSolicitacaoAlteracaoCadastralInfo : ICodigoEntidade
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
        /// Data da pendencia cadastral
        /// </summary>
        public DateTime DtSolicitacao { get; set; }

        /// <summary>
        /// Data da Resolução
        /// </summary>
        public DateTime DtResolucao { get; set; }

        /// <summary>
        /// Campo à ser alterado
        /// </summary>
        public string DsInformacao { get; set; }

        /// <summary>
        /// Descrição do tipo de Solicitação
        /// </summary>
        public string DsTipo { get; set; }

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

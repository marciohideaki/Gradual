using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteSituacaoFinanceiraPatrimonialInfo : ICodigoEntidade
    {
        #region | Propriedades

        /// <summary>
        /// Código da Situação Financeira patrimonial
        /// </summary>
        public Nullable<int> IdSituacaoFinanceiraPatrimonial { get; set; }

        /// <summary>
        /// Código do Cliente
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Valor total dos bens
        /// </summary>
        public Nullable<decimal> VlTotalBensImoveis { get; set; }

        /// <summary>
        /// Valor dos bens móveis
        /// </summary>
        public Nullable<decimal> VlTotalBensMoveis { get; set; }

        /// <summary>
        /// Total aplicado
        /// </summary>
        public Nullable<decimal> VlTotalAplicacaoFinanceira { get; set; }

        /// <summary>
        /// Total do salário prolabore
        /// </summary>
        public Nullable<decimal> VlTotalSalarioProLabore { get; set; }

        /// <summary>
        /// Total de outros rendimentos
        /// </summary>
        public Nullable<decimal> VlTotalOutrosRendimentos { get; set; }

        /// <summary>
        /// Capitação social em: (Data para demarcação da propriedade  VlrCapitalSocial)
        /// </summary>
        public Nullable<DateTime> DtCapitalSocial { get; set; }

        /// <summary>
        /// Patrimonio Liquido em: (Data para demarcação da propriedade VlrPatrimonioLiquido)
        /// </summary>     
        public Nullable<decimal> VlTotalPatrimonioLiquido { get; set; }

        /// <summary>
        /// Valor capital
        /// </summary>
        public Nullable<Decimal> VTotalCapitalSocial { get; set; }

        /// <summary>
        /// Valor do patrimonio liquido
        /// </summary>
        public Nullable<DateTime> DtPatrimonioLiquido { get; set; }

        /// <summary>
        /// Outros Rendimentos
        /// </summary>
        public string DsOutrosRendimentos { get; set; }

        /// <summary>
        /// Data de Atualização
        /// </summary>
        public Nullable<DateTime> DtAtualizacao { get; set; } 

        #endregion

        #region | Construtor

        public ClienteSituacaoFinanceiraPatrimonialInfo() { }
        
        public ClienteSituacaoFinanceiraPatrimonialInfo(string pIdcliente) 
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

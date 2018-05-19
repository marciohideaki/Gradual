using System;
using System.Collections.Generic;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteFichaPessoaFisicaInfo : ICodigoEntidade
    {
        /// <summary>
        /// Entidade de Cliente
        /// </summary>        
        public ClienteInfo ECliente { get; set; }

        /// <summary>
        /// Lista com os enderecos do cliente
        /// </summary>       
        public List<ClienteEnderecoInfo> LstEndereco { get; set; }

        /// <summary>
        /// Lista com as informações bancárias, agencia, conta, nº do banco
        /// </summary>       
        public List<ClienteBancoInfo> LstInfoBancaria { get; set; }

        /// <summary>
        /// Lista os procuradores ou representantes da empresa
        /// </summary>       
        public List<ClienteProcuradorRepresentanteInfo> LstProcuradorRepresentante { get; set; }

        /// <summary>
        /// Lista de contas do cliente
        /// </summary>        
        public List<ClienteContaInfo> LstInfoConta { get; set; }

        /// <summary>
        /// Situação financeira com os dados de:
        /// Capital Social em: (Data), 
        /// ValorCapital, 
        /// Patrimonio líquido em:(Data), 
        /// ValorPatrimonio
        /// </summary>        
        public List<ClienteSituacaoFinanceiraPatrimonialInfo> LstSituacaoFinanceira { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

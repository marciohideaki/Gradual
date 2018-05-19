using System;
using System.Collections.Generic;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class SinacorClienteInfo : ICodigoEntidade
    {
        public SinacorChaveClienteInfo ChaveCliente {get;set;}
  
        public SinacorClienteGeralInfo ClienteGeral { get; set; }
  
        public SinacorClienteComplementoInfo ClienteComplemento { get; set; }
      
        public List<SinacorEmitenteInfo> Emitentes { get; set; }
    
        public SinacorDiretorInfo Diretor { get; set; }

        public SinacorCcInfo Cc { get; set; }

        public Boolean StReimportacao { get; set; }

        public List<SinacorEnderecoInfo> Enderecos { get; set; }
 
        public List<SinacorContaBancariaInfo> ContasBancarias { get; set; }
   
        public List<SinacorTelefoneInfo> Telefones { get; set; }
     
        public List<SinacorSituacaoFinanceiraPatrimonialInfo> SituacaoFinaceiraPatrimoniais { get; set; }
  
        public List<SinacorBovespaInfo> Bovespas { get; set; }

        public List<SinacorAtividadeCcInfo> AtividadesCc { get; set; }

        public List<SinacorAtividadeCustodiaInfo> AtividadesCustodia { get; set; }
            
        public List<SinacorBmfInfo> Bmfs { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

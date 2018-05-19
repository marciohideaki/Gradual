using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class ClienteSinacorInfo : ICodigoEntidade
    {
      
        public SinacorChaveClienteInfo ChaveCliente {get;set;}
     
        public SinacorClienteGeralInfo ClienteGeral { get; set; }
       
        public SinacorClienteComplementoInfo ClienteComplemento { get; set; }
      
        public List<SinacorEmitenteInfo> Emitentes { get; set; }
    
        public SinacorDiretorInfo Diretor { get; set; }
      
        public SinacorCcInfo Cc { get; set; }
      
        public List<SinacorEnderecoInfo> Enderecos { get; set; }
       
        public List<SinacorContaBancariaInfo> ContasBancarias { get; set; }
      
        public List<SinacorTelefoneInfo> Telefones { get; set; }

        public List<SinacorSituacaoFinanceiraPatrimonialInfo> SituacaoFinaceiraPatrimoniais { get; set; }
      
        public List<SinacorBovespaInfo> Bovespas { get; set; }

        public List<SinacorBmfInfo> Bmfs { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

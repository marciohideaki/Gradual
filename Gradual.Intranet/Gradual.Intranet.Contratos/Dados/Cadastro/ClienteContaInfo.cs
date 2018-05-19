using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteContaInfo : ICodigoEntidade
    {
        #region ICodigoEntidade Members
    
        public Nullable<Int32> IdClienteConta { get; set; }

        public Int32? IdCliente { get; set; }
      
        public int CdAssessor { get; set; }

        public Int32? CdCodigo { get; set; }

        public eAtividade CdSistema { get; set; }
     
        public Boolean StContaInvestimento { get; set; }
      
        public Boolean? StPrincipal { get; set; }
       
        public Boolean StAtiva { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

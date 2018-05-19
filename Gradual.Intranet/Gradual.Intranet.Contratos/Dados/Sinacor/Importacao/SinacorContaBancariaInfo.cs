using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class SinacorContaBancariaInfo : ICodigoEntidade
    {
        public string CD_AGENCIA { get; set; }

        public string DV_AGENCIA { get; set; }
 
        public string CD_BANCO { get; set; }

        public string NR_CONTA { get; set; }
  
        public string DV_CONTA { get; set; }
  
        public Nullable<char> IN_PRINCIPAL { get; set; }
      
        public string TP_CONTA { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

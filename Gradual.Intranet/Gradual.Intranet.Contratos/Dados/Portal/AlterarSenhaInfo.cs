using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public  class AlterarSenhaInfo : ICodigoEntidade
    {

       public int IdLogin { get; set; }
       public string CdSenhaAntiga { get; set; }
       public string CdSenhaNova { get; set; }

        
       #region ICodigoEntidade Members

       public string ReceberCodigo()
       {
           throw new NotImplementedException();
       }

       #endregion
    }

    public class AlterarSenhaDinamicaInfo : AlterarSenhaInfo
    {
        public Gradual.OMS.Seguranca.Lib.SenhaInfo SenhaDinamica        { get; set; }
        public Gradual.OMS.Seguranca.Lib.SenhaInfo SenhaDinamicaNova    { get; set; }
    }
}

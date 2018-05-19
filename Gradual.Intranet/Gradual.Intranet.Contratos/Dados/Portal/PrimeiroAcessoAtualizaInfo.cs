using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class PrimeiroAcessoAtualizaInfo : ICodigoEntidade
    {

        public int IdLogin {get;set;} 
        public string DsEmail  {get;set;} 
        public string CdSenha   {get;set;}
        public string CdAssinaturaEletronica { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

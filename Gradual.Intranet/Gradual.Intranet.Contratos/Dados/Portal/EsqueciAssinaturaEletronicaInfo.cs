using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class EsqueciAssinaturaEletronicaInfo : ICodigoEntidade
    {

        public string dsEmail { get; set; }
        public string dsCpfCnpj { get; set; }
        public DateTime dtNascimentoFundacao { get; set; }
        public string cdAssinaturaEletronica { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

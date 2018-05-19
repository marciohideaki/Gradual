using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class MensagemAjudaInfo : ICodigoEntidade
    {
        public int IdMensagem { get; set; }
        public string DsTitulo { get; set; }
        public string DsMensagem { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

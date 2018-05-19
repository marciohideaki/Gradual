using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using System.Runtime.Serialization;

namespace Gradual.Servicos.Contratos.TesteWCF.Dados
{
    [Serializable]
    [DataContract]
    public class MensagemTextoInfo : ICodigoEntidade
    {
        public string CodigoMensagemTexto { get; set; }

        public string TextoDaMensagem { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

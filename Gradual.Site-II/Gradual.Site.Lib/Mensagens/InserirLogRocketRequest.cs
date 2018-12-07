using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.Lib.Mensagens
{
    [Serializable]
    [DataContract]
    public class InserirLogRocketRequest : MensagemRequestBase
    {
        #region Propriedades

        [DataContract]
        public string DadosEnviados { get; set; }

        [DataContract]
        public string Resposta { get; set; }

        #endregion
    }
}

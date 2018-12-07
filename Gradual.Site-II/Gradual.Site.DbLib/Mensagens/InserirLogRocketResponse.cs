using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class InserirLogRocketResponse : MensagemResponseBase
    {
        #region Propriedades

        public int IdLogRocket { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Mensagens
{
    public enum TipoTeclado
    {
          QWERTY              = 0
        , DINAMICO            = 1
        , DINAMICO_SENHA      = 2
        , DINAMICO_ASSINATURA = 3
    }

    [Serializable]
    [DataContract]
    public class TipoTecladoResponse: Gradual.OMS.Library.MensagemResponseBase
    {
        [DataMember]
        public String CodigoCliente { get; set; }

        [DataMember]
        public TipoTeclado Teclado { get; set; }

        [DataMember]
        public String Mensagem { get; set; }
    }
}

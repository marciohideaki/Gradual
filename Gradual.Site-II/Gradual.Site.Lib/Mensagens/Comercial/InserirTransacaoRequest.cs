﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.Lib.Dados.MinhaConta.Comercial;

namespace Gradual.Site.Lib.Mensagens
{
    [Serializable]
    [DataContract]
    public class InserirTransacaoRequest : MensagemRequestBase
    {
        [DataMember]
        public TransacaoInfo Transacao { get; set; }
    }
}

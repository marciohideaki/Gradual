﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using System.Runtime.Serialization;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [DataContract]
    public class SelecionarOrdemRequest 
    {
        [DataMember]
        public int IdStopStart { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Automacao.Ordens.Mensagens
{
    [Serializable]
    public class ListarOrdensRequest : MensagemResponseClienteBase
    {
        public string TipoOrdem { get; set; }
    }
}

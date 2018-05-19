﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class CalendarioEventoResponse : MensagemResponseBase
    {
        public List<CalendarioEventoInfo> ListaEventos{ get; set; }
    }
}

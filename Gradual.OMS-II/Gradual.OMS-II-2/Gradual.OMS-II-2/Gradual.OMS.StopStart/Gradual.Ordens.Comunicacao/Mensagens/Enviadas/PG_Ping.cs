using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Cabecalho;

namespace Gradual.OMS.Ordens.Comunicacao.Mensagens.Enviadas
{
    public class PG_Ping : Header
    {
        public string GetMessage()
        {
            return base.GetHeader("PG", "PP", "PING");
        }
    }
}

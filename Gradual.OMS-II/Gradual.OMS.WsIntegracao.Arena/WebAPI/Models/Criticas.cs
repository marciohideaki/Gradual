using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao.Arena.Models
{
    public class Criticas
    {
        public int IdCritica { get; set; }
        public string Descricao { get; set; }
        public DateTime DataEvento { get; set; }
    }
}
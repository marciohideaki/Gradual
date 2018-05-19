using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao.Arena.Models
{
    public class AssinaturaEletronica 
    {
        public int IdClienteGradual { get; set; }

        public string Assinatura    { get; set; }

        public bool Valida          { get; set; }
    }
}
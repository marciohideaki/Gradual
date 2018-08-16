using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteSegurancaItemFilho
    {
        public string Id { get; set; }

        public string  ParentId { get; set; }

        public string  TipoDeItem { get; set; }

        public string Item { get; set; }

        public string ItemDesc { get; set; }
    }
}
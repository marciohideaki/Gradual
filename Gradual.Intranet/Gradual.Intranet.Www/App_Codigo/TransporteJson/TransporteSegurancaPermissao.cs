using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteSegurancaPermissao
    {

        public TransporteSegurancaPermissao()
        {
            Permissoes = new List<string>();
        }

        public string Id { get; set; }

        public string ParentId { get; set; }

        public List<string> Permissoes { get; set; }

        public string TipoDeObjetoPai { get; set; }

        public string TipoDeItem
        {
            get
            {
                return "Permissoes";
            }
        }
    }
}
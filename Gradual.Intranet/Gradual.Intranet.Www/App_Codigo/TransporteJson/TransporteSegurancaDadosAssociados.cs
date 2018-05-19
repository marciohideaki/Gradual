using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteSegurancaDadosAssociados
    {

        public bool EhUsuario { get; set; }

        public bool EhGrupo { get; set; }

        public string Usuario { get; set; }

        public string Grupo { get; set; }

        public string Subsistema { get; set; }

        public string Interface { get; set; }

        public bool Consultar { get; set; }

        public bool Salvar { get; set; }

        public bool Excluir { get; set; }

        public bool Executar { get; set; }
   }
}
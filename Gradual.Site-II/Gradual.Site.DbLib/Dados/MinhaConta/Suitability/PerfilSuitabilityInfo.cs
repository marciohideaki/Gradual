using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.Suitability
{
    public class PerfilSuitabilityInfo
    {
        public int IdPerfil { get; set; }

        public string Descricao { get; set; }

        public decimal FaixaDe { get; set; }

        public decimal FiaxaAte { get; set; }

        public string DescricaoIntranet { get; set; }
    }
}

using System;

namespace Gradual.Migracao.Educacional.Entidades
{
    public class FichaPerfilInfo
    {
        public int IdFichaPerfilOracle { get; set; }

        public int IdFichaPerfilSql { get; set; }

        public int IdCliente { get; set; }

        public string DsFaixaEtaria { get; set; }

        public string DsConhecimento { get; set; }

        public string DsOcupacao { get; set; }

        public string TpInvestidor { get; set; }

        public string TpInvestimento { get; set; }

        public string TpInstituicao { get; set; }

        public string DsRendaFamiliar { get; set; }

        public DateTime DtInclusao { get; set; }
    }
}

using System;

namespace Gradual.Migracao.Educacional.Entidades
{
    public class AvaliacaoPalestraInfo
    {
        public int IdAvaliacaoPalestraOracle { get; set; }

        public int IdAvaliacaoPalestraSql { get; set; }

        public int IdCursoPalestra { get; set; }

        public string DsAvaliaPalestrante { get; set; }

        public string DsMaterial { get; set; }

        public string DsInfraEstrutura { get; set; }

        public string DsExpectativa { get; set; }

        public DateTime DtAvaliacao { get; set; }
    }
}

using System;

namespace Gradual.Migracao.Educacional.Entidades
{
    public class CursoPalestraInfo
    {
        public int IdCursoPalestraOracle { get; set; }

        public int IdCursoPalestraSql { get; set; }

        public int IdTema { get; set; }

        public int IdLocalidade { get; set; }

        public int IdAssessor { get; set; }

        public string DsMunicipio { get; set; }

        public string DsEndereco { get; set; }

        public string DsCEP { get; set; }

        public string DsTexto { get; set; }

        public DateTime DtCriacao { get; set; }

        public int NrVagaLimite { get; set; }

        public int NrVagaInscritos { get; set; }

        public int StSituacao { get; set; }

        public char StRealizado { get; set; }

        public char StTipoEvento { get; set; }

        public decimal Valor { get; set; }

        public DateTime DtDataHoraLimite { get; set; }

        public DateTime DtDataHoraCurso { get; set; }

        public char FlHome { get; set; }
    }
}

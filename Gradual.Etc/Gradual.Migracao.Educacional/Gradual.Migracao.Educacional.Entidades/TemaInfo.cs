using System;

namespace Gradual.Migracao.Educacional.Entidades
{
    public class TemaInfo
    {
        public int IdTemaOracle { get; set; }

        public int IdTemaSql { get; set; }

        public int IdNivel { get; set; }

        public string DsTitulo { get; set; }

        public string DsChamada { get; set; }

        public char StSituacao { get; set; }

        public DateTime DtCriacao { get; set; }
    }
}

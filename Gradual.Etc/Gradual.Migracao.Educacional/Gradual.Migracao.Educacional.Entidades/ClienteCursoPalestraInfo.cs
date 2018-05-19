using System;

namespace Gradual.Migracao.Educacional.Entidades
{
    public class ClienteCursoPalestraInfo
    {
        public int IdCursoPalestraOracle { get; set; }

        public int IdCursoPalestraSql { get; set; }

        public int IdCliente { get; set; }

        public DateTime DtCadastro { get; set; }

        public char StPresenca { get; set; }

        public char StConfirmaInscricao { get; set; }

        public char StListaEspera { get; set; }
    }
}

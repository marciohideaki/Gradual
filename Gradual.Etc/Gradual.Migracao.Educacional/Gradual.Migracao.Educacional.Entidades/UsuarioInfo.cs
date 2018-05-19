namespace Gradual.Migracao.Educacional.Entidades
{
    public class UsuarioInfo
    {
        public int IdUsuarioOracle { get; set; }

        public int IdUsuarioSql { get; set; }

        public int IdPerfil { get; set; }

        public int IdLocalidade { get; set; }

        public int IdAssessor { get; set; }

        public string DsNome { get; set; }

        public string DsEmail { get; set; }

        public string DsSenha { get; set; }

        public char StUsuario { get; set; }
    }
}

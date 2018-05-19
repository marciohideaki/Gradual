
namespace Gradual.Cadastro.Entidades
{
    public class ETelefone
    {
        public int ID_Telefone { get; set; }
        public int ID_Cliente { get; set; }
        public string DDD { get; set; }
        public string Telefone { get; set; }
        public string Ramal { get; set; }
        public System.Nullable<char> Principal { get; set; }
        public System.Nullable<char> Tipo { get; set; }
    }
}

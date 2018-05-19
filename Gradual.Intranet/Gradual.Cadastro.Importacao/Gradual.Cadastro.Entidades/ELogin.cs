
namespace Gradual.Cadastro.Entidades
{
    public class ELogin
    {
        public int ID_Cliente {get; set;}
        public int ID_Login { get; set; }
        public char Ativo { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int Tipo { get; set; }
        public string Nome { get; set; }
        public string Assinatura { get; set; }
    }
}

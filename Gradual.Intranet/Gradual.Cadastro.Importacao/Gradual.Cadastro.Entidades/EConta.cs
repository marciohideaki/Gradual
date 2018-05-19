
namespace Gradual.Cadastro.Entidades
{
    public class EConta
    {
        public int ID_Conta { get; set; }
        public int ID_Cliente { get; set; }
        public int Banco { get; set; }
        public int Agencia { get; set; }
        public int AgenciaDigito { get; set; }
        public string Conta { get; set; }
        public string ContaDigito { get; set; }
        public char Tipo { get; set; }
        public char Principal { get; set; }
    }
}

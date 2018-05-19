
namespace Gradual.Cadastro.Entidades
{
    public class EEndereco
    {
        public int ID_Endereco { get; set; }
        public int ID_Cliente { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Pais { get; set; }
        public char Correspondencia { get; set; }
        public char Tipo { get; set; }
    }
}

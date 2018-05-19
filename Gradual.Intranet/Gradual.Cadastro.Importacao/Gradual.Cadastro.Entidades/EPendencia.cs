using System;

namespace Gradual.Cadastro.Entidades
{
    public class EPendencia
    {
        public int ID_Pendencia { get; set; }
        public int ID_Cliente { get; set; }
        public char Documento { get; set; }
        public char CPF { get; set; }
        public char CertidaoCasamento { get; set; }
        public char ComprovanteEndereco { get; set; }
        public char Procuracao { get; set; }
        public char ComprovanteRenda { get; set; }
        public char	Contrato { get; set; }
	    public DateTime DataCadastro { get; set;}
	    public System.Nullable<DateTime> DataResolucao { get; set;}
        public string Descricao { get; set; }
        public char Serasa { get; set; }
        public char WTR { get; set; }
    }
}

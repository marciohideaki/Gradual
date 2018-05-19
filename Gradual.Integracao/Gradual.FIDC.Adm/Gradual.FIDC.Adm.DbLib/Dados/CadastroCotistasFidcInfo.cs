using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info utilizada para gerencias dados referentes a Cadastro de Fundos
    /// </summary>
    public class CadastroCotistasFidcInfo
    {
        public int IdCotistaFidc { get; set; }
        public string NomeCotista { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public DateTime DataNascFundacao { get; set; }
        public bool IsAtivo { get; set; }
        public DateTime DtInclusao { get; set; }        
        public int QuantidadeCotas { get; set; }
        public string ClasseCotas { get; set; }
        public DateTime DtVencimentoCadastro { get; set; }

        public string DataNascFundacaoFormatada { get; set; }
        public string DtVencimentoCadastroFormatada { get; set; }
    }
}

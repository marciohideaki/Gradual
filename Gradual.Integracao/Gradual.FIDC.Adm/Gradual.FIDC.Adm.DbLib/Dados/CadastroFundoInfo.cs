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
    public class CadastroFundoInfo
    {
        public int IdFundoCadastro { get; set; }
        public string NomeFundo { get; set; }
        public string CNPJFundo { get; set; }
        public string NomeAdministrador { get; set; }
        public string CNPJAdministrador { get; set; }
        public string NomeCustodiante { get; set; }
        public string CNPJCustodiante { get; set; }
        public string NomeGestor { get; set; }
        public string CNPJGestor { get; set; }
        public decimal TxGestao { get; set; }
        public decimal TxConsultoria { get; set; }
        public decimal TxCustodia { get; set; }
        public bool IsAtivo { get; set; }
        /// <summary>
        /// Indica se o fundo já pertence à relação categoria x subcategoria
        /// </summary>
        public bool Pertence { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info utilizada para gerencias dados referentes a Fundos x categoria x subcategoria
    /// </summary>
    public class FundoCategoriaSubCategoriaInfo
    {
        public int IdFundoCategoriaSubCategoria { get; set; }
        public int IdFundoCadastro { get; set; }
        public int IdFundoCategoria { get; set; }
        public int IdFundoSubCategoria { get; set; }
    }
}

using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de request de subcategoria de fundos
    /// </summary>
    public class FundoSubCategoriaRequest : MensagemRequestBase
    {
        public int IdFundoSubCategoria { get; set; }
        public string DsFundoSubCategoria { get; set; }
    }
}

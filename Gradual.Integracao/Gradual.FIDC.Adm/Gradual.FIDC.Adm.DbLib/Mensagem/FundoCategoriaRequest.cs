using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de request das categorias de fundos
    /// </summary>
    public class FundoCategoriaRequest : MensagemRequestBase
    {
        public int IdFundoCategoria { get; set; }
        public string DsFundoCategoria { get; set; }
    }
}

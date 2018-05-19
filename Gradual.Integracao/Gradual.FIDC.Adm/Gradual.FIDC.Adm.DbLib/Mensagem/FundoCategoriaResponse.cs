using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de response das categorias de fundos
    /// </summary>
    public class FundoCategoriaResponse : MensagemResponseBase
    {
        public List<FundoCategoriaInfo> ListaCategorias { get; set; }
    }
}

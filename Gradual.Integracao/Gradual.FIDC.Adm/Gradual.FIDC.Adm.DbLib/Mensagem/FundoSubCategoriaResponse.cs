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
    /// Classe de response de subcategoria de fundos
    /// </summary>
    public class FundoSubCategoriaResponse : MensagemResponseBase
    {
        public List<FundoSubCategoriaInfo> ListaSubCategorias { get; set; }
        
    }
}

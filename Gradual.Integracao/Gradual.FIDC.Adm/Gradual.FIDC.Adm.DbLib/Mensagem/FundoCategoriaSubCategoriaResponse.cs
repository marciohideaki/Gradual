using System.Collections.Generic;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de response de fundos x categoria x subcategoria
    /// </summary>
    public class FundoCategoriaSubCategoriaResponse : MensagemResponseBase
    {
        public List<CadastroFundoInfo> ListaFundos { get; set; }
    }
}

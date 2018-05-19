using Gradual.OMS.Library;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de request de fundos x categoria x subcategoria
    /// </summary>
    public class FundoCategoriaSubCategoriaRequest : MensagemRequestBase
    {
        public int IdFundoCategoriaSubCategoria { get; set; }
        public int IdFundoCadastro { get; set; }
        public int IdFundoCategoria { get; set; }
        public int IdFundoSubCategoria { get; set; }
    }
}

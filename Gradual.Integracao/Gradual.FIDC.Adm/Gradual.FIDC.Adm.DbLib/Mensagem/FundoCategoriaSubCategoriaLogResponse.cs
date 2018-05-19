using Gradual.OMS.Library;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de response das categorias de fundos
    /// </summary>
    public class FundoCategoriaSubCategoriaLogResponse : MensagemResponseBase
    {
        public string NomeFundo { get; set; }
        public string DsFundoCategoria { get; set; }
        public string DsFundoSubCategoria { get; set; }
    }
}

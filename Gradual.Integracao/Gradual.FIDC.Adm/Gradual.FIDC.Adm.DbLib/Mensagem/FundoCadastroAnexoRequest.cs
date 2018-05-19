using Gradual.OMS.Library;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class FundoCadastroAnexoRequest : MensagemRequestBase
    {
        public int IdFundoCadastroAnexo { get; set; }
        public int IdFundoCadastro { get; set; }
        public string CaminhoAnexo { get; set; }
        public string TipoAnexo { get; set; }
    }
}

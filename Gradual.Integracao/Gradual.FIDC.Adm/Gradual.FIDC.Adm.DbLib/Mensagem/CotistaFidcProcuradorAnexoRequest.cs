using Gradual.OMS.Library;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class CotistaFidcProcuradorAnexoRequest : MensagemRequestBase
    {
        public int IdCotistaFidcProcurador { get; set; }
        public string CaminhoAnexo { get; set; }
        public string TipoAnexo { get; set; }
    }
}

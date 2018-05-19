using Gradual.OMS.Library;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class CotistaFidcProcuradorRequest : MensagemRequestBase
    {
        public int IdCotistaFidcProcurador { get; set; }
        public int IdCotistaFidc { get; set; }
        public string NomeProcurador { get; set; }
        public string CPF { get; set; }
    }
}

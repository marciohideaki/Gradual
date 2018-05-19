using System.Collections.Generic;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class CotistaFidcProcuradorResponse : MensagemResponseBase
    {
        public List<CotistaFidcProcuradorInfo> ListaCotistaFidcProcurador { get; set; }
    }
}

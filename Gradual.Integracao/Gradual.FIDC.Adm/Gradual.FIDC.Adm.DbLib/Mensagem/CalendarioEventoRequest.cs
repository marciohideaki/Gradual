using System;
using Gradual.OMS.Library;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class CalendarioEventoRequest : MensagemRequestBase
    {
        public int IdCalendarioEvento{ get; set; }
        public int IdFundoCadastro { get; set; }
        public string DescEvento { get; set; }
        public string NomeFundo { get; set; }
        public DateTime DtEvento { get; set; }
        public DateTime DtEventoEnd { get; set; }
        public string EmailEvento { get; set; }
        public bool EnviarNotificacaoDia { get; set; }
        public bool MostrarHome { get; set; }
    }
}

using System;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    public class CalendarioEventoInfo
    {
        public int IdCalendarioEvento { get; set; }
        public int IdFundoCadastro { get; set; }
        public string NomeFundo { get; set; }
        public DateTime DtEvento { get; set; }
        public string DescEvento { get; set; }
        public string EmailEvento { get; set; }
        public bool EnviarNotificacaoDia { get; set; }
        public bool MostrarHome { get; set; }
    }
}

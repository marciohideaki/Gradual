using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class OrdemStartStopResponse : MensagemRequestBase
    {
        public string CodigoOperacao { get; set; }
        public string NumeroDaOrdem { get; set; }
        public string TipoOperacao { get; set; }
        public string Instrumento { get; set; }
        public DateTime Hora { get; set; }
        public decimal PrecoStop { get; set; }
        public decimal PrecoLimite { get; set; }
        public string EnviadoBolsa { get; set; }
        public string StatusOrdem { get; set; }
        public string StopGainLoss { get; set; }
        public string ClienteCodigo { get; set; }
        public DateTime Validade { get; set; }
        public decimal InicioMovel { get; set; }
        public decimal InicioAjuste { get; set; }
    }
}

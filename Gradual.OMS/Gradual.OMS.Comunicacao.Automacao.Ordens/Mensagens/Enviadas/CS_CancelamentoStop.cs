using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Cabecalho;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas
{
    public class CS_CancelamentoStop:Header
    {
        public CS_CancelamentoStop(){
            tpBolsa = "BV";
        }

        private const char Delimiter = '0';
        private string tpBolsa { set; get; }

        private string _IdStopStart;
        public string IdStopStart
        {
            set { _IdStopStart = base.GetPosition(36, value, Delimiter); }
            get { return _IdStopStart; }
        }

        private string _CodigoInstrumento;
        public string CodigoInstrumento
        {
            set { _CodigoInstrumento = base.GetPosition(20, value, ' '); }
            get { return _CodigoInstrumento; }
        }

        public string getMessageCE()
        {
            return string.Format("{0}{1}", base.GetHeader("CE", this.tpBolsa, this.CodigoInstrumento), this.IdStopStart);
        }

    }
}

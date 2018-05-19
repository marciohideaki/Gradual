using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Cabecalho;


namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas
{
    public class CI_CancelInstrument : Header
    {

        public CI_CancelInstrument(string _tpBolsa){
            this.tpBolsa = _tpBolsa;
        }

        private const char Delimiter =  ' ';
        private string tpBolsa { set; get; }

        private string _TipoRequisicao;
        public string TipoRequisicao
        {
            set { _TipoRequisicao = base.GetPosition(2, value, Delimiter); }
            get { return _TipoRequisicao; }
        }

        private string _Instrument;
        public string Instrument
        {
            set { _Instrument = base.GetPosition(20, value, Delimiter); }
            get { return _Instrument; }
        }

        public string getMessageCI()
        {
            return string.Format("{0}{1}{2}", base.GetHeader("CI", this.tpBolsa), this.TipoRequisicao, this.Instrument);
        }
    }
}

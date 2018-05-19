using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Cabecalho;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas
{
    public class SS_StopSimples : Header
    {
        public SS_StopSimples(string TipoBolsa){            
            this.tpBolsa = TipoBolsa;            
        }

        private const char Delimiter =  ' ';
        private string tpBolsa { set; get; }

        private string _IdStopStart;
        public string IdStopStart{
            set { _IdStopStart = base.GetPosition(36, value, '0'); }
            get { return _IdStopStart; }
        }

        private string _CodigoInstrumento;
        public string CodigoInstrumento
        {
            set { _CodigoInstrumento = base.GetPosition(20, value, Delimiter); }
            get { return _CodigoInstrumento; }
        }

        private string _IdTipoOrdem;
        public string IdTipoOrdem{
            set { _IdTipoOrdem = base.GetPosition(1, value, Delimiter); }
            get { return _IdTipoOrdem; }
        }

        private string _PrecoLoss;
        public string PrecoLoss
        {
            set { _PrecoLoss = base.GetPosition(12, value, '0'); }
            get { return _PrecoLoss; }
        }

        private string _PrecoGain;
        public string PrecoGain
        {
            set { _PrecoGain = base.GetPosition(12, value, '0'); }
            get { return _PrecoGain; }
        }

        private string _PrecoStart;
        public string PrecoStart
        {
            set { _PrecoStart = base.GetPosition(12, value, '0'); }
            get { return _PrecoStart; }
        }

        private string _InicioMovel;
        public string InicioMovel
        {
            set { _InicioMovel = base.GetPosition(12, value, '0'); }
            get { return _InicioMovel; }
        }

        private string _AjusteMovel;
        public string AjusteMovel
        {
            set { _AjusteMovel = base.GetPosition(12, value, '0'); }
            get { return _AjusteMovel; }
        }


        public string getMessageSS(){

            string Header = base.GetHeader("SS", this.tpBolsa, CodigoInstrumento);
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", base.GetHeader("SS", this.tpBolsa,CodigoInstrumento), this.IdStopStart, this.IdTipoOrdem, this.PrecoLoss, this.PrecoGain, this.PrecoStart ,this.InicioMovel,this.AjusteMovel);
        }     


    }
}

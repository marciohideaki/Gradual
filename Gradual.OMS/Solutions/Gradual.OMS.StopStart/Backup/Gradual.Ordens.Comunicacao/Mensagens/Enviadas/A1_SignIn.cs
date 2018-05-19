using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Cabecalho;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas
{
    public class A1_SignIn : Header
    {
        public A1_SignIn(string _tpBolsa){
            this.tpBolsa = _tpBolsa;
        }

        private const char Delimiter =  '0';
        private string tpBolsa { set; get; }

        private string _idCliente;
        public string idCliente{
            set { _idCliente = base.GetPosition(8, value, Delimiter); }
            get { return _idCliente; }
        }

        private string _idSistema;
        public string idSistema{
            set { _idSistema = base.GetPosition(2, value, Delimiter); }
            get { return _idSistema; }
        }

        public string getMessageA1(){
            return string.Format("{0}{1}{2}", base.GetHeader("A1", this.tpBolsa), this.idCliente, this.idSistema);
        }
    }
}

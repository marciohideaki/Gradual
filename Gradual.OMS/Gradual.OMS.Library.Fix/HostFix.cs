using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickFix;

namespace Gradual.OMS.Library.Fix
{
    public class HostFix<T> where T : AplicacaoFixBase
    {
        private SocketInitiator _socketInitiator = null;
        private SocketAcceptor _socketAcceptor = null;

        public string ArquivoConfiguracao { get; set; }
        public T AplicacaoFix { get; set; }
        public bool EhInitiator { get; set; }


        public HostFix(string arquivoConfiguracao, bool ehInitiator)
        {
            this.ArquivoConfiguracao = arquivoConfiguracao;
            this.AplicacaoFix = Activator.CreateInstance<T>();
            this.EhInitiator = ehInitiator;
        }

        public void Iniciar()
        {
            SessionSettings settings = new SessionSettings(this.ArquivoConfiguracao);
            FileStoreFactory store = new FileStoreFactory(settings);
            FileLogFactory logs = new FileLogFactory(settings);
            MessageFactory msgs = new DefaultMessageFactory();

            // Cria o socket
            if (this.EhInitiator)
            {
                _socketInitiator =
                    new SocketInitiator(
                        this.AplicacaoFix, store, settings, logs, msgs);
                _socketInitiator.start();
            }
            else
            {
                _socketAcceptor =
                    new SocketAcceptor(
                        this.AplicacaoFix, store, settings, logs, msgs);
                _socketAcceptor.start();
            }

            // Envia mensagem para aplicação
            this.AplicacaoFix.Iniciar();
        }

        public void Parar()
        {
            // Log
            Gradual.OMS.Library.Log.EfetuarLog("HostFix-Parando1", LogTipoEnum.Passagem, "Gradual.OMS.Library");

            // Envia mensagem para aplicação
            this.AplicacaoFix.Parar();

            // Log
            Gradual.OMS.Library.Log.EfetuarLog("HostFix-Parando2", LogTipoEnum.Passagem, "Gradual.OMS.Library");

            // Para o socket
            if (this.EhInitiator)
            {
                _socketInitiator.stop();
                _socketInitiator.Dispose();
                _socketInitiator = null;
            }
            else
            {
                _socketAcceptor.stop();
                _socketAcceptor.Dispose();
                _socketAcceptor = null;
            }

            // Log
            Gradual.OMS.Library.Log.EfetuarLog("HostFix-Parando3", LogTipoEnum.Passagem, "Gradual.OMS.Library");
        }
    }
}

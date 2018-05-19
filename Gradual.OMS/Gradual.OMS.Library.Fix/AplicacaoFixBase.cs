using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickFix;

namespace Gradual.OMS.Library.Fix
{
    public class AplicacaoFixBase : QuickFix.MessageCracker, QuickFix.Application
    {
        public event EventHandler<HostFixEventoAplicacaoEventArgs> ApplicationEvent;
        public event EventHandler<HostFixMensagemErroEventArgs> MessageErrorEvent;

        public void Iniciar()
        {
            OnIniciar();
        }

        protected virtual void OnIniciar()
        {
        }

        public void Parar()
        {
            OnParar();
        }

        protected virtual void OnParar()
        {
        }
        
        #region Application Members

        public void fromAdmin(Message message, SessionID session)
        {
            // Dispara evento
            if (ApplicationEvent != null)
                ApplicationEvent(this,
                    new HostFixEventoAplicacaoEventArgs(
                        QuickFixApplicationEvent.fromAdmin, session, message));
            
            // Repassa mensagem
            OnFromAdmin(message, session);
        }

        protected virtual void OnFromAdmin(Message message, SessionID session)
        {
        }

        public void fromApp(Message message, SessionID session)
        {
            // Dispara evento
            if (ApplicationEvent != null)
                ApplicationEvent(this,
                    new HostFixEventoAplicacaoEventArgs(
                        QuickFixApplicationEvent.fromApp, session, message));

            // Repassa mensagem
            OnFromApp(message, session);
        }

        protected virtual void OnFromApp(Message message, SessionID session)
        {
            // Faz o processamento
            try
            {
                this.crack(message, session);
            }
            catch (Exception ex)
            {
                Gradual.OMS.Library.Log.EfetuarLog(ex, "AplicacaoFixBase.OnFromApp", "Gradual.OMS.Library");
                if (MessageErrorEvent != null)
                    MessageErrorEvent(this,
                        new HostFixMensagemErroEventArgs(session, message, this, ex));
            }
        }

        public void onCreate(SessionID session)
        {
            // Dispara evento
            if (ApplicationEvent != null)
                ApplicationEvent(this,
                    new HostFixEventoAplicacaoEventArgs(
                        QuickFixApplicationEvent.onCreate, session, null));

            // Repassa mensagem
            OnCreate(session);
        }

        protected virtual void OnCreate(SessionID session)
        {
        }

        public void onLogon(SessionID session)
        {
            // Dispara evento
            if (ApplicationEvent != null)
                ApplicationEvent(this,
                    new HostFixEventoAplicacaoEventArgs(
                        QuickFixApplicationEvent.onLogon, session, null));

            // Repassa mensagem
            OnLogon(session);
        }

        protected virtual void OnLogon(SessionID session)
        {
        }

        public void onLogout(SessionID session)
        {
            // Dispara evento
            if (ApplicationEvent != null)
                ApplicationEvent(this,
                    new HostFixEventoAplicacaoEventArgs(
                        QuickFixApplicationEvent.onLogout, session, null));

            // Repassa mensagem
            OnLogout(session);
        }

        protected virtual void OnLogout(SessionID session)
        {
        }

        public void toAdmin(Message message, SessionID session)
        {
            // Dispara evento
            if (ApplicationEvent != null)
                ApplicationEvent(this,
                    new HostFixEventoAplicacaoEventArgs(
                        QuickFixApplicationEvent.toAdmin, session, message));

            // Repassa mensagem
            OnToAdmin(message, session);
        }

        protected virtual void OnToAdmin(Message message, SessionID session)
        {
        }

        public void toApp(Message message, SessionID session)
        {
            // Dispara evento
            if (ApplicationEvent != null)
                ApplicationEvent(this,
                new HostFixEventoAplicacaoEventArgs(
                    QuickFixApplicationEvent.toApp, session, message));

            // Repassa mensagem
            OnToApp(message, session);
        }

        protected virtual void OnToApp(Message message, SessionID session)
        {
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Implementação do callback utilizado no modelo de chamadas do WCF.
    /// Utilizado também quando não se está utilizando WCF.
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, IncludeExceptionDetailInFaults = true)]
    public class CallbackEvento : ICallbackEvento
    {
        /// <summary>
        /// Evento disparado para sinalizar informações que chegaram do servidor
        /// </summary>
        public event EventHandler<EventoEventArgs> Evento;

        /// <summary>
        /// Evento disparado para sinalizar informações que chegaram do servidor
        /// </summary>
        public event EventHandler<EventArgs> Evento2;

        #region ICallbackEvento Members

        /// <summary>
        /// Implementação da interface. Identificador do callback
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Implementação da interface, faz o disparo do evento para sinalizar mensagem do servidor.
        /// </summary>
        /// <param name="evento"></param>
        public void SinalizarEvento(EventoInfo evento)
        {
            if (this.Evento != null)
                this.Evento(this, new EventoEventArgs() { EventoInfo = evento });
        }

        /// <summary>
        /// Implementação da interface, faz o disparo do evento para sinalizar mensagem do servidor.
        /// </summary>
        /// <param name="evento"></param>
        public void SinalizarEvento2(EventArgs args)
        {
            if (this.Evento2 != null)
                this.Evento2(this, args);
        }

        /// <summary>
        /// Método para retornar o id do callback
        /// </summary>
        /// <returns></returns>
        public string ReceberId()
        {
            return this.Id;
        }

        #endregion

        /// <summary>
        /// Contrutor.
        /// Gera o id deste callback
        /// </summary>
        public CallbackEvento()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}

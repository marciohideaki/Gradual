using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Representa um cliente do serviço de mensageria com callback.
    /// No caso de WCF o callback é pego pelo framework do WCF, e no caso
    /// de serviços locais, a referencia ao callback é passada explicitamente.
    /// Mantem informações sobre os serviços assinados, etc.
    /// </summary>
    public class ServicoMensageriaClienteHelper
    {
        /// <summary>
        /// Callback do cliente
        /// </summary>
        public ICallbackEvento Callback { get; set; }

        /// <summary>
        /// Informações da sessao do cliente
        /// </summary>
        public SessaoInfo Sessao { get; set; }

        /// <summary>
        /// Faz assinatura do evento para esta sessao
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AssinarEventoResponse AssinarEvento(AssinarEventoRequest parametros)
        {
            // Prepara o retorno
            AssinarEventoResponse resposta = 
                new AssinarEventoResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            
            // Pega referencia do servico
            Type tipoServico = Type.GetType(parametros.TipoServico);
            object servico = Ativador.Get(tipoServico);

            // Faz a assinatura
            EventInfo eventInfo = tipoServico.GetEvent(parametros.NomeEvento);
            MethodInfo mi = typeof(ServicoMensageriaClienteHelper).GetMethod("processarEvento", BindingFlags.NonPublic | BindingFlags.Instance);
            Delegate del = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, mi);
            eventInfo.GetAddMethod().Invoke(servico, new object[] { del });

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Rotina de processamento do evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void processarEvento(object sender, EventArgs args)
        {
            // Repassa o evento para o cliente
            this.Callback.SinalizarEvento2(args);
        }
    }
}

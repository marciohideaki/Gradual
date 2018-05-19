using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.espertech.esper.client;
using log4net;
using Gradual.OMS.AutomacaoDesktop.Events;
using com.espertech.esper.compat.collections;
using System.Threading;
using Gradual.OMS.AutomacaoDesktop.Consumer;

namespace Gradual.OMS.AutomacaoDesktop.Listeners
{
    public class BovespaLivroOfertasListener : UpdateListener
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private LinkedBlockingQueue<EventoBovespa> filaMensagensLivroOfertas;

        public BovespaLivroOfertasListener( DadosGlobais dadosGlobais, int maximoItens)
        {
            filaMensagensLivroOfertas = new LinkedBlockingQueue<EventoBovespa>();

            BovespaLivroOfertasConsumer consumer = new BovespaLivroOfertasConsumer(
                dadosGlobais,
                filaMensagensLivroOfertas,
                maximoItens);

            Thread thconsumer = new Thread(new ThreadStart(consumer.Run));
            thconsumer.Start();

            return;
        }

        public void Update(EventBean[] newEvents, EventBean[] oldEvents)
        {

            EventoBovespa bovEvent = (EventoBovespa)newEvents[0].Underlying;

            try
            {
                filaMensagensLivroOfertas.Push(bovEvent);
                if (logger.IsDebugEnabled)
                   logger.Debug("Mensagens na fila: " + filaMensagensLivroOfertas.Count);
            }

            catch (Exception e)
            {
                logger.Error(e);
            }
        }

    }
}

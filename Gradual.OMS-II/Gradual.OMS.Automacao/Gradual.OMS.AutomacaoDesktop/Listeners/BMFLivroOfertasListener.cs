using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.espertech.esper.client;
using log4net;
using com.espertech.esper.compat.collections;
using Gradual.OMS.AutomacaoDesktop.Events;
using Gradual.OMS.AutomacaoDesktop.Consumer;
using System.Threading;

namespace Gradual.OMS.AutomacaoDesktop.Listeners
{
    public class BMFLivroOfertasListener :UpdateListener
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private LinkedBlockingQueue<EventoBMF> filaMensagensLivroOfertas;

        /**
         * Construtor que cria a fila e instancia a camada consumer.
         * 
         * @param dadosGlobais Atributo que é repassado para a classe consumer.
         * @param maximoItens Quantidade maxima de itens no livro de ofertas.
         */
        public BMFLivroOfertasListener( DadosGlobais dadosGlobais, int maximoItens)
        {
            this.filaMensagensLivroOfertas = new LinkedBlockingQueue<EventoBMF>();

            BMFLivroOfertasConsumer consumer = new BMFLivroOfertasConsumer(dadosGlobais, filaMensagensLivroOfertas, maximoItens);
            consumer.Start();

            return;
        }

        /**
         * Método chamado quando recebe os eventos BMF. 
         * Os eventos são tratados e inseridos na fila da camada consumer.
         */
        public void Update(EventBean[] newEvents, EventBean[] oldEvents)
        {

            EventoBMF bmEvent = (EventoBMF)newEvents[0].Underlying;

            try
            {
                filaMensagensLivroOfertas.Push(bmEvent);
                logger.Debug("Mensagens na fila: " + filaMensagensLivroOfertas.Count);
            }

            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }



    }
}

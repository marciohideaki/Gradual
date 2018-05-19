using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Sinal;
using Gradual.MDS.Eventos.Lib;
using Gradual.MDS.Core.Lib;

namespace Gradual.MDS.Core
{
    public class ContainerManager
    {
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ContainerManager _me = null;

        public LivroOfertasConsumerBase LivroOfertasConsumer {get;set;}
        public NegociosConsumerBase NegociosConsumer { get; set; }
        public NewsConsumerBase NewsConsumer { get; set; }

        public static ContainerManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new ContainerManager();
                }

                return _me;
            }
        }

        public void Start(Dictionary<string, ChannelUDMF> _dctCanais)
        {
            LivroOfertasConsumer = new LivroOfertasConsumerBase(_dctCanais);
            LivroOfertasConsumer.Start();

            NegociosConsumer = new NegociosConsumerBase(_dctCanais);
            NegociosConsumer.Start();

            NewsConsumer = new NewsConsumerBase(_dctCanais);
            NewsConsumer.Start();

        }

        public void EnqueueEventoUmdf(EventoUmdf e)
        {
            //logger.DebugFormat("Enfileirando msg [{0}] para LOF e NEG consumer", e.MsgSeqNum);

            if (!e.MsgType.Equals(ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA) &&
                !e.MsgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SECURITYSTATUS) )
            {
                LivroOfertasConsumer.EnqueueEventoUmdf(e);
            }

            NegociosConsumer.EnqueueEventoUmdf(e);
        }

        public void EnqueueNews(EventoUmdf e)
        {
            NewsConsumer.EnqueueEventoUmdf(e);
        }

        public void EnqueueFIX(EventoFIX e)
        {
            LivroOfertasConsumer.EnqueueFIX(e);
            NegociosConsumer.EnqueueFIX(e);
        }


        public void Stop()
        {
            if ( LivroOfertasConsumer != null )
                LivroOfertasConsumer.Stop();

            if ( NewsConsumer != null )
                NewsConsumer.Stop();

            if ( NegociosConsumer != null )
                NegociosConsumer.Stop();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.espertech.esper.client;
using Gradual.MDS.Eventos.Lib;
using log4net;

namespace Gradual.MDS.Core
{
    public class NesperManager
    {
        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static NesperManager _me = null;

        public EPServiceProvider epService { get; set; }

        public static NesperManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new NesperManager();
                }

                return _me;
            }
        }


        public void Configure()
        {
            logger.Info("Configurando NEsper");
            
            logger.Info("Adicionando event Types");

            Configuration nesperConfig = new Configuration();

            // ATP: enriquecer essa parte, parametrizar, etc
            nesperConfig.AddEventType<EventoUmdf>();
            nesperConfig.AddEventType<EventoHttpLivroOfertas>();
            nesperConfig.AddEventType<EventoHttpLivroOfertasAgregado>();
            nesperConfig.AddEventType<EventoHttpSonda>();
            nesperConfig.AddEventType<EventoHttpNegocio>();
            nesperConfig.AddEventType<EventoHBNegocio>();
            nesperConfig.AddEventType<EventoHBLivroOfertas>();
            nesperConfig.AddEventType<EventoHBLivroOfertasAgregado>();
            nesperConfig.AddEventType<EventoNegocioANG>();
            logger.Info("Inicializando EPL");

            epService = EPServiceProviderManager.GetProvider(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, nesperConfig);
            
            epService.Initialize();

            epService.EPRuntime.UnmatchedEvent += new EventHandler<UnmatchedEventArgs>(EPRuntime_UnmatchedEvent);
        }

        void EPRuntime_UnmatchedEvent(object sender, UnmatchedEventArgs e)
        {
            logger.Error("UnmatchedEvent [" + e.Event.EventType.ToString() + "]");

                EventoUmdf evento = e.Event.Underlying as EventoUmdf;

                if (evento != null)
                {
                    logger.Debug("UNMATCHED " +
                                        "MsgSeqNum=[" + evento.MsgSeqNum +
                                        "] MsgType=[" + evento.MsgType +
                                        "] TemplateId=[" + evento.TemplateID +
                                        "] Segment=[" + evento.UmdfSegment +
                                        "] Channel=[" + evento.ChannelID +
                                        "] message=[" + evento.MsgBody.ToString() + "]");
                }
        }

    }
}

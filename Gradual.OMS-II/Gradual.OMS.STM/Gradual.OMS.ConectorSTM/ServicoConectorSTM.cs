using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.ConectorSTM.Lib;
using Gradual.OMS.Library.Servicos;
using com.espertech.esper.client;
using log4net;
using Gradual.OMS.ConectorSTM.Eventos;
using System.ServiceModel;

namespace Gradual.OMS.ConectorSTM
{
    public class ServicoConectorSTM : IServicoControlavel, IServicoSTM
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;
        private ClienteSTM clistm;
        private ProcessadorMensagens processadormsgs;
        private List<IServicoSTMCallback> _subscribers = new List<IServicoSTMCallback>();
        private List<STMSubscriberWorker> _workers = new List<STMSubscriberWorker>();

        public static EPServiceProvider epService;


        #region IServicoControlavel Members
        public void IniciarServico()
        {
            Configuration esperConfig = new Configuration();

            //ATP: toda essa parte aqui tem que carregar dinamicamente 
            // e ficar parametrizada
            //ATP: Start fucking unparametrized section
            // Tipar o evento no nesper
            esperConfig.AddEventType<EventoSTM>();
            esperConfig.AddEventType<EventoMega>();
            esperConfig.AddEventType<EventoCBLC>();

            startEsper(esperConfig);

            processadormsgs = new ProcessadorMensagens();
            processadormsgs.Start();

            clistm = new ClienteSTM();
            clistm.Start();

            _status = ServicoStatus.EmExecucao;
        }

        public void PararServico()
        {
            clistm.Stop();
            processadormsgs.Stop();

            _status = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion //IServicoControlavel Members

        #region IServicoSTM Members
        public AssinarEventosSTMResponse AssinarEventosSTM(AssinarEventosSTMRequest request)
        {
            AssinarEventosSTMResponse response = new AssinarEventosSTMResponse();
            response.Status = "OK";

            try
            {
                response.Status = "ERRO";

                IServicoSTMCallback subscriber = Ativador.GetCallback<IServicoSTMCallback>();

                logger.Debug("Recebeu pedido de assinatura de mensagens: " + ((IContextChannel)subscriber).RemoteAddress.ToString());

                // Guarda a referencia do assinante na lista interna de
                // assinantes
                if (subscriber != null)
                {
                    lock (_subscribers)
                    {
                        _subscribers.Add(subscriber);
                    }

                    STMSubscriberWorker worker = new STMSubscriberWorker(subscriber);
                    worker.Start();

                    lock (_workers)
                    {
                        _workers.Add(worker);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("AssinarEventosSTM() Erro: " + ex.Message, ex);

                response.Status = "ERRO";
                response.Mensagem = ex.Message;
            }

            return response;
        }
        #endregion //IServicoSTM Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        private void startEsper(Configuration config)
        {
            logger.Info("Inicializando EPL");
            epService = EPServiceProviderManager.GetProvider(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, config);
            epService.Initialize();
        }

    }
}

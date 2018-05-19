using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Automacao.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Automacao.Lib.Mensagens;
using Gradual.OMS.Library;
using com.espertech.esper.client;
using Gradual.OMS.AutomacaoDesktop.Events;
using log4net;
using Gradual.OMS.AutomacaoDesktop.Monitors;
using Gradual.OMS.AutomacaoDesktop.Adapters;
using System.IO;

namespace Gradual.OMS.AutomacaoDesktop
{
    public class ServicoAutomacaoDesktop : IServicoControlavel, IServicoAutomacaoDesktop
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;
        private AutomacaoConfig parametros = null;
        private DadosGlobais dadosGlobais = null;
        //private GeradorEventosBovespa gerador = null;
        private GeradorEventosDump2Bovespa gerador = null;
        private RetransmissorBMF retransmissorBMF = null;


        #region IServicoControlavel Members
        public void IniciarServico()
        {

            logger.Info("Iniciando Servico Automacao de Ordens");

            parametros = GerenciadorConfig.ReceberConfig<AutomacaoConfig>();

            dadosGlobais = new DadosGlobais();
            dadosGlobais.Parametros = parametros;
            dadosGlobais.KeepRunning = true;

            Configuration esperConfig = new Configuration();

            //ATP: toda essa parte aqui tem que carregar dinamicamente 
            // e ficar parametrizada
            //ATP: Start fucking unparametrized section
            // Tipar o evento no nesper
            esperConfig.AddEventType<EventoBovespa>();
            esperConfig.AddEventType<EventoBMF>();

            startEsper(esperConfig);

            // Carrega livros de ofertas
            PersistenciaMarketData persistencia = new PersistenciaMarketData(dadosGlobais);
            if (File.Exists(parametros.DiretorioDB + "\\LofBov.dat"))
            {
                persistencia.LoadLOFBovespa();
            }

            // O monitor faz o registro do evento na engine, associando a um listener
            AutomacaoMonitorBase monitor = new BovespaLivroOfertasMonitor(dadosGlobais);
            monitor.Start();

            // O monitor faz o registro do evento na engine, associando a um listener
            AutomacaoMonitorBase monitorLOFBMF = new BMFLivroOfertasMonitor(dadosGlobais);
            monitorLOFBMF.Start();

            // Inicia o adapter. O adapter deve sempre ser iniciado apos o 
            // evento ser conhecido e registrado no nesper

            //gerador = new GeradorEventosBovespa(dadosGlobais);
            gerador = new GeradorEventosDump2Bovespa(dadosGlobais);
            gerador.Start();

            retransmissorBMF = new RetransmissorBMF(dadosGlobais);
            retransmissorBMF.Start();

            //ATP: End of fucking unparametrized section

            _status = ServicoStatus.EmExecucao;

            logger.Info("Servico Automacao de Ordens iniciado");
        }

        public void PararServico()
        {
            logger.Info("Finalizando Servico Automacao de Ordens");
            dadosGlobais.KeepRunning = false;

            //ATP: start fucking unparametrized section
            PersistenciaMarketData persistencia = new PersistenciaMarketData(dadosGlobais);
            persistencia.SaveLOFBovespa();

            gerador.Stop();
            retransmissorBMF.Stop();

            //ATP: end of fuckng unparametrized section

            _status = ServicoStatus.Parado;

            logger.Info("Servico Automacao de Ordens finalizado");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion // IServicoControlavel Members

        #region IServicoAutomacaoDesktop Members
        public CadastrarRoboResponse CadastrarRobo(CadastrarRoboRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion //IServicoAutomacaoDesktop Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        private void startEsper(Configuration config) 
        {
    	    EPServiceProvider epService;
    	
    	    logger.Info("Inicializando EPL");
            epService = EPServiceProviderManager.GetProvider(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, config);
            epService.Initialize();
            dadosGlobais.EpService = epService;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using log4net;
using System.Threading;
using Gradual.Spider.ServicoSupervisor.Memory;
using Gradual.Spider.ServicoSupervisor.Cotacao;
using Gradual.Spider.SupervisorRiscoADM.Lib;
using System.ServiceModel;
using Gradual.Spider.ServicoSupervisor.Cron;
using Gradual.Spider.ServicoSupervisor.Calculator;

namespace Gradual.Spider.ServicoSupervisor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoSupervisorRisco : IServicoControlavel, ISupervisorRiscoAdm
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        private ServicoStatus _status = ServicoStatus.Parado;
        CronStyleScheduler _cron;
        CronTasks _cTask;
        public void IniciarServico()
        {
            try
            {

                logger.Info("**********************************************************************");
                logger.Info("**********************************************************************");
                logger.Info("ServicoSupervisorRisco v." + typeof(ServicoSupervisorRisco).Assembly.GetName().Version);
                logger.Info("*** IniciarServico(): ServicoSupervisorRisco....");

                logger.Info("Iniciando cron scheduler do servico...");
                _cTask = CronTasks.GetInstance();

                _cron = new CronStyleScheduler();
                _cron.Start();

                logger.Info("Iniciando carga das informacoes de risco...");
                Thread t = new Thread(new ThreadStart(_loadRiskData));
                t.Start();

                _status = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do servico ServicoSupervisorRisco: " + ex.Message, ex);
                throw;
            }

        }

        
        public void PararServico()
        {
            RiskClientManager.Instance.Stop();

            logger.Info("*** PararServico(): ServicoSupervisorRisco....");
            _status = ServicoStatus.Parado;

            logger.Info("Parando Cron scheduler...");
            if (_cron != null)
                _cron.Stop();
            _cron = null;

            

            logger.Info("Parando Acompanhamento Cache");
            AcSpiderCache.Instance.Stop();

            logger.Info("Parando o Bmf P/L Calculator...");
            BmfCalculator.Instance.Stop();

            logger.Info("Finalizando gerenciador dos RiskClients");
            RiskClientManager.Instance.Stop();

            logger.Info("Finalizando gerenciador Risco Consolidado...");
            ConsolidatedRiskManager.Instance.Stop();
            
            logger.Info("Finalizando manager PositionClient...");
            PositionClientManager.Instance.Stop();
            
            logger.Info("Desconectando do MDS");
            CotacaoManager.Instance.Stop();

            logger.Info("*** PararServico(): ServicoSupervisorRisco FINALIZADO!!....");
        }

        
        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }


        void _loadRiskData()
        {
            try
            {
                Thread.Sleep(15000);
                // Calculos BMF
                BmfCalculator.Instance.Start();

                RiskCache.Instance.LoadRiskData();

                // Calculo de risco consolidado
                ConsolidatedRiskManager.Instance.Start();
                // Calculo de posicao de cliente
                PositionClientManager.Instance.Start();
                // Recebimento de acompanhamento de ordens
                AcSpiderCache.Instance.Start();
                // Conexoes e memoria "cliente"
                RiskClientManager.Instance.Start();

                CotacaoManager.Instance.OnNegocio += new OnMDSNegocioHandler(RiskCache.Instance.Instance_OnNegocio);
                CotacaoManager.Instance.Start();
                
                
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no _loadRiskData / start dos managers: " + ex.Message, ex);
            }
        }

        public bool ReloadResync()
        {
            try
            {
                // RiskCache.Instance.LoadRiskData();
                RiskClientManager.Instance.SendSnapshot(null);
            }
            catch (Exception ex)
            {
                logger.Error("ReloadResync:" + ex.Message, ex);
            }

            return true;
        }
    }
}

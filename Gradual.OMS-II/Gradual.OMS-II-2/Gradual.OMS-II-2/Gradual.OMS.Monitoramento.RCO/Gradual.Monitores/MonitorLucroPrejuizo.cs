using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Risco.Info;
using Gradual.Monitores.Risco;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using log4net;

namespace Gradual.Monitores.Risco
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProfitLossMonitor : IServicoMonitorRisco, IServicoControlavel
    {

        ServerMonitor _Monitor = new ServerMonitor();

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IServicoControlavel Members

        ServicoStatus _ServiceStatus { set; get; }

        public void IniciarServico()
        {
            try
            {
                //log4net.Config.XmlConfigurator.Configure();

                _Monitor.StartMonitor(true);                           
                Thread.Sleep(2000);

                logger.Info("Monitor Iniciado com sucesso."); 
                _ServiceStatus = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            } 

        }

        public void PararServico()
        {
            logger.Info("Tentando parar o servico");
           // _Monitor.LoopControler = false;
            _Monitor = null;
            GC.Collect();
        
            _ServiceStatus = ServicoStatus.Parado;
            logger.Info("Servico Parado com sucesso.");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServiceStatus;
        }

        #endregion

        #region IProfitLossMonitor Members

        public MonitorLucroPrejuizoResponse ObterMonitorLucroPrejuizo(MonitorLucroPrejuizoRequest pRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoMonitorRisco Members


        public MonitorPLDResponse ObterMonitorPLD(MonitorPLDRequest pRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoMonitorRisco Members


        public ObterDadosClienteResponse ObterDadosCliente(ObterDadosClienteRequest pRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoMonitorRisco Members


        public ObterPosicaoBtcResponse ObterPosicaoBTC(ObterPosicaoBtcRequest pRequest)
        {
            throw new NotImplementedException();
        }

        public ObterPosicaoTermoResponse ObterPosicaoTermo(ObterPosicaoTermoRequest pRequest)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

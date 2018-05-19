using Gradual.Spider.PositionClient.Monitor.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;

namespace Gradual.Spider.PositionClient.Monitor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MonitorPositionClientServer : IServicoPositionClientMonitor, IServicoControlavel
    {
        #region Atributos
        /// <summary>
        /// Booleano que indica se o serviço está rodando ou não
        /// </summary>
        private bool _bKeepRunning;

        /// <summary>
        /// Atributo de log da classe
        /// </summary>
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Objeto de status do serviço
        /// </summary>
        private ServicoStatus _ServicoStatus;
        #endregion

        #region Construtores
        /// <summary>
        /// Construtor do serviço
        /// </summary>
        public MonitorPositionClientServer()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que inicia do serviço gerenciado pelo Serviço controlável
        /// </summary>
        public void IniciarServico()
        {
            try
            {

                _bKeepRunning = true;
                _ServicoStatus = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("IniciarServico()  StackTrace - {0}", ex.StackTrace);
            }
        }

        /// <summary>
        /// Método que para o serviço gerenciado pelo serviço controlável
        /// </summary>
        public void PararServico()
        {
            try
            {
                _bKeepRunning = false;
                _ServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("PararServico()  StackTrace - {0}", ex.StackTrace);
            }
        }

        /// <summary>
        /// Método que retorna o status do serviço, gerenciado pelo serviço controlável
        /// </summary>
        /// <returns>Retorna o objeto de status do serviço gerenciado pelo serviço controlável</returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        public Lib.Message.BuscarPositionClientResponse BuscarPositionClient(Lib.Message.BuscarPositionClientRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public Lib.Message.BuscarOperacoesIntradayResponse BuscarOperacoesIntraday(Lib.Message.BuscarOperacoesIntradayRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public Lib.Message.BuscarRiscoResumidoResponse BuscarRiscoResumido(Lib.Message.BuscarRiscoResumidoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion




        public string BuscarOperacoesIntradayJSON()
        {
            throw new NotImplementedException();
        }
    }
}

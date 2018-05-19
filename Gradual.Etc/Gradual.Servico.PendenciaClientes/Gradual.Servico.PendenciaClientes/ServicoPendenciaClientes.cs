using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using log4net;

namespace Gradual.Servico.PendenciaClientes
{
    public class ServicoPendenciaClientes : IServicoControlavel
    {
        private ServicoStatus GetServicoStatus { set; get; }

        private NotificacaoPendencias gNotificacaoPendencias;

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServicoPendenciaClientes()
        {
            log4net.Config.XmlConfigurator.Configure();     
        }

        public void IniciarServico()
        {
            try
            {
                gLogger.Info("Iniciando Serviço de envio de email com pendenca cadastral [START].");
                gNotificacaoPendencias = new NotificacaoPendencias();
                gNotificacaoPendencias.ServicoIniciar();

                GetServicoStatus = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                gLogger.Info(string.Format("Falha ao iniciar o Servico de envio de email com pendenca cadastral [START]. ", ex.Message));
                GetServicoStatus = ServicoStatus.Erro;
            }
        }

        public void PararServico()
        {
            try
            {
                gNotificacaoPendencias.ServicoParar();
                gNotificacaoPendencias = null;
                gLogger.Info("Encerrando  Servico de Opções [STOP].");
                GetServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                gLogger.Info(string.Concat("Falha ao encerrar o Servico de Opções [START]. ", ex.Message));
                GetServicoStatus = ServicoStatus.Erro;
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return GetServicoStatus;
        }
    }
}

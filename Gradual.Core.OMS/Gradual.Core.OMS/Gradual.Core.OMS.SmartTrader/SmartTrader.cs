using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using log4net;

using Gradual.OMS.Library.Servicos;
using Gradual.Core.OMS.SmartTrader.Lib;
using Gradual.Core.OMS.SmartTrader.Lib.Mensagens;

namespace Gradual.Core.OMS.SmartTrader
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SmartTrader:IServicoControlavel, ISmartTrader
    {

        #region log4net declaration

        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        ServicoStatus _status;

        ProcessarOrdemSmart _orderProcessing;
        #endregion

        // Constructor / Destructor
        public SmartTrader()
        {
            _status = ServicoStatus.Indefinido;
            _orderProcessing = null;
        }

        ~SmartTrader()
        {
            
        }


        #region IServicoControlavel Members
        public void IniciarServico()
        {
            try
            {
                logger.Info("**********************************************************************");
                logger.Info("**********************************************************************");
                logger.Info("Iniciando Servico SmartTrader");

                _status = ServicoStatus.EmExecucao;
                _orderProcessing = new ProcessarOrdemSmart();
            }
            catch (Exception ex)
            {
                _status = ServicoStatus.Erro;
                logger.Error("Erro ao iniciar o servico: " + ex.Message, ex);
            }
        }


        public void PararServico()
        {
            try
            {
                logger.Info("Parando Servico SmartTrader");

                _orderProcessing = null;
                _status = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao parar o servico: " + ex.Message, ex);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion


        #region ISmartTrader Members
        public void DummyFunction()
        {
            logger.Info("DummyFunction");
        }

        public EnviarOrdemSmartResponse EnviarOrdemSmart(EnviarOrdemSmartRequest ordemReq)
        {
            try
            {
                EnviarOrdemSmartResponse resp = new EnviarOrdemSmartResponse();
                resp = _orderProcessing.EnviarOrdem(ordemReq);
                return resp;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio da ordem smart: " + ex.Message, ex);
                return null;
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using System.ServiceModel;
using log4net;
using Gradual.Core.Spider.AcompanhamentoOrdens.Cache;
using System.Threading;

namespace Gradual.Core.Spider.AcompanhamentoOrdens
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AcompanhamentoServer:IServicoControlavel
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        ServicoStatus _status;
        OrderCache _orderMng = null;


        #endregion

        public AcompanhamentoServer()
        {
            
        }

        #region IServicoControlavel Members
        public void IniciarServico()
        {
            try
            {
                logger.Info("Iniciando AcompanhamentoOrdens Spider...");
                
                logger.Info("Efetuando carga do acompanhamento de ordens...");
                
                OrderCache.GetInstance().Start();

                _status = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na inicializacao do servico...: " + ex.Message, ex);
                throw ex;
            }
        }

        public void PararServico()
        {
            try
            {
                logger.Info("Finalizando AcompanhamentoOrdens Spider...");
                OrderCache.GetInstance().Stop();
                _status = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na parada do servico... " + ex.Message, ex);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        #endregion


    }
}

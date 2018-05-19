using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.Spider.Acompanhamento4Socket.Cache;
using Gradual.Spider.Acompanhamento4Socket.Rede;
using System.Threading;

namespace Gradual.Spider.Acompanhamento4Socket
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Acompanhamento4SocketSrv: IServicoControlavel
    {


        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region private variables
        ServicoStatus _status;
        A4SocketClient _a4Client;

        #endregion

        public Acompanhamento4SocketSrv()
        {


        }

        #region IServicoControlavel Members
        public void IniciarServico()
        {
            try
            {
                logger.Info("**********************************************************************");
                logger.Info("**********************************************************************");
                logger.Info("Iniciando Spider - Acompanhamento4Socket - Ordens Spider");
                logger.Info("Acompanhamento4Socket v." + typeof(Acompanhamento4SocketSrv).Assembly.GetName().Version);
                logger.Info("Iniciando OrderCache 4 Socket...");
                // Thread.Sleep(20000);
                OrderCache4Socket.GetInstance().Start();

                //logger.Info(" Iniciando o roteador callback...");
                //RoteadorCallback.GetInstance().Start();

                logger.Info("Iniciando A4Socket Server...");
                A4SocketSrv.GetInstance().Start();

                logger.Info("Iniciando A4Socket Client...");
                _a4Client = new A4SocketClient();
                _a4Client.Start();


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
                _status = ServicoStatus.Parado;

                
                logger.Info("Parando A4SocketServer...");
                A4SocketSrv.GetInstance().Stop();
 
                
                logger.Info("Parando A4SocketClient...");
                if (null != _a4Client)
                {
                    _a4Client.Stop();
                    _a4Client = null;
                }

                logger.Info("Parando OrderCache 4 Socket...");
                OrderCache4Socket.GetInstance().Stop();

                logger.Info("Parando RoteadorCallback...");
                RoteadorCallback.GetInstance().Stop();


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

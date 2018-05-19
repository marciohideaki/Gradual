using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using System.Configuration;
using AS.Sockets;
using System.Diagnostics;
using log4net;


namespace Gradual.OMS.AutenticacaoMDS
{
    public class AutenticacaoMDS : IServicoControlavel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region IServicoControlavel Members

        private ServicoStatus _ServicoStatus { set; get; }
        private SocketPackage SockPkg = new SocketPackage();

        public void IniciarServico()
        {
            try
            {
                int PortNumber = int.Parse(ConfigurationManager.AppSettings["NumeroPorta"].ToString());
                SockPkg.StartListen(PortNumber);
            }
            catch (Exception ex)
            {
                logger.Error("Erro em IniciarServico():" + ex.Message, ex);
                throw ex;
            }

            _ServicoStatus = ServicoStatus.EmExecucao;
        }

        public void PararServico()
        {
            SockPkg.StopListen();
            _ServicoStatus = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion
    }
}

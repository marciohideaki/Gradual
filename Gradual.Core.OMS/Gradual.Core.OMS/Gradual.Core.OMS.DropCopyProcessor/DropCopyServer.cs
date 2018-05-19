using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.Core.OMS.DropCopyProcessor.Database;
using System.Diagnostics;
using System.IO;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Gradual.Core.OMS.DropCopyProcessor.Rede;
using Gradual.Core.OMS.DropCopy.Lib;
using Gradual.Core.OMS.DropCopy.Lib.Mensagens;
using System.ServiceModel.Channels;

namespace Gradual.Core.OMS.DropCopyProcessor
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DropCopyServer:IServicoControlavel, IAssinaturaDropCopyCallback
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        ServicoStatus _status;
        List<FixSessionItem> _lstCfg;
        List<FixInitiatorDropCopy> _lstfixInitiatorDC;
        #endregion

        public DropCopyServer()
        {
            _lstCfg = new List<FixSessionItem>();
            _lstfixInitiatorDC = new List<FixInitiatorDropCopy>();
        }

        #region IServicoControlavel Members
        public void IniciarServico()
        {
            try
            {
                logger.Info("**********************************************************************");
                logger.Info("**********************************************************************");
                logger.Info("*** IniciarServico(): DropCopyServer....");
                // Carrega configurações a partir do banco de dados
                DbDropCopy dbDropCopy = new DbDropCopy();

                string strExecFile = Process.GetCurrentProcess().MainModule.FileName;
                strExecFile = strExecFile.Substring(strExecFile.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                logger.Info("ExecFile: " + strExecFile);
                _lstCfg = dbDropCopy.BuscarSessoesFIXServer(strExecFile);
                List<FixSessionItem> lstDropCopyInit = new List<FixSessionItem>();

                //logger.Info("Iniciando Gerenciador de Limites");
                //LimiteManager.GetInstance().Start();
                string strServerName = System.Net.Dns.GetHostName() + "." + System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                logger.Info("Atualizando 'server names' das sessoes fix");
                foreach (FixSessionItem item in _lstCfg)
                {
                    if (item.ConnectionType.ToLower().Equals(FixConnectionType.DROPCOPY_INITIATOR))
                        lstDropCopyInit.Add(item);

                    // Atualizar o nome do servidor para definir a "origem" das sessoes
                    dbDropCopy.AtualizarServerName(item.IdSessaoFix, strServerName);
                }

                //Inicia Thread de tratamento de callbacks
                DropCopyCallbackManager.Instance.Start();

                if (lstDropCopyInit.Count == 0)
                {
                    logger.Info("Configurações de FixDropCopy Initiator não encontradas. Não criará o SocketInitator DropCopy");
                }
                else
                {
                    foreach (FixSessionItem item in lstDropCopyInit)
                    {
                        FixInitiatorDropCopy aux = new FixInitiatorDropCopy(item);
                        _lstfixInitiatorDC.Add(aux);
                        logger.Info("IniciarServico(): iniciando SocketInitiator DropCopy");
                        aux.Start();
                    }
                }
                logger.Info("*** IniciarServico(): DropCopyServer inicializado...");
                dbDropCopy = null;
                _status = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na inicializacao do servico: " + ex.Message, ex);
                _status = ServicoStatus.Erro;
                throw ex;
            }
        }

        public void PararServico()
        {
            try
            {
                logger.Info("*** Parando DropCopyServer...");
                
                if (_lstfixInitiatorDC != null)
                {
                    int length = _lstfixInitiatorDC.Count;
                    for (int i = 0; i < length; i++)
                    {
                        logger.Info("Parando Fix SocketInitiator");
                        _lstfixInitiatorDC[i].Stop();
                        _lstfixInitiatorDC[i] = null;
                    }
                    _lstfixInitiatorDC.Clear();
                    _lstfixInitiatorDC = null;
                }

                DbDropCopy dbDropCopy = new DbDropCopy();
                logger.Info("Atualizando 'server names' das sessoes fix");
                foreach (FixSessionItem item in _lstCfg)
                {
                    dbDropCopy.AtualizarServerName(item.IdSessaoFix, null);
                }
                _lstCfg.Clear();
                _lstCfg = null;
                dbDropCopy = null;

                logger.Info("Finalizando DropCopyCallbackManager");
                DropCopyCallbackManager.Instance.Stop();

                _status = ServicoStatus.Parado;
                logger.Info("*** DropCopyServer finalizado");
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do servico: " + ex.Message, ex);
            }
        }
        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion

        #region IAssinaturaDropCopyCallback Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AssinarDropCopyResponse AssinarDropCopy(AssinarDropCopyRequest request)
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            logger.Info("Recebeu assinatura de dropcopy [" + endpoint.Address.ToString() + ":" + endpoint.Port.ToString() + "]");

            DropCopyCallbackManager.Instance.AssinarDropCopyCallback();

            AssinarDropCopyResponse response = new AssinarDropCopyResponse();
            response.StatusResposta = Gradual.OMS.Library.MensagemResponseStatusEnum.OK;

            return response;
        }
        #endregion //IAssinaturaDropCopyCallback Members
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;
using System.IO;
using System.Diagnostics;

using log4net;
using QuickFix;

using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using Gradual.Core.OMS.FixServerLowLatency.Rede;
using Gradual.Core.OMS.FixServerLowLatency.Database;
using Gradual.Core.OMS.FixServerLowLatency.Util;
using Gradual.Core.OMS.FixServerLowLatency.Lib;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Cortex.OMS.ServidorFIX;
using Cortex.OMS.ServidorFIXAdm.Lib;
using Cortex.OMS.ServidorFIXAdm.Lib.Mensagens;



namespace Gradual.Core.OMS.FixServerLowLatency
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FixServer : IServicoControlavel, IFixServerLowLatencyAdm
    {

        #region log4net declaration
        
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        ServicoStatus _status;
        // Pode ouvir varias sessoes, a partir da mesma porta
        FixAcceptor _fixAcceptor;
        FixDropCopy _fixDropCopy;
        // Usando array (list), para separar cada tipo de sessao initiator para bolsa
        List <FixInitiator> _lstfixInitiator;
        //List<FixDropCopy> _lstfixDropCopy;
        GeneralTasks _tasks;
        Gradual.Core.OMS.FixServerLowLatency.Util.CronStyleScheduler _cron;
        DbFix _dbFix = null;
        List<FixSessionItem> _lstCfg;
        #endregion

        public FixServer()
        {
            _status = ServicoStatus.Indefinido;
            _fixAcceptor = null;
            //_fixDropCopy = null;
            _lstfixInitiator = new List<FixInitiator>();
            //_lstfixDropCopy = new List<FixDropCopy>();
            _tasks = GeneralTasks.GetInstance();
            _cron = new Gradual.Core.OMS.FixServerLowLatency.Util.CronStyleScheduler();
            
        }

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            try
            {
                logger.Info("**********************************************************************");
                logger.Info("**********************************************************************");
                logger.Info("*** IniciarServico(): FixServer....");
                // Thread.Sleep(20000);
                // Carrega configurações a partir do banco de dados
                _dbFix = new DbFix();
                string strExecFile = Process.GetCurrentProcess().MainModule.FileName;
                
                strExecFile = strExecFile.Substring(strExecFile.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                logger.Info("ExecFile: " + strExecFile);
                
                _lstCfg = _dbFix.BuscarSessoesFIXServer(strExecFile);
                List<FixSessionItem> lstInit = new List<FixSessionItem>();
                List<FixSessionItem> lstAccept = new List<FixSessionItem>();
                List<FixSessionItem> lstDropCopy = new List<FixSessionItem>();

                //logger.Info("Iniciando Gerenciador de Limites");
                //LimiteManager.GetInstance().Start();
                string strServerName = System.Net.Dns.GetHostName() + "." + System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                logger.Info("Atualizando 'server names' das sessoes fix");
                foreach (FixSessionItem item in _lstCfg)
                {
                    if (item.ConnectionType.ToLower().Equals(FixConnectionType.ACCEPTOR))
                        lstAccept.Add(item);

                    if (item.ConnectionType.ToLower().Equals(FixConnectionType.INITIATOR))
                        lstInit.Add(item);

                    if (item.ConnectionType.ToLower().Equals(FixConnectionType.DROPCOPY_ACCEPTOR))
                        lstDropCopy.Add(item);

                    // Atualizar o nome do servidor para definir a "origem" das sessoes
                    _dbFix.AtualizarServerName(item.IdSessaoFix, strServerName);
                }

                // Criar primeiramente Initiator, para ser passado como parametro para as sessoes acceptor
                if (lstInit.Count == 0)
                {
                    logger.Info("Configurações de FixInitiator não encontradas. Não criará o SocketInitiator");
                }
                else
                {
                    foreach (FixSessionItem item in lstInit)
                    {
                        FixInitiator aux = new FixInitiator(item);
                        _lstfixInitiator.Add(aux);
                        logger.Info("IniciarServico(): iniciando SocketInitiator");
                        aux.Start();
                    }
                    // Atribuir as sessoes para executar as tarefas (como dump do dicionario de msgs)
                    _tasks.SetFixInitiators(_lstfixInitiator);
                }

                if (lstDropCopy.Count == 0)
                {
                    logger.Info("Configurações de FixDropCopy não encontradas. Não criará o SocketAcceptor DropCopy");
                }
                else
                {
                    logger.Info("IniciarServico(): iniciando Sessao Drop Copy ");
                    _fixDropCopy = new FixDropCopy(lstDropCopy);
                    _fixDropCopy.Start();
                    //_lstfixDropCopy.Add(aux);
                    
                }

                if (lstAccept.Count == 0)
                {
                    logger.Info("Configurações de FixAcceptor não encontradas. Não criará o SocketAcceptor");
                }
                else
                {
                    logger.Info("IniciarServico(): iniciando SocketAcceptor");
                    _fixAcceptor = new FixAcceptor(lstAccept, _lstfixInitiator, _fixDropCopy);
                    _fixAcceptor.Start();
                }

                logger.Info("IniciarServico(): iniciando crontab Scheduler");
                if (null == _cron)
                    _cron = new Util.CronStyleScheduler();
                _cron.Start();


                logger.Info("*** IniciarServico(): FixServer inicializado...");

                _dbFix = null;
                _status = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.Error("Erro inicializacao: " + ex.Message, ex);
                throw ex;
            }
            
        }
        
        public void PararServico()
        {
            try
            {
                logger.Info("*** Parando FixServer");
                if (_fixAcceptor != null)
                {
                    logger.Info("Parando Fix SocketAcceptor");
                    _fixAcceptor.Stop();
                    _fixAcceptor = null;
                }

                if (_fixDropCopy != null)
                {
                    logger.Info("Parando Fix SocketAcceptor - Drop Copy");
                    _fixDropCopy.Stop();
                    _fixDropCopy = null;
                }

                if (_lstfixInitiator != null)
                {
                    int length = _lstfixInitiator.Count;
                    for (int i = 0; i < length; i++)
                    {
                        logger.Info("Parando Fix SocketInitiator");
                        _lstfixInitiator[i].Stop();
                        _lstfixInitiator[i] = null;
                    }
                    _lstfixInitiator.Clear();
                    _lstfixInitiator = null;
                }
                _tasks = null;
                DbFix dbFix = new DbFix();
                logger.Info("Atualizando 'server names' das sessoes fix");
                foreach (FixSessionItem item in _lstCfg)
                {
                    dbFix.AtualizarServerName(item.IdSessaoFix, null);
                }
                _lstCfg.Clear();
                _lstCfg = null;
                dbFix = null;
                //logger.Info("Parando Gerenciador de Limites");
                //LimiteManager.GetInstance().Stop();
                //logger.Info("Parando Cron Tab Scheduler");

                if (null != _cron)
                {
                    _cron.Stop();
                    _cron = null;
                }

                _status = ServicoStatus.Parado;
                
                
                logger.Info("*** FixServer finalizado");

                // Forcar queda do executavel 
            }
            catch (Exception ex)
            {
                logger.Error("*** Erro na parada do servico: " + ex.Message, ex);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion
        
        #region IFixServerLowLatencyAdm Members
        public OrderCancelingResponse CancelOrder(OrderCancelingRequest req)
        {
            OrderCancelingResponse resp = new OrderCancelingResponse();
            try
            {
                if (null != _lstfixInitiator)
                {
                    // Buscar o FixInitiator de acordo com o canal 
                    int canal = Convert.ToInt32(req.ChannelID);
                    FixInitiator item = _lstfixInitiator.FirstOrDefault(x => x.Canal.ToString() == req.ChannelID);
                    if (null != item)
                    {
                        item.CancelOrderFromIntranet(req.Account, req.OrigClOrdID, req.ExchangeNumberID, req.Symbol);
                        resp.StatusResponse = 1;
                    }
                    else
                    {
                        resp.StatusResponse = 0;
                        resp.DescricaoErro = "Canal não encontrado para efetuar cancelamento da oferta";
                    }
                }
                else
                {
                    resp.StatusResponse = 0;
                    resp.DescricaoErro = "Lista de Initiators não definida";
                }
                return resp;
            }
            catch (Exception ex)
            {
                logger.Error("CancelOrder(): Problemas no cancelamento da ordem: " + ex.Message, ex);
                resp.StatusResponse = 0;
                resp.StackTrace = ex.StackTrace;
                resp.DescricaoErro = ex.Message;
            }
            return resp;
        }

        #endregion
    }
}


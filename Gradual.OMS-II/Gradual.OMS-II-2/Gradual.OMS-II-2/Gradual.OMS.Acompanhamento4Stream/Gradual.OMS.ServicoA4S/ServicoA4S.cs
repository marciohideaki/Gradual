using System;
using System.Collections.Generic;
using System.Threading;
using Gradual.OMS.A4s.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using System.Configuration;
using Gradual.Core.OMS.DropCopy.Lib;
using Gradual.Core.OMS.DropCopy.Lib.Mensagens;
using System.Collections.Concurrent;


namespace Gradual.OMS.ServicoA4S
{
    public class ServicoA4S : IServicoControlavel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _servicoStatus = ServicoStatus.Parado;

        private Queue<OrdemInfo> queue2MDS = new Queue<OrdemInfo>();
        private Queue<StatusConexaoBolsaInfo> queueStatus = new Queue<StatusConexaoBolsaInfo>();
        private Queue<DadosRequisicaoAcompanhamento> queueAssinatura = new Queue<DadosRequisicaoAcompanhamento>();
        private Dictionary<int, List<OrdemInfo>> gAcompanhamentosEmMemoria = new Dictionary<int, List<OrdemInfo>>();                         // Dicionário de acompanhamentos que ficam em memória; a chave é o id do cliente
        private bool _bKeepRunning = false;
        private Thread _thOrdemToMdsDispatcher = null;
        private Thread _thStatusProcessor = null;
        private Thread gThreadDeCallBack;
        private Thread _thSinacorProcessor = null;
        private DateTime _lastStatus = DateTime.MinValue;
        private DateTime _lastCapturaSinacor = DateTime.MinValue;
        private DateTime _lastCapturaClientes = DateTime.MinValue;
        private IAssinaturasRoteadorOrdensCallback gClienteRoteadorOrdens = null;
        private CamadaDeDados gCamadaDeConsulta;                                                                                            // Classe de acesso a dados para consultas
        private OrdemAlteradaCallBack gCallBacker = null;
        private Dictionary<int, int> gCodigosBmfEmMemoria = new Dictionary<int, int>();                                                      // Dicionário de códigos BMF que ficam em memória; a chave é o id do cliente
        private long ultimodiacarregado;
        private Dictionary<string, List<string>> dctContasSinacor = new Dictionary<string, List<string>>();
        private Queue<DadosRequisicaoAcompanhamento> qConta = new Queue<DadosRequisicaoAcompanhamento>();
        private DateTime _ultimaConsultaSinacor;
        private List<string> lstClientesAtivos = new List<string>();

        private SocketPackage sckServer = null;

        private A4SConfig _config = null;
        private bool _processandoVirada = false;

        private Thread thFIXDropCopyCallback = null;
        private Thread thDropCopyProcessor = null;
        private DropCopyCallbackImpl dropCopyImpl = null;
        private IAssinaturaDropCopyCallback dropCopyClient = null;
        private Queue<OrdemInfo> qDropCopy = new Queue<OrdemInfo>();
        private bool bProcessarSpider = false;
        private long lastDropCopyHeartbeat = 0;
        private ConcurrentQueue<OrdemInfo> qFromRoteador = new ConcurrentQueue<OrdemInfo>();
        private Thread _thAcompanhamentoProcessor = null;
        private bool bRemoveDigito = true;


        #region IServicoControlavel
        /// <summary>
        /// 
        /// </summary>
        public void IniciarServico()
        {
            logger.Info("Iniciando ServicoA4S");

            _config = GerenciadorConfig.ReceberConfig<A4SConfig>();

            logger.Info("Portas Bovespa do OMS .............: " + _config.PortasOMS);
            logger.Info("Debug do evento de ordens .........: " + _config.DebugOrdem);
            logger.Info("Debug do status de conexao bolsa ..: " + _config.DebugStatus);
            logger.Info("Intervalo de captura Sinacor ......: " + _config.IntervaloSinacor);

            if (ConfigurationManager.AppSettings["DropCopySpider"] != null)
            {
                bProcessarSpider = ConfigurationManager.AppSettings["DropCopySpider"].ToString().ToLower().Equals("true");
            }


            if (ConfigurationManager.AppSettings["AccountStripDigit"] != null)
            {
                bRemoveDigito = ConfigurationManager.AppSettings["AccountStripDigit"].ToString().ToLower().Equals("true");

                logger.WarnFormat("REMOCAO DO DIGITO BOVESPA ESTA {0}", bRemoveDigito ? "HABILITADO" : "DESABILITADO");
            }

            _bKeepRunning = true;
            _ultimaConsultaSinacor = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);


            sckServer = new SocketPackage();

            sckServer.OnClientConnected += new ClientConnectedHandler(sckServer_OnClientConnected);
            sckServer.OnRequestReceived += new MessageReceivedHandler(sckServer_OnMessageReceived);

            _thAcompanhamentoProcessor = new Thread(new ThreadStart(this.monitorAcompanhamento));
            _thAcompanhamentoProcessor.Start();

            _thOrdemToMdsDispatcher = new Thread(new ThreadStart(ordemToMDSDispatcher));
            _thOrdemToMdsDispatcher.Start();

            _thStatusProcessor = new Thread(new ThreadStart(StatusProcessor));
            _thStatusProcessor.Start();

            _thSinacorProcessor = new Thread(new ThreadStart(SinacorProcessor));
            _thSinacorProcessor.Start();

            if (bProcessarSpider)
            {
                thDropCopyProcessor = new Thread(new ThreadStart(dropCopyProcessor));
                thDropCopyProcessor.Name = "dropCopyProcessor";
                thDropCopyProcessor.Start();
            }

            IniciarThreadDeCallBack();
            
            _CarregarOrdensAtivas();

            sckServer.StartListen(_config.ListenPort);

            _servicoStatus = ServicoStatus.EmExecucao;

            logger.Info("ServicoA4S Iniciado");
        }


        /// <summary>
        ///  Carrega todas as ordens ativas
        /// </summary>
        private void _CarregarOrdensAtivas()
        {
            try
            {
                lock(gAcompanhamentosEmMemoria)
                {
                    gAcompanhamentosEmMemoria.Clear();

                    gCamadaDeConsulta = new CamadaDeDados();

                    logger.Debug("Abrindo conexão com o banco de dados para consultas...");

                    gCamadaDeConsulta.AbrirConexao();

                    if (gCamadaDeConsulta.ConexaoAberta)
                        logger.Debug("Conexão aberta com sucesso.");

                    List<OrdemInfo> ordemsativas = gCamadaDeConsulta.BuscarOrdensStreamer();

                    foreach (OrdemInfo ordem in ordemsativas)
                    {
                        List<OrdemInfo> ordenscli = null;

                        if (gAcompanhamentosEmMemoria.ContainsKey(ordem.Account))
                        {
                            ordenscli = gAcompanhamentosEmMemoria[ordem.Account];

                            if (ordenscli == null)
                            {
                                ordenscli = new List<OrdemInfo>();
                            }

                            logger.InfoFormat("Tipo de Ordem no _CarregarOrdensAtivas - [{0}]", ordem.OrdType.ToString());

                            ordenscli.Add(ordem);

                            gAcompanhamentosEmMemoria[ordem.Account] = ordenscli;

                            
                        }
                        else
                        {
                            ordenscli = new List<OrdemInfo>();
                            ordenscli.Add(ordem);
                            gAcompanhamentosEmMemoria.Add(ordem.Account, ordenscli);
                        }
                    }

                    gCamadaDeConsulta.FecharConexao();

                    if (bProcessarSpider)
                    {
                        // Buscar ordens do OMS Spider
                        PersistenciaSpiderDB spiderDB = new PersistenciaSpiderDB();
                        List<OrdemInfo> ordemsSpider = spiderDB.BuscarOrdensSpider();

                        foreach (OrdemInfo ordem in ordemsSpider)
                        {
                            List<OrdemInfo> ordenscli = null;

                            if (gAcompanhamentosEmMemoria.ContainsKey(ordem.Account))
                            {
                                ordenscli = gAcompanhamentosEmMemoria[ordem.Account];

                                if (ordenscli == null)
                                {
                                    ordenscli = new List<OrdemInfo>();
                                }

                                ordenscli.Add(ordem);

                                gAcompanhamentosEmMemoria[ordem.Account] = ordenscli;
                            }
                            else
                            {
                                ordenscli = new List<OrdemInfo>();
                                ordenscli.Add(ordem);
                                gAcompanhamentosEmMemoria.Add(ordem.Account, ordenscli);
                            }
                        }
                    }

                    ultimodiacarregado = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd"));
                }
            }
            catch(Exception ex )
            {
                logger.Error("Erro em _CarregarOrdensAtivas(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PararServico()
        {
            logger.Info("Finalizando ServicoA4S");

            _bKeepRunning = false;

            while (_thOrdemToMdsDispatcher != null && _thOrdemToMdsDispatcher.IsAlive)
                Thread.Sleep(250);

            while (_thStatusProcessor != null && _thStatusProcessor.IsAlive)
                Thread.Sleep(250);

            while (this._thAcompanhamentoProcessor != null && _thAcompanhamentoProcessor.IsAlive)
                Thread.Sleep(250);

            _servicoStatus = ServicoStatus.Parado;

            logger.Info("ServicoA4S Finalizado");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _servicoStatus;
        }

        #endregion // IServicoControlavel



        /// <summary>
        /// Thread de processamento da fila de acompanhamento de ordens
        /// </summary>
        private void ordemToMDSDispatcher()
        {
            logger.Info("Inicio thread OrdemProcessor()");
            while (_bKeepRunning)
            {
                try
                {
                    lock (queue2MDS)
                    {
                        while (queue2MDS.Count > 0)
                        {
                            OrdemInfo ordem = queue2MDS.Dequeue();
                            JsonSerializerSettings settings = new JsonSerializerSettings();

                            settings.Converters.Add(new JsonBaludaDateTimeConverter());
                            settings.Converters.Add(new JsonOrderTypeConverter());

                            JsonConverter[] converters = new JsonConverter[2];
                            converters[0] = new JsonBaludaDateTimeConverter();
                            converters[1] = new JsonOrderTypeConverter();

                            string msg = JsonConvert.SerializeObject(ordem, Formatting.None, settings);

                            // TODO: serializar ordem
                            logger.Debug("A4S >> Streamer: [" + ordem.ClOrdID +
                                "] OrigClOrdId [" + ordem.OrigClOrdID +
                                "] Channel [" + ordem.ChannelID +
                                "] Conta [" + ordem.Account +
                                "] Papel [" + ordem.Symbol +
                                "] Acompanhamentos [" + ordem.Acompanhamentos.Count + "]" +
                                "] OrdStatus [" + ordem.OrdStatus + "]" +
                                "] OrdType [" + ordem.OrdType.ToString() + "]");

                            sckServer.SendToAll("O" + msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("OrdemProcessor: " + ex.Message, ex);
                }

                Thread.Sleep(100);
            }

            logger.Info("Fim thread OrdemProcessor()");
        }


        /// <summary>
        /// Thread de processamento da fila de status de conexao com as bolsas
        /// </summary>
        private void StatusProcessor()
        {
            logger.Info("Inicio thread StatusProcessor()");

            while (_bKeepRunning)
            {
                try
                {
                    TimeSpan lastInterval = new TimeSpan(DateTime.Now.Ticks - _lastStatus.Ticks);

                    lock (queueStatus)
                    {
                        while (queueStatus.Count > 0)
                        {
                            StatusConexaoBolsaInfo status = queueStatus.Dequeue();

                            string msg = JsonConvert.SerializeObject(status);

                            sckServer.SendToAll("S" + msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("StatusProcessor: " + ex.Message, ex);
                }

                Thread.Sleep(100);
            }

            logger.Info("Fim thread StatusProcessor()");
        }




        private void SinacorProcessor()
        {
            try
            {
                while (_bKeepRunning)
                {

                    
                    DadosRequisicaoAcompanhamento[] reqs = null;
                    List<String> lstLogins = new List<string>();
                    StringBuilder builder = new StringBuilder();

                    lock (qConta)
                    {
                        if (qConta.Count > 0)
                        {
                            reqs = qConta.ToArray();

                            qConta.Clear();
                        }
                    }


                    // Processa as requisicoes de assinatura (novos logins de usuarios GTI)
                    if (reqs != null && reqs.Length > 0)
                    {
                        foreach (DadosRequisicaoAcompanhamento req in reqs)
                        {
                            if (!dctContasSinacor.ContainsKey(req.account))
                            {
                                List<string> sessions = new List<string>();
                                sessions.Add(req.sessionID);
                                dctContasSinacor.Add(req.account, sessions);
                            }
                            else
                            {
                                List<string> sessions = dctContasSinacor[req.account];

                                if (!sessions.Contains(req.sessionID))
                                {
                                    sessions.Add(req.sessionID);
                                    dctContasSinacor[req.account] = sessions;
                                }
                            }

                            lstLogins.Add(req.account);
                        }

                        for (int i=0, j=0; i < lstLogins.Count; i++)
                        {
                            if ( j == 50 || i==lstLogins.Count-1 )
                            {
                                j = 0;
                                BuscarOrdensOutrosSistemasLogin(builder.ToString());
                                builder.Clear();
                            }
                            else
                            {
                                lock (lstClientesAtivos)
                                {
                                    if (lstClientesAtivos.Contains(lstLogins[i]))
                                    {
                                        builder.Append(lstLogins[i]);
                                        builder.Append(",");
                                        j++;
                                    }
                                }
                            }
                        }
                    }

                    // Busca clientes ativos para melhorar busca pelas ordens no Sinacor
                    TimeSpan lastInterval = new TimeSpan(DateTime.Now.Ticks - this._lastCapturaClientes.Ticks); 
                    if (lastInterval.TotalSeconds > 60)
                    {
                        logger.Debug("Buscando lista de clientes ativos hoje.");
                        BuscarListaClientesAtivosHoje();
                    }


                    if (ConfigurationManager.AppSettings["BuscarOrdensSinacor"] != null &&
                        ConfigurationManager.AppSettings["BuscarOrdensSinacor"].ToString().ToLower().Equals("true"))
                    {
                        lastInterval = new TimeSpan(DateTime.Now.Ticks - this._lastCapturaSinacor.Ticks);
                        if (lastInterval.TotalSeconds > _config.IntervaloSinacor)
                        {
                            logger.Debug("Buscando ordems de outros sistemas");

                            BuscarOrdensOutrosSistemas();
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                logger.Error("AssinaturaProcessor: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void BuscarListaClientesAtivosHoje()
        {
            try
            {
                List<string> lstTmp = new List<string>();

                CamadaDeDados db = new CamadaDeDados();

                lstTmp = db.BuscarClientesAtivosHoje();

                this._lastCapturaClientes = DateTime.Now;

                lock (lstClientesAtivos)
                {
                    lstClientesAtivos = lstTmp;
                }
            }
            catch (Exception ex)
            {
                logger.Error("BuscarListaClientesAtivosHoje: " + ex.Message, ex);
            }

        }

        private void BuscarOrdensOutrosSistemasLogin(string account)
        {
            try
            {
                if (ConfigurationManager.AppSettings["BuscarOrdensSinacor"] != null &&
                    ConfigurationManager.AppSettings["BuscarOrdensSinacor"].ToString().ToLower().Equals("true"))
                {
                    List<OrdemInfo> lstOrdemSinacor = new List<OrdemInfo>();

                    logger.Debug("Capturando ordens de outros novo login: [" + account + "]");

                    CamadaDeDados db = new CamadaDeDados();

                    DateTime hj = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    lstOrdemSinacor = db.BuscarOrdensOutrasPlataformas(_config.PortasOMS, account, hj);

                    foreach (OrdemInfo ordem in lstOrdemSinacor)
                    {
                        gCallBacker_ChegadaDeAcompanhamento(ordem);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("BuscarOrdensOutrosSistemas: " + ex.Message, ex);
            }
        }

        //TODO: implementar o conteudo dessa porra
        private void BuscarOrdensOutrosSistemas()
        {
            try
            {
                List<OrdemInfo> lstOrdemSinacor = new List<OrdemInfo>();
                List<String> lstCapturas = new List<string>();
                StringBuilder builder = new StringBuilder();

                //TODO: popular a lista
                // usar _config.PortasOMS, dctContasSinacor;
                lock (dctContasSinacor)
                {
                    string[] arrContas = new string[dctContasSinacor.Keys.Count];

                    if (dctContasSinacor.Count <= 0) return;

                    dctContasSinacor.Keys.CopyTo(arrContas, 0);

                    int i = 0;
                    foreach (string account in dctContasSinacor.Keys)
                    {
                        lock (lstClientesAtivos)
                        {
                            if (lstClientesAtivos.Contains(account))
                            {
                                builder.Append(account);
                                builder.Append(",");
                                i++;
                            }
                        }

                        if (i == 50)
                        {
                            lstCapturas.Add(builder.ToString());
                            builder.Clear();
                            i = 0;
                        }
                    }

                    if ( builder.Length > 0 )
                        lstCapturas.Add(builder.ToString());
                }

                if (ConfigurationManager.AppSettings["BuscarOrdensSinacor"] != null && 
                    ConfigurationManager.AppSettings["BuscarOrdensSinacor"].ToString().ToLower().Equals("true"))
                {
                    foreach (string contas in lstCapturas)
                    {
                        logger.Debug("Capturando ordens de outros sistemas para as contas: [" + contas + "] [" + _ultimaConsultaSinacor.ToString("dd/MM/yyyy HH:mm:ss.fff") + "]");

                        CamadaDeDados db = new CamadaDeDados();

                        lstOrdemSinacor = db.BuscarOrdensOutrasPlataformas(_config.PortasOMS, contas, _ultimaConsultaSinacor.AddSeconds(-30));

                        if (lstOrdemSinacor.Count > 0)
                        {
                            foreach (OrdemInfo ordem in lstOrdemSinacor)
                            {
                                gCallBacker_ChegadaDeAcompanhamento(ordem);
                            }
                            _ultimaConsultaSinacor = DateTime.Now;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("BuscarOrdensOutrosSistemas: " + ex.Message, ex);
            }
            finally
            {
                _lastCapturaSinacor = DateTime.Now;
            }

        }

        /// <summary>
        /// Event Handler do evento de conexao de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void sckServer_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            string msg = string.Empty;
            try
            {
                logger.Info( "Recebeu conexao do StreamerServer: " + args.ClientNumber );

                // Ao receber a conexao, enviar as ordens em memoria
                lock (gAcompanhamentosEmMemoria)
                {
                    JsonConverter[] converters = new JsonConverter[2];
                    converters[0] = new JsonBaludaDateTimeConverter();
                    converters[1] = new JsonOrderTypeConverter();

                    msg = JsonConvert.SerializeObject(new Dictionary<int, List<OrdemInfo>>(), converters);
                    if (msg != null)
                    {
                        sckServer.SendData("G" + msg, args.ClientSocket);
                    }

                    foreach (List<OrdemInfo> lstOrdens in gAcompanhamentosEmMemoria.Values)
                    {
                        foreach (OrdemInfo info in lstOrdens)
                        {
                            JsonSerializerSettings settings = new JsonSerializerSettings();

                            settings.Converters.Add(new JsonBaludaDateTimeConverter());
                            settings.Converters.Add(new JsonOrderTypeConverter());

                            converters = new JsonConverter[2];
                            converters[0] = new JsonBaludaDateTimeConverter();
                            converters[1] = new JsonOrderTypeConverter();

                            msg = JsonConvert.SerializeObject(info, Formatting.None, settings);

                            // TODO: serializar ordem
                            logger.Debug("A4S >> Streamer: [" + info.ClOrdID +
                                "] OrigClOrdId [" + info.OrigClOrdID +
                                "] Channel [" + info.ChannelID +
                                "] Conta [" + info.Account +
                                "] Papel [" + info.Symbol +
                                "] Acompanhamentos [" + info.Acompanhamentos.Count + "]" +
                                "] OrdStatus [" + info.OrdStatus + "]" +
                                "] OrdType [" + info.OrdType.ToString() + "]");

                            sckServer.SendData("O" + msg, args.ClientSocket);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("OnClientConnected: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Event Handler para evento de recebimento de mensagens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private  void sckServer_OnMessageReceived(object sender, MessageEventArgs args)
        {
            try
            {
                string msg = args.Message;
                string tipo = args.TipoMsg;

                DadosRequisicaoAcompanhamento req = JsonConvert.DeserializeObject<DadosRequisicaoAcompanhamento>(msg);

                if (req.account.Equals("AcompanhamentoOrdens"))
                {
                    logger.Error("Assinatura invalida: [" + req.sessionID + "]");
                    return;
                }

                lock (dctContasSinacor)
                {
                    switch (req.acao)
                    {
                        case DadosRequisicaoAcompanhamento.SUBSCRIBE:
                            logger.Debug("Sessao [" + req.sessionID + "] assinou conta [" + req.account + "]");

                            lock (qConta)
                            {
                                if (!qConta.Contains(req))
                                {
                                    qConta.Enqueue(req);
                                }
                            }

                            /*
                            if (!dctContasSinacor.ContainsKey(req.account))
                            {
                                List<string> sessions = new List<string>();
                                sessions.Add(req.sessionID);
                                dctContasSinacor.Add(req.account, sessions);
                            }
                            else
                            {
                                List<string> sessions = dctContasSinacor[req.account];

                                if (!sessions.Contains(req.sessionID))
                                {
                                    sessions.Add(req.sessionID);
                                    dctContasSinacor[req.account] = sessions;
                                }
                            }*/
                            break;
                        case DadosRequisicaoAcompanhamento.UNSUBSCRIBE:
                            logger.Debug("Sessao [" + req.sessionID + "] cancelou assinatura conta [" + req.account + "]");
                            if (dctContasSinacor.ContainsKey(req.account))
                            {
                                List<string> sessions = dctContasSinacor[req.account];

                                if (sessions.Contains(req.sessionID))
                                {
                                    sessions.Remove(req.sessionID);
                                    dctContasSinacor[req.account] = sessions;
                                }

                                if (sessions.Count == 0)
                                {
                                    dctContasSinacor.Remove(req.account);
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("OnMessageReceived: " + ex.Message, ex);
            }
        }

        private void gCallBacker_ChegadaDeAcompanhamento(OrdemInfo pOrdemAlterada)
        {
            try
            {
                qFromRoteador.Enqueue(pOrdemAlterada);
            }
            catch (Exception ex)
            {
                logger.Error("gCallBacker_ChegadaDeAcompanhamento(): " + ex.Message, ex);
            }
        }

        private void monitorAcompanhamento()
        {
            logger.Info("Iniciando thread de monitoracao dos acompanhamentos de ordem");
            while (_bKeepRunning)
            {
                try
                {
                    OrdemInfo pOrdemAlterada = null;

                    if (qFromRoteador.TryDequeue(out pOrdemAlterada))
                    {
                        _processaChegadaDeAcompanhamento(pOrdemAlterada);

                        continue;
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("monitorAcompanhamento(): " + ex.Message, ex);
                }
            }

            logger.Info("Finalizando thread de monitoracao dos acompanhamentos de ordem");
        }


        private void _processaChegadaDeAcompanhamento(OrdemInfo pOrdemAlterada)
        {
            try
            {
                OrdemInfo ordemoriginal = null;

                // Está combinado com o roteador (Ponso) que sempre vem um OrdemInfo com 1 AcompanhamentoInfo; safety check:
                //if (pOrdemAlterada.Acompanhamentos.Count == 0)
                //{
                //    logger.Debug("OrdemInfo para ordem [{0}] está sem acompanhamentos na lista, ignorando..."
                //                               , pOrdemAlterada.ClOrdID);

                //    return;
                //}
                //else if (pOrdemAlterada.Acompanhamentos.Count > 1)
                //{
                //    logger.Debug("OrdemInfo para ordem [{0}] tem [{1}] acompanhamentos, somente um era esperado! O primeiro será gravado, mas os outros serão perdidos!!"
                //                               , pOrdemAlterada.ClOrdID
                //                               , pOrdemAlterada.Acompanhamentos.Count);
                //}

                lock (gAcompanhamentosEmMemoria)
                {
                    logger.Info("Chegou Acompanhamento: [" + pOrdemAlterada.ClOrdID +
                        "] OrigClOrdId [" + pOrdemAlterada.OrigClOrdID +
                        "] Channel [" + pOrdemAlterada.ChannelID +
                        "] Exch [" + pOrdemAlterada.Exchange +
                        "] Conta [" + pOrdemAlterada.Account +
                        "] Papel [" + pOrdemAlterada.Symbol +
                        "] Acompanhamentos [" + pOrdemAlterada.Acompanhamentos.Count + "]" +
                        "] OrdStatus [" + pOrdemAlterada.OrdStatus + "]" +
                        " OrdType [" + pOrdemAlterada.OrdType.ToString()  + "]");

                    string lMensagem = "";

                    int lCodDaOrdem = pOrdemAlterada.Account;

                    if (bRemoveDigito)
                    {
                        if (pOrdemAlterada.Exchange != null && pOrdemAlterada.Exchange.ToUpper() == "BOVESPA")
                            lCodDaOrdem = Convert.ToInt32(pOrdemAlterada.Account.ToString().Substring(0, pOrdemAlterada.Account.ToString().Length - 1)); // Trunca o ultimo digito do codigo do cliente (DV) quando for mercado BOVESPA
                    }

                    int lCodBovespa = lCodDaOrdem;                                                                                                   //Assume o mais comum, que o código da ordem é o código bovespa
                    int lCodBmf;

                    // Se foi rejeicao de uma modificacao de ordem, efetua um tratamento diferenciado
                    if (pOrdemAlterada.OrigClOrdID != null &&
                        pOrdemAlterada.OrigClOrdID.Length > 0 &&
                        pOrdemAlterada.OrdStatus == OrdemStatusEnum.REJEITADA)
                    {
                        logger.Warn("Modificacao [" + pOrdemAlterada.ClOrdID + "] da ordem [" + pOrdemAlterada.OrigClOrdID + "] rejeitada, enfileirando para chamada de prc_trocar_numero_controle");

                        CamadaDeDados db = new CamadaDeDados();

                        ordemoriginal = db.BuscarOrdemOriginal(pOrdemAlterada);

                        logger.Debug("Buscou ordem original [" + ordemoriginal.ClOrdID + "] [" + ordemoriginal.Symbol + "] p=[" + ordemoriginal.Price + "] q=[" + ordemoriginal.OrderQty + "]");
                    }

                    if (!gAcompanhamentosEmMemoria.ContainsKey(lCodDaOrdem))                                                                         // Verifica se já tem no dicionário assumindo que a ordem chegou como conta bovespa mesmo
                    {
                        foreach (int lCodBov in gCodigosBmfEmMemoria.Keys)
                        {
                            if (gCodigosBmfEmMemoria[lCodBov] == lCodDaOrdem)
                            {
                                lCodBovespa = lCodBov;
                                lCodBmf = lCodDaOrdem;

                                logger.InfoFormat(" (ordem executada com código BMF [{0}], mapeada para código bovespa [{1}])", lCodBmf, lCodBovespa);

                                break;
                            }
                        }
                    }

                    // Se não tem um dicionário para esse código de cliente, adiciona (isso pode acontecer se chegarem ordens de outros sistemas, que não chamaram o InformarLogin)
                    if (!gAcompanhamentosEmMemoria.ContainsKey(lCodBovespa))
                    {
                        gAcompanhamentosEmMemoria.Add(lCodBovespa, new List<OrdemInfo>());

                        logger.Debug("(lista pra conta do cliente ainda não existia)");
                    }

                    bool bAcompanhamentoFound = false;
                    OrdemInfo ordem2mds       = _cloneOrdemInfo(pOrdemAlterada);
                    ordem2mds.Account         = lCodDaOrdem;
                    pOrdemAlterada.Account    = lCodDaOrdem;

                    for (int a = gAcompanhamentosEmMemoria[lCodBovespa].Count - 1; a >= 0; a--)
                    {
                        if (gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID == pOrdemAlterada.ClOrdID)                                             // Verifica se a ordem já existia na memória, ou seja, é um novo acompanhamento de uma ordem que já havia recebido ao menos um acompanhamento previamente
                        {
                            if (pOrdemAlterada.OrigClOrdID != null &&
                                pOrdemAlterada.OrigClOrdID.Length > 0 &&
                                pOrdemAlterada.OrdStatus == OrdemStatusEnum.REJEITADA)
                            {
                                gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID = pOrdemAlterada.OrigClOrdID;

                                if (ordemoriginal != null)
                                {
                                    logger.Debug("Restaurando ultimo status da ordem [" + ordemoriginal.ClOrdID + "]");

                                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrigClOrdID        = ordemoriginal.OrigClOrdID;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrdStatus          = ordemoriginal.OrdStatus;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQtyRemmaining = ordemoriginal.OrderQtyRemmaining;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQty           = ordemoriginal.OrderQty;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].MinQty             = ordemoriginal.MinQty;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].MaxFloor           = ordemoriginal.MaxFloor;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQty           = ordemoriginal.OrderQty;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].Price              = ordemoriginal.Price;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].ExpireDate         = ordemoriginal.ExpireDate;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].TransactTime       = ordemoriginal.TransactTime;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrdType            = ordemoriginal.OrdType;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].CompIDOMS          = ordemoriginal.CompIDOMS;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].TimeInForce        = ordemoriginal.TimeInForce;
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].CumQty             = ordemoriginal.CumQty;

                                    // Nao eh legal isso, mas estava igualando o clordid com origclordid
                                    if (gAcompanhamentosEmMemoria[lCodBovespa][a].OrigClOrdID.Equals(gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID))
                                        gAcompanhamentosEmMemoria[lCodBovespa][a].OrigClOrdID = "";

                                    logger.InfoFormat("Tipo de Ordem no gCallBacker_ChegadaDeAcompanhamento - [{0}]", ordemoriginal.OrdType.ToString());

                                    pOrdemAlterada.ExpireDate   = ordemoriginal.ExpireDate;
                                    pOrdemAlterada.TransactTime = DateTime.Now;

                                    foreach (AcompanhamentoOrdemInfo acompanhamento in pOrdemAlterada.Acompanhamentos)
                                    {
                                        AcompanhamentoOrdemInfo newacomp = _cloneAcompanhamentoInfo(acompanhamento);
                                        newacomp.NumeroControleOrdem = ordemoriginal.ClOrdID;

                                        gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.Add(newacomp);
                                    }

                                    foreach (AcompanhamentoOrdemInfo acompanhamento in gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos)
                                    {
                                        acompanhamento.NumeroControleOrdem = gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID;
                                    }

                                    lock (queue2MDS)
                                    {
                                        queue2MDS.Enqueue(gAcompanhamentosEmMemoria[lCodBovespa][a]);
                                    }
                                    //ordem2mds = _cloneOrdemInfo(gAcompanhamentosEmMemoria[lCodBovespa][a]);

                                    pOrdemAlterada.Acompanhamentos.Clear();
                                    foreach (AcompanhamentoOrdemInfo acompanhamento in gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos)
                                    {
                                        AcompanhamentoOrdemInfo newacomp = _cloneAcompanhamentoInfo(acompanhamento);
                                        newacomp.NumeroControleOrdem = pOrdemAlterada.ClOrdID;
                                        pOrdemAlterada.Acompanhamentos.Add(newacomp);
                                    }
                                    pOrdemAlterada.OrigClOrdID += "rst";

                                    //ordem2mds = _cloneOrdemInfo(pOrdemAlterada);
                                    //lock (queue2MDS)
                                    //{
                                    //    queue2MDS.Enqueue(ordem2mds);
                                    //}
                                    //bAcompanhamentoFound = true;
                                }
                                else
                                {
                                    logger.Error("Nao restaurou status da ordem [" + pOrdemAlterada.OrigClOrdID + "]");
                                }
                            }
                            else
                            {
                                // Atualiza a ordem
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrdStatus          = pOrdemAlterada.OrdStatus;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQtyRemmaining = pOrdemAlterada.OrderQtyRemmaining;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].CumQty             = pOrdemAlterada.CumQty;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].Price              = pOrdemAlterada.Price;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].TransactTime       = pOrdemAlterada.TransactTime;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrdType            = pOrdemAlterada.OrdType;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].CompIDOMS          = pOrdemAlterada.CompIDOMS;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].StopPrice          = pOrdemAlterada.StopPrice;

                                logger.InfoFormat("Tipo de Ordem no gCallBacker_ChegadaDeAcompanhamento - [{0}]", pOrdemAlterada.OrdType.ToString());

                                // Insere o detalhe/acompanhamento
                                if (pOrdemAlterada.Acompanhamentos.Count > 0)
                                {
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.Add(pOrdemAlterada.Acompanhamentos[0]);                        // Adiciona somente o acompanhamento que chegou
                                }

                                ordem2mds = _cloneOrdemInfo(gAcompanhamentosEmMemoria[lCodBovespa][a]);

                                logger.DebugFormat("Atualizacao da ordem [{0}] que já estava em memória", pOrdemAlterada.ClOrdID);

                                bAcompanhamentoFound = true;
                            }

                            break;
                        }

                        // Ordem modificada
                        if (gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID == pOrdemAlterada.OrigClOrdID)
                        {
                            if (pOrdemAlterada.OrdStatus != OrdemStatusEnum.REJEITADA)
                            {
                                // Atualiza a ordem
                                gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID = pOrdemAlterada.ClOrdID;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrigClOrdID = pOrdemAlterada.OrigClOrdID;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrdStatus = pOrdemAlterada.OrdStatus;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQtyRemmaining = pOrdemAlterada.OrderQtyRemmaining;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].CumQty = pOrdemAlterada.CumQty;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].TransactTime = pOrdemAlterada.TransactTime;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].Price = pOrdemAlterada.Price;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQty = pOrdemAlterada.OrderQty;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].ExpireDate = pOrdemAlterada.ExpireDate;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].TimeInForce = pOrdemAlterada.TimeInForce;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].CompIDOMS = pOrdemAlterada.CompIDOMS;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].StopPrice = pOrdemAlterada.StopPrice;

                                // Insere o detalhe/acompanhamento
                                if (pOrdemAlterada.Acompanhamentos.Count > 0)
                                {
                                    gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.Add(pOrdemAlterada.Acompanhamentos[0]);                        // Adiciona somente o acompanhamento que chegou
                                }

                                logger.DebugFormat(" Modificacao [{0}] da ordem [{1}] que já estava em memória", pOrdemAlterada.ClOrdID, pOrdemAlterada.OrigClOrdID);

                                bAcompanhamentoFound = true;
                            }
                            else
                            {
                                logger.InfoFormat(" Rejeicao do cancelamento [{0}] da ordem [{1}] que já estava em memória", pOrdemAlterada.ClOrdID, pOrdemAlterada.OrigClOrdID);
                                gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.AddRange(pOrdemAlterada.Acompanhamentos.ToArray());
                                ordem2mds = _cloneOrdemInfo(gAcompanhamentosEmMemoria[lCodBovespa][a]);
                                bAcompanhamentoFound = true;
                            }

                            break;
                        }

                    }

                    if (!bAcompanhamentoFound)
                    {
                        gAcompanhamentosEmMemoria[lCodBovespa].Add(pOrdemAlterada);

                        ordem2mds = _cloneOrdemInfo(pOrdemAlterada);

                        logger.Debug(" Acompanhamento de uma ordem que foi incluida em memória agora" + lMensagem);
                    }

                    lock (queue2MDS)
                    {
                        queue2MDS.Enqueue(ordem2mds);
                    }
                } //lock(gAcompanhamentosEmMemoria)
            }
            catch (Exception ex)
            {
                logger.Error("gCallBacker_ChegadaDeAcompanhamento(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Abre um thread separado para ficar verificando a fila de objetos para persistir e incluir no banco
        /// </summary>
        private  void IniciarThreadDeCallBack()
        {
            gThreadDeCallBack = new Thread(new ThreadStart(ObservarRoteadorParaCallBacks));

            gThreadDeCallBack.Start();

            if (bProcessarSpider)
            {
                thFIXDropCopyCallback = new Thread(new ThreadStart(fixDropCopyMonitor));
                thFIXDropCopyCallback.Name = "fixDropCopyProc";
                thFIXDropCopyCallback.Start();
            }
        }

        private void fixDropCopyMonitor()
        {
            try
            {
                if (this.dropCopyImpl == null)
                {
                    logger.Info("Assinando callback de drop copy");

                    dropCopyImpl = new DropCopyCallbackImpl();

                    dropCopyImpl.OnFIXDropCopy += new FIXDropCopyCallbackHandler(dropCopyClient_OnFIXDropCopy);
                    dropCopyImpl.OnHeartBeat += new FIXHeartBeatHandler(dropCopyClient_OnHeartBeat);
                }

                //ATP: 15/09/2010
                // Inclusao do tratamento da assinatura com roteador.
                // Refaz a conexao
                int i = 0;
                do
                {
                    // Se ficou mais de 60 segundos sem receber status
                    // de conexao, reinicia o channel WCF ( 1 tentativa a cada minuto) 
                    if (dropCopyImpl.LastTimeStampInterval() > 60)
                    {
                        if ( i > 600)
                        {
                            cancelaDropCopy();
                            assinarDropCopy(dropCopyImpl);

                            i = 0;
                        }
                        else
                            i++;
                    }

                    Thread.Sleep(100);
                }
                while (_bKeepRunning);
            }
            catch (Exception ex)
            {
                logger.Error("fixDropCopyMonitor():" + ex.Message, ex);
            }
        }

        private void assinarDropCopy(DropCopyCallbackImpl objectImpl)
        {
            try
            {
                logger.Info("Chamando ativador para instanciar o cliente do processador DropCopy...");

                dropCopyClient = Ativador.Get<IAssinaturaDropCopyCallback>(objectImpl);

                if (dropCopyClient != null)
                {
                    logger.Info("Cliente do processador DropCopy instanciado, enviando request de assinatura...");

                    // Faz a chamada pra abrir a conexão com o roteador; só serve pra enviar o contexto, e o roteador assinar a ponte duplex 
                    AssinarDropCopyResponse lResposta = dropCopyClient.AssinarDropCopy(new AssinarDropCopyRequest());

                    if (lResposta.StatusResposta == Library.MensagemResponseStatusEnum.OK)
                    {
                        logger.Info("Conexão com o processador DropCopy aberta, resposta do servidor: [" + lResposta.StatusResposta + "] [" + lResposta.DescricaoResposta + "]");
                    }
                    else
                    {
                        logger.Info("Conexão com o processador DropCopy nao aberta, resposta do servidor: [" + lResposta.StatusResposta + "] [" + lResposta.DescricaoResposta + "]");

                        dropCopyClient = null;                                                                                   // Setando como null pra tentar novamente depois, ver conforme o protocolo o que fazer
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em assinarDropCopy():" + ex.Message, ex);
            }

        }

        private void cancelaDropCopy()
        {
            try
            {
                Ativador.AbortChannel(dropCopyClient);
            }
            catch (Exception ex)
            {
                logger.Error("Erro em cancelaDropCopy():" + ex.Message, ex);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private void dropCopyClient_OnHeartBeat()
        {
            logger.Info("DropCopy Heartbeat");
            lastDropCopyHeartbeat = DateTime.Now.Ticks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAlteracao"></param>
        private void dropCopyClient_OnFIXDropCopy(OrdemInfo pAlteracao)
        {
            try
            {
                lock (qDropCopy)
                {
                    qDropCopy.Enqueue(pAlteracao);
                }
            }
            catch (Exception ex)
            {
                logger.Error("dropCopyClient_OnFIXDropCopy: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void dropCopyProcessor()
        {
            logger.Info("Iniciando thread de processamento de ordens Spider");

            PersistenciaSpiderDB spiderDB = new PersistenciaSpiderDB();

            while (_bKeepRunning)
            {
                try
                {
                    if (qDropCopy.Count > 0)
                    {
                        OrdemInfo info = null;

                        lock (qDropCopy)
                        {
                            info = qDropCopy.Dequeue();
                        }

                        if (info != null)
                        {
                            List<OrdemInfo> dbInfos = spiderDB.BuscarOrdemSpider(info.IdOrdem);

                            bool bFound = false;
                            // So ira ter uma ordem na lista...mas enfim
                            foreach( OrdemInfo dbInfo in dbInfos )
                            {
                                lock (gAcompanhamentosEmMemoria)
                                {
                                    if (gAcompanhamentosEmMemoria.ContainsKey(dbInfo.Account))
                                    {
                                        List<OrdemInfo> ordenscli = gAcompanhamentosEmMemoria[dbInfo.Account];

                                        if (ordenscli == null)
                                        {
                                            ordenscli = new List<OrdemInfo>();
                                        }

                                        for (int i = 0; i < ordenscli.Count; i++)
                                        {
                                            if (ordenscli[i].ClOrdID.Equals(dbInfo.ClOrdID))
                                            {
                                                ordenscli[i] = dbInfo;
                                                bFound = true;
                                                break;
                                            }
                                        }

                                        if (!bFound)
                                            ordenscli.Add(dbInfo);

                                        gAcompanhamentosEmMemoria[dbInfo.Account] = ordenscli;
                                    }
                                    else
                                    {
                                        List<OrdemInfo> ordenscli = new List<OrdemInfo>();
                                        ordenscli.Add(dbInfo);
                                        gAcompanhamentosEmMemoria.Add(dbInfo.Account, ordenscli);
                                    }
                                }

                                lock (queue2MDS)
                                {
                                    queue2MDS.Enqueue(dbInfo);
                                }
                            }
                        }
                    }
                    else
                        Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("dropCopyProcessor(): " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Rotina principal da thread de recepcao dos callbacks do Roteador
        /// </summary>
        private  void ObservarRoteadorParaCallBacks()
        {
            try
            {
                if (gClienteRoteadorOrdens == null)
                {
                    logger.Debug("Conexão não iniciada, abrindo...");

                    gCallBacker = new OrdemAlteradaCallBack();                                                                           // Classe responsável por receber a resposta do servidor via tcp e "alertar" o cliente que a contém usando um evento 
                    // (implementação logo abaixo, nesse mesmo arquivo)

                    gCallBacker.ChegadaDeAcompanhamento += new ChegadaDeAcompanhamentoHandler(gCallBacker_ChegadaDeAcompanhamento);      // Assina o evento do "callbakcer", é aqui que vão chegar efetivamente as "respostas" duplex do roteador de ordens
                    gCallBacker.OnStatusConexao += new StatusConexaoBolsaHandler(gCallBacker_OnStatusConexao);

                    _assinaCallbackRoteador(gCallBacker);
                }

                //ATP: 15/09/2010
                // Inclusao do tratamento da assinatura com roteador.
                // Refaz a conexao
                int i = 0;
                do
                {
                    // Se ficou mais de 60 segundos sem receber status
                    // de conexao, reinicia o channel WCF ( 1 tentativa a cada minuto) 
                    if (gCallBacker.LastTimeStampInterval() > 60 )
                    {
                        if ( i > 600)
                        {
                            _cancelRoteadorCallback();
                            _assinaCallbackRoteador(gCallBacker);

                            i = 0;
                        }
                        else
                            i++;
                    }

                    long hj = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd"));

                    if (hj > ultimodiacarregado && !_processandoVirada)
                    {
                        _processandoVirada = true;

                        ThreadPool.QueueUserWorkItem(new WaitCallback(_ProcessarViradaDia));  
                    }

                    Thread.Sleep(100);
                }
                while (_bKeepRunning);
            }
            catch (ThreadAbortException)
            {
                logger.Debug("Thread de observação do roteador sendo fechado");
            }
            catch (Exception ex)
            {
                logger.Error("observar roteador para CallBacks", ex);
            }
        }

        private void gCallBacker_OnStatusConexao(StatusConexaoBolsaInfo status)
        {
            try
            {
                lock (queueStatus)
                {
                    queueStatus.Enqueue(status);
                }
            }
            catch (Exception ex)
            {
                logger.Error("gCallBacker_OnStatusConexao()", ex);
            }
        }

        /// <summary>
        /// Efetua a recarga das ordens ao mudar o dia
        /// </summary>
        private void _ProcessarViradaDia(object objeto)
        {
            try
            {

                logger.Info("Virou dia, recarregar ordens validas... dando um tempo pra recarga");

                // Espera 5 minutos por conta da query... as vezes o SQL
                // esta com uma ligeira defasagem no horario e a query retorna
                // as ordens antigas
                Thread.Sleep(5 * 60 * 1000);

                logger.Info("Here we go, go, go, go!");

                _CarregarOrdensAtivas();
                // Ao receber a conexao, enviar as ordens em memoria

                string msg = null;
                lock (gAcompanhamentosEmMemoria)
                {
                    JsonConverter[] converters = new JsonConverter[2];
                    converters[0] = new JsonBaludaDateTimeConverter();
                    converters[1] = new JsonOrderTypeConverter();

                    msg = JsonConvert.SerializeObject(new Dictionary<int, List<OrdemInfo>>(), converters);
                    if (msg != null)
                    {
                        sckServer.SendToAll("G" + msg);
                    }

                    logger.Info("Na virada do dia, serializou " + gAcompanhamentosEmMemoria.Count + " itens");
                    foreach (List<OrdemInfo> lstOrdens in gAcompanhamentosEmMemoria.Values)
                    {
                        foreach (OrdemInfo info in lstOrdens)
                        {
                            lock (queue2MDS)
                            {
                                queue2MDS.Enqueue(info);
                            }
                        }
                    }
                }

                logger.Info("Fim do processamento da virada do dia");
            }
            catch (Exception ex)
            {
                logger.Error("_ProcessarViradaDia():" + ex.Message, ex);
            }
            finally
            {
                _processandoVirada = false;
            }
        }



        /// <summary>
        /// Aborta a conexao com Roteador
        /// </summary>
        private void _cancelRoteadorCallback()
        {
            try
            {
                Ativador.AbortChannel(gClienteRoteadorOrdens);
            }
            catch (Exception ex)
            {
                logger.Error("Erro em _cancelRoteadorCallback():" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Abre o canal de callbacks com o Roteador e efetua a assinatura
        /// </summary>
        /// <param name="objectimpl"></param>
        private void _assinaCallbackRoteador(IRoteadorOrdensCallback objectimpl)
        {
            try
            {
                logger.Info("Chamando ativador para instanciar o cliente do roteador...");

                gClienteRoteadorOrdens = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(objectimpl);

                if (gClienteRoteadorOrdens != null)
                {
                    logger.Info("Cliente do roteador instanciado, enviando request de assinatura...");

                    AssinarExecucaoOrdemResponse lResposta = gClienteRoteadorOrdens.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());                         // Faz a chamada pra abrir a conexão com o roteador; só serve pra enviar o contexto, e o roteador assinar a ponte duplex 

                    if (lResposta.StatusResposta == Library.MensagemResponseStatusEnum.OK)
                    {
                        logger.Info("Conexão com o roteador aberta, resposta do servidor: [" + lResposta.StatusResposta + "] [" + lResposta.DescricaoResposta + "]");                    // Abriu ok, solta o evento de mensagem
                    }
                    else
                    {
                        logger.Info("Conexão com o roteador aberta, resposta do servidor: [" + lResposta.StatusResposta + "] [" + lResposta.DescricaoResposta + "]"); // Erro na abertura de conexão; TODO: verificar protocolo de erro nesse caso

                        gClienteRoteadorOrdens = null;                                                                                   // Setando como null pra tentar novamente depois, ver conforme o protocolo o que fazer
                    }

                    // Assina os status de conexao a bolsa para manter o canal aberto.
                    AssinarStatusConexaoBolsaResponse resp = gClienteRoteadorOrdens.AssinarStatusConexaoBolsa(new AssinarStatusConexaoBolsaRequest());
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em _assinaCallbackRoteador():" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        public OrdemInfo _cloneOrdemInfo( OrdemInfo ordem ) 
        {
            OrdemInfo clone = new OrdemInfo();
            clone.Acompanhamentos = new List<AcompanhamentoOrdemInfo>();

            clone.Account            = ordem.Account;
            clone.ChannelID          = ordem.ChannelID;
            clone.ClOrdID            = ordem.ClOrdID;
            clone.CumQty             = ordem.CumQty;
            clone.Exchange           = ordem.Exchange;
            clone.ExchangeNumberID   = ordem.ExchangeNumberID;
            clone.ExecBroker         = ordem.ExecBroker;
            clone.ExpireDate         = ordem.ExpireDate;
            clone.IdOrdem            = ordem.IdOrdem;
            clone.MaxFloor           = ordem.MaxFloor;
            clone.MinQty             = ordem.MinQty;
            clone.OrderQty           = ordem.OrderQty;
            clone.OrderQtyRemmaining = ordem.OrderQtyRemmaining;
            clone.OrdStatus          = ordem.OrdStatus;
            clone.OrdType            = ordem.OrdType;
            clone.OrigClOrdID        = ordem.OrigClOrdID;
            clone.Price              = ordem.Price;
            clone.RegisterTime       = ordem.RegisterTime;
            clone.SecurityExchangeID = ordem.SecurityExchangeID;
            clone.SecurityID         = ordem.SecurityID;
            clone.SecurityIDSource   = ordem.SecurityIDSource;
            clone.Side               = ordem.Side;
            clone.StopPrice          = ordem.StopPrice;
            clone.StopStartID        = ordem.StopStartID;
            clone.Symbol             = ordem.Symbol;
            clone.TimeInForce        = ordem.TimeInForce;
            clone.TransactTime       = ordem.TransactTime;
            clone.FixMsgSeqNum       = ordem.FixMsgSeqNum;
            clone.CompIDOMS          = ordem.CompIDOMS;

            logger.InfoFormat("Tipo de Ordem no _cloneOrdemInfo - [{0}]", clone.OrdType.ToString());
            
            if (ordem.Acompanhamentos != null && ordem.Acompanhamentos.Count > 0)
                clone.Acompanhamentos = ordem.Acompanhamentos.GetRange(0, ordem.Acompanhamentos.Count);

            return clone;
        }

        public AcompanhamentoOrdemInfo _cloneAcompanhamentoInfo(AcompanhamentoOrdemInfo orig)
        {
            AcompanhamentoOrdemInfo clone = new AcompanhamentoOrdemInfo();

            clone.CanalNegociacao = orig.CanalNegociacao;
            clone.CodigoDoCliente = orig.CodigoDoCliente;
            clone.CodigoRejeicao = orig.CodigoRejeicao;
            clone.CodigoResposta = orig.CodigoResposta;
            clone.CodigoTransacao = orig.CodigoTransacao;
            clone.CompIDBolsa = orig.CompIDBolsa;
            clone.CompIDOMS = orig.CompIDOMS;
            clone.DataAtualizacao = orig.DataAtualizacao;
            clone.DataOrdemEnvio = orig.DataOrdemEnvio;
            clone.DataValidade = orig.DataValidade;
            clone.Descricao = orig.Descricao;
            clone.Direcao = orig.Direcao;
            clone.Emolumentos = orig.Emolumentos;
            clone.ExchangeExecID = orig.ExchangeExecID;
            clone.ExchangeOrderID = orig.ExchangeOrderID;
            clone.ExchangeQuoteID = orig.ExchangeQuoteID;
            clone.ExchangeSecondaryOrderID = orig.ExchangeSecondaryOrderID;
            clone.Instrumento = orig.Instrumento;
            clone.LastPx = orig.LastPx;
            clone.LastPxInIssuedCurrency = orig.LastPxInIssuedCurrency;
            clone.NumeroControleOrdem = orig.NumeroControleOrdem;
            clone.OrderLinkID = orig.OrderLinkID;
            clone.PossDupFlag = orig.PossDupFlag;
            clone.PossResend = orig.PossResend;
            clone.Preco = orig.Preco;
            clone.PriceInIssuedCurrency = orig.PriceInIssuedCurrency;
            clone.QuantidadeExecutada = orig.QuantidadeExecutada;
            clone.QuantidadeNegociada = orig.QuantidadeNegociada;
            clone.QuantidadeRemanescente = orig.QuantidadeRemanescente;
            clone.QuantidadeSolicitada = orig.QuantidadeSolicitada;
            clone.SecurityID = orig.SecurityID;
            clone.StatusOrdem = orig.StatusOrdem;
            clone.TradeDate = orig.TradeDate;
            clone.TradeLinkID = orig.TradeLinkID;
            clone.UniqueTradeID = orig.UniqueTradeID;
            
            return clone;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Gradual.OMS.Alertas.Lib;
using Gradual.OMS.Alertas.Lib.Dados;
using Gradual.OMS.Alertas.Lib.Mensagens;
using Gradual.OMS.Library.Servicos;
using log4net;
using Newtonsoft.Json;

namespace Gradual.OMS.Alertas
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoAlertasUMDF : IServicoControlavel, IServicoAlertas
    {
        private ServicoStatus _status = ServicoStatus.Parado;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SocketPackage[] umdfSockets;
        private GerenciadorAlertas gerenciadorAlertas;
        private GdbAlertas dbAlertas;
        private Dictionary<String, DadosInstrumento> instrumentosMonitorados;
        private bool bKeepRunning = true;
        private int TimeoutMDS;
        private Thread thrMonitorConexao = null;
        private bool pingPending = false;
        private SocketPackage serverAlertas;

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            logger.Debug("IniciarServico");

            // Inicializa Map de Instrumentos Monitorados
            instrumentosMonitorados = new Dictionary<string, DadosInstrumento>();

            // Inicializa Gerenciador de Alertas Cadastrados
            gerenciadorAlertas = new GerenciadorAlertas();

            // Inicializa base de dados
            dbAlertas = new GdbAlertas();

            UMDFConfig umdfconfig = Gradual.OMS.Library.GerenciadorConfig.ReceberConfig<UMDFConfig>();

            if (umdfconfig != null && umdfconfig.Portas.Count > 0)
            {
                umdfSockets = new SocketPackage[umdfconfig.Portas.Count];

                int i = 0;
                foreach (int porta in umdfconfig.Portas)
                {
                    // Inicializa conexão com MDS
                    SocketPackage mdsSocket = new SocketPackage();
                    mdsSocket.OnConnectionOpened += new ConnectionOpenedHandler(Mds_OnConnectionOpened);
                    mdsSocket.OnRequestReceived += new MessageReceivedHandler(Mds_OnRequestReceived);

                    mdsSocket.IpAddr = ConfigurationManager.AppSettings["AlertasMDSIp"].ToString();
                    mdsSocket.Port = porta.ToString();

                    mdsSocket.OpenConnection();

                    umdfSockets[i] = mdsSocket;

                    i++;
                }
            }
            // Configura timeout de conexão com MDS e ativa thread de monitoração
            TimeoutMDS = 300;
            if (ConfigurationManager.AppSettings["TimeoutMDS"] != null)
            {
                TimeoutMDS = Convert.ToInt32(ConfigurationManager.AppSettings["TimeoutMDS"].ToString());
            }
            TimeoutMDS *= 1000;
            thrMonitorConexao = new Thread(new ThreadStart(MonitorConexaoMDS));
            thrMonitorConexao.Start();

            // Inicializa server Alertas
            serverAlertas = new SocketPackage();
            serverAlertas.OnClientConnected += new ClientConnectedHandler(serverAlertas_OnClientConnected);
            serverAlertas.OnRequestReceived += new MessageReceivedHandler(serverAlertas_OnRequestReceived);

            int portaServer = 55555;
            if (ConfigurationManager.AppSettings["AlertasServerPort"] != null)
            {
                portaServer = Convert.ToInt32(ConfigurationManager.AppSettings["AlertasServerPort"].ToString());
            }
            serverAlertas.StartListen(portaServer);

            _status = ServicoStatus.EmExecucao;
        }

        void Mds_OnConnectionOpened(object sender, ConnectionOpenedEventArgs args)
        {
            try
            {
                SocketPackage mdsSocket = sender as SocketPackage;
           
                // Obtem alertas cadastrados na base
                Dictionary<String, DadosAlerta> alertasCadastrados = dbAlertas.ListarAlertas();

                if (alertasCadastrados != null && alertasCadastrados.Count > 0)
                {
                    gerenciadorAlertas.Carregar(alertasCadastrados);
                    RecadastrarInstrumentosMDS(mdsSocket);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Mds_OnConnectionOpened(): " + ex.Message, ex);
            }
        }

        void Mds_OnRequestReceived(object sender, MessageEventArgs args)
        {
            String codigoMensagem = args.TipoMsg;
            SocketPackage mdsSocket = sender as SocketPackage;
            if (codigoMensagem.Equals("AR"))
            {
                List<DadosAlerta> alertasAtingidos = gerenciadorAlertas.Checar(args.Message, instrumentosMonitorados);
                if (alertasAtingidos.Count > 0)
                {
                    // Serializa alertas atingidos e envia para AlertasCliente
                    string alertasAtingidosSerializados = JsonConvert.SerializeObject(alertasAtingidos);

                    StringBuilder retornoAtingidos = new StringBuilder();
                    retornoAtingidos.Append("AT");
                    retornoAtingidos.Append(alertasAtingidosSerializados);

                    logger.Debug("Server - Enviando alertas atingidos: [" + retornoAtingidos.ToString() + "]");
                    serverAlertas.SendToAll(retornoAtingidos.ToString());

                    // Atualiza status de alertas na base de dados
                    foreach (DadosAlerta detalheAlertaAtingido in alertasAtingidos)
                    {
                        dbAlertas.AtualizarAlertaAtingido(
                            detalheAlertaAtingido.IdAlerta,
                            detalheAlertaAtingido.DataAtingimento,
                            detalheAlertaAtingido.Cotacao);

                        logger.Debug("Alerta atingido: Id=[" + detalheAlertaAtingido.IdAlerta +
                            "] DataAtingimento=[" + detalheAlertaAtingido.DataAtingimento +
                            "] Valor=[" + detalheAlertaAtingido.Cotacao + "]");
                    }

                }
            }
            else if (codigoMensagem.Equals("PG"))
            {
                pingPending = false;
                logger.Debug("Recebida mensagem PG=[" + args.Message + "] " + mdsSocket.IpAddr + ":" + mdsSocket.Port);
            }
            else
            {
                logger.Error("Recebida mensagem com código inválido: [" + args.Message + "]");
            }
        }

        public void PararServico()
        {
            logger.Info("Recebida requisição de parada");

            bKeepRunning = false;

            _status = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        private void RecadastrarInstrumentosMDS( SocketPackage mdsSocket )
        {
            DateTime dataHoraCadastro = DateTime.Now;

            HashSet<String> instrumentosCadastrados = gerenciadorAlertas.ObterInstrumentos();

            foreach (String instrumento in instrumentosCadastrados)
            {
                if (!instrumentosMonitorados.Keys.Contains(instrumento))
                {
                    DadosInstrumento dadosInstrumento = new DadosInstrumento();
                    dadosInstrumento.minimo = Decimal.MaxValue;
                    dadosInstrumento.maximo = Decimal.MinValue;
                    instrumentosMonitorados.Add(instrumento, dadosInstrumento);

                    StringBuilder requisicaoAlerta = new StringBuilder();

                    requisicaoAlerta.Append("AL");
                    requisicaoAlerta.Append("  ");
                    requisicaoAlerta.Append(dataHoraCadastro.ToString(string.Format("{0}{1}", "yyyyMMddHHmmssmmm", "0")));
                    string instrumentoFormatado = String.Format("{0,-20}", instrumento);
                    requisicaoAlerta.Append(instrumentoFormatado);
                    requisicaoAlerta.Append("1");

                    try
                    {
                        if ( mdsSocket!=null && mdsSocket.IsConectado() )
                            mdsSocket.Send(requisicaoAlerta.ToString());
                    }
                    catch (Exception ex)
                    {
                        logger.Error("RecadastrarInstrumentosMDS():" + ex.Message, ex);
                    }
                }
            }
        }

        private void MonitorConexaoMDS()
        {
            TimeSpan iTrialInterval;
            DateTime lastPing = DateTime.Now;

            logger.Info("Iniciando thread de monitoracao de conexao com MDS");

            while (bKeepRunning)
            {
                // ajusta Timeout, caso esteja em horário sem movimento
                if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 21)
                    iTrialInterval = (new TimeSpan(0, 0, 0, 0, TimeoutMDS));
                else
                    iTrialInterval = (new TimeSpan(0, 0, 0, 0, TimeoutMDS * 5));

                foreach (SocketPackage mdsSocket in umdfSockets)
                {
                    if (!mdsSocket.IsConectado())
                    {
                        gerenciadorAlertas.Limpar();
                        instrumentosMonitorados.Clear();
                        logger.Info("Reabrindo conexao com MDS...");
                        mdsSocket.OpenConnection();
                    }
                    else
                    {
                        // Verifica ultima comunicacao com MDS
                        TimeSpan tslastping = DateTime.Now.Subtract(lastPing);
                        if (tslastping.CompareTo(new TimeSpan(0, 0, 0, 0, 30000)) > 0)
                        {
                            SendPing();
                            lastPing = DateTime.Now;
                        }

                        // Verifica ultima comunicacao com MDS
                        TimeSpan tslastpkt = DateTime.Now.Subtract(mdsSocket.LastPacket);
                        if (tslastpkt.CompareTo(iTrialInterval) > 0)
                        {
                            logger.Warn("Finalizando conexao com MDS por timeout!!!");
                            mdsSocket.CloseSocket();
                        }
                    }
                }
                Thread.Sleep(250);
            }

            logger.Info("Thread de monitoracao de conexao com MDS finalizacao");
        }

        private void SendPing()
        {
            StringBuilder requisicaoAlerta = new StringBuilder();

            requisicaoAlerta.Append("PG");
            requisicaoAlerta.Append("  ");
            requisicaoAlerta.Append(DateTime.Now.ToString(string.Format("{0}{1}", "yyyyMMddHHmmssmmm", "0")));
            string instrumento = String.Format("{0,-20}", "PING");
            requisicaoAlerta.Append(instrumento);

            foreach (SocketPackage mdsSocket in umdfSockets)
            {
                try
                {
                    if (mdsSocket != null && mdsSocket.IsConectado())
                    {
                        logger.DebugFormat("Enviando ping para [{0}:{1}]", mdsSocket.IpAddr, mdsSocket.Port);
                        mdsSocket.Send(requisicaoAlerta.ToString());
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("SendPing(): " + ex.Message, ex);
                }
            }

            pingPending = true;
        }

        void serverAlertas_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            // gera snapshot dos alertas cadastrados
            string snapshotAlertas = gerenciadorAlertas.GerarSnapshot();
            StringBuilder retornoSnapshot = new StringBuilder();
            retornoSnapshot.Append("SS");
            retornoSnapshot.Append(snapshotAlertas);

            logger.Debug("Server - Enviando Snapshot [" + retornoSnapshot.ToString() + "]");
            serverAlertas.SendData(retornoSnapshot.ToString(), args.ClientSocket);
        }

        void serverAlertas_OnRequestReceived(object sender, MessageEventArgs args)
        {
            string codigoMensagem = args.TipoMsg;
            if (codigoMensagem.Equals("PG"))
            {
                logger.Debug("Server - Recebida mensagem PG=[" + args.Message + "]");
                StringBuilder respostaPing = new StringBuilder();
                respostaPing.Append("PG");
                respostaPing.Append("  ");
                respostaPing.Append(DateTime.Now.ToString(string.Format("{0}{1}", "yyyyMMddHHmmssmmm", "0")));
                string instrumento = String.Format("{0,-20}", "PING");
                respostaPing.Append(instrumento);

                logger.Debug("Server - respondendo mensagem PG=[" + respostaPing.ToString() + "]");
                serverAlertas.SendToAll(respostaPing.ToString());
            }
            else
                logger.Error("Server - Recebida mensagem deesconhecida=[" + args.Message + "]");
        }

        #endregion

        #region IServicoAlertas Members

        public CadastrarAlertaResponse CadastrarAlerta(CadastrarAlertaRequest request)
        {
            CadastrarAlertaResponse responseAlerta = new CadastrarAlertaResponse();

            try
            {
                logger.Debug("Executando Cadastrar Alerta - IdCliente=[" + request.Alerta.IdCliente + "] " +
                    "Instrumento=[" + request.Alerta.Instrumento + "] " +
                    "TipoOperador=[" + (int)request.Alerta.TipoOperador + "] " +
                    "TipoOperando=[" + (int)request.Alerta.TipoOperando + "] " +
                    "Valor=[" + request.Alerta.Valor + "] "
                    );

                DateTime dataHoraCadastro = DateTime.Now;

                if (!instrumentosMonitorados.Keys.Contains(request.Alerta.Instrumento))
                {
                    DadosInstrumento dadosInstrumento = new DadosInstrumento();
                    dadosInstrumento.minimo = Decimal.MaxValue;
                    dadosInstrumento.maximo = Decimal.MinValue;
                    instrumentosMonitorados.Add(request.Alerta.Instrumento, dadosInstrumento);

                    StringBuilder requisicaoAlerta = new StringBuilder();

                    requisicaoAlerta.Append("AL");
                    requisicaoAlerta.Append("  ");
                    requisicaoAlerta.Append(dataHoraCadastro.ToString(string.Format("{0}{1}", "yyyyMMddHHmmssmmm", "0")));
                    string instrumento = String.Format("{0,-20}", request.Alerta.Instrumento);
                    requisicaoAlerta.Append(instrumento);
                    requisicaoAlerta.Append("1");

                    foreach (SocketPackage mdsSocket in umdfSockets)
                    {
                        try
                        {
                            if ( mdsSocket.IsConectado() )
                                mdsSocket.Send(requisicaoAlerta.ToString());
                        }
                        catch (Exception ex)
                        {
                            logger.Error("CadastrarAlerta(): " + ex.Message, ex);
                        }
                    }
                }

                String idCadastrado = dbAlertas.CadastrarAlerta(
                    request.Alerta.IdCliente,
                    request.Alerta.Instrumento,
                    request.Alerta.TipoOperando,
                    request.Alerta.TipoOperador,
                    request.Alerta.Valor,
                    dataHoraCadastro
                    );

                if (idCadastrado == null)
                {
                    logger.Error("CadastrarAlerta: Erro na inserção na base.");
                    responseAlerta.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
                    responseAlerta.DescricaoErro = "Ocorreu erro na inserção das informações na base de dados. Alerta não foi cadastrado";
                }
                else
                {
                    logger.Debug("Cadastrar Alerta - Atribuído IdAlerta=[" + idCadastrado + "]");

                    responseAlerta.IdAlerta = idCadastrado;
                    responseAlerta.StatusResposta = Library.MensagemResponseStatusEnum.OK;

                    DadosAlerta dados = gerenciadorAlertas.Cadastrar(
                        idCadastrado,
                        request.Alerta.IdCliente,
                        request.Alerta.Instrumento,
                        request.Alerta.TipoOperando,
                        request.Alerta.TipoOperador,
                        request.Alerta.Valor,
                        dataHoraCadastro);

                    // Serializa alerta cadastrado e envia para AlertasCliente
                    string alertaCadastradoSerializado = JsonConvert.SerializeObject(dados);

                    StringBuilder retornoCadastrado = new StringBuilder();
                    retornoCadastrado.Append("CA");
                    retornoCadastrado.Append(alertaCadastradoSerializado);

                    logger.Debug("Server - Enviando alertas cadastrado: [" + retornoCadastrado.ToString() + "]");
                    serverAlertas.SendToAll(retornoCadastrado.ToString());

                }
            }
            catch (Exception e)
            {
                logger.Debug("CadastrarAlerta: Exception [" + e.Message + "]");
                logger.Debug(e.StackTrace);
                responseAlerta.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
                responseAlerta.IdAlerta = null;
            }

            return responseAlerta;
        }

        public Lib.Mensagens.ExcluirAlertaResponse ExcluirAlerta(Lib.Mensagens.ExcluirAlertaRequest request)
        {
            logger.Debug("Excluir Alerta: IdCliente=[" + request.IdCliente + "] IdAlerta=[" + request.IdAlerta + "]");

            ExcluirAlertaResponse response = new ExcluirAlertaResponse();

            dbAlertas.ExcluirAlerta(request.IdAlerta);

            logger.Debug("IdAlerta=[" + request.IdAlerta + "] removido da base.");

            DadosAlerta alertaExcluido = gerenciadorAlertas.Excluir(request.IdAlerta);
            if (alertaExcluido != null)
            {
                // Serializa alerta exlcuido e envia para AlertasCliente
                string alertaExcluidoSerializado = JsonConvert.SerializeObject(alertaExcluido);

                StringBuilder retornoExcluido = new StringBuilder();
                retornoExcluido.Append("EX");
                retornoExcluido.Append(alertaExcluidoSerializado);

                logger.Debug("Server - Enviando alerta excluído: [" + retornoExcluido.ToString() + "]");
                serverAlertas.SendToAll(retornoExcluido.ToString());
            }

            logger.Debug("IdAlerta=[" + request.IdAlerta + "] removido da memória.");

            response.StatusResposta = Library.MensagemResponseStatusEnum.OK;

            return response;
        }

        public Lib.Mensagens.ListarAlertasResponse ListarAlertas(Lib.Mensagens.ListarAlertasRequest request)
        {
            logger.Debug("ListarAlertas: IdCliente=[" + request.IdCliente + "]");

            ListarAlertasResponse response = new ListarAlertasResponse();

            List<AlertaInfo> listaAlertasResponse = new List<AlertaInfo>();

            Dictionary<String, DadosAlerta> listaAlertas =
                gerenciadorAlertas.VerificarAlertas(request.IdCliente);

            foreach (KeyValuePair<string, DadosAlerta> kvAlerta in listaAlertas)
            {
                if (kvAlerta.Value.IdCliente.Equals(request.IdCliente))
                {
                    AlertaInfo respAlertaInfo = new AlertaInfo();
                    respAlertaInfo.IdAlerta = kvAlerta.Key;
                    respAlertaInfo.IdCliente = kvAlerta.Value.IdCliente;
                    respAlertaInfo.Instrumento = kvAlerta.Value.Instrumento;
                    respAlertaInfo.TipoOperador = kvAlerta.Value.TipoOperador;
                    respAlertaInfo.TipoOperando = kvAlerta.Value.TipoOperando;
                    respAlertaInfo.Valor = kvAlerta.Value.Valor;
                    respAlertaInfo.Atingido = kvAlerta.Value.Atingido;
                    respAlertaInfo.Exibido = kvAlerta.Value.Exibido;
                    respAlertaInfo.DataCadastro = kvAlerta.Value.DataCadastro;
                    respAlertaInfo.DataAtingimento = kvAlerta.Value.DataAtingimento;

                    listaAlertasResponse.Add(respAlertaInfo);
                }
            }

            response.Alertas = listaAlertasResponse;

            response.StatusResposta = Library.MensagemResponseStatusEnum.OK;

            return response;
        }

        public Lib.Mensagens.VerificarAlertasResponse VerificarAlertas(
            Lib.Mensagens.VerificarAlertasRequest request)
        {
            logger.Debug("VerificarAlertas iniciado");

            VerificarAlertasResponse response = new VerificarAlertasResponse();

            List<AlertaInfo> listaAlertasResponse = new List<AlertaInfo>();

            Dictionary<String, DadosAlerta> listaAlertas =
                gerenciadorAlertas.VerificarAlertas(request.IdCliente);

            foreach (KeyValuePair<string, DadosAlerta> kvAlerta in listaAlertas)
            {
                if (kvAlerta.Value.IdCliente.Equals(request.IdCliente) &&
                    kvAlerta.Value.Atingido == true &&
                    kvAlerta.Value.Exibido == false)
                {
                    AlertaInfo respAlertaInfo = new AlertaInfo();
                    respAlertaInfo.IdAlerta = kvAlerta.Key;
                    respAlertaInfo.IdCliente = kvAlerta.Value.IdCliente;
                    respAlertaInfo.Instrumento = kvAlerta.Value.Instrumento;
                    respAlertaInfo.TipoOperador = kvAlerta.Value.TipoOperador;
                    respAlertaInfo.TipoOperando = kvAlerta.Value.TipoOperando;
                    respAlertaInfo.Valor = kvAlerta.Value.Valor;
                    respAlertaInfo.Atingido = kvAlerta.Value.Atingido;
                    respAlertaInfo.Exibido = kvAlerta.Value.Exibido;
                    respAlertaInfo.DataCadastro = kvAlerta.Value.DataCadastro;
                    respAlertaInfo.DataAtingimento = kvAlerta.Value.DataAtingimento;
                    respAlertaInfo.Cotacao = kvAlerta.Value.Cotacao;

                    listaAlertasResponse.Add(respAlertaInfo);
                }
            }

            response.Alertas = listaAlertasResponse;

            return response;
        }

        public Lib.Mensagens.MarcarComoExibidoResponse MarcarComoExibido(
            Lib.Mensagens.MarcarComoExibidoRequest request)
        {
            logger.Debug("MarcarComoExibido: IdCliente = [" + request.IdCliente + "]");

            List<DadosAlerta> listaMarcados = new List<DadosAlerta>();

            foreach (String idAlertaParametro in request.listaIdAlerta)
            {
                DadosAlerta alertaMarcado = gerenciadorAlertas.MarcarComoExibido(idAlertaParametro);
                dbAlertas.AtualizarAlertaExibido(idAlertaParametro);

                if (alertaMarcado != null)
                    listaMarcados.Add(alertaMarcado);
            }

            // Serializa alerta exlcuido e envia para AlertasCliente
            string listaMarcadosSerializado = JsonConvert.SerializeObject(listaMarcados);
            StringBuilder retornoMarcarExibido = new StringBuilder();
            retornoMarcarExibido.Append("AT");
            retornoMarcarExibido.Append(listaMarcadosSerializado);
            logger.Debug("Server - Enviando alertas marcados como exibido: [" + retornoMarcarExibido.ToString() + "]");
            serverAlertas.SendToAll(retornoMarcarExibido.ToString());

            // Monta response
            MarcarComoExibidoResponse response = new MarcarComoExibidoResponse();
            response.StatusResposta = Library.MensagemResponseStatusEnum.OK;

            return response;
        }

        #endregion
    }

}

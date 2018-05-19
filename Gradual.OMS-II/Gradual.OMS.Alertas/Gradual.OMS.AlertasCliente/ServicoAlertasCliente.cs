using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Alertas.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Alertas.Lib.Mensagens;
using log4net;
using Gradual.OMS.Alertas.Lib.Dados;
using Gradual.OMS.Alertas;
using System.Threading;
using System.Configuration;
using Newtonsoft.Json;
using System.ServiceModel;

namespace Gradual.OMS.AlertasCliente
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoAlertasCliente : IServicoControlavel, IServicoAlertas
    {
        public string urlalertas = null;
        private ServicoStatus _status = ServicoStatus.Parado;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MemoriaAlertas gerenciadorAlertas;
        private SocketPackage socket;
        public bool bKeepRunning = false;
        private int  TimeoutSocket;
        Thread thrMonitorConexao;
        private bool pingPending = false;

        public void IniciarServico()
        {
            logger.Debug("IniciarServico");


            if (ConfigurationManager.AppSettings["EndPointServicoAlertas"] != null)
            {
                urlalertas = ConfigurationManager.AppSettings["EndPointServicoAlertas"].ToString();
            }
            else
            {
                logger.Fatal("'EndPointServicoAlertas' nao foi definido. Finalizando!!!!!");
                return;
            }

            // Inicializa Gerenciador de Alertas Cadastrados
            gerenciadorAlertas = new MemoriaAlertas();

            // Inicializa conexão com MDS
            socket = new SocketPackage();
            socket.OnRequestReceived += new MessageReceivedHandler(socket_OnRequestReceived);
            socket.IpAddr = ConfigurationManager.AppSettings["AlertasMDSIp"].ToString();
            socket.Port = ConfigurationManager.AppSettings["AlertasMDSPort"].ToString();
            socket.OpenConnection();


            // Configura timeout de conexão com MDS e ativa thread de monitoração
            TimeoutSocket = 60;
            if (ConfigurationManager.AppSettings["TimeoutSocket"] != null)
            {
                TimeoutSocket = Convert.ToInt32(ConfigurationManager.AppSettings["TimeoutSocket"].ToString());
            }
            TimeoutSocket *= 1000;

            bKeepRunning = true;
            thrMonitorConexao = new Thread(new ThreadStart(MonitorConexaoServicoAlertas));
            thrMonitorConexao.Start();

            _status = ServicoStatus.EmExecucao;
        }

        void socket_OnRequestReceived(object sender, MessageEventArgs args)
        {
            logger.Debug("Tipo MSG [" + args.TipoMsg + "]");
            logger.Debug("MSG [" + args.Message + "]");

            String codigoMensagem = args.TipoMsg;
            String mensagem = args.Message.Substring(2);
            switch (codigoMensagem)
            {
                case "SS":
                    _carregarSnapshot(mensagem);
                    break;
                case "AT":
                    _atualizarAlerta(mensagem);
                    break;
                case "CA":
                    _cadastrarAlerta(mensagem);
                    break;
                case "EX":
                    _cancelarAlerta(mensagem);
                    //TODO: tratar exclusao
                    break;
                case "PG":
                    pingPending = false;
                    logger.Debug("Recebida mensagem PG=[" + mensagem + "]");
                    break;
                default:
                    logger.Error("Recebida mensagem com código inválido: [" + mensagem + "]");
                    break;
            }
        }

        private void _atualizarAlerta(string mensagem)
        {
            try
            {
                List<DadosAlerta> lalerta;

                lalerta = JsonConvert.DeserializeObject<List<DadosAlerta>>(mensagem);

                foreach (DadosAlerta alerta in lalerta)
                {
                    gerenciadorAlertas.AtualizarAlerta(alerta.IdAlerta, alerta);
                }
            }
            catch (Exception ex)
            {
                logger.Error("_atualizarAlerta(): " + ex.Message, ex);
            }
        }

        private void _cancelarAlerta(string mensagem)
        {
            try
            {
                DadosAlerta alerta;

                alerta = JsonConvert.DeserializeObject<DadosAlerta>(mensagem);

                logger.Debug("Excluir alerta [" + alerta.IdAlerta + "] do cliente [" + alerta.IdCliente + "]");

                gerenciadorAlertas.Excluir(alerta.IdAlerta);
            }
            catch (Exception ex)
            {
                logger.Error("_atualizarAlerta(): " + ex.Message, ex);
            }
        }

        private void _cadastrarAlerta(string mensagem)
        {
            try
            {
                DadosAlerta alerta;

                alerta = JsonConvert.DeserializeObject<DadosAlerta>(mensagem);

                gerenciadorAlertas.AtualizarAlerta(alerta.IdAlerta, alerta);
            }
            catch (Exception ex)
            {
                logger.Error("_cadastrarAlerta(): " + ex.Message, ex);
            }
        }

        private void _carregarSnapshot(string message)
        {
            try
            {
                Dictionary<string, DadosAlerta> todosalertas;

                todosalertas = JsonConvert.DeserializeObject<Dictionary<string, DadosAlerta>>(message);

                logger.Info("Snapshot com " + todosalertas.Count + " items");

                foreach (KeyValuePair<string, DadosAlerta> alerta in todosalertas)
                {
                    logger.Debug("Atualizando/Incluindo alerta [" + alerta.Key + "] cliente [" + alerta.Value.IdCliente + "]");
                    gerenciadorAlertas.AtualizarAlerta(alerta.Key, alerta.Value);
                }
            }
            catch (Exception ex)
            {
                logger.Error("_carregarSnapshot() " + ex.Message, ex);
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

        private void MonitorConexaoServicoAlertas()
        {
            TimeSpan iTrialInterval;

            logger.Info("Iniciando thread de monitoracao de conexao com ServicodeAlertas");
            Thread.Sleep(TimeoutSocket);

            while (bKeepRunning)
            {
                // ajusta Timeout, caso esteja em horário sem movimento
                if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 21)
                    iTrialInterval = (new TimeSpan(0, 0, 0, 0, TimeoutSocket));
                else
                    iTrialInterval = (new TimeSpan(0, 0, 0, 0, TimeoutSocket * 5));

                if (!socket.IsConectado())
                {
                    gerenciadorAlertas.Limpar();
                    logger.Info("Reabrindo conexao com MDS...");
                    socket.OpenConnection();
                }
                else
                {
                    // Verifica ultima comunicacao com MDS
                    TimeSpan tslastpkt = DateTime.Now.Subtract(socket.LastPacket);
                    if (tslastpkt.CompareTo(iTrialInterval) > 0)
                    {
                        if (!pingPending)
                        {
                            SendPing();
                        }
                        else
                        {
                            logger.Warn("Finalizando conexao com MDS por timeout!!!");
                            socket.CloseSocket();
                        }
                    }
                }
                Thread.Sleep(iTrialInterval);
            }

            logger.Info("Thread de monitoracao de conexao com ServicoAlertas finalizacao");
        }

        private void SendPing()
        {
            StringBuilder requisicaoAlerta = new StringBuilder();

            requisicaoAlerta.Append("PG");
            requisicaoAlerta.Append("  ");
            requisicaoAlerta.Append(DateTime.Now.ToString(string.Format("{0}{1}", "yyyyMMddHHmmssmmm", "0")));
            string instrumento = String.Format("{0,-20}", "PING");
            requisicaoAlerta.Append(instrumento);

            socket.Send(requisicaoAlerta.ToString());

            pingPending = true;
        }


        #region IServicoAlertas Members
        public Alertas.Lib.Mensagens.CadastrarAlertaResponse CadastrarAlerta(Alertas.Lib.Mensagens.CadastrarAlertaRequest request)
        {

            logger.Debug("Executando Cadastrar Alerta - IdCliente=[" + request.Alerta.IdCliente + "] " +
                "Instrumento=[" + request.Alerta.Instrumento + "] " +
                "TipoOperador=[" + (int)request.Alerta.TipoOperador + "] " +
                "TipoOperando=[" + (int)request.Alerta.TipoOperando + "] " +
                "Valor=[" + request.Alerta.Valor + "] "
                );

            CadastrarAlertaResponse resp = new CadastrarAlertaResponse();

            try
            {
                IServicoAlertas servico = Ativador.GetByAddr<IServicoAlertas>(urlalertas);

                resp = servico.CadastrarAlerta(request);

                logger.Debug("Executando Cadastrar Alerta - IdCliente=[" + request.Alerta.IdCliente + "] alerta cadastrado");
            }
            catch (Exception ex)
            {
                logger.Error( "Erro CadastrarAlerta(): " + ex.Message, ex);

                resp.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
                resp.DescricaoErro = ex.Message;
            }

            return resp;
        }

        public Alertas.Lib.Mensagens.VerificarAlertasResponse VerificarAlertas(Alertas.Lib.Mensagens.VerificarAlertasRequest request)
        {
            VerificarAlertasResponse response = new VerificarAlertasResponse();

            if (ConfigurationManager.AppSettings["DebugVerificacaoAlertas"] != null &&
                ConfigurationManager.AppSettings["DebugVerificacaoAlertas"].ToString().Equals("true") )
            {
                logger.Debug("VerificarAlertas: IdCliente=[" + request.IdCliente + "]");
            }

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

            if (ConfigurationManager.AppSettings["DebugVerificacaoAlertas"] != null &&
                ConfigurationManager.AppSettings["DebugVerificacaoAlertas"].ToString().Equals("true"))
            {
                logger.Debug("VerificarAlertas: IdCliente=[" + request.IdCliente + "] " + listaAlertasResponse.Count + " disparados");
            }

            response.Alertas = listaAlertasResponse;

            return response;
        }

        public Alertas.Lib.Mensagens.ListarAlertasResponse ListarAlertas(Alertas.Lib.Mensagens.ListarAlertasRequest request)
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

            logger.Debug("ListarAlertas: IdCliente=[" + request.IdCliente + "] com " + listaAlertas.Count + " itens");

            response.Alertas = listaAlertasResponse;

            response.StatusResposta = Library.MensagemResponseStatusEnum.OK;

            return response;
        }

        public Alertas.Lib.Mensagens.ExcluirAlertaResponse ExcluirAlerta(Alertas.Lib.Mensagens.ExcluirAlertaRequest request)
        {
            logger.Debug("Excluir Alerta: IdCliente=[" + request.IdCliente + "] IdAlerta=[" + request.IdAlerta + "]");

            ExcluirAlertaResponse response = new ExcluirAlertaResponse();

            try
            {
                IServicoAlertas servico = Ativador.GetByAddr<IServicoAlertas>(urlalertas);
                response = servico.ExcluirAlerta(request);
                logger.Debug("Excluir Alerta: IdCliente=[" + request.IdCliente + "] IdAlerta=[" + request.IdAlerta + "] excluido");
            }
            catch (Exception ex)
            {
                logger.Error("Erro ExcluirAlerta(): " + ex.Message, ex);
                response.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
                response.DescricaoResposta = ex.Message;
            }

            return response;
        }

        public Alertas.Lib.Mensagens.MarcarComoExibidoResponse MarcarComoExibido(Alertas.Lib.Mensagens.MarcarComoExibidoRequest request)
        {
            logger.Debug("MarcarComoExibido: IdCliente = [" + request.IdCliente + "]");

            MarcarComoExibidoResponse response = new MarcarComoExibidoResponse();

            try
            {
                IServicoAlertas servico = Ativador.GetByAddr<IServicoAlertas>(urlalertas);

                response = servico.MarcarComoExibido(request);
            }
            catch (Exception ex)
            {
                logger.Error("Erro MarcarComoExibido(): " + ex.Message, ex);
                response.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
                response.DescricaoResposta = ex.Message;
            }

            return response;
        }
        #endregion //IServicoAlertas Members
    }
}

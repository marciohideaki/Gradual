using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;
using Gradual.OMS.Comunicacao.Automacao.Ordens;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Recebidas;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Ordens.StartStop;
using Gradual.OMS.Ordens.StartStop.Lib;
using Gradual.OMS.Ordens.StartStop.Lib.Enum;
using log4net;
using Gradual.OMS.Library.Servicos;

namespace StopStartForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        #region [Propriedades]

        /// <summary>
        /// 
        /// </summary>
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IServicoOrdemStopStart _Servico = null;

        /// <summary>
        /// Source do EventLog
        /// </summary>
        private string EventLogSource{
            get{
                return ConfigurationManager.AppSettings["EventLogSource"].ToString();
            }
        }

        /// <summary>
        /// Código do cliente
        /// </summary>
        private string IdCliente{
            get{
                return ConfigurationManager.AppSettings["IdCliente"].ToString();
            }
        }

        private string IdSistema{
            get{
                return ConfigurationManager.AppSettings["IdSistema"].ToString();
            }
        }


        #endregion

        

        private void Form1_Load(object sender, EventArgs e)
        {

            cbTipoOrdem.DataSource = AdicionarCombo();
            cbTipoOrdem.DisplayMember = "Value";
            cbTipoOrdem.ValueMember = "Key";

            Control.CheckForIllegalCrossThreadCalls = false;

            Initialize();
            EnviarMensagemAutenticacao();

            Thread.Sleep(500);    
      
        }


        private IList AdicionarCombo()
        {
            ArrayList Lista = new ArrayList();

            Lista.Add(new KeyValuePair<int, string>(1, "Stop Loss"));
            Lista.Add(new KeyValuePair<int, string>(2, "Stop Gain"));
            Lista.Add(new KeyValuePair<int, string>(3, "Stop Simultaneo"));
            Lista.Add(new KeyValuePair<int, string>(5, "Start Compra"));         

            return Lista;

        }

        #region ["Declarações"]

        //MDSEventFactory EventMds = new MDSEventFactory();
         List<string> Eventos = new List<string>();

        #endregion

        #region ["Enumeração"]

        private enum RespostaOrdem : int
        {
            Rejeitado = 0,
            Aceito    = 1,
        };

        public enum OrdemStopStatus : int
        {
            RegistradaAplicacao             = 1,
            EnviadaMDS                      = 2,
            AceitoMDS                       = 3,
            RejeitadoMDS                    = 4,
            ExecutadoMDS                    = 5,
            CancelamentoRegistradoAplicacao = 6,
            CancelamentoEnviadoMDS          = 7,
            CancelamentoAceitoExecutadoMDS  = 8,
            CancelamentoRejeitadoMDS        = 9,
        };

        #endregion

        #region ["Eventos"]

        /// <summary>
        /// Inicializa os callback's que o programa faz referencia.
        /// </summary>
        private  void Initialize()
        {
            try
            {

                
                //Event._MDSAuthenticationResponse +=
                //    new Event.MDSAuthenticationResponseEventHandler(Event__MDSAuthenticationResponse);

                //OMSEventHandlerClass omsEHC =
                //    new OMSEventHandlerClass(EventMds);

                //// Callback de execucao de ordem
                //EventMds.OnMDSStopStartEvent +=
                //    new MDSEventFactory.MDSStopStartEventHandler(EventMds_OnMDSStopStartEvent);

                //EventMds.OnMDSSRespostaSolicitacaoEvent
                //    += new MDSEventFactory.MDSSRespostaSolicitacaoEventHandler(EventMds_OnMDSSRespostaSolicitacaoEvent);

                //EventMds.OnMDSSRespostaCancelamentoEvent +=
                //    new MDSEventFactory.MDSSRespostaCancelamentoEventHandler(EventMds_OnMDSSRespostaCancelamentoEvent);

                //Registrador.AddListener(EventMds);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "Initialize: ",ex.Message));
                throw new Exception(string.Format("{0}{1}", "Initialize: ", ex.Message));
            }

        }

        void EventMds_OnMDSSRespostaCancelamentoEvent(object sender, MDSEventArgs e)
        {
            try
            {
                CR_CancelamentoStopResposta _CR_CancelamentoStopResposta
                    = (CR_CancelamentoStopResposta)(sender);

                int id_stopstart              = int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart);
                int id_status                 = int.Parse(_CR_CancelamentoStopResposta.pStrStatus);
                OrdemStopStatus ordemStopEnum = OrdemStopStatus.CancelamentoAceitoExecutadoMDS;


                if (id_status == (int)RespostaOrdem.Aceito)
                {
                    ordemStopEnum = OrdemStopStatus.CancelamentoAceitoExecutadoMDS;

                    logger.Info(string.Format("{0}{1}", "\r\n " + int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart).ToString(), " Stop Cancelado pelo MDS"));

                    txtHistorico.Text += string.Format("{0}{1}", "\r\n " + int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart).ToString(), " Stop Cancelado pelo MDS");
                }

                if (id_status == (int)RespostaOrdem.Rejeitado)
                {
                    ordemStopEnum = OrdemStopStatus.CancelamentoRejeitadoMDS;

                    logger.Info(string.Format("{0}{1}", "\r\n Cancelamento do stop " + int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart).ToString(), " foi rejeitado pelo MDS"));

                    txtHistorico.Text += string.Format("{0}{1}", "\r\n Cancelamento do stop " + int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart).ToString(), " foi rejeitado pelo MDS");
                }

                //Altera o status no banco de dados
                new AutomacaoOrdensDados().CancelaOrdemStopStart(id_stopstart, (int)ordemStopEnum);

            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "EventMds_OnMDSSRespostaCancelamentoEvent: ", ex.Message));
            }
        }


        public void EventMds_OnMDSSRespostaSolicitacaoEvent(object sender, MDSEventArgs e)
        {
            try
            {
                RS_RespostaStop _RS_RespostaStop
                    = (RS_RespostaStop)(sender);

                int id_stopstart = int.Parse(_RS_RespostaStop.pStrIdStopStart);
                int id_status = int.Parse(_RS_RespostaStop.pStrStatus);

                if (id_status == (int)RespostaOrdem.Rejeitado){ // Stop Rejeitado

                    new AutomacaoOrdensDados().AtualizaOrdemStop
                    (
                        int.Parse(_RS_RespostaStop.pStrIdStopStart),
                        (int)OrdemStopStatus.RejeitadoMDS
                    );
               
                    logger.Info(string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopStart).ToString(), " Stop rejeitado pelo MDS"));

                    txtHistorico.Text +=
                        string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopStart).ToString(), " Stop rejeitado pelo MDS");

                }
                else if (id_status == (int)RespostaOrdem.Aceito) // Stop Aceito
                {

                    // Atualiza status da ordem para aceito.
                    new AutomacaoOrdensDados().AtualizaOrdemStop
                    (
                        int.Parse(_RS_RespostaStop.pStrIdStopStart),
                        (int)OrdemStopStatus.AceitoMDS
                    );

                    logger.Info(string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopStart).ToString(), " Stop aceito pelo MDS"));

                    txtHistorico.Text +=
                        string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopStart).ToString(), " Stop aceito pelo MDS");
                }

            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "EventMds_OnMDSSRespostaAutenticacaoEvent: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "EventMds_OnMDSSRespostaAutenticacaoEvent: ", ex.Message));
            }

        }

        /// <summary>
        /// Evento que recebe uma resposta de execução de ordens do sistema MDS.
        /// </summary>
        /// <param name="sender">Objeto com os atributos da ordem </param>
        /// <param name="e"></param>
         void EventMds_OnMDSStopStartEvent(object sender, MDSEventArgs e)
        {
            try
            {
                // Adiciona uma Thread no pool responsavel por executar o processamento do Stop.
                ThreadPool.QueueUserWorkItem(new WaitCallback(

                                        delegate(object required)
                                        {
                                            ProcessarEventoExecutouMDS(
                                                (object)(sender)
                                                );
                                        }));
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "EventMds_OnMDSStopStartEvent: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "EventMds_OnMDSStopStartEvent: ", ex.Message));
            }
        }

        /// <summary>
        /// Resposta de permissão de conexão no sistema MDS.
        /// </summary>
        /// <param name="Response"> Objeto de autenticação. </param>
        /// <param name="_ClientSocket"> Socket com a conexão do cliente conectado.</param>
         void Event__MDSAuthenticationResponse(object Response, System.Net.Sockets.Socket _ClientSocket)
        {
            try
            {
                if (Response.ToString().Trim() != string.Empty)
                {
                    switch (int.Parse(Response.ToString().Trim()))
                    {
                        case 0:                            
                            txtHistorico.Text += "Usuário Logado no sistema ";
                            break;
                        case 1:                    
                            txtHistorico.Text += "Cliente autenticado com sucesso ";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "Event__MDSAuthenticationResponse: ", ex.Message));
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region ["Funções"]

        /// <summary>
        /// Callback de respostas de mensagem de ordem Stop.
        /// </summary>
        /// <param name="sender"> Objeto de resposta.</param>
         void ProcessarEventoExecutouMDS(object sender)
         {
            try
            {
                
                SS_StopStartResposta _SS_StopSimplesResposta =
                    (SS_StopStartResposta)(sender);


                // Atualiza a ordem stop para o status executada , bem como atualiza seu preço de referencia com o valor disparado pelo MDS
                new AutomacaoOrdensDados().AtualizaOrdemStop
                    (
                        int.Parse(_SS_StopSimplesResposta.pStrIdStopStart),
                        (int)OrdemStopStatus.ExecutadoMDS,
                        decimal.Parse(_SS_StopSimplesResposta.pStrPrecoReferencia.ToString())
                    );

                logger.Info(string.Format("{0}{1}", "\r\n " + int.Parse(_SS_StopSimplesResposta.pStrIdStopStart).ToString(), " Stop Executado."));

                //IServicoOrdensServidorSemCallback servicoOrdens = Ativador.Get<IServicoOrdensServidorSemCallback>();

                //ExecutarOrdemResponse resposta =
                //    (ExecutarOrdemResponse)
                //        servicoOrdens.ProcessarMensagem(
                //            new ExecutarOrdemRequest()
                //            {
                //                //CodigoBolsa = _SS_StopSimplesResposta.pStrTipoBolsa,
                //                //Account = "2222" ,
                //                //Symbol = _SS_StopSimplesResposta.pStrInstrument,
                //                //OrderQty = _SS_StopSimplesResposta.,
                //                //Price = 10
                //            });


                // Caso tenha ocorrido erro, mostra
                //if (resposta.StatusResposta != MensagemResponseStatusEnum.OK)
                //    txtHistorico.Text += "Erro no envio: " + resposta.DescricaoResposta;
                //else
                //    txtHistorico.Text += "Ordem enviada com sucesso!";

                logger.Info(string.Format("{0}{1}", "\r\n " + int.Parse(_SS_StopSimplesResposta.pStrIdStopStart).ToString(), " Stop Executado."));

                txtHistorico.Text +=
                    string.Format("{0}{1}", "\r\n " + int.Parse(_SS_StopSimplesResposta.pStrIdStopStart).ToString(), " Stop Executado.");

            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "ProcessarEventoMDS: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "ProcessarEventoMDS: ", ex.Message));
            }

        }
        
         void EnviarMensagemAutenticacao()
         {

            A1_SignIn A1 = new A1_SignIn("BV");

            A1.idCliente = IdCliente;
            A1.idSistema = IdSistema;

            try
            {
                ASSocketConnection _Client =
                new ASSocketConnection();

                _Client.ASSocketOpen();
                _Client.SendData(A1.getMessageA1());
                _Client = null;
            }

            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "EnviarMensagemAutenticacao: ", ex.Message));
                throw new Exception(ex.Message);
            }
        }

         private void ArmarStopLoss(int idCliente, string Instrumento, decimal preco, decimal preco2, StopStartTipoEnum StopStartTipo)
        {
            try
            {

                //AutomacaoOrdensInfo _Order = new AutomacaoOrdensInfo();

                ///*
                //_Order.IdBolsa           = 1;
                //_Order.IdCliente         = idCliente;
                //_Order.IdMercadoTipo     = 1;
                //_Order.IdStopstartStatus = (int)OrdemStopStatus.Registrado;
                //_Order.IdStopStartTipo   = StopStartTipo;
                //_Order.Instrumento       = Instrumento;
                //_Order.DataValidade      = DateTime.Now;
                //*/
                //_Order.IdBolsa = 1;
                //_Order.Symbol = Instrumento;
                //_Order.StopStartStatusID = (int)OrdemStopStatus.RegistradaAplicacao;
                //_Order.ExpireDate = DateTime.Now;
                //_Order.Account = idCliente;
                //_Order.OrdTypeID = 'S';
                //_Order.IdStopStartTipo = StopStartTipo;
                ////_Order.
                
                //if (StopStartTipo == StopStartTipoEnum.StopLoss)
                //{
                //    _Order.SendStopGainPrice      = null;
                //    _Order.StopGainValuePrice     = null;
                //    _Order.StopLossValuePrice     = preco;
                //    _Order.SendStopLossValuePrice = preco2;
                //    _Order.OrderQty               = 100;
                //    _Order.AdjustmentMovelPrice   = null;
                //    _Order.InitialMovelPrice      = null;


                //    ArmarStartStopResponse _Response = _Servico.ArmarStopLoss(
                //          new ArmarStartStopRequest()
                //          {
                //              _AutomacaoOrdensInfo = _Order
                //          });


                //    Registrador.AddEvent(EventMds, Eventos);

                //    if (Contexto.SocketPrincipal != null)
                //    {
                //        Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                //    }
                //    else
                //    {
                //        Console.WriteLine("Cliente não esta conectado.");
                //    }
                //}

                //if (StopStartTipo == StopStartTipoEnum.StopGain)
                //{
                //    _Order.StopGainValuePrice       = preco;
                //    _Order.SendStopGainPrice        = 35.60M;
                //    _Order.StopLossValuePrice       = null;
                //    _Order.SendStopLossValuePrice   = null;
                //    _Order.OrderQty                 = 100;
                //    _Order.AdjustmentMovelPrice     = null;
                //    _Order.InitialMovelPrice        = null;


                //    ArmarStartStopResponse _Response = _Servico.ArmarStopGain(
                //          new ArmarStartStopRequest()
                //          {
                //              _AutomacaoOrdensInfo = _Order
                //          });

                //    Registrador.AddEvent(EventMds, Eventos);

                //    if (Contexto.SocketPrincipal != null)
                //    {
                //        Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                //    }
                //    else
                //    {
                //        Console.WriteLine("Cliente não esta conectado.");
                //    }
                //}

                //if (StopStartTipo == StopStartTipoEnum.StopSimultaneo)
                //{
                //    _Order.StopLossValuePrice     = preco;
                //    _Order.StopGainValuePrice     = preco2;
                //    _Order.SendStopGainPrice      = 34.90M;
                //    _Order.SendStopLossValuePrice = 34.50M;
                //    _Order.OrderQty               = 100;
                //    _Order.AdjustmentMovelPrice   = null;
                //    _Order.InitialMovelPrice      = null;


                //    ArmarStartStopResponse _Response = _Servico.ArmarStopSimultaneo(
                //          new ArmarStartStopRequest()
                //          {
                //              _AutomacaoOrdensInfo = _Order
                //          });


                //    Registrador.AddEvent(EventMds, Eventos);

                //    if (Contexto.SocketPrincipal != null)
                //    {
                //        Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                //    }
                //    else
                //    {
                //        Console.WriteLine("Cliente não esta conectado.");
                //    }
                //}


                //if (StopStartTipo == StopStartTipoEnum.StartCompra)
                //{

                //    _Order.StartPriceValue        = preco;
                //    _Order.SendStartPrice         = preco2;
                //    _Order.StopLossValuePrice     = null;
                //    _Order.SendStopLossValuePrice = null;
                //    _Order.OrderQty               = 100;
                //    _Order.AdjustmentMovelPrice   = null;
                //    _Order.InitialMovelPrice      = null;

                //    ArmarStartStopResponse _Response = _Servico.ArmarStartCompra(
                //      new ArmarStartStopRequest()
                //      {
                //          _AutomacaoOrdensInfo = _Order
                //      });

                //    Registrador.AddEvent(EventMds, Eventos);

                //    if (Contexto.SocketPrincipal != null)
                //    {
                //        Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                //    }
                //    else
                //    {
                //        Console.WriteLine("Cliente não esta conectado.");
                //    }


                //}

                //if (StopStartTipo == StopStartTipoEnum.StopMovel)
                //{

                //    _Order.StartPriceValue = preco;
                //    _Order.SendStartPrice = preco2;
                //    _Order.StopLossValuePrice = null;
                //    _Order.SendStopLossValuePrice = null;
                //    _Order.OrderQty = 100;
                //    _Order.AdjustmentMovelPrice = decimal.Parse(txtAdjMovelPrice.Text);
                //    _Order.InitialMovelPrice = decimal.Parse(txtInitialMovelPrice.Text);

                //    ArmarStartStopResponse _Response = _Servico.ArmarStartCompra(
                //      new ArmarStartStopRequest()
                //      {
                //          _AutomacaoOrdensInfo = _Order
                //      });

                //    Registrador.AddEvent(EventMds, Eventos);

                //    if (Contexto.SocketPrincipal != null)
                //    {
                //        Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _Order.Symbol, " - " + _Order.StopStartID.ToString(), " - " + _Order.StopLossValuePrice.ToString()));
                //    }
                //    else
                //    {
                //        Console.WriteLine("Cliente não esta conectado.");
                //    }


                //}
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao armar o StopLoss: ", ex.Message));
            }
        }

        #endregion

        private void btnDisparar_Click(object sender, EventArgs e)
        {

            StopStartTipoEnum StopStartTipo = (StopStartTipoEnum)(int.Parse(cbTipoOrdem.SelectedValue.ToString()));

            ArmarStopLoss(int.Parse(txtCodigoCliente.Text), txtInstrumento.Text, decimal.Parse(txtPreco.Text), decimal.Parse(txtPreco2.Text), StopStartTipo);
        }

        private void btLimpar_Click(object sender, EventArgs e)
        {
            txtHistorico.Text = string.Empty;
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            _Servico = Ativador.Get<IServicoOrdemStopStart>();

            if (_Servico != null)
            {
                CancelarStartStopOrdensRequest lRequestCancelamento = new CancelarStartStopOrdensRequest();

                lRequestCancelamento.IdStopStart = int.Parse(txtIdStopStart.Text);
                lRequestCancelamento.IdStopStartStatus = (int)OrdemStopStatus.CancelamentoEnviadoMDS;
                lRequestCancelamento.Instrument = txtInstrumento.Text;

                CancelarStartStopOrdensResponse lRespostaCancelamento = _Servico.CancelaOrdemStopStart(lRequestCancelamento);
            }
        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            AutomacaoOrdensDados ordemStop = new AutomacaoOrdensDados();

            AutomacaoOrdensInfo list = ordemStop.SelecionaOrdemStopStart(993);

            //Formatador.ArmarStopSimples(list);
        }
    }
}

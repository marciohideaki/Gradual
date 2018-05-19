using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Configuration;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Recebidas;
using Gradual.OMS.Contratos.Automacao.Ordens;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas;
using Gradual.OMS.Comunicacao.Automacao.Ordens;


namespace StopStartForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region [Propriedades]

        /// <summary>
        /// Source do EventLog
        /// </summary>
        private string EventLogSource{
            get{
                return ConfigurationSettings.AppSettings["EventLogSource"].ToString();
            }
        }

        /// <summary>
        /// Código do cliente
        /// </summary>
        private string IdCliente{
            get{
                return ConfigurationSettings.AppSettings["IdCliente"].ToString();
            }
        }

        private string IdSistema{
            get{
                return ConfigurationSettings.AppSettings["IdSistema"].ToString();
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

        MDSEventFactory EventMds = new MDSEventFactory();
         List<string> Eventos = new List<string>();
         IServicoAutomacaoOrdens _Ordens = Ativador.Get<IServicoAutomacaoOrdens>();

        #endregion

        #region ["Enumeração"]

        private enum RespostaOrdem : int
        {
            Rejeitado = 0,
            Aceito = 1,
        };

        public enum OrdemStopStatus : int
        {
            Registrado = 1,
            Aceito = 2,
            Rejeitado = 3,
            Enviado = 4,
            Executado = 5,
            Cancelado = 6,
            CancelamentoEnviado = 7,
            CancelamentoAceito = 9,
            AguardandoExecucao = 8 ,
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

                Event._MDSAuthenticationResponse +=
                    new Event._onMDSAuthenticationResponse(Event__MDSAuthenticationResponse);

                OMSEventHandlerClass omsEHC =
                    new OMSEventHandlerClass(EventMds);

                // Callback de execucao de ordem
                EventMds.OnMDSStopStartEvent +=
                    new MDSEventFactory._OnMDSStopStartEvent(EventMds_OnMDSStopStartEvent);

                EventMds.OnMDSSRespostaAutenticacaoEvent
                    += new MDSEventFactory._OnMDSSRespostaAutenticacaoEvent(EventMds_OnMDSSRespostaAutenticacaoEvent);

                EventMds.OnMDSSRespostaCancelamentoEvent +=
                    new MDSEventFactory._OnMDSSRespostaCancelamentoEvent(EventMds_OnMDSSRespostaCancelamentoEvent);

                Registrador.AddListener(EventMds);
            }
            catch (Exception ex)
            {
                WriteEventLog(string.Format("{0}{1}", "Initialize: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "Initialize: ", ex.Message));
            }

        }

        void EventMds_OnMDSSRespostaCancelamentoEvent(object sender, MDSEventArgs e)
        {
            try
            {
                CR_CancelamentoStopResposta _CR_CancelamentoStopResposta
                    = (CR_CancelamentoStopResposta)(sender);

                int id_stopstart = int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart);
                int id_status = int.Parse(_CR_CancelamentoStopResposta.pStrStatus);

                if (id_status == (int)RespostaOrdem.Aceito)
                {
                    _Ordens.CancelaOrdemStopStart(new CancelarStartStopOrdensRequest()
                    {
                        IdStopStart = id_stopstart,
                        IdStopStartStatus = (int)OrdemStopStatus.Cancelado,
                        Instrument = _CR_CancelamentoStopResposta.pStrInstrument
                    });
                    txtHistorico.Text += string.Format("{0}{1}", "\r\n " + int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart).ToString(), " Stop Cancelado pelo MDS");
                }
            }
            catch (Exception ex){
                WriteEventLog(string.Format("{0}{1}", "EventMds_OnMDSSRespostaCancelamentoEvent: ", ex.Message));
            }
        }


         void EventMds_OnMDSSRespostaAutenticacaoEvent(object sender, MDSEventArgs e)
        {
            try
            {
                RS_RespostaStop _RS_RespostaStop
                    = (RS_RespostaStop)(sender);

                int id_stopstart = int.Parse(_RS_RespostaStop.pStrIdStopstart);
                int id_status = int.Parse(_RS_RespostaStop.pStrStatus);

                if (id_status == (int)RespostaOrdem.Rejeitado){ // Stop Rejeitado

                    // Atualiza status da ordem para rejeitado.
                    //new DOrdem().AtualizaOrdemStop(
                    //  id_stopstart,
                    //  (int)OrdemStopStatus.Rejeitado
                    //  );
                                        
                    txtHistorico.Text +=
                        string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopstart).ToString(), " Stop rejeitado pelo MDS");

                }
                else if (id_status == (int)RespostaOrdem.Aceito) // Stop Aceito
                {

                    // Atualiza status da ordem para aceito.
                    //new DOrdem().AtualizaOrdemStop(
                    //  id_stopstart,
                    //  (int)OrdemStopStatus.Aceito
                    //  );

                    //// Atualiza Status da ordem para aguardando execução.
                    //new DOrdem().AtualizaOrdemStop(
                    // id_stopstart,
                    // (int)OrdemStopStatus.AguardandoExecucao
                    // );
                    
                    txtHistorico.Text +=
                        string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopstart).ToString(), " Stop aceito pelo MDS");
                }

            }
            catch (Exception ex)
            {
                WriteEventLog(string.Format("{0}{1}", "EventMds_OnMDSSRespostaAutenticacaoEvent: ", ex.Message));
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
                                            ProcessarEventoMDS(
                                                (object)(sender)
                                                );
                                        }));
            }
            catch (Exception ex)
            {
                WriteEventLog(string.Format("{0}{1}", "EventMds_OnMDSStopStartEvent: ", ex.Message));
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
                            Contexto.SocketPrincipal = _ClientSocket;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteEventLog(string.Format("{0}{1}", "Event__MDSAuthenticationResponse: ", ex.Message));
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region ["Funções"]

        /// <summary>
        /// Callback de respostas de mensagem de ordem Stop.
        /// </summary>
        /// <param name="sender"> Objeto de resposta.</param>
         void ProcessarEventoMDS(object sender){
            try
            {
                
                SS_StopSimplesResposta _SS_StopSimplesResposta =
                    (SS_StopSimplesResposta)(sender);


                // Atualiza a ordem stop para o status executada , bem como atualiza seu preço de referencia com o valor disparado pelo MDS
                //new DOrdem().AtualizaOrdemStop
                //    (
                //        int.Parse(_SS_StopSimplesResposta.pStrIdStopStart),
                //        (int)OrdemStopStatus.Executado,
                //        decimal.Parse(_SS_StopSimplesResposta.pStrPrecoReferencia.ToString())
                //    );


                IServicoOrdensServidorSemCallback servicoOrdens = Ativador.Get<IServicoOrdensServidorSemCallback>();

                ExecutarOrdemResponse resposta =
                    (ExecutarOrdemResponse)
                        servicoOrdens.ProcessarMensagem(
                            new ExecutarOrdemRequest()
                            {
                                //CodigoBolsa = _SS_StopSimplesResposta.pStrTipoBolsa,
                                //Account = "2222" ,
                                //Symbol = _SS_StopSimplesResposta.pStrInstrument,
                                //OrderQty = _SS_StopSimplesResposta.,
                                //Price = 10
                            });


                // Caso tenha ocorrido erro, mostra
                if (resposta.StatusResposta != MensagemResponseStatusEnum.OK)
                    txtHistorico.Text += "Erro no envio: " + resposta.DescricaoResposta;
                else
                    txtHistorico.Text += "Ordem enviada com sucesso!";


                txtHistorico.Text +=
                    string.Format("{0}{1}", "\r\n " + int.Parse(_SS_StopSimplesResposta.pStrIdStopStart).ToString(), " Stop Executado.");

            }
            catch (Exception ex)
            {
                WriteEventLog(string.Format("{0}{1}", "ProcessarEventoMDS: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "ProcessarEventoMDS: ", ex.Message));
            }

        }



        /// <summary>
        /// Escreve no EventLog do servidor
        /// </summary>
        /// <param name="Message"></param>
         private void WriteEventLog(string Message){
             EventLog.WriteEntry(EventLogSource, Message, EventLogEntryType.Error);
         }

         void EnviarMensagemAutenticacao(){

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
                WriteEventLog(string.Format("{0}{1}", "EnviarMensagemAutenticacao: ", ex.Message));
                throw new Exception(ex.Message);
            }
        }

        private void ArmarStopLoss(int idCliente, string Instrumento, decimal preco,decimal preco2, int idStopstart_tipo)
        {
            //try
            //{
            //    TOrdem _TOrder = new TOrdem();

            //    _TOrder.id_bolsa = 1;
            //    _TOrder.id_cliente = idCliente;
            //    _TOrder.id_mercado_tipo = 1;
            //    _TOrder.id_stopstart_status = (int)OrdemStopStatus.Registrado;
            //    _TOrder.id_stopstart_tipo = idStopstart_tipo;
            //    _TOrder.instrumento = Instrumento;
            //    _TOrder.data_validade = DateTime.Now;

            //    if (idStopstart_tipo == 1)
            //    {
            //        _TOrder.preco_envio_gain = null;
            //        _TOrder.preco_gain = null;
            //        _TOrder.preco_loss = preco;
            //        _TOrder.preco_envio_loss = 35.60M;
            //        _TOrder.quantidade = 100;
            //        _TOrder.valor_ajuste_movel = null;
            //        _TOrder.valor_inicio_movel = null;
            //        _TOrder.id_stopstart = _Ordens.ArmarStopLoss(_TOrder);

            //        Registrador.AddEvent(EventMds, Eventos);

            //        if (Contexto.SocketPrincipal != null){                        
            //            Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _TOrder.instrumento, " - " + _TOrder.id_stopstart.ToString(), " - " + _TOrder.preco_loss.ToString()));
            //        }
            //        else{
            //            Console.WriteLine("Cliente não esta conectado.");
            //        }
            //    }

            //    if (idStopstart_tipo == 2)
            //    {            
            //        _TOrder.preco_gain = preco;
            //        _TOrder.preco_envio_gain = 35.60M;
            //        _TOrder.preco_loss = null;
            //        _TOrder.preco_envio_loss = null;
            //        _TOrder.quantidade = 100;
            //        _TOrder.valor_ajuste_movel = null;
            //        _TOrder.valor_inicio_movel = null;

            //        _TOrder.id_stopstart = _Ordens.ArmarStopGain(_TOrder);

            //        Registrador.AddEvent(EventMds, Eventos);

            //        if (Contexto.SocketPrincipal != null){    
            //            Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _TOrder.instrumento, " - " + _TOrder.id_stopstart.ToString(), " - " + _TOrder.preco_loss.ToString()));
            //        }
            //        else{
            //            Console.WriteLine("Cliente não esta conectado.");
            //        }
            //    }

            //    if (idStopstart_tipo == 3)
            //    {
            //        _TOrder.preco_loss = preco;
            //        _TOrder.preco_gain = preco2;
            //        _TOrder.preco_envio_gain = 34.90M;
            //        _TOrder.preco_envio_loss = 34.50M;
            //        _TOrder.quantidade = 100;
            //        _TOrder.valor_ajuste_movel = null;
            //        _TOrder.valor_inicio_movel = null;

            //        _TOrder.id_stopstart = _Ordens.ArmarStopSimultaneo(_TOrder);
            //        Registrador.AddEvent(EventMds, Eventos);

            //        if (Contexto.SocketPrincipal != null)
            //        {
            //            Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _TOrder.instrumento, " - " + _TOrder.id_stopstart.ToString(), " - " + _TOrder.preco_loss.ToString()));
            //        }
            //        else
            //        {
            //            Console.WriteLine("Cliente não esta conectado.");
            //        }
            //    }

            //    if (idStopstart_tipo == 5){

            //        _TOrder.preco_start = preco;
            //        _TOrder.preco_envio_start = 35.60M;
            //        _TOrder.preco_loss = null;
            //        _TOrder.preco_envio_loss = null;
            //        _TOrder.quantidade = 100;
            //        _TOrder.valor_ajuste_movel = null;
            //        _TOrder.valor_inicio_movel = null;

            //        _TOrder.id_stopstart = _Ordens.ArmarStartCompra(_TOrder);
            //        Registrador.AddEvent(EventMds, Eventos);

            //        if (Contexto.SocketPrincipal != null)
            //        {
            //            Console.WriteLine(string.Format("{0}{1}{2}{3}", "Stop armado com sucesso:  ", _TOrder.instrumento, " - " + _TOrder.id_stopstart.ToString(), " - " + _TOrder.preco_loss.ToString()));
            //        }
            //        else
            //        {
            //            Console.WriteLine("Cliente não esta conectado.");
            //        }


            //    }
            //}
            //catch (Exception ex){
            //    throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao armar o StopLoss: ", ex.Message));
            //}
        }

        #endregion

        private void btnDisparar_Click(object sender, EventArgs e){
            ArmarStopLoss(int.Parse(txtCodigoCliente.Text), txtInstrumento.Text, decimal.Parse(txtPreco.Text),decimal.Parse(txtPreco2.Text),int.Parse(cbTipoOrdem.SelectedValue.ToString()));
        }

        private void btLimpar_Click(object sender, EventArgs e){
            txtHistorico.Text = string.Empty;
        }

        private void btCancelar_Click(object sender, EventArgs e){            
            //_Ordens.CancelaOrdemStopStart(txtInstrumento.Text, int.Parse(txtIdStopStart.Text), (int)OrdemStopStatus.CancelamentoEnviado);
        }
    }
}

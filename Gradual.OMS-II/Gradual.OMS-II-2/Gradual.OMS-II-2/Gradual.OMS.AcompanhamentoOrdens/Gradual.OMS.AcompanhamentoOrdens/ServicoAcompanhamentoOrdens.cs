using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.Threading;
using Gradual.OMS.AcompanhamentoOrdens.Lib;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using log4net;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib;
using Gradual.OMS.Ordens.Lib.Info;
using System.Globalization;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Gradual.OMS.AcompanhamentoOrdens
{
    public delegate void ChegadaDeAcompanhamentoHandler(OrdemInfo pAlteracao);

    public delegate void MensagemDeAcompanhamentoHandler(string pMensagem);


    /// <summary>
    /// Classe que implementa o serviço de acompanhamento de ordens, conectando com o roteador de ordens e realizando os logs e persistências necessários
    /// </summary>
    [ServiceBehavior(InstanceContextMode= InstanceContextMode.Single)]
    public class ServicoAcompanhamentoOrdens : IServicoAcompanhamentoOrdens, IServicoControlavel
    {
        #region Globais

        private System.Threading.Timer _TickerOrdens;

        private  IAssinaturasRoteadorOrdensCallback gClienteRoteadorOrdens;                                                                  // Proxy para conexão duplex com o roteador de ordens

        private  OrdemAlteradaCallBack gCallBacker;                                                                                          // Objeto de callback que recebe os "eventos" da conexão duplex com o roteador de ordens

        private  Queue<AcompanhamentoOrdemInfo> gFilaDeAcompanhamentosParaPersistencia = new Queue<AcompanhamentoOrdemInfo>();               // Fila de objetos que chegaram do roteador para inserção no banco; a persistência lê dessa fila porque roda em um thread separado.

        private  Thread gThreadDePersistencia;                                                                                               // Thread que roda a função de persistência no banco
        
        private  Thread gThreadDeCallBack;                                                                                                   // Thread que roda a função de persistência no banco

        private  StreamWriter gStreamDeLog = null;                                                                                           // Stream que aponta para um arquivo de Log que a aplicação possa utilizar

        private  StreamWriter gStreamDeComandos = null;                                                                                      // Stream que aponta para um arquivo de saída, onde serão escritos os comandos que deveriam ir para o banco de dados porém não foram por algum problema no banco

        private static string gComandoExecPrc_Update_Order =                                                                                 // String para formatação do comanado quando for feita saída pra arquivo
        "    EXECUTE PRC_UPDATE_ORDER     /* @ClOrdID: */   '{0}',      /* @OrdStatusId: */   {1},      /* @ExchangeNumberID: */   '{2}',     /* @OrderQtyRemaining: */   {3},  /* @CumQty */ {4}, /* @Preco */ {5}";

        private static string gComandoExecPrc_Ins_Order_Detail =                                                                             // String para formatação do comanado quando for feita saída pra arquivo
        "    EXECUTE PRC_INS_ORDER_DETAIL_IND /* @TransactID:*/ '{0}',      /* @ClOrdID:*/       '{1}',     /* @OrderQty: */            {2},      /* @OrdQtyRemaining: */     {3},  /* @CumQty */ {4}, /* TradeQty */ {5},  /* @Price: */   {6}, /*  @OrderStatusID: */ {7}, /* Description */ '{8}',  /* EventTime */ '{9}'";

        private static readonly ILog gLog4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private Dictionary<int, List<OrdemInfo>> gAcompanhamentosEmMemoria = new Dictionary<int, List<OrdemInfo>>();                         // Dicionário de acompanhamentos que ficam em memória; a chave é o id do cliente
        
        private Dictionary<int, int> gCodigosBmfEmMemoria = new Dictionary<int, int>();                                                      // Dicionário de códigos BMF que ficam em memória; a chave é o id do cliente

        private  Dictionary<int, DateTime> gListaDeUsuariosDeslogados = new Dictionary<int, DateTime>();                                     // Lista de usuários que foram deslogados, para liberar memória quando for necessário; o DateTime marca a hora de corte que eles podem ficar na memória

        private  CamadaDeDados gCamadaDePersistencia;                                                                                        // Classe de acesso a dados para persistência

        private  CamadaDeDados gCamadaDeConsulta;                                                                                            // Classe de acesso a dados para consultas

        private ServicoStatus _status = ServicoStatus.Parado;

        private bool _bKeepRunning = true;
        
        private DateTime gDiaEmQueoServicoFoiIniciado = DateTime.Today.AddDays(1).AddSeconds(-1);  //Dia em que o serviço foi iniciado, para que quando "vire" a madrugada ele recarregue as ordens online. Está como 23:59:59 porque o algoritmo de comparação verifica pelo maior ou menor

        private long ultimodiacarregado = 0;

        private bool gbAtualizarDB = false;

        private bool bRemoveDigito = true;

        private List<string> lstFiltroPorta = new List<string>();
        private Queue<OrdemInfo> queueOrdens = new Queue<OrdemInfo>();
        Thread thProcOrdens;
        Semaphore semProcOrdens = new Semaphore(0,int.MaxValue);

        //para teste:
        //private DateTime gDiaEmQueoServicoFoiIniciado = DateTime.Now.AddMinutes(5);                                            //Dia em que o serviço foi iniciado, para que quando "vire" a madrugada ele recarregue as ordens online. Está como 23:59:59 porque o algoritmo de comparação verifica pelo maior ou menor

        private Dictionary<string, Dictionary<System.String, Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento>> ListaPosicoes;

        #endregion

        #region Propriedades

        private  string _PersistenciaEmArquivo;

        private static bool OrdensExpiradasAtualizadas { set; get; }
        private const int HorarioFechamentoBolsa = 20;

        /// <summary>
        /// Flag de persistência dos comandos em arquivo: 'nunca', 'sempre' ou 'erro'
        /// </summary>
        private  string PersistenciaEmArquivo                                                                                          // Flag de configuração: 'nunca' nunca salva persistencia em arquivo, 'sempre' sempre salva, e 'erro' salva somente quando houver erro do banco
        {
            get
            {
                if(_PersistenciaEmArquivo == null)
                {
                    _PersistenciaEmArquivo = ConfigurationManager.AppSettings["PersistenciaEmArquivo"];

                    if (string.IsNullOrEmpty(_PersistenciaEmArquivo))
                        _PersistenciaEmArquivo = "erro";                                                                                     //valor padrão: 'erro'
                    else
                        _PersistenciaEmArquivo = _PersistenciaEmArquivo.ToLower();
                }

                return _PersistenciaEmArquivo;
            }
        }

        #endregion

        #region Eventos

        public  event MensagemDeAcompanhamentoHandler MensagemDeAcompanhamento;

        private  void OnMensagemDeAcompanhamento(string pMensagem, params object[] pParams)
        {
            if (MensagemDeAcompanhamento != null)
                MensagemDeAcompanhamento(string.Format(pMensagem, pParams));

            SaidaParaLog(pMensagem, pParams);

            gLog4Net.InfoFormat(pMensagem, pParams);
        }

        #endregion

        #region Métodos Private

        /// <summary>
        /// Inicializa o TimerCallback com 5 minutos de intervalo.
        /// </summary>
        private void IniciarThreadMonitoramentoOrdensExpiradas()
        {
            _TickerOrdens = new System.Threading.Timer(new TimerCallback(CancelarOrdensExpiradas), null, 0, 300000);
        }

        /// <summary>
        /// Método responsável por excluir todas as ordens com validade para o dia que não foram executadas ou que foram parcialmente executadas pelo Roteador de Ordens
        /// </summary>
        /// <param name="state">Estado da Thread que invoka o método </param>
        private void CancelarOrdensExpiradas(object state)
        {
            gLog4Net.Info("Callback CancelarOrdensExperiradas Inicializado com sucesso");

            if (DateTime.Now.Hour < HorarioFechamentoBolsa){
                OrdensExpiradasAtualizadas = false;

                gLog4Net.Info(string.Format("{0}{1}", "Mudança de dia. Atualiza a variavel de OrdensDiariasAtualizadas para falso; Horário de referencia: ", DateTime.Now.ToString()));
            }

            if ((DateTime.Now.Hour >= HorarioFechamentoBolsa) && (OrdensExpiradasAtualizadas == false))
            {
                gLog4Net.Info("Inicializa rotina de cancelamento de ordens executadas");
                CamadaDeDados _CamadaDeDados = new CamadaDeDados();
                //_CamadaDeDados.AbrirConexao();

                // Busca todas as ordens em aberto
                gLog4Net.Info("Inicializa pesquisa de ordens em aberto");
                List<string> _lstOrdens = _CamadaDeDados.BuscarOrdensValidasParaoDia();            

                // Ativa o seviço de ordens
                IServicoOrdens ServicoOrdens = Ativador.Get<IServicoOrdens>();

                //Verifica se existem ordens a serem canceladas.
                if (_lstOrdens.Count > 0)
                {
                    gLog4Net.Info("Foram encontradas: " + _lstOrdens.Count.ToString() + " a serem cancelas");

                    for (int i = 0; i <= _lstOrdens.Count - 1; i++)
                    {
                        ClienteCancelamentoInfo ClienteCancelamentoInfo = new ClienteCancelamentoInfo()
                        {
                            OrderID = _lstOrdens[i].ToString()
                        };

                        EnviarCancelamentoOrdemRequest request = new EnviarCancelamentoOrdemRequest()
                        {
                            ClienteCancelamentoInfo = ClienteCancelamentoInfo
                        };

                        gLog4Net.Info("Tentando cancelar ClOrdID :" + ClienteCancelamentoInfo.OrderID);
                        EnviarCancelamentoOrdemResponse response =
                         ServicoOrdens.CancelarOrdem(request);

                        if (response.StatusResposta == Ordens.Lib.Enum.CriticaRiscoEnum.Sucesso)
                        {

                            gLog4Net.Info(ClienteCancelamentoInfo.OrderID + " enviada para o roteador ");
                            gLog4Net.Info("Descricao :" + response.DescricaoResposta);
                        }
                        else
                        {
                            gLog4Net.Info("Ocorreu um erro ao enviar o cancelamento de ordens para o Roteador");

                            gLog4Net.Info("Descricao :" + response.DescricaoResposta);
                            gLog4Net.Info("StackTrace :" + response.StackTrace);

                        }

                    }

                   gLog4Net.Info("Atualiza status da variavel de controle para true, e aguarda ate o próximo dia.");      

                }
                else
                {
                    gLog4Net.Info("O sistema não encontrou nenhuma ordem a ser expirada");
                }

                OrdensExpiradasAtualizadas = true;

            }
        }
       
        /// <summary>
        /// Escreve uma mensagem no arquivo de log
        /// </summary>
        /// <param name="pMensagem">Mensagem, com opção de formatações</param>
        /// <param name="pParams">Parâmetros para formatação</param>
        private  void SaidaParaLog(string pMensagem, params object[] pParams)
        {
            if (gStreamDeLog != null)
            {
                try
                {
                    gStreamDeLog.WriteLine(string.Format("{0}> {1}"
                                           , DateTime.Now.ToString("HH:mm:ss")
                                           , string.Format(pMensagem, pParams)));
                }
                catch { }
            }
        }

        /// <summary>
        /// Escreve uma mensagem no arquivo de comandos
        /// </summary>
        /// <param name="pMensagem">Mensagem, com opção de formatações</param>
        /// <param name="pParams">Parâmetros para formatação</param>
        private  void SaidaParaComando(string pMensagem, params object[] pParams)
        {
            if (gStreamDeComandos != null)
            {
                try
                {
                    gStreamDeComandos.WriteLine(string.Format(pMensagem, pParams));
                    gStreamDeComandos.Flush();
                }
                catch(Exception ex)
                {
                    gLog4Net.Error("SaidaParaComando: " + ex.Message, ex);
                }
            }
        }

        private  string GerarComandoDoAcompanhamento(AcompanhamentoOrdemInfo pAcompanhamento)
        {
            string lComando = "/* Comandos para inclusão de acompanhamento para ordem [{0}] */\r\n{1}\r\n{2}\r\n\r\n";

            lComando = string.Format(lComando,
                                          pAcompanhamento.NumeroControleOrdem
                                        , string.Format(gComandoExecPrc_Update_Order
                                                        , pAcompanhamento.NumeroControleOrdem
                                                        , Convert.ToInt32(pAcompanhamento.StatusOrdem)
                                                        , pAcompanhamento.CodigoResposta
                                                        , pAcompanhamento.QuantidadeRemanescente
                                                        , pAcompanhamento.QuantidadeExecutada
                                                        , pAcompanhamento.Preco.ToString(CultureInfo.InvariantCulture))
                                        , string.Format(gComandoExecPrc_Ins_Order_Detail
                                                        , pAcompanhamento.CodigoTransacao
                                                        , pAcompanhamento.NumeroControleOrdem
                                                        , pAcompanhamento.QuantidadeSolicitada
                                                        , pAcompanhamento.QuantidadeRemanescente
                                                        , pAcompanhamento.QuantidadeExecutada
                                                        , pAcompanhamento.QuantidadeNegociada
                                                        , pAcompanhamento.Preco.ToString(CultureInfo.InvariantCulture)
                                                        , Convert.ToInt32(pAcompanhamento.StatusOrdem)
                                                        , pAcompanhamento.Descricao
                                                        , pAcompanhamento.DataAtualizacao.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                                                        );

            return lComando;
        }

        /// <summary>
        /// Abre um thread separado para ficar verificando a fila de objetos para persistir e incluir no banco
        /// </summary>
        private  void IniciarThreadDeCallBack()
        {
            gThreadDeCallBack = new Thread(new ThreadStart(ObservarRoteadorParaCallBacks));

            gThreadDeCallBack.Start();
        }


        /// <summary>
        /// Rotina principal da thread de recepcao dos callbacks do Roteador
        /// </summary>
        private  void ObservarRoteadorParaCallBacks()
        {
            try
            {
                gCamadaDeConsulta = new CamadaDeDados();

                OnMensagemDeAcompanhamento("Abrindo conexão com o banco de dados para consultas...");

                //try
                //{
                //    gCamadaDeConsulta.AbrirConexao();

                //    if (gCamadaDeConsulta.ConexaoAberta)
                //        OnMensagemDeAcompanhamento("Conexão aberta com sucesso.");
                //}
                //catch (Exception ex)
                //{
                //    gLog4Net.Fatal("ObservarRoteadorParaCallBacks(): abrir conexão com o banco:", ex);
                //}

                if (gClienteRoteadorOrdens == null)
                {
                    OnMensagemDeAcompanhamento("Conexão não iniciada, abrindo...");

                    gCallBacker = new OrdemAlteradaCallBack();                                                                           // Classe responsável por receber a resposta do servidor via tcp e "alertar" o cliente que a contém usando um evento 
                    // (implementação logo abaixo, nesse mesmo arquivo)

                    gCallBacker.ChegadaDeAcompanhamento += new ChegadaDeAcompanhamentoHandler(gCallBacker_ChegadaDeAcompanhamento);      // Assina o evento do "callbakcer", é aqui que vão chegar efetivamente as "respostas" duplex do roteador de ordens

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

                    Thread.Sleep(100);
                }
                while (_bKeepRunning);
            }
            catch (ThreadAbortException threx)
            {
                gLog4Net.Info("ObservarRoteadorParaCallBacks(): " + threx.Message, threx);
                OnMensagemDeAcompanhamento("Thread de observação do roteador sendo fechado");
            }
            catch (Exception ex)
            {
                gLog4Net.Error("ObservarRoteadorParaCallBacks(): " + ex.Message, ex);
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
                gLog4Net.Error("Erro em _cancelRoteadorCallback():" + ex.Message, ex);
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
                OnMensagemDeAcompanhamento("Chamando ativador para instanciar o cliente do roteador...");

                gClienteRoteadorOrdens = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(objectimpl);

                if (gClienteRoteadorOrdens != null)
                {
                    OnMensagemDeAcompanhamento("Cliente do roteador instanciado, enviando request de assinatura...");

                    AssinarExecucaoOrdemResponse lResposta = gClienteRoteadorOrdens.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());                         // Faz a chamada pra abrir a conexão com o roteador; só serve pra enviar o contexto, e o roteador assinar a ponte duplex 

                    if (lResposta.StatusResposta == Library.MensagemResponseStatusEnum.OK)
                    {
                        OnMensagemDeAcompanhamento("Conexão com o roteador aberta, resposta do servidor: [{0}] [{1}]"                    // Abriu ok, solta o evento de mensagem
                                                   , lResposta.StatusResposta
                                                   , lResposta.DescricaoResposta);
                    }
                    else
                    {
                        OnMensagemDeAcompanhamento("Conexão com o roteador com erro, resposta do servidor: [{0}] [{1}]"                  // Erro na abertura de conexão; TODO: verificar protocolo de erro nesse caso
                                                   , lResposta.StatusResposta
                                                   , lResposta.DescricaoResposta);

                        gClienteRoteadorOrdens = null;                                                                                   // Setando como null pra tentar novamente depois, ver conforme o protocolo o que fazer
                    }

                    // Assina os status de conexao a bolsa para manter o canal aberto.
                    AssinarStatusConexaoBolsaResponse resp = gClienteRoteadorOrdens.AssinarStatusConexaoBolsa(new AssinarStatusConexaoBolsaRequest());
                }
            }
            catch (Exception ex)
            {
                gLog4Net.Error("Erro em _assinaCallbackRoteador():" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Abre um thread separado para ficar verificando a fila de objetos para persistir e incluir no banco
        /// </summary>
        private  void IniciarThreadDePersistencia()
        {
            gThreadDePersistencia = new Thread(new ThreadStart(ObservarListaParaPersistencia));

            gThreadDePersistencia.Start();
        }

        /// <summary>
        /// Verifica a fila de persistência e inclui os acompanhamentos no banco
        /// </summary>
        private  void ObservarListaParaPersistencia()
        {
            try
            {
                AcompanhamentoOrdemInfo lInfo;

                object lAcompanhamentoID;

                byte lContagemDeAguarde = 0;

                gCamadaDePersistencia = new CamadaDeDados();

                while (_bKeepRunning)
                {
                    try
                    {
                        //if (!gCamadaDePersistencia.ConexaoAberta)
                        //{
                        //    OnMensagemDeAcompanhamento("Abrindo conexão com o banco de dados para persistência...");
                        //    try
                        //    {
                        //        gCamadaDePersistencia.AbrirConexao();

                        //        if (gCamadaDePersistencia.ConexaoAberta)
                        //            OnMensagemDeAcompanhamento("Conexão aberta com sucesso.");
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        gLog4Net.Fatal("ObservarListaParaPersistencia(): Erro ao abrir conexao com banco: " + ex.Message, ex);
                        //        Thread.Sleep(500);
                        //        continue;
                        //    }
                        //}

                        while (gFilaDeAcompanhamentosParaPersistencia.Count > 0)                                                                // Verifica se tem itens la fila para incluir
                        {
                            lock (gFilaDeAcompanhamentosParaPersistencia)
                            {
                                lInfo = gFilaDeAcompanhamentosParaPersistencia.Dequeue();                                                       // Pega o primeiro da fila
                            }

                            OnMensagemDeAcompanhamento("Acompanhamento CodigoTransacao [{0}] NumeroControleOrdem [{1}] removido da fila para persistência."
                                                        , lInfo.CodigoTransacao
                                                        , lInfo.NumeroControleOrdem);   // Mensagem de que o objeto foi retirado da fila pra inclusão

                            if (PersistenciaEmArquivo == "sempre")
                                SaidaParaComando(GerarComandoDoAcompanhamento(lInfo));

                            if (gbAtualizarDB)
                            {
                                try
                                {
                                    lAcompanhamentoID = gCamadaDePersistencia.InserirAcompanhamentoDeOrdem(lInfo);                               // Realiza a inclusão no banco via camada de persistência

                                    OnMensagemDeAcompanhamento("Acompanhamento CodigoTransacao [{0}] NumeroControleOrdem [{1}] incluído com sucesso. ID: [{2}]"
                                                                , lInfo.CodigoTransacao
                                                                , lInfo.NumeroControleOrdem
                                                                , lAcompanhamentoID);                            // Mensagem que o objeto foi incluído com sucesso

                                }
                                catch (Exception exinc)
                                {
                                    gLog4Net.Error("Erro ao incluir acompanhamento no banco:" + exinc.Message, exinc);

                                    if (PersistenciaEmArquivo == "erro")
                                        SaidaParaComando(GerarComandoDoAcompanhamento(lInfo));

                                }
                            }
                            lContagemDeAguarde = 0;
                        }

                        lContagemDeAguarde++;
                        if (lContagemDeAguarde >= 100)
                        {
                            OnMensagemDeAcompanhamento("Aguardando acompanhamentos para persistência...");                               // Não havia itens na fila para incluir, dá mensagem

                            lContagemDeAguarde = 0;

                            VerificarViradaDoDia();
                        }

                        Thread.Sleep(200);                                                                                               // Aguarda um tempo pro roteador mandar mais mensagens
                    }
                    catch (Exception ex)
                    {
                        gLog4Net.Error("ObservarListaParaPersistencia() main loop: " + ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                gLog4Net.Fatal("ObservarListaParaPersistencia(): " + ex.Message, ex);

                // mata o processo, forcando o cluster a reiniciar o servico
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// Abre os arquivos de log e de comandos
        /// </summary>
        private  void AbrirArquivosDeSaida()
        {
            string lCaminhoDoLog;

            //Arquivo de log:

            lCaminhoDoLog = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);                               // Pega o diretório em que a DLL está sendo executada

            lCaminhoDoLog = Path.Combine(lCaminhoDoLog
                                         , string.Format("Acompanhamento-de-Ordens-{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));          // Concatena o nome do arquivo de log

            OnMensagemDeAcompanhamento("Abrindo arquivo de Log [{0}]", lCaminhoDoLog);

            gStreamDeLog = File.AppendText(lCaminhoDoLog);                                                                                   // Abre o arquivo no reader gStreamDeLog

            gStreamDeLog.AutoFlush = true;                                                                                                   // Sempre que der uma mensagem já salva o arquivo

            gStreamDeLog.Write("\r\nStream aberto em [{0}]\r\n", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));                              // Loga a abertura

            OnMensagemDeAcompanhamento("Arquivo de Log aberto com sucesso.");


            //Arquivo de saída:

            lCaminhoDoLog = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);                               // Pega o diretório em que a DLL está sendo executada

            lCaminhoDoLog = Path.Combine(lCaminhoDoLog
                                         , string.Format("Acompanhamento-de-Ordens-{0}.sql", DateTime.Now.ToString("yyyy-MM-dd")));          // Concatena o nome do arquivo de log

            OnMensagemDeAcompanhamento("Abrindo arquivo de Saída [{0}]", lCaminhoDoLog);

            gStreamDeComandos = File.AppendText(lCaminhoDoLog);                                                                              // Abre o arquivo no reader gStreamDeLog

            gStreamDeComandos.AutoFlush = true;

            gStreamDeComandos.Write("\r\n/* Stream aberto em [{0}] */\r\n", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

            OnMensagemDeAcompanhamento("Arquivo de Comandos aberto com sucesso.");

        }

        /// <summary>
        /// Adiciona as ordens de hoje do cliente com a conta pContaDoCliente à memória, a partir do banco de dados
        /// </summary>
        /// <param name="pContaDoCliente"></param>
        private  void AdicionarOrdensDeHojeDoClienteAosAcompanhamentosEmMemoria(int pContaDoCliente, int pCodigoBmfDoCliente)
        {
            if (!gAcompanhamentosEmMemoria.ContainsKey(pContaDoCliente))                                                                     // Pode ser que a chave desse cliente ainda exista por causa da tolerância de 10 min pra tirar da memória
            {
                List<OrdemInfo> lOrdensDeHojeDoCliente;                                                                                      // Variável pra guardar a lista das ordens de hoje do cliente

                int lCodBmf = 0;

                if (pCodigoBmfDoCliente > 0) lCodBmf = pCodigoBmfDoCliente;

                lOrdensDeHojeDoCliente = gCamadaDeConsulta.BuscarOrdensOnline(pContaDoCliente, lCodBmf);                         // Realiza a consulta das ordens online

                gAcompanhamentosEmMemoria.Add(pContaDoCliente, lOrdensDeHojeDoCliente);                                                      // Adiciona a lista de ordens do usuário à memória

                ProcessarPosicaoNet(pContaDoCliente.ToString(), lOrdensDeHojeDoCliente);
            }
        }

        /// <summary>
        /// Verifica a lista de usuários que fizeram logout para liberar espaço na memória
        /// </summary>
        private  void VerificarListaDeUsuariosDeslogadosParaLiberarMemoria()
        {
            OnMensagemDeAcompanhamento("Verificando lista de usuários para liberar memória; [{0}] usuários em potencial, consumo de memória: [{1}] bytes"
                                       , gListaDeUsuariosDeslogados.Count
                                       , System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64);

            List<int> lListaParaExcluir = new List<int>();                                                                             // Inicia lista das chaves que serão removidas

            foreach (int lChave in gListaDeUsuariosDeslogados.Keys)                                                                          // Roda a lista de usuários deslogados verificando se a tolerância já passou
            {
                if (gListaDeUsuariosDeslogados[lChave] < DateTime.Now)
                    lListaParaExcluir.Add(lChave);                                                                                           // Passado o tempo, adiciona a chave na lista de exclusão
            }

            OnMensagemDeAcompanhamento("[{0}] usuários em memória, [{1}] serão removidos"
                                        , gAcompanhamentosEmMemoria.Keys.Count
                                        , lListaParaExcluir.Count);

            foreach (int lChave in lListaParaExcluir)
            {
                try
                {
                    gListaDeUsuariosDeslogados.Remove(lChave);                                                                               // Remove da lista de usuários deslogados

                    gAcompanhamentosEmMemoria.Remove(lChave);                                                                                // Remove da memória de acompanhamento

                    OnMensagemDeAcompanhamento("Usuário [{0}] removido", lChave);
                }
                catch { }
            }
        }

        /// <summary>
        /// Verifica se passou o dia em que o serviço foi iniciado, para recarregar as ordens online dos clientes
        /// </summary>
        private void VerificarViradaDoDia()
        {
            // como o "filtro" das ordens online é dependente do dia, precisamos verificar se é necessário recarregá-las

            string lMensagem = "Verificando virada do dia... ";

            long hj = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd"));

            if (hj > ultimodiacarregado)
            {
                lMensagem += "virada necessária!";

                // Espera 10 segundos por conta da query... as vezes o SQL
                // esta com uma ligeira defasagem no horario e a query retorna
                // as ordens antigas
                Thread.Sleep(10 * 1000);

                OnMensagemDeAcompanhamento(lMensagem);
                
                List<int> lIdsEmMemoria = new List<int>();

                OnMensagemDeAcompanhamento("Extraindo chaves da memória...");

                foreach (int lChave in gAcompanhamentosEmMemoria.Keys)
                {
                    lIdsEmMemoria.Add(lChave);
                }

                OnMensagemDeAcompanhamento("[{0}] chaves para recuperar.", lIdsEmMemoria.Count);

                gLog4Net.Info("Recarregando posicao net e acompanhamentos");
                gAcompanhamentosEmMemoria = new Dictionary<int, List<OrdemInfo>>();

                ListaPosicoes = new Dictionary<string, Dictionary<System.String, Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento>>();

                CamadaDeDados camada = new CamadaDeDados();
                List<OrdemInfo> ordensAtivas = camada.BuscarOrdensAtivas();

                foreach (OrdemInfo ordem in ordensAtivas)
                {
                    int lCodBovespa = ordem.Account;

                    // Obtem cod bovespa caso seja ordem BMF com cod conta diferente
                    if (!gCodigosBmfEmMemoria.ContainsKey(lCodBovespa))
                    {
                        foreach (int lCodBov in gCodigosBmfEmMemoria.Keys)
                        {
                            if (gCodigosBmfEmMemoria[lCodBov] == ordem.Account)
                            {
                                lCodBovespa = lCodBov;
                                break;
                            }
                        }
                    }

                    if (!gAcompanhamentosEmMemoria.ContainsKey(lCodBovespa))
                    {
                        gAcompanhamentosEmMemoria.Add(lCodBovespa, new List<OrdemInfo>());
                    }

                    gAcompanhamentosEmMemoria[lCodBovespa].Add(ordem);
                }

                foreach (int lCodBovespa in gAcompanhamentosEmMemoria.Keys)
                {
                    ProcessarPosicaoNet(lCodBovespa.ToString(), gAcompanhamentosEmMemoria[lCodBovespa]);
                }

                //foreach (int lChave in lIdsEmMemoria)
                //{
                //    try
                //    {
                //        OnMensagemDeAcompanhamento("Recuperando dados para [{0}]", lChave);

                //        AdicionarOrdensDeHojeDoClienteAosAcompanhamentosEmMemoria(lChave, gCodigosBmfEmMemoria.ContainsKey(lChave) ? gCodigosBmfEmMemoria[lChave] : int.MinValue);
                //    }
                //    catch (Exception ex)
                //    {
                //        gLog4Net.Error("Erro ao recuperar dados da chave [" + lChave + "]: " + ex.Message, ex);
                //    }
                //}
                
                gDiaEmQueoServicoFoiIniciado = DateTime.Today.AddDays(1).AddMinutes(-1);
                
                //para teste:
                //gDiaEmQueoServicoFoiIniciado = DateTime.Now.AddMinutes(5);
                ultimodiacarregado = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd"));

                OnMensagemDeAcompanhamento("Fim da virada para o dia [{0}]", gDiaEmQueoServicoFoiIniciado);
            }
            else
            {
                lMensagem += "virada desnecessária.";
                
                OnMensagemDeAcompanhamento(lMensagem);
            }
        }

        #endregion

        #region Event Handlers
        //void gCallBacker_ChegadaDeAcompanhamento(OrdemInfo pOrdemAlterada)
        //{
        //    try
        //    {
        //        OrdemInfo ordemoriginal = null;

        //        // Está combinado com o roteador (Ponso) que sempre vem um OrdemInfo com 1 AcompanhamentoInfo; safety check:
        //        if (pOrdemAlterada.Acompanhamentos.Count == 0)
        //        {
        //            OnMensagemDeAcompanhamento("OrdemInfo para ordem [{0}] está sem acompanhamentos na lista, ignorando..."
        //                                       , pOrdemAlterada.ClOrdID);

        //            return;
        //        }
        //        else if (pOrdemAlterada.Acompanhamentos.Count > 1)
        //        {
        //            OnMensagemDeAcompanhamento("OrdemInfo para ordem [{0}] tem [{1}] acompanhamentos, somente um era esperado! O primeiro será gravado, mas os outros serão perdidos!!"
        //                                       , pOrdemAlterada.ClOrdID
        //                                       , pOrdemAlterada.Acompanhamentos.Count);
        //        }

        //        string lMensagem = "";

        //        int lCodDaOrdem = pOrdemAlterada.Account;

        //        if (pOrdemAlterada.Exchange != null && pOrdemAlterada.Exchange.ToUpper() == "BOVESPA")
        //            lCodDaOrdem = Convert.ToInt32(pOrdemAlterada.Account.ToString().Substring(0, pOrdemAlterada.Account.ToString().Length - 1)); // Trunca o ultimo digito do codigo do cliente (DV) quando for mercado BOVESPA

        //        int lCodBovespa = lCodDaOrdem;                                                                                                   //Assume o mais comum, que o código da ordem é o código bovespa
        //        int lCodBmf;

        //        // Clona a info de acompanhamento, para restaurar a ordem corretamente em caso de rejeicao
        //        AcompanhamentoOrdemInfo infoPersistencia = _cloneAcompanhamentoInfo(pOrdemAlterada.Acompanhamentos[0]);

        //        if (lstFiltroPorta != null && lstFiltroPorta.Contains(pOrdemAlterada.ChannelID.ToString()))
        //        {
        //            gLog4Net.Warn("Ordem [" + pOrdemAlterada.ClOrdID + "] filtrada para gravacao na base, channel [" + pOrdemAlterada.ChannelID.ToString() + "]");
        //        }
        //        else
        //        {
        //            lock (gFilaDeAcompanhamentosParaPersistencia)
        //            {
        //                gFilaDeAcompanhamentosParaPersistencia.Enqueue(pOrdemAlterada.Acompanhamentos[0]);                                               // Adiciona o acompanhamento pra lista de persistência
        //            }
        //        }

        //        // Se foi rejeicao de uma modificacao de ordem, efetua um tratamento diferenciado
        //        if (pOrdemAlterada.OrigClOrdID != null &&
        //            pOrdemAlterada.OrigClOrdID.Length > 0 &&
        //            pOrdemAlterada.OrdStatus == OrdemStatusEnum.REJEITADA)
        //        {
        //            gLog4Net.Warn("Modificacao [" + pOrdemAlterada.ClOrdID + "] da ordem [" + pOrdemAlterada.OrigClOrdID + "] rejeitada, efetuando restauro do ultimo status");

        //            CamadaDeDados db = new CamadaDeDados();

        //            gLog4Net.Debug("Trocando numero de controle");

        //            db.TrocarNumeroControleOrdem(pOrdemAlterada);

        //            gLog4Net.Debug("Obtendo ordem Original");

        //            ordemoriginal = db.BuscarOrdemOriginal(pOrdemAlterada);

        //            gLog4Net.Debug("Atualizando ordem original");

        //            db.AtualizarOrdem(ordemoriginal);

        //            gLog4Net.Debug("Buscou ordem original [" + ordemoriginal.ClOrdID + "] [" + ordemoriginal.Symbol + "] p=[" + ordemoriginal.Price + "] q=[" + ordemoriginal.OrderQty + "]");
        //        }



        //        if (!gAcompanhamentosEmMemoria.ContainsKey(lCodDaOrdem))                                                                         // Verifica se já tem no dicionário assumindo que a ordem chegou como conta bovespa mesmo
        //        {
        //            foreach (int lCodBov in gCodigosBmfEmMemoria.Keys)
        //            {
        //                if (gCodigosBmfEmMemoria[lCodBov] == lCodDaOrdem)
        //                {
        //                    lCodBovespa = lCodBov;
        //                    lCodBmf = lCodDaOrdem;

        //                    lMensagem += string.Format(" (ordem executada com código BMF [{0}], mapeada para código bovespa [{1}])", lCodBmf, lCodBovespa);

        //                    break;
        //                }
        //            }
        //        }

        //        // Se não tem um dicionário para esse código de cliente, adiciona (isso pode acontecer se chegarem ordens de outros sistemas, que não chamaram o InformarLogin)

        //        if (!gAcompanhamentosEmMemoria.ContainsKey(lCodBovespa))
        //        {
        //            gAcompanhamentosEmMemoria.Add(lCodBovespa, new List<OrdemInfo>());

        //            lMensagem += " (lista pra conta do cliente ainda não existia)";
        //        }

        //        bool bAcompanhamentoFound = false;

        //        for (int a = gAcompanhamentosEmMemoria[lCodBovespa].Count - 1; a >= 0; a--)
        //        {
        //            if (gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID == pOrdemAlterada.ClOrdID)                                             // Verifica se a ordem já existia na memória, ou seja, é um novo acompanhamento de uma ordem que já havia recebido ao menos um acompanhamento previamente
        //            {
        //                if (pOrdemAlterada.OrigClOrdID != null &&
        //                    pOrdemAlterada.OrigClOrdID.Length > 0 &&
        //                    pOrdemAlterada.OrdStatus == OrdemStatusEnum.REJEITADA)
        //                {
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID = pOrdemAlterada.OrigClOrdID;

        //                    if (ordemoriginal != null)
        //                    {
        //                        gLog4Net.Debug("Restaurando ultimo status da ordem [" + ordemoriginal.ClOrdID + "]");

        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].OrigClOrdID = ordemoriginal.OrigClOrdID;
        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].OrdStatus = ordemoriginal.OrdStatus;
        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQtyRemmaining = ordemoriginal.OrderQtyRemmaining;
        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQty = ordemoriginal.OrderQty;
        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].MinQty = ordemoriginal.MinQty;
        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].MaxFloor = ordemoriginal.MaxFloor;
        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQty = ordemoriginal.OrderQty;
        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].Price = ordemoriginal.Price;
        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].ExpireDate = ordemoriginal.ExpireDate;
        //                        gAcompanhamentosEmMemoria[lCodBovespa][a].TransactTime = ordemoriginal.TransactTime;

        //                        pOrdemAlterada.ExpireDate = ordemoriginal.ExpireDate;
        //                        pOrdemAlterada.RegisterTime = ordemoriginal.RegisterTime;
        //                        pOrdemAlterada.TransactTime = DateTime.Now;
        //                        pOrdemAlterada.OrderQtyRemmaining = ordemoriginal.OrderQtyRemmaining;
        //                        pOrdemAlterada.MinQty = ordemoriginal.MinQty;
        //                        pOrdemAlterada.MaxFloor = ordemoriginal.MaxFloor;
                                
        //                    }
        //                    else
        //                    {
        //                        gLog4Net.Error("Nao restaurou status da ordem [" + pOrdemAlterada.OrigClOrdID + "]");
        //                    }
        //                }
        //                else
        //                {
        //                    // Atualiza a ordem
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrdStatus = pOrdemAlterada.OrdStatus;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQtyRemmaining = pOrdemAlterada.OrderQtyRemmaining;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].CumQty = pOrdemAlterada.CumQty;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].TransactTime = pOrdemAlterada.TransactTime;

        //                    // Insere o detalhe/acompanhamento
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.Add(pOrdemAlterada.Acompanhamentos[0]);                        // Adiciona somente o acompanhamento que chegou

        //                    lMensagem = " Acompanhamento de uma ordem que já estava em memória " + lMensagem;
        //                    bAcompanhamentoFound = true;
        //                }

        //                break;
        //            }

        //            // Ordem modificada
        //            if (gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID == pOrdemAlterada.OrigClOrdID)
        //            {
        //                if (pOrdemAlterada.OrdStatus != OrdemStatusEnum.REJEITADA)
        //                {
        //                    // Atualiza a ordem
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].ClOrdID = pOrdemAlterada.ClOrdID;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrigClOrdID = pOrdemAlterada.OrigClOrdID;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrdStatus = pOrdemAlterada.OrdStatus;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQtyRemmaining = pOrdemAlterada.OrderQtyRemmaining;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].CumQty = pOrdemAlterada.CumQty;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].TransactTime = pOrdemAlterada.TransactTime;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQty = pOrdemAlterada.OrderQty;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].Price = pOrdemAlterada.Price;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].ExpireDate = pOrdemAlterada.ExpireDate;
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].TimeInForce = pOrdemAlterada.TimeInForce;


        //                    // Insere o detalhe/acompanhamento
        //                    gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.Add(pOrdemAlterada.Acompanhamentos[0]);                        // Adiciona somente o acompanhamento que chegou

        //                    lMensagem = " Modificacao de uma ordem que já estava em memória" + lMensagem;

        //                    bAcompanhamentoFound = true;
        //                }
        //                //else
        //                //{
        //                //    gLog4Net.Warn("Modificacao [" + pOrdemAlterada.ClOrdID + "] da ordem [" + pOrdemAlterada.OrigClOrdID + "] [" + pOrdemAlterada.OrdStatus.ToString() + "]");
        //                //}

        //                break;
        //            }
        //        }

        //        if (!bAcompanhamentoFound)
        //        {
        //            gAcompanhamentosEmMemoria[lCodBovespa].Add(pOrdemAlterada);

        //            lMensagem = " Acompanhamento de uma ordem que foi incluida em memória agora" + lMensagem;
        //        }

        //        OnMensagemDeAcompanhamento("Acompanhamento: Account [{0}] CodigoBovespa[{1}] CodigoTransacao [{2}] Direcao [{3}] QuantidadeSoliciada [{4}] Instrumento [{5}] Preco [{6}] QuantidadeExecutada [{7}] StatusOrdem [{8}]{9}"
        //                                    , lCodDaOrdem
        //                                    , lCodBovespa
        //                                    , pOrdemAlterada.Acompanhamentos[0].CodigoTransacao
        //                                    , pOrdemAlterada.Acompanhamentos[0].Direcao
        //                                    , pOrdemAlterada.Acompanhamentos[0].QuantidadeSolicitada
        //                                    , pOrdemAlterada.Acompanhamentos[0].Instrumento
        //                                    , pOrdemAlterada.Acompanhamentos[0].Preco.ToString("n")
        //                                    , pOrdemAlterada.Acompanhamentos[0].QuantidadeExecutada
        //                                    , pOrdemAlterada.Acompanhamentos[0].StatusOrdem
        //                                    , lMensagem);
        //    }
        //    catch (Exception ex)
        //    {
        //        gLog4Net.Error("gCallBacker_ChegadaDeAcompanhamento(): " + ex.Message, ex);
        //    }
                
        //}

        void gCallBacker_ChegadaDeAcompanhamento(OrdemInfo pOrdemAlterada)
        {
            try
            {
                lock(queueOrdens)
                {
                    queueOrdens.Enqueue(pOrdemAlterada);
                    semProcOrdens.Release(1);
                }
            }
            catch (Exception ex)
            {
                gLog4Net.Error("gCallBacker_ChegadaDeAcompanhamento(): " + ex.Message, ex);
            }
        }


        private void ordemProcessor()
        {
            while (_bKeepRunning)
            {
                try
                {
                    if (!semProcOrdens.WaitOne(100))
                    {
                        continue;
                    }

                    OrdemInfo [] ordensAlteradas;

                    lock (queueOrdens)
                    {
                        ordensAlteradas = queueOrdens.ToArray();
                        queueOrdens.Clear();
                    }

                    foreach (OrdemInfo ordemAlterada in ordensAlteradas)
                    {
                        processaOrdemAlterada(ordemAlterada);
                    }
                }
                catch (Exception ex)
                {
                    gLog4Net.Error("ordemProcessor(): " + ex.Message, ex);
                }
            }
        }


        void processaOrdemAlterada(OrdemInfo pOrdemAlterada)
        {
            try
            {
                OrdemInfo ordemoriginal = null;

                // Está combinado com o roteador (Ponso) que sempre vem um OrdemInfo com 1 AcompanhamentoInfo; safety check:
                if (pOrdemAlterada.Acompanhamentos.Count == 0)
                {
                    OnMensagemDeAcompanhamento("OrdemInfo para ordem [{0}] está sem acompanhamentos na lista, ignorando..."
                                               , pOrdemAlterada.ClOrdID);

                    return;
                }
                else if (pOrdemAlterada.Acompanhamentos.Count > 1)
                {
                    OnMensagemDeAcompanhamento("OrdemInfo para ordem [{0}] tem [{1}] acompanhamentos, somente um era esperado! O primeiro será gravado, mas os outros serão perdidos!!"
                                               , pOrdemAlterada.ClOrdID
                                               , pOrdemAlterada.Acompanhamentos.Count);


                }

                //string lMensagem = "";

                int lCodDaOrdem = pOrdemAlterada.Account;

                if (bRemoveDigito)
                {
                    if (pOrdemAlterada.Exchange != null && pOrdemAlterada.Exchange.ToUpper() == "BOVESPA")
                        lCodDaOrdem = Convert.ToInt32(pOrdemAlterada.Account.ToString().Substring(0, pOrdemAlterada.Account.ToString().Length - 1)); // Trunca o ultimo digito do codigo do cliente (DV) quando for mercado BOVESPA
                }

                int lCodBovespa = lCodDaOrdem;                                                                                                   //Assume o mais comum, que o código da ordem é o código bovespa
                int lCodBmf;

                string sOrdem = string.Format("[{0}] [{1}] [{2}] [{3}] [{4}] [{5}]",
                        pOrdemAlterada.ClOrdID,
                        pOrdemAlterada.Account,
                        pOrdemAlterada.Symbol,
                        pOrdemAlterada.Side.ToString(),
                        pOrdemAlterada.ChannelID.ToString(),
                        pOrdemAlterada.Price);

                // Clona a info de acompanhamento, para restaurar a ordem corretamente em caso de rejeicao
                AcompanhamentoOrdemInfo infoPersistencia = _cloneAcompanhamentoInfo(pOrdemAlterada.Acompanhamentos[0]);

                if (lstFiltroPorta != null && lstFiltroPorta.Contains(pOrdemAlterada.ChannelID.ToString()))
                {
                    gLog4Net.Warn("Ordem [" + pOrdemAlterada.ClOrdID + "] filtrada para gravacao na base, channel [" + pOrdemAlterada.ChannelID.ToString() + "]");
                }
                else
                {
                    AcompanhamentoOrdemInfo acpt = pOrdemAlterada.Acompanhamentos[0];

                    acpt.Preco = Convert.ToDecimal(pOrdemAlterada.Price);
                     
                    persistirAcompanhamento(acpt);
                }

                // Se foi rejeicao de uma modificacao de ordem, efetua um tratamento diferenciado
                if (pOrdemAlterada.OrigClOrdID != null &&
                    pOrdemAlterada.OrigClOrdID.Length > 0 &&
                    pOrdemAlterada.OrdStatus == OrdemStatusEnum.REJEITADA)
                {
                    gLog4Net.Warn("Modificacao [" + pOrdemAlterada.ClOrdID + "] da ordem [" + pOrdemAlterada.OrigClOrdID + "] rejeitada, efetuando restauro do ultimo status");

                    CamadaDeDados db = new CamadaDeDados();

                    gLog4Net.Debug("Trocando numero de controle");

                    db.TrocarNumeroControleOrdem(pOrdemAlterada);

                    gLog4Net.Debug("Obtendo ordem Original");

                    ordemoriginal = db.BuscarOrdemOriginal(pOrdemAlterada);

                    if (ordemoriginal.TimeInForce == OrdemValidadeEnum.ValidaAteSerCancelada)
                    {
                        ordemoriginal.ExpireDate = null;
                    }

                    gLog4Net.Debug("Atualizando ordem original");

                    db.AtualizarOrdem(ordemoriginal);

                    gLog4Net.Debug("Buscou ordem original [" + ordemoriginal.ClOrdID + "] [" + ordemoriginal.Symbol + "] p=[" + ordemoriginal.Price + "] q=[" + ordemoriginal.OrderQty + "]");
                }


                if (lCodDaOrdem == 0)
                {
                    gLog4Net.Error( sOrdem  + " acompanhamento com codigo de conta zerado, descartando para acompanhamento em memoria" );
                    return;
                }

                if (!gAcompanhamentosEmMemoria.ContainsKey(lCodDaOrdem))                                                                         // Verifica se já tem no dicionário assumindo que a ordem chegou como conta bovespa mesmo
                {
                    foreach (int lCodBov in gCodigosBmfEmMemoria.Keys)
                    {
                        if (gCodigosBmfEmMemoria[lCodBov] == lCodDaOrdem)
                        {
                            lCodBovespa = lCodBov;
                            lCodBmf = lCodDaOrdem;

                            gLog4Net.InfoFormat("{0} ordem executada com código BMF [{1}], mapeada para código bovespa [{2}]", sOrdem, lCodBmf, lCodBovespa);

                            break;
                        }
                    }
                }


                // Se não tem um dicionário para esse código de cliente, adiciona (isso pode acontecer se chegarem ordens de outros sistemas, que não chamaram o InformarLogin)

                if (!gAcompanhamentosEmMemoria.ContainsKey(lCodBovespa))
                {
                    gAcompanhamentosEmMemoria.Add(lCodBovespa, new List<OrdemInfo>());

                    gLog4Net.InfoFormat("{0} lista pra conta do cliente ainda não existia", sOrdem);
                }

                bool bAcompanhamentoFound = false;

                OrdemInfo ordemPosicaoNet = null;
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
                                gLog4Net.Debug("Restaurando ultimo status da ordem [" + ordemoriginal.ClOrdID + "]");

                                //gAcompanhamentosEmMemoria[lCodBovespa][a].OrigClOrdID = ordemoriginal.OrigClOrdID;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrigClOrdID = string.Empty;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrdStatus = ordemoriginal.OrdStatus;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQtyRemmaining = ordemoriginal.OrderQtyRemmaining;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQty = ordemoriginal.OrderQty;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].MinQty = ordemoriginal.MinQty;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].MaxFloor = ordemoriginal.MaxFloor;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQty = ordemoriginal.OrderQty;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].Price = ordemoriginal.Price;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].ExpireDate = ordemoriginal.ExpireDate;
                                gAcompanhamentosEmMemoria[lCodBovespa][a].TransactTime = ordemoriginal.TransactTime;

                                pOrdemAlterada.ExpireDate = ordemoriginal.ExpireDate;
                                pOrdemAlterada.RegisterTime = ordemoriginal.RegisterTime;
                                pOrdemAlterada.TransactTime = DateTime.Now;
                                pOrdemAlterada.OrderQtyRemmaining = ordemoriginal.OrderQtyRemmaining;
                                pOrdemAlterada.MinQty = ordemoriginal.MinQty;
                                pOrdemAlterada.MaxFloor = ordemoriginal.MaxFloor;

                                gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.AddRange(pOrdemAlterada.Acompanhamentos.ToArray());

                            }
                            else
                            {
                                gLog4Net.Error("Nao restaurou status da ordem [" + pOrdemAlterada.OrigClOrdID + "]");
                            }
                        }
                        else
                        {
                            // Atualiza a ordem
                            gAcompanhamentosEmMemoria[lCodBovespa][a].OrdStatus = pOrdemAlterada.OrdStatus;
                            gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQtyRemmaining = pOrdemAlterada.OrderQtyRemmaining;
                            gAcompanhamentosEmMemoria[lCodBovespa][a].CumQty = pOrdemAlterada.CumQty;
                            gAcompanhamentosEmMemoria[lCodBovespa][a].TransactTime = pOrdemAlterada.TransactTime;

                            // Insere o detalhe/acompanhamento
                            gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.Add(pOrdemAlterada.Acompanhamentos[0]);                        // Adiciona somente o acompanhamento que chegou

                            if (pOrdemAlterada.Acompanhamentos[0].StatusOrdem == OrdemStatusEnum.EXECUTADA ||
                                pOrdemAlterada.Acompanhamentos[0].StatusOrdem == OrdemStatusEnum.PARCIALMENTEEXECUTADA)
                            {
                                gAcompanhamentosEmMemoria[lCodBovespa][a].Price = Convert.ToDouble(pOrdemAlterada.Acompanhamentos[0].LastPx);
                            }

                            gLog4Net.Info(sOrdem  + " Acompanhamento de uma ordem que já estava em memória " );

                            ordemPosicaoNet = gAcompanhamentosEmMemoria[lCodBovespa][a];

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
                            gAcompanhamentosEmMemoria[lCodBovespa][a].OrderQty = pOrdemAlterada.OrderQty;
                            gAcompanhamentosEmMemoria[lCodBovespa][a].Price = pOrdemAlterada.Price;
                            gAcompanhamentosEmMemoria[lCodBovespa][a].ExpireDate = pOrdemAlterada.ExpireDate;
                            gAcompanhamentosEmMemoria[lCodBovespa][a].TimeInForce = pOrdemAlterada.TimeInForce;
                            gAcompanhamentosEmMemoria[lCodBovespa][a].StopPrice = pOrdemAlterada.StopPrice;

                            // Insere o detalhe/acompanhamento
                            gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.Add(pOrdemAlterada.Acompanhamentos[0]);                        // Adiciona somente o acompanhamento que chegou

                            if (pOrdemAlterada.Acompanhamentos[0].StatusOrdem == OrdemStatusEnum.EXECUTADA ||
                                pOrdemAlterada.Acompanhamentos[0].StatusOrdem == OrdemStatusEnum.PARCIALMENTEEXECUTADA)
                            {
                                gAcompanhamentosEmMemoria[lCodBovespa][a].Price = Convert.ToDouble(pOrdemAlterada.Acompanhamentos[0].LastPx);
                            }

                            gLog4Net.Info(sOrdem + " Modificacao de uma ordem que já estava em memória");

                            ordemPosicaoNet = gAcompanhamentosEmMemoria[lCodBovespa][a];

                            bAcompanhamentoFound = true;
                        }
                        else
                        {
                            gLog4Net.Warn("Rejeicao da modificacao ou cancelamento [" + pOrdemAlterada.ClOrdID + "] da ordem [" + pOrdemAlterada.OrigClOrdID + "] [" + pOrdemAlterada.OrdStatus.ToString() + "]");
                            gAcompanhamentosEmMemoria[lCodBovespa][a].Acompanhamentos.AddRange(pOrdemAlterada.Acompanhamentos.ToArray());
                            bAcompanhamentoFound = true;
                        }

                        break;
                    }
                }

                if (!bAcompanhamentoFound)
                {
                    gAcompanhamentosEmMemoria[lCodBovespa].Add(pOrdemAlterada);

                    ordemPosicaoNet = pOrdemAlterada;
                    gLog4Net.Info(sOrdem  + " Acompanhamento de uma ordem que foi incluida em memória agora" );
                }

                // Chama o metodo que calculas as posições
                if ( ordemPosicaoNet != null )
                {
                    ProcessarPosicaoNet(lCodBovespa.ToString(), ordemPosicaoNet);
                }

                OnMensagemDeAcompanhamento("Acompanhamento: Account [{0}] CodigoBovespa[{1}] CodigoTransacao [{2}] Direcao [{3}] QuantidadeSoliciada [{4}] Instrumento [{5}] Preco [{6}] QuantidadeExecutada [{7}] StatusOrdem [{8}]{9}"
                                            , lCodDaOrdem
                                            , lCodBovespa
                                            , pOrdemAlterada.Acompanhamentos[0].CodigoTransacao
                                            , pOrdemAlterada.Acompanhamentos[0].Direcao
                                            , pOrdemAlterada.Acompanhamentos[0].QuantidadeSolicitada
                                            , pOrdemAlterada.Acompanhamentos[0].Instrumento
                                            , pOrdemAlterada.Acompanhamentos[0].Preco.ToString("n")
                                            , pOrdemAlterada.Acompanhamentos[0].QuantidadeExecutada
                                            , pOrdemAlterada.Acompanhamentos[0].StatusOrdem
                                            , "****");
            }
            catch (Exception ex)
            {
                gLog4Net.Error("gCallBacker_ChegadaDeAcompanhamento(): " + ex.Message, ex);
            }

        }

        private void persistirAcompanhamento(AcompanhamentoOrdemInfo acompanhamento)
        {
            object lAcompanhamentoID;

            OnMensagemDeAcompanhamento("Acompanhamento CodigoTransacao [{0}] NumeroControleOrdem [{1}] removido da fila para persistência."
                                        , acompanhamento.CodigoTransacao
                                        , acompanhamento.NumeroControleOrdem);   // Mensagem de que o objeto foi retirado da fila pra inclusão

            if (PersistenciaEmArquivo == "sempre")
                SaidaParaComando(GerarComandoDoAcompanhamento(acompanhamento));

            if (gbAtualizarDB)
            {
                try
                {
                    lAcompanhamentoID = gCamadaDePersistencia.InserirAcompanhamentoDeOrdem(acompanhamento);                               // Realiza a inclusão no banco via camada de persistência

                    OnMensagemDeAcompanhamento("Acompanhamento CodigoTransacao [{0}] NumeroControleOrdem [{1}] incluído com sucesso. ID: [{2}]"
                                                , acompanhamento.CodigoTransacao
                                                , acompanhamento.NumeroControleOrdem
                                                , lAcompanhamentoID);                            // Mensagem que o objeto foi incluído com sucesso

                }
                catch (Exception exinc)
                {
                    gLog4Net.Error("Erro ao incluir acompanhamento no banco:" + exinc.Message, exinc);

                    if (PersistenciaEmArquivo == "erro")
                        SaidaParaComando(GerarComandoDoAcompanhamento(acompanhamento));

                }
            }
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Busca ordens com base nos campos de pesquisa em pRequest
        /// </summary>
        /// <param name="pRequest">Dados para pesquisa; nenhum campo é obrigatório, mas todos vazios trazem o histórico inteiro de ordens</param>
        /// <returns></returns>
        public BuscarOrdensResponse BuscarOrdens(BuscarOrdensRequest pRequest)
        {
            BuscarOrdensResponse lResposta = new BuscarOrdensResponse();

            Nullable<int> lStatus = null;

            if (pRequest.Status.HasValue) lStatus = (int)pRequest.Status.Value;

            OnMensagemDeAcompanhamento(" > BuscarOrdens([{0}], [{1}], [{2}], [{3}], [{4}], [{5}], [{6}])"
                                        , pRequest.ContaDoCliente
                                        , pRequest.CodigoBmfDoCliente
                                        , pRequest.DataDe
                                        , pRequest.DataAte
                                        , pRequest.Canal
                                        , pRequest.Instrumento
                                        , lStatus);

            lResposta.Ordens = gCamadaDeConsulta.BuscarOrdens(pRequest.ContaDoCliente
                                                              , pRequest.CodigoBmfDoCliente
                                                              , pRequest.DataDe
                                                              , pRequest.DataAte
                                                              , pRequest.Canal
                                                              , pRequest.Instrumento
                                                              , lStatus
                                                              , pRequest.CodigoAssessor);

            lResposta.StatusResposta = Library.MensagemResponseStatusEnum.OK;
            
            OnMensagemDeAcompanhamento(" < BuscarOrdens: [{0}], [{1}] Ordens encontradas"
                                        , lResposta.StatusResposta 
                                        , lResposta.Ordens.Count);

            return lResposta;
        }

        /// <summary>
        /// Verifica o status das ordens com base nos dados em memória; 
        /// é mais eficiente que a busca (não vai no banco) mas só vale para ordens do dia.
        /// </summary>
        /// <param name="pRequest">Dados para buscar as ordens; o número da conta do cliente é obrigatório.</param>
        /// <returns>Response com a lista de ordens do dia</returns>
        public VerificarStatusDasOrdensResponse VerificarStatusDasOrdens(VerificarStatusDasOrdensRequest pRequest)
        {
            VerificarStatusDasOrdensResponse lResposta = new VerificarStatusDasOrdensResponse();

            string lMensagem = "";

            OnMensagemDeAcompanhamento(" > VerificarStatusDasOrdens([{0}, {1}])    //CodBovespa, CodBmf"
                                        , pRequest.ContaDoCliente
                                        , pRequest.CodigoBmfDoCliente);

            if (pRequest.ContaDoCliente < 1)
            {
                lResposta.Criticas.Add(new Library.CriticaInfo()
                                        {
                                              Descricao = "Campo 'Código do Cliente' obrigatório"
                                            , Status = Library.CriticaStatusEnum.ErroNegocio
                                        });

                lResposta.StatusResposta = Library.MensagemResponseStatusEnum.ErroValidacao;
            }
            
            if (pRequest.CodigoBmfDoCliente < 1)
            {
                lResposta.Criticas.Add(new Library.CriticaInfo()
                                        {
                                              Descricao = "Campo 'Código BMF do Cliente' obrigatório"
                                            , Status = Library.CriticaStatusEnum.ErroNegocio
                                        });

                lResposta.StatusResposta = Library.MensagemResponseStatusEnum.ErroValidacao;
            }

            if (!gAcompanhamentosEmMemoria.ContainsKey(pRequest.ContaDoCliente))
            {
                //Aqui nunca deveria entrar porque o InformarLogindoCliente sempre põe ele na memória, mas vai just-in-case:
                AdicionarOrdensDeHojeDoClienteAosAcompanhamentosEmMemoria(pRequest.ContaDoCliente, pRequest.CodigoBmfDoCliente);

                lMensagem = " (a conta desse cliente não estava em memória, foi incluida)";
            }

            if (!gCodigosBmfEmMemoria.ContainsKey(pRequest.ContaDoCliente))
            {
                gCodigosBmfEmMemoria.Add(pRequest.ContaDoCliente, pRequest.CodigoBmfDoCliente);

                lMensagem = " (o código BMF desse cliente não estava em memória, foi incluido)";
            }

            lResposta.Ordens = gAcompanhamentosEmMemoria[pRequest.ContaDoCliente];

            lResposta.StatusResposta = Library.MensagemResponseStatusEnum.OK;
            
            OnMensagemDeAcompanhamento(" < VerificarStatusDasOrdens: [{0}], [{1}] Ordens encontradas{2}"
                                        , lResposta.StatusResposta 
                                        , lResposta.Ordens.Count
                                        , lMensagem);

            // ATP: 
            //foreach (OrdemInfo ordem in lResposta.Ordens)
            //{
            //    DumpOrdemInfo(ordem);
            //}

            return lResposta;
        }

        /// <summary>
        /// Informa ao serviço de acompanhamento que um cliente realizou login no sistema, resgatando suas ordens do dia do banco e armazenando em memória
        /// </summary>
        /// <param name="pRequest">Informações da requisição, conta do cliente obrigatória</param>
        /// <returns>OK</returns>
        public InformarLoginDoClienteResponse InformarLoginDoCliente(InformarLoginDoClienteRequest pRequest)
        {
            InformarLoginDoClienteResponse lResposta = new InformarLoginDoClienteResponse();
            
            OnMensagemDeAcompanhamento(" > InformarLoginDoCliente([{0}])"
                                        , pRequest.ContaDoCliente);

            gListaDeUsuariosDeslogados.Remove(pRequest.ContaDoCliente);                                                                      // Caso ele estivesse na lista de usuários deslogados, tira (ele saiu e entrou novamente antes dos 10 minutos)

            lock (gAcompanhamentosEmMemoria)
            {
                if (!gAcompanhamentosEmMemoria.ContainsKey(pRequest.ContaDoCliente))                                                             // Pode ser que a chave desse cliente ainda exista por causa da tolerância de 10 min pra tirar da memória
                {
                    VerificarListaDeUsuariosDeslogadosParaLiberarMemoria();                                                                      // Já que incluiu um, verifica se pode liberar espaço
                }
                else
                {
                    OnMensagemDeAcompanhamento("Usuário [{0}] ainda está com cache em memória, foi removido da lista de logout."
                                               , pRequest.ContaDoCliente);
                    
                    gAcompanhamentosEmMemoria.Remove(pRequest.ContaDoCliente);
                }

                lock (ListaPosicoes)
                {
                    if (ListaPosicoes.ContainsKey(pRequest.ContaDoCliente.ToString()))
                    {
                        ListaPosicoes.Remove(pRequest.ContaDoCliente.ToString());
                    }
                }
                AdicionarOrdensDeHojeDoClienteAosAcompanhamentosEmMemoria(pRequest.ContaDoCliente, pRequest.CodigoBmfDoCliente);             // Adiciona as ordens de hoje desse cliente à memória
            }

            lResposta.StatusResposta = Library.MensagemResponseStatusEnum.OK;

            OnMensagemDeAcompanhamento(" < InformarLoginDoCliente: [{0}]", lResposta.StatusResposta);

            return lResposta;
        }

        /// <summary>
        /// Informa ao serviço de acompanhamento que um cliente realizou logout do sistema, marcando seus dados como passíveis de remoção da memória
        /// </summary>
        /// <param name="pRequest">Informações da requisição, conta do cliente obrigatória</param>
        /// <returns>OK</returns>
        public InformarLogoutDoClienteResponse InformarLogoutDoCliente(InformarLogoutDoClienteRequest pRequest)
        {
            InformarLogoutDoClienteResponse lResposta = new InformarLogoutDoClienteResponse();
            
            OnMensagemDeAcompanhamento(" > InformarLogoutDoCliente([{0}])"
                                        , pRequest.ContaDoCliente);

            gListaDeUsuariosDeslogados.Add(pRequest.ContaDoCliente, DateTime.Now.AddMinutes(1));                                     // Tolerância de 10 minutos pra deixar o cara em memória ainda, caso volte

            OnMensagemDeAcompanhamento("Usuário [{0}] adicionado à lista de logout, será removido em [{1}]."
                                       , pRequest.ContaDoCliente
                                       , gListaDeUsuariosDeslogados[pRequest.ContaDoCliente].ToString("HH:mm:ss"));

            lResposta.StatusResposta = Library.MensagemResponseStatusEnum.OK;
            
            OnMensagemDeAcompanhamento(" < InformarLogoutDoCliente: [{0}]", lResposta.StatusResposta);

            return lResposta;
        }

        /// <summary>
        /// Imprime o conteudo dos campos do objeto OrdemInfo
        /// </summary>
        /// <param name="ordem">objeto OrdemInfo</param>
        public static void DumpOrdemInfo(OrdemInfo ordem)
        {
            if (gLog4Net.IsDebugEnabled)
            {
                gLog4Net.Debug("Account ...........: " + ordem.Account);
                gLog4Net.Debug("ID da Ordem (DB) ..: " + ordem.IdOrdem);
                gLog4Net.Debug("Numero da ordem ...: " + ordem.ClOrdID);
                gLog4Net.Debug("ExchangeNumber ....: " + ordem.ExchangeNumberID);
                gLog4Net.Debug("Bolsa .............: " + ordem.Exchange);
                gLog4Net.Debug("Operador...........: " + ordem.ChannelID);
                gLog4Net.Debug("Symbol ............: " + ordem.Symbol);
                gLog4Net.Debug("Status ............: " + ordem.OrdStatus);
                gLog4Net.Debug("Side ..............: " + ordem.Side);
                gLog4Net.Debug("TimeInForce .......: " + ordem.TimeInForce);
                gLog4Net.Debug("TransactTime ......: " + ordem.TransactTime.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
                gLog4Net.Debug("OrderType .........: " + ordem.OrdType.ToString());
                gLog4Net.Debug("Qtde  .............: " + ordem.OrderQty);
                gLog4Net.Debug("Qtde Pendente .....: " + ordem.OrderQtyRemmaining);
                gLog4Net.Debug("Qtde Exec Total ...: " + ordem.CumQty);
                gLog4Net.Debug("Preco .............: " + ordem.Price);
                gLog4Net.Debug("Stop Price ........: " + ordem.StopPrice);
                gLog4Net.Debug("Register Time  ....: " + ordem.RegisterTime);
                gLog4Net.Debug("Qtde Aparente .....: " + ordem.MaxFloor);
                gLog4Net.Debug("Qtde minima .......: " + ordem.MinQty);
                gLog4Net.Debug("Expiration Date ...: " + ordem.ExpireDate);

                if (ordem.Acompanhamentos != null)
                {
                    int i = 0;
                    foreach (AcompanhamentoOrdemInfo acomp in ordem.Acompanhamentos)
                    {
                        gLog4Net.Debug("Acompanhamento : " + i);

                        gLog4Net.Debug("(A) DataAtualizacao ......: " + acomp.DataAtualizacao.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
                        gLog4Net.Debug("(A) CodigoRejeicao .......: " + acomp.CodigoRejeicao);
                        gLog4Net.Debug("(A) Descricao ............: " + acomp.Descricao);
                        gLog4Net.Debug("(A) Qtde Exc .............: " + acomp.QuantidadeExecutada);
                        gLog4Net.Debug("(A) Qtde Solicitada ......: " + acomp.QuantidadeSolicitada);
                        gLog4Net.Debug("(A) Qtde Negociada  ......: " + acomp.QuantidadeNegociada);
                        gLog4Net.Debug("(A) Qtde Remanescente ....: " + acomp.QuantidadeRemanescente);
                        gLog4Net.Debug("(A) Status Ordem .........: " + acomp.StatusOrdem);
                        gLog4Net.Debug("(A) NumeroControleOrdem ..: " + acomp.NumeroControleOrdem);
                        gLog4Net.Debug("(A) CodigoResposta .......: " + acomp.CodigoResposta);
                        gLog4Net.Debug("(A) Data Envio Ordem   ...: " + acomp.DataOrdemEnvio.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
                        gLog4Net.Debug("(A) Data Validade ........: " + acomp.DataValidade.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));

                        i++;
                    }
                }
            } //if (logger.IsDebugEnabled)
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (gStreamDeLog != null)
                {
                    gStreamDeLog.Close();

                    gStreamDeLog.Dispose();
                }

                if (gStreamDeComandos != null)
                {
                    gStreamDeComandos.Close();

                    gStreamDeComandos.Dispose();
                }

                //gCamadaDeConsulta.FecharConexao();

                //gCamadaDePersistencia.FecharConexao();
            }
            catch { }
        }

        #endregion

        #region IServicoControlavel Members

        /// <summary>
        /// Invocado pelo hoster durante a inicializacao do servico
        /// </summary>
        public void IniciarServico()
        {
            gLog4Net.Info("Inicializando servico de acompanhamento");

            string strAtualizaDB = (string)ConfigurationManager.AppSettings["AtualizarBancoDados"];

            if ( strAtualizaDB != null )
                gbAtualizarDB = Convert.ToBoolean(strAtualizaDB);
            
            if ( gbAtualizarDB )
                gLog4Net.Info("Gravacao das atualizações em base de dados HABILITADA!!");
            else
                gLog4Net.Info("Gravacao das atualizações em base de dados DESABILITADA.");

            //ATP 2015-10-10
            if (ConfigurationManager.AppSettings["AccountStripDigit"] != null)
            {
                bRemoveDigito = ConfigurationManager.AppSettings["AccountStripDigit"].ToString().ToLower().Equals("true");

                gLog4Net.WarnFormat("REMOCAO DO DIGITO BOVESPA ESTA {0}", bRemoveDigito ? "HABILITADO" : "DESABILITADO");
            }


            CamadaDeDados camada = new CamadaDeDados();
            if (ConfigurationManager.ConnectionStrings["DirectTradeCadastro"] != null)
            {
                gLog4Net.Info("Carregando lista de contas BMF diferentes de Bovespa para mapeamento");
                
                Dictionary<int, int> contasBMFDiferentes = camada.BuscarCodigoBovespaBMFDiferentes();
                foreach (int key in contasBMFDiferentes.Keys)
                {
                    if (!gCodigosBmfEmMemoria.ContainsKey(key))
                    {
                        gLog4Net.Info("Incluindo mapeamento de Bov[" + key + "] para Bmf[" + contasBMFDiferentes[key] + "]");
                        gCodigosBmfEmMemoria.Add(key, contasBMFDiferentes[key]);
                    }
                }
            }
            else
            {
                gLog4Net.Error("NAO FOI POSSIVEL CARREGAR LISTA DE CONTAS BMF DIFERENTES DE BOVESPA PARA MAPEAMENTO");
            }

            gLog4Net.Info("Carga das ordens ativas e posicao NET");

            // Carga da posicao inicial
            List<OrdemInfo> ordensAtivas = camada.BuscarOrdensAtivas();

            foreach (OrdemInfo ordem in ordensAtivas)
            {
                int lCodBovespa = ordem.Account;

                // Obtem cod bovespa caso seja ordem BMF com cod conta diferente
                if (!gCodigosBmfEmMemoria.ContainsKey(lCodBovespa))
                {
                    foreach (int lCodBov in gCodigosBmfEmMemoria.Keys)
                    {
                        if (gCodigosBmfEmMemoria[lCodBov] == ordem.Account)
                        {
                            lCodBovespa = lCodBov;
                            break;
                        }
                    }
                }

                if (!gAcompanhamentosEmMemoria.ContainsKey(lCodBovespa))
                {
                    gAcompanhamentosEmMemoria.Add(lCodBovespa, new List<OrdemInfo>());
                }

                gAcompanhamentosEmMemoria[lCodBovespa].Add(ordem);
            }

            ListaPosicoes = new Dictionary<string, Dictionary<string, Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento>>();

            foreach (int lCodBovespa in gAcompanhamentosEmMemoria.Keys)
            {
                ProcessarPosicaoNet(lCodBovespa.ToString(), gAcompanhamentosEmMemoria[lCodBovespa]);
            }

            if ( ConfigurationManager.AppSettings["FiltrarPortas"] != null )
            {
                string portasFiltradasDB = ConfigurationManager.AppSettings["FiltrarPortas"].ToString();

                lstFiltroPorta.AddRange(portasFiltradasDB.Split(';'));
            }

            ultimodiacarregado = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd"));

            AbrirArquivosDeSaida();         

            IniciarThreadDeCallBack();

            IniciarThreadDePersistencia();

            thProcOrdens = new Thread(new ThreadStart(ordemProcessor));
            thProcOrdens.Name = "ordemProcessor";
            thProcOrdens.Start();


            _status = ServicoStatus.EmExecucao;            
        }


        /// <summary>
        /// Invocado pelo Hoster durante a finalizacao deste servico
        /// </summary>
        public void PararServico()
        {
            gLog4Net.Info("Finalizando servico de acompanhamento");

            // sinaliza parada da thread
            _bKeepRunning = false;

            try
            {
                gLog4Net.Info("Fechando stream de logs");
                if (gStreamDeLog != null)
                {
                    gStreamDeLog.Close();

                    gStreamDeLog.Dispose();
                }

                gLog4Net.Info("Fechando stream de comandos");
                if (gStreamDeComandos != null)
                {
                    gStreamDeComandos.Close();

                    gStreamDeComandos.Dispose();
                }

                //gLog4Net.Info("Fechando conexoes da camada de consulta");
                //gCamadaDeConsulta.FecharConexao();

                //gLog4Net.Info("Fechando conexoes da camada de persistencia");
                //gCamadaDePersistencia.FecharConexao();

                gLog4Net.Info("Finalizando threads de monitoracao de callbacks e persistencias");
                while (this.gThreadDeCallBack.IsAlive)
                {
                    gLog4Net.Info("Aguardando finalizacao da thread de callbacks do roteador");
                    Thread.Sleep(250);
                }

                while (this.gThreadDePersistencia.IsAlive)
                {
                    gLog4Net.Info("Aguardando finalizacao da thread de persistencias");
                    Thread.Sleep(250);
                }
            }
            catch(Exception ex)
            {
                gLog4Net.Error("Erro em PararServico()" + ex.Message, ex);
            }

            _status = ServicoStatus.Parado;

            gLog4Net.Info("Servico finalizado");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        #endregion //IServicoControlavel Members

        #region IServicoAcompanhamentoOrdens Members

        /// <summary>
        /// Busca ordens do tipo StopStar
        /// </summary>
        /// <param name="pRequest">Entidade do tipo BuscarOrdensStopStartRequest</param>
        /// <returns></returns>
        public BuscarOrdensStopStartResponse BuscarOrdensStopStart(BuscarOrdensStopStartRequest pRequest)
        {
            BuscarOrdensStopStartResponse lResposta = new BuscarOrdensStopStartResponse();

            try
            {
                OnMensagemDeAcompanhamento(" > BuscarOrdensStopStart([{0}], [{1}], [{2}], [{3}], [{4}], [{5}], [{6}])"
                                            , pRequest.Account
                                            , pRequest.CodBmf
                                            , pRequest.DataDe
                                            , pRequest.DataAte
                                            , pRequest.Symbol
                                            , pRequest.OrderStatusId
                                            , pRequest.CodigoAssessor);

                lResposta.OrdensStartStop = gCamadaDeConsulta.BuscarOrdensStopStart(pRequest);

                lResposta.StatusResposta = Library.MensagemResponseStatusEnum.OK;
                
                OnMensagemDeAcompanhamento(" < BuscarOrdensStopStart: [{0}], [{1}] Ordens encontradas"
                                            , lResposta.StatusResposta 
                                            , lResposta.OrdensStartStop.Count);
            }
            catch (Exception ex)
            {
                lResposta.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
                gLog4Net.Error(string.Format("Message:{0} \nStackTrace - {1}", ex.Message, ex.StackTrace));
            }

            return lResposta;
        }

        #endregion

        ///// <summary>
        ///// Imprime o conteudo dos campos do objeto OrdemInfo
        ///// </summary>
        ///// <param name="ordem">objeto OrdemInfo</param>
        //public static void DumpOrdemInfo(OrdemInfo ordem)
        //{
        //    if (gLog4Net.IsDebugEnabled)
        //    {
        //        gLog4Net.Debug("Account ...........: " + ordem.Account);
        //        gLog4Net.Debug("ID da Ordem (DB) ..: " + ordem.IdOrdem);
        //        gLog4Net.Debug("Numero da ordem ...: " + ordem.ClOrdID);
        //        gLog4Net.Debug("ExchangeNumber ....: " + ordem.ExchangeNumberID);
        //        gLog4Net.Debug("Bolsa .............: " + ordem.Exchange);
        //        gLog4Net.Debug("Operador...........: " + ordem.ChannelID);
        //        gLog4Net.Debug("Symbol ............: " + ordem.Symbol);
        //        gLog4Net.Debug("Status ............: " + ordem.OrdStatus);
        //        gLog4Net.Debug("Side ..............: " + ordem.Side);
        //        gLog4Net.Debug("TimeInForce .......: " + ordem.TimeInForce);
        //        gLog4Net.Debug("TransactTime ......: " + ordem.TransactTime.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
        //        gLog4Net.Debug("OrderType .........: " + ordem.OrdType.ToString());
        //        gLog4Net.Debug("Qtde  .............: " + ordem.OrderQty);
        //        gLog4Net.Debug("Qtde Pendente .....: " + ordem.OrderQtyRemmaining);
        //        gLog4Net.Debug("Preco .............: " + ordem.Price);
        //        gLog4Net.Debug("Register Time  ....: " + ordem.RegisterTime);
        //        gLog4Net.Debug("Qtde Aparente .....: " + ordem.MaxFloor);
        //        gLog4Net.Debug("Qtde minima .......: " + ordem.MinQty);
        //        gLog4Net.Debug("Expiration Date ...: " + ordem.ExpireDate);

        //        if (ordem.Acompanhamentos != null)
        //        {
        //            int i = 0;
        //            foreach (AcompanhamentoOrdemInfo acomp in ordem.Acompanhamentos)
        //            {
        //                gLog4Net.Debug("Acompanhamento : " + i);

        //                gLog4Net.Debug("(A) DataAtualizacao ......: " + acomp.DataAtualizacao.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
        //                gLog4Net.Debug("(A) CodigoRejeicao .......: " + acomp.CodigoRejeicao);
        //                gLog4Net.Debug("(A) Descricao ............: " + acomp.Descricao);
        //                gLog4Net.Debug("(A) Qtde Exc .............: " + acomp.QuantidadeExecutada);
        //                gLog4Net.Debug("(A) Qtde Solicitada ......: " + acomp.QuantidadeSoliciada);
        //                gLog4Net.Debug("(A) Status Ordem .........: " + acomp.StatusOrdem);
        //                gLog4Net.Debug("(A) NumeroControleOrdem ..: " + acomp.NumeroControleOrdem);
        //                gLog4Net.Debug("(A) CodigoResposta .......: " + acomp.CodigoResposta);
        //                gLog4Net.Debug("(A) Data Envio Ordem   ...: " + acomp.DataOrdemEnvio.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
        //                gLog4Net.Debug("(A) Data Validade ........: " + acomp.DataValidade.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));

        //                i++;
        //            }
        //        }
        //    } //if (logger.IsDebugEnabled)
        //}

        #region IServicoAcompanhamentoOrdens Members
        public BuscarOrdensResponse BuscarOrdensHistorico(BuscarOrdensRequest pRequest)
        {
            BuscarOrdensResponse lResposta = new BuscarOrdensResponse();

            Nullable<int> lStatus = null;

            if (pRequest.Status.HasValue) lStatus = (int)pRequest.Status.Value;

            lResposta = gCamadaDeConsulta.BuscarOrdensHistorico( pRequest.ContaDoCliente
                                                                      , pRequest.CodigoBmfDoCliente
                                                                      , pRequest.DataDe
                                                                      , pRequest.DataAte
                                                                      , pRequest.Canal
                                                                      , pRequest.Instrumento
                                                                      , lStatus
                                                                      , pRequest.CodigoAssessor
                                                                      , pRequest.TotalRegistros
                                                                      , pRequest.PaginaCorrente.Value
                                                                      );

            lResposta.StatusResposta = Library.MensagemResponseStatusEnum.OK;

            OnMensagemDeAcompanhamento(" < BuscarOrdensHistorico: [{0}], [{1}] Ordens encontradas"
                                        , lResposta.StatusResposta
                                        , lResposta.Ordens.Count);

            return lResposta;
        }
        /// <summary>
        /// Busca ordens no Sinacor
        /// </summary>
        /// <param name="pRequest">Request do Tipo de busca de Ordens</param>
        /// <returns>Retorna uma lista de Ordens</returns>
        public BuscarOrdensResponse BuscarOrdensSinacor(BuscarOrdensRequest pRequest)
        {
            BuscarOrdensResponse lReturn = new BuscarOrdensResponse();
            
            try
            {
                CamadaDeDados lDados = new CamadaDeDados();

                lReturn = lDados.BuscarOrdensSinacor(pRequest);

                gLog4Net.Info(string.Format("Entrou no Método BuscarOrdensSinacor e retornou {0} ordens", lReturn.Ordens.Count));

            }
            catch (Exception ex)
            {
                gLog4Net.Error(string.Format("Erro:{0}, StackTrace: {1}", ex.Message, ex.StackTrace));
            }
            return lReturn;
        }

        /// <summary>
        /// Busca ordens no Sinacor
        /// </summary>
        /// <param name="pRequest">Request do Tipo de busca de Ordens de BMF</param>
        /// <returns>Retorna uma lista de Ordens BMF</returns>
        public BuscarOrdensResponse BuscarOrdensSinacorBmf(BuscarOrdensRequest pRequest)
        {
            BuscarOrdensResponse lReturn = new BuscarOrdensResponse();

            try
            {
                CamadaDeDados lDados = new CamadaDeDados();

                lReturn = lDados.BuscarOrdensSinacorBmf(pRequest);

                gLog4Net.Info(string.Format("Entrou no Método BuscarOrdensSinacorBmf e retornou {0} ordens", lReturn.Ordens.Count));

            }
            catch (Exception ex)
            {
                gLog4Net.Error(string.Format("Erro:{0}, StackTrace: {1}", ex.Message, ex.StackTrace));
            }
            return lReturn;
        }

        public BuscarOrdemResponse BuscarOrdem(BuscarOrdemRequest pRequest)
        {
            BuscarOrdemResponse lReturn = new BuscarOrdemResponse();

            try
            {
                CamadaDeDados lDados = new CamadaDeDados();

                lReturn = lDados.BuscarOrdem(pRequest.ClOrdID);

                gLog4Net.Info(string.Format("Entrou no Método BuscarOrdem"));

            }
            catch (Exception ex)
            {
                gLog4Net.Error(string.Format("Erro:{0}, StackTrace: {1}", ex.Message, ex.StackTrace));
            }
            return lReturn;
        }


        #endregion

        public AcompanhamentoOrdemInfo _cloneAcompanhamentoInfo(AcompanhamentoOrdemInfo orig)
        {
            AcompanhamentoOrdemInfo clone = new AcompanhamentoOrdemInfo();

            clone.CanalNegociacao = orig.CanalNegociacao;
            clone.CodigoDoCliente = orig.CodigoDoCliente;
            clone.CodigoRejeicao = orig.CodigoRejeicao;
            clone.CodigoResposta = orig.CodigoResposta;
            clone.CodigoTransacao = orig.CodigoTransacao;
            clone.DataAtualizacao = orig.DataAtualizacao;
            clone.DataOrdemEnvio = orig.DataOrdemEnvio;
            clone.DataValidade = orig.DataValidade;
            clone.Descricao = orig.Descricao;
            clone.Direcao = orig.Direcao;
            clone.FixMsgSeqNum = orig.FixMsgSeqNum;
            clone.Instrumento = orig.Instrumento;
            clone.LastPx = orig.LastPx;
            clone.NumeroControleOrdem = orig.NumeroControleOrdem;
            clone.Preco = orig.Preco;
            clone.QuantidadeExecutada = orig.QuantidadeExecutada;
            clone.QuantidadeNegociada = orig.QuantidadeNegociada;
            clone.QuantidadeRemanescente = orig.QuantidadeRemanescente;
            clone.QuantidadeSolicitada = orig.QuantidadeSolicitada;
            clone.SecurityID = orig.SecurityID;
            clone.StatusOrdem = orig.StatusOrdem;
            clone.TradeDate = orig.TradeDate;
            clone.UniqueTradeID = orig.UniqueTradeID;

            return clone;
        }
        
        private void ProcessarPosicaoNet(String CodigoBovespa, List<OrdemInfo> e)
        {
            foreach (OrdemInfo ordem in e)
            {
                if (ordem.Acompanhamentos.Count >= 0 )
                {
                    ProcessarPosicaoNet(CodigoBovespa, ordem);
                }
            }
        }
        private void ProcessarPosicaoNet(String CodigoBovespa, OrdemInfo e)
        {
            int lCodBovespa = Int32.Parse(CodigoBovespa);                                                                                                   //Assume o mais comum, que o código da ordem é o código bovespa
            int lCodBmf;

            try
            {
                if (!gAcompanhamentosEmMemoria.ContainsKey(Int32.Parse(CodigoBovespa)))
                {
                    if (gCodigosBmfEmMemoria.ContainsKey(lCodBovespa))
                    {
                        lCodBmf = gCodigosBmfEmMemoria[lCodBovespa];
                    }
                    else
                    {
                        lCodBmf = lCodBovespa;
                    }

                    //Aqui nunca deveria entrar porque o InformarLogindoCliente sempre põe ele na memória, mas vai just-in-case:
                    gLog4Net.InfoFormat("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Cliente {0} não possui dados em memória. Efetuando requisição para carregar dados em memória.", lCodBovespa);
                    AdicionarOrdensDeHojeDoClienteAosAcompanhamentosEmMemoria(lCodBovespa, lCodBmf);
                }

                if (e.OrdStatus.Equals(OrdemStatusEnum.REJEITADA))
                {
                    return;
                }

                if (ListaPosicoes == null)
                {
                    gLog4Net.Info("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Lista de posições não existe em memória. Criando a lista de Posições.");
                    ListaPosicoes = new Dictionary<string, Dictionary<string, Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento>>();
                }

                if (e.Acompanhamentos.Count != 0)
                {
                    if (ListaPosicoes.ContainsKey(CodigoBovespa))
                    {
                        if (ListaPosicoes[CodigoBovespa].ContainsKey(e.Symbol))
                        {
                            gLog4Net.InfoFormat("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Cliente {0} já possui dados em memória para {1}.", CodigoBovespa, e.Symbol.ToString());
                            ListaPosicoes[CodigoBovespa][e.Symbol].Add((OrdemInfo)e);
                        }
                        else
                        {
                            gLog4Net.InfoFormat("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Cliente {0} não possui posição para {1}", CodigoBovespa, e.Symbol.ToString());
                            Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento acompanhamento = new Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento();
                            acompanhamento.Instrumento = e.Symbol;
                            acompanhamento.Add(e);
                            ListaPosicoes[CodigoBovespa].Add(e.Symbol, acompanhamento);
                            gLog4Net.InfoFormat("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Posição de {0} criada para o cliente {1}", e.Symbol.ToString(), CodigoBovespa);
                        }
                    }
                    else
                    {
                        gLog4Net.InfoFormat("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Cliente {0} não existe em memória", CodigoBovespa);
                        Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento acompanhamento = new Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento();

                        acompanhamento.Instrumento = e.Symbol;
                        gLog4Net.InfoFormat("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Adicionando o Cliente {0} pois o mesmo não existe em memória", CodigoBovespa);
                        acompanhamento.Add(e);

                        Dictionary<System.String, Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento> ocorrencias = new Dictionary<string, Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento>();

                        gLog4Net.InfoFormat("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Adicionando posição para o Cliente {0} pois o mesmo não possui posição em memória", CodigoBovespa);
                        ocorrencias.Add(e.Symbol, acompanhamento);

                        ListaPosicoes.Add(CodigoBovespa, ocorrencias);

                        gLog4Net.InfoFormat("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Cliente {0} criado em memória", CodigoBovespa);
                    }
                }
                else
                {
                    gLog4Net.InfoFormat("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Ordem não possui acompanhamentos", CodigoBovespa);
                }
            }
            catch (Exception ex)
            {
                gLog4Net.Error(string.Format("ServicoAcompanhamentoOrdens.ProcessarPosicaoNet(): Erro:{0}, StackTrace: {1}", ex.Message, ex.StackTrace));
            }
        }

        public BuscarPosicaoNetResponse BuscarPosicaoNet(BuscarPosicaoNetRequest pRequest)
        {
            gLog4Net.DebugFormat("ServicoAcompanhamentoOrdens.BuscarPosicaoNet(): Solicitação de PosiçãoNet recebida para o Cliente {0}", pRequest.ContaDoCliente.ToString());
            BuscarPosicaoNetResponse lReturn = new BuscarPosicaoNetResponse();
            try
            {
                if (ListaPosicoes.ContainsKey(pRequest.ContaDoCliente.ToString()))
                {
                    gLog4Net.DebugFormat("ServicoAcompanhamentoOrdens.BuscarPosicaoNet(): Foi encontrada posição para o Cliente {0} em memória", pRequest.ContaDoCliente.ToString());
                    if (ListaPosicoes[pRequest.ContaDoCliente.ToString()].Count > 0)
                    {

                        gLog4Net.DebugFormat("ServicoAcompanhamentoOrdens.BuscarPosicaoNet(): Foram encontradas {0} posição(ões) para o Cliente {1} em memória", ListaPosicoes[pRequest.ContaDoCliente.ToString()].Count, pRequest.ContaDoCliente.ToString());

                        foreach (KeyValuePair<string, Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento> pair in ListaPosicoes[pRequest.ContaDoCliente.ToString()])
                        {
                            lReturn.Posicao.Add(((Gradual.OMS.AcompanhamentoOrdens.Lib.PosicaoNet.Acompanhamento)pair.Value).Posicao);
                        }
                    }
                    return lReturn;
                }

                gLog4Net.DebugFormat("ServicoAcompanhamentoOrdens.BuscarPosicaoNet(): Não existe posição para o Cliente {0} em memória", pRequest.ContaDoCliente.ToString());

            }
            catch (Exception ex)
            {
                gLog4Net.Error(string.Format("ServicoAcompanhamentoOrdens.BuscarPosicaoNet() Erro:{0}, StackTrace: {1}", ex.Message, ex.StackTrace));
            }
            return lReturn;
        }

        void GravaInfosOrdens(OrdemInfo Ordem)
        {
            System.Text.StringBuilder infos = new System.Text.StringBuilder();

            infos.Append("\r\n***************Ordem Recebida do acompanhamento ordens***********\r\n");
            infos.AppendFormat("******CLORDID...........: {0}\r\n", Ordem.ClOrdID);
            infos.AppendFormat("******ID................: {0}\r\n", Ordem.IdOrdem);
            infos.AppendFormat("******CLIENTE...........: {0}\r\n", Ordem.Account);
            infos.AppendFormat("******PAPEL.............: {0}\r\n", Ordem.Symbol);
            infos.AppendFormat("******SIDE..............: {0}\r\n", Ordem.Side);
            infos.AppendFormat("******REGISTERTIME......: {0}\r\n", Ordem.RegisterTime);
            infos.AppendFormat("******PRICE.............: {0}\r\n", Ordem.Price);
            infos.AppendFormat("******STATUS............: {0}\r\n", Ordem.OrdStatus);
            infos.AppendFormat("******QUANTIDADE........: {0}\r\n", Ordem.OrderQty);
            infos.AppendFormat("******ORDERQTYREMMAINING: {0}\r\n", Ordem.OrderQtyRemmaining);
            infos.AppendFormat("******ORIGCLORDID.......: {0}\r\n", Ordem.OrigClOrdID);
            infos.AppendFormat("******CANAL.............: {0}\r\n", Ordem.ChannelID);
            infos.AppendFormat("******CUMQTY............: {0}\r\n", Ordem.CumQty);
            infos.AppendFormat("******EXPIRAÇÃO.........: {0}\r\n", Ordem.ExpireDate);
            infos.AppendFormat("******FIXMSGSEQNUM......: {0}\r\n", Ordem.FixMsgSeqNum);
            infos.AppendFormat("******EXCHANGENUMBERID..: {0}\r\n", Ordem.ExchangeNumberID);
            infos.AppendFormat("**********************************************************************");

            gLog4Net.Info(infos.ToString());
        }

    }

    /// <summary>
    /// Implementacao dos callbacks invocados pelo Roteador de Ordens
    /// </summary>
    public class OrdemAlteradaCallBack : IRoteadorOrdensCallback
    {
        private  readonly ILog gLog4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Timestamp da ultima mensagem recebida do Roteador
        /// </summary>
        public long Timestamp { get; set; }

        public OrdemAlteradaCallBack()
        {
            Timestamp = _getSecsFromTicks(DateTime.Now.Ticks);
        }
        #region Eventos

        public event ChegadaDeAcompanhamentoHandler ChegadaDeAcompanhamento;

        private void OnChegadaDeAcompanhamento(OrdemInfo pAlteracao)
        {
            if (ChegadaDeAcompanhamento != null)
                ChegadaDeAcompanhamento(pAlteracao);
        }

        #endregion

        #region IServicoRoteadorOrdensCallback Members

        public void OrdemAlterada(OrdemInfo pAlteracao)
        {
            gLog4Net.Debug("OrdemAlterada():");

            Timestamp = _getSecsFromTicks(DateTime.Now.Ticks);

            OnChegadaDeAcompanhamento(pAlteracao);
        }

        public void StatusConexaoAlterada(StatusConexaoBolsaInfo status)
        {
            gLog4Net.Debug(string.Format("Ex [{0}] Chan [{1}] Conn [{2}]", status.Bolsa, status.Operador, status.Conectado));

            Timestamp = _getSecsFromTicks(DateTime.Now.Ticks);
        }

        #endregion

        public long LastTimeStampInterval()
        {
            return (_getSecsFromTicks(DateTime.Now.Ticks) - Timestamp);
        }

        /// <summary>
        /// Converte DateTime.Ticks em segundos
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        private long _getSecsFromTicks(long ticks)
        {
            // From fucking MSDN:
            //A single tick represents one hundred nanoseconds or one
            //ten-millionth of a second. There are 10,000 ticks in a millisecond. 
            return ticks / 10000 / 1000;
        }

        
    }
}

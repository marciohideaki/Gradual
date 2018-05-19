using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System.Threading;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Cotacao.Lib;
using System.ServiceModel;
using System.Runtime.InteropServices;

namespace Gradual.OMS.AutomacaoOrdensTeste
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ServicoAutomacaoOrdensTeste : IServicoControlavel, IRoteadorOrdensCallback
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;
        private ParametrosTesteConfig _config = null;
        private IAssinaturasRoteadorOrdensCallback gClienteRoteadorOrdens = null;
        private Thread thMonitorRoteador = null;
        private Thread thProcCallback = null;
        private Dictionary<string, OrdemInfo> dctOrdens = new Dictionary<string, OrdemInfo>();
        private bool _bKeepRunning = false;
        private long _lastStatusBolsa = 0;
        private CronStyleScheduler _cron = null;
        private Queue<OrdemInfo> qCallback = new Queue<OrdemInfo>();
        private static ServicoAutomacaoOrdensTeste _me;
        private StringBuilder relatorioTeste = new StringBuilder();


        public static ServicoAutomacaoOrdensTeste GetInstance()
        {
            if (_me == null)
            {
                _me = new ServicoAutomacaoOrdensTeste();
            }

            return _me;
        }

        public void IniciarServico()
        {
            _me = this;

            logger.Info("Iniciando ServicoAutomacaoOrdensTeste");

            _bKeepRunning = true;

            logger.Info("Carregando configuracao e parametros de teste");

            _config = Gradual.OMS.Library.GerenciadorConfig.ReceberConfig<ParametrosTesteConfig>();

            thMonitorRoteador = new Thread(new ThreadStart(MonitorRoteador));
            thMonitorRoteador.Start();

            thProcCallback = new Thread(new ThreadStart(ProcessadorCallback));
            thProcCallback.Start();

            logger.Info("Iniciando scheduler");

            _cron = new CronStyleScheduler();
            _cron.Start();
            
            _status = ServicoStatus.EmExecucao;
        }


        public void PararServico()
        {
            _bKeepRunning = false;

            while (thMonitorRoteador != null && thMonitorRoteador.IsAlive)
            {
                Thread.Sleep(250);
            }

            _status = ServicoStatus.Parado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExecutarTeste()
        {
            relatorioTeste.Clear();
            dctOrdens.Clear();

            logger.Info("Iniciando execucao do teste");

            relatorioTeste.Append("Iniciando execucao do teste - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n\n");

            relatorioTeste.Append("Envio de ordens de compra \n\n");

            logger.Info("Envio de ordens de compra");
            foreach (ParametroTesteConfig parametro in _config.Parametros)
            {
                try
                {
                    char [] separadores = new char[2];

                    separadores[0] = ',';
                    separadores[1] = ';';

                    string [] ativos = parametro.Papeis.Split(separadores);

                    foreach (string ativo in ativos)
                    {
                        relatorioTeste.Append("Enviando compra de [" + parametro.Bolsa + "] [" + ativo + "] operador [" + parametro.Porta + "]\n\n");
                        executarTesteAtivo(parametro, ativo, OrdemDirecaoEnum.Compra);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Erro na execucao do teste: " + ex.Message, ex);
                }
            }

            relatorioTeste.Append("Envio de ordens de venda \n\n");
            logger.Info("Envio de ordens de venda");
            foreach (ParametroTesteConfig parametro in _config.Parametros)
            {
                try
                {
                    char[] separadores = new char[2];

                    separadores[0] = ',';
                    separadores[1] = ';';

                    string[] ativos = parametro.Papeis.Split(separadores);

                    foreach (string ativo in ativos)
                    {
                        relatorioTeste.Append("Enviando venda de [" + parametro.Bolsa + "] [" + ativo + "] operador [" + parametro.Porta + "]\n\n");
                        executarTesteAtivo(parametro, ativo, OrdemDirecaoEnum.Venda);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Erro na execucao do teste: " + ex.Message, ex);
                }
            }

            relatorioTeste.Append("*** Teste executado *** \n\n");

            relatorioTeste.Append("Status das ordens: \n\n");

            foreach (OrdemInfo info in dctOrdens.Values)
            {
                relatorioTeste.AppendFormat("ClOrdID [{0}] Bolsa [{1}] [{2}]  Papel [{3}] Qtde [{4}] Prc [{5}] Side [{6}] Status [{7}]\n\n",
                    info.ClOrdID,
                    info.Exchange,
                    info.ChannelID,
                    info.Symbol,
                    info.OrderQty,
                    info.Price,
                    info.Side.ToString(),
                    info.OrdStatus.ToString());

            }

            relatorioTeste.AppendLine("\n\nFinal da execucao - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n\n");

            Utilities.EnviarEmail("Relatorio de testes matinais " + DateTime.Now.ToString("dd/MM/yyyy"), relatorioTeste.ToString());

            logger.Info("Teste executado");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <param name="ativo"></param>
        private void executarTesteAtivo(ParametroTesteConfig parametro, string ativo, OrdemDirecaoEnum sentido)
        {
            logger.InfoFormat("Executando teste com [{0}] [{1}] [{2}] [{3}]", parametro.Bolsa, parametro.Porta, ativo, parametro.Account, parametro.EnteringTrader);

            logger.Info("Enviando nova ordem para " + ativo);
            
            OrdemInfo ordem = enviarNovaOrdem(parametro, ativo, sentido);

            logger.Info("Alterando ordem de " + ativo + " clOrdID [" + ordem.ClOrdID + "]");

            Thread.Sleep(1500);

            OrdemInfo ordemAlterada = alterarOrdem(ordem);

            if (dctOrdens.ContainsKey(ordemAlterada.ClOrdID))
            {
                if (dctOrdens[ordemAlterada.ClOrdID].OrdStatus == OrdemStatusEnum.NOVA ||
                    dctOrdens[ordemAlterada.ClOrdID].OrdStatus == OrdemStatusEnum.PARCIALMENTEEXECUTADA ||
                    dctOrdens[ordemAlterada.ClOrdID].OrdStatus == OrdemStatusEnum.SUBSTITUIDA )
                {

                    logger.Info("Cancelando ordem [" + ordemAlterada.ClOrdID + "]");

                    Thread.Sleep(1500);

                    cancelarOrdem(ordemAlterada);
                }
            }

            logger.InfoFormat("Final do teste de [{0}] [{1}] [{2}] [{3}]", parametro.Bolsa, parametro.Porta, ativo, parametro.Account, parametro.EnteringTrader);
        }


        private OrdemInfo enviarNovaOrdem(ParametroTesteConfig parametro, string ativo, OrdemDirecaoEnum sentido)
        {
            OrdemInfo ordem = new OrdemInfo();

            ordem.ClOrdID = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + ativo + parametro.Porta + parametro.Account;
            ordem.Account = Convert.ToInt32(parametro.Account);
            ordem.OrdType = OrdemTipoEnum.Limitada;
            ordem.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
            ordem.Symbol = ativo;
            ordem.SecurityID = ativo;
            ordem.Price = obterPrecoOrdem(ativo);
            ordem.Exchange = parametro.Bolsa;
            ordem.ChannelID = Convert.ToInt32(parametro.Porta);
            ordem.Side = sentido;
            ordem.OrdStatus = OrdemStatusEnum.ENVIADAPARAOROTEADORDEORDENS;
            ordem.OrderQty=parametro.Qtde;
            ordem.RegisterTime = DateTime.Now;

            if (String.IsNullOrEmpty(parametro.EnteringTrader))
                ordem.ExecBroker = "227";
            else
                ordem.ExecBroker = parametro.EnteringTrader;

            lock (dctOrdens)
            {
                dctOrdens.Add(ordem.ClOrdID, ordem);
            }

            ExecutarOrdemRequest request = new ExecutarOrdemRequest();
            request.info = ordem;

            IRoteadorOrdens roteador = Ativador.Get<IRoteadorOrdens>();
            ExecutarOrdemResponse response = roteador.ExecutarOrdem(request);

            if (response.DadosRetorno.StatusResposta != StatusRoteamentoEnum.Sucesso)
            {
                logger.Error("Erro ao enviar ordem [" + ordem.ClOrdID + "]");
                foreach (OcorrenciaRoteamentoOrdem ocorr in response.DadosRetorno.Ocorrencias)
                {
                    logger.Error("Erro: " + ocorr.Ocorrencia);
                }
            }

            return ordem;
        }


        private double obterPrecoOrdem(string ativo)
        {
            try
            {
                double preco = 10.0;

                Gradual.OMS.Cotacao.Lib.IServicoCotacao cotacao = Ativador.Get<IServicoCotacao>();

                string dados = cotacao.ReceberLivroNegocios(ativo);

                MSG_LivroNegocio_MDS negocio = Utilities.MarshalFromStringBlock<MSG_LivroNegocio_MDS>(dados.Substring(Marshal.SizeOf(new Header_MDS())));

                preco = Convert.ToDouble(negocio.Preco.ByteArrayToDecimal(3));

                if ( preco <= 0.0 )
                    preco = 10.0;

                logger.InfoFormat("Definido [{0}] preco [{1}]", ativo, preco);

                return preco;

            }
            catch( Exception ex )
            {
                logger.Error("obterPrecoOrdem: " + ex.Message, ex);
            }

            return 10.0;
        }


        private OrdemInfo alterarOrdem(OrdemInfo ordem)
        {
            OrdemInfo alterada = new OrdemInfo();

            try
            {
                lock (dctOrdens)
                {
                    if (dctOrdens.ContainsKey(ordem.ClOrdID))
                    {
                        if (dctOrdens[ordem.ClOrdID].OrdStatus == OrdemStatusEnum.CANCELADA ||
                            dctOrdens[ordem.ClOrdID].OrdStatus == OrdemStatusEnum.REJEITADA ||
                            dctOrdens[ordem.ClOrdID].OrdStatus == OrdemStatusEnum.EXECUTADA )
                            return ordem;
                    }
                }

                alterada.Account = ordem.Account;
                alterada.ChannelID = ordem.ChannelID;
                alterada.ClOrdID = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-A" + ordem.Symbol + ordem.ChannelID + ordem.Account;
                alterada.OrigClOrdID = ordem.ClOrdID;
                alterada.Symbol = ordem.Symbol;
                alterada.OrderQty = ordem.OrderQty + 1;
                alterada.Price = obterPrecoOrdem(ordem.Symbol);
                alterada.SecurityID = ordem.SecurityID;
                alterada.Exchange = ordem.Exchange;
                alterada.ChannelID = ordem.ChannelID;
                if (!String.IsNullOrEmpty(ordem.ExecBroker))
                    alterada.ExecBroker = ordem.ExecBroker;
                alterada.OrdStatus = OrdemStatusEnum.ENVIADAPARAOROTEADORDEORDENS;
                alterada.Side = ordem.Side;
                alterada.TimeInForce = ordem.TimeInForce;
                alterada.OrdType = ordem.OrdType;

                ExecutarModificacaoOrdensRequest request = new ExecutarModificacaoOrdensRequest();
                request.info = alterada;

                IRoteadorOrdens roteador = Ativador.Get<IRoteadorOrdens>();
                ExecutarModificacaoOrdensResponse response = roteador.ModificarOrdem(request);

                if (response.DadosRetorno.StatusResposta != StatusRoteamentoEnum.Sucesso)
                {
                    logger.Error("Erro ao alterar ordem [" + alterada.ClOrdID + "] alteracao de [" + ordem.ClOrdID + "]");
                    foreach (OcorrenciaRoteamentoOrdem ocorr in response.DadosRetorno.Ocorrencias)
                    {
                        logger.Error("Erro: " + ocorr.Ocorrencia);
                    }
                    return ordem;
                }

                lock (dctOrdens)
                {
                    dctOrdens.Add(alterada.ClOrdID, alterada);
                }

                return alterada;

            }
            catch (Exception ex)
            {
                logger.Error("alterarOrdem(): " + ex.Message, ex);
            }

            return alterada;
        }



        private void cancelarOrdem(OrdemInfo ordem)
        {
            try
            {
                OrdemCancelamentoInfo info = new OrdemCancelamentoInfo();
                info.ClOrdID = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-C" + ordem.Symbol + ordem.ChannelID + ordem.Account;
                info.Account = ordem.Account;
                info.ChannelID = ordem.ChannelID;
                info.Exchange = ordem.Exchange;
                info.OrigClOrdID = ordem.ClOrdID;
                info.Symbol = ordem.Symbol;
                info.SecurityID = ordem.SecurityID;
                info.Side = ordem.Side;
                if (!String.IsNullOrEmpty(ordem.ExecBroker))
                    info.ExecBroker = ordem.ExecBroker;
                ExecutarCancelamentoOrdemRequest request = new ExecutarCancelamentoOrdemRequest();

                IRoteadorOrdens roteador = Ativador.Get<IRoteadorOrdens>();
                ExecutarCancelamentoOrdemResponse response = roteador.CancelarOrdem(request);

                if (response.DadosRetorno.StatusResposta != StatusRoteamentoEnum.Sucesso)
                {
                    logger.Error("Erro ao cancelar ordem [" + ordem.ClOrdID + "] alteracao de [" + ordem.ClOrdID + "]");
                    foreach (OcorrenciaRoteamentoOrdem ocorr in response.DadosRetorno.Ocorrencias)
                    {
                        logger.Error("Erro: " + ocorr.Ocorrencia);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("cancelarOrdem(): " + ex.Message, ex);
            }
        }

        private void ProcessadorCallback()
        {
            OrdemInfo info = null;
            while (_bKeepRunning)
            {
                try
                {
                    lock (qCallback)
                    {
                        if (qCallback.Count > 0)
                        {
                            info = qCallback.Dequeue();
                        }
                        else
                        {
                            if (!Monitor.Wait(qCallback, 100))
                                continue;
                        }
                    }

                    lock (dctOrdens)
                    {
                        if (dctOrdens.ContainsKey(info.ClOrdID))
                        {
                            dctOrdens[info.ClOrdID] = info;
                        }

                        if (info.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                        {
                            if ( dctOrdens.ContainsKey(info.OrigClOrdID) )
                                dctOrdens.Remove(info.OrigClOrdID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("ProcessadorCallback: " + ex.Message, ex);
                }
            }
        }



        /// <summary>
        /// Metodo apenas para logar atividade do cronscheduler
        /// </summary>
        public void CronWatchDog()
        {
            logger.Info("CronWatchDog called");
        }

        /// <summary>
        /// Rotina principal da thread de recepcao dos callbacks do Roteador
        /// </summary>
        private void MonitorRoteador()
        {
            try
            {
                if (gClienteRoteadorOrdens == null)
                {
                    logger.Debug("Conexão não iniciada, abrindo...");

                    _assinaCallbackRoteador(this);
                }

                //ATP: 15/09/2010
                // Inclusao do tratamento da assinatura com roteador.
                // Refaz a conexao
                int i = 0;
                do
                {
                    // Se ficou mais de 60 segundos sem receber status
                    // de conexao, reinicia o channel WCF ( 1 tentativa a cada minuto) 
                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - _lastStatusBolsa);
                    if (ts.TotalSeconds > 60)
                    {
                        if (i > 600)
                        {
                            _cancelRoteadorCallback();
                            _assinaCallbackRoteador(this);

                            i = 0;
                        }
                        else
                            i++;
                    }

                    long hj = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd"));

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
                    _lastStatusBolsa = DateTime.Now.Ticks;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em _assinaCallbackRoteador():" + ex.Message, ex);
            }
        }

        public void OrdemAlterada(OrdemInfo report)
        {
            lock (qCallback)
            {
                qCallback.Enqueue(report);
                Monitor.Pulse(qCallback);
            }
        }

        public void StatusConexaoAlterada(RoteadorOrdens.Lib.Dados.StatusConexaoBolsaInfo status)
        {
            logger.InfoFormat("Status: [{0}] [{1}] [{2}]", status.Bolsa, status.Operador, (status.Conectado ? "true" : "false"));
            _lastStatusBolsa = DateTime.Now.Ticks;
        }
    }
}

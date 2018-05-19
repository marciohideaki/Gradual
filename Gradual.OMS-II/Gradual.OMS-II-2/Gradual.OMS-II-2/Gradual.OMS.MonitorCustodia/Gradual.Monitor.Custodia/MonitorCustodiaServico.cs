using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Monitor.Custodia.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.Threading;
using System.ServiceModel;
using Gradual.Monitor.Custodia.DB;
using Gradual.Monitor.Custodia.Lib.Util;
using Gradual.Monitor.Custodia.Lib.Info;
using Gradual.Monitor.Custodia.Lib.Mensageria;
using System.Configuration;
using System.Collections;

namespace Gradual.Monitor.Custodia
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MonitorCustodiaServico : IServicoMonitorCustodia, IServicoControlavel
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread thThreadClientes;
        public bool _bKeepRunning = false;
        private ServicoStatus _ServicoStatus { set; get; }
        private List<int> ClientesMonitor          = new List<int>();
        private Hashtable MonitorCustodiaMemoria = new Hashtable();
        private Hashtable htVencimentoDI = new Hashtable();
        private List<DateTime> lsFeriadosDI = new List<DateTime>();
        #endregion

        #region Construtores
        public MonitorCustodiaServico()
        {
            log4net.Config.XmlConfigurator.Configure();  
        }
        #endregion

        #region Propriedades
        private int IntervaloRecalculo
        {
            get
            {
                int intervaloRecalculo = 60000;

                if (ConfigurationManager.AppSettings["IntervaloRecalculo"] != null)
                {
                    intervaloRecalculo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloRecalculo"].ToString());
                    intervaloRecalculo *= 1000;
                }

                return intervaloRecalculo;
            }
        }

        private int IntervaloRecalculoForaPregao
        {
            get
            {
                int intervaloRecalculo = 300000;

                if (ConfigurationManager.AppSettings["IntervaloRecalculoForaPregao"] != null)
                {
                    intervaloRecalculo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloRecalculoForaPregao"].ToString());
                    intervaloRecalculo *= 1000;
                }

                return intervaloRecalculo;
            }
        }
        #endregion

        #region IServicoControlavel Members
        void IServicoControlavel.IniciarServico()
        {
            try
            {
                _bKeepRunning = true;
                _ServicoStatus = ServicoStatus.EmExecucao;
                this.StartMonitor(null);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("IniciarServico()  StackTrace - {0}",ex.StackTrace);
            }
        }

        void IServicoControlavel.PararServico()
        {
            try
            {
                _bKeepRunning = false;

                gLogger.Info("Stop de monitoramento de Custódia.");

                while (thThreadClientes.IsAlive)
                {
                    gLogger.Info("Aguardando finalizar ThreadClientes");

                    Thread.Sleep(250);
                }
                _ServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao iniciar o servico de monitoramento de Custódia -> " + ex.Message, ex);

                _ServicoStatus = ServicoStatus.Erro;
            }
        }

        ServicoStatus IServicoControlavel.ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion

        #region Métodos
        public void StartMonitor(object sender)
        {
            try
            {
                gLogger.Info("StartMonitor - Iniciando Monitor de Custodia");
                htVencimentoDI = MonitorCustodiaDB.ObterVencimentosDI();
                //lsFeriadosDI = MonitorCustodiaDB.Ob

                gLogger.Info("StartMonitor - Inicia rotina principal do sistema");

                gLogger.Info("Pausa de 2 segundos");
                Thread.Sleep(2000);

                gLogger.Info("Inicia Thread de calculo de posição de Custódia dos clientes a cada 5 segundos.");
                thThreadClientes      = new Thread(new ThreadStart(ThreadClientes));
                thThreadClientes.Name = "ThreadClientes";
                thThreadClientes.Start();

                gLogger.Info("Processo de inicialização finalizado");

                gLogger.Info("Aguardando Transações ...");
            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro ao acessar o metodo StartMonitor.", ex);
            }
        }

        private void ThreadClientes()
        {
            DateTime lastRun = DateTime.MinValue;

            while (_bKeepRunning)
            {
                TimeSpan interval = DateTime.Now - lastRun;

                try
                {
                    if (interval.TotalMilliseconds > 30000)
                    {
                        lastRun = DateTime.Now;

                        gLogger.Debug("Obtendo relacao de clientes que operaram no dia");

                        List<int> RelacaoClientesRodada = new MonitorCustodiaDB().ObterClientesPosicaoDia();

                        #region Clientes BMF AFTER

                        gLogger.Debug("Obtendo relacao de clientes com posicao AFTER BMF");

                        List<int> BMFAfter = new MonitorCustodiaDB().ObterClientesPosicaoBMFAfter();

                        foreach (int Cliente in BMFAfter)
                        {
                            if (!RelacaoClientesRodada.Contains(Cliente))
                            {
                                RelacaoClientesRodada.Add(Cliente);
                            }
                        }

                        #endregion

                        if (RelacaoClientesRodada.Count > 0)
                        {
                            gLogger.Info("Relacao de clientes encontrados.[" + RelacaoClientesRodada.Count.ToString() + "].");
                        }
                        else
                        {
                            gLogger.Info("Não existe clientes para serem calculados nesta tentativa.");
                        }

                        lock (ClientesMonitor)
                        {
                            foreach (int Cliente in RelacaoClientesRodada)
                            {
                                if (!ClientesMonitor.Contains(Cliente))
                                {
                                    gLogger.Info("Acrescentando [" + Cliente + "] na lista de monitoracao");
                                    this.ClientesMonitor.Add(Cliente);
                                    this.RunTimer(Cliente);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    gLogger.Error("Ocorreu um erro ao acessar o método ThreadClientes:" + ex.Message, ex);
                }

                Thread.Sleep(250);
            }
        }

        private void RunTimer(int CodigoCliente)
        {
            try
            {
                ClienteMutexInfo StateObj  = new ClienteMutexInfo();
                StateObj.TimerCanceled     = false;
                StateObj.SomeValue         = 1;
                StateObj.IdCliente         = CodigoCliente;
                StateObj.StatusProcessando = EnumProcessamento.LIVRE;

                System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(TimerTask);

                gLogger.Debug("Inicia timer para cliente [" + CodigoCliente + "] com [" + IntervaloRecalculo + "] ms");

                System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, StateObj, 1000, IntervaloRecalculo);

                StateObj.TimerReference = TimerItem;
            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro no método RunTimer ", ex);
            }
        }

        private void TimerTask(object StateObj)
        {
            gLogger.Debug("Entrando no timertask [" + ((ClienteMutexInfo)StateObj).IdCliente + "]");
            try
            {
                ClienteMutexInfo State = (ClienteMutexInfo)StateObj;

                int Cliente = State.IdCliente;

                System.Threading.Interlocked.Increment(ref State.SomeValue);

                gLogger.Info("Thread disparada  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());

                if (State.StatusProcessando == EnumProcessamento.LIVRE)
                {
                    State.StatusProcessando = EnumProcessamento.EMPROCESSAMENTO;

                    gLogger.Debug(" Cliente: " + State.IdCliente.ToString() + " - Aguardando Mutex");
                    State._Mutex.WaitOne();

                    gLogger.Info("INICIA CALCULO DE POSICAO  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());

                    lock (MonitorCustodiaMemoria)
                    {
                        MonitorCustodiaInfo info = this.CalcularPosicaoCustodia(Cliente);

                        if (MonitorCustodiaMemoria.Contains(info.CodigoClienteBov))
                        {
                            MonitorCustodiaMemoria[info.CodigoClienteBov] = info;
                        }
                        else
                        {
                            MonitorCustodiaMemoria.Add(info.CodigoClienteBov, info);
                        }
                    }

                    gLogger.Info("POSICAO CALCULADA  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());

                    State._Mutex.ReleaseMutex();
                    State.StatusProcessando = EnumProcessamento.LIVRE;
                }
                else
                {
                    gLogger.Info("processando  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());
                }


                System.Threading.Timer myTimer = State.TimerReference;

                if (DateTime.Now.Hour <= 8 || DateTime.Now.Hour >= 20)
                {
                    if (State.Intervalo != IntervaloRecalculoForaPregao)
                    {
                        myTimer.Change(0, IntervaloRecalculoForaPregao);
                        State.Intervalo = IntervaloRecalculoForaPregao;
                        gLogger.Info("Alterando intervalo de recalculo do cliente [" + Cliente + "] para " + IntervaloRecalculoForaPregao + "ms");
                    }
                }
                else
                {
                    if (State.Intervalo != IntervaloRecalculo)
                    {
                        myTimer.Change(0, IntervaloRecalculo);
                        State.Intervalo = IntervaloRecalculo;
                        gLogger.Info("Alterando intervalo de recalculo do cliente [" + Cliente + "] para " + IntervaloRecalculo + "ms");
                    }
                }


                if (State.TimerCanceled)
                {
                    State.TimerReference.Dispose();
                    gLogger.Info("Done  " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao processar método TimerTask {requisição de processamento da rotina CalcularPosicao} " + ex.Message, ex);
            }
        }

        private MonitorCustodiaInfo CalcularPosicaoCustodia(int idCliente)
        {
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();
            try
            {
                //if (idCliente == 31940)
                //{
                    lRetorno                       = MonitorCustodiaDB.ConsultarDadosClienteMonitorCustodia(new MonitorCustodiaInfo() { CodigoClienteBov = idCliente });
                    lRetorno.ListaCustodia         = MonitorCustodiaDB.ConsultarCustodiaNormal(new Lib.Mensageria.MonitorCustodiaRequest() { CodigoCliente = idCliente });
                    lRetorno.ListaPosicaoDiaBMF    = MonitorCustodiaDB.ConsultarCustodiaPosicaoDiaBMF(new MonitorCustodiaInfo() { CodigoClienteBmf = idCliente });
                    lRetorno.ListaGarantias        = MonitorCustodiaDB.ConsultarFinanceiroGarantiaBMF(new MonitorCustodiaInfo() { CodigoClienteBmf = idCliente });
                    //lRetorno.ListaGarantiasBMFOuro = MonitorCustodiaDB.ConsultarFinanceiroGarantiaBMFOuro(new MonitorCustodiaInfo() { CodigoClienteBmf = idCliente });
                    lRetorno.ListaGarantiasBMFOuro = new List<MonitorCustodiaInfo.CustodiaGarantiaBMFOuro>();
                    lRetorno.ListaGarantiasBovespa = MonitorCustodiaDB.ConsultarFinanceiroGarantiaBovespa(new MonitorCustodiaInfo() { CodigoClienteBov = idCliente });
                //}
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro calcular posição do cliente [{0}] - StackTrace - {1}", idCliente, ex.StackTrace);
            }

            return lRetorno;
        }

        public MonitorCustodiaResponse ObterMonitorCustodiaMemoria (MonitorCustodiaRequest lRequest)
        {
            gLogger.Debug("Solicitação de consulta [ ObterMonitorCustodiaMemoria ] requisitada. Cliente = " + lRequest.CodigoCliente.ToString());

            MonitorCustodiaResponse lRetorno = new MonitorCustodiaResponse();

            MonitorCustodiaInfo lMonitorCliente = MonitorCustodiaDB.ConsultarDadosClienteMonitorCustodia(new MonitorCustodiaInfo() { CodigoClienteBov = lRequest.CodigoCliente.Value });

            if ((lMonitorCliente.CodigoClienteBov.HasValue) || (lMonitorCliente.CodigoClienteBmf.HasValue))
            {
                lock (MonitorCustodiaMemoria)
                {
                    if (MonitorCustodiaMemoria.ContainsKey(lRequest.CodigoCliente))
                    {
                        gLogger.InfoFormat("Pegou posicao do Cliente[{0}] da memoria", lRequest.CodigoCliente);

                        lRetorno.MonitorCustodia = MonitorCustodiaMemoria[lRequest.CodigoCliente] as MonitorCustodiaInfo;
                    }
                    else
                    {
                        gLogger.Debug("A posicao do clienet[" + lRequest.CodigoCliente + "] não estava na memória");
                        gLogger.Debug("Recalcular posicao [" + lRequest.CodigoCliente + "] novamente");
                        MonitorCustodiaInfo lInfoPosicao = this.CalcularPosicaoCustodia(lRequest.CodigoCliente.Value);
                        lRetorno.MonitorCustodia = lInfoPosicao;
                        MonitorCustodiaMemoria.Add(lInfoPosicao.CodigoClienteBov, lInfoPosicao);
                    }
                }
            }

            return lRetorno;
        }
        #endregion

        #region Métodos de auxilio
        private double CalcularTaxaPtax(string Instrumento, double taxaOperada)
        {
            double lRetorno = 0.0;

            double taxaMercado = MonitorCustodiaDB.ObteCotacaoPtax();

            lRetorno = taxaOperada * taxaMercado;

            return lRetorno;
        }

        private double CalcularTaxaDI(string Instrumento, double taxaOperada)
        {

            double Ajuste = 0;
            double Numerador = 100000;
            double NumeroDiasBase = 252;

            try
            {
                DateTime dtVencimento = DateTime.Parse(htVencimentoDI[Instrumento].ToString());

                double NumeroDiasCalculados = this.ObterDiasUteis(DateTime.Now, dtVencimento);
                double Exponencial = (NumeroDiasCalculados / NumeroDiasBase);

                double taxaMercado = double.Parse(ObterCotacaoAtual(Instrumento).ToString());

                if (taxaOperada == taxaMercado)
                {
                    return 0;
                }

                #region CALCULO PU

                double PUInicial = (Numerador / Math.Pow(((1 + (taxaOperada / 100))), Exponencial));
                double PUFinal = (Numerador / Math.Pow(((1 + (taxaMercado / 100))), Exponencial));

                Ajuste = (Math.Round(PUFinal, 2) - Math.Round(PUInicial, 2));

                #endregion


            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao calcular o Spreed do DI", ex);
            }

            return Math.Round(Ajuste, 2);

        }

        private decimal ObterCotacaoAtual(string Instument)
        {
            MonitorCustodiaDB.CotacaoValor lRetorno = new MonitorCustodiaDB.CotacaoValor();

            lRetorno = new MonitorCustodiaDB().ObterCotacaoAtual(Instument);

            return lRetorno.ValorCotacao;
        }

        public int ObterDiasUteis(DateTime dataInicial, DateTime dataFinal)
        {
            int dias = 0;
            int ContadorDias = 0;

            dias = dataInicial.Date.Subtract(dataFinal).Days;

            DateTime lDataAtual = dataInicial.Date;

            if (dias < 0)
                dias = dias * -1;

            var lContFeriados = from feriado in this.lsFeriadosDI where feriado >= lDataAtual && feriado <= dataFinal select feriado;

            for (int i = 1; i <= dias; i++)
            {
                dataInicial = dataInicial.AddDays(1).Date;

                if ((dataInicial.DayOfWeek == DayOfWeek.Sunday || dataInicial.DayOfWeek == DayOfWeek.Saturday) && lContFeriados.Contains(dataInicial))
                {
                    ContadorDias++;
                }

                if (dataInicial.DayOfWeek != DayOfWeek.Sunday && dataInicial.DayOfWeek != DayOfWeek.Saturday)
                    ContadorDias++;
            }

            if (lContFeriados != null)
            {
                ContadorDias = ContadorDias - lContFeriados.Count();
            }

            return ContadorDias;
        }

        #endregion
    }
}

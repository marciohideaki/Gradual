using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections;
using log4net;
using System.Globalization;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using System.Configuration;
using Gradual.OMS.Monitor.Posicao.DB;

namespace Gradual.OMS.Monitor.Posicao.Servico
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MonitorPosicaoServico : IServicoControlavel
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread thThreadClientes;
        public bool _bKeepRunning = false;
        private ServicoStatus _ServicoStatus { set; get; }
        private List<int> ClientesMonitor = new List<int>();
        private ConcurrentDictionary<int, MonitorCustodiaInfo> MonitorCustodiaMemoria = new ConcurrentDictionary<int, MonitorCustodiaInfo>();
        private List<DateTime> lsFeriadosDI = new List<DateTime>();
        private CultureInfo gCultura = new CultureInfo("pt-BR");
        private ConcurrentDictionary<int, ClienteMutexInfo> htClientes = new ConcurrentDictionary<int, ClienteMutexInfo>();
        #endregion

        

        #region Atributos de fato de cotação com 1000
        private List<string> lstFatorCotacao1000 = new List<string>();
        #endregion

        #region Construtores
        public MonitorPosicaoServico()
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

        #region Métodos
        public void StartMonitorPosicao(object sender)
        {
            try
            {
                gLogger.Info("StartMonitorPosicao - Iniciando Monitor de Posicao");
                //lsFeriadosDI = MonitorCustodiaDB.Ob

                gLogger.Info("StartMonitorPosicao - Inicia rotina principal do sistema");

                gLogger.Info("Zerando a posição de todos os clientes no banco de dados SQL");
                MonitorPosicaoDB.ZeraPosicaoTodos();

                gLogger.Info("Pausa de 2 segundos");
                Thread.Sleep(2000);

                gLogger.Info("Inicia Thread de calculo de posição de Custódia dos clientes a cada 5 segundos.");
                thThreadClientes = new Thread(new ThreadStart(ThreadClientes));
                thThreadClientes.Name = "ThreadClientes";
                thThreadClientes.Start();

                lstFatorCotacao1000.Add("CEGR3");
                lstFatorCotacao1000.Add("CAFE3");
                lstFatorCotacao1000.Add("CAFE4");
                lstFatorCotacao1000.Add("CBEE3");
                lstFatorCotacao1000.Add("SGEN4");
                lstFatorCotacao1000.Add("PMET6");
                lstFatorCotacao1000.Add("EBTP3");
                lstFatorCotacao1000.Add("EBTP4");
                lstFatorCotacao1000.Add("TOYB3");
                lstFatorCotacao1000.Add("TOYB4");
                lstFatorCotacao1000.Add("FNAM11");
                lstFatorCotacao1000.Add("FNOR11");
                lstFatorCotacao1000.Add("NORD3");

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
                    if (interval.TotalMilliseconds > 120000)
                    {
                        lastRun = DateTime.Now;

                        gLogger.Debug("Obtendo relacao de clientes que operaram nos ultimos 2 minutos");

                        List<int> RelacaoClientesOperaram = new  MonitorPosicaoDB().ListaClientesOperaramUltimoMomento();

                        if (RelacaoClientesOperaram.Count > 0)
                        {
                            gLogger.Info("Relacao de clientes encontrados que operaram nos ultimos 2 minutos.[" + RelacaoClientesOperaram.Count.ToString() + "].");
                        }
                        else
                        {
                            gLogger.Info("Não existe clientes para serem calculados nesta tentativa.");
                        }

                        lock (ClientesMonitor)
                        {
                            foreach (int Cliente in RelacaoClientesOperaram)
                            {
                                //this.AddRemoveClientRunTimerProcessed(Cliente);

                                if (!ClientesMonitor.Contains(Cliente))
                                {
                                    gLogger.Info("Acrescentando [" + Cliente + "] na lista de monitoracao");
                                    this.ClientesMonitor.Add(Cliente);
                                }

                                this.RunTimer(Cliente);
                                Thread.Sleep(250);
                            }
                        }

                        if (interval.TotalMilliseconds > 86400000)
                        {
                            lastRun = DateTime.Now;

                            gLogger.Debug("Obtendo relacao de clientes que operaram no dia");

                            List<int> RelacaoClientesRodada = new MonitorPosicaoDB().ObterClientesPosicaoDia();

                            #region Clientes BMF AFTER

                            gLogger.Debug("Obtendo relacao de clientes com posicao AFTER BMF");

                            List<int> BMFAfter = new MonitorPosicaoDB().ObterClientesPosicaoBMFAfter();

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
                                    this.AddRemoveClientRunTimerProcessed(Cliente);

                                    if (!ClientesMonitor.Contains(Cliente))
                                    {
                                        gLogger.Info("Acrescentando [" + Cliente + "] na lista de monitoracao");
                                        this.ClientesMonitor.Add(Cliente);
                                        this.RunTimer(Cliente);
                                        Thread.Sleep(250);
                                    }
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
                ClienteMutexInfo StateObj = new ClienteMutexInfo();
                StateObj.TimerCanceled = false;
                StateObj.SomeValue = 1;
                StateObj.IdCliente = CodigoCliente;
                StateObj.StatusProcessando = EnumProcessamento.LIVRE;
                StateObj.FirstTimeProcessed = DateTime.Now;

                System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(TimerTask);

                gLogger.Debug("Inicia timer para cliente [" + CodigoCliente + "] com [" + IntervaloRecalculo + "] ms");

                System.Threading.Timer TimerItem;

                TimerItem = new System.Threading.Timer(TimerDelegate, StateObj, 1000, IntervaloRecalculo);

                StateObj.TimerReference = TimerItem;

                if (htClientes.ContainsKey(CodigoCliente))
                {
                    htClientes.TryRemove(CodigoCliente, out StateObj);
                }

                htClientes.AddOrUpdate(CodigoCliente, StateObj, (key, oldValue) => StateObj);
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

                    MonitorCustodiaInfo info = this.CalcularPosicaoCustodia(Cliente);

                    MonitorCustodiaMemoria.AddOrUpdate(info.CodigoClienteBov.Value, info, (key, oldValue) => info);

                    gLogger.InfoFormat("**************************************************************************************");
                    gLogger.InfoFormat("*******Total de calculos efetuados na memória [{0}]", MonitorCustodiaMemoria.Count.ToString());
                    gLogger.InfoFormat("**************************************************************************************");

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

                if (State.TimerReference != null)
                {
                    State.TimerReference.Dispose();
                    State.TimerReference = null;
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

        public MonitorCustodiaInfo CalcularPosicaoCustodia(int idCliente)
        {
            var lRetorno = new MonitorCustodiaInfo();

            try
            {
                lRetorno =  MonitorPosicaoDB.ConsultarDadosClienteMonitorCustodia(new MonitorCustodiaInfo() { CodigoClienteBov = idCliente });

                MonitorPosicaoDB.ConsultarCustodiaNormal(lRetorno.CodigoClienteBov, lRetorno.CodigoClienteBmf);

                if (lRetorno.CodigoClienteBmf.HasValue)
                {
                    MonitorPosicaoDB.ConsultarCustodiaPosicaoDiaBMF(lRetorno.CodigoClienteBmf);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro calcular posição do cliente [{0}] - StackTrace - {1} -> error {2}", idCliente, ex.StackTrace, ex);
            }
            return lRetorno;
        }

        private void AddRemoveClientRunTimerProcessed(int IdCliente)
        {
            ClienteMutexInfo StateObj = new ClienteMutexInfo();

            StateObj.TimerCanceled = false;
            StateObj.SomeValue = 1;
            StateObj.IdCliente = IdCliente;
            StateObj.StatusProcessando = EnumProcessamento.LIVRE;
            //StateObj.FirstTimeProcessed = DateTime.Now;
            StateObj.Intervalo = IntervaloRecalculo;

            if (htClientes.ContainsKey(IdCliente))
            {
                var ClienteMutex = new ClienteMutexInfo();

                ClienteMutex = htClientes[IdCliente] as ClienteMutexInfo;

                StateObj.SomeValue = ClienteMutex.SomeValue;
                StateObj.FirstTimeProcessed = ClienteMutex.FirstTimeProcessed;

                double lMinutes = (DateTime.Now - ClienteMutex.FirstTimeProcessed).TotalMinutes;

                gLogger.InfoFormat("FirstTimeProcessed {0} - Datetime. Now {1}", ClienteMutex.FirstTimeProcessed, DateTime.Now);

                gLogger.InfoFormat("Consulta do Cliente {1} - Ultimo acesso {0}", lMinutes, IdCliente);

                if (lMinutes > 3)
                {
                    gLogger.InfoFormat("Já passou 3 minutos da ultima consulta do Cliente {1} - Ultimo acesso {0}", lMinutes, IdCliente);

                    htClientes.TryRemove(IdCliente, out StateObj);

                    StateObj.FirstTimeProcessed = DateTime.Now;

                    if (ClientesMonitor.Contains(IdCliente))
                    {
                        lock (ClientesMonitor)
                        {
                            ClientesMonitor.Remove(IdCliente);
                        }
                    }

                    if (MonitorCustodiaMemoria.ContainsKey(IdCliente))
                    {
                        var lInfo = new MonitorCustodiaInfo();

                        MonitorCustodiaMemoria.TryRemove(IdCliente, out lInfo);
                    }
                }
                else
                {
                    //StateObj.FirstTimeProcessed = DateTime.Now;
                    htClientes.TryRemove(IdCliente, out StateObj);

                    if (!ClientesMonitor.Contains(IdCliente))
                    {
                        lock (ClientesMonitor)
                        {
                            ClientesMonitor.Add(IdCliente);
                        }
                    }
                }
            }
            else
            {
                StateObj.FirstTimeProcessed = DateTime.Now;
            }

            htClientes.AddOrUpdate(IdCliente, StateObj, (key, oldValue) => StateObj);
        }
        #endregion

        #region IServicoControlavel
        public void IniciarServico()
        {
            try
            {

                _bKeepRunning = true;
                _ServicoStatus = ServicoStatus.EmExecucao;
                this.StartMonitorPosicao(null);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("IniciarServico()  StackTrace - {0}", ex.StackTrace);
            }
        }

        public void PararServico()
        {
            try
            {
                _bKeepRunning = false;

                gLogger.Info("Stop de monitoramento de Posição.");

                while (thThreadClientes.IsAlive)
                {
                    gLogger.Info("Aguardando finalizar ThreadClientes");

                    Thread.Sleep(250);
                }
                _ServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao iniciar o servico de monitoramento de Posição -> " + ex.Message, ex);

                _ServicoStatus = ServicoStatus.Erro;
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }
        #endregion
    }
}

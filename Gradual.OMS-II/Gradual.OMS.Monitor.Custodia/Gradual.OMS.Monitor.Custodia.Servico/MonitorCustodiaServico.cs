using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Monitor.Custodia.Lib.Util;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.OMS.Monitor.Custodia.DB;
using System.Configuration;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Collections.Concurrent;

namespace Gradual.OMS.Monitor.Custodia.Servico
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode=ConcurrencyMode.Multiple )]
    public class MonitorCustodiaServico : IServicoMonitorCustodia, IServicoControlavel
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread thThreadClientes;
        public bool _bKeepRunning = false;
        private ServicoStatus _ServicoStatus { set; get; }
        private List<int> ClientesMonitor          = new List<int>();
        private ConcurrentDictionary<int, MonitorCustodiaInfo> MonitorCustodiaMemoria = new ConcurrentDictionary<int, MonitorCustodiaInfo>();
        private Hashtable htVencimentoDI = new Hashtable();
        private List<DateTime> lsFeriadosDI = new List<DateTime>();
        private CultureInfo gCultura = new CultureInfo("pt-BR");
        private ConcurrentDictionary<int, ClienteMutexInfo> htClientes = new ConcurrentDictionary<int, ClienteMutexInfo>();
        #endregion

        #region Atributos de INstrumentos de BMF
        private const string DI            = "DI1";
        private const string DOLAR         = "DOL";
        private const string INDICE        = "IND";
        private const string MINIBOLSA     = "WIN";
        private const string MINIDOLAR     = "WDL";
        private const string MINIDOLARFUT  = "WDO";
        private const string CHEIOBOI      = "BGI";
        private const string MINIBOI       = "WBG";
        private const string EURO          = "EUR";
        private const string MINIEURO      = "WEU";
        private const string CAFE          = "ICF";
        private const string MINICAFE      = "WCF";
        private const string FUTUROACUCAR  = "ISU";
        private const string ETANOL        = "ETH";
        private const string ETANOLFISICO  = "ETN";
        private const string MILHO         = "CCM";
        private const string SOJA          = "SFI";
        private const string OURO          = "OZ1";
        private const string ROLAGEMDOLAR  = "DR1";
        private const string ROLAGEMINDICE = "IR1";
        private const string ROLAGEMBOI    = "BR1";
        private const string ROLAGEMCAFE   = "CR1";
        private const string ROLAGEMMILHO  = "MR1";
        private const string ROLAGEMSOJA   = "SR1";
        #endregion

        #region Atributos de fato de cotação com 1000
        private List<string> lstFatorCotacao1000 = new List<string>();
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
        public void IniciarServico()
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

                        List<int> RelacaoClientesOperaram = new MonitorCustodiaDB().ListaClientesOperaramUltimoMomento();

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
                                if (!ClientesMonitor.Contains(Cliente))
                                {
                                    gLogger.Info("Acrescentando [" + Cliente + "] na lista de monitoracao");
                                    this.ClientesMonitor.Add(Cliente);
                                    this.RunTimer(Cliente);
                                    Thread.Sleep(250);
                                }
                            }
                        }

                        if (interval.TotalMilliseconds > 2400000)
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
                ClienteMutexInfo StateObj  = new ClienteMutexInfo();
                StateObj.TimerCanceled     = false;
                StateObj.SomeValue         = 1;
                StateObj.IdCliente         = CodigoCliente;
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

                htClientes.AddOrUpdate(CodigoCliente, StateObj, (key, oldValue)=> StateObj);
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
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();
            try
            {
                lRetorno                        = MonitorCustodiaDB.ConsultarDadosClienteMonitorCustodia(new MonitorCustodiaInfo()       { CodigoClienteBov = idCliente });

                List<MonitorCustodiaInfo.CustodiaPosicao> lPosicaoCustodia = MonitorCustodiaDB.ConsultarCustodiaNormal(new Lib.Mensageria.MonitorCustodiaRequest() { CodigoCliente = lRetorno.CodigoClienteBov, CodigoClienteBmf = lRetorno.CodigoClienteBmf });
                lRetorno.ListaCustodia = TratarListaCustodia(lPosicaoCustodia);

                List<MonitorCustodiaInfo.CustodiaPosicao> lPosicaoCustodiaAberturaBMF = MonitorCustodiaDB.ObterCustodiaAberturaBMF(new Lib.Mensageria.MonitorCustodiaRequest() { CodigoCliente = lRetorno.CodigoClienteBov, CodigoClienteBmf = lRetorno.CodigoClienteBmf });
                lRetorno.ListaCustodia.AddRange(TratarListaCustodia(lPosicaoCustodiaAberturaBMF));

                List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> lPosicaoCustodiaDia = MonitorCustodiaDB.ConsultarCustodiaPosicaoDiaBMF(new MonitorCustodiaInfo() { CodigoClienteBmf = lRetorno.CodigoClienteBmf });
                lRetorno.ListaPosicaoDiaBMF = TratarListaCustodia(lPosicaoCustodiaDia);

                lRetorno.ListaGarantias         = MonitorCustodiaDB.ConsultarFinanceiroGarantiaBMF(new MonitorCustodiaInfo()                    { CodigoClienteBmf = lRetorno.CodigoClienteBmf });
                //lRetorno.ListaGarantiasBMFOuro  = MonitorCustodiaDB.ConsultarFinanceiroGarantiaBMFOuro(new MonitorCustodiaInfo()         { CodigoClienteBmf = lRetorno.CodigoClienteBmf });
                lRetorno.ListaGarantiasBMFOuro = new List<MonitorCustodiaInfo.CustodiaGarantiaBMFOuro>();
                lRetorno.ListaGarantiasBovespa  = MonitorCustodiaDB.ConsultarFinanceiroGarantiaBovespa(new MonitorCustodiaInfo()         { CodigoClienteBov = lRetorno.CodigoClienteBov });
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro calcular posição do cliente [{0}] - StackTrace - {1} -> error {2}", idCliente, ex.StackTrace, ex);
            }

            return lRetorno;
        }

        private List<MonitorCustodiaInfo.CustodiaPosicao> TratarListaCustodia(List<MonitorCustodiaInfo.CustodiaPosicao> list )
        {
            List<MonitorCustodiaInfo.CustodiaPosicao> lRetorno = new List<MonitorCustodiaInfo.CustodiaPosicao>();
            try
            {
                MonitorCustodiaInfo.CustodiaPosicao lPosicao ;

                foreach (MonitorCustodiaInfo.CustodiaPosicao posicao in list)
                {
                    lPosicao = new MonitorCustodiaInfo.CustodiaPosicao();

                    lPosicao = posicao;

                    if (lPosicao.TipoMercado.Equals("FUT") || lPosicao.TipoMercado.Equals("OPF"))
                    {
                        decimal lQuantidadeAtual = posicao.QtdeAtual;

                        decimal lValorCotacao = posicao.Cotacao;

                        decimal lFatorMultiplicador = RetornaFatorMultiplicador(posicao.CodigoInstrumento, Convert.ToDouble(posicao.ValorFechamento, gCultura), Convert.ToDouble(posicao.Cotacao, gCultura));

                        decimal lValorPosicao = posicao.ValorFechamento;

                        decimal lDiferenca = Convert.ToDecimal((lValorCotacao - lValorPosicao), gCultura);

                        lPosicao.Resultado = ((lDiferenca * lFatorMultiplicador) * lQuantidadeAtual);
                    }
                    else
                    {
                        string lpapel = posicao.CodigoInstrumento;

                        if (lpapel != string.Empty)
                        {
                            if (lpapel.Substring(lpapel.Length - 1, 1).Equals("F"))
                            {
                                lpapel = lpapel.Substring(0, lpapel.Length - 1);
                            }
                        }

                        decimal lQuantidadeAtual = posicao.QtdeAtual;

                        decimal lValorCotacao = posicao.Cotacao;

                        decimal lFatorMultiplicador = (lstFatorCotacao1000.Contains(lpapel) ? 1000 : 1);

                        //decimal lValorPosicao = posicao.ValorFechamento;

                        //decimal lDiferenca = Convert.ToDecimal((lValorCotacao - lValorPosicao), gCultura);

                        lPosicao.Resultado = ((lValorCotacao / lFatorMultiplicador) * lQuantidadeAtual);
                    }

                    lRetorno.Add(lPosicao);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro ao tratar posição de custódia - StackTrace - {0}", ex.StackTrace);
            }

            return lRetorno;
        }

        private List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> TratarListaCustodia(List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> list)
        {
            List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> lRetorno = new List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF>();
            try
            {
                MonitorCustodiaInfo.CustodiaPosicaoDiaBMF lPosicao;

                foreach (MonitorCustodiaInfo.CustodiaPosicaoDiaBMF posicao in list)
                {
                    lPosicao = new MonitorCustodiaInfo.CustodiaPosicaoDiaBMF();

                    lPosicao = posicao;

                    if (lPosicao.TipoMercado.Equals("FUT") || lPosicao.TipoMercado.Equals("OPF"))
                    {
                        decimal lQuantidadeAtual = posicao.QtdeDisponivel;

                        decimal lValorCotacao = posicao.Cotacao;

                        decimal lFatorMultiplicador = RetornaFatorMultiplicador(posicao.CodigoInstrumento, Convert.ToDouble(posicao.ValorFechamento, gCultura), Convert.ToDouble(posicao.Cotacao, gCultura));

                        decimal lValorPosicao = posicao.ValorFechamento;

                        decimal lDiferenca = Convert.ToDecimal((lValorCotacao - lValorPosicao), gCultura);

                        lPosicao.Resultado = ((lDiferenca * lFatorMultiplicador) * lQuantidadeAtual);
                    }

                    lRetorno.Add(lPosicao);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro ao tratar posição de custódia - StackTrace - {0}", ex.StackTrace);
            }

            return lRetorno;
        }

        public MonitorCustodiaResponse ObterMonitorCustodiaMemoria (MonitorCustodiaRequest lRequest)
        {
            gLogger.Debug("Solicitação de consulta [ ObterMonitorCustodiaMemoria ] requisitada. Cliente = " + lRequest.CodigoCliente.ToString());

            MonitorCustodiaResponse lRetorno = new MonitorCustodiaResponse(); 

            try
            {
                gLogger.InfoFormat("Entrou no método-->> ObterMonitorCustodiaMemoria --> Antes do ConsultarDadosClienteMonitorCustodia");

                MonitorCustodiaInfo lMonitorCliente = MonitorCustodiaDB.ConsultarDadosClienteMonitorCustodia(new MonitorCustodiaInfo() { CodigoClienteBov = lRequest.CodigoCliente.Value, CodAssessor = lRequest.CodAssessor });

                if ((lMonitorCliente.CodigoClienteBov.HasValue) || (lMonitorCliente.CodigoClienteBmf.HasValue))
                {
                    gLogger.InfoFormat("Consulta do Cliente[{0}] ObterMonitorCustodiaMemoria -->> depois do ConsultarDadosClienteMonitorCustodia", lRequest.CodigoCliente.Value);

                    this.AddRemoveClientRunTimerProcessed(lRequest.CodigoCliente.Value);

                    if (MonitorCustodiaMemoria.ContainsKey(lRequest.CodigoCliente.Value))
                    {
                        if (!ClientesMonitor.Contains(lRequest.CodigoCliente.Value))
                        {
                            gLogger.InfoFormat("O Cliente[{0}] Está no MonitorCustodiaMemoria, mas não está sendo monitorado no ClientesMonitor nesse instante", lRequest.CodigoCliente);
                            lRetorno.MonitorCustodia = this.CalcularPosicaoCustodia(lRequest.CodigoCliente.Value);
                            gLogger.Debug("Cliente [" + lRequest.CodigoCliente + "] recalculado novamente");
                        }
                        else
                        {
                            gLogger.InfoFormat("Pegou posicao do Cliente[{0}] da memoria", lRequest.CodigoCliente);
                            lRetorno.MonitorCustodia = MonitorCustodiaMemoria[lRequest.CodigoCliente.Value] as MonitorCustodiaInfo;
                        }
                    }
                    else
                    {
                        gLogger.Debug("A posicao do cliente[" + lRequest.CodigoCliente + "] não estava na memória");
                        lRetorno.MonitorCustodia = this.CalcularPosicaoCustodia(lRequest.CodigoCliente.Value);
                        gLogger.Debug("Cliente [" + lRequest.CodigoCliente + "] recalculado novamente");
                    }

                    MonitorCustodiaMemoria.AddOrUpdate(lRetorno.MonitorCustodia.CodigoClienteBov.Value, lRetorno.MonitorCustodia, (key, oldValue) => lRetorno.MonitorCustodia);

                    gLogger.InfoFormat("**************************************************************************************");
                    gLogger.InfoFormat("*******Total de calculos efetuados na memória [{0}]", MonitorCustodiaMemoria.Count);
                    gLogger.InfoFormat("**************************************************************************************");
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro em ObterMonitorCustodiaMemoria -> ", ex);
            }

            return lRetorno;
            
        }
        public void ClearMonitorCustodiaMemoria()
        {
            MonitorCustodiaMemoria.Clear();
        }
        #endregion

        #region Métodos de auxilio
        private decimal RetornaFatorMultiplicador(string pInstrumento, double? taxaOperada, double? taxaMercado)
        {
            decimal lRetorno = 0.0M;

            string ClassificacaoInstrumento = pInstrumento.Substring(0, 3);

            switch (ClassificacaoInstrumento)
            {
                case INDICE:
                    lRetorno = 1;
                    break;
                case DOLAR:
                    lRetorno = 50;
                    break;
                case DI:
                    {
                        double lRetornoTemp = CalcularTaxaDI(pInstrumento, taxaOperada.Value);

                        lRetorno = Convert.ToDecimal(lRetornoTemp, gCultura);
                    }
                    break;
                case CHEIOBOI:
                    lRetorno = 330;
                    break;
                case MINIBOLSA:
                    lRetorno = 0.2M;
                    break;
                case MINIDOLARFUT:
                case MINIDOLAR:
                    lRetorno =10;
                    break;

                case MINIBOI:
                    lRetorno = 33;
                    break;
                case EURO:
                    lRetorno = 50;
                    break;
                case MINIEURO:
                    lRetorno = 10;
                    break;
                case CAFE:
                    lRetorno = CalcularTaxaPtax(100);
                    break;
                case MINICAFE:
                    lRetorno = CalcularTaxaPtax(10);
                    break;
                case FUTUROACUCAR:
                    lRetorno = CalcularTaxaPtax(270);
                    break;
                case ETANOL:
                    lRetorno = 30;
                    break;
                case ETANOLFISICO:
                    lRetorno = 30;
                    break;
                case MILHO:
                    lRetorno = 450;
                    break;
                case SOJA:
                    lRetorno = CalcularTaxaPtax(450);
                    break;
                case OURO:
                    lRetorno = 249.75M;
                    break;
                case ROLAGEMDOLAR:
                    lRetorno = 50;
                    break;
                case ROLAGEMINDICE:
                    lRetorno = 1;
                    break;
                case ROLAGEMBOI:
                    lRetorno = 330;
                    break;
                case ROLAGEMCAFE:
                    lRetorno = 100;
                    break;
                case ROLAGEMMILHO:
                    lRetorno = 450;
                    break;
                case ROLAGEMSOJA:
                    lRetorno = CalcularTaxaPtax(450);
                    break;
            }

            return lRetorno;
        }
        private decimal CalcularTaxaPtax(double taxaOperada)
        {
            decimal lRetorno = 0.0M;

            double taxaMercado = MonitorCustodiaDB.ObteCotacaoPtax();

            lRetorno = Convert.ToDecimal((taxaOperada * taxaMercado), gCultura);

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

        private decimal ObterCotacaoAtual(string Instument)
        {
            MonitorCustodiaDB.CotacaoValor lRetorno = new MonitorCustodiaDB.CotacaoValor();

            lRetorno =  MonitorCustodiaDB.ObterCotacaoAtual(Instument);

            return lRetorno.ValorCotacao;
        }
        #endregion
    }
}


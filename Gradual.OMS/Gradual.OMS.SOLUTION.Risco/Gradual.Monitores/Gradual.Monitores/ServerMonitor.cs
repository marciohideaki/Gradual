using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.ConectorSTM.Lib;
using log4net;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using Gradual.OMS.ConectorSTM.Lib.Mensagens;
using Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Persistencia;
using System.Collections;
using Gradual.Monitores.Risco.Info;
using System.ServiceModel;
using System.Configuration;
using Gradual.Monitores.Risco.Enum;

namespace Gradual.Monitores.Risco
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServerMonitor : IServicoMonitorRisco,IServicoControlavel
    {
        #region Declaracoes
        
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
  
        private DateTime UpdatedData   { set; get; }
        private bool     LoopControler { set; get; }
     
        private List<int> ClientProcessing         = new List<int>();       
        private List<int> ClientesMovimentoDiario  = new List<int>();
        private List<int> ClientesMonitor          = new List<int>();
        private List<PLDOperacaoInfo> ClientesPLD  = new List<PLDOperacaoInfo>();

        private System.Threading.Timer TimerClientes;
        private System.Threading.Timer TimerCotacao;
        private System.Threading.Timer TimerPLD;
        private System.Threading.Timer TimerAlertas;

        private Hashtable htClientes              = new Hashtable();
        private Hashtable htClienteMonitor        = new Hashtable();
        private Hashtable MercadoInstumento       = new Hashtable();
        private Hashtable CotacaoAtual            = new Hashtable();
        private Hashtable CotacaoFechamento       = new Hashtable();
        private Hashtable CotacaoAbertura         = new Hashtable();
        private Hashtable MonitorLucroPrejuizo    = new Hashtable();
        private Hashtable MonitorLucroPrejuizoBMF = new Hashtable();
        private Hashtable htData                  = new Hashtable();
        private Hashtable htVencimentoDI          = new Hashtable();

        #endregion

        #region Constantes utilizadas nos monitores de risco

        private const string DI            = "DI1";
        private const string DOLAR         = "DOL";
        private const string INDICE        = "IND";
        private const string MINIBOLSA     = "WIN";
        private const string MINIDOLAR     = "WDL";
        private const string MINIDOLARFUT  = "WDO";
        private const string CHEIOAGRICOLA = "BGI";
        private const string MINIAGRICOLA  = "WBG";

        private const int    CodigoCarteiraBTC = 22012;

        private const string AVISTA        = "VIS";
        private const string OPCAOCOMPRA   = "OPV";
        private const string OPCAOVENDA    = "OPV";
        private const string BMF           = "BMF";

        #endregion

        #region Constantes utilizadas nos monitores de PLD

        private const string PLDACEITO    = "ACEITO";        
        private const string PLDREJEITADO = "REJEITADO";
        private const string PLDANALISE   = "ANALISE";


        #endregion

        #region Propriedades e Configurações

        private string FATORCALCULOMINIINDICE{
            get{
                return ConfigurationManager.AppSettings["MINICONTRATO"].ToString();
            }
        }

        private string FATORCALCULOMINIDOLAR
        {
            get
            {
                return ConfigurationManager.AppSettings["MINICONTRATODOLAR"].ToString();
            }
        }

        private string FATORCALCULOINDICE{
            get{
                return ConfigurationManager.AppSettings["INDICE"].ToString();
            }
        }

        private string FATORCALCULODOLAR{
            get{
                return ConfigurationManager.AppSettings["DOLAR"].ToString();
            }
        }

        private string FATORCALCULOAGRICOLACHEIO
        {
            get{
                return ConfigurationManager.AppSettings["BGI"].ToString();
            }
        }

        private string FATORCALCULOAGRICOLAMINI
        {
            get{
                return ConfigurationManager.AppSettings["WBG"].ToString();
            }
        }
        private string FATORCALCULODI{
            get{
                return ConfigurationManager.AppSettings["DI"].ToString();
            }
        }

        private int PLDTEMPOTOTAL
        {
            get
            {
                return ConfigurationManager.AppSettings["TEMPOPLD"].DBToInt32();
            }
        }

        private int PLDCRITICO
        {
            get
            {
                return ConfigurationManager.AppSettings["PLDCRITICO"].DBToInt32();
            }
        }

        private int PLDALERTA
        {
            get
            {
                return ConfigurationManager.AppSettings["PLDALERTA"].DBToInt32();
            }
        }

        private int PLDFOLGA
        {
            get
            {
                return ConfigurationManager.AppSettings["PLDFOLGA"].DBToInt32();
            }
        }

        #endregion

        public ServerMonitor(){
            log4net.Config.XmlConfigurator.Configure();
            TimerCotacao = new Timer(new TimerCallback(ObterCotacoes), null, 0, 3000); 
        }

        public void StartMonitor(object sender)
        {
            try
            {

                logger.Info("Iniciando Monitor de Risco");
                logger.Info("Inicializando Thread para obter cotações em tempo real");
                htVencimentoDI = new PersistenciaPLD().ObterVencimentosDI();

                LoopControler = true;
                TimerCotacao = new Timer(new TimerCallback(ObterCotacoes), null, 0, 3000);               

                logger.Info("Pausa de 5 segundos"); 
                Thread.Sleep(5000);

                logger.Info("Popula objetos de memoria do sistema (Fechamentos,aberturas e tipo de mercado.)");

                logger.Info("Inicianliza Hash de fechamentos");
                CotacaoFechamento    = ObterCotacaoFechamento();

                logger.Info("Inicializa Hash de aberturas");
                CotacaoAbertura      = ObterCotacaoAbertura();

                logger.Info("Inicializa Hash de tipo de mercado");
                MercadoInstumento    = ObterMercadoInstrumento();

                logger.Info("Objetos carregados com sucesso");
                logger.Info("Inicia rotina principal do sistema");

                logger.Info("Pausa de 2 segundos");
                Thread.Sleep(2000);

                logger.Info("Inicia Thread de calculo de posição de lucro e prej dos clientes a cada 5 segundos.");
                TimerClientes = new System.Threading.Timer(new TimerCallback(ThreadClientes), null, 0, 30000);
                
                logger.Info("Thread de PLD iniciada");
                TimerPLD = new Timer(new TimerCallback(ObterRelacaoPLD), null, 0, 20000);

                logger.Info("Thread de Alarmes de PLD (ENVIO DE E-MAIL)");
                TimerAlertas = new Timer(new TimerCallback(ListarPLDAlerta), null, 0, 600000);

                logger.Info("Processo de inicialização finalizado");

                logger.Info("Aguardando Transações ...");
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o metodo StartMonitor.", ex);
            }
        }

        #region Controle PLD / REPASSE

        #region Sessão de PLD

        public void ListarPLDAlerta(object sender)
        {
            try{

                List<PLDOperacaoInfo> Operacoes = new List<PLDOperacaoInfo>();
                List<PLDOperacaoInfo> PLDCritico = new List<PLDOperacaoInfo>();

                Operacoes = ClientesPLD;

                for (int i = 0; i <= Operacoes.Count - 1; i++){
                    if ((Operacoes[i].Criticidade == EnumCriticidadePLD.CRITICO) || (Operacoes[i].STATUS == PLDREJEITADO)){               
                        PLDCritico.Add(Operacoes[i]);
                    }
                }

                if (PLDCritico.Count > 0){
                    EmailInfo _EmailInfo = new EmailInfo();
                    _EmailInfo.EnviarEmail(PLDCritico);
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um problema ao enviar e-mails de PLD.", ex);
            }
        }

        public int ObterDiasUteis(DateTime dataInicial, DateTime dataFinal)
        {
            int dias = 0;
            int ContadorDias = 0;

            dias = dataInicial.Subtract(dataFinal).Days;

            if (dias < 0)
                dias = dias * -1;

            for (int i = 1; i <= dias; i++)
            {
                dataInicial = dataInicial.AddDays(1);
                if (dataInicial.DayOfWeek != DayOfWeek.Sunday &&
                    dataInicial.DayOfWeek != DayOfWeek.Saturday)
                    ContadorDias++;
            }
            return ContadorDias;
        }

        private double CalcularTaxaDI(string Instrumento, double taxaOperada)
        {
            try
            {
                DateTime dtVencimento       = DateTime.Parse(htVencimentoDI[Instrumento].ToString());
                double Numerador            = 100.000;
                double NumeroDiasBase       = 252;
                double NumeroDiasCalculados = this.ObterDiasUteis(DateTime.Now, dtVencimento);
                double Exponencial          = (NumeroDiasCalculados / NumeroDiasBase);

                double taxaMercado = double.Parse(ObterCotacaoAtual(Instrumento).ToString());

                //DI Neutralizado
                if (taxaOperada == taxaMercado)
                {
                    return 0;
                }

                #region PU Mercado

                double Denominador = Math.Pow((1 + (taxaOperada / 100)), Exponencial);
                Denominador        = Math.Round(Denominador, 5);

                double PU          = (Numerador / Denominador);

                #endregion

                #region DI MERCADO
            
                double DenominadorMercado = Math.Pow((1 + (taxaMercado / 100)), Exponencial);
                DenominadorMercado        = Math.Round(DenominadorMercado, 5);

                double DIMercado          = (Numerador / DenominadorMercado);

                #endregion

                double Spread = (DIMercado - PU);

                return Math.Round(Spread,4);
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao calcular o Spreed do DI", ex);
            }

            return 0;
           
        }

        public void ObterRelacaoPLD(object sender)
        {
            try{
                List<PLDOperacaoInfo> Operacoes = new List<PLDOperacaoInfo>();      

                Operacoes = new PersistenciaPLD().ObterOperacoesPLD();

                lock (Operacoes)
                {
                    #region Monitor de PLD { Posição de PLD , Lucro e Prej e Criticidade dos PLDS } 

                    #region Calcula os fatores de multiplicação dos instrumentos BMF bem como o Lucro e Prejuiso dos derivativos

                    List<LucroPrejuisoPLDInfo> lstPLDLucroPrejuiso = new List<LucroPrejuisoPLDInfo>();

                    foreach (var item in Operacoes){

                        LucroPrejuisoPLDInfo Info = new LucroPrejuisoPLDInfo();

                        string ClassificacaoInstrumento = item.Intrumento.Substring(0, 3);

                        switch (ClassificacaoInstrumento)
                        {
                            case INDICE:
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOINDICE.ToString());
                                break;
                            case DOLAR:
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULODOLAR.ToString());
                                break;
                            case DI:
                                Info.FatorMultiplicador = decimal.Parse(CalcularTaxaDI(item.Intrumento, double.Parse(item.PrecoNegocio.ToString())).ToString());                            
                                break;
                            case MINIBOLSA:
                                 Info.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIINDICE.ToString());
                                break;
                            case MINIDOLAR:
                                 Info.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIINDICE.ToString());
                                break;
                        }
                        
                        Info.Instrumento    = item.Intrumento;
                        Info.PrecoNegocio   = item.PrecoNegocio;
                        Info.QtdeNegocio    = item.Quantidade;
                        Info.PrecoMercado   = ObterCotacaoAtual(Info.Instrumento);
                        Info.Sentido        = item.Sentido;
                        Info.Seq            = item.Seq;

                        if (Info.Sentido == "V"){

                            if (ClassificacaoInstrumento == DI)
                            {
                                Info.LucroPrejuiso = Math.Round((Info.QtdeNegocio * Info.FatorMultiplicador), 2);
                            }
                            else
                            {
                                Info.LucroPrejuiso = (((Info.PrecoNegocio - Info.PrecoMercado) * Info.QtdeNegocio) * Info.FatorMultiplicador);
                            }
                        }
                        else{
                            if (ClassificacaoInstrumento == DI)
                            {
                                Info.LucroPrejuiso = Math.Round((Info.QtdeNegocio * Info.FatorMultiplicador), 2);
                            }
                            else
                            {
                                Info.LucroPrejuiso = (((Info.PrecoMercado - Info.PrecoNegocio) * Info.QtdeNegocio) * Info.FatorMultiplicador);
                            }
                        }

                        lstPLDLucroPrejuiso.Add(Info);
                       
                    }

                    #endregion                           

                    for (int i = 0; i <= Operacoes.Count - 1; i++){

                        if (Operacoes.Count == lstPLDLucroPrejuiso.Count){
                            if (Operacoes[i].Seq == lstPLDLucroPrejuiso[i].Seq)
                            {
                                Operacoes[i].PrecoMercado = lstPLDLucroPrejuiso[i].PrecoMercado;
                                Operacoes[i].LucroPrejuiso = lstPLDLucroPrejuiso[i].LucroPrejuiso;

                                if (Operacoes[i].STATUS != PLDACEITO)
                                {
                                    TimeSpan MinutosRemanescentes = new TimeSpan();

                                    DateTime dataNegocio = (Operacoes[i].HR_NEGOCIO);
                                    DateTime dataExpiracaoNegocio = dataNegocio.AddMinutes(PLDTEMPOTOTAL);

                                    MinutosRemanescentes = (dataExpiracaoNegocio - DateTime.Now);
                                    Operacoes[i].MinutosRestantesPLD = MinutosRemanescentes;

                                    int MinutosRestantes = MinutosRemanescentes.Minutes;

                                    if (MinutosRestantes <= PLDCRITICO)
                                    {
                                        Operacoes[i].Criticidade = EnumCriticidadePLD.CRITICO;
                                        Operacoes[i].DescricaoCriticidade = "CRITICO";
                                    }
                                    else if ((MinutosRestantes >= PLDCRITICO) && (MinutosRestantes <= PLDALERTA))
                                    {
                                        Operacoes[i].Criticidade = EnumCriticidadePLD.ALERTA;
                                        Operacoes[i].DescricaoCriticidade = "ALERTA";
                                    }
                                    else
                                    {
                                        Operacoes[i].Criticidade = EnumCriticidadePLD.FOLGA;
                                        Operacoes[i].DescricaoCriticidade = "FOLGA";
                                    }
                                  
                                }
                            }
                        }
                    }
           
                    #endregion
                }

                lock (ClientesPLD)
                {
                    ClientesPLD.Clear();
                    ClientesPLD = Operacoes;
                }
            }
            catch (Exception ex){
                logger.Info("Ocorreu um erro ao carregar a relação PLD dos clientes. " + ex.Message);
            }
        }

        #endregion

        #endregion

        #region Monitor de Risco (L/P) 

        #region Gerenciamento de controle de threads

        private void ThreadClientes(object StateObj)
        {
            try
            {
                List<int> RelacaoClientesRodada = 
                    new PersistenciaMonitorRisco().ObterClientesPosicaoDia();

                #region Clientes BMF AFTER

                List<int> BMFAfter = new PersistenciaMonitorRisco().ObterClientesPosicaoBMFAfter();

                foreach (int Cliente in BMFAfter){
                    if(!RelacaoClientesRodada.Contains(Cliente)){
                        RelacaoClientesRodada.Add(Cliente);
                    }
                }

                #endregion

                if (RelacaoClientesRodada.Count > 0)
                {
                    logger.Info("Relacao de clientes encontrados.[" + RelacaoClientesRodada.Count.ToString() + "].");
                }
                else{
                    logger.Info("Não existe clientes para serem calculados nesta tentativa.");
                }

                foreach (int Cliente in RelacaoClientesRodada){
                    if (!ClientesMonitor.Contains(Cliente)){

                       
                            this.ClientesMonitor.Add(Cliente);
                            this.RunTimer(Cliente);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ThreadClientes ", ex);
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

                System.Threading.TimerCallback TimerDelegate =
                    new System.Threading.TimerCallback(TimerTask);

                System.Threading.Timer TimerItem =
                    new System.Threading.Timer(TimerDelegate, StateObj, 1000, 30000);

                StateObj.TimerReference = TimerItem;

                htClientes.Add(CodigoCliente, StateObj);
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro no método RunTimer ", ex);
            }

        }


        private List<string> ContratosAgricula()
        {

            List<string> lstAgricula = new List<string>();

            lstAgricula.Add("ICF");
            lstAgricula.Add("CR1");
            lstAgricula.Add("BGI");
            lstAgricula.Add("ETN");
            lstAgricula.Add("ETH");
            lstAgricula.Add("ISU");
            lstAgricula.Add("CCM");
            lstAgricula.Add("CTM");
            lstAgricula.Add("COP");
            lstAgricula.Add("CRV");
            lstAgricula.Add("CPG");
            lstAgricula.Add("MR1");
            lstAgricula.Add("SOJ");
            lstAgricula.Add("SR1");
            lstAgricula.Add("SFI");

            return lstAgricula;
        }

        private void TimerTask(object StateObj)
        {
            try
            {
                ClienteMutexInfo State = (ClienteMutexInfo)StateObj;

                int Cliente = State.IdCliente;

                System.Threading.Interlocked.Increment(ref State.SomeValue);

                logger.Info("Thread disparada  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());

                if (State.StatusProcessando == EnumProcessamento.LIVRE)
                {
                    State.StatusProcessando = EnumProcessamento.EMPROCESSAMENTO;

                    State._Mutex.WaitOne();

                    if (Cliente == 1085)
                    {

                        logger.Info("INICIA CALCULO DE POSICAO  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());

                        ExposicaoClienteInfo info =
                            this.CalcularPosicao(Cliente);

                        lock (MonitorLucroPrejuizo)
                        {
                            MonitorLucroPrejuizo.Remove(info.CodigoBovespa);
                            MonitorLucroPrejuizo.Add(info.CodigoBovespa, info);
                        }
                    }

                    logger.Info("POSICAO CALCULADA  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());

                    State._Mutex.ReleaseMutex();
                    State.StatusProcessando = EnumProcessamento.LIVRE;

                }
                else
                {
                    logger.Info("processando  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());
                }

                if (State.TimerCanceled)
                {
                    State.TimerReference.Dispose();
                    logger.Info("Done  " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao processar método TimerTask {requisição de processamento da rotina CalcularPosicao} ", ex);
            }
        }

        #endregion


        public List<BTCInfo> ObterPosicaoBTC(int idCliente)
        {
             List<BTCInfo> lstBtc = new PersistenciaMonitorRisco().ObterPosicaoBTC(idCliente);

             for (int i = 0; i <= lstBtc.Count - 1; i++)
             {
                 string Instrumento = lstBtc[i].Instrumento;
                 lstBtc[i].PrecoMercado = ObterCotacaoAtual(Instrumento);

             }          

             return lstBtc;
        }

        public List<PosicaoTermoInfo> ObterPosicaoTermo(int idCliente)
        {
            List<PosicaoTermoInfo> lstTermo = new PersistenciaMonitorRisco().ObterPosicaoTermo(idCliente);
            List<PosicaoTermoInfo> lstTermoAux = new List<PosicaoTermoInfo>();

            for (int i = 0; i <= lstTermo.Count - 1; i++)
            {
                string Instrumento = lstTermo[i].Instrumento;
                decimal LucroPrejuizo = 0;

                lstTermo[i].PrecoMercado = ObterCotacaoAtual(Instrumento);

                decimal diferencial = (lstTermo[i].PrecoMercado - lstTermo[i].PrecoExecucao);
                LucroPrejuizo = (lstTermo[i].Quantidade * diferencial);

                lstTermo[i].LucroPrejuizo = LucroPrejuizo;
            }

            var ListaTermo = from p in lstTermo
                             orderby p.Instrumento, p.DataVencimento
                             select p;


            foreach (var item in lstTermo)
            {
                lstTermoAux.Add(item);
            }

            return lstTermoAux;
        }

        public decimal ObterPosicaoFinanceiraPatrimonial(int IdCliente)
        {
            try
            {
                decimal SFP = new PersistenciaMonitorRisco().ObterSituacaoFinanceiraPatrimonial(IdCliente);
                return SFP;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao calcular a sfp do cliente: " + IdCliente.ToString(),ex);
            }

            return 0;
            
        }
  
        public List<ClienteFundoInfo> ObterPosicaoFundos(int idCliente)
        {
            try
            {
                List<ClienteFundoInfo> lstFundos = new PersistenciaMonitorRisco().ObterPosicaoFundoCliente(idCliente);
                return lstFundos;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao obter a posicao de fundos do cliente: " + idCliente.ToString(), ex);
            }

            return null;
        }


        #region Rotinas de Calculo de Posicao de Lucro e Prej nos mercado de Bovespa e bmf.

        /// <summary>
        /// Rotina responsavel por recalcular a posicao do cliente on-line.
        /// </summary>
        private ExposicaoClienteInfo CalcularPosicao(object CodigoCliente)
        {
            ExposicaoClienteInfo      _MonitorExposicao   = new ExposicaoClienteInfo();
            List<ClienteRiscoResumo>  _ClienteResumoLista = new List<ClienteRiscoResumo>();

            decimal LucroPrejuizoAcumulado = 0;  

            decimal CustodiaValorizadaCompraIntraday = 0;
            decimal CustodiaValorizadaVendaIntraday = 0;
            decimal CustodiaValorizadaAbertura;

            try
            {

                #region Informações básicas sobre a conta do cliente ( codigos,nome,assessor etc )

                int CodigoBovespa = int.Parse(CodigoCliente.ToString());

                _MonitorExposicao.OrdensExecutadas = new List<OperacoesInfo>();

                _MonitorExposicao.DtAtualizacao = DateTime.Now;    

                DateTime dataIniDadosCli = DateTime.Now;

                ClienteInfo _ClienteInfo = new PersistenciaMonitorRisco().ObterDadosCliente(CodigoBovespa);

                if ((_ClienteInfo.CodigoBMF == string.Empty) || (_ClienteInfo.CodigoBMF == null)){

                    int CodigoBmf =  new PersistenciaMonitorRisco().ObterContaBMF(CodigoBovespa);

                    if (CodigoBmf > 0){
                        _ClienteInfo.CodigoBMF = CodigoBmf.ToString();
                    }

                }

                TimeSpan stampDadosCli = (DateTime.Now - dataIniDadosCli);

                _MonitorExposicao.CodigoBovespa = CodigoBovespa;
                _MonitorExposicao.Cliente       = _ClienteInfo.NomeCliente;
                _MonitorExposicao.Assessor      = _ClienteInfo.Assessor;


                #endregion

                #region Situacao Financeira Patrimonial

                _MonitorExposicao.SituacaoFinanceiraPatrimonial = this.ObterPosicaoFinanceiraPatrimonial(int.Parse(CodigoCliente.ToString()));

                #endregion

                #region Posicao Fundos

                List<ClienteFundoInfo> lstFundos = ObterPosicaoFundos(int.Parse(CodigoCliente.ToString()));

                if (lstFundos.Count > 0){

                    _MonitorExposicao.PosicaoFundos = lstFundos;

                    foreach (var item in lstFundos){
                        _MonitorExposicao.TotalFundos += item.Saldo;
                    }
                }

                #endregion

                #region Rotina de Calculo e processamento de bmf.

                int QuantidadeContrato = 0;
                decimal PosicaoAberturaDia = 0;

                if ((_ClienteInfo.CodigoBMF != string.Empty) && (_ClienteInfo.CodigoBMF != null))
                {
                    #region Abertura

                    // POSICAO ABERTURA DIA
                    List<CustodiaAberturaInfo> PosicaoAberturaBMF = new PersistenciaMonitorRisco().ObterCustodiaAberturaBMF(int.Parse(_ClienteInfo.CodigoBMF));                    

                    if (PosicaoAberturaBMF.Count > 0)
                    {
                        decimal LucroPrejuizoCompraAbertura = 0;
                        decimal LucroPrejuizoVendaAbertura  = 0;
                        for (int i = 0; i <= PosicaoAberturaBMF.Count - 1; i++)
                        {
                            decimal FatorMultiplicador = 0;
                            decimal PrecoContratoMercado = 0;
                            decimal PrecoContratoAbertura = 0;
                            decimal DiferencialPontos = 0;

                            //BGI: (PF – PI) * Q * 330
                            //WBG: (PF – PI) * Q * 33

                            CustodiaAberturaInfo _PosicaoAberturaBmf =
                                (CustodiaAberturaInfo)(PosicaoAberturaBMF[i]);

                            string ClassificacaoInstrumento = _PosicaoAberturaBmf.Instrumento.Substring(0, 3);

                            switch (ClassificacaoInstrumento)
                            {
                                case INDICE:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOINDICE.ToString());
                                    break;
                                case DOLAR:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULODOLAR.ToString());
                                    break;
                                case DI:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULODI.ToString());
                                    break;
                                case MINIBOLSA:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOMINIINDICE.ToString());
                                    break;
                                case MINIDOLAR:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOMINIDOLAR.ToString());
                                    break;
                                case MINIDOLARFUT:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOMINIINDICE.ToString());
                                    break;
                                case CHEIOAGRICOLA:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOAGRICOLACHEIO.ToString());
                                    break;
                                case MINIAGRICOLA:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOAGRICOLAMINI.ToString());
                                    break;

                            }

                            PrecoContratoMercado = ObterCotacaoAtual(_PosicaoAberturaBmf.Instrumento);
                            PrecoContratoAbertura = ObterCotacaoAberturaInstrumento((_PosicaoAberturaBmf.Instrumento));

                            if (_PosicaoAberturaBmf.Quantidade < 0) // Posicao Vendida
                            {
                                DiferencialPontos = ((PrecoContratoMercado - PrecoContratoAbertura  ) * _PosicaoAberturaBmf.Quantidade);
                                LucroPrejuizoVendaAbertura += (DiferencialPontos * FatorMultiplicador);

                            }
                            else if (_PosicaoAberturaBmf.Quantidade > 0) // Posicao comprada
                            {
                                DiferencialPontos = ((PrecoContratoMercado - PrecoContratoAbertura) * _PosicaoAberturaBmf.Quantidade);
                                LucroPrejuizoCompraAbertura += (DiferencialPontos * FatorMultiplicador);                                   
                            }
                         
                        }

                        PosicaoAberturaDia = (LucroPrejuizoCompraAbertura + LucroPrejuizoVendaAbertura) ;
                        _MonitorExposicao.PLAberturaBMF = PosicaoAberturaDia;

                    }

                    #endregion

                    #region Lucro / Prej BMF

                    // POSICAO INTRADAY
                    List<PosicaoBmfInfo> PosicaoIntradayBMF = new PersistenciaMonitorRisco().ObtemPosicaoIntradayBMF(int.Parse(_ClienteInfo.CodigoBMF));
                    
                    List<PosicaoBmfInfo> PosicaoIntradayBMFAFTER = new PersistenciaMonitorRisco().ObtemPosicaoIntradayBMFAFTER(int.Parse(_ClienteInfo.CodigoBMF));

                    List<string> lstAgricula = this.ContratosAgricula();

                    foreach (string contrato in lstAgricula){

                        for (int i = 0; i <= PosicaoIntradayBMFAFTER.Count - 1; i++){

                            string ClassificacaoInstrumento = 
                                PosicaoIntradayBMFAFTER[i].Contrato.Substring(0, 3);

                            if (lstAgricula.Contains(ClassificacaoInstrumento)){

                                lock (PosicaoIntradayBMF){
                                    PosicaoIntradayBMF.Add(PosicaoIntradayBMFAFTER[i]);
                                }

                                lock (PosicaoIntradayBMFAFTER){
                                    PosicaoIntradayBMFAFTER.RemoveAt(i);
                                }
                            }
                        }
                    }

                    decimal LucroPrejuizoContratoCompra = 0;
                    decimal LucroPrejuizoContratoVenda = 0;

                    List<PosicaoBmfInfo> PosicaoBMFCalculada = new List<PosicaoBmfInfo>();

                    if (PosicaoIntradayBMF.Count > 0)
                    {

                        string Instrumento = string.Empty;
                        string InstrumentoAnterior = string.Empty;
                        decimal PrecoContrato = 0;

                        for (int i = 0; i <= PosicaoIntradayBMF.Count - 1; i++)
                        {
                            PosicaoBmfInfo _PosicaoBmfInfo =
                                (PosicaoBmfInfo)(PosicaoIntradayBMF[i]);

                            string ClassificacaoInstrumento = _PosicaoBmfInfo.Contrato.Substring(0, 3);

                            switch (ClassificacaoInstrumento)
                            {
                                case INDICE:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOINDICE.ToString());
                                    break;
                                case DOLAR:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULODOLAR.ToString());
                                    break;
                                case DI:                                    
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(CalcularTaxaDI(_PosicaoBmfInfo.Contrato, double.Parse(_PosicaoBmfInfo.PrecoAquisicaoContrato.ToString())).ToString()); 
                                    break;
                                case MINIBOLSA:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIINDICE.ToString());
                                    break;
                                case MINIDOLAR:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIINDICE.ToString());
                                    break;
                                case CHEIOAGRICOLA:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOAGRICOLACHEIO.ToString());
                                    break;
                                case MINIAGRICOLA:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOAGRICOLAMINI.ToString());
                                    break;

                            }

                            #region CONTROLE DE COTACAO P/ CONTRATO

                            Instrumento = _PosicaoBmfInfo.Contrato;

                            if (Instrumento != InstrumentoAnterior)
                            {
                                InstrumentoAnterior = Instrumento;
                                _PosicaoBmfInfo.PrecoContatoMercado = ObterCotacaoAtual(_PosicaoBmfInfo.Contrato);
                                PrecoContrato = _PosicaoBmfInfo.PrecoContatoMercado;
                            }
                            else
                            {
                                InstrumentoAnterior = Instrumento;
                                _PosicaoBmfInfo.PrecoContatoMercado = PrecoContrato;
                            }

                            #endregion

                            if (_PosicaoBmfInfo.Sentido == "V")
                            {
                                if (ClassificacaoInstrumento != DI)
                                {

                                    _PosicaoBmfInfo.DiferencialPontos = (_PosicaoBmfInfo.PrecoAquisicaoContrato - _PosicaoBmfInfo.PrecoContatoMercado);
                                    _PosicaoBmfInfo.LucroPrejuizoContrato = ((_PosicaoBmfInfo.DiferencialPontos * _PosicaoBmfInfo.FatorMultiplicador) * _PosicaoBmfInfo.QuantidadeContato);
                                    LucroPrejuizoContratoVenda += _PosicaoBmfInfo.LucroPrejuizoContrato;
                                    QuantidadeContrato -= _PosicaoBmfInfo.QuantidadeContato;
                                }
                                else
                                {
                                    _PosicaoBmfInfo.DiferencialPontos = (_PosicaoBmfInfo.PrecoAquisicaoContrato - _PosicaoBmfInfo.PrecoContatoMercado);
                                    _PosicaoBmfInfo.LucroPrejuizoContrato = (_PosicaoBmfInfo.DiferencialPontos * (_PosicaoBmfInfo.FatorMultiplicador * -1));
                                    LucroPrejuizoContratoVenda += _PosicaoBmfInfo.LucroPrejuizoContrato;
                                    QuantidadeContrato -= _PosicaoBmfInfo.QuantidadeContato;

                                }
                            }
                            if (_PosicaoBmfInfo.Sentido == "C")
                            {
                                if (ClassificacaoInstrumento != DI)
                                {
                                    _PosicaoBmfInfo.DiferencialPontos = (_PosicaoBmfInfo.PrecoContatoMercado - _PosicaoBmfInfo.PrecoAquisicaoContrato);
                                    _PosicaoBmfInfo.LucroPrejuizoContrato = ((_PosicaoBmfInfo.DiferencialPontos * _PosicaoBmfInfo.FatorMultiplicador) * _PosicaoBmfInfo.QuantidadeContato);
                                    LucroPrejuizoContratoCompra += _PosicaoBmfInfo.LucroPrejuizoContrato;
                                    QuantidadeContrato += _PosicaoBmfInfo.QuantidadeContato;
                                }
                                else
                                {
                                    _PosicaoBmfInfo.DiferencialPontos = (_PosicaoBmfInfo.PrecoContatoMercado - _PosicaoBmfInfo.PrecoAquisicaoContrato);
                                    _PosicaoBmfInfo.LucroPrejuizoContrato = (_PosicaoBmfInfo.DiferencialPontos * _PosicaoBmfInfo.FatorMultiplicador);
                                    LucroPrejuizoContratoCompra += _PosicaoBmfInfo.LucroPrejuizoContrato;
                                    QuantidadeContrato += _PosicaoBmfInfo.QuantidadeContato;

                                }
                            }

                            PosicaoBMFCalculada.Add(_PosicaoBmfInfo);
                            

                        }
                 
                    }
                   
                    #endregion

                    _MonitorExposicao.LucroPrejuizoBMF = ((LucroPrejuizoContratoCompra + LucroPrejuizoContratoVenda));
                 
                    _MonitorExposicao.OrdensBMF = PosicaoBMFCalculada;
                }
            

                #endregion

                #region Saldo BMF

                decimal PosicaoBMF;

                if ((_ClienteInfo.CodigoBMF != string.Empty) && (_ClienteInfo.CodigoBMF != null))
                {
                    PosicaoBMF = new PersistenciaMonitorRisco().
                        ObterSaldoBMF(int.Parse(_ClienteInfo.CodigoBMF));

                    _MonitorExposicao.CodigoBMF = int.Parse(_ClienteInfo.CodigoBMF.ToString());
                    _MonitorExposicao.SaldoBMF = PosicaoBMF;

                }
                else
                {
                    PosicaoBMF = 0;
                    _MonitorExposicao.CodigoBMF = 0;
                    _MonitorExposicao.SaldoBMF = PosicaoBMF;
                }



                #endregion

                #region Rotina de calculo e processamento de bovespa [custodia / termo / btc / Intraday)


                #region Posicao de BTC

                _MonitorExposicao.OrdensBTC = this.ObterPosicaoBTC(int.Parse(CodigoCliente.ToString()));

                #endregion

                #region Obter Posicao Termo

                _MonitorExposicao.OrdensTermo = this.ObterPosicaoTermo(int.Parse(CodigoCliente.ToString()));

                decimal LucroPrejuizoTermo = 0;

                foreach (var item in _MonitorExposicao.OrdensTermo){
                    LucroPrejuizoTermo += item.LucroPrejuizo;
                }

                _MonitorExposicao.LucroPrejuizoTermo = LucroPrejuizoTermo;

                #endregion

                #region Posicoes de Abertura

                #region Conta Corrente na abertura do dia

                DateTime dataIniCC = DateTime.Now;
               

                decimal PosicaoContaCorrenteAbertura = 0;
                PosicaoContaCorrenteAbertura = new PersistenciaMonitorRisco().ObterSaldoAbertura(CodigoBovespa);

                TimeSpan stampDadosCC = (DateTime.Now - dataIniCC);

                // CONTA CORRENTE DE ABERTURA + SALDO BMF 
                _MonitorExposicao.ContaCorrenteAbertura = (PosicaoContaCorrenteAbertura + PosicaoBMF);

                #endregion

                #region Posicao de Conta Margem na abertura do dia

                decimal PosicaoContaMargem = 0;
                  PosicaoContaMargem = new PersistenciaMonitorRisco().ObterSaldoContaMargem(CodigoBovespa);
                _MonitorExposicao.SaldoContaMargem = PosicaoContaMargem;

                #endregion

                #region Posicao de custodia de abertura

                decimal PosicaoCustodiaAbertura = 0;

                DateTime dataIniCus = DateTime.Now;

                List<CustodiaAberturaInfo> _lstCustodiaAbertura = new PersistenciaMonitorRisco().ObterCustodiaAbertura(CodigoBovespa);         

                TimeSpan stampDadosCus = (DateTime.Now - dataIniCus);

                // Calcula Patrimonio Liquido Abertura
                foreach (var CustodiaItem in _lstCustodiaAbertura)
                {

                    decimal CotacaoAtualAbert = ObterCotacaoAtual(CustodiaItem.Instrumento);

                    // SUBTRAIR O BTC DO PL.
                    if (CustodiaItem.TipoMercado != "BMF")
                    {
                        if (CustodiaItem.CodigoCarteira != CodigoCarteiraBTC)
                        {
                            PosicaoCustodiaAbertura += (CustodiaItem.Quantidade * CotacaoAtualAbert);
                        }
                        else
                        {
                            PosicaoCustodiaAbertura -= (CustodiaItem.Quantidade * CotacaoAtualAbert);
                        }
                    }
                }

                _MonitorExposicao.CustodiaAbertura = (PosicaoCustodiaAbertura);
                CustodiaValorizadaAbertura = PosicaoCustodiaAbertura;

                #endregion

                #endregion

                #region LimitesOperacionais

                _MonitorExposicao.LimitesOperacionais = new PersistenciaMonitorRisco().ObterLimiteOperacional(CodigoBovespa);

                #endregion

                #region Resumo das posicoes Intraday

                DateTime dataIniIntra = DateTime.Now;
                List<string> lstPapeisOperados = new List<string>();

                List<PosicaoClienteIntradayInfo> PosicaoIntraday =
                    new PersistenciaMonitorRisco().ObtemPosicaoIntraday(CodigoBovespa.ToString());

                TimeSpan stampDadosIntra = (DateTime.Now - dataIniIntra);

                #region Popula o tipo de mercado dos instrumentos

                for (int i = 0; i <= PosicaoIntraday.Count - 1; i++)
                {
                    PosicaoIntraday[i].TipoMercado = this.ObterTipoMercadoInstrumento(PosicaoIntraday[i].Instrumento);
                }

                #endregion

                #region Papeis operados na Compra


                var _posicaoComprada = from p in PosicaoIntraday
                                       where p.SentidoOperacao == 'C'
                                       select p;

                lock (lstPapeisOperados)
                {
                    lstPapeisOperados.Clear();
                }

                foreach (var itemCompra in _posicaoComprada)
                {
                    string Instrumento = itemCompra.Instrumento;

                    if (!lstPapeisOperados.Contains(Instrumento))
                    {                        
                        lstPapeisOperados.Add(Instrumento);
                    }

                }


                #endregion

                #region Papeis operados na Venda

                var _posicaoVendida = from p in PosicaoIntraday
                                      where p.SentidoOperacao == 'V'
                                      select p;

                //lock (lstPapeisOperados)
                //{
                //    lstPapeisOperados.Clear();
                //}

                foreach (var itemVenda in _posicaoVendida)
                {
                    string Instrumento = itemVenda.Instrumento;

                    if (!lstPapeisOperados.Contains(Instrumento))
                    {
                        lstPapeisOperados.Add(Instrumento);
                    }

                }

                #endregion

                #endregion

                #region Lucro Prejuizo

                lstPapeisOperados = TratarListaInstrumentos(lstPapeisOperados);

         
                decimal Cotacao = 0;

                foreach (string item in lstPapeisOperados)
                {
                 
                    ClienteRiscoResumo _ClienteRiscoResumo = new ClienteRiscoResumo();
                    decimal CotacaoAtual = ObterCotacaoAtual(item);

                    int QuantidadeAbertura = 0;
                    int QuantidadeTotal = 0;

                    decimal VolumeAbertura = 0;
                    decimal LucroPrejuisoCompras = 0;
                    decimal LucroPrejuisoVendas = 0;


                    //if ((item == "") || (item == "VALEE40") || (item == "BVMF3") || (item == "PETRE21"))
                    //{

                    #region Custodia Abertura


                    var PosicaoAbertura = from p in _lstCustodiaAbertura
                                          where p.Instrumento == item
                                          select p;

                    if (PosicaoAbertura.Count() > 0)
                    {
                        foreach (var ItemAbertura in PosicaoAbertura)
                        {
                            QuantidadeAbertura = ItemAbertura.Quantidade;
                            _ClienteRiscoResumo.QtdeAber = QuantidadeAbertura;
                        }

                        VolumeAbertura = (QuantidadeAbertura * ObterCotacaoAtual(item));
                    }

                    #endregion

                    #region Posicao Comprada Dia

                    var _PosicaoCompradaPapel = from p in _posicaoComprada
                                                where p.Instrumento == item
                                                select p;

                    if (_PosicaoCompradaPapel.Count() > 0)
                    {
                        OperacoesInfo _OrdemExecInfo = null;
                        foreach (var ItemComprado in _PosicaoCompradaPapel)
                        {
                            _OrdemExecInfo = new OperacoesInfo();

                            decimal ValorInstrumentoMercado = ObterCotacaoAtual(ItemComprado.Instrumento);

                            _OrdemExecInfo.Cliente = _MonitorExposicao.CodigoBovespa.ToString();
                            _OrdemExecInfo.Instrumento = ItemComprado.Instrumento;
                            _OrdemExecInfo.Sentido = "C";
                            _OrdemExecInfo.Quantidade = ItemComprado.Quantidade;
                            _OrdemExecInfo.PrecoMercado = ValorInstrumentoMercado;
                            _OrdemExecInfo.PrecoNegocio = ItemComprado.PrecoMedioNegocio;
                            _OrdemExecInfo.TotalNegocio = (_OrdemExecInfo.Quantidade * _OrdemExecInfo.PrecoNegocio);
                            _OrdemExecInfo.TotalMercado = (_OrdemExecInfo.Quantidade * _OrdemExecInfo.PrecoMercado);
                            _OrdemExecInfo.LucroPrejuiso = (_OrdemExecInfo.TotalMercado - _OrdemExecInfo.TotalNegocio);
                            LucroPrejuisoCompras += _OrdemExecInfo.LucroPrejuiso;
                            _MonitorExposicao.OrdensExecutadas.Add(_OrdemExecInfo);

                            _OrdemExecInfo = null;

                        }

                    }

                    #endregion

                    #region Posicao Vendida Dia

                    var _PosicaoVendidaPapel = from p in _posicaoVendida
                                               where p.Instrumento == item
                                               select p;

                    if (_PosicaoVendidaPapel.Count() > 0)
                    {
                        OperacoesInfo _OrdemExecInfo = null;
                        foreach (var ItemVendido in _PosicaoVendidaPapel)
                        {
                            _OrdemExecInfo = new OperacoesInfo();

                            decimal ValorInstrumentoMercado = ObterCotacaoAtual(ItemVendido.Instrumento);

                            _OrdemExecInfo.Cliente = _MonitorExposicao.CodigoBovespa.ToString();
                            _OrdemExecInfo.Instrumento = ItemVendido.Instrumento;
                            _OrdemExecInfo.Sentido = "V";
                            _OrdemExecInfo.Quantidade = ItemVendido.Quantidade;
                            _OrdemExecInfo.PrecoMercado = ValorInstrumentoMercado;
                            _OrdemExecInfo.PrecoNegocio = ItemVendido.PrecoMedioNegocio;
                            _OrdemExecInfo.TotalNegocio = (_OrdemExecInfo.Quantidade * _OrdemExecInfo.PrecoNegocio);
                            _OrdemExecInfo.TotalMercado = (_OrdemExecInfo.Quantidade * _OrdemExecInfo.PrecoMercado);
                            _OrdemExecInfo.LucroPrejuiso = (_OrdemExecInfo.TotalNegocio - _OrdemExecInfo.TotalMercado);
                            LucroPrejuisoVendas += _OrdemExecInfo.LucroPrejuiso;

                            _MonitorExposicao.OrdensExecutadas.Add(_OrdemExecInfo);

                            _OrdemExecInfo = null;
                        }

                    }

                    #endregion

                    #region Totais de Compra (Somatoria)

                    var ExecucaoCompras = from p in _MonitorExposicao.OrdensExecutadas
                                          where p.Sentido == "C"
                                          && p.Instrumento == item
                                          select p;

                    if (ExecucaoCompras.Count() > 0)
                    {
                        foreach (var itemCompra in ExecucaoCompras)
                        {
                            _ClienteRiscoResumo.QtdeComprada += itemCompra.Quantidade;
                            _ClienteRiscoResumo.FinanceiroComprado += itemCompra.TotalNegocio;
                            _ClienteRiscoResumo.VLNegocioCompra += itemCompra.TotalNegocio;
                            _ClienteRiscoResumo.VLMercadoCompra += itemCompra.TotalMercado;


                            CustodiaValorizadaCompraIntraday += itemCompra.TotalMercado;
                        }
                    }

                    #endregion

                    #region Totais de Venda (Somatoria)

                    var ExecucaoVendas = from p in _MonitorExposicao.OrdensExecutadas
                                         where p.Sentido == "V"
                                          && p.Instrumento == item
                                         select p;

                    if (ExecucaoVendas.Count() > 0)
                    {
                        foreach (var itemVenda in ExecucaoVendas)
                        {
                            _ClienteRiscoResumo.QtdeVendida += itemVenda.Quantidade;
                            _ClienteRiscoResumo.FinanceiroVendido += itemVenda.TotalNegocio;
                            _ClienteRiscoResumo.VLNegocioVenda += itemVenda.TotalNegocio;
                            _ClienteRiscoResumo.VLMercadoVenda += itemVenda.TotalMercado;

                            CustodiaValorizadaVendaIntraday += itemVenda.TotalMercado;
                        }
                    }

                    #endregion

                    #region Lucro Prejuiso Acumulado

                    decimal _LucroPrejuisoAcum = (LucroPrejuisoCompras + LucroPrejuisoVendas);
       
                    QuantidadeTotal = (_ClienteRiscoResumo.QtdeComprada - _ClienteRiscoResumo.QtdeVendida);

                    _ClienteRiscoResumo.QtdeAtual = QuantidadeTotal;

                    decimal TotalCompraCalculado = (_ClienteRiscoResumo.VLMercadoCompra - _ClienteRiscoResumo.VLNegocioCompra);
                    decimal TotalVendaCalculado = (_ClienteRiscoResumo.VLNegocioVenda - _ClienteRiscoResumo.VLMercadoVenda);

                    if (QuantidadeTotal == 0){
                        _ClienteRiscoResumo.LucroPrejuizo = _LucroPrejuisoAcum;
                    }
                    else{
                        _ClienteRiscoResumo.LucroPrejuizo = _LucroPrejuisoAcum; //(TotalVendaCalculado + TotalCompraCalculado);
                    }


                    #endregion

                    _ClienteRiscoResumo.Cotacao = ObterCotacaoAtual(item);
                    _ClienteRiscoResumo.Cliente = CodigoBovespa;
                    _ClienteRiscoResumo.Instrumento = item;                    
                    _ClienteRiscoResumo.FinanceiroAbertura = VolumeAbertura;
                    _ClienteRiscoResumo.FinanceiroComprado = _ClienteRiscoResumo.VLNegocioCompra;
                    _ClienteRiscoResumo.FinanceiroVendido = _ClienteRiscoResumo.VLNegocioVenda;
                    _ClienteRiscoResumo.NetOperacao = (_ClienteRiscoResumo.FinanceiroAbertura + (_ClienteRiscoResumo.FinanceiroComprado - _ClienteRiscoResumo.FinanceiroVendido));

                    _ClienteRiscoResumo.TipoMercado = this.ObterTipoMercadoInstrumento(_ClienteRiscoResumo.Instrumento);

                    if (_ClienteRiscoResumo.QtdeAtual != 0){
                        if (CotacaoAtual > 0){
                            _ClienteRiscoResumo.QtReversao = Math.Round((Math.Abs(_ClienteRiscoResumo.LucroPrejuizo) / CotacaoAtual), 0) + 1;
                            
                            if (_ClienteRiscoResumo.QtReversao < 1){
                                _ClienteRiscoResumo.QtReversao = 1;
                            }
                            _ClienteRiscoResumo.PrecoReversao = CotacaoAtual;
                        }

                        if (_ClienteRiscoResumo.QtdeAtual > 0){
                            _ClienteRiscoResumo.QtReversao = (_ClienteRiscoResumo.LucroPrejuizo * -1);
                            if (_ClienteRiscoResumo.QtReversao < 1){
                                _ClienteRiscoResumo.QtReversao = 1;
                            }
                            _ClienteRiscoResumo.PrecoReversao = CotacaoAtual;
                        }
                    }
                    else{
                        _ClienteRiscoResumo.QtReversao = 0;
                        _ClienteRiscoResumo.PrecoReversao = 0;
                    }

                    _ClienteResumoLista.Add(_ClienteRiscoResumo);
                    _ClienteRiscoResumo = null;

                    //}

                }// fim loop
                #endregion

                #region Lucro Prejuizo Consolidado

                _MonitorExposicao.Operacoes = _ClienteResumoLista;

                foreach (var item in _ClienteResumoLista)
                {
                    LucroPrejuizoAcumulado += item.LucroPrejuizo;                       
                }

                _MonitorExposicao.LucroPrejuizoBOVESPA    = LucroPrejuizoAcumulado;
                _MonitorExposicao.LucroPrejuizoTOTAL = (_MonitorExposicao.LucroPrejuizoBOVESPA + (_MonitorExposicao.LucroPrejuizoBMF + _MonitorExposicao.PLAberturaBMF));

                #endregion

                #region PatrimonioLiquidoTempoReal

                decimal PLCustodia             = ((CustodiaValorizadaCompraIntraday - CustodiaValorizadaVendaIntraday) + CustodiaValorizadaAbertura);
                decimal ContaCorrenteProjetado = (_MonitorExposicao.ContaCorrenteAbertura + (CustodiaValorizadaVendaIntraday - CustodiaValorizadaCompraIntraday));

                _MonitorExposicao.PLAberturaBovespa = (CustodiaValorizadaAbertura + _MonitorExposicao.ContaCorrenteAbertura);
                _MonitorExposicao.PatrimonioLiquidoTempoReal = ((ContaCorrenteProjetado + (CustodiaValorizadaAbertura)) + (_MonitorExposicao.PLAberturaBMF + _MonitorExposicao.LucroPrejuizoBMF));

                _MonitorExposicao.PatrimonioLiquidoTempoReal += _MonitorExposicao.TotalFundos; // Inclusao de Fundos no PL

                #endregion

                #region Outras Informações consolidadas      

                _MonitorExposicao.PLAberturaBovespa = (_MonitorExposicao.ContaCorrenteAbertura + _MonitorExposicao.CustodiaAbertura);
                _MonitorExposicao.PatrimonioLiquidoTempoReal = (_MonitorExposicao.PLAberturaBovespa + (CustodiaValorizadaCompraIntraday - CustodiaValorizadaVendaIntraday) + ContaCorrenteProjetado);
                _MonitorExposicao.TotalContaCorrenteTempoReal = ContaCorrenteProjetado;

                #endregion

                #region Semaforo e proporcao de prejuiso

                decimal PercentualPrejuiso = 0;
                decimal ProporcaoPrejuiso = 0;

                if (_MonitorExposicao.LucroPrejuizoTOTAL < 0)
                {                    

                    PercentualPrejuiso = ((_MonitorExposicao.LucroPrejuizoTOTAL / _MonitorExposicao.PatrimonioLiquidoTempoReal) * 100);

                    if ((_MonitorExposicao.LucroPrejuizoTOTAL < 0) && (_MonitorExposicao.PatrimonioLiquidoTempoReal < 0))
                    {
                        PercentualPrejuiso = ((100 + PercentualPrejuiso));
                        _MonitorExposicao.PercentualPrejuiso = PercentualPrejuiso;
                    }
                    else
                    {
                        _MonitorExposicao.PercentualPrejuiso = PercentualPrejuiso;
                    }

                    _MonitorExposicao.PercentualPrejuiso = Math.Round(_MonitorExposicao.PercentualPrejuiso, 2);
                    ProporcaoPrejuiso = Math.Round(_MonitorExposicao.LucroPrejuizoTOTAL, 2);

                    if (ProporcaoPrejuiso < 2000){
                        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.ATE2K;
                    }
                    if (ProporcaoPrejuiso >= 2000){
                        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE2K;
                    }
                    if (ProporcaoPrejuiso >= 5000){
                        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE5K;
                    }
                    if (ProporcaoPrejuiso >= 10000){
                        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE10K;
                    }
                    if (ProporcaoPrejuiso >= 15000){
                        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE15K;
                    }
                    if (ProporcaoPrejuiso >= 20000){
                        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE20K;
                    }

                    if(PercentualPrejuiso < 20 ){
                        _MonitorExposicao.Semaforo = EnumSemaforo.VERDE;
                    }
                    if ((PercentualPrejuiso >= 20) && (PercentualPrejuiso < 70)){
                        _MonitorExposicao.Semaforo = EnumSemaforo.AMARELO;
                    }
                    if (PercentualPrejuiso > 70){
                        _MonitorExposicao.Semaforo = EnumSemaforo.VERMELHO;
                    }

                }

                #endregion

                #endregion

                string TempoGasto = "Tempo gasto no processamento do calculo >> Dados Basicos:" + stampDadosCC.ToString() + " | Dados ContaCorrente: " + stampDadosCC.ToString() + " | Dados Custodia: " + stampDadosCus + " | Dados Intraday: " + stampDadosIntra.ToString();
                logger.Info(TempoGasto);
            }
            catch (Exception ex)
            {
                string stackTrade = ex.StackTrace;
                logger.Info("Ocorreu um erro ao recalcular a posição do cliente. " + ex.Message);
            }

            return _MonitorExposicao;
        }

        #endregion

        #region  Metodos de apoio

        /// <summary>
        /// Metodo responsavel por gerar codigos bovespa validos.
        /// </summary>
        /// <param name="BovespaRef"></param>
        /// <returns></returns>
        private string ConverterCodigoBovespa(string BovespaRef)
        {
            string Bovespa = string.Empty;
            long BovespaAux = long.Parse(BovespaRef);

            Bovespa =
                BovespaAux.ToString().Remove(BovespaAux.ToString().Length - 1, 1);

            return Bovespa;

        }

        public List<ExposicaoClienteInfo> ObterItemsMonitor()
        {
            List<ExposicaoClienteInfo> lstClientes = new List<ExposicaoClienteInfo>();
            IDictionaryEnumerator denum;
            DictionaryEntry dentry;

            lock (MonitorLucroPrejuizo)
            {
                denum = MonitorLucroPrejuizo.GetEnumerator();
            }

            while (denum.MoveNext())
            {
                dentry = (DictionaryEntry)denum.Current;
                lstClientes.Add((ExposicaoClienteInfo)(dentry.Value));
            }
            return lstClientes;
        }

        private void TheadCalculoPosicaoIntraday(object state)
        {
            lock (ClientesMovimentoDiario){
                ClientesMovimentoDiario.Clear();
               
                ClientesMovimentoDiario = 
                    new PersistenciaMonitorRisco().ObterClientesPosicaoDia();               
            }
        }

        private List<string> TratarListaInstrumentos(List<string> ListaInstrumentos)
        {
            List<string> ListaTratada = new List<string>();

            string InstrumentoAux;
            foreach (string item in ListaInstrumentos)
            {
                InstrumentoAux = item;

                if (item.Substring(item.Length - 1, 1) == "F")
                {
                    InstrumentoAux = item.Remove(item.Length - 1);
                }

                if (!ListaTratada.Contains(InstrumentoAux))
                {
                    lock (ListaTratada)
                    {
                        ListaTratada.Add(InstrumentoAux);
                    }
                }

            }

            return ListaTratada;

        }
        
        #region Cotação Atual

        /// <summary>
        /// Metodo responsavel por obter o sinal de todas as cotações do pregão
        /// </summary>
        /// <param name="status"></param>
        private void ObterCotacoes(object state)
        {
            try
            {
                lock (CotacaoAtual)
                {
                    CotacaoAtual.Clear();
                    CotacaoAtual = new PersistenciaMonitorRisco().ObterCotacaoAtual(ref CotacaoAtual);                    

                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Método responsavel por obter a ultima cotação de um determinado instrumento.
        /// </summary>
        /// <param name="Instument"></param>
        /// <returns></returns>
        private decimal ObterCotacaoAtual(string Instument)
        {
            lock (CotacaoAtual)
            {
                if (CotacaoAtual.Contains(Instument))
                {
                    return CotacaoAtual[Instument].DBToDecimal();
                }
            }

            return 0;
        }

        #endregion

        #region Cotação Fechamento

        /// <summary>
        /// Método responsavel por obter a ultima cotação de um determinado instrumento.
        /// </summary>
        /// <param name="Instument"></param>
        /// <returns></returns>
        private decimal ObterCotacaoFechamentoInstrumento(string Instument)
        {
            lock (CotacaoFechamento)
            {
                if (CotacaoFechamento.Contains(Instument))
                {
                    return CotacaoFechamento[Instument].DBToDecimal();
                }
            }

            return 0;
        }

        /// <summary>
        /// Método responsavel por obter a cotação de abertura de um determinado instrumento.
        /// </summary>
        /// <param name="Instument"></param>
        /// <returns></returns>
        private decimal ObterCotacaoAberturaInstrumento(string Instument)
        {
            lock (CotacaoAbertura)
            {
                if (CotacaoAbertura.Contains(Instument))
                {
                    return CotacaoAbertura[Instument].DBToDecimal();
                }
            }

            return 0;
        }

        /// <summary>
        /// Método responsavel por obter a cotação de todos os fechamentos
        /// </summary>
        /// <param name="status"></param>
        private Hashtable ObterCotacaoFechamento()
        {
            lock (CotacaoFechamento)
            {
                CotacaoFechamento.Clear();
                CotacaoFechamento = new PersistenciaMonitorRisco().ObtemCotacaoFechamento();
            }

            return CotacaoFechamento;

        }

        private string ObterTipoMercadoInstrumento(string Instumento)
        {
            lock (MercadoInstumento)
            {
                if (MercadoInstumento.Contains(Instumento))
                {
                    return MercadoInstumento[Instumento].DBToString();
                }
            }

            return string.Empty;
        }

        private Hashtable ObterCotacaoAbertura()
        {
            lock (CotacaoAbertura)
            {
                CotacaoAbertura.Clear();
                CotacaoAbertura = new PersistenciaMonitorRisco().ObtemCotacaoAbertura();
            }

            return CotacaoAbertura;
        }


        private Hashtable ObterMercadoInstrumento()
        {
            lock (MercadoInstumento)
            {
                MercadoInstumento.Clear();
                MercadoInstumento = new PersistenciaMonitorRisco().ObtemTipoMercadoInstrumento();
            }

            return MercadoInstumento;

        }

        #endregion

        #endregion   

        #endregion

        #region Implementação das Interfaces de RISCO e PLD

        #region RISCO Members

        public MonitorLucroPrejuizoResponse ObterMonitorLucroPrejuizo(MonitorLucroPrejuizoRequest pRequest)
        {
            logger.Info("Solicitação de consulta [ ObterMonitorLucroPrejuizo ] requisitada. Cliente = " + pRequest.Cliente.ToString() + " Assessor = " + pRequest.Assessor.ToString());
            MonitorLucroPrejuizoResponse _response = new MonitorLucroPrejuizoResponse();
   
            try
            {

                #region Monitor Completo

                if ((pRequest.Cliente == 0) && (pRequest.Assessor == 0))
                {
                    lock (MonitorLucroPrejuizo)
                    {
                        List<ExposicaoClienteInfo> lstClientes = this.ObterItemsMonitor();                               
                                       
                        if (lstClientes.Count > 0)
                        {
                            #region Verifica Semaforo

                            if (pRequest.Semaforo != 0)
                            {
                                var Items = from p in lstClientes                                    
                                            where p.Semaforo == pRequest.Semaforo
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion

                            #region Verifica Proporcao Lucro/Prejuiso

                            if (pRequest.ProporcaoPrejuiso != 0)
                            {
                                var Items = from p in lstClientes                                       
                                            where p.ProporcaoLucroPrejuiso == pRequest.ProporcaoPrejuiso
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion

                            #region Completo

                            if ((pRequest.ProporcaoPrejuiso == 0) && (pRequest.Semaforo == 0))
                            {
                                foreach (var item in lstClientes)
                                {
                                    _response.Monitor.Add(item);
                                }
                            }

                            #endregion
                        }

                        return _response;
                    }
                }

                #endregion

                #region Pesquisa por Cliente e Assessor

                if  ((pRequest.Cliente != 0) && (pRequest.Assessor != 0))
                {                    
                    lock (MonitorLucroPrejuizo)
                    {
                        if (MonitorLucroPrejuizo.Count > 0)
                        {
                            List<ExposicaoClienteInfo> lstClientes = this.ObterItemsMonitor();

                            #region Verifica Semaforo

                            if (pRequest.Semaforo != 0)
                            {
                                var Items = from p in lstClientes
                                            where p.Semaforo == pRequest.Semaforo
                                            && p.Assessor == pRequest.Assessor.ToString()
                                            && p.Cliente == pRequest.Cliente.ToString()
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion

                            #region Verifica Proporcao Lucro/Prejuiso

                            if (pRequest.ProporcaoPrejuiso != 0)
                            {
                                var Items = from p in lstClientes
                                            where p.ProporcaoLucroPrejuiso == pRequest.ProporcaoPrejuiso
                                            && p.Assessor == pRequest.Assessor.ToString()
                                            && p.Cliente == pRequest.Cliente.ToString()
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion

                            #region cLiente & Assessor

                            if ((pRequest.ProporcaoPrejuiso == 0) && (pRequest.Semaforo == 0))
                            {

                                var Items = from p in lstClientes
                                            where p.Assessor == pRequest.Assessor.ToString()
                                            && p.Cliente == pRequest.Cliente.ToString()
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion
                        }
                    }

                    return _response;
                }

                #endregion

                #region Pesquisa por assessor

                if ((pRequest.Assessor != 0) && (pRequest.Cliente == 0))
                {
                    lock (MonitorLucroPrejuizo)
                    {
                        if (MonitorLucroPrejuizo.Count > 0)
                        {
                            List<ExposicaoClienteInfo> lstClientes = this.ObterItemsMonitor();


                            #region Verifica Semaforo

                            if (pRequest.Semaforo != 0)
                            {
                                var Items = from p in lstClientes
                                            where p.Semaforo == pRequest.Semaforo
                                            && p.Assessor == pRequest.Assessor.ToString()                                   
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion

                            #region Verifica Proporcao Lucro/Prejuiso

                            if (pRequest.ProporcaoPrejuiso != 0)
                            {
                                var Items = from p in lstClientes
                                            where p.ProporcaoLucroPrejuiso == pRequest.ProporcaoPrejuiso
                                            && p.Assessor == pRequest.Assessor.ToString()                                    
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion

                            #region Assessor

                            if ((pRequest.ProporcaoPrejuiso == 0) && (pRequest.Semaforo == 0))
                            {
                                var Items = from p in lstClientes
                                            where p.Assessor == pRequest.Assessor.ToString()
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion
                        }
                        return _response;
                    }
                }

                #endregion

                #region Pesquisa por Cliente e Assessor

                if ((pRequest.Assessor == 0) && (pRequest.Cliente != 0))
                {
                    lock (MonitorLucroPrejuizo)
                    {
                        if (MonitorLucroPrejuizo.Count > 0)
                        {
                            List<ExposicaoClienteInfo> lstClientes = this.ObterItemsMonitor();

                            #region Verifica Semaforo

                            if (pRequest.Semaforo != 0)
                            {
                                var Items = from p in lstClientes
                                            where p.Semaforo == pRequest.Semaforo
                                            &&(p.CodigoBovespa == pRequest.Cliente || p.CodigoBMF == pRequest.Cliente)
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion

                            #region Verifica Proporcao Lucro/Prejuiso

                            if (pRequest.ProporcaoPrejuiso != 0)
                            {
                                var Items = from p in lstClientes
                                            where p.ProporcaoLucroPrejuiso == pRequest.ProporcaoPrejuiso
                                            && (p.CodigoBovespa == pRequest.Cliente || p.CodigoBMF == pRequest.Cliente)

                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }
                            }

                            #endregion

                            #region Consulta por Cliente

                            if ((pRequest.ProporcaoPrejuiso == 0) && (pRequest.Semaforo == 0))
                            {
                                var Items = from p in lstClientes
                                            where   (p.CodigoBovespa == pRequest.Cliente || p.CodigoBMF == pRequest.Cliente)
                                            select p;

                                if (Items.Count() > 0)
                                {
                                    foreach (ExposicaoClienteInfo info in Items)
                                    {
                                        _response.Monitor.Add(info);
                                    }
                                }

                            }
                            #endregion
                        }
                        return _response;
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao acessar o metodo ObterMonitorLucroPrejuizo.", ex);
            }
            return _response;              
        }

        #endregion

        #region PLD Members

        public MonitorPLDResponse ObterMonitorPLD(MonitorPLDRequest pRequest)
        {
            MonitorPLDResponse _MonitorPLDResponse = new MonitorPLDResponse();
             List<PLDOperacaoInfo> lstPLD = new List<PLDOperacaoInfo>();

            lock(ClientesPLD){
                 lstPLD = ClientesPLD;
            }

            try
            {
                switch (pRequest.EnumStatusPLD)
                {
                    case EnumStatusPLD.COMPLETO:

                        #region Completo

                        if ((pRequest.Instrumento != string.Empty) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstCompleto = from p in lstPLD
                                              where p.Intrumento == pRequest.Instrumento
                                              && p.STATUS == PLDANALISE
                                                || p.STATUS == PLDREJEITADO     
                                              select p;

                            if (lstCompleto.Count() > 0)
                            {
                                foreach (var item in lstCompleto)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }
                        }

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstCompleto = from p in lstPLD
                                              where p.Intrumento == pRequest.Instrumento
                                              && p.NumeroNegocio == pRequest.NumeroNegocio
                                              && p.STATUS == PLDANALISE
                                                || p.STATUS == PLDREJEITADO   
                                              select p;

                            if (lstCompleto.Count() > 0)
                            {
                                foreach (var item in lstCompleto)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }
                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstPLDRetorno = from p in lstPLD
                                                where p.STATUS == PLDANALISE
                                                || p.STATUS == PLDREJEITADO
                                                select p;

                            if (lstPLDRetorno.Count() > 0)
                            {
                                foreach (var item in lstPLDRetorno)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em analise e reprovados");
                            }
                        }

                        #endregion

                        break;

                    case EnumStatusPLD.APROVADO:

                        #region Aprovado

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstAceitos = from p in lstPLD
                                             where p.STATUS == PLDACEITO
                                             && p.Intrumento == pRequest.Instrumento
                                             select p;

                            if (lstAceitos.Count() > 0)
                            {
                                foreach (var item in lstAceitos)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }

                        }

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstAceitos = from p in lstPLD
                                             where p.STATUS == PLDACEITO
                                             && p.Intrumento == pRequest.Instrumento
                                             && p.NumeroNegocio == pRequest.NumeroNegocio
                                             select p;

                            if (lstAceitos.Count() > 0)
                            {
                                foreach (var item in lstAceitos)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }
                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstAceitos = from p in lstPLD
                                             where p.STATUS == PLDACEITO
                                             select p;

                            if (lstAceitos.Count() > 0)
                            {
                                foreach (var item in lstAceitos)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }



                        }

                        #endregion

                        break;

                    case EnumStatusPLD.REJEITADO:

                        #region [Rejeitado]

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstRejeitado = from p in lstPLD
                                               where p.STATUS == PLDREJEITADO
                                               && p.Intrumento == pRequest.Instrumento
                                               select p;

                            if (lstRejeitado.Count() > 0)
                            {
                                foreach (var item in lstRejeitado)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }
                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstRejeitado = from p in lstPLD
                                               where p.STATUS == PLDREJEITADO
                                               && p.Intrumento == pRequest.Instrumento
                                               && p.NumeroNegocio == pRequest.NumeroNegocio
                                               select p;

                            if (lstRejeitado.Count() > 0)
                            {
                                foreach (var item in lstRejeitado)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }

                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstRejeitado = from p in lstPLD
                                               where p.STATUS == PLDREJEITADO
                                               select p;

                            if (lstRejeitado.Count() > 0)
                            {
                                foreach (var item in lstRejeitado)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }
                        }

                        #endregion

                        break;

                    case EnumStatusPLD.EMANALISE:

                        #region [ Analise ]

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio == 0))
                        {

                            var lstAnalise = from p in lstPLD
                                             where p.STATUS == PLDANALISE
                                             && p.Intrumento == pRequest.Instrumento
                                             select p;

                            if (lstAnalise.Count() > 0)
                            {
                                foreach (var item in lstAnalise)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }
                        }

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstAnalise = from p in lstPLD
                                             where p.STATUS == PLDANALISE
                                             && p.Intrumento == pRequest.Instrumento
                                             && p.NumeroNegocio == pRequest.NumeroNegocio
                                             select p;

                            if (lstAnalise.Count() > 0)
                            {
                                foreach (var item in lstAnalise)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }

                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio == 0))
                        {
                            for (int i = 0; i <= lstPLD.Count - 1; i++)
                            {
                                string status = lstPLD[i].STATUS;

                                if (status == PLDANALISE)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(lstPLD[i]);
                                }
                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em analise");
                            }

                        }

                        #endregion

                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao acessar o metodo ObterMonitorPLD. ", ex);
            }

            return _MonitorPLDResponse;

        }

        #endregion

        #endregion

        #region IServicoControlavel Members

        private ServicoStatus _ServicoStatus { set; get; }
        public void IniciarServico()
        {
            try
            {
                logger.Info("Inicializa o servico de Monitoramento de Risco e PLD");
                ThreadPool.QueueUserWorkItem(new WaitCallback(StartMonitor), null);
                _ServicoStatus = ServicoStatus.EmExecucao;
                logger.Info("Inicializa o servico de Monitoramento de Risco e PLD");
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao iniciar o servico de monitoramento de risco e pld", ex);
                _ServicoStatus = ServicoStatus.Erro;
            }
        }

        public void PararServico()
        {
            try
            {
                logger.Info("Stop servido de monitoramento de risco e pld.");
                _ServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao iniciar o servico de monitoramento de risco", ex);
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
  
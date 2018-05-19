using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cortex.OMS.ServidorFIXAdm.Lib.Dados;
using Cortex.OMS.ServidorFIX;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System.Threading;
using System.Collections;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cortex.OMS.ServidorFIX
{
    public class LimiteManager
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static LimiteManager _me = null;
        private Dictionary<int, LimiteClienteInfo> _limiteClientesBVSP = new Dictionary<int,LimiteClienteInfo>();
        private Dictionary<int, LimiteClienteInfo> _limiteClientesBMF = new Dictionary<int, LimiteClienteInfo>();
        private Dictionary<int, Dictionary<string, LimiteClienteContratoBMF>> _limiteClienteContratoBMFCompra = new Dictionary<int, Dictionary<string, LimiteClienteContratoBMF>>();
        private Dictionary<int, Dictionary<string, LimiteClienteContratoBMF>> _limiteClienteContratoBMFVenda = new Dictionary<int, Dictionary<string, LimiteClienteContratoBMF>>();
        private Dictionary<int, Dictionary<string, LimiteClienteInstrumentoBMF>> _limiteClienteInstrumentoBMFCompra = new Dictionary<int, Dictionary<string, LimiteClienteInstrumentoBMF>>();
        private Dictionary<int, Dictionary<string, LimiteClienteInstrumentoBMF>> _limiteClienteInstrumentoBMFVenda = new Dictionary<int, Dictionary<string, LimiteClienteInstrumentoBMF>>();
        private Queue<LimiteMovimentoInfo> qMovPersist = new Queue<LimiteMovimentoInfo>();
        private Queue<LimiteClienteInstrumentoBMF> qMovInstBMF = new Queue<LimiteClienteInstrumentoBMF>();
        private Queue<LimiteClienteContratoBMF> qMovContBMF = new Queue<LimiteClienteContratoBMF>();
        private Hashtable _hstOrders = new Hashtable();
        private Thread _thPersistenciaMovimento = null;
        private bool _bKeepRunning;

        private Dictionary<string, PersistenciaArquivo> _movPersistencia = new Dictionary<string, PersistenciaArquivo>();
        private bool _writeMovementToFile = false;
        
        private const string EXCHANGEBOVESPA = "BOVESPA";
        private const string EXCHANGEBMF = "BMF";
        private CamadaDeDados _db;

        private bool _serializing = false;

        private bool _updateDB = true;

        public int AccountLimit{get;set;}

        // Constructor / Destructor
        public LimiteManager()
        {

        }

        public LimiteManager(int account)
        {
            this.AccountLimit = account;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static LimiteManager GetInstance()
        {
            if (_me == null)
            {
                _me = new LimiteManager();
                _me.AccountLimit = 0;
                _me.CarregarLimites();
                _me.Start();
            }

            return _me;
        }

        public void Start()
        {
            _bKeepRunning = true;

            _thPersistenciaMovimento = new Thread(new ThreadStart(_procPersisteciaMovimento));
            _thPersistenciaMovimento.Name = "_thPersistenciaMovimento";
            _thPersistenciaMovimento.Start();
            _db = new CamadaDeDados();
        }

        public void Stop()
        {
            _bKeepRunning = false;
            while (_thPersistenciaMovimento.IsAlive)
            {
                logger.Info("Aguardando finalizar thread de persistencia dos movimentos de limite");
                Thread.Sleep(250);
            }
            _db = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CarregarLimites(int account = 0)
        {
            try
            {
                int i = 0;
                int j = 0;
                CamadaDeDados db = new CamadaDeDados();
                if (0 != account)
                    this.AccountLimit = account;

                // Validar se a insercao dos Movimentos de Limite serao adicionados em arquivo ou no banco de dados
                // - db.InserirMovimentoLimite
                // - db.InserirMovimentoLimiteInstrumentoBMF
                // - db.InserirMovimentoLimiteContratoBMF
                if (!ConfigurationManager.AppSettings.AllKeys.Contains("WriteToFile"))
                {
                    _writeMovementToFile = false;
                }
                else
                {
                    string strWrite = ConfigurationManager.AppSettings["WriteToFile"].ToString();
                    if (strWrite.Equals("0") || strWrite.Equals("N", StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(strWrite))
                        _writeMovementToFile = false;
                    else
                        _writeMovementToFile = true;
                }


                logger.Info("Carregar limites de clientes");

                List<LimiteClienteInfo> lstLimites;
                
                lstLimites = db.CarregarLimiteClientes(account);

                _limiteClientesBVSP.Clear();
                _limiteClientesBMF.Clear();
                _movPersistencia.Clear();
                foreach (LimiteClienteInfo info in lstLimites)
                {
                    if (info.CodigoBovespa > 0)
                    {
                        lock (_limiteClientesBVSP)
                        {
                            if (_limiteClientesBVSP.ContainsKey(info.CodigoBovespa))
                                _limiteClientesBVSP[info.CodigoBovespa] = info;
                            else
                                _limiteClientesBVSP.Add(info.CodigoBovespa, info);
                        }
                        lock (_movPersistencia)
                        {
                            string key = EXCHANGEBOVESPA + info.CodigoBovespa.ToString();
                            if (!_movPersistencia.ContainsKey(key))
                            {
                                PersistenciaArquivo pstA = new PersistenciaArquivo(EXCHANGEBOVESPA, info.CodigoBovespa);
                                _movPersistencia.Add(key, pstA);
                            }
                        }
                    }

                    if (info.CodigoBMF > 0)
                    {
                        lock (_limiteClientesBMF)
                        {
                            if (_limiteClientesBMF.ContainsKey(info.CodigoBMF))
                                _limiteClientesBMF[info.CodigoBMF] = info;
                            else
                                _limiteClientesBMF.Add(info.CodigoBMF, info);
                        }

                        lock (_movPersistencia)
                        {
                            string key = EXCHANGEBMF + info.CodigoBMF.ToString();
                            if (!_movPersistencia.ContainsKey(key))
                            {
                                PersistenciaArquivo pstA = new PersistenciaArquivo(EXCHANGEBMF, info.CodigoBMF);
                                _movPersistencia.Add(key, pstA);
                            }
                        }
                    }

                    logger.InfoFormat("Acct[{0,6}] Limite[{1,20:F2}] Consumido [{2,20:F2}] Restante [{3,20:F2}] Validade [{4}]",
                        info.CodigoBovespa,
                        info.valorTotal,
                        info.valorConsumido,
                        info.valorRestante,
                        info.dataValidade.ToString("dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture));

                    i++;
                }

                logger.Info("Carregados [" + i + "] registros de limites");

                logger.Info("Carregando limites BMF");

                List<LimiteClienteContratoBMF> contratosBMF = db.CarregarLimiteContratosBMF(account);
                _limiteClienteContratoBMFCompra.Clear();
                _limiteClienteContratoBMFVenda.Clear();
                j=0;
                i=0;
                foreach (LimiteClienteContratoBMF limitecontrato in contratosBMF)
                {
                    if (limitecontrato.sentido.Equals("C"))
                    {
                        _atualizarLimiteContratoBMFCompra(limitecontrato.account, limitecontrato);
                        i++;
                    }
                    else
                    {
                        _atualizarLimiteContratoBMFVenda(limitecontrato.account, limitecontrato);
                        j++;
                    }

                    logger.InfoFormat("Acct[{0,6}] Cont[{1}] Sent[{2}] QtdeTot[{3}] QtdeDisp[{4}] Validade [{5}]",
                        limitecontrato.account,
                        limitecontrato.contrato,
                        limitecontrato.sentido,
                        limitecontrato.qtTotal,
                        limitecontrato.qtDisponivel,
                        limitecontrato.dtValidade.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                }

                logger.Info("Carregados [" + i + "] limites de contratos de compra BMF");
                logger.Info("Carregados [" + j + "] limites de contratos de venda BMF");

                List<LimiteClienteInstrumentoBMF> intrumentosBMF = db.CarregarLimiteInstrumentosBMF(account);
                _limiteClienteInstrumentoBMFCompra.Clear();
                _limiteClienteInstrumentoBMFVenda.Clear();
                j = 0;
                i = 0;
                foreach (LimiteClienteInstrumentoBMF limiteinstrumento in intrumentosBMF)
                {
                    if (limiteinstrumento.sentido.Equals("C"))
                    {
                        _atualizarLimiteInstrumentoBMFCompra(limiteinstrumento.account, limiteinstrumento);
                        i++;
                    }
                    else
                    {
                        _atualizarLimiteInstrumentoBMFVenda(limiteinstrumento.account, limiteinstrumento);
                        j++;
                    }

                    logger.InfoFormat("Acct[{0,6}] Cont[{1}] Inst[{2}] Sent[{3}] QtdeTot[{4}] QtdeDisp[{5}] Validade [{6}]",
                        limiteinstrumento.account,
                        limiteinstrumento.contrato,
                        limiteinstrumento.instrumento,
                        limiteinstrumento.sentido,
                        limiteinstrumento.qtTotalInstrumento,
                        limiteinstrumento.qtDispInstrumento,
                        limiteinstrumento.dtValidade.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

                }
                logger.Info("Carregados [" + i + "] limites de instrumentos de compra BMF");
                logger.Info("Carregados [" + j + "] limites de instrumentos de venda BMF");
                _updateDB = true;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("CarregarLimites(): " + ex.Message, ex);
            }

            return false;
        }

        private void _atualizarLimiteInstrumentoBMFVenda(int account, LimiteClienteInstrumentoBMF limiteinstrumento)
        {
            lock (this._limiteClienteInstrumentoBMFVenda)
            {
                Dictionary<string, LimiteClienteInstrumentoBMF> instrumentos;

                if (_limiteClienteInstrumentoBMFVenda.ContainsKey(account))
                    instrumentos = _limiteClienteInstrumentoBMFVenda[account];
                else
                    instrumentos = new Dictionary<string, LimiteClienteInstrumentoBMF>();

                // Insere ou atualiza novo parametro de contrato
                if (instrumentos.ContainsKey(limiteinstrumento.instrumento))
                    instrumentos[limiteinstrumento.instrumento] = limiteinstrumento;
                else
                    instrumentos.Add(limiteinstrumento.instrumento, limiteinstrumento);

                // Insere ou atualiza lista de contratos autorizados
                if (_limiteClienteInstrumentoBMFVenda.ContainsKey(account))
                    _limiteClienteInstrumentoBMFVenda[account] = instrumentos;
                else
                    _limiteClienteInstrumentoBMFVenda.Add(account, instrumentos);
            }
        }

        private void _atualizarLimiteInstrumentoBMFCompra(int account, LimiteClienteInstrumentoBMF limiteinstrumento)
        {
            lock (this._limiteClienteInstrumentoBMFCompra)
            {
                Dictionary<string, LimiteClienteInstrumentoBMF> instrumentos;

                if (_limiteClienteInstrumentoBMFCompra.ContainsKey(account))
                    instrumentos = _limiteClienteInstrumentoBMFCompra[account];
                else
                    instrumentos = new Dictionary<string, LimiteClienteInstrumentoBMF>();

                // Insere ou atualiza novo parametro de contrato
                if (instrumentos.ContainsKey(limiteinstrumento.instrumento))
                    instrumentos[limiteinstrumento.instrumento] = limiteinstrumento;
                else
                    instrumentos.Add(limiteinstrumento.instrumento, limiteinstrumento);

                // Insere ou atualiza lista de contratos autorizados
                if (_limiteClienteInstrumentoBMFCompra.ContainsKey(account))
                    _limiteClienteInstrumentoBMFCompra[account] = instrumentos;
                else
                    _limiteClienteInstrumentoBMFCompra.Add(account, instrumentos);
            }
        }

        private void _atualizarLimiteContratoBMFVenda(int account, LimiteClienteContratoBMF limitecontrato)
        {

            lock (_limiteClienteContratoBMFVenda)
            {
                Dictionary<string, LimiteClienteContratoBMF> contratos;
                if (_limiteClienteContratoBMFVenda.ContainsKey(account))
                    contratos = _limiteClienteContratoBMFVenda[account];
                else
                    contratos = new Dictionary<string, LimiteClienteContratoBMF>();

                // Insere ou atualiza novo parametro de contrato
                if (contratos.ContainsKey(limitecontrato.contrato))
                    contratos[limitecontrato.contrato] = limitecontrato;
                else
                    contratos.Add(limitecontrato.contrato, limitecontrato);

                // Insere ou atualiza lista de contratos autorizados
                if (_limiteClienteContratoBMFVenda.ContainsKey(account))
                    _limiteClienteContratoBMFVenda[account] = contratos;
                else
                    _limiteClienteContratoBMFVenda.Add(account, contratos);
            }
        }

        private void _atualizarLimiteContratoBMFCompra(int account, LimiteClienteContratoBMF limitecontrato)
        {
            lock (_limiteClienteContratoBMFCompra)
            {
                Dictionary<string, LimiteClienteContratoBMF> contratos;
                if (_limiteClienteContratoBMFCompra.ContainsKey(account))
                    contratos = _limiteClienteContratoBMFCompra[account];
                else
                    contratos = new Dictionary<string, LimiteClienteContratoBMF>();

                // Insere ou atualiza novo parametro de contrato
                if (contratos.ContainsKey(limitecontrato.contrato))
                    contratos[limitecontrato.contrato] = limitecontrato;
                else
                    contratos.Add(limitecontrato.contrato, limitecontrato);

                // Insere ou atualiza lista de contratos autorizados
                if (_limiteClienteContratoBMFCompra.ContainsKey(account))
                    _limiteClienteContratoBMFCompra[account] = contratos;
                else
                    _limiteClienteContratoBMFCompra.Add(account, contratos);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetarLimiteDiario()
        {
            logger.Info("ResetarLimiteDiario(): inicio da recarga dos limites");
            lock (_db)
            {
                //CamadaDeDados db = new CamadaDeDados();
                _db.ZerarLimiteDiario();
                _db.ZerarLimiteDiarioContratoBMF();
                _db.ZerarLimiteDiarioInstrumentoBMF();
                CarregarLimites(this.AccountLimit);
            }
            // Purgar ordens do dia
            //lock (_hstOrders)
            //{
            //    _hstOrders.Clear();
            //}
        }

        public void AtualizarClienteLimite(int Account, Decimal valorTotal)
        {
            logger.Info("ResetarLimiteDiario(): inicio da recarga dos limites");
            CamadaDeDados db = new CamadaDeDados();
            db.AtualizarClienteLimite(Account, valorTotal, Decimal.Zero, valorTotal);
            CarregarLimites(Account);
        }
        public void SinalizarAtualizacao(bool valor)
        {
            _updateDB = valor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        public int ObterOperadorBovespa(int account)
        {
            int ret = -1;
            lock (_limiteClientesBVSP)
            {
                if (_limiteClientesBVSP.ContainsKey(account))
                {
                    LimiteClienteInfo info = _limiteClientesBVSP[account];
                    ret = info.OperadorBovespa;
                }
            }

            return ret;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        public int ObterOperadorBMF(int account )
        {
            int ret = -1;
            lock (_limiteClientesBMF)
            {
                if (_limiteClientesBMF.ContainsKey(account))
                {
                    LimiteClienteInfo info = _limiteClientesBMF[account];
                    ret = info.OperadorBMF;
                }
            }

            return ret;
        }

        /// <summary>
        /// Efetua checagem do limite sem efetivamente alocar o consumo do mesmo
        /// </summary>
        /// <param name="ordem">objeto do tipo OrdemInfo</param>
        /// <param name="altecacao">flag indicando que eh alteracao de ordem</param>
        /// <returns></returns>
        public bool PrealocarLimite(OrdemInfo ordem, bool alteracao = false)
        {
            try
            {
                int conta = ordem.Account;

                logger.Debug("PrealocandoLimite [" + conta + "]");

                lock (_limiteClientesBVSP)
                {
                    if (_limiteClientesBVSP.ContainsKey(conta) == false)
                    {
                        logger.Error("PrealocarLimite: conta[" + conta + "] sem limite cadastrado");
                        return false;
                    }

                    LimiteClienteInfo limiteCliente = _limiteClientesBVSP[conta];

                    if (limiteCliente.flgContaMasterBovespa)
                    {
                        logger.InfoFormat("Conta [{0}] Ordem[{1}] master Bovespa, ignorando verif limites", conta, ordem.ClOrdID);
                        return true;
                    }

                    Decimal volumeOrdem = Convert.ToDecimal(ordem.OrderQty) * Convert.ToDecimal(ordem.Price);
                    Decimal limiteConsumidoTotal = limiteCliente.valorConsumido + limiteCliente.valorPreAlocado;
                    Decimal saldo = limiteCliente.valorTotal - (limiteConsumidoTotal + volumeOrdem);
                    Decimal volumeOrdemOriginal = 0;

                    // Se for alteracao de ordem, 
                    if (alteracao)
                    {
                        OrdemInfo ordemoriginal = _hstOrders[ordem.OrigClOrdID] as OrdemInfo;

                        if (ordemoriginal != null)
                        {
                            volumeOrdemOriginal = Convert.ToDecimal(ordemoriginal.OrderQty - ordemoriginal.CumQty) * Convert.ToDecimal(ordemoriginal.Price);
                        }
                    }

                    logger.DebugFormat("PrealocarLimite: Conta [{0}] Limite [{1}] Consumido [{2}] Solicitado[{3}] PreSaldo [{4}]",
                        conta,
                        limiteCliente.valorTotal,
                        limiteConsumidoTotal,
                        volumeOrdem,
                        saldo);

                    if (limiteConsumidoTotal + volumeOrdem > limiteCliente.valorTotal)
                    {
                        logger.ErrorFormat("PrealocarLimite: Conta [{0}] Limite [{1}] Consumido [{2}] Solicitado[{3}] Saldo [{4}] estourou prealocacao do limite operacional",
                            conta,
                            limiteCliente.valorTotal,
                            limiteConsumidoTotal,
                            volumeOrdem,
                            saldo);

                        return false;
                    }
                    
                    StoreOrder(ordem.ClOrdID, ordem);

                    limiteCliente.valorPreAlocado += (volumeOrdem-volumeOrdemOriginal);

                    _limiteClientesBVSP[conta] = limiteCliente;

                    //LimiteMovimentoInfo mov = new LimiteMovimentoInfo();

                    //mov.Account = ordem.Account;
                    //mov.dataMovimento = DateTime.Now;
                    //mov.instrumento = ordem.Symbol;
                    //mov.quantidade = ordem.OrderQty;
                    //mov.valorConsumido = limiteCliente.valorConsumido;
                    //mov.valorPrealocado = limiteCliente.valorPreAlocado;
                    //mov.valorRestante = limiteCliente.valorTotal - limiteCliente.valorConsumido;  // limiteCliente.valorPreAlocado)
                    //mov.valorTotal = limiteCliente.valorTotal;

                    logger.DebugFormat("PrealocarLimite: Conta [{0}] Limite [{1}] Consumido [{2}] Prealocado[{3}] Restante[{4}]",
                        conta,
                        limiteCliente.valorTotal,
                        limiteCliente.valorConsumido,
                        limiteCliente.valorPreAlocado,
                        limiteCliente.valorRestante);

                    //lock (qMovPersist)
                    //{
                    //    qMovPersist.Enqueue(mov);
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.Error("PrealocarLimite(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <param name="alteracao"></param>
        /// <returns></returns>
        public bool RecalcularLimite(OrdemInfo ordem)
        {
            try
            {
                int conta = ordem.Account;

                logger.Debug("RecalcularLimite [" + conta + "]");

                lock (_limiteClientesBVSP)
                {
                    if (_limiteClientesBVSP.ContainsKey(conta) == false)
                    {
                        logger.Error("Erro: conta[" + conta + "] sem limite cadastrado");
                        return false;
                    }

                    LimiteClienteInfo limiteCliente = _limiteClientesBVSP[conta];

                    if (limiteCliente.flgContaMasterBovespa)
                    {
                        logger.InfoFormat("Conta [{0}] Ordem[{1}] master Bovespa, ignorando verif limites", conta, ordem.ClOrdID);
                        return true;
                    }

                    Decimal volumeOrdem = Convert.ToDecimal(ordem.OrderQty) * Convert.ToDecimal(ordem.Price);
                    Decimal saldo = limiteCliente.valorTotal - (limiteCliente.valorConsumido + volumeOrdem);
                    Decimal volumeOrdemOriginal = 0;

                    if (ordem.OrdStatus == OrdemStatusEnum.NOVA)
                    {
                        logger.DebugFormat("RecalcularLimite: Conta [{0}] Limite [{1}] Consumido [{2}] Solicitado[{3}] PreSaldo [{4}]",
                            conta,
                            limiteCliente.valorTotal,
                            limiteCliente.valorConsumido,
                            volumeOrdem,
                            saldo);

                        //TODO: teoricamente, nao devo cair nessa condicao, ja houve checagem pela 
                        // prealocacao
                        if (limiteCliente.valorConsumido + volumeOrdem > limiteCliente.valorTotal)
                        {

                            logger.ErrorFormat("Conta [{0}] Limite [{1}] Consumido [{2}] Solicitado[{3}] Saldo [{4}] estourou limite operacional",
                                conta,
                                limiteCliente.valorTotal,
                                limiteCliente.valorConsumido,
                                volumeOrdem,
                                saldo);

                            return false;
                        }

                        // Libera prealocacao e recalcula limite
                        limiteCliente.valorPreAlocado -= volumeOrdem;
                        limiteCliente.valorConsumido += volumeOrdem;
                        limiteCliente.valorRestante = limiteCliente.valorTotal - limiteCliente.valorConsumido;
                    }


                    if ( ordem.OrdStatus == OrdemStatusEnum.PARCIALMENTEEXECUTADA ||
                        ordem.OrdStatus == OrdemStatusEnum.EXECUTADA )
                    {
                        Decimal precoExec = ordem.Acompanhamentos[0].LastPx;
                        Decimal qtdeExec = Convert.ToDecimal(ordem.Acompanhamentos[0].QuantidadeNegociada);
                        Decimal volumeExec = precoExec * qtdeExec;

                        OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.ClOrdID);

                        volumeOrdemOriginal = Convert.ToDecimal(ordemOriginal.Price) * Convert.ToDecimal(ordemOriginal.OrderQty - ordemOriginal.CumQty);
                        Decimal volumeRemanescente = Convert.ToDecimal(ordemOriginal.Price) * Convert.ToDecimal(ordem.Acompanhamentos[0].QuantidadeRemanescente);

                        saldo = limiteCliente.valorTotal - (limiteCliente.valorConsumido + volumeRemanescente + volumeExec);

                        logger.DebugFormat("RecLim(PARCEXEC): Conta [{0}] Limite [{1}] Consumido [{2}] Original[{3}] Executado [{4}] Remanescente[{5}] Saldo[{6}]",
                            conta,
                            limiteCliente.valorTotal,
                            limiteCliente.valorConsumido,
                            volumeOrdemOriginal,
                            volumeExec,
                            volumeRemanescente,
                            saldo);

                        limiteCliente.valorConsumido -= volumeOrdemOriginal;
                        limiteCliente.valorConsumido += volumeRemanescente + volumeExec;
                        limiteCliente.valorRestante = limiteCliente.valorTotal - limiteCliente.valorConsumido;

                        ordemOriginal.CumQty = ordem.CumQty;
                        ordemOriginal.OrderQtyRemmaining = ordem.Acompanhamentos[0].QuantidadeRemanescente;
                        if (ordem.OrdStatus == OrdemStatusEnum.EXECUTADA)
                            RemoveOrder(ordemOriginal.ClOrdID);
                        //else // Parcialmente executada
                        //    StoreOrder(ordemOriginal.ClOrdID, ordemOriginal);
                    }



                    if (ordem.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                    {
                        OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.OrigClOrdID);
                        //OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.ClOrdID);

                        if (ordemOriginal != null)
                        {
                            volumeOrdemOriginal = Convert.ToDecimal(ordemOriginal.OrderQty - ordemOriginal.CumQty) * Convert.ToDecimal(ordemOriginal.Price);
                            saldo = limiteCliente.valorTotal - (limiteCliente.valorConsumido + volumeOrdem - volumeOrdemOriginal);

                            logger.DebugFormat("RecLim SUBST: Conta [{0}] Limite [{1}] Consumido [{2}] Solicitado[{3}] PreSaldo [{4}] Original [{5}]",
                                conta,
                                limiteCliente.valorTotal,
                                limiteCliente.valorConsumido,
                                volumeOrdem,
                                saldo,
                                volumeOrdemOriginal);
                        }
                        else
                        {
                            logger.Error("Erro: Ordem original não armazenada, alocando volume total da ordem a ser modificada");
                        }

                        // Libera prealocacao e recalcula limite
                        limiteCliente.valorPreAlocado -= (volumeOrdem - volumeOrdemOriginal);
                        limiteCliente.valorConsumido += (volumeOrdem - volumeOrdemOriginal);
                        limiteCliente.valorRestante = limiteCliente.valorTotal - limiteCliente.valorConsumido;

                        RemoveOrder(ordem.OrigClOrdID);
                    }

                    _limiteClientesBVSP[conta] = limiteCliente;

                    LimiteMovimentoInfo mov = new LimiteMovimentoInfo();

                    mov.Account = ordem.Account;
                    mov.dataMovimento = DateTime.Now;
                    mov.instrumento = ordem.Symbol;
                    mov.quantidade = ordem.OrderQty;
                    mov.Preco = Convert.ToDecimal(ordem.Price);
                    mov.idLancamento = ordem.Side == OrdemDirecaoEnum.Compra ? 1 : 2;
                    mov.idLimite = limiteCliente.idLimite;
                    mov.valorConsumido = limiteCliente.valorConsumido;
                    mov.valorPrealocado = limiteCliente.valorPreAlocado;
                    mov.valorRestante = limiteCliente.valorTotal - limiteCliente.valorConsumido;  // limiteCliente.valorPreAlocado)
                    mov.valorTotal = limiteCliente.valorTotal;

                    logger.DebugFormat("RecalcularLimite: Conta [{0}] Limite [{1}] Consumido [{2}] Prealocado[{3}]  Restante [{4}] para persistencia",
                        conta,
                        limiteCliente.valorTotal,
                        limiteCliente.valorConsumido,
                        limiteCliente.valorPreAlocado,
                        limiteCliente.valorRestante);

                    lock (qMovPersist)
                    {
                        qMovPersist.Enqueue(mov);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("PrealocarLimite(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private OrdemInfo _obterOrdemOriginal(string clordid)
        {
            OrdemInfo ret = null;
            lock (_hstOrders)
            {
                if (_hstOrders.ContainsKey(clordid))
                    ret = _hstOrders[clordid] as OrdemInfo;
            }

            return ret;
        }

        /// <summary>
        /// Armazena a ordem para recalculos futuros
        /// </summary>
        /// <param name="order"></param>
        public void StoreOrder(string clordid, OrdemInfo order)
        {
            logger.DebugFormat("StoreOrder: [{0}] [{1}] [{2}] Qtd:[{3}] Rest:[{4}] Cum:[{5}] Price:[{6}]",
                order.ClOrdID,
                order.Account,
                order.Symbol,
                order.OrderQty,
                order.OrderQtyRemmaining,
                order.CumQty,
                order.Price);

            lock (_hstOrders)
            {
                if (_hstOrders.ContainsKey(clordid))
                    _hstOrders[clordid] = order;
                else
                    _hstOrders.Add(clordid, order);
            }

            logger.Debug("StoreOrder: _hstOrders.Count: " + _hstOrders.Count);
        }

        /// <summary>
        /// Remover ordens que não sao mais utilizadas para calculos futuros
        /// </summary>
        /// <param name="clordid"></param>
        /// <param name="order"></param>
        public void RemoveOrder(string clordid)
        {
            OrdemInfo order = null;
            lock (_hstOrders)
            {
                if (_hstOrders.ContainsKey(clordid))
                {
                    order =(OrdemInfo) _hstOrders[clordid];
                    _hstOrders.Remove(clordid);
                }
            }
            
            logger.DebugFormat("RemoveOrder: [{0}] [{1}] [{2}] Qtd:[{3}] Rest:[{4}] Cum:[{5}] Price:[{6}]",
                order.ClOrdID,
                order.Account,
                order.Symbol,
                order.OrderQty,
                order.OrderQtyRemmaining,
                order.CumQty,
                order.Price);
            order = null;
            logger.Debug("RemoveOrder: _hstOrders.Count: " + _hstOrders.Count);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        public bool EstornarLimite(OrdemInfo ordem)
        {
            try
            {
                int conta = ordem.Account;

                logger.Debug("EstornarLimite [" + conta + "]");

                lock (_limiteClientesBVSP)
                {
                    if (_limiteClientesBVSP.ContainsKey(conta) == false)
                    {
                        logger.Error("Erro: conta[" + conta + "] sem limite cadastrado");
                        return false;
                    }

                    LimiteClienteInfo limiteCliente = _limiteClientesBVSP[conta];

                    if (limiteCliente.flgContaMasterBovespa)
                    {
                        logger.InfoFormat("Conta [{0}] Ordem[{1}] master Bovespa, ignorando verif limites", conta, ordem.ClOrdID);
                        return true;
                    }

                    Decimal volumeEstorno = Convert.ToDecimal(ordem.OrderQty - ordem.CumQty) * Convert.ToDecimal(ordem.Price);
                    Decimal volumeOriginal = 0;

                    // Verificar se eh estorno por rejeicao ou cancelamento de ordem
                    // se for rejeicao, estorna da prealocacao, caso contrario, estorna no limite total
                    // Libera prealocacao e recalcula limite

                    if (ordem.OrdStatus == OrdemStatusEnum.CANCELADA)
                    {
                        logger.DebugFormat("EstornarLimite: estornar [{0}] do limite CONSUMIDO", volumeEstorno);
                        limiteCliente.valorConsumido -= volumeEstorno;
                        RemoveOrder(ordem.OrigClOrdID);
                    }
                    else
                    {
                        logger.DebugFormat("EstornarLimite: estornar [{0}] do limite PREALOCADO", volumeEstorno);

                        // Se for uma rejeicao de substituicao, tem que devolver a diferenca somente
                        if (ordem.OrigClOrdID != null && ordem.OrigClOrdID.Length > 0)
                        {
                            OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.OrigClOrdID);

                            if (ordemOriginal != null)
                            {
                                volumeOriginal = Convert.ToDecimal(ordemOriginal.OrderQty - ordemOriginal.CumQty) * Convert.ToDecimal(ordemOriginal.Price); 
                            }
                        }
                        limiteCliente.valorPreAlocado -= (volumeEstorno - volumeOriginal);
                    }

                    limiteCliente.valorRestante = limiteCliente.valorTotal - limiteCliente.valorConsumido;
                    _limiteClientesBVSP[conta] = limiteCliente;

                    LimiteMovimentoInfo mov = new LimiteMovimentoInfo();

                    mov.Account = ordem.Account;
                    mov.dataMovimento = DateTime.Now;
                    mov.instrumento = ordem.Symbol;
                    mov.quantidade = ordem.OrderQty;
                    mov.Preco = Convert.ToDecimal(ordem.Price);
                    mov.idLancamento = ordem.Side == OrdemDirecaoEnum.Compra ? 1 : 2;
                    mov.idLimite = limiteCliente.idLimite;
                    mov.valorConsumido = limiteCliente.valorConsumido;
                    mov.valorPrealocado = limiteCliente.valorPreAlocado;
                    mov.valorRestante = limiteCliente.valorTotal - limiteCliente.valorConsumido;  // limiteCliente.valorPreAlocado)
                    mov.valorTotal = limiteCliente.valorTotal;

                    logger.DebugFormat("EstornarLimite: Conta [{0}] Limite [{1}] Consumido [{2}] Prealocado[{3}] Restante[{4}] para persistencia",
                        conta,
                        limiteCliente.valorTotal,
                        limiteCliente.valorConsumido,
                        limiteCliente.valorPreAlocado,
                        limiteCliente.valorRestante);


                    lock (qMovPersist)
                    {
                        qMovPersist.Enqueue(mov);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("EstornoLimite(): " + ex.Message, ex);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Thread para gravacao dos movimentos no limite
        /// 
        /// </summary>
        public void _procPersisteciaMovimento()
        {
            while (_bKeepRunning)
            {
                if (qMovPersist.Count > 0 || qMovInstBMF.Count > 0 ||  qMovContBMF.Count > 0 )
                {
                    // Movimentos de limite BVSP
                    if (qMovPersist.Count > 0)
                    {
                        List<LimiteMovimentoInfo> movimentos = new List<LimiteMovimentoInfo>();
                        lock (qMovPersist)
                        {
                            movimentos.AddRange(qMovPersist.ToArray());
                            qMovPersist.Clear();
                        }

                        //CamadaDeDados db = new CamadaDeDados();
                        foreach (LimiteMovimentoInfo info in movimentos)
                        {
                            if (!_writeMovementToFile)
                            {
                                if (_updateDB)
                                {
                                    if (_db.InserirMovimentoLimite(info) == false)
                                    {
                                        //TODO: dumpear o conteudo
                                        logger.Error("Erro ao inserir movto ");
                                    }
                                }
                            }
                            // Gravar em arquivo
                            else
                            {
                                 string key = EXCHANGEBOVESPA + info.Account.ToString();
                                 if(_movPersistencia.ContainsKey(key))
                                 {
                                     if (_movPersistencia[key].InserirMovimentoLimite(info) == false)
                                     {
                                         logger.Error("Erro ao inserir movto no arquivo");
                                     }
                                     // Gravado movimento, entao atualizar a tabela de limites 
                                     // (conforme procedure prc_fixsrv_ins_movimento_limite, ha a insercao do movimento e depois atualiza 
                                     // o limite. Neste caso atualizaremos somente os valores do limite)
                                     //else
                                     //{
                                     //    if (!db.AtualizarClienteLimite(info.Account, info.valorTotal, info.valorConsumido, info.valorRestante))
                                     //    {
                                     //        logger.Error("Erro ao atualizar o limite do cliente");
                                     //    }
                                     //}
                                 }
                            }
                        }
                        // Se esta gravando em arquivo, entao necessita atualizar a posicao da tabela tb_limite
                        // (conforme procedure prc_fixsrv_ins_movimento_limite, ha a insercao do movimento e depois atualiza 
                        // o tb_limite. Neste caso atualizaremos somente o valor do ultimo registro da colecao, pois eh o mais atualizado, 
                        // evitando acessos repetitivos na base)
                        if (_writeMovementToFile)
                        {
                            LimiteMovimentoInfo aux = movimentos[movimentos.Count - 1];
                            if (_updateDB)
                            {
                                if (!_db.AtualizarClienteLimite(aux.Account, aux.valorTotal, aux.valorConsumido, aux.valorRestante))
                                {
                                    logger.Error("Erro ao atualizar o limite do cliente");
                                }
                            }
                        }
                        int len = movimentos.Count;
                        for (int x = 0; x < len; x++)
                        {
                            movimentos[x] = null;
                        }
                        movimentos.Clear();
                        movimentos = null;
                        //db = null;
                    }

                    //Movimentacao dos limites de instrumentos BMF
                    if (qMovInstBMF.Count > 0 )
                    {
                        List<LimiteClienteInstrumentoBMF> movimentos = new List<LimiteClienteInstrumentoBMF>();
                        lock (qMovInstBMF)
                        {
                            movimentos.AddRange(qMovInstBMF.ToArray());
                            qMovInstBMF.Clear();
                        }

                        // CamadaDeDados db = new CamadaDeDados();
                        foreach (LimiteClienteInstrumentoBMF info in movimentos)
                        {
                            
                            if (!_writeMovementToFile)
                            {
                                // Sempre atualizar as informacoes de limite da tabela do banco de dados
                                // (manter versao original)
                                if (_updateDB)
                                {
                                    if (_db.InserirMovimentoLimiteInstrumentoBMF(info) == false)
                                    {
                                        //TODO: dumpear o conteudo
                                        logger.Error("Erro ao inserir movto de limite instrumento BMF ");
                                    }
                                }
                            }
                            else
                            {
                                string key = EXCHANGEBMF + info.account.ToString();
                                if (_movPersistencia.ContainsKey(key))
                                {
                                    if (_movPersistencia[key].InserirMovimentoLimiteInstrumentoBMF(info) == false)
                                    {
                                        logger.Error("Erro ao inserir movto de limite instrumento BMF no arquivo");
                                    }
                                }
                            }
                        }
                        // Atualiar o limite bmf usando somente o ultimo registro da lista,
                        // pois onde contem os valores mais atualizados
                        if (_writeMovementToFile)
                        {
                            LimiteClienteInstrumentoBMF aux = movimentos[movimentos.Count - 1];
                            if (_updateDB)
                            {
                                if (_db.InserirMovimentoLimiteInstrumentoBMF(aux) == false)
                                {
                                    //TODO: dumpear o conteudo
                                    logger.Error("Erro ao inserir movto de limite instrumento BMF ");
                                }
                            }
                        }
                        int len = movimentos.Count;
                        for (int x = 0; x < len; x++)
                        {
                            movimentos[x] = null;
                        }
                        movimentos.Clear();
                        movimentos = null;
                        //db = null;
                        
                    }

                    //Movimentacao dos limites de contrato BMF
                    if (qMovContBMF.Count > 0)
                    {
                        List<LimiteClienteContratoBMF> movimentos = new List<LimiteClienteContratoBMF>();
                        lock (qMovContBMF)
                        {
                            movimentos.AddRange(qMovContBMF.ToArray());
                            qMovContBMF.Clear();
                        }

                        // CamadaDeDados db = new CamadaDeDados();
                        foreach (LimiteClienteContratoBMF info in movimentos)
                        {

                            if (!_writeMovementToFile)
                            {
                                if (_updateDB)
                                {
                                    if (_db.InserirMovimentoLimiteContratoBMF(info) == false)
                                    {
                                        //TODO: dumpear o conteudo
                                        logger.Error("Erro ao inserir movto de limite de contrato BMF ");
                                    }
                                }
                            }
                            else
                            {
                                string key = EXCHANGEBMF + info.account.ToString();
                                if (_movPersistencia.ContainsKey(key))
                                    if (_movPersistencia[key].InserirMovimentoLimiteContratoBMF(info) == false)
                                        logger.Error("Erro ao inserir movto de limite de contrato BMF no arquivo");
                            }
                        }
                        if (_writeMovementToFile)
                        {
                            LimiteClienteContratoBMF aux = movimentos[movimentos.Count - 1];
                            if (_updateDB)
                            {
                                if (_db.InserirMovimentoLimiteContratoBMF(aux) == false)
                                {
                                    //TODO: dumpear o conteudo
                                    logger.Error("Erro ao inserir movto de limite instrumento BMF ");
                                }
                            }
                        }
                        int len = movimentos.Count;
                        for (int x = 0; x < len; x++)
                        {
                            movimentos[x] = null;
                        }
                        movimentos.Clear();
                        movimentos = null;
                        // db = null;
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        public bool VerificarOrdemAdministrada(OrdemInfo ordem)
        {
            bool ret = false;
            lock (_limiteClientesBVSP)
            {
                if (_limiteClientesBVSP.ContainsKey(ordem.Account))
                {
                    LimiteClienteInfo info = _limiteClientesBVSP[ordem.Account];
                    ret = info.flgClienteAdministrada;
                }
            }

            return ret;
        }


        public bool VerificarOrdemRepasse(OrdemInfo ordem)
        {
            bool ret = false;
            lock (_limiteClientesBMF)
            {
                if (_limiteClientesBMF.ContainsKey(ordem.Account))
                {
                    LimiteClienteInfo info = _limiteClientesBMF[ordem.Account];
                    ret = info.flgClienteRepasse;
                }
            }

            return ret;
        }

        /// <summary>
        /// Prealoca limite baseado na qtde de contratos, primeiramente
        /// pelos limites estabelecidos por instrumento, caso nao 
        /// exista, pelo contrato pai
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        public bool PrealocarLimiteBMF(OrdemInfo ordem, bool bAlteracao=false)
        {
            bool ret = false;

            if (ordem.Side == OrdemDirecaoEnum.Compra)
                ret = _prealocarLimiteBMFCompra(ordem, bAlteracao);
            else
                ret = _prealocarLimiteBMFVenda(ordem, bAlteracao);


            return ret;
        }

        /// <summary>
        /// Prealoca limite baseado na qtde de contratos, primeiramente
        /// pelos limites estabelecidos por instrumento, caso nao 
        /// exista, pelo contrato pai
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        public bool RecalcularLimiteBMF(OrdemInfo ordem)
        {
            bool ret = false;

            if (ordem.Side == OrdemDirecaoEnum.Compra)
                ret = _recalcularLimiteBMFCompra(ordem);
            else
                ret = _recalcularLimiteBMFVenda(ordem);


            return ret;
        }


        private bool _recalcularLimiteBMFCompra(OrdemInfo ordem)
        {
            string instrumento = ordem.Symbol;
            string contrato = instrumento.Substring(0, 3);
            int conta = ordem.Account;


            LimiteClienteInstrumentoBMF limiteInst = _obterLimiteInstrumentoCompra(conta, instrumento);
            LimiteClienteContratoBMF limiteCont = _obterLimiteContratoCompra(conta, contrato);
            if (limiteInst == null && limiteCont == null)
            {
                logger.ErrorFormat("COMPRA: Nao ha limite cadastrado para instrumento ou contrato. Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}]",
                    ordem.Account,
                    ordem.Symbol,
                    ordem.OrderQty,
                    ordem.Price);

                return false;
            }

            logger.InfoFormat("RECCOMPRA: Acc[{0}] Sym[{1}] Prc[{2}]  Qtde[{3}] LTot[{4}] LDisp[{5}] LPre[{6}] Stat[{7}]",
                ordem.Account,
                ordem.Symbol,
                ordem.Price,
                ordem.OrderQty,
                limiteCont.qtTotal,
                limiteCont.qtDisponivel,
                limiteCont.qtPrealocado,
                ordem.OrdStatus.ToString());

            if (ordem.OrdStatus == OrdemStatusEnum.NOVA)
            {
                limiteCont.qtPrealocado -= ordem.OrderQty;
                limiteCont.qtDisponivel -= ordem.OrderQty;

                if (limiteInst != null)
                {
                    limiteInst.qtPrealocado -= ordem.OrderQty;
                    limiteInst.qtDispInstrumento -= ordem.OrderQty;
                    limiteInst.qtDispContrato = limiteCont.qtDisponivel;

                    _atualizarLimiteInstrumentoBMFCompra(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFCompra(conta, limiteCont);
            }

            if (ordem.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
            {
                int qtdeOriginal = 0;

                OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.OrigClOrdID);

                if (ordemOriginal != null)
                {
                    qtdeOriginal = ordemOriginal.OrderQty;
                }

                int qtdeSolicitada = ordem.OrderQty - qtdeOriginal;

                limiteCont.qtPrealocado -= qtdeSolicitada;
                limiteCont.qtDisponivel -= qtdeSolicitada;

                if (limiteInst != null)
                {
                    limiteInst.qtPrealocado -= qtdeSolicitada;
                    limiteInst.qtDispInstrumento -= qtdeSolicitada;
                    limiteInst.qtDispContrato = limiteCont.qtDisponivel;

                    _atualizarLimiteInstrumentoBMFCompra(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFCompra(conta, limiteCont);
                RemoveOrder(ordem.OrigClOrdID);
            }

            if (ordem.OrdStatus == OrdemStatusEnum.EXECUTADA ||
                ordem.OrdStatus == OrdemStatusEnum.PARCIALMENTEEXECUTADA)
            {
                ordem.CumQty = ordem.CumQty;
                ordem.OrderQtyRemmaining = ordem.Acompanhamentos[0].QuantidadeRemanescente;

                if (ordem.OrdStatus == OrdemStatusEnum.EXECUTADA)
                    RemoveOrder(ordem.ClOrdID);
            }
            
            logger.InfoFormat("RECCOMPRA: Acc[{0}] Sym[{1}] Prc[{2}]  Qtde[{3}] LTot[{4}] LDisp[{5}] LPre[{6}], enfileirando para persistencia",
                ordem.Account,
                ordem.Symbol,
                ordem.Price,
                ordem.OrderQty,
                limiteCont.qtTotal,
                limiteCont.qtDisponivel,
                limiteCont.qtPrealocado);

            // StoreOrder(ordem.ClOrdID, ordem);

            if (limiteInst != null)
            {
                lock (qMovInstBMF)
                {
                    qMovInstBMF.Enqueue(limiteInst);
                }
            }

            lock (qMovContBMF)
            {
                qMovContBMF.Enqueue(limiteCont);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        private bool _recalcularLimiteBMFVenda(OrdemInfo ordem)
        {
            string instrumento = ordem.Symbol;
            string contrato = instrumento.Substring(0, 3);
            int conta = ordem.Account;


            LimiteClienteInstrumentoBMF limiteInst = _obterLimiteInstrumentoVenda(conta, instrumento);
            LimiteClienteContratoBMF limiteCont = _obterLimiteContratoVenda(conta, contrato);
            if (limiteInst == null && limiteCont == null)
            {
                logger.ErrorFormat("VENDA: Nao ha limite cadastrado para instrumento ou contrato. Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}]",
                    ordem.Account,
                    ordem.Symbol,
                    ordem.OrderQty,
                    ordem.Price);

                return false;
            }

            logger.InfoFormat("RECVENDA: Acc[{0}] Sym[{1}] Prc[{2}]  Qtde[{3}] LTot[{4}] LDisp[{5}] LPre[{6}] Stat[{7}]",
                ordem.Account,
                ordem.Symbol,
                ordem.Price,
                ordem.OrderQty,
                limiteCont.qtTotal,
                limiteCont.qtDisponivel,
                limiteCont.qtPrealocado,
                ordem.OrdStatus.ToString());

            if (ordem.OrdStatus == OrdemStatusEnum.NOVA)
            {
                limiteCont.qtPrealocado -= ordem.OrderQty;
                limiteCont.qtDisponivel -= ordem.OrderQty;

                if (limiteInst != null)
                {
                    limiteInst.qtPrealocado -= ordem.OrderQty;
                    limiteInst.qtDispInstrumento -= ordem.OrderQty;
                    limiteInst.qtDispContrato = limiteCont.qtDisponivel;

                    _atualizarLimiteInstrumentoBMFVenda(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFVenda(conta, limiteCont);
            }

            if (ordem.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
            {
                int qtdeOriginal = 0;

                OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.OrigClOrdID);

                if (ordemOriginal != null)
                {
                    qtdeOriginal = ordemOriginal.OrderQty;
                }

                int qtdeSolicitada = ordem.OrderQty - qtdeOriginal;

                limiteCont.qtPrealocado -= qtdeSolicitada;
                limiteCont.qtDisponivel -= qtdeSolicitada;

                if (limiteInst != null)
                {
                    limiteInst.qtPrealocado -= qtdeSolicitada;
                    limiteInst.qtDispInstrumento -= qtdeSolicitada;
                    limiteInst.qtDispContrato = limiteCont.qtDisponivel;

                    _atualizarLimiteInstrumentoBMFVenda(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFVenda(conta, limiteCont);
                RemoveOrder(ordem.OrigClOrdID);
            }

            if (ordem.OrdStatus == OrdemStatusEnum.EXECUTADA ||
                ordem.OrdStatus == OrdemStatusEnum.PARCIALMENTEEXECUTADA)
            {
                ordem.CumQty = ordem.CumQty;
                ordem.OrderQtyRemmaining = ordem.Acompanhamentos[0].QuantidadeRemanescente;
                
                if (ordem.OrdStatus == OrdemStatusEnum.EXECUTADA)
                    RemoveOrder(ordem.ClOrdID);
            }

            logger.InfoFormat("RECVENDA: Acc[{0}] Sym[{1}] Prc[{2}]  Qtde[{3}] LTot[{4}] LDisp[{5}] LPre[{6}], enfileirando para persistencia",
                ordem.Account,
                ordem.Symbol,
                ordem.Price,
                ordem.OrderQty,

                limiteCont.qtTotal,
                limiteCont.qtDisponivel,
                limiteCont.qtPrealocado);

            // StoreOrder(ordem.ClOrdID, ordem);

            if (limiteInst != null)
            {
                lock (qMovInstBMF)
                {
                    qMovInstBMF.Enqueue(limiteInst);
                }
            }

            lock (qMovContBMF)
            {
                qMovContBMF.Enqueue(limiteCont);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        private bool _prealocarLimiteBMFCompra(OrdemInfo ordem, bool bAlteracao)
        {
            string instrumento = ordem.Symbol;
            string contrato = instrumento.Substring(0, 3);
            int conta = ordem.Account;


            LimiteClienteInstrumentoBMF limiteInst = _obterLimiteInstrumentoCompra(conta, instrumento);
            LimiteClienteContratoBMF limiteCont = _obterLimiteContratoCompra(conta, contrato);
            if (limiteInst == null && limiteCont==null)
            {
                logger.ErrorFormat("COMPRA: Nao ha limite cadastrado para instrumento ou contrato. Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}]",
                    ordem.Account,
                    ordem.Symbol,
                    ordem.OrderQty,
                    ordem.Price);

                return false;
            }

            
            int qtdeDistTotalInstr = 0;
            int qtdeDistTotalCont = limiteCont.qtDisponivel - limiteCont.qtPrealocado;
            int qtdeOriginal = 0;

            if (bAlteracao)
            {
                OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.OrigClOrdID);

                if (ordemOriginal != null)
                {
                    qtdeOriginal = ordemOriginal.OrderQty;
                }
            }

            if (limiteInst != null)
            {
                qtdeDistTotalInstr = limiteInst.qtDispInstrumento - limiteInst.qtPrealocado;

                if (qtdeDistTotalInstr < (ordem.OrderQty-qtdeOriginal))
                {
                    logger.ErrorFormat("COMPRA: Atingiu limite para instrumento Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}], verificando somatorio para o contrato",
                        ordem.Account,
                        ordem.Symbol,
                        ordem.OrderQty,
                        ordem.Price);

                    return false;
                }
            }

            // Se nao tem limite por instrumento definido, verifica no pai
            if (limiteInst == null && qtdeDistTotalCont < (ordem.OrderQty - qtdeOriginal))
            {
                logger.ErrorFormat("COMPRA: Atingiu limite contrato pai do instrumento Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}]",
                    ordem.Account,
                    ordem.Symbol,
                    ordem.OrderQty,
                    ordem.Price);

                return false;
            }


            StoreOrder(ordem.ClOrdID, ordem);

            limiteCont.qtPrealocado += (ordem.OrderQty-qtdeOriginal);
            _atualizarLimiteContratoBMFCompra(conta, limiteCont);

            if (limiteInst != null)
            {
                limiteInst.qtPrealocado += (ordem.OrderQty - qtdeOriginal);
            
                _atualizarLimiteInstrumentoBMFCompra(conta, limiteInst);
            }


            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        private bool _prealocarLimiteBMFVenda(OrdemInfo ordem, bool bAlteracao)
        {

            string instrumento = ordem.Symbol;
            string contrato = instrumento.Substring(0, 3);
            int conta = ordem.Account;


            LimiteClienteInstrumentoBMF limiteInst = _obterLimiteInstrumentoVenda(conta, instrumento);
            LimiteClienteContratoBMF limiteCont = _obterLimiteContratoVenda(conta, contrato);
            if (limiteInst == null && limiteCont == null)
            {
                logger.ErrorFormat("VENDA:  Nao ha limite cadastrado para instrumento ou contrato. Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}]",
                    ordem.Account,
                    ordem.Symbol,
                    ordem.OrderQty,
                    ordem.Price);

                return false;
            }

            int qtdeDistTotalInstr = 0;
            int qtdeDistTotalCont = limiteCont.qtDisponivel - limiteCont.qtPrealocado;
            int qtdeOriginal = 0;

            if (bAlteracao)
            {
                OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.OrigClOrdID);

                if (ordemOriginal != null)
                {
                    qtdeOriginal = ordemOriginal.OrderQty;
                }
            }


            if (limiteInst != null )
            {
                qtdeDistTotalInstr = limiteInst.qtDispInstrumento - limiteInst.qtPrealocado;
                if (qtdeDistTotalInstr < (ordem.OrderQty - qtdeOriginal) )
                {
                    logger.ErrorFormat("VENDA: Atingiu limite para instrumento Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}], verificando somatorio para o contrato",
                        ordem.Account,
                        ordem.Symbol,
                        ordem.OrderQty,
                        ordem.Price);

                    return false;
                }
            }

            // Se nao tem limite por instrumento definido, verifica no pai
            if (limiteInst == null && qtdeDistTotalCont < (ordem.OrderQty - qtdeOriginal))
            {
                logger.ErrorFormat("VENDA: Atingiu limite contrato pai do instrumento Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}]",
                    ordem.Account,
                    ordem.Symbol,
                    ordem.OrderQty,
                    ordem.Price);

                return false;
            }

            StoreOrder(ordem.ClOrdID, ordem);

            limiteCont.qtPrealocado += ordem.OrderQty;

            if (limiteInst != null)
            {
                limiteInst.qtPrealocado += ordem.OrderQty;

                _atualizarLimiteInstrumentoBMFVenda(conta, limiteInst);
            }

            _atualizarLimiteContratoBMFVenda(conta, limiteCont);

            return true;
        }


        /// <summary>
        /// Estorna limites na ocasiao de cancelamento ou rejeicao de ofertas
        /// 
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns></returns>
        public bool EstornarLimiteBMF(OrdemInfo ordem)
        {
            bool ret = false;

            if (ordem.Side == OrdemDirecaoEnum.Compra)
                ret = _estornarLimiteBMFCompra(ordem);
            else
                ret = _estornarLimiteBMFVenda(ordem);


            return ret;
        }

        private bool _estornarLimiteBMFCompra(OrdemInfo ordem)
        {
            string instrumento = ordem.Symbol;
            string contrato = instrumento.Substring(0, 3);
            int conta = ordem.Account;


            LimiteClienteInstrumentoBMF limiteInst = _obterLimiteInstrumentoCompra(conta, instrumento);
            LimiteClienteContratoBMF limiteCont = _obterLimiteContratoCompra(conta, contrato);
            if (limiteInst == null && limiteCont == null)
            {
                logger.ErrorFormat("ESTCOMPRA: Nao ha limite cadastrado para instrumento ou contrato. Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}]",
                    ordem.Account,
                    ordem.Symbol,
                    ordem.OrderQty,
                    ordem.Price);

                return false;
            }

            logger.InfoFormat("ESTCOMPRA: Acc[{0}] Sym[{1}] Prc[{2}]  Qtde[{3}] LTot[{4}] LDisp[{5}] LPre[{6}] Stat[{7}]",
                ordem.Account,
                ordem.Symbol,
                ordem.Price,
                ordem.OrderQty,
                limiteCont.qtTotal,
                limiteCont.qtDisponivel,
                limiteCont.qtPrealocado,
                ordem.OrdStatus.ToString());

            // Verificar se eh estorno por rejeicao ou cancelamento de ordem
            // se for rejeicao, estorna da prealocacao, caso contrario, estorna no limite total
            // Libera prealocacao e recalcula limite

            if (ordem.OrdStatus == OrdemStatusEnum.CANCELADA)
            {
                // Em mensagem cancelada, o execution report nao retorna "LeavesQty" (responsavel pelo OrderQtyReimmaining),
                // entao monta-se a qtd disponivel a partir de OrderQty e CumQty
                int qtd = (ordem.OrderQty - ordem.CumQty);
                logger.DebugFormat("ESTCOMPRA: estornar [{0}] do limite CONSUMIDO", qtd);
                //limiteCont.qtDisponivel += ordem.OrderQtyRemmaining;
                limiteCont.qtDisponivel += qtd;
                if (limiteInst != null)
                {
                    limiteInst.qtDispInstrumento += qtd;
                    limiteInst.qtDispContrato = limiteCont.qtDisponivel;

                    _atualizarLimiteInstrumentoBMFCompra(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFCompra(conta, limiteCont);
                RemoveOrder(ordem.OrigClOrdID);
            }
            else
            {
                logger.DebugFormat("ESTCOMPRA: estornar [{0}] do limite PREALOCADO", ordem.OrderQtyRemmaining);
                // Se for uma rejeicao de substituicao, tem que devolver a diferenca somente
                int qtdeOriginal=0;
                if (ordem.OrigClOrdID != null && ordem.OrigClOrdID.Length > 0)
                {
                    OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.OrigClOrdID);

                    if (ordemOriginal != null)
                    {
                        qtdeOriginal = ordemOriginal.OrderQty - ordemOriginal.CumQty;
                    }
                }

                limiteCont.qtPrealocado -= (ordem.OrderQty - qtdeOriginal);

                if (limiteInst != null)
                {
                    limiteInst.qtPrealocado += (ordem.OrderQty - qtdeOriginal);

                    _atualizarLimiteInstrumentoBMFCompra(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFCompra(conta, limiteCont);
            }


            if (ordem.OrdStatus == OrdemStatusEnum.NOVA)
            {
                limiteCont.qtPrealocado -= ordem.OrderQty;
                limiteCont.qtDisponivel -= ordem.OrderQty;

                if (limiteInst != null)
                {
                    limiteInst.qtPrealocado -= ordem.OrderQty;
                    limiteInst.qtDispInstrumento -= ordem.OrderQty;
                    limiteInst.qtDispContrato = limiteCont.qtDisponivel;

                    _atualizarLimiteInstrumentoBMFCompra(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFCompra(conta, limiteCont);
            }

            logger.InfoFormat("ESTCOMPRA: Acc[{0}] Sym[{1}] Prc[{2}]  Qtde[{3}] LTot[{4}] LDisp[{5}] LPre[{6}], enfileirando para persistencia",
                ordem.Account,
                ordem.Symbol,
                ordem.Price,
                ordem.OrderQty,
                limiteCont.qtTotal,
                limiteCont.qtDisponivel,
                limiteCont.qtPrealocado);

            // StoreOrder(ordem.ClOrdID, ordem);

            if (limiteInst != null)
            {
                lock (qMovInstBMF)
                {
                    qMovInstBMF.Enqueue(limiteInst);
                }
            }

            lock (qMovContBMF)
            {
                qMovContBMF.Enqueue(limiteCont);
            }


            return true;
        }

        private bool _estornarLimiteBMFVenda(OrdemInfo ordem)
        {
            string instrumento = ordem.Symbol;
            string contrato = instrumento.Substring(0, 3);
            int conta = ordem.Account;


            LimiteClienteInstrumentoBMF limiteInst = _obterLimiteInstrumentoVenda(conta, instrumento);
            LimiteClienteContratoBMF limiteCont = _obterLimiteContratoVenda(conta, contrato);
            if (limiteInst == null && limiteCont == null)
            {
                logger.ErrorFormat("ESTVENDA: Nao ha limite cadastrado para instrumento ou contrato. Conta [{0}] Sym [{1}] Qtde [{2}] Prc [{3}]",
                    ordem.Account,
                    ordem.Symbol,
                    ordem.OrderQty,
                    ordem.Price);

                return false;
            }

            logger.InfoFormat("ESTVENDA: Acc[{0}] Sym[{1}] Prc[{2}]  Qtde[{3}] LTot[{4}] LDisp[{5}] LPre[{6}] Stat[{7}]",
                ordem.Account,
                ordem.Symbol,
                ordem.Price,
                ordem.OrderQty,
                limiteCont.qtTotal,
                limiteCont.qtDisponivel,
                limiteCont.qtPrealocado,
                ordem.OrdStatus.ToString());

            // Verificar se eh estorno por rejeicao ou cancelamento de ordem
            // se for rejeicao, estorna da prealocacao, caso contrario, estorna no limite total
            // Libera prealocacao e recalcula limite

            if (ordem.OrdStatus == OrdemStatusEnum.CANCELADA)
            {
                // Em mensagem cancelada, o execution report nao retorna "LeavesQty" (responsavel pelo OrderQtyReimmaining),
                // entao monta-se a qtd disponivel a partir de OrderQty e CumQty
                int qtd = (ordem.OrderQty - ordem.CumQty);
                logger.DebugFormat("ESTVENDA: estornar [{0}] do limite CONSUMIDO", qtd);
                //limiteCont.qtDisponivel += ordem.OrderQtyRemmaining;
                limiteCont.qtDisponivel += qtd;

                if (limiteInst != null)
                {
                    limiteInst.qtDispInstrumento += qtd;
                    limiteInst.qtDispContrato = limiteCont.qtDisponivel;

                    _atualizarLimiteInstrumentoBMFVenda(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFVenda(conta, limiteCont);
                RemoveOrder(ordem.OrigClOrdID);

            }
            else
            {
                logger.DebugFormat("ESTVENDA: estornar [{0}] do limite PREALOCADO", ordem.OrderQtyRemmaining);

                // Se for uma rejeicao de substituicao, tem que devolver a diferenca somente
                int qtdeOriginal = 0;
                if (ordem.OrigClOrdID != null && ordem.OrigClOrdID.Length > 0)
                {
                    OrdemInfo ordemOriginal = _obterOrdemOriginal(ordem.OrigClOrdID);

                    if (ordemOriginal != null)
                    {
                        qtdeOriginal = ordemOriginal.OrderQty - ordemOriginal.CumQty;
                    }
                }

                limiteCont.qtPrealocado -= (ordem.OrderQty - qtdeOriginal);

                if (limiteInst != null)
                {
                    limiteInst.qtPrealocado += (ordem.OrderQty - qtdeOriginal);

                    _atualizarLimiteInstrumentoBMFVenda(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFVenda(conta, limiteCont);
            }


            if (ordem.OrdStatus == OrdemStatusEnum.NOVA)
            {
                limiteCont.qtPrealocado -= ordem.OrderQty;
                limiteCont.qtDisponivel -= ordem.OrderQty;

                if (limiteInst != null)
                {
                    limiteInst.qtPrealocado -= ordem.OrderQty;
                    limiteInst.qtDispInstrumento -= ordem.OrderQty;
                    limiteInst.qtDispContrato = limiteCont.qtDisponivel;

                    _atualizarLimiteInstrumentoBMFVenda(conta, limiteInst);
                }

                _atualizarLimiteContratoBMFVenda(conta, limiteCont);
            }

            logger.InfoFormat("ESTVENDA: Acc[{0}] Sym[{1}] Prc[{2}]  Qtde[{3}] LTot[{4}] LDisp[{5}] LPre[{6}], enfileirando para persistencia",
                ordem.Account,
                ordem.Symbol,
                ordem.Price,
                ordem.OrderQty,
                limiteCont.qtTotal,
                limiteCont.qtDisponivel,
                limiteCont.qtPrealocado);

            // StoreOrder(ordem.ClOrdID, ordem);

            if (limiteInst != null)
            {
                lock (qMovInstBMF)
                {
                    qMovInstBMF.Enqueue(limiteInst);
                }
            }

            lock (qMovContBMF)
            {
                qMovContBMF.Enqueue(limiteCont);
            }

            return true;
        }

        private LimiteClienteInstrumentoBMF _obterLimiteInstrumentoVenda(int conta, string instrumento)
        {
            LimiteClienteInstrumentoBMF ret = null;

            lock (_limiteClienteInstrumentoBMFVenda)
            {
                if (_limiteClienteInstrumentoBMFVenda.ContainsKey(conta))
                {
                    Dictionary<string, LimiteClienteInstrumentoBMF> limites = _limiteClienteInstrumentoBMFVenda[conta];

                    if (limites.ContainsKey(instrumento))
                    {
                        ret = limites[instrumento];
                    }
                }
            }

            return ret;
        }

        private LimiteClienteContratoBMF _obterLimiteContratoVenda(int conta, string contrato)
        {
            LimiteClienteContratoBMF ret = null;

            lock (_limiteClienteContratoBMFVenda)
            {
                if (_limiteClienteContratoBMFVenda.ContainsKey(conta))
                {
                    Dictionary<string, LimiteClienteContratoBMF> contratos = _limiteClienteContratoBMFVenda[conta];

                    if (contratos.ContainsKey(contrato))
                    {
                        ret = contratos[contrato];
                    }
                }
            }

            return ret;
        }

        private LimiteClienteInstrumentoBMF _obterLimiteInstrumentoCompra(int conta, string instrumento)
        {
            LimiteClienteInstrumentoBMF ret = null;

            lock (_limiteClienteInstrumentoBMFCompra)
            {
                if (_limiteClienteInstrumentoBMFCompra.ContainsKey(conta))
                {
                    Dictionary<string, LimiteClienteInstrumentoBMF> limites = _limiteClienteInstrumentoBMFCompra[conta];

                    if (limites.ContainsKey(instrumento))
                    {
                        ret = limites[instrumento];
                    }
                }
            }

            return ret;
        }

        private LimiteClienteContratoBMF _obterLimiteContratoCompra(int conta, string contrato)
        {
            LimiteClienteContratoBMF ret = null;

            lock (_limiteClienteContratoBMFCompra)
            {
                if (_limiteClienteContratoBMFCompra.ContainsKey(conta))
                {
                    Dictionary<string, LimiteClienteContratoBMF> contratos = _limiteClienteContratoBMFCompra[conta];

                    if (contratos.ContainsKey(contrato))
                    {
                        ret = contratos[contrato];
                    }
                }
            }

            return ret;
        }

        #region Serialization Manager
        public bool SerializeHistoryOrders(string file)
        {
            try
            {
                if (!_serializing)
                {
                    _serializing = true;
                    string fileAux = string.Format(file, this.AccountLimit);
                    List<OrdemInfo> lstAux = new List<OrdemInfo>();
                    lock (_hstOrders)
                    {
                        lstAux = _hstOrders.Values.Cast<OrdemInfo>().ToList();
                    }

                    FileStream fs = File.Open(fileAux, FileMode.Create, FileAccess.Write);
                    BinaryFormatter bs = new BinaryFormatter();
                    bs.Serialize(fs, lstAux);
                    bs = null;
                    logger.InfoFormat("SerializeHistoryOrders(): Registros serializados: [{0}] [{1}]", lstAux.Count, fileAux);
                    lstAux.Clear();
                    lstAux = null;
                    fs.Close();
                    fs = null;
                    _serializing = false;
                }
                else
                    logger.Info("Jah em processo de serializacao, aguardando proxima iteracao");

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na gravacao do OrdemInfo do LimiteManager: " + ex.Message, ex);
                return false;
            }
        }

        public bool LoadHistoryOrders(string fileName)
        {

            try
            {
                string file = string.Format(fileName, this.AccountLimit);

                if (File.Exists(file))
                {
                    logger.Info("Arquivo de leitura: Carga de Historico de Ordens: " + file);

                    
                    List<OrdemInfo> lst = new List<OrdemInfo>();
                    FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read);
                    
                    
                    BinaryFormatter bformatter = new BinaryFormatter();
                    lst = (List<OrdemInfo>)bformatter.Deserialize(fs);
                    int length = lst.Count;
                    if (lst.Count > 0)
                    {
                        lock (_hstOrders)
                        {
                            for (int i = 0; i < length; i++)
                            {
                                _hstOrders.Add(lst[i].ClOrdID, lst[i]);
                            }
                        }
                    }
                    logger.Info("LoadHistoryOrders(): Registros recuperados: " + lst.Count);
                    lst.Clear();
                    lst = null;
                    bformatter = null;
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                    return true;
                }
                else
                {
                    logger.Info("Arquivo nao encontrado para efetuar a carga do historico de ordens: " + file);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("LoadHistoryOrders(): Erro na deserializacao da hashtable de controle : " + ex.Message, ex);
                return false;
            }
        }


        #endregion
    }
}

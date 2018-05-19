using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections.Concurrent;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using Gradual.Spider.SupervisorRisco.DB.Lib;
using System.Threading;
using System.Configuration;
using Gradual.Spider.SupervisorRisco.Lib.Handlers;
using Gradual.Spider.ServicoSupervisor.Calculator;

namespace Gradual.Spider.ServicoSupervisor.Memory
{
    public class ConsolidatedRiskManager
    {

        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Private Variables
        ConcurrentDictionary<int, ConsolidatedRiskInfo> _cdRiscoConsolidado;
        ConcurrentDictionary<string, CRLucroPrej> _cdCRLucroPrej;
        DbRisco _dbRisco;
        DbRiscoOracle _dbOracle;
        bool _isRunning = false;
        object _syncDb = new object();

        bool _isRunningDB = false;
        
        Thread _thRiskDB = null;

        object _syncSnap = new object();

        //ConcurrentDictionary<string, CustodiaInfo> _cdBvsp;
        // ConcurrentDictionary<string, CustodiaInfo> _cdBmf;
        ConcurrentDictionary<string, List<CustodiaInfo>> _cdBvsp;
        ConcurrentDictionary<string, List<CustodiaInfo>> _cdBmf;
        ConcurrentDictionary<int, List<CustodiaInfo>> _cdBvspByAcc;
        ConcurrentDictionary<int, List<CustodiaInfo>> _cdBmfByAcc;
        ConcurrentDictionary<int, TesouroDiretoAbInfo> _cdTedi;

        bool _isLoadingCRM = false;

        #endregion
        public delegate void ConsolidatedRiskUpdateHandler(object sender, ConsolidatedRiskEventArgs args);
        public event ConsolidatedRiskUpdateHandler OnConsolidatedRiskUpdate;

        private static ConsolidatedRiskManager _me = null;
        public static ConsolidatedRiskManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new ConsolidatedRiskManager();
                }
                return _me;
            }
        }

        public ConsolidatedRiskManager()
        {
            
            
        }

        public void Start()
        {
            try
            {
                logger.Info("Iniciando Manager Risco Consolidado...");
                if (null == _dbRisco)
                    _dbRisco = new DbRisco();
                if (null == _dbOracle)
                    _dbOracle = new DbRiscoOracle();

                _cdRiscoConsolidado = new ConcurrentDictionary<int, ConsolidatedRiskInfo>();
                _cdCRLucroPrej = new ConcurrentDictionary<string, CRLucroPrej>();
                logger.Info("Carregar informacoes de abertura (custodia, conta corrente, garantias, produtos...");

                logger.Info("Limpando valores de tb_risco_consolidado...");
                _dbRisco.LimparRiscoConsolidado();

                logger.Info("Carregando informacoes iniciais de risco consolidado");
                this.LoadConsolidatedRisk();


                _isRunning = true;
                logger.Info("Iniciando thread de atualizacao no banco de dados...");
                _thRiskDB = new Thread(new ThreadStart(_processConsolidatedRiskDB));
                _thRiskDB.Start();
                
                
                
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do manager Risco Consolidado: " + ex.Message, ex);
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                _isRunning = false;
                logger.Info("Parando Manager Risco Consolidado...");

                logger.Info("Parando thread Banco de Dados");
                if (_thRiskDB != null && _thRiskDB.IsAlive)
                {
                    _thRiskDB.Join(500);
                    if (_thRiskDB.IsAlive)
                        _thRiskDB.Abort();
                    _thRiskDB = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop de Risco Consolidado: " + ex.Message, ex);
                throw;
            }
        }


        #region Cargas iniciais de risco consolidado
        public void CarregarGarantias()
        {
            try
            {
                ConcurrentDictionary<int, GarantiaInfo> cdBov = _dbOracle.CarregarGarantiaBovespa();
                ConcurrentDictionary<int, GarantiaInfo> cdBmf = _dbOracle.CarregarGarantiaBmf();
                if (cdBov == null)
                {
                    logger.Error("Problemas na carga de garantias de BOVESPA");
                    return;
                }
                if (cdBmf == null)
                {
                    logger.Error("Problemas na carga de garantias de BMF");
                    return;
                }

                logger.InfoFormat("Fazendo cargas de garantias BOVESPA[{0}] BMF[{1}]...", cdBov.Count, cdBmf.Count);
                foreach (KeyValuePair<int, GarantiaInfo> gBov in cdBov)
                {
                    ConsolidatedRiskInfo cr = null;
                    logger.InfoFormat("Garantia Bovespa - Account[{0}] GarantiaDisponivel[{1}]", gBov.Key, gBov.Value.GarantiaDisponivel);
                    if (_cdRiscoConsolidado.TryGetValue(gBov.Key, out cr))
                        cr.TotalGarantias += gBov.Value.GarantiaDisponivel;
                    else
                    {
                        cr = new ConsolidatedRiskInfo();
                        cr.TotalGarantias += gBov.Value.GarantiaDisponivel;
                        cr.Account = gBov.Key;

                    }
                    cr.SaldoTotalAbertura = cr.TotalContaCorrenteAbertura + cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalProdutos;
                    cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
                    cr.DtMovimento = DateTime.Now;
                    _cdRiscoConsolidado.AddOrUpdate(gBov.Key, cr, (key, oldvalue) => cr);
                }
                foreach (KeyValuePair<int, GarantiaInfo> gBmf in cdBmf)
                {
                    ConsolidatedRiskInfo cr = null;
                    logger.InfoFormat("Garantia Bmf - Account[{0}] GarantiaDisponivel[{1}]", gBmf.Key, gBmf.Value.GarantiaDisponivel);
                    
                    if (_cdRiscoConsolidado.TryGetValue(gBmf.Key, out cr))
                        cr.TotalGarantias += gBmf.Value.GarantiaDisponivel;
                    else
                    {
                        cr = new ConsolidatedRiskInfo();
                        cr.TotalGarantias += gBmf.Value.GarantiaDisponivel;
                        cr.Account = gBmf.Key;
                    }
                    cr.SaldoTotalAbertura = cr.TotalContaCorrenteAbertura + cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalProdutos;
                    cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
                    cr.DtMovimento = DateTime.Now;
                    _cdRiscoConsolidado.AddOrUpdate(gBmf.Key, cr, (key, oldvalue) => cr);
                }

            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga inicial das informacoes de garantia: " + ex.Message, ex);
                throw;
            }

        }


        public void CarregarProdutos()
        {
            try
            {
                ConcurrentDictionary<int, ProdutosInfo> cdProd = _dbRisco.CarregarProdutos();
                if (cdProd != null)
                {
                    foreach (KeyValuePair<int, ProdutosInfo> item in cdProd)
                    {
                        ConsolidatedRiskInfo cr = null;
                        logger.InfoFormat("Produtos - Account[{0}] Produto Saldo Liquido[{1}]", item.Key, item.Value.SaldoLiquido);
                        if (_cdRiscoConsolidado.TryGetValue(item.Key, out cr))
                        {
                            cr.TotalProdutos = item.Value.SaldoLiquido;
                        }
                        else
                        {
                            cr = new ConsolidatedRiskInfo();
                            cr.TotalProdutos = item.Value.SaldoLiquido;
                            cr.Account = item.Key;
                        }
                        cr.SaldoTotalAbertura = cr.TotalContaCorrenteAbertura + cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalProdutos;
                        cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
                        cr.DtMovimento = DateTime.Now;
                        _cdRiscoConsolidado.AddOrUpdate(item.Key, cr, (key, oldvalue) => cr);
                    }
                    logger.InfoFormat("Fazendo carga de produtos. Total[{0}]", cdProd.Count);
                }
                else
                {
                    logger.Error("Problemas na carga de produtos!!");
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga inicial das informacoes de produtos (fundos): " + ex.Message, ex);
                throw;
            }
        }

        public void CarregarContaCorrente()
        {
            try
            {
                ConcurrentDictionary<int, ContaCorrenteInfo> cdCc = _dbOracle.CarregarContaCorrente();
                if (cdCc != null)
                {
                    foreach (KeyValuePair<int, ContaCorrenteInfo> item in cdCc)
                    {
                        ConsolidatedRiskInfo cr = null;
                        logger.InfoFormat("ContaCorrente - Account[{0}] VlrTotal[{1}]", item.Key, item.Value.VlrTotal);
                        if (_cdRiscoConsolidado.TryGetValue(item.Key, out cr))
                        {
                            cr.TotalContaCorrenteAbertura = item.Value.VlrTotal;
                        }
                        else
                        {
                            cr = new ConsolidatedRiskInfo();
                            cr.TotalContaCorrenteAbertura = item.Value.VlrTotal;
                            cr.Account = item.Key;
                        }
                        cr.SaldoTotalAbertura = cr.TotalContaCorrenteAbertura + cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalProdutos;
                        cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
                        cr.DtMovimento = DateTime.Now;
                        _cdRiscoConsolidado.AddOrUpdate(item.Key, cr, (key, oldvalue) => cr);
                    }
                    logger.InfoFormat("Fazendo carga de conta corrente: [{0}]", cdCc.Count);
                }
                else
                {
                    logger.Error("Problemas na carga de conta corrente!!");
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga de informacoes de conta corrente: " + ex.Message, ex);
                throw;
            }
        }

        public void CarregarCustodiaBovespa()
        {
            try
            {
                _cdBvsp = _dbOracle.CarregarCustodiaBvsp();
                _cdBvspByAcc = new ConcurrentDictionary<int, List<CustodiaInfo>>();
                if (_cdBvsp != null)
                {
                    foreach (KeyValuePair<string, List<CustodiaInfo>> item2 in _cdBvsp)
                    {
                        foreach (CustodiaInfo item in item2.Value)
                        {
                            logger.InfoFormat("CarregarCustodiaBovespa - Account[{0}] Symbol[{1}] Qtd[{2}]", item.Account, item.Symbol, item.Qty);
                            List<CustodiaInfo> lstByAcc = null;
                            if (!_cdBvspByAcc.TryGetValue(item.Account, out lstByAcc))
                            {
                                lstByAcc = new List<CustodiaInfo>();
                                lstByAcc.Add(item);
                                _cdBvspByAcc.AddOrUpdate(item.Account, lstByAcc, (key, old) => lstByAcc);
                            }
                            else
                            {
                                lstByAcc.Add(item);
                            }
                             
                            SymbolInfo s = RiskCache.Instance.GetSymbol(item.Symbol);
                            if (s != null)
                            {
                                ConsolidatedRiskInfo cr = null;
                                item.ValorCustodia = item.Qty * s.VlrFechamento;
                                if (_cdRiscoConsolidado.TryGetValue(item.Account, out cr))
                                {
                                    cr.TotalCustodiaBvsp = cr.TotalCustodiaBvsp + item.ValorCustodia;
                                }
                                else
                                {
                                    cr = new ConsolidatedRiskInfo();
                                    cr.TotalCustodiaBvsp = (item.ValorCustodia);
                                    cr.Account = item.Account;
                                }
                                cr.TotalCustodiaAbertura = cr.TotalCustodiaBvsp + cr.TotalCustodiaBmf + cr.TotalCustodiaTesouroDireto;
                                cr.SaldoTotalAbertura = cr.TotalContaCorrenteAbertura + cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalProdutos;
                                cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
                                cr.DtMovimento = DateTime.Now;
                                _cdRiscoConsolidado.AddOrUpdate(cr.Account, cr, (key, oldvalue) => cr);
                            }
                        }
                    }
                }
                else
                    logger.Error("CarregarCustodiaBovespa - Erro na carga da collection. Dicionario nulo");
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga de informacoes de custodia bovespa: " + ex.Message, ex);
            }
        }

        public void CarregarCustodiaBmf()
        {
            try
            {
                _cdBmf = _dbOracle.CarregarCustodiaBmf();
                _cdBmfByAcc = new ConcurrentDictionary<int, List<CustodiaInfo>>();
                if (_cdBmf != null)
                {
                    foreach (KeyValuePair<string, List<CustodiaInfo>> item2 in _cdBmf)
                    {
                        foreach (CustodiaInfo item in item2.Value)
                        {
                            logger.InfoFormat("CarregarCustodiaBmf - Account[{0}] Symbol[{1}] Qtd[{2}]", item.Account, item.Symbol, item.Qty);
                            
                            List<CustodiaInfo> lstByAcc = null;
                            if (!_cdBmfByAcc.TryGetValue(item.Account, out lstByAcc))
                            {
                                lstByAcc = new List<CustodiaInfo>();
                                lstByAcc.Add(item);
                                _cdBmfByAcc.AddOrUpdate(item.Account, lstByAcc, (key, old) => lstByAcc);
                            }
                            else
                            {
                                lstByAcc.Add(item);
                            }
                            
                            SymbolInfo s = RiskCache.Instance.GetSymbol(item.Symbol);
                            if (s != null)
                            {
                                ConsolidatedRiskInfo cr = null;
                                decimal bmf = BmfCalculator.Instance.CalcularCustodiaAberturaBmf(s, item.Qty);
                                item.ValorCustodia = bmf;
                                if (_cdRiscoConsolidado.TryGetValue(item.Account, out cr))
                                {
                                    cr.TotalCustodiaBmf = cr.TotalCustodiaBmf + item.ValorCustodia;
                                }
                                else
                                {
                                    cr = new ConsolidatedRiskInfo();
                                    cr.TotalCustodiaBmf = item.ValorCustodia;
                                    cr.Account = item.Account;
                                }
                                cr.TotalCustodiaAbertura = cr.TotalCustodiaBvsp + cr.TotalCustodiaBmf + cr.TotalCustodiaTesouroDireto;
                                cr.SaldoTotalAbertura = cr.TotalContaCorrenteAbertura + cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalProdutos;
                                cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
                                cr.DtMovimento = DateTime.Now;
                                _cdRiscoConsolidado.AddOrUpdate(cr.Account, cr, (key, oldvalue) => cr);
                            }
                        }
                    }
                }
                else
                    logger.Error("CarregarCustodiaBmf - Erro na carga da collection. Dicionario nulo");
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga de informacoes de custodia bmf: " + ex.Message, ex);
            }
        }


        //public void CarregarCustodia()
        //{
        //    try
        //    {
        //        ConcurrentDictionary<int, CustodiaInfo> cdCustodia = _dbOracle.CarregarCustodia(RiskCache.Instance.GetAllSymbols());
        //        if (cdCustodia != null)
        //        {
        //            foreach (KeyValuePair<int, CustodiaInfo> item in cdCustodia)
        //            {
        //                ConsolidatedRiskInfo cr = null;
        //                logger.InfoFormat("Custodia - Account[{0}] ValorCustodia[{1}]", item.Key, item.Value.VlrCustodia);
        //                if (_cdRiscoConsolidado.TryGetValue(item.Key, out cr))
        //                {
        //                    cr.TotalCustodiaAbertura = item.Value.VlrCustodia;
        //                }
        //                else
        //                {
        //                    cr = new ConsolidatedRiskInfo();
        //                    cr.TotalCustodiaAbertura = item.Value.VlrCustodia;
        //                    cr.Account = item.Key;
        //                }
        //                cr.SaldoTotalAbertura = cr.TotalContaCorrenteAbertura + cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalProdutos;
        //                cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
        //                cr.DtMovimento = DateTime.Now;
        //                _cdRiscoConsolidado.AddOrUpdate(item.Key, cr, (key, oldvalue) => cr);
        //            }
        //            logger.InfoFormat("Fazendo carga de custodia: [{0}]", cdCustodia.Count);
        //        }
        //        else
        //        {
        //            logger.Error("Problemas na carga de custodia!!");
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Problemas na carga de informacoes de cutodia: " + ex.Message, ex);
        //    }
        //}

        public void CarregarCustodiaTesouroDireto()
        {
            try
            {
                _cdTedi = _dbOracle.CarregarTesouroDireto();
                if (_cdTedi != null)
                {
                    foreach (KeyValuePair<int, TesouroDiretoAbInfo> item in _cdTedi)
                    {
                        logger.InfoFormat("CarregarCustodiaTesouroDireto - Account[{0}] Valor[{1}]", item.Value.CodCliente, item.Value.ValPosi);
                        ConsolidatedRiskInfo cr = null;
                        if (_cdRiscoConsolidado.TryGetValue(item.Value.CodCliente, out cr))
                        {
                            cr.TotalCustodiaTesouroDireto = cr.TotalCustodiaTesouroDireto + item.Value.ValPosi;
                        }
                        else
                        {
                            cr = new ConsolidatedRiskInfo();
                            cr.TotalCustodiaTesouroDireto = item.Value.ValPosi;
                            cr.Account = item.Value.CodCliente;
                        }
                        cr.TotalCustodiaAbertura = cr.TotalCustodiaBvsp + cr.TotalCustodiaBmf + cr.TotalCustodiaTesouroDireto;
                        cr.SaldoTotalAbertura = cr.TotalContaCorrenteAbertura + cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalProdutos;
                        cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
                        cr.DtMovimento = DateTime.Now;
                        _cdRiscoConsolidado.AddOrUpdate(cr.Account, cr, (key, oldvalue) => cr);
                    }
                }
                else
                {
                    logger.Error("CarregarCustodiaTesouroDireto - Collection nulo ou vazio");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga de custodia de tesouro direto: " + ex.Message, ex);
            }
            
            //try
            //{
            //    ConcurrentDictionary<int, TesouroDiretoAbInfo> cdCustodiaTedi = _dbOracle.CarregarTesouroDireto();
            //    if (cdCustodiaTedi != null)
            //    {
            //        foreach (KeyValuePair<int, TesouroDiretoAbInfo> item in cdCustodiaTedi)
            //        {
            //            ConsolidatedRiskInfo cr = null;
            //            logger.InfoFormat("Custodia Tesouro Direto - Account[{0}] ValorPosicao[{1}]", item.Key, item.Value.ValPosi);
            //            if (_cdRiscoConsolidado.TryGetValue(item.Key, out cr))
            //            {
            //                cr.TotalCustodiaAbertura = item.Value.ValPosi;
            //            }
            //            else
            //            {
            //                cr = new ConsolidatedRiskInfo();
            //                cr.TotalCustodiaAbertura = item.Value.ValPosi;
            //                cr.Account = item.Key;
            //            }
            //            cr.SaldoTotalAbertura = cr.TotalContaCorrenteAbertura + cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalProdutos;
            //            cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
            //            cr.DtMovimento = DateTime.Now;
            //            _cdRiscoConsolidado.AddOrUpdate(item.Key, cr, (key, oldvalue) => cr);
            //        }
            //        logger.InfoFormat("Fazendo carga de custodia tesouro direto: [{0}]", cdCustodiaTedi.Count);
            //    }
            //    else
            //    {
            //        logger.Error("Problemas na carga de custodia abertura tesouro direto!!");
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    logger.Error("Problemas na carga de informacoes de cutodia tesouro direto: " + ex.Message, ex);
            //}

        }

        public void LoadConsolidatedRisk()
        {
            try
            {
                if (_isLoadingCRM)
                    logger.Info("LoadConsolidatedRisk já em execucao");

                _isLoadingCRM = true;
                this.CarregarGarantias();
                this.CarregarProdutos();
                this.CarregarContaCorrente();
                this.CarregarCustodiaBovespa();
                this.CarregarCustodiaBmf();
                this.CarregarCustodiaTesouroDireto();
                _isLoadingCRM = false;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga inicial do risco consolidado: " + ex.Message, ex);
            }
        }


        #endregion

        public void CalculateConsolidatedPosition(PosClientSymbolInfo pc)
        {
            try
            {
                ConsolidatedRiskInfo item = null;
                if (!_cdRiscoConsolidado.TryGetValue(pc.Account, out item))
                {
                    item = new ConsolidatedRiskInfo();
                }

                // Buscar o L/P anterior da position client para descontar
                string chave = string.Format("{0}-{1}", pc.Account, pc.Ativo);
                CRLucroPrej cr = null;
                if (!_cdCRLucroPrej.TryGetValue(chave, out cr))
                {
                    cr = new CRLucroPrej();
                    cr.Account = pc.Account;
                    cr.Ativo = pc.Ativo;
                    cr.Bolsa = pc.Bolsa;
                    _cdCRLucroPrej.AddOrUpdate(chave, cr, (key, old) => cr);
                }

                // logger.DebugFormat("CalculateConsolidatedPosition ACCOUNT[{0}] SYMBOL[{1}] EXCHANGE[{2}] LP[{3}]", pc.Account, pc.Ativo, pc.Bolsa, pc.LucroPrej);
                // Calcular valores
                item.Account = pc.Account;
                if (pc.Bolsa.Equals(Exchange.Bovespa, StringComparison.CurrentCultureIgnoreCase))
                    item.PLBovespa = item.PLBovespa - cr.LucroPrejuizo + pc.LucroPrej;
                else
                    item.PLBmf = item.PLBmf - cr.LucroPrejuizo + pc.LucroPrej;

                cr.LucroPrejuizo = pc.LucroPrej;
                _cdCRLucroPrej.AddOrUpdate(chave, cr, (key, old) => cr);

                item.SaldoTotalAbertura = item.TotalCustodiaAbertura + item.TotalContaCorrenteAbertura + item.TotalGarantias + item.TotalProdutos;
                item.PLTotal = item.PLBovespa + item.PLBmf;
                item.SFP = item.SaldoTotalAbertura + item.PLTotal;
                if (item.SaldoTotalAbertura.Equals(decimal.Zero))
                    item.TotalPercentualAtingido = decimal.Zero;
                else
                {
                    if (item.SaldoTotalAbertura > 0)
                        item.TotalPercentualAtingido = (item.PLTotal / item.SaldoTotalAbertura) * 100;
                    else
                        item.TotalPercentualAtingido = (item.PLTotal + item.SaldoTotalAbertura) / item.SaldoTotalAbertura * (-100);

                }
                item.DtMovimento = DateTime.Now;
                // Atualizar memoria
                _cdRiscoConsolidado.AddOrUpdate(item.Account, item, (key, oldvalue) => item);
                if (OnConsolidatedRiskUpdate != null)
                {
                    ConsolidatedRiskEventArgs sync = new ConsolidatedRiskEventArgs();
                    sync.Action = EventAction.UPDATE;
                    sync.Account = item.Account;
                    ConcurrentDictionary<int, ConsolidatedRiskInfo> cd = new ConcurrentDictionary<int, ConsolidatedRiskInfo>();
                    cd.AddOrUpdate(item.Account, item, (key, oldvalue) => item);
                    sync.ConsolidatedRisk = cd;
                    OnConsolidatedRiskUpdate(this, sync);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de risco consolidado: " + ex.Message, ex);
            }
        }

        public void CalcularTotalCustodia(SymbolInfo inst)
        {
            try
            {
                
                List<int> lstAccounts = new List<int>();
                //List<CustodiaInfo> lstBvsp = _cdBvsp.Where(x => x.Key.IndexOf(inst.Instrumento) >= 0).Select(x => x.Value).ToList();

                List<CustodiaInfo> lstBvsp = null;
                if (_cdBvsp.TryGetValue(inst.Instrumento, out lstBvsp))
                {
                    foreach (CustodiaInfo x in lstBvsp)
                    {
                        x.ValorCustodia = x.Qty * inst.VlrUltima;
                        lstAccounts.Add(x.Account);
                    }
                }

                // Buscar todas as chaves bmf que tenham a chave
                List<CustodiaInfo> lstBmf = null;
                //List<CustodiaInfo> lstBmf = _cdBmf.Where(x => x.Key.IndexOf(inst.Instrumento) >= 0).Select(x => x.Value).ToList();
                if (_cdBmf.TryGetValue(inst.Instrumento, out lstBmf))
                {
                    
                    foreach (CustodiaInfo y in lstBmf)
                    {
                        decimal aux = BmfCalculator.Instance.CalcularCustodiaAberturaBmf(inst, y.Qty);
                        y.ValorCustodia = aux;
                        lstAccounts.Add(y.Account);
                    }
                }
                foreach (int item in lstAccounts)
                {
                    // Buscar tesouro direto e recalcular o RiscoConsolidado
                    
                    ConsolidatedRiskInfo cr = null;
                    if (_cdRiscoConsolidado.TryGetValue(item, out cr))
                    {

                        List<CustodiaInfo> lst1 = null;
                        decimal vlrCustodiaBov = decimal.Zero;
                        decimal vlrCustodiaBmf = decimal.Zero;
                        decimal vlrCustodiaAbTedi = decimal.Zero;
                        if (_cdBvspByAcc.TryGetValue(item, out lst1))
                            vlrCustodiaBov = lst1.Sum(x => x.ValorCustodia);

                        List<CustodiaInfo> lst2 = null;
                        if (_cdBmfByAcc.TryGetValue(item, out lst2))
                            vlrCustodiaBmf = lst2.Sum(x => x.ValorCustodia);

                        TesouroDiretoAbInfo tD = null;
                        vlrCustodiaAbTedi = decimal.Zero;
                        if (_cdTedi.TryGetValue(item, out tD))
                            vlrCustodiaAbTedi = tD.ValPosi;
                        

                        cr.TotalCustodiaBmf = vlrCustodiaBmf;
                        cr.TotalCustodiaBvsp = vlrCustodiaBov;
                        cr.TotalCustodiaTesouroDireto = vlrCustodiaAbTedi;
                        cr.TotalCustodiaAbertura = cr.TotalCustodiaBmf + cr.TotalCustodiaBvsp + cr.TotalCustodiaTesouroDireto;
                        cr.SaldoTotalAbertura = cr.TotalCustodiaAbertura + cr.TotalGarantias + cr.TotalContaCorrenteAbertura + cr.TotalProdutos;
                        cr.SFP = cr.SaldoTotalAbertura + cr.PLTotal;
                        if (cr.SaldoTotalAbertura.Equals(decimal.Zero))
                            cr.TotalPercentualAtingido = decimal.Zero;
                        else
                        {
                            if (cr.SaldoTotalAbertura > 0)
                                cr.TotalPercentualAtingido = (cr.PLTotal / cr.SaldoTotalAbertura) * 100;
                            else
                                cr.TotalPercentualAtingido = (cr.PLTotal + cr.SaldoTotalAbertura) / cr.SaldoTotalAbertura * (-100);
                        }
                        cr.DtMovimento = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de Total de Custodia: " + ex.Message, ex);
            }
        }


        private void _processConsolidatedRiskDB()
        {
            int i = 0;
            int mdsRefresh = 10;
            int sleep = 10;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("PositionClientDBRefresh"))
                mdsRefresh = Convert.ToInt32(ConfigurationManager.AppSettings["PositionClientDBRefresh"].ToString());
            while (_isRunning)
            {
                try
                {
                    if (i >= mdsRefresh * sleep)
                    {
                        if (_isRunningDB)
                            break;
                        _isRunningDB = true;
                        this._atualizaConsolidatedRiskDB();
                        i = 0;
                        _isRunningDB = false;
                    }
                    Thread.Sleep(sleep * 10);
                    i++;
                }
                catch
                {
                    i = 0;
                    _isRunningDB = false;
                }
            }
        }

        private void _atualizaConsolidatedRiskDB()
        {
            try
            {
                List<PosClientSymbolInfo> lstAux = new List<PosClientSymbolInfo>();

                if (_dbRisco == null)
                    _dbRisco = new DbRisco();

                logger.Info("======> Atualizando registros Risco Consolidado");
                foreach (KeyValuePair<int, ConsolidatedRiskInfo> item in _cdRiscoConsolidado)
                {
                    if (!_dbRisco.AtualizarRiscoConsolidado(item.Value))
                        logger.ErrorFormat("Problemas na atualizacao/ insercao de Risco Consolidado : [{0}]", item.Value.Account);
                }
                logger.Info("======> Registros atualizados (risco consolidado): " + _cdRiscoConsolidado.Count);
                
                lstAux.Clear();
                lstAux = null;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao das informacoes de cotacao: " + ex.Message, ex);
            }
        }

        public ConcurrentDictionary<int, ConsolidatedRiskInfo> SnapshotConsolidatedRisk()
        {
            ConcurrentDictionary<int, ConsolidatedRiskInfo> ret = new ConcurrentDictionary<int, ConsolidatedRiskInfo>();
            try
            {
                lock (_syncSnap)
                {
                    if (null != _cdRiscoConsolidado)
                    {
                        KeyValuePair<int, ConsolidatedRiskInfo>[] items = _cdRiscoConsolidado.ToArray();

                        foreach (KeyValuePair<int, ConsolidatedRiskInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("Risco Consolidado sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot de risco consolidado: " + ex.Message, ex);
            }
            return ret;
        }
    }
}

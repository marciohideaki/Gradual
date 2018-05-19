using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Gradual.Spider.SupervisorRisco.DB.Lib;
using System.Collections;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using log4net;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using Gradual.Spider.ServicoSupervisor.Memory;

namespace Gradual.Spider.ServicoSupervisor.Calculator
{
    public class BmfCalculator
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private CultureInfo gCultura = new CultureInfo("pt-BR");

        #region Atributos de INstrumentos de BMF
        private const string DI = "DI1";
        private const string DOLAR = "DOL";
        private const string INDICE = "IND";
        private const string MINIBOLSA = "WIN";
        private const string MINIDOLAR = "WDL";
        private const string MINIDOLARFUT = "WDO";
        private const string CHEIOBOI = "BGI";
        private const string MINIBOI = "WBG";
        private const string EURO = "EUR";
        private const string MINIEURO = "WEU";
        private const string CAFE = "ICF";
        private const string MINICAFE = "WCF";
        private const string FUTUROACUCAR = "ISU";
        private const string ETANOL = "ETH";
        private const string ETANOLFISICO = "ETN";
        private const string MILHO = "CCM";
        private const string SOJA = "SFI";
        private const string OURO = "OZ1";
        private const string ROLAGEMDOLAR = "DR1";
        private const string ROLAGEMINDICE = "IR1";
        private const string ROLAGEMBOI = "BR1";
        private const string ROLAGEMCAFE = "CR1";
        private const string ROLAGEMMILHO = "MR1";
        private const string ROLAGEMSOJA = "SR1";
        #endregion

        #region Static Objects
        private static BmfCalculator _me = null;
        public static BmfCalculator Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new BmfCalculator();
                }
                return _me;
            }
        }
        #endregion

        private Hashtable _htVencimentoDI = new Hashtable();
        private List<DateTime> _lsFeriadosDI = new List<DateTime>();

        public BmfCalculator()
        {

        }

        public void Start()
        {

            DbRisco dbSql = new DbRisco();
            DbRiscoOracle dbOracle = new DbRiscoOracle();

            _htVencimentoDI = dbOracle.ObterVencimentosDI();
            _lsFeriadosDI = dbSql.ObterFeriadosDI();
        }

        public void Stop()
        {
            if (_htVencimentoDI != null)
                _htVencimentoDI.Clear();
            _htVencimentoDI = null;

            if (_lsFeriadosDI != null)
                _lsFeriadosDI.Clear();
            _lsFeriadosDI = null;
        }



        public decimal CalcularTaxaPtax(double taxaOperada)
        {
            decimal lRetorno = 0.0M;
            DbRiscoOracle db = new DbRiscoOracle();
            double taxaMercado = db.ObteCotacaoPtax();

            lRetorno = Convert.ToDecimal((taxaOperada * taxaMercado), gCultura);

            return lRetorno;
        }


        public int ObterDiasUteis(DateTime dataInicial, DateTime dataFinal)
        {
            int dias = 0;
            int ContadorDias = 0;

            dias = dataInicial.Date.Subtract(dataFinal).Days;

            DateTime lDataAtual = dataInicial.Date;

            if (dias < 0)
                dias = dias * -1;

            var lContFeriados = from feriado in this._lsFeriadosDI where feriado >= lDataAtual && feriado <= dataFinal select feriado;

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

        private double CalcularTaxaDI(string Instrumento, double taxaOperada, double dblVlrUltima)
        {

            double Ajuste = 0;
            double Numerador = 100000;
            double NumeroDiasBase = 252;

            try
            {
                DateTime dtVencimento = DateTime.Parse(_htVencimentoDI[Instrumento].ToString());

                double NumeroDiasCalculados = this.ObterDiasUteis(DateTime.Now, dtVencimento);
                double Exponencial = (NumeroDiasCalculados / NumeroDiasBase);

                double taxaMercado = dblVlrUltima;

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
                logger.Error("Prpblemas no calculo de taxa de DI: " + ex.Message, ex);
                return Double.MinValue;
            }

            return Math.Round(Ajuste, 2);

        }

        // private decimal RetornaFatorMultiplicador(string pInstrumento, double? taxaOperada, double? taxaMercado, double vlrUltimoValor)
        public decimal RetornaFatorMultiplicador(string pInstrumento, double vlrFechamento, double vlrUltimoValor)
        {
            decimal lRetorno = 1.0M;

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
                        double lRetornoTemp = CalcularTaxaDI(pInstrumento, vlrFechamento, vlrUltimoValor); // TODO [FF]: rever o calculo
                        lRetorno = Convert.ToDecimal(lRetornoTemp);
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
                    lRetorno = 10;
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

        public bool CalcularPosicaoBmf(PosClientSymbolInfo pos)
        {
            bool ret = false;
            try
            {
                decimal vlrUltimo = pos.UltPreco;
                
                decimal vlrFechamento = pos.PrecoFechamento;

                decimal lAbertura = 0M;

                if (pos.QtdAbertura != 0)
                {
                    SymbolInfo lSymb = RiskCache.Instance.GetSymbol(pos.Ativo);

                    lAbertura = this.CalcularCustodiaAberturaBmf(lSymb, int.Parse(pos.QtdAbertura.ToString()));
                }

                decimal fatorMultiplicador = this.RetornaFatorMultiplicador(pos.Ativo, Convert.ToDouble(vlrFechamento), Convert.ToDouble(vlrUltimo));
                // pos.FinancNet = (pos.PcMedV - pos.PcMedC) * pos.NetExec * fatorMultiplicador; 
                pos.FinancNet = pos.NetExec; // Alterado para simplificar
                
                decimal buy = (vlrUltimo - pos.PcMedC) * pos.QtdExecC * fatorMultiplicador;
                
                decimal sell = (pos.PcMedV - vlrUltimo) * pos.QtdExecV * fatorMultiplicador;
                
                pos.LucroPrej = sell + buy + (lAbertura);
                
                ret = true;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Problemas no calculo L/P da posicao BMF. Cliente [{0}]. Symbol [{1}]  Msg:[{2}]", pos.Account, pos.Ativo,  ex.Message, ex);
            }

            return ret;
        }
        
        public decimal CalcularCustodiaAberturaBmf(SymbolInfo symb, int qtd)
        {
            decimal ret = decimal.Zero;
            try
            {
                decimal fatorMultiplicador = this.RetornaFatorMultiplicador(symb.Instrumento, Convert.ToDouble(symb.VlrFechamento), Convert.ToDouble(symb.VlrUltima));
                ret = (symb.VlrUltima - symb.VlrAjuste) * qtd * fatorMultiplicador;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de resultado financeiro bmf: " + ex.Message, ex);
            }
            return ret;
        }
    }
}

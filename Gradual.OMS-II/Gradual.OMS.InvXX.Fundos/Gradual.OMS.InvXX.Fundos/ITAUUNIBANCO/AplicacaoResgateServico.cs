using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using log4net;
using System.Xml.Linq;
using System.Configuration;
using Gradual.OMS.InvXX.Fundos.Lib.UNIBANCO;
using System.Globalization;
using System.Xml;
using System.IO;
using Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO;
using System.Net;
using System.Collections;
using Gradual.OMS.InvXX.Fundos.Lib.FINANCIAL;
using Gradual.OMS.InvXX.Fundos.Lib.ITAUUNIBANCO;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO.Info;
using Ionic.Zip;
using System.ServiceModel;
using System.Web;

namespace Gradual.OMS.InvXX.Fundos.ITAUUNIBANCO
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AplicacaoResgateServico : IServicoControlavel
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private WaitOrTimerCallback ThreadResetAplicacaoResgateItauFinancial        = null;
        private WaitOrTimerCallback ThreadResetMonitorAplicacaoResgateFinancial = null;
        private WaitOrTimerCallback ThreadResetAplicacaoAgendadaFinancial       = null;
        private AutoResetEvent lThreadEvent = new AutoResetEvent(false);

        public CultureInfo gCultura = new CultureInfo("pt-BR");
        #endregion

        #region Propriedades
        private ServicoStatus _ServicoStatus 
        { 
            set; get; 
        }
        private string UsuarioFinancial
        {
            get { return ConfigurationManager.AppSettings["UsuarioFinancial"].ToString(); }
        }
        private string SenhaFinancial
        {
            get { return ConfigurationManager.AppSettings["SenhaFinancial"].ToString(); }
        }
        private Int64 IntervaloRelizarAplicacaoResgate
        {
            get { return Int64.Parse( ConfigurationManager.AppSettings["intervaloRelizarAplicacaoResgate"].ToString()); }
        }
        private Int64 IntervaloMudarStatusAplicacaoAgendada
        {
            get { return Int64.Parse(ConfigurationManager.AppSettings["intervaloMudarStatusAplicacaoAgendada"].ToString()); }
        }
        private Int64 IntervaloMonitorAplicacaoResgate
        {
            get { return Int64.Parse(ConfigurationManager.AppSettings["intervaloMonitorAplicacaoResgate"].ToString()); }
        }

        
        private string GetCodigoGestorItau
        {
            get { return ConfigurationManager.AppSettings["CodigoGestorItau"].ToString(); }
        }

        private string GetUsuarioItau
        {
            get { return ConfigurationManager.AppSettings["UsuarioItau"].ToString(); }
        }
        private string GetSenhaItau
        {
            get { return ConfigurationManager.AppSettings["SenhaItau"].ToString(); }
        }
        private string HorariosRealizarTrocarStatusAplicacao
        {
            get { return ConfigurationManager.AppSettings["HorariosRelizarTrocaStatusAplicacao"].ToString(); }
        }
        #endregion

        #region Construtores
        public AplicacaoResgateServico()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion
        
        #region Metodos
        private string InputDotString(string lValor)
        {
            string lRetorno = string.Empty;

            lRetorno = lValor.Insert(lValor.Length - 2, ",");

            return lRetorno;
        }

        private List<string> ListaHorarios(string pHorarios)
        {
            List<string> lretorno = new List<string>();

            try
            {
                if (pHorarios.Contains(";"))
                {
                    char[] lchars = { ';' };
                    string[] lhr = pHorarios.Split(lchars, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string hr in lhr) lretorno.Add(hr);
                }
                else if (!string.IsNullOrEmpty(pHorarios))
                {
                    lretorno.Add(pHorarios);
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("ListaHorarios() - ", ex);
            }

            return lretorno;
        }

        public List<AplicacaoResgateInfo> CarregaMonitorAplicacaoResgaste()
        {
            List<AplicacaoResgateInfo>  lRetorno = new List<AplicacaoResgateInfo>();
            try
            {
                string lUsuarioItau = ConfigurationManager.AppSettings["UsuarioItau"];

                string lSenhaItau = ConfigurationManager.AppSettings["SenhaItau"];

                string CodigoGestor = ConfigurationManager.AppSettings["CodigoGestorItau"];

                br.com.itaucustodia.www.DownloadArquivoServiceService cliente = new br.com.itaucustodia.www.DownloadArquivoServiceService();

                string lXml = cliente.historicoAplicacaoResgateEstornoXML(lUsuarioItau, lSenhaItau, CodigoGestor);

                XDocument document = XDocument.Parse(lXml.Replace("<![CDATA[","").Replace("]]>",""));

                var lListaResgateAplicacao = from operacao in document.Descendants("HistoricoAplicacaoResgateEstornoBean")
                                             select new
                                             {
                                                 CodigoGestor                   = operacao.Element("CDBANC").Value,
                                                 CodigoFundo                    = operacao.Element("CDFDO").Value,
                                                 CodigoBancoCliente             = operacao.Element("CDBANCLI").Value,
                                                 CodigoAgencia                  = operacao.Element("AGENCIA").Value,
                                                 CodigoConta                    = operacao.Element("CDCTA").Value,
                                                 DigitoConferencia              = operacao.Element("DAC10").Value,
                                                 CodigoSubConta                 = operacao.Element("SUBCONT").Value,
                                                 DataAplicacao                  = operacao.Element("DTAPLK1").Value,
                                                 CodigoSequenciaAplicacao       = operacao.Element("LANMOV1").Value,
                                                 TipoRegistro                   = operacao.Element("IDTIPREG").Value,
                                                 DataLancamento                 = operacao.Element("DTLANCT").Value,
                                                 TipoMovimento                  = operacao.Element("OPEMOV").Value,
                                                 CodigoSeqLancamento            = operacao.Element("LANMOV").Value,
                                                 DataProcessamento              = operacao.Element("DTAPROCE").Value,
                                                 QtdeCotasMovimento             = operacao.Element("QTCOTMVN").Value,
                                                 QtdeUfirMovimento              = operacao.Element("QTMVUFIR").Value,
                                                 VlrCotacaoMovimento            = operacao.Element("VLCOTMVN").Value,
                                                 VlrBrutoMovimento              = operacao.Element("VALBRUT").Value,
                                                 VlrCustoMovimento              = operacao.Element("VLCUST").Value,
                                                 VlrCustoAplicacao              = operacao.Element("VLCUSTAP").Value,
                                                 VlrCustoMedioResgate           = operacao.Element("VCUSTRSG").Value,
                                                 VlrIRMovimento                 = operacao.Element("VLIRRF").Value,
                                                 VlrIOFMovimento                = operacao.Element("VLIOFRMV").Value,
                                                 //VlrTaxaPerfomance            = operacao.Element("VLTXPRF").Value,
                                                 //VlrTaxaResgateAntecipado     = operacao.Element("VALTXSAI").Value,
                                                 VlrLiquidoCalculadoMovimento   = operacao.Element("VLIQCALC").Value,
                                                 VlrLiquidoSolicitado           = operacao.Element("VLIQSOL").Value,
                                             };
                
                foreach (var info in lListaResgateAplicacao)
                {
                    AplicacaoResgateInfo lAplicacaoResgate = new AplicacaoResgateInfo();
                    
                    lAplicacaoResgate.CodigoGestor                = info.CodigoGestor                  ;
                    lAplicacaoResgate.CodigoFundoItau             = info.CodigoFundo                   ;
                    lAplicacaoResgate.CodigoBancoCliente          = info.CodigoBancoCliente            ;
                    lAplicacaoResgate.CodigoAgencia               = info.CodigoAgencia                 ;
                    lAplicacaoResgate.CodigoConta                 = info.CodigoConta                   ;
                    lAplicacaoResgate.DigitoConferencia           = info.DigitoConferencia             ;
                    lAplicacaoResgate.CodigoSubConta              = info.CodigoSubConta                ;
                    lAplicacaoResgate.DataAplicacao               = info.DataAplicacao                 ;
                    lAplicacaoResgate.CodigoSequenciaAplicacao    = info.CodigoSequenciaAplicacao      ;
                    lAplicacaoResgate.TipoRegistro                = info.TipoRegistro                  ;
                    lAplicacaoResgate.DataLancamento              = info.DataLancamento                ;
                    lAplicacaoResgate.TipoMovimento               = info.TipoMovimento                 ;
                    lAplicacaoResgate.CodigoSeqLancamento         = info.CodigoSeqLancamento           ;
                    lAplicacaoResgate.DataProcessamento           = info.DataProcessamento             ;
                    lAplicacaoResgate.QtdeCotasMovimento          = info.QtdeCotasMovimento            ;
                    lAplicacaoResgate.QtdeUfirMovimento           = info.QtdeUfirMovimento             ;
                    lAplicacaoResgate.VlrCotacaoMovimento         = Convert.ToDecimal(InputDotString(info.VlrCotacaoMovimento),gCultura);
                    lAplicacaoResgate.VlrBrutoMovimento           = Convert.ToDecimal(InputDotString(info.VlrBrutoMovimento),gCultura);
                    lAplicacaoResgate.VlrCustoMovimento           = Convert.ToDecimal(InputDotString(info.VlrCustoMovimento),gCultura);
                    lAplicacaoResgate.VlrCustoAplicacao           = Convert.ToDecimal(InputDotString(info.VlrCustoAplicacao),gCultura);
                    lAplicacaoResgate.VlrCustoMedioResgate        = Convert.ToDecimal(InputDotString(info.VlrCustoMedioResgate),gCultura);
                    lAplicacaoResgate.VlrIRMovimento              = Convert.ToDecimal(InputDotString(info.VlrIRMovimento),gCultura);
                    lAplicacaoResgate.VlrIOFMovimento             = Convert.ToDecimal(InputDotString(info.VlrIOFMovimento),gCultura);
                    lAplicacaoResgate.VlrLiquidoCalculadoMovimento= Convert.ToDecimal(InputDotString(info.VlrLiquidoCalculadoMovimento),gCultura);
                    lAplicacaoResgate.VlrLiquidoSolicitado        = Convert.ToDecimal(InputDotString(info.VlrLiquidoSolicitado),gCultura);
                    //lAplicacaoResgate.VlrTaxaPerfomance           = Convert.ToDecimal(info.VlrTaxaPerfomance           ,gCultura);
                    //lAplicacaoResgate.VlrTaxaResgateAntecipado    = Convert.ToDecimal(info.VlrTaxaResgateAntecipado    ,gCultura);

                    lRetorno.Add(lAplicacaoResgate);
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método CarregaMonitorAplicacaoResgaste:" + ex.Message, ex);
            }
            return lRetorno;
        }

        public void ExportaMovimentoParaFinancial(AplicacaoResgateInfo pOperacao)
        {
            try
            {
                //gLogger.InfoFormat("");

                OperacaoCotista.OperacaoCotistaWSSoapClient lServico = new OperacaoCotista.OperacaoCotistaWSSoapClient();

                OperacaoCotista.ValidateLogin lLogin = new OperacaoCotista.ValidateLogin();

                lLogin.Username = UsuarioFinancial;
                lLogin.Password = SenhaFinancial;

                /* TipoOperacao
                 * 1 – Aplicação
                 * 2 – Resgate Bruto
                 * 3 – Resgate Bruto
                 * 4 – Resgate em Cotas
                 * 5 – Resgate Total
                 */

                /* Tipo Resgate
                 * 2 - Fifo
                 * 3 - Lifo
                 * 4 - Menor Imposto
                 * Em branco = Aplicação
                 */

                byte lTipoOperacao = 0;
                byte lTipoResgate = 0;
                string lTipoOperacaoString = "APLICACAO";

                switch (pOperacao.TipoMovimento)
                {
                    //case TabelaOperacoesInfo.ESTORNOAPLICAAO_OPERACAOINTERNA:
                    //case TabelaOperacoesInfo.ESTORNOAPLICACAO:
                    //case TabelaOperacoesInfo.CANCELAMENTOAPLICACAO:
                    //    return;
                    //break;
                    case TabelaOperacoesInfo.APLICACAO:
                    case TabelaOperacoesInfo.APLICAAOOPERACAOINTERNA:
                        lTipoOperacao = 1;
                        break;
                    //case TabelaOperacoesInfo.ESTORNORESGATE:
                    //    return;
                    //break;
                    //case TabelaOperacoesInfo.CANCELAMENTORESGATE:
                    //    return;
                    //break;
                    case TabelaOperacoesInfo.RESGATE_OPERACAOINTERNA:
                        lTipoOperacao = 2;
                        lTipoResgate = 2;
                        lTipoOperacaoString = "RESGATE";
                        break;
                    case TabelaOperacoesInfo.RESGATETOTALCOTA:
                    case TabelaOperacoesInfo.RESGATE:
                        lTipoOperacao = 4;
                        lTipoResgate = 2;
                        lTipoOperacaoString = "RESGATE";
                        break;
                    case TabelaOperacoesInfo.RESGATETOTAL_OPERACAOINTERNA:
                        lTipoOperacao = 5;
                        lTipoResgate = 2;
                        lTipoOperacaoString = "RESGATE";
                        break;
                    case TabelaOperacoesInfo.RESGATETOTAL:
                        lTipoOperacao = 5;
                        lTipoResgate = 2;
                        lTipoOperacaoString = "RESGATE";
                        break;
                }

                if (lTipoOperacao == 0) { return; }

                gLogger.InfoFormat("Exportando operação de [{0}] do cliente [{1}], Carteira [{2}], Cotas[{3}], Valor Liquido Calculado [{4}]",
                    lTipoOperacaoString,
                    pOperacao.CodigoCliente,
                    pOperacao.CodigoCarteira,
                    decimal.Parse(pOperacao.QtdeCotasMovimento, gCultura),
                    pOperacao.VlrLiquidoCalculadoMovimento
                    );

                OperacaoCotista.OperacaoCotistaViewModel lRetorno = lServico.Importa(lLogin,
                                                                        pOperacao.CodigoCliente,
                                                                        pOperacao.CodigoCarteira,
                                                                        DateTime.Parse(pOperacao.DataProcessamento),
                                                                        null,
                                                                        null,
                                                                        lTipoOperacao,
                                                                        lTipoResgate,
                                                                        decimal.Parse(pOperacao.QtdeCotasMovimento, gCultura),
                                                                        pOperacao.VlrCotacaoMovimento,
                                                                        pOperacao.VlrBrutoMovimento,
                                                                        pOperacao.VlrLiquidoCalculadoMovimento,
                                                                        pOperacao.VlrIRMovimento,
                                                                        pOperacao.VlrIOFMovimento,
                                                                        pOperacao.VlrTaxaPerfomance,
                                                                        pOperacao.VlrTaxaResgateAntecipado
                                                                        );
                
                ImportacaoItauDbLib lDb = new ImportacaoItauDbLib();

                pOperacao.IdOperacaoFinancial = lRetorno.IdOperacao;

                pOperacao.StatusMovimento = 3;

                switch(pOperacao.TipoMovimento)
                {
                    case "105":
                        pOperacao.TipoMovimento = "2";
                        break;
                    default:
                        pOperacao.TipoMovimento = "1";
                        break;
                }

                lDb.AtualizaAplicacaoResgateEmProcessamento(pOperacao);
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ExportaMovimentoParaFinancial:" + ex.Message, ex);
            }
        }

        private static XElement FindParameter(XElement element, string type)
        {
            return element.Elements("param").SingleOrDefault(p => (string)p.Attribute("id") == type);
        }

        #region Threads
        public void ThreadMonitorAplicacaoResgate(object sender, bool signed)
        {
            try
            {
                var lDb = new ImportacaoItauDbLib();

                DateTime lNow = DateTime.Now;
                
                gLogger.InfoFormat("Verificando STATUS de Aplicação/Resgates.");

                //List<AplicacaoResgateInfo> lListaAplicacaoResgate = CarregaMonitorAplicacaoResgaste();
                List<AplicacaoResgateInfo> lListaAplicacaoResgate = lDb.SelecionaAplicacaoResgateParaEnvio(123);

                if (lListaAplicacaoResgate.Count > 0)
                {
                    gLogger.InfoFormat("Foram encontradas o STATUS  [{0}] de Aplicação/Resgates.", lListaAplicacaoResgate.Count);
                }
                else
                {
                    gLogger.InfoFormat("NÃO FORAM encontradas Aplicação/Resgates.");
                }


                foreach (AplicacaoResgateInfo info in lListaAplicacaoResgate)
                {
                    //AplicacaoResgateInfo linfoTmp = lDb.SelecionaCodigoClienteFundo(info.CodigoBancoCliente, info.CodigoAgencia, info.CodigoConta);

                    //if (linfoTmp.CodigoInternoFundo == 0)
                    //{
                    //    gLogger.InfoFormat("Cliente não encontrato na tabela tbClientePosicaoFundos! Dados bancários: Banco [{0}], Agencia [{1}], Conta [{2}] Codigo Itaú FUNDO [{3}]", info.CodigoBancoCliente, info.CodigoAgencia, info.CodigoConta, info.CodigoFundoItau);
                    //    continue;
                    //}

                    //info.CodigoInternoFundo = linfoTmp.CodigoInternoFundo;

                    //info.CodigoCliente = linfoTmp.CodigoCliente;

                    StatusOperacaoEnum lTipoOperacao = StatusOperacaoEnum.SOLICITADO;

                    AplicacaoResgateInfo infoSalvar = new AplicacaoResgateInfo();

                    infoSalvar = info;

                    switch (info.TipoMovimento)
                    {
                        case TabelaOperacoesInfo.ESTORNOAPLICAAO_OPERACAOINTERNA:
                        case TabelaOperacoesInfo.ESTORNOAPLICACAO:
                        case TabelaOperacoesInfo.CANCELAMENTOAPLICACAO:
                            lTipoOperacao = StatusOperacaoEnum.CANCELADO;

                            break;
                        case TabelaOperacoesInfo.APLICACAO:
                        case TabelaOperacoesInfo.APLICAAOOPERACAOINTERNA:
                            lTipoOperacao = StatusOperacaoEnum.EXECUTADO;
                            break;
                        case TabelaOperacoesInfo.ESTORNORESGATE:
                            lTipoOperacao = StatusOperacaoEnum.CANCELADO;
                            break;
                        case TabelaOperacoesInfo.CANCELAMENTORESGATE:
                            lTipoOperacao = StatusOperacaoEnum.CANCELADO;
                            break;
                        case TabelaOperacoesInfo.RESGATE_OPERACAOINTERNA:
                            lTipoOperacao = StatusOperacaoEnum.EXECUTADO;
                            break;
                        case TabelaOperacoesInfo.RESGATETOTALCOTA:
                        case TabelaOperacoesInfo.RESGATE:
                            lTipoOperacao = StatusOperacaoEnum.EXECUTADO;
                            break;
                        case TabelaOperacoesInfo.RESGATETOTAL_OPERACAOINTERNA:
                            lTipoOperacao = StatusOperacaoEnum.EXECUTADO;
                            break;
                        case TabelaOperacoesInfo.RESGATETOTAL:
                            lTipoOperacao = StatusOperacaoEnum.EXECUTADO;
                            break;
                    }

                    if (lTipoOperacao == StatusOperacaoEnum.EXECUTADO) infoSalvar.StAprovado = "S"; else infoSalvar.StAprovado = "N";

                    infoSalvar.StatusMovimento = (int)lTipoOperacao;

                    infoSalvar.DtInclusao = Convert.ToDateTime(info.DataLancamento.Substring(0, 2) + "/" +
                                                                info.DataLancamento.Substring(2, 2) + "/" +
                                                                info.DataLancamento.Substring(4, 4), gCultura);

                    gLogger.InfoFormat("****Atualizando operações de aplicação e resgate********");
                    gLogger.InfoFormat("********************Tipo Movimento      : [{0}]", info.TipoMovimento);
                    gLogger.InfoFormat("********************CodigoCliente       : [{0}]", info.CodigoCliente);
                    gLogger.InfoFormat("********************Codigo Interno Fundo: [{0}]", info.CodigoInternoFundo);
                    gLogger.InfoFormat("********************VlrLiquidoSolicitado: [{0}]", info.VlrLiquidoSolicitado.ToString("N2", gCultura));
                    gLogger.InfoFormat("********************VlrBrutoMovimento   : [{0}]", info.VlrBrutoMovimento.ToString("N2", gCultura));
                    gLogger.InfoFormat("********************DtInclusao          : [{0}]", info.DtInclusao);
                    gLogger.InfoFormat("********************StAprovado          : [{0}]", info.StAprovado);
                    gLogger.InfoFormat("********************StatusMovimento     : [{0}]", info.StatusMovimento);
                    gLogger.InfoFormat("********************************************************");

                    infoSalvar.TipoMovimento = infoSalvar.TipoMovimento == "030" ? "1" : "2";

                    lDb.AtualizaAplicacaoResgateMovimento(infoSalvar);

                    string lAssunto = string.Empty;

                    this.EnviarEmailTesourariaControladoria( info);
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ThreadMonitorAplicacaoResgate:", ex);
            }
        }

        public void ThreadAplicacaoResgate(object sender, bool signed)
        {
            try
            {
                var lDestinatarios = new List<string>();

                DateTime lNow = DateTime.Now;

                List<string> listHorarios = ListaHorarios(HorariosRealizarTrocarStatusAplicacao);

                if (listHorarios.Contains(lNow.ToString("HH:mm")))
                {
                    ImportacaoItauDbLib lDb = new ImportacaoItauDbLib();

                    gLogger.InfoFormat("Verificando se há solicitações de Aplicação e Resgate.");

                    List<AplicacaoResgateInfo> ListaAplicacaoResgate = lDb.SelecionaAplicacaoResgateParaEnvio();

                    if (ListaAplicacaoResgate.Count > 0)
                    {
                        gLogger.InfoFormat("Foram encontradas [{0}] solicitações de Aplicação/Resgate.", ListaAplicacaoResgate.Count);
                    }
                    else
                    {
                        gLogger.InfoFormat("NÃO FORAM encontradas solicitações de Aplicação/Resgate.");
                    }

                    foreach (AplicacaoResgateInfo info in ListaAplicacaoResgate)
                    {
                        //-->> Layout do arquivo XML - ARQEOP_XML

                        /************************************************/
                        /* Envio de operações - Aplicação e Resgate ITAÚ*/
                        /************************************************/

                        if (!info.CodigoFundoItau.Equals(string.Empty))
                        {
                            string lTipoLiquidacao       = string.Empty;
                            string lDataLancamento       = string.Empty;
                            string lPrimeiraContaCredito = string.Empty;


                            if (info.TipoMovimento == "030")
                            {
                                lTipoLiquidacao = TabelaLiquidacaoInfo.APLICACAODISPONIVEL;
                                lPrimeiraContaCredito = "                        ";
                            }
                            else
                            {
                                lTipoLiquidacao = TabelaLiquidacaoInfo.RESGATEDOCCOMPE;

                                if (info.VlrLiquidoSolicitado > 1000)
                                {
                                    lTipoLiquidacao = TabelaLiquidacaoInfo.RESGATETEDSTR;
                                }

                                //lTipoLiquidacao = lDb.SelecionaTipoLiquidacaoMovimento(info.CodigoCliente, info.VlrLiquidoSolicitado);

                                lPrimeiraContaCredito = string.Concat(info.CodigoBancoCliente, info.CodigoConta, info.DigitoConferencia);

                                lPrimeiraContaCredito = "0237023740110787".PadRight(24, ' ');
                            }

                            if (!string.IsNullOrWhiteSpace(info.DataLancamento))
                            {
                                lDataLancamento = info.DataLancamento;
                            }
                            else
                            {
                                lDataLancamento = "        ";
                            }

                            string EBUSINESSID = this.GetUsuarioItau;
                            string SENHA       = this.GetSenhaItau;
                            string CDBANC      = this.GetCodigoGestorItau;
                            string CDFDO       = info.CodigoFundoItau;
                            string CDBANCLI    = this.GetCodigoGestorItau; ;
                            string AGENCIA     = info.CodigoAgencia;
                            string CDCTA       = info.CodigoConta;
                            string DAC10       = info.DigitoConferencia;
                            string SUBCONT     = "201"; //info.CodigoSubConta;
                            string OPEMOV      = info.TipoMovimento;
                            string VLIQSOL     = info.VlrLiquidoSolicitado.ToString("N").Replace(",", "").Replace(".", "").PadLeft(15, '0');
                            string BCOAGCT1    = lPrimeiraContaCredito;
                            string IDOPEMAC    = "000000";
                            string CDTIPLIQ    = lTipoLiquidacao;
                            string CDAPL       = "0000000000";
                            string IDTIPCT1    = "C";
                            string DATAGEND    = "        "; //lDataLancamento.ToString("ddMMyyyy");
                            string DTLANCT     = "        ";//lDataLancamento;

                            string lStringXML = @"<itaumsg>";
                            lStringXML += @"<parameter>";
                            lStringXML += @"<param id=""campo0""  value=""" + EBUSINESSID   + "\" />"; //--> EBUSINESSID - Código do Usuário                              - Tam 33
                            lStringXML += @"<param id=""campo1""  value=""" + SENHA         + "\" />"; //--> SENHA       - Senha                                          - Tam 8
                            lStringXML += @"<param id=""campo2""  value=""" + CDBANC        + "\" />"; //--> CDBANC      - Código do Gestor do Fundo                      - Tam 6
                            lStringXML += @"<param id=""campo3""  value=""" + CDFDO         + "\" />"; //--> CDFDO       - Código do Fundo                                - Tam 5
                            lStringXML += @"<param id=""campo4""  value=""" + CDBANCLI      + "\" />"; //--> CDBANCLI    - Código do Gestor do Cliente                    - Tam 6
                            lStringXML += @"<param id=""campo5""  value=""" + AGENCIA       + "\" />"; //--> AGENCIA     - Código da Agência do Cliente                   - Tam 4
                            lStringXML += @"<param id=""campo6""  value=""" + CDCTA         + "\" />"; //--> CDCTA       - Código da Conta                                - Tam 9
                            lStringXML += @"<param id=""campo7""  value=""" + DAC10         + "\" />"; //--> DAC10       - Dígito de Auto Conferência (Agência/Conta)     - Tam 1
                            lStringXML += @"<param id=""campo8""  value=""" + SUBCONT       + "\" />"; //--> SUBCONT     - Código da Subconta de Fundo de Investimento    - Tam 3
                            lStringXML += @"<param id=""campo9""  value=""" + OPEMOV        + "\" />"; //--> OPEMOV      - Código do Tipo de Operação                     - Tam 3
                            lStringXML += @"<param id=""campo10"" value=""" + VLIQSOL       + "\" />"; //--> VLIQSOL     - Valor Líquido ou Quantidade de Cotas Solicitado- Tam 15
                            lStringXML += @"<param id=""campo11"" value=""" + BCOAGCT1      + "\" />"; //--> BCOAGCT1    - Identificador da Primeira Conta para Crédito   - Tam 24
                            lStringXML += @"<param id=""campo12"" value=""" + IDOPEMAC      + "\" />"; //--> IDOPEMAC    - Identificador do Arquivo                       - Tam 6
                            lStringXML += @"<param id=""campo13"" value=""" + CDTIPLIQ      + "\" />"; //--> CDTIPLIQ    - Código do Tipo de Liquidação                   - Tam 1
                            lStringXML += @"<param id=""campo14"" value=""" + CDAPL         + "\" />"; //--> CDAPL       - Número do Certificado                          - Tam 10
                            lStringXML += @"<param id=""campo15"" value=""" + IDTIPCT1      + "\" />"; //--> IDTIPCT1    - Identificador do Tipo da Conta para Crédito    - Tam 1
                            lStringXML += @"<param id=""campo16"" value=""" + DATAGEND      + "\" />"; //--> DATAGEND    - Data do Agendamento do Resgate                 - Tam 8
                            lStringXML += @"<param id=""campo17"" value=""" + DTLANCT       + "\" />"; //--> DTLANCT     - Data de Lançamento                             - Tam 8
                            lStringXML += @"</parameter>";
                            lStringXML += @"</itaumsg>";

                            gLogger.InfoFormat("Enviando informações de Aplicação e Resgate {0}", lStringXML);

                            string lResponseHtml = string.Empty;

                            lResponseHtml = new WebClient().DownloadString("http://www.itaucustodia.com.br/PassivoWebServices/xmlmva.jsp?strXML=" + lStringXML);

                            gLogger.InfoFormat("Recebeu informações de Aplicação e Resgate {0}", lResponseHtml);

                            XDocument document = XDocument.Parse(lResponseHtml);

                            if (info.TipoMovimento == "030")
                            {
                                info.TipoMovimento = "1";
                            }
                            else
                            {
                                info.TipoMovimento = "2";
                            }

                            var lRetornoResgateAplicacao = from operacao in document.Root.Elements("parameter")
                                                            select new
                                                                {
                                                                    MsgRetorno = FindParameter(operacao, "MSGRETORNO")
                                                                };

                            if (lResponseHtml.Contains("OPERACAO+EFETUADA"))
                            {
                                info.StatusMovimento = 3; //-->> Processamento

                                info.StAprovado = "True";

                                foreach (var retorno in lRetornoResgateAplicacao)
                                {
                                    info.DsMotivoStatus = System.Web.HttpUtility.UrlDecode(retorno.MsgRetorno.Attribute("value").Value);
                                }
                            }
                            else
                            {
                                info.StatusMovimento = 5; //--> Rejeitado

                                info.StAprovado = "False";

                                foreach (var retorno in lRetornoResgateAplicacao)
                                {
                                    info.DsMotivoStatus = System.Web.HttpUtility.UrlDecode(retorno.MsgRetorno.Attribute("value").Value);
                                }
                            }
                        }

                            

                        if (info.TipoMovimento == "1")
                        {
                            info.TipoMovimento = "030";
                        }
                        else
                        {
                            info.TipoMovimento = "105";
                        }

                        this.ExportaMovimentoParaFinancial(info);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ThreadAplicacaoResgate:" + ex.Message, ex);
            }
        }

        public void ThreadMudaStatusAplicacaoAgendada(object sender, bool signed)
        {
            try
            {
                DateTime lNow = DateTime.Now;

                List<string> listHorarios = ListaHorarios(HorariosRealizarTrocarStatusAplicacao);

                if ( listHorarios.Contains(lNow.ToString("HH:mm")))
                {
                    ImportacaoItauDbLib lDb = new ImportacaoItauDbLib();

                    lDb.AtualizaAplicacaoAgendada();
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ThreadMudaStatusAplicacaoAgendada:" + ex.Message, ex);
            }
        }
        #endregion

        private void EnviarEmailTesourariaControladoria(AplicacaoResgateInfo info)
        {
            try
            {
                var lAtivador = Ativador.Get<IServicoEmail>();
                List<string> lDestinatarios = new List<string>();

                if (info.EhFundoItau)
                {
                    lDestinatarios.Add(ConfigurationManager.AppSettings["EmailMovimentacao"].ToString());
                    lDestinatarios.Add(ConfigurationManager.AppSettings["EmailTesouraria"].ToString());
                }
                else
                {
                    lDestinatarios.Add(ConfigurationManager.AppSettings["EmailMovimentacaoWM"].ToString());
                }

                string lNomeArquivo        = "AvisoAplicacaoResgate.htm";
                string lPath               = ConfigurationManager.AppSettings["ArquivosEmail"].ToString();
                string lCaminho            = Path.Combine(lPath, lNomeArquivo);
                string lCorpoEmail         = File.ReadAllText(lCaminho);
                string lArquivoConcatenado = lCorpoEmail;

                var lVariaveis = new Dictionary<string, string>();
                
                string lAssunto = string.Empty;

                if (info.TipoMovimento == "1")
                {
                    lAssunto = "Aplicação Efetuada - Cliente: " + info.CodigoCliente + " no Fundo " + info.NomeFundo;

                    lVariaveis.Add("##NomeCliente##", info.NomeCliente);

                    lVariaveis.Add("##CodigoCliente##", info.CodigoCliente.ToString());

                    lVariaveis.Add("##NomeFundo##", info.NomeFundo);

                    lVariaveis.Add("##ValorSolicitado##", info.VlrBrutoMovimento.ToString("N2", gCultura));

                    lVariaveis.Add("##Data##", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                    lVariaveis.Add("##Operacao##", "Aplicação");

                    lVariaveis.Add("##TotalParcial##", "-");

                    lVariaveis.Add("##DataLiquidacao##", "n\\d");
                }
                else
                {
                    lAssunto = "Resgate solicitado - Cliente: " + info.CodigoCliente + " no Fundo " + info.NomeFundo;

                    lVariaveis.Add("##NomeCliente##", info.NomeCliente);

                    lVariaveis.Add("##CodigoCliente##", info.CodigoCliente.ToString());

                    lVariaveis.Add("##NomeFundo##", info.NomeFundo);

                    lVariaveis.Add("##ValorSolicitado##", info.VlrBrutoMovimento.ToString("N2"));

                    lVariaveis.Add("##Data##", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                    lVariaveis.Add("##Operacao##", "Resgate");

                    lVariaveis.Add("##TotalParcial##", info.EhResgateTotal ? "Resgate Total" : "Resgate Parcial");

                    lVariaveis.Add("##DataLiquidacao##", info.DsDiasPagResgate);
                }

                foreach (KeyValuePair<string, string> lItem in lVariaveis)
                {
                    gLogger.InfoFormat("PaginaBase.EnviarEmail: Arquivo [{0}], Substituindo [{1}] por [{2}]...", lCaminho, lItem.Key, lItem.Value);

                    lArquivoConcatenado = lArquivoConcatenado.Replace(lItem.Key, lItem.Value);
                }

                lCorpoEmail                        = lArquivoConcatenado;

                var lEmailEntrada                  = new EnviarEmailRequest();
                lEmailEntrada.Objeto               = new EmailInfo();
                lEmailEntrada.Objeto.Assunto       = lAssunto;
                lEmailEntrada.Objeto.Destinatarios = lDestinatarios;
                lEmailEntrada.Objeto.Remetente     = ConfigurationManager.AppSettings["EmailRemetenteGradual"].ToString();
                lEmailEntrada.Objeto.CorpoMensagem = lCorpoEmail;

                EnviarEmailResponse lEmailRetorno = lAtivador.Enviar(lEmailEntrada);

                if ( lEmailRetorno.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    gLogger.InfoFormat("E-mail enviado com sucesso...Assunto - [{0}] - Corpo - ", lAssunto, lCorpoEmail);
                }
                else
                {
                    gLogger.ErrorFormat("O e-mail não foi enviado - Erro - [{0}]", lEmailRetorno.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro ao enviar e-mail- ", ex);
            }
        }

        public void StartAplicacaoResgate(object sender)
        {
            try
            {
                gLogger.Info("StartAplicacaoResgate - Iniciando Serviço de Aplicacao e Resgate");

                //ThreadResetAplicacaoResgateItauFinancial = new WaitOrTimerCallback(ThreadAplicacaoResgate);

                //ThreadPool.RegisterWaitForSingleObject(lThreadEvent, ThreadResetAplicacaoResgateItauFinancial, null, this.IntervaloMudarStatusAplicacaoAgendada, false);

                ThreadResetMonitorAplicacaoResgateFinancial = new WaitOrTimerCallback(ThreadMonitorAplicacaoResgate);

                ThreadPool.RegisterWaitForSingleObject(lThreadEvent, ThreadResetMonitorAplicacaoResgateFinancial, null, this.IntervaloMonitorAplicacaoResgate, false);

                ThreadResetAplicacaoAgendadaFinancial = new WaitOrTimerCallback(ThreadMudaStatusAplicacaoAgendada);

                ThreadPool.RegisterWaitForSingleObject(lThreadEvent, ThreadResetAplicacaoAgendadaFinancial, null, this.IntervaloMudarStatusAplicacaoAgendada, false);

                //thThreadAplicacaoResgate      = new Thread(new ThreadStart(ThreadAplicacaoResgate));
                //thThreadAplicacaoResgate.Name = "ThreadAplicacaoResgate";
                //thThreadAplicacaoResgate.Start();

                //thThreadMonitorAplicacaoResgate      = new Thread(new ThreadStart(ThreadMonitorAplicacaoResgate));
                //thThreadMonitorAplicacaoResgate.Name = "ThreadMonitorAplicacaoResgate";
                //thThreadMonitorAplicacaoResgate.Start();

                

                //thThreadAplicacaoAgendada = new Thread(new ThreadStart(ThreadMudaStatusAplicacaoAgendada));
                //thThreadAplicacaoAgendada.Name = "ThreadAplicacaoAgendada";
                //thThreadAplicacaoAgendada.Start();

                //thThreadExportacaoImportacaoCliente = new Thread(new ThreadStart(ThreadExportacaoImportacaoClienteItau))

                gLogger.Info("*****************************************************************");
                gLogger.Info("***********Processo de inicialização finalizado******************");
                gLogger.Info("*****************************************************************");
            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro ao acessar o metodo StartAplicacaoResgate.", ex);
            }
        }

        public void IniciarServico()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                
                _ServicoStatus                    = ServicoStatus.EmExecucao;

                this.StartAplicacaoResgate(null);
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
                _ServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("PararServico()  StackTrace - {0}",ex.StackTrace);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }
        #endregion
    }
}

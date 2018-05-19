using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.ServiceModel;
using System.Threading;
using Gradual.OMS.InvXX.Fundos.DbLib;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA;
using Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO;
using Ionic.Zip;
using Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO.Info;
using Gradual.OMS.InvXX.Fundos.Lib.ANBIMA;

namespace Gradual.OMS.InvXX.Fundos
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ImportacaoFundosServico : IServicoControlavel
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool _bKeepRunningItau = false;
        private ServicoStatus _ServicoStatus { set; get; }
        private List<string> gListaCodigoAnbima = new List<string>();
        private WaitOrTimerCallback ThreadResetAnbima = null;
        private AutoResetEvent lThreadEvent = new AutoResetEvent(false);
        #endregion

        #region Prorpiedades
        private Int64 IntervaloImportacaoRentabilidadeAnbima
        {
            get { return Int64.Parse(ConfigurationManager.AppSettings["intervaloImportacaoRentabilidadeAnbima"]); }
        }
        

        private string HorariosImportacaoRentabilidade
        {
            get { return ConfigurationManager.AppSettings["HorariosImportacaoRentabilidadeANBIMA"].ToString(); }
        }
        #endregion

        #region Construtor
        public ImportacaoFundosServico()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        #region Métodos

        public void ImportacaoAvulsaANBIMA()
        {
            try
            {
                gLogger.Debug("Obtendo relacao de arquivos para serem importados");

                ImportacaoDbLib lImpDB = new ImportacaoDbLib();

                List<ProdutosInfo> lProdutos = lImpDB.PesquisarProduto();

                List<ProdutosInfo> ListaProduto = lImpDB.ListarProduto();

                Dictionary<string, int> ListaIdProdutoAnbima = new Dictionary<string, int>();

                ListaProduto.ForEach(produto =>
                {
                    if (!ListaIdProdutoAnbima.ContainsKey(produto.IdCodigoAnbima))
                    {
                        ListaIdProdutoAnbima.Add(produto.IdCodigoAnbima, produto.IdProduto);
                    }
                });

                this.gListaCodigoAnbima.Clear();

                lProdutos.ForEach(produto =>
                {
                    gLogger.InfoFormat("Fundo encontrado: [{0} - {1}] ", produto.IdCodigoAnbima, produto.NomeProduto);

                    gListaCodigoAnbima.Add(produto.IdCodigoAnbima);
                });

                LeituraArquivos lLeitura = new LeituraArquivos();

                //List<FundosDiaInfo> lFundosDia = lLeitura.LeArquivoFundosDiaAvulsa();

                List<FundosMesInfo> lFundosMes = lLeitura.LeArquivoFundosMesAvulsa();

                //List<FundosMovCotaInfo> lFundosMovCota = lLeitura.LeArquivoFundosMovCotaAvulsa();

                //List<TaxaAdministracaoInfo> lFundosTaxaAdm = lLeitura.LeArquivoFundosTaxaAdm();

                //lFundosDia.ForEach(
                //    fundo =>
                //    {
                //        if (gListaCodigoAnbima.Count > 0 && gListaCodigoAnbima.Contains(fundo.CodigoFundo))
                //        {
                //            lImpDB.SalvarRentabilidadeDiaria(fundo);

                //            gLogger.InfoFormat("Rentabilidade Dia -> [{0}] importado com sucesso", fundo.CodigoFundo);
                //        }
                //    });

                lFundosMes.ForEach(
                    fundo =>
                    {
                        if (gListaCodigoAnbima.Count > 0 && gListaCodigoAnbima.Contains(fundo.CodigoFundo))
                        {
                            lImpDB.SalvarRentabilidadeMes(fundo);

                            gLogger.InfoFormat("Rentabiliadde Mês -> [{0}] importado com sucesso", fundo.CodigoFundo);
                        }
                    });

                //lFundosMovCota.ForEach(
                //    fundo =>
                //    {
                //        if (gListaCodigoAnbima.Count > 0 ) 
                //        {
                //            if (ListaIdProdutoAnbima.ContainsKey(fundo.CodigoFundo))
                //            {
                //                fundo.IdProduto = ListaIdProdutoAnbima[fundo.CodigoFundo];

                //                lImpDB.SalvarProdutoMovimento(fundo);

                //                gLogger.InfoFormat("Fundos Mov Cota -> [{0}] importado com sucesso", fundo.CodigoFundo);
                //            }
                //        }
                //    });
                
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o metodo ImportacaoAvulsaANBIMA.", ex);
            }
        }

        public void ThreadImportacaoAnbima(object sender, bool signed)
        {
            DateTime lNow = DateTime.Now;

            List<string> listHorarios = ListaHorarios(this.HorariosImportacaoRentabilidade);

            try
            {
                if (listHorarios.Contains(lNow.ToString("HH:mm")))
                {
                    //var lDb = new ImportacaoANBIMADbLib();

                    //DateTime dtInicial = DateTime.Now.AddDays(-60);//.AddYears(-5);

                    //var listaAnbima = lDb.GetListaCodigoAnbimaImportacao();

                    //foreach ( KeyValuePair<int, string> fundo in listaAnbima)
                    //{
                    //    string CodigoAnbima = fundo.Value;

                    //    gLogger.InfoFormat("Importando fundo dados do Fundo = [{0}]", CodigoAnbima);
                    //    lDb.ImportarFundos(CodigoAnbima);
                    //    gLogger.InfoFormat("Importação dos dados de fundo [{0}] efetuada com sucesso", CodigoAnbima);

                    //    gLogger.InfoFormat("Importando Taxa Admin do fundo [{0}]", CodigoAnbima);
                    //    lDb.ImportarTaxaAdm(CodigoAnbima);
                    //    gLogger.InfoFormat("Importação da taxa de admin do fundo [{0}] efetuada com sucesso", CodigoAnbima);

                    //    gLogger.InfoFormat("Importando Movimento de cotas do fundo [{0}]", CodigoAnbima);
                    //    lDb.ImportarFundosMovimentoCota(CodigoAnbima);
                    //    gLogger.InfoFormat("Importação do Movimento de cotas do fundo [{0}] efetuada com sucesso", CodigoAnbima);

                    //    gLogger.InfoFormat("Importando Status do fundo [{0}]", CodigoAnbima);
                    //    lDb.ImportarFundosStatus(CodigoAnbima);
                    //    gLogger.InfoFormat("Importação dos Status do fundo [{0}] efetuada com sucesso", CodigoAnbima);
                    //    /*
                    //    gLogger.InfoFormat("Importando Rentabilidade dia (BackUp) do fundo [{0}]", CodigoAnbima);
                    //    lDb.ImportarRentabilidadeDia(CodigoAnbima, dtInicial);
                    //    gLogger.InfoFormat("Importação Rentabilidade dia (BackUp) do fundo [{0}] efetuada com sucesso", CodigoAnbima);

                    //    gLogger.InfoFormat("Importando Rentabilidade Mes (BackUp) do fundo [{0}]", CodigoAnbima);
                    //    lDb.ImportarRentabilidadeMes(CodigoAnbima, dtInicial);
                    //    gLogger.InfoFormat("Importação Rentabilidade Mes (BackUp) do fundo [{0}] efetuada com sucesso", CodigoAnbima);
                    //    */
                    //    gLogger.InfoFormat("Importando Movimento de cotas (Site) do [{0}]", CodigoAnbima);
                    //    lDb.ImportarMovimentoCotaSite(CodigoAnbima);
                    //    gLogger.InfoFormat("Importação do Movimento de cotas (Site) Fundo [{0}] efetuada com sucesso", CodigoAnbima);

                    //    gLogger.InfoFormat("Importando Rentabilidade Dia (Site) do fundo [{0}]", CodigoAnbima);
                    //    lDb.ImportarRentabilidadeDiaSite(CodigoAnbima, dtInicial);
                    //    gLogger.InfoFormat("Importação Rentabilidade Dia (Site) do fundo [{0}] efetuada com sucesso", CodigoAnbima);

                    //    gLogger.InfoFormat("Importando Rentabilidade Mes (Site) do fundo [{0}]", CodigoAnbima);
                    //    lDb.ImportarRentabilidadeMesSite(CodigoAnbima, dtInicial);
                    //    gLogger.InfoFormat("Importação Rentabilidade Mes (Site) do fundo [{0}] efetuada com sucesso", CodigoAnbima);
                    //}

                    //gLogger.InfoFormat("Importando Indicadores");
                    //lDb.ImportarIndicadores();
                    //gLogger.InfoFormat("Importação dos dados de Indicadores efetuada com sucesso");

                    //gLogger.InfoFormat("Importando Indicadores Mes");
                    //lDb.ImportarIndicadoresMes(dtInicial);
                    //gLogger.InfoFormat("Importação dos dados de Indicadores Mes efetuada com sucesso");

                    gLogger.Debug("Obtendo relacao de arquivos para serem importados");

                    SharpSSH lSftp = new SharpSSH();

                    bool lTransferiu = lSftp.TranferirArquivo();

                    if (lTransferiu)
                    {
                        ImportacaoDbLib lImpDB = new ImportacaoDbLib();

                        List<ProdutosInfo> lProdutos = lImpDB.PesquisarProduto();

                        List<ProdutosInfo> ListaProduto = lImpDB.ListarProduto();

                        Dictionary<string, int> ListaIdProdutoAnbima = new Dictionary<string, int>();

                        ListaProduto.ForEach(produto => {
                            if (!ListaIdProdutoAnbima.ContainsKey(produto.IdCodigoAnbima))
                            {
                                ListaIdProdutoAnbima.Add(produto.IdCodigoAnbima, produto.IdProduto);
                            }
                        });

                        this.gListaCodigoAnbima.Clear();

                        lProdutos.ForEach(produto =>
                        {
                            gLogger.InfoFormat("Fundo encontrado: [{0} - {1}] ", produto.IdCodigoAnbima, produto.NomeProduto);

                            gListaCodigoAnbima.Add(produto.IdCodigoAnbima);
                        });

                        LeituraArquivos lLeitura                       = new LeituraArquivos();
                        List<FundosDiaInfo> lFundosDia                 = lLeitura.LeArquivoFundosDia();
                        List<FundosMesInfo> lFundosMes                 = lLeitura.LeArquivoFundosMes();
                        //List<FundosMovCotaInfo> lFundosMovCota         = lLeitura.LeArquivoFundosMovCota();
                        List<TaxaAdministracaoInfo> lTaxaAdministracao = lLeitura.LeArquivoFundosTaxaAdm();
                        List<ANBIMAIndicadoresMesInfo> lIndicadoresMes = lLeitura.LerArquivoIndcadoresMes();

                        lIndicadoresMes.ForEach(
                            indicador => {
                                lImpDB.SalvarIndicadoresMes(indicador);
                                gLogger.InfoFormat("Indicador  -> [{0}] importado com sucesso", indicador.CodigoIndicador);

                            });

                        lTaxaAdministracao.ForEach(
                            taxa => {
                                if (gListaCodigoAnbima.Count > 0 && gListaCodigoAnbima.Contains(taxa.CodigoFundo))
                                {
                                    lImpDB.SalvarTaxaAdministrador(taxa);

                                    gLogger.InfoFormat("Taxa administracao  -> [{0}] importada com sucesso", taxa.CodigoFundo);
                                }
                            }
                            );

                        lFundosDia.ForEach(
                            fundo =>
                            {
                                if (gListaCodigoAnbima.Count > 0 && gListaCodigoAnbima.Contains(fundo.CodigoFundo))
                                {
                                    lImpDB.SalvarRentabilidadeDiaria(fundo);

                                    gLogger.InfoFormat("Rentabilidade Dia -> [{0}] importado com sucesso", fundo.CodigoFundo);
                                }
                            });

                        lFundosMes.ForEach(
                            fundo =>
                            {
                                if (gListaCodigoAnbima.Count > 0 && gListaCodigoAnbima.Contains(fundo.CodigoFundo))
                                {
                                    lImpDB.SalvarRentabilidadeMes(fundo);

                                    gLogger.InfoFormat("Rentabiliadde Mês -> [{0}] importado com sucesso", fundo.CodigoFundo);
                                }
                            });

                        //lFundosMovCota.ForEach(
                        //    fundo =>
                        //    {
                        //        if (gListaCodigoAnbima.Count > 0 && gListaCodigoAnbima.Contains(fundo.CodigoFundo))
                        //       // if (gListaCodigoAnbima.Count > 0)
                        //        {
                        //            if (ListaIdProdutoAnbima.ContainsKey(fundo.CodigoFundo))
                        //            {
                        //                fundo.IdProduto = ListaIdProdutoAnbima[fundo.CodigoFundo];

                        //                lImpDB.SalvarProdutoMovimento(fundo);

                        //                gLogger.InfoFormat("Fundos Mov Cota -> [{0}] importado com sucesso", fundo.CodigoFundo);
                        //            }
                        //        }
                        //    });

                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ThreadImportacaoAnbima:" + ex.Message, ex);
            }

            //Thread.Sleep(250);
        }
        
        public void StartImportacaoANBIMA(object sender)
        {
            try
            {
                gLogger.Info("StartImportacao ANBIMA - Iniciando Serviço de Importação de Rentabilidade de Fundos");

                ThreadResetAnbima = new WaitOrTimerCallback(ThreadImportacaoAnbima);

                ThreadPool.RegisterWaitForSingleObject(lThreadEvent, ThreadResetAnbima, null, this.IntervaloImportacaoRentabilidadeAnbima, false);

                //thThreadImportacao      = new Thread(new ThreadStart(ThreadImportacaoAnbima));
                //thThreadImportacao.Name = "ThreadImportacao";
                //thThreadImportacao.Start();

                gLogger.Info("*****************************************************************");
                gLogger.Info("***********Processo de inicialização finalizado******************");
                gLogger.Info("*****************************************************************");
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o metodo StartImportacaoANBIMA.", ex);
            }
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

        public void IniciarServico()
        {
            try
            {
               
                _ServicoStatus    = ServicoStatus.EmExecucao;
                this.StartImportacaoANBIMA(null);
            }
            catch (Exception ex)
            {
                gLogger.Error("ImportacaoFundosServico IniciarServico() - ", ex);
            }
        }

        public void PararServico()
        {
            try
            {
                gLogger.Info("Parando o servico de IMportação de arquivos Anbima");
                _ServicoStatus = ServicoStatus.Parado;
                gLogger.Info("Servico parado com sucesso.");
            }
            catch (Exception ex)
            {
                gLogger.Error("ImportacaoFundosServico ANBIMA PararServico() - ", ex);

                _ServicoStatus = ServicoStatus.Erro;
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        public void ImportarDadosFundos(string CodigoAnbima, DateTime dtInicial)
        {
            var lDb = new ImportacaoANBIMADbLib();

            lDb.ImportarFundos(CodigoAnbima);
            lDb.ImportarTaxaAdm(CodigoAnbima);
            //lDb.ImportarFundosMovimentoCota(CodigoAnbima);
            //lDb.ImportarFundosStatus(CodigoAnbima);
            //lDb.ImportarRentabilidadeDia(CodigoAnbima, dtInicial);
            //lDb.ImportarRentabilidadeMes(CodigoAnbima, dtInicial);
            //lDb.ImportarIndicadores();
            //lDb.ImportarIndicadoresMes(dtInicial);
            lDb.ImportarFundosSite(CodigoAnbima);
            lDb.ImportarMovimentoCotaSite(CodigoAnbima);
            lDb.ImportarRentabilidadeDiaSite(CodigoAnbima, dtInicial);
            lDb.ImportarRentabilidadeMesSite(CodigoAnbima, dtInicial);

        }
        #endregion
    }
}

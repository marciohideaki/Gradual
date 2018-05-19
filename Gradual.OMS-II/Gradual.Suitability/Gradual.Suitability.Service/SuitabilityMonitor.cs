using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.InteropServices;

namespace Gradual.Suitability.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SuitabilityMonitor: Gradual.OMS.Library.Servicos.IServicoControlavel, Gradual.Suitability.Service.Lib.ISuitabilityMonitor
    {
        private Gradual.OMS.Library.Servicos.ServicoStatus _status = Gradual.OMS.Library.Servicos.ServicoStatus.Parado;


        public List<DateTime> gListaDatas = new List<DateTime>();

        #region Carga de configurações
        private String lEmailDe                 = System.Configuration.ConfigurationManager.AppSettings["EmailDe"].ToString();
        private String lEmailErroPara           = System.Configuration.ConfigurationManager.AppSettings["EmailParaErro"].ToString();
        private String lEmailCCO                = System.Configuration.ConfigurationManager.AppSettings["EmailCCO"].ToString();
        private String lEmailSubject            = System.Configuration.ConfigurationManager.AppSettings["EmailSubject"].ToString();
        private String lCampoAssunto            = System.Configuration.ConfigurationManager.AppSettings["CampoAssunto"].ToString();
        private String lCaminhoConservador      = System.Configuration.ConfigurationManager.AppSettings["EmailConservador"].ToString();
        private String lCaminhoModerado         = System.Configuration.ConfigurationManager.AppSettings["EmailModerado"].ToString();
        private String lCaminhoArrojado         = System.Configuration.ConfigurationManager.AppSettings["EmailArrojado"].ToString();
        private String lCaminhoErro             = System.Configuration.ConfigurationManager.AppSettings["EmailErro"].ToString();
        private String lCampoProdutos           = System.Configuration.ConfigurationManager.AppSettings["CampoProdutos"].ToString();
        private String lCampoErros              = System.Configuration.ConfigurationManager.AppSettings["CampoErros"].ToString();
        private String lCodigoTeste             = System.Configuration.ConfigurationManager.AppSettings["CodigoTeste"].ToString();
        private String lCaminhoArquivoErro      = System.Configuration.ConfigurationManager.AppSettings["CaminhoArquivoErro"].ToString();
        #endregion

        #region Conexões
        private const string gNomeConexaoSuitability  = "SUITABILITY";
        private const string gNomeConexaoSQL          = "Risco";
        private const string gNomeConexaoContaMargem  = "CONTAMARGEM";
        private const string gNomeConexaoCadastro     = "Cadastro";
        private const string gNomeConexaoSQLConfig    = "Config";
        private const string gNomeConexaoItau         = "FundosItau";
        private const string gNomeConexaoInviXX       = "PlataformaInviXX";
        #endregion

        #region Listas de infos em memória
        private Dictionary<int,List<Gradual.Suitability.Service.Objetos.ProdutoExclusao>> ListaPerfilExclusao   = new Dictionary<int, List<Gradual.Suitability.Service.Objetos.ProdutoExclusao>>();
        private List<Gradual.Suitability.Service.Objetos.ClienteSuitability> gListaClientes                     = new List<Gradual.Suitability.Service.Objetos.ClienteSuitability>();
        private List<Gradual.Suitability.Service.Objetos.PosicaoCotistaItau> gListaCotistaItau                  = new List<Gradual.Suitability.Service.Objetos.PosicaoCotistaItau>();
        private List<Objetos.PosicaoCotistaFinancial> gListaCotistaFinancialCompleta                            = new List<Objetos.PosicaoCotistaFinancial>();
        private List<Objetos.ProdutoExclusao> gListaExclusao                                                    = new List<Objetos.ProdutoExclusao>();
        private Dictionary<string, string> gListaDePara                                                         = new Dictionary<string, string>();
        private List<Gradual.Suitability.Service.Objetos.Fundo> gListaFundos                                    = new List<Gradual.Suitability.Service.Objetos.Fundo>();
        private List<Objetos.Erro> gListaErro                                                                   = new List<Objetos.Erro>();
        private List<Objetos.Erro> gListaOk                                                                     = new List<Objetos.Erro>();
        private List<Objetos.PosicaoCotistaFinancial> gListaCotistaFinancial                                    = new List<Objetos.PosicaoCotistaFinancial>();
        private List<String> gListaExclusaoFinancial                                                            = new List<String>();
        #endregion
        
        #region Listas de Desenquadramento
        private List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> ListaClientesDesenquadradosBovespa = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
        private List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> ListaClientesDesenquadradosBMF = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
        private List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> ListaClientesDesenquadradosBTC = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
        private List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> ListaClientesDesenquadradosFundoFinancial = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
        private List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> ListaClientesDesenquadradosFundoItau = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
        private Dictionary<string,List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>> gListaDesenquadramento = new Dictionary<string, List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>>();
        #endregion

        #region Variáveis Globais
        private static SuitabilityMonitor _self = null;
        private static System.Threading.Mutex _mutex = new System.Threading.Mutex();
        bool _bKeepRunning = false;
        private System.Threading.Thread thrMonitor = null;
        private Gradual.OMS.Library.CronStyleScheduler _cron = null;
        #endregion

        #region Propriedades
        public List<Objetos.PosicaoCotistaFinancial> CotistasFinancial
        {
            get { return gListaCotistaFinancial; }
            set { gListaCotistaFinancial = value; }
        }
        #endregion


        StringBuilder lExclusao = new StringBuilder();
        StringBuilder lEmailsEnviados = new StringBuilder();

        public static SuitabilityMonitor GetInstance()
        {
            _mutex.WaitOne();

            if (_self == null)
            {
                _self = new SuitabilityMonitor();
            }

            _mutex.ReleaseMutex();

            return _self;
        }

        public void Inicializar()
        {
            gListaDatas.Add(new DateTime(2017, 05, 01));
            gListaDatas.Add(new DateTime(2017, 05, 08));
            gListaDatas.Add(new DateTime(2017, 05, 15));
            gListaDatas.Add(new DateTime(2017, 05, 22));
        }

        int gContadorData = 0;
        DateTime gDataInicial;

        public void PegarNovaData(int pContador)
        {
            gDataInicial = gListaDatas[pContador];
        }

        public void Clear()
        {
            _self = null;
            ListaPerfilExclusao = new Dictionary<int, List<Gradual.Suitability.Service.Objetos.ProdutoExclusao>>();
            gListaClientes = new List<Gradual.Suitability.Service.Objetos.ClienteSuitability>();
            gListaCotistaItau = new List<Gradual.Suitability.Service.Objetos.PosicaoCotistaItau>();
            gListaCotistaFinancialCompleta = new List<Objetos.PosicaoCotistaFinancial>();
            gListaExclusao = new List<Objetos.ProdutoExclusao>();
            gListaDePara = new Dictionary<string, string>();
            gListaFundos = new List<Gradual.Suitability.Service.Objetos.Fundo>();
            gListaErro = new List<Objetos.Erro>();
            gListaOk = new List<Objetos.Erro>();
            gListaCotistaFinancial = new List<Objetos.PosicaoCotistaFinancial>();
            gListaExclusaoFinancial = new List<String>();
            ListaClientesDesenquadradosBovespa = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            ListaClientesDesenquadradosBMF = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            ListaClientesDesenquadradosBTC = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            ListaClientesDesenquadradosFundoFinancial = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            ListaClientesDesenquadradosFundoItau = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            gListaDesenquadramento = new Dictionary<string, List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>>();
        }

    public void IniciarServico()
        {

            //throw new NotImplementedException();

            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Iniciando serviço"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            
            _bKeepRunning = true;

            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}",
                    Gradual.Utils.MethodHelper.GetCurrentMethod(),
                    "Criando thread do monitor"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            thrMonitor = new System.Threading.Thread(new System.Threading.ThreadStart(Monitor));
            thrMonitor.Start();

            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}",
                    Gradual.Utils.MethodHelper.GetCurrentMethod(),
                    "Iniciando Schenduler"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            _cron = new Gradual.OMS.Library.CronStyleScheduler();
            _cron.Start();

            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}",
                    Gradual.Utils.MethodHelper.GetCurrentMethod(),
                    "Serviço inicializado"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            _status = Gradual.OMS.Library.Servicos.ServicoStatus.EmExecucao;

        }

        public void PararServico()
        {
            //throw new NotImplementedException();
            _bKeepRunning = false;

            while (thrMonitor != null && thrMonitor.IsAlive)
            {
                System.Threading.Thread.Sleep(250);
            }

            _status = Gradual.OMS.Library.Servicos.ServicoStatus.Parado;

        }

        public OMS.Library.Servicos.ServicoStatus ReceberStatusServico()
        {
            //throw new NotImplementedException();
            return _status;
        }

        public void CronWatchDog()
        {
            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}",
                Gradual.Utils.MethodHelper.GetCurrentMethod(),
                "CronWatchDog called"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
        }

        private void Monitor()
        {
            while (_bKeepRunning)
            {
                try
                {
                    System.Threading.Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }
            }
        }

        public void VerificarSuitability()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterListaClientes();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterListaDePara();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterListaPerfilExclusao();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterExclusao();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterListaFundos();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterPosicaoFundosItau();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterPosicaoFundosFinancial();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().MontarForaPerfil();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarBovespa();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarBMF();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarBTC();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarFundosFinancial();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarFundosItau();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().GerarNotificacoes();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().GerarNotificacaoErros();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().LimparDadosTemporariosOracle();
                Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().LimparMemoria();
            }
            catch(Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        #region Métodos

        public void ObterListaPerfilExclusao()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Generico.Dados.AcessaDados lAcessaDados = new Gradual.Generico.Dados.AcessaDados();

                string procedure = "sp_cliente_perfil_tipoproduto_lst";

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, procedure))
                {
                    System.Data.DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        var lOcorrenciasPerfil = (from lRow in lDataTable.AsEnumerable() select new { idPerfil = lRow.Field<int>("idPerfil") }).Distinct();

                        foreach (var lRow in lOcorrenciasPerfil)
                        {
                            List<Gradual.Suitability.Service.Objetos.ProdutoExclusao> lListaExclusao = new List<Gradual.Suitability.Service.Objetos.ProdutoExclusao>();

                            var lOcorrencias = from prod in lDataTable.AsEnumerable() where prod.Field<int>("idPerfil") == lRow.idPerfil select prod;

                            foreach (var lProduto in lOcorrencias)
                            {
                                Gradual.Suitability.Service.Objetos.ProdutoExclusao lProdutoExclusao = new Gradual.Suitability.Service.Objetos.ProdutoExclusao();
                                lProdutoExclusao.Codigo = lProduto.Field<int>("idPerfil");
                                lProdutoExclusao.CodigoProduto = lProduto.Field<int>("idTipoProduto");
                                lProdutoExclusao.Descricao = lProduto.Field<string>("Descricao");
                                lProdutoExclusao.Sigla = lProduto.Field<string>("Sigla");
                                lProdutoExclusao.Cnpj = lProduto.Field<string>("CNPJ");

                                lListaExclusao.Add(lProdutoExclusao);
                                gListaExclusao.Add(lProdutoExclusao);
                            }

                            if (lListaExclusao.Count > 0)
                            {
                                ListaPerfilExclusao.Add(lRow.idPerfil, lListaExclusao);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

        }

        public void ObterListaDePara()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                using (var lAcessaDados = new Gradual.Generico.Dados.AcessaDados() { ConnectionStringName = gNomeConexaoCadastro })
                {
                    using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "sp_depara_suitability"))
                    {
                        System.Data.DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                String lSigla = (lDataTable.Rows[i]["Sigla"]).DBToString();
                                String lNomeAmigavel = (lDataTable.Rows[i]["NomeAmigavel"]).DBToString();

                                if (!gListaDePara.ContainsKey(lSigla))
                                {
                                    gListaDePara.Add(lSigla, lNomeAmigavel);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void ObterListaClientes()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Generico.Dados.AcessaDados lAcessaDados = new Gradual.Generico.Dados.AcessaDados();

                string procedure = "rel_cliente_suitability_efetuados_lst_sp";

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, procedure))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, new DateTime(2000, 1, 1));
                    lAcessaDados.AddInParameter(lDbCommand, "@DtAte", DbType.DateTime, DateTime.Now);
                    lAcessaDados.AddInParameter(lDbCommand, "@StRealizado", DbType.Int32, 1);
                    System.Data.DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                    
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            String lIdCliente       = (lDataTable.Rows[i]["id_cliente"]).DBToString();
                            String lds_nome         = (lDataTable.Rows[i]["ds_nome"]).DBToString();
                            String lds_cpfcnpj      = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();
                            String lcd_codigo       = (lDataTable.Rows[i]["cd_codigo"]).DBToString();
                            String lcd_assessor     = (lDataTable.Rows[i]["cd_assessor"]).DBToString();
                            String lEmail           = (lDataTable.Rows[i]["ds_email"]).DBToString();
                            String lFonte           = (lDataTable.Rows[i]["ds_fonte"]).DBToString();
                            String lStatus          = (lDataTable.Rows[i]["ds_status"]).DBToString();
                            String lidPerfil        = (lDataTable.Rows[i]["idPerfil"]).DBToString();
                            String lidQuestionario  = (lDataTable.Rows[i]["idQuestionario"]).DBToString();
                            String lDescricao       = (lDataTable.Rows[i]["ds_perfil"]).DBToString();
                            String lPerfilAnterior  = (lDataTable.Rows[i]["PerfilAnterior"]).DBToString();
                            String lData            = (lDataTable.Rows[i]["dt_realizacao"]).DBToString();
                            String lPeso            = (lDataTable.Rows[i]["Peso"]).DBToString();
                            String lLogin           = (lDataTable.Rows[i]["ds_loginrealizado"]).DBToString();
                            String lPreenchimento   = (lDataTable.Rows[i]["st_preenchidopelocliente"]).DBToString();

                            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Debug,
                                String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15}"
                                , lIdCliente.PadRight(8, ' ')
                                , lds_nome.PadRight(60, ' ')
                                , lds_cpfcnpj.PadRight(20, ' ')
                                , lcd_codigo.PadRight(8, ' ')
                                , lcd_assessor.PadRight(8, ' ')
                                , lEmail.PadRight(30, ' ')
                                , lFonte.PadRight(100, ' ')
                                , lStatus.PadRight(30, ' ')
                                , lidPerfil.PadRight(15, ' ')
                                , lidQuestionario.PadRight(5, ' ')
                                , lDescricao.PadRight(15, ' ')
                                , lPerfilAnterior.PadRight(15, ' ')
                                , lData.PadRight(18, ' ')
                                , lPeso.PadRight(8, ' ')
                                , lLogin.PadRight(60, ' ')
                                , lPreenchimento.PadRight(3, ' ')
                                )
                                , new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                            Gradual.Suitability.Service.Objetos.ClienteSuitability lCliente = new Gradual.Suitability.Service.Objetos.ClienteSuitability();

                            lCliente.Codigo = lIdCliente;
                            lCliente.Nome = lds_nome;
                            lCliente.CpfCnpj = lds_cpfcnpj;
                            lCliente.Conta = lcd_codigo;
                            lCliente.CodigoAssessor = lcd_assessor;
                            lCliente.Email = lEmail;
                            lCliente.Fonte = lFonte;
                            lCliente.Status = lStatus;
                            lCliente.idPerfil = lidPerfil;
                            lCliente.idQuestionario = lidQuestionario;
                            lCliente.Descricao = lDescricao;
                            lCliente.PerfilAnterior = lPerfilAnterior;
                            lCliente.Data = lData;
                            lCliente.Peso = lPeso;
                            lCliente.Login = lLogin;
                            lCliente.Preenchimento = lPreenchimento;

                            if (String.IsNullOrEmpty(lCodigoTeste))
                            {
                                gListaClientes.Add(lCliente);
                            }
                            else
                            {
                                if(lCliente.Conta.ToString().Equals(lCodigoTeste))
                                {
                                    gListaClientes.Add(lCliente);
                                }
                            }
                            
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void ObterListaFundos()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                using (var lAcessaDados = new Gradual.Generico.Dados.AcessaDados() { ConnectionStringName = gNomeConexaoInviXX })
                {
                    using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_FUNDOS_LST"))
                    {
                        System.Data.DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                Objetos.Fundo lFundo = new Objetos.Fundo();
                            
                                lFundo.CodigoANBIMA     = (lDataTable.Rows[i]["CodigoANBIMA"]).DBToInt32();
                                lFundo.CodigoItau       = (lDataTable.Rows[i]["CodigoFundoItau"]).DBToInt32();
                                lFundo.CodigoFinancial  = (lDataTable.Rows[i]["CodigoCarteirafinancial"]).DBToInt32();
                                lFundo.Nome             = (lDataTable.Rows[i]["dsProduto"]).DBToString();
                                lFundo.Cnpj             = (lDataTable.Rows[i]["CPFCNPJ"]).DBToString();
                                lFundo.RiscoInvix       = (lDataTable.Rows[i]["dsRisco"]).DBToString();
                                lFundo.Perfil           = (lDataTable.Rows[i]["Perfil"]).DBToString();

                                gListaFundos.Add(lFundo);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public String VerificarSigla(String pDescricao, String pCnpj)
        {
            String lRetorno =  String.Empty;

            if (!String.IsNullOrEmpty(pDescricao))
            {
                var lOcorrencias = from exclusao in gListaExclusao where exclusao.Descricao.Equals(pDescricao) select exclusao;

                if (lOcorrencias.Count() > 0)
                {
                    return lOcorrencias.ToList()[0].Sigla;
                }
            }

            if (!String.IsNullOrEmpty(pCnpj))
            {
                var lOcorrencias = from exclusao in gListaExclusao where exclusao.Cnpj.Equals(pCnpj) select exclusao;
                
                if (lOcorrencias.Count() > 0)
                {
                    return lOcorrencias.ToList()[0].Sigla;
                }
            }

            return lRetorno;
        }

        public void ObterExclusao()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                using (var lAcessaDados = new Gradual.Generico.Dados.AcessaDados() { ConnectionStringName = gNomeConexaoCadastro })
                {
                    using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "sp_suitability_exclusao"))
                    {
                        System.Data.DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                gListaExclusaoFinancial.Add((lDataTable.Rows[i]["Valor"]).DBToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

        }


        public void ObterPosicaoFundosItau()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                using (var lAcessaDados = new Gradual.Generico.Dados.AcessaDados() { ConnectionStringName = gNomeConexaoItau })
                {
                    using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_SUITA_POSICAO"))
                    {
                        // Removido pois se o cliente esta posicionado no fundo, estara sempre desenquadrado (Bianca: 22/06/2015)
                        //DateTime lDataInicio = RetornarSegundaFeiraAnterior();
                        //DateTime lDataFim = RetornarSegundaFeiraAnterior().AddDays(4);
                        //lAcessaDados.AddInParameter(lDbCommand, "@DtDe", System.Data.DbType.DateTime, lDataInicio);
                        //lAcessaDados.AddInParameter(lDbCommand, "@DtAte", System.Data.DbType.DateTime, lDataFim);

                        System.Data.DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                var Ocorrencias = from cliente in gListaClientes where cliente.CpfCnpj.PadLeft(15, '0') == (lDataTable.Rows[i]["dsCpfCnpj"]).DBToString().Trim() select cliente;

                                if (Ocorrencias.Count() > 0)
                                {
                                    Gradual.Suitability.Service.Objetos.PosicaoCotistaItau lCliente = new Gradual.Suitability.Service.Objetos.PosicaoCotistaItau();

                                    lCliente.Codigo = Ocorrencias.ToList()[0].Conta;
                                    lCliente.idMovimento = (lDataTable.Rows[i]["idMovimento"]).DBToString();
                                    lCliente.idProcessamento = (lDataTable.Rows[i]["idProcessamento"]).DBToString();
                                    lCliente.dsCpfCnpj = (lDataTable.Rows[i]["dsCpfCnpj"]).DBToString();
                                    lCliente.banco = (lDataTable.Rows[i]["banco"]).DBToString();
                                    lCliente.agencia = (lDataTable.Rows[i]["agencia"]).DBToString();
                                    lCliente.conta = (lDataTable.Rows[i]["conta"]).DBToString();
                                    lCliente.digitoConta = (lDataTable.Rows[i]["digitoConta"]).DBToString();
                                    lCliente.subconta = (lDataTable.Rows[i]["subconta"]).DBToString();
                                    lCliente.idCotista = (lDataTable.Rows[i]["idCotista"]).DBToString();
                                    lCliente.idfundo = (lDataTable.Rows[i]["idfundo"]).DBToString();
                                    lCliente.dsRazaoSocial = (lDataTable.Rows[i]["dsRazaoSocial"]).DBToString();
                                    lCliente.quantidadeCotas = (lDataTable.Rows[i]["quantidadeCotas"]).DBToString();
                                    lCliente.valorCota = (lDataTable.Rows[i]["valorCota"]).DBToString();
                                    lCliente.valorBruto = (lDataTable.Rows[i]["valorBruto"]).DBToString();
                                    lCliente.valorIR = (lDataTable.Rows[i]["valorIR"]).DBToString();
                                    lCliente.valorIOF = (lDataTable.Rows[i]["valorIOF"]).DBToString();
                                    lCliente.valorLiquido = (lDataTable.Rows[i]["valorLiquido"]).DBToString();
                                    lCliente.dtReferencia = (lDataTable.Rows[i]["dtReferencia"]).DBToString();
                                    lCliente.dtProcessamento = (lDataTable.Rows[i]["dtProcessamento"]).DBToString();
                                    lCliente.dsCnpj = (lDataTable.Rows[i]["dsCnpj"]).DBToString();
                                    lCliente.dsCodFundo = (lDataTable.Rows[i]["dsCodFundo"]).DBToString();
                                    lCliente.dsNomeFantasia = (lDataTable.Rows[i]["dsNomeFantasia"]).DBToString();

                                    if (String.IsNullOrEmpty(lCodigoTeste))
                                    {
                                        gListaCotistaItau.Add(lCliente);
                                    }
                                    else
                                    {
                                        if (lCliente.Codigo.ToString().Equals(lCodigoTeste))
                                        {
                                            gListaCotistaItau.Add(lCliente);
                                        }
                                    }
                                }
                                else
                                {
                                    gListaErro.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = " - ", Mensagem = String.Format("Não foi possível carregar a posição do fundo {0} - {1} para o CPF/CNPJ: {2} pois o cliente não foi encontrado", (lDataTable.Rows[i]["dsCnpj"]).DBToString(), (lDataTable.Rows[i]["dsRazaoSocial"]).DBToString(), (lDataTable.Rows[i]["dsCpfCnpj"]).DBToString()) });
                                    
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

        }

        public void ObterPosicaoFundosFinancial()
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;

                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                intra.gradual.financial.ValidateLogin validateLogin = new intra.gradual.financial.ValidateLogin();

                validateLogin.Username = "brocha";
                validateLogin.Password = "gradual12345*";

                intra.gradual.financial.PosicaoCotistaWS ws = new intra.gradual.financial.PosicaoCotistaWS();

                ws.ValidateLoginValue = validateLogin;

                intra.gradual.financial.PosicaoCotistaViewModel []  vm = ws.Exporta(null, null, null);

                gListaCotistaFinancial = ParseFinancial<intra.gradual.financial.PosicaoCotistaViewModel>(vm);
            }
            catch(Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private List<Objetos.PosicaoCotistaFinancial> ParseFinancial<T>(T[] arr)
        {
            List<Objetos.PosicaoCotistaFinancial> lRetorno = new List<Objetos.PosicaoCotistaFinancial>();

            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                DateTime lDataInicio = RetornarSegundaFeiraAnterior();
                DateTime lDataFim = RetornarSegundaFeiraAnterior().AddDays(4);

                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, String.Format("{0} - Período da verificação - De: {1} à {2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lDataInicio.ToString(), lDataFim.ToString()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            
                foreach (T item in arr)
                {
                    Objetos.PosicaoCotistaFinancial lCotista = new Objetos.PosicaoCotistaFinancial();

                    foreach (System.Reflection.PropertyInfo property in item.GetType().GetProperties())
                    {
                        lCotista.GetType().GetProperty(property.Name).SetValue(lCotista, property.GetValue(item, null).DBToString(), null);
                    }
                
                    // Removido pois se o cliente esta posicionado no fundo, estara sempre desenquadrado (Bianca: 22/06/2015)
                    //DateTime lDataInicio = RetornarSegundaFeiraAnterior();
                    //DateTime lDataFim = RetornarSegundaFeiraAnterior().AddDays(4);
                    //if (lCotista.DataAplicacao.DBToDateTime() > lDataInicio && lCotista.DataAplicacao.DBToDateTime() < lDataFim)
                    //{
                        if (String.IsNullOrEmpty(lCodigoTeste))
                        {
                            lRetorno.Add(lCotista);
                        }
                        else
                        {
                            if (lCotista.IdCotista.ToString().Equals(lCodigoTeste))
                            {
                                lRetorno.Add(lCotista);
                            }
                        }
                    //}
                }
            }
            catch(Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return lRetorno;
        }


        public void MontarForaPerfil()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Debug, String.Format("Inicio gravação fora perfil"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                foreach (Gradual.Suitability.Service.Objetos.ClienteSuitability lCliente in gListaClientes)
                {
                    if (!String.IsNullOrEmpty(lCliente.Conta))
                    {
                        if (ListaPerfilExclusao.ContainsKey(Int32.Parse(lCliente.idPerfil)))
                        {
                            List<Gradual.Suitability.Service.Objetos.ProdutoExclusao> lLista = ListaPerfilExclusao[Int32.Parse(lCliente.idPerfil)];

                            foreach (Gradual.Suitability.Service.Objetos.ProdutoExclusao lProduto in lLista)
                            {
                                try
                                {
                                    using (var lAcessaDados = new Gradual.Generico.Dados.AcessaDados() { ConnectionStringName = gNomeConexaoSuitability })
                                    {
                                        using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_SUITA_PERFIL_INS"))
                                        {
                                            lAcessaDados.AddInParameter(lDbCommand, "pCodigo", System.Data.DbType.Int32, lCliente.Conta);
                                            lAcessaDados.AddInParameter(lDbCommand, "pSigla", System.Data.DbType.AnsiString, lProduto.Sigla);

                                            lAcessaDados.ExecuteNonQuery(lDbCommand);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw (ex);
                                }
                            }

                            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Debug, String.Format("Fim gravação fora perfil"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void VerificarBovespa()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Generico.Dados.AcessaDados lAcessaDados = new Gradual.Generico.Dados.AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoSuitability;

                //using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_SUITA_DESENQUADRA_BOV"))
                using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_SUITA_DESEN_BOV_AJUSTE"))
                {
                    //DateTime lDataInicio = RetornarSegundaFeiraAnterior();
                    //DateTime lDataFim = RetornarSegundaFeiraAnterior().AddDays(4);

                    DateTime lDataInicio = gDataInicial;
                    DateTime lDataFim = gDataInicial.AddDays(4);

                    Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, String.Format("{0} - Período da verificação - De: {1} à {2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lDataInicio.ToString(), lDataFim.ToString()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                    lAcessaDados.AddInParameter(lDbCommand, "pDataInicio", System.Data.DbType.DateTime, lDataInicio);
                    lAcessaDados.AddInParameter(lDbCommand, "pDataFim", System.Data.DbType.DateTime, lDataFim);

                    System.Data.DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            Gradual.Suitability.Service.Objetos.ClienteDesenquadrado lCliente = new Gradual.Suitability.Service.Objetos.ClienteDesenquadrado();

                            lCliente.Codigo         = (lDataTable.Rows[i]["Codigo"]     ).DBToString();
                            lCliente.CpfCnpj        = (lDataTable.Rows[i]["CpfCnpj"]    ).DBToString();
                            lCliente.Instrumento    = (lDataTable.Rows[i]["Instrumento"]).DBToString();
                            lCliente.Quantidade     = (lDataTable.Rows[i]["Quantidade"] ).DBToString();
                            lCliente.Sentido        = (lDataTable.Rows[i]["NaturezaOperacao"]    ).DBToString();
                            lCliente.TipoMercado    = (lDataTable.Rows[i]["TipoMercado"]).DBToString();
                            lCliente.Data           = (lDataTable.Rows[i]["DataNegocio"]).DBToDateTime().ToString("dd/MM/yyyy");
                            lCliente.Hora           = (lDataTable.Rows[i]["HoraNegocio"]).DBToString();

                            ListaClientesDesenquadradosBovespa.Add(lCliente);
                            lExclusao.AppendLine(String.Format("{0};{1};{2};{3};{4};{5};", lCliente.Codigo, lCliente.CpfCnpj, lCliente.Data, lCliente.TipoMercado, String.Empty, String.Empty));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void VerificarBMF()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Generico.Dados.AcessaDados lAcessaDados = new Gradual.Generico.Dados.AcessaDados();
                
                lAcessaDados.ConnectionStringName = gNomeConexaoSuitability;

                //using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_SUITA_DESENQUADRA_BMF"))
                using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_SUITA_DESEN_BMF_AJUSTE"))
                {
                    //DateTime lDataInicio = RetornarSegundaFeiraAnterior();
                    //DateTime lDataFim = RetornarSegundaFeiraAnterior().AddDays(4);
                    DateTime lDataInicio = gDataInicial;
                    DateTime lDataFim = gDataInicial.AddDays(4);

                    Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, String.Format("{0} - Período da verificação - De: {1} à {2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lDataInicio.ToString(), lDataFim.ToString()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    
                    lAcessaDados.AddInParameter(lDbCommand, "pDataInicio", System.Data.DbType.DateTime, lDataInicio);
                    lAcessaDados.AddInParameter(lDbCommand, "pDataFim", System.Data.DbType.DateTime, lDataFim);

                    System.Data.DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {

                            Gradual.Suitability.Service.Objetos.ClienteDesenquadrado lCliente = new Gradual.Suitability.Service.Objetos.ClienteDesenquadrado();
                            lCliente.Codigo = (lDataTable.Rows[i]["Codigo"]).DBToString();
                            lCliente.CpfCnpj = (lDataTable.Rows[i]["CpfCnpj"]).DBToString();
                            lCliente.TipoMercado = (lDataTable.Rows[i]["TipoMercado"]).DBToString();
                            lCliente.Data = (lDataTable.Rows[i]["Data"]).DBToString();

                            ListaClientesDesenquadradosBMF.Add(lCliente);
                            lExclusao.AppendLine(String.Format("{0};{1};{2};{3};{4};{5}", lCliente.Codigo, lCliente.CpfCnpj, lCliente.Data, lCliente.TipoMercado, String.Empty, String.Empty));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void VerificarBTC()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Generico.Dados.AcessaDados lAcessaDados = new Gradual.Generico.Dados.AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoSuitability;

                //using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_SUITA_DESENQUADRA_BTC"))
                using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_SUITA_DESEN_BTC_AJUSTE"))
                {
                    //DateTime lDataInicio = RetornarSegundaFeiraAnterior();
                    //DateTime lDataFim = RetornarSegundaFeiraAnterior().AddDays(4);
                    DateTime lDataInicio = gDataInicial;
                    DateTime lDataFim = gDataInicial.AddDays(4);

                    Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, String.Format("{0} - Período da verificação - De: {1} à {2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lDataInicio.ToString(), lDataFim.ToString()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                    lAcessaDados.AddInParameter(lDbCommand, "pDataInicio", System.Data.DbType.DateTime, lDataInicio);
                    lAcessaDados.AddInParameter(lDbCommand, "pDataFim", System.Data.DbType.DateTime, lDataFim);

                    System.Data.DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {

                            Gradual.Suitability.Service.Objetos.ClienteDesenquadrado lCliente = new Gradual.Suitability.Service.Objetos.ClienteDesenquadrado();
                            lCliente.Codigo = (lDataTable.Rows[i]["Codigo"]).DBToString();
                            lCliente.CpfCnpj = (lDataTable.Rows[i]["CpfCnpj"]).DBToString();
                            lCliente.TipoMercado = (lDataTable.Rows[i]["TipoMercado"]).DBToString();
                            lCliente.Data = (lDataTable.Rows[i]["Data"]).DBToString();

                            ListaClientesDesenquadradosBTC.Add(lCliente);
                            lExclusao.AppendLine(String.Format("{0};{1};{2};{3};{4};{5}", lCliente.Codigo, lCliente.CpfCnpj, lCliente.Data, lCliente.TipoMercado, String.Empty, String.Empty));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void VerificarFundosFinancial()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                // Verificação de desenquadramento por aplicação no FINANCIAL
                foreach (Objetos.PosicaoCotistaFinancial lClientePosicionado in gListaCotistaFinancial)
                {
                    if (!gListaExclusaoFinancial.Contains(lClientePosicionado.IdCarteira))
                    {

                        var lOcorrenciasClientes = from cliente in gListaClientes where cliente.Conta.Equals(lClientePosicionado.IdCotista) select cliente;

                        if (lOcorrenciasClientes.Count() > 0)
                        {
                            Objetos.ClienteSuitability lCliente = lOcorrenciasClientes.ToList()[0];

                            if (!String.IsNullOrEmpty(lClientePosicionado.IdCarteira))
                            {
                                var lOcorrenciasFundos = from fundo in gListaFundos where fundo.CodigoANBIMA.Equals(Int32.Parse(lClientePosicionado.IdCarteira)) select fundo;

                                if (lOcorrenciasFundos.Count() > 0)
                                {
                                    Objetos.Fundo lFundo = lOcorrenciasFundos.ToList()[0];

                                    if (String.IsNullOrEmpty(lFundo.Perfil))
                                    {
                                        gListaErro.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = lCliente.Conta, Mensagem = String.Format("Não foi possível verificar o risco para a aplicação no fundo {0} - {1}", lFundo.Cnpj, lFundo.Nome) });
                                    }
                                    else
                                    {
                                        if (
                                            (lCliente.Descricao.Equals("Conservador") && (lFundo.Perfil.Equals("Arrojado") || lFundo.Perfil.Equals("Moderado")))
                                            || (lCliente.Descricao.Equals("Moderado") && lFundo.Perfil.Equals("Arrojado"))
                                            )
                                        {
                                            Objetos.ClienteDesenquadrado lClienteDesenquadrado  = new Objetos.ClienteDesenquadrado();
                                            lClienteDesenquadrado.Codigo = lCliente.Conta;
                                            lClienteDesenquadrado.CpfCnpj = lCliente.CpfCnpj;
                                            lClienteDesenquadrado.TipoMercado = "";
                                            lClienteDesenquadrado.NomeFundo = lFundo.Nome;
                                            lClienteDesenquadrado.PerfilFundo = lFundo.Perfil;
                                            var lOcorrencias = from cliente in ListaClientesDesenquadradosFundoFinancial where cliente.NomeFundo.Equals(lFundo.Nome) && cliente.PerfilFundo.Equals(lFundo.Perfil) select cliente;

                                            if (lOcorrencias.Count() <= 0)
                                            {
                                                ListaClientesDesenquadradosFundoFinancial.Add(lClienteDesenquadrado);
                                                lExclusao.AppendLine(String.Format("{0};{1};{2};{3};{4};{5};", lCliente.Codigo, lCliente.CpfCnpj, lClienteDesenquadrado.Data, String.Empty, lClienteDesenquadrado.NomeFundo, lClienteDesenquadrado.PerfilFundo));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    gListaErro.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = lCliente.Conta, Mensagem = String.Format("Não foi possível verificar o risco para a aplicação no fundo código {0} pois não foi encontrado um fundo com código equivalente", lClientePosicionado.CodigoAnbima) });
                                }
                            }
                            else
                            {
                                gListaErro.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = lCliente.Conta, Mensagem = String.Format("Não foi possível verificar o risco para a aplicação no fundo para a operação {0} pois o código ANBIMA estava vazio", lClientePosicionado.IdOperacao) });
                            }
                        }
                        else
                        {
                            gListaErro.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = lClientePosicionado.IdCotista, Mensagem = String.Format("Não foi possível carregar a posição do fundo CodANBIMA: {0} para o cliente {1} pois o cliente não foi encontrado", lClientePosicionado.CodigoAnbima, lClientePosicionado.IdCotista) });
                        }
                    }
                    else
                    {
                        gListaErro.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = lClientePosicionado.IdCotista, Mensagem = String.Format("Posição ignorada Carteira: {0} CodANBIMA: {1} para o cliente {2} ", lClientePosicionado.IdCarteira, lClientePosicionado.CodigoAnbima, lClientePosicionado.IdCotista) });
                    }
                }
            }
            catch(Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void VerificarFundosItau()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                foreach (Objetos.PosicaoCotistaItau lClientePosicionado in gListaCotistaItau)
                {
                    var lOcorrenciasClientes = from cliente in gListaClientes where cliente.Conta.Equals(lClientePosicionado.Codigo) select cliente;

                    if (lOcorrenciasClientes.Count() > 0)
                    {
                        Objetos.ClienteSuitability lCliente = lOcorrenciasClientes.ToList()[0];

                        var lOcorrenciasFundos = from fundo in gListaFundos where fundo.CodigoItau.Equals(Int32.Parse(lClientePosicionado.dsCodFundo)) select fundo;

                        if (lOcorrenciasFundos.Count() > 0)
                        {
                            Objetos.Fundo lFundo = lOcorrenciasFundos.ToList()[0];

                            if (String.IsNullOrEmpty(lFundo.Perfil))
                            {
                                gListaErro.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = lCliente.Conta, Mensagem = String.Format("Não foi possível verificar o risco para a aplicação no fundo {0} - {1} pois o fundo não possui perfil cadastrado", lFundo.Cnpj, lFundo.Nome) });
                            }
                            else
                            {
                                if (
                                    (lCliente.Descricao.Equals("Conservador") && (lFundo.Perfil.Equals("Arrojado") || lFundo.Perfil.Equals("Moderado")))
                                    || (lCliente.Descricao.Equals("Moderado") && lFundo.Perfil.Equals("Arrojado"))
                                    )
                                {
                                    Objetos.ClienteDesenquadrado lClienteDesenquadrado  = new Objetos.ClienteDesenquadrado();
                                    lClienteDesenquadrado.Codigo = lCliente.Conta;
                                    lClienteDesenquadrado.CpfCnpj = lCliente.CpfCnpj;
                                    lClienteDesenquadrado.TipoMercado = "";
                                    lClienteDesenquadrado.NomeFundo = lFundo.Nome;
                                    lClienteDesenquadrado.PerfilFundo = lFundo.Perfil;
                                    var lOcorrencias = from cliente in ListaClientesDesenquadradosFundoItau where cliente.NomeFundo.Equals(lFundo.Nome) && cliente.PerfilFundo.Equals(lFundo.Perfil) select cliente;

                                    if (lOcorrencias.Count() <= 0)
                                    {
                                        ListaClientesDesenquadradosFundoItau.Add(lClienteDesenquadrado);
                                        lExclusao.AppendLine(String.Format("{0};{1};{2};{3};{4};{5}", lCliente.Codigo.ToString(), lCliente.CpfCnpj.ToString(), "", lClienteDesenquadrado.NomeFundo.ToString(), lClienteDesenquadrado.PerfilFundo.ToString(), ""));
                                    }
                                }
                                else
                                {
                                    gListaOk.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = lCliente.Conta, Mensagem = String.Format("O cliente com conta {0} - ({1}) efetuou uma aplicacao no fundo {2} de perfil {3} correspondente com o seu perfil {4} ", lCliente.Conta, lCliente.CpfCnpj, lFundo.Nome, lFundo.Perfil, lCliente.Descricao) });
                                }
                            }
                        }
                        else
                        {
                            gListaErro.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = lCliente.Conta, Mensagem = String.Format("Não foi possível verificar o risco para a aplicação no fundo código {0} pois não foi encontrado um fundo com código equivalente", lClientePosicionado.dsCodFundo) });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void GerarNotificacoes()
        {
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                foreach (Gradual.Suitability.Service.Objetos.ClienteDesenquadrado lCliente in ListaClientesDesenquadradosBovespa)
                {
                    if (!gListaDesenquadramento.ContainsKey(lCliente.Codigo))
                    {
                        List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> lLista = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
                        lLista.Add(lCliente);

                        gListaDesenquadramento.Add(lCliente.Codigo, lLista);
                    }
                    else
                    {
                        List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> lLista = gListaDesenquadramento[lCliente.Codigo];
                        lLista.Add(lCliente);
                        gListaDesenquadramento[lCliente.Codigo] = lLista;
                    }
                }

                foreach (Gradual.Suitability.Service.Objetos.ClienteDesenquadrado lCliente in ListaClientesDesenquadradosBMF)
                {
                    if (!gListaDesenquadramento.ContainsKey(lCliente.Codigo))
                    {
                        List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> lLista = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
                        lLista.Add(lCliente);

                        gListaDesenquadramento.Add(lCliente.Codigo, lLista);
                    }
                    else
                    {
                        List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> lLista = gListaDesenquadramento[lCliente.Codigo];
                        lLista.Add(lCliente);
                        gListaDesenquadramento[lCliente.Codigo] = lLista;
                    }
                }

                foreach (Gradual.Suitability.Service.Objetos.ClienteDesenquadrado lCliente in ListaClientesDesenquadradosBTC)
                {
                    if (!gListaDesenquadramento.ContainsKey(lCliente.Codigo))
                    {
                        List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> lLista = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
                        lLista.Add(lCliente);

                        gListaDesenquadramento.Add(lCliente.Codigo, lLista);
                    }
                    else
                    {
                        List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> lLista = gListaDesenquadramento[lCliente.Codigo];
                        lLista.Add(lCliente);
                        gListaDesenquadramento[lCliente.Codigo] = lLista;
                    }
                }

                foreach (Gradual.Suitability.Service.Objetos.ClienteDesenquadrado lCliente in ListaClientesDesenquadradosFundoFinancial)
                {
                    if (!gListaDesenquadramento.ContainsKey(lCliente.Codigo))
                    {
                        List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> lLista = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
                        lLista.Add(lCliente);

                        gListaDesenquadramento.Add(lCliente.Codigo, lLista);
                    }
                    else
                    {
                        List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado> lLista = gListaDesenquadramento[lCliente.Codigo];
                        lLista.Add(lCliente);
                        gListaDesenquadramento[lCliente.Codigo] = lLista;
                    }
                }

                foreach (KeyValuePair<String, List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>> lKey in gListaDesenquadramento)
                {
                    List<String> lProdutos = new List<string>();
                    Gradual.Suitability.Service.Objetos.ClienteSuitability lCliente = null;

                    var Ocorrencias = from cliente in  gListaClientes where cliente.Conta == lKey.Key select cliente;

                    if (Ocorrencias.Count() > 0)
                    {
                         lCliente = Ocorrencias.ToList()[0];
                    }

                    foreach (Gradual.Suitability.Service.Objetos.ClienteDesenquadrado lClienteDesenquadrado in lKey.Value)
                    {
                        if (String.IsNullOrEmpty(lClienteDesenquadrado.TipoMercado))
                        {
                            if (!String.IsNullOrEmpty(lClienteDesenquadrado.NomeFundo))
                            {
                                if (!lProdutos.Contains(String.Format("{0} ({1})", lClienteDesenquadrado.NomeFundo, lClienteDesenquadrado.PerfilFundo)))
                                {
                                    lProdutos.Add(String.Format("{0} ({1})", lClienteDesenquadrado.NomeFundo, lClienteDesenquadrado.PerfilFundo));
                                }
                            }

                        }
                        else
                        {
                            String lNomeAmigavel = String.Empty;

                            if (gListaDePara.ContainsKey(lClienteDesenquadrado.TipoMercado))
                            {
                                lNomeAmigavel = gListaDePara[lClienteDesenquadrado.TipoMercado];
                                if (!lProdutos.Contains(lNomeAmigavel))
                                {
                                    lProdutos.Add(lNomeAmigavel);
                                }
                            }
                            else
                            {
                                if (lProdutos.Contains(lClienteDesenquadrado.TipoMercado.ToString()))
                                {
                                    lProdutos.Add(lClienteDesenquadrado.TipoMercado.ToString());
                                }
                            }


                            //lProdutos.Add(String.Format("<tr  width='588'><td width='100' style='font-family: Arial, sans-serif; font-size: 14px; color: #808285; border-right: 1px solid #808285; padding: 6px;'>{0}</td><td width='100' style='font-family: Arial, sans-serif; font-size: 14px; color: #808285; border-right: 1px solid #808285; padding: 6px;'>{1}</td><td width='100' style='font-family: Arial, sans-serif; font-size: 14px; color: #808285; border-right: 1px solid #808285; padding: 6px;'>{2}</td><td width='175' style='font-family: Arial, sans-serif; font-size: 14px; color: #808285; border-right: 1px solid #808285; padding: 6px;'>{3} {4}</td></tr>", lClienteDesenquadrado.Sentido.Equals("C") ? "Compra" : "Venda ", lClienteDesenquadrado.Quantidade, lClienteDesenquadrado.Instrumento, lClienteDesenquadrado.Data, lClienteDesenquadrado.Hora));
                        }
                    }

                    if (String.IsNullOrEmpty(lCodigoTeste))
                    {
                        EnviarEmail(lCliente, lProdutos);
                    }
                    else
                    {
                        if (lCliente.Conta.ToString().Equals(lCodigoTeste))
                        {
                            EnviarEmail(lCliente, lProdutos);
                        }
                    }
                }

                GerarArquivoDesenquadramento();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void GerarArquivoDesenquadramento()
        {
            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            System.IO.StreamReader str = System.IO.File.OpenText(lCaminhoErro);
            String lCorpoEmail = str.ReadToEnd();
            str.Close();

            String lCaminhoArquivoErro = GerarArquivo("DesenquadramentoSuitability", lExclusao);

            var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
            Gradual.OMS.Email.Lib.EmailAnexoInfo lEmailInfo = new Gradual.OMS.Email.Lib.EmailAnexoInfo();
            lEmailInfo.Arquivo = System.IO.File.ReadAllBytes(lCaminhoArquivoErro);
            lEmailInfo.Nome = String.Format("{0}_{1}.desenquadramento","DesenquadramentoSuitability", DateTime.Now.ToString("yyyyMMdd_Hmmss"));
            lAnexos.Add(lEmailInfo);

            Gradual.OMS.Email.Lib.IServicoEmail lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.Email.Lib.IServicoEmail>();
            var lRequest = new Gradual.OMS.Email.Lib.EnviarEmailRequest();
            lRequest.Objeto = new Gradual.OMS.Email.Lib.EmailInfo();
            lRequest.Objeto.Assunto = String.Format("Suitability - Desenquadramentos de processamento {0}", DateTime.Now.ToString());
            lRequest.Objeto.Destinatarios = new List<string>() { lEmailErroPara };
            lRequest.Objeto.Remetente = lEmailDe;
            lRequest.Objeto.CorpoMensagem = lCorpoEmail.ToString();
            lRequest.Objeto.Anexos = lAnexos;
            lRequest.Objeto.MensagemHTML = true;

            var lResponse = lServico.Enviar(lRequest);

        }

        public void GerarNotificacaoErros()
        {
            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            if (gListaErro.Count > 0)
            {
                System.IO.StreamReader str = System.IO.File.OpenText(lCaminhoErro);
                String lCorpoEmail = str.ReadToEnd();
                str.Close();

                StringBuilder lErros = new StringBuilder();

                lErros.AppendLine(String.Format("{0};{1};{2}", "Tipo", "Cliente", "Mensagem"));
                foreach (Objetos.Erro lErro in gListaErro)
                {
                    lErros.AppendLine(String.Format("{0};{1};{2}", lErro.Tipo, lErro.CodigoCliente, lErro.Mensagem));
                }

                String lCaminhoArquivoErro = GerarArquivo("ErrosSuitability", lErros);

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                Gradual.OMS.Email.Lib.EmailAnexoInfo lEmailInfo = new Gradual.OMS.Email.Lib.EmailAnexoInfo();
                lEmailInfo.Arquivo = System.IO.File.ReadAllBytes( lCaminhoArquivoErro );
                lEmailInfo.Nome = String.Format("{0}_{1}.err", "ErrosSuitability", DateTime.Now.ToString("yyyyMMdd_Hmmss"));
                lAnexos.Add(lEmailInfo);

                if(!String.IsNullOrEmpty(lCampoErros))
                {
                    lCorpoEmail = lCorpoEmail.Replace(lCampoErros, lErros.ToString());
                }

                Gradual.OMS.Email.Lib.IServicoEmail lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.Email.Lib.IServicoEmail>();
                var lRequest = new Gradual.OMS.Email.Lib.EnviarEmailRequest();
                lRequest.Objeto = new Gradual.OMS.Email.Lib.EmailInfo();
                lRequest.Objeto.Assunto = String.Format("Suitability - Erros de processamento {0}", DateTime.Now.ToString());
                lRequest.Objeto.Destinatarios = new List<string>() { lEmailErroPara };
                lRequest.Objeto.Remetente = lEmailDe;
                lRequest.Objeto.CorpoMensagem = lCorpoEmail.ToString();
                lRequest.Objeto.Anexos = lAnexos;
                lRequest.Objeto.MensagemHTML = true;

                var lResponse = lServico.Enviar(lRequest);

            }

            if (gListaOk.Count > 0)
            {
                System.IO.StreamReader str = System.IO.File.OpenText(lCaminhoErro);
                String lCorpoEmail = str.ReadToEnd();
                str.Close();

                StringBuilder lListaOk = new StringBuilder();

                lListaOk.AppendLine(String.Format("{0};{1};{2}", "Tipo", "Cliente", "Mensagem"));
                foreach (Objetos.Erro lOk in gListaOk)
                {
                    lListaOk.AppendLine(String.Format("{0};{1};{2}", lOk.Tipo, lOk.CodigoCliente, lOk.Mensagem));
                }

                String lCaminhoArquivoErro = GerarArquivo("FundosOk", lListaOk);

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                Gradual.OMS.Email.Lib.EmailAnexoInfo lEmailInfo = new Gradual.OMS.Email.Lib.EmailAnexoInfo();
                lEmailInfo.Arquivo = System.IO.File.ReadAllBytes(lCaminhoArquivoErro);
                lEmailInfo.Nome = String.Format("{0}_{1}.err", "FundosOk", DateTime.Now.ToString("yyyyMMdd_Hmmss"));
                lAnexos.Add(lEmailInfo);

                if (!String.IsNullOrEmpty(lCampoErros))
                {
                    lCorpoEmail = lCorpoEmail.Replace(lCampoErros, lListaOk.ToString());
                }

                Gradual.OMS.Email.Lib.IServicoEmail lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.Email.Lib.IServicoEmail>();
                var lRequest = new Gradual.OMS.Email.Lib.EnviarEmailRequest();
                lRequest.Objeto = new Gradual.OMS.Email.Lib.EmailInfo();
                lRequest.Objeto.Assunto = String.Format("Suitability - Fundos processados corretamente {0}", DateTime.Now.ToString());
                lRequest.Objeto.Destinatarios = new List<string>() { lEmailErroPara };
                lRequest.Objeto.Remetente = lEmailDe;
                lRequest.Objeto.CorpoMensagem = lCorpoEmail.ToString();
                lRequest.Objeto.Anexos = lAnexos;
                lRequest.Objeto.MensagemHTML = true;

                var lResponse = lServico.Enviar(lRequest);

            }

            if (lEmailsEnviados.Length > 0)
            {
                System.IO.StreamReader str = System.IO.File.OpenText(lCaminhoErro);
                String lCorpoEmail = str.ReadToEnd();
                str.Close();
                
                String lCaminhoArquivoErro = GerarArquivo("EmailEnviados", lEmailsEnviados);

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                Gradual.OMS.Email.Lib.EmailAnexoInfo lEmailInfo = new Gradual.OMS.Email.Lib.EmailAnexoInfo();
                lEmailInfo.Arquivo = System.IO.File.ReadAllBytes(lCaminhoArquivoErro);
                lEmailInfo.Nome = String.Format("{0}_{1}.emails", "EmailsEnviados", DateTime.Now.ToString("yyyyMMdd_Hmmss"));
                lAnexos.Add(lEmailInfo);


                Gradual.OMS.Email.Lib.IServicoEmail lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.Email.Lib.IServicoEmail>();
                var lRequest = new Gradual.OMS.Email.Lib.EnviarEmailRequest();
                lRequest.Objeto = new Gradual.OMS.Email.Lib.EmailInfo();
                lRequest.Objeto.Assunto = String.Format("Suitability - Emails enviados {0}", DateTime.Now.ToString());
                lRequest.Objeto.Destinatarios = new List<string>() { lEmailErroPara };
                lRequest.Objeto.Remetente = lEmailDe;
                lRequest.Objeto.CorpoMensagem = lCorpoEmail.ToString();
                lRequest.Objeto.Anexos = lAnexos;
                lRequest.Objeto.MensagemHTML = true;

                var lResponse = lServico.Enviar(lRequest);

            }
        }

        public string GerarArquivo(String pPrefixoNome, StringBuilder pTexto)
        {
            string lRetorno = String.Empty;

            try
            {
                string lNomeArquivo =  String.Format("{0}{1}_{2}.err", lCaminhoArquivoErro, pPrefixoNome, DateTime.Now.ToString("yyyyMMdd_Hmmss"));
                string lRenamedArquivo = String.Format("{0}{1}_{2}.old", lCaminhoArquivoErro, pPrefixoNome, DateTime.Now.ToString("yyyyMMdd_Hmmss")); ;

                if (!System.IO.File.Exists(lNomeArquivo))
                {
                    System.IO.File.Create(lNomeArquivo).Close();
                }
                else
                {
                    System.IO.File.Move(lNomeArquivo, lRenamedArquivo);
                    System.IO.File.Create(lNomeArquivo).Close();
                }

                System.IO.TextWriter lArquivo = System.IO.File.AppendText(lNomeArquivo);
                lArquivo.Write(pTexto.ToString());

                lArquivo.Close();
                lRetorno = lNomeArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lRetorno;
        }

        public void EnviarEmail(Gradual.Suitability.Service.Objetos.ClienteSuitability pCliente, List<String> pProdutos)
        {
            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            try
            {
                String lEmailDestinatario   = String.Empty;
                String lCaminho             = String.Empty;
                String lProdutosCorpoEmail  = String.Empty;

                if(String.IsNullOrEmpty(lCodigoTeste))
                {
                    if (!String.IsNullOrEmpty(pCliente.Email))
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            //lEmailDestinatario = lEmailCCO;
                            lEmailDestinatario = "mmaebara@gradualinvestimentos.com.br";
                        }
                        else
                        {
                            lEmailDestinatario = pCliente.Email;
                        }
                    }
                    else
                    {
                        gListaErro.Add(new Objetos.Erro() { Tipo = Objetos.TipoErro.Fundo, CodigoCliente = pCliente.Conta, Mensagem = String.Format("Não foi possível enviar o email para o cliente Cod: {0} Conta: {1} pois o mesmo não possui e-mail [{2}] cadastrado.", pCliente.Codigo, pCliente.Conta, pCliente.Email) });
                    }
                }

                switch (pCliente.Descricao)
                {
                    case "Conservador":
                        lCaminho = lCaminhoConservador;        
                        break;
                    case "Moderado":
                        lCaminho = lCaminhoModerado;
                        break;
                    case "Arrojado":
                        lCaminho = lCaminhoArrojado;
                        break;
                }

                System.IO.StreamReader str = System.IO.File.OpenText(lCaminho);
                String lCorpoEmail = str.ReadToEnd();
                str.Close();

                //lProdutosCorpoEmail += "<table>";
                foreach (String lProduto in pProdutos)
                {
                    lProdutosCorpoEmail += String.Format("<br>{0}", lProduto);
                }
                //lProdutosCorpoEmail += "</table>";

                lCorpoEmail = lCorpoEmail.Replace(lCampoProdutos, lProdutosCorpoEmail.ToString());
                
                Gradual.OMS.Email.Lib.IServicoEmail lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.Email.Lib.IServicoEmail>();
                var lRequest = new Gradual.OMS.Email.Lib.EnviarEmailRequest();
                lRequest.Objeto = new Gradual.OMS.Email.Lib.EmailInfo();
                lRequest.Objeto.Assunto = lEmailSubject.Replace(lCampoAssunto, pCliente.Descricao.ToString() + String.Format(" - Codigo: {0} Conta: {1}", pCliente.Codigo, pCliente.Conta));
                lRequest.Objeto.Destinatarios = new List<string>() { lEmailDestinatario };

                List<String> lEmailsCCO = new List<string>();
                //if (lEmailCCO.Contains(';'))
                //{
                //    string[] lEmails = lEmailCCO.Split(';');

                //    foreach (String lEmail in lEmails)
                //    {
                //        lEmailsCCO.Add(lEmail);
                //    }

                //    if (System.Diagnostics.Debugger.IsAttached)
                //    {
                //        lRequest.Objeto.Destinatarios = lEmailsCCO;
                //    }
                //    else
                //    {
                //        lRequest.Objeto.DestinatariosCO = lEmailsCCO;
                //    }
                //}
                //else
                //{
                //    lRequest.Objeto.DestinatariosCO = new List<string>() { lEmailCCO, lEmailErroPara };
                //}
                
                lRequest.Objeto.Remetente = lEmailDe;
                lRequest.Objeto.CorpoMensagem = lCorpoEmail;
                lRequest.Objeto.MensagemHTML = true;

                var lResponse = lServico.Enviar(lRequest);

                if (lResponse.StatusResposta.Equals(OMS.Library.MensagemResponseStatusEnum.OK))
                {
                    lEmailsEnviados.AppendLine(String.Format("{0};{1};{2};{3};{4};{5};", pCliente.Conta, pCliente.CpfCnpj, pCliente.Data, pCliente.Descricao, pCliente.Email, String.Format("[{0}]", lProdutosCorpoEmail.Replace("<br>", ";"))));
                    SalvarEmailDisparado(Int32.Parse(pCliente.Codigo), pCliente.Descricao, lRequest);
                }
                else
                {
                    Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, String.Format("{0} - {1}:{2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lResponse.StatusResposta, lResponse.DescricaoResposta), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void LimparDadosTemporariosOracle()
        {

            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            
            try
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Debug, String.Format("Inicio limpeza"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                using (var lAcessaDados = new Gradual.Generico.Dados.AcessaDados() { ConnectionStringName = gNomeConexaoSuitability })
                {
                    using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_SUITA_LIMPA_FORA_PERFIL"))
                    {
                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }

                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Debug, String.Format("Fim limpeza"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public Int32 SalvarEmailDisparado(Int32 pIdCliente, String pPerfil, Gradual.OMS.Email.Lib.EnviarEmailRequest pEnviarEmailRequest)
        {

            Gradual.Utils.Logger.Log("Suitability", Gradual.Utils.LoggingLevel.Info, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            using (var lAcessaDados = new Gradual.Generico.Dados.AcessaDados() { ConnectionStringName = gNomeConexaoCadastro })
            {
                //using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "rel_email_disparado_periodo_ins_sp"))
                using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "rel_email_nao_disparado_periodo_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipoemail", DbType.Int64, (long)pEnviarEmailRequest.Objeto.ETipoTemplate);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_corpoemail", DbType.String, pEnviarEmailRequest.Objeto.CorpoMensagem);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_emailremetente", DbType.String, pEnviarEmailRequest.Objeto.Remetente.ToString());
                    
                    String lDestinatarios = String.Empty;

                    if(pEnviarEmailRequest.Objeto.Destinatarios.Count > 0)
                    {

                        foreach (String lDestinatario in pEnviarEmailRequest.Objeto.Destinatarios)
                        {
                            lDestinatarios += String.Format("{0};", lDestinatario);
                        }

                        lAcessaDados.AddInParameter(lDbCommand, "@ds_emaildestinatario", DbType.String, lDestinatarios);
                    }

                    
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_envioemail", DbType.DateTime, gDataInicial.AddDays(7));
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_assuntoemail", DbType.String, pEnviarEmailRequest.Objeto.Assunto);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pIdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_perfil", DbType.String, pPerfil);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_email", DbType.Int64, 16);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    var lResponse = Convert.ToInt32(lDbCommand.Parameters["@id_email"].Value);

                    return lResponse;
                }
            }
        }

        private void LimparMemoria()
        {
            ListaPerfilExclusao                         = new Dictionary<int, List<Gradual.Suitability.Service.Objetos.ProdutoExclusao>>();
            gListaClientes                              = new List<Gradual.Suitability.Service.Objetos.ClienteSuitability>();
            gListaCotistaItau                           = new List<Gradual.Suitability.Service.Objetos.PosicaoCotistaItau>();
            gListaCotistaFinancialCompleta              = new List<Objetos.PosicaoCotistaFinancial>();
            gListaExclusao                              = new List<Objetos.ProdutoExclusao>();
            gListaDePara                                = new Dictionary<string, string>();
            gListaFundos                                = new List<Gradual.Suitability.Service.Objetos.Fundo>();
            gListaErro                                  = new List<Objetos.Erro>();
            gListaOk                                    = new List<Objetos.Erro>();
            gListaCotistaFinancial                      = new List<Objetos.PosicaoCotistaFinancial>();

        
            ListaClientesDesenquadradosBovespa          = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            ListaClientesDesenquadradosBMF              = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            ListaClientesDesenquadradosBTC              = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            ListaClientesDesenquadradosFundoFinancial   = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            ListaClientesDesenquadradosFundoItau        = new List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>();
            gListaDesenquadramento                      = new Dictionary<string, List<Gradual.Suitability.Service.Objetos.ClienteDesenquadrado>>();
        }
        #endregion

        public DateTime RetornarSegundaFeiraAnterior()
        {
            DateTime lData = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int lDiaDaSemana = (int)lData.DayOfWeek;
            DateTime lSegundaAnterior = lData.AddDays((6 + lDiaDaSemana) * -1);
            return lSegundaAnterior;
        }
    }
}

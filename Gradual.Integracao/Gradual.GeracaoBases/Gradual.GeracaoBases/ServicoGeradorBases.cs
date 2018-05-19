using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using Gradual.GeracaoBasesDB.Lib.Dados;
using Gradual.OMS.Library;
using log4net;
using System.IO;
using Gradual.GeracaoBasesDB.Lib;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.Threading;

namespace Gradual.GeracaoBases
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoGeradorBases : IServicoControlavel
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private ServicoStatus _status = ServicoStatus.Parado;

        private GeracaoBaseConfig _config = null;
        private CronStyleScheduler _cron = null;
        private Dictionary<string, BaseParam> dctParametros = new Dictionary<string, BaseParam>();
        private static ServicoGeradorBases _me = null;
        private int portaHttp = 18080;
        private MyHttpServer httpServer = null;
        private Thread thHttp = null;

        public static ServicoGeradorBases GetInstance()
        {
            if (_me == null)
            {
                _me = new ServicoGeradorBases();
            }

            return _me;
        }

        public void IniciarServico()
        {
            _me = this;
            _config = GerenciadorConfig.ReceberConfig<GeracaoBaseConfig>();

            if (_config != null)
            {
                foreach (BaseParam parametro in _config.Parametros)
                {
                    logger.Info("Carregando parametros da funcao [" + parametro.FunctionName + "]");
                    dctParametros.Add(parametro.FunctionName, parametro);
                }
            }

            if (ConfigurationManager.AppSettings["PortaHttp"] != null)
                portaHttp = Convert.ToInt32(ConfigurationManager.AppSettings["PortaHttp"].ToString());

            httpServer = new MyHttpServer(portaHttp, _config);

            thHttp = new Thread(new ThreadStart(httpServer.listen));
            thHttp.Start();

            _cron = new CronStyleScheduler();
            _cron.Start();

            _status = ServicoStatus.EmExecucao;
        }

        public void PararServico()
        {
            if (thHttp != null)
            {
            }

            _cron.Stop();
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        public void GerarBase(string functionName)
        {
            try
            {

                logger.Info("GerarBase(" + functionName + ")");

                if (!dctParametros.ContainsKey(functionName))
                {
                    logger.Error("GerarBase(" + functionName + ") nao ha parametros definidos");
                    return;
                }

                BaseParam parametros = dctParametros[functionName];

                string scriptPath = ConfigurationManager.AppSettings["DiretorioScripts"].ToString();
                string sheetPath = ConfigurationManager.AppSettings["DiretorioPlanilhas"].ToString();
                string rede = "";

                if (ConfigurationManager.AppSettings["DiretorioRede"] != null)
                {
                    rede = ConfigurationManager.AppSettings["DiretorioRede"].ToString();
                }

                string scriptFilename = scriptPath + Path.DirectorySeparatorChar + parametros.ScriptFile;
                string excelFile = sheetPath + Path.DirectorySeparatorChar + parametros.ExcelFilePrefix + "-" + DateTime.Now.ToString("yyyMMdd-HHmmss") + ".xlsx";

                logger.Info("GerarBase(" + functionName + ") Carregando script: " + scriptFilename);

                string scriptSQL = File.ReadAllText(scriptFilename);

                PersistenciaDB db = new PersistenciaDB();

                logger.Info("GerarBase(" + functionName + ") executando script");

                DataSet ds = db.ExecutarScriptRetDS(scriptSQL);

                if (ds.Tables.Count == 0 || (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0) )
                {
                    logger.Error("GerarBase(" + functionName + ") Nao ha registros para se gerar uma planilha");
                    return;
                }

                logger.Info("GerarBase(" + functionName + ") dataset carregado, gerando planilha");

                if (ExcelCreator.CreateExcel(ds, excelFile, parametros.WorksheetName))
                {
                    logger.Info("GerarBase(" + functionName + ") planilha gerada com sucesso");

                    string subject = parametros.Subject + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                    string message = "Planilha " + functionName + " gerada as " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    message += "\n\n";
                    message += "Gravado em [" + excelFile + "]";

                    if (!String.IsNullOrEmpty(rede))
                    {
                        message += "\n\n";
                        message += "Disponivel na pasta de rede: [" + rede + "]";
                    }
                         

                    if ( !String.IsNullOrEmpty(parametros.Message) )
                    {
                        message += "\n\n" + parametros.Message;
                    }

                    message += "\n\n";

                    string [] anexos = new string[1];
                    anexos[0] = excelFile;

                    MailUtil.EnviarPlanilhaPorEmail( parametros.MailFrom,
                        parametros.MailTo,
                        null,
                        null,
                        subject,
                        message,
                        anexos);

                }

                try
                {
                    if (!String.IsNullOrEmpty(rede))
                    {
                        FileInfo excelInfo = new FileInfo(excelFile);
                        rede += Path.DirectorySeparatorChar;
                        rede += excelInfo.Name;

                        logger.Info("GerarBase(" + functionName + ") Copiando arquivo [" + excelFile + "] para [" + rede + "]");
                        File.Copy(excelFile, rede);
                    }
                    else
                        logger.Info("GerarBase(" + functionName + ") Chave appsettings 'DiretorioRede' nao existe para copia do arquivo!");
                }
                catch (Exception ex)
                {
                    logger.Error("GerarBase(" + functionName + "): Erro ao copiar para pasta de rede");
                    logger.Error("GerarBase(" + functionName + "): " + ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                logger.Error("GerarBase(" + functionName + "): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        public void ExecutarScriptOracle(string fullFileName)
        {
            try
            {
                string scriptPath = ConfigurationManager.AppSettings["DiretorioScripts"].ToString();

                string scriptFilename = scriptPath + Path.DirectorySeparatorChar + fullFileName;

                logger.Info("ExecutarScriptOracle carregando script [" + scriptFilename + "]");

                string scriptSQL = File.ReadAllText(scriptFilename);

                PersistenciaDB db = new PersistenciaDB();

                if ( !db.ExecutarScript(scriptSQL) )
                {
                    logger.Error("Erro ao executar script (" + scriptFilename + ")");
                    return;
                }

            }
            catch (Exception ex)
            {
                logger.Error("ExecutarScriptOracle(" + fullFileName + "): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo apenas para logar atividade do cronscheduler
        /// </summary>
        public void CronWatchDog()
        {
            logger.Info("CronWatchDog called");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        public void GenerateFromSQLServer(string functionName)
        {
            try
            {

                logger.Info("GenerateFromSQLServer(" + functionName + ")");

                if (!dctParametros.ContainsKey(functionName))
                {
                    logger.Error("GenerateFromSQLServer(" + functionName + ") nao ha parametros definidos");
                    return;
                }

                BaseParam parametros = dctParametros[functionName];

                string scriptPath = ConfigurationManager.AppSettings["DiretorioScripts"].ToString();
                string sheetPath = ConfigurationManager.AppSettings["DiretorioPlanilhas"].ToString();
                string rede = "";

                if (ConfigurationManager.AppSettings["DiretorioRede"] != null)
                {
                    rede = ConfigurationManager.AppSettings["DiretorioRede"].ToString();
                }

                string scriptFilename = scriptPath + Path.DirectorySeparatorChar + parametros.ScriptFile;
                string excelFile = sheetPath + Path.DirectorySeparatorChar + parametros.ExcelFilePrefix + "-" + DateTime.Now.ToString("yyyMMdd-HHmmss") + ".xlsx";

                logger.Info("GenerateFromSQLServer(" + functionName + ") Carregando script: " + scriptFilename);

                string scriptSQL = File.ReadAllText(scriptFilename);

                PersistenciaDB db = new PersistenciaDB();

                logger.Info("GenerateFromSQLServer(" + functionName + ") executando script");

                DataSet ds = db.ExecutarScriptSqlServer(parametros.ConnStringName, scriptSQL);

                if (ds.Tables.Count == 0 || (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0))
                {
                    logger.Error("GenerateFromSQLServer(" + functionName + ") Nao ha registros para se gerar uma planilha");
                    return;
                }

                logger.Info("GenerateFromSQLServer(" + functionName + ") dataset carregado, gerando planilha");

                if (ExcelCreator.CreateExcel(ds, excelFile, parametros.WorksheetName))
                {
                    logger.Info("GenerateFromSQLServer(" + functionName + ") planilha gerada com sucesso");


                    string subject = parametros.Subject + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                    string message = "Planilha " + functionName + " gerada as " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    message += "\n\n";
                    message += "Gravado em [" + excelFile + "]";

                    if (!String.IsNullOrEmpty(rede))
                    {
                        message += "\n\n";

                        message += "Disponivel na pasta de rede: [" + rede + "]";
                    }


                    if (!String.IsNullOrEmpty(parametros.Message))
                    {
                        message += "\n\n" + parametros.Message;
                    }

                    message += "\n\n";

                    string[] anexos = new string[1];
                    anexos[0] = excelFile;

                    MailUtil.EnviarPlanilhaPorEmail(parametros.MailFrom,
                        parametros.MailTo,
                        null,
                        null,
                        subject,
                        message,
                        anexos);

                }

                try
                {
                    if (!String.IsNullOrEmpty(rede))
                    {
                        FileInfo excelInfo = new FileInfo(excelFile);
                        rede += Path.DirectorySeparatorChar;
                        rede += excelInfo.Name;

                        logger.Info("GenerateFromSQLServer(" + functionName + ") Copiando arquivo [" + excelFile + "] para [" + rede + "]");
                        File.Copy(excelFile, rede);
                    }
                    else
                        logger.Info("GenerateFromSQLServer(" + functionName + ") Chave appsettings 'DiretorioRede' nao existe para copia do arquivo!");
                }
                catch (Exception ex)
                {
                    logger.Error("GenerateFromSQLServer(" + functionName + "): Erro ao copiar para pasta de rede");
                    logger.Error("GenerateFromSQLServer(" + functionName + "): " + ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                logger.Error("GenerateFromSQLServer(" + functionName + "): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Gera o relatorio de vencimentos de renda fixa
        /// quebrou o esquema, pq tem que buscar a relacao de contas x assessor do oracle pra cruzar com
        /// a consulta do MSSQL
        /// </summary>
        /// <param name="functionName"></param>
        public void GenerateVctosRendaFixa(string functionName)
        {
            try
            {

                logger.Info("GenerateVctosRendaFixa(" + functionName + ")");

                if (!dctParametros.ContainsKey(functionName))
                {
                    logger.Error("GenerateVctosRendaFixa(" + functionName + ") nao ha parametros definidos");
                    return;
                }

                BaseParam parametros = dctParametros[functionName];

                string scriptPath = ConfigurationManager.AppSettings["DiretorioScripts"].ToString();
                string sheetPath = ConfigurationManager.AppSettings["DiretorioPlanilhas"].ToString();
                string rede = "";

                if (ConfigurationManager.AppSettings["DiretorioRede"] != null)
                {
                    rede = ConfigurationManager.AppSettings["DiretorioRede"].ToString();
                }

                string scriptFilename = scriptPath + Path.DirectorySeparatorChar + parametros.ScriptFile;
                string scriptAssessores = scriptPath + Path.DirectorySeparatorChar + "getassess.sql";
                string excelFile = sheetPath + Path.DirectorySeparatorChar + parametros.ExcelFilePrefix + "-" + DateTime.Now.ToString("yyyMMdd-HHmmss") + ".xlsx";

                logger.Info("GenerateVctosRendaFixa(" + functionName + ") Carregando script: " + scriptFilename);

                PersistenciaDB db = new PersistenciaDB();

                string scriptAssessor = File.ReadAllText(scriptAssessores);

                logger.Info("GenerateVctosRendaFixa(" + functionName + ") obtendo tabela assessores");

                DataSet dsAss = db.ExecutarScriptRetDS(scriptAssessor);

                logger.Info("GenerateVctosRendaFixa(" + functionName + ") executando script");

                string scriptSQL = File.ReadAllText(scriptFilename);

                DataSet ds = db.ExecutarScriptSqlServer(parametros.ConnStringName, scriptSQL);

                if (ds.Tables.Count == 0 || (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0))
                {
                    logger.Error("GenerateVctosRendaFixa(" + functionName + ") Nao ha registros para se gerar uma planilha");
                    return;
                }

                var query = from record in ds.Tables[0].AsEnumerable()
                            join assessor in dsAss.Tables[0].AsEnumerable()
                            on record.Field<string>("Cliente") equals assessor.Field<string>("Cliente")
                            select new
                            {
                                Cliente = record.Field<string>("Cliente").Trim(),
                                Nome = assessor.Field<string>("Nome").Trim(),
                                CpfCnpj = assessor.Field<string>("CPF/CNPJ").Trim(),
                                Assessor = assessor.Field<string>("Assessor").Trim(),
                                NomeAssessor = assessor.Field<string>("Nome Assessor").Trim(),
                                Titulo = record.Field<string>("Titulo").Trim(),
                                Aplicacao = record.Field<string>("Aplicacao").Trim(),
                                Vencimento = record.Field<string>("Vencimento").Trim(),
                                Taxa = record.Field<string>("Taxa").Trim(),
                                Quantidade = record.Field<string>("Quantidade").Trim(),
                                Indice = record.Field<string>("Indice").Trim(),
                                ValorOriginal = record.Field<string>("ValorOriginal").Trim(),
                                IRRF = record.Field<string>("IRRF").Trim(),
                                IOF = record.Field<string>("IOF").Trim(),
                                SaldoLiquido = record.Field<string>("SaldoLiquido").Trim(),
                                DataAtualizacao = record.Field<string>("DataAtualizacao").Trim(),
                            };

                DataSet ds1 = new DataSet();

                ds1.Tables.Add(query.ToDataTable());

                logger.Info("GenerateVctosRendaFixa(" + functionName + ") dataset carregado, gerando planilha");

                if (ExcelCreator.CreateExcel(ds1, excelFile, parametros.WorksheetName))
                {
                    logger.Info("GenerateVctosRendaFixa(" + functionName + ") planilha gerada com sucesso");


                    string subject = parametros.Subject + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                    string message = "Planilha " + functionName + " gerada as " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    message += "\n\n";
                    message += "Gravado em [" + excelFile + "]";

                    if (!String.IsNullOrEmpty(rede))
                    {
                        message += "\n\n";

                        message += "Disponivel na pasta de rede: [" + rede + "]";
                    }


                    if (!String.IsNullOrEmpty(parametros.Message))
                    {
                        message += "\n\n" + parametros.Message;
                    }

                    message += "\n\n";

                    string[] anexos = new string[1];
                    anexos[0] = excelFile;

                    MailUtil.EnviarPlanilhaPorEmail(parametros.MailFrom,
                        parametros.MailTo,
                        null,
                        null,
                        subject,
                        message,
                        anexos);

                }

                try
                {
                    if (!String.IsNullOrEmpty(rede))
                    {
                        FileInfo excelInfo = new FileInfo(excelFile);
                        rede += Path.DirectorySeparatorChar;
                        rede += excelInfo.Name;

                        logger.Info("GenerateVctosRendaFixa(" + functionName + ") Copiando arquivo [" + excelFile + "] para [" + rede + "]");
                        File.Copy(excelFile, rede);
                    }
                    else
                        logger.Info("GenerateVctosRendaFixa(" + functionName + ") Chave appsettings 'DiretorioRede' nao existe para copia do arquivo!");
                }
                catch (Exception ex)
                {
                    logger.Error("GenerateVctosRendaFixa(" + functionName + "): Erro ao copiar para pasta de rede");
                    logger.Error("GenerateVctosRendaFixa(" + functionName + "): " + ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                logger.Error("GenerateVctosRendaFixa(" + functionName + "): " + ex.Message, ex);
            }
        }



        /// <summary>
        /// Gera o relatorio de usuarios logados no HB e saldo
        /// quebrou o esquema, pq tem que buscar a relacao de contas x assessor do oracle pra cruzar com
        /// a consulta do MSSQL
        /// </summary>
        /// <param name="functionName"></param>
        public void GenerateLogadosHBSaldo(string functionName)
        {
            try
            {

                logger.Info("GenerateLogadosHBSaldo(" + functionName + ")");

                if (!dctParametros.ContainsKey(functionName))
                {
                    logger.Error("GenerateLogadosHBSaldo(" + functionName + ") nao ha parametros definidos");
                    return;
                }

                BaseParam parametros = dctParametros[functionName];

                string scriptPath = ConfigurationManager.AppSettings["DiretorioScripts"].ToString();
                string sheetPath = ConfigurationManager.AppSettings["DiretorioPlanilhas"].ToString();
                string rede = "";

                if (ConfigurationManager.AppSettings["DiretorioRede"] != null)
                {
                    rede = ConfigurationManager.AppSettings["DiretorioRede"].ToString();
                }

                string scriptFilename = scriptPath + Path.DirectorySeparatorChar + parametros.ScriptFile;
                string scriptAssessores = scriptPath + Path.DirectorySeparatorChar + "getassess.sql";
                string excelFile = sheetPath + Path.DirectorySeparatorChar + parametros.ExcelFilePrefix + "-" + DateTime.Now.ToString("yyyMMdd-HHmmss") + ".xlsx";

                logger.Info("GenerateLogadosHBSaldo(" + functionName + ") Carregando script: " + scriptFilename);


                PersistenciaDB db = new PersistenciaDB();

                string idlogins = "select distinct(IdLogin) from tb_logacesso where Sistema='Portal' and DataLogIn > DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))";

                DataSet dsLogins = db.ExecutarScriptSqlServer("ControleAcesso", idlogins);

                string idClientes = "select C.cd_codigo,L.id_login  from tb_cliente_conta C, tb_cliente L where C.cd_sistema = 'BOL' and L.id_cliente = C.id_cliente";

                DataSet dsClientes = db.ExecutarScriptSqlServer("Cadastro", idClientes);


                string scriptAssessor = File.ReadAllText(scriptAssessores);

                logger.Info("GenerateLogadosHBSaldo(" + functionName + ") obtendo tabela assessores");

                DataSet dsAss = db.ExecutarScriptRetDS(scriptAssessor);

                logger.Info("GenerateLogadosHBSaldo(" + functionName + ") executando script");

                string scriptSQL = File.ReadAllText(scriptFilename);

                DataSet ds = db.ExecutarScriptSqlServer(parametros.ConnStringName, scriptSQL);

                if (ds.Tables.Count == 0 || (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0))
                {
                    logger.Error("GenerateLogadosHBSaldo(" + functionName + ") Nao ha registros para se gerar uma planilha");
                    return;
                }

                var query = from record in ds.Tables[0].AsEnumerable()
                            join assessor in dsAss.Tables[0].AsEnumerable()
                            on record.Field<string>("Cliente") equals assessor.Field<string>("Cliente")
                            select new
                            {
                                Cliente = record.Field<string>("Cliente").Trim(),
                                Nome = assessor.Field<string>("Nome").Trim(),
                                CpfCnpj = assessor.Field<string>("CPF/CNPJ").Trim(),
                                Assessor = assessor.Field<string>("Assessor").Trim(),
                                NomeAssessor = assessor.Field<string>("Nome Assessor").Trim(),
                                Titulo = record.Field<string>("Titulo").Trim(),
                                Aplicacao = record.Field<string>("Aplicacao").Trim(),
                                Vencimento = record.Field<string>("Vencimento").Trim(),
                                Taxa = record.Field<string>("Taxa").Trim(),
                                Quantidade = record.Field<string>("Quantidade").Trim(),
                                Indice = record.Field<string>("Indice").Trim(),
                                ValorOriginal = record.Field<string>("ValorOriginal").Trim(),
                                IRRF = record.Field<string>("IRRF").Trim(),
                                IOF = record.Field<string>("IOF").Trim(),
                                SaldoLiquido = record.Field<string>("SaldoLiquido").Trim(),
                                DataAtualizacao = record.Field<string>("DataAtualizacao").Trim(),
                            };

                DataSet ds1 = new DataSet();

                ds1.Tables.Add(query.ToDataTable());

                logger.Info("GenerateVctosRendaFixa(" + functionName + ") dataset carregado, gerando planilha");

                if (ExcelCreator.CreateExcel(ds1, excelFile, parametros.WorksheetName))
                {
                    logger.Info("GenerateVctosRendaFixa(" + functionName + ") planilha gerada com sucesso");


                    string subject = parametros.Subject + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                    string message = "Planilha " + functionName + " gerada as " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    message += "\n\n";
                    message += "Gravado em [" + excelFile + "]";

                    if (!String.IsNullOrEmpty(rede))
                    {
                        message += "\n\n";

                        message += "Disponivel na pasta de rede: [" + rede + "]";
                    }


                    if (!String.IsNullOrEmpty(parametros.Message))
                    {
                        message += "\n\n" + parametros.Message;
                    }

                    message += "\n\n";

                    string[] anexos = new string[1];
                    anexos[0] = excelFile;

                    MailUtil.EnviarPlanilhaPorEmail(parametros.MailFrom,
                        parametros.MailTo,
                        null,
                        null,
                        subject,
                        message,
                        anexos);

                }

                try
                {
                    if (!String.IsNullOrEmpty(rede))
                    {
                        FileInfo excelInfo = new FileInfo(excelFile);
                        rede += Path.DirectorySeparatorChar;
                        rede += excelInfo.Name;

                        logger.Info("GenerateVctosRendaFixa(" + functionName + ") Copiando arquivo [" + excelFile + "] para [" + rede + "]");
                        File.Copy(excelFile, rede);
                    }
                    else
                        logger.Info("GenerateVctosRendaFixa(" + functionName + ") Chave appsettings 'DiretorioRede' nao existe para copia do arquivo!");
                }
                catch (Exception ex)
                {
                    logger.Error("GenerateVctosRendaFixa(" + functionName + "): Erro ao copiar para pasta de rede");
                    logger.Error("GenerateVctosRendaFixa(" + functionName + "): " + ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                logger.Error("GenerateVctosRendaFixa(" + functionName + "): " + ex.Message, ex);
            }
        }


    }
}

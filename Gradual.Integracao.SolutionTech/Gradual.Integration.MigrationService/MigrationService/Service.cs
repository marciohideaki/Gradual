using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using System.ServiceModel;
using System.Runtime.InteropServices;

namespace Gradual.MigrationService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service : Gradual.OMS.Library.Servicos.IServicoControlavel, Gradual.MigrationService.Lib.IService
    {
        private Gradual.OMS.Library.Servicos.ServicoStatus _status = Gradual.OMS.Library.Servicos.ServicoStatus.Parado;
        private static Service _self = null;
        private static System.Threading.Mutex _mutex = new System.Threading.Mutex();
        bool _bKeepRunning = false;
        private System.Threading.Thread thrMonitor = null;
        private Gradual.OMS.Library.CronStyleScheduler _cron = null;

        public static Service GetInstance()
        {
            _mutex.WaitOne();

            if (_self == null)
            {
                _self = new Service();
            }

            _mutex.ReleaseMutex();

            return _self;
        }

        public void IniciarServico()
        {
            //throw new NotImplementedException();

            Gradual.Utils.Logger.Initialize();

            Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Iniciando serviço"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            _bKeepRunning = true;

            Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}",
                    Gradual.Utils.MethodHelper.GetCurrentMethod(),
                    "Criando thread do monitor"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            thrMonitor = new System.Threading.Thread(new System.Threading.ThreadStart(Monitor));
            thrMonitor.Start();

            Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}",
                    Gradual.Utils.MethodHelper.GetCurrentMethod(),
                    "Iniciando Schenduler"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            _cron = new Gradual.OMS.Library.CronStyleScheduler();
            _cron.Start();

            Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}",
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

            Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Parando serviço"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

        }

        public Gradual.OMS.Library.Servicos.ServicoStatus ReceberStatusServico()
        {
            //throw new NotImplementedException();
            return _status;
        }

        public void CronWatchDog()
        {
            Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}",
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
                    Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }
            }
        }

        public void Migrate()
        {
            try
            {
                Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Iniciando migração"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                string lCodigosAssessores = System.Configuration.ConfigurationManager.AppSettings["CodigoAssessor"];
                string[] lListaAssessores = lCodigosAssessores.Split(';');

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                List<Cliente> lListaClientesSinacor = new List<Cliente>();
                List<Cliente> lListaClientesMigrados = new List<Cliente>();
                List<Cliente> lListaClientesMigrar = new List<Cliente>();

                lAcessaDados.ConnectionStringName = "GradualOMS";

                Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Carregando lista de clientes já migrados"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_sel_cliente_migracao_soltech"))
                {
                    System.Data.DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow lRow in lDataTable.Rows)
                        {
                            lListaClientesMigrados.Add(new Cliente() { CodigoCliente = lRow["CodigoCliente"].DBToInt32() });
                        }
                    }
                }
                Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Lista de clientes já migrados carregada"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Carregando lista de clientes a serem migrados"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                foreach (string lCodigoAssessor in lListaAssessores)
                {
                    lAcessaDados = new ConexaoDbHelper();
                    lAcessaDados.ConnectionStringName = "Sinacor";
                    
                    Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1} {2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), " pesquisando clientes do assessor ", lCodigoAssessor), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    
                    using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_CLIENTES_MIGRACAO_SOLTECH"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESSOR", System.Data.DbType.Int32, Int32.Parse(lCodigoAssessor));

                        System.Data.DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                          
                            foreach (System.Data.DataRow lRow in lDataTable.Rows)
                            {
                                Cliente lCliente = new Cliente() { CodigoCliente = lRow["CD_CLIENTE"].DBToInt32() };

                                var lClienteExiste = from c in lListaClientesMigrados where c.CodigoCliente == lCliente.CodigoCliente select c;

                                if (lClienteExiste.Count().Equals(0))
                                {
                                    Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}{2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Cliente a ser migrado:", lCliente.CodigoCliente.ToString()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                                    lListaClientesMigrar.Add(lCliente);
                                }
                            }
                        }
                    }
                }
                Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Lista de clientes a serem migrados carregada"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "verificando clientes à migar"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                if (lListaClientesMigrar.Count > 0)
                {
                    Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}{2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lListaClientesMigrar.Count.ToString(), " a ser(em) migrado(s)"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    foreach (Cliente lCliente in lListaClientesMigrar)
                    {
                        Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}{2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lCliente.CodigoCliente.ToString(), " sendo preparado para migração."), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                        lAcessaDados = new ConexaoDbHelper();
                        lAcessaDados.ConnectionStringName = "GradualOMS";

                        using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_ins_cliente_migracao_soltech"))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente", System.Data.DbType.Int32, lCliente.CodigoCliente);

                            lAcessaDados.ExecuteNonQuery(lDbCommand);
                        }

                        Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}{2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lCliente.CodigoCliente.ToString(), " migrado com sucesso."), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    }
                }
                
            }
            catch (Exception ex)
            {
                //throw ex;
                Gradual.Utils.Logger.Log("Service", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }
    }

    public class Cliente
    {
        public int CodigoCliente { get; set; }
    }

}

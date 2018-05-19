using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.ServiceProcess;
using System.Timers;
using Gradual.Cadastro.Exportacao;

namespace Gradual.Cadastro.ExportaClientes
{
    public partial class ExportaCliente : ServiceBase
    {
        #region | Atributos

        private System.Timers.Timer gDtpHora = null;

        #endregion

        #region | Propriedades

        private string GetBancoSql
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["BancoSql"].ToString();
            }
        }

        private string GetBancoOracle
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["BancoOracle"].ToString();
            }
        }

        private string GetCaminhoCliente
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ArquivoClientes"].ToString();
            }
        }

        private string GetCaminhoAssessor
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ArquivoAssessores"].ToString();
            }
        }

        private double GetPeriodoDeExecucao
        {
            get
            {
                double lPeriodoDeExecucaoEmHoras = default(double);

                if (!double.TryParse(ConfigurationManager.AppSettings["PeriodoDeExecucaoEmHoras"], out lPeriodoDeExecucaoEmHoras))
                    lPeriodoDeExecucaoEmHoras = 2D; //--> Caso ocorra problema ao ler o AppSetting é setado 2 horas de intervalo.

                return lPeriodoDeExecucaoEmHoras * 60D * 1000D;
            }
        }

        #endregion

        #region | Construtores

        public ExportaCliente()
        {
            InitializeComponent();
        }

        #endregion

        #region | Eventos

        protected override void OnStart(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR"); 

            //Segundo = 1000
            //minito = 60
            gDtpHora.Interval = this.GetPeriodoDeExecucao;
            gDtpHora.Enabled = true;
            gDtpHora.Start();
        }

        protected override void OnStop()
        {
            gDtpHora.Enabled = false;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            bool lRealizarGeracaoDoArquivo = !DayOfWeek.Sunday.Equals(DateTime.Today.DayOfWeek)
                                          && !DayOfWeek.Saturday.Equals(DateTime.Today.DayOfWeek)
                                          && DateTime.Now.Hour >= 4
                                          && DateTime.Now.Hour <= 6;

            if (lRealizarGeracaoDoArquivo)
            {
                try
                {
                    new Exporta().ExportarClientes(GetCaminhoCliente, GetBancoSql, GetBancoOracle);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Serviço Gradual.Cadastro.ExportaClientes", string.Concat("Erro ao Exportar Clientes: ", ex.ToString()), EventLogEntryType.Error);
                }
                try
                {
                    new Exporta().ExportarAssessores(GetCaminhoAssessor, GetBancoOracle);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Serviço Gradual.Cadastro.ExportaClientes", string.Concat("Erro ao Exportar Assessores: ", ex.ToString()), EventLogEntryType.Error);
                }
            }
        }

        #endregion
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Gradual.MinhaConta.Negocio;

namespace Gradual.MinhaConta.Servico.ImportacaoFundos
{
    public partial class ImportacaoClubesEFundos : ServiceBase
    {
        #region | Atributos

        private DateTime UltimaAtualizacao;

        #endregion

        #region | Propriedades

        private int GetInterval
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings["intervalo"].ToString()) * 60000; }
        }

        private string GetPathFundos
        {
            get { return @System.Configuration.ConfigurationManager.AppSettings["pathFundos"].ToString(); }
        }

        private string GetArquivoFundos
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["arquivoFundos"].ToString(); }
        }

        private string GetPathClubes
        {
            get { return @System.Configuration.ConfigurationManager.AppSettings["pathClubes"].ToString(); }
        }

        private string GetHoje
        {
            get { return DateTime.Now.ToString("ddMMyyyy"); }
        }

        #endregion

        #region | Construtores

        public ImportacaoClubesEFundos()
        {
            this.InitializeComponent();
        }

        #endregion

        #region | Eventos

        protected override void OnStart(string[] args)
        {
            try
            {
                this.UltimaAtualizacao = DateTime.Now.AddDays(-1);

                
                this.Timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
                this.Timer1.Interval = this.GetInterval;
                this.Timer1.Enabled = true;
                //this.Timer1.Start();

                this.RegistrarEvento(new Exception(string.Format("Iniciou, timer = {0} segundos", (this.GetInterval / 1000).ToString())), EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                this.RegistrarEvento(ex, EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            this.Timer1.Enabled = false;
            //this.Timer1.Stop();
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.RegistrarEvento(new Exception("O Serviço de Atualização da Posição de Clubes e Fundos Iniciou a Execução"), EventLogEntryType.Information);

            this.ProcessarArquivosFundos();

            this.ProcessarArquivosClubes();
        }

        #endregion

        #region | Métodos

        public void ProcessarArquivosFundos()
        {
            FileInfo[] filesInfo = new DirectoryInfo(GetPathFundos).GetFiles(string.Concat(this.GetArquivoFundos, "*", this.GetHoje, "*"));
            var nomeArquivo = string.Empty;

            try
            {
                foreach (FileInfo item in filesInfo.OrderBy(f => f.CreationTime))
                    if (item.CreationTime > this.UltimaAtualizacao)
                    {
                        nomeArquivo = item.FullName;
                        new NFundos().ProcessarArquivos(nomeArquivo);
                    }
            }
            catch (Exception ex)
            {
                this.RegistrarEvento(new Exception(string.Format("Fundos - Erro ao Processar o Arquivo: {0} Exception: {1}", nomeArquivo, ex.Message)), EventLogEntryType.Error);
            }

            this.UltimaAtualizacao = DateTime.Now;
        }

        public void ProcessarArquivosClubes()
        {
            try
            {
                new NClubes().AtualizarClubes(this.@GetPathClubes);
            }
            catch (Exception ex)
            {
                this.RegistrarEvento(new Exception(string.Format("Clubes - Erro ao Processar o Arquivo: {0} Exception: {1}", this.@GetPathClubes, ex.Message)), EventLogEntryType.Error);
            }
        }

        private void RegistrarEvento(Exception ex, EventLogEntryType tipo)
        {
            EventLog.WriteEntry("Importação de Fundos", ex.Message, tipo);
        }

        #endregion
    }
}

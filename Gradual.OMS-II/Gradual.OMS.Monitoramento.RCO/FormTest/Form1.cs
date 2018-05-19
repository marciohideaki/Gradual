using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Risco.Info;
using Gradual.OMS.Library.Servicos;
using Gradual.Monitores.Risco;
using Gradual.Monitores.Compliance;
using System.Threading;
using System.Collections;
using log4net;
using Gradual.Monitores.Persistencia;
using Gradual.Monitores.Risco.Enum;


namespace FormTest
{
    public partial class frmMonitor : Form
    {
        public frmMonitor()
        {
            InitializeComponent();
        }

        ServerMonitor _Monitor = new ServerMonitor();
        ServerMonitorCompliance _MonitorCompliance = new ServerMonitorCompliance();

        IServicoMonitorRisco _gServico;

        System.Threading.Timer _TimerGrid;

        private void Form1_Load(object sender, EventArgs e)
        {
            //_MonitorCompliance.IniciarServico();
            //_MonitorCompliance.StartLoadSuitability(null, false);
            //_MonitorCompliance.StartLoadComplianceChurning(null, false);
            //_MonitorCompliance.StartLoadSuitability(null, true);
            //_MonitorCompliance.StartLoadComplianceChurningAvulso(new DateTime(2014, 6, 3));

            //_Monitor.TotalCustodiaMonitorIntradiario(47787, 47787);
           // _gServico = Ativador.Get<IServicoMonitorRisco>();   
            //_Monitor._bKeepRunning = true;
            //_Monitor
            _Monitor.StartMonitor(null);
            //_Monitor.ObterCotacaoAbertura();
            //_Monitor.ObterItemsMonitor();
            //_MonitorCompliance.CarregarMonitoresMemoria(new object());

            //  _Monitor.ObterRelacaoPLD(null);

        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MonitorLucroPrejuizoRequest _request = new MonitorLucroPrejuizoRequest();

            //40511
            //_request.Semaforo = EnumSemaforo.SEMINFORMACAO;        
           
            if (txtCliente.Text != string.Empty)
            {
                _request.Cliente= int.Parse(txtCliente.Text);
                //_request.Assessor = 403;
            }

            var item = _Monitor.ObterMonitorLucroPrejuizo(_request);

        }

        private void AtualizaGrid(object DataSource)
        { //140192

            this.Invoke(new MethodInvoker(delegate()
              {
                  MonitorLucroPrejuizoRequest _request = new MonitorLucroPrejuizoRequest();

                  if (txtCliente.Text != string.Empty){
                      _request.Cliente = int.Parse(txtCliente.Text);
                      //_request.Assessor = 403;
                      //_request.CodigoLogin = 101743;
                  }

                  MonitorLucroPrejuizoResponse _response =
                      _Monitor.ObterMonitorLucroPrejuizo(_request);

                  dwMonitor.DataSource = 
                      _response.Monitor; // RESPONSE.MONITOR -> Monitor Principal contemplando todos os clientes e/ou filtros

                  if (_response.Monitor.Count > 0)
                  {
                      dgResumo.DataSource = _response.Monitor[0].Operacoes; // GRID CONSOLIDADA POR PAPEL ( RESUMO DE CADA PAPEL OPERADO )
                      dwOperacoes.DataSource = _response.Monitor[0].OrdensExecutadas; // PARTE INTEGRANTE DA GRID CONSOLIDADA, DESCEVENDO OPERACAO A OPERACAO 
                      dwBmf.DataSource = _response.Monitor[0].OrdensBMF;
                  }

                  this.FormatarValores();

                  dtatualizacao.Text = "Data da ultima atualização: " + DateTime.Now.ToString();

              }));

        }

        private void btnBuscarClientes_Click(object sender, EventArgs e)
        {  
        
        }

        private void FormatarValores()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    #region Grid principal dwMonitor


                    if (dwMonitor.RowCount > 0)
                    {
                        dwMonitor.Columns[7].DefaultCellStyle.Format = "C2";
                        dwMonitor.Columns[6].DefaultCellStyle.Format = "C2";

                        dwMonitor.Columns[8].DefaultCellStyle.Format = "C2";
                        dwMonitor.Columns[9].DefaultCellStyle.Format = "C2";
                        dwMonitor.Columns[11].DefaultCellStyle.Format = "C2";

                        dwMonitor.Columns[10].DefaultCellStyle.Format = "C2";

                        for (int i = 0; i <= dwMonitor.Rows.Count - 1; i++)
                        {
                            DataGridViewRow col = dwMonitor.Rows[i];

                            decimal valor = decimal.Parse((col.Cells[10].Value.ToString()));

                            if (valor < 0)
                            {
                                col.DefaultCellStyle.ForeColor = Color.Red;
                            }
                            else if (valor > 0)
                            {
                                col.DefaultCellStyle.ForeColor = Color.Green;
                            }
                        }
                    }

                    #endregion

                    #region Grid Resumo
                    //3 ,5,12,13
                    if (dgResumo.RowCount > 0)
                    {
                        dgResumo.Columns[2].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[3].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[4].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[5].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[6].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[7].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[12].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[13].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[14].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[16].DefaultCellStyle.Format = "C2";
                        dgResumo.Columns[17].DefaultCellStyle.Format = "C2";

                        for (int i = 0; i <= dgResumo.Rows.Count - 1; i++)
                        {
                            DataGridViewRow col = dgResumo.Rows[i];

                            decimal valor = decimal.Parse((col.Cells[3].Value.ToString()));

                            if (valor < 0)
                            {
                                col.DefaultCellStyle.ForeColor = Color.Red;
                            }
                            else if (valor > 0)
                            {
                                col.DefaultCellStyle.ForeColor = Color.Green;
                            }
                        }

                    }

                    #endregion

                    #region Grid Operacoes

                    if (dwOperacoes.RowCount > 0)
                    {
                        dwOperacoes.Columns[4].DefaultCellStyle.Format = "C2";
                        dwOperacoes.Columns[5].DefaultCellStyle.Format = "C2";
                        dwOperacoes.Columns[6].DefaultCellStyle.Format = "C2";
                        dwOperacoes.Columns[7].DefaultCellStyle.Format = "C2";

                        for (int i = 0; i <= dwOperacoes.Rows.Count - 1; i++)
                        {
                            DataGridViewRow col = dwOperacoes.Rows[i];

                            decimal valor = decimal.Parse((col.Cells[7].Value.ToString()));

                            if (valor < 0)
                            {
                                col.DefaultCellStyle.ForeColor = Color.Red;
                            }
                            else if (valor > 0)
                            {
                                col.DefaultCellStyle.ForeColor = Color.Green;
                            }
                        }

                    }

                    #endregion

                    #region Grid BMF

                    if (dwBmf.RowCount > 0)
                    {

                        //4,5,6,8
                        //dwBmf.Columns[4].DefaultCellStyle.Format = "C2";
                        //dwBmf.Columns[5].DefaultCellStyle.Format = "C2";
                        //dwBmf.Columns[6].DefaultCellStyle.Format = "C2";
                        //dwBmf.Columns[8].DefaultCellStyle.Format = "C2";

                        for (int i = 0; i <= dwBmf.Rows.Count - 1; i++)
                        {
                            DataGridViewRow col = dwBmf.Rows[i];

                            decimal valor = decimal.Parse((col.Cells[8].Value.ToString()));

                            if (valor < 0)
                            {
                                col.DefaultCellStyle.ForeColor = Color.Red;
                            }
                            else if (valor > 0)
                            {
                                col.DefaultCellStyle.ForeColor = Color.Green;
                            }
                        }

                    }

                    #endregion

                }));
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                AtualizaGrid(null);
            }));
        }
    }
}


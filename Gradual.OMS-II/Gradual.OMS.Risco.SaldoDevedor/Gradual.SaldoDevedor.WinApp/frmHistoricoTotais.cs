using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Gradual.SaldoDevedor.lib;
using Gradual.OMS.Library.Servicos;
using Gradual.SaldoDevedor.lib.Mensagens;
using Gradual.SaldoDevedor.lib.Info;

namespace Gradual.SaldoDevedor.WinApp
{
    public partial class frmHistoricoTotais : GradualForm.GradualForm
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFormatProvider culture = new System.Globalization.CultureInfo("pt-BR", true);

        private List<InformacoesClienteInfo> ListaTotais = new List<InformacoesClienteInfo>();
        private List<int> ListaExcecaoAssessor = new List<int>();
        private List<int> ListaExcecaoCliente = new List<int>();

        private string AssesssoresExcluidosTotalSaldoHistorico
        {
            get
            {
                if (ConfigurationManager.AppSettings["AssesssoresExcluidosTotalSaldoHistorico"] == null)
                    return "251;252;253";
                return ConfigurationManager.AppSettings["AssesssoresExcluidosTotalSaldoHistorico"];
            }
        }

        public frmHistoricoTotais()
        {
            InitializeComponent();
            GradualForm.StyleSettings.CarregarSkin("Gradual.Consulta");
            GradualForm.Engine.ConfigureFormRender(this);

            CreateColumns();
            ObterParametros();
        }

        private void CreateColumns()
        {
            grdHistoricoTotais.ScrollBars = ScrollBars.Vertical;
            grdHistoricoTotais.VirtualMode = true;
            grdHistoricoTotais.RowTemplate.DefaultCellStyle.Font = new Font("Calibri", (float)12.0, FontStyle.Regular, GraphicsUnit.Pixel);
            grdHistoricoTotais.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", (float)11.0, FontStyle.Bold, GraphicsUnit.Pixel);
            grdHistoricoTotais.ForeColor = Color.WhiteSmoke;
            grdHistoricoTotais.BackgroundColor = Color.FromArgb(32, 32, 32);

            DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
            grdHistoricoTotais.Columns.Add("Data", "Data");
            grdHistoricoTotais.Columns.Add("Total", "Total");

            grdHistoricoTotais.Columns["Data"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdHistoricoTotais.Columns["Total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdHistoricoTotais.Columns["Data"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdHistoricoTotais.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdHistoricoTotais.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdHistoricoTotais.AllowUserToAddRows = false;
            grdHistoricoTotais.AllowUserToDeleteRows = false;
            grdHistoricoTotais.AllowUserToResizeRows = false;
            grdHistoricoTotais.EditMode = DataGridViewEditMode.EditProgrammatically;
            grdHistoricoTotais.RowTemplate.Height = 20;

            grdHistoricoTotais.Columns["Data"].Width = (35 * grdHistoricoTotais.Width) / 100;
            grdHistoricoTotais.Columns["Total"].Width = (54 * grdHistoricoTotais.Width) / 100;
        }

        private void ObterParametros()
        {
            try
            {
                txtDataInicial.Text = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
                txtDataFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");

                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                ParametroResponse parametros = serv.ObterParametros();

                ListaExcecaoAssessor.Clear();
                ListaExcecaoAssessor = parametros.Parametro.ListaExcecaoAssessor;

                List<string> lista = AssesssoresExcluidosTotalSaldoHistorico.Split(';').ToList();
                foreach (string item in lista)
                    if (!ListaExcecaoAssessor.Contains(Convert.ToInt32(item)))
                        ListaExcecaoAssessor.Add(Convert.ToInt32(item));

                ListaExcecaoCliente.Clear();
                ListaExcecaoCliente = parametros.Parametro.ListaExcecaoCliente;
            }
            catch (Exception ex)
            {
                string msg = "Obter Parâmetros: " + ex.Message;
                MessageBox.Show(msg, "Histórico Totais", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                bool result = false;
                DateTime dataInicial;
                DateTime dataFinal;

                result = DateTime.TryParse(txtDataInicial.Text, culture, System.Globalization.DateTimeStyles.None, out dataInicial);
                if (!result)
                {
                    MessageBox.Show("Data inicial inválido. Formato: dd/mm/aaaa", "Histórico Totais", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result = DateTime.TryParse(txtDataFinal.Text, culture, System.Globalization.DateTimeStyles.None, out dataFinal);
                if (!result)
                {
                    MessageBox.Show("Data final inválido. Formato: dd/mm/aaaa", "Histórico Totais", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnConsultar.Enabled = false;
                ListaTotais.Clear();
                grdHistoricoTotais.Rows.Clear();

                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                for (var dia = dataInicial.Date; dia.Date <= dataFinal.Date; dia = dia.AddDays(1))
                {
                    HistoricoRequest request = new HistoricoRequest();
                    request.DataHistorico = dia;

                    HistoricoResponse response = serv.ObterListaHistorico(request);
                    if (response.Lista.Count > 0)
                    {
                        decimal totalSaldo = 0;
                        List<InformacoesClienteInfo> lista = new List<InformacoesClienteInfo>();
                        foreach (InformacoesClienteInfo item in response.Lista)
                        {
                            if (!ListaExcecaoAssessor.Contains(item.CodigoAssessor))
                                if (!ListaExcecaoCliente.Contains(item.CodigoCliente))
                                    totalSaldo += item.SaldoDisponivel;
                        }
                        InformacoesClienteInfo info = new InformacoesClienteInfo();
                        info.DataMovimento = dia;
                        info.SaldoDisponivel = totalSaldo;
                        ListaTotais.Add(info);
                    }
                }
                grdHistoricoTotais.Rows.Clear();
                grdHistoricoTotais.RowCount = ListaTotais.Count;
                grdHistoricoTotais.Invalidate();
                btnConsultar.Enabled = true;
            }
            catch (Exception ex)
            {
                string msg = "Consultar: " + ex.Message;
                MessageBox.Show(msg, "Histórico Totais", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdHistoricoTotais_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (ListaTotais.Count <= 0)
                return;

            InformacoesClienteInfo info = ListaTotais[e.RowIndex];

            switch (grdHistoricoTotais.Columns[e.ColumnIndex].Name)
            {
                case "Data":
                    e.Value = info.DataMovimento.ToString("dd/MM/yyyy");
                    break;
                case "Total":
                    e.Value = Math.Abs(info.SaldoDisponivel).ToString("N2");
                    break;
            }
        }
    }
}

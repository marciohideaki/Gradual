using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.SaldoDevedor.lib.Info;
using System.Configuration;
using Gradual.SaldoDevedor.lib.Mensagens;
using Gradual.SaldoDevedor.lib;
using Gradual.OMS.Library.Servicos;

namespace Gradual.SaldoDevedor.WinApp
{
    public partial class frmHistorico : GradualForm.GradualForm
    {
        private const int IDEMAIL_RISCO = 4;

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFormatProvider culture = new System.Globalization.CultureInfo("pt-BR", true);

        private List<InformacoesClienteInfo> ListaDevedores = new List<InformacoesClienteInfo>();
        private List<int> ListaExcecaoAssessor = new List<int>();
        private List<int> ListaExcecaoCliente = new List<int>();

        private decimal totalSaldo = 0;

        private string EmailTestePara
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailTestePara"] == null)
                    return "";
                return ConfigurationManager.AppSettings["EmailTestePara"];
            }
        }

        private string EmailTesteCopia
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailTesteCopia"] == null)
                    return "";
                return ConfigurationManager.AppSettings["EmailTesteCopia"];
            }
        }

        private string EmailTesteCopiaOculta
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailTesteCopiaOculta"] == null)
                    return "";
                return ConfigurationManager.AppSettings["EmailTesteCopiaOculta"];
            }
        }

        private string AssesssoresExcluidosTotalSaldoHistorico
        {
            get
            {
                if (ConfigurationManager.AppSettings["AssesssoresExcluidosTotalSaldoHistorico"] == null)
                    return "251;252;253";
                return ConfigurationManager.AppSettings["AssesssoresExcluidosTotalSaldoHistorico"];
            }
        }

        public frmHistorico()
        {
            InitializeComponent();
            GradualForm.StyleSettings.CarregarSkin("Gradual.Consulta");
            GradualForm.Engine.ConfigureFormRender(this);

            CreateColumns();
            ObterParametros();
            MontarDiasHistorico();
        }

        private void CreateColumns()
        {
            grdHistorico.ScrollBars = ScrollBars.Vertical;
            grdHistorico.VirtualMode = true;
            grdHistorico.RowTemplate.DefaultCellStyle.Font = new Font("Calibri", (float)12.0, FontStyle.Regular, GraphicsUnit.Pixel);
            grdHistorico.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", (float)11.0, FontStyle.Bold, GraphicsUnit.Pixel);
            grdHistorico.ForeColor = Color.WhiteSmoke;
            grdHistorico.BackgroundColor = Color.FromArgb(32, 32, 32);

            DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
            chkSelecionar.Name = "Sel";
            chkSelecionar.HeaderText = "";
            chkSelecionar.ReadOnly = false;

            grdHistorico.Columns.Add(chkSelecionar);
            grdHistorico.Columns.Add("Data", "Data");
            grdHistorico.Columns.Add("CodigoCliente", "Cliente");
            grdHistorico.Columns.Add("NomeCliente", "Nome Cliente");
            grdHistorico.Columns.Add("CodigoAssessor", "Assessor");
            grdHistorico.Columns.Add("NomeAssessor", "Nome Assessor");
            grdHistorico.Columns.Add("SaldoDisponivel", "Saldo Abertura");
            grdHistorico.Columns.Add("NrDiasNegativo", "Dias Atraso");
            grdHistorico.Columns.Add("SaldoTotal", "Saldo Total");

            grdHistorico.Columns["Sel"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdHistorico.Columns["Data"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdHistorico.Columns["CodigoCliente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdHistorico.Columns["CodigoAssessor"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdHistorico.Columns["SaldoDisponivel"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdHistorico.Columns["NrDiasNegativo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdHistorico.Columns["SaldoTotal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            grdHistorico.Columns["Sel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdHistorico.Columns["Data"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdHistorico.Columns["CodigoCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdHistorico.Columns["CodigoAssessor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdHistorico.Columns["SaldoDisponivel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdHistorico.Columns["NrDiasNegativo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdHistorico.Columns["SaldoTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            grdHistorico.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdHistorico.AllowUserToAddRows = false;
            grdHistorico.AllowUserToDeleteRows = false;
            grdHistorico.AllowUserToResizeRows = false;
            grdHistorico.EditMode = DataGridViewEditMode.EditProgrammatically;
            grdHistorico.RowTemplate.Height = 20;

            grdHistorico.Columns["Sel"].Width = (3 * grdHistorico.Width) / 100;
            grdHistorico.Columns["Data"].Width = (7 * grdHistorico.Width) / 100;
            grdHistorico.Columns["CodigoCliente"].Width = (5 * grdHistorico.Width) / 100;
            grdHistorico.Columns["NomeCliente"].Width = (28 * grdHistorico.Width) / 100;
            grdHistorico.Columns["CodigoAssessor"].Width = (5 * grdHistorico.Width) / 100;
            grdHistorico.Columns["NomeAssessor"].Width = (28 * grdHistorico.Width) / 100;
            grdHistorico.Columns["SaldoDisponivel"].Width = (8 * grdHistorico.Width) / 100;
            grdHistorico.Columns["NrDiasNegativo"].Width = (5 * grdHistorico.Width) / 100;
            grdHistorico.Columns["SaldoTotal"].Width = (9 * grdHistorico.Width) / 100;
        }

        private void ObterParametros()
        {
            try
            {
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

                lblAssessoresExcl.Text = string.Join(", ", ListaExcecaoAssessor);
                lblClientesExcl.Text = string.Join(", ", ListaExcecaoCliente);
            }
            catch (Exception ex)
            {
                string msg = "Obter Parâmetros: " + ex.Message;
                MessageBox.Show(msg, "Histórico Movimento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MontarDiasHistorico()
        {
            try
            {
                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                HistoricoDatasResponse listaDias = serv.ObterListaHistoricoDias();

                treeDiasHistorico.Nodes.Clear();
                foreach (string item in listaDias.Lista)
                {
                    string ano = item.Substring(0, 4);
                    string mes = item.Substring(5, 2);
                    string dia = item.Substring(8, 2);

                    if (!treeDiasHistorico.Nodes.ContainsKey(ano))
                        treeDiasHistorico.Nodes.Add(ano, "Ano: " + ano);

                    if (!treeDiasHistorico.Nodes[ano].Nodes.ContainsKey(ano + mes))
                        treeDiasHistorico.Nodes[ano].Nodes.Add(ano + mes, "Mês: " + mes);

                    if (!treeDiasHistorico.Nodes[ano].Nodes[ano + mes].Nodes.ContainsKey(ano + mes + dia))
                        treeDiasHistorico.Nodes[ano].Nodes[ano + mes].Nodes.Add(ano + mes + dia, "Dia: " + dia);
                }
                btnGerarHistorico.Enabled = false;
            }
            catch (Exception ex)
            {
                string msg = "Obter Dados: " + ex.Message;
                MessageBox.Show(msg, "Histórico Movimento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGerarHistorico_Click(object sender, EventArgs e)
        {
            try
            {
                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                EnviarEmailRequest request = new EnviarEmailRequest();

                EmailResponse dadosEmail = serv.ObterDetalheEmail(IDEMAIL_RISCO);
                request.DadosEmail = dadosEmail.Lista[IDEMAIL_RISCO];

                var listaSelecionados =
                    from InformacoesClienteInfo lstcli in ListaDevedores
                    orderby lstcli.CodigoAssessor, lstcli.CodigoCliente ascending
                    where lstcli.Selecionado == true
                    select lstcli;

                request.ListaClientes = listaSelecionados.ToList();

                if (request.ListaClientes.Count > 0)
                {
                    if (!EmailTestePara.Equals(""))
                        request.DadosEmail.EmailPara = EmailTestePara;
                    if (!EmailTesteCopia.Equals(""))
                        request.DadosEmail.EmailCopia = EmailTesteCopia;
                    if (!EmailTesteCopiaOculta.Equals(""))
                        request.DadosEmail.EmailCopiaOculta = EmailTesteCopiaOculta;

                    EnviarEmailResponse resp = serv.EnviarEmailRisco(request);
                    if (resp.Retorno == EnviarEmailResponse.RETORNO_OK)
                    {
                        MessageBox.Show("E-mail enviado com sucesso!", "Histórico Movimento",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("FALHA NO ENVIO DO E-MAIL!", "Histórico Movimento",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Enviar E-Mail: " + ex.Message;
                MessageBox.Show(msg, "Histórico Movimento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void treeDiasHistorico_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Name.Length == 8)
                {
                    grdHistorico.Rows.Clear();

                    ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                    HistoricoRequest request = new HistoricoRequest();
                    request.DataHistorico = Convert.ToDateTime(
                        e.Node.Name.Substring(6, 2) + "/" + e.Node.Name.Substring(4, 2) + "/" + e.Node.Name.Substring(0, 4), culture);

                    HistoricoResponse response = serv.ObterListaHistorico(request);

                    if (response.Lista.Count > 0)
                    {
                        totalSaldo = 0;
                        ListaDevedores.Clear();
                        foreach (InformacoesClienteInfo item in response.Lista)
                        {
                            item.Selecionado = true;
                            ListaDevedores.Add(item);
                            if (!ListaExcecaoAssessor.Contains(item.CodigoAssessor))
                                if (!ListaExcecaoCliente.Contains(item.CodigoCliente))
                                    totalSaldo += item.SaldoDisponivel;
                        }

                        grdHistorico.Rows.Clear();
                        grdHistorico.RowCount = ListaDevedores.Count;
                        grdHistorico.Invalidate();

                        var listaSelecionados =
                            from InformacoesClienteInfo lstcli in ListaDevedores
                            orderby lstcli.CodigoAssessor, lstcli.CodigoCliente ascending
                            where lstcli.Selecionado == true
                            select lstcli;

                        if (listaSelecionados.ToList().Count == 0)
                            btnGerarHistorico.Enabled = false;
                        else
                            btnGerarHistorico.Enabled = true;

                        lblQtdDevedores.Text = listaSelecionados.ToList().Count + " / " + ListaDevedores.Count;
                        txtTotalSaldo.Text = totalSaldo.ToString("N2", culture).Replace('-', ' ').Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Lista Histórico: " + ex.Message;
                MessageBox.Show(msg, "Histórico Movimento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdHistorico_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (ListaDevedores.Count <= 0)
                return;

            InformacoesClienteInfo info = ListaDevedores[e.RowIndex];

            switch (grdHistorico.Columns[e.ColumnIndex].Name)
            {
                case "Sel":
                    e.Value = info.Selecionado;
                    break;
                case "Data":
                    e.Value = info.DataMovimento.ToString("dd/MM/yyyy");
                    break;
                case "CodigoAssessor":
                    e.Value = info.CodigoAssessor + "  ";
                    break;
                case "NomeCliente":
                    e.Value = info.NomeCliente;
                    break;
                case "CodigoCliente":
                    e.Value = info.CodigoCliente + "  ";
                    break;
                case "NomeAssessor":
                    e.Value = info.NomeAssessor;
                    break;
                case "SaldoDisponivel":
                    e.Value = info.SaldoDisponivel.ToString("N2");
                    break;
                case "NrDiasNegativo":
                    e.Value = info.NrDiasNegativo;
                    break;
                case "SaldoTotal":
                    e.Value = info.SaldoTotal.ToString("N2");
                    break;
            }
        }

        private void grdHistorico_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;

            if (e.ColumnIndex == grdHistorico.Columns["SaldoTotal"].Index ||
                e.ColumnIndex == grdHistorico.Columns["SaldoDisponivel"].Index)
            {
                if (e.Value.ToString().Contains('-'))
                    e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor = Color.FromArgb(255, 104, 114);
                else
                    e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor = Color.FromArgb(0, 158, 15);
            }

            if (e.ColumnIndex == grdHistorico.Columns["Data"].Index ||
                e.ColumnIndex == grdHistorico.Columns["CodigoAssessor"].Index ||
                e.ColumnIndex == grdHistorico.Columns["NomeAssessor"].Index ||
                e.ColumnIndex == grdHistorico.Columns["CodigoCliente"].Index ||
                e.ColumnIndex == grdHistorico.Columns["NomeCliente"].Index)
            {
                foreach (int assessor in ListaExcecaoAssessor)
                {
                    if (ListaDevedores[e.RowIndex].CodigoAssessor == assessor)
                        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor = Color.Gray;
                    //else
                        //e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor = Color.White;
                }
                foreach (int cliente in ListaExcecaoCliente)
                {
                    if (ListaDevedores[e.RowIndex].CodigoCliente == cliente)
                        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor = Color.Gray;
                    //else
                        //e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor = Color.White;
                }
            }
        }

        private void grdHistorico_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ListaDevedores[e.RowIndex].Selecionado = !ListaDevedores[e.RowIndex].Selecionado;

            var listaSelecionados =
                from InformacoesClienteInfo lstcli in ListaDevedores
                orderby lstcli.CodigoAssessor, lstcli.CodigoCliente ascending
                where lstcli.Selecionado == true
                select lstcli;

            if (listaSelecionados.ToList().Count == 0)
                btnGerarHistorico.Enabled = false;
            else
                btnGerarHistorico.Enabled = true;

            totalSaldo = 0;
            foreach (InformacoesClienteInfo item in listaSelecionados)
            {
                if (!ListaExcecaoAssessor.Contains(item.CodigoAssessor))
                    if (!ListaExcecaoCliente.Contains(item.CodigoCliente))
                        totalSaldo += item.SaldoDisponivel;
            }

            lblQtdDevedores.Text = listaSelecionados.ToList().Count + " / " + ListaDevedores.Count;
            txtTotalSaldo.Text = totalSaldo.ToString("N2", culture).Replace('-', ' ').Trim();
        }

        private void txtTotalSaldo_Click(object sender, EventArgs e)
        {
            txtTotalSaldo.Text = String.Format("{0:0.00}", Math.Abs(totalSaldo));
        }

        private void btnTotais_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.Invoke(new MethodInvoker(delegate()
            {
                frmHistoricoTotais historicoTotais = new frmHistoricoTotais();
                historicoTotais.Owner = this;
                historicoTotais.StartPosition = FormStartPosition.CenterParent;
                historicoTotais.ShowDialog();
                historicoTotais.Close();
                this.TopMost = true;
            }));
        }
    }
}

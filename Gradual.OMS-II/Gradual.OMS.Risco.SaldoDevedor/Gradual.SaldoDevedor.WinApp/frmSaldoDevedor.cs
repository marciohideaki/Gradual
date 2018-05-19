using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Gradual.OMS.Library.Servicos;
using Gradual.SaldoDevedor.lib;
using Gradual.SaldoDevedor.lib.Info;
using Gradual.SaldoDevedor.lib.Mensagens;

namespace Gradual.SaldoDevedor.WinApp
{
    public partial class frmSaldoDevedor : GradualForm.GradualForm
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFormatProvider culture = new System.Globalization.CultureInfo("pt-BR", true);

        private List<InformacoesClienteInfo> ListaDevedores = new List<InformacoesClienteInfo>();
        private List<InformacoesClienteInfo> ListaDevedoresFiltrada = new List<InformacoesClienteInfo>();
        private List<int> ListaExcecaoAssessor = new List<int>();
        private List<int> ListaExcecaoCliente = new List<int>();
        private List<string> ListaAssessoresSelecionados = new List<string>();

        private decimal LimiteSaldoDisponivel = 0;
        private decimal TaxaJuros = 0;

        private bool ordemCrescente = false;

        private delegate void OnAtualizaDados(object sender);
        private event OnAtualizaDados OnAtualizaDadosEvent;

        public frmSaldoDevedor()
        {
            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();

            GradualForm.StyleSettings.CarregarSkin("Gradual.Consulta");
            GradualForm.Engine.ConfigureFormRender(this);
            CreateColumns();

            this.OnAtualizaDadosEvent += new OnAtualizaDados(frmSaldoDevedor_OnAtualizaDadosEvent);

            Thread threadAcessarDados = new Thread(new ThreadStart(AcessarDadosPersistencia));
            threadAcessarDados.Start();
        }

        void frmSaldoDevedor_OnAtualizaDadosEvent(object sender)
        {
            this.OnAtualizaDadosEvent -= new OnAtualizaDados(frmSaldoDevedor_OnAtualizaDadosEvent);
            ExibirParametros();
            ExibirListaDevedores();
            lblAguardeDados.Visible = false;
        }

        private void AcessarDadosPersistencia()
        {
            ObterParametros();
            ObterListaDevedores();
            OnAtualizaDadosEvent(this);
        }

        private void CreateColumns()
        {
            grdClientesDevedores.ScrollBars = ScrollBars.Vertical;
            grdClientesDevedores.VirtualMode = true;

            grdClientesDevedores.RowTemplate.DefaultCellStyle.Font = new Font("Calibri", (float)12.0, FontStyle.Regular, GraphicsUnit.Pixel);
            grdClientesDevedores.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", (float)11.0, FontStyle.Bold, GraphicsUnit.Pixel);
            grdClientesDevedores.ForeColor = Color.WhiteSmoke;
            grdClientesDevedores.BackgroundColor = Color.FromArgb(32, 32, 32);

            grdClientesDevedores.Columns.Add("CodigoCliente", "Cliente");
            grdClientesDevedores.Columns.Add("NomeCliente", "Nome Cliente");
            grdClientesDevedores.Columns.Add("CodigoAssessor", "Assessor");
            grdClientesDevedores.Columns.Add("NomeAssessor", "Nome Assessor");
            grdClientesDevedores.Columns.Add("SaldoDisponivel", "Saldo Abertura");
            grdClientesDevedores.Columns.Add("JurosCalculado", "Taxa");
            grdClientesDevedores.Columns.Add("NrDiasNegativo", "Dias Atraso");
            grdClientesDevedores.Columns.Add("SaldoTotal", "Saldo Total");
            grdClientesDevedores.Columns.Add("Desenquadrado", "Desenquadrado");

            grdClientesDevedores.Columns["CodigoCliente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["NomeCliente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdClientesDevedores.Columns["CodigoAssessor"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["NomeAssessor"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdClientesDevedores.Columns["SaldoDisponivel"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["JurosCalculado"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["SaldoTotal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["Desenquadrado"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            grdClientesDevedores.Columns["CodigoCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["CodigoAssessor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["SaldoDisponivel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["JurosCalculado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["NrDiasNegativo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdClientesDevedores.Columns["SaldoTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdClientesDevedores.Columns["Desenquadrado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            grdClientesDevedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdClientesDevedores.AllowUserToAddRows = false;
            grdClientesDevedores.AllowUserToDeleteRows = false;
            grdClientesDevedores.AllowUserToResizeRows = false;
            grdClientesDevedores.EditMode = DataGridViewEditMode.EditProgrammatically;
            grdClientesDevedores.RowTemplate.Height = 20;

            grdClientesDevedores.Rows.Add(50);
        }

        private void ObterParametros()
        {
            try
            {
                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                ParametroResponse parametros = serv.ObterParametros();

                LimiteSaldoDisponivel = parametros.Parametro.LimiteSaldoMulta;

                TaxaJuros = parametros.Parametro.TaxaJuros;

                ListaExcecaoAssessor.Clear();
                ListaExcecaoAssessor = parametros.Parametro.ListaExcecaoAssessor;

                ListaExcecaoCliente.Clear();
                ListaExcecaoCliente = parametros.Parametro.ListaExcecaoCliente;
            }
            catch (Exception ex)
            {
                string msg = "Obter Parâmetros: " + ex.Message;
                MessageBox.Show(msg, "Saldo Devedor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExibirParametros()
        {
            try
            {
                lblLimiteMulta.Text = LimiteSaldoDisponivel.ToString("N2");
                lblTaxaJuros.Text = ((TaxaJuros - 1) * 100).ToString("N0") + "%";
                updwQtdDiasAtraso.Value = 1;
            }
            catch (Exception ex)
            {
                string msg = "Exibir Parâmetros: " + ex.Message;
                MessageBox.Show(msg, "Saldo Devedor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsClienteDevedor(InformacoesClienteInfo cliente)
        {
            if (cliente.SaldoDisponivel > LimiteSaldoDisponivel)
                return false;

            if (cliente.NrDiasNegativo == 0)
                return false;

            return true;
        }

        private void ObterListaDevedores()
        {
            try
            {
                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                InformacaoClienteRequest request = new InformacaoClienteRequest();
                request.DadosCliente = new InformacoesClienteInfo();
                request.DadosCliente.DataMovimento = DateTime.Now;

                InformacaoClienteResponse clientes = serv.ObterClientesSaldoDevedor(request);

                ListaDevedores.Clear();
                foreach (KeyValuePair<int, InformacoesClienteInfo> info in clientes.ListaInformacoesCliente)
                {
                    if (IsClienteDevedor(info.Value))
                        ListaDevedores.Add(info.Value);
                }

                ordemCrescente = true;
            }
            catch (Exception ex)
            {
                string msg = "Obter Lista Devedores: " + ex.Message;
                MessageBox.Show(msg, "Saldo Devedor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExibirListaDevedores()
        {
            try
            {
                chklistAssessores.ItemCheck -= new ItemCheckEventHandler(this.chklistAssessores_ItemCheck);

                chklistAssessores.Items.Clear();
                chklistAssessores.Items.Add("Todos", true);
                foreach (InformacoesClienteInfo info in ListaDevedores)
                    if (!chklistAssessores.Items.Contains(info.CodigoAssessor))
                        chklistAssessores.Items.Add(info.CodigoAssessor, true);

                ListaAssessoresSelecionados.Clear();
                for (int i = 1; i < chklistAssessores.Items.Count; i++)
                    if (chklistAssessores.GetItemChecked(i))
                        ListaAssessoresSelecionados.Add(chklistAssessores.Items[i].ToString());

                chklistAssessores.ItemCheck += new ItemCheckEventHandler(this.chklistAssessores_ItemCheck);

                AtualizaListaFiltrada();

                lblDataHoraAtualizacao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                lblQtdClientes.Text = ListaDevedoresFiltrada.Count + " / " + ListaDevedores.Count;
            }
            catch (Exception ex)
            {
                string msg = "Exibir Lista Devedores: " + ex.Message;
                MessageBox.Show(msg, "Saldo Devedor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdClientesDevedores_Resize(object sender, EventArgs e)
        {
            this.grdClientesDevedores.Columns["CodigoCliente"].Width = (5 * this.grdClientesDevedores.Width) / 100;
            this.grdClientesDevedores.Columns["NomeCliente"].Width = (29 * this.grdClientesDevedores.Width) / 100;
            this.grdClientesDevedores.Columns["CodigoAssessor"].Width = (5 * this.grdClientesDevedores.Width) / 100;
            this.grdClientesDevedores.Columns["NomeAssessor"].Width = (25 * this.grdClientesDevedores.Width) / 100;
            this.grdClientesDevedores.Columns["SaldoDisponivel"].Width = (9 * this.grdClientesDevedores.Width) / 100;
            this.grdClientesDevedores.Columns["JurosCalculado"].Width = (6 * this.grdClientesDevedores.Width) / 100;
            this.grdClientesDevedores.Columns["NrDiasNegativo"].Width = (5 * this.grdClientesDevedores.Width) / 100;
            this.grdClientesDevedores.Columns["SaldoTotal"].Width = (7 * this.grdClientesDevedores.Width) / 100;
            this.grdClientesDevedores.Columns["Desenquadrado"].Width = (8 * this.grdClientesDevedores.Width) / 100;
        }

        private void grdClientesDevedores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;

            if (e.ColumnIndex == grdClientesDevedores.Columns["SaldoTotal"].Index ||
                e.ColumnIndex == grdClientesDevedores.Columns["SaldoDisponivel"].Index ||
                e.ColumnIndex == grdClientesDevedores.Columns["Desenquadrado"].Index)
            {
                if (e.Value.ToString().Contains('-'))
                    e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor = Color.FromArgb(255, 104, 114);
                else
                    e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor = Color.FromArgb(0, 158, 15);
            }

            if (e.ColumnIndex == grdClientesDevedores.Columns["CodigoCliente"].Index ||
                e.ColumnIndex == grdClientesDevedores.Columns["NomeCliente"].Index)
            {
                e.CellStyle.Font = new Font("Calibri", (float)12.0, FontStyle.Bold, GraphicsUnit.Pixel);
            }
        }

        private void grdClientesDevedores_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (ListaDevedoresFiltrada.Count <= 0)
                return;

            InformacoesClienteInfo info = ListaDevedoresFiltrada[e.RowIndex];

            switch (grdClientesDevedores.Columns[e.ColumnIndex].Name)
            {
                case "CodigoCliente":
                    e.Value = info.CodigoCliente + "  ";
                    break;
                case "NomeCliente":
                    e.Value = info.NomeCliente;
                    break;
                case "CodigoAssessor":
                    e.Value = info.CodigoAssessor + "  ";
                    break;
                case "NomeAssessor":
                    e.Value = info.NomeAssessor;
                    break;
                case "SaldoDisponivel":
                    e.Value = info.SaldoDisponivel.ToString("N2");
                    break;
                case "JurosCalculado":
                    e.Value = info.JurosCalculado.ToString("N2").Replace('-', ' ');
                    break;
                case "NrDiasNegativo":
                    e.Value = info.NrDiasNegativo;
                    break;
                case "SaldoTotal":
                    e.Value = info.SaldoTotal.ToString("N2");
                    break;
                case "Desenquadrado":
                    e.Value = info.Desenquadrado.ToString("N2") + "  ";
                    break;
            }
        }

        private void grdClientesDevedores_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ordemCrescente = !ordemCrescente;

            var listaFiltrada =
                from InformacoesClienteInfo lstcli in ListaDevedoresFiltrada
                select lstcli;

            switch (grdClientesDevedores.Columns[e.ColumnIndex].Name)
            {
                case "CodigoCliente":
                    listaFiltrada = (ordemCrescente ?
                        listaFiltrada.OrderBy(x => x.CodigoCliente) :
                        listaFiltrada.OrderByDescending(x => x.CodigoCliente));
                    break;
                case "NomeCliente":
                    listaFiltrada = (ordemCrescente ?
                        listaFiltrada.OrderBy(x => x.NomeCliente) :
                        listaFiltrada.OrderByDescending(x => x.NomeCliente));
                    break;
                case "CodigoAssessor":
                    listaFiltrada = (ordemCrescente ?
                        listaFiltrada.OrderBy(x => x.CodigoAssessor).ThenBy(y => y.CodigoCliente) :
                        listaFiltrada.OrderByDescending(x => x.CodigoAssessor).ThenBy(y => y.CodigoCliente));
                    break;
                case "NomeAssessor":
                    listaFiltrada = (ordemCrescente ?
                        listaFiltrada.OrderBy(x => x.NomeAssessor) :
                        listaFiltrada.OrderByDescending(x => x.NomeAssessor));
                    break;
                case "SaldoDisponivel":
                    listaFiltrada = (ordemCrescente ?
                        listaFiltrada.OrderBy(x => x.SaldoDisponivel) :
                        listaFiltrada.OrderByDescending(x => x.SaldoDisponivel));
                    break;
                case "JurosCalculado":
                    listaFiltrada = (ordemCrescente ?
                        listaFiltrada.OrderBy(x => x.JurosCalculado) :
                        listaFiltrada.OrderByDescending(x => x.JurosCalculado));
                    break;
                case "NrDiasNegativo":
                    listaFiltrada = (ordemCrescente ?
                        listaFiltrada.OrderBy(x => x.NrDiasNegativo) :
                        listaFiltrada.OrderByDescending(x => x.NrDiasNegativo));
                    break;
                case "SaldoTotal":
                    listaFiltrada = (ordemCrescente ?
                        listaFiltrada.OrderBy(x => x.SaldoTotal) :
                        listaFiltrada.OrderByDescending(x => x.SaldoTotal));
                    break;
                case "Desenquadrado":
                    listaFiltrada = (ordemCrescente ?
                        listaFiltrada.OrderBy(x => x.Desenquadrado) :
                        listaFiltrada.OrderByDescending(x => x.Desenquadrado));
                    break;
            }

            ListaDevedoresFiltrada = listaFiltrada.ToList();
            grdClientesDevedores.Rows.Clear();
            grdClientesDevedores.RowCount = ListaDevedoresFiltrada.Count;
            grdClientesDevedores.Invalidate();
        }

        private void chklistAssessores_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            chklistAssessores.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.chklistAssessores_ItemCheck);

            bool selecionado = (e.CurrentValue == CheckState.Checked ? true : false);

            if (e.Index == 0)
            {
                for (int i = 1; i < chklistAssessores.Items.Count; i++)
                    chklistAssessores.SetItemChecked(i, !selecionado);
            }
            chklistAssessores.SetSelected(e.Index, false);

            ListaAssessoresSelecionados.Clear();
            for (int i = 1; i < chklistAssessores.Items.Count; i++)
                if (chklistAssessores.GetItemChecked(i) && i != e.Index)
                    ListaAssessoresSelecionados.Add(chklistAssessores.Items[i].ToString());
            if (!selecionado)
                ListaAssessoresSelecionados.Add(chklistAssessores.Items[e.Index].ToString());

            if (e.Index != 0 && selecionado)
                chklistAssessores.SetItemChecked(0, false);

            AtualizaListaFiltrada();
            chklistAssessores.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chklistAssessores_ItemCheck);
        }

        private void txtCliente_KeyUp(object sender, KeyEventArgs e)
        {
            AtualizaListaFiltrada();
        }

        private void updwQtdDiasAtraso_ValueChanged(object sender, EventArgs e)
        {
            if (updwQtdDiasAtraso.Value < 1)
            {
                updwQtdDiasAtraso.Value = 1;
                return;
            }

            AtualizaListaFiltrada();
        }

        private void chkDesenquadrado_CheckedChanged(object sender, EventArgs e)
        {
            AtualizaListaFiltrada();
        }

        private void AtualizaListaFiltrada()
        {
            var listaFiltrada =
                from InformacoesClienteInfo lstcli in ListaDevedores
                select lstcli;

            listaFiltrada = listaFiltrada.Where(lst => ListaAssessoresSelecionados.Contains(lst.CodigoAssessor.ToString()));
            listaFiltrada = listaFiltrada.Where(lst => lst.NrDiasNegativo >= updwQtdDiasAtraso.Value);
            if (txtCliente.Text.Length > 0)
                listaFiltrada = listaFiltrada.Where(lst => lst.CodigoCliente.ToString().StartsWith(txtCliente.Text));

            if (!chkExcecoes.Checked)
            {
                if (ListaExcecaoAssessor.Count > 0)
                    listaFiltrada = listaFiltrada.Where(lst => !ListaExcecaoAssessor.Contains(lst.CodigoAssessor));
                if (ListaExcecaoCliente.Count > 0)
                    listaFiltrada = listaFiltrada.Where(lst => !ListaExcecaoCliente.Contains(lst.CodigoCliente));
            }

            if (!chkDesenquadrado.Checked)
            {
                listaFiltrada = listaFiltrada.Where(lst => lst.Desenquadrado < 0);
            }

            if (rbValorDesenquadrado1.Checked)
            {
                listaFiltrada = listaFiltrada.Where(lst => lst.Desenquadrado >= -5000);
            }

            if (rbValorDesenquadrado2.Checked)
            {
                listaFiltrada = listaFiltrada.Where(lst => lst.Desenquadrado <= -5000 && lst.Desenquadrado >= -15000);
            }

            if (rbValorDesenquadrado3.Checked)
            {
                listaFiltrada = listaFiltrada.Where(lst => lst.Desenquadrado < -15000);
            }

            ListaDevedoresFiltrada = listaFiltrada.ToList();
            grdClientesDevedores.Rows.Clear();
            grdClientesDevedores.RowCount = ListaDevedoresFiltrada.Count;
            grdClientesDevedores.Invalidate();

            lblQtdClientes.Text = ListaDevedoresFiltrada.Count + " / " + ListaDevedores.Count;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfiguracao_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.Invoke(new MethodInvoker(delegate()
            {
                frmConfiguracao configuracao = new frmConfiguracao();
                configuracao.GravaConfiguracao += new EventHandler<GravaConfiguracaoEventArgs>(configuracao_GravaConfiguracao);
                configuracao.Owner = this;
                configuracao.StartPosition = FormStartPosition.CenterParent;
                configuracao.ShowDialog();
                configuracao.Close();
                this.TopMost = true;
            }));
        }

        void configuracao_GravaConfiguracao(object sender, GravaConfiguracaoEventArgs e)
        {
            ObterParametros();
            ExibirParametros();
            ObterListaDevedores();
            ExibirListaDevedores();
        }

        private void btnNotificarAssessores_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.Invoke(new MethodInvoker(delegate()
            {
                frmNotificarAssessores notificarAssessores = new frmNotificarAssessores(ListaDevedoresFiltrada);
                notificarAssessores.Owner = this;
                notificarAssessores.StartPosition = FormStartPosition.CenterParent;
                notificarAssessores.ShowDialog();
                notificarAssessores.Close();
                this.TopMost = true;
            }));
        }

        private void btnNotificarTesouraria_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.Invoke(new MethodInvoker(delegate()
            {
                frmNotificarTesouraria notificarTesouraria = new frmNotificarTesouraria(ListaDevedoresFiltrada);
                notificarTesouraria.Owner = this;
                notificarTesouraria.StartPosition = FormStartPosition.CenterParent;
                notificarTesouraria.ShowDialog();
                notificarTesouraria.Close();
                this.TopMost = true;
            }));
        }

        private void btnConfigEmail_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.Invoke(new MethodInvoker(delegate()
            {
                frmConfigEmail configuracao = new frmConfigEmail();
                configuracao.Owner = this;
                configuracao.StartPosition = FormStartPosition.CenterParent;
                configuracao.ShowDialog();
                configuracao.Close();
                this.TopMost = true;
            }));
        }

        private void chkExcecoes_CheckedChanged(object sender, EventArgs e)
        {
            AtualizaListaFiltrada();
        }

        private void btnNotificarAtendimento_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.Invoke(new MethodInvoker(delegate()
            {
                frmNotificarAtendimento notificarAtendimento = new frmNotificarAtendimento(ListaDevedoresFiltrada);
                notificarAtendimento.Owner = this;
                notificarAtendimento.StartPosition = FormStartPosition.CenterParent;
                notificarAtendimento.ShowDialog();
                notificarAtendimento.Close();
                this.TopMost = true;
            }));
        }

        private void btnHistorico_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.Invoke(new MethodInvoker(delegate()
            {
                frmHistorico historico = new frmHistorico();
                historico.Owner = this;
                historico.StartPosition = FormStartPosition.CenterParent;
                historico.ShowDialog();
                historico.Close();
                this.TopMost = true;
            }));
        }

        private void rbValorDesenquadrado_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                AtualizaListaFiltrada();
            }
        }
    }
}

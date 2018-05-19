using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Library.Servicos;
using Gradual.SaldoDevedor.lib;
using Gradual.SaldoDevedor.lib.Info;
using Gradual.SaldoDevedor.lib.Mensagens;
using System.Configuration;

namespace Gradual.SaldoDevedor.WinApp
{
    public partial class frmNotificarTesouraria : GradualForm.GradualForm
    {
        private const int IDEMAIL_TESOURARIA = 0;

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFormatProvider culture = new System.Globalization.CultureInfo("pt-BR", true);

        private List<InformacoesClienteInfo> ListaDevedoresFiltrada;

        public frmNotificarTesouraria(List<InformacoesClienteInfo> lista)
        {
            InitializeComponent();
            GradualForm.StyleSettings.CarregarSkin("Gradual.Consulta");
            GradualForm.Engine.ConfigureFormRender(this);

            ListaDevedoresFiltrada = lista;
            CreateColumns();
            MontarLista();
        }

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

        private void CreateColumns()
        {
            grdDevedores.ScrollBars = ScrollBars.Vertical;
            grdDevedores.VirtualMode = true;
            grdDevedores.RowTemplate.DefaultCellStyle.Font = new Font("Calibri", (float)12.0, FontStyle.Regular, GraphicsUnit.Pixel);
            grdDevedores.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", (float)11.0, FontStyle.Bold, GraphicsUnit.Pixel);
            grdDevedores.ForeColor = Color.WhiteSmoke;
            grdDevedores.BackgroundColor = Color.FromArgb(32, 32, 32);

            DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
            chkSelecionar.Name = "Sel";
            chkSelecionar.HeaderText = "";
            chkSelecionar.ReadOnly = false;

            grdDevedores.Columns.Add(chkSelecionar);
            grdDevedores.Columns.Add("CodigoCliente", "Cliente");
            grdDevedores.Columns.Add("NomeCliente", "Nome");
            grdDevedores.Columns.Add("CodigoAssessor", "Assessor");
            grdDevedores.Columns.Add("DataMovimento", "Data");
            grdDevedores.Columns.Add("SaldoDisponivel", "Financiado");
            grdDevedores.Columns.Add("JurosCalculado", "Taxa");

            grdDevedores.Columns["CodigoCliente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["NomeCliente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdDevedores.Columns["CodigoAssessor"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["DataMovimento"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdDevedores.Columns["SaldoDisponivel"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["JurosCalculado"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            grdDevedores.Columns["CodigoCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["CodigoAssessor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["DataMovimento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdDevedores.Columns["SaldoDisponivel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["JurosCalculado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            grdDevedores.Columns["Sel"].Width = (5 * grdDevedores.Width) / 100;
            grdDevedores.Columns["CodigoCliente"].Width = (10 * grdDevedores.Width) / 100;
            grdDevedores.Columns["NomeCliente"].Width = (35 * grdDevedores.Width) / 100;
            grdDevedores.Columns["CodigoAssessor"].Width = (10 * grdDevedores.Width) / 100;
            grdDevedores.Columns["DataMovimento"].Width = (13 * grdDevedores.Width) / 100;
            grdDevedores.Columns["SaldoDisponivel"].Width = (15 * grdDevedores.Width) / 100;
            grdDevedores.Columns["JurosCalculado"].Width = (10 * grdDevedores.Width) / 100;

            grdDevedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdDevedores.AllowUserToAddRows = false;
            grdDevedores.AllowUserToDeleteRows = false;
            grdDevedores.AllowUserToResizeRows = false;
            grdDevedores.EditMode = DataGridViewEditMode.EditProgrammatically;
            grdDevedores.RowTemplate.Height = 18;
        }

        private void MontarLista()
        {
            foreach (InformacoesClienteInfo item in ListaDevedoresFiltrada)
                item.Selecionado = true;

            grdDevedores.Rows.Clear();
            grdDevedores.RowCount = ListaDevedoresFiltrada.Count;
            grdDevedores.Invalidate();

            if (ListaDevedoresFiltrada.Count == 0)
                btnEnviarEmail.Enabled = false;
            else
                btnEnviarEmail.Enabled = true;

            lblQtdDevedores.Text = ListaDevedoresFiltrada.Count + " / " + ListaDevedoresFiltrada.Count;
        }

        private void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                EnviarEmailRequest request = new EnviarEmailRequest();

                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                EmailResponse dadosEmail = serv.ObterDetalheEmail(IDEMAIL_TESOURARIA);
                request.DadosEmail = dadosEmail.Lista[IDEMAIL_TESOURARIA];

                var listaSelecionados =
                    from InformacoesClienteInfo lstcli in ListaDevedoresFiltrada
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

                    EnviarEmailResponse resp = serv.EnviarEmailTesouraria(request);
                    if (resp.Retorno == EnviarEmailResponse.RETORNO_OK)
                    {
                        MessageBox.Show("Notificação enviada com sucesso!", "Notificar Tesouraria",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("FALHA NO ENVIO DA NOTIFICAÇÃO!", "Notificar Tesouraria",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Enviar E-Mail: " + ex.Message;
                MessageBox.Show(msg, "Notificar Tesouraria", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdDevedores_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (ListaDevedoresFiltrada.Count <= 0)
                return;

            InformacoesClienteInfo info = ListaDevedoresFiltrada[e.RowIndex];

            switch (grdDevedores.Columns[e.ColumnIndex].Name)
            {
                case "Sel":
                    e.Value = info.Selecionado;
                    break;
                case "CodigoCliente":
                    e.Value = info.CodigoCliente + "  ";
                    break;
                case "NomeCliente":
                    e.Value = info.NomeCliente;
                    break;
                case "CodigoAssessor":
                    e.Value = info.CodigoAssessor + "  ";
                    break;
                case "DataMovimento":
                    e.Value = info.DataMovimento.ToString("dd/MM/yyyy");
                    break;
                case "SaldoDisponivel":
                    e.Value = info.SaldoDisponivel.ToString("N2").Replace('-', ' ');
                    break;
                case "JurosCalculado":
                    e.Value = info.JurosCalculado.ToString("N2").Replace('-', ' ');
                    break;
            }
        }

        private void grdDevedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ListaDevedoresFiltrada[e.RowIndex].Selecionado = !ListaDevedoresFiltrada[e.RowIndex].Selecionado;

            var listaSelecionados =
                from InformacoesClienteInfo lstcli in ListaDevedoresFiltrada
                orderby lstcli.CodigoAssessor, lstcli.CodigoCliente ascending
                where lstcli.Selecionado == true
                select lstcli;

            if (listaSelecionados.ToList().Count == 0)
                btnEnviarEmail.Enabled = false;
            else
                btnEnviarEmail.Enabled = true;

            lblQtdDevedores.Text = listaSelecionados.ToList().Count + " / " + ListaDevedoresFiltrada.Count;
        }
    }
}

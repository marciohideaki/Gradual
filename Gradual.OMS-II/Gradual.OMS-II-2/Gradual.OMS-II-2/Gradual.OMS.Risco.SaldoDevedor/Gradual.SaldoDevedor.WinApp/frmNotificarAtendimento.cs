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
    public partial class frmNotificarAtendimento : GradualForm.GradualForm
    {
        private const int IDEMAIL_ATENDIMENTO = 2;

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFormatProvider culture = new System.Globalization.CultureInfo("pt-BR", true);

        private List<InformacoesClienteInfo> ListaDevedoresFiltrada;

        int notificarAtendimentoDiasAlternados = 0;
        private List<string> ListaAssessoresNotificadosDiasAlternados;

        public frmNotificarAtendimento(List<InformacoesClienteInfo> lista)
        {
            InitializeComponent();
            GradualForm.StyleSettings.CarregarSkin("Gradual.Consulta");
            GradualForm.Engine.ConfigureFormRender(this);

            ListaDevedoresFiltrada = lista;
            ObterParametros();
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

        private string AssessoresNotificadosDiasAlternados
        {
            get
            {
                if (ConfigurationManager.AppSettings["AssessoresNotificadosDiasAlternados"] == null)
                    return "251;253";
                return ConfigurationManager.AppSettings["AssessoresNotificadosDiasAlternados"];
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
            grdDevedores.Columns.Add("CodigoAssessor", "Assessor");
            grdDevedores.Columns.Add("CodigoCliente", "Cliente");
            grdDevedores.Columns.Add("NomeCliente", "Nome");
            grdDevedores.Columns.Add("Email", "Email");

            grdDevedores.Columns["CodigoAssessor"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["CodigoCliente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["NomeCliente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdDevedores.Columns["Email"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            grdDevedores.Columns["CodigoAssessor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["CodigoCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdDevedores.Columns["NomeCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdDevedores.Columns["Email"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            grdDevedores.Columns["Sel"].Width = (5 * grdDevedores.Width) / 100;
            grdDevedores.Columns["CodigoAssessor"].Width = (10 * grdDevedores.Width) / 100;
            grdDevedores.Columns["CodigoCliente"].Width = (10 * grdDevedores.Width) / 100;
            grdDevedores.Columns["NomeCliente"].Width = (37 * grdDevedores.Width) / 100;
            grdDevedores.Columns["Email"].Width = (35 * grdDevedores.Width) / 100;

            grdDevedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdDevedores.AllowUserToAddRows = false;
            grdDevedores.AllowUserToDeleteRows = false;
            grdDevedores.AllowUserToResizeRows = false;
            grdDevedores.EditMode = DataGridViewEditMode.EditProgrammatically;
            grdDevedores.RowTemplate.Height = 18;
        }

        private void ObterParametros()
        {
            try
            {
                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                ParametroResponse parametros = serv.ObterParametroNotificarAtendimentoDiasAlternados();

                notificarAtendimentoDiasAlternados = parametros.Parametro.NotificarAtendimentoDiasAlternados;

                ListaAssessoresNotificadosDiasAlternados = AssessoresNotificadosDiasAlternados.Split(';').ToList();
            }
            catch (Exception ex)
            {
                string msg = "Obter Parâmetros: " + ex.Message;
                MessageBox.Show(msg, "Notificar Atendimento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MontarLista()
        {
            foreach (InformacoesClienteInfo item in ListaDevedoresFiltrada)
            {
                item.Selecionado = true;
                if (notificarAtendimentoDiasAlternados == 0)
                {
                    foreach (string assessor in ListaAssessoresNotificadosDiasAlternados)
                        if (item.CodigoAssessor == Convert.ToInt32(assessor))
                            item.Selecionado = false;
                }
            }

            grdDevedores.Rows.Clear();
            grdDevedores.RowCount = ListaDevedoresFiltrada.Count;
            grdDevedores.Invalidate();

            if (ListaDevedoresFiltrada.Count == 0)
                btnEnviarEmail.Enabled = false;
            else
                btnEnviarEmail.Enabled = true;

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

        private void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                EnviarEmailRequest request = new EnviarEmailRequest();

                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                EmailResponse dadosEmail = serv.ObterDetalheEmail(IDEMAIL_ATENDIMENTO);
                request.DadosEmail = dadosEmail.Lista[IDEMAIL_ATENDIMENTO];

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

                    EnviarEmailResponse resp = serv.EnviarEmailAtendimento(request);
                    if (resp.Retorno == EnviarEmailResponse.RETORNO_OK)
                    {
                        ParametroRequest req = new ParametroRequest();
                        req.Parametro = new ParametroInfo();
                        if (notificarAtendimentoDiasAlternados == ParametroRequest.NAO_NOTIFICAR_ATENDIMENTO_DIAS_ALTERNADOS)
                            req.Parametro.NotificarAtendimentoDiasAlternados = ParametroRequest.NOTIFICAR_ATENDIMENTO_DIAS_ALTERNADOS;
                        else
                            req.Parametro.NotificarAtendimentoDiasAlternados = ParametroRequest.NAO_NOTIFICAR_ATENDIMENTO_DIAS_ALTERNADOS;

                        serv.GravarParametroNotificarAtendimentoDiasAlternados(req);

                        MessageBox.Show("Notificação enviada com sucesso!", "Notificar Atendimento",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("FALHA NO ENVIO DA NOTIFICAÇÃO!", "Notificar Atendimento",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Enviar E-Mail: " + ex.Message;
                MessageBox.Show(msg, "Notificar Atendimento", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                case "CodigoAssessor":
                    e.Value = info.CodigoAssessor + "  ";
                    break;
                case "CodigoCliente":
                    e.Value = info.CodigoCliente + "  ";
                    break;
                case "NomeCliente":
                    e.Value = info.NomeCliente;
                    break;
                case "Email":
                    e.Value = info.EmailCliente;
                    break;
            }
        }

        private void grdDevedores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;

            if (e.ColumnIndex == grdDevedores.Columns["CodigoAssessor"].Index ||
                e.ColumnIndex == grdDevedores.Columns["CodigoCliente"].Index ||
                e.ColumnIndex == grdDevedores.Columns["NomeCliente"].Index ||
                e.ColumnIndex == grdDevedores.Columns["Email"].Index)
            {
                if (notificarAtendimentoDiasAlternados == 0)
                {
                    foreach (string assessor in ListaAssessoresNotificadosDiasAlternados)
                        if (ListaDevedoresFiltrada[e.RowIndex].CodigoAssessor == Convert.ToInt32(assessor))
                            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor = Color.Gray;
                }
            }
        }

        private void grdDevedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (notificarAtendimentoDiasAlternados == 0)
            {
                foreach (string assessor in ListaAssessoresNotificadosDiasAlternados)
                    if (ListaDevedoresFiltrada[e.RowIndex].CodigoAssessor == Convert.ToInt32(assessor))
                        return;
            }

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

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

namespace Gradual.SaldoDevedor.WinApp
{
    public partial class frmConfigEmail : GradualForm.GradualForm
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFormatProvider culture = new System.Globalization.CultureInfo("pt-BR", true);

        private List<int> ListaAssessorEmailExcluido = new List<int>();
        private DataTable ComboIdTextoEmail;
        private DataTable ComboDescricaoEmail;

        public event EventHandler<GravaConfiguracaoEventArgs> GravaConfigEmail;

        public frmConfigEmail()
        {
            InitializeComponent();
            GradualForm.StyleSettings.CarregarSkin("Gradual.Consulta");
            GradualForm.Engine.ConfigureFormRender(this);

            CreateColumns();
            ObterParametros();
        }

        private void OnGravaConfigEmail()
        {
            if (GravaConfigEmail != null)
            {
                GravaConfigEmail(this, new GravaConfiguracaoEventArgs());
            }
        }

        private void CreateColumns()
        {
            ComboIdTextoEmail = new DataTable();
            ComboIdTextoEmail.Columns.Add("Key");
            ComboIdTextoEmail.Columns.Add("Value");

            ComboDescricaoEmail = new DataTable();
            ComboDescricaoEmail.Columns.Add("Key");
            ComboDescricaoEmail.Columns.Add("Value");

            grdTextoEmail.ScrollBars = ScrollBars.Vertical;
            grdTextoEmail.RowTemplate.DefaultCellStyle.Font = new Font("Calibri", (float)12.0, FontStyle.Regular, GraphicsUnit.Pixel);
            grdTextoEmail.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", (float)12.0, FontStyle.Bold, GraphicsUnit.Pixel);
            grdTextoEmail.ForeColor = Color.WhiteSmoke;
            grdTextoEmail.BackgroundColor = Color.FromArgb(32, 32, 32);

            grdTextoEmail.Columns.Add("IdTextoEmail", "Id");
            grdTextoEmail.Columns.Add("TextoEmail", "Texto E-mail");
            grdTextoEmail.Columns.Add("DataAtualizacao", "Data");

            grdTextoEmail.Columns["IdTextoEmail"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdTextoEmail.Columns["DataAtualizacao"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdTextoEmail.Columns["IdTextoEmail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdTextoEmail.Columns["DataAtualizacao"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdTextoEmail.Columns["IdTextoEmail"].ReadOnly = true;
            grdTextoEmail.Columns["IdTextoEmail"].Selected = false;
            grdTextoEmail.Columns["DataAtualizacao"].ReadOnly = true;
            grdTextoEmail.Columns["DataAtualizacao"].Selected = false;

            grdTextoEmail.Columns["TextoEmail"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grdTextoEmail.RowTemplate.Height = 30;

            grdTextoEmail.Columns["IdTextoEmail"].Width = (5 * grdTextoEmail.Width) / 100;
            grdTextoEmail.Columns["TextoEmail"].Width = (78 * grdTextoEmail.Width) / 100;
            grdTextoEmail.Columns["DataAtualizacao"].Width = (15 * grdTextoEmail.Width) / 100;

            grdTextoEmail.AllowUserToAddRows = false;
            grdTextoEmail.AllowUserToDeleteRows = false;
            grdTextoEmail.AllowUserToResizeRows = false;

            grdEmail.ScrollBars = ScrollBars.Vertical;
            grdEmail.RowTemplate.DefaultCellStyle.Font = new Font("Calibri", (float)10.0, FontStyle.Regular, GraphicsUnit.Pixel);
            grdEmail.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", (float)10.0, FontStyle.Bold, GraphicsUnit.Pixel);
            grdEmail.ForeColor = Color.WhiteSmoke;
            grdEmail.BackgroundColor = Color.FromArgb(32, 32, 32);

            grdEmail.Columns.Add("IdEmail", "Id");
            grdEmail.Columns.Add("Descricao", "Descrição");
            grdEmail.Columns.Add("Titulo", "Introdução");
            grdEmail.Columns.Add("Assunto", "Assunto e-mail");
            grdEmail.Columns.Add("Para", "Para");
            grdEmail.Columns.Add("Copia", "Cópia");
            grdEmail.Columns.Add("CopiaOculta", "Cópia oculta");
            DataGridViewComboBoxColumn cmbTextoEmail = new DataGridViewComboBoxColumn();
            cmbTextoEmail.Name = "IdTextoEmail";
            cmbTextoEmail.HeaderText = "Corpo";
            cmbTextoEmail.FlatStyle = FlatStyle.Popup;
            cmbTextoEmail.DataSource = ComboIdTextoEmail;
            cmbTextoEmail.DisplayMember = "Value";
            cmbTextoEmail.ValueMember = "Key";
            grdEmail.Columns.Add(cmbTextoEmail);
            grdEmail.Columns.Add("DataAtualizacao", "Data");

            grdEmail.Columns["IdEmail"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdEmail.Columns["DataAtualizacao"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdEmail.Columns["IdEmail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdEmail.Columns["DataAtualizacao"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdEmail.Columns["IdEmail"].ReadOnly = true;
            grdEmail.Columns["IdEmail"].Selected = false;
            grdEmail.Columns["DataAtualizacao"].ReadOnly = true;
            grdEmail.Columns["DataAtualizacao"].Selected = false;
            grdEmail.RowTemplate.Height = 18;

            grdEmail.Columns["IdEmail"].Width = (4 * grdEmail.Width) / 100;
            grdEmail.Columns["Descricao"].Width = (14 * grdEmail.Width) / 100;
            grdEmail.Columns["Titulo"].Width = (15 * grdEmail.Width) / 100;
            grdEmail.Columns["Assunto"].Width = (14 * grdEmail.Width) / 100;
            grdEmail.Columns["Para"].Width = (13 * grdEmail.Width) / 100;
            grdEmail.Columns["Copia"].Width = (13 * grdEmail.Width) / 100;
            grdEmail.Columns["CopiaOculta"].Width = (10 * grdEmail.Width) / 100;
            grdEmail.Columns["IdTextoEmail"].Width = (6 * grdEmail.Width) / 100;
            grdEmail.Columns["DataAtualizacao"].Width = (10 * grdEmail.Width) / 100;

            grdEmail.AllowUserToAddRows = false;
            grdEmail.AllowUserToDeleteRows = false;
            grdEmail.AllowUserToResizeRows = false;

            grdAssessorEmail.ScrollBars = ScrollBars.Vertical;
            grdAssessorEmail.RowTemplate.DefaultCellStyle.Font = new Font("Calibri", (float)11.0, FontStyle.Regular, GraphicsUnit.Pixel);
            grdAssessorEmail.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", (float)11.0, FontStyle.Bold, GraphicsUnit.Pixel);
            grdAssessorEmail.ForeColor = Color.WhiteSmoke;
            grdAssessorEmail.BackgroundColor = Color.FromArgb(32, 32, 32);

            grdAssessorEmail.Columns.Add("CodigoAssessor", "Assessor");
            DataGridViewComboBoxColumn cmbDescricaoEmail = new DataGridViewComboBoxColumn();
            cmbDescricaoEmail.Name = "DescricaoEmail";
            cmbDescricaoEmail.HeaderText = "E-mail";
            cmbDescricaoEmail.FlatStyle = FlatStyle.Popup;
            cmbDescricaoEmail.DataSource = ComboDescricaoEmail;
            cmbDescricaoEmail.DisplayMember = "Value";
            cmbDescricaoEmail.ValueMember = "Key";
            grdAssessorEmail.Columns.Add(cmbDescricaoEmail);
            grdAssessorEmail.Columns.Add("DataAtualizacao", "Data");

            grdAssessorEmail.Columns["CodigoAssessor"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdAssessorEmail.Columns["DataAtualizacao"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdAssessorEmail.Columns["CodigoAssessor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdAssessorEmail.Columns["DataAtualizacao"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdAssessorEmail.Columns["DataAtualizacao"].ReadOnly = true;
            grdAssessorEmail.Columns["DataAtualizacao"].Selected = false;
            grdAssessorEmail.RowTemplate.Height = 18;

            grdAssessorEmail.Columns["CodigoAssessor"].Width = (15 * grdAssessorEmail.Width) / 100;
            grdAssessorEmail.Columns["DescricaoEmail"].Width = (52 * grdAssessorEmail.Width) / 100;
            grdAssessorEmail.Columns["DataAtualizacao"].Width = (30 * grdAssessorEmail.Width) / 100;

            grdAssessorEmail.AllowUserToAddRows = false;
            grdAssessorEmail.AllowUserToDeleteRows = false;
            grdAssessorEmail.AllowUserToResizeRows = false;
        }

        private void ObterParametros()
        {
            try
            {
                grdTextoEmail.CellValueChanged -= new DataGridViewCellEventHandler(grdTextoEmail_CellValueChanged);
                grdAssessorEmail.CellValueChanged -= new DataGridViewCellEventHandler(grdAssessorEmail_CellValueChanged);
                grdEmail.CellValueChanged -= new DataGridViewCellEventHandler(grdEmail_CellValueChanged);

                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                TextoEmailResponse listaTextoEmail = serv.ObterListaTextoEmail();

                grdTextoEmail.Rows.Clear();
                ComboIdTextoEmail.Clear();
                foreach (KeyValuePair<int, TextoEmailInfo> item in listaTextoEmail.Lista)
                {
                    grdTextoEmail.Rows.Add(item.Value.Id, item.Value.Texto, item.Value.DataAtualizacao);
                    ComboIdTextoEmail.Rows.Add(item.Value.Id.ToString(), item.Value.Id.ToString());
                }

                EmailResponse listaEmail = serv.ObterListaDetalheEmail();

                grdEmail.Rows.Clear();
                ComboDescricaoEmail.Clear();
                foreach (KeyValuePair<int, EmailInfo> item in listaEmail.Lista)
                {
                    int reg = grdEmail.Rows.Add(new DataGridViewRow());
                    grdEmail.Rows[reg].Cells["IdEmail"].Value = item.Value.Id;
                    grdEmail.Rows[reg].Cells["Descricao"].Value = item.Value.Descricao;
                    grdEmail.Rows[reg].Cells["Titulo"].Value = item.Value.Titulo;
                    grdEmail.Rows[reg].Cells["Assunto"].Value = item.Value.Assunto;
                    grdEmail.Rows[reg].Cells["Para"].Value = item.Value.EmailPara;
                    grdEmail.Rows[reg].Cells["Copia"].Value = item.Value.EmailCopia;
                    grdEmail.Rows[reg].Cells["CopiaOculta"].Value = item.Value.EmailCopiaOculta;
                    grdEmail.Rows[reg].Cells["IdTextoEmail"].Value = item.Value.IdTextoEmail.ToString();
                    grdEmail.Rows[reg].Cells["DataAtualizacao"].Value = item.Value.DataAtualizacao;
                    ComboDescricaoEmail.Rows.Add(item.Value.Id, item.Value.Descricao);
                }

                AssessorEmailResponse listaAssessorEmail = serv.ObterListaAssessorEmail();

                grdAssessorEmail.Rows.Clear();
                foreach (AssessorEmailInfo item in listaAssessorEmail.Lista)
                {
                    if (item.IdAssessor == 0)
                        continue;
                    int reg = grdAssessorEmail.Rows.Add(new DataGridViewRow());
                    grdAssessorEmail.Rows[reg].Cells["CodigoAssessor"].Value = item.IdAssessor;
                    grdAssessorEmail.Rows[reg].Cells["DescricaoEmail"].Value = item.IdEmail.ToString();
                    grdAssessorEmail.Rows[reg].Cells["DataAtualizacao"].Value = item.DataAtualizacao;
                }

                grdTextoEmail.CellValueChanged += new DataGridViewCellEventHandler(grdTextoEmail_CellValueChanged);
                grdAssessorEmail.CellValueChanged += new DataGridViewCellEventHandler(grdAssessorEmail_CellValueChanged);
                grdEmail.CellValueChanged += new DataGridViewCellEventHandler(grdEmail_CellValueChanged);
            }
            catch (Exception ex)
            {
                string msg = "Obter Parâmetros: " + ex.Message;
                MessageBox.Show(msg, "Configuração E-mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdTextoEmail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (grdTextoEmail.Columns[e.ColumnIndex].Name)
            {
                case "IdTextoEmail":
                    e.CellStyle.ForeColor = Color.Gray;
                    e.CellStyle.SelectionForeColor = Color.Gray;
                    break;
                case "TextoEmail":
                    e.CellStyle.Font = new Font("Ariel", (float)10.0, FontStyle.Regular, GraphicsUnit.Pixel);
                    break;
                case "DataAtualizacao":
                    e.CellStyle.ForeColor = Color.Gray;
                    e.CellStyle.SelectionForeColor = Color.Gray;
                    break;
            }
        }

        private void btnFecharConfig_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSalvarConfiguracoes_Click(object sender, EventArgs e)
        {
            try
            {
                TextoEmailRequest textoEmailRequest = new TextoEmailRequest();
                textoEmailRequest.Lista = new List<TextoEmailInfo>();

                foreach (DataGridViewRow item in grdTextoEmail.Rows)
                {
                    TextoEmailInfo info = new TextoEmailInfo();
                    info.Id = Convert.ToInt32(item.Cells["IdTextoEmail"].Value);
                    info.Texto = (item.Cells["TextoEmail"].Value == null ? "" : item.Cells["TextoEmail"].Value.ToString());
                    info.DataAtualizacao = Convert.ToDateTime(item.Cells["DataAtualizacao"].Value);
                    textoEmailRequest.Lista.Add(info);
                }

                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                TextoEmailResponse respTextoEmail = serv.GravarListaTextoEmail(textoEmailRequest);

                if (respTextoEmail.Retorno == TextoEmailResponse.RETORNO_OK)
                {
                    EmailRequest emailRequest = new EmailRequest();
                    emailRequest.Lista = new List<EmailInfo>();

                    foreach (DataGridViewRow item in grdEmail.Rows)
                    {
                        EmailInfo info = new EmailInfo();
                        info.Id = Convert.ToInt32(item.Cells["IdEmail"].Value);
                        info.Descricao = (item.Cells["Descricao"].Value == null ? "" : item.Cells["Descricao"].Value.ToString());
                        info.Titulo = (item.Cells["Titulo"].Value == null ? "" : item.Cells["Titulo"].Value.ToString());
                        info.Assunto = (item.Cells["Assunto"].Value == null ? "" : item.Cells["Assunto"].Value.ToString());
                        info.EmailPara = (item.Cells["Para"].Value == null ? "" : item.Cells["Para"].Value.ToString());
                        info.EmailCopia = (item.Cells["Copia"].Value == null ? "" : item.Cells["Copia"].Value.ToString());
                        info.EmailCopiaOculta = (item.Cells["CopiaOculta"].Value == null ? "" : item.Cells["CopiaOculta"].Value.ToString());
                        info.IdTextoEmail = Convert.ToInt32(item.Cells["IdTextoEmail"].Value);
                        info.DataAtualizacao = Convert.ToDateTime(item.Cells["DataAtualizacao"].Value);
                        emailRequest.Lista.Add(info);
                    }

                    EmailResponse respEmail = serv.GravarListaEmail(emailRequest);
                    if (respEmail.Retorno == EmailResponse.RETORNO_OK)
                    {
                        AssessorEmailRequest assessorEmailRequest = new AssessorEmailRequest();
                        assessorEmailRequest.Lista = new List<AssessorEmailInfo>();

                        foreach (int item in ListaAssessorEmailExcluido)
                        {
                            AssessorEmailInfo info = new AssessorEmailInfo();
                            info.IdAssessor = item;
                            assessorEmailRequest.Lista.Add(info);
                        }

                        AssessorEmailResponse respDelAssessorEmail = serv.RemoverListaAssessorEmail(assessorEmailRequest);
                        if (respDelAssessorEmail.Retorno == AssessorEmailResponse.RETORNO_OK)
                        {
                            assessorEmailRequest.Lista.Clear();

                            foreach (DataGridViewRow item in grdAssessorEmail.Rows)
                            {
                                AssessorEmailInfo info = new AssessorEmailInfo();
                                info.IdAssessor = Convert.ToInt32(item.Cells["CodigoAssessor"].Value);
                                info.IdEmail = Convert.ToInt32(item.Cells["DescricaoEmail"].Value);
                                info.DataAtualizacao = Convert.ToDateTime(item.Cells["DataAtualizacao"].Value);
                                assessorEmailRequest.Lista.Add(info);
                            }

                            AssessorEmailResponse respAssessorEmail = serv.GravarListaAssessorEmail(assessorEmailRequest);
                            if (respAssessorEmail.Retorno == AssessorEmailResponse.RETORNO_OK)
                            {
                                MessageBox.Show("Configuração de E-mail gravada com sucesso!", "Configuração",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("FALHA NA GRAVAÇÃO DE ASSESSORES QUE RECEBEM E-MAIL!", "Configuração",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("FALHA NA EXCLUSÃO DE ASSESSORES QUE RECEBEM E-MAIL!", "Configuração",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("FALHA NA GRAVAÇÃO DOS E-MAILS!", "Configuração",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("FALHA NA GRAVAÇÃO DOS TEXTOS DE E-MAIL!", "Configuração",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string msg = "Salvar Configurações: " + ex.Message;
                MessageBox.Show(msg, "Configuração E-mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            OnGravaConfigEmail();
            this.Close();
        }

        private void btnInserirTextoEmail_Click(object sender, EventArgs e)
        {
            int novoReg = grdTextoEmail.Rows.Add(new DataGridViewRow());
            grdTextoEmail.Rows[novoReg].Height = 30;

            grdTextoEmail.Rows[novoReg].Cells["IdTextoEmail"].Value = Convert.ToInt32(grdTextoEmail.Rows[grdTextoEmail.RowCount - 2].Cells["IdTextoEmail"].Value) + 1;
            grdTextoEmail.Rows[novoReg].Cells["DataAtualizacao"].Value = DateTime.Now;
        }

        private void grdEmail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (grdEmail.Columns[e.ColumnIndex].Name)
            {
                case "IdEmail":
                    e.CellStyle.ForeColor = Color.Gray;
                    e.CellStyle.SelectionForeColor = Color.Gray;
                    break;
                case "DataAtualizacao":
                    e.CellStyle.ForeColor = Color.Gray;
                    e.CellStyle.SelectionForeColor = Color.Gray;
                    break;
            }
        }

        private void btnInserirEmail_Click(object sender, EventArgs e)
        {
            int novoReg = grdEmail.Rows.Add(new DataGridViewRow());
            grdEmail.Rows[novoReg].Cells["IdEmail"].Value = Convert.ToInt32(grdEmail.Rows[grdEmail.RowCount - 2].Cells["IdEmail"].Value) + 1;
            grdEmail.Rows[novoReg].Cells["DataAtualizacao"].Value = DateTime.Now;
        }

        private void btnInserirAssessorEmail_Click(object sender, EventArgs e)
        {
            int novoReg = grdAssessorEmail.Rows.Add(new DataGridViewRow());
            grdAssessorEmail.Rows[novoReg].Cells["DataAtualizacao"].Value = DateTime.Now;
        }

        private void btnExcluirAssessorEmail_Click(object sender, EventArgs e)
        {
            int reg = grdAssessorEmail.SelectedCells[0].RowIndex;
            ListaAssessorEmailExcluido.Add(Convert.ToInt32(grdAssessorEmail.Rows[reg].Cells["CodigoAssessor"].Value));

            grdAssessorEmail.Rows.RemoveAt(reg);
        }

        private void grdAssessorEmail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (grdAssessorEmail.Columns[e.ColumnIndex].Name)
            {
                case "DataAtualizacao":
                    e.CellStyle.ForeColor = Color.Gray;
                    e.CellStyle.SelectionForeColor = Color.Gray;
                    break;
            }
        }

        private void grdEmail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            grdEmail.Rows[e.RowIndex].Cells["DataAtualizacao"].Value = DateTime.Now;
        }

        private void grdAssessorEmail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            grdAssessorEmail.Rows[e.RowIndex].Cells["DataAtualizacao"].Value = DateTime.Now;
        }

        private void grdTextoEmail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            grdTextoEmail.Rows[e.RowIndex].Cells["DataAtualizacao"].Value = DateTime.Now;
        }
    }
}

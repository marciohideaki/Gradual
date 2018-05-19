using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Interface.Desktop.Dados;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum
{
    public partial class FormDialog : DevExpress.XtraEditors.XtraForm
    {
        public FormDialog(Control controle, FormDialogTipoEnum tipo)
        {
            InitializeComponent();

            ajustarControlesPorTipo(tipo);

            pnl.Controls.Add(controle);
            controle.Dock = DockStyle.Fill;

            this.Load += new EventHandler(FormDialog_Load);
        }

        private void FormDialog_Load(object sender, EventArgs e)
        {
            this.Width += 200;
            this.Left -= 100;
        }

        public FormDialog(ControleInfo controleInfo, FormDialogTipoEnum tipo)
        {
            InitializeComponent();

            ajustarControlesPorTipo(tipo);

            this.Controle = new Controle(controleInfo);
            Control instancia = (Control)this.Controle.Instancia;
            pnl.Controls.Add(instancia);
            instancia.Dock = DockStyle.Fill;
            this.Text = controleInfo.Titulo;
            this.Load += new EventHandler(FormDialog_Load);
        }

        public Controle Controle { get; set; }

        private void ajustarControlesPorTipo(FormDialogTipoEnum tipo)
        {
            switch (tipo)
            {
                case FormDialogTipoEnum.Fechar:
                    cmdOK.Visible = false;
                    cmdFechar.Visible = true;
                    cmdFechar.Text = "Fechar";
                    break;
                case FormDialogTipoEnum.OkCancelar:
                    cmdOK.Visible = true;
                    cmdFechar.Visible = true;
                    cmdFechar.Text = "Cancelar";
                    break;
            }
        }

        private void cmdFechar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
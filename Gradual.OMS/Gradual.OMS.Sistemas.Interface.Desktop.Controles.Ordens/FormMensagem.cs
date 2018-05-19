using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Ordens
{
    public partial class FormMensagem : DevExpress.XtraEditors.XtraForm
    {
        public FormMensagem(string mensagem, object obj)
        {
            InitializeComponent();

            lblMensagem.Text = mensagem;
            ppg.SelectedObject = obj;
        }

        private void cmdFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
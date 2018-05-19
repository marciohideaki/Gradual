using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.StockMarket.Excel2003.AddIn
{
    public partial class frmInputBox_Login : Form
    {
        #region Propriedades

        public string CodigoEmail
        {
            get
            {
                return txtCodigoEmail.Text;
            }

            set
            {
                txtCodigoEmail.Text = value;
            }
        }

        public string Senha
        {
            get
            {
                return txtSenha.Text;
            }

            set
            {
                txtSenha.Text = value;
            }
        }

        #endregion

        public frmInputBox_Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void txtSenha_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void txtCodigoEmail_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }
    }
}

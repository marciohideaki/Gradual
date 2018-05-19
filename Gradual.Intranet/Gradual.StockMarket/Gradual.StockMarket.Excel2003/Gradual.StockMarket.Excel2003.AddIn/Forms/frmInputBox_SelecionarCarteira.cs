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
    public partial class frmInputBox_SelecionarCarteira : Form
    {
        #region Propriedades

        public List<string> ListaDeCarteiras
        {
            get
            {
                return (List<string>)cboCarteira.DataSource;
            }

            set
            {
                cboCarteira.DataSource = value;
            }
        }

        public int IndiceDaCarteiraSelecionada
        {
            get
            {
                return cboCarteira.SelectedIndex;
            }
        }

        #endregion

        #region Construtor

        public frmInputBox_SelecionarCarteira()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void btnImportar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppTesteRoteadorOrdens
{
    public partial class frmPontaVenda : Form
    {
        public string ClOrdID { get; set; }
        public string Account { get; set; }
        public string Qtde { get; set; }
        public string InvestorID { get; set; }

        public frmPontaVenda()
        {
            InitializeComponent();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            ClOrdID = txtClordIDVenda.Text;
            Account = txtAccountVenda.Text;
            Qtde = txtQtdeVenda.Text;
            InvestorID = txtInvestorIDVenda.Text;


            if (ClOrdID.Length > 0 && Account.Length > 0 && Qtde.Length > 0)
            {
                DialogResult = DialogResult.OK;

                this.Close();
            }
        }


        public new DialogResult ShowDialog()
        {
            txtClordIDVenda.Text = ClOrdID;
            txtAccountVenda.Text = Account;
            txtQtdeVenda.Text = Qtde;
            txtInvestorIDVenda.Text = InvestorID;

            DialogResult result = base.ShowDialog();

            return result;
        }

    }
}

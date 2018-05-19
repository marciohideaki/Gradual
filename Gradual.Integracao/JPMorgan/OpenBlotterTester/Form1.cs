using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBlotterTester.localhost;
using System.Globalization;

namespace OpenBlotterTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtUsr.Text = "jpmorgan";
            txtPwd.Text = "jpm2011*";

            txtTradeID.Text = DateTime.Now.ToString("yyyy/MM/dd 08:00:00");

            cmbUrl.Items.Add("Prod: http://wstradeinterface.gradualinvestimentos.com.br");
            cmbUrl.Items.Add("Prod: http://localhost:2769");
            cmbUrl.Items.Add("Homo: https://wstradeinterfacehomo.gradualinvestimentos.com.br");
            cmbUrl.SelectedIndex = 0;
        }

        private void btGo_Click(object sender, EventArgs e)
        {
            try
            {
                WSTradeInterface xxxx = new WSTradeInterface();

                switch (cmbUrl.SelectedIndex)
                {
                    case 0:
                        xxxx.Url = "http://wstradeinterface.gradualinvestimentos.com.br/WSTradeInterface.asmx";
                        break;
                    case 1:
                        xxxx.Url = "http://localhost:2769/WSTradeInterface.asmx";
                        break;
                    case 2:
                        xxxx.Url = "https://wstradeinterfacehomo.gradualinvestimentos.com.br/WSTradeInterface.asmx";
                        break;
                    default:
                        MessageBox.Show("Invalid URL");
                        break;
                }
                     

                DateTime tradeID = DateTime.ParseExact(txtTradeID.Text, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

                txtResults.Text = xxxx.QueryTradesStr(
                    txtUsr.Text,
                    txtPwd.Text,
                    0,
                    tradeID,
                    tradeID,
                    ref tradeID,
                    null,
                    null,
                    null,
                    null);
            }
            catch(Exception ex )
            {
                MessageBox.Show("Erro:" + ex.Message + "\r\n" + ex.StackTrace);
            }

        }
    }
}

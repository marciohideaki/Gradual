using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppTesteWSBTG
{
    public partial class frmTrades : Form
    {
        public frmTrades()
        {
            InitializeComponent();
        }

        private void btConsultar_Click(object sender, EventArgs e)
        {
            br.com.gradualinvestimentos.wsbtg.WSBTG servico = new br.com.gradualinvestimentos.wsbtg.WSBTG();

            switch (cmbUrl.SelectedIndex)
            {
                case 0:
                    servico.Url = "http://wsbtg.gradualinvestimentos.com.br/WSBTG.asmx";
                    break;
                case 1:
                    servico.Url = "http://localhost:1785/WSBTG.asmx";
                    break;
                default:
                    MessageBox.Show("Invalid URL");
                    break;
            }

            DataSet ret = servico.GetBMFIntraday(this.txtUsername.Text, txtPassword.Text, Convert.ToInt64(txtLastSeq.Text));

            txtTrades.Text = ret.GetXml().Replace("<trade>","\r\n<trade>");
        }

        private void frmTrades_Load(object sender, EventArgs e)
        {
            cmbUrl.Items.Add("Prod: http://wsbtg.gradualinvestimentos.com.br");
            cmbUrl.Items.Add("Prod: http://localhost:1785");
            cmbUrl.SelectedIndex = 0;

        }
    }
}

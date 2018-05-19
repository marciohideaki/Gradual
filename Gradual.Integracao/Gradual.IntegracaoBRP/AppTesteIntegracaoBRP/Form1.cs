using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppTesteIntegracaoBRP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btConsultar_Click(object sender, EventArgs e)
        {
            localhost.InativacaoClientesWS servico = new localhost.InativacaoClientesWS();

            switch (cmbUrl.SelectedIndex)
            {
                case 0:
                    servico.Url = "http://wsintegracaoBRP.gradualinvestimentos.com.br/InativacaoClientesWS.asmx";
                    break;
                case 1:
                    servico.Url = "http://localhost:39565/InativacaoClientesWS.asmx";
                    break;
                default:
                    MessageBox.Show("Invalid URL");
                    break;
            }

            DataSet retorno = servico.GetInactiveAccounts(txtUsername.Text, txtPassword.Text);

            txtRetorno.Text = retorno.GetXml();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbUrl.Items.Add("Prod: http://wsintegracaoBRP.gradualinvestimentos.com.br");
            cmbUrl.Items.Add("Prod: http://localhost:39565");
            cmbUrl.SelectedIndex = 1;

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace APPTesteWSBNP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtURL.Text = "http://wsblotter2.gradualinvestimentos.com.br/WSBNP.asmx";
            txtLastSeq.Text = "0";
        }

        private void btConsultar_Click(object sender, EventArgs e)
        {
            br.com.gradualinvestimentos.wsbnpparibas.WSBNP servico = new br.com.gradualinvestimentos.wsbnpparibas.WSBNP();

            servico.Url = txtURL.Text;

            long lastSeq = Convert.ToInt64(txtLastSeq.Text);

            txtResposta.Text = servico.GetBMFIntraday("bnpparibas", "w4prajaS", lastSeq).GetXml();
        }
    }
}

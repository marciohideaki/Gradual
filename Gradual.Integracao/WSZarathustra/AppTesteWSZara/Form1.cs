using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppTesteWSZara.br.com.gradualinvestimentos.wszarathustra;

namespace AppTesteWSZara
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            txtURL.Text = "http://wsblotter1.gradualinvestimentos.com.br/WSTraderBean.asmx";
        }


        private void btConsultar_Click(object sender, EventArgs e)
        {
            br.com.gradualinvestimentos.wszarathustra.WSTraderBean servico = new br.com.gradualinvestimentos.wszarathustra.WSTraderBean();

            servico.Url = txtURL.Text.Trim();

            //DateTime initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0,0,0);
            DateTime initialDate = DateTime.MinValue;

            traderBean []  ret = servico.GetBeans("zarathustra", "4aWrudrU", initialDate.ToString("yyyy/MM/dd HH:mm:ss"));


            this.txtResult.Text = "";

            foreach (traderBean bean in ret)
            {
                this.txtResult.Text += bean.ToString() + "\r\n";
            }
        }

    }
}

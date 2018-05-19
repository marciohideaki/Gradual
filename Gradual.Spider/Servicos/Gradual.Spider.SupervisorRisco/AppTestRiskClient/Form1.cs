using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Spider.RiskClient.Lib;
using Gradual.Spider.Ordem.Lib.Dados;
using Gradual.Spider.RiskClient.Lib.Mensagens;
using System.Diagnostics;
using Gradual.Spider.SupervisorRiscoADM.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Spider.RiskClient.Lib.Dados;
using AppTestRiskClient.Tools;
using Gradual.Spider.ServicoSupervisor;

namespace AppTestRiskClient
{
    public partial class Form1 : Form
    {
        RiskClient rskClient;

        public Form1()
        {
            InitializeComponent();
            rdBtCompra.Checked = true;
            rdBtVenda.Checked = false;

            rdbtBovespa.Checked = true;
            rdbtBMF.Checked = false;

            rskClient = RiskClient.Instance;

            txtOrderID.Text = DateTime.Now.ToString("yyyymmddhhmmssfff");
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            rskClient.StartClient();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            rskClient.StopClient();
        }

        private void btValidNova_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();

            SpiderOrderInfo info = new SpiderOrderInfo();

            info.OrderID = txtOrderID.Text.ToUpperInvariant().Trim();
            info.Account = Convert.ToInt32(txtAccount.Text.Trim());
            info.OrderQty = Convert.ToInt32(txtOrderQty.Text.Trim());
            info.Price = Convert.ToDouble(txtPrice.Text.Trim().Replace(',','.'));
            info.Symbol = txtSymbol.Text.ToUpperInvariant().Trim();

            if (rdBtCompra.Checked)
                info.Side = "1";
            else
                info.Side = "2";


            if (rdbtBovespa.Checked)
                info.Exchange = ExchangePrefixes.BOVESPA;
            else
                info.Exchange = ExchangePrefixes.BMF;


            ValidarRiscoRequest req = new ValidarRiscoRequest();
            req.FixMsgType = "D";
            req.Ordem = info;

            ValidarRiscoResponse resp = rskClient.ValidarPermissaoERisco(req);

            watch.Stop();

            if (resp.ValidationResult)
            {
                txtResult.Text += "\r\nValidacao da ordem " + info.OrderID + " OK! " + watch.ElapsedMilliseconds + "ms.";
                txtOrderID.Text = DateTime.Now.ToString("yyyymmddhhmmssfff");
            }
            else
                txtResult.Text += "\r\nValidacao da ordem " + info.OrderID + " erro [" + resp.RejectMessage + "] " + watch.ElapsedMilliseconds + "ms.";

        }

        private void rdBtCompra_CheckedChanged(object sender, EventArgs e)
        {
            if ( rdBtCompra.Checked )
                rdBtVenda.Checked = false;
        }

        private void rdBtVenda_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtVenda.Checked)
                rdBtCompra.Checked = false;
        }

        private void rdbtBovespa_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtBovespa.Checked)
                rdbtBMF.Checked = false;
        }

        private void rdbtBMF_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtBMF.Checked)
                rdbtBovespa.Checked = false;
        }

        private void btReloadResync_Click(object sender, EventArgs e)
        {
            ISupervisorRiscoAdm supervisorADM = Ativador.Get<ISupervisorRiscoAdm>();

            if (supervisorADM !=null )
            {
                supervisorADM.ReloadResync();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> subs = new List<int>();
            // subs.Add(SubscriptionTypes.POSITION_CLIENT);
            subs.Add(SubscriptionTypes.CONSOLIDATED_RISK);
            rskClient.StartClient(subs);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<Class1>> dict = new Dictionary<string, List<Class1>>();
            int maxElem = 50;
            int maxItem = 10000;
            for (int i = 0; i < maxElem; i++)
            {
                List<Class1> lst = new List<Class1>();
                for (int j = 0; j<maxItem; j++)
                {
                    Class1 xx = new Class1();
                    xx.Chave = j;
                    xx.Valor1 = "Valor 1 " + j;
                    xx.Valor2 = "Valor 2 " + j;
                    lst.Add(xx);
                }
                dict.Add(i.ToString(), lst);
            }


            
            Stopwatch sw = new Stopwatch();
            Stopwatch sw1 = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();

            List<Class1> aa = null;
            
            dict.TryGetValue("49", out aa);
            sw.Start();
            aa.ForEach(c => { c.Valor1 = "asdf"; c.Valor2 = "asdf2"; });
            sw.Stop();
            sw1.Start();
            foreach (Class1 xx in aa)
            {
                xx.Valor1 = "asdf XX 1";
                xx.Valor2 = "asdf XX 2";
            }
            sw1.Stop();

            sw2.Start();
            int len = aa.Count;
            for (int i =0; i < len; i++)
            {
                aa[i].Valor1 = "asdf XX 3";
                aa[i].Valor2 = "asdf XX 4";
            }
            sw2.Stop();
            MessageBox.Show("Count " + dict.Count);
            MessageBox.Show("List Count " + aa.Count);
            MessageBox.Show("ElapsedMili " + sw.ElapsedMilliseconds);
            MessageBox.Show("ElapsedTick " + sw.ElapsedTicks);

            MessageBox.Show("ElapsedTick 2 ===> " + sw1.ElapsedTicks);
            MessageBox.Show("ElapsedTick 3 ===> " + sw2.ElapsedTicks);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime dt1 = DateTime.Now;
            DateTime dt2 = DateTime.MinValue;
            TimeSpan dt3 = dt1.Subtract(dt2);
            
            MessageBox.Show(dt3.TotalMilliseconds.ToString());
            
            //TimeSpan t1 = new TimeSpan(dt1.Ticks);
            //TimeSpan t2 = new TimeSpan(dt2.Ticks);

            //var aa = t1 - t2;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Stopwatch sw = new Stopwatch();
            Dictionary<string, string> item = new Dictionary<string, string>();
            
            for (int i=0; i< 10000; i++) 
            {
                string chave = "ASDF-" + i.ToString();
                item.Add(chave, chave);
            }

            sw.Start();
            List<string> lst = item.Where(x => x.Key.IndexOf("00") >= 0).Select(x=>x.Value).ToList();
            sw.Stop();

            MessageBox.Show(sw.ElapsedTicks.ToString());
            MessageBox.Show(sw.ElapsedMilliseconds.ToString());    
            
        }

        private void btnConsolidatedRisk_Click(object sender, EventArgs e)
        {
            ServicoSupervisorRisco lServico = new ServicoSupervisorRisco();

            lServico.IniciarServico();
        }

        private void btnPositionClient_Click(object sender, EventArgs e)
        {
            ServicoSupervisorRisco lServico = new ServicoSupervisorRisco();

            lServico.IniciarServico();
        }

    }
}

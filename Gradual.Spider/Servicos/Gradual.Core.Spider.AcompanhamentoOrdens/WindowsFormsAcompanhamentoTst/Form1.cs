using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Core.Spider.AcompanhamentoOrdens.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Core.Spider.AcompanhamentoOrdens.Lib.Mensageria;
using log4net;
using Gradual.Core.Spider.AcompanhamentoOrdens.Lib.Dados;
using System.Threading;


namespace WindowsFormsAcompanhamentoTst
{
    public partial class Form1 : Form
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region Private variables
        Thread _thAcMon;
        bool _runMonitor;
        #endregion

        public Form1()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                IAcompanhamentoOrdensAdm iAcOrdens = Ativador.Get<IAcompanhamentoOrdensAdm>();
                iAcOrdens.DummyFunction();
            }

            catch
            {
                MessageBox.Show("Erro");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                IAcompanhamentoOrdensAdm iAcOrdens = Ativador.Get<IAcompanhamentoOrdensAdm>();
                OrderManagerMsgRequest req = new OrderManagerMsgRequest();

                if (listBox1.Items.Count > 0)
                {
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        req.FilterOptions.Conta.Add(Convert.ToInt32(listBox1.Items[i].ToString()));
                    }
                }

                if (listBox2.Items.Count > 0)
                {
                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                        req.FilterOptions.Ativo.Add(listBox2.Items[i].ToString());
                    }
                }

                if (!string.IsNullOrEmpty(textBox3.Text))
                    req.FilterOptions.DataInicial = Convert.ToDateTime(textBox3.Text);
                
                if (!string.IsNullOrEmpty(textBox4.Text))
                    req.FilterOptions.DataFinal = Convert.ToDateTime(textBox4.Text);
                
                if (listBox3.Items.Count > 0)
                {
                    for (int i = 0; i < listBox3.Items.Count; i++)
                    {
                        req.FilterOptions.Sessao.Add(listBox3.Items[i].ToString());
                    }
                }

                if (listBox4.Items.Count > 0)
                {
                    for (int i = 0; i < listBox4.Items.Count; i++)
                    {
                        req.FilterOptions.Sentido.Add(listBox4.Items[i].ToString());
                    }
                }

                if (listBox5.Items.Count > 0)
                {
                    for (int i = 0; i < listBox5.Items.Count; i++)
                    {
                        req.FilterOptions.Bolsa.Add(listBox5.Items[i].ToString());
                    }
                }

                if (listBox6.Items.Count > 0)
                {
                    for (int i = 0; i < listBox6.Items.Count; i++)
                    {
                        req.FilterOptions.OrderStatusID.Add(Convert.ToInt32(listBox6.Items[i].ToString()));
                    }
                }
                
                if (listBox7.Items.Count > 0)
                {
                    for (int i = 0; i < listBox7.Items.Count; i++)
                    {
                        req.FilterOptions.HandlInst.Add(listBox7.Items[i].ToString());
                    }
                }

                OrderManagerMsgResponse resp =  iAcOrdens.GetOrders(req);

                this.Invoke(new passo2(AtualizaDataSource), resp.ListOrders);
                /*
                for (int i = 0; i < resp.ListOrders.Count; i++)
                {
                    logger.Info("============================");
                    _imprimirOrdem(resp.ListOrders[i]);
                }
                */

                MessageBox.Show("Itens: " + resp.ListOrders.Count);
            }

            catch(Exception ex)
            {
                MessageBox.Show("Erro GetOrders: " + ex.StackTrace + " " + ex.Message);
            }
        }

        private void _imprimirOrdem(SpiderOrderInfo item)
        {
            logger.InfoFormat("Account [{0}]ChannelID [{1}]ClOrdID [{2}]CumQty [{3}]Description [{4}]ExchangeNumberID [{5}]ExecBroker [{6}]ExpireDate [{7}]FixMsgSeqNum [{8}]HandlInst [{9}]IdSession [{10}]MaxFloor [{11}]Memo [{12}]MinQty [{13}]Msg42Base64 [{14}]MsgFix [{15}]OrderID [{16}]OrderQty [{17}]OrderQtyRemaining [{18}]OrdStatusID [{19}]OrdTypeID [{20}]OrigClOrdID [{21}]Price [{22}]RegisterTime [{23}]SecurityExchangeID [{24}]SessionID [{25}]SessionIDOrigin [{26}]Side [{27}]StopPx [{28}]StopStartID [{29}]Symbol [{30}]SystemID [{31}]TimeInForce [{32}]TransactTime [{33}] Exchange[{34}] IntegrationName[{35}]",
                                item.Account,
                                item.ChannelID,
                                item.ClOrdID,
                                item.CumQty,
                                item.Description,
                                item.ExchangeNumberID,
                                item.ExecBroker,
                                item.ExpireDate,
                                item.FixMsgSeqNum,
                                item.HandlInst,
                                item.IdSession,
                                item.MaxFloor,
                                item.Memo,
                                item.MinQty,
                                item.Msg42Base64,
                                item.MsgFix,
                                item.OrderID,
                                item.OrderQty,
                                item.OrderQtyRemaining,
                                item.OrdStatusID,
                                item.OrdTypeID,
                                item.OrigClOrdID,
                                item.Price,
                                item.RegisterTime,
                                item.SecurityExchangeID,
                                item.SessionID,
                                item.SessionIDOrigin,
                                item.Side,
                                item.StopPx,
                                item.StopStartID,
                                item.Symbol,
                                item.SystemID,
                                item.TimeInForce,
                                item.TransactTime,
                                item.Exchange,
                                item.IntegrationName);


            for (int i = 0; i < item.Details.Count; i++)
            {
                logger.InfoFormat("=========> OrderDetailID[{0}] TransactID [{1}] OrderID [{2}] OrderQty [{3}] OrderQtyRemaining [{4}] Price [{5}] StopPx [{6}] OrderStatusID [{7}] TransactTime [{8}] Description [{9}] TradeQty [{10}] CumQty [{11}] FixMsgSeqNum[{12}]  CxlRejResponseTo [{13}] CxlRejReason [{14}] MsgFixDetail [{15}]",
                item.Details[i].OrderDetailID,
                item.Details[i].TransactID,
                item.Details[i].OrderID,
                item.Details[i].OrderQty,
                item.Details[i].OrderQtyRemaining,
                item.Details[i].Price,
                item.Details[i].StopPx,
                item.Details[i].OrderStatusID,
                item.Details[i].TransactTime,
                item.Details[i].Description,
                item.Details[i].TradeQty,
                item.Details[i].CumQty,
                item.Details[i].FixMsgSeqNum,
                item.Details[i].CxlRejResponseTo,
                item.Details[i].CxlRejReason,
                item.Details[i].MsgFixDetail);

            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime ini = new DateTime(2014, 10, 01);

            DateTime fim = new DateTime(2014, 10, 01);


            if (now.Date >=ini.Date  && now.Date <= fim.Date)
                MessageBox.Show("ASDFEFASDFEFEFADSF");
        }

        private void spiderOrderInfoBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {
                _runMonitor = true;
                if (_thAcMon != null && _thAcMon.IsAlive)
                {
                    MessageBox.Show("Já iniciado");
                    return;
                }
                _thAcMon = new Thread(new ThreadStart(_monitorProcess));
                _thAcMon.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start monitor: " + ex.Message, ex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                _runMonitor = false;
                if (null != _thAcMon && _thAcMon.IsAlive)
                {
                    _thAcMon.Join(1000);
                    _thAcMon.Abort();
                    _thAcMon = null;
                }
                MessageBox.Show("Parado!");
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start monitor: " + ex.Message, ex);
            }
        }

        private void addItem(ListBox a, string txt)
        {
            try
            {
                a.Items.Add(txt);
            }
            catch { }
        }

        private void removeItem(ListBox a)
        {
            try
            {
                a.Items.RemoveAt(a.SelectedIndex);
            }
            catch { }
        }


        delegate void passo2(List<SpiderOrderInfo> lst);
        private void AtualizaDataSource(List<SpiderOrderInfo> lst)
        {
            spiderOrderInfoBindingSource.DataSource = lst;
        }   

        private void _monitorProcess()
        {
            IAcompanhamentoOrdensAdm iAcOrdens = Ativador.Get<IAcompanhamentoOrdensAdm>();
            OrderManagerMsgRequest req = new OrderManagerMsgRequest();
            while (_runMonitor)
            {
                try
                {
                    req.FilterOptions.Conta.Clear();
                    req.FilterOptions.HandlInst.Clear();
                    req.FilterOptions.Conta.Add(31940);
                    req.FilterOptions.HandlInst.Add("3");
                    OrderManagerMsgResponse resp = iAcOrdens.GetOrders(req);
                    this.Invoke(new passo2(AtualizaDataSource), resp.ListOrders);
                    // spiderOrderInfoBindingSource.DataSource = resp.ListOrders;
                    logger.Info("Consultado... " + resp.ListOrders.Count);
                }
                catch (Exception ex)
                {
                    logger.Error("Erro monitor.. " + ex.Message);
                }
                Thread.Sleep(500);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addItem(listBox1, textBox1.Text);
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeItem(listBox1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addItem(listBox2, textBox2.Text);
        }

        private void listBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeItem(listBox2);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addItem(listBox3, textBox5.Text);
        }

        private void listBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeItem(listBox3);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addItem(listBox4, textBox6.Text);
        }

        private void listBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeItem(listBox4);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addItem(listBox5, textBox7.Text);
        }

        private void listBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeItem(listBox5);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addItem(listBox6, textBox8.Text);
        }

        private void listBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeItem(listBox7);
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addItem(listBox7, textBox9.Text);
        }

        private void listBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeItem(listBox6);
        }


    }
}

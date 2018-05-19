using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using Gradual.Spider.CommSocket;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens;
using System.Net.Sockets;
using Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using Gradual.Spider.Acompanhamento4Socket.Lib.Dados;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        #region log4net declaration
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region private vars
        SpiderSocket _client = null;
        #endregion
        public Form1()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            dataGridView1.Rows.Add("Account", "51452", "0");
            //dataGridView1.Rows.Add("OrderID", "", "0");
            //dataGridView1.Rows.Add("ClOrdID", "", "0");
            //dataGridView1.Rows.Add("Price", "", "0");
            //dataGridView1.Rows.Add("RegisterTime", "", "0");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (null == _client)
                {
                    _client = new SpiderSocket();
                    _client.OnConnectionOpened += new ConnectionOpenedHandler(_client_OnConnectionOpened);
                    _client.AddHandler<LogonSrvResponse>(new ProtoObjectReceivedHandler<LogonSrvResponse>(_client_OnLogonResponse));
                    _client.AddHandler<SpiderOrderInfo>(new ProtoObjectReceivedHandler<SpiderOrderInfo>(_client_OnSpiderOrderInfoReceived));
                    _client.AddHandler<SondaSrvInfo>(new ProtoObjectReceivedHandler<SondaSrvInfo>(_client_OnSondaSrvInfoReceived));
                    _client.AddHandler<FilterInfoResponse>(new ProtoObjectReceivedHandler<FilterInfoResponse>(_client_OnFilterInfoResponseReceived));
                    _client.AddHandler<FilterDetailInfoResponse>(new ProtoObjectReceivedHandler<FilterDetailInfoResponse>(_client_OnFilterDetailInfoResponseReceived));
                    _client.AddHandler<StreamerOrderInfo>(new ProtoObjectReceivedHandler<StreamerOrderInfo>(_client_OnStreamerOrderInfoReceived));
                    _client.AddHandler<FilterStreamerResponse>(new ProtoObjectReceivedHandler<FilterStreamerResponse>(_client_OnFilterStreamerResponseReceived));

                }
                if (_client.IsConectado())
                {
                    MessageBox.Show("Já conectado!!!");
                    return;
                }
                _client.IpAddr = textBox1.Text;
                _client.Port = Convert.ToInt32(textBox2.Text);

                _client.OpenConnection();
            }
            catch (Exception ex)
            {
                logger.Error("Zica na conexao do socket..." + ex.Message, ex);
            }
        }

        void _client_OnConnectionOpened(object sender, ConnectionOpenedEventArgs args)
        {
            try
            {
                logger.Info("Cliente CONECTADO!!");
                LogonSrvRequest req = new LogonSrvRequest();
                // OBS: Se requisicao tiverAppType.A4S_FULL_DETAIL, nao limita o numero de registros dos details do order

                if (checkBox1.Checked)
                    req.AppDescription = AppType.A4SOCKET_FULL_DETAIL;
                else
                    req.AppDescription = "WindowsForm Ac4S Test"; // Qq coisa
                if (null != _client)
                    _client.SendObject(req);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na abertura da conexao: " + ex.Message, ex);
            }
        }

        void _client_OnLogonResponse(object sender, int clientNumber, Socket clientSocket, LogonSrvResponse args)
        {

            MessageBox.Show("Conectado: " + args.SessionID);
            logger.Info("Logon Received: " + args.SessionID);
            // OrderCache4Socket.GetInstance().Connected(true);
        }

        void _client_OnSpiderOrderInfoReceived(object sender, int clientNumber, Socket clientSocket, SpiderOrderInfo args)
        {
            try
            {
                logger.InfoFormat("ClientNumber [{0}] OrderID[{1}] Detail: [{2}]", clientNumber, args.OrderID, args.Details.Count);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no recebimento do SpiderOrderInfo: " + ex.Message, ex);
            }
        }

        void _client_OnSondaSrvInfoReceived(object sender, int clientNumber, Socket clientSocket, SondaSrvInfo args)
        {
            logger.Info("Sonda received: " + args.TimeStamp);
            if (_client.IsConectado())
            {
                SondaSrvInfo sonda = new SondaSrvInfo();
                sonda.TimeStamp = DateTime.Now.Ticks;
                _client.SendObject(sonda);
            }
        }

        void _client_OnFilterInfoResponseReceived(object sender, int clientNumber, Socket clientSocket, FilterInfoResponse args)
        {
            logger.Info("============================");
            logger.Info("ID: " + args.Id);
            logger.Info("ErrCode: " + args.ErrCode);
            logger.Info("ErrMsg: " + args.ErrMsg);
            logger.Info("ListCount: " + args.Orders.Count);
            for (int i = 0; i < args.Orders.Count; i++)
            {
                logger.InfoFormat("OrderID [{0}] AvgPx[{1}] AvgPxW[{2}] OrdStatus [{3}] DetailCount [{4}]", args.Orders[i].OrderID, 
                    args.Orders[i].AvgPx, args.Orders[i].AvgPxW,args.Orders[i].OrdStatus, args.Orders[i].Details.Count);
            }
        }

        void _client_OnFilterDetailInfoResponseReceived(object sender, int clientNumber, Socket clientSocket, FilterDetailInfoResponse args)
        {
            logger.Info("============================");
            logger.Info("ID: " + args.Id);
            logger.Info("ErrCode: " + args.ErrCode);
            logger.Info("ErrMsg: " + args.ErrMsg);
            logger.Info("ListCount: " + args.Details.Count);
            for (int i = 0; i < args.Details.Count; i++)
            {
                logger.InfoFormat("Detail===> OrderID [{0}] OrderDetailID [{1}]", args.Details[i].OrderID, args.Details[i].OrderDetailID);
            }
        }

        void _client_OnStreamerOrderInfoReceived(object sender, int clientNumber, Socket clientSocket, StreamerOrderInfo args)
        {
            try
            {
                int max = args.Order.Details.Count-1;
                logger.InfoFormat("StreamerOrderInfo => ClientNumber[{0}] IdReq[{1}] MsgType[{2}] OrderID[{3}] Account[{4}] Symbol[{5}] SessionOrigin[{6}] DetailCount[{7}] DetailTime[{8}] Exchange [{9}] AccountDv[{10}]", 
                    clientNumber, args.Id, args.MsgType, args.Order.OrderID, args.Order.Account, args.Order.Symbol, args.Order.SessionIDOriginal, args.Order.Details.Count, args.Order.Details[max].TransactTime, args.Order.Exchange, args.Order.AccountDv);
                if (args.Order.Details[0].OrderStatusID == 101 || args.Order.Details[0].OrderStatusID == 102 || args.Order.Details[0].OrderStatusID == 103)
                {
                    logger.Error("ORderStatus: " + args.Order.OrdStatus);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no recebimento do SpiderOrderInfo: " + ex.Message, ex);
            }
        }

        void _client_OnFilterStreamerResponseReceived(object sender, int clientNumber, Socket clientSocket, FilterStreamerResponse args)
        {
            try
            {
                logger.InfoFormat("Retorno do FilterStreamResponse: [{0}]   [{1}]", args.Id,args.ErrMsg);


            }
            catch (Exception ex)
            {
                logger.Error("Deu zica no FilterStreamer Response: " + ex.Message, ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (_client != null && _client.IsConectado())
                {
                    _client.OnConnectionOpened -= _client_OnConnectionOpened;
                    _client.CloseSocket();
                    _client.Dispose();
                    _client = null;
                }
                MessageBox.Show("Desconectou a bagaça");

            }
            catch (Exception ex)
            {
                logger.Error("Zica na desconexao do socket: " + ex.Message, ex);
                MessageBox.Show("Zica na desconexao do socket: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //A4SocketRequest
            if (_client.IsConectado())
            {
                A4SocketRequest req = new A4SocketRequest();
                req.AppDescription = "App Teste";
                _client.SendObject(req);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Info("Enviando Requisicao....");
                FilterInfoRequest req = new FilterInfoRequest();
                req.RecordLimit = 20000;
                req.Filter = _composeFilter();
                req.Id = Guid.NewGuid().ToString();
                if (_client.IsConectado())
                    _client.SendObject(req);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio do filter request: " + ex.Message, ex);
            }
        }

        #region Filter
        private FilterSpiderOrder _composeFilter()
        {
            int len = dataGridView1.Rows.Count -1 ;
            FilterSpiderOrder ret = new FilterSpiderOrder();
            for (int i = 0; i < len; i++)
            {
                string fld = dataGridView1.Rows[i].Cells[0].Value.ToString();
                switch (fld)
                {
                    case "OrderID":
                        ret.OrderID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.OrderID.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value!=null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.OrderID.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string [] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char [] {';'});
                            for (int k = 0; k < arr.Length; k++)
                                ret.OrderID.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "OrigClOrdID":
                        ret.OrigClOrdID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.OrigClOrdID.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.OrigClOrdID.ListValue.Add(arr[k]);
                        }
                        break;
                    case "ExchangeNumberID":
                        ret.ExchangeNumberID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.ExchangeNumberID.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.ExchangeNumberID.ListValue.Add(arr[k]);
                        }
                        break;
                    case "ClOrdID":
                        ret.ClOrdID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.ClOrdID.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.ClOrdID.ListValue.Add(arr[k]);
                        }
                        break;
                    case "Account":
                        ret.Account.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.Account.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value!=null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.Account.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.Account.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "Symbol":
                        ret.Symbol.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.Symbol.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.Symbol.ListValue.Add(arr[k]);
                        }
                        break;
                    case "SecurityExchangeID":
                        ret.SecurityExchangeID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.SecurityExchangeID.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.SecurityExchangeID.ListValue.Add(arr[k]);
                        }
                        break;
                    case "StopStartID":
                        ret.StopStartID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.StopStartID.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.StopStartID.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.StopStartID.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "OrdTypeID":
                        ret.OrdTypeID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.OrdTypeID.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.OrdTypeID.ListValue.Add(arr[k]);
                        }
                        break;
                    case "OrdStatus":
                        ret.OrdStatus.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.OrdStatus.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.OrdStatus.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.OrdStatus.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "RegisterTime":
                        ret.RegisterTime.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.RegisterTime.Value = Convert.ToDateTime(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.RegisterTime.Value2 = Convert.ToDateTime(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.RegisterTime.ListValue.Add(Convert.ToDateTime(arr[k]));
                        }
                        break;
                    case "TransactTime":
                        ret.TransactTime.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.TransactTime.Value = Convert.ToDateTime(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.TransactTime.Value2 = Convert.ToDateTime(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.TransactTime.ListValue.Add(Convert.ToDateTime(arr[k]));
                        }
                        break;
                    case "ExpireDate":
                        ret.ExpireDate.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.ExpireDate.Value = Convert.ToDateTime(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.ExpireDate.Value2 = Convert.ToDateTime(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.ExpireDate.ListValue.Add(Convert.ToDateTime(arr[k]));
                        }
                        break;
                    case "TimeInForce":
                        ret.TimeInForce.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.TimeInForce.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.TimeInForce.ListValue.Add(arr[k]);
                        }
                        break;
                    case "ChannelID":
                        ret.ChannelID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.ChannelID.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.ChannelID.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.ChannelID.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "ExecBroker":
                        ret.ExecBroker.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.ExecBroker.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.ExecBroker.ListValue.Add(arr[k]);
                        }
                        break;
                    case "Side":
                        ret.Side.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.Side.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.Side.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.Side.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "OrderQty":
                        ret.OrderQty.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.OrderQty.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.OrderQty.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.OrderQty.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "OrderQtyMin":
                        ret.OrderQtyMin.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.OrderQtyMin.Value = Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.OrderQtyMin.Value2 = Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.OrderQtyMin.ListValue.Add(Convert.ToDecimal(arr[k]));
                        }
                        break;
                    case "OrderQtyApar":
                        ret.OrderQtyApar.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.OrderQtyApar.Value = Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.OrderQtyApar.Value2 = Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.OrderQtyApar.ListValue.Add(Convert.ToDecimal(arr[k]));
                        }
                        break;
                    case "OrderQtyRemaining":
                        ret.OrderQtyRemaining.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.OrderQtyRemaining.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.OrderQtyRemaining.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.OrderQtyApar.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "Price":
                        ret.Price.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.Price.Value = Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.Price.Value2 = Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.Price.ListValue.Add(Convert.ToDecimal(arr[k]));
                        }
                        break;
                    case "StopPx":
                        ret.StopPx.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.StopPx.Value = Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.StopPx.Value2 = Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.StopPx.ListValue.Add(Convert.ToDecimal(arr[k]));
                        }
                        break;
                    case "Description":
                        ret.Description.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.Description.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.Description.ListValue.Add(arr[k]);
                        }
                        break;
                    case "SystemID":
                        ret.SystemID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.SystemID.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.SystemID.ListValue.Add(arr[k]);
                        }
                        break;
                    case "Memo":
                        ret.Memo.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.Memo.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.Memo.ListValue.Add(arr[k]);
                        }
                        break;
                    case "CumQty":
                        ret.CumQty.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.CumQty.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.CumQty.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.CumQty.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "FixMsgSeqNum":
                        ret.FixMsgSeqNum.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.FixMsgSeqNum.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.FixMsgSeqNum.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.FixMsgSeqNum.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "SessionID":
                        ret.SessionID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.SessionID.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.SessionID.ListValue.Add(arr[k]);
                        }
                        break;
                    case "SessionIDOriginal":
                        ret.SessionIDOriginal.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.SessionIDOriginal.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.SessionIDOriginal.ListValue.Add(arr[k]);
                        }
                        break;
                    case "IdFix":
                        ret.IdFix.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.IdFix.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[3].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                            ret.IdFix.Value2 = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.IdFix.ListValue.Add(Convert.ToInt32(arr[k]));
                        }
                        break;
                    case "MsgFix":
                        ret.MsgFix.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.MsgFix.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.MsgFix.ListValue.Add(arr[k]);
                        }
                        break;
                    case "Msg42Base64":
                        ret.Msg42Base64.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.Msg42Base64.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.Msg42Base64.ListValue.Add(arr[k]);
                        }
                        break;
                    case "HandlInst":
                        ret.HandlInst.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.HandlInst.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.HandlInst.ListValue.Add(arr[k]);
                        }
                        break;
                    case "Exchange":
                        ret.Exchange.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.Exchange.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.Exchange.ListValue.Add(arr[k]);
                        }
                        break;
                    case "IntegrationName":
                        ret.IntegrationName.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        ret.IntegrationName.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[4].Value.ToString()))
                        {
                            string[] arr = dataGridView1.Rows[i].Cells[4].Value.ToString().Split(new char[] { ';' });
                            for (int k = 0; k < arr.Length; k++)
                                ret.IntegrationName.ListValue.Add(arr[k]);
                        }
                        break;
                }
            }
            return ret;
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            button2.PerformClick();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<Class1> lst = new List<Class1>();

            for (int i = 0; i < 500; i++)
            {
                Class1 xx = new Class1();
                xx.Symbol = i.ToString();
                xx.Aux1 = i;
                lst.Add(xx);
            }
            int val = 5;
            var query = from orders in lst.AsQueryable().Where(x=> ((string) x.GetType().GetProperty("Symbol").GetValue(x, null) =="5") )
                        select orders;
            MessageBox.Show(query.ToList().Count().ToString());
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                logger.Info("Enviando Requisicao....");
                FilterDetailInfoRequest req = new FilterDetailInfoRequest();
                req.OrderID = Convert.ToInt32(textBox4.Text);
                req.Id = Guid.NewGuid().ToString();
                if (_client.IsConectado())
                    _client.SendObject(req);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio do filter request: " + ex.Message, ex);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                FilterStreamerRequest req = new FilterStreamerRequest();
                int xx;
                req.Id = Guid.NewGuid().ToString();
                if (int.TryParse(textBox3.Text, out xx))
                    req.Account = new FilterIntVal(xx, TypeCompare.EQUAL);
                if (!string.IsNullOrEmpty(textBox6.Text))
                    req.Symbol = new FilterStringVal(textBox6.Text, TypeCompare.EQUAL);
                if (!string.IsNullOrEmpty(textBox5.Text))
                    req.SessionID = new FilterStringVal(textBox5.Text, TypeCompare.EQUAL);

                if (_client.IsConectado())
                    _client.SendObject(req);

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio do filter stream request: " + ex.Message, ex);
            }
            
            



        }

        










    }
}

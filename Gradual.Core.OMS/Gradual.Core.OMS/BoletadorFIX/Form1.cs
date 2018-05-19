using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using log4net;
using System.Configuration;
using Gradual.OMS.ServicoRoteador;

using QuickFix;
using System.Drawing;
using Gradual.Core.OMS.FixServerLowLatency;
using Gradual.Core.OMS.FixServerLowLatency.Lib;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Gradual.Core.OMS.LimiteManager.Lib;
using Gradual.Core.OMS.LimiteManager.Lib.Mensageria;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;

namespace BoletadorFIX
{
    public partial class Form1 : Form, IRoteadorOrdensCallback
    {
        private static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        long lastCLOrdID = 0;
        private IRoteadorOrdens _roteador = null;
        private List<OrdemInfo> ofertasenviadas = SerializadorOfertas.LoadOfertas();
        private Queue<string> qReport = new Queue<string>();

        private System.Timers.Timer sundaTimer = new System.Timers.Timer();
        private QuickFix.SessionID _session;
        private Dictionary _mainDic;
        QuickFix.Transport.SocketInitiator _initiator;
        private long version = 0;
        FIX42Client _fix42Client;
        FIX44Client _fix44Client;

        public Button ButtonClient42 { get { return btClient42; } }
        public Button ButtonClient44 { get { return btClient44; } }

        string _sender;
        string _target;
        string _sendersub;
        string _targetsub;
        string _pwd;
        string _host;
        string _port;
        string _dictionary;
        string _version;
        bool _useDictionary;
        bool _running = false;       
        public void OnTimer( Object source, ElapsedEventArgs e )
        {
            string[] msgs;
            StringBuilder report = new StringBuilder();


            lock (qReport)
            {
                msgs = qReport.ToArray();
                qReport.Clear();
            }

            foreach (string msg in msgs)
            {
                report.Append(msg);
                report.Append("\r\n");
            }

            if (report.Length > 0 )
                this.BeginInvoke(new MethodInvoker(delegate() { _atualizaReport(report.ToString()); }));

            //this.BeginInvoke(new MethodInvoker(delegate() { _atualizaReport(report.ToString()); }));
        }


        public Form1()
        {
            InitializeComponent();

            sundaTimer.Elapsed += new ElapsedEventHandler(OnTimer);
            sundaTimer.Interval = 1000;
            sundaTimer.Enabled = true;

            cmbBolsa.Items.Add("BOVESPA");
            cmbBolsa.Items.Add("BMF");
            cmbBolsa.SelectedIndex = 0;

            cmbTipoValidade.Items.Add("0-Para o dia");
            cmbTipoValidade.Items.Add("3-Executa ou cancela");
            cmbTipoValidade.Items.Add("4-Tudo ou Nada");
            cmbTipoValidade.Items.Add("1-Ate cancelar");
            cmbTipoValidade.Items.Add("6-Data especifica");
            cmbTipoValidade.Items.Add("2-Abertura Mercado");
            cmbTipoValidade.Items.Add("7-Fechamento Mercado");
            cmbTipoValidade.Items.Add("A-Boa para Leilao");
            cmbTipoValidade.SelectedIndex = 0;

            rdCompra.Checked = true;
            rdVenda.Checked = false;

            cmbOrderType.Items.Add("2-Normal/Limitada");
            cmbOrderType.Items.Add("4-Stop Limitada");
            cmbOrderType.Items.Add("K-A mercado com restante limitada");
            cmbOrderType.Items.Add("Abertura");
            cmbOrderType.Items.Add("StopStart Gradual");
            cmbOrderType.Items.Add("A mercado-1");
            cmbOrderType.Items.Add("3-StopLoss (protecao)");
            cmbOrderType.Items.Add("5-Market OnClose( fix 4.2)");
            cmbOrderType.Items.Add("A-OnClose(fix 4.2)");
            cmbOrderType.Items.Add("B-Limit OnClose(fix 4.2)");
            cmbOrderType.SelectedIndex = 0;

            lastCLOrdID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmm"));
            txtClOrdID.Text = lastCLOrdID.ToString();
            //txtOperador.Text = ConfigurationManager.AppSettings["CanalBovespaPadrao"].ToString();
        }

        private void btExecRep_Click(object sender, EventArgs e)
        {
            IAssinaturasRoteadorOrdensCallback roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);

            AssinarExecucaoOrdemResponse resp = roteador.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());
            AssinarStatusConexaoBolsaResponse cnxresp = roteador.AssinarStatusConexaoBolsa(new AssinarStatusConexaoBolsaRequest());
        }


        #region IRoteadorOrdensCallback Members
        public void OrdemAlterada(OrdemInfo report)
        {
            string msg = "Order Dump Begin =======================\r\n";
            msg += "\r\n";

            msg += "OrdemInfo:\r\n";
            msg += "==========\r\n";
            msg += "Account .........: " + report.Account  + "\r\n";
            msg += "Numero da ordem .: " + report.ClOrdID + "\r\n";
            msg += "Symbol ..........: " + report.Symbol + "\r\n";
            msg += "Bolsa ...........: " + report.Exchange + "\r\n";
            msg += "ExchangeNumber ..: " + report.ExchangeNumberID + "\r\n";
            msg += "Status ..........: " + report.OrdStatus + "\r\n";
            msg += "Quantidade ......: " + report.OrderQty + "\r\n";
            msg += "Qtde restante ...: " + report.OrderQtyRemmaining + "\r\n";
            msg += "Preco ...........: " + report.Price + "\r\n";
            msg += "Memo5149 ........: " + report.Memo5149 + "\r\n";

            msg += "\r\n";
            msg += "AcompanhamentoOrdemInfo:\r\n";
            msg += "========================\r\n";

            int i = 0;
            foreach(AcompanhamentoOrdemInfo acompanhamento in report.Acompanhamentos)
            {
                msg += "Item " + i++ + " \r\n";
                msg += "---------\r\n";
                msg += "NumeroControleOrdem ..: " + acompanhamento.NumeroControleOrdem + "\r\n";
                msg += "CodigoDoCliente ......: " + acompanhamento.CodigoDoCliente + "\r\n";
                msg += "CodigoResposta .......: " + acompanhamento.CodigoResposta + "\r\n";
                msg += "CodigoTransacao ......: " + acompanhamento.CodigoTransacao + "\r\n";
                msg += "Instrumento ..........: " + acompanhamento.Instrumento + "\r\n";
                msg += "CanalNegociacao ......: " + acompanhamento.CanalNegociacao + "\r\n";
                msg += "Direcao ..............: " + acompanhamento.Direcao + "\r\n";
                msg += "QuantidadeSoliciada ..: " + acompanhamento.QuantidadeSolicitada + "\r\n";
                msg += "QuantidadeExecutada ..: " + acompanhamento.QuantidadeExecutada + "\r\n";
                msg += "Preco ................: " + acompanhamento.Preco + "\r\n";
                msg += "StatusOrdem ..........: " + acompanhamento.StatusOrdem + "\r\n";
                msg += "DataOrdemEnvio .......: " + acompanhamento.DataOrdemEnvio + "\r\n";
                msg += "DataAtualizacao ......: " + acompanhamento.DataAtualizacao + "\r\n";
                msg += "CodigoRejeicao .......: " + acompanhamento.CodigoRejeicao + "\r\n";
                msg += "Descricao ............: " + acompanhamento.Descricao + "\r\n";
            }

            msg += "\r\nOrder Dump End =========================\r\n";

            //if (report.FixMsgSeqNum > 0)
            //{
            //    txtFixBeginSeqNo.Text = report.FixMsgSeqNum.ToString();
            //    txtFixEndSeqNo.Text = (report.FixMsgSeqNum + 1).ToString();
            //}

            _addMsg(msg);

        }

        public void _addMsg(string msg)
        {
            logger.Debug(msg);

            lock (qReport)
            {
                qReport.Enqueue(msg);
            }
        }

        public void _atualizaReport(string msg)
        {
            //this.txtExecReport.Text += DateTime.Now.ToString("HH:mm:ss.fff") + ": " + msg + "\r\n";
            //this.txtExecReport.Select(txtExecReport.Text.Length - 1, 0);
            //this.txtExecReport.ScrollToCaret();
            this.rtExecReport.Text += DateTime.Now.ToString("HH:mm:ss.fff") + ": " + msg + "\r\n";
            this.rtExecReport.SelectionStart = this.rtExecReport.Text.Length;
            this.rtExecReport.ScrollToCaret();
        }

        

        public void StatusConexaoAlterada(StatusConexaoBolsaInfo status)
        {
            string msg = "";

            msg += "Bolsa ..: " + status.Bolsa + "\r\n";
            msg += "Operador ..: " + status.Operador + "\r\n";
            msg += "Conectado .: " + status.Conectado.ToString() + "\r\n";

            this.txtConexoes.Text = msg;
            //_addMsg(msg);
        }

        #endregion


        /// <summary>
        /// Envio de nova ordem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEnviarOrdem_Click(object sender, EventArgs e)
        {
            try
            {

                OrdemInfo ordem = new OrdemInfo();

                ordem.ClOrdID = txtClOrdID.Text;
                ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";

                ordem.ExchangeNumberID = txtExchangeNumber.Text;
                ordem.Price = Convert.ToDouble(txtPreco.Text);
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);
                ordem.MinQty = Convert.ToInt32(txtQtdeMin.Text);
                ordem.MaxFloor = Convert.ToInt32(txtQtdeAparente.Text);
                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.RegisterTime = DateTime.Now;
                ordem.TransactTime = DateTime.Now;
                ordem.ExecBroker = txtTraderID.Text;
                ordem.Memo5149 = "Nova " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                if (txtSenderLocation.Text.Length > 0)
                    ordem.SenderLocation = txtSenderLocation.Text;

                if (chkAccountType.Checked)
                    ordem.AcountType = ContaTipoEnum.GIVE_UP_LINK_IDENTIFIER;

                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;

                if (txtStopPX.Text.Length > 0 && Convert.ToDouble(txtStopPX.Text) > 0)
                {
                    ordem.StopPrice = Convert.ToDouble(txtStopPX.Text);
                }

                if (txtInvestorID.Text.Length > 0)
                {
                    ordem.InvestorID = txtInvestorID.Text;
                }

                if (txtExecTrader.Text.Length > 0)
                {
                    ordem.ExecutingTrader = txtExecTrader.Text;
                }

                switch (cmbOrderType.SelectedIndex)
                {
                    case 0: ordem.OrdType = OrdemTipoEnum.Limitada; break;
                    case 1: ordem.OrdType = OrdemTipoEnum.StopLimitada; break;
                    case 2: ordem.OrdType = OrdemTipoEnum.MarketWithLeftOverLimit; break;
                    case 3: ordem.OrdType = OrdemTipoEnum.OnClose; break;
                    case 4: ordem.OrdType = OrdemTipoEnum.StopStart; break;
                    case 5: ordem.OrdType = OrdemTipoEnum.Mercado; break;
                    case 6: ordem.OrdType = OrdemTipoEnum.StopLoss; break;
                    default:
                        ordem.OrdType = OrdemTipoEnum.OnClose; break;
                }

                //0- Para o dia");
                //1- Executa ou cancela");
                //2- Tudo ou Nada");
                //3- Ate cancelar");
                //4- Data especifica");
                //5- Abertura Mercado");
                //6- Fechamento Mercado");
                //7- Boa para Leilao");
                switch (cmbTipoValidade.SelectedIndex)
                {
                    case 0: 
                        ordem.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                        ordem.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    case 1: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela; break;
                    case 2: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela; break;
                    case 3: ordem.TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada; break;
                    case 4:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidoAteDeterminadaData;

                        ordem.ExpireDate = DateTime.ParseExact(txtDataValidade.Text + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        break;

                    case 5: ordem.TimeInForce = OrdemValidadeEnum.ValidaParaAberturaDoMercado; break;
                    case 6: ordem.TimeInForce = OrdemValidadeEnum.FechamentoDoMercado; break;
                    case 7: ordem.TimeInForce = OrdemValidadeEnum.BoaParaLeilao; break;
                    default:
                        MessageBox.Show("Time in force invalido");
                        break;
                }

                switch (version)
                {
                    case 44:
                        _fix44Client.EnviarOrdem(ordem);
                        break;
                    case 42:
                    default:
                        _fix42Client.EnviarOrdem(ordem);
                        break;
                }

                _addMsg("Nova ordem enviada com sucesso v[" + version + "]");

                ofertasenviadas.Add(ordem);

                SerializadorOfertas.SaveOfertas(ofertasenviadas);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                logger.Error("Erro: " + ex.Message, ex);
            }

            lastCLOrdID++;
            txtClOrdID.Text = lastCLOrdID.ToString();
            
        }


        /// <summary>
        /// Cancelamento da ordem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                OrdemCancelamentoInfo ordem = new OrdemCancelamentoInfo();
                ordem.ClOrdID = txtClOrdID.Text;
                ordem.OrigClOrdID = txtOrigOrdID.Text;
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);
                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";
                ordem.OrderID = txtExchangeNumber.Text;
                ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.ExecBroker = txtTraderID.Text;
                ordem.Memo5149 = "Cancelto " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                if (txtSenderLocation.Text.Length > 0)
                    ordem.SenderLocation = txtSenderLocation.Text;

                if (txtExecTrader.Text.Length > 0)
                {
                    ordem.ExecutingTrader = txtExecTrader.Text;
                }


                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);

                switch (version)
                {
                    case 44:
                        _fix44Client.CancelarOrdem(ordem);
                        break;
                    case 42:
                    default:
                        _fix42Client.CancelarOrdem(ordem);
                        break;
                }

                _addMsg("Cancelamento de ordem enviada com sucesso v[" + version + "]");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                logger.Error("Erro: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Alteracao da Ordem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAlterar_Click(object sender, EventArgs e)
        {
            try
            {

                OrdemInfo ordem = new OrdemInfo();

                ordem.OrigClOrdID = txtOrigOrdID.Text;
                ordem.ClOrdID = txtClOrdID.Text;
                ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";

                ordem.ExchangeNumberID = txtExchangeNumber.Text;
                ordem.Price = Convert.ToDouble(txtPreco.Text);
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);
                ordem.MinQty = Convert.ToInt32(txtQtdeMin.Text);
                ordem.MaxFloor = Convert.ToInt32(txtQtdeAparente.Text);

                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.ExecBroker = txtTraderID.Text;
                ordem.Memo5149 = "Alteracao " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                if (txtSenderLocation.Text.Length > 0)
                    ordem.SenderLocation = txtSenderLocation.Text;

                if (txtExecTrader.Text.Length > 0)
                {
                    ordem.ExecutingTrader = txtExecTrader.Text;
                }

                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;

                switch (cmbOrderType.SelectedIndex)
                {
                    case 0: ordem.OrdType = OrdemTipoEnum.Limitada; break;
                    case 1: ordem.OrdType = OrdemTipoEnum.StopLimitada; break;
                    case 2: ordem.OrdType = OrdemTipoEnum.MarketWithLeftOverLimit; break;
                    case 3: ordem.OrdType = OrdemTipoEnum.OnClose; break;
                    case 4: ordem.OrdType = OrdemTipoEnum.StopStart; break;
                    case 5: ordem.OrdType = OrdemTipoEnum.Mercado; break;
                    case 6: ordem.OrdType = OrdemTipoEnum.StopLoss; break;
                    default:
                        ordem.OrdType = OrdemTipoEnum.OnClose; break;
                }


                //0- Para o dia");
                //1- Executa ou cancela");
                //2- Tudo ou Nada");
                //3- Ate cancelar");
                //4- Data especifica");
                //5- Abertura Mercado");
                //6- Fechamento Mercado");
                //7- Boa para Leilao");
                switch (cmbTipoValidade.SelectedIndex)
                {
                    case 0:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                        ordem.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    case 1: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela; break;
                    case 2: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela; break;
                    case 3: ordem.TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada; break;
                    case 4:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidoAteDeterminadaData;

                        ordem.ExpireDate = DateTime.ParseExact(txtDataValidade.Text + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        break;

                    case 5: ordem.TimeInForce = OrdemValidadeEnum.ValidaParaAberturaDoMercado; break;
                    case 6: ordem.TimeInForce = OrdemValidadeEnum.FechamentoDoMercado; break;
                    case 7: ordem.TimeInForce = OrdemValidadeEnum.BoaParaLeilao; break;
                    default:
                        MessageBox.Show("Time in force invalido");
                        break;
                }

                if (txtStopPX.Text.Length > 0 && Convert.ToDouble(txtStopPX.Text) > 0)
                {
                    ordem.StopPrice = Convert.ToDouble(txtStopPX.Text);
                }

                if (txtInvestorID.Text.Length > 0)
                {
                    ordem.InvestorID = txtInvestorID.Text;
                }

                switch (version)
                {
                    case 44:
                        _fix44Client.AlterarOrdem(ordem);
                        break;
                    case 42:
                    default:
                        _fix42Client.AlterarOrdem(ordem);
                        break;
                }

                _addMsg("Alteracao de ordem enviada com sucesso v[" + version + "]");

                ofertasenviadas.Add(ordem);

                SerializadorOfertas.SaveOfertas(ofertasenviadas);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                logger.Error("Erro: " + ex.Message, ex);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _roteador = Ativador.Get<IRoteadorOrdens>();
            lastCLOrdID = Convert.ToInt64(txtClOrdID.Text);

            for (int i = 0; i < 10; i++)
            {
                lastCLOrdID++;

                enviar_ordem(lastCLOrdID);

                Thread.Sleep(1000);
            }

            txtClOrdID.Text = lastCLOrdID.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _roteador = Ativador.Get<IRoteadorOrdens>();
            lastCLOrdID = Convert.ToInt64(txtClOrdID.Text);

            for (int i = 0; i < 10; i++)
            {
                lastCLOrdID++;

                enviar_ordem(lastCLOrdID);
            }

            txtClOrdID.Text = lastCLOrdID.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _roteador = Ativador.Get<IRoteadorOrdens>();
            lastCLOrdID = Convert.ToInt64(txtClOrdID.Text);

            for (int i = 0; i < 50; i++)
            {
                lastCLOrdID++;

                enviar_ordem(lastCLOrdID);
            }

            txtClOrdID.Text = lastCLOrdID.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void enviar_ordem(long last)
        {
            try
            {
                OrdemInfo ordem = new OrdemInfo();

                ordem.ClOrdID = last.ToString();
                ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";

                //ordem.ExchangeNumberID = txtExchangeNumber.Text;
                ordem.Price = Convert.ToDouble(txtPreco.Text);
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);
                ordem.MinQty = Convert.ToInt32(txtQtdeMin.Text);
                ordem.MaxFloor = Convert.ToInt32(txtQtdeAparente.Text);
                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.RegisterTime = DateTime.Now;
                ordem.TransactTime = DateTime.Now;
                ordem.ExecBroker = txtTraderID.Text;


                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;


                switch (cmbOrderType.SelectedIndex)
                {
                    case 0: ordem.OrdType = OrdemTipoEnum.Limitada; break;
                    case 1: ordem.OrdType = OrdemTipoEnum.StopLimitada; break;
                    case 2: ordem.OrdType = OrdemTipoEnum.MarketWithLeftOverLimit; break;
                    case 3: ordem.OrdType = OrdemTipoEnum.OnClose; break;
                    case 4: ordem.OrdType = OrdemTipoEnum.StopStart; break;
                    case 5: ordem.OrdType = OrdemTipoEnum.Mercado; break;
                    case 6: ordem.OrdType = OrdemTipoEnum.StopLoss; break;
                    default:
                        ordem.OrdType = OrdemTipoEnum.OnClose; break;
                }


                switch (cmbTipoValidade.SelectedIndex)
                {
                    case 0: 
                        ordem.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                        ordem.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    case 1: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela; break;
                    case 2: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela; break;
                    case 3: ordem.TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada; break;
                    case 4:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidoAteDeterminadaData;

                        ordem.ExpireDate = DateTime.ParseExact(txtDataValidade.Text + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        break;
                    default:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidaParaAberturaDoMercado;
                        break;
                }


                if (_roteador != null)
                {

                    ThreadPool.QueueUserWorkItem(new WaitCallback(
                     delegate(object required)
                     {
                         ExecutarOrdemRequest request = new ExecutarOrdemRequest();

                         request.info = ordem;

                         ExecutarOrdemResponse resp = _roteador.ExecutarOrdem(request);

                         if (resp.DadosRetorno != null)
                         {
                             string msg = "";

                             foreach (OcorrenciaRoteamentoOrdem ocorr in resp.DadosRetorno.Ocorrencias)
                             {
                                 msg += ocorr.Ocorrencia + "\r\n";
                             }
                             _addMsg(msg);
                         }
                     }));
                }
            }
            catch (Exception ex)
            {
                _addMsg(ex.Message);
                logger.Error("Erro: " + ex.Message, ex);
            }
        }


        private void btnOfertasEnviadas_Click(object sender, EventArgs e)
        {
            frmOfertasEnviadas frm = new frmOfertasEnviadas();

            DialogResult result = frm.ShowDialog();

            if (result == DialogResult.OK)
            {
                _populateFields(frm.SelectedOrder);
            }
        }

        private void _populateFields(OrdemInfo ordem)
        {
            //txtClOrdID.Text = ordem.ClOrdID;
            txtOrigOrdID.Text = ordem.ClOrdID;
            txtCodCliente.Text = ordem.Account.ToString();
            txtOperador.Text = ordem.ChannelID.ToString();

            int index = cmbBolsa.FindString(ordem.Exchange);
            cmbBolsa.SelectedIndex = index;

            txtExchangeNumber.Text = ordem.ExchangeNumberID;
            txtPreco.Text = ordem.Price.ToString();
            txtQtde.Text = ordem.OrderQty.ToString();
            txtQtdeMin.Text = ordem.MinQty.ToString();
            txtQtdeAparente.Text = ordem.MaxFloor.ToString();
            txtPapel.Text = ordem.Symbol;
            txtSecurityId.Text = ordem.SecurityID;

            if (ordem.Side == OrdemDirecaoEnum.Compra)
            {
                rdCompra.Checked = true;
                rdVenda.Checked = false;
            }
            else
            {
                rdCompra.Checked = false;
                rdVenda.Checked = true;
            }

            if (ordem.StopPrice > 0)
                txtStopPX.Text = ordem.StopPrice.ToString();
            else
                txtStopPX.Text = "";


            switch (ordem.OrdType)
            {
                case OrdemTipoEnum.Limitada: cmbOrderType.SelectedIndex = 0; break;
                case OrdemTipoEnum.StopLimitada: cmbOrderType.SelectedIndex = 1; break;
                case OrdemTipoEnum.MarketWithLeftOverLimit: cmbOrderType.SelectedIndex = 2; break;
                case OrdemTipoEnum.OnClose: cmbOrderType.SelectedIndex = 3; break;
                case OrdemTipoEnum.StopStart: cmbOrderType.SelectedIndex = 4; break;
                case OrdemTipoEnum.Mercado: cmbOrderType.SelectedIndex = 5; break;
                case OrdemTipoEnum.StopLoss: cmbOrderType.SelectedIndex = 6; break;
                default:
                    cmbOrderType.SelectedIndex = 7; break;
            }

            switch (ordem.TimeInForce)
            {
                case OrdemValidadeEnum.ValidaParaODia:
                    cmbTipoValidade.SelectedIndex = 0;
                    break;
                case OrdemValidadeEnum.ExecutaIntegralParcialOuCancela: cmbTipoValidade.SelectedIndex = 1; break;
                case OrdemValidadeEnum.ExecutaIntegralOuCancela: cmbTipoValidade.SelectedIndex = 2; break;
                case OrdemValidadeEnum.ValidaAteSerCancelada: cmbTipoValidade.SelectedIndex = 3; break;
                case OrdemValidadeEnum.ValidoAteDeterminadaData:
                    cmbTipoValidade.SelectedIndex = 4;
                    txtDataValidade.Text = ordem.ExpireDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    break;
                default:
                    cmbTipoValidade.SelectedIndex = 5;
                    break;
            }
            
        }

        private void btOrderX_Click(object sender, EventArgs e)
        {
            try
            {
                frmPontaVenda frm = new frmPontaVenda();

                OrdemInfo ordemCompra = new OrdemInfo();

                ordemCompra.ClOrdID = txtClOrdID.Text;
                ordemCompra.Account = Convert.ToInt32(txtCodCliente.Text);
                ordemCompra.ChannelID = Convert.ToInt32(txtOperador.Text);

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordemCompra.Exchange = "BOVESPA";
                else
                    ordemCompra.Exchange = "BMF";

                ordemCompra.ExchangeNumberID = txtExchangeNumber.Text;
                ordemCompra.Price = Convert.ToDouble(txtPreco.Text);
                ordemCompra.OrderQty = Convert.ToInt32(txtQtde.Text);
                ordemCompra.MinQty = Convert.ToInt32(txtQtdeMin.Text);
                ordemCompra.MaxFloor = Convert.ToInt32(txtQtdeAparente.Text);
                ordemCompra.Symbol = txtPapel.Text;
                ordemCompra.SecurityID = txtSecurityId.Text;
                ordemCompra.RegisterTime = DateTime.Now;
                ordemCompra.TransactTime = DateTime.Now;
                ordemCompra.ExecBroker = txtTraderID.Text;

                if (rdCompra.Checked)
                    ordemCompra.Side = OrdemDirecaoEnum.Compra;
                else
                    ordemCompra.Side = OrdemDirecaoEnum.Venda;

                if (txtStopPX.Text.Length > 0 && Convert.ToDouble(txtStopPX.Text) > 0)
                {
                    ordemCompra.StopPrice = Convert.ToDouble(txtStopPX.Text);
                }

                if (txtInvestorID.Text.Length > 0)
                {
                    ordemCompra.InvestorID = txtInvestorID.Text;
                }

                switch (cmbOrderType.SelectedIndex)
                {
                    case 0: ordemCompra.OrdType = OrdemTipoEnum.Limitada; break;
                    case 1: ordemCompra.OrdType = OrdemTipoEnum.StopLimitada; break;
                    case 2: ordemCompra.OrdType = OrdemTipoEnum.MarketWithLeftOverLimit; break;
                    case 3: ordemCompra.OrdType = OrdemTipoEnum.OnClose; break;
                    case 4: ordemCompra.OrdType = OrdemTipoEnum.StopStart; break;
                    case 5: ordemCompra.OrdType = OrdemTipoEnum.Mercado; break;
                    case 6: ordemCompra.OrdType = OrdemTipoEnum.StopLoss; break;
                    default:
                        ordemCompra.OrdType = OrdemTipoEnum.OnClose; break;
                }


                switch (cmbTipoValidade.SelectedIndex)
                {
                    case 0:
                        ordemCompra.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                        ordemCompra.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    case 1: ordemCompra.TimeInForce = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela; break;
                    case 2: ordemCompra.TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela; break;
                    case 3: ordemCompra.TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada; break;
                    case 4:
                        ordemCompra.TimeInForce = OrdemValidadeEnum.ValidoAteDeterminadaData;

                        ordemCompra.ExpireDate = DateTime.ParseExact(txtDataValidade.Text + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        break;
                    default:
                        ordemCompra.TimeInForce = OrdemValidadeEnum.ValidaParaAberturaDoMercado;
                        break;
                }

                lastCLOrdID++;
                frm.ClOrdID = lastCLOrdID.ToString();
                frm.Qtde = txtQtde.Text;

                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    OrdemInfo ordemVenda = RoteadorOrdensUtil.CloneOrder(ordemCompra);

                    ordemVenda.Account = Convert.ToInt32(frm.Account);
                    ordemVenda.OrderQty =  Convert.ToInt32(frm.Qtde);
                    ordemVenda.ClOrdID = frm.ClOrdID;
                    if (frm.InvestorID.Length > 0)
                    {
                        ordemVenda.InvestorID = frm.ClOrdID;
                    }

                    OrdemCrossInfo cross = new OrdemCrossInfo();
                    cross.ChannelID = ordemCompra.ChannelID;
                    cross.CrossID = ordemCompra.ClOrdID + "X";
                    cross.Exchange = ordemCompra.Exchange;
                    cross.OrdType = ordemCompra.OrdType;
                    cross.Price = ordemCompra.Price;
                    cross.SecurityID = ordemCompra.SecurityID;
                    cross.SecurityIDSource = ordemCompra.SecurityIDSource;
                    cross.Symbol = ordemCompra.Symbol;
                    cross.TransactTime = ordemCompra.RegisterTime;

                    cross.OrdemInfoCompra = ordemCompra;
                    cross.OrdemInfoVenda = ordemVenda;

                    cross.Memo5149 = "Cross " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                    IRoteadorOrdens roteador = Ativador.Get<IRoteadorOrdens>();

                    if (roteador != null)
                    {
                        ExecutarOrdemCrossRequest request = new ExecutarOrdemCrossRequest();

                        request.info = cross;

                        ExecutarOrdemCrossResponse resp = roteador.ExecutarOrdemCross(request);

                        if (resp.DadosRetorno != null)
                        {
                            string msg = "";

                            foreach (OcorrenciaRoteamentoOrdem ocorr in resp.DadosRetorno.Ocorrencias)
                            {
                                msg += ocorr.Ocorrencia + "\r\n";
                            }

                            if (resp.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                MessageBox.Show(msg);
                            else
                            {
                                _addMsg(msg);

                                ofertasenviadas.Add(ordemCompra);

                                SerializadorOfertas.SaveOfertas(ofertasenviadas);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _addMsg(ex.Message);
                logger.Error("Erro: " + ex.Message, ex);
            }

            lastCLOrdID++;
            txtClOrdID.Text = lastCLOrdID.ToString();
        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            _roteador = Ativador.Get<IRoteadorOrdens>();
            lastCLOrdID = Convert.ToInt64(txtClOrdID.Text);

            for( int j=0; j < 30; j++ )
            {
                for (int i = 0; i < 10; i++)
                {
                    lastCLOrdID++;

                    enviar_ordem(lastCLOrdID);

                    System.Windows.Forms.Application.DoEvents();
                }
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(500);
                System.Windows.Forms.Application.DoEvents();
            }

            txtClOrdID.Text = lastCLOrdID.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btClient42.Enabled = true;
            btClient44.Enabled = true;
            btStopFixClients.Enabled = false;

            // Cria dicionario da configuracao 
            _mainDic = new Dictionary();

            _mainDic.SetString("SocketConnectHost", "10.0.11.152");
            _mainDic.SetLong("SocketConnectPort", 61000);
            _mainDic.SetLong("HeartBtInt", 30);
            _mainDic.SetLong("ReconnectInterval", 5);

            _mainDic.SetString("FileStorePath", ".\\FixStore");

            _mainDic.SetString("FileLogPath", ".\\FixAudit");
            _mainDic.SetString("StartTime", "00:00:00");
            _mainDic.SetString("EndTime", "23:59:59");
            _mainDic.SetString("ConnectionType", "initiator");

            _sender = ConfigurationManager.AppSettings["Sender"].ToString();
            _sendersub = ConfigurationManager.AppSettings["SenderSub"].ToString();
            _target = ConfigurationManager.AppSettings["Target"].ToString();
            _targetsub = ConfigurationManager.AppSettings["TargetSub"].ToString();
            _pwd = ConfigurationManager.AppSettings["Pwd"].ToString();
            _host = ConfigurationManager.AppSettings["Host"].ToString();
            _port = ConfigurationManager.AppSettings["Port"].ToString();
            _dictionary = ConfigurationManager.AppSettings["Dictionary"].ToString();
            _useDictionary = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDictionary"]);

            txtSender.Text = _sender;
            txtTarget.Text = _target;
            txtTargetSubID.Text = _targetsub;
            txtSenderSubID.Text = _sendersub;
            txtPwd.Text = _pwd;
            txtIP.Text = _host;
            txtPort.Text = _port;
            txtTimes.Text = ConfigurationManager.AppSettings["NumOrders"].ToString();
            txtInterval.Text = ConfigurationManager.AppSettings["Intervalo"].ToString();

            comboBox2.Text = ConfigurationManager.AppSettings["Version"].ToString();
            _version = ConfigurationManager.AppSettings["Version"].ToString();
            
            string[] arr = Environment.GetCommandLineArgs();
            // if automatic, get infos from parameters
            if (arr.Length > 1)
            {
                if (arr.Length != 12)
                {
                    MessageBox.Show("Numero de parametros invalido");
                    return;
                }
                if (arr[1] != "-a")
                    return;
                txtTarget.Text = arr[3];
                txtSender.Text = arr[5];
                txtPwd.Text = arr[7];
                txtIP.Text = arr[9];
                txtPort.Text = arr[11];
                _atualizaReport("Modo automatico ativado");
                timer1.Interval = Convert.ToInt32(txtInterval.Text) * 1000;
                timer1.Start();
                btConnect.PerformClick();
            }

        }

        private void btClient42_Click(object sender, EventArgs e)
        {
            btClient42.BackColor = Form.DefaultBackColor;
            btClient44.BackColor = Form.DefaultBackColor;
            btClient42.Enabled = true;
            btClient44.Enabled = false;

            // Configure the session settings
            SessionSettings settings = new SessionSettings();

            settings.Set(_mainDic);

            Dictionary sessDic = new Dictionary();

            sessDic.SetString("DataDictionary", ".\\Dictionary\\FIX42.xml");
            sessDic.SetBool("UseDataDictionary", true);
            sessDic.SetBool("ResetOnLogon", true);
            sessDic.SetBool("PersistMessages", true);

            _session = new SessionID("FIX.4.2",
                    ConfigurationManager.AppSettings["SenderCompID42"].ToString(),
                    ConfigurationManager.AppSettings["TargerCompID42"].ToString());

            settings.Set(_session, sessDic);

            FileStoreFactory store = new FileStoreFactory(settings);
            FileLogFactory logs = new FileLogFactory(settings);

            IMessageFactory msgs = new DefaultMessageFactory();

            _fix42Client = new FIX42Client(this);

            _initiator = new QuickFix.Transport.SocketInitiator(_fix42Client, store, settings, logs, msgs);
            _initiator.Start();

            btClient42.Enabled = false;
            btClient44.Enabled = false;
            btStopFixClients.Enabled = true;
            btClient42.BackColor = Form.DefaultBackColor;
            btClient44.BackColor = Form.DefaultBackColor;
            version = 42;
        }

        private void btClient44_Click(object sender, EventArgs e)
        {
            btClient42.BackColor = Form.DefaultBackColor;
            btClient44.BackColor = Form.DefaultBackColor;
            btClient42.Enabled = false;
            btClient44.Enabled = true;

            // Configure the session settings
            SessionSettings settings = new SessionSettings();

            settings.Set(_mainDic);

            Dictionary sessDic = new Dictionary();

            sessDic.SetString("DataDictionary", ".\\Dictionary\\FIX44.xml");
            sessDic.SetBool("UseDataDictionary", true);
            sessDic.SetBool("ResetOnLogon", true);
            sessDic.SetBool("PersistMessages", true);

            _session = new SessionID("FIX.4.4",
                    ConfigurationManager.AppSettings["SenderCompID44"].ToString(),
                    ConfigurationManager.AppSettings["TargerCompID44"].ToString());

            settings.Set(_session, sessDic);

            FileStoreFactory store = new FileStoreFactory(settings);
            FileLogFactory logs = new FileLogFactory(settings);
            IMessageFactory msgs = new DefaultMessageFactory();

            _fix44Client = new FIX44Client(this);

            _initiator = new QuickFix.Transport.SocketInitiator(_fix44Client, store, settings, logs, msgs);
            _initiator.Start();

            btClient42.Enabled = false;
            btClient44.Enabled = false;
            btStopFixClients.Enabled = true;
            btClient42.BackColor = Form.DefaultBackColor;
            btClient44.BackColor = Form.DefaultBackColor;
            version = 44;

        }

        private void btStopFixClients_Click(object sender, EventArgs e)
        {
            
            btStopFixClients.BackColor = Color.Yellow;

            if (_initiator != null && _initiator.IsStopped == false)
                _initiator.Stop();

            int i = 0;
            while ( !_initiator.IsStopped && i < 60 )
            {
                Thread.Sleep(1000);
                i++;
            }

            _initiator = null;

            btClient42.BackColor = Form.DefaultBackColor;
            btClient44.BackColor = Form.DefaultBackColor;
            btClient42.Enabled = true;
            btClient44.Enabled = true;
            btStopFixClients.BackColor = Form.DefaultBackColor;
            version = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cortex.OMS.ServidorFIXAdm.Lib.IServidorFixAdmin fixadmin = Ativador.Get<Cortex.OMS.ServidorFIXAdm.Lib.IServidorFixAdmin>();

            fixadmin.ReloadConfiguration(new Cortex.OMS.ServidorFIXAdm.Lib.Mensagens.ReloadConfigRequest());
        }

        private void btDummyBL_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DateTime.Now.ToString("HH:mm:ss.ffffff"));
        }

        private void btEnviarBL_Click(object sender, EventArgs e)
        {
            try
            {
                string strMnemonico = string.Empty;
                if (!string.IsNullOrEmpty(txtTimes.Text))
                {
                    long aux1 = Convert.ToInt64(DateTime.Now.ToString("MMddhhmmssffffff"));
                    
                    txtClOrdIni.Text = aux1.ToString();
                    txtClOrdFim.Text = (aux1+Convert.ToInt32(txtTimes.Text)).ToString();
                    _atualizaReport("New Order ClOrdID Ini: " + txtClOrdIni.Text);
                    _atualizaReport("New Order ClOrdID Fim: " + txtClOrdFim.Text);
                }
                OrdemInfo ordem = new OrdemInfo();
                ordem.ClOrdID = txtClOrdID.Text;
                try
                {
                    ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                }
                catch
                {
                    // nao numerico = mnemonico
                    strMnemonico = txtCodCliente.Text;
                }
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";

                ordem.ExchangeNumberID = txtExchangeNumber.Text;
                ordem.Price = Convert.ToDouble(txtPreco.Text);
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);
                ordem.MinQty = Convert.ToInt32(txtQtdeMin.Text);
                ordem.MaxFloor = Convert.ToInt32(txtQtdeAparente.Text);
                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.RegisterTime = DateTime.Now;
                ordem.TransactTime = DateTime.Now;
                ordem.ExecBroker = txtTraderID.Text;
                ordem.Memo5149 = "Nova " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                if (txtSenderLocation.Text.Length > 0)
                    ordem.SenderLocation = txtSenderLocation.Text;

                if (chkAccountType.Checked)
                    ordem.AcountType = ContaTipoEnum.GIVE_UP_LINK_IDENTIFIER;

                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;

                if (txtStopPX.Text.Length > 0 && Convert.ToDouble(txtStopPX.Text) > 0)
                {
                    ordem.StopPrice = Convert.ToDouble(txtStopPX.Text);
                }

                if (txtInvestorID.Text.Length > 0)
                {
                    ordem.InvestorID = txtInvestorID.Text;
                }

                if (txtExecTrader.Text.Length > 0)
                {
                    ordem.ExecutingTrader = txtExecTrader.Text;
                }

                switch (cmbOrderType.SelectedIndex)
                {
                    case 0: ordem.OrdType = OrdemTipoEnum.Limitada; break;
                    case 1: ordem.OrdType = OrdemTipoEnum.StopLimitada; break;
                    case 2: ordem.OrdType = OrdemTipoEnum.MarketWithLeftOverLimit; break;
                    case 3: ordem.OrdType = OrdemTipoEnum.OnClose; break;
                    case 4: ordem.OrdType = OrdemTipoEnum.StopStart; break;
                    case 5: ordem.OrdType = OrdemTipoEnum.Mercado; break;
                    case 6: ordem.OrdType = OrdemTipoEnum.StopLoss; break;
                    default:
                        ordem.OrdType = OrdemTipoEnum.OnClose; break;
                }

                //0- Para o dia");
                //1- Executa ou cancela");
                //2- Tudo ou Nada");
                //3- Ate cancelar");
                //4- Data especifica");
                //5- Abertura Mercado");
                //6- Fechamento Mercado");
                //7- Boa para Leilao");
                switch (cmbTipoValidade.SelectedIndex)
                {
                    case 0: 
                        ordem.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                        ordem.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    case 1: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela; break;
                    case 2: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela; break;
                    case 3: ordem.TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada; break;
                    case 4:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidoAteDeterminadaData;

                        ordem.ExpireDate = DateTime.ParseExact(txtDataValidade.Text + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        break;

                    case 5: ordem.TimeInForce = OrdemValidadeEnum.ValidaParaAberturaDoMercado; break;
                    case 6: ordem.TimeInForce = OrdemValidadeEnum.FechamentoDoMercado; break;
                    case 7: ordem.TimeInForce = OrdemValidadeEnum.BoaParaLeilao; break;
                    default:
                        MessageBox.Show("Time in force invalido");
                        break;
                }

                FIX44EntryPointClient aux = null;

                logger.Info(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " ******** Nova ordem enviada INICIO ====> Regs: " + txtTimes.Text);

                if (_dicExecutors.TryGetValue(txtSender.Text, out aux))
                {
                    if (txtTimes.Text.Equals(string.Empty))
                        aux.EnviarOrdem(ordem);
                    else
                        aux.EnviarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonico);
                        // aux.EnviarOrdem(ordem, Convert.ToInt32(txtTimes.Text));
                }
                else
                {
                    _addMsg("Executor: " + txtSender.Text + " not found");
                    return;
                }
                logger.Info(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " ******** Nova ordem enviada FIM ====> Regs:" + txtTimes.Text);
                //_addMsg("Nova ordem enviada com sucesso v[" + "f44EP" + "]");

                //ofertasenviadas.Add(ordem);

                //SerializadorOfertas.SaveOfertas(ofertasenviadas);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                logger.Error("Erro: " + ex.Message, ex);
            }

            lastCLOrdID++;
            txtClOrdID.Text = lastCLOrdID.ToString();
            
        }

        private void btOrderX_BL_Click(object sender, EventArgs e)
        {
            try
            {
                frmPontaVenda frm = new frmPontaVenda();
                OrdemInfo ordemCompra = new OrdemInfo();
                ordemCompra.ClOrdID = txtClOrdID.Text;
                ordemCompra.Account = Convert.ToInt32(txtCodCliente.Text);
                ordemCompra.ChannelID = Convert.ToInt32(txtOperador.Text);

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordemCompra.Exchange = "BOVESPA";
                else
                    ordemCompra.Exchange = "BMF";

                ordemCompra.ExchangeNumberID = txtExchangeNumber.Text;
                ordemCompra.Price = Convert.ToDouble(txtPreco.Text);
                ordemCompra.OrderQty = Convert.ToInt32(txtQtde.Text);
                ordemCompra.MinQty = Convert.ToInt32(txtQtdeMin.Text);
                ordemCompra.MaxFloor = Convert.ToInt32(txtQtdeAparente.Text);
                ordemCompra.Symbol = txtPapel.Text;
                ordemCompra.SecurityID = txtSecurityId.Text;
                ordemCompra.RegisterTime = DateTime.Now;
                ordemCompra.TransactTime = DateTime.Now;
                ordemCompra.ExecBroker = txtTraderID.Text;

                if (rdCompra.Checked)
                    ordemCompra.Side = OrdemDirecaoEnum.Compra;
                else
                    ordemCompra.Side = OrdemDirecaoEnum.Venda;

                if (txtStopPX.Text.Length > 0 && Convert.ToDouble(txtStopPX.Text) > 0)
                {
                    ordemCompra.StopPrice = Convert.ToDouble(txtStopPX.Text);
                }

                if (txtInvestorID.Text.Length > 0)
                {
                    ordemCompra.InvestorID = txtInvestorID.Text;
                }

                switch (cmbOrderType.SelectedIndex)
                {
                    case 0: ordemCompra.OrdType = OrdemTipoEnum.Limitada; break;
                    case 1: ordemCompra.OrdType = OrdemTipoEnum.StopLimitada; break;
                    case 2: ordemCompra.OrdType = OrdemTipoEnum.MarketWithLeftOverLimit; break;
                    case 3: ordemCompra.OrdType = OrdemTipoEnum.OnClose; break;
                    case 4: ordemCompra.OrdType = OrdemTipoEnum.StopStart; break;
                    case 5: ordemCompra.OrdType = OrdemTipoEnum.Mercado; break;
                    case 6: ordemCompra.OrdType = OrdemTipoEnum.StopLoss; break;
                    default:
                        ordemCompra.OrdType = OrdemTipoEnum.OnClose; break;
                }


                switch (cmbTipoValidade.SelectedIndex)
                {
                    case 0:
                        ordemCompra.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                        ordemCompra.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    case 1: ordemCompra.TimeInForce = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela; break;
                    case 2: ordemCompra.TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela; break;
                    case 3: ordemCompra.TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada; break;
                    case 4:
                        ordemCompra.TimeInForce = OrdemValidadeEnum.ValidoAteDeterminadaData;

                        ordemCompra.ExpireDate = DateTime.ParseExact(txtDataValidade.Text + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        break;
                    default:
                        ordemCompra.TimeInForce = OrdemValidadeEnum.ValidaParaAberturaDoMercado;
                        break;
                }

                lastCLOrdID++;
                frm.ClOrdID = lastCLOrdID.ToString();
                frm.Qtde = txtQtde.Text;

                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    OrdemInfo ordemVenda = RoteadorOrdensUtil.CloneOrder(ordemCompra);

                    ordemVenda.Account = Convert.ToInt32(frm.Account);
                    ordemVenda.OrderQty = Convert.ToInt32(frm.Qtde);
                    ordemVenda.ClOrdID = frm.ClOrdID;
                    if (frm.InvestorID.Length > 0)
                    {
                        ordemVenda.InvestorID = frm.ClOrdID;
                    }

                    OrdemCrossInfo cross = new OrdemCrossInfo();
                    cross.ChannelID = ordemCompra.ChannelID;
                    cross.CrossID = ordemCompra.ClOrdID + "X";
                    cross.Exchange = ordemCompra.Exchange;
                    cross.OrdType = ordemCompra.OrdType;
                    cross.Price = ordemCompra.Price;
                    cross.SecurityID = ordemCompra.SecurityID;
                    cross.SecurityIDSource = ordemCompra.SecurityIDSource;
                    cross.Symbol = ordemCompra.Symbol;
                    cross.TransactTime = ordemCompra.RegisterTime;

                    cross.OrdemInfoCompra = ordemCompra;
                    cross.OrdemInfoVenda = ordemVenda;

                    cross.Memo5149 = "Cross " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");
                    FIX44EntryPointClient aux = null;
                    if (_dicExecutors.TryGetValue(txtSender.Text, out aux))
                    {
                        //if (txtTimes.Text.Equals(string.Empty))
                            aux.EnviarOrdemCross(cross);
                        //else
                        //    aux.EnviarOrdem(cross, Convert.ToInt32(txtTimes.Text));
                    }
                    else
                    {
                        _addMsg("Executor: " + txtSender.Text + " not found");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _addMsg(ex.Message);
                logger.Error("Erro: " + ex.Message, ex);
            }

            lastCLOrdID++;
            txtClOrdID.Text = lastCLOrdID.ToString();
        }

        private void btAlterarBL_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTimes.Text))
                {
                    long aux1 = Convert.ToInt64(DateTime.Now.ToString("MMddhhmmssffffff"));

                    txtClOrdIni.Text = aux1.ToString();
                    txtClOrdFim.Text = (aux1 + Convert.ToInt32(txtTimes.Text)).ToString();
                    _atualizaReport("Alter Order ClOrdID Ini: " + txtClOrdIni.Text);
                    _atualizaReport("Alter Order ClOrdID Fim: " + txtClOrdFim.Text);
                }
                
                
                OrdemInfo ordem = new OrdemInfo();
                ordem.OrigClOrdID = txtOrigOrdID.Text;
                ordem.ClOrdID = txtClOrdID.Text;
                string strMnemonic = string.Empty;
                try
                {
                    ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                }
                catch
                {
                    strMnemonic = txtCodCliente.Text;
                }
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";

                ordem.ExchangeNumberID = txtExchangeNumber.Text;
                ordem.Price = Convert.ToDouble(txtPreco.Text);
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);
                ordem.MinQty = Convert.ToInt32(txtQtdeMin.Text);
                ordem.MaxFloor = Convert.ToInt32(txtQtdeAparente.Text);

                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.ExecBroker = txtTraderID.Text;
                ordem.Memo5149 = "Alteracao " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                if (txtSenderLocation.Text.Length > 0)
                    ordem.SenderLocation = txtSenderLocation.Text;

                if (txtExecTrader.Text.Length > 0)
                {
                    ordem.ExecutingTrader = txtExecTrader.Text;
                }

                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;

                switch (cmbOrderType.SelectedIndex)
                {
                    case 0: ordem.OrdType = OrdemTipoEnum.Limitada; break;
                    case 1: ordem.OrdType = OrdemTipoEnum.StopLimitada; break;
                    case 2: ordem.OrdType = OrdemTipoEnum.MarketWithLeftOverLimit; break;
                    case 3: ordem.OrdType = OrdemTipoEnum.OnClose; break;
                    case 4: ordem.OrdType = OrdemTipoEnum.StopStart; break;
                    case 5: ordem.OrdType = OrdemTipoEnum.Mercado; break;
                    case 6: ordem.OrdType = OrdemTipoEnum.StopLoss; break;
                    default:
                        ordem.OrdType = OrdemTipoEnum.OnClose; break;
                }


                //0- Para o dia");
                //1- Executa ou cancela");
                //2- Tudo ou Nada");
                //3- Ate cancelar");
                //4- Data especifica");
                //5- Abertura Mercado");
                //6- Fechamento Mercado");
                //7- Boa para Leilao");
                switch (cmbTipoValidade.SelectedIndex)
                {
                    case 0:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                        ordem.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    case 1: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela; break;
                    case 2: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela; break;
                    case 3: ordem.TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada; break;
                    case 4:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidoAteDeterminadaData;

                        ordem.ExpireDate = DateTime.ParseExact(txtDataValidade.Text + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        break;

                    case 5: ordem.TimeInForce = OrdemValidadeEnum.ValidaParaAberturaDoMercado; break;
                    case 6: ordem.TimeInForce = OrdemValidadeEnum.FechamentoDoMercado; break;
                    case 7: ordem.TimeInForce = OrdemValidadeEnum.BoaParaLeilao; break;
                    default:
                        MessageBox.Show("Time in force invalido");
                        break;
                }

                if (txtStopPX.Text.Length > 0 && Convert.ToDouble(txtStopPX.Text) > 0)
                {
                    ordem.StopPrice = Convert.ToDouble(txtStopPX.Text);
                }

                if (txtInvestorID.Text.Length > 0)
                {
                    ordem.InvestorID = txtInvestorID.Text;
                }
                FIX44EntryPointClient aux = null;
                if (_dicExecutors.TryGetValue(txtSender.Text, out aux))
                {
                    
                    if (string.IsNullOrEmpty(txtTimes.Text))
                        aux.AlterarOrdem(ordem);
                    else
                        aux.AlterarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text),
                                         Convert.ToInt64(txtOriIni.Text), Convert.ToInt64(txtOriFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic);
                }
                else
                {
                    _addMsg("Executor: " + txtSender.Text + " not found");
                    return;
                }
                //_addMsg("Alteracao de ordem enviada com sucesso v[" + "f44EP" + "]");
                lastCLOrdID++;
                txtClOrdID.Text = lastCLOrdID.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                logger.Error("Erro: " + ex.Message, ex);
            }
        }

        private void btCancelarBL_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTimes.Text))
                {
                    long aux1 = Convert.ToInt64(DateTime.Now.ToString("MMddhhmmssffffff"));

                    txtClOrdIni.Text = aux1.ToString();
                    txtClOrdFim.Text = (aux1 + Convert.ToInt32(txtTimes.Text)).ToString();
                    _atualizaReport("Cancel Order ClOrdID Ini: " + txtClOrdIni.Text);
                    _atualizaReport("Cancel Order ClOrdID Fim: " + txtClOrdFim.Text);
                }
                
                OrdemCancelamentoInfo ordem = new OrdemCancelamentoInfo();
                ordem.ClOrdID = txtClOrdID.Text;
                ordem.OrigClOrdID = txtOrigOrdID.Text;
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);
                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";
                ordem.OrderID = txtExchangeNumber.Text;
                string strMnemonic = string.Empty;
                try
                {
                    ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                }
                catch
                {
                    strMnemonic = txtCodCliente.Text;
                }
                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.ExecBroker = txtTraderID.Text;
                ordem.Memo5149 = "Cancelto " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                if (txtSenderLocation.Text.Length > 0)
                    ordem.SenderLocation = txtSenderLocation.Text;

                if (txtExecTrader.Text.Length > 0)
                {
                    ordem.ExecutingTrader = txtExecTrader.Text;
                }


                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);
                FIX44EntryPointClient aux = null;

                if (_dicExecutors.TryGetValue(txtSender.Text, out aux))
                {
                    if (string.IsNullOrEmpty(txtTimes.Text))
                        aux.CancelarOrdem(ordem);
                    else
                        aux.CancelarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text),
                                          Convert.ToInt64(txtOriIni.Text), Convert.ToInt64(txtOriFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic);
                }
                else
                {
                    _addMsg("Executor: " + txtSender.Text + " not found");
                    return;
                }

                //_addMsg("Cancelamento de ordem enviada com sucesso v[" + "f44EP" + "]");
                lastCLOrdID++;
                txtClOrdID.Text = lastCLOrdID.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                logger.Error("Erro: " + ex.Message, ex);
            }
        }


        Dictionary<string, QuickFix.Transport.SocketInitiator> _dicConnections = new Dictionary<string,QuickFix.Transport.SocketInitiator>();
        Dictionary<string, FIX44EntryPointClient> _dicExecutors = new Dictionary<string, FIX44EntryPointClient>();
        Dictionary<string, FIX42BloombergClient> _dicExecutors42Bb = new Dictionary<string, FIX42BloombergClient>();
        Dictionary<string, FIX42BnpClient> _dicExecutors42BNP = new Dictionary<string, FIX42BnpClient>();
        Dictionary<string, FIX42RealTickClient> _dicExecutors42RTK = new Dictionary<string, FIX42RealTickClient>();
        Dictionary<string, object> _dicExecutors42 = new Dictionary<string, object>();
        private void btConnect_Click(object sender, EventArgs e)
        {

            //try
            //{
                // Configure the session settings
                SessionSettings settings = new QuickFix.SessionSettings();
                
                settings.Set(_mainDic);

                Dictionary sessDic = new Dictionary();
                string strDictionary = string.Empty;
                string strBeginString = string.Empty;
                
                if (_version.IndexOf("FIX.4.4") >= 0)
                    strBeginString = "FIX.4.4";
                if (_version.IndexOf("FIX.4.2") >= 0)
                    strBeginString = "FIX.4.2";

                sessDic.SetString("DataDictionary", _dictionary);
                sessDic.SetBool("UseDataDictionary", _useDictionary);
                sessDic.SetBool("ResetOnLogon", false);
                sessDic.SetBool("ResetOnLogout", false);
                sessDic.SetBool("ResetOnDisconnect", false);    
                sessDic.SetBool("PersistMessages", false);
                sessDic.SetLong("ReconnectInterval", 5);
                sessDic.SetString("SocketConnectHost", txtIP.Text);
                sessDic.SetString("SocketConnectPort", txtPort.Text);

                
                if (_dicConnections.ContainsKey(txtSender.Text))
                {
                    MessageBox.Show("Sessao ja existe no dicionario");
                    return;
                }
                if ( (!string.IsNullOrEmpty(txtSenderSubID.Text)) && (!string.IsNullOrEmpty(txtTargetSubID.Text)) )
                    _session = new SessionID(strBeginString, txtSender.Text, txtSenderSubID.Text, txtTarget.Text, txtTargetSubID.Text);
                else
                    _session = new SessionID(strBeginString, txtSender.Text, txtTarget.Text);

                logger.Info("Session ID: " + _session.ToString());
                //ConfigurationManager.AppSettings["TargerCompID44"].ToString());
                settings.Set(_session, sessDic);

                FileStoreFactory store = new FileStoreFactory(settings);
                FileLogFactory logs = new FileLogFactory(settings);
                IMessageFactory msgs = new DefaultMessageFactory();

                FIX44EntryPointClient fixClient44EP = null;
                FIX42BloombergClient fixCliente42BB = null;
                FIX42BnpClient fixCliente42BNP = null;
                FIX42RealTickClient fixCliente42RTK = null;
                if (comboBox2.Text.Equals("FIX.4.2BB"))
                {
                    fixCliente42BB = new FIX42BloombergClient(this);
                    fixCliente42BB.Password = txtPwd.Text;
                    fixCliente42BB.CancelOnDisconnect = "3";
                    fixCliente42BB.ResetOnLogon = false;
                    _initiator = new QuickFix.Transport.SocketInitiator(fixCliente42BB, store, settings, logs, msgs);
                    // _dicExecutors42Bb.Add(txtSender.Text, fixCliente42BB);
                    _dicExecutors42.Add(txtSender.Text, fixCliente42BB);
                }
                
                if (comboBox2.Text.Equals("FIX.4.2BNP"))
                {
                    fixCliente42BNP = new FIX42BnpClient(this);
                    fixCliente42BNP.Password = txtPwd.Text;
                    fixCliente42BNP.CancelOnDisconnect = "3";
                    fixCliente42BNP.ResetOnLogon = false;
                    _initiator = new QuickFix.Transport.SocketInitiator(fixCliente42BNP, store, settings, logs, msgs);
                    // _dicExecutors42BNP.Add(txtSender.Text, fixCliente42BNP);
                    _dicExecutors42.Add(txtSender.Text, fixCliente42BNP);
                }

                if (comboBox2.Text.Equals("FIX.4.2RTK"))
                {
                    fixCliente42RTK = new FIX42RealTickClient(this);
                    fixCliente42RTK.Password = txtPwd.Text;
                    fixCliente42RTK.CancelOnDisconnect = "3";
                    fixCliente42RTK.ResetOnLogon = false;
                    _initiator = new QuickFix.Transport.SocketInitiator(fixCliente42RTK, store, settings, logs, msgs);
                    // _dicExecutors42RTK.Add(txtSender.Text, fixCliente42RTK);
                    _dicExecutors42.Add(txtSender.Text, fixCliente42RTK);
                }

                if (comboBox2.Text.Equals("FIX.4.4EP"))
                {
                    fixClient44EP = new FIX44EntryPointClient(this);
                    fixClient44EP.Password = txtPwd.Text;
                    fixClient44EP.CancelOnDisconnect = "3";
                    _initiator = new QuickFix.Transport.SocketInitiator(fixClient44EP, store, settings, logs, msgs);
                    _dicExecutors.Add(txtSender.Text, fixClient44EP);
                }
                
                //QuickFix.Transport.SocketInitiator init = new QuickFix.Transport.SocketInitiator(fixClient, store, settings, logs, msgs);
                _dicConnections.Add(txtSender.Text, _initiator);
                _dicConnections[txtSender.Text].Start();

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Exception: " + ex.StackTrace + " " + ex.Message);
            //}

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!_dicConnections.ContainsKey(txtSender.Text))
                {
                    MessageBox.Show(string.Format("Sessao {0} nao existe no dicionario", txtSender.Text));
                    return;
                }
                QuickFix.Transport.SocketInitiator item = null;
                if (_dicConnections.TryGetValue(txtSender.Text, out item))
                {
                    if (item != null && item.IsStopped == false)
                    {
                        item.Stop();
                    }
                    _dicConnections.Remove(txtSender.Text);
                }
                FIX44EntryPointClient fixClient = null;
                object fixClient42 = null;
                if (_dicExecutors.TryGetValue(txtSender.Text, out fixClient))
                {
                    fixClient = null;
                    _dicExecutors.Remove(txtSender.Text);
                }

                if (_dicExecutors42.TryGetValue(txtSender.Text, out fixClient42))
                {
                    fixClient42 = null;
                    _dicExecutors42.Remove(txtSender.Text);
                }

                int i = 0;
                while (!item.IsStopped && i < 60)
                {
                    Thread.Sleep(1000);
                    i++;
                }
                item = null;
            }
            catch (Exception ex)
            {
                logger.Error("Erro", ex);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer1 != null)
                timer1.Stop();
            foreach (KeyValuePair<string, QuickFix.Transport.SocketInitiator> item in _dicConnections)
            {
                QuickFix.Transport.SocketInitiator xx = item.Value;
                if (xx != null && xx.IsStopped == false)
                {
                    xx.Stop();
                }
            }
            System.Windows.Forms.Application.ExitThread();
            System.Windows.Forms.Application.Exit();
        }

  
        

        /// <summary>
        /// Timer para execucao automatica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("aqui");
            ExecutaModificaCancela44EP();
        }

        int _totalOrdens = 0;
        private void ExecutaModificaCancela44EP()
        {
            try
            {
                if (_running)
                {
                    _atualizaReport("ERRO: Já em execucao!");
                    return;
                }
                FIX44EntryPointClient fx44EP = null;

                if (!_dicExecutors.TryGetValue(txtSender.Text, out fx44EP))
                {
                    _atualizaReport("ERRO: Executor não existe no dicionario");
                    return;
                }
                if (!fx44EP.Connected)
                {
                    _atualizaReport("ERRO: Sessao não conectada");
                    return;
                }
                _running = true;
                _atualizaReport("========================================");

                _atualizaReport("Enviando " + txtTimes.Text + " ordens");
                txtQtde.Text = "1";
                btEnviarBL_Click(null, null);
                _totalOrdens += Convert.ToInt32(txtTimes.Text);
                txtOriIni.Text = txtClOrdIni.Text;
                txtOriFim.Text = txtClOrdFim.Text;
                txtQtde.Text = "2";
                _atualizaReport("Alterando " + txtTimes.Text + " ordens");
                btAlterarBL_Click(null, null);//.PerformClick();
                _totalOrdens += Convert.ToInt32(txtTimes.Text);
                txtOriIni.Text = txtClOrdIni.Text;
                txtOriFim.Text = txtClOrdFim.Text;
                _atualizaReport("Cancelando " + txtTimes.Text + " ordens");
                btCancelarBL_Click(null, null);//.PerformClick();
                _totalOrdens += Convert.ToInt32(txtTimes.Text);
                label33.Text = _totalOrdens.ToString();
                _running = false;
            }
            catch (Exception ex)
            {
                _running = false;
                logger.Error("ExecutaModificaCancela(): " + ex.Message, ex);
            }
        }

        private void ExecutaModificaCancela42BB()
        {
            try
            {
                if (_running)
                {
                    _atualizaReport("ERRO: Já em execucao!");
                    return;
                }
                object fx42BB = null;
                FIX42BloombergClient fix = null;
                if (!_dicExecutors42.TryGetValue(txtSender.Text, out fx42BB))
                {
                    _atualizaReport("ERRO: Executor não existe no dicionario");
                    return;
                }
                fix = fx42BB as FIX42BloombergClient;

                if (!fix.Connected)
                {
                    _atualizaReport("ERRO: Sessao não conectada");
                    return;
                }
                _running = true;
                _atualizaReport("========================================");

                _atualizaReport("Enviando " + txtTimes.Text + " ordens");
                txtQtde.Text = "1";
                btEnviar42_Click(null, null);
                _totalOrdens += Convert.ToInt32(txtTimes.Text);
                txtOriIni.Text = txtClOrdIni.Text;
                txtOriFim.Text = txtClOrdFim.Text;
                txtQtde.Text = "2";
                _atualizaReport("Alterando " + txtTimes.Text + " ordens");
                btAlterar42_Click(null, null);//.PerformClick();
                _totalOrdens += Convert.ToInt32(txtTimes.Text);
                txtOriIni.Text = txtClOrdIni.Text;
                txtOriFim.Text = txtClOrdFim.Text;
                _atualizaReport("Cancelando " + txtTimes.Text + " ordens");
                btCancelar42_Click(null, null);//.PerformClick();
                _totalOrdens += Convert.ToInt32(txtTimes.Text);
                label33.Text = _totalOrdens.ToString();
                _running = false;
            }
            catch (Exception ex)
            {
                _running = false;
                logger.Error("ExecutaModificaCancela42BB(): " + ex.Message, ex);
            }
        }

        private void ExecutaModificaCancela42BNP()
        {
            try
            {
                if (_running)
                {
                    _atualizaReport("ERRO: Já em execucao!");
                    return;
                }
                object fx42BNP = null;

                if (!_dicExecutors42.TryGetValue(txtSender.Text, out fx42BNP))
                {
                    _atualizaReport("ERRO: Executor não existe no dicionario");
                    return;
                }
                FIX42BnpClient fix = fx42BNP as FIX42BnpClient;
                if (!fix.Connected)
                {
                    _atualizaReport("ERRO: Sessao não conectada");
                    return;
                }
                _running = true;
                _atualizaReport("========================================");

                _atualizaReport("Enviando " + txtTimes.Text + " ordens");
                txtQtde.Text = "1";
                btEnviar42_Click(null, null);
                _totalOrdens += Convert.ToInt32(txtTimes.Text);
                txtOriIni.Text = txtClOrdIni.Text;
                txtOriFim.Text = txtClOrdFim.Text;
                txtQtde.Text = "2";
                _atualizaReport("Alterando " + txtTimes.Text + " ordens");
                btAlterar42_Click(null, null);//.PerformClick();
                _totalOrdens += Convert.ToInt32(txtTimes.Text);
                txtOriIni.Text = txtClOrdIni.Text;
                txtOriFim.Text = txtClOrdFim.Text;
                _atualizaReport("Cancelando " + txtTimes.Text + " ordens");
                btCancelar42_Click(null, null);//.PerformClick();
                _totalOrdens += Convert.ToInt32(txtTimes.Text);
                label33.Text = _totalOrdens.ToString();
                _running = false;
            }
            catch (Exception ex)
            {
                _running = false;
                logger.Error("ExecutaModificaCancela42BNB(): " + ex.Message, ex);
            }
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            ExecutaModificaCancela44EP();
        }

        private void btIni_Click(object sender, EventArgs e)
        {
            
            if (comboBox2.Text == "FIX.4.4EP")
            {
                if (null != timer1)
                {
                    timer1.Interval = Convert.ToInt32(txtInterval.Text) * 1000;
                    timer1.Start();
                }
            }

            if (comboBox2.Text == "FIX.4.2BB")
            {
                if (null != timer2)
                {
                    timer2.Interval = Convert.ToInt32(txtInterval.Text) * 1000;
                    timer2.Start();
                }
            }

            if (comboBox2.Text == "FIX.4.2BNP")
            {
                if (null != timer3)
                {
                    timer3.Interval = Convert.ToInt32(txtInterval.Text) * 1000;
                    timer3.Start();
                }
            }

            if (comboBox2.Text == "FIX.4.2RTK")
            {
                if (null != timer4)
                {
                    timer4.Interval = Convert.ToInt32(txtInterval.Text) * 1000;
                    timer4.Start();
                }
            }
        }

        private void btParar_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "FIX.4.4EP")
            {
                if (null != timer1)
                    timer1.Stop();
            }
            if (comboBox2.Text == "FIX.4.2BB")
                if (null != timer2)
                    timer2.Stop();

            if (comboBox2.Text == "FIX.4.2BNP")
                if (null != timer3)
                    timer3.Stop();

            if (comboBox2.Text == "FIX.4.2RTK")
                if (null != timer3)
                    timer4.Stop();
        }

        private void btnLimitManager_Click(object sender, EventArgs e)
        {
            int idx = comboBox1.SelectedIndex;
            ProcessUpdate(idx);
        }

        void ProcessUpdate(int op)
        {
            try
            {
                ILimiteManager lmtMng = Ativador.Get<ILimiteManager>();
                switch (op)
                {
                    case 0:
                        lmtMng.DummyFunction();
                        break;
                    case 1:
                        _reloadLimits(lmtMng);
                        break;
                    case 2:
                        _reloadClientLimit(lmtMng);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problema no LimitManager: " + ex.Message, ex);
                MessageBox.Show("LimiteManager - Erro na atualizacao dos parametros ");
            }
        }
        /*
        private void _updateSymbol(ILimiteManager i)
        {
            SymbolUpdateResponse resp = new SymbolUpdateResponse();
            SymbolUpdateRequest req = new SymbolUpdateRequest();
            SymbolInfo symb = new SymbolInfo();
            symb.Instrumento = "XPTO";
            symb.LotePadrao = 1;
            symb.SegmentoMercado = SegmentoMercadoEnum.AVISTA;
            req.Symbol = symb;
            req.OperationType = DbOperation.Add;

            i.UpdateSymbol(req);
        }
        */
        private void _reloadLimits(ILimiteManager i)
        {
            ILimiteManager lmtMng = Ativador.Get<ILimiteManager>();
            ReloadLimitsResponse resp = new ReloadLimitsResponse();
            ReloadLimitsRequest req = new ReloadLimitsRequest();
            req.ReloadSecurityList = true;
            i.ReloadLimitStructures(req);
        }

        private void _reloadClientLimit(ILimiteManager i)
        {
            ILimiteManager lmtMng = Ativador.Get<ILimiteManager>();
            ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
            ReloadClientLimitRequest req = new ReloadClientLimitRequest();
            req.CodCliente = Convert.ToInt32(txtParam.Text);
            req.DeleteOnly = chkDel.Checked;
            i.ReloadLimitClientLimitStructures(req);
        }

        private void btEnviar42_Click(object sender, EventArgs e)
        {
            try
            {

                OrdemInfo ordem = new OrdemInfo();

                if (!string.IsNullOrEmpty(txtTimes.Text))
                {
                    long aux1 = Convert.ToInt64(DateTime.Now.ToString("MMddhhmmssffffff"));

                    txtClOrdIni.Text = aux1.ToString();
                    txtClOrdFim.Text = (aux1 + Convert.ToInt32(txtTimes.Text)).ToString();
                    _atualizaReport("New Order ClOrdID Ini: " + txtClOrdIni.Text);
                    _atualizaReport("New Order ClOrdID Fim: " + txtClOrdFim.Text);
                }
                
                ordem.ClOrdID = txtClOrdID.Text;
                string strMnemonic = string.Empty;
                try
                {
                    ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                }
                catch
                {
                    strMnemonic = txtCodCliente.Text;
                }
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";

                ordem.ExchangeNumberID = txtExchangeNumber.Text;
                ordem.Price = Convert.ToDouble(txtPreco.Text);
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);
                ordem.MinQty = Convert.ToInt32(txtQtdeMin.Text);
                ordem.MaxFloor = Convert.ToInt32(txtQtdeAparente.Text);
                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.RegisterTime = DateTime.Now;
                ordem.TransactTime = DateTime.Now;
                ordem.ExecBroker = txtTraderID.Text;
                // ordem.Memo5149 = "Nova " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                if (txtSenderLocation.Text.Length > 0)
                    ordem.SenderLocation = txtSenderLocation.Text;

                if (chkAccountType.Checked)
                    ordem.AcountType = ContaTipoEnum.GIVE_UP_LINK_IDENTIFIER;

                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;

                if (txtStopPX.Text.Length > 0 && Convert.ToDouble(txtStopPX.Text) > 0)
                {
                    ordem.StopPrice = Convert.ToDouble(txtStopPX.Text);
                }

                if (txtInvestorID.Text.Length > 0)
                {
                    ordem.InvestorID = txtInvestorID.Text;
                }

                if (txtExecTrader.Text.Length > 0)
                {
                    ordem.ExecutingTrader = txtExecTrader.Text;
                }

                switch (cmbOrderType.SelectedIndex)
                {
                    case 0: ordem.OrdType = OrdemTipoEnum.Limitada; break;
                    case 1: ordem.OrdType = OrdemTipoEnum.StopLimitada; break;
                    case 2: ordem.OrdType = OrdemTipoEnum.MarketWithLeftOverLimit; break;
                    case 3: ordem.OrdType = OrdemTipoEnum.OnClose; break;
                    case 4: ordem.OrdType = OrdemTipoEnum.StopStart; break;
                    case 5: ordem.OrdType = OrdemTipoEnum.Mercado; break;
                    case 6: ordem.OrdType = OrdemTipoEnum.StopLoss; break;
                    case 7: ordem.Memo5149 = "5"; break;
                    case 8: ordem.Memo5149 = "A"; break;
                    case 9: ordem.Memo5149 = "B"; break;
                    default:
                        ordem.OrdType = OrdemTipoEnum.OnClose; break;
                }

                //0- Para o dia");
                //1- Executa ou cancela");
                //2- Tudo ou Nada");
                //3- Ate cancelar");
                //4- Data especifica");
                //5- Abertura Mercado");
                //6- Fechamento Mercado");
                //7- Boa para Leilao");
                switch (cmbTipoValidade.SelectedIndex)
                {
                    case 0:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                        ordem.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    case 1: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela; break;
                    case 2: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela; break;
                    case 3: ordem.TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada; break;
                    case 4:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidoAteDeterminadaData;

                        ordem.ExpireDate = DateTime.ParseExact(txtDataValidade.Text + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        break;

                    case 5: ordem.TimeInForce = OrdemValidadeEnum.ValidaParaAberturaDoMercado; break;
                    case 6: ordem.TimeInForce = OrdemValidadeEnum.FechamentoDoMercado; break;
                    case 7: ordem.TimeInForce = OrdemValidadeEnum.BoaParaLeilao; break;
                    default:
                        MessageBox.Show("Time in force invalido");
                        break;
                }
                
                // Montagem das tags extras
                string strExtraTags = string.Empty;
                if (richTextBox2.Lines.Length > 0)
                {
                    for (int i = 0; i < richTextBox2.Lines.Length; i++)
                    {
                        strExtraTags = strExtraTags + richTextBox2.Lines[i] + ";";
                    }
                }

                logger.Info(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " ******** Nova ordem enviada INICIO ====> Regs: " + txtTimes.Text);
                bool ret = false;
                object aux3 = null;
                _dicExecutors42.TryGetValue(txtSender.Text, out aux3);
                if (null != aux3)
                {
                    switch (comboBox2.Text)
                    {
                        case "FIX.4.2BB":
                            if (string.IsNullOrEmpty(txtTimes.Text))
                                ret = (aux3 as FIX42BloombergClient).EnviarOrdem(ordem, 0, 0, 0, "", txtSsID.Text, strExtraTags);
                            else
                                ret = (aux3 as FIX42BloombergClient).EnviarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic, txtSsID.Text, strExtraTags);
                            break;
                        case "FIX.4.2BNP":
                            if (string.IsNullOrEmpty(txtTimes.Text))
                                ret = (aux3 as FIX42BnpClient).EnviarOrdem(ordem, 0, 0, 0, "", txtSsID.Text, strExtraTags);
                            else
                                ret = (aux3 as FIX42BnpClient).EnviarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic, txtSsID.Text, strExtraTags);
                            break;
                        case "FIX.4.2RTK":
                            if (string.IsNullOrEmpty(txtTimes.Text))
                                ret = (aux3 as FIX42RealTickClient).EnviarOrdem(ordem, 0, 0, 0, "", txtSsID.Text, strExtraTags);
                            else
                                ret = (aux3 as FIX42RealTickClient).EnviarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic, txtSsID.Text, strExtraTags);
                            break;
                    }
                }
                else
                    _addMsg("Executor: " + txtSender.Text + " not found");
                logger.Info(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " ******** Nova ordem enviada FIM ====> Regs:" + txtTimes.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                logger.Error("Erro: " + ex.Message, ex);
            }

            lastCLOrdID++;
            txtClOrdID.Text = lastCLOrdID.ToString();
        }

        private void btAlterar42_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTimes.Text))
                {
                    long aux1 = Convert.ToInt64(DateTime.Now.ToString("MMddhhmmssffffff"));

                    txtClOrdIni.Text = aux1.ToString();
                    txtClOrdFim.Text = (aux1 + Convert.ToInt32(txtTimes.Text)).ToString();
                    _atualizaReport("Alter Order ClOrdID Ini: " + txtClOrdIni.Text);
                    _atualizaReport("Alter Order ClOrdID Fim: " + txtClOrdFim.Text);
                }

                OrdemInfo ordem = new OrdemInfo();

                ordem.OrigClOrdID = txtOrigOrdID.Text;
                ordem.ClOrdID = txtClOrdID.Text;
                string strMnemonic = string.Empty;
                try
                {
                    ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                }
                catch
                {
                    strMnemonic = txtCodCliente.Text;
                }
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";

                ordem.ExchangeNumberID = txtExchangeNumber.Text;
                ordem.Price = Convert.ToDouble(txtPreco.Text);
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);
                ordem.MinQty = Convert.ToInt32(txtQtdeMin.Text);
                ordem.MaxFloor = Convert.ToInt32(txtQtdeAparente.Text);

                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.ExecBroker = txtTraderID.Text;
                

                if (txtSenderLocation.Text.Length > 0)
                    ordem.SenderLocation = txtSenderLocation.Text;

                if (txtExecTrader.Text.Length > 0)
                {
                    ordem.ExecutingTrader = txtExecTrader.Text;
                }

                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;

                switch (cmbOrderType.SelectedIndex)
                {
                    case 0: ordem.OrdType = OrdemTipoEnum.Limitada; break;
                    case 1: ordem.OrdType = OrdemTipoEnum.StopLimitada; break;
                    case 2: ordem.OrdType = OrdemTipoEnum.MarketWithLeftOverLimit; break;
                    case 3: ordem.OrdType = OrdemTipoEnum.OnClose; break;
                    case 4: ordem.OrdType = OrdemTipoEnum.StopStart; break;
                    case 5: ordem.OrdType = OrdemTipoEnum.Mercado; break;
                    case 6: ordem.OrdType = OrdemTipoEnum.StopLoss; break;
                    case 7: ordem.Memo5149 = "5"; break;
                    case 8: ordem.Memo5149 = "A"; break;
                    case 9: ordem.Memo5149 = "B"; break;
                    default:
                        ordem.OrdType = OrdemTipoEnum.OnClose; break;
                }


                //0- Para o dia");
                //1- Executa ou cancela");
                //2- Tudo ou Nada");
                //3- Ate cancelar");
                //4- Data especifica");
                //5- Abertura Mercado");
                //6- Fechamento Mercado");
                //7- Boa para Leilao");
                switch (cmbTipoValidade.SelectedIndex)
                {
                    case 0:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                        ordem.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    case 1: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela; break;
                    case 2: ordem.TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela; break;
                    case 3: ordem.TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada; break;
                    case 4:
                        ordem.TimeInForce = OrdemValidadeEnum.ValidoAteDeterminadaData;

                        ordem.ExpireDate = DateTime.ParseExact(txtDataValidade.Text + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        break;

                    case 5: ordem.TimeInForce = OrdemValidadeEnum.ValidaParaAberturaDoMercado; break;
                    case 6: ordem.TimeInForce = OrdemValidadeEnum.FechamentoDoMercado; break;
                    case 7: ordem.TimeInForce = OrdemValidadeEnum.BoaParaLeilao; break;
                    default:
                        MessageBox.Show("Time in force invalido");
                        break;
                }

                if (txtStopPX.Text.Length > 0 && Convert.ToDouble(txtStopPX.Text) > 0)
                {
                    ordem.StopPrice = Convert.ToDouble(txtStopPX.Text);
                }

                if (txtInvestorID.Text.Length > 0)
                {
                    ordem.InvestorID = txtInvestorID.Text;
                }

                // Montagem das tags extras
                string strExtraTags = string.Empty;
                if (richTextBox2.Lines.Length > 0)
                {
                    for (int i = 0; i < richTextBox2.Lines.Length; i++)
                    {
                        strExtraTags = strExtraTags + richTextBox2.Lines[i] + ";";
                    }
                }
                bool ret = false;
                object aux3 = null;
                _dicExecutors42.TryGetValue(txtSender.Text, out aux3);
                logger.Info(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " ******** Alterar ordem enviada INICIO ====> Regs: " + txtTimes.Text);
                if (null != aux3)
                {
                    switch (comboBox2.Text)
                    {
                        case "FIX.4.2BB":
                            if (!string.IsNullOrEmpty(txtTimes.Text))
                                ret = (aux3 as FIX42BloombergClient).AlterarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text),
                                                      Convert.ToInt64(txtOriIni.Text), Convert.ToInt64(txtOriFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic, txtSsID.Text, strExtraTags);
                            else
                                ret = (aux3 as FIX42BloombergClient).AlterarOrdem(ordem, 0, 0, 0, 0, 0, "", txtSsID.Text, strExtraTags);
                            break;
                        case "FIX.4.2BNP":
                            if (!string.IsNullOrEmpty(txtTimes.Text))
                                ret = (aux3 as FIX42BnpClient).AlterarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text),
                                                      Convert.ToInt64(txtOriIni.Text), Convert.ToInt64(txtOriFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic, txtSsID.Text, strExtraTags);
                            else
                                ret = (aux3 as FIX42BnpClient).AlterarOrdem(ordem, 0, 0, 0, 0, 0, "", txtSsID.Text, strExtraTags);
                            break;
                        case "FIX.4.2RTK":
                            if (!string.IsNullOrEmpty(txtTimes.Text))
                                ret = (aux3 as FIX42RealTickClient).AlterarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text),
                                                      Convert.ToInt64(txtOriIni.Text), Convert.ToInt64(txtOriFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic, txtSsID.Text, strExtraTags);
                            else
                                ret = (aux3 as FIX42RealTickClient).AlterarOrdem(ordem, 0, 0, 0, 0, 0, "", txtSsID.Text, strExtraTags);
                            break;
                    }
                }
                else
                    _addMsg("Executor: " + txtSender.Text + " not found");
                logger.Info(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " ******** Alterar ordem enviada FIM ====> Regs: " + txtTimes.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                logger.Error("Erro: " + ex.Message, ex);
            }
            lastCLOrdID++;
            txtClOrdID.Text = lastCLOrdID.ToString();
        }

        private void btCancelar42_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (!string.IsNullOrEmpty(txtTimes.Text))
                {
                    long aux1 = Convert.ToInt64(DateTime.Now.ToString("MMddhhmmssffffff"));

                    txtClOrdIni.Text = aux1.ToString();
                    txtClOrdFim.Text = (aux1 + Convert.ToInt32(txtTimes.Text)).ToString();
                    _atualizaReport("Cancel Order ClOrdID Ini: " + txtClOrdIni.Text);
                    _atualizaReport("Cancel Order ClOrdID Fim: " + txtClOrdFim.Text);
                }
                
                OrdemCancelamentoInfo ordem = new OrdemCancelamentoInfo();
                ordem.ClOrdID = txtClOrdID.Text;
                ordem.OrigClOrdID = txtOrigOrdID.Text;
                ordem.ChannelID = Convert.ToInt32(txtOperador.Text);
                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    ordem.Exchange = "BOVESPA";
                else
                    ordem.Exchange = "BMF";
                ordem.OrderID = txtExchangeNumber.Text;
                string strMnemonic = string.Empty;
                try
                {
                    ordem.Account = Convert.ToInt32(txtCodCliente.Text);
                }
                catch
                {
                    strMnemonic = txtCodCliente.Text;
                }
                ordem.Symbol = txtPapel.Text;
                ordem.SecurityID = txtSecurityId.Text;
                ordem.ExecBroker = txtTraderID.Text;
                ordem.Memo5149 = "Cancelto " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                if (txtSenderLocation.Text.Length > 0)
                    ordem.SenderLocation = txtSenderLocation.Text;

                if (txtExecTrader.Text.Length > 0)
                {
                    ordem.ExecutingTrader = txtExecTrader.Text;
                }


                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);

                // Montagem das tags extras
                string strExtraTags = string.Empty;
                if (richTextBox2.Lines.Length > 0)
                {
                    for (int i = 0; i < richTextBox2.Lines.Length; i++)
                    {
                        strExtraTags = strExtraTags + richTextBox2.Lines[i] + ";";
                    }
                }
                bool ret = false;
                object aux3 = null;
                _dicExecutors42.TryGetValue(txtSender.Text, out aux3);
                logger.Info(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " ******** Cancelar ordem enviada INICIO ====> Regs: " + txtTimes.Text);
                if (null != aux3)
                {
                    switch (comboBox2.Text)
                    {
                        case "FIX.4.2BB":
                            if (!string.IsNullOrEmpty(txtTimes.Text))
                                ret = (aux3 as FIX42BloombergClient).CancelarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text),
                                                           Convert.ToInt64(txtOriIni.Text), Convert.ToInt64(txtOriFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic, txtSsID.Text, strExtraTags);
                            else
                                ret = (aux3 as FIX42BloombergClient).CancelarOrdem(ordem, 0, 0, 0, 0, 0, "", txtSsID.Text, strExtraTags);
                            break;
                        case "FIX.4.2BNP":
                            if (!string.IsNullOrEmpty(txtTimes.Text))
                                ret = (aux3 as FIX42BnpClient).CancelarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text),
                                                           Convert.ToInt64(txtOriIni.Text), Convert.ToInt64(txtOriFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic, txtSsID.Text, strExtraTags);
                            else
                                ret = (aux3 as FIX42BnpClient).CancelarOrdem(ordem, 0, 0, 0, 0, 0, "", txtSsID.Text, strExtraTags);
                            break;
                        case "FIX.4.2RTK":
                            if (!string.IsNullOrEmpty(txtTimes.Text))
                                ret = (aux3 as FIX42RealTickClient).CancelarOrdem(ordem, Convert.ToInt64(txtClOrdIni.Text), Convert.ToInt64(txtClOrdFim.Text),
                                                           Convert.ToInt64(txtOriIni.Text), Convert.ToInt64(txtOriFim.Text), Convert.ToInt32(nmDelay.Value), strMnemonic, txtSsID.Text, strExtraTags);
                            else
                                ret = (aux3 as FIX42RealTickClient).CancelarOrdem(ordem, 0, 0, 0, 0, 0, "", txtSsID.Text, strExtraTags);
                            break;
                    }
                }
                else
                    _addMsg("Executor: " + txtSender.Text + " not found");
                logger.Info(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " ******** Cancelar ordem enviada FIM ====> Regs: " + txtTimes.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                logger.Error("Erro: " + ex.Message, ex);
            }
            
            lastCLOrdID++;
            txtClOrdID.Text = lastCLOrdID.ToString();
        }

        private void btnResendRequest_Click(object sender, EventArgs e)
        {
            FIX42BloombergClient aux = null;
            if (_dicExecutors42Bb.TryGetValue(txtSender.Text, out aux))
            {
                //bool ret = false;
                bool ret = aux.EnviarResend(Convert.ToInt32(txtSeqIni.Text), Convert.ToInt32(txtSeqFim.Text));
                //if (ret) _addMsg("Cancelamento de Ordem enviado: " + ordem.ClOrdID);
                //else _addMsg("Erro no cancelamento de ordem: " + ordem.ClOrdID);
            }
            else
            {
                _addMsg("Executor: " + txtSender.Text + " not found");
                return;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("aqui");
            ExecutaModificaCancela42BB();
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            string pwd = txtPwd1.Text;
            
            // MessageBox.Show(pwd.GetHashCode().ToString());
            txtPwd2.Text = pwd.GetHashCode().ToString();
            
            
        }

        private void btnTeste_Click(object sender, EventArgs e)
        {
            FIX42BloombergClient aux = null;
            if (_dicExecutors42Bb.TryGetValue(txtSender.Text, out aux))
            {
                //bool ret = false;
                bool ret = aux.DummyTest();
                //if (ret) _addMsg("Cancelamento de Ordem enviado: " + ordem.ClOrdID);
                //else _addMsg("Erro no cancelamento de ordem: " + ordem.ClOrdID);
            }
        }

        public void _atualizaReportOrderMngr(string msg)
        {
            //this.txtExecReport.Text += DateTime.Now.ToString("HH:mm:ss.fff") + ": " + msg + "\r\n";
            //this.txtExecReport.Select(txtExecReport.Text.Length - 1, 0);
            //this.txtExecReport.ScrollToCaret();
            this.richTextBox1.Text += DateTime.Now.ToString("HH:mm:ss.fff") + ": " + msg + "\r\n";
            this.richTextBox1.SelectionStart = this.rtExecReport.Text.Length;
            this.richTextBox1.ScrollToCaret();
        }
        private void button4_Click_2(object sender, EventArgs e)
        {
            IFixServerLowLatencyAdm fixAdm = Ativador.Get<IFixServerLowLatencyAdm>();
            OrderCancelingRequest req = new OrderCancelingRequest();
            req.Account = textBox1.Text;
            req.OrigClOrdID = textBox2.Text;
            req.ChannelID = textBox3.Text;
            req.Symbol = textBox4.Text;

            OrderCancelingResponse resp = fixAdm.CancelOrder(req);
            _atualizaReportOrderMngr("Status...: " + resp.StatusResponse.ToString());
            if (!string.IsNullOrEmpty(resp.DescricaoErro))
                _atualizaReportOrderMngr("DescErro...: " + resp.DescricaoErro);
            _atualizaReportOrderMngr("===========");
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (1 == 1)
            {
                MessageBox.Show("teste 1");
            }
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            // MessageBox.Show(_truncateDecimal(4.5M,2).ToString());
            //20140724-18:03:57.126 : 8=FIX.4.49=11335=534=108049=AGRA30252=20140724-18:03:57.12656=TMAT3020158=MsgSeqNum too low, expecting 990 but received 110=134


            string msg = "MsgSeqNum too low, expecting 990 but received 1";
            if (msg.IndexOf("MsgSeqNum too low") >= 0)
            {
                string[] arr = msg.Split(' ');
                for (int i = 0; i < arr.Length; i++)
                {
                    if (i==4 || i==7)
                        MessageBox.Show("[" + arr[i] + "]");
                }

            }
            
        }


        private Decimal _truncateDecimal(decimal value, int digits)
        {

            Decimal factor = Convert.ToDecimal(Math.Pow(10, digits));
            Decimal ret = Convert.ToDecimal(Math.Truncate(value * factor) / factor);

            return ret;
        }

        
    }

}

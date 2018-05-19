using System;
using System.Diagnostics;
using System.Windows.Forms;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.ServicoRoteador;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Configuration;
using System.Timers;
using System.Text;
using log4net;
using Gradual.OMS.RoteadorOrdensAdm.Lib.Mensagens;
using Gradual.OMS.RoteadorOrdensAdm.Lib;

namespace AppTesteRoteadorOrdens
{
    public partial class Form1 : Form, IRoteadorOrdensCallback
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        long lastCLOrdID = 0;
        private IRoteadorOrdens _roteador = null;
        private List<OrdemInfo> ofertasenviadas = SerializadorOfertas.LoadOfertas();
        private Queue<string> qReport = new Queue<string>();

        private System.Timers.Timer sundaTimer = new System.Timers.Timer();

        
        
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

            cmbTipoValidade.Items.Add("Para o dia");
            cmbTipoValidade.Items.Add("Executa ou cancela");
            cmbTipoValidade.Items.Add("Tudo ou Nada");
            cmbTipoValidade.Items.Add("Ate cancelar");
            cmbTipoValidade.Items.Add("Data especifica");
            cmbTipoValidade.Items.Add("Abertura Mercado");
            cmbTipoValidade.Items.Add("Fechamento Mercado");
            cmbTipoValidade.Items.Add("Boa para Leilao");
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
            cmbOrderType.SelectedIndex = 0;

            lastCLOrdID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmm"));
            txtClOrdID.Text = lastCLOrdID.ToString();
            txtOperador.Text = ConfigurationManager.AppSettings["CanalBovespaPadrao"].ToString();
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

            if (report.FixMsgSeqNum > 0)
            {
                txtFixBeginSeqNo.Text = report.FixMsgSeqNum.ToString();
                txtFixEndSeqNo.Text = (report.FixMsgSeqNum + 1).ToString();
            }

            _addMsg(msg);

            lock (ofertasenviadas)
            {
                for (int j = 0; j < ofertasenviadas.Count; j++)
                {
                    if (ofertasenviadas[j].ClOrdID.Equals(report.OrigClOrdID))
                    {
                        if (report.OrdStatus == OrdemStatusEnum.CANCELADA ||
                            report.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                        {
                            ofertasenviadas[j].OrdStatus = report.OrdStatus;
                        }
                    }

                    if (ofertasenviadas[j].ClOrdID.Equals(report.ClOrdID))
                    {
                        ofertasenviadas[j].OrdStatus = report.OrdStatus;
                        break;
                    }
                }
                SerializadorOfertas.SaveOfertas(ofertasenviadas);
            }


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
            this.txtExecReport.Text += msg + "\r\n";
            this.txtExecReport.Select(txtExecReport.Text.Length - 1, 0);
            this.txtExecReport.ScrollToCaret();
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

                IRoteadorOrdens roteador = Ativador.Get<IRoteadorOrdens>();

                if (roteador != null)
                {
                    ExecutarOrdemRequest request = new ExecutarOrdemRequest();

                    request.info = ordem;

                    ExecutarOrdemResponse resp = roteador.ExecutarOrdem(request);

                    if ( resp.DadosRetorno != null )
                    {
                        string msg="";

                        foreach(OcorrenciaRoteamentoOrdem ocorr in resp.DadosRetorno.Ocorrencias)
                        {
                            msg += ocorr.Ocorrencia + "\r\n";
                        }

                        if (resp.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                            MessageBox.Show(msg);
                        else
                        {
                            _addMsg(msg);

                            lock (ofertasenviadas)
                            {
                                ofertasenviadas.Add(ordem);

                                SerializadorOfertas.SaveOfertas(ofertasenviadas);
                            }
                        }
                    }
                }
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

                /*if (!String.IsNullOrEmpty(txtCompIDBolsa.Text))
                {
                    ordem.CompIDBolsa = txtCompIDBolsa.Text;
                }*/


                if (rdCompra.Checked)
                    ordem.Side = OrdemDirecaoEnum.Compra;
                else
                    ordem.Side = OrdemDirecaoEnum.Venda;
                ordem.OrderQty = Convert.ToInt32(txtQtde.Text);

                IRoteadorOrdens roteador = Ativador.Get<IRoteadorOrdens>();

                if (roteador != null)
                {
                    ExecutarCancelamentoOrdemRequest request = new ExecutarCancelamentoOrdemRequest();

                    request.info = ordem;

                    ExecutarCancelamentoOrdemResponse resp = roteador.CancelarOrdem(request);

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
                        }
                    }
                }
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

                IRoteadorOrdens roteador = Ativador.Get<IRoteadorOrdens>();

                if (roteador != null)
                {
                    ExecutarModificacaoOrdensRequest request = new ExecutarModificacaoOrdensRequest();

                    request.info = ordem;

                    ExecutarModificacaoOrdensResponse resp = roteador.ModificarOrdem(request);

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

                            lock (ofertasenviadas)
                            {
                                ofertasenviadas.Add(ordem);

                                SerializadorOfertas.SaveOfertas(ofertasenviadas);
                            }
                        }
                    }
                }
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

                                lock (ofertasenviadas)
                                {
                                    ofertasenviadas.Add(ordemCompra);

                                    SerializadorOfertas.SaveOfertas(ofertasenviadas);
                                }
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

        private void btResendMsg_Click(object sender, EventArgs e)
        {
            try
            {
                int msgBeginSeqNum = Convert.ToInt32(txtFixBeginSeqNo.Text);
                int msgEndSeqNum = Convert.ToInt32(txtFixEndSeqNo.Text);

                FixResendRequest req = new FixResendRequest();

                if (cmbBolsa.SelectedItem.Equals("BOVESPA"))
                    req.Bolsa = "BOVESPA";
                else
                    req.Bolsa = "BMF";

                req.Canal = Convert.ToInt32(txtOperador.Text);

                req.BeginSeqNo = msgBeginSeqNum;
                req.EndSeqNo = msgEndSeqNum;


                IRoteadorOrdensAdmin roteadoradm = Ativador.Get<IRoteadorOrdensAdmin>();

                if (roteadoradm != null)
                {
                    FixResendResponse resp = roteadoradm.ExecutarFixResend(req);

                    if (resp.DadosRetorno != null)
                    {
                        string msg = "";

                        foreach (string ocorr in resp.DadosRetorno.Ocorrencias)
                        {
                            msg += ocorr + "\r\n";
                        }

                        if (resp.DadosRetorno.Erro)
                            MessageBox.Show(msg);
                        else
                        {
                            _addMsg(msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _addMsg(ex.Message);
                logger.Error("btResendMsg_Click(): " + ex.Message, ex);
            }
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

                    Application.DoEvents();
                }
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
            }

            txtClOrdID.Text = lastCLOrdID.ToString();
        }

    }

    [Serializable]
    public class SerializedOrdemInfo : OrdemInfo
    {
    }

}

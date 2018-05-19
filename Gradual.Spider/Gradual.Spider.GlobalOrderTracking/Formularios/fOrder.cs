/**************************************************************************************************
 MainModule...: fOrder
 SubModule....:
 Author.......: Hideaki
 Date.........: 16/03/2015
 Porpouse.....: 

 Modifications:
 Author                                    Date       Reason
 -------------------- -------------------- ---------- ---------------------------------------------
 Hideaki              Rafael Sanches       25/03/2015  Alteração da ordem dos campos (sem status)
 Hideaki              Rafael Sanches       30/03/2015  Limitação de registros em 1000 (aprovada)
 Hideaki              Rodrigo Sampaio      08/05/2015  Inclusão de botões Cancelar e Cancelar Tudo
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AjaxLoading;

namespace Gradual.Spider.GlobalOrderTracking.Formularios
{
    public partial class fOrder : FormularioBase
    {

        //TODO: alterar o ExpressionBuilder para aceitar vários parametros ao invés de efetuar o filtro duas vezes
        Gradual.Spider.CommSocket.SpiderSocket lClient = null;
        DateTime lLastMessage;
        Guid lGuid;
        int AlturaDetalhe = 0;
        int lSelectedOrderID;
        int lInteractions = 0;
        bool lFiltered = false;
        List<Utils.Filter.Filter> lFilters = new List<Utils.Filter.Filter>();
        Dictionary<string, string> lStatus = new Dictionary<string, string>();
        List<Gradual.Utils.Filter.Filter> filter;

        private SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo> lOriginalSource = new SortableBindingList<Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>();
        private SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo> lFilteredSource = new SortableBindingList<Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>();
        private SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo> lOrderDetails = new SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo>();
        private SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo> lSelectedOrderDetails = new SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo>();

        private Gradual.Utils.Fila<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterInfoResponse> filaResponse { get; set; }
        private Gradual.Utils.Fila<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo> filaResponseStream { get; set; }
        private Gradual.Utils.Fila<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo> filaResponseDetailStream { get; set; }

        System.Threading.Thread tTrataResponse;
        System.Threading.Thread tTrataResponseStream;
        System.Threading.Thread tTrataResponseDetailStream;

        private Aplicacao.OnAlterarCoresHandler lOnAlterarCoresHandler;
        private Boolean lConfirmation = System.Configuration.ConfigurationManager.AppSettings["Confirmation"].Equals("true");
        private Dictionary<string, string> _statusDictionary;
        public Dictionary<string, string> StatusDictionary
        {
            get
            {
                if(_statusDictionary == null)
                {

                    _statusDictionary = new Dictionary<string, string>();

                    _statusDictionary.Add("NOVA", "Nova");
                    _statusDictionary.Add("PARCIALMENTE EXECUTADA", "Parc. Executada");
                    _statusDictionary.Add("EXECUTADA", "Executada");
                    _statusDictionary.Add("CANCELADA", "Cancelada");
                    _statusDictionary.Add("SUBSTITUIDA", "Substituída");
                    _statusDictionary.Add("REJEITADA", "Rejeitada");
                    _statusDictionary.Add("SUSPENSA", "Suspensa");
                    _statusDictionary.Add("EXPIRADA", "Expirada");
                    //_statusDictionary.Add("PENDENTE DE CANCELAMENTO","Pendente de Cancelamento");
                    //_statusDictionary.Add("PENDENTE DE NOVA ORDEM", "Pendente de Nova Ordem");
                    //_statusDictionary.Add("REAPRESENTADA", "Reapresentada");
                    //_statusDictionary.Add("PENDENTE DE ALTERACAO", "Pendente de Alteração");
                    //_statusDictionary.Add("NEGOCIO CANCELADO", "Negócio Cencelado");
                    //_statusDictionary.Add("NOVA ORDEM SOLICITADA", "Nova Ordem Solicitada");
                    //_statusDictionary.Add("SUBSTITUICAO SOLICITADA", "Substituição Solicitada");
                    //_statusDictionary.Add("CANCELAMENTO SOLICITADO", "Cancelamento Solicitado");
                    
                }

                return _statusDictionary;
            }
        }

        #region Colunas

        private System.Windows.Forms.DataGridViewCellStyle dgvMoedaCellStyle;

        //private System.Windows.Forms.DataGridViewButtonColumn columnCancelar;
        private System.Windows.Forms.DataGridViewButtonColumn columnCancelar;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnClOrdID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAccountDv;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSymbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrdStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHora;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrdTypeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRegisterTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTransactTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTimeInForce;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnExpireDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnChannelID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnExchange;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSide;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrderQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrderQtyRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCumQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrderQtyMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrderQtyApar;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnStopPx;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnExecBroker;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSessionIDOriginal;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrderID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrigClOrdID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnExchangeNumberID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSecurityExchangeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnStopStartID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn columSystemID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMemo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFixMsgSeqNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSessionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIdFix;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMsgFix;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMsg42Base64;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHandlInst;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIntegrationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAveragePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAveragePriceW;
        
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailOrderId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailOrderDetailID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailOrderQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailCumQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailOrderQtyRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailOrderStatusId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailTransactTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailStopPx;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailTransactID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailTradeQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailFixMsgSeqNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailCxlRejResponseTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailCxlRejReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailMsgFixDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDetailContraBroker;
        
        
        #endregion


        private Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo resumoInEdit;
        private int rowInEdit = -1;
        private bool rowScopeCommit = true;

        #region Métodos do client
       
        private void Conectar()
        {
            try
            {
                if (null == lClient)
                {
                    lClient = new Gradual.Spider.CommSocket.SpiderSocket();
                    lClient.OnConnectionOpened += new Gradual.Spider.CommSocket.ConnectionOpenedHandler(Client_OnConnectionOpened);
                    lClient.AddHandler<Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.LogonSrvResponse>(new Gradual.Spider.CommSocket.ProtoObjectReceivedHandler<Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.LogonSrvResponse>(Client_OnLogonResponse));
                    lClient.AddHandler<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>(new Gradual.Spider.CommSocket.ProtoObjectReceivedHandler<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>(Client_OnSpiderOrderInfoReceived));
                    lClient.AddHandler<Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.SondaSrvInfo>(new Gradual.Spider.CommSocket.ProtoObjectReceivedHandler<Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.SondaSrvInfo>(Client_OnSondaSrvInfoReceived));
                    lClient.AddHandler<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterInfoResponse>(new Gradual.Spider.CommSocket.ProtoObjectReceivedHandler<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterInfoResponse>(Client_OnFilterInfoResponseReceived));
                    lClient.AddHandler<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterDetailInfoResponse>(new Gradual.Spider.CommSocket.ProtoObjectReceivedHandler<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterDetailInfoResponse>(Client_OnFilterDetailInfoResponseReceived));
                    lClient.AddHandler<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo>(new Gradual.Spider.CommSocket.ProtoObjectReceivedHandler<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo>(Client_OnStreamerOrderInfoReceived));
                }
                if (lClient.IsConectado())
                {
                    MessageBox.Show("Já conectado!!!");
                    return;
                }
                lClient.IpAddr = System.Configuration.ConfigurationManager.AppSettings["IpAddr"];
                lClient.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);

                lClient.OpenConnection();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Client_OnConnectionOpened(object sender, Gradual.Spider.CommSocket.ConnectionOpenedEventArgs args)
        {
            try
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}:{1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Cliente conectado."), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.LogonSrvRequest req = new Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.LogonSrvRequest();
                req.AppDescription = "WindowsForm Ac4S Test";
                if (null != lClient)
                    lClient.SendObject(req);
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Client_OnLogonResponse(object sender, int clientNumber, System.Net.Sockets.Socket clientSocket, Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.LogonSrvResponse args)
        {
            try
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: SessionID [{1}]", Gradual.Utils.MethodHelper.GetCurrentMethod(), args.SessionID), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Client_OnSpiderOrderInfoReceived(object sender, int clientNumber, System.Net.Sockets.Socket clientSocket, Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo args)
        {
            try
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: OrderID [{1}]", Gradual.Utils.MethodHelper.GetCurrentMethod(), args.OrderID), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Client_OnSondaSrvInfoReceived(object sender, int clientNumber, System.Net.Sockets.Socket clientSocket, Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.SondaSrvInfo args)
        {
            try
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: SondaReceived [{1}]", Gradual.Utils.MethodHelper.GetCurrentMethod(), args.TimeStamp), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                if (lClient.IsConectado())
                {
                    Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.SondaSrvInfo sonda = new Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens.SondaSrvInfo();
                    sonda.TimeStamp = DateTime.Now.Ticks;
                    lClient.SendObject(sonda);
                }
            }
            catch(Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public delegate void OnFilterInfoResponseReceivedCallBack(object sender, int clientNumber, System.Net.Sockets.Socket clientSocket, Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterInfoResponse args);
        private void Client_OnFilterInfoResponseReceived(object sender, int clientNumber, System.Net.Sockets.Socket clientSocket, Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterInfoResponse args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    OnFilterInfoResponseReceivedCallBack d = new OnFilterInfoResponseReceivedCallBack(Client_OnFilterInfoResponseReceived);
                    this.BeginInvoke(d, new object[] { sender, clientNumber, clientSocket, args });
                }
                else
                {
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: *************************", Gradual.Utils.MethodHelper.GetCurrentMethod()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: ErrCode   [{1}]", Gradual.Utils.MethodHelper.GetCurrentMethod(), args.ErrCode), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: ErrMsg    [{1}]", Gradual.Utils.MethodHelper.GetCurrentMethod(), args.ErrMsg), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: ListCount [{1}]", Gradual.Utils.MethodHelper.GetCurrentMethod(), args.Orders.Count), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: ****************************************************************************************************", Gradual.Utils.MethodHelper.GetCurrentMethod()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                    switch (args.ErrCode)
                    {
                        case 0:
                            {
                                if (args.Orders.Count > 0)
                                { 
                                    filaResponse.Enfileira(args);
                                }
                                else
                                {
                                    MessageBox.Show("A pesquisa não retornou resultado!");
                                }
                            }
                            break;
                        case 1001: //Account not found
                            {
                                MethodInvoker wrapper = () =>
                                {
                                    MessageBox.Show("Conta não encontrada!");
                                };

                                try
                                {
                                    this.BeginInvoke(wrapper, null);
                                }
                                catch (System.ObjectDisposedException)
                                {
                                    // Ignore.  Control is disposed cannot update the UI.
                                }
                            }
                            break;
                        case 1002: //Maximum number of registries exceeded
                            {
                                MethodInvoker wrapper = () =>
                                {
                                    MessageBox.Show(String.Format("Número máximo de registros ({0}) excedido!\r\nFavor refinar a pesquisa!", Constantes.NumeroMaximoRegistros));
                                };

                                try
                                {
                                    this.BeginInvoke(wrapper, null);
                                }
                                catch (System.ObjectDisposedException)
                                {
                                    // Ignore.  Control is disposed cannot update the UI.
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Client_OnFilterDetailInfoResponseReceived(object sender, int clientNumber, System.Net.Sockets.Socket clientSocket, Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterDetailInfoResponse args)
        {
            try
            {
                MethodInvoker wrapper = () =>
                {
                    foreach (Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo lDetail in args.Details)
                    {
                        spiderOrderDetailInfoBindingSource.Add(lDetail);
                    }
                };

                try
                {
                    this.BeginInvoke(wrapper, null);
                }
                catch (System.ObjectDisposedException)
                {
                    // Ignore.  Control is disposed cannot update the UI.
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Client_OnStreamerOrderInfoReceived(object sender, int clientNumber, System.Net.Sockets.Socket clientSocket, Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo args)
        {
            try
            {
                filaResponseStream.Enfileira(args);
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void CancelarOrdem()
        {
            try
            {
                //String lAccount = String.Empty;
                //String lClOrdID =String.Empty;
                //String lRegisterTime = String.Empty;
                //String lSymbol = String.Empty;

                //Int32 lStatus;
                //Gradual.OMS.RoteadorOrdens.Lib.IRoteadorOrdens lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.RoteadorOrdens.Lib.IRoteadorOrdens>();

                //Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemRequest lRequest = new Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemRequest();
                //Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemResponse lResponse;
                //lRequest.info = new OMS.RoteadorOrdens.Lib.Dados.OrdemCancelamentoInfo();

                //foreach (DataGridViewRow lRow in gridResumo.SelectedRows)
                //{
                //    lStatus = Int32.Parse(lRow.Cells["columnOrdStatus"].Value.ToString());
                //    lClOrdID = lRow.Cells["columnClOrdID"].Value.ToString();
                //    lAccount = lRow.Cells["columnAccount"].Value.ToString();
                //    lRegisterTime = lRow.Cells["columnRegisterTime"].Value.ToString();
                //    lSymbol = lRow.Cells["columnSymbol"].Value.ToString();

                //    DialogResult result = MessageBox.Show(String.Format("Essa ação irá cancelar a ordem\r\nOrdem: {0}\r\nAtivo: {1}\r\nCliente: {2}\r\nData/Hora: {3}.\r\nDeseja continuar mesmo assim?", lClOrdID, lSymbol, lAccount, lRegisterTime), "Cancelamento Global", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                //    if (result == DialogResult.Yes)
                //    {
                //        OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum lSide = lRow.Cells["columnSide"].Value.ToString().Equals("1") ? OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra : OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda;

                //        if (lRow.Cells["columnAccountDv"].Value.ToInt32() > -1)
                //        {
                //            lAccount = lRow.Cells["columnAccountDv"].Value.ToString();
                //        }
                //        else
                //        {
                //            lAccount = lRow.Cells["columnAccount"].Value.ToString();
                //        }

                //        if (lStatus.Equals(0) || lStatus.Equals(1) || lStatus.Equals(5))
                //        {
                //            lRequest.info.Account = Int32.Parse(lAccount);
                //            lRequest.info.ChannelID = Constantes.ChannelIDCancelamento; //TODO: colocar configuravel
                //            lRequest.info.ClOrdID = Aplicacao.GerarClOrdId(lAccount, lClOrdID);
                //            lRequest.info.OrigClOrdID = lRow.Cells["columnClOrdID"].Value.ToString();
                //            lRequest.info.OrderID = lRow.Cells["columnExchangeNumberID"].Value.ToString();
                //            lRequest.info.Symbol = lRow.Cells["columnSymbol"].Value.ToString();
                //            lRequest.info.SecurityID = lRow.Cells["columnSymbol"].Value.ToString();
                //            lRequest.info.CompIDBolsa = lRow.Cells["columnSessionIDOriginal"].Value.ToString();
                //            lRequest.info.Exchange = lRow.Cells["columnExchange"].Value.ToString().ToUpper();
                //            lRequest.info.Side = lSide;
                //            lRequest.info.OrderQty = Int32.Parse(lRow.Cells["columnOrderQty"].Value.ToString());
                //            lRequest.info.ExecBroker = lRow.Cells["columnExecBroker"].Value.ToString();

                //            lResponse = lServico.CancelarOrdem(lRequest);

                //            if (lResponse.DadosRetorno.StatusResposta == Gradual.OMS.RoteadorOrdens.Lib.Dados.StatusRoteamentoEnum.Sucesso)
                //            {
                //                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lResponse.DadosRetorno.StackTrace), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                //            }
                //            else
                //            {
                //                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lResponse.DadosRetorno.StackTrace), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                //            }
                //        }
                //        else
                //        {
                //            MessageBox.Show(String.Format("Somente ordens NOVAS, SUBSTITUIDAS ou PARCIALMENTE EXECUTADAS podem ser canceladas.\r\nOrdem:{0}", lClOrdID), "Cancelamento de ordem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //            return;
                //        }
                //    }
                //}
                
                String lAccount = String.Empty;
                String lClOrdID =String.Empty;
                String lRegisterTime = String.Empty;
                String lSymbol = String.Empty;

                Int32 lStatus;
                Gradual.Spider.SupervisorFix.Lib.ISupervisorFix lServicoFix = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.Spider.SupervisorFix.Lib.ISupervisorFix>();
                Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemRequest lRequest = new Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemRequest();

                lRequest.info = new OMS.RoteadorOrdens.Lib.Dados.OrdemCancelamentoInfo();
                string ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToString();
                string id = ContextoGlobal.CodigoUsuario;
                string nome = ContextoGlobal.Usuario.NomeUsuario;
                string email = ContextoGlobal.Usuario.Email;
                string codassessor = ContextoGlobal.Usuario.CodAssessor.ToString();

                foreach (DataGridViewRow lRow in gridResumo.SelectedRows)
                {
                    lStatus = Int32.Parse(lRow.Cells["columnOrdStatus"].Value.ToString());
                    lClOrdID = lRow.Cells["columnClOrdID"].Value.ToString();
                    lAccount = lRow.Cells["columnAccount"].Value.ToString();
                    lRegisterTime = lRow.Cells["columnRegisterTime"].Value.ToString();
                    lSymbol = lRow.Cells["columnSymbol"].Value.ToString();

                    DialogResult result = MessageBox.Show(String.Format("Essa ação irá cancelar a ordem\r\nOrdem: {0}\r\nAtivo: {1}\r\nCliente: {2}\r\nData/Hora: {3}.\r\nDeseja continuar mesmo assim?", lClOrdID, lSymbol, lAccount, lRegisterTime), "Cancelamento Global", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum lSide = lRow.Cells["columnSide"].Value.ToString().Equals("1") ? OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra : OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda;

                        if (lRow.Cells["columnAccountDv"].Value.ToInt32() > -1)
                        {
                            lAccount = lRow.Cells["columnAccountDv"].Value.ToString();
                        }
                        else
                        {
                            lAccount = lRow.Cells["columnAccount"].Value.ToString();
                        }

                        if (lStatus.Equals(0) || lStatus.Equals(1) || lStatus.Equals(5))
                        {
                            Gradual.Spider.SupervisorFix.Lib.Dados.OrderCancelationRequest lReqCancel = new Gradual.Spider.SupervisorFix.Lib.Dados.OrderCancelationRequest();
                            lReqCancel.User = id;
                            lReqCancel.Ip = ip;
                            lReqCancel.NomeUsuario = nome;
                            lReqCancel.Email = email;
                            lReqCancel.CodAssessor = codassessor;
                            lRequest.info.Account = Int32.Parse(lAccount);
                            lRequest.info.ChannelID = Constantes.ChannelIDCancelamento; //TODO: colocar configuravel
                            lRequest.info.ClOrdID = Aplicacao.GerarClOrdId(lAccount, lClOrdID);
                            lRequest.info.OrigClOrdID = lRow.Cells["columnClOrdID"].Value.ToString();
                            lRequest.info.OrderID = lRow.Cells["columnExchangeNumberID"].Value.ToString();
                            lRequest.info.Symbol = lRow.Cells["columnSymbol"].Value.ToString();
                            lRequest.info.SecurityID = lRow.Cells["columnSymbol"].Value.ToString();
                            lRequest.info.CompIDBolsa = lRow.Cells["columnSessionIDOriginal"].Value.ToString();
                            lRequest.info.Exchange = lRow.Cells["columnExchange"].Value.ToString().ToUpper();
                            lRequest.info.Side = lSide;
                            lRequest.info.OrderQty = Int32.Parse(lRow.Cells["columnOrderQty"].Value.ToString());
                            lRequest.info.ExecBroker = lRow.Cells["columnExecBroker"].Value.ToString();
                            lReqCancel.CancelReq = lRequest;

                            //lResponse = lServico.CancelarOrdem(lRequest);
                            Gradual.Spider.SupervisorFix.Lib.Dados.OrderCancelationResponse lResponse = lServicoFix.CancelOrder(lReqCancel);

                            if (lResponse.ErrCode.Equals(0))
                            {
                                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lResponse.ErrDesc), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                            }
                            else
                            {
                                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lResponse.ErrDesc), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                            }
                        }
                        else
                        {
                            MessageBox.Show(String.Format("Somente ordens NOVAS, SUBSTITUIDAS ou PARCIALMENTE EXECUTADAS podem ser canceladas.\r\nOrdem:{0}", lClOrdID), "Cancelamento de ordem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void CancelarTudo()
        {
            fCancel frmCancel = new fCancel();
            Loading(true, true);

            try
            {


                DialogResult lMainResult = MessageBox.Show("Essa ação irá cancelar todas as ordens Abertas ou Parcialmente Executadas.\r\nDeseja continuar mesmo assim?", "Cancelamento Global", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (lMainResult == DialogResult.Yes)
                {
                    SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo> lSource;

                    if (lFiltered)
                    {
                        lSource = lFilteredSource;
                    }
                    else
                    {
                        lSource = lOriginalSource;
                    }

                    foreach (Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo lOrder in lSource)
                    {
                        if (Int32.Parse(lOrder.OrdStatus.ToString()).Equals(0) || Int32.Parse(lOrder.OrdStatus.ToString()).Equals(1) || Int32.Parse(lOrder.OrdStatus.ToString()).Equals(5))
                        {
                            frmCancel.AddOrder(lOrder);
                        }
                    }

                    frmCancel.ShowDialog();
                    
                }
                else
                {
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: o cancelamento global foi abortado pelo usuário.", Gradual.Utils.MethodHelper.GetCurrentMethod()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            Loading(false, true);
        }

        private void Desconectar()
        {
            try
            {
                if (lClient != null && lClient.IsConectado())
                {
                    lClient.OnConnectionOpened -= Client_OnConnectionOpened;
                    lClient.CloseSocket();
                    lClient.Dispose();
                    lClient = null;
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        #endregion

        #region Filtro

        private Gradual.Spider.Acompanhamento4Socket.Lib.Dados.FilterSpiderOrder MontarFiltro()
        {
            Gradual.Spider.Acompanhamento4Socket.Lib.Dados.FilterSpiderOrder lRetorno = new Gradual.Spider.Acompanhamento4Socket.Lib.Dados.FilterSpiderOrder();

            try
            {
                //"ClOrdId":
                if (!String.IsNullOrEmpty(txtNumeroOrdem.Text))
                {
                    lRetorno.ClOrdID.Compare = Constantes.Igual;
                    lRetorno.ClOrdID.Value = txtNumeroOrdem.Text.Trim();
                }

                //"Symbol":
                if (!String.IsNullOrEmpty(txtSimbolo.Text.ToUpper()))
                {
                    lRetorno.Symbol.Compare = Constantes.Igual;
                    lRetorno.Symbol.Value = txtSimbolo.Text.Trim();
                }

                //"Account":
                if (!String.IsNullOrEmpty(txtConta.Text))
                {
                    lRetorno.Account.Compare = Constantes.Igual;
                    lRetorno.Account.Value = Int32.Parse(txtConta.Text.Trim());
                }

                //"Side":
                if (cmbLado.SelectedIndex > 0)
                {
                    lRetorno.Side.Compare = Constantes.Igual;
                    lRetorno.Side.Value = Int32.Parse(((ComboboxItem)cmbLado.SelectedItem).Value.ToString());
                }

                //"OrdStatus":
                if (cmbEstado.SelectedIndex > 0)
                {
                    lRetorno.OrdStatus.Compare = Constantes.Igual;
                    lRetorno.OrdStatus.Value = Int32.Parse(((ComboboxItem)cmbEstado.SelectedItem).Value.ToString());
                }

                //"OrderType:"
                if (cmbTipo.SelectedIndex > 0)
                {
                    lRetorno.OrdTypeID.Compare = Constantes.Igual;
                    lRetorno.OrdTypeID.Value = ((ComboboxItem)cmbTipo.SelectedItem).Value.ToString();
                }

                //"ExecBroker":
                if (cmbCorretora.SelectedIndex > 0)
                {
                    lRetorno.ExecBroker.Compare = Constantes.Igual;
                    lRetorno.ExecBroker.Value = ((ComboboxItem)cmbCorretora.SelectedItem).Value.ToString();
                }

                //"TimeInForce":
                if (cmbValidade.SelectedIndex > 0)
                {
                    lRetorno.TimeInForce.Compare = Constantes.Igual;
                    lRetorno.TimeInForce.Value = ((ComboboxItem)cmbValidade.SelectedItem).Value.ToString();
                }

                //"SessionIDOriginal":
                if (cmbSessao.SelectedIndex > 0)
                {
                    lRetorno.SessionIDOriginal.Compare = Constantes.Igual;
                    lRetorno.SessionIDOriginal.Value = ((ComboboxItem)cmbSessao.SelectedItem).Value.ToString();
                }

                //"Exchange":
                //lRetorno.Exchange.Compare = Igual;
                //lRetorno.Exchange.Value = "";

                //"OrigClOrdID"
                //lRetorno.OrigClOrdID.Compare = Igual;
                //lRetorno.OrigClOrdID.Value = ;

                //"ExchangeNumberID":
                //lRetorno.ExchangeNumberID.Compare = Igual;
                //lRetorno.ExchangeNumberID.Value = ;

                //"ClOrdID":
                //lRetorno.ClOrdID.Compare = Igual;
                //lRetorno.ClOrdID.Value = ;

                //"SecurityExchangeID":
                //ret.SecurityExchangeID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                //ret.SecurityExchangeID.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();

                //"StopStartID":
                //ret.StopStartID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                //ret.StopStartID.Value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());

                //"OrdTypeID":
                //lRetorno.OrdTypeID.Compare = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                //lretorno.OrdTypeID.Value = dataGridView1.Rows[i].Cells[1].Value.ToString();

                //"RegisterTime":
                //lRetorno.RegisterTime.Compare = Igual;
                //lRetorno.RegisterTime.Value = ;

                //"TransactTime":
                //lRetorno.TransactTime.Compare = Igual;
                //lRetorno.TransactTime.Value = ;

                //"ExpireDate":
                //lRetorno.ExpireDate.Compare = Igual;
                //lRetorno.ExpireDate.Value = ;

                //"ChannelID":
                //lRetorno.ChannelID.Compare = Igual;
                //lRetorno.ChannelID.Value = "";

                //"ExecBroker":
                //lRetorno.ExecBroker.Compare = Igual;
                //lRetorno.ExecBroker.Value = "";

                //"Side":
                //lRetorno.Side.Compare = Igual;
                //lRetorno.Side.Value = "";

                //"OrderQty":
                //lRetorno.OrderQty.Compare = Igual;
                //lRetorno.OrderQty.Value = "";

                //"OrderQtyMin":
                //lRetorno.OrderQtyMin.Compare = Igual;
                //lRetorno.OrderQtyMin.Value = 0;

                //"OrderQtyApar":
                //lRetorno.OrderQtyApar.Compare = Igual;
                //lRetorno.OrderQtyApar.Value = 0;

                //"OrderQtyRemaining":
                //lRetorno.OrderQtyRemaining.Compare = Igual;
                //lRetorno.OrderQtyRemaining.Value = "";

                //"Price":
                //lRetorno.Price.Compare = Igual;
                //lRetorno.Price.Value = 0;

                //"StopPx":
                //lRetorno.StopPx.Compare = Igual;
                //lRetorno.StopPx.Value = 0;

                //"Description":
                //lRetorno.Description.Compare = Igual;
                //lRetorno.Description.Value = "";

                //"SystemID":
                //lRetorno.SystemID.Compare = Igual;
                //lRetorno.SystemID.Value = "";

                //"Memo":
                //lRetorno.Memo.Compare = Igual;
                //lRetorno.Memo.Value = "";

                //"CumQty":
                //lRetorno.CumQty.Compare = Igual;
                //lRetorno.CumQty.Value = "";

                //"FixMsgSeqNum":
                //lRetorno.FixMsgSeqNum.Compare = Igual;
                //lRetorno.FixMsgSeqNum.Value = "";

                //"SessionID":
                //lRetorno.SessionID.Compare = Igual;
                //lRetorno.SessionID.Value = "";

                //"SessionIDOriginal":
                //lRetorno.SessionIDOriginal.Compare = Igual;
                //lRetorno.SessionIDOriginal.Value = "";

                //"IdFix":
                //lRetorno.IdFix.Compare = Igual;
                //lRetorno.IdFix.Value = "";

                //"MsgFix":
                //lRetorno.MsgFix.Compare = Igual;
                //lRetorno.MsgFix.Value = "";

                //"Msg42Base64":
                //lRetorno.Msg42Base64.Compare = Igual;
                //lRetorno.Msg42Base64.Value = "";

                //"HandlInst":
                //lRetorno.HandlInst.Compare = Igual;
                //lRetorno.HandlInst.Value = "";



                //"IntegrationName":
                //lRetorno.IntegrationName.Compare = Igual;
                //lRetorno.IntegrationName.Value = "";

                return lRetorno;
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
            finally
            {
                
            }

            return lRetorno;
        }

        #endregion

        #region Métodos e procedimentos privados de ainterface

        public fOrder()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            loadingPanel1.Dock = DockStyle.Fill;

            loadingPanel1.UseFadedBackground = true;
            loadingPanel1.BackgroundForm = this;

            GradualForm.Engine.ConfigureFormRender(this);

            CriarColunas();

            IniciarProcessamento();

            splitContainer1.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;

            if (lOnAlterarCoresHandler == null)
            {
                lOnAlterarCoresHandler = new Aplicacao.OnAlterarCoresHandler(OnAlterarCores);
            }

            Aplicacao.OnAlterarCoresEvent += lOnAlterarCoresHandler;

            Conectar();
        }

        private void fOrder_Load(object sender, EventArgs e)
        {
            try
            {
                BuscarParametrosConfiguracoes();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }


        private void IniciarProcessamento()
        {
            filaResponseStream = new Gradual.Utils.Fila<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo>();
            tTrataResponseStream = new System.Threading.Thread(ThreadTrataResponseStream);
            tTrataResponseStream.Name = "fOrder.tTrataResponseStream()";
            tTrataResponseStream.SetApartmentState(System.Threading.ApartmentState.MTA);
            tTrataResponseStream.Start(filaResponseStream);

            filaResponseDetailStream = new Gradual.Utils.Fila<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo>();
            tTrataResponseDetailStream = new System.Threading.Thread(ThreadTrataResponseDetailStream);
            tTrataResponseDetailStream.Name = "fOrder.tTrataResponseDetailStream()";
            tTrataResponseDetailStream.SetApartmentState(System.Threading.ApartmentState.MTA);
            tTrataResponseDetailStream.Start(filaResponseDetailStream);
        }

        private void ThreadTrataResponse(Object filaMensagens)
        {
            Gradual.Utils.Fila<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterInfoResponse> fila = ((Gradual.Utils.Fila<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterInfoResponse>)filaMensagens);

            while (true)
            {
                try
                {
                    Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterInfoResponse msg;
                    fila.TentaDesinfileirar(out msg);
                    if (!this.IsDisposed)
                    {
                        ProcessarResumo(msg);
                    }
                }
                catch (Exception ex)
                {
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }
            }
        }

        private void ThreadTrataResponseStream(Object filaMensagens)
        {
            Gradual.Utils.Fila<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo> fila = ((Gradual.Utils.Fila<Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo>)filaMensagens);

            while (true)
            {
                try
                {
                    Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo msg;
                    fila.TentaDesinfileirar(out msg);
                    
                    if (!this.IsDisposed)
                    {
                        ProcessarResumo(msg);
                    }
                }
                catch (Exception ex)
                {
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }
            }
        }

        private void ThreadTrataResponseDetailStream(Object filaMensagens)
        {
            Gradual.Utils.Fila<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo> fila = ((Gradual.Utils.Fila<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo>)filaMensagens);

            while (true)
            {
                try
                {
                    Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo msg;
                    fila.TentaDesinfileirar(out msg);

                    if (!this.IsDisposed)
                    {
                        ProcessarDetail(msg);
                    }
                }
                catch (Exception ex)
                {
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }
            }
        }

        private void BuscarParametrosConfiguracoes()
        {
            try
            {
                Gradual.Spider.Servicos.Configuracoes.Lib.IServicoConfiguracoes lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.Spider.Servicos.Configuracoes.Lib.IServicoConfiguracoes>();
                Gradual.Spider.Servicos.Configuracoes.Lib.Mensageria.BuscarParametrosRequest lRequest = new Gradual.Spider.Servicos.Configuracoes.Lib.Mensageria.BuscarParametrosRequest();

                var lResponse = lServico.BuscarParametros(lRequest);

                if (lResponse.StatusResposta.Equals(Gradual.OMS.Library.MensagemResponseStatusEnum.OK))
                {
                    foreach (Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro lParametro in lResponse.Parametros)
                    {
                        switch (lParametro.Parameter)
                        {
                            case Servicos.Configuracoes.Lib.Classes.ParameterType.ExecBroker:
                                {
                                    if (!Parametros.lListaExecBrokers.Contains(lParametro))
                                    {
                                        Parametros.lListaExecBrokers.Add(lParametro);
                                    }
                                    cmbCorretora.Items.Add(new ComboboxItem(lParametro));
                                }
                                break;
                            case Servicos.Configuracoes.Lib.Classes.ParameterType.OrderSide:
                                {
                                    if (!Parametros.lListaOrderSides.Contains(lParametro))
                                    {
                                        Parametros.lListaOrderSides.Add(lParametro);
                                    }

                                    if (lParametro.Value.ToString().Equals("1"))
                                    {
                                        ComboboxItem lCombo = new ComboboxItem(lParametro);
                                        lCombo.Text = "Compra";
                                        cmbLado.Items.Add(lCombo);
                                    }

                                    if (lParametro.Value.ToString().Equals("2"))
                                    {
                                        ComboboxItem lCombo = new ComboboxItem(lParametro);
                                        lCombo.Text = "Venda";
                                        cmbLado.Items.Add(lCombo);
                                    }
                                }
                                break;
                            case Servicos.Configuracoes.Lib.Classes.ParameterType.OrderStatus:
                                {
                                    if (!Parametros.lListaOrderStatus.Contains(lParametro))
                                    {
                                        Parametros.lListaOrderStatus.Add(lParametro);
                                    }

                                    if (StatusDictionary.Keys.Contains(lParametro.Description))
                                    {
                                        String lDescription = StatusDictionary[lParametro.Description];
                                        checkedListStatus.Items.Add(new CheckedListItem(lDescription, Int32.Parse(lParametro.Value)), true);
                                    }
                                }
                                break;
                            case Servicos.Configuracoes.Lib.Classes.ParameterType.OrderType:
                                {
                                    if (!Parametros.lListaOrderTypes.Contains(lParametro))
                                    {
                                        Parametros.lListaOrderTypes.Add(lParametro);
                                    }

                                    cmbTipo.Items.Add(new ComboboxItem(lParametro));
                                }
                                break;
                            case Servicos.Configuracoes.Lib.Classes.ParameterType.Session:
                                {
                                    if (!Parametros.lListaSessions.Contains(lParametro))
                                    {
                                        Parametros.lListaSessions.Add(lParametro);
                                    }

                                    cmbSessao.Items.Add(new ComboboxItem(lParametro));
                                    cmbSignSession.Items.Add(new ComboboxItem(lParametro));
                                }
                                break;
                            case Servicos.Configuracoes.Lib.Classes.ParameterType.Validity:
                                {
                                    if (!Parametros.lListaValidities.Contains(lParametro))
                                    {
                                        Parametros.lListaValidities.Add(lParametro);
                                    }

                                    cmbValidade.Items.Add(new ComboboxItem(lParametro));
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                Pesquisar();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void pnlDetailTop_Click(object sender, EventArgs e)
        {

        }

        private void lblDetalhe_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlDetailMain.Visible)
                {
                    AlturaDetalhe = pnlDetailMain.Height;
                    pnlDetailMain.Visible = false;
                    splitContainer1.SplitterDistance = pnlMiddle.Height - 20;
                    lblDetalhe.Text = "+ Detalhe";
                }
                else
                {
                    pnlDetailMain.Visible = true;
                    splitContainer1.SplitterDistance = pnlMiddle.Height - (AlturaDetalhe + 20);
                    lblDetalhe.Text = "- Detalhe";
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Resumo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.ColumnIndex == 0)
                {
                    if (lConfirmation)
                    {
                        CancelarOrdem();
                    }
                }

                this.spiderOrderDetailInfoBindingSource.Clear();
                this.spiderOrderDetailInfoBindingSource.DataSource = this.lSelectedOrderDetails;

                lSelectedOrderID = Int32.Parse(gridResumo.Rows[e.RowIndex].Cells["colOrderId"].Value.ToString());
                var lOcorrencias = this.lOrderDetails.AsEnumerable();
                lOcorrencias =
                from a in lOcorrencias
                where
                a.OrderID.Equals(lSelectedOrderID)
                orderby a.TransactTime descending 
                select a;

                if (lOcorrencias.Count() > 0)
                {
                    foreach(Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo lDetail in lOcorrencias)
                    {
                        this.lSelectedOrderDetails.Add(lDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Resumo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (e.Value == null)
                    {
                        return;
                    }

                    if (gridResumo.Columns[e.ColumnIndex].Name.Equals("columnSide"))
                    {
                        if (e.Value.ToString().Equals("1"))
                        {
                            e.Value = "Compra";
                        }

                        if (e.Value.ToString().Equals("2"))
                        {
                            e.Value = "Venda";
                        }
                    }

                    if (gridResumo.Columns[e.ColumnIndex].Name.Equals("columnOrdStatus"))
                    {
                        var lOcorrencias = Parametros.lListaOrderStatus.AsEnumerable();
                        lOcorrencias =
                            from a in lOcorrencias
                            where
                            a.Value.Equals(e.Value.ToString())
                            select a;

                        if (lOcorrencias.Count() > 0)
                        {
                            e.Value = ((Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro)lOcorrencias.ToList()[0]).Description.ToString();
                        }
                    }

                    if (gridResumo.Columns[e.ColumnIndex].Name.Equals("columnTimeInForce"))
                    {
                        var lOcorrencias = Parametros.lListaValidities.AsEnumerable();
                        lOcorrencias =
                            from a in lOcorrencias
                            where
                            a.Value.Equals(e.Value.ToString())
                            select a;

                        if (lOcorrencias.Count() > 0)
                        {
                            e.Value = ((Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro)lOcorrencias.ToList()[0]).Description.ToString();
                        }
                    }

                    if (gridResumo.Columns[e.ColumnIndex].Name.Equals("columnOrdTypeID"))
                    {
                        var lOcorrencias = Parametros.lListaOrderTypes.AsEnumerable();
                        lOcorrencias =
                            from a in lOcorrencias
                            where
                            a.Value.Equals(e.Value.ToString())
                            select a;

                        if (lOcorrencias.Count() > 0)
                        {
                            e.Value = ((Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro)lOcorrencias.ToList()[0]).Description.ToString();
                        }
                    }

                    if (gridResumo.Columns[e.ColumnIndex].Name.Equals("columnRegisterTime"))
                    {
                        string lDataHora = e.Value.ToString();
                        gridResumo.Rows[e.RowIndex].Cells["columnHora"].Value = lDataHora.ToString().Substring(11, 8);
                    }

                    if (gridResumo.Columns[e.ColumnIndex].Name.Equals("Price") || gridResumo.Columns[e.ColumnIndex].Name.Equals("StopPx"))
                    {
                        e.Value = String.Format("{0:C}", e.Value);
                    }

                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Resumo_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewCellStyle lRowStyle = this.gridResumo.Rows[e.RowIndex].DefaultCellStyle;

                    string lStatus = this.gridResumo.Rows[e.RowIndex].Cells["columnOrdStatus"].Value.ToString();

                    var lOcorrencias = Parametros.lListaOrderStatus.AsEnumerable();
                    lOcorrencias =
                        from a in lOcorrencias
                        where
                        a.Value.Equals(lStatus)
                        select a;

                    if (lOcorrencias.Count() > 0)
                    {
                        lStatus = ((Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro)lOcorrencias.ToList()[0]).Description.ToString();

                        if (Aplicacao.CoresStatus.ContainsKey(lStatus))
                        {
                            if (e.RowIndex % 2 == 0)
                            {
                                lRowStyle.BackColor = Aplicacao.CoresStatus[lStatus];
                            }
                            else
                            {
                                Color _cor = Aplicacao.CoresStatus[lStatus];
                                lRowStyle.BackColor = Color.FromArgb(_cor.R.SubtractFromInt(20), _cor.G.SubtractFromInt(20), _cor.B.SubtractFromInt(20));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Detalhe_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewCellStyle lRowStyle = this.gridDetalhe.Rows[e.RowIndex].DefaultCellStyle;

                    string lStatus = this.gridDetalhe.Rows[e.RowIndex].Cells["columnDetailOrderStatusId"].Value.ToString();

                    var lOcorrencias = Parametros.lListaOrderStatus.AsEnumerable();
                    lOcorrencias =
                        from a in lOcorrencias
                        where
                        a.Value.Equals(lStatus)
                        select a;

                    if (lOcorrencias.Count() > 0)
                    {
                        lStatus = ((Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro)lOcorrencias.ToList()[0]).Description.ToString();

                        if (Aplicacao.CoresStatus.ContainsKey(lStatus))
                        {
                            lRowStyle.BackColor = Aplicacao.CoresStatus[lStatus];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void Detalhe_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (gridDetalhe.Columns[e.ColumnIndex].Name.Equals("columnDetailOrderStatusId"))
                {
                    var lOcorrencias = Parametros.lListaOrderStatus.AsEnumerable();
                    lOcorrencias =
                        from a in lOcorrencias
                        where
                        a.Value.Equals(e.Value.ToString())
                        select a;

                    if (lOcorrencias.Count() > 0)
                    {
                        e.Value = ((Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro)lOcorrencias.ToList()[0]).Description.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void btnConfiguracoes_Click(object sender, EventArgs e)
        {
            try
            {
                fCores lfCores = new fCores();
                lfCores.Show();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void OnAlterarCores(object sender, EventArgs e)
        {
            try
            {
                this.gridResumo.Invalidate();
                this.gridDetalhe.Invalidate();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void fOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void ProcessarResumo(Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterInfoResponse msg)
        {
            try
            {

                foreach (Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo lOrder in msg.Orders)
                {
                    //Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo lNewOrder = new Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo();
                    //lNewOrder.Account = lOrder.Account;
                    //lNewOrder.ChannelID = lOrder.ChannelID;
                    //lNewOrder.ClOrdID = lOrder.ClOrdID;
                    //lNewOrder.CumQty = lOrder.CumQty;
                    //lNewOrder.Description = lOrder.Description;
                    //lNewOrder.Exchange = lOrder.Exchange;
                    //lNewOrder.ExchangeNumberID = lOrder.ExchangeNumberID;
                    //lNewOrder.ExecBroker = lOrder.ExecBroker;
                    //lNewOrder.ExpireDate = lOrder.ExpireDate;
                    //lNewOrder.FixMsgSeqNum = lOrder.FixMsgSeqNum;
                    //lNewOrder.HandlInst = lOrder.HandlInst;
                    //lNewOrder.IdFix = lOrder.IdFix;
                    //lNewOrder.IntegrationName = lOrder.IntegrationName;
                    //lNewOrder.Memo = lOrder.Memo;
                    //lNewOrder.Msg42Base64 = lOrder.Msg42Base64;
                    //lNewOrder.MsgFix = lOrder.MsgFix;
                    //lNewOrder.OrderID = lOrder.OrderID;
                    //lNewOrder.OrderQty = lOrder.OrderQty;
                    //lNewOrder.OrderQtyApar = lOrder.OrderQtyApar;
                    //lNewOrder.OrderQtyMin = lOrder.OrderQtyMin;
                    //lNewOrder.OrderQtyRemaining = lOrder.OrderQtyRemaining;
                    //lNewOrder.OrdStatus = lOrder.OrdStatus;
                    //lNewOrder.OrdTypeID = lOrder.OrdTypeID;
                    //lNewOrder.OrigClOrdID = lOrder.OrigClOrdID;
                    //lNewOrder.Price = lOrder.Price;
                    //lNewOrder.RegisterTime = lOrder.RegisterTime;
                    //lNewOrder.SecurityExchangeID = lOrder.SecurityExchangeID;
                    //lNewOrder.SessionID = lOrder.SessionID;
                    //lNewOrder.SessionIDOriginal = lOrder.SessionIDOriginal;
                    //lNewOrder.Side = lOrder.Side;
                    //lNewOrder.StopPx = lOrder.StopPx;
                    //lNewOrder.StopStartID = lOrder.StopStartID;
                    //lNewOrder.Symbol = lOrder.Symbol;
                    //lNewOrder.SystemID = lOrder.SystemID;
                    //lNewOrder.TimeInForce = lOrder.TimeInForce;
                    //lNewOrder.TransactTime = lOrder.TransactTime;
                    //AdicionarRegistro(lOrder);
                    //this.lOrders.Add(lOrder);
                    MethodInvoker wrapperTela = () =>
                    {
                        this.spiderOrderInfoBindingSource.Add(lOrder);
                        Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lOrder.OrigClOrdID.ToString()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                        this.gridResumo.Refresh();
                    };

                    try
                    {
                        this.BeginInvoke(wrapperTela);
                    }
                    catch (System.ObjectDisposedException ex)
                    {
                        Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                    }
                    catch (Exception ex)
                    {
                        Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                    }
                }
                //this.gridResumo.RowCount = this.lOrders.Count;
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        int lOrdersLoaded = 0;
        public delegate void OnProcessarCallBack(Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo msg);
        private void ProcessarResumo(Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.StreamerOrderInfo msg)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    OnProcessarCallBack call = new OnProcessarCallBack(ProcessarResumo);
                    this.Invoke(call, new object[] { msg });
                }
                else
                {
                    if (!lGuid.ToString().Equals(msg.Id))
                    {
                        return;
                    }

                    if (String.IsNullOrEmpty(msg.Order.ClOrdID))
                    {
                        Loading(false);
                        MessageBox.Show("Não existem ordens para a consulta efetuada!");
                        return;
                    }

                    if (msg.Order.OrdStatus.ToString().Equals("8"))
                    {
                        Console.Write("");
                    }

                    lLastMessage = DateTime.Now;

                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: Tipo -> {1} OrigClOrdId -> {2} Detalhes -> {3}", Gradual.Utils.MethodHelper.GetCurrentMethod(), msg.MsgType.ToString(), msg.Order.OrigClOrdID.ToString(), msg.Order.Details.Count), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                    var lOcorrencias = lOriginalSource.AsEnumerable();
                    lOcorrencias =
                        from a in lOcorrencias
                        where
                        a.OrderID.Equals(msg.Order.OrderID)
                        select a;

                    if (lOcorrencias.Count() > 0)
                    {
                        Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo lOrdemAntiga = lOcorrencias.ToList()[0];
                        this.lOriginalSource.Remove(lOrdemAntiga);
                        lOrdersLoaded--;
                        this.StatusText(String.Format("Ordens: {0}", lOrdersLoaded.ToString()));
                    }

                    if (msg.MsgType.Equals(Gradual.Spider.Acompanhamento4Socket.Lib.Dados.MsgTypeConst.SNAPSHOT))
                    {
                        lOrdersLoaded++;

                        this.lOriginalSource.Add(msg.Order);

                        if ((lOrdersLoaded % 100) == 0)
                        {
                            this.StatusText(String.Format("Ordens: {0}", lOrdersLoaded.ToString()));
                        }
                    }

                    if (msg.MsgType.Equals(Gradual.Spider.Acompanhamento4Socket.Lib.Dados.MsgTypeConst.INCREMENTAL))
                    {
                        if (loadingPanel1.Visible == true)
                        {
                            Loading(false);
                        }

                        if (this.lOriginalSource.Count > 0)
                        {
                            this.lOriginalSource.Insert(0, msg.Order);
                        }
                        else
                        {
                            this.lOriginalSource.Add(msg.Order);
                        }

                        lOrdersLoaded++;
                        this.StatusText(String.Format("Ordens: {0}", lOrdersLoaded.ToString()));

                        AddOrder(msg.Order);
                    }

                    if (lSelectedOrderID.Equals(msg.Order.OrderID))
                    {
                        if (msg.Order.Details.Count > 0)
                        {
                            this.spiderOrderDetailInfoBindingSource.Clear();
                            foreach (Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo lDetail in msg.Order.Details)
                            {
                                if (!this.spiderOrderDetailInfoBindingSource.Contains(lDetail))
                                {
                                    if (this.spiderOrderDetailInfoBindingSource.Count > 0)
                                    {
                                        this.spiderOrderDetailInfoBindingSource.Insert(0, lDetail);
                                    }
                                    else
                                    {
                                        this.spiderOrderDetailInfoBindingSource.Add(lDetail);
                                    }
                                }
                            }
                        }
                    }

                    if (msg.Order.Details.Count > 0)
                    {
                        foreach (Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo lDetail in msg.Order.Details)
                        {
                            filaResponseDetailStream.Enfileira(lDetail);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public static bool GetPropValue(object pSource, string pPropertyName, object pPropertyValue)
        {
            pSource.GetType().GetProperty(pPropertyName).GetValue(pSource, null);

            if (pSource.GetType().GetProperty(pPropertyName).GetValue(pSource, null) != null)
            {
                if (pSource.GetType().GetProperty(pPropertyName).GetValue(pSource, null).Equals(pPropertyValue))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddOrder(Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo pOrder)
        {
            if (lFiltered)
            {
                bool lAdd = true;
                
                if (lFiltered || (lStatus.Count() > 0))
                {
                    foreach (Utils.Filter.Filter lFilter in lFilters)
                    {
                        if (!GetPropValue(pOrder, lFilter.PropertyName, lFilter.Value))
                        {
                            lAdd = false;
                            break;
                        }
                    }

                    if (lAdd)
                    {
                        if (lStatus.Count() > 0)
                        {
                            foreach (String lStat in lStatus.Keys)
                            {
                                if (pOrder.OrdStatus.ToString().Equals(lStat))
                                {
                                    lAdd = true;
                                    break;
                                }
                                else
                                {
                                    lAdd = false;
                                }
                            }
                        }
                    }

                    var lOcorrencias = lFilteredSource.AsEnumerable();
                    lOcorrencias =
                        from a in lOcorrencias
                        where
                        a.OrderID.Equals(pOrder.OrderID)
                        select a;

                    if (lOcorrencias.Count() > 0)
                    {
                        Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo lOrdemAntiga = lOcorrencias.ToList()[0];
                        this.lFilteredSource.Remove(lOrdemAntiga);
                    }
                        
                    if (lAdd)
                    {
                        if (((SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>)this.lFilteredSource).Count > 0)
                        {
                            ((SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>)this.lFilteredSource).Insert(0, pOrder);
                        }
                        else
                        {
                            ((SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>)this.lFilteredSource).Add(pOrder);
                        }
                    }

                }
            }

            //this.lFilteredSource.Add(pOrder);
            //this.lOriginalSource.Add(pOrder);
        }

        public void Filtrar()
        {
            filter = new List<Gradual.Utils.Filter.Filter>();

            if (cmbSessao.SelectedIndex > 0)
            {
                String lSessao;
                lSessao = ((ComboboxItem)cmbSessao.SelectedItem).Value.ToString();

                filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "SessionIDOriginal", Operation = Gradual.Utils.Filter.Operation.AndEquals, Value = lSessao });
            }

            if (!String.IsNullOrEmpty(txtConta.Text))
            {
                Int32 lConta = Int32.Parse(txtConta.Text.ToString());
                filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "Account", Operation = Gradual.Utils.Filter.Operation.AndEquals, Value = lConta });
            }

            if (!String.IsNullOrEmpty(txtSimbolo.Text))
            {
                String lAtivo = txtSimbolo.Text.ToString().ToUpper();
                filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "Symbol", Operation = Gradual.Utils.Filter.Operation.AndEquals, Value = lAtivo });
            }

            if (!String.IsNullOrEmpty(txtNumeroOrdem.Text))
            {
                String lNumeroOrdem = txtNumeroOrdem.Text.ToString();
                filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "ClOrdID", Operation = Gradual.Utils.Filter.Operation.AndEquals, Value = lNumeroOrdem });
            }

            if (cmbEstado.SelectedIndex > 0)
            {
                Int32 lEstado;
                lEstado = Int32.Parse(((ComboboxItem)cmbEstado.SelectedItem).Value.ToString());
                filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "OrdStatus", Operation = Gradual.Utils.Filter.Operation.AndEquals, Value = lEstado });
            }

            if (cmbLado.SelectedIndex > 0)
            {
                Int32 lLado;
                lLado = Int32.Parse(((ComboboxItem)cmbLado.SelectedItem).Value.ToString());

                filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "Side", Operation = Gradual.Utils.Filter.Operation.AndEquals, Value = lLado });
            }

            if (cmbCorretora.SelectedIndex > 0)
            {
                String lCorretora;
                lCorretora = ((ComboboxItem)cmbCorretora.SelectedItem).Value.ToString();

                filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "ExecBroker", Operation = Gradual.Utils.Filter.Operation.AndEquals, Value = lCorretora });
            }

            if (cmbTipo.SelectedIndex > 0)
            {
                String lTipo;
                lTipo = ((ComboboxItem)cmbTipo.SelectedItem).Value.ToString();

                filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "OrdTypeID", Operation = Gradual.Utils.Filter.Operation.AndEquals, Value = lTipo});
            }

            if (cmbValidade.SelectedIndex > 0)
            {
                String lValidade;
                lValidade = ((ComboboxItem)cmbValidade.SelectedItem).Value.ToString();

                filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "TimeInForce", Operation = Gradual.Utils.Filter.Operation.AndEquals, Value = lValidade });
            }

            if ((filter.Count > 0) || (lStatus.Count > 0))
            {
                lFiltered = true;
                lFilters = new List<Utils.Filter.Filter>();

                List<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo> lOcorrencias;

                if (filter.Count > 0)
                {

                    lFilters = filter;
                    var deleg = Gradual.Utils.Filter.ExpressionBuilder.GetExpression<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>(filter).Compile();

                    lOcorrencias = ((SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>)this.lOriginalSource).Where(deleg).ToList();
                }
                else
                {
                    lOcorrencias = this.lOriginalSource.ToList();
                }


                if (lStatus.Count > 0)
                {

                    //lStatus = new string[ccmbStatus.CheckedItems.Count];
                    ////foreach (GradualForm.Controls.CCBoxItem item in ccmbStatus.CheckedItems)
                    //for (int i = 0; i < ccmbStatus.CheckedItems.Count; i++ )
                    //{
                    //    //filter.Add(new Gradual.Utils.Filter.Filter { PropertyName = "OrdStatus", Operation = Gradual.Utils.Filter.Operation.OrEquals, Value = Int32.Parse(item.Value.ToString()) });
                    //    lStatus[i] = ((GradualForm.Controls.CCBoxItem)ccmbStatus.CheckedItems[i]).Value.ToString();

                    //}

                    var lStatusFiltered = 
                        from a in lOcorrencias
                        where lStatus.ContainsKey(a.OrdStatus.ToString())
                        select a;

                    lOcorrencias = lStatusFiltered.ToList();
                }

                this.lFilteredSource = new SortableBindingList<Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>();
                this.spiderOrderInfoBindingSource.DataSource = this.lFilteredSource;

                foreach (Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo lOrder in lOcorrencias)
                {
                    this.spiderOrderInfoBindingSource.Add(lOrder);
                }

                ((System.Windows.Forms.BindingSource)this.spiderOrderInfoBindingSource).DataSource = lFilteredSource;
            }
            else
            {

            }

        }
        Int64 lDetailCount = 0;
        public delegate void OnProcessarDetailCallBack(Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo msg);
        private void ProcessarDetail(Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo msg)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    OnProcessarDetailCallBack call = new OnProcessarDetailCallBack(ProcessarDetail);
                    this.Invoke(call, new object[] { msg });
                }
                else
                {
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: OrderId -> {1} OrderDetailID -> {2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), msg.OrderID.ToString(), msg.OrderDetailID.ToString()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                    var lOcorrencias = lOrderDetails.AsEnumerable();
                    lOcorrencias =
                        from a in lOcorrencias
                        where
                        a.OrderDetailID.Equals(msg.OrderDetailID)
                        select a;

                    if (lOcorrencias.Count() > 0)
                    {
                        Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo lOrdemAntiga = lOcorrencias.ToList()[0];
                        this.lOrderDetails.Remove(lOrdemAntiga);
                    }

                    this.lOrderDetails.Add(msg);
                    lDetailCount++;
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        #endregion

        #region Métodos do modo virtual da Grid de Resumo 

        private void gridResumo_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // If this is the row for new records, no values are needed. 
            //if (e.RowIndex == ((DataGridView)sender).RowCount - 1) return;

            Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo resumoTmp = null;

            // Store a reference to the object for the row being painted. 
            if (e.RowIndex == rowInEdit)
            {
                resumoTmp = this.resumoInEdit;
            }
            else
            {
                resumoTmp = (Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex];
            }

            // Set the cell value to paint using the object retrieved. 
            switch (this.gridResumo.Columns[e.ColumnIndex].Name)
            {
                case "columnAccount":
                    e.Value = resumoTmp.Account;
                    break;

                case "columnChannelID":
                    e.Value = resumoTmp.ChannelID;
                    break;

                case "columnClOrdID":
                    e.Value = resumoTmp.ClOrdID;
                    break;

                case "columnCumQty":
                    e.Value = resumoTmp.CumQty;
                    break;

                case "columnDescription":
                    e.Value = resumoTmp.Description;
                    break;

                case "columnExchange":
                    e.Value = resumoTmp.Exchange;
                    break;

                case "columnExchangeNumberId":
                    e.Value = resumoTmp.ExchangeNumberID;
                    break;

                case "columnExecBroker":
                    e.Value = resumoTmp.ExecBroker;
                    break;

                case "columnExpireDate":
                    e.Value = resumoTmp.ExpireDate;
                    break;

                case "columnFixMsgSeqNum":
                    e.Value = resumoTmp.FixMsgSeqNum;
                    break;

                case "columnHandlInst":
                    e.Value = resumoTmp.HandlInst;
                    break;

                case "columnIdFix":
                    e.Value = resumoTmp.IdFix;
                    break;

                case "columnIntegrationName":
                    e.Value = resumoTmp.IntegrationName;
                    break;

                case "columnMemo":
                    e.Value = resumoTmp.Memo;
                    break;

                case "columnMsg42Base64":
                    e.Value = resumoTmp.Msg42Base64;
                    break;

                case "columnMsgFix":
                    e.Value = resumoTmp.MsgFix;
                    break;

                case "columnOrderID":
                    e.Value = resumoTmp.OrderID;
                    break;

                case "columnOrderQty":
                    e.Value = resumoTmp.OrderQty;
                    break;

                case "columnOrderQtyApar":
                    e.Value = resumoTmp.OrderQtyApar;
                    break;

                case "columnOrderQtyMin":
                    e.Value = resumoTmp.OrderQtyMin;
                    break;

                case "columnOrderQtyRemaining":
                    e.Value = resumoTmp.OrderQtyRemaining;
                    break;

                case "columnOrdStatus":
                    e.Value = resumoTmp.OrdStatus;
                    break;

                case "columnOrdTypeID":
                    e.Value = resumoTmp.OrdTypeID;
                    break;

                case "columnOrigClOrdID":
                    e.Value = resumoTmp.OrigClOrdID;
                    break;

                case "columnPrice":
                    e.Value = resumoTmp.Price;
                    break;

                case "columnRegisterTime":
                    e.Value = resumoTmp.RegisterTime;
                    break;

                case "columnSecurityExchangeID":
                    e.Value = resumoTmp.SecurityExchangeID;
                    break;

                case "columnSessionID":
                    e.Value = resumoTmp.SessionID;
                    break;

                case "columnSessionIDOriginal":
                    e.Value = resumoTmp.SessionIDOriginal;
                    break;

                case "columnStopPx":
                    e.Value = resumoTmp.StopPx;
                    break;

                case "columnStopStartID":
                    e.Value = resumoTmp.StopStartID;
                    break;

                case "columnSymbol":
                    e.Value = resumoTmp.Symbol;
                    break;

                case "columnSystemID":
                    e.Value = resumoTmp.SystemID;
                    break;

                case "columnTimeInForce":
                    e.Value = resumoTmp.TimeInForce;
                    break;

                case "columnTransactTime":
                    e.Value = resumoTmp.TransactTime;
                    break;
            }
        }

        private void gridResumo_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo resumoTmp = null;

            // Store a reference to theobject for the row being edited. 
            if (e.RowIndex < this.lOriginalSource.Count)
            {
                // If the user is editing a new row, create a new object. 
                if (this.resumoInEdit == null)
                {
                    this.resumoInEdit = new Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo();
                    this.resumoInEdit.Account  = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).Account;
                    this.resumoInEdit.ChannelID = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).ChannelID;
                    this.resumoInEdit.ClOrdID = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).ClOrdID;
                    this.resumoInEdit.CumQty = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).CumQty;
                    this.resumoInEdit.Description = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).Description;
                    this.resumoInEdit.Exchange = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).Exchange;
                    this.resumoInEdit.ExchangeNumberID = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).ExchangeNumberID;
                    this.resumoInEdit.ExecBroker = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).ExecBroker;
                    this.resumoInEdit.ExpireDate =  ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).ExpireDate;
                    this.resumoInEdit.FixMsgSeqNum = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).FixMsgSeqNum;
                    this.resumoInEdit.HandlInst = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).HandlInst;
                    this.resumoInEdit.IdFix = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).IdFix;
                    this.resumoInEdit.IntegrationName = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).IntegrationName;
                    this.resumoInEdit.Memo = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).Memo;
                    this.resumoInEdit.Msg42Base64 = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).Msg42Base64;
                    this.resumoInEdit.MsgFix = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).MsgFix;
                    this.resumoInEdit.OrderID = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).OrderID;
                    this.resumoInEdit.OrderQty = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).OrderQty;
                    this.resumoInEdit.OrderQtyApar = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).OrderQtyApar;
                    this.resumoInEdit.OrderQtyMin = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).OrderQtyMin;
                    this.resumoInEdit.OrderQtyRemaining = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).OrderQtyRemaining;
                    this.resumoInEdit.OrdStatus = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).OrdStatus;
                    this.resumoInEdit.OrdTypeID = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).OrdTypeID;
                    this.resumoInEdit.OrigClOrdID = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).OrigClOrdID;
                    this.resumoInEdit.Price = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).Price;
                    this.resumoInEdit.RegisterTime = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).RegisterTime;
                    this.resumoInEdit.SecurityExchangeID  = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).SecurityExchangeID;
                    this.resumoInEdit.SessionID = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).SessionID;
                    this.resumoInEdit.SessionIDOriginal = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).SessionIDOriginal;
                    this.resumoInEdit.Side = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).Side;
                    this.resumoInEdit.StopPx = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).StopPx;
                    this.resumoInEdit.StopStartID = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).StopStartID;
                    this.resumoInEdit.Symbol = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).Symbol;
                    this.resumoInEdit.SystemID = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).SystemID;
                    this.resumoInEdit.TimeInForce = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).TimeInForce;
                    this.resumoInEdit.TransactTime = ((Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo)this.lOriginalSource[e.RowIndex]).TransactTime;
                }

                resumoTmp = this.resumoInEdit;
                this.rowInEdit = e.RowIndex;
            }
            else
            {
                resumoTmp = this.resumoInEdit;
            }

            // Set the appropriate property to the cell value entered.
            String newValue = e.Value as String;
            switch (this.gridResumo.Columns[e.ColumnIndex].Name)
            {
                case "columnAccount":
                    e.Value = resumoTmp.Account;
                    break;

                case "columnChannelID":
                    e.Value = resumoTmp.ChannelID;
                    break;

                case "columnClOrdID":
                    e.Value = resumoTmp.ClOrdID;
                    break;

                case "columnCumQty":
                    e.Value = resumoTmp.CumQty;
                    break;

                case "columnDescription":
                    e.Value = resumoTmp.Description;
                    break;

                case "columnExchange":
                    e.Value = resumoTmp.Exchange;
                    break;

                case "columnExchangeNumberId":
                    e.Value = resumoTmp.ExchangeNumberID;
                    break;

                case "columnExecBroker":
                    e.Value = resumoTmp.ExecBroker;
                    break;

                case "columnExpireDate":
                    e.Value = resumoTmp.ExpireDate;
                    break;

                case "columnFixMsgSeqNum":
                    e.Value = resumoTmp.FixMsgSeqNum;
                    break;

                case "columnHandlInst":
                    e.Value = resumoTmp.HandlInst;
                    break;

                case "columnIdFix":
                    e.Value = resumoTmp.IdFix;
                    break;

                case "columnIntegrationName":
                    e.Value = resumoTmp.IntegrationName;
                    break;

                case "columnMemo":
                    e.Value = resumoTmp.Memo;
                    break;

                case "columnMsg42Base64":
                    e.Value = resumoTmp.Msg42Base64;
                    break;

                case "columnMsgFix":
                    e.Value = resumoTmp.MsgFix;
                    break;

                case "columnOrderID":
                    e.Value = resumoTmp.OrderID;
                    break;

                case "columnOrderQty":
                    e.Value = resumoTmp.OrderQty;
                    break;

                case "columnOrderQtyApar":
                    e.Value = resumoTmp.OrderQtyApar;
                    break;

                case "columnOrderQtyMin":
                    e.Value = resumoTmp.OrderQtyMin;
                    break;

                case "columnOrderQtyRemaining":
                    e.Value = resumoTmp.OrderQtyRemaining;
                    break;

                case "columnOrdStatus":
                    e.Value = resumoTmp.OrdStatus;
                    break;

                case "columnOrdTypeID":
                    e.Value = resumoTmp.OrdTypeID;
                    break;

                case "columnOrigClOrdID":
                    e.Value = resumoTmp.OrigClOrdID;
                    break;

                case "columnPrice":
                    e.Value = resumoTmp.Price;
                    break;

                case "columnRegisterTime":
                    e.Value = resumoTmp.RegisterTime;
                    break;

                case "columnSecurityExchangeID":
                    e.Value = resumoTmp.SecurityExchangeID;
                    break;

                case "columnSessionID":
                    e.Value = resumoTmp.SessionID;
                    break;

                case "columnSessionIDOriginal":
                    e.Value = resumoTmp.SessionIDOriginal;
                    break;

                case "columnStopPx":
                    e.Value = resumoTmp.StopPx;
                    break;

                case "columnStopStartID":
                    e.Value = resumoTmp.StopStartID;
                    break;

                case "columnSymbol":
                    e.Value = resumoTmp.Symbol;
                    break;

                case "columnSystemID":
                    e.Value = resumoTmp.SystemID;
                    break;

                case "columnTimeInForce":
                    e.Value = resumoTmp.TimeInForce;
                    break;

                case "columnTransactTime":
                    e.Value = resumoTmp.TransactTime;
                    break;

            }
        }

        private void gridResumo_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // Create a new object when the user edits 
            // the row for new records. 
            this.resumoInEdit = new Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo();
            this.rowInEdit = this.gridResumo.Rows.Count - 1;
        }

        private void gridResumo_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            // Save row changes if any were made and release the edited  object if there is one. 
            if (e.RowIndex >= this.lOriginalSource.Count && e.RowIndex != this.gridResumo.Rows.Count - 1)
            {
                // Add the new object to the data store. 
                this.lOriginalSource.Add(this.resumoInEdit);
                this.resumoInEdit = null;
                this.rowInEdit = -1;
            }
            else if (this.resumoInEdit != null && e.RowIndex < this.lOriginalSource.Count)
            {
                // Save the modified object in the data store. 
                this.lOriginalSource[e.RowIndex] = this.resumoInEdit;
                this.resumoInEdit = null;
                this.rowInEdit = -1;
            }
            else if (this.gridResumo.ContainsFocus)
            {
                this.resumoInEdit = null;
                this.rowInEdit = -1;
            }
        }

        private void gridResumo_RowDirtyStateNeeded(object sender, QuestionEventArgs e)
        {
            if (!rowScopeCommit)            {
                // In cell-level commit scope, indicate whether the value 
                // of the current cell has been modified.
                e.Response = this.gridResumo.IsCurrentCellDirty;
            }
        }

        private void gridResumo_CancelRowEdit(object sender, QuestionEventArgs e)
        {
            if (this.rowInEdit == this.gridResumo.Rows.Count - 2 && this.rowInEdit == this.lOriginalSource.Count)
            {
                // If the user has canceled the edit of a newly created row,  
                // replace the corresponding object with a new, empty one. 
                this.resumoInEdit = new Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo();
            }
            else
            {
                // If the user has canceled the edit of an existing row,  
                // release the corresponding object. 
                this.resumoInEdit = null;
                this.rowInEdit = -1;
            }
        }

        private void gridResumo_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Index < this.lOriginalSource.Count)
            {
                // If the user has deleted an existing row, remove the  
                // corresponding object from the data store. 
                this.lOriginalSource.RemoveAt(e.Row.Index);
            }

            if (e.Row.Index == this.rowInEdit)
            {
                // If the user has deleted a newly created row, release 
                // the corresponding object.  
                this.rowInEdit = -1;
                this.resumoInEdit = null;
            }
        }

        #endregion

        private void fOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {
                    Pesquisar();
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private bool ValidarPesquisa()
        {
            try
            {
                int lParametros = 0;

                //"Account":
                if (!String.IsNullOrEmpty(btxtSignAccount.Text))
                {
                    lParametros++;
                }

                //"Symbol":
                if (!String.IsNullOrEmpty(btxtSignSymbol.Text.ToUpper()))
                {
                    lParametros++;
                }

                //"Session":
                if (cmbSignSession.SelectedIndex > 0)
                {
                    lParametros++;
                }

                if (lParametros >= 1)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return false;
        }

        private bool ValidarFiltro()
        {
            try
            {
                int lParametros = 0;

                //"ClOrdId":
                if (!String.IsNullOrEmpty(txtNumeroOrdem.Text))
                {
                    lParametros++;
                }

                //"Symbol":
                if (!String.IsNullOrEmpty(txtSimbolo.Text.ToUpper()))
                {
                    lParametros++;
                }

                //"Account":
                if (!String.IsNullOrEmpty(txtConta.Text))
                {
                    lParametros++;
                }

                //"Side":
                if (cmbLado.SelectedIndex > 0)
                {
                    lParametros++;
                }

                //"OrdStatus":
                if (cmbEstado.SelectedIndex > 0)
                {
                    lParametros++;
                }

                //"OrderType:"
                if (cmbTipo.SelectedIndex > 0)
                {
                    lParametros++;
                }

                //"ExecBroker":
                if (cmbCorretora.SelectedIndex > 0)
                {
                    lParametros++;
                }

                //"TimeInForce":
                if (cmbValidade.SelectedIndex > 0)
                {
                    lParametros++;
                }

                //"SessionIDOriginal":
                if (cmbSessao.SelectedIndex > 0)
                {
                    lParametros++;
                }

                if (lParametros >= Constantes.ParametrosMinimo)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return false;
        }

        private void Pesquisar()
        {
            if (ValidarPesquisa())
            {
                lGuid = Guid.NewGuid();
                lOrdersLoaded = 0;

                Loading(true);

                this.lOriginalSource = new SortableBindingList<Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>();
                this.spiderOrderInfoBindingSource.DataSource = this.lOriginalSource;

                this.gridResumo.Rows.Clear();
                this.gridDetalhe.Rows.Clear();

                Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterStreamerRequest lRequisicao = new Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem.FilterStreamerRequest();
                int xx;

                if (int.TryParse(btxtSignAccount.Text, out xx))
                {
                    lRequisicao.Account = new Gradual.Spider.Acompanhamento4Socket.Lib.Dados.FilterIntVal(xx, Gradual.Spider.Acompanhamento4Socket.Lib.Dados.TypeCompare.EQUAL);
                }

                if (!string.IsNullOrEmpty(btxtSignSymbol.Text))
                {
                    lRequisicao.Symbol = new Gradual.Spider.Acompanhamento4Socket.Lib.Dados.FilterStringVal(btxtSignSymbol.Text.ToUpper(), Gradual.Spider.Acompanhamento4Socket.Lib.Dados.TypeCompare.EQUAL);
                }
                
                if (cmbSignSession.SelectedIndex > 0)
                {
                    lRequisicao.SessionID = new Gradual.Spider.Acompanhamento4Socket.Lib.Dados.FilterStringVal(((ComboboxItem)cmbSignSession.SelectedItem).Value.ToString(), Gradual.Spider.Acompanhamento4Socket.Lib.Dados.TypeCompare.EQUAL);
                }
                
                lRequisicao.Id = lGuid.ToString();

                if (lClient.IsConectado())
                {
                    lClient.SendObject(lRequisicao);
                }
            }
            else
            {
                MessageBox.Show(String.Format("É necessário selecionar ao menos {0} parametro{1}!", Constantes.ParametrosMinimo, Constantes.ParametrosMinimo > 1 ? "(s)" : ""));
            }
        }

        private void CriarColunas()
        {
            //this.columnCancelar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.columnCancelar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.columnClOrdID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnAccountDv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSymbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrdStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrdTypeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRegisterTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTransactTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTimeInForce = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnExpireDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnChannelID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnExchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSide = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrderQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrderQtyRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCumQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrderQtyMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrderQtyApar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnStopPx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnExecBroker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSessionIDOriginal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrigClOrdID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnExchangeNumberID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSecurityExchangeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnStopStartID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columSystemID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMemo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnFixMsgSeqNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSessionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnIdFix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMsgFix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMsg42Base64 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHandlInst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnIntegrationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnAveragePrice= new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnAveragePriceW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            
            this.dgvMoedaCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMoedaCellStyle.Format = "N2";

            // 
            // columnCancelar
            // 
            this.columnCancelar.HeaderText = "Cancelar?";
            this.columnCancelar.Text = "Cancelar";
            this.columnCancelar.UseColumnTextForButtonValue = true;
            this.columnCancelar.FlatStyle = FlatStyle.Flat;
            //this.columnCancelar.CellTemplate.Style.BackColor = Color.FromArgb(255, 195, 35);
            this.columnCancelar.CellTemplate.Style.BackColor = Color.FromArgb(115, 175, 215);
            this.columnCancelar.Name = "columnCancelar";
            this.columnCancelar.Width = 74;
            
            //this.columnCancelar.UseColumnTextForButtonValue = true;
            //this.columnCancelar.FlatStyle = FlatStyle.Popup;
            //this.columnCancelar.DefaultCellStyle.ForeColor = Color.Navy;
            //this.columnCancelar.DefaultCellStyle.BackColor = Color.Yellow;
            // 
            // columnClOrdID
            // 
            this.columnClOrdID.DataPropertyName = "ClOrdID";
            this.columnClOrdID.HeaderText = "Nr. Ordem";
            this.columnClOrdID.Name = "columnClOrdID";
            this.columnClOrdID.Width = 74;
            // 
            // columnAccount
            // 
            this.columnAccount.DataPropertyName = "Account";
            this.columnAccount.HeaderText = "Conta";
            this.columnAccount.Name = "columnAccount";
            this.columnAccount.Width = 60;
            // 
            // columnAccountDv
            // 
            this.columnAccountDv.DataPropertyName = "AccountDv";
            this.columnAccountDv.HeaderText = "Digito";
            this.columnAccountDv.Name = "columnAccountDv";
            this.columnAccountDv.Width = 60;
            // 
            // columnSymbol
            // 
            this.columnSymbol.DataPropertyName = "Symbol";
            this.columnSymbol.HeaderText = "Ativo";
            this.columnSymbol.Name = "columnSymbol";
            this.columnSymbol.Width = 56;
            // 
            // columnOrdStatus
            // 
            this.columnOrdStatus.DataPropertyName = "OrdStatus";
            this.columnOrdStatus.HeaderText = "Status";
            this.columnOrdStatus.Name = "columnOrdStatus";
            this.columnOrdStatus.Width = 62;
            // 
            // columnHora
            // 
            this.columnHora.HeaderText = "Hora";
            this.columnHora.Name = "columnHora";
            this.columnHora.Width = 55;
            // 
            // columnOrdTypeID
            // 
            this.columnOrdTypeID.DataPropertyName = "OrdTypeID";
            this.columnOrdTypeID.HeaderText = "Tp. Ordem";
            this.columnOrdTypeID.Name = "columnOrdTypeID";
            this.columnOrdTypeID.Width = 76;
            // 
            // columnRegisterTime
            // 
            this.columnRegisterTime.DataPropertyName = "RegisterTime";
            this.columnRegisterTime.HeaderText = "Dt. Envio";
            this.columnRegisterTime.Name = "columnRegisterTime";
            this.columnRegisterTime.Width = 70;
            // 
            // columnTransactTime
            // 
            this.columnTransactTime.DataPropertyName = "TransactTime";
            this.columnTransactTime.HeaderText = "Dt. Atualização";
            this.columnTransactTime.Name = "columnTransactTime";
            this.columnTransactTime.Width = 96;
            // 
            // columnTimeInForce
            // 
            this.columnTimeInForce.DataPropertyName = "TimeInForce";
            this.columnTimeInForce.HeaderText = "Validade";
            this.columnTimeInForce.Name = "columnTimeInForce";
            this.columnTimeInForce.Width = 73;
            // 
            // columnExpireDate
            // 
            this.columnExpireDate.DataPropertyName = "ExpireDate";
            this.columnExpireDate.HeaderText = "Vencimento";
            this.columnExpireDate.Name = "columnExpireDate";
            this.columnExpireDate.Width = 88;
            // 
            // columnChannelID
            // 
            this.columnChannelID.DataPropertyName = "ChannelID";
            this.columnChannelID.HeaderText = "Porta";
            this.columnChannelID.Name = "columnChannelID";
            this.columnChannelID.Width = 57;
            // 
            // columnExchange
            // 
            this.columnExchange.DataPropertyName = "Exchange";
            this.columnExchange.HeaderText = "Bolsa";
            this.columnExchange.Name = "columnExchange";
            this.columnExchange.Width = 58;
            // 
            // columnSide
            // 
            this.columnSide.DataPropertyName = "Side";
            this.columnSide.HeaderText = "C/V";
            this.columnSide.Name = "columnSide";
            this.columnSide.Width = 51;
            // 
            // columnOrderQty
            // 
            this.columnOrderQty.DataPropertyName = "OrderQty";
            this.columnOrderQty.HeaderText = "Qtde.  Solic.";
            this.columnOrderQty.Name = "columnOrderQty";
            this.columnOrderQty.Width = 58;
            // 
            // columnOrderQtyRemaining
            // 
            this.columnOrderQtyRemaining.DataPropertyName = "OrderQtyRemaining";
            this.columnOrderQtyRemaining.HeaderText = "Saldo";
            this.columnOrderQtyRemaining.Name = "columnOrderQtyRemaining";
            this.columnOrderQtyRemaining.Width = 59;
            // 
            // columnCumQty
            // 
            this.columnCumQty.DataPropertyName = "CumQty";
            this.columnCumQty.HeaderText = "Qtde. Exec.";
            this.columnCumQty.Name = "columnCumQty";
            this.columnCumQty.Width = 81;
            // 
            // columnOrderQtyMin
            // 
            this.columnOrderQtyMin.DataPropertyName = "OrderQtyMin";
            this.columnOrderQtyMin.HeaderText = "Qtde. Mín.";
            this.columnOrderQtyMin.Name = "columnOrderQtyMin";
            this.columnOrderQtyMin.Width = 77;
            // 
            // columnOrderQtyApar
            // 
            this.columnOrderQtyApar.DataPropertyName = "OrderQtyApar";
            this.columnOrderQtyApar.HeaderText = "Qtde. Apar.";
            this.columnOrderQtyApar.Name = "columnOrderQtyApar";
            this.columnOrderQtyApar.Width = 79;
            // 
            // columnPrice
            // 
            this.columnPrice.DataPropertyName = "Price";
            this.columnPrice.HeaderText = "Preço";
            this.columnPrice.Name = "columnPrice";
            this.columnPrice.DefaultCellStyle = this.dgvMoedaCellStyle;
            this.columnPrice.Width = 60;
            // 
            // columnStopPx
            // 
            this.columnStopPx.DataPropertyName = "StopPx";
            this.columnStopPx.HeaderText = "Pr. Stop/Start";
            this.columnStopPx.Name = "columnStopPx";
            this.columnStopPx.DefaultCellStyle = this.dgvMoedaCellStyle;
            this.columnStopPx.Width = 89;
            // 
            // columnAveragePrice
            // 
            this.columnAveragePrice.DataPropertyName = "AvgPx";
            this.columnAveragePrice.HeaderText = "Pr. Médio";
            this.columnAveragePrice.Name = "columnAveragePrice";
            this.columnAveragePrice.Visible = false;
            this.columnAveragePrice.DefaultCellStyle = this.dgvMoedaCellStyle;
            this.columnAveragePrice.Width = 110;
            // 
            // columnAveragePriceW
            // 
            this.columnAveragePriceW.DataPropertyName = "AvgPxW";
            this.columnAveragePriceW.HeaderText = "Pr. Médio";
            this.columnAveragePriceW.Name = "columnAveragePriceW";
            this.columnAveragePriceW.Visible = true;
            this.columnAveragePriceW.DefaultCellStyle = this.dgvMoedaCellStyle;
            this.columnAveragePriceW.Width = 110;
            


            
            // 
            // columnExecBroker
            // 
            this.columnExecBroker.DataPropertyName = "ExecBroker";
            this.columnExecBroker.HeaderText = "ExecBroker";
            this.columnExecBroker.Name = "columnExecBroker";
            this.columnExecBroker.Width = 87;
            // 
            // columnSessionIDOriginal
            // 
            this.columnSessionIDOriginal.DataPropertyName = "SessionIDOriginal";
            this.columnSessionIDOriginal.HeaderText = "Sessão";
            this.columnSessionIDOriginal.Name = "columnSessionIDOriginal";
            this.columnSessionIDOriginal.Width = 67;
            // 
            // colOrderId
            // 
            this.columnOrderID.DataPropertyName = "OrderID";
            this.columnOrderID.HeaderText = "OrderID";
            this.columnOrderID.Name = "colOrderId";
            this.columnOrderID.Visible = false;
            this.columnOrderID.Width = 69;
            // 
            // columnOrigClOrdID
            // 
            this.columnOrigClOrdID.DataPropertyName = "OrigClOrdID";
            this.columnOrigClOrdID.HeaderText = "OrigClOrdID";
            this.columnOrigClOrdID.Name = "columnOrigClOrdID";
            this.columnOrigClOrdID.Visible = false;
            this.columnOrigClOrdID.Width = 88;
            // 
            // columnExchangeNumberID
            // 
            this.columnExchangeNumberID.DataPropertyName = "ExchangeNumberID";
            this.columnExchangeNumberID.HeaderText = "ExchangeNumberID";
            this.columnExchangeNumberID.Name = "columnExchangeNumberID";
            this.columnExchangeNumberID.Width = 128;
            // 
            // columnSecurityExchangeID
            // 
            this.columnSecurityExchangeID.DataPropertyName = "SecurityExchangeID";
            this.columnSecurityExchangeID.HeaderText = "SecurityExchangeID";
            this.columnSecurityExchangeID.Name = "columnSecurityExchangeID";
            this.columnSecurityExchangeID.Visible = false;
            this.columnSecurityExchangeID.Width = 129;
            // 
            // columnStopStartID
            // 
            this.columnStopStartID.DataPropertyName = "StopStartID";
            this.columnStopStartID.HeaderText = "StopStartID";
            this.columnStopStartID.Name = "columnStopStartID";
            this.columnStopStartID.Visible = false;
            this.columnStopStartID.Width = 87;
            // 
            // columnDescription
            // 
            this.columnDescription.DataPropertyName = "Description";
            this.columnDescription.HeaderText = "Description";
            this.columnDescription.Name = "columnDescription";
            this.columnDescription.Visible = false;
            this.columnDescription.Width = 85;
            // 
            // columSystemID
            // 
            this.columSystemID.DataPropertyName = "SystemID";
            this.columSystemID.HeaderText = "SystemID";
            this.columSystemID.Name = "columSystemID";
            this.columSystemID.Visible = false;
            this.columSystemID.Width = 77;
            // 
            // columnMemo
            // 
            this.columnMemo.DataPropertyName = "Memo";
            this.columnMemo.HeaderText = "Memo";
            this.columnMemo.Name = "columnMemo";
            this.columnMemo.Visible = false;
            this.columnMemo.Width = 61;
            // 
            // columnFixMsgSeqNum
            // 
            this.columnFixMsgSeqNum.DataPropertyName = "FixMsgSeqNum";
            this.columnFixMsgSeqNum.HeaderText = "FixMsgSeqNum";
            this.columnFixMsgSeqNum.Name = "columnFixMsgSeqNum";
            this.columnFixMsgSeqNum.Visible = false;
            this.columnFixMsgSeqNum.Width = 106;
            // 
            // columnSessionID
            // 
            this.columnSessionID.DataPropertyName = "SessionID";
            this.columnSessionID.HeaderText = "SessionID";
            this.columnSessionID.Name = "columnSessionID";
            this.columnSessionID.Visible = false;
            this.columnSessionID.Width = 80;
            // 
            // columnIdFix
            // 
            this.columnIdFix.DataPropertyName = "IdFix";
            this.columnIdFix.HeaderText = "IdFix";
            this.columnIdFix.Name = "columnIdFix";
            this.columnIdFix.Visible = false;
            this.columnIdFix.Width = 54;
            // 
            // columnMsgFix
            // 
            this.columnMsgFix.DataPropertyName = "MsgFix";
            this.columnMsgFix.HeaderText = "MsgFix";
            this.columnMsgFix.Name = "columnMsgFix";
            this.columnMsgFix.Visible = false;
            this.columnMsgFix.Width = 65;
            // 
            // columnMsg42Base64
            // 
            this.columnMsg42Base64.DataPropertyName = "Msg42Base64";
            this.columnMsg42Base64.HeaderText = "Msg42Base64";
            this.columnMsg42Base64.Name = "columnMsg42Base64";
            this.columnMsg42Base64.Visible = false;
            // 
            // columnHandlInst
            // 
            this.columnHandlInst.DataPropertyName = "HandlInst";
            this.columnHandlInst.HeaderText = "HandlInst";
            this.columnHandlInst.Name = "columnHandlInst";
            this.columnHandlInst.Visible = false;
            this.columnHandlInst.Width = 77;
            // 
            // columnIntegrationName
            // 
            this.columnIntegrationName.DataPropertyName = "IntegrationName";
            this.columnIntegrationName.HeaderText = "IntegrationName";
            this.columnIntegrationName.Name = "columnIntegrationName";
            this.columnIntegrationName.Visible = false;
            this.columnIntegrationName.Width = 110;
            // 
            // columnDetailOrderId
            // 
            this.columnDetailOrderId.DataPropertyName = "OrderID";
            this.columnDetailOrderId.HeaderText = "Nr.Ordem";
            this.columnDetailOrderId.Name = "columnDetailOrderId";
            // 
            // columnDetailOrderDetailID
            // 
            this.columnDetailOrderDetailID.DataPropertyName = "OrderDetailID";
            this.columnDetailOrderDetailID.HeaderText = "OrderDetailID";
            this.columnDetailOrderDetailID.Name = "columnDetailOrderDetailID";
            // 
            // columnDetailOrderQty
            // 
            this.columnDetailOrderQty.DataPropertyName = "OrderQty";
            this.columnDetailOrderQty.HeaderText = "Qtde. Solic.";
            this.columnDetailOrderQty.Name = "columnDetailOrderQty";
            // 
            // columnDetailCumQty
            // 
            this.columnDetailCumQty.DataPropertyName = "CumQty";
            this.columnDetailCumQty.HeaderText = "Qtde. Exec.";
            this.columnDetailCumQty.Name = "columnDetailCumQty";
            // 
            // columnDetailOrderQtyRemaining
            // 
            this.columnDetailOrderQtyRemaining.DataPropertyName = "OrderQtyRemaining";
            this.columnDetailOrderQtyRemaining.HeaderText = "Saldo";
            this.columnDetailOrderQtyRemaining.Name = "columnDetailOrderQtyRemaining";
            // 
            // columnDetailOrderStatusId
            // 
            this.columnDetailOrderStatusId.DataPropertyName = "OrderStatusID";
            this.columnDetailOrderStatusId.HeaderText = "Status";
            this.columnDetailOrderStatusId.Name = "columnDetailOrderStatusId";
            // 
            // columnDetailTransactTime
            // 
            this.columnDetailTransactTime.DataPropertyName = "TransactTime";
            this.columnDetailTransactTime.HeaderText = "Dt. Atualização";
            this.columnDetailTransactTime.Name = "columnDetailTransactTime";
            // 
            // columnDetailPrice
            // 
            this.columnDetailPrice.DataPropertyName = "Price";
            this.columnDetailPrice.HeaderText = "Preço";
            this.columnDetailPrice.Name = "columnDetailPrice";
            this.columnDetailPrice.DefaultCellStyle = this.dgvMoedaCellStyle;
            // 
            // columnDetailDescription
            // 
            this.columnDetailDescription.DataPropertyName = "Description";
            this.columnDetailDescription.HeaderText = "Observação";
            this.columnDetailDescription.Name = "columnDetailDescription";
            this.columnDetailDescription.Width = 150;
            // 
            // columnDetailStopPx
            // 
            this.columnDetailStopPx.DataPropertyName = "StopPx";
            this.columnDetailStopPx.HeaderText = "Preço Stop";
            this.columnDetailStopPx.Name = "columnDetailStopPx";
            this.columnDetailStopPx.DefaultCellStyle = this.dgvMoedaCellStyle;
            
            // columnDetailTransactID
            // 
            this.columnDetailTransactID.DataPropertyName = "TransactID";
            this.columnDetailTransactID.HeaderText = "TransactID";
            this.columnDetailTransactID.Name = "columnDetailTransactID";
            this.columnDetailTransactID.Visible = false;
            // 
            // columnDetailTradeQty
            // 
            this.columnDetailTradeQty.DataPropertyName = "TradeQty";
            this.columnDetailTradeQty.HeaderText = "TradeQty";
            this.columnDetailTradeQty.Name = "columnDetailTradeQty";
            this.columnDetailTradeQty.Visible = false;
            // 
            // columnDetailFixMsgSeqNum
            // 
            this.columnDetailFixMsgSeqNum.DataPropertyName = "FixMsgSeqNum";
            this.columnDetailFixMsgSeqNum.HeaderText = "FixMsgSeqNum";
            this.columnDetailFixMsgSeqNum.Name = "columnDetailFixMsgSeqNum";
            this.columnDetailFixMsgSeqNum.Visible = false;
            // 
            // columnDetailCxlRejResponseTo
            // 
            this.columnDetailCxlRejResponseTo.DataPropertyName = "CxlRejResponseTo";
            this.columnDetailCxlRejResponseTo.HeaderText = "CxlRejResponseTo";
            this.columnDetailCxlRejResponseTo.Name = "columnDetailCxlRejResponseTo";
            this.columnDetailCxlRejResponseTo.Visible = false;
            // 
            // columnDetailCxlRejReason
            // 
            this.columnDetailCxlRejReason.DataPropertyName = "CxlRejReason";
            this.columnDetailCxlRejReason.HeaderText = "CxlRejReason";
            this.columnDetailCxlRejReason.Name = "columnDetailCxlRejReason";
            this.columnDetailCxlRejReason.Visible = false;
            // 
            // columnDetailMsgFixDetail
            // 
            this.columnDetailMsgFixDetail.DataPropertyName = "MsgFixDetail";
            this.columnDetailMsgFixDetail.HeaderText = "MsgFixDetail";
            this.columnDetailMsgFixDetail.Name = "columnDetailMsgFixDetail";
            this.columnDetailMsgFixDetail.Visible = false;
            // 
            // columnDetailContraBroker
            // 
            this.columnDetailContraBroker.DataPropertyName = "ContraBroker";
            this.columnDetailContraBroker.HeaderText = "ContraBroker";
            this.columnDetailContraBroker.Name = "columnDetailContraBroker";
            this.columnDetailContraBroker.Visible = false;


            

            this.columnAccountDv.Visible = false;
            this.columnOrderID.Visible = false;
            this.columnOrdTypeID.Visible = false;
            this.columnTimeInForce.Visible = false;
            this.columnExpireDate.Visible = false;
            this.columnChannelID.Visible = false;
            this.columnExchange.Visible = false;
            this.columnOrigClOrdID.Visible = false;
            this.columnExchangeNumberID.Visible = false;
            this.columnSecurityExchangeID.Visible = false;
            this.columnStopStartID.Visible = false;
            this.columnDescription.Visible = false;
            this.columSystemID.Visible = false;
            this.columnMemo.Visible = false;
            this.columnFixMsgSeqNum.Visible = false;
            this.columnSessionID.Visible = false;
            this.columnIdFix.Visible = false;
            this.columnMsgFix.Visible = false;
            this.columnMsg42Base64.Visible = false;
            this.columnHandlInst.Visible = false;
            this.columnIntegrationName.Visible = false;
            this.columnHora.Visible = false;
            this.columnAveragePrice.Visible = false;

            this.gridResumo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnCancelar,
            this.columnClOrdID,
            this.columnRegisterTime,
            this.columnHora,
            this.columnTransactTime,
            this.columnAccount,
            this.columnAccountDv,
            this.columnSide,
            this.columnSymbol,
            this.columnPrice,
            this.columnAveragePrice,
            this.columnAveragePriceW,
            this.columnOrderQty,
            this.columnCumQty,
            this.columnOrderQtyRemaining,
            this.columnOrderQtyMin,
            this.columnOrderQtyApar,
            this.columnStopPx,
            this.columnExecBroker,
            this.columnSessionIDOriginal,
            this.columnOrdStatus,
            this.columnOrdTypeID,
            this.columnTimeInForce,
            this.columnExpireDate,
            this.columnChannelID,
            this.columnExchange,
            this.columnOrderID,
            this.columnOrigClOrdID,
            this.columnExchangeNumberID,
            this.columnSecurityExchangeID,
            this.columnStopStartID,
            this.columnDescription,
            this.columSystemID,
            this.columnMemo,
            this.columnFixMsgSeqNum,
            this.columnSessionID,
            this.columnIdFix,
            this.columnMsgFix,
            this.columnMsg42Base64,
            this.columnHandlInst,
            this.columnIntegrationName
            });

            this.columnAccountDv.Visible = false;
            this.columnDetailOrderId.Visible = false;
            this.columnDetailOrderDetailID.Visible = false;


            this.gridDetalhe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnDetailOrderId,
            this.columnDetailOrderDetailID,
            this.columnDetailOrderQty,
            this.columnDetailCumQty,
            this.columnDetailOrderQtyRemaining,
            this.columnDetailOrderStatusId,
            this.columnDetailTransactTime,
            this.columnDetailPrice,
            this.columnDetailDescription,
            this.columnDetailStopPx,
            this.columnDetailTransactID,
            this.columnDetailTradeQty,
            this.columnDetailFixMsgSeqNum,
            this.columnDetailCxlRejResponseTo,
            this.columnDetailCxlRejReason,
            this.columnDetailMsgFixDetail,
            this.columnDetailContraBroker});

            this.gridResumo.DataSource = spiderOrderInfoBindingSource;
            this.gridDetalhe.DataSource = spiderOrderDetailInfoBindingSource;
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            lFiltered = false;
            this.spiderOrderInfoBindingSource.DataSource = this.lOriginalSource;
            this.lFilteredSource = new SortableBindingList<Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>();

            //"ClOrdId":
            txtNumeroOrdem.Text = String.Empty;
            //"Symbol":
            txtSimbolo.Text = String.Empty;
            //"Account":
            txtConta.Text = String.Empty;
            //"Side":
            cmbLado.SelectedIndex = 0;
            //"OrdStatus":
            cmbEstado.SelectedIndex = 0;
            //"OrderType:"
            cmbTipo.SelectedIndex = 0;
            //"ExecBroker":
            cmbCorretora.SelectedIndex = 0;
            //"TimeInForce":
            cmbValidade.SelectedIndex = 0;
            //"SessionIDOriginal":
            cmbSessao.SelectedIndex = 0;

            txtConta.textBox.Clear();
            txtConta.textBox.Focus();
            txtConta.textBox.ScrollToCaret();
        }

        private void Loading(Boolean pVisible)
        {
            Loading(pVisible, false);
        }

        private void Loading(Boolean pVisible, Boolean pJustWaiting)
        {

            if (!pJustWaiting)
            {
                tmrLoading.Enabled = pVisible;
            }
         
            this.CanMaximize = !pVisible;
            this.CanMinimize = !pVisible;
            this.TamanhoFixo = pVisible;

            loadingPanel1.Progress = null;
            loadingPanel1.CaptureBackgroundForm();
            loadingPanel1.Visible = pVisible;

            if (pVisible.Equals(false))
            {
                tmrLoading.Stop();
                this.StatusText(null);
            }
            else
            {
                this.StatusText(String.Format("Ordens: {0}", lOrdersLoaded.ToString()));
            }
        }

        
        private void tmrLoading_Tick(object sender, EventArgs e)
        {
            if (lLastMessage != Convert.ToDateTime("01/01/0001 00:00:00"))
            {
                if (((new TimeSpan(DateTime.Now.Ticks - lLastMessage.Ticks)).TotalMilliseconds > 1000) || lInteractions > 10)
                {
                    System.Threading.Thread.Sleep(100);
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: Suprimindo o carregamento por tempo de inatividade.", Gradual.Utils.MethodHelper.GetCurrentMethod()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    Loading(false);
                }
            }
            else
            {
                if (lInteractions > 10)
                {
                    System.Threading.Thread.Sleep(100);
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: Suprimindo o carregamento por interações.", Gradual.Utils.MethodHelper.GetCurrentMethod()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    Loading(false);
                }

                lInteractions++;
            }

            this.StatusText(String.Format("Ordens: {0}", lOrdersLoaded.ToString()));
        }

        private void gridResumo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Delete))
            {
                CancelarOrdem();
            }
        }

        private void gridResumo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                CancelarOrdem();
            }
        }



        private void fOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Desconectar();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            Filtrar();
        }

        // this message handler gets called when the user checks/unchecks an item the combo box
        private void cComboBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (sender is CheckComboBox.CheckComboBoxItem)
            {
                CheckComboBox.CheckComboBoxItem item = (CheckComboBox.CheckComboBoxItem)sender;

                if (item.CheckState.Equals(true))
                {
                    if (!lStatus.ContainsKey(item.Value.ToString()))
                    {
                        lStatus.Add(item.Value.ToString(), item.Text);
                    }
                }
                else
                {
                    if (lStatus.ContainsKey(item.Value.ToString()))
                    {
                        lStatus.Remove(item.Value.ToString());
                    }
                }
            }
        }

        private void checkedListStatus_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (!lStatus.ContainsKey(((CheckedListItem)checkedListStatus.Items[e.Index]).Value.ToString()))
                {
                    lStatus.Add(((CheckedListItem)checkedListStatus.Items[e.Index]).Value.ToString(), ((CheckedListItem)checkedListStatus.Items[e.Index]).ToString());
                }
            }
            else
            {
                if (lStatus.ContainsKey(((CheckedListItem)checkedListStatus.Items[e.Index]).Value.ToString()))
                {
                    lStatus.Remove(((CheckedListItem)checkedListStatus.Items[e.Index]).Value.ToString());
                }
            }
        }

        private void tsmConfiguracoes_Click(object sender, EventArgs e)
        {
            try
            {
                fCores lfCores = new fCores();
                lfCores.Show();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void tsmSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CancelarOrdem();
        }

        private void btnCancelarTudo_Click(object sender, EventArgs e)
        {
            CancelarTudo();
        }

        private void cancelarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelarOrdem();
        }

        private void cancelarTudoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelarTudo();
        }

        private void gridResumo_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex.Equals(0))
                {
                    //this.gridResumo.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.FromArgb(255, 195, 35);
                    this.gridResumo.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.FromArgb(115, 175, 215);
                }
            }
        }
    }

    public class CheckedListItem
    {
        public string Description { get; set; }
        public object Value { get; set;  }

        public CheckedListItem(String pDescription, Object pValue)
        {
            this.Description = pDescription;
            this.Value = pValue;
        }

        public override string ToString()
        {
            return this.Description;
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public ComboboxItem()
        {
        }

        public ComboboxItem(Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro pParametro)
        {
            Text = pParametro.Description;
            Value = pParametro.Value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}

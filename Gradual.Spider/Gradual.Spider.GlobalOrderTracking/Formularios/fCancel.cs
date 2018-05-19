using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.Spider.GlobalOrderTracking.Formularios
{
    public partial class fCancel : FormularioBase
    {
        private SortableBindingList<Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo> lSource = new SortableBindingList<Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo>();

        public fCancel()
        {
            InitializeComponent();
            GradualForm.Engine.ConfigureFormRender(this);
            this.spiderOrderCancelBindingSource.DataSource = lSource;
        }

        public void AddOrder(Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo pOrder)
        {
            lSource.Add(pOrder);
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Gradual.OMS.RoteadorOrdens.Lib.IRoteadorOrdens lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.RoteadorOrdens.Lib.IRoteadorOrdens>();

            //    Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemRequest lRequest = new Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemRequest();
            //    Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemResponse lResponse;
            //    lRequest.info = new OMS.RoteadorOrdens.Lib.Dados.OrdemCancelamentoInfo();

            //    foreach (Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo lOrder in lSource)
            //    {
            //        lRequest.info.Account = lOrder.Account;
            //        lRequest.info.ChannelID = Constantes.ChannelIDCancelamento; //TODO: colocar configuravel
            //        lRequest.info.ClOrdID = Aplicacao.GerarClOrdId(lOrder.Account.ToString(), lOrder.ClOrdID.ToString());
            //        lRequest.info.OrigClOrdID = lOrder.ClOrdID;
            //        lRequest.info.OrderID = lOrder.ExchangeNumberID;
            //        lRequest.info.Symbol = lOrder.Symbol;
            //        lRequest.info.SecurityID = lOrder.Symbol;
            //        lRequest.info.CompIDBolsa = lOrder.SessionIDOriginal;
            //        lRequest.info.Exchange = lOrder.Exchange.ToString().ToUpper();
            //        lRequest.info.Side = lOrder.Side.ToString().Equals("1") ? OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra : OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda;
            //        lRequest.info.OrderQty = lOrder.OrderQty;
            //        lRequest.info.ExecBroker = lOrder.ExecBroker;

            //        lResponse = lServico.CancelarOrdem(lRequest);

            //        if (lResponse.DadosRetorno.StatusResposta == Gradual.OMS.RoteadorOrdens.Lib.Dados.StatusRoteamentoEnum.Sucesso)
            //        {
            //            Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: A ordem {1} foi cancelada (Resp: {2}).", Gradual.Utils.MethodHelper.GetCurrentMethod(), lOrder.ClOrdID.ToString(), lResponse.DadosRetorno.StackTrace), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            //        }
            //        else
            //        {
            //            Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: A ordem {1} não foi cancelada (Resp: {2}).", Gradual.Utils.MethodHelper.GetCurrentMethod(), lOrder.ClOrdID.ToString(), lResponse.DadosRetorno.StackTrace), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            //        }
            //    }

            //    this.Close();
            //}
            //catch (Exception ex)
            //{
            //    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            //}

            try
            {
                //Gradual.OMS.RoteadorOrdens.Lib.IRoteadorOrdens lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.RoteadorOrdens.Lib.IRoteadorOrdens>();
                Gradual.Spider.SupervisorFix.Lib.ISupervisorFix lServicoFix = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.Spider.SupervisorFix.Lib.ISupervisorFix>();
                Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemRequest lRequest = new Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemRequest();
                //Gradual.OMS.RoteadorOrdens.Lib.Mensagens.ExecutarCancelamentoOrdemResponse lResponse;

                lRequest.info = new OMS.RoteadorOrdens.Lib.Dados.OrdemCancelamentoInfo();
                System.Net.IPAddress[] localIPs = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
                string ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToString();
                string id = ContextoGlobal.CodigoUsuario;
                string nome = ContextoGlobal.Usuario.NomeUsuario;
                string email = ContextoGlobal.Usuario.Email;
                string codassessor = ContextoGlobal.Usuario.CodAssessor.ToString();

                foreach (Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo lOrder in lSource)
                {
                    Gradual.Spider.SupervisorFix.Lib.Dados.OrderCancelationRequest lReqCancel = new Gradual.Spider.SupervisorFix.Lib.Dados.OrderCancelationRequest();
                    lReqCancel.User = id; // necessita do usuario
                    lReqCancel.Ip = ip;
                    lReqCancel.NomeUsuario = nome;
                    lReqCancel.Email = email;
                    lReqCancel.CodAssessor = codassessor;
                    lRequest.info.Account = lOrder.AccountDv;
                    lRequest.info.ChannelID = Constantes.ChannelIDCancelamento; //TODO: colocar configuravel
                    lRequest.info.ClOrdID = Aplicacao.GerarClOrdId(lOrder.Account.ToString(), lOrder.ClOrdID.ToString());
                    
                    lRequest.info.OrigClOrdID = lOrder.ClOrdID;
                    lRequest.info.OrderID = lOrder.ExchangeNumberID;
                    lRequest.info.Symbol = lOrder.Symbol;
                    lRequest.info.SecurityID = lOrder.Symbol;
                    lRequest.info.CompIDBolsa = lOrder.SessionIDOriginal;
                    lRequest.info.Exchange = lOrder.Exchange.ToString().ToUpper();
                    lRequest.info.Side = lOrder.Side.ToString().Equals("1") ? OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra : OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda;
                    lRequest.info.OrderQty = lOrder.OrderQty;
                    lRequest.info.ExecBroker = lOrder.ExecBroker; // Mudar para o codigo assessor????
                    lReqCancel.CancelReq = lRequest;

                    Gradual.Spider.SupervisorFix.Lib.Dados.OrderCancelationResponse lResponse = lServicoFix.CancelOrder(lReqCancel);

                    //if (lResponse.DadosRetorno.StatusResposta == Gradual.OMS.RoteadorOrdens.Lib.Dados.StatusRoteamentoEnum.Sucesso)
                    if (lResponse.ErrCode.Equals(0))
                    {
                        Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: A ordem {1} foi cancelada (Resp: {2}).", Gradual.Utils.MethodHelper.GetCurrentMethod(), lOrder.ClOrdID.ToString(),lResponse.ErrDesc), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    }
                    else
                    {
                        Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: A ordem {1} não foi cancelada (Resp: {2}).", Gradual.Utils.MethodHelper.GetCurrentMethod(), lOrder.ClOrdID.ToString(), lResponse.ErrDesc), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridCancel_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (e.Value == null)
                    {
                        return;
                    }

                    if (gridCancel.Columns[e.ColumnIndex].Name.Equals("columnSide"))
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

                    if (gridCancel.Columns[e.ColumnIndex].Name.Equals("columnOrdStatus"))
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

                    if (gridCancel.Columns[e.ColumnIndex].Name.Equals("columnTimeInForce"))
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

                    if (gridCancel.Columns[e.ColumnIndex].Name.Equals("columnOrdTypeID"))
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

                    if (gridCancel.Columns[e.ColumnIndex].Name.Equals("columnRegisterTime"))
                    {
                        string lDataHora = e.Value.ToString();
                        gridCancel.Rows[e.RowIndex].Cells["columnHora"].Value = lDataHora.ToString().Substring(11, 8);
                    }

                    if (gridCancel.Columns[e.ColumnIndex].Name.Equals("Price") || gridCancel.Columns[e.ColumnIndex].Name.Equals("StopPx"))
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
    }
}

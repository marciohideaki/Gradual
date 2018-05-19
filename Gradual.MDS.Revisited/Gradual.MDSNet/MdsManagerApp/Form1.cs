using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Library.Servicos;
using Gradual.MDS.Adm.Lib;
using System.Configuration;

namespace MdsManagerApp
{
    public partial class frmMdsManagerApp : Form
    {
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string tcpConflatedSenderCompID;

        string [] products = { "0", "2", "3", "4", "5", "6", "7", "15", "16" };

        

        private struct FIXSubscription
        {
            public string Product { get; set; }
            public string CFICode { get; set; }
            public string SecurityTye { get; set; }
            public string Instrumento { get; set; }
            public string ReqID { get; set; }
        }

        private Dictionary<string, FIXSubscription> dctRequests = new Dictionary<string, FIXSubscription>();

        public frmMdsManagerApp()
        {
            InitializeComponent();


            if (ConfigurationManager.AppSettings["ChannelList"] != null)
            {
                string[] list = ConfigurationManager.AppSettings["ChannelList"].ToString().Split(",;".ToCharArray());

                foreach (string channelID in list)
                {
                    cmbChannelID.Items.Add(channelID);
                }
            }
            else
            {

                cmbChannelID.Items.Add("001");
                cmbChannelID.Items.Add("002");
                cmbChannelID.Items.Add("003");
                cmbChannelID.Items.Add("004");
                cmbChannelID.Items.Add("005");
                cmbChannelID.Items.Add("006");
                cmbChannelID.Items.Add("007");
                cmbChannelID.Items.Add("008");
                cmbChannelID.Items.Add("009");
                cmbChannelID.Items.Add("010");

                cmbChannelID.Items.Add("050");
                cmbChannelID.Items.Add("051");
                cmbChannelID.Items.Add("052");
                cmbChannelID.Items.Add("055");
                cmbChannelID.Items.Add("056");
                cmbChannelID.Items.Add("057");
                cmbChannelID.Items.Add("058");
            }
        }

        private void btAtivar_Click(object sender, EventArgs e)
        {
            Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

            if (serv != null)
            {
                serv.AtivarChannel(cmbChannelID.Items[cmbChannelID.SelectedIndex].ToString());
            }
        }

        private void btDesativar_Click(object sender, EventArgs e)
        {
            Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

            if (serv != null)
            {
                serv.DesativarChannel(cmbChannelID.Items[cmbChannelID.SelectedIndex].ToString());
            }
        }

        private void btPausarCanal_Click(object sender, EventArgs e)
        {
            Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

            if (serv != null)
            {
                serv.PauseChannelIncremental(cmbChannelID.Items[cmbChannelID.SelectedIndex].ToString());
            }
        }

        private void btRetomar_Click(object sender, EventArgs e)
        {
            Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

            if (serv != null)
            {
                serv.ResumeChannelIncremental(cmbChannelID.Items[cmbChannelID.SelectedIndex].ToString());
            }
        }

        private void btD330_Click(object sender, EventArgs e)
        {
            Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

            if (serv != null)
            {
                serv.DoD330(cmbChannelID.Items[cmbChannelID.SelectedIndex].ToString());
            }

        }

        private void btRecInterval_Click(object sender, EventArgs e)
        {
            Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();



            //int seqIni = Convert.ToInt32(txtSeqNumIni.Text.Trim());
            //int seqFim = Convert.ToInt32(txtSeqNumFim.Text.Trim());

            //if (seqIni < 1)
            //{
            //    MessageBox.Show("SeqNum inicial invalido");
            //    return;
            //}

            //if (seqFim < seqIni)
            //{
            //    MessageBox.Show("SeqNum final invalido");
            //    return;
            //}

            if (serv != null)
            {
                //serv.RecoveryInterval(cmbChannelID.Items[cmbChannelID.SelectedIndex].ToString(), seqIni, seqFim);
                serv.PauseChannelIncremental("055");
                serv.PauseChannelIncremental("056");
                serv.PauseChannelIncremental("057");
                serv.PauseChannelIncremental("058");

                serv.ResumeChannelIncremental("055");
                serv.ResumeChannelIncremental("056");
                serv.ResumeChannelIncremental("057");
                serv.ResumeChannelIncremental("058");
            }

        }

        private void frmMdsManagerApp_Load(object sender, EventArgs e)
        {
            cbProduct.Items.Add("0-Todos");
            cbProduct.Items.Add("2-Commodity");
            cbProduct.Items.Add("3-Corporate");
            cbProduct.Items.Add("4-Currency");
            cbProduct.Items.Add("5-Equity");
            cbProduct.Items.Add("6-Government");
            cbProduct.Items.Add("7-Index");
            cbProduct.Items.Add("15-Economic Indicator");
            cbProduct.Items.Add("16-Multileg");
            cbProduct.SelectedIndex = 4;

            cbSecurityType.Items.Add("Todos");
            cbSecurityType.Items.Add("CORP");
            cbSecurityType.Items.Add("FUT");
            cbSecurityType.Items.Add("SPOT");
            cbSecurityType.Items.Add("SOPT");
            cbSecurityType.Items.Add("FOPT");
            cbSecurityType.Items.Add("DTERM");
            cbSecurityType.Items.Add("CS");
            cbSecurityType.Items.Add("PS");
            cbSecurityType.Items.Add("FORWARD");
            cbSecurityType.Items.Add("ETF");
            cbSecurityType.Items.Add("OPT");
            cbSecurityType.Items.Add("INDEX");
            cbSecurityType.Items.Add("OPTEXER");
            cbSecurityType.Items.Add("MLEG");
            cbSecurityType.Items.Add("SECLOAN");
            cbSecurityType.Items.Add("INDEXOPT");
            cbSecurityType.SelectedIndex = 7;

            tcpConflatedSenderCompID = ConfigurationManager.AppSettings["ConflatedSenderCompID"].ToString();
        }


        private void btResend_Click(object sender, EventArgs e)
        {
            Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

            int seqIni = Convert.ToInt32(txtSeqNumIni.Text.Trim());
            int seqFim = Convert.ToInt32(txtSeqNumFim.Text.Trim());

            if (seqIni < 1)
            {
                MessageBox.Show("SeqNum inicial invalido");
                return;
            }

            if (seqFim < seqIni)
            {
                seqFim = 0;
            }

            if (serv != null)
            {
                serv.EnviarResendRequestConflated(tcpConflatedSenderCompID, seqIni, seqFim);
            }

        }

        private void btSeqReset_Click(object sender, EventArgs e)
        {
            Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

            if (serv != null)
            {
                serv.EnviarSequenceResetConflated(tcpConflatedSenderCompID, Convert.ToInt32(txtSeqNumIni.Text));
            }
        }

        private void btSubSecList_Click(object sender, EventArgs e)
        {
            try
            {
                Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

                string secType = null;
                if (cbSecurityType.SelectedIndex != 0)
                {
                    secType = cbSecurityType.Items[cbSecurityType.SelectedIndex].ToString();
                }

                string product = null;

                if (cbProduct.SelectedIndex != 0)
                {
                    product = products[cbProduct.SelectedIndex];
                }

                string cfiCode = null;
                if (!String.IsNullOrEmpty(txtCFICode.Text))
                {
                    cfiCode = txtCFICode.Text;
                }

                if (serv != null)
                {
                    string reqID = serv.EnviarAssinaturaSecurityListConflated(
                        tcpConflatedSenderCompID,
                        secType,
                        product,
                        cfiCode);

                    FIXSubscription subscription = new FIXSubscription();
                    subscription.CFICode = cfiCode;
                    subscription.Product = product;
                    subscription.ReqID = reqID;
                    subscription.SecurityTye = secType;

                    cbRequestSent.Items.Add(reqID);
                    dctRequests.Add(reqID, subscription);
                }
            }
            catch (Exception ex)
            {
                logger.Error("btSubSecList_Click: " + ex.Message, ex);
            }

        }

        private void btUnsubSecList_Click(object sender, EventArgs e)
        {
            try
            {
                string reqID = cbRequestSent.Items[cbRequestSent.SelectedIndex].ToString();

                if (dctRequests.ContainsKey(reqID))
                {
                    FIXSubscription subscription = dctRequests[reqID];

                    Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

                    serv.CancelarAssinaturaSecurityListConflated(tcpConflatedSenderCompID,
                        subscription.SecurityTye,
                        subscription.Product,
                        subscription.CFICode,
                        reqID);
                }
            }
            catch (Exception ex)
            {
                logger.Error("btUnsubSecList_Click: " + ex.Message, ex);
            }
        }

        private void btSubMarketData_Click(object sender, EventArgs e)
        {
            try
            {
                Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

                string secType = null;
                if (cbSecurityType.SelectedIndex != 0)
                {
                    secType = cbSecurityType.Items[cbSecurityType.SelectedIndex].ToString();
                }

                string product = null;

                if (cbProduct.SelectedIndex != 0)
                {
                    product = products[cbProduct.SelectedIndex];
                }

                string cfiCode = null;
                if (!String.IsNullOrEmpty(txtCFICode.Text))
                {
                    cfiCode = txtCFICode.Text;
                }

                if (serv != null)
                {
                    string reqID = serv.EnviarAssinaturaMarketDataConflated(
                        tcpConflatedSenderCompID,
                        txtInstrumento.Text.ToUpper().Trim(),
                        secType,
                        product,
                        cfiCode);

                    FIXSubscription subscription = new FIXSubscription();
                    subscription.CFICode = cfiCode;
                    subscription.Instrumento = txtInstrumento.Text.ToUpper().Trim();
                    subscription.Product = product;
                    subscription.ReqID = reqID;
                    subscription.SecurityTye = secType;

                    cbRequestSent.Items.Add(reqID);
                    dctRequests.Add(reqID, subscription);
                }
            }
            catch (Exception ex)
            {
                logger.Error("btSubMarketData_Click: " + ex.Message, ex);
            }
        }

        private void btUnsubMData_Click(object sender, EventArgs e)
        {
            try
            {
                string reqID = cbRequestSent.Items[cbRequestSent.SelectedIndex].ToString();

                if (dctRequests.ContainsKey(reqID))
                {
                    FIXSubscription subscription = dctRequests[reqID];

                    Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

                    serv.CancelarAssinaturaMarketDataConflated(tcpConflatedSenderCompID,
                        subscription.Instrumento,
                        subscription.SecurityTye,
                        subscription.Product,
                        subscription.CFICode,
                        reqID);
                }
            }
            catch (Exception ex)
            {
                logger.Error("btUnsubMData_Click: " + ex.Message, ex);
            }

        }

        private void btAtivarConflated_Click(object sender, EventArgs e)
        {
            try
            {
                Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

                serv.AtivarChannelConflated(tcpConflatedSenderCompID);
            }
            catch (Exception ex)
            {
                logger.Error("btAtivarConflated_Click: " + ex.Message, ex);
            }
        }

        private void btDesativConflated_Click(object sender, EventArgs e)
        {
            try
            {
                Gradual.MDS.Adm.Lib.IServicoMdsAdm serv = Ativador.Get<IServicoMdsAdm>();

                serv.DesativarChannelConflated(tcpConflatedSenderCompID);
            }
            catch (Exception ex) 
            {
                logger.Error("btDesativConflated_Click: " + ex.Message, ex);
            }
        }

    }
}

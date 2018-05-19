using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.AutomacaoDesktop;

namespace AppTesteAutomacao
{
    public  delegate void UpdateSunda(string msg);

    public partial class Form1 : Form
    {
        private BovespaClientSinal bovcli = new BovespaClientSinal();
        private UpdateSunda DoSunda;
        private long rcvMsgs = 0;

        public Form1()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            DoSunda += new UpdateSunda(UpdateSunda);

            txtRequestMsg.Text = DateTime.Now.ToString("yyyyMMddA0000000002");

            bovcli.OnDataReceived += new BovespaDataReceivedEventHandler(OnDataReceived);
        }

        private void btConectar_Click(object sender, EventArgs e)
        {
            bovcli.LastMsg = txtRequestMsg.Text;
            bovcli.Connect("10.10.91.32", "15000");
        }

        private void OnDataReceived(string LastMsgId, string SPF_Header, string DataPtr, int DataSize)
        {

            rcvMsgs++;
            
            if ( (rcvMsgs % 100) == 0 )
                this.BeginInvoke(DoSunda, LastMsgId+DataPtr);
        }

        private void btDesconectar_Click(object sender, EventArgs e)
        {
            bovcli.Disconnect();
        }

        private void OnError(int error, string msg, string description)
        {
            MessageBox.Show("Erro (" + error + "): " + msg + "-" + description);
        }

        private void UpdateSunda(string msg)
        {
            this.txtMsg.Text = msg;
        }
    }
}

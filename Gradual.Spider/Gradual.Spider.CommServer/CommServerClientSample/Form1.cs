using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Spider.CommSocket;
using Gradual.Spider.Communications.Lib.Mensagens;
using System.Net.Sockets;
using log4net;
using CommServerProviderSample;

namespace CommServerClientSample
{
    public partial class Form1 : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SpiderSocket spiderSocket;
        private string SessionID = "";

        public Form1()
        {
            InitializeComponent();

            spiderSocket = new SpiderSocket();
            spiderSocket.IpAddr = "127.0.0.1";
            spiderSocket.Port = 23231;
            spiderSocket.OnConnectionOpened += new ConnectionOpenedHandler(spiderSocket_OnConnectionOpened);
            spiderSocket.AddHandler<SondaCommServer>(new ProtoObjectReceivedHandler<SondaCommServer>(sondaCommserver));
            spiderSocket.AddHandler<LoginCommServerResponse>(new ProtoObjectReceivedHandler<LoginCommServerResponse>(loginResponse));
            spiderSocket.AddHandler<SimpleProviderPublishing>(new ProtoObjectReceivedHandler<SimpleProviderPublishing>(simpleProviderSinal));
        }

        void spiderSocket_OnConnectionOpened(object sender, ConnectionOpenedEventArgs args)
        {
            MessageBox.Show("Conexao aberta, enviando assinatura");

            LoginCommServerRequest loginRequest = new LoginCommServerRequest();
            loginRequest.AppDescription = "CommServerClientSample";

            spiderSocket.SendObject(loginRequest);
        }

        
        private void simpleProviderSinal(object sender, int clientNumber, Socket clientSock, SimpleProviderPublishing args)
        {
            logger.DebugFormat("Recebeu {0} {1} {2}", args.Instrumento, args.TimeStampSinal.ToString(), args.SinalDescription);
        }


        private void sondaCommserver(object sender, int clientNumber, Socket clientSock, SondaCommServer args)
        {
            logger.Info("RecebeuSonda, respondendo");

            spiderSocket.SendObject(args);
        }

        private void loginResponse(object sender, int clientNumber, Socket clientSock, LoginCommServerResponse args)
        {
            logger.Info("Recebeu resposta do login, SessionID [" +args.SessionID + "]");
            SessionID = args.SessionID;
        }

        private void btConectar_Click(object sender, EventArgs e)
        {
            spiderSocket.OpenConnection();
        }

        private void btAssinar_Click(object sender, EventArgs e)
        {
            AssinaturaCommServerRequest assinatura = new AssinaturaCommServerRequest();
            assinatura.SessionID = this.SessionID;
            assinatura.Instrumento = txtInstrumento.Text.Trim();

            //assinatura.Parametros = new SimpleProviderSubscriptionRequest[1];
            //assinatura.Tipos = new Type[1];


            SimpleProviderSubscriptionRequest req = new SimpleProviderSubscriptionRequest();
            req.Instrumento = txtInstrumento.Text.Trim();

            AssinaturaCommServerRequest.AddObject(assinatura,req);
            assinatura.TiposAssinados.Add(typeof(SimpleProviderPublishing));

            spiderSocket.SendObject(assinatura);

        }

        private void btCancelarAssinatura_Click(object sender, EventArgs e)
        {
            CancelAssinaturaCommServerRequest assinatura = new CancelAssinaturaCommServerRequest();
            assinatura.SessionID = this.SessionID;
            assinatura.Instrumento = txtInstrumento.Text.Trim();
            assinatura.TiposAssinados.Add(typeof(SimpleProviderPublishing));

            spiderSocket.SendObject(assinatura);
        }


    }
}

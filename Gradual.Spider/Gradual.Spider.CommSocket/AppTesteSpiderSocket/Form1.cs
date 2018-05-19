using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Spider.CommSocket;
using ProtoBuf;
using System.IO;
using System.Net.Sockets;

namespace AppTesteSpiderSocket
{
    public partial class Form1 : Form
    {
        public delegate void InvokeDelegate(TextBox textBox, string text);

        public void UpdateTextBox(TextBox textBox, string text)
        {
            textBox.Text = text;
        }

        private SpiderSocket serverSck;
        private SpiderSocket clientSck;

        [ProtoContract]
        class Sunda
        {
            [ProtoMember(1)]
            public string Texto { get; set; }
        }

        [ProtoContract]
        class Munga
        {
            [ProtoMember(1)]
            public string Texto { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btStartServer_Click(object sender, EventArgs e)
        {
            serverSck = new SpiderSocket();
            serverSck.StartListen(13432);
            serverSck.OnMessageReceived += new MessageReceivedHandler(serverSck_OnMessageReceived);
            serverSck.AddHandler<Sunda>(new ProtoObjectReceivedHandler<Sunda>(serverSck_OnObjectReceived));
            serverSck.AddHandler<Munga>(new ProtoObjectReceivedHandler<Munga>(serverSck_OnObjectReceived2));
            //serverSck.OnObjectReceived += new ProtoObjectReceivedHandler<Sunda>(serverSck_OnObjectReceived);
        }

        void serverSck_OnObjectReceived(object sender, int clientNumber, Socket clientSocket, Sunda args)
        {
            Sunda sunda = args;

            string msg = args.GetType().ToString() + ":" + sunda.Texto;

            object[] param = new object[2];

            param[0] = txtMsgServer;
            param[1] = msg;

            txtMsgServer.BeginInvoke(new InvokeDelegate(UpdateTextBox), param);

        }

        void serverSck_OnObjectReceived2(object sender, int clientNumber, Socket clientSocket, Munga args)
        {
            Munga munga = args;

            string msg = args.GetType().ToString() + ":" + munga.Texto + "MUNGA!!!";

            object[] param = new object[2];

            param[0] = txtMsgServer;
            param[1] = msg;

            txtMsgServer.BeginInvoke(new InvokeDelegate(UpdateTextBox), param);

        }

        void serverSck_OnMessageReceived(object sender, MessageEventArgs args)
        {
            MemoryStream xxx = new MemoryStream(args.Data, 0, args.DataLen);

            Envelope envelope = Serializer.Deserialize<Envelope>(xxx);

            Type tipo = envelope.Tipo;

            xxx = new MemoryStream(envelope.Data);

            Object sunda = Serializer.NonGeneric.Deserialize(tipo,xxx);

            if (sunda != null)
            {
                string caralho = sunda.GetType().ToString();

                //string msg = System.Text.Encoding.ASCII.GetString(args.Data,0,args.DataLen);
                string msg = ((Sunda)sunda).Texto;

                object[] param = new object[2];

                param[0] = txtMsgServer;
                param[1] = msg;


                txtMsgServer.BeginInvoke(new InvokeDelegate(UpdateTextBox), param);
            }
        }

        private void btEnviarServer_Click(object sender, EventArgs e)
        {
            serverSck.SendToAll(txtMsgServer.Text);
        }

        private void btEnviarCliente_Click(object sender, EventArgs e)
        {
            Sunda sunda = new Sunda();
            sunda.Texto = txtClient.Text;

            MemoryStream xxx = new MemoryStream();

            Serializer.NonGeneric.Serialize(xxx, sunda);

            Envelope envelope = new Envelope();
            envelope.Tipo = sunda.GetType();
            envelope.Data = xxx.ToArray();

            xxx = new MemoryStream();
            Serializer.Serialize<Envelope>(xxx, envelope);

            clientSck.Send(xxx.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sunda sunda = new Sunda();
            sunda.Texto = txtClient.Text;

            clientSck.SendObject(sunda);
        }


        private void btStartClient_Click(object sender, EventArgs e)
        {
            clientSck = new SpiderSocket();
            clientSck.OnMessageReceived += new MessageReceivedHandler(clientSck_OnMessageReceived);
            clientSck.IpAddr = "127.0.0.1";
            clientSck.Port = 13432;
            clientSck.OpenConnection();
        }

        void clientSck_OnMessageReceived(object sender, MessageEventArgs args)
        {
            string msg  = System.Text.Encoding.ASCII.GetString(args.Data, 0, args.DataLen);

            object[] param = new object[2];

            param[0] = txtClient;
            param[1] = msg;


            txtClient.BeginInvoke(new InvokeDelegate(UpdateTextBox), param);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Munga munga = new Munga();
            munga.Texto = txtClient.Text;

            clientSck.SendObject(munga);

        }

       
    }
}

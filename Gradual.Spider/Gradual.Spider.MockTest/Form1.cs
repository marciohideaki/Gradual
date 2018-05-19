using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Spider.CommSocket;
using System.Collections;
using System.Reflection;
using ProtoBuf;
using System.IO;

namespace Gradual.Spider.MockTest
{
    public partial class Form1 : Form
    {
        private MdsBayeuxClient.MdsAssinatura lAssinatura = new MdsBayeuxClient.MdsAssinatura();
        private MdsBayeuxClient.MdsHttpClient.OnAcompanhamentoOrdensHandler gOnAcompanhamentoOrdensHandler;
        private BayeuxClient.LinkedBlockingQueue<MdsBayeuxClient.MdsAcompanhamentoOrdensEventArgs> filaAcompanhamentoOrdens{ get; set; }

        private SortableBindingList<Ordem> Todas = new SortableBindingList<Ordem>();

        private SpiderSocket serverSck;
        private SpiderSocket clientSck;

        public delegate void InvokeDelegateTextBox(TextBox textBox, string text);
        public delegate void InvokeDelegateLabel(Label label, string text);

        public delegate void InvokeDelegateOrdem(object sender, Ordem ordem);

        private bool isSnapshot = true;
        private bool Transmiting = false;

        public Form1()
        {
            InitializeComponent();
        }

        public void UpdateTextBox(TextBox textBox, string text)
        {
            textBox.AppendText(text + "\r\n");
        }

        public void UpdateDataGridView(object sender, Ordem ordem)
        {
            ordemBindingSource.Add(ordem);
        }

        public void UpdateLabel(Label label, string text)
        {
            label.Text = text;
        }

        void clientSck_OnMessageReceived(object sender, MessageEventArgs args)
        {
            string msg  = System.Text.Encoding.ASCII.GetString(args.Data, 0, args.DataLen);

            Ordem ord = Deserialize((byte[])args.Data);
            
            object[] param = new object[2];
            object[] paramGrid = new object[2];

            param[0] = txtClient;
            param[1] = ord.ClOrdID.ToString();

            txtClient.BeginInvoke(new InvokeDelegateTextBox(UpdateTextBox), param);

            //ordemBindingSource.Add(ord);
            paramGrid[0] = dataGridView1;
            paramGrid[1] = ord;
            dataGridView1.BeginInvoke(new InvokeDelegateOrdem(UpdateDataGridView), paramGrid);
        }

        private void btStartServer_Click(object sender, EventArgs e)
        {
            serverSck = new SpiderSocket();
            serverSck.StartListen(13432);
            serverSck.OnMessageReceived += new MessageReceivedHandler(serverSck_OnMessageReceived);
        }

        private void btStartClient_Click(object sender, EventArgs e)
        {
            clientSck = new SpiderSocket();
            clientSck.OnMessageReceived += new MessageReceivedHandler(clientSck_OnMessageReceived);
            clientSck.IpAddr = "127.0.0.1";
            clientSck.Port = 13432;
            clientSck.OpenConnection();
        }

        void serverSck_OnMessageReceived(object sender, MessageEventArgs args)
        {
            string msg = System.Text.Encoding.ASCII.GetString(args.Data, 0, args.DataLen);

            object [] param = new object[2];

            param[0] = txtMsgServer;
            param[1] = msg;


            //txtMsgServer.BeginInvoke(new InvokeDelegate(UpdateTextBox), param);
        }

        private void btEnviarServer_Click(object sender, EventArgs e)
        {
            serverSck.SendToAll(txtMsgServer.Text);
        }

        private void btEnviarCliente_Click(object sender, EventArgs e)
        {
            clientSck.Send(txtClient.Text);
            
        }

        public void IniciaProcessamento()
        {
            try
            {
                filaAcompanhamentoOrdens = new BayeuxClient.LinkedBlockingQueue<MdsBayeuxClient.MdsAcompanhamentoOrdensEventArgs>();
                System.Threading.Thread trataAcompanhamentoOrdens = new System.Threading.Thread(ThreadTrataAcompanhamento);
                trataAcompanhamentoOrdens.Name = "TrataAcompanhamentoOrdens";
                trataAcompanhamentoOrdens.Start(filaAcompanhamentoOrdens);
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        private void ThreadTrataAcompanhamento(Object filaMensagens)
        {
            BayeuxClient.LinkedBlockingQueue<MdsBayeuxClient.MdsAcompanhamentoOrdensEventArgs> fila =
                (BayeuxClient.LinkedBlockingQueue<MdsBayeuxClient.MdsAcompanhamentoOrdensEventArgs>)filaMensagens;
            try
            {
                while (true)
                {
                    MdsBayeuxClient.MdsAcompanhamentoOrdensEventArgs msg = fila.Pop();
                    MethodInvoker wrapper = () =>
                    {

                        if (lblInicioMds.Text.Equals("(vazio)"))
                        {
                            lblInicioMds.Text = DateTime.Now.ToString();
                        }

                        Processar(msg);
                    };

                    try
                    {
                        this.BeginInvoke(wrapper, null);
                    }
                    catch (ObjectDisposedException)
                    {
                        // Ignore.  Control is disposed cannot update the UI.
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        int ContatodMensagensRecebidas = 0;
        public delegate void OnProcessarCallBack(Object pOrdem);
        public void Processar(Object pOrdem)
        {

            object[] param = new object[2];

            param[0] = lblFimMDS;
            param[1] = DateTime.Now.ToString();

            if (lblFimMDS.Text.Equals("(vazio)"))
            {
                lblFimMDS.BeginInvoke(new InvokeDelegateLabel(UpdateLabel), param);
            }

            if (this.InvokeRequired)
            {
                OnProcessarCallBack call = new OnProcessarCallBack(Processar);
                this.Invoke(call, new object[] { pOrdem });
            }
            else
            {
                try
                {
                    if (pOrdem.GetType().Equals(typeof(MdsBayeuxClient.MdsAcompanhamentoOrdensEventArgs)))
                    {
                        if (!((MdsBayeuxClient.MdsAcompanhamentoOrdensEventArgs)pOrdem).cb.ac.Equals("1"))
                        {
                            isSnapshot = false;
                        }

                        Ordem lOrdemNova = new Ordem(((MdsBayeuxClient.MdsAcompanhamentoOrdensEventArgs)pOrdem).ordem);

                        if (lOrdemNova.Acompanhamentos.Count != 0)
                        {
                            if (lOrdemNova.Acompanhamentos.Count != 0)
                            {
                                try
                                {
                                    if (String.IsNullOrEmpty(lOrdemNova.OrigClOrdID))
                                    {
                                        lOrdemNova.OrigClOrdID = "";
                                    }

                                    var Ocorrencias = 
                                    from Ordem a in Todas
                                    where a.ClOrdID.Equals(lOrdemNova.ClOrdID) || a.ClOrdID.Equals(lOrdemNova.OrigClOrdID)
                                    select a;

                                    if (Ocorrencias.Count() > 0)
                                    {
                                        Ordem lOrdemAntiga = Ocorrencias.ToList()[0];

                                        lOrdemAntiga.ClOrdID = lOrdemNova.ClOrdID;
                                        lOrdemAntiga.Acompanhamentos = lOrdemNova.Acompanhamentos;
                                        lOrdemAntiga.CumQty = lOrdemNova.CumQty;
                                        lOrdemAntiga.MaxFloor = lOrdemAntiga.MaxFloor;
                                        lOrdemAntiga.MinQty = lOrdemNova.MinQty;
                                        lOrdemAntiga.OrderQty = lOrdemNova.OrderQty;
                                        lOrdemAntiga.OrderQtyRemmaining = lOrdemNova.OrderQtyRemmaining;
                                        lOrdemAntiga.OrdStatus = lOrdemNova.OrdStatus;
                                        lOrdemAntiga.OrigClOrdID = lOrdemNova.OrigClOrdID;
                                        lOrdemAntiga.Price = lOrdemNova.Price;
                                        lOrdemAntiga.TransactTime = lOrdemNova.TransactTime;
                                    }
                                    else
                                    {
                                        if (Todas.Count > 0)
                                        {
                                            Todas.Insert(0, lOrdemNova);
                                        }
                                        else
                                        {
                                            Todas.Add(lOrdemNova);
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

                    if (!isSnapshot)
                    {
                        //if (!lblFimMDS.Text.Equals("(vazio)"))
                        //{
                        //    lblFimMDS.Text = DateTime.Now.ToString();
                        //}
                        if (!isSnapshot && Transmiting == false)
                        {
                            //Console.WriteLine(DateTime.Now.ToString());
                            param = new object[2];

                            param[0] = lblInicioSocket;
                            param[1] = DateTime.Now.ToString();

                            if (lblInicioSocket.Text.Equals("(vazio)"))
                            {
                                lblInicioSocket.BeginInvoke(new InvokeDelegateLabel(UpdateLabel), param);
                            }

                            Transmiting = true;

                            Transmitir();

                            Transmiting = false;

                            Console.WriteLine("Ordens:" + total.ToString());
                            //Console.WriteLine(DateTime.Now.ToString());
                            //object[] 
                            param = new object[2];

                            param[0] = lblFimSocket;
                            param[1] = DateTime.Now.ToString();

                            if (lblFimSocket.Text.Equals("(vazio)"))
                            {
                                lblFimSocket.BeginInvoke(new InvokeDelegateLabel(UpdateLabel), param);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }
            }
        }

        private void Assinar()
        {
            if (!MdsBayeuxClient.MdsHttpClient.Conectado)
            {
                MdsBayeuxClient.MdsHttpClient.Instance.Conecta(@"http://192.168.254.110:9090");
            }
            
            lAssinatura.Sinal = MdsBayeuxClient.TipoSinal.Acompanhamento;

            if (gOnAcompanhamentoOrdensHandler == null)
            {
                gOnAcompanhamentoOrdensHandler = new MdsBayeuxClient.MdsHttpClient.OnAcompanhamentoOrdensHandler(OnAcompanhamentoOrdens);
                MdsBayeuxClient.GerenciadorCotacoes.Instance.OnAcompanhamentoEvent += gOnAcompanhamentoOrdensHandler;
            }

            lAssinatura.CodigoCliente = new List<string>();
            
            lAssinatura.CodigoCliente.Add("53084");

            MdsBayeuxClient.MdsHttpClient.Instance.Assina(lAssinatura);
        }

        private void OnAcompanhamentoOrdens(object sender, MdsBayeuxClient.MdsAcompanhamentoOrdensEventArgs e)
        {
            try
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, System.String.Format("{0}: Recebeu acompanhamento de {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), e.ordem.Symbol), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                filaAcompanhamentoOrdens.Push(e);
            } 
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        int total = 0;

        private void Transmitir()
        {
            Console.WriteLine(DateTime.Now.ToString());

            var Ocorrencias = 
            from Ordem a in Todas
            where a.Transmitida.Equals(false)
            select a;

            if (Ocorrencias.Count() > 0)
            {
                foreach(Ordem lOrdem in Ocorrencias)
                {
                    lOrdem.Transmitida = true;
                    byte[] b = SerializeProtoData(lOrdem);
                    serverSck.SendToAll(b, b.Length);
                    total++;
                }
            }
        }

        private void bAssinar_Click(object sender, EventArgs e)
        {
            IniciaProcessamento();
            Assinar();
            //Console.WriteLine(DateTime.Now.ToString());
            object[] param = new object[2];

            param[0] = lblInicioMds;
            param[1] = DateTime.Now.ToString();

            if (lblInicioMds.Text.Equals("(vazio)"))
            {
                lblInicioMds.BeginInvoke(new InvokeDelegateLabel(UpdateLabel), param);
            }
        }

        private void bTransmit_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Proto Buf Serialization
        /// </summary>
        /// <param name="oObj"></param>
        /// <param name="sFile"></param>
        private static byte[] SerializeProtoData(object oObj)
        {
            byte[] b = null;

            try
            {
                //using (var file = File.Create(sFile))
                //{
                //    ProtoBuf.Serializer.Serialize(file, oObj);
                //}
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(ms, oObj);
                    b = new byte[ms.Position];
                    var fullB = ms.GetBuffer();
                    Array.Copy(fullB, b, b.Length);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERRO:" + ex.Message);
            }

            return b;
        }

        public static Ordem Deserialize(byte[] serializationBytes)
        {
            Ordem m = null;

            try
            {
                using (var ms = new MemoryStream(serializationBytes))
                {
                    m = Serializer.Deserialize<Ordem>(ms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO:" + ex.Message);
            }

            return m;
        }
    }
    
    public class ProtoAddress
    {
        [ProtoMember(1)]
        public string Line1 { get; set; }
        [ProtoMember(2)]
        public string Line2 { get; set; }
    }
    
    [ProtoContract]
    public class Ordem //: Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemInfo
    {
        [ProtoMember(1)]
        public decimal AveragePrice { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string ClOrdID { get; set; }
        [ProtoMember(4)]
        public string OrigClOrdID { get; set; }
        [ProtoMember(5)]
        public int Account { get; set; }
        [ProtoMember(6)]
        public string Symbol { get; set; }
        [ProtoMember(7)]
        public OMS.RoteadorOrdens.Lib.Dados.OrdemStatusEnum OrdStatus { get; set; }
        [ProtoMember(8)]
        public OMS.RoteadorOrdens.Lib.Dados.OrdemTipoEnum OrdType { get; set; }
        [ProtoMember(9)]
        public DateTime RegisterTime { get; set; }
        [ProtoMember(10)]
        public DateTime TransactTime { get; set; }
        [ProtoMember(11)]
        public OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum TimeInForce { get; set; }
        [ProtoMember(12)]
        public DateTime? ExpireDate { get; set; }
        [ProtoMember(13)]
        public int ChannelID { get; set; }
        [ProtoMember(14)]
        public string Exchange { get; set; }
        [ProtoMember(15)]
        public OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum Side { get; set; }
        [ProtoMember(16)]
        public int OrderQty { get; set; }
        [ProtoMember(17)]
        public int OrderQtyRemmaining { get; set; }
        [ProtoMember(18)]
        public int CumQty { get; set; }
        [ProtoMember(19)]
        public double? MinQty { get; set; }
        [ProtoMember(20)]
        public double? MaxFloor { get; set; }
        [ProtoMember(21)]
        public double Price { get; set; }
        [ProtoMember(22)]
        public double StopPrice { get; set; }
        [ProtoMember(23)]
        public string CompIDOMS { get; set; }
        [ProtoMember(24)]
        public bool Transmitida { get; set; }
        //[ProtoMember(24)]
        public List<OMS.RoteadorOrdens.Lib.Dados.AcompanhamentoOrdemInfo> Acompanhamentos { get; set; }

        public Ordem()
        {
        }

        public Ordem(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemInfo pOrdem)
        {
            this.ClOrdID = pOrdem.ClOrdID;
            this.OrigClOrdID = pOrdem.OrigClOrdID;
            this.Account = pOrdem.Account;
            this.Symbol = pOrdem.Symbol;
            this.OrdStatus = pOrdem.OrdStatus;
            this.OrdType = pOrdem.OrdType;
            this.RegisterTime = pOrdem.RegisterTime;
            this.TransactTime = pOrdem.TransactTime;
            this.TimeInForce = pOrdem.TimeInForce;
            this.ExpireDate = pOrdem.ExpireDate;
            this.ChannelID = pOrdem.ChannelID;
            this.Exchange = pOrdem.Exchange;
            this.Side = pOrdem.Side;
            this.OrderQty = pOrdem.OrderQty;
            this.OrderQtyRemmaining = pOrdem.OrderQtyRemmaining;
            this.CumQty = pOrdem.CumQty;
            this.MinQty = pOrdem.MinQty;
            this.MaxFloor = pOrdem.MaxFloor;
            this.Price = pOrdem.Price;
            this.StopPrice = pOrdem.StopPrice;
            this.CompIDOMS = pOrdem.CompIDOMS;

            this.Acompanhamentos = pOrdem.Acompanhamentos;
        }
    }

    public class SortableBindingList<T> : BindingList<T>
    {
        private readonly Dictionary<Type, PropertyComparer<T>> comparers;
        private bool isSorted;
        private ListSortDirection listSortDirection;
        private PropertyDescriptor propertyDescriptor;

        public SortableBindingList()
            : base(new List<T>())
        {
            this.comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        public SortableBindingList(IEnumerable<T> enumeration)
            : base(new List<T>(enumeration))
        {
            this.comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return this.isSorted; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return this.propertyDescriptor; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return this.listSortDirection; }
        }

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> itemsList = (List<T>)this.Items;

            Type propertyType = property.PropertyType;
            PropertyComparer<T> comparer;
            if (!this.comparers.TryGetValue(propertyType, out comparer))
            {
                comparer = new PropertyComparer<T>(property, direction);
                this.comparers.Add(propertyType, comparer);
            }

            comparer.SetPropertyAndDirection(property, direction);
            itemsList.Sort(comparer);

            this.propertyDescriptor = property;
            this.listSortDirection = direction;
            this.isSorted = true;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            this.isSorted = false;
            this.propertyDescriptor = base.SortPropertyCore;
            this.listSortDirection = base.SortDirectionCore;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override int FindCore(PropertyDescriptor property, object key)
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                T element = this[i];
                if (property.GetValue(element).Equals(key))
                {
                    return i;
                }
            }

            return -1;
        }
    }

    public class PropertyComparer<T> : IComparer<T>
    {
        private readonly IComparer comparer;
        private PropertyDescriptor propertyDescriptor;
        private int reverse;

        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            this.propertyDescriptor = property;
            Type comparerForPropertyType = typeof(Comparer<>).MakeGenericType(property.PropertyType);
            this.comparer = (IComparer)comparerForPropertyType.InvokeMember("Default", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public, null, null, null);
            this.SetListSortDirection(direction);
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return this.reverse * this.comparer.Compare(this.propertyDescriptor.GetValue(x), this.propertyDescriptor.GetValue(y));
        }

        #endregion

        private void SetPropertyDescriptor(PropertyDescriptor descriptor)
        {
            this.propertyDescriptor = descriptor;
        }

        private void SetListSortDirection(ListSortDirection direction)
        {
            this.reverse = direction == ListSortDirection.Ascending ? 1 : -1;
        }

        public void SetPropertyAndDirection(PropertyDescriptor descriptor, ListSortDirection direction)
        {
            this.SetPropertyDescriptor(descriptor);
            this.SetListSortDirection(direction);
        }
    }


}

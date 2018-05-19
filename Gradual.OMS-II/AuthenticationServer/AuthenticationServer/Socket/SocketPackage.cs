using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AS.Negocio;
using AS.Messages;
using log4net;


namespace AS.Sockets
{
    public class SocketPackage
    {
        #region [local variables]

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Socket MainSocket = null;
        private ArrayList lstCom = new ArrayList();
        private AsyncCallback WorkerCallBack;
        private const string SignIn = "A1";
        private const string SignOut = "A5";
        private const int BytesOfUID = 36;
        private string RemoteIP = "";
        private string RemotePort = "";

        int ClientCount;

        private enum ERetorno
        {
            Sucesso = 1,
            FalhaAutenticacao = 0,
            UsuarioLogado = 2
        };

        #endregion

        #region [ Const variables ]

        private int DataBuffer
        {
            get { return int.Parse(ConfigurationSettings.AppSettings["DataBuffer"].ToString()); }
        }

        #endregion

        #region [ Stock variables ]
        /// <summary>
        /// Armazena os ID's das conexões Ativas.
        /// </summary>	
        private Hashtable HashActiveSockets =
             new Hashtable();

        /// <summary>
        /// Armazena os ID's das conexões Ativas.
        /// </summary>	
        private Hashtable HashActiveClients =
             new Hashtable();

        #endregion

        #region [ Delegates ]

        delegate void _MessageAuthentication(string message, Socket socketData);

        #endregion

        [MTAThread]
        private void OnDataReceived(IAsyncResult asyn)
        {
            SocketPacket socketData = (SocketPacket)asyn.AsyncState;
            try
            {
                lock (this)
                {
                    int ResultBytesLength = socketData.CurrentSocket.EndReceive(asyn);

                    logger.Debug("ResultBytesLength=" + ResultBytesLength);
                    if (ResultBytesLength == 3)
                    {
                        return;
                    }

                    if (ResultBytesLength <= 0)
                    {
                        this.CloseSocket(socketData.CurrentSocket);
                        return;
                    }

                    /* 

                   char[] charsLength =
                       new char[ResultBytesLength];

                   System.Text.Decoder _Decoder = System.Text.Encoding.UTF8.GetDecoder();

                   _Decoder.GetChars(
                       socketData.dataBuffer,
                       0,
                       ResultBytesLength,
                       charsLength,
                       0);

                   string recByteLength = new string(charsLength);
                   int BufferSize = int.Parse(recByteLength);

                   byte[] dataBuffer = new byte[BufferSize];

                   int BytesReceive = socketData.CurrentSocket.Receive(dataBuffer,
                                                                 dataBuffer.Length,
                                                                 SocketFlags.None
                                                                 );

                   char[] charLenght = new char[BytesReceive];

                   string Message = string.Empty;

                   System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();

                   decoder.GetChars(
                       dataBuffer, 0, BytesReceive, charLenght, 0);

                   Message += new string(charLenght);

                   if (BytesReceive < BufferSize)
                   {
                       Message +=
                           ReceiveData(
                           BytesReceive,
                           BufferSize,
                           socketData.CurrentSocket
                           );
                   } */

                    int BufferSize = 0;
                    int pos = 0;

                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 24;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 16;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 8;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF);

                    logger.Debug("BufferSize = " + BufferSize);

                    byte[] dataBuffer = new byte[BufferSize];

                    int BytesReceive = socketData.CurrentSocket.Receive(dataBuffer,
                                                                  dataBuffer.Length,
                                                                  SocketFlags.None
                                                                  );

                    char[] charLenght = new char[BytesReceive];

                    string Message = string.Empty;

                    System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();

                    decoder.GetChars(
                        dataBuffer, 0, BytesReceive, charLenght, 0);

                    Message += new string(charLenght);

                    if (BytesReceive < BufferSize)
                    {
                        Message +=
                            ReceiveData(
                            BytesReceive,
                            BufferSize,
                            socketData.CurrentSocket
                            );
                    }

                    logger.Debug("Message [" + Message + "]");

                    List<object> lstObject = new List<object>();

                    lstObject.Add(Message);
                    lstObject.Add(socketData.CurrentSocket);

                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(MessageBroker), lstObject
                        );                  


                    this.WaitForData(
                                     socketData.CurrentSocket,
                                     socketData.ClientNumber
                                     );
                }
            }
            catch (SocketException se){
                if (se.ErrorCode == (int)(SocketError.ConnectionReset)){
                    this.CloseSocket(socketData.CurrentSocket);
                }
            }
            catch (Exception ex){
                logger.Error("OnDataReceived(): " + ex.Message, ex);
                this.CloseSocket(socketData.CurrentSocket);
            }
        }

        private void CloseSocket(Socket CurrentSocket){

            CurrentSocket.Shutdown(SocketShutdown.Both);
            CurrentSocket.Close();
            CurrentSocket = null;
        }

        private string ReceiveData(int BytesReceive, int BufferSize, Socket SocketData)
        {
            string Message = string.Empty;

            if ((BufferSize - BytesReceive) != 0)
            {
                while (BytesReceive < BufferSize)
                {
                    byte[] Buffer =
                        new byte[(BufferSize - BytesReceive)];

                    int ByteRest = SocketData.Receive(Buffer,
                                                              (BufferSize - BytesReceive),
                                                              SocketFlags.None
                                                              );

                    char[] charLenght = new char[ByteRest];

                    System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();

                    decoder.GetChars(
                        Buffer,
                        0,
                        ByteRest,
                        charLenght,
                        0
                     );

                    BytesReceive += ByteRest;
                    Message += new string(charLenght);
                }
            }
           return Message;
        }

        private void MessageBroker(object Objects)
        {
            try
            {
                // Converte os objetos recebidos por parametro pela Thread principal.
                List<object> LstObjects = ( List<object>)(Objects);
                string recv = LstObjects[0].ToString();                              

                // Header
                Header _GenericHeader;
                IntPtr pBufHeader = Marshal.StringToBSTR(recv.ToString());
                _GenericHeader = (Header)(Marshal.PtrToStructure(pBufHeader, typeof(Header)));

                // Mensagem de SignIn (MsgCode = A1)
                if (_GenericHeader.pStrTipoMensagem == SignIn)
                {
                    logger.Info("Efetuando login...");

                    // Recorta os bytes da mensagem
                    SignIn _stAuthentication;
                    IntPtr pBuf = Marshal.StringToBSTR(recv.ToString());
                    _stAuthentication = (SignIn)(Marshal.PtrToStructure(pBuf, typeof(SignIn)));

                    // Obtem retorno da solicitação de autenticação do banco de dados.
                     object ResultSet = new BAuthenticationServer().InsertAccess(_stAuthentication);

                    // Inicializa a composição da mensagem A2 
                     MDSSignIn _MDSSignIn = new MDSSignIn();                    

                    // Verifica pela quantidade de bytes se o valor do campo é um guid ou um status de erro.
                     if (ResultSet.ToString().Length != BytesOfUID){
                         
                         // STATUS DE ERRO
                         _MDSSignIn.pStrErrorCode = GenericMessage.GetPosition(2, ((int)ResultSet).ToString(), ' ');
                         _MDSSignIn.pStrUniqueId = GenericMessage.GetPosition(36, string.Empty, ' ');

                         // Byte = 0 falha
                         _MDSSignIn.pStrStatusRequest = "0";
                     }
                     else{
                         // SUCESSO pStrErrorCode = vazio.
                         _MDSSignIn.pStrErrorCode = GenericMessage.GetPosition(2, string.Empty, ' ');
                         _MDSSignIn.pStrUniqueId = ResultSet.ToString();

                         // Byte = 1 sucesso
                         _MDSSignIn.pStrStatusRequest = "1";
                     }
                   
                    _MDSSignIn.pStrIdCliente = _stAuthentication.pStrIdCliente;
                    _MDSSignIn.pStrIdSistema = _stAuthentication.pStrIdSistema;                   

                    byte[] ByteArray = new Message().MDSSignIn(_MDSSignIn);

                    //Envia a resposta da autenticação para o Client {msgCode = A2}
                    Socket _currentSocket = (Socket)LstObjects[1];
                    string response = new Message().MDSSignInAsString(_MDSSignIn);

                    logger.Debug("Resposta MDS [" + response + "]");
                    SendData( response, _currentSocket);
                    //((Socket)(LstObjects[1])).Send(ByteArray);
                                      
                }
                else
                {

                    // Mensagem de SignOut (msgCode = A5)
                    if (_GenericHeader.pStrTipoMensagem == SignOut)
                    {
                        logger.Info("Efetuando logout...");

                        SignOut _stAuthentication;
                        IntPtr pBuf = Marshal.StringToBSTR(recv.ToString());
                        _stAuthentication = (SignOut)(Marshal.PtrToStructure(pBuf, typeof(SignOut)));

                        new BAuthenticationServer().UpdateAccess(_stAuthentication);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("MessageBroker(): " + ex.Message, ex);
            }
        }

        private void ClearSocketReset(object _currentSocket){

            Socket currentSocket =
                (Socket)(_currentSocket);

            foreach (DictionaryEntry Dictionary in HashActiveClients)
            {
                if ((IntPtr)Dictionary.Value == currentSocket.Handle)
                {
                    lock (HashActiveClients)
                    {
                        HashActiveClients.Remove
                            (Dictionary.Key.ToString());
                    }
                    break;
                }
            }
        }

        private void MessageAuthentication(object Instance)
        {
            List<object> lstInstance =
                (List<object>)Instance;

            string Recv = (string)lstInstance[0];
            Socket socketData = (Socket)lstInstance[1];

        }

        private void WaitForData(Socket socket, int clientNumber)
        {
            try{
                if (WorkerCallBack == null){
                    WorkerCallBack = new AsyncCallback(OnDataReceived);
                }

                SocketPacket _SocketPacket =
                    new SocketPacket(socket, clientNumber);

                socket.BeginReceive(_SocketPacket.dataBuffer, 0,
                      _SocketPacket.dataBuffer.Length,
                      SocketFlags.None,
                      WorkerCallBack,
                      _SocketPacket
                      );
            }
            catch (SocketException se){
                logger.Error("WaitForData(): " + se.Message, se);
                throw se;
            }
        }

        public void StopListen()
        {
            if (MainSocket != null){
                if (MainSocket.Connected == true){
                    MainSocket.Close();
                }
            }
        }

        public void StartListen(int PortNumber)
        {
            try
            {
                MainSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);

                IPEndPoint ipLocal =
                    new IPEndPoint(IPAddress.Any, PortNumber);

                MainSocket.Bind(ipLocal);
                MainSocket.Listen(4);

                MainSocket.BeginAccept(
                    new AsyncCallback(OnClientConnect), null
                    );
            }
            catch (SocketException se)
            {
                logger.Error("StartListen(): " + se.Message, se);
                throw se;
            }
        }

        private void FillRemoteHost(object RemoteEndPoint)
        {
            try
            {
                if (RemoteEndPoint != null)
                {
                    string[] Host = RemoteEndPoint.ToString().Split(':');
                    RemoteIP = Host[0].ToString();
                    RemotePort = Host[1].ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error("FillRemoteHost(): " + ex.Message, ex);
                throw ex;
            }
        }

        private void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                Socket ActiveSocket = MainSocket.EndAccept(asyn);

                Interlocked.Increment(ref ClientCount);
                this.WaitForData(ActiveSocket, ClientCount);

                MainSocket.BeginAccept(
                    new AsyncCallback(OnClientConnect), null
                    );
            }

            catch (SocketException se)
            {
                logger.Error("OnClientConnect(): " + se.Message, se);
                throw se;
            }
        }

        //public void SendData(string msg)
        //{
        //    try
        //    {
        //        if (MainSocket != null)
        //        {
        //            System.Text.Encoding enc = System.Text.Encoding.ASCII;
        //            byte[] ByteArray = enc.GetBytes(msg);
        //            MainSocket.Send(ByteArray);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("SendData(): " + ex.Message, ex);
        //        throw ex;
        //    }
        //}

        public void SendData(string msg, Socket socket)
        {
            try
            {
                if (socket.Connected)
                {

                    System.Text.Encoding enc = System.Text.Encoding.ASCII;

                    byte[] msgLenBuf = new byte[4];
                    int msgLen = msg.Length;

                    msgLenBuf[0] = (byte)((msgLen & 0xFF000000) >> 24);
                    msgLenBuf[1] = (byte)((msgLen & 0x00FF0000) >> 16);
                    msgLenBuf[2] = (byte)((msgLen & 0x0000FF00) >> 8);
                    msgLenBuf[3] = (byte)(msgLen & 0x000000FF);

                    byte[] ByteArray = enc.GetBytes(msg);
                    byte[] ByteVector = new byte[4 + msg.Length];

                    System.Buffer.BlockCopy(msgLenBuf, 0, ByteVector, 0, 4);
                    System.Buffer.BlockCopy(ByteArray, 0, ByteVector, 4, msg.Length);

                    socket.Send(ByteVector);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public sealed class SocketPacket
    {
        #region [Constructor]
        //Construtor responsável por encapsular o socket e número do cliente corrente.
        public SocketPacket(System.Net.Sockets.Socket socket, int _clientNumber)
        {
            //Encapsula os membros.
            CurrentSocket = socket;
            ClientNumber = _clientNumber;
        }
        #endregion

        #region [Members]strData

        public Socket CurrentSocket;
        public int ClientNumber;
        public byte[] dataBuffer = new byte[4];

        #endregion
    }
}

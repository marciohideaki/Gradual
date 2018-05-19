using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Configuration;
using log4net;

namespace Gradual.OMS.CapturadorCotacaoOracle
{
    public delegate void MDSMessageReceivedHandler (object sender, MDSMessageEventArgs args );

    public class MDSMessageEventArgs : EventArgs
    {
        public string TipoMsg { get; set; }
        public string Instrumento { get; set; }
        public string Message { get; set; }
    }

    public class MDSPackageSocket
    {
        #region [Declarações]
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Socket _ClientSocket = null;
        private AsyncCallback WorkerCallBack;
        private IAsyncResult AsyncResult;

        public const string TIPOMSG_NEGOCIO = "NE";
        public const string TIPOMSG_CADASTROBASICO = "CB";
        public const string TIPOMSG_FECHAMENTO = "FE";
        public const string TIPOMSG_ABERTURA = "AB";
        public const string TIPOMSG_AJUSTE = "AJ";
        public const string TIPOMSG_COMPOSICAOINDICE = "CI";

        private bool bConectado = false;

 
        public string IpAddr { get; set; }

        public int Port { get; set; }

        #endregion

        public DateTime LastPacket { get; set; }

        public event MDSMessageReceivedHandler OnFastQuoteReceived;

        public void OpenConnection()
        {
            try
            {
                _ClientSocket =
                    new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp
                        );


                IPAddress IP = IPAddress.Parse(IpAddr);
                IPEndPoint ipEnd = new IPEndPoint(IP, Convert.ToInt32(Port));

                _ClientSocket.Connect(ipEnd);
                bConectado = true;
                this.TheadWaitOne();
            }
            catch (Exception ex)
            {
                logger.Error("OpenConnection():" + ex.Message);
            }
        }

        public void SendData(string msg, bool SendLenght)
        {
            try
            {
                if (_ClientSocket.Connected)
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

                    _ClientSocket.Send(ByteVector);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void SendData(string msg)
        {
            try
            {
                if (_ClientSocket.Connected)
                {
                    System.Text.Encoding enc = System.Text.Encoding.ASCII;
                    byte[] ByteArray = enc.GetBytes(msg);
                    _ClientSocket.Send(ByteArray);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsConectado()
        {
            return bConectado;
        }



        private void TheadWaitOne()
        {
            try
            {
                if (WorkerCallBack == null){
                    //Cria o evento assíncrono que será disparado quando ocorrer um input
                    //de dados
                    WorkerCallBack = new AsyncCallback(OnDataReceived);
                }

                // Encapsula o client socket solicitante e o clientNumber.
                MDSSocketPacket _SocketPacket = new MDSSocketPacket();
                _SocketPacket._Socket = _ClientSocket;

                //Inicializa a recepção assíncrona dos Dados
                AsyncResult = _ClientSocket.BeginReceive(_SocketPacket.dataBuffer,
                    0, _SocketPacket.dataBuffer.Length,
                    SocketFlags.None,
                    WorkerCallBack,
                    _SocketPacket);

            }
            catch (SocketException se)
            {
                throw (se);
            }
        }

        [MTAThread]
        private void OnDataReceived(IAsyncResult asyn)
        {
            string Instrumento = string.Empty;

            int BufferAlocate = 0;
            int BufferReceiveLenght = 4;

            MDSSocketPacket socketData = (MDSSocketPacket)asyn.AsyncState;
            try
            {
                lock (this)
                {
                    int ResultBytesLength = socketData._Socket.EndReceive(asyn);                    

                    // Falha de conexão.
                    if (ResultBytesLength <= 0){
                        this.MDSSocketClose(socketData);
                        return;
                    }

                    int xBufferSize = 0;
                    int posx = 0;
                 

                    if (BufferReceiveLenght > ResultBytesLength){

                        switch (ResultBytesLength)
                        {
                            case 1 :
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF) << 24;                                
                                break;
                            case 2:
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF) << 24;
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF) << 16;
                                break;
                            case 3:
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF) << 24;
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF) << 16;
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF) << 8;
                                break;
                            case 4:
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF) << 24;
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF) << 16;
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF) << 8;
                                xBufferSize += (((int)socketData.dataBuffer[posx++]) & 0xFF);    
                                break;

                        }

                       BufferAlocate =  ReceiveData(
                           (BufferReceiveLenght-ResultBytesLength),
                           4, 
                           xBufferSize, 
                           socketData                           
                           );
                                                   
                    }         
                              

                    char[] charsLength =
                        new char[ResultBytesLength];

                    System.Text.Decoder _Decoder = System.Text.Encoding.UTF8.GetDecoder();

                    _Decoder.GetChars(
                        socketData.dataBuffer,
                        0,
                        ResultBytesLength,
                        charsLength,
                        0);

                    int BufferSize = 0;
                    int pos = 0;

                    byte[] dataBuffer;

                    if (BufferAlocate == 0)
                    {

                        BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 24;
                        BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 16;
                        BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 8;
                        BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF);

                        dataBuffer = new byte[BufferSize];
                    }
                    else{

                         dataBuffer = new byte[BufferAlocate];
                    }

                
               
                    int BytesReceive = socketData._Socket.Receive(dataBuffer,
                                                                  dataBuffer.Length,
                                                                  SocketFlags.None
                                                                  );

                    char[] charLenght = new char[BytesReceive];
                    string Message = string.Empty;

                    System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();

                    decoder.GetChars(
                        dataBuffer, 0, BytesReceive, charLenght, 0);

                    Message += new string(charLenght);

                    if (BytesReceive < BufferSize){
                        Message +=
                            this.ReceiveData(
                            BytesReceive,
                            BufferSize,
                            socketData._Socket
                            );
                    }

                    if ((Message != null) || (Message != ""))
                    {
                        Instrumento = Message.Substring(0, 8);
                        Message = Message.Remove(0, 8);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(
                         delegate(object required)
                         {
                             this.MessageBroker(
                                 Instrumento.Trim(),
                                 Message);
                         }));
                    }

                    BufferAlocate = 0;

                    LastPacket = DateTime.Now;
                    TheadWaitOne();                  
                }
            }
            catch (SocketException se){
                logger.Error("OnDataReceived(SocketEx): " + se.Message + " ErrorCode: " + se.ErrorCode +
                    " Native: " + se.NativeErrorCode +
                    " SocketError: " + se.SocketErrorCode, se);

                logger.Warn("OnDataReceived(): Encerrando conexao com MDS");

                // Falha de conexão
                this.MDSSocketClose(socketData);
            }
            catch (Exception ex)
            {
                logger.Error("OnDataReceived(): " + ex.Message, ex);
                this.MDSSocketClose(socketData);
            }
        }

        private int ReceiveData( int BytesResult , int BufferSize,int xBufferSize, MDSSocketPacket socketData)
        {
            string Message = string.Empty;
            int posx = 0;

                byte[] Buffer =
                    new byte[BytesResult];

                int ByteRest = socketData._Socket.Receive(Buffer,
                                                          BytesResult,
                                                          SocketFlags.None
                                                          );

                switch (ByteRest)
                {
                    case 1:
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF);
                        break;
                    case 2:
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF) << 8;
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF);
                        break;
                    case 3:
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF) << 16;
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF) << 8;
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF);
                        break;
                    case 4:
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF) << 24;
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF) << 16;
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF) << 8;
                        xBufferSize += (((int)Buffer[posx++]) & 0xFF);
                        break;
                }

                return xBufferSize;
        }

        private string ReceiveData(int BytesReceive, int BufferSize, Socket SocketData)
        {
            string Message = string.Empty;

            if ((BufferSize - BytesReceive) > 0){

                while (BytesReceive < BufferSize){

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
                        //(BufferSize - BytesReceive);

                    Message +=
                        new string(charLenght);
                }
            }
            return Message;
        }

        public void MDSSocketClose(MDSSocketPacket _SocketPacket)
        {
            try
            {
                // Finaliza a instancia do Socket
                _SocketPacket._Socket.Shutdown(SocketShutdown.Both);
                _SocketPacket._Socket.Close();
                _SocketPacket._Socket = null;
                _SocketPacket = null;
            }
            catch{}
            finally
            {
                bConectado = false;
            }
        }

        private void MessageBroker(string Instrumento, string Mensagem)
        {
            string tipoMensagem = Mensagem.ToString().Substring(0, 2);

            switch (tipoMensagem)
            {
                case TIPOMSG_NEGOCIO:
                case TIPOMSG_ABERTURA:
                case TIPOMSG_FECHAMENTO:
                    //logger.DebugFormat("Tipo[{0}] Instrumento[{1}]: [{2}]", tipoMensagem, Instrumento, Mensagem);
                    MDSMessageEventArgs args = new MDSMessageEventArgs();
                    args.Instrumento = Instrumento;
                    args.TipoMsg = tipoMensagem;
                    args.Message = Mensagem;
                    if (OnFastQuoteReceived != null)
                        OnFastQuoteReceived(this, args);
                    break;

                default:
                    break;
            }
        }            
    }

    public sealed class MDSSocketPacket
    {
        public Socket _Socket;
        public byte[] dataBuffer = new byte[4];
    }
}

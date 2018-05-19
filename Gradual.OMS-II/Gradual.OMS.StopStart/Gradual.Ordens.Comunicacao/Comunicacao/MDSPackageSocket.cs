using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Recebidas;
using Gradual.OMS.Ordens.Comunicacao.Mensagens.Recebidas;
using log4net;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens
{

    /// <summary>
    /// Rafael Sanches Garcia
    /// </summary>
    /// 
    public class MDSPackageSocket
    {
        #region [Declarações]
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Socket _ClientSocket = null;
        private AsyncCallback WorkerCallBack;
        private IAsyncResult AsyncResult;
        private bool bConectado = false;

        private const string StopSimples = "SS";
        private const string RespostaStop = "RS";
        private const string RespostaCancelamento = "CR";
        private const string A4 = "A4";
        private const string Pong = "PG";

        public DateTime LastPacket { get; set; }
        public string LastMsg { get; set; }

        public delegate void MDSSRespostaCancelamentoEventHandler(object sender, MDSEventArgs e);
        public event MDSSRespostaCancelamentoEventHandler OnMDSSRespostaCancelamentoEvent;

        public delegate void MDSSRespostaSolicitacaoEventHandler(object sender, MDSEventArgs e);
        public event MDSSRespostaSolicitacaoEventHandler OnMDSSRespostaSolicitacaoEvent;

        public delegate void MDSStopStartEventHandler(object sender, MDSEventArgs e);
        public event MDSStopStartEventHandler OnMDSStopStartEvent;

        public delegate void MDSAuthenticationResponseEventHandler(object Response, MDSEventArgs e);
        public event MDSAuthenticationResponseEventHandler OnMDSAuthenticationResponse;

        public delegate void MDSPingEventHandler(object Response, MDSEventArgs e);
        public event MDSPingEventHandler OnMDSPing;


        public string IpAddr{ get;set;}
        public string Port {get;set;} 
        #endregion


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
                this.ThreadWaitOne();
            }
            catch (SocketException se)
            {
                logger.Error(string.Format("{0}{1}", "OpenConnection: ", se.Message));
                throw (se);
            }
        }

        public void CloseConnection()
        {
            try
            {
                // Finaliza a instancia do Socket
                _ClientSocket.Shutdown(SocketShutdown.Both);
                _ClientSocket.Close();
                _ClientSocket = null;
            }
            catch (Exception ex)
            {
                logger.Error("CloseConnection(): " + ex.Message, ex);
            }
            finally
            {
                bConectado = false;
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
                logger.Error(string.Format("{0}{1}", "SendData: ", ex.Message));
                throw ex;
            }

        }
#if SUNDA
        private string ReceiveData(int BytesReceive, int BufferSize, Socket SocketData)
        {
            try
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
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "ReceiveData: ", ex.Message));
                throw ex;
            }
        }
#endif
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
                logger.Error(string.Format("{0}{1}", "SendData: ", ex.Message));
                throw ex;
            }
        }

        public bool IsConectado()
        {
            return bConectado;
        }

        private void ThreadWaitOne()
        {
            try
            {
                if (WorkerCallBack == null){
                    //Cria o evento assíncrono que será disparado quando ocorrer um imput
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
                logger.Error(string.Format("{0}{1}", "TheadWaitOne: ", se.Message));
                throw (se);
            }
        }
#if SUNDA
        [MTAThread]
        private void OnDataReceived(IAsyncResult asyn)
        {
            MDSSocketPacket socketData = (MDSSocketPacket)asyn.AsyncState;
            try
            {
                lock (this)
                {
                    int ResultBytesLength = socketData._Socket.EndReceive(asyn);

                    logger.Info(string.Format("{0}{1}", "OnDataReceived - ResultBytesLength ", ResultBytesLength));

                    // Falha de conexão.
                    if (ResultBytesLength <= 0)
                    {

                        this.MDSSocketClose(socketData);
                        return;
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

                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 24;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 16;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 8;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF);

                    byte[] dataBuffer = new byte[BufferSize];

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

                    if (BytesReceive < BufferSize)
                    {
                        Message +=
                            this.ReceiveData(
                            BytesReceive,
                            BufferSize,
                            socketData._Socket
                            );
                    }

                    this.MessageBroker(Message);
                    ThreadWaitOne();
                }
            }
            catch (SocketException se)
            {
                logger.Error("OnDataReceived(SocketEx): " + se.Message + " ErrorCode: " + se.ErrorCode +
                    " Native: " + se.NativeErrorCode +
                    " SocketError: " + se.SocketErrorCode, se);

                logger.Warn("OnDataReceived(): Encerrando conexao com MDS");

                // Falha de conexão
                this.MDSSocketClose(socketData);
            }
            catch (Exception ex)
            {
                this.MDSSocketClose(socketData);

                logger.Error(string.Format("{0}{1}", "OnDataReceived: ", ex.Message), ex);
                //throw ex;
            }
        }
#else
        [MTAThread]
        private void OnDataReceived(IAsyncResult asyn)
        {
            string Instrumento = string.Empty;

            MDSSocketPacket socketData = (MDSSocketPacket)asyn.AsyncState;
            try
            {
                lock (this)
                {
                    int ResultBytesLength = socketData._Socket.EndReceive(asyn);

                    // Falha de conexão.
                    if (ResultBytesLength <= 0)
                    {
                        logger.Warn("Encerrou conexao com MDS (" + ResultBytesLength + ") recebendo tamanho");
                        this.MDSSocketClose(socketData);
                        return;
                    }

                    // Se recebeu menos que 4 bytes, recebe os bytes restantes para compor o tamanho 
                    while (ResultBytesLength < 4)
                    {
                        int BytesReceived = socketData._Socket.Receive(socketData.dataBuffer,
                                                                    ResultBytesLength,
                                                                    4 - ResultBytesLength,
                                                                    SocketFlags.None);
                        if (BytesReceived <= 0)
                        {
                            logger.Warn("Encerrou conexao com MDS (" + ResultBytesLength + ") recebendo complemento do tamanho");
                            this.MDSSocketClose(socketData);
                            return;
                        }

                        ResultBytesLength += BytesReceived;
                    }

                    char[] charsLength = new char[ResultBytesLength];

                    System.Text.Decoder _Decoder = System.Text.Encoding.UTF8.GetDecoder();

                    _Decoder.GetChars(
                        socketData.dataBuffer,
                        0,
                        ResultBytesLength,
                        charsLength,
                        0);

                    int BufferSize = 0;
                    int pos = 0;

                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 24;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 16;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 8;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF);

                    int TotalBytesReceived = 0;
                    string Message = string.Empty;

                    while (TotalBytesReceived < BufferSize)
                    {
                        int ToReceive = BufferSize - TotalBytesReceived;

                        byte[] dataBuffer = new byte[ToReceive];

                        int BytesReceived = socketData._Socket.Receive(dataBuffer,
                                                                      ToReceive,
                                                                      SocketFlags.None
                                                                      );
                        if (BytesReceived <= 0)
                        {
                            logger.Warn("Encerrou conexao com MDS (" + ResultBytesLength + ") recebendo dados");
                            this.MDSSocketClose(socketData);
                            return;
                        }


                        char[] charLenght = new char[BytesReceived];

                        System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();

                        decoder.GetChars(dataBuffer, 0, BytesReceived, charLenght, 0);

                        Message += new string(charLenght);

                        TotalBytesReceived += BytesReceived;
                    }

                    if ((Message != null) || (Message != ""))
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(
                         delegate(object required)
                         {
                             this.MessageBroker(Message);
                         }));
                    }


                    LastPacket = DateTime.Now;
                    LastMsg = Message;

                    ThreadWaitOne();
                }
            }
            catch (SocketException se)
            {
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

#endif
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
            catch (Exception ex)
            {
                logger.Error("MDSSocketClose(): " + ex.Message, ex);
            }
            finally
            {
                bConectado = false;
            }
        }

        private void MessageBroker(object recv)
        {
            try
            {
                logger.Debug("MDS Msg [" + recv.ToString() + "]");

                switch (recv.ToString().Substring(0, 2))
                {
                    case A4:
                        {
                            A4_ResponseSignIn _A4_ResponseSignIn;

                            // Cria um ponteiro inteiro apontando para a string recebida do sinal
                            IntPtr pBufMessageA4 = Marshal.StringToBSTR(recv.ToString());

                            // Aloca memoria dinamicamente para armazenar a struct de acordo com o numero de bytes da Struct.
                            // utiliza MALLOC para alocação dinamica de memoria (simula uma passagem para o Heap ( operador new ) .
                            // Construção similar a :
                            // int *p = (int *) (malloc(sizeof(int));
                            _A4_ResponseSignIn = (A4_ResponseSignIn)(Marshal.PtrToStructure(pBufMessageA4, typeof(A4_ResponseSignIn)));

                            // Esvazia o espaço de memória alocado para armazenar a struct.
                            Marshal.FreeBSTR(pBufMessageA4);

                            MDSEventArgs args = new MDSEventArgs(_A4_ResponseSignIn);
                            if (this.OnMDSAuthenticationResponse != null)
                            {
                                OnMDSAuthenticationResponse(this, args);
                            }

                            //Event.MDSAuthenticationResponse(_A4_ResponseSignIn.pStrStatusRequest, _ClientSocket);
                        }
                        break;

                    case StopSimples:
                        {
                            SS_StopStartResposta _SS_StopSimplesResposta;


                            // Cria um ponteiro inteiro apontando para a string recebida do sinal
                            IntPtr pBufMessage = Marshal.StringToBSTR(recv.ToString());

                            // Aloca memoria dinamicamente para armazenar a struct de acordo com o numero de bytes da Struct.
                            // utiliza MALLOC para alocação dinamica de memoria (simula uma passagem para o stack ( operador new ) .
                            // Construção similar a :
                            // int *p = (int *) (malloc(sizeof(int));
                            _SS_StopSimplesResposta = (SS_StopStartResposta)(Marshal.PtrToStructure(pBufMessage, typeof(SS_StopStartResposta)));

                            // Esvazia o espaço de memória alocado para armazenar a struct.
                            Marshal.FreeBSTR(pBufMessage);

                            MDSEventArgs args = new MDSEventArgs(_SS_StopSimplesResposta);
                            if (this.OnMDSStopStartEvent != null)
                            {
                                this.OnMDSStopStartEvent(this, args);
                            }

                            //ThreadPool.QueueUserWorkItem(new WaitCallback(ExecuteFactoryEvent), _SS_StopSimplesResposta);
                        }
                        break;

                    case RespostaStop:
                        {
                            RS_RespostaStop _RS_RespostaStop;
                            // Cria um ponteiro inteiro apontando para a string recebida do sinal
                            IntPtr pBufMessageRS = Marshal.StringToBSTR(recv.ToString());

                            // Aloca memoria dinamicamente para armazenar a struct de acordo com o numero de bytes da Struct.
                            // utiliza MALLOC para alocação dinamica de memoria (simula uma passagem para o stack ( operador new ) .
                            // Construção similar a :
                            // int *p = (int *) (malloc(sizeof(int));
                            _RS_RespostaStop = (RS_RespostaStop)(Marshal.PtrToStructure(pBufMessageRS, typeof(RS_RespostaStop)));

                            // Esvazia o espaço de memória alocado para armazenar a struct. 
                            Marshal.FreeBSTR(pBufMessageRS);

                            MDSEventArgs args = new MDSEventArgs(_RS_RespostaStop);
                            if ( this.OnMDSSRespostaSolicitacaoEvent != null )
                            {
                                this.OnMDSSRespostaSolicitacaoEvent(this, args);
                            }

                            //ThreadPool.QueueUserWorkItem( new WaitCallback( ExecuteFactoryEvent), _RS_RespostaStop);
                        }
                        break;

                    case RespostaCancelamento:
                        {
                            CR_CancelamentoStopResposta _CR_CancelamentoStopResposta;

                            // Cria um ponteiro inteiro apontando para a string recebida do sinal
                            IntPtr pBufMessageCR = Marshal.StringToBSTR(recv.ToString());

                            // Aloca memoria dinamicamente para armazenar a struct de acordo com o numero de bytes da Struct.
                            // utiliza MALLOC para alocação dinamica de memoria (simula uma passagem para o stack ( operador new ) .
                            // Construção similar a :
                            // int *p = (int *) (malloc(sizeof(int));
                            _CR_CancelamentoStopResposta = (CR_CancelamentoStopResposta)(Marshal.PtrToStructure(pBufMessageCR, typeof(CR_CancelamentoStopResposta)));

                            // Esvazia o espaço de memória alocado para armazenar a struct.
                            Marshal.FreeBSTR(pBufMessageCR);

                            MDSEventArgs args = new MDSEventArgs(_CR_CancelamentoStopResposta);
                            if (this.OnMDSSRespostaCancelamentoEvent != null)
                            {
                                this.OnMDSSRespostaCancelamentoEvent(this, args);
                            }
                            //ThreadPool.QueueUserWorkItem(new WaitCallback(ExecuteFactoryEvent
                            //    ), _CR_CancelamentoStopResposta);
                        }
                        break;
                    case Pong:
                        {
                            PG_Pong pong;

                            // Cria um ponteiro inteiro apontando para a string recebida do sinal
                            IntPtr pBufMessagePG = Marshal.StringToBSTR(recv.ToString());

                            // Aloca memoria dinamicamente para armazenar a struct de acordo com o numero de bytes da Struct.
                            // utiliza MALLOC para alocação dinamica de memoria (simula uma passagem para o stack ( operador new ) .
                            // Construção similar a :
                            // int *p = (int *) (malloc(sizeof(int));
                            pong = (PG_Pong)(Marshal.PtrToStructure(pBufMessagePG, typeof(PG_Pong)));

                            // Esvazia o espaço de memória alocado para armazenar a struct.
                            Marshal.FreeBSTR(pBufMessagePG);

                            MDSEventArgs args = new MDSEventArgs(pong);
                            if (this.OnMDSPing != null)
                            {
                                this.OnMDSPing(this, args);
                            }

                            //ThreadPool.QueueUserWorkItem(new WaitCallback(ExecuteFactoryEvent), pong);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("MessageBroker(): " + ex.Message, ex);
            }
        }

        //private void ExecuteFactoryEvent(object pObject)
        //{
        //    try
        //    {
        //        //execucao de preco no mds
        //        if (pObject.GetType() == typeof(SS_StopStartResposta))
        //        {
        //            SS_StopStartResposta _SS_StopSimplesResposta = (SS_StopStartResposta)(pObject);
        //            Registrador.SendMDSEventFactory(_SS_StopSimplesResposta);
        //        }

        //        // resposta de status ( ex. stop start cadastrado com sucesso )
        //        if (pObject.GetType() == typeof(RS_RespostaStop))
        //        {
        //            RS_RespostaStop _RS_RespostaStop = (RS_RespostaStop)(pObject);
        //            Registrador.SendMDSEventFactory(_RS_RespostaStop);
        //        }

        //        // resposta de cancelamento de stop start (ex. idstopstart 30 cancelado com sucesso)
        //        if (pObject.GetType() == typeof(CR_CancelamentoStopResposta))
        //        {
        //            CR_CancelamentoStopResposta _CR_CancelamentoStopResposta = (CR_CancelamentoStopResposta)(pObject);
        //            Registrador.SendMDSEventFactory(_CR_CancelamentoStopResposta);
        //        }

        //        // resposta de ping (heartbeat)
        //        if (pObject.GetType() == typeof(PG_Pong))
        //        {
        //            PG_Pong pong = (PG_Pong)(pObject);
        //            Registrador.SendMDSEventFactory(pong);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(string.Format("{0}{1}", "ExecuteFactoryEvent: ", ex.Message));
        //        throw ex;
        //    }
        //}
    }

    public sealed class MDSSocketPacket
    {
        #region [Members]strData

        public Socket _Socket;
        public byte[] dataBuffer = new byte[4];

        #endregion
    }
}


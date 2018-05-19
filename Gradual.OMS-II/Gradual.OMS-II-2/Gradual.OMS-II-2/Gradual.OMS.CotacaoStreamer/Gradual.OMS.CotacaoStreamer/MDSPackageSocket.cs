using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Configuration;
using System.Globalization;
using log4net;

namespace Gradual.OMS.CotacaoStreamer
{

    public class MDSPackageSocket
    {
        #region [Declarações]
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Socket _ClientSocket = null;
        private AsyncCallback WorkerCallBack;
        private IAsyncResult AsyncResult;

        private const string Negocio = "NE";
        private const string Sonda = "SD";

        private bool bConectado = false;
        #endregion

        public string IpAddr { get; set; }
        public string Port { get; set; }
        public DateTime LastPacket { get; set; }
        public DateTime LastNegocioPacket { get; set; }
        public string LastMsg { get; set; }
        public string LastNegocioMsg { get; set; }
        public string LastSondaMsg { get; set; }
        public DateTime HorarioUltimaSonda { get; set; }

        public event MDSMessageReceivedHandler OnFastQuoteReceived;

        public void OpenConnection()
        {
            try
            {
                logger.Info("Abrindo conexao com MDS: " + IpAddr + ":" + Port);

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
                logger.Error("OpenConnection(): " + ex.Message, ex);
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
                if (WorkerCallBack == null)
                {
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
                throw (se);
            }
        }

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
                        Instrumento = Message.Substring(0, 8);
                        Message = Message.Remove(0, 8);

                        QueueManager.Instance.EnqueueSinal(Instrumento, Message);

                        //ThreadPool.QueueUserWorkItem(new WaitCallback(
                        // delegate(object required)
                        // {
                        //     this.MessageBroker(
                        //         Instrumento.Trim(),
                        //         Message);
                        // }));
                    }

                    LastPacket = DateTime.Now;
                    LastMsg = Message;

                    TheadWaitOne();
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

        private void MDSSocketClose(MDSSocketPacket _SocketPacket)
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

        private void MessageBroker(string Instrumento, string Mensagem)
        {
            try
            {
                if (Mensagem.Length > 2)
                {
                    string tpmsg = Mensagem.ToString().Substring(0, 2);
                    switch (tpmsg)
                    {
                        case Negocio:
                            LastNegocioPacket = DateTime.Now;
                            LastNegocioMsg = Mensagem;
                            //logger.Debug("Negocio  [" + Instrumento + "] [" + Mensagem + "]");
                            MDSMessageEventArgs args = new MDSMessageEventArgs();
                            args.Instrumento = Instrumento;
                            args.TipoMsg = tpmsg;
                            args.Mensagem = Mensagem;
                            if (OnFastQuoteReceived != null)
                                OnFastQuoteReceived(this, args);
                            break;

                        case Sonda:
                            logger.Info("Sonda [" + Mensagem + "]");
                            LastSondaMsg = Mensagem;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("MessageBroker(): " + ex.Message, ex);
            }
        }

        private void ProcessaSonda(string Instrumento, string Mensagem)
        {

            //SDBV20110627151711021SONDA               20110627151636000
            //012345678901234567890123456789012345678901234567890123456789
            //          1         2         3         4         5
            if (Mensagem.Length > 41)
            {
                string horario = Mensagem.Substring(41, 14);
                HorarioUltimaSonda = DateTime.ParseExact(horario, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
        }
    }

    public sealed class MDSSocketPacket
    {
        public Socket _Socket;
        public byte[] dataBuffer = new byte[4];
    }
}

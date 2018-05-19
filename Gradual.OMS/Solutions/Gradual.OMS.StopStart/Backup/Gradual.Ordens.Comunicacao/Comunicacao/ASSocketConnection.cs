using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens
{
    public class ASSocketConnection
    {
        #region [Declarações]

        private Socket _ClientSocket = null;
        private AsyncCallback WorkerCallBack;
        private IAsyncResult AsyncResult;

        private string IpAddr{
            get{
                return ConfigurationSettings.AppSettings["ASConnIp"].ToString();
            }
        }

        private string Port{
            get{
                return ConfigurationSettings.AppSettings["ASConnPort"].ToString();
            }
        }

        private enum EStatusRequest
        {
            Sucesso = 1,
            FalhaAutenticacao = 0,
            UsuarioLogado = 2
        };


        #endregion

        public void ASSocketOpen()
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
                this.TheadWaitOne();
            }
            catch (SocketException se)
            {
                throw (se);
            }

        }

        public void ASSocketClose(ASSocketPacket _SocketPacket)
        {
            _SocketPacket._Socket.Shutdown(SocketShutdown.Both);
            _SocketPacket._Socket.Close();
            _SocketPacket._Socket = null;
            _SocketPacket = null;

        }

        public void SendData(string msg)
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
                ASSocketPacket _SocketPacket = new ASSocketPacket();
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
            ASSocketPacket socketData = (ASSocketPacket)asyn.AsyncState;

            try
            {
                lock (this)
                {
                    int ResultBytesLength = socketData._Socket.EndReceive(asyn);

                    if (ResultBytesLength <= 0)
                    {
                        this.ASSocketClose(socketData);
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
                            ReceiveData(
                            BytesReceive,
                            BufferSize,
                            socketData._Socket
                            );
                    }
               
                    
                    // Rebate a mensagem apenas substituindo o tipo para A3
                    Message = Message.Replace("A2", "A3");

                    // Realiza a conexão com o MDS e solicita Login.
                     MDSPackageSocket _ClientSocket = new MDSPackageSocket();
                    _ClientSocket.OpenConnection();
                    _ClientSocket.SendData(Message.ToString(), true);

                    // Apos envias a mensagem fecha a conexão.
                    this.ASSocketClose(socketData);
               
                }
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == (int)(SocketError.ConnectionReset))
                {
                    this.ASSocketClose(socketData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ReceiveData(int BytesReceive, int BufferSize, Socket SocketData)
        {
            string Message = string.Empty;

            if ((BufferSize - BytesReceive) != 0){

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

                    BytesReceive += (BufferSize - BytesReceive);
                    Message += new string(charLenght);
                }
            }
            return Message;
        }

        public void MessageBroker(object recv)
        {
            // Recorta os bytes da mensagem
           /* A3_SignIn _stAuthentication;
            IntPtr pBuf = Marshal.StringToBSTR(recv.ToString());
            _stAuthentication = (A3_SignIn)(Marshal.PtrToStructure(pBuf, typeof(A3_SignIn)));            

             AS.ClientSocket.ClientSocket _ClientSocket = new AS.ClientSocket.ClientSocket();
            _ClientSocket.OpenConnection();
            _ClientSocket.SendData(recv.ToString(), true);*/        
                   
          /*  if (_stAuthentication.pStrErrorCode.Trim() == string.Empty)
           {
                AS.ClientSocket.ClientSocket _ClientSocket = new AS.ClientSocket.ClientSocket();
                _ClientSocket.OpenConnection();
                _ClientSocket.SendData(recv, true);

            }*/
            
        }
    }    


    public sealed class ASSocketPacket
    {
        #region [Members]

        public Socket _Socket;
        public byte[] dataBuffer = new byte[4];

        #endregion
    }

   

}

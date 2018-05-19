using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto
{
    public static class Contexto
    {
        /// <summary>
        /// Socket que controla todo o contexto de sessão com o MDS.
        /// </summary>
        public static  Socket SocketPrincipal { set; get; }

        /// <summary>
        /// Método que envia um array de bytes para o servidor
        /// </summary>
        /// <param name="mensagem"> Mensagem que sera enviada para o MDS processar. </param>
        public static void EnviarDados(string mensagem){
            try{
                if (SocketPrincipal.Connected)
                {
                    System.Text.Encoding enc = System.Text.Encoding.ASCII;

                    byte[] msgLenBuf = new byte[4];
                    int msgLen = mensagem.Length;

                    msgLenBuf[0] = (byte)((msgLen & 0xFF000000) >> 24);
                    msgLenBuf[1] = (byte)((msgLen & 0x00FF0000) >> 16);
                    msgLenBuf[2] = (byte)((msgLen & 0x0000FF00) >> 8);
                    msgLenBuf[3] = (byte)(msgLen & 0x000000FF);

                    byte[] ByteArray = enc.GetBytes(mensagem);
                    byte[] ByteVector = new byte[4 + mensagem.Length];

                    System.Buffer.BlockCopy(msgLenBuf, 0, ByteVector, 0, 4);
                    System.Buffer.BlockCopy(ByteArray, 0, ByteVector, 4, mensagem.Length);

                    SocketPrincipal.Send(ByteVector);
                }
            }
            catch (Exception ex){
                throw new Exception(string.Format("{0}{1}", "Não foi possível enviar a ordem: EnviarDados: ", ex.Message));
            }
        }
    }
}

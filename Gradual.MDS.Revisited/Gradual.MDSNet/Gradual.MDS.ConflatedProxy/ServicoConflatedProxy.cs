using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using Gradual.MDS.Core.Lib;
using System.IO.Compression;
using System.IO;
using Gradual.OMS.Library;
using System.Collections.Concurrent;
using System.Threading;
using System.Configuration;

namespace Gradual.MDS.ConflatedProxy
{
    public struct UncompressedPacket 
    {
        public byte [] Buffer {get;set;}
        public long BufferLen { get;set;}
    }
    public class ServicoConflatedProxy : IServicoControlavel
    {
        public const int BUFFER_SIZE = 32255;

        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;
        private bool _bKeepRunning = false;

        private int acceptorPort = 25432;

        private ProxySocketPackage _initiator = new ProxySocketPackage();
        private ProxySocketPackage _acceptor = new ProxySocketPackage();
        private MemoryStream compressedStream = new MemoryStream(); 
        //private Ionic.Zlib.ZlibStream zlib;
        private ICSharpCode.SharpZipLib.Zip.Compression.Inflater zlib;
        private string conflatedServer;
        private int conflatedPort;
        private byte[] unzipped;
        private ConcurrentQueue<UncompressedPacket> qUnpack = new ConcurrentQueue<UncompressedPacket>();
        private object syncQueue = new object();
        private long lastLogSEC = 0;
        private Thread thMain;
        private byte[] _inputBuffer;
        private int _pendingBytes;

        #region IServicoControlavel members
        public void IniciarServico()
        {
            try
            {
                logger.Info("Iniciando ServicoConflatedProxy");

                if (ConfigurationManager.AppSettings["ConflatedServer"] == null)
                {
                    logger.Fatal("Chave 'ConflatedServer' nao esta definida nos appsettings");
                    return;
                }

                conflatedServer = ConfigurationManager.AppSettings["ConflatedServer"].ToString().Trim();


                if (ConfigurationManager.AppSettings["ConflatedPort"] == null)
                {
                    logger.Fatal("Chave 'ConflatedPort' nao esta definida nos appsettings");
                    return;
                }

                conflatedPort = Convert.ToInt32(ConfigurationManager.AppSettings["ConflatedPort"].ToString().Trim());


                if (ConfigurationManager.AppSettings["acceptorPort"] != null)
                {
                    try
                    {
                        acceptorPort = Convert.ToInt32(ConfigurationManager.AppSettings["AcceptorPort"].ToString().Trim());
                    }
                    catch (Exception ex) { }
                }

                _bKeepRunning = true;
                _status = ServicoStatus.EmExecucao;

                thMain = new Thread(new ThreadStart(mainProc));
                thMain.Name = "mainProc";
                thMain.Start();

                logger.Info("ServicoConflatedProxy iniciado");
            }
            catch (Exception ex)
            {
                logger.Error("IniciarServico: " + ex.Message, ex);
            }
        }

        public void  PararServico()
        {
            logger.Info("Finalizando ServicoConflatedProxy");
            _bKeepRunning = false;

            while (thMain.IsAlive)
            {
                Thread.Sleep(250);
                logger.Info("Aguardando finalizar thread principal");
            }

            _status = ServicoStatus.Parado;
            logger.Info("ServicoConflatedProxy finalizado");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        private void mainProc()
        {
            try
            {
                logger.Info("Iniciando thread principal");


                _acceptor.OnClientConnected += new ClientConnectedHandler(_acceptor_OnClientConnected);
                _acceptor.OnClientDisconnected += new ClientDisconnectedHandler(_acceptor_OnClientDisconnected);
                _acceptor.OnRequestReceived += new MessageReceivedHandler(_acceptor_OnRequestReceived);

                _initiator.OnRequestReceived += new MessageReceivedHandler(_initiator_OnDataReceived);
                _initiator.OnClientDisconnected += new ClientDisconnectedHandler(_initiator_OnClientDisconnected);

                logger.Info("Inicializando deflater");

                unzipped = new byte[BUFFER_SIZE];




                //zlib = new Ionic.Zlib.ZlibStream(compressedStream, Ionic.Zlib.CompressionMode.Decompress, true);
                //zlib.BufferSize = BUFFER_SIZE;
                zlib = new ICSharpCode.SharpZipLib.Zip.Compression.Inflater();
                //zlib.FlushMode = Ionic.Zlib.FlushType.Full;

                logger.Info("Iniciando acceptor listening at " + acceptorPort);
                _acceptor.StartListen(acceptorPort);

                while (_bKeepRunning)
                {
                    try
                    {
                        UncompressedPacket pkt;

                        if (qUnpack.TryDequeue(out pkt))
                        {
                            _acceptor.SendToAll(pkt.Buffer);

                            continue;
                        }

                        Thread.Sleep(25);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("mainProc(while{}): " + ex.Message, ex);
                    }
                }

                if ( _acceptor!=null && _acceptor.IsConectado() )
                    _acceptor.CloseSocket();

                if ( _initiator !=null && _initiator.IsConectado() )
                    _initiator.CloseSocket();
            }
            catch (Exception ex)
            {
                logger.Error("mainProc(): " + ex.Message, ex);
            }
        }




        private void OnReceiveDecompressed(IAsyncResult ar)
        {
            try
            {
                if (!_bKeepRunning)
                    return;

                int blidos = 0; // zlib.EndRead(ar);

                if (blidos <= 0)
                {
                    logger.Error("OnReceiveDecompressed Blidos <= 0");
                    return;
                }

                UncompressedPacket pkt = new UncompressedPacket();
                pkt.Buffer = unzipped;
                pkt.BufferLen = blidos;

                qUnpack.Enqueue(pkt);

                unzipped = new byte[BUFFER_SIZE];


                if (DateTime.UtcNow.Ticks - lastLogSEC > TimeSpan.TicksPerSecond)
                {
                    lastLogSEC = DateTime.UtcNow.Ticks;
                    logger.Info("OnReceiveDecompressed: pacotes na fila: " + qUnpack.Count);
                }

                //Start listening to receive more data from the deflater
                //decompressedStream.BeginRead(unzipped, 0, unzipped.Length, new AsyncCallback(OnReceiveDecompressed), null);
            }
            catch (ObjectDisposedException)
            {
                logger.Error("OnReceiveDecompressed(): deflator finalizado!");
            }
            catch (Exception ex)
            {
                logger.Error("OnReceiveDecompressed(): " + ex.Message, ex);
            }
        }


        private void _initiator_OnDataReceived(object sender, MessageEventArgs args)
        {
            try
            {
                if (logger.IsDebugEnabled )
                    logger.Debug("_initiator_OnDataReceived: [" + System.Text.Encoding.ASCII.GetString(args.Message) + "]");

                //compressedStream.Write(args.Message, 0, Convert.ToInt32(args.MessageLen));

                _pendingBytes = appendBytes(args.Message, 0, Convert.ToInt32(args.MessageLen));

                byte [] buff = new byte[BUFFER_SIZE];

                if (zlib.IsNeedingInput)
                {
                    zlib.SetInput( _inputBuffer, 0, _pendingBytes);
                    _pendingBytes = 0;
                }

                int blidos = zlib.Inflate(buff, 0, BUFFER_SIZE);

                if (blidos > 0)
                {

                    UncompressedPacket pkt = new UncompressedPacket();
                    pkt.Buffer = new byte[blidos];
                    pkt.BufferLen = blidos;

                    System.Array.Copy(buff, pkt.Buffer, blidos);

                    //compressedStream.Flush();
                    //compressedStream.Position = 0;

                    qUnpack.Enqueue(pkt);

                    if (logger.IsDebugEnabled)
                        logger.Debug("leu : [" + System.Text.Encoding.ASCII.GetString(buff, 0, blidos) + "]");

                    if (DateTime.UtcNow.Ticks - lastLogSEC > TimeSpan.TicksPerSecond)
                    {
                        lastLogSEC = DateTime.UtcNow.Ticks;
                        logger.Info("Pacotes descompactados na fila: " + qUnpack.Count);
                    }
                }
                //zlib.BeginRead(unzipped, 0, unzipped.Length, new AsyncCallback(OnReceiveDecompressed), null);
            }
            catch (Exception ex)
            {
                logger.Error("_initiator_OnDataReceived: " + ex.Message, ex);
            }
        }

        private void _initiator_OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            try
            {
                logger.Error("_initiator_OnClientDisconnected: desconectou [" + args.ClientNumber + "]");
                compressedStream = new MemoryStream();
                //zlib = new Ionic.Zlib.ZlibStream(compressedStream, Ionic.Zlib.CompressionMode.Decompress, true);
                //zlib.BufferSize = BUFFER_SIZE;
                zlib = new ICSharpCode.SharpZipLib.Zip.Compression.Inflater();
                _pendingBytes = 0;

                //zlib.FlushMode = Ionic.Zlib.FlushType.Full;
            }
            catch (Exception ex)
            {
                logger.Error("_initiator_OnClientDisconnected: " + ex.Message, ex);
            }
        }

        private void _acceptor_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            try
            {
                logger.Info("Recebeu conexao. Conectando initiator em [" + conflatedServer + ":" + conflatedPort + "]");

                compressedStream = new MemoryStream();
                //zlib = new Ionic.Zlib.ZlibStream(compressedStream, Ionic.Zlib.CompressionMode.Decompress, true);
                //zlib.BufferSize = BUFFER_SIZE;
                zlib = new ICSharpCode.SharpZipLib.Zip.Compression.Inflater();
                //zlib.FlushMode = Ionic.Zlib.FlushType.Full;

                _initiator.Port = conflatedPort.ToString();
                _initiator.IpAddr = conflatedServer;
                _initiator.OpenConnection();

            }
            catch (Exception ex)
            {
                logger.Error("_acceptor_OnClientConnected: " + ex.Message, ex);
            }

        }

        private void _acceptor_OnRequestReceived(object sender, MessageEventArgs args)
        {
            try
            {
                if (logger.IsDebugEnabled)
                    logger.Debug("_acceptor_OnRequestReceived: [" + System.Text.Encoding.ASCII.GetString(args.Message) + "]");

                _initiator.Send(args.Message);
            }
            catch (Exception ex)
            {
                logger.Error("_acceptor_OnRequestReceived: " + ex.Message, ex);
            }
        }

        private void _acceptor_OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            try
            {
                _initiator.CloseSocket();
                _pendingBytes = 0;
            }
            catch (Exception ex)
            {
                logger.Error("_acceptor_OnClientDisconnected(): " + ex.Message, ex);
            }
        }


        private int appendBytes(byte[] buffer, int offset, int count)
        {
            byte[] tempBuff = new byte[count + _pendingBytes];

            if ( _pendingBytes > 0 )
                System.Array.Copy(_inputBuffer, tempBuff, _pendingBytes);
            System.Array.Copy(buffer, offset, tempBuff, _pendingBytes, count);

            _inputBuffer = tempBuff;

            return count + _pendingBytes;
        }
        #endregion //IServicoControlavel members
    }
}

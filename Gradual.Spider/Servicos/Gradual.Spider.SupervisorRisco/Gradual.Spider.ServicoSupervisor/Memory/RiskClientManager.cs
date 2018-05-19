using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.CommSocket;
using Gradual.Spider.DataSync.Lib.Mensagens;
using log4net;
using System.Net;
using System.Diagnostics;
using Gradual.Spider.DataSync.Lib;
using System.Net.Sockets;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using System.Collections.Concurrent;
using System.Configuration;
using System.Threading;
using Gradual.Spider.SupervisorRisco.Lib.Handlers;


namespace Gradual.Spider.ServicoSupervisor.Memory
{
    public class RiskClientManager
    {
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static RiskClientManager _me = null;
        private bool _bKeepRunning = false;
        private Spider.CommSocket.SpiderSocket _sckServer = null;

        object _syncPosClient = new object();
        Thread _thSendPosClient;
        Thread _thEnableConn;
        int _port = 0;
        ConcurrentQueue<PositionClientEventArgs> _cqPosCli;
        
        
        Thread _thCRisk;
        object _syncCRisk = new object();
        ConcurrentQueue<ConsolidatedRiskEventArgs> _cqConsolidatedRisk;


        public static RiskClientManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new RiskClientManager();
                }

                return _me;
            }
        }

        public void Start()
        {
            logger.Info("Iniciando server gerenciador");
            _cqPosCli = new ConcurrentQueue<PositionClientEventArgs>();
            _cqConsolidatedRisk = new ConcurrentQueue<ConsolidatedRiskEventArgs>();
            _bKeepRunning = true;

            RiskCache.Instance.OnSymbolListUpdate += new SymbolListUpdateHandler(Instance_OnSymbolListUpdate);
            

            // Managers separados para cada tipo de calculo
            PositionClientManager.Instance.OnPositionClientUpdate+=new Gradual.Spider.ServicoSupervisor.Memory.PositionClientManager.PositionClientUpdateHandler(Instance_OnPositionClientUpdate);
            ConsolidatedRiskManager.Instance.OnConsolidatedRiskUpdate += new ConsolidatedRiskManager.ConsolidatedRiskUpdateHandler(Instance_OnConsolidatedRiskUpdate);


            _sckServer = new CommSocket.SpiderSocket();

            _sckServer.OnClientConnected += new CommSocket.ClientConnectedHandler(_sckServer_OnClientConnected);
            _sckServer.OnClientDisconnected += new CommSocket.ClientDisconnectedHandler(_sckServer_OnClientDisconnected);
            if (ConfigurationManager.AppSettings.AllKeys.Contains("RMSPortListener"))
            {
                _port = Convert.ToInt32(ConfigurationManager.AppSettings["RMSPortListener"].ToString());
                _sckServer.StartListen(_port);
            }
            else
            {
                _port = 5454;
                _sckServer.StartListen(5454);
            }

            logger.Info("Iniciando thread de envio de position client");
            _thSendPosClient = new Thread(new ThreadStart(this._queuePositionClient));
            _thSendPosClient.Start();


            logger.Info("Iniciando thread de calculo / envio de risco consolidado");
            _thCRisk = new Thread(new ThreadStart(this._queueConsolidatedRisk));
            _thCRisk.Start();

            //logger.Info("Iniciando thread de verificacao para habilitar conexoes client...");
            //_thEnableConn = new Thread(new ThreadStart(this._enableConnectionThread));
            //_thEnableConn.Start();

            logger.Info("Server iniciado");
        }

        

        

        void _enableConnectionThread()
        {
            int i = 0;
            while (true)
            {
                if (i > 120)
                {
                    logger.Error("Expirou tempo maximo de espera de composicao do snapshot (60 s). Vai habilitar conexao mesmo assim!!!");
                    _sckServer.StartListen(_port);
                    break;
                }
                if (AcSpiderCache.Instance.GetSnapshotFlag())
                {
                    _sckServer.StartListen(_port);
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                    i++;
                }
            }

        }

        

        void Instance_OnRestrictionGlobalUpdate(object sender, RestrictionGlobalEventArgs args)
        {
            RestrictionGlobalSyncMsg syncMsg = new RestrictionGlobalSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.RestrictionGlobal.AddOrUpdate(args.Account, args.RestrictionGlobal, (key, oldValue) => args.RestrictionGlobal);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnRestrictionGroupSymbolUpdate(object sender, RestrictionGroupSymbolEventArgs args)
        {
            RestrictionGroupSymbolSyncMsg syncMsg = new RestrictionGroupSymbolSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.RestrictionGroupSymbol.AddOrUpdate(args.IdGrupo, args.RestrictionGroupSymbol, (key, oldValue) => args.RestrictionGroupSymbol);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnRestrictionSymbolUpdate(object sender, RestrictionSymbolEventArgs args)
        {
            RestrictionSymbolSyncMsg syncMsg = new RestrictionSymbolSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.RestrictionSymbol.AddOrUpdate(args.Symbol, args.RestrictionSymbol, (key, oldValue) => args.RestrictionSymbol);

            _sckServer.SendToAll(syncMsg);
            
        }

        public void Instance_OnPositionClientUpdate(object sender, PositionClientEventArgs args)
        {
            _cqPosCli.Enqueue(args);
            lock (_syncPosClient)
                Monitor.Pulse(_syncPosClient);
        }

        void Instance_OnConsolidatedRiskUpdate(object sender, ConsolidatedRiskEventArgs args)
        {
            _cqConsolidatedRisk.Enqueue(args);
            lock (_syncCRisk)
                Monitor.Pulse(_syncCRisk);
        }

        private void _queuePositionClient()
        {
            try
            {
                while (_bKeepRunning)
                {
                    PositionClientEventArgs item = null;
                    if (_cqPosCli.TryDequeue(out item))
                    {
                        if (item != null)
                        {
                            _processPositionClient(item);
                        }
                    }
                    else
                    {
                        lock (_syncPosClient)
                            Monitor.Wait(_syncPosClient, 200);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da atualizacao de PostionClient: " + ex.Message, ex);
            }
        }

        private void _queueConsolidatedRisk()
        {
            try
            {
                while (_bKeepRunning)
                {
                    ConsolidatedRiskEventArgs item = null;
                    if (_cqConsolidatedRisk.TryDequeue(out item))
                    {
                        if (item != null)
                        {
                            _processConsolidatedRisk(item);
                        }
                    }
                    else
                    {
                        lock (_syncCRisk)
                            Monitor.Wait(_syncCRisk, 200);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da atualizacao de PostionClient: " + ex.Message, ex);
            }
        }

        private void _processPositionClient(PositionClientEventArgs aux)
        {
            try
            {
                PositionClientSyncMsg syncMsg = new PositionClientSyncMsg();

                syncMsg.SyncAction = (SyncMsgAction)aux.Action;
                syncMsg.PositionClient.AddOrUpdate(aux.Account, aux.PosClient, (key, oldValue) => aux.PosClient);

                _sckServer.SendToAll(syncMsg);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio de PositionClient para os clientes: " + ex.Message, ex);
            }
        }

        private void _processConsolidatedRisk(ConsolidatedRiskEventArgs aux)
        {
            try
            {
                ConsolidatedRiskSyncMsg syncMsg = new ConsolidatedRiskSyncMsg();

                syncMsg.SyncAction = (SyncMsgAction)aux.Action;
                syncMsg.ConsolidatedRisk = aux.ConsolidatedRisk;

                _sckServer.SendToAll(syncMsg);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio de risco consolidado para os clientes: " + ex.Message, ex);
            }
        }


        void Instance_OnContaBrokerUpdate(object sender, ContaBrokerEventArgs args)
        {
            ContaBrokerSyncMsg syncMsg = new ContaBrokerSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.ContaBroker.Add(args.Account, args.ContaBroker);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnOperatingLimitUpdate(object sender, OperatingLimitEventArgs args)
        {
            OperatingLimitSyncMsg syncMsg = new OperatingLimitSyncMsg();

            syncMsg.OperatingLimits.AddOrUpdate(args.Account, args.OperationLimit, (key, oldValue) => args.OperationLimit);
            syncMsg.SyncAction = (SyncMsgAction) args.Action;

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnMaxLossUpdate(object sender, MaxLossEventArgs args)
        {
            MaxLossSyncMsg syncMsg = new MaxLossSyncMsg();

            syncMsg.MaxLoss.AddOrUpdate(args.Account, args.MaxLoss, (key, oldValue) => args.MaxLoss);
            syncMsg.SyncAction = (SyncMsgAction)args.Action;

            _sckServer.SendToAll(syncMsg);
        }

        public void Stop()
        {
            try
            {
                _bKeepRunning = false;

                //Unassign events
                PositionClientManager.Instance.OnPositionClientUpdate -= Instance_OnPositionClientUpdate;

                if (_thSendPosClient != null && _thSendPosClient.IsAlive)
                {
                    _thSendPosClient.Join(200);
                    if (_thSendPosClient.IsAlive)
                    {
                        try
                        {
                            _thSendPosClient.Abort();
                        }
                        catch
                        {
                            logger.Error("Thread _thSendPosCli aborted.");
                        }
                    }
                    _thSendPosClient = null;
                }

                //if (_thEnableConn != null && _thEnableConn.IsAlive)
                //{
                //    _thEnableConn.Join(200);
                //    if (_thEnableConn.IsAlive)
                //    {
                //        try
                //        {
                //            _thEnableConn.Abort();
                //        }
                //        catch
                //        {
                //            logger.Error("Thread _thEnableConn aborted.");
                //        }
                //    }
                //    _thEnableConn = null;
                //}

                if (_thCRisk != null && _thCRisk.IsAlive)
                {
                    _thCRisk.Join(200);
                    if (_thCRisk.IsAlive)
                    {
                        try
                        {
                            _thCRisk.Abort();
                        }
                        catch
                        {
                            logger.Error("Thread _thCRisk aborted.");
                        }
                    }
                    _thCRisk = null;
                }


                _sckServer.StopListen();

                _sckServer.CloseSocket();
            }
            catch (Exception ex)
            {
                logger.Error("Stop(): " + ex.Message, ex);
            }
        }

        void Instance_OnAccountBvspBMFUpdate(object sender, AccountBvspBMFEventArgs args)
        {
            AccountBvspBMFSyncMsg syncMsg = new AccountBvspBMFSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction) args.Action;
            syncMsg.Accounts.Add(args.AccountBvsp, args.AccountBMF);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnBlockedInstrumentUpdate(object sender, BlockedInstrumentEventArgs args)
        {
            BlockedInstrumentSyncMsg syncMsg = new BlockedInstrumentSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.BlockedInstruments.AddOrUpdate(args.SymbolKey, args.BlockedInstrument, (key, oldValue) => args.BlockedInstrument);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnClientLimitBMFUpdate(object sender, ClientLimiBMFEventArgs args)
        {
            ClientLimitBMFSyncMsg syncMsg = new ClientLimitBMFSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.ClientLimits.AddOrUpdate(args.Account, args.ClientLimitBMF, (key, oldValue) => args.ClientLimitBMF);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnClientParameterPermissionUpdate(object sender, ClientParameterPermissionEventArgs args)
        {
            ClientParameterPermissionSyncMsg syncMsg = new ClientParameterPermissionSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.Parametros.AddOrUpdate(args.Account, args.ClientParameterPermission, (key, oldValue) => args.ClientParameterPermission);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnFatFingerUpdate(object sender, FatFingerEventArgs args)
        {
            FatFingerSyncMsg syncMsg = new FatFingerSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.FatFingers.AddOrUpdate(args.Account, args.FatFinger, (key, oldValue) => args.FatFinger);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnOptionBlockUpdate(object sender, OptionBlockEventArgs args)
        {
            OptionBlockSyncMsg syncMsg = new OptionBlockSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.OptionsBlocks.AddOrUpdate(args.Key, args.OptionBlock, (key, oldValue) => args.OptionBlock);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnSymbolListUpdate(object sender, SymbolListEventArgs args)
        {
            SymbolListSyncMsg syncMsg = new SymbolListSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.Symbols.AddOrUpdate(args.Instrumento, args.Symbol, (key, oldValue) => args.Symbol);

            _sckServer.SendToAll(syncMsg);
        }

        void Instance_OnTestSymbolUpdate(object sender, TestSymbolEventArgs args)
        {
            TestSymbolSyncMsg syncMsg = new TestSymbolSyncMsg();

            syncMsg.SyncAction = (SyncMsgAction)args.Action;
            syncMsg.TestSymbols.AddOrUpdate(args.Instrumento, args.TestSymbol, (key, oldValue) => args.TestSymbol);

            _sckServer.SendToAll(syncMsg);
        }

        void _sckServer_OnClientDisconnected(object sender, CommSocket.ClientDisconnectedEventArgs args)
        {
            logger.Info("Cliente [" + args.ClientNumber + "] desconectou");
        }

        void _sckServer_OnClientConnected(object sender, CommSocket.ClientConnectedEventArgs args)
        {
            try
            {
                
                Stopwatch crono = new Stopwatch();
                
                crono.Start();

                IPEndPoint remoteIpEndPoint = (IPEndPoint)args.ClientSocket.RemoteEndPoint;

                logger.Info("Cliente [" + args.ClientNumber + "] [" + remoteIpEndPoint.Address.ToString() + ":" + remoteIpEndPoint.Port + "] conectou, enviando snapshots");

                SendSnapshot(args.ClientSocket);

                crono.Stop();
                
                logger.Info("Enviado snapshot de lista de ativos/instrumentos de testes");

                logger.Info("Fim do envio dos snapshots. Elapsed: " + crono.ElapsedMilliseconds + "ms");

            }
            catch (Exception ex)
            {
                logger.Error("_sckServer_OnClientConnected(): " + ex.Message, ex);
            }
        }

        public void SendSnapshot(Socket clientSocket)
        {
            try
            {

                //AcSpiderCache.Instance.SetMemorySnapshot(true);
                //AccountBvspBMFSyncMsg accMsg = new AccountBvspBMFSyncMsg();
                //accMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //accMsg.Accounts = RiskCache.Instance.SnapshotAccountBvspBMF();

                //if (clientSocket != null)
                //    SpiderSocket.SendObject(accMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(accMsg);

                //logger.Info("Enviado snapshot de contas bmf x bovespa");

                ////
                //BlockedInstrumentSyncMsg blkMsg = new BlockedInstrumentSyncMsg();
                //blkMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //blkMsg.BlockedInstrumentType = BlockedInstrumentMsgType.BlockedSymbolClient;
                //blkMsg.BlockedInstruments = RiskCache.Instance.SnapshotBlockedInstrument(BlockedInstrumentMsgType.BlockedSymbolClient);
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(blkMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(blkMsg);

                //logger.Info("Enviado snapshot de instrumento bloqueados por cliente");

                ////
                //blkMsg = new BlockedInstrumentSyncMsg();
                //blkMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //blkMsg.BlockedInstrumentType = BlockedInstrumentMsgType.BlockedSymbolGroupClient;
                //blkMsg.BlockedInstruments = RiskCache.Instance.SnapshotBlockedInstrument(BlockedInstrumentMsgType.BlockedSymbolGroupClient);
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(blkMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(blkMsg);

                //logger.Info("Enviado snapshot de grupos de instrumentos bloqueados por cliente");


                ////
                //blkMsg = new BlockedInstrumentSyncMsg();
                //blkMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //blkMsg.BlockedInstrumentType = BlockedInstrumentMsgType.BlockedSymbolGroupGlobal;
                //blkMsg.BlockedInstruments = RiskCache.Instance.SnapshotBlockedInstrument(BlockedInstrumentMsgType.BlockedSymbolGroupGlobal);
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(blkMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(blkMsg);

                //logger.Info("Enviado snapshot de instrumentos bloqueados por globalmente");

                ////
                //ClientLimitBMFSyncMsg bmfMsg = new ClientLimitBMFSyncMsg();
                //bmfMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //bmfMsg.ClientLimits = RiskCache.Instance.SnapshotClientLimitBMF();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(bmfMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(bmfMsg);

                //logger.Info("Enviado snapshot de limites BMF por cliente");

                ////
                //ClientParameterPermissionSyncMsg paramMsg = new ClientParameterPermissionSyncMsg();
                //paramMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //paramMsg.Parametros = RiskCache.Instance.SnapshotClientParameterPermission();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(paramMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(paramMsg);

                //logger.Info("Enviado snapshot de parametros de permissoes por cliente");

                //OperatingLimitSyncMsg limMsg = new OperatingLimitSyncMsg();
                //limMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //limMsg.OperatingLimits = RiskCache.Instance.SnapshotOperatingLimit();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(limMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(limMsg);

                //logger.Info("Enviado snapshot de limites operacionais");

                ////
                //FatFingerSyncMsg fatMsg = new FatFingerSyncMsg();
                //fatMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //fatMsg.FatFingers = RiskCache.Instance.SnapshotFatFinger();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(fatMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(fatMsg);

                //logger.Info("Enviado snapshot de fatfinger");


                //OptionBlockSyncMsg optionMsg = new OptionBlockSyncMsg();
                //optionMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //optionMsg.OptionsBlocks = RiskCache.Instance.SnapshotOptionBlock();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(optionMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(optionMsg);

                //logger.Info("Enviado snapshot de bloqueios de opção");

                //SymbolListSyncMsg symMsg = new SymbolListSyncMsg();
                //symMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //symMsg.Symbols = RiskCache.Instance.SnapshotSymbolList();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(symMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(symMsg);

                //logger.Info("Enviado snapshot de lista de ativos teste/instrumentos");


                //TestSymbolSyncMsg tstMsg = new TestSymbolSyncMsg();
                //tstMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //tstMsg.TestSymbols = RiskCache.Instance.SnapshotTestSymbol();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(tstMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(tstMsg);

                //logger.Info("Enviando snapshot de conta broker");
                //ContaBrokerSyncMsg contaBrokerMsg = new ContaBrokerSyncMsg();
                //contaBrokerMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //contaBrokerMsg.ContaBroker = RiskCache.Instance.SnapshotContaBroker();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(contaBrokerMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(contaBrokerMsg);


                //logger.Info("Enviando snapshot de restricao de ativo");
                //RestrictionSymbolSyncMsg rstSymbolMsg = new RestrictionSymbolSyncMsg();
                //rstSymbolMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //rstSymbolMsg.RestrictionSymbol = RiskCache.Instance.SnapshotRestrictionSymbol();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(rstSymbolMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(rstSymbolMsg);

                //logger.Info("Enviando snapshot de restricao de grupo de ativos");
                //RestrictionGroupSymbolSyncMsg rstGroupSymbolMsg = new RestrictionGroupSymbolSyncMsg();
                //rstGroupSymbolMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //rstGroupSymbolMsg.RestrictionGroupSymbol = RiskCache.Instance.SnapshotRestrictionGroupSymbol();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(rstGroupSymbolMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(rstGroupSymbolMsg);

                //logger.Info("Enviando snapshot de restricao global");
                //RestrictionGlobalSyncMsg rstGlobalMsg = new RestrictionGlobalSyncMsg();
                //rstGlobalMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                //rstGlobalMsg.RestrictionGlobal = RiskCache.Instance.SnapshotRestrictionGlobal();
                //if (clientSocket != null)
                //    SpiderSocket.SendObject(rstGlobalMsg, clientSocket);
                //else
                //    _sckServer.SendToAll(rstGlobalMsg);

                logger.Info("Enviando snapshot de PositionClient");
                PositionClientSyncMsg posClientMsg = new PositionClientSyncMsg();
                posClientMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                posClientMsg.PositionClient = PositionClientManager.Instance.SnapshotPositionClient();
                if (clientSocket != null)
                    SpiderSocket.SendObject(posClientMsg, clientSocket);
                else
                    _sckServer.SendToAll(posClientMsg);


                logger.Info("Enviando snapshot de Risco Consolidado...");
                ConsolidatedRiskSyncMsg cRiskMsg = new ConsolidatedRiskSyncMsg();
                cRiskMsg.SyncAction = SyncMsgAction.SNAPSHOT;
                cRiskMsg.ConsolidatedRisk = ConsolidatedRiskManager.Instance.SnapshotConsolidatedRisk();
                if (clientSocket != null)
                    SpiderSocket.SendObject(posClientMsg, clientSocket);
                else
                    _sckServer.SendToAll(posClientMsg);


            }
            catch (Exception ex)
            {
                logger.Error("Problemas na geracao / envio do snapshot: " + ex.Message, ex);
            }
            finally
            {
                AcSpiderCache.Instance.SetMemorySnapshot(false);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Gradual.Spider.CommSocket;
using System.Reflection;
using log4net;
using Gradual.Spider.Communications.Lib.Mensagens;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Configuration;
using System.Net;

namespace Gradual.Spider.CommServer
{
    public class ProvidersManager
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ProvidersManager _me = null;
        private bool bKeepRunning = false;
        private Thread thMonitorProviders = null;

        public static ProvidersManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new ProvidersManager();
                }

                return _me;
            }
        }

        private ConcurrentDictionary<Type, string> dctTipos = new ConcurrentDictionary<Type, string>();
        private ConcurrentDictionary<string, SpiderSocket> dctProviders = new ConcurrentDictionary<string, SpiderSocket>();
        private ProviderManagerConfig _providerConfig;

        public void Start()
        {
            bKeepRunning = true;

            LoadConfig();

            LoadProviders();

            thMonitorProviders = new Thread(new ThreadStart(monitorProviders));
            thMonitorProviders.Start();
        }

        public void Stop()
        {
            bKeepRunning = false;

            while (thMonitorProviders.IsAlive)
            {
                Thread.Sleep(100);
            }

            foreach (SpiderSocket spidersk in dctProviders.Values)
            {
                try
                {
                    if (spidersk.IsConectado())
                    {
                        spidersk.CloseSocket();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Stop: " + ex.Message, ex);

                    spidersk.CloseSocket();
                }
            }
        }

        public void LoadConfig()
        {
            _providerConfig = Gradual.OMS.Library.GerenciadorConfig.ReceberConfig<ProviderManagerConfig>();

            foreach (ProviderDescription provider in _providerConfig.Providers)
            {
                string provkey = provider.ServiceIP + ":" + provider.ServicePort;

                SpiderSocket spidersk = new SpiderSocket();
                spidersk.IpAddr = provider.ServiceIP;
                spidersk.Port = provider.ServicePort;
                spidersk.AddHandler<PublishCommServerResponse>(new ProtoObjectReceivedHandler<PublishCommServerResponse>(publishResponse));
                spidersk.AddHandler<SondaCommServer>(new ProtoObjectReceivedHandler<SondaCommServer>(providerSonda));

                dctProviders.AddOrUpdate(provkey, spidersk, (key, oldvalue) => spidersk);
            }
        }

        private void providerSonda(object sender, int clientNumber, Socket clientSocket, SondaCommServer args)
        {
            string sAddress = IPAddress.Parse(((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString()).ToString();
            int iPort = ((IPEndPoint)clientSocket.RemoteEndPoint).Port;

            logger.Debug("Sonda from [" + clientNumber + "] [" + sAddress + ":" + iPort + "]");
        }

        private void publishResponse(object sender, int clientNumber, Socket clientSocket, PublishCommServerResponse args)
        {
            SubscriptionManager.Instance.Publish(args);
        }

        private void monitorProviders()
        {
            logger.Info("Iniciando thread de monitoracao dos providers");
            long lastSonda = 0;

            while (bKeepRunning)
            {
                TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastSonda);
                if (ts.TotalMilliseconds > 30000)
                {
                    lastSonda = DateTime.Now.Ticks;
                    foreach (SpiderSocket spidersk in dctProviders.Values)
                    {
                        try
                        {
                            if (!spidersk.IsConectado())
                            {
                                logger.Info("Conectando com provider [" + spidersk.IpAddr + ":" + spidersk.Port + "]");
                                spidersk.OpenConnection();
                                continue;
                            }

                            SondaCommServer sonda = new SondaCommServer();
                            spidersk.SendObject(sonda);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("monitorProviders: " + ex.Message, ex);

                            spidersk.CloseSocket();
                        }
                    }
                }

                Thread.Sleep(100);
            }

            logger.Info("Finalizando thread de monitoracao dos providers");
        }

        /// <summary>
        /// Associa o Type a um servico spider
        /// </summary>
        public void LoadProviders()
        {
            //if (ConfigurationManager.AppSettings["PathProviderDLL"] !=null )
            //{
            //    string diretorioDLLs = ConfigurationManager.AppSettings["PathProviderDLL"].ToString();
            //    string [] arquivos = Directory.GetFiles(diretorioDLLs, "*.dll", SearchOption.AllDirectories);

            //    foreach( string arquivo in arquivos )
            //    {
            //        logger.Info("Loading from [" + arquivo + "]");
            //        Assembly.LoadFrom(arquivo);
            //    }
            //}
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            AppDomain.CurrentDomain.AssemblyResolve += 
                new ResolveEventHandler(CurrentDomain_AssemblyResolve);
    

            foreach (ProviderDescription provider in _providerConfig.Providers)
            {
                foreach (string tipoprovider in provider.Tipo)
                {
                    logger.Info("Tentando carregar Type [" + tipoprovider + "]");
                    Type tipo = Type.GetType(tipoprovider);
                    if (tipo != null)
                    {
                        //TODO, acrescentar o tipo a lista de providers (que podem funcionar)
                        string provkey = provider.ServiceIP + ":" + provider.ServicePort;

                        logger.Info("Associando Classe [" + tipo.Name + "] com provider [" + provkey + "]");

                        dctTipos.AddOrUpdate(tipo, provkey, (key, oldValue) => provkey);
                    }
                    else
                    {
                        logger.Warn("NAO SERA POSSIVEL TRATAR [" + tipoprovider + "]");
                    }
                }
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error("CurrentDomain_UnhandledException: " + e.ExceptionObject.ToString());
        }


        public static Assembly CurrentDomain_AssemblyResolve(object sender, 
                                                      ResolveEventArgs args)
        {
            try
            {
                string diretorioDLLs = AppDomain.CurrentDomain.BaseDirectory;

                if (ConfigurationManager.AppSettings["PathProviderDLL"] != null)
                {
                    diretorioDLLs = ConfigurationManager.AppSettings["PathProviderDLL"].ToString();
                }

                string assemblyname = new AssemblyName(args.Name).Name;
                string assemblyFileName = Path.Combine(diretorioDLLs, assemblyname + ".dll");
                Assembly assembly = Assembly.LoadFrom(assemblyFileName);

                return assembly;
            }
            catch (Exception e)
            {
            }

            return null;

        }


        public void SendRequest(Type tipo, object objeto)
        {
            try
            {
                string provkey = null;

                if (dctTipos.TryGetValue(tipo, out provkey))
                {
                    SpiderSocket provider = null;
                    if (dctProviders.TryGetValue(provkey, out provider))
                    {
                        provider.SendObject(objeto);
                        return;
                    }
                }

                logger.Error("Nao ha provider mapeado para objeto tipo [" + tipo.ToString() + "]");
            }
            catch(Exception ex)
            {
                logger.Error("Erro em SendRequest(): " + ex.Message, ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using log4net;
using System.Threading;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Função semelhante ao do ativador mas apenas para o IServicoLocalizador.
    /// A rotina de ativação deste componente não consulta o localizador e sim 
    /// diretamente um arquivo de configuração.
    /// </summary>
    public static class LocalizadorCliente
    {
        private static IServicoLocalizador _servicoLocalizador = null;
        private static IChannelFactory<IServicoLocalizador> _canal = null;
        private static LocalizadorClienteConfig _config = null;
        private static long _lastRequest = 0;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Mutex _mtxLocalizador = null;
        
        // Best binding to acess the services
        public static string PreferedBinding {get;set;}

        public static void Inicializar(LocalizadorClienteConfig config)
        {
            _config = config;
        }

        public static IServicoLocalizador ServicoLocalizador
        {
            get 
            {
                try
                {
                    if (_servicoLocalizador == null || _canal.State != CommunicationState.Opened )
                        Conectar();
                    return _servicoLocalizador;
                }
                catch (Exception ex)
                {
                    logger.Error("Erro em ServicoLocalizador()", ex);
                    throw ex;
                }
            }
        }

        private static Mutex MutexLocalizador
        {
            get
            {
                try
                {
                    if (_mtxLocalizador == null)
                    {
                        _mtxLocalizador = new Mutex();
                    }
                    return _mtxLocalizador;
                }
                catch (Exception ex)
                {
                    logger.Error("Erro em MutexLocalizador()", ex);
                    throw ex;
                }
            }
        }

        public static void Conectar()
        {
            try
            {
                // Inicializa
                bool local = true;

                // Verifica como o servico deve ser criado
                if (_config == null)
                    _config = GerenciadorConfig.ReceberConfig<LocalizadorClienteConfig>();
                if (_config != null && _config.AtivacaoTipo == ServicoAtivacaoTipo.WCF)
                    local = false;

                // Cria o servico
                if (local)
                {
                    // Cria local
                    _servicoLocalizador =
                        (IServicoLocalizador)
                            ServicoHostColecao.Default.ReceberServico<IServicoLocalizador>();
                }
                else
                {
                    // Cria via wcf
                    string bindingType = _getBindingType(_config.EndPoint.Endereco);

                    Binding binding =
                        (Binding)
                            typeof(BasicHttpBinding).Assembly.CreateInstance(bindingType);
                    binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
                    binding.SendTimeout = new TimeSpan(0, 1, 0);
                    binding.OpenTimeout = new TimeSpan(0, 0, 30);
                    binding.CloseTimeout = new TimeSpan(0, 0, 30);

                    if (_config.EndPoint.Endereco.StartsWith("https://"))
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(
                            delegate(object sender,
                                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; });

                        ((BasicHttpBinding)binding).Security.Mode = BasicHttpSecurityMode.Transport;
                        ((BasicHttpBinding)binding).Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    }


                    _canal = new ChannelFactory<IServicoLocalizador>(binding);
                    _servicoLocalizador =
                        _canal.CreateChannel(
                            new EndpointAddress(
                                _config.EndPoint.Endereco));
                    _canal.Faulted += new EventHandler(_channelFaulted);

                    _lastRequest = DateTime.Now.Ticks;

                    ((IContextChannel)_servicoLocalizador).OperationTimeout = new TimeSpan(0, 10, 0);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em Conectar(): ", ex);
                throw ex;
            }
        }

        public static void Desconectar()
        {
            try
            {
                LocalizadorCliente.MutexLocalizador.WaitOne();

                if (_servicoLocalizador != null)
                    _servicoLocalizador = null;
                if (_canal != null)
                    _canal.Close();

                LocalizadorCliente.MutexLocalizador.ReleaseMutex();
            }
            catch (Exception ex)
            {
                logger.Error("Erro em Desconectar(): ", ex);
            }
        }

        public static ServicoInfo Consultar(Type servicoInterface)
        {
            ServicoInfo info = null;

            try
            {
                LocalizadorCliente.MutexLocalizador.WaitOne();

                info = LocalizadorCliente.ServicoLocalizador.Consultar(servicoInterface.FullName);

                LocalizadorCliente.MutexLocalizador.ReleaseMutex();

                Desconectar();

            }
            catch (Exception ex)
            {
                logger.Error("Erro em LocalizadorCliente.Consultar()", ex);
                _canal.Abort();
                throw ex;
            }

            return info;
        }

        public static ServicoInfo Consultar(Type servicoInterface, string id)
        {
            ServicoInfo info = null;

            try
            {
                LocalizadorCliente.MutexLocalizador.WaitOne();

                info = LocalizadorCliente.ServicoLocalizador.Consultar(
                        servicoInterface.FullName, id);

                LocalizadorCliente.MutexLocalizador.ReleaseMutex();

                Desconectar();
            }
            catch (Exception ex)
            {
                logger.Error("Erro em LocalizadorCliente.Consultar()", ex);
                _canal.Abort();
                throw ex;
            }

            return info;
        }


        public static void Registrar(ServicoInfo servico)
        {
            try
            {
                string servername = System.Environment.MachineName;

                LocalizadorCliente.ServicoLocalizador.Registrar(servico);

                Desconectar();
            }
            catch (Exception ex)
            {
                logger.Error("Erro em LocalizadorCliente.Registrar()", ex);
                _canal.Abort();
                throw ex;
            }
        }

        public static void Remover(Type servicoInterface)
        {
            try
            {
                LocalizadorCliente.ServicoLocalizador.Remover(servicoInterface.FullName);

                Desconectar();
            }
            catch (Exception ex)
            {
                logger.Error("Erro em LocalizadorCliente.Remover(" + servicoInterface.FullName + ")", ex);
                _canal.Abort();
                throw ex;
            }
        }

        public static void Remover(Type servicoInterface, string id)
        {
            try
            {
                LocalizadorCliente.ServicoLocalizador.Remover(servicoInterface.FullName, id);

                Desconectar();
            }
            catch (Exception ex)
            {
                logger.Error("Erro em LocalizadorCliente.Remover(" + servicoInterface.FullName + ")", ex);
                _canal.Abort();
                throw ex;
            }
        }

        public static List<ServicoInfo> ListarServicos()
        {
            try
            {
                return  LocalizadorCliente.ServicoLocalizador.Consultar();

                Desconectar();
            }
            catch (Exception ex)
            {
                logger.Error("Erro em LocalizadorCliente.Consultar()", ex);
                _canal.Abort();
                throw ex;
            }
        }

        private static string _getBindingType(string address)
        {
            PreferedBinding = "net.tcp";

            if (address.StartsWith("net.pipe://"))
            {
                PreferedBinding = "net.pipe";
                return "System.ServiceModel.NetNamedPipeBinding";
            }

            if (address.StartsWith("http://"))
            {
                PreferedBinding = "http:";
                return "System.ServiceModel.BasicHttpBinding";
            }

            if (address.StartsWith("https://"))
            {
                PreferedBinding = "https:";
                return "System.ServiceModel.BasicHttpBinding";
            }

            return "System.ServiceModel.NetTcpBinding";
        }

        private static void _channelFaulted(object sender, EventArgs args)
        {
            logger.Error("Channel faulted state for " + sender.ToString());
        }

    }
}

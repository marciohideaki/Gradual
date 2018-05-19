using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Reflection;
using System.Text;
using log4net;
using System.Collections;
using System.ServiceModel.Description;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Utilizado pelos programas clientes para receber instancias de serviços.
    /// Utiliza o localizador para receber serviços externos.
    /// </summary>
    public static class Ativador
    {
        private static AtivadorConfig _config = null;
        private static ServicoHostConfig _configServicoHost = null;
        private static Dictionary<Type, ServicoInfo> _cache = new Dictionary<Type, ServicoInfo>();
        private static Hashtable _clientChannels = new Hashtable();
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Inicializar(AtivadorConfig config)
        {
            _config = config;
        }

        public static object Get(Type tipo)
        {
            // Transforma o tipo na chamada com generics
            MethodInfo metodo = 
                typeof(Ativador).GetMethod(
                    "Get", 
                    new Type[] { }, 
                    (ParameterModifier[]) null).MakeGenericMethod(tipo);

            // Faz a chamada
            return metodo.Invoke(null, null);
        }

        public static T Get<T>()
        {
            return Get<T>(null, null);
        }

        public static T Get<T>(ICallbackEvento callback, object parametros)
        {
            try
            {
                logger.Info("Ativador.Get<" + typeof(T).ToString() + ">()");

                // Pega config
                if (_config == null)
                    _config = GerenciadorConfig.ReceberConfig<AtivadorConfig>();

                // Pega config do host, caso exista
                if (_configServicoHost == null)
                    if (ServicoHostColecao.Default.IdConfigCarregado != null)
                        _configServicoHost = GerenciadorConfig.ReceberConfig<ServicoHostConfig>(
                            ServicoHostColecao.Default.IdConfigCarregado);

                // Inicializa
                T servico = default(T);
                Type tipo = typeof(T);
                ServicoInfo servicoInfo = null;

                // Verifica se tem no cache
                bool adicionarNoCache = false;
                lock (_cache)
                {
                    if (_cache.ContainsKey(typeof(T)))
                        servicoInfo = _cache[typeof(T)];
                    else
                        adicionarNoCache = true;
                }

                // Verifica se tem na lista interna
                if (servicoInfo == null && _config != null)
                {
                    logger.Info("Buscando informacoes do servico " + tipo.FullName + " na lista interna");
                    servicoInfo = _findServicoInfo(tipo.FullName, _config.Servicos);
                }

                // Verifica se tem na lista de servicos do host
                if (servicoInfo == null && _configServicoHost != null)
                {
                    logger.Info("Buscando informacoes do servico " + tipo.FullName + " na lista de servicos do host");
                    servicoInfo = _findServicoInfo(tipo.FullName, _configServicoHost.Servicos);
                }

                // Se ainda não achou, pega serviço info no localizador
                if (servicoInfo == null)
                {
                    logger.Info("Buscando informacoes do servico " + tipo.FullName + " no localizador");
                    servicoInfo = LocalizadorCliente.Consultar(typeof(T));
                }

                // Se até este ponto não achou o serviçoInfo, dispara erro
                if (servicoInfo == null)
                {
                    logger.Error("Não foi possível conseguir informações para a ativação do serviço " + tipo.FullName );

                    throw new Exception("Não foi possível conseguir informações para a ativação do serviço " + tipo.FullName + ".");
                }
                
                // Verifica se deve adicionar no cache
                if (adicionarNoCache && servicoInfo != null)
                {
                    lock (_cache)
                    {
                        if ( !_cache.ContainsKey(typeof(T)) )
                        {
                            logger.Info("Armazenando informacoes do servico " + tipo.FullName + " no cache");
                            _cache.Add(typeof(T), servicoInfo);
                        }
                    }
                }

                // Primeira tentativa deve ser criação local?
                if (servicoInfo.AtivacaoDefaultTipo == ServicoAtivacaoTipo.Local)
                {
                    logger.Debug("Ativando " + tipo.FullName + " localmente");

                    // Cria o serviço
                    servico = ServicoHostColecao.Default.ReceberServico<T>();

                    // Caso seja servico com callback, faz a chamada do registro
                    IServicoComCallback servicoComCallback = servico as IServicoComCallback;
                    if (servicoComCallback != null && callback != null)
                        servicoComCallback.Registrar(parametros, callback);
                }

                if (servico == null && servicoInfo.EndPoints.Count > 0)
                {
                    ContractDescription cd;
                    logger.Debug("Ativando " + tipo.FullName + " como WCF");

                    string epaddress = _findEndpointForInterface<T>(servicoInfo);
                    
                    logger.Debug("Criando Binding para Endpoint: " + epaddress);

                    // Cria via wcf
                    Binding binding = Utilities.GetBinding(epaddress);
                        //(Binding)
                        //    typeof(BasicHttpBinding).Assembly.CreateInstance(
                        //        servicoInfo.EndPoints[0].NomeBindingType);
                    binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
                    binding.SendTimeout = new TimeSpan(0, 1, 0);
                    binding.OpenTimeout = new TimeSpan(0, 0, 30);
                    binding.CloseTimeout = new TimeSpan(0, 0, 30);
                    if (servicoInfo.EndPoints[0].NomeBindingType.Equals("System.ServiceModel.NetTcpBinding"))
                    {
                        ((NetTcpBinding)binding).MaxReceivedMessageSize = 8000000;
                        ((NetTcpBinding)binding).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                        //((NetTcpBinding)binding).Security.Mode = SecurityMode.None;
                    }

                    if (servicoInfo.EndPoints[0].NomeBindingType.Equals("System.ServiceModel.BasicHttpBinding"))
                    {
                        logger.Debug("Binding setado BasicHttpBinding , verificando por callback");

                        ((BasicHttpBinding)binding).MaxReceivedMessageSize = int.MaxValue;
                        ((BasicHttpBinding)binding).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                        //((NetTcpBinding)binding).Security.Mode = SecurityMode.None;
                    }



                    logger.Debug("Binding criado, verificando por callback");

                    // Verifica se tem callback
                    // Mesmo que não tenha informado, tem que ver se tem contrato especificado na interface
                    bool temCallback = callback != null;
                    if (!temCallback)
                    {
                        object[] attrs = typeof(T).GetCustomAttributes(typeof(ServiceContractAttribute), true);
                        if (attrs.Length == 1)
                            if (((ServiceContractAttribute)attrs[0]).CallbackContract != null)
                                temCallback = true;
                    }

                    // Cria dependendo se tem contrato ou não
                    if (!temCallback)
                    {
                        logger.Debug("Servico " + tipo.FullName + " nao tem callback, criando channel ");

                        IChannelFactory<T> canal = new ChannelFactory<T>(binding);
                        canal.Faulted += new EventHandler(_channelFaulted);
                        string uri = _findEndpointForInterface<T>(servicoInfo);
                        if (uri == null)
                        {
                            string msg = "Servico [" + tipo.FullName + "] nao pôde ser ativado.\n";
                            msg += "Verifique se existe <ServicoEndPointInfo> nas configuracoes locais,\n";
                            msg += "ou se um endpoint foi criado para o mesmo e registrado no ServicoLocalizador.";

                            logger.Error("ERRO: NENHUM ENDERECO DE ENDPOINT PARA SERVICO [" + tipo.FullName + "]!");
                            throw new Exception(msg);
                        }

                        servico = canal.CreateChannel(new EndpointAddress(uri));

                        cd = ((ChannelFactory)canal).Endpoint.Contract;
                    }
                    else
                    {
                        logger.Debug("Servico " + tipo.FullName + " tem callback, criando channel duplex");

                        DuplexChannelFactory<T> canal = null;
                        canal.Faulted += new EventHandler(_channelFaulted);
                        if (callback == null)
                        {
                            logger.Error("Contratos que recebem callbacks tem necessariamente que receber um objeto de callback.");

                            throw new Exception("Contratos que recebem callbacks tem necessariamente que receber um objeto de callback.");
                        }

                        canal = new DuplexChannelFactory<T>(new InstanceContext(callback), binding);
                        string uri = _findEndpointForInterface<T>(servicoInfo);
                        if (uri == null)
                        {
                            string msg = "Servico [" + tipo.FullName + "] nao pôde ser ativado.\n";
                            msg += "Verifique se existe <ServicoEndPointInfo> nas configuracoes locais,\n";
                            msg += "ou se um endpoint foi criado para o mesmo e registrado no ServicoLocalizador.";

                            logger.Error("ERRO: NENHUM ENDERECO DE ENDPOINT PARA SERVICO [" + tipo.FullName + "]!");
                            throw new Exception(msg);
                        }
                        servico = canal.CreateChannel(new EndpointAddress(uri));

                        cd = canal.Endpoint.Contract;


                        IServicoComCallback servicoComCallback = servico as IServicoComCallback;
                        if (servicoComCallback != null)
                            servicoComCallback.Registrar(parametros);
                    }

                    if (cd != null)
                    {
                        foreach (OperationDescription od in cd.Operations)
                        {
                            DataContractSerializerOperationBehavior serializerBh = od.Behaviors.Find<DataContractSerializerOperationBehavior>();
                            if (serializerBh == null)
                            {
                                logger.Info("Adicionando DataContractSerializerOperationBehavior");
                                serializerBh = new DataContractSerializerOperationBehavior(od);
                                od.Behaviors.Add(serializerBh);
                            }

                            logger.Info("Setando MaxItemsInObjectGraph para operacao: " + od.Name);
                            serializerBh.MaxItemsInObjectGraph = 8000000;
                        }
                    }
                    ((IContextChannel)servico).OperationTimeout = new TimeSpan(0, 10, 0);

                }

                if (servico == null)
                {
                    string msg = "Servico [" + tipo.FullName + "] nao pôde ser ativado.\n";
                    msg += "Verifique se existe <servicoinfo> nas configuracoes locais,\n";
                    msg += "ou se o mesmo foi registrado no ServicoLocalizador e seu hoster esta ativo.";

                    logger.Error("ERRO: SERVICO [" + tipo.FullName + "] NAO FOI ATIVADO!!!");
                    throw new Exception(msg);
                }

                // Retorna
                return servico;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em Ativador.Get(" + typeof(T).FullName + ")", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Retorna uma instancia do servico correspondente ao 
        /// </summary>
        /// <typeparam name="T">Class Type a ser instanciada</typeparam>
        /// <param name="callbackImpl">objeto que implementa o tratamento dos callbacks</param>
        /// <returns>Nova instancia de T</returns>
        public static T Get<T>(object callbackImpl)
        {
            try
            {
                logger.Info("Ativador.Get<" + typeof(T).ToString() + ">(" + callbackImpl.GetType() + ")");

                // Pega config
                if (_config == null)
                    _config = GerenciadorConfig.ReceberConfig<AtivadorConfig>();

                // Pega config do host, caso exista
                if (_configServicoHost == null)
                    if (ServicoHostColecao.Default.IdConfigCarregado != null)
                        _configServicoHost = GerenciadorConfig.ReceberConfig<ServicoHostConfig>(
                            ServicoHostColecao.Default.IdConfigCarregado);

                // Inicializa
                T servico = default(T);
                Type tipo = typeof(T);
                ServicoInfo servicoInfo = null;

                // Verifica se tem no cache
                bool adicionarNoCache = false;
                lock (_cache)
                {
                    if (_cache.ContainsKey(typeof(T)))
                        servicoInfo = _cache[typeof(T)];
                    else
                        adicionarNoCache = true;
                }

                // Verifica se tem na lista interna
                if (servicoInfo == null && _config != null)
                {
                    logger.Info("Buscando informacoes do servico " + tipo.FullName + " na lista interna");
                    servicoInfo = _findServicoInfo(tipo.FullName, _config.Servicos);
                }

                // Verifica se tem na lista de servicos do host
                if (servicoInfo == null && _configServicoHost != null)
                {
                    logger.Info("Buscando informacoes do servico " + tipo.FullName + " na lista de servicos do host");
                    servicoInfo = _findServicoInfo(tipo.FullName, _configServicoHost.Servicos);
                }

                // Se ainda não achou, pega serviço info no localizador
                if (servicoInfo == null)
                {
                    logger.Info("Buscando informacoes do servico " + tipo.FullName + " no localizador");
                    servicoInfo = LocalizadorCliente.Consultar(typeof(T));
                }

                // Se até este ponto não achou o serviçoInfo, dispara erro
                if (servicoInfo == null)
                {
                    logger.Error("Não foi possível conseguir informações para a ativação do serviço " + tipo.FullName);

                    throw new Exception("Não foi possível conseguir informações para a ativação do serviço " + tipo.FullName + ".");
                }

                // Verifica se deve adicionar no cache
                if (adicionarNoCache && servicoInfo != null)
                {
                    lock (_cache)
                    {
                        if ( !_cache.ContainsKey(typeof(T)) )
                        {
                            logger.Info("Armazenando informacoes do servico " + tipo.FullName + " no cache");
                            _cache.Add(typeof(T), servicoInfo);
                        }
                    }
                }

                // Primeira tentativa deve ser criação local?
                if (servicoInfo.AtivacaoDefaultTipo == ServicoAtivacaoTipo.Local)
                {
                    logger.Debug("Ativando " + tipo.FullName + " localmente");

                    // Cria o serviço
                    servico = ServicoHostColecao.Default.ReceberServico<T>();

                    // Caso seja servico com callback, faz a chamada do registro
                    //IServicoComCallback servicoComCallback = servico as IServicoComCallback;
                    //if (servicoComCallback != null && callback != null)
                    //    servicoComCallback.Registrar(parametros, callback);
                }

                if (servico == null && servicoInfo.EndPoints.Count > 0)
                {
                    ContractDescription cd;
                    logger.Debug("Ativando " + tipo.FullName + " como WCF");

                    string epaddress = _findEndpointForInterface<T>(servicoInfo);

                    logger.Debug("Criando Binding para Endpoint: " + epaddress);

                    // Cria via wcf
                    Binding binding = Utilities.GetBinding(epaddress);
                    //(Binding)
                    //    typeof(BasicHttpBinding).Assembly.CreateInstance(
                    //        servicoInfo.EndPoints[0].NomeBindingType);
                    binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
                    binding.SendTimeout = new TimeSpan(0, 1, 0);
                    binding.OpenTimeout = new TimeSpan(0, 0, 30);
                    binding.CloseTimeout = new TimeSpan(0, 0, 30);
                    
                    if (servicoInfo.EndPoints[0].NomeBindingType.Equals("System.ServiceModel.NetTcpBinding"))
                    {
                        ((NetTcpBinding)binding).MaxReceivedMessageSize = 8000000;
                        ((NetTcpBinding)binding).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                        //((NetTcpBinding)binding).Security.Mode = SecurityMode.None;
                    }

                    if (servicoInfo.EndPoints[0].NomeBindingType.Equals("System.ServiceModel.BasicHttpBinding"))
                    {
                        logger.Debug("Binding setado BasicHttpBinding , verificando por callback");

                        ((BasicHttpBinding)binding).MaxReceivedMessageSize = int.MaxValue;
                        ((BasicHttpBinding)binding).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                        //((NetTcpBinding)binding).Security.Mode = SecurityMode.None;
                    }

                    logger.Debug("Binding criado, verificando por callback");

                    // Verifica se tem callback
                    // Mesmo que não tenha informado, tem que ver se tem contrato especificado na interface
                    bool temCallback = callbackImpl != null;
                    if (!temCallback)
                    {
                        object[] attrs = typeof(T).GetCustomAttributes(typeof(ServiceContractAttribute), true);
                        if (attrs.Length == 1)
                            if (((ServiceContractAttribute)attrs[0]).CallbackContract != null)
                                temCallback = true;
                    }

                    // Cria dependendo se tem contrato ou não
                    if (!temCallback)
                    {
                        logger.Debug("Servico " + tipo.FullName + " nao tem callback, criando channel ");

                        IChannelFactory<T> canal = new ChannelFactory<T>(binding);
                        canal.Faulted += new EventHandler(_channelFaulted);
                        string uri = _findEndpointForInterface<T>(servicoInfo);
                        if (uri == null)
                        {
                            string msg = "Servico [" + tipo.FullName + "] nao pôde ser ativado.\n";
                            msg += "Verifique se existe <ServicoEndPointInfo> nas configuracoes locais,\n";
                            msg += "ou se um endpoint foi criado para o mesmo e registrado no ServicoLocalizador.";

                            logger.Error("ERRO: NENHUM ENDERECO DE ENDPOINT PARA SERVICO [" + tipo.FullName + "]!");
                            throw new Exception(msg);
                        }
                        servico = canal.CreateChannel(new EndpointAddress(uri));

                        cd = ((ChannelFactory)canal).Endpoint.Contract;
                    }
                    else
                    {
                        logger.Debug("Servico " + tipo.FullName + " tem callback, criando channel duplex");

                        DuplexChannelFactory<T> canal = null;

                        if (callbackImpl == null)
                        {
                            logger.Error("Contratos que recebem callbacks tem necessariamente que receber um objeto de callback.");

                            throw new Exception("Contratos que recebem callbacks tem necessariamente que receber um objeto de callback.");
                        }

                        canal = new DuplexChannelFactory<T>(callbackImpl, binding);
                        canal.Faulted += new EventHandler(_channelFaulted);
                        string uri = _findEndpointForInterface<T>(servicoInfo);
                        if (uri == null)
                        {
                            string msg = "Servico [" + tipo.FullName + "] nao pôde ser ativado.\n";
                            msg += "Verifique se existe <ServicoEndPointInfo> nas configuracoes locais,\n";
                            msg += "ou se um endpoint foi criado para o mesmo e registrado no ServicoLocalizador.";

                            logger.Error("ERRO: NENHUM ENDERECO DE ENDPOINT PARA SERVICO [" + tipo.FullName + "]!");
                            throw new Exception(msg);
                        }
                        servico = canal.CreateChannel(new EndpointAddress(uri));
                        cd = canal.Endpoint.Contract;

                        //IServicoComCallback servicoComCallback = servico as IServicoComCallback;
                        //if (servicoComCallback != null)
                        //    servicoComCallback.Registrar(parametros);
                    }

                    if (cd != null)
                    {
                        foreach (OperationDescription od in cd.Operations)
                        {
                            DataContractSerializerOperationBehavior serializerBh = od.Behaviors.Find<DataContractSerializerOperationBehavior>();
                            if (serializerBh == null)
                            {
                                logger.Info("Adicionando DataContractSerializerOperationBehavior");
                                serializerBh = new DataContractSerializerOperationBehavior(od);
                                od.Behaviors.Add(serializerBh);
                            }

                            logger.Info("Setando MaxItemsInObjectGraph para operacao: " + od.Name);
                            serializerBh.MaxItemsInObjectGraph = 8000000;
                        }
                    }

                    ((IContextChannel)servico).OperationTimeout = new TimeSpan(0, 10, 0);
                }

                if (servico == null)
                {
                    string msg = "Servico [" + tipo.FullName + "] nao pôde ser ativado.\n";
                    msg += "Verifique se existe <servicoinfo> nas configuracoes locais,\n";
                    msg += "ou se o mesmo foi registrado no ServicoLocalizador e seu hoster esta ativo.";

                    logger.Error("ERRO: SERVICO [" + tipo.FullName + "] NAO FOI ATIVADO!!!");
                    throw new Exception(msg);
                }

                // Retorna
                return servico;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em Ativador.Get(" + typeof(T).FullName + ")", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Retorna uma instancia do servico correspondente ao tipo
        /// no endereco fornecido
        /// </summary>
        /// <typeparam name="T">Class Type a ser instanciada</typeparam>
        /// <param name="url">Url completa do Endpoint</param>
        /// <param name="ativacaolocal">Flag indicando se a ativacao sera local ou remota (via wcf)</param>
        /// <param name="callbackImpl">objeto que implementa o tratamento de callbacks</param>
        /// <returns>Nova instancia de T</returns>
        public static T GetByAddr<T>( string url, bool ativacaolocal=false, object callbackImpl=null)
        {
            try
            {
                if (callbackImpl != null)
                {
                    logger.Info("Ativador.GetByAddr<" + typeof(T).ToString() + ">(" + callbackImpl.GetType().ToString() + "," +url+ ","+ativacaolocal.ToString() +")");
                }
                else
                {
                    logger.Info("Ativador.GetByAddr<" + typeof(T).ToString() + ">(null," +url+ ","+ativacaolocal.ToString() +")");
                }

                // Inicializa
                T servico = default(T);
                Type tipo = typeof(T);
//                ServicoInfo servicoInfo = new ServicoInfo();

//                servicoInfo.AtivacaoDefaultTipo = ServicoAtivacaoTipo.WCF;
//                if (ativacaolocal)
//                {
//                    servicoInfo.AtivacaoDefaultTipo = ServicoAtivacaoTipo.Local;
//                }
//                servicoInfo.AtivarWCF = true;
////                servicoInfo.
//                ServicoEndPointInfo endpoint = new ServicoEndPointInfo();
//                endpoint.Endereco = url;
//                endpoint.NomeBindingType = Utilities.GetBindingType(url);

//                servicoInfo.N



                    // Primeira tentativa deve ser criação local?
                if (ativacaolocal)
                {
                    logger.Debug("Ativando " + tipo.FullName + " localmente");

                    // Cria o serviço
                    servico = ServicoHostColecao.Default.ReceberServico<T>();
                }

                if (servico == null )
                {
                    ContractDescription cd;
                    logger.Debug("Ativando " + tipo.FullName + " como WCF");

                    logger.Debug("Criando Binding para Endpoint: " + url);

                    // Cria via wcf
                    Binding binding = Utilities.GetBinding(url);
                    //(Binding)
                    //    typeof(BasicHttpBinding).Assembly.CreateInstance(
                    //        servicoInfo.EndPoints[0].NomeBindingType);
                    binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
                    binding.SendTimeout = new TimeSpan(0, 1, 0);
                    binding.OpenTimeout = new TimeSpan(0, 0, 30);
                    binding.CloseTimeout = new TimeSpan(0, 0, 30);
                    string bindtype = Utilities.GetBindingType(url);
                    
                    if (bindtype.Equals("System.ServiceModel.NetTcpBinding"))
                    {
                        ((NetTcpBinding)binding).MaxReceivedMessageSize = 8000000;
                        ((NetTcpBinding)binding).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                        //((NetTcpBinding)binding).Security.Mode = SecurityMode.None;
                    }

                    if (bindtype.Equals("System.ServiceModel.BasicHttpBinding"))
                    {
                        logger.Debug("Binding setado BasicHttpBinding , verificando por callback");

                        ((BasicHttpBinding)binding).MaxReceivedMessageSize = int.MaxValue;
                        ((BasicHttpBinding)binding).ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                        //((NetTcpBinding)binding).Security.Mode = SecurityMode.None;
                    }

                    logger.Debug("Binding criado, verificando por callback");

                    // Verifica se tem callback
                    // Mesmo que não tenha informado, tem que ver se tem contrato especificado na interface
                    bool temCallback = callbackImpl != null;
                    if (!temCallback)
                    {
                        object[] attrs = typeof(T).GetCustomAttributes(typeof(ServiceContractAttribute), true);
                        if (attrs.Length == 1)
                            if (((ServiceContractAttribute)attrs[0]).CallbackContract != null)
                                temCallback = true;
                    }

                    // Cria dependendo se tem contrato ou não
                    if (!temCallback)
                    {
                        logger.Debug("Servico " + tipo.FullName + " nao tem callback, criando channel ");

                        IChannelFactory<T> canal = new ChannelFactory<T>(binding);
                        canal.Faulted += new EventHandler(_channelFaulted);

                        servico = canal.CreateChannel(new EndpointAddress(url));

                        cd = ((ChannelFactory)canal).Endpoint.Contract;
                    }
                    else
                    {
                        logger.Debug("Servico " + tipo.FullName + " tem callback, criando channel duplex");

                        DuplexChannelFactory<T> canal = null;

                        if (callbackImpl == null)
                        {
                            logger.Error("Contratos que recebem callbacks tem necessariamente que receber um objeto de callback.");

                            throw new Exception("Contratos que recebem callbacks tem necessariamente que receber um objeto de callback.");
                        }

                        canal = new DuplexChannelFactory<T>(callbackImpl, binding);
                        canal.Faulted += new EventHandler(_channelFaulted);
                        servico = canal.CreateChannel(new EndpointAddress(url));
                        cd = canal.Endpoint.Contract;

                        //IServicoComCallback servicoComCallback = servico as IServicoComCallback;
                        //if (servicoComCallback != null)
                        //    servicoComCallback.Registrar(parametros);
                    }

                    if (cd != null)
                    {
                        foreach (OperationDescription od in cd.Operations)
                        {
                            DataContractSerializerOperationBehavior serializerBh = od.Behaviors.Find<DataContractSerializerOperationBehavior>();
                            if (serializerBh == null)
                            {
                                logger.Info("Adicionando DataContractSerializerOperationBehavior");
                                serializerBh = new DataContractSerializerOperationBehavior(od);
                                od.Behaviors.Add(serializerBh);
                            }

                            logger.Info("Setando MaxItemsInObjectGraph para operacao: " + od.Name);
                            serializerBh.MaxItemsInObjectGraph = 8000000;
                        }
                    }

                    ((IContextChannel)servico).OperationTimeout = new TimeSpan(0, 10, 0);
                }

                if (servico == null)
                {
                    string msg = "Servico [" + tipo.FullName + "] nao pôde ser ativado.\n";
                    msg += "Verifique se existe <servicoinfo> nas configuracoes locais,\n";
                    msg += "ou se o mesmo foi registrado no ServicoLocalizador e seu hoster esta ativo.";

                    logger.Error("ERRO: SERVICO [" + tipo.FullName + "] NAO FOI ATIVADO!!!");
                    throw new Exception(msg);
                }

                // Retorna
                return servico;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em Ativador.Get(" + typeof(T).FullName + ")", ex);
                throw ex;
            }
        }

        public static string GetInfo()
        {
            try
            {
                return System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em Ativador.GetInfo()", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Retorna o objeto de callback a ser invocado pelo servico
        /// </summary>
        /// <typeparam name="T">Interface que define o callback</typeparam>
        /// <returns>Instancia do tipo [Interface]</returns>
        public static T GetCallback<T>()
        {
            T callback = OperationContext.Current.GetCallbackChannel<T>();
            IContextChannel ctxChannel = OperationContext.Current.Channel;

            lock (_clientChannels)
            {
                if (_clientChannels.ContainsKey(callback))
                    _clientChannels[callback] = ctxChannel;
                else
                    _clientChannels.Add(callback, ctxChannel);
            }

            return callback;
        }

        /// <summary>
        /// Valida se canal de callback eh valido
        /// </summary>
        /// <param name="callback">instancia do callback, retornada em GetCallback[T}()</param>
        /// <returns>true, se o ChannelState = Opened </returns>
        public static bool IsValidChannel(object callback)
        {
            bool bRet = false;

            if (callback != null)
            {
                lock (_clientChannels)
                {
                    if (_clientChannels.ContainsKey(callback))
                    {
                        IContextChannel channel = (IContextChannel)_clientChannels[callback];
                        if (channel.State == CommunicationState.Opened)
                        {
                            bRet = true;
                        }
                        else
                        {
                            channel.Abort();
                            _clientChannels.Remove(callback);
                            return false;
                        }
                    }
                }
            }

            return bRet;
        }

        /// <summary>
        /// Fecha um wcf channel associado a uma instancia de servico
        /// </summary>
        /// <param name="servico">instancia de um objeto criado anteriormente com Ativador.Get()</param>
        public static void AbortChannel(object servico)
        {
            try
            {
                if (servico == null)
                {
                    logger.Error("Erro em Ativador.AbortChannel(): Parametro nao pode ser nulo");
                    return;
                }

                IDuplexChannel channel2 = servico as IDuplexChannel;
                if (channel2 != null)
                {
                    logger.Info("Abort duplex channel for: " + servico.ToString());
                    channel2.Close();
                }
                else
                {
                    IChannel channel = servico as IChannel;
                    if (channel != null)
                    {
                        logger.Info("Abort channel for: " + servico.ToString());
                        try
                        {
                            channel.Close();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Erro em Ativador.AbortChannel()" + ex.Message, ex);
                            logger.Error("Trying really abort the channel");
                            channel.Abort();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("Erro em Ativador.AbortChannel()" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca ServicoInfo correspondente a um nome de interface
        /// </summary>
        /// <param name="interfaceName"></param>
        /// <param name="listaservico"></param>
        /// <returns>objeto ServicoInfo ou nulo, se nao encontrado</returns>
        private static ServicoInfo _findServicoInfo(string interfaceName, List<ServicoInfo> listaservico)
        {
            ServicoInfo servicoInfo = null;

            foreach (ServicoInfo info in listaservico)
            {
                bool bFound = false;

                foreach (string Interface in info.NomeInterface)
                {
                    if (Interface.StartsWith(interfaceName))
                    {
                        servicoInfo = info;
                        break;
                    }
                }

                if (bFound) break;
            }

            return servicoInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="servicoinfo"></param>
        /// <returns></returns>
        private static string _findEndpointForInterface<T>(ServicoInfo servicoinfo)
        {
            string ret = null;

            // Efetua 2 buscas, se nao achar o protocolo preferencial, retorna 
            // o primeiro endpoint disponivel
            if (LocalizadorCliente.PreferedBinding != null && LocalizadorCliente.PreferedBinding.Length > 0)
            {
                foreach (ServicoEndPointInfo endpoint in servicoinfo.EndPoints)
                {
                    if (endpoint.Endereco.Contains(typeof(T).ToString()) &&
                        endpoint.Endereco.StartsWith(LocalizadorCliente.PreferedBinding))
                    {
                        ret = endpoint.Endereco;
                    }
                }
            }

            if (ret == null)
            {
                foreach (ServicoEndPointInfo endpoint in servicoinfo.EndPoints)
                {
                    if (endpoint.Endereco.Contains(typeof(T).ToString()) )
                    {
                        ret = endpoint.Endereco;
                    }
                }
            }

            return ret;
        }

        private static void _channelFaulted(object sender, EventArgs args)
        {
            logger.Error("Channel faulted state for " + sender.ToString());
        }
    }
}

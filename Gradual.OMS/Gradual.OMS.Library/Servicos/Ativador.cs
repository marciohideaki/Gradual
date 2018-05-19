using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Reflection;
using System.Text;

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
                if (_cache.ContainsKey(typeof(T)))
                    servicoInfo = _cache[typeof(T)];
                else
                    adicionarNoCache = true;

                // Verifica se tem na lista interna
                if (servicoInfo == null && _config != null)
                    servicoInfo = (from s in _config.Servicos
                                   where s.NomeInterface.StartsWith(tipo.FullName)
                                   select s).FirstOrDefault();

                // Verifica se tem na lista de servicos do host
                if (servicoInfo == null && _configServicoHost != null)
                    servicoInfo = (from s in _configServicoHost.Servicos
                                   where s.NomeInterface.StartsWith(tipo.FullName)
                                   select s).FirstOrDefault();

                // Se ainda não achou, pega serviço info no localizador
                if (servicoInfo == null)
                    servicoInfo = LocalizadorCliente.Consultar(typeof(T));

                // Se até este ponto não achou o serviçoInfo, dispara erro
                if (servicoInfo == null)
                    throw new Exception("Não foi possível conseguir informações para a ativação do serviço " + tipo.FullName + ".");
                
                // Verifica se deve adicionar no cache
                if (adicionarNoCache && servicoInfo != null)
                    _cache.Add(typeof(T), servicoInfo);

                // Primeira tentativa deve ser criação local?
                if (servicoInfo.AtivacaoDefaultTipo == ServicoAtivacaoTipo.Local)
                {
                    // Cria o serviço
                    servico = ServicoHostColecao.Default.ReceberServico<T>();

                    // Caso seja servico com callback, faz a chamada do registro
                    IServicoComCallback servicoComCallback = servico as IServicoComCallback;
                    if (servicoComCallback != null && callback != null)
                        servicoComCallback.Registrar(parametros, callback);
                }

                if (servico == null && servicoInfo.EndPoints.Count > 0)
                {
                    // Cria via wcf
                    Binding binding =
                        (Binding)
                            typeof(BasicHttpBinding).Assembly.CreateInstance(
                                servicoInfo.EndPoints[0].NomeBindingType);
                    binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
                    binding.SendTimeout = new TimeSpan(0, 1, 0);
                    binding.OpenTimeout = new TimeSpan(0, 0, 30);
                    binding.CloseTimeout = new TimeSpan(0, 0, 30);
                    ((NetTcpBinding)binding).MaxReceivedMessageSize = 8000000;

                    // ATP: nao eh "tem contrato", e sim "tem callback"
                    // ATP: considerar isso em todo o trecho abaixo.

                    // Verifica se tem contrato
                    // Mesmo que não tenha informado, tem que ver se tem contrato especificado na interface
                    bool temContrato = callback != null;
                    if (!temContrato)
                    {
                        object[] attrs = typeof(T).GetCustomAttributes(typeof(ServiceContractAttribute), true);
                        if (attrs.Length == 1)
                            if (((ServiceContractAttribute)attrs[0]).CallbackContract != null)
                                temContrato = true;
                    }

                    // Cria dependendo se tem contrato ou não
                    if (!temContrato)
                    {
                        IChannelFactory<T> canal = new ChannelFactory<T>(binding);
                        servico =
                            canal.CreateChannel(
                                new EndpointAddress(
                                    servicoInfo.EndPoints[0].Endereco));
                    }
                    else
                    {
                        DuplexChannelFactory<T> canal = null;
                        if (callback != null)
                            canal = new DuplexChannelFactory<T>(new InstanceContext(callback), binding);
                        else
                            throw new Exception("Contratos que recebem callbacks tem necessariamente que receber um objeto de callback.");
                        servico =
                            canal.CreateChannel(
                                new EndpointAddress(
                                    servicoInfo.EndPoints[0].Endereco));

                        IServicoComCallback servicoComCallback = servico as IServicoComCallback;
                        if (servicoComCallback != null)
                            servicoComCallback.Registrar(parametros);
                    }

                    ((IContextChannel)servico).OperationTimeout = new TimeSpan(0, 10, 0);
                }

                // Retorna
                return servico;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, "typeof(T): " + typeof(T).FullName, "OMS.Library.Servicos");
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
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }
    }
}

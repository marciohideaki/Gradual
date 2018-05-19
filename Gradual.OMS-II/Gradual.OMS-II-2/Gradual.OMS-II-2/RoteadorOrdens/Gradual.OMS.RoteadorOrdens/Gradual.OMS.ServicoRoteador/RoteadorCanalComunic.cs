using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using log4net;

namespace Gradual.OMS.ServicoRoteador
{
    public class RoteadorCanalComunic
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GetCallback - assina um
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callbackImpl"></param>
        /// <param name="endpointaddr"></param>
        /// <returns></returns>
        public static T GetChannel<T>(string endpointaddr, object callbackImpl )
        {
            try
            {
                // Inicializa
                T servico = default(T);
                Type tipo = typeof(T);

                logger.Debug("Ativando " + tipo.FullName + " como WCF");

                logger.Debug("Criando Binding para Endpoint: " + endpointaddr);

                // Cria via wcf
                Binding binding = (Binding)
                        typeof(BasicHttpBinding).Assembly.CreateInstance(_getBindingType(endpointaddr));
                binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
                binding.SendTimeout = new TimeSpan(0, 1, 0);
                binding.OpenTimeout = new TimeSpan(0, 0, 30);
                binding.CloseTimeout = new TimeSpan(0, 0, 30);
                if (_getBindingType(endpointaddr).Equals("System.ServiceModel.NetTcpBinding"))
                {
                    ((NetTcpBinding)binding).MaxReceivedMessageSize = 8000000;
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
                    //canal.Faulted += new EventHandler(_channelFaulted);
                    if (endpointaddr == null)
                    {
                        string msg = "Servico [" + tipo.FullName + "] nao pôde ser ativado.\n";
                        msg += "Verifique se existe <ServicoEndPointInfo> nas configuracoes locais,\n";
                        msg += "ou se um endpoint foi criado para o mesmo e registrado no ServicoLocalizador.";

                        logger.Error("ERRO: NENHUM ENDERECO DE ENDPOINT PARA SERVICO [" + tipo.FullName + "]!");
                        throw new Exception(msg);
                    }
                    servico = canal.CreateChannel(new EndpointAddress(endpointaddr));
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
                    //canal.Faulted += new EventHandler(_channelFaulted);
                    if (endpointaddr == null)
                    {
                        string msg = "Servico [" + tipo.FullName + "] nao pôde ser ativado.\n";
                        msg += "Verifique se existe <ServicoEndPointInfo> nas configuracoes locais,\n";
                        msg += "ou se um endpoint foi criado para o mesmo e registrado no ServicoLocalizador.";

                        logger.Error("ERRO: NENHUM ENDERECO DE ENDPOINT PARA SERVICO [" + tipo.FullName + "]!");
                        throw new Exception(msg);
                    }
                    servico = canal.CreateChannel(new EndpointAddress(endpointaddr));
                }

                ((IContextChannel)servico).OperationTimeout = new TimeSpan(0, 10, 0);

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
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static string _getBindingType(string address)
        {
            if (address.StartsWith("net.pipe://"))
                return "System.ServiceModel.NetNamedPipeBinding";

            if (address.StartsWith("http://"))
                return "System.ServiceModel.BasicHttpBinding";

            if (address.StartsWith("https://"))
                return "System.ServiceModel.WSHttpBinding";

            return "System.ServiceModel.NetTcpBinding";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using log4net;
using System.ServiceModel.Channels;
using System.Threading;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Implementação do serviço de localização (interface IServicoLocalizador)
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoLocalizador : IServicoLocalizador, IServicoLocalizadorReplicacao, IServicoControlavel
    {
        private Dictionary<string, Dictionary<string, ServicoInfo>> _servicos = 
            new Dictionary<string, Dictionary<string, ServicoInfo>>();
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;
        private ReplicacaoLocalizadorClient replclient = null;

        #region IServicoLocalizador Members

        public List<ServicoInfo> Consultar()
        {
            try
            {
                List<ServicoInfo> lista = new List<ServicoInfo>();
                foreach (KeyValuePair<string, Dictionary<string, ServicoInfo>> item1 in _servicos)
                    foreach (KeyValuePair<string, ServicoInfo> item2 in item1.Value)
                        lista.Add(item2.Value);
                return lista;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em ServicoLocalizador.Consultar():", ex);
                throw ex;
            }
        }

        public ServicoInfo Consultar(string servicoInterface)
        {
            try
            {
                logger.Debug("Consultando registro: " + servicoInterface );
                // Verifica se o seviço solicitado consta na lista
                if (!_servicos.ContainsKey(servicoInterface))
                {
                    // Informa
                    string msg = "O serviço solicitado (interface: " + servicoInterface + ") não consta na lista de serviços. " +
                        "Isto indica que o serviço em questão não fez o registro no serviço de localização. " +
                        "Verifique o processo que inicia este serviço;";
                    logger.Warn(msg);
                    
                    // Retorna
                    return null;
                }
                else if (!_servicos[servicoInterface].ContainsKey(""))
                {
                    // Informa
                    string msg = "O serviço solicitado (interface: " + servicoInterface + ") consta na lista de serviços mas não " +
                        "possui a instância com Id '' (default).";
                    logger.Warn(msg);

                    // Retorna
                    return null;
                }

                // Retorna o serviço solicitado
                return _servicos[servicoInterface][""];
            }
            catch (Exception ex)
            {
                logger.Error("Erro em ServicoLocalizador.Consultar(" + servicoInterface + "):", ex);
                throw ex;
            }
        }

        public ServicoInfo Consultar(string servicoInterface, string id)
        {
            try
            {
                logger.Debug("Consultando registro: " + servicoInterface + ", " + id);

                // Verifica se o seviço solicitado consta na lista
                if (!_servicos.ContainsKey(servicoInterface))
                {
                    // Informa
                    string msg = "O serviço solicitado (interface: " + servicoInterface + ") não consta na lista de serviços. " +
                        "Isto indica que o serviço em questão não fez o registro no serviço de localização. " +
                        "Verifique o processo que inicia este serviço;";

                    logger.Warn(msg);
                }
                else if (!_servicos[servicoInterface].ContainsKey(""))
                {
                    // Informa
                    string msg = "O serviço solicitado (interface: " + servicoInterface + ") consta na lista de serviços mas não " +
                        "possui a instância com Id '' (default).";

                    logger.Warn(msg);
                }

                // Retorna
                return _servicos[servicoInterface][id];
            }
            catch (Exception ex)
            {
                logger.Error("Erro em ServicoLocalizador.Consultar(" + servicoInterface + "," + id + "):", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Registra um servico e replica para os demais localizadores
        /// </summary>
        /// <param name="servico">objeto ServicoInfo</param>
        public void Registrar(ServicoInfo servico)
        {
            OperationContext context = OperationContext.Current;

            LocalizadorConfig config = GerenciadorConfig.ReceberConfig<LocalizadorConfig>();


            //context.RequestContext.//
            

            MessageProperties messageProperties = context.IncomingMessageProperties;

            RemoteEndpointMessageProperty endpointProperty =  messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            
            if (endpointProperty != null)
            {
                foreach (string instancia in servico.NomeInterface)
                {
                    logger.Debug("[" + endpointProperty.Address + ":" +
                        endpointProperty.Port +
                        "] Requisicao de registro do servico [" + servico.NomeInterface[0] + "]");
                }

                if ( config!=null && config.AllowedHosts.AllowedHost.Contains(endpointProperty.Address)==false)
                {
                    string msg = "Host [" + endpointProperty.Address + "] nao tem permissao para registrar servicos neste Localizador!!!";
                    logger.Fatal(msg);
                    Thread.Sleep(250);
                    throw new Exception(msg);
                }
            }
            //foreach (KeyValuePair<string, object> entry in messageProperties)
            //{
            //    logger.Debug("messageProperties[" + entry.Key + "]=" + entry.Value.GetType().ToString());
            //}
            
            _registrar(servico);

            if (this.replclient != null)
            {
                replclient.ReplicarRegistro(servico);
            }
        }

        /// <summary>
        /// Recebe um servico para ser registrado via processo de replicacao
        /// </summary>
        /// <param name="servico">objeto ServicoInfo</param>
        public void ReplicarRegistro(ServicoInfo servico)
        {
            _registrar(servico);
        }

        /// <summary>
        /// Registra um servico.
        /// </summary>
        /// <param name="servico">objeto ServicoInfo</param>
        private void _registrar(ServicoInfo servico)
        {
            try
            {
                // Dump do servico
                logger.Info("Registrando Servico: " + servico.NomeInstancia);

                foreach (string Interface in servico.NomeInterface)
                    logger.Info("          Interface: " + Interface);

                foreach (ServicoEndPointInfo einfo in servico.EndPoints)
                    logger.Info("           Endpoint: " + einfo.Endereco);

                lock (_servicos)
                {
                    foreach (string Interface in servico.NomeInterface)
                    {
                        string nomeServico = Interface.Split(',')[0];

                        string id = servico.ID == null ? "" : servico.ID;
                        if (!_servicos.ContainsKey(nomeServico))
                            _servicos.Add(nomeServico, new Dictionary<string, ServicoInfo>());

                        if (_servicos[nomeServico].ContainsKey(id))
                            _servicos[nomeServico][id] = servico;
                        else
                            _servicos[nomeServico].Add(id, servico);

                        // Faz o log do registro
                        logger.Info("Serviço registrado. Interface [" + Interface + "] Instancia =>" + servico.NomeInstancia);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em ServicoLocalizador.Registrar():", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Remove um servico registrado e replica as informacoes para os demais
        /// localizadores
        /// </summary>
        /// <param name="servicoInterface">string com a interface do servico</param>
        public void Remover(string servicoInterface)
        {
            _remover(servicoInterface);

            if (replclient != null)
            {
                replclient.ReplicarRemocao(servicoInterface);
            }
        }

        /// <summary>
        /// Remove um servico registrado. Este metodoé invocado somente pelo cliente 
        /// da replicacao
        /// </summary>
        /// <param name="servicoInterface">string com a interface do servico</param>
        public void ReplicarRemocao(string servicoInterface)
        {
            _remover(servicoInterface);
        }

        /// <summary>
        /// Remove um registro da lista de servicos registrado
        /// </summary>
        /// <param name="servicoInterface">string com a interface do servico</param>
        private void _remover(string servicoInterface)
        {
            try
            {
                logger.Info("Removendo interface [" + servicoInterface + "]");
                lock (_servicos)
                    _servicos[servicoInterface].Remove("");
            }
            catch (Exception ex)
            {
                logger.Error("Erro em ServicoLocalizador.Remover(" + servicoInterface + "):", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Remove um servico registrado e replica as informacoes para os demais
        /// localizadores
        /// </summary>
        /// <param name="servicoInterface">string com a interface do servico</param>
        /// <param name="id">ID do servico</param>
        public void Remover(string servicoInterface, string id)
        {
            _remover(servicoInterface, id);

            if (replclient != null)
            {
                replclient.ReplicarRemocaoID(servicoInterface, id);
            }
        }


        /// <summary>
        /// Remove um servico registrado. Este metodoé invocado somente pelo cliente 
        /// da replicacao
        /// </summary>
        /// <param name="servicoInterface">string com a interface do servico</param>
        /// <param name="id">ID do servico</param>
        public void ReplicarRemocaoID(string servicoInterface, string id)
        {
            _remover(servicoInterface,id);
        }

        /// <summary>
        /// Remove um registro da lista de servicos registrado
        /// </summary>
        /// <param name="servicoInterface">string com a interface do servico</param>
        /// <param name="id">ID do servico</param>
        private void _remover(string servicoInterface, string id)
        {
            try
            {
                logger.Info("Removendo interface [" + servicoInterface + "] do id [" + id + "]");
                lock (_servicos)
                    _servicos[servicoInterface].Remove(id);
            }
            catch (Exception ex)
            {
                logger.Error("Erro em ServicoLocalizador.Remover(" + servicoInterface + "," + id + "):", ex);
                throw ex;
            }
        }
        #endregion

        #region IServicoLocalizadorReplicacao
        /// <summary>
        /// Retorna o dicionario dos servicos registrados nesse localizador
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, ServicoInfo>> ObterListaServicos()
        {
            Dictionary<string, Dictionary<string, ServicoInfo>> servicos;

            lock (_servicos)
            {
                servicos = _servicos;
            }

            return servicos;
        }


        /// <summary>
        /// Registra uma lista de servicos recebida via processo de replicacao
        /// </summary>
        /// <param name="servicos">Dicionario de servicos</param>
        public void ReplicarLista(Dictionary<string, Dictionary<string, ServicoInfo>> servicos)
        {
            List<ServicoInfo> lista = new List<ServicoInfo>();
            foreach (KeyValuePair<string, Dictionary<string, ServicoInfo>> item1 in servicos)
                foreach (KeyValuePair<string, ServicoInfo> item2 in item1.Value)
                    lista.Add(item2.Value);

            foreach (ServicoInfo info in lista)
            {
                _registrar(info);
            }
        }

        #endregion  //IServicoLocalizadorReplicacao

        #region IServicoControlavel Members
        public void IniciarServico()
        {
            replclient = new ReplicacaoLocalizadorClient();
            Dictionary<string, Dictionary<string, ServicoInfo>> servicos;

            replclient.Conectar();

            if (replclient.QtdeReplicadores > 0)
            {
                servicos = replclient.ObterListaServicos();

                if (servicos != null && servicos.Count > 0)
                {
                    foreach (KeyValuePair<string, Dictionary<string, ServicoInfo>> item1 in servicos)
                    {
                        logger.Info("Replicando: [" + item1.Key + "]");

                        if (!_servicos.ContainsKey(item1.Key))
                            _servicos.Add(item1.Key, item1.Value);
                        else
                            _servicos[item1.Key] = item1.Value;
                    }
                }
            }
            _status = ServicoStatus.EmExecucao;
        }

        public void PararServico()
        {
            _status = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion // IServicoControlavel Members
    }
}

using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using log4net;
using System.Collections.Generic;
using System.ServiceModel.Dispatcher;
using Gradual.OMS.Library.Bindings;
using System.Security.Principal;
using System.Diagnostics;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Gerencia a vida do serviço, enquanto o objeto é tratado como serviço.
    /// Faz o bind dos canais de comunicação e registra no localizador.
    /// </summary>
    public class Servico
    {
        public object Instancia { get; set; }
        public Type TipoInstancia { get; set; }
        public ServicoInfo ServicoInfo { get; set; }

        public List<Type> Interface
        {
            get { return _interfaces; }
        }

        public ServiceHost Host { get; set; }
        public List<string> BaseAddress { get; set; }
        public string MexBaseAddress { get; set; }

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<Type> _interfaces = new List<Type>();
        private bool bAtivado = false;

        public Servico()
        {
            BaseAddress = new List<string>();
        }

        /// <summary>
        /// Ativar() - Cria instancia do servico, para uso local ou como WCF
        /// </summary>
        public void Ativar()
        {
            bool bSingleton = false;
            try
            {
                if (bAtivado)
                {
                    logger.Info("Servico já ativado por outra interface, ignorando");
                    return;
                }


                // Cria o tipo da interface
                Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (string nomeInterface in this.ServicoInfo.NomeInterface)
                {
                    logger.Debug("Servico.Ativar(): procurando interface nos assemblies:" + nomeInterface);
                    foreach (Assembly assembly in assemblies)
                    {
                        Type Interface = assembly.GetType(nomeInterface);
                        if (Interface != null)
                        {
                            _interfaces.Add(Interface);
                        }
                    }
                }

                // Melhorar....
                if ( _interfaces.Count == 0 || _interfaces.Count < this.ServicoInfo.NomeInterface.Count)
                {
                    foreach (string nomeInterface in this.ServicoInfo.NomeInterface)
                    {
                        logger.Info("Interface " + nomeInterface + " nao encontrada direto, usando Type.GetType()");
                        Type Interface = Type.GetType(nomeInterface);
                        if (Interface != null)
                        {
                            _interfaces.Add(Interface);
                        }
                        else
                        {
                            logger.Error("Interface " + nomeInterface + " nao encontrada em nenhum dos assemblies.");
                            return;
                        }
                    }
                }

                if ( this.ServicoInfo.NomeInstancia == null || this.ServicoInfo.NomeInstancia.Length == 0 )
                {
                    throw new Exception("Erro em Ativar(): Nome da instancia nao pode ser nula! Verifique a tag <NomeInstancia>");
                }


                // ATP: verificar esse ponto: se nao achou a interface, nao deve continuar aqui.
                // Tem instancia criada?
                if (this.Instancia == null)
                {
                    this.TipoInstancia = Type.GetType(this.ServicoInfo.NomeInstancia);
                    if ( this.TipoInstancia != null )
                    {
                        this.Instancia = Activator.CreateInstance(this.TipoInstancia); 
                    }
                    else
                    {
                        string msg = "Ativar(): ClassType da instancia nao encontrado para " + this.ServicoInfo.NomeInstancia;
                        msg += "\nVerifique Assemblies/Namespace da classe: " + this.ServicoInfo.NomeInstancia;
                        throw new Exception(msg);
                    }
                }

                // Registra o canal de comunicação se necessário (casos de wcf)
                if (this.ServicoInfo.AtivarWCF && this.Instancia!=null)
                {
                    logger.Info("Ativando " + this.ServicoInfo.NomeInstancia + " como WCF");

                    ServiceBehaviorAttribute [] srvbehavior = this.Instancia.GetType().GetCustomAttributes(typeof(ServiceBehaviorAttribute), true) as ServiceBehaviorAttribute[];
                    if ( srvbehavior != null && srvbehavior.Length > 0 )
                    {
                        foreach (ServiceBehaviorAttribute attribute in srvbehavior)
                        {
                            logger.Info("InstanceContextMode : " + attribute.InstanceContextMode.ToString());
                            if (attribute.InstanceContextMode == InstanceContextMode.Single)
                            {
                                bSingleton = true;
                                logger.Info("ConcurrencyMode: " + attribute.ConcurrencyMode.ToString());
                                //if (attribute.ConcurrencyMode == ConcurrencyMode.Single)
                                //{
                                //    attribute.ConcurrencyMode = ConcurrencyMode.Multiple;
                                //    logger.Info("Novo ConcurrencyMode: " + attribute.ConcurrencyMode.ToString());
                                //}
                            }
                            else
                            {
                                logger.Warn("********************************************************");
                                logger.Warn("InstanceContextMode para a classe " + this.ServicoInfo.NomeInstancia);
                                logger.Warn("Esta definida como: " + attribute.InstanceContextMode.ToString());
                                logger.Warn("NÃO É UM SINGLETON!!! VERIFIQUE SE SUA CLASSE FOI PENSADA");
                                logger.Warn("PARA TER MAIS DE UMA INSTANCIA ATIVA");
                                logger.Warn("********************************************************");
                            }
                        }
                    }

                    // Faz a ativação WCF
                    if ( bSingleton )
                        this.Host = new ServiceHost(this.Instancia);
                    else
                        this.Host = new ServiceHost(this.Instancia.GetType());
                    this.Host.OpenTimeout = new TimeSpan(0, 2, 0);
                    this.Host.Faulted += new EventHandler(_hostFaulted);

                    logger.Info("Adicionando MetaData Behavior para " + this.ServicoInfo.NomeInstancia );

                    //ATP: Adicionando informacoes do MEX
                    ServiceMetadataBehavior metadataBehavior;
                    metadataBehavior = this.Host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                    if (metadataBehavior == null)
                    {
                        logger.Info("Criando MetaData Behavior para " + this.ServicoInfo.NomeInstancia );
                        metadataBehavior = new ServiceMetadataBehavior();
                        string mexendpointaddr = this.MexBaseAddress + "/" + this.Instancia.ToString();
                        metadataBehavior.HttpGetUrl = new Uri(mexendpointaddr);
                        metadataBehavior.HttpGetEnabled = true;
                        this.Host.Description.Behaviors.Add(metadataBehavior);
                    }


                    // Se configurado, acrescenta as configurações de throttling
                    if (this.ServicoInfo.MaxConcurrentCalls > 0 &&
                        this.ServicoInfo.MaxConcurrentInstances > 0 &&
                        this.ServicoInfo.MaxConcurrentSessions > 0)
                    {
                        logger.Info("Adicionando ServiceThrottleBehavior para " + this.ServicoInfo.NomeInstancia);

                        ServiceThrottlingBehavior throttleBehavior = new ServiceThrottlingBehavior();
                        throttleBehavior.MaxConcurrentCalls =  this.ServicoInfo.MaxConcurrentCalls.Value;
                        throttleBehavior.MaxConcurrentInstances = this.ServicoInfo.MaxConcurrentInstances.Value;
                        throttleBehavior.MaxConcurrentSessions = this.ServicoInfo.MaxConcurrentSessions.Value;
                        logger.Info("MaxConcurrentCalls ......: " + throttleBehavior.MaxConcurrentCalls);
                        logger.Info("MaxConcurrentInstances ..: " + throttleBehavior.MaxConcurrentInstances);
                        logger.Info("MaxConcurrentSessions ...: " + throttleBehavior.MaxConcurrentSessions);

                        this.Host.Description.Behaviors.Add(throttleBehavior);
                    }


                    logger.Info("Adicionando ErrorServiceBehavior para " + this.ServicoInfo.NomeInstancia);
                    this.Host.Description.Behaviors.Add(new ErrorServiceBehavior());

                    logger.Info("Configurando ServiceDebugBehavior para " + this.ServicoInfo.NomeInstancia);
                    ServiceDebugBehavior debugBehavior = this.Host.Description.Behaviors.Find<ServiceDebugBehavior>();
                    if (debugBehavior != null)
                    {
                        debugBehavior.IncludeExceptionDetailInFaults = true;
                    }
                    else
                    {
                        debugBehavior = new ServiceDebugBehavior();
                        debugBehavior.IncludeExceptionDetailInFaults = true;
                        this.Host.Description.Behaviors.Add( debugBehavior );
                    }

                    // Cria endpoint se nao tiver algum definido no app.config
                    logger.Info("Adicionando endpoints para " + this.ServicoInfo.NomeInstancia);
                    if (this.Host.Description.Endpoints.Count == 0)
                    {
                        logger.Info("Nenhum endpoint declarado para o servico : " + this.ServicoInfo.NomeInstancia);

                        foreach (string baseAddress in this.BaseAddress)
                        {
                            Binding tcpBinding = null;

#if PRIVILEDGED_NAMED_PIPE
                            // NAO REMOVER O CODIGO ABAIXO
                            // PODERA SER USADO PARA ESCALAR A CAPACIDADE DO AMBIENTE
                            if ( !baseAddress.StartsWith("net.pipe") )
                                tcpBinding = Utilities.GetBinding(baseAddress);
                            else
                            {

                                Process myproc = Process.GetCurrentProcess();

                                // Get the privileges and associated attributes.
                                PrivilegeAndAttributesCollection privileges = myproc.GetPrivileges();

                                //int maxPrivilegeLength = privileges..Max(privilege => privilege.Privilege.ToString().Length);

                                foreach (PrivilegeAndAttributes privilegeAndAttributes in privileges)
                                {
                                    // The privilege.
                                    Privilege privilege = privilegeAndAttributes.Privilege;

                                    // The privilege state.
                                    PrivilegeState privilegeState = privilegeAndAttributes.PrivilegeState;

                                    // Write out the privilege and its state.
                                    logger.DebugFormat(
                                        "{0}{1} => {2}",
                                        privilege,
                                        privilege.ToString(),
                                        privilegeState);
                                }


                                // Enable the TakeOwnership privilege on it.
                                AdjustPrivilegeResult result = myproc.EnablePrivilege(Privilege.CreateGlobal);

                                tcpBinding = new AclSecuredNamedPipeBinding();

                                try
                                {
                                    SecurityIdentifier allow = (SecurityIdentifier)(new NTAccount("Everyone").Translate(typeof(SecurityIdentifier)));

                                    ((AclSecuredNamedPipeBinding)tcpBinding).AddUserOrGroup(allow);
                                }
                                catch(Exception ex)
                                {
                                    logger.Error(ex);
                                }

                                try
                                {
                                    SecurityIdentifier allow = (SecurityIdentifier)(new NTAccount("Todos").Translate(typeof(SecurityIdentifier)));

                                    ((AclSecuredNamedPipeBinding)tcpBinding).AddUserOrGroup(allow);
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex);
                                }

                                logger.Debug("Apos setar privilegios");
                                // Get the privileges and associated attributes.
                                privileges = myproc.GetPrivileges();


                                foreach (PrivilegeAndAttributes privilegeAndAttributes in privileges)
                                {
                                    // The privilege.
                                    Privilege privilege = privilegeAndAttributes.Privilege;

                                    // The privilege state.
                                    PrivilegeState privilegeState = privilegeAndAttributes.PrivilegeState;

                                    // Write out the privilege and its state.
                                    logger.DebugFormat(
                                        "{0}{1} => {2}",
                                        privilege,
                                        privilege.ToString(),
                                        privilegeState);
                                }

                            }
#else
                            tcpBinding = Utilities.GetBinding(baseAddress);
#endif
                            foreach (Type iface in this._interfaces)
                            {
                                string endpointaddr = baseAddress + "/" + iface.ToString();

                                logger.Info("Criando endpoint: Interface=[" + iface.ToString() + "]  Instancia=[" + this.Instancia.ToString() + "] Address=[" + endpointaddr + "]");

                                this.Host.AddServiceEndpoint(iface, tcpBinding, endpointaddr);
                            }
                        }
                    }

                    // Adiciona o enpoint MEX para o fim da lista
                    logger.Info("Criando endpoint MEX para " + this.ServicoInfo.NomeInstancia);
                    this.Host.AddServiceEndpoint(typeof(IMetadataExchange), new BasicHttpBinding(), this.MexBaseAddress + "/" + this.Instancia.ToString());

                    // Popula as informacoes dos endpoints para o servico Localizador
                    // Ver Ativador.Get<>()
                    if (this.Host.Description.Endpoints.Count > 0)
                    {
                        // Para cada endpoint do wcf, cria na colecao do ServicoInfo para publicar no localizador
                        foreach (System.ServiceModel.Description.ServiceEndpoint endPoint in this.Host.Description.Endpoints)
                        {
                            logger.Info("Verificando validade do endpoint [" + endPoint.Address.Uri.ToString() + "] tipo: [" + endPoint.Binding.GetType().FullName + "] para localizador");

                            if ( endPoint.Address.Uri.ToString().Contains("/MEX") == false)
                            {
                                logger.Info("Adicionando informacoes do endpoint [" + endPoint.Address.Uri.ToString() + "] tipo: [" + endPoint.Binding.GetType().FullName + "] para localizador");
                                this.ServicoInfo.EndPoints.Add(
                                    new ServicoEndPointInfo()
                                    {
                                        Endereco = endPoint.Address.Uri.ToString(),
                                        //NomeBindingType = endPoint.Binding.GetType().FullName
                                        NomeBindingType = Utilities.GetBindingType(endPoint.Address.Uri.ToString())
                                    });
                            }

                            ContractDescription cd = endPoint.Contract;

                            foreach(OperationDescription od in cd.Operations)
                            {
                                DataContractSerializerOperationBehavior serializerBh = od.Behaviors.Find<DataContractSerializerOperationBehavior>();
                                if ( serializerBh== null )
                                {
                                    logger.Info("Adicionando DataContractSerializerOperationBehavior");
                                    serializerBh = new DataContractSerializerOperationBehavior(od);
                                    od.Behaviors.Add(serializerBh);
                                }

                                logger.Info("Setando MaxItemsInObjectGraph para operacao: " + od.Name);
                                serializerBh.MaxItemsInObjectGraph = 8000000;
                            }
                        }
                    }

                    logger.Info("Servico: " + this.ServicoInfo.NomeInstancia  + " opening host...");

                    // Inicia o ServiceHost
                    this.Host.Open();
                }

                // Marca servico como ativo.
                bAtivado = true;

                // Registra no localizador se necessário
                if (this.ServicoInfo.RegistrarLocalizador)
                {
                    logger.Info("Registrando servico no Localizador");
                    LocalizadorCliente.Registrar(this.ServicoInfo);
                }

                logger.Info("***");
                logger.Info("   ");
            }
            catch (Exception ex)
            {
                logger.Error("Erro em Servico.Ativar(): ", ex);
                throw ex;
            }
        }

        private void _hostFaulted(object sender, EventArgs e)
        {
            logger.Error("Host Faulted event raised");
        }

        /// <summary>
        /// Desativar - Remove servico do localizador e WCF
        /// </summary>
        public void Desativar()
        {
            try
            {
                // Desregistra do localizador
                if (this.ServicoInfo.RegistrarLocalizador)
                {
                    foreach (Type iface in this._interfaces)
                    {
                        LocalizadorCliente.Remover(iface);
                    }
                }

                // Desregistra o canal de comunicacao
                if (this.ServicoInfo.AtivarWCF)
                {
                    // Desregistra WCF
                    if ( this.Host.State != CommunicationState.Closed )
                        this.Host.Close();
                }

                bAtivado = false;

            }
            catch (Exception ex)
            {
                logger.Error("Erro em Servico.Desativar(): ", ex);
            }
        }
    }
}

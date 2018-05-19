using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.ServiceModel.Description;

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
        public Type Interface { get; set; }
        public ServiceHost Host { get; set; }

        public void Ativar()
        {
            try
            {
                // Cria o tipo da interface
                Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    this.Interface = assembly.GetType(this.ServicoInfo.NomeInterface);
                    if (this.Interface != null)
                        break;
                }

                // ATP: verificar esse ponto: se nao achou a interface, nao deve continuar aqui.
                // Tem instancia criada?
                if (this.Instancia == null)
                {
                    this.TipoInstancia = Type.GetType(this.ServicoInfo.NomeInstancia);
                    this.Instancia = Activator.CreateInstance(this.TipoInstancia);
                }

                // Registra o canal de comunicação se necessário (casos de wcf)
                if (this.ServicoInfo.AtivarWCF)
                {
                    // Faz a ativação WCF
                    this.Host = new ServiceHost(this.Instancia);
                    this.Host.OpenTimeout = new TimeSpan(0, 2, 0);

                    //ATP: Adicionando informacoes do MEX
                    ServiceMetadataBehavior metadataBehavior;
                    metadataBehavior = this.Host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                    if (metadataBehavior == null)
                    {
                        metadataBehavior = new ServiceMetadataBehavior();
                        this.Host.Description.Behaviors.Add(metadataBehavior);
                    }

                    // Para cada endpoint do wcf, cria na colecao do ServicoInfo para publicar no localizador
                    foreach (System.ServiceModel.Description.ServiceEndpoint endPoint in this.Host.Description.Endpoints)
                    {
                        this.ServicoInfo.EndPoints.Add(
                            new ServicoEndPointInfo()
                            {
                                Endereco = endPoint.Address.Uri.ToString(),
                                NomeBindingType = endPoint.Binding.GetType().FullName
                            });
                        //foreach (ServicoEndPointInfo sei in this.ServicoInfo.EndPoints)
                        //{
                        //    Binding binding = (Binding)typeof(BasicHttpBinding).Assembly.CreateInstance(sei.NomeBindingType);
                        //    binding.ReceiveTimeout = new TimeSpan(1, 0, 0);
                        //    this.Host.AddServiceEndpoint(this.Interface, binding, new Uri(sei.Endereco));
                        //}
                    }


                    // Inicia o ServiceHost
                    this.Host.Open();
                }

                // Registra no localizador se necessário
                if (this.ServicoInfo.RegistrarLocalizador)
                    LocalizadorCliente.Registrar(this.ServicoInfo);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public void Desativar()
        {
            try
            {
                // Desregistra do localizador
                if (this.ServicoInfo.RegistrarLocalizador)
                    LocalizadorCliente.Remover(this.Interface);

                // Desregistra o canal de comunicacao
                if (this.ServicoInfo.AtivarWCF)
                {
                    // Desregistra WCF
                    this.Host.Close();
                }
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }
    }
}

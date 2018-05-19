using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    /// <summary>
    /// Wrapper para Host.
    /// Pode criar o host internamente ou receber um host previamente criado.
    /// </summary>
    public class Host 
    {
        public List<Janela> Janelas { get; set; }

        public Host()
        {
            criarInstancia();
        }

        public HostInfo Info { get; set; }

        public IServicoInterfaceDesktopHost Instancia { get; set; }

        public Host(HostInfo hostInfo)
        {
            this.Info = hostInfo;
            this.Janelas = new List<Janela>();
            criarInstancia();
        }

        public Host(HostInfo hostInfo, IServicoInterfaceDesktopHost instancia) 
        {
            this.Info = hostInfo;
            this.Janelas = new List<Janela>();
            this.Instancia = instancia;
            this.Instancia.Inicializar(hostInfo);
        }

        private void criarInstancia()
        {
            // Cria a instancia
            switch (this.Info.InicializacaoTipo)
            {
                case HostInicializacaoTipoEnum.Normal:
                    this.Instancia = new ServicoInterfaceDesktopHost();
                    break;
                case HostInicializacaoTipoEnum.NovoAppDomain:
                    AppDomain appDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
                    this.Instancia = 
                        (IServicoInterfaceDesktopHost)appDomain.CreateInstanceAndUnwrap(
                            typeof(ServicoInterfaceDesktopHost).Assembly.FullName, 
                            typeof(ServicoInterfaceDesktopHost).FullName);
                    this.Info.AppDomainId = appDomain.Id;
                    break;
            }

            // Inicializa
            this.Instancia.Inicializar(this.Info);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Host.WindowsService.AtivadorServicos
{
    public partial class ServiceHost : ServiceBase
    {
        public ServiceHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Pega o config
            WindowsServiceHostConfig config = GerenciadorConfig.ReceberConfig<WindowsServiceHostConfig>();
            if (config == null)
                config = new WindowsServiceHostConfig();

            // Carrega servicos do config
            ServicoHostColecao.Default.CarregarConfig(config.ServicoHostId);
            ServicoHostColecao.Default.IniciarServicos();
        }

        protected override void OnStop()
        {
            // Para os serviços
            ServicoHostColecao.Default.PararServicos();
        }
    }
}

using System.ServiceProcess;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.IO;

namespace Gradual.OMS.Host.WindowsService.AtivadorServicos
{
    public partial class ServiceHost : ServiceBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _me;

        public ServiceHost()
        {
            string [] args = System.Environment.GetCommandLineArgs();

            InitializeComponent();

            if (args.Length == 1)
            {
                _me = args[0];
                this.ServiceName = Path.GetFileNameWithoutExtension(args[0]);
            }
            else
            {
                _me = args[1];
                this.ServiceName = args[1];
            }
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("*** Iniciando servico " + _me + " ***" );

            // Pega o config
            WindowsServiceHostConfig config = GerenciadorConfig.ReceberConfig<WindowsServiceHostConfig>();
            if (config == null)
                config = new WindowsServiceHostConfig();

            // Carrega servicos do config
            ServicoHostColecao.Default.CarregarConfig(config.ServicoHostId);
            ServicoHostColecao.Default.IniciarServicos();

            logger.Info("Servico " + _me + " iniciado");
        }

        protected override void OnStop()
        {
            logger.Info("Finalizando servico " + _me);
            // Para os serviços
            ServicoHostColecao.Default.PararServicos();
            logger.Info("*** Servico " + _me + " finalizado. ***");
        }
    }
}

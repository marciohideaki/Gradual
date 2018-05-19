using System.ServiceProcess;

namespace Gradual.MinhaConta.Servico.ImportacaoFundos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new ImportacaoClubesEFundos() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}

using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Xml.Serialization;
using log4net;
using Microsoft.Win32;


namespace Gradual.OMS.Host.WindowsService.AtivadorServicos
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProjectInstaller()
        {
            InitializeComponent();

        }

        private void configurar()
        {
            // Pega ou cria o config
            WindowsServiceHostConfig config = lerConfig();

            // Atribui os valores
            this.serviceInstaller.ServiceName = config.ServiceName;
            this.serviceInstaller.Description = config.ServiceDescription;
            this.serviceInstaller.ServicesDependedOn = config.ServiceDependedOn;
            this.serviceInstaller.DisplayName = config.ServiceDisplayName;

            logger.Warn("Reg: " + config.ServiceName);
        }

        private WindowsServiceHostConfig lerConfig()
        {
            // Inicializa
            string nomeArquivo = "serviceInstall.config";

            // Apenas se tem o arquivo
            if (File.Exists(nomeArquivo))
            {
                // Abre o arquivo
                FileStream fs = File.Open(nomeArquivo, FileMode.Open, FileAccess.Read);

                // Desserializa
                XmlSerializer serializer = new XmlSerializer(typeof(WindowsServiceHostConfig));
                WindowsServiceHostConfig config = (WindowsServiceHostConfig)serializer.Deserialize(fs);

                // Finaliza
                fs.Close();

                // Retorna
                return config;
            }
            else
            {
                // Retorna
                return new WindowsServiceHostConfig();
            }
        }

        //protected override void OnBeforeInstall(IDictionary savedState)
        //{
        //    base.OnBeforeInstall(savedState);

        //    System.Console.WriteLine("Parameters: " + this.Context.Parameters.Count);

        //    foreach (string parameter in this.Context.Parameters.Values)
        //    {
        //        System.Console.WriteLine("Parameter: " + parameter);
        //    }

        //    // Gets the parameter passed across in the CustomActionData.
        //    string cmdLineParameter1 = this.Context.Parameters["Parameter1"];
        //    string cmdLineParameter2 = this.Context.Parameters["Parameter2"];
        //    string cmdLineParameter3 = this.Context.Parameters["Parameter3"];
        //}


        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            RegistryKey system;
            RegistryKey currentControlSet;
            RegistryKey services;
            RegistryKey service;
            RegistryKey config;

            //Open the HKEY_LOCAL_MACHINE\SYSTEM key
            system = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System");
            //Open CurrentControlSet
            currentControlSet = system.OpenSubKey("CurrentControlSet");
            //Go to the services key
            services = currentControlSet.OpenSubKey("Services");

            //Open the key for your service, and allow writing
            service = services.OpenSubKey(this.serviceInstaller.ServiceName, true);

            ////(Optional) Add some custom information your service will use...
            //config = service.CreateSubKey("Parameters");
            //config.SetValue("Arguments", command);

            string path = service.GetValue("ImagePath") + " " + this.serviceInstaller.ServiceName;
            service.SetValue("ImagePath", path);
        }

        // Uncomment this to implement custom command line parameters on uninstall
        //public override void Uninstall(System.Collections.IDictionary stateSaver)
        //{
        //    base.Uninstall(stateSaver);

        //    // Gets the parameter passed across in the CustomActionData.
        //    string cmdLineParameter1 = this.Context.Parameters["Parameter1"];
        //    string cmdLineParameter2 = this.Context.Parameters["Parameter2"];
        //    string cmdLineParameter3 = this.Context.Parameters["Parameter3"];

        //    //RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework\AssemblyFolders", true);
        //    //regKey.DeleteSubKey(KeyName);
        //    //regKey.Close();
        //}

    }
}

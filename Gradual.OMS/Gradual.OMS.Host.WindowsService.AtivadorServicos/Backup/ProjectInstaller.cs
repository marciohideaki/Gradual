using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;

using Gradual.OMS.Library;
using System.Xml.Serialization;


namespace Gradual.OMS.Host.WindowsService.AtivadorServicos
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
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

            Log.EfetuarLog("Reg: " + config.ServiceName, LogTipoEnum.Aviso);
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
    }
}

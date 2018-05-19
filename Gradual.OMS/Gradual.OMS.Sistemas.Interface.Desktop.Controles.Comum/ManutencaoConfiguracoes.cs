using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum
{
    public partial class ManutencaoConfiguracoes : XtraUserControl, IControle
    {
        private Configuration _exeConfiguration = null;
        private XmlDocument _xmlDocument = null;

        public ManutencaoConfiguracoes()
        {
            InitializeComponent();

            inicializarControles();
        }

        private void inicializarControles()
        {
            // Abre o xml
            _exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(_exeConfiguration.FilePath);

            // Lista sessões
            foreach (XmlNode node in _xmlDocument.SelectNodes(@"/configuration/configSections/section"))
                if (node.Attributes["type"].Value.StartsWith(typeof(Gradual.OMS.Library.ConfigurationHandler).FullName))
                    cmbSessao.Properties.Items.Add(node.Attributes["name"].Value);
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            return null;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion

        private void cmbSessao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSessao.SelectedItem != null)
            {
                string tipoStr = (string)cmbSessao.SelectedItem;
                string id = null;
                if (tipoStr.Contains("-"))
                {
                    string[] tipoStr2 = tipoStr.Split('-');
                    tipoStr = tipoStr2[0];
                    id = tipoStr2[1];
                }
                
                // Cria o tipo
                Type tipo = null;
                Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    tipo = assembly.GetType(tipoStr);
                    if (tipo != null)
                        break;
                }

                if (id == null)
                    ppg.SelectedObject = GerenciadorConfig.ReceberConfig(tipo);
                else
                    ppg.SelectedObject = GerenciadorConfig.ReceberConfig(tipo, id);
                ppg.RetrieveFields();
            }
        }
    }
}

using System.ComponentModel;
using System.Configuration.Install;

namespace Gradual.MinhaConta.Servico.ImportacaoFundos
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Risco.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    public partial class PerfilRiscoDetalhe : XtraUserControl
    {
        /// <summary>
        /// Parametros da tela
        /// </summary>
        private PerfilRiscoDetalheParametros _parametros = new PerfilRiscoDetalheParametros();

        /// <summary>
        /// Contexto da interface. Necessário para repassar a
        /// informação da sessão.
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Regra de risco que está sendo alterada
        /// </summary>
        private PerfilRiscoInfo _perfilRiscoInfo = null;

        /// <summary>
        /// Construtor. Recebe o perfil a ser editado
        /// </summary>
        public PerfilRiscoDetalhe(PerfilRiscoInfo perfilRiscoInfo)
        {
            InitializeComponent();

            // Inicializa
            _perfilRiscoInfo = perfilRiscoInfo;

            // Pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Captura os eventos
            this.Load += new EventHandler(PerfilRiscoDetalhe_Load);
        }

        public void SalvarTabs()
        {
            // Envia mensagem de salvar para o tab de regras de risco
            ((IControle)tabRegraRiscoPerfilRisco).ProcessarMensagem(
                new SinalizarSalvarPerfilRiscoRequest()
                {
                    PerfilRisco = _perfilRiscoInfo
                });
        }
        
        private void PerfilRiscoDetalhe_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
                carregarTela();
        }

        private void carregarTela()
        {
            // Carrega elementos da regra de risco
            ppg.SelectedObject = _perfilRiscoInfo;

            // Envia mensagem de inicialização para o tab de regras de risco
            ((IControle)tabRegraRiscoPerfilRisco).ProcessarMensagem(
                new SinalizarInicializarPerfilRiscoRequest()
                {
                    PerfilRisco = _perfilRiscoInfo
                });
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Salva layouts
            _parametros.LayoutsDevExpress.SalvarLayout(this.layoutControl);
            _parametros.ParametrosTabRegraRisco = (TabRegraRiscoParametros)tabRegraRiscoPerfilRisco.SalvarParametros(evento);

            // Retorna
            return _parametros;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            // Seta a classe de parametros. Será utilizada no load do controle
            PerfilRiscoDetalheParametros parametros2 = parametros as PerfilRiscoDetalheParametros;
            if (parametros2 != null)
            {
                _parametros = parametros2;
                tabRegraRiscoPerfilRisco.CarregarParametros(parametros2.ParametrosTabRegraRisco, evento);
            }
        }
    }
}

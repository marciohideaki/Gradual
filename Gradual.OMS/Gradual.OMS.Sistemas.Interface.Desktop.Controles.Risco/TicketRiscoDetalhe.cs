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
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    public partial class TicketRiscoDetalhe : XtraUserControl
    {
        /// <summary>
        /// Contexto da interface. Necessário para repassar a
        /// informação da sessão.
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Ticket de risco que está sendo alterada
        /// </summary>
        private TicketRiscoInfo _ticketRisco = null;

        /// <summary>
        /// Construtor que recebe o item a ser trabalhado
        /// </summary>
        public TicketRiscoDetalhe(TicketRiscoInfo ticketRisco)
        {
            InitializeComponent();

            // Inicializa
            _ticketRisco = ticketRisco;

            // Pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Captura os eventos
            this.Load += new EventHandler(TicketRiscoDetalhe_Load);
        }

        private void TicketRiscoDetalhe_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
                carregarTela();
        }

        private void carregarTela()
        {
            // Carrega elementos da regra de risco
            ppg.SelectedObject = _ticketRisco;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Custodia
{
    public partial class ContaCorrenteDetalhe : XtraUserControl
    {
        /// <summary>
        /// Contexto da interface. Necessário para repassar a
        /// informação da sessão.
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Conta Corrente que está sendo alterada
        /// </summary>
        private ContaCorrenteInfo _contaCorrenteInfo = null;

        /// <summary>
        /// Construtor que recebe a conta corrente a ser editada
        /// </summary>
        public ContaCorrenteDetalhe(ContaCorrenteInfo contaCorrente)
        {
            InitializeComponent();

            // Inicializa
            _contaCorrenteInfo = contaCorrente;

            // Pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Captura os eventos
            this.Load += new EventHandler(ContaCorrenteDetalhe_Load);
        }

        private void ContaCorrenteDetalhe_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
                carregarTela();
        }

        private void carregarTela()
        {
            // Carrega elementos da regra de risco
            ppg.SelectedObject = _contaCorrenteInfo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Custodia.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Custodia
{
    /// <summary>
    /// Controle de detalhe de custódia
    /// </summary>
    public partial class CustodiaDetalhe : XtraUserControl
    {
        /// <summary>
        /// Contexto da interface. Necessário para repassar a
        /// informação da sessão.
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Custódia que está sendo alterada
        /// </summary>
        private CustodiaInfo _custodiaInfo = null;

        /// <summary>
        /// Construtor que recebe a custódia a ser editada
        /// </summary>
        public CustodiaDetalhe(CustodiaInfo custodia)
        {
            InitializeComponent();

            // Inicializa
            _custodiaInfo = custodia;

            // Pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Captura os eventos
            this.Load += new EventHandler(CustodiaDetalhe_Load);
        }

        public void SalvarTabs()
        {
            // Envia mensagem de salvar para o tab de regras de risco
            ((IControle)tabCustodiaPosicao).ProcessarMensagem(
                new SinalizarSalvarCustodiaRequest()
                {
                    Custodia = _custodiaInfo
                });
        }

        private void CustodiaDetalhe_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
                carregarTela();
        }

        private void carregarTela()
        {
            // Carrega elementos da regra de risco
            ppg.SelectedObject = _custodiaInfo;

            // Envia inicialização para a tab de posições
            ((IControle)tabCustodiaPosicao).ProcessarMensagem(
                new SinalizarInicializarCustodiaRequest()
                {
                    Custodia = _custodiaInfo
                });
        }
    }
}

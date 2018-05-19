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
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;
using Orbite.RV.Contratos.MarketData.Bovespa.Dados;
using Orbite.RV.Contratos.MarketData.Bovespa;
using Orbite.RV.Contratos.MarketData.Bovespa.Mensagens;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.MarketData
{
    public partial class CadastroPapeisBovespa : XtraUserControl, IControle
    {
        private Controle _item = null;
        private InterfaceContextoOMS _contexto = null;
        private CadastroPapeisBovespaParametros _parametros = new CadastroPapeisBovespaParametros();

        public CadastroPapeisBovespa()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroPapeisBovespa_Load);
        }

        private void CadastroPapeisBovespa_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo design
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Carrega lista de usuários
                carregarLista();

                // Carrega os layouts dos controles
                _parametros.LayoutsDevExpress.RecuperarLayouts(this);

                // Captura eventos
                grdv.DoubleClick += new EventHandler(grdv_DoubleClick);
            }
        }

        private void grdv_DoubleClick(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            InstrumentoBovespaInfo info = (InstrumentoBovespaInfo)this.grdv.GetFocusedRow();
            if (info != null)
            {
                // Mostra detalhe do usuario e salva se ok
                PapelBovespaDetalhe controleDetalhe = new PapelBovespaDetalhe(info);
                FormDialog frm = new FormDialog(controleDetalhe, FormDialogTipoEnum.OkCancelar);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Pede para o controle salvar demais informações
                    controleDetalhe.SalvarTabs();

                    // Pede para salvar tabs
                    controleDetalhe.SalvarTabs();
                }
            }
        }

        private void carregarLista()
        {
            // Guarda eventual selecao
            int selecao = grdv.FocusedRowHandle;

            // Referencia ao servico de seguranca
            IServicoMarketDataBovespa servicoMarketData = Ativador.Get<IServicoMarketDataBovespa>();

            // Pede a lista
            List<InstrumentoBovespaInfo> lista =
                ((ListarInstrumentosBovespaResponse)
                    servicoMarketData.ListarInstrumentosBovespa(
                        new ListarInstrumentosBovespaRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao
                        })).Instrumentos;

            // Associa ao grid
            grd.DataSource = lista;

            // Mantem selecao anterior
            grdv.FocusedRowHandle = selecao;
        }
        
        #region IControle Members

        public void Inicializar(Controle controle)
        {
            _item = controle;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Salva layouts
            _parametros.LayoutsDevExpress.SalvarLayout(this.grdv);

            // Retorna
            return _parametros;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            // Seta a classe de parametros. Será utilizada no load do controle
            CadastroPapeisBovespaParametros parametros2 = parametros as CadastroPapeisBovespaParametros;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion
    }
}

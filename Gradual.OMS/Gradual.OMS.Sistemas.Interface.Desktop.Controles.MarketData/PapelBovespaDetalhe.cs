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
using Gradual.OMS.Contratos.Interface.Desktop.Controles.MarketData.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

using Orbite.RV.Contratos.MarketData.Bovespa;
using Orbite.RV.Contratos.MarketData.Bovespa.Dados;
using Orbite.RV.Contratos.MarketData.Bovespa.Mensagens;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.MarketData
{
    /// <summary>
    /// Controle de detalhe de papel bovespa
    /// </summary>
    public partial class PapelBovespaDetalhe : XtraUserControl
    {
        /// <summary>
        /// Contexto da interface. Necessário para repassar a
        /// informação da sessão.
        /// </summary>
        private InterfaceContextoOMS _contexto = null;

        /// <summary>
        /// Instrumento Bovespa que está sendo alterado
        /// </summary>
        private InstrumentoBovespaInfo _instrumentoBovespa = null;

        /// <summary>
        /// Construtor que recebe o item a ser trabalhado
        /// </summary>
        /// <param name="instrumentoBovespa"></param>
        public PapelBovespaDetalhe(InstrumentoBovespaInfo instrumentoBovespa)
        {
            InitializeComponent();

            // Inicializa
            _instrumentoBovespa = instrumentoBovespa;

            // Pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Captura os eventos
            this.Load += new EventHandler(CustodiaDetalhe_Load);
        }
        
        public void SalvarTabs()
        {
            // Envia mensagem de salvar para o tab de regras de risco
            ((IControle)tabRegraRisco).ProcessarMensagem(
                new SinalizarSalvarInstrumentoBovespaRequest()
                {
                    InstrumentoBovespa = _instrumentoBovespa
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
            ppg.SelectedObject = _instrumentoBovespa;

            // Seta periodo sugerido de cotação inicial
            txtCotacaoFim.DateTime = DateTime.Now.Date;
            txtCotacaoInicio.DateTime = DateTime.Now.Date.AddDays(-5);
            
            // Pede a lista de séries deste instrumento
            grdSeries.DataSource =
                Ativador.Get<IServicoMarketDataBovespa>().ListarInstrumentosBovespa(
                    new ListarInstrumentosBovespaRequest()
                    {
                        TipoLista = ListarInstrumentosBovespaTipoListaEnum.HistoricoCompleto,
                        Instrumento = 
                            new InstrumentoBovespaInfo()
                            {
                                CodigoNegociacao = _instrumentoBovespa.CodigoNegociacao
                            }
                    }).Instrumentos;
            
            // Pede a lista de eventos deste instrumento
            grdEventos.DataSource =
                Ativador.Get<IServicoMarketDataBovespa>().ReceberSerieBovespa(
                    new ReceberSerieBovespaRequest()
                    {
                        Instrumento = _instrumentoBovespa
                    }).Resultado;

            // Envia inicialização para a tab de posições
            ((IControle)tabRegraRisco).ProcessarMensagem(
                new SinalizarInicializarInstrumentoBovespaRequest()
                {
                    InstrumentoBovespa = _instrumentoBovespa
                });
        }

        private void cmdConsultarCotacao_Click(object sender, EventArgs e)
        {
            // Pede a lista de cotações deste instrumento
            grdCotacao.DataSource = null;
            grdCotacao.DataSource =
                Ativador.Get<IServicoMarketDataBovespa>().ReceberHistoricoCotacaoBovespa(
                    new ReceberHistoricoCotacaoBovespaRequest()
                    {
                        Instrumento =
                            new InstrumentoBovespaInfo()
                            {
                                CodigoNegociacao = _instrumentoBovespa.CodigoNegociacao
                            },
                        DataInicial = txtCotacaoInicio.DateTime,
                        DataFinal = txtCotacaoFim.DateTime
                    }).Resultado;
        }
    }
}

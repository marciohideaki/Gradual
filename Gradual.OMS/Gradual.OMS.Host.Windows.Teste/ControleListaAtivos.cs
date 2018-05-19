using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Gradual.OMS.Contratos.CanaisNegociacao;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop;

namespace Gradual.OMS.Host.Windows.Teste
{
    public partial class ControleListaAtivos : UserControl, IControle
    {
        private Controle _controle = null;
        private IServicoOrdens _servicoOrdens = null;
        private IServicoCanaisNegociacao _servicoCanais = null;

        public ControleListaAtivos()
        {
            InitializeComponent();

            if (!this.DesignMode)
                this.Load += new EventHandler(ControleListaAtivos_Load);
        }

        private delegate void InvokeDelegate();
        void ControleListaAtivos_Load(object sender, EventArgs e)
        {
            // Referencia aos serviços
            _servicoOrdens = Ativador.Get<IServicoOrdens>();
            _servicoCanais = Ativador.Get<IServicoCanaisNegociacao>();

            // Pede lista de ativos
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(
                    delegate(object parametros)
                    {
                        ListarInstrumentosResponse listarInstrumentosResponse =
                            _servicoOrdens.ListarInstrumentos(
                                new ListarInstrumentosRequest()
                                {
                                    CodigoMensagem = "reqi01",
                                    CodigoBolsa = "BMF",
                                    CodigoCliente = "123",
                                    DataReferencia = DateTime.Now
                                });
                        var t = from s in listarInstrumentosResponse.Instrumentos
                                select new InstrumentoInfo() { Symbol = s };
                        grdInstrumentos.Invoke(
                            new InvokeDelegate(
                                delegate()
                                {
                                    grdInstrumentos.DataSource = new List<InstrumentoInfo>(t);
                                }));
                    }));
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
            _controle = controle;
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
    }
}

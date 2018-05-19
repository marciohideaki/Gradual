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
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    public partial class CadastroRegrasRisco : XtraUserControl, IControle
    {
        private Controle _item = null;
        private InterfaceContextoOMS _contexto = null;
        private CadastroRegrasRiscoParametros _parametros = new CadastroRegrasRiscoParametros();

        public CadastroRegrasRisco()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroRegrasRisco_Load);
            this.cmdAdicionar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(cmdAdicionar_ItemClick);
            this.cmdRemover.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(cmdRemover_ItemClick);

            this.cadastroRegrasRiscoBase.EventoRemover += new EventHandler<CadastroRegrasRiscoBaseEventArgs>(cadastroRegrasRiscoBase_EventoRemover);
            this.cadastroRegrasRiscoBase.EventoSalvar += new EventHandler<CadastroRegrasRiscoBaseEventArgs>(cadastroRegrasRiscoBase_EventoSalvar);
        }

        private void cadastroRegrasRiscoBase_EventoSalvar(object sender, CadastroRegrasRiscoBaseEventArgs e)
        {
            // Referencia ao servico de seguranca
            IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();

            // Salva 
            servicoRisco.SalvarRegraRisco(
                new SalvarRegraRiscoRequest()
                {
                    CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                    RegraRiscoInfo = e.RegraRisco
                });
        }

        private void cadastroRegrasRiscoBase_EventoRemover(object sender, CadastroRegrasRiscoBaseEventArgs e)
        {
            // Referencia ao servico de seguranca
            IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();

            // Remove
            servicoRisco.RemoverRegraRisco(
                new RemoverRegraRiscoRequest()
                {
                    CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                    CodigoRegraRisco = e.RegraRisco.CodigoRegraRisco
                });
        }

        private void CadastroRegrasRisco_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo design
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Inicializa o controle base
                cadastroRegrasRiscoBase.Inicializar();

                // Carrega lista de usuários
                carregarLista();

                // Carrega os layouts dos controles
                _parametros.LayoutsDevExpress.RecuperarLayouts(cadastroRegrasRiscoBase);
            }
        }

        private void carregarLista()
        {
            // Referencia ao servico de seguranca
            IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();

            // Pede a lista
            List<RegraRiscoInfo> lista =
                ((ListarRegraRiscoResponse)
                    servicoRisco.ListarRegraRisco(
                        new ListarRegraRiscoRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao
                        })).Resultado;

            // Associa ao grid
            cadastroRegrasRiscoBase.CarregarLista(lista);
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
            _item = controle;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Salva layouts
            _parametros.LayoutsDevExpress.SalvarLayout(cadastroRegrasRiscoBase.GridView);

            // Retorna
            return _parametros;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            // Seta a classe de parametros. Será utilizada no load do controle
            CadastroRegrasRiscoParametros parametros2 = parametros as CadastroRegrasRiscoParametros;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion

        private void cmdAdicionar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            cadastroRegrasRiscoBase.CriarRegraRisco();
        }

        private void cmdRemover_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            cadastroRegrasRiscoBase.RemoverRegraRiscoSelecionada();
        }

        private void cmdAtualizar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            carregarLista();
        }
    }
}

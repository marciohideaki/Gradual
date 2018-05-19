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
    public partial class CadastroTicketsRisco : XtraUserControl, IControle
    {
        private Controle _item = null;
        private InterfaceContextoOMS _contexto = null;
        private CadastroTicketsRiscoParametros _parametros = new CadastroTicketsRiscoParametros();

        public CadastroTicketsRisco()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroPerfilRisco_Load);
            this.cmdAdicionar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(cmdAdicionar_ItemClick);
            this.cmdRemover.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(cmdRemover_ItemClick);
        }

        private void CadastroPerfilRisco_Load(object sender, EventArgs e)
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
            TicketRiscoInfo info = (TicketRiscoInfo)this.grdv.GetFocusedRow();
            if (info != null)
            {
                // Mostra detalhe e salva se ok
                TicketRiscoDetalhe controleDetalhe = new TicketRiscoDetalhe(info);
                FormDialog frm = new FormDialog(controleDetalhe, FormDialogTipoEnum.OkCancelar);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Salva informações do perfil de risco
                    salvar(info);
                }
            }
        }

        private void carregarLista()
        {
            // Guarda eventual selecao
            int selecao = grdv.FocusedRowHandle;

            // Referencia ao servico de seguranca
            IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();

            // Pede a lista
            List<TicketRiscoInfo> lista =
                ((ListarTicketsRiscoResponse)
                    servicoRisco.ListarTicketsRisco(
                        new ListarTicketsRiscoRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao
                        })).Resultado;

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
            CadastroTicketsRiscoParametros parametros2 = parametros as CadastroTicketsRiscoParametros;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion

        private void salvar(TicketRiscoInfo info)
        {
            // Referencia ao servico de seguranca
            IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();

            // Salva 
            servicoRisco.SalvarTicketRisco(
                new SalvarTicketRiscoRequest()
                {
                    CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                    TicketRiscoInfo = info
                });

            // Atualiza a lista
            carregarLista();
        }

        private void cmdAdicionar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Cria novo ticket
            TicketRiscoInfo info = new TicketRiscoInfo();

            // Mostra tela de detalhe e salva se ok
            TicketRiscoDetalhe controleDetalhe = new TicketRiscoDetalhe(info);
            FormDialog frm = new FormDialog(controleDetalhe, FormDialogTipoEnum.OkCancelar);
            if (frm.ShowDialog() == DialogResult.OK)
                salvar(info);
        }

        private void cmdRemover_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Tenta pegar o objeto selecionado
            TicketRiscoInfo info = (TicketRiscoInfo)this.grdv.GetFocusedRow();
            if (info != null)
            {
                // Referencia ao servico de risco
                IServicoRisco servicoRisco = Ativador.Get<IServicoRisco>();

                // Solicita remoção
                servicoRisco.RemoverTicketRisco(
                    new RemoverTicketRiscoRequest()
                    {
                        CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                        CodigoTicketRisco = info.CodigoTicketRisco
                    });

                // Atualiza a lista
                carregarLista();
            }
        }
    }
}

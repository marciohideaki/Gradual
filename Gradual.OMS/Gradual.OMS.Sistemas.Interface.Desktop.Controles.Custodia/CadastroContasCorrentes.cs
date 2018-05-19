using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.ContaCorrente;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.ContaCorrente.Mensagens;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Custodia
{
    public partial class CadastroContasCorrentes : XtraUserControl, IControle
    {
        private Controle _item = null;
        private InterfaceContextoOMS _contexto = null;

        public CadastroContasCorrentes()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroCustodias_Load);
            this.cmdAdicionar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(cmdAdicionar_ItemClick);
            this.cmdAtualizarLista.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(cmdAtualizarLista_ItemClick);
        }

        private void CadastroCustodias_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo design
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Carrega lista de usuários
                carregarLista();

                // Captura eventos
                grdv.DoubleClick += new EventHandler(grdv_DoubleClick);
            }
        }

        private void grdv_DoubleClick(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            ContaCorrenteInfo info = (ContaCorrenteInfo)this.grdv.GetFocusedRow();
            if (info != null)
            {
                // Mostra detalhe do usuario e salva se ok
                ContaCorrenteDetalhe formDetalhe = new ContaCorrenteDetalhe(info);
                FormDialog frm = new FormDialog(formDetalhe, FormDialogTipoEnum.OkCancelar);
                if (frm.ShowDialog() == DialogResult.OK)
                    salvar(info);
            }
        }

        private void carregarLista()
        {
            // Referencia ao servico de seguranca
            IServicoContaCorrente servicoContaCorrente = Ativador.Get<IServicoContaCorrente>();

            // Pede a lista
            List<ContaCorrenteInfo> lista =
                ((ConsultarContasCorrentesResponse)
                    servicoContaCorrente.ConsultarContasCorrentes(
                        new ConsultarContasCorrentesRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao
                        })).ContasCorrentes;

            // Associa ao grid
            grd.DataSource = lista;
        }

        #region IControle Members

        public void Inicializar(Controle controle)
        {
            _item = controle;
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

        private void salvar(ContaCorrenteInfo contaCorrenteInfo)
        {
            // Referencia ao servico de seguranca
            IServicoContaCorrente servicoContaCorrente = Ativador.Get<IServicoContaCorrente>();

            // Salva o usuário
            servicoContaCorrente.SalvarContaCorrente(
                new SalvarContaCorrenteRequest()
                {
                    CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                    ContaCorrenteInfo = contaCorrenteInfo
                });

            // Atualiza a lista
            carregarLista();
        }

        private void cmdAtualizarLista_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Recarrega a lista
            carregarLista();
        }

        private void cmdAdicionar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Cria nova custodia
            ContaCorrenteInfo info = new ContaCorrenteInfo();

            // Mostra tela de detalhe e salva se ok
            ContaCorrenteDetalhe controleDetalhe = new ContaCorrenteDetalhe(info);
            FormDialog frm = new FormDialog(controleDetalhe, FormDialogTipoEnum.OkCancelar);
            if (frm.ShowDialog() == DialogResult.OK)
                salvar(info);
        }
    }
}

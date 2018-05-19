using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    public partial class CadastroPerfis : DevExpress.XtraEditors.XtraUserControl, IControle
    {
        private Controle _item = null;
        private InterfaceContextoOMS _contexto = null;

        public CadastroPerfis()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroPerfis_Load);
        }

        private void CadastroPerfis_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo design
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Carrega lista de usuários
                carregarListaPerfis();

                // Captura eventos
                grdv.DoubleClick += new EventHandler(grdv_DoubleClick);
            }
        }

        private void grdv_DoubleClick(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            PerfilInfo perfilInfo = (PerfilInfo)this.grdv.GetFocusedRow();
            if (perfilInfo != null)
            {
                // Mostra detalhe do usuario e salva se ok
                PerfilDetalhe perfilDetalhe = new PerfilDetalhe(perfilInfo);
                FormDialog frm = new FormDialog(perfilDetalhe, FormDialogTipoEnum.OkCancelar);
                if (frm.ShowDialog() == DialogResult.OK)
                    salvarPerfil(perfilInfo);
            }
        }

        private void carregarListaPerfis()
        {
            // Referencia ao servico de seguranca
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

            // Pede a lista
            List<PerfilInfo> perfis =
                ((ListarPerfisResponse)
                    servicoAutenticador.ProcessarMensagem(
                        new ListarPerfisRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao
                        })).Perfis;

            // Associa ao grid
            grd.DataSource = perfis;
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

        private void salvarPerfil(PerfilInfo perfilInfo)
        {
            // Referencia ao servico de seguranca
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

            // Salva o usuário
            servicoAutenticador.ProcessarMensagem(
                new SalvarPerfilRequest()
                {
                    CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                    Perfil = perfilInfo
                });

            // Atualiza a lista
            carregarListaPerfis();
        }

        private void cmdAdicionar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Cria novo usuario 
            PerfilInfo perfilInfo = new PerfilInfo();

            // Mostra tela de detalhe e salva usuario se ok
            PerfilDetalhe perfilDetalhe = new PerfilDetalhe(perfilInfo);
            FormDialog frm = new FormDialog(perfilDetalhe, FormDialogTipoEnum.OkCancelar);
            if (frm.ShowDialog() == DialogResult.OK)
                salvarPerfil(perfilInfo);
        }

        private void cmdRemover_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Tenta pegar o objeto selecionado
            PerfilInfo perfilInfo = (PerfilInfo)this.grdv.GetFocusedRow();
            if (perfilInfo != null)
            {
                // Referencia ao servico de seguranca
                IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

                // Solicita remoção
                servicoAutenticador.ProcessarMensagem(
                    new RemoverPerfilRequest()
                    {
                        CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                        CodigoPerfil = perfilInfo.CodigoPerfil
                    });

                // Atualiza a lista
                carregarListaPerfis();
            }
        }
    }
}

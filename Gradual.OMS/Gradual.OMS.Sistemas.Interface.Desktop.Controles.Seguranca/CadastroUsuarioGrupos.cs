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
    public partial class CadastroUsuarioGrupos : DevExpress.XtraEditors.XtraUserControl, IControle
    {
        private Controle _item = null;
        private InterfaceContextoOMS _contexto = null;

        public CadastroUsuarioGrupos()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroUsuarioGrupos_Load);
        }

        private void CadastroUsuarioGrupos_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo design
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Carrega lista de grupos de usuários
                carregarListaUsuarioGrupos();

                // Captura eventos
                grdv.DoubleClick += new EventHandler(grdv_DoubleClick);
            }
        }

        private void grdv_DoubleClick(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            UsuarioGrupoInfo usuarioGrupoInfo = (UsuarioGrupoInfo)this.grdv.GetFocusedRow();
            if (usuarioGrupoInfo != null)
            {
                // Mostra detalhe do grupo de usuario e salva se ok
                UsuarioGrupoDetalhe usuarioGrupoDetalhe = new UsuarioGrupoDetalhe(usuarioGrupoInfo);
                FormDialog frm = new FormDialog(usuarioGrupoDetalhe, FormDialogTipoEnum.OkCancelar);
                if (frm.ShowDialog() == DialogResult.OK)
                    salvarUsuarioGrupo(usuarioGrupoInfo);
            }
        }

        private void carregarListaUsuarioGrupos()
        {
            // Referencia ao servico de seguranca
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

            // Pede a lista
            List<UsuarioGrupoInfo> usuarioGrupos =
                ((ListarUsuarioGruposResponse)
                    servicoAutenticador.ProcessarMensagem(
                        new ListarUsuarioGruposRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao
                        })).UsuarioGrupos;

            // Associa ao grid
            grd.DataSource = usuarioGrupos;

            // Pede para o contexto recarregar os grupos
            _contexto.CarregarUsuarioGrupos(usuarioGrupos);
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

        private void salvarUsuarioGrupo(UsuarioGrupoInfo usuarioGrupoInfo)
        {
            // Referencia ao servico de seguranca
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

            // Salva o usuário
            servicoAutenticador.ProcessarMensagem(
                new SalvarUsuarioGrupoRequest()
                {
                    CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                    UsuarioGrupo = usuarioGrupoInfo
                });

            // Atualiza a lista
            carregarListaUsuarioGrupos();
        }

        private void cmdAdicionar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Cria novo usuario grupo
            UsuarioGrupoInfo usuarioGrupoInfo = new UsuarioGrupoInfo();

            // Mostra tela de detalhe e salva usuario se ok
            UsuarioGrupoDetalhe usuarioGrupoDetalhe = new UsuarioGrupoDetalhe(usuarioGrupoInfo);
            FormDialog frm = new FormDialog(usuarioGrupoDetalhe, FormDialogTipoEnum.OkCancelar);
            if (frm.ShowDialog() == DialogResult.OK)
                salvarUsuarioGrupo(usuarioGrupoInfo);

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Integracao.Sinacor.OMS;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens;
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
    public partial class CadastroUsuarios : DevExpress.XtraEditors.XtraUserControl, IControle
    {
        private Controle _item = null;
        private InterfaceContextoOMS _contexto = null;
        private CadastroUsuarioParametros _parametros = new CadastroUsuarioParametros();
        
        public CadastroUsuarios()
        {
            InitializeComponent();

            this.Load += new EventHandler(CadastroUsuarios_Load);
        }

        private void CadastroUsuarios_Load(object sender, EventArgs e)
        {
            // Apenas se não estiver em modo design
            if (!this.DesignMode)
            {
                // Pega o contexto
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
                _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

                // Carrega lista de usuários
                carregarListaUsuarios();

                // Carrega os layouts dos controles
                _parametros.LayoutsDevExpress.RecuperarLayouts(this);

                // Captura eventos
                grdv.DoubleClick += new EventHandler(grdv_DoubleClick);
            }
        }

        private void grdv_DoubleClick(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            UsuarioInfo usuarioInfo = (UsuarioInfo)this.grdv.GetFocusedRow();
            if (usuarioInfo != null)
            {
                // Carrega o usuario
                IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();
                UsuarioInfo usuarioInfo2 = 
                    ((ReceberUsuarioResponse)
                        servicoAutenticador.ProcessarMensagem(
                            new ReceberUsuarioRequest() 
                            { 
                                CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                                CodigoUsuario = usuarioInfo.CodigoUsuario
                            })).Usuario;

                // Mostra detalhe do usuario e salva se ok
                UsuarioDetalhe usuarioDetalhe = new UsuarioDetalhe(usuarioInfo2);
                usuarioDetalhe.CarregarParametros(_parametros.ParametrosUsuarioDetalhe, EventoManipulacaoParametrosEnum.Persistencia);
                FormDialog frm = new FormDialog(usuarioDetalhe, FormDialogTipoEnum.OkCancelar);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Salva usuario
                    salvarUsuario(usuarioInfo2);

                    // Pede para salvar as tabs complementares
                    usuarioDetalhe.SalvarTabs();

                    // Recarega a lista
                    carregarListaUsuarios();
                }
                
                // Salva layout da tela de detalhe
                _parametros.ParametrosUsuarioDetalhe =
                    (UsuarioDetalheParametros)
                        usuarioDetalhe.SalvarParametros(EventoManipulacaoParametrosEnum.Persistencia);
            }
        }

        private void carregarListaUsuarios()
        {
            // Referencia ao servico de seguranca
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

            // Pede a lista
            List<UsuarioInfo> usuarios =
                ((ListarUsuariosResponse)
                    servicoAutenticador.ProcessarMensagem(
                        new ListarUsuariosRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao
                        })).Usuarios;

            // Associa ao grid
            grd.DataSource = usuarios;
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
            CadastroUsuarioParametros parametros2 = parametros as CadastroUsuarioParametros;
            if (parametros2 != null)
                _parametros = parametros2;
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion

        private void salvarUsuario(UsuarioInfo usuarioInfo)
        {
            // Referencia ao servico de seguranca
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

            // Salva o usuário
            servicoAutenticador.ProcessarMensagem(
                new SalvarUsuarioRequest()
                {
                    CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                    Usuario = usuarioInfo
                });

            // Atualiza a lista
            carregarListaUsuarios();
        }

        private void cmdAdicionar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Cria novo usuario 
            UsuarioInfo usuarioInfo = new UsuarioInfo();

            // Mostra tela de detalhe e salva usuario se ok
            UsuarioDetalhe usuarioDetalhe = new UsuarioDetalhe(usuarioInfo);
            usuarioDetalhe.CarregarParametros(_parametros.ParametrosUsuarioDetalhe, EventoManipulacaoParametrosEnum.Persistencia);
            FormDialog frm = new FormDialog(usuarioDetalhe, FormDialogTipoEnum.OkCancelar);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Salva o usuario
                salvarUsuario(usuarioInfo);

                // Pede para salvar as tabs complementares
                usuarioDetalhe.SalvarTabs();
            }

            // Salva layout da tela de detalhe
            _parametros.ParametrosUsuarioDetalhe =
                (UsuarioDetalheParametros)
                    usuarioDetalhe.SalvarParametros(EventoManipulacaoParametrosEnum.Persistencia);
        }

        private void cmdRemover_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Tenta pegar o objeto selecionado
            UsuarioInfo usuarioInfo = (UsuarioInfo)this.grdv.GetFocusedRow();
            if (usuarioInfo != null)
            {
                // Referencia ao servico de seguranca
                IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();

                // Solicita remoção
                servicoAutenticador.ProcessarMensagem(
                    new RemoverUsuarioRequest() 
                    { 
                        CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                        CodigoUsuario = usuarioInfo.CodigoUsuario
                    });

                // Atualiza a lista
                carregarListaUsuarios();
            }
        }

        private void cmdSincronizarCustodia_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Tenta pegar o objeto selecionado
            UsuarioInfo usuarioInfo = (UsuarioInfo)this.grdv.GetFocusedRow();
            if (usuarioInfo != null)
            {
                // Pede a sincronizacao de custodia
                IServicoIntegracaoSinacorOMS servicoIntegracaoSinacor = Ativador.Get<IServicoIntegracaoSinacorOMS>();
                SincronizarCustodiaResponse sincronizarCustodiaResponse = 
                    servicoIntegracaoSinacor.SincronizarCustodia(
                        new SincronizarCustodiaRequest() 
                        { 
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                            CodigoUsuario = usuarioInfo.CodigoUsuario
                        });

                // Se deu erro, informa o resultado
                if (sincronizarCustodiaResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                    MessageBox.Show(
                        "Erro na sincronização: " + sincronizarCustodiaResponse.DescricaoResposta, 
                        "Erro na sincronização", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdSincronizarContaCorrente_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Tenta pegar o objeto selecionado
            UsuarioInfo usuarioInfo = (UsuarioInfo)this.grdv.GetFocusedRow();
            if (usuarioInfo != null)
            {
                // Pede a sincronizacao de conta corrente
                IServicoIntegracaoSinacorOMS servicoIntegracaoSinacor = Ativador.Get<IServicoIntegracaoSinacorOMS>();
                SincronizarContaCorrenteResponse sincronizarContaCorrenteResponse =
                    servicoIntegracaoSinacor.SincronizarContaCorrente(
                        new SincronizarContaCorrenteRequest()
                        {
                            CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                            CodigoUsuario = usuarioInfo.CodigoUsuario
                        });

                // Se deu erro, informa o resultado
                if (sincronizarContaCorrenteResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                    MessageBox.Show(
                        "Erro na sincronização: " + sincronizarContaCorrenteResponse.DescricaoResposta,
                        "Erro na sincronização",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdSincronizarSinacor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Tenta pegar o objeto selecionado
            UsuarioInfo usuarioInfo = (UsuarioInfo)this.grdv.GetFocusedRow();
            if (usuarioInfo != null)
            {
                // Solicita a inicialização
                InicializarUsuarioResponse respostaInicializar =
                    (InicializarUsuarioResponse)
                        Ativador.Get<IServicoIntegracaoSinacorOMS>().InicializarUsuario(
                            new InicializarUsuarioRequest()
                            {
                                CodigoSessao = _contexto.SessaoInfo.CodigoSessao,
                                CodigoUsuario = usuarioInfo.CodigoUsuario,
                                SincronizarContaCorrente = true,
                                SincronizarCustodia = true,
                                SincronizarContaInvestimento = true,
                                SincronizarContaMargem = true,
                                InferirCBLCInvestimento = true
                            });

                // Se deu erro, informa o resultado
                if (respostaInicializar.StatusResposta != MensagemResponseStatusEnum.OK)
                    MessageBox.Show(
                        "Erro na sincronização: " + respostaInicializar.DescricaoResposta,
                        "Erro na sincronização",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

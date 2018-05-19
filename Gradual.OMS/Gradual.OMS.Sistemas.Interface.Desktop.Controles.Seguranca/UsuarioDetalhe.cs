using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Seguranca.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Seguranca.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    public partial class UsuarioDetalhe : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// Parametros da tela
        /// </summary>
        private UsuarioDetalheParametros _parametros = new UsuarioDetalheParametros();

        private InterfaceContextoOMS _contexto = null;

        private UsuarioInfo _usuarioInfo = null;
        private List<UsuarioGrupoInfo> _usuarioGrupos = new List<UsuarioGrupoInfo>();
        private List<PerfilInfo> _perfis = new List<PerfilInfo>();
        private Dictionary<TabComplementarInfo, IControle> _tabsComplementares = new Dictionary<TabComplementarInfo, IControle>();

        public UsuarioDetalhe(UsuarioInfo usuarioInfo)
        {
            // Inicializa
            InitializeComponent();
            _usuarioInfo = usuarioInfo;

            // Pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Adiciona os grupos
            foreach (string usuarioGrupo in _usuarioInfo.Grupos)
                if (_contexto.UsuarioGrupos.ContainsKey(usuarioGrupo))
                    _usuarioGrupos.Add(_contexto.UsuarioGrupos[usuarioGrupo]);

            // Adiciona os perfis
            foreach (string perfil in _usuarioInfo.Perfis)
                if (_contexto.Perfis.ContainsKey(perfil))
                    _perfis.Add(_contexto.Perfis[perfil]);

            // Adiciona os complementos
            foreach (object obj in _usuarioInfo.Complementos.Colecao)
                cmbComplemento.Properties.Items.Add(obj);
            if (cmbComplemento.Properties.Items.Count > 0)
                cmbComplemento.SelectedIndex = 0;

            // Cria tabs complementares
            ControlesSegurancaConfig config = GerenciadorConfig.ReceberConfig<ControlesSegurancaConfig>();
            foreach (TabComplementarInfo tabComplementarInfo in config.TabsComplementaresUsuario)
            {
                // Cria o controle
                IControle tabComplementar = (IControle)Activator.CreateInstance(tabComplementarInfo.TipoControle);

                // Adiciona na coleção
                _tabsComplementares.Add(tabComplementarInfo, tabComplementar);
            }

            // Captura os eventos
            this.Load += new EventHandler(UsuarioDetalhe_Load);
        }

        public void SalvarTabs()
        {
            // Cria mensagem de sinalização de salvar para as tabs
            SinalizarSalvarUsuarioRequest mensagemSalvar =
                new SinalizarSalvarUsuarioRequest()
                {
                    Usuario = _usuarioInfo
                };

            // Sinaliza
            foreach (KeyValuePair<TabComplementarInfo, IControle> item in _tabsComplementares)
                item.Value.ProcessarMensagem(mensagemSalvar);
        }

        private void UsuarioDetalhe_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                // Mensagem de inicializacao para as tabs complementares
                SinalizarInicializarUsuarioRequest mensagemInicializacao = 
                    new SinalizarInicializarUsuarioRequest() 
                    { 
                        Usuario = _usuarioInfo
                    };

                // Inicializa as tabs complementares
                ControlesSegurancaConfig config = GerenciadorConfig.ReceberConfig<ControlesSegurancaConfig>();
                foreach (KeyValuePair<TabComplementarInfo, IControle> item in _tabsComplementares)
                {
                    // Referencia
                    IControle tabComplementar = item.Value;

                    // Pede inicializacao
                    tabComplementar.ProcessarMensagem(mensagemInicializacao);

                    // Cria a tab
                    tabbedControlGroup1.AddTabPage(item.Key.Titulo).AddItem("", (Control)tabComplementar).TextVisible = false;
                }

                // Seta tab inicial
                tabbedControlGroup1.SelectedTabPage = grupoUsuario;

                // Solicita a carga da tela
                carregarTela();
            }
        }

        private void carregarTela()
        {
            // Repositório de lista de permissoes
            repPermissoes.DataSource = _contexto.Permissoes.Values.ToList();
            
            // Carrega elementos do usuário
            ppgUsuario.SelectedObject = _usuarioInfo;
            grdUsuarioGrupo.DataSource = _usuarioGrupos;
            grdPerfil.DataSource = _perfis;
            grdPermissao.DataSource = _usuarioInfo.Permissoes;
            grdRelacao.DataSource = _usuarioInfo.Relacoes;

            // Informa inicialização para os tabs complementares
        }

        private void cmdUsuarioGrupoAdicionar_Click(object sender, EventArgs e)
        {
            // Mostra janela de escolha de grupos
            FormUsuarioGrupoLista frm = new FormUsuarioGrupoLista();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Adiciona na lista do usuario
                _usuarioInfo.Grupos.Add(frm.UsuarioGrupo.CodigoUsuarioGrupo);
                
                // Adiciona na lista interna
                _usuarioGrupos.Add(frm.UsuarioGrupo);

                // Refresh no grid
                grdUsuarioGrupo.DataSource = null;
                grdUsuarioGrupo.DataSource = _usuarioGrupos;
            }
        }

        private void cmdPermissaoAdicionar_Click(object sender, EventArgs e)
        {
            FormAssociarPermissao frm = new FormAssociarPermissao();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Cria permissao associada
                PermissaoAssociadaInfo permissaoAssociada = new PermissaoAssociadaInfo();
                permissaoAssociada.CodigoPermissao = frm.Permissao.PermissaoInfo.CodigoPermissao;
                permissaoAssociada.Status = frm.PermissaoStatus;

                // Adiciona na coleção
                _usuarioInfo.Permissoes.Add(permissaoAssociada);

                // Atualiza lista de permissoes
                grdPermissao.DataSource = null;
                grdPermissao.DataSource = _usuarioInfo.Permissoes;
            }
        }

        private void cmdPermissaoRemover_Click(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            PermissaoAssociadaInfo permissao = (PermissaoAssociadaInfo)this.grdvPermissao.GetFocusedRow();
            if (permissao != null)
            {
                // Remove a linha
                _usuarioInfo.Permissoes.Remove(permissao);

                // Atualiza a lista
                grdPermissao.DataSource = null;
                grdPermissao.DataSource = _usuarioInfo.Permissoes;
            }
        }

        private void cmdUsuarioGrupoRemover_Click(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            UsuarioGrupoInfo usuarioGrupo = (UsuarioGrupoInfo)this.grdvUsuarioGrupo.GetFocusedRow();
            if (usuarioGrupo != null)
            {
                // Remove as linhas
                _usuarioInfo.Grupos.Remove(usuarioGrupo.CodigoUsuarioGrupo);
                _usuarioGrupos.Remove(usuarioGrupo);

                // Atualiza a lista
                grdUsuarioGrupo.DataSource = null;
                grdUsuarioGrupo.DataSource = _usuarioGrupos;
            }
        }

        private void cmdPerfilAdicionar_Click(object sender, EventArgs e)
        {
            // Mostra janela de escolha de grupos
            FormPerfilLista frm = new FormPerfilLista();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Adiciona na lista do usuario
                _usuarioInfo.Perfis.Add(frm.Perfil.CodigoPerfil);

                // Adiciona na lista interna
                _perfis.Add(frm.Perfil);

                // Refresh no grid
                grdPerfil.DataSource = null;
                grdPerfil.DataSource = _perfis;
            }
        }

        private void cmdPerfilRemover_Click(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            PerfilInfo perfilInfo = (PerfilInfo)this.grdvPerfil.GetFocusedRow();
            if (perfilInfo != null)
            {
                // Remove as linhas
                _usuarioInfo.Perfis.Remove(perfilInfo.CodigoPerfil);
                _perfis.Remove(perfilInfo);

                // Atualiza a lista
                grdPerfil.DataSource = null;
                grdPerfil.DataSource = _perfis;
            }
        }

        private void cmdRelacaoAdicionarSuperior_Click(object sender, EventArgs e)
        {
            // Chama o formulario para adicionar relacao
            FormAssociarUsuarioRelacao frm = 
                new FormAssociarUsuarioRelacao(
                    _usuarioInfo, FormAssociarUsuarioRelacaoVisaoEnum.Superior);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Adiciona a relacao
                _usuarioInfo.Relacoes.Add(frm.UsuarioRelacao);

                // Atualiza o grid
                grdRelacao.DataSource = null;
                grdRelacao.DataSource = _usuarioInfo.Relacoes;
            }
        }

        private void cmdRelacaoAdicionarSubordinado_Click(object sender, EventArgs e)
        {
            // Chama o formulario para adicionar relacao
            FormAssociarUsuarioRelacao frm =
                new FormAssociarUsuarioRelacao(
                    _usuarioInfo, FormAssociarUsuarioRelacaoVisaoEnum.Subordinado);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Adiciona a relacao
                _usuarioInfo.Relacoes.Add(frm.UsuarioRelacao);

                // Atualiza o grid
                grdRelacao.DataSource = null;
                grdRelacao.DataSource = _usuarioInfo.Relacoes;
            }
        }

        private void cmdRelacaoRemover_Click(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            UsuarioRelacaoInfo relacaoInfo = (UsuarioRelacaoInfo)this.grdvPerfil.GetFocusedRow();
            if (relacaoInfo != null)
            {
                // Remove as linhas
                _usuarioInfo.Relacoes.Remove(relacaoInfo);

                // Atualiza a lista
                grdRelacao.DataSource = null;
                grdRelacao.DataSource = _usuarioInfo.Relacoes;
            }
        }

        private void cmbComplemento_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbComplemento.SelectedItem != null)
                ppgComplemento.SelectedObject = cmbComplemento.SelectedItem;
        }

        public object SalvarParametros(EventoManipulacaoParametrosEnum evento)
        {
            // Salva layouts
            _parametros.LayoutsDevExpress.SalvarLayout(this.layoutControl);

            // Salva os parametros das tabs complementares
            _parametros.ParametrosTabsComplementares.Clear();
            foreach (KeyValuePair<TabComplementarInfo, IControle> item in _tabsComplementares)
                _parametros.ParametrosTabsComplementares.Add(item.Value.GetType(), item.Value.SalvarParametros(evento));

            // Retorna
            return _parametros;
        }

        public void CarregarParametros(object parametros, EventoManipulacaoParametrosEnum evento)
        {
            // Seta a classe de parametros. Será utilizada no load do controle
            UsuarioDetalheParametros parametros2 = parametros as UsuarioDetalheParametros;
            if (parametros2 != null)
            {
                _parametros = parametros2;

                // Caso exista, repassa os parametros de inicialização do controle
                foreach (KeyValuePair<TabComplementarInfo, IControle> item in _tabsComplementares)
                    if (parametros2.ParametrosTabsComplementares.ContainsKey(item.Value.GetType()))
                        item.Value.CarregarParametros(parametros2.ParametrosTabsComplementares[item.Value.GetType()], evento);
            }
        }
    }
}

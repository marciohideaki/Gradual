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
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    public partial class UsuarioGrupoDetalhe : DevExpress.XtraEditors.XtraUserControl
    {
        private InterfaceContextoOMS _contexto = null;

        private UsuarioGrupoInfo _usuarioGrupoInfo = null;
        private List<PerfilInfo> _perfis = new List<PerfilInfo>();

        public UsuarioGrupoDetalhe(UsuarioGrupoInfo usuarioGrupoInfo)
        {
            // Inicializa
            InitializeComponent();
            _usuarioGrupoInfo = usuarioGrupoInfo;

            // Pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Adiciona os perfis
            foreach (string perfil in _usuarioGrupoInfo.Perfis)
                _perfis.Add(_contexto.Perfis[perfil]);

            // Captura os eventos
            this.Load += new EventHandler(UsuarioGrupoDetalhe_Load);
        }

        private void UsuarioGrupoDetalhe_Load(object sender, EventArgs e)
        {
            carregarTela();
        }

        private void carregarTela()
        {
            // Repositório de lista de permissoes
            repPermissoes.DataSource = _contexto.Permissoes.Values.ToList();

            // Carrega elementos do grupo
            ppg.SelectedObject = _usuarioGrupoInfo;
            grdPerfil.DataSource = _perfis;
            grdPermissao.DataSource = _usuarioGrupoInfo.Permissoes;
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
                _usuarioGrupoInfo.Permissoes.Add(permissaoAssociada);

                // Atualiza lista de permissoes
                grdPermissao.DataSource = null;
                grdPermissao.DataSource = _usuarioGrupoInfo.Permissoes;
            }
        }

        private void cmdPermissaoRemover_Click(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            PermissaoAssociadaInfo permissao = (PermissaoAssociadaInfo)this.grdvPermissao.GetFocusedRow();
            if (permissao != null)
            {
                // Remove a linha
                _usuarioGrupoInfo.Permissoes.Remove(permissao);

                // Atualiza a lista
                grdPermissao.DataSource = null;
                grdPermissao.DataSource = _usuarioGrupoInfo.Permissoes;
            }
        }

        private void cmdPerfilAdicionar_Click(object sender, EventArgs e)
        {
            // Mostra janela de escolha de grupos
            FormPerfilLista frm = new FormPerfilLista();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Adiciona na lista do usuario
                _usuarioGrupoInfo.Perfis.Add(frm.Perfil.CodigoPerfil);

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
                _usuarioGrupoInfo.Perfis.Remove(perfilInfo.CodigoPerfil);
                _perfis.Remove(perfilInfo);

                // Atualiza a lista
                grdPerfil.DataSource = null;
                grdPerfil.DataSource = _perfis;
            }
        }
    }
}

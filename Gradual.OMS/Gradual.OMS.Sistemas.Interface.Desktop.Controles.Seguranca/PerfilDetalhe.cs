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
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    public partial class PerfilDetalhe : DevExpress.XtraEditors.XtraUserControl
    {
        private InterfaceContextoOMS _contexto = null;

        private PerfilInfo _perfilInfo = null;
        
        public PerfilDetalhe(PerfilInfo perfilInfo)
        {
            // Inicializa
            InitializeComponent();
            _perfilInfo = perfilInfo;

            // Pega o contexto
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            _contexto = servicoInterface.Contexto.ReceberItem<InterfaceContextoOMS>();

            // Captura os eventos
            this.Load += new EventHandler(PerfilDetalhe_Load);
        }
        
        private void PerfilDetalhe_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
                carregarTela();
        }

        private void carregarTela()
        {
            // Repositório de lista de permissoes
            repPermissoes.DataSource = _contexto.Permissoes.Values.ToList();

            // Carrega elementos do usuário
            ppg.SelectedObject = _perfilInfo;
            grdPermissao.DataSource = _perfilInfo.Permissoes;
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
                _perfilInfo.Permissoes.Add(permissaoAssociada);

                // Atualiza lista de permissoes
                grdPermissao.DataSource = null;
                grdPermissao.DataSource = _perfilInfo.Permissoes;
            }
        }

        private void cmdPermissaoRemover_Click(object sender, EventArgs e)
        {
            // Tenta pegar o objeto selecionado
            PermissaoAssociadaInfo permissao = (PermissaoAssociadaInfo)this.grdvPermissao.GetFocusedRow();
            if (permissao != null)
            {
                // Remove a linha
                _perfilInfo.Permissoes.Remove(permissao);

                // Atualiza a lista
                grdPermissao.DataSource = null;
                grdPermissao.DataSource = _perfilInfo.Permissoes;
            }
        }
    }
}

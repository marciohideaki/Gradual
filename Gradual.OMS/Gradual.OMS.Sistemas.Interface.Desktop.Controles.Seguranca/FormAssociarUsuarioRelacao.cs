using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    public partial class FormAssociarUsuarioRelacao : XtraForm
    {
        private UsuarioInfo _usuarioSuperior = null;
        private UsuarioInfo _usuarioSubordinado = null;
        private FormAssociarUsuarioRelacaoVisaoEnum _visao = FormAssociarUsuarioRelacaoVisaoEnum.Superior;

        public UsuarioRelacaoInfo UsuarioRelacao { get; set; }

        public FormAssociarUsuarioRelacao(UsuarioInfo usuarioBase, FormAssociarUsuarioRelacaoVisaoEnum visao)
        {
            InitializeComponent();

            // Inicializa o form
            inicializar();

            // Mostra visao informada e define o que é o usuario informado
            _visao = visao;
            if (_visao == FormAssociarUsuarioRelacaoVisaoEnum.Superior)
            {
                _usuarioSubordinado = usuarioBase; 
                txtUsuarioSubordinado.Text = _usuarioSubordinado.CodigoUsuario;
                txtUsuarioSubordinado.Enabled = false;
            }
            else
            {
                _usuarioSuperior = usuarioBase;
                txtUsuarioSuperior.Text = _usuarioSuperior.CodigoUsuario;
                txtUsuarioSuperior.Enabled = false;
            }
        }

        private void inicializar()
        {
            // Carrega lista de tipos de relacoes
            foreach (string tipoRelacao in Enum.GetNames(typeof(UsuarioRelacaoTipoEnum)))
                cmbRelacao.Properties.Items.Add(tipoRelacao);
        }

        private void cmdSalvar_Click(object sender, EventArgs e)
        {
            // Traduz o tipo da relacao
            UsuarioRelacaoTipoEnum tipoRelacao = 
                (UsuarioRelacaoTipoEnum)Enum.Parse(
                    typeof(UsuarioRelacaoTipoEnum), 
                    (string)cmbRelacao.EditValue);
            
            // Cria a relacao
            this.UsuarioRelacao = 
                new UsuarioRelacaoInfo() 
                { 
                    CodigoUsuario1 = _usuarioSuperior.CodigoUsuario,
                    CodigoUsuario2 = _usuarioSubordinado.CodigoUsuario,
                    TipoRelacao = tipoRelacao 
                };

            // Retorna
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtUsuarioSuperior_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FormUsuarioLista frm = new FormUsuarioLista();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                _usuarioSuperior = frm.Usuario;
                txtUsuarioSuperior.Text = _usuarioSuperior.CodigoUsuario;
            }
        }

        private void txtUsuarioSubordinado_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FormUsuarioLista frm = new FormUsuarioLista();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                _usuarioSubordinado = frm.Usuario;
                txtUsuarioSubordinado.Text = _usuarioSubordinado.CodigoUsuario;
            }
        }
    }
}

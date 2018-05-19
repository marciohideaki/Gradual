using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.SkinDevExpress
{
    public partial class FormLogin : DevExpress.XtraEditors.XtraForm
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        public string Usuario { get; set; }
        public string Senha { get; set; }
        public SessaoInfo SessaoInfo { get; set; }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            // Valida
            IServicoAutenticador servicoAutenticador = Ativador.Get<IServicoAutenticador>();
            AutenticarUsuarioResponse autenticarResponse =
                (AutenticarUsuarioResponse)
                    servicoAutenticador.ProcessarMensagem(
                        new AutenticarUsuarioRequest() 
                        { 
                            CodigoUsuario = txtUsuario.Text,
                            Senha = txtSenha.Text
                        });

            // Verifica se deu certo
            if (autenticarResponse.Sessao == null)
            {
                // Mostra mensagem
                MessageBox.Show("Erro ao autenticar", "Erro na autenticação", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Retorna
                return;
            }

            // Retorna
            this.SessaoInfo = autenticarResponse.Sessao;
            this.Usuario = txtUsuario.Text;
            this.Senha = txtSenha.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            // Retorna
            this.SessaoInfo = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
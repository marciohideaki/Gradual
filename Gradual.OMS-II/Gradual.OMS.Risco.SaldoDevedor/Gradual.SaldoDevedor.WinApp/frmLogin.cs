using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.SaldoDevedor.WinApp
{
    public partial class frmLogin : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static System.Threading.Thread _threadProcessar = null;

        private System.Windows.Forms.AutoCompleteStringCollection _usuarios = null;

        private static frmLogin _formularioDeLogin;
        public static frmLogin FormularioDeLogin
        {
            get
            {
                if (_formularioDeLogin == null)
                    _formularioDeLogin = new frmLogin();

                return _formularioDeLogin;
            }
        }

        public frmLogin()
        {
            CheckForIllegalCrossThreadCalls = false;

            this.Text = "";
            this.ControlBox = false;

            InitializeComponent();

            var pos                             = this.PointToScreen(lblMensagem.Location);
            pos                                 = pictureBox1.PointToClient(pos);
            
            lblMensagem.Parent                  = pictureBox1;
            lblMensagem.BackColor               = Color.Transparent;
            
            chkManterAutenticacao.Parent        = pictureBox1;
            chkManterAutenticacao.BackColor     = Color.Transparent;

            picLoader.Parent = pictureBox1;
            picLoader.BackColor = Color.Transparent;

            _usuarios                           = Classes.Aplicacao.ListaUsuarios;
            txtUsuario.AutoCompleteCustomSource = _usuarios;


            if (!Classes.Aplicacao.UsuarioPadrao.Usuario.ToString().Equals(String.Empty) && !Classes.Aplicacao.UsuarioPadrao.Senha.ToString().Equals(String.Empty))
            {
                txtUsuario.Text = Classes.Aplicacao.UsuarioPadrao.Usuario.ToString();
                txtSenha.Text = Classes.Aplicacao.UsuarioPadrao.Senha.ToString();

                txtSenha.UseSystemPasswordChar = true;
                chkManterAutenticacao.Checked = true;
                
                btnEntrar.Focus();
            }
            
            txtUsuario.Focus();
        }

        private void Autenticar()
        {
            Gradual.OMS.Seguranca.Lib.AutenticarUsuarioResponse lAutenticarResponse = new Gradual.OMS.Seguranca.Lib.AutenticarUsuarioResponse();
            try
            {
                Classes.Seguranca lSeguranca = new Classes.Seguranca();

                String _usuario = txtUsuario.Text;
                String _senha = txtSenha.Text;
                String _host = System.Net.Dns.GetHostName();
                lAutenticarResponse = lSeguranca.AutenticarUsuario(_usuario, _senha, _host, "");

                if (lAutenticarResponse.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    bool lRespostaPermissao = lSeguranca.VerificarPermissoes(lAutenticarResponse.Sessao.CodigoUsuario, lAutenticarResponse.Sessao.CodigoSessao);

                    if (lRespostaPermissao)
                    {
                        if (this.chkManterAutenticacao.Checked)
                        {
                            Classes.Preferencias.SalvarUsuario(_usuario, _senha);
                        }
                        else
                        {
                            Classes.Preferencias.SalvarUsuario(_usuario, _senha, true);
                        }

                        this.Invoke(new System.Windows.Forms.MethodInvoker(delegate()
                        {
                            frmLogin.FormularioDeLogin.Owner.Show();
                        }));

                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        logger.Info("Falha na autenticação. Usuário não possui permissão de acesso ao GTI.");

                        this.Invoke(new System.Windows.Forms.MethodInvoker(delegate()
                        {
                            UpdateText("Usuário sem permissão de acesso ao GTI!");

                            this.btnEntrar.Enabled = true;
                            this.txtUsuario.Enabled = true;
                            this.txtSenha.Enabled = true;
                        }));
                    }
                    //if (!this.DialogResult.Equals(DialogResult.OK) && )
                    //{
                    //    logger.Info("Falha na autenticação. Usuário não possui permissão de acesso ao GTI.");

                    //    this.Invoke(new System.Windows.Forms.MethodInvoker(delegate()
                    //    {
                    //        UpdateText("Usuário sem permissão de acesso ao GTI!");

                    //        this.btnEntrar.Enabled = true;
                    //        this.txtUsuario.Enabled = true;
                    //        this.txtSenha.Enabled = true;
                    //    }));
                    //}
                }
                else
                {
                    this.Invoke(new System.Windows.Forms.MethodInvoker(delegate()
                    {
                        logger.Info("Falha na autenticação. Usuário ou senha inválidos.");
                        //MessageBox.Show("Usuário ou senha inválidos!");
                        UpdateText("Usuário ou senha inválidos!");

                        this.btnEntrar.Enabled = true;
                        this.txtUsuario.Enabled = true;
                        this.txtSenha.Enabled = true;
                    }));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("MESSAGE[{0}] \n\n SOURCE[{1}] \n\n STACK[{2}]", ex.Message, ex.Source, ex.StackTrace);
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnEntrar.Enabled = false;
                this.txtUsuario.Enabled = false;
                this.txtSenha.Enabled = false;

                if (_threadProcessar == null || !_threadProcessar.IsAlive)
                {
                    _threadProcessar = new System.Threading.Thread(new System.Threading.ThreadStart(Autenticar));
                }

                _threadProcessar.Start();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("MESSAGE[{0}] \n\n SOURCE[{1}] \n\n STACK[{2}]", ex.Message, ex.Source, ex.StackTrace);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_threadProcessar != null)
                {
                    if (_threadProcessar.IsAlive)
                    {
                        logger.Info("Abortando o processo responsável pela autenticação.");
                        try
                        {
                            _threadProcessar.Abort();
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("MESSAGE[{0}] \n\n SOURCE[{1}] \n\n STACK[{2}]", ex.Message, ex.Source, ex.StackTrace);
                        }
                    }
                }

                logger.Info("Autenticação cancelada pelo usuário.");

                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("MESSAGE[{0}] \n\n SOURCE[{1}] \n\n STACK[{2}]", ex.Message, ex.Source, ex.StackTrace);
            }
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                switch (e.CloseReason)
                {
                    case CloseReason.ApplicationExitCall:
                        {
                            logger.Info("Autenticação cancelada pela aplicação.");
                            this.DialogResult = DialogResult.Cancel;
                            break;
                        }

                    case CloseReason.FormOwnerClosing:
                        {
                            logger.Info("Autenticação cancelada pelo proprietário.");
                            this.DialogResult = DialogResult.Cancel;
                            break;
                        }

                    case CloseReason.MdiFormClosing:
                        {
                            logger.Info("Autenticação cancelada pelo Mdi.");
                            this.DialogResult = DialogResult.Cancel;
                            break;
                        }

                    case CloseReason.None:
                        {

                            if (this.DialogResult.Equals(DialogResult.OK))
                            {
                                logger.Info("Autenticação realizada com sucesso.");
                            }
                            else
                            {
                                logger.Info("Autenticação cancelada pelo usuário ou sem razão aparente.");
                                this.DialogResult = DialogResult.Cancel;
                            }

                            break;
                        }

                    case CloseReason.TaskManagerClosing:
                        {
                            logger.Info("Autenticação cancelada pelo gerenciador de processos.");
                            this.DialogResult = DialogResult.Cancel;
                            break;
                        }

                    case CloseReason.UserClosing:
                        {
                            logger.Info("Autenticação cancelada pelo usuário.");
                            this.DialogResult = DialogResult.Cancel;
                            break;
                        }

                    case CloseReason.WindowsShutDown:
                        {
                            logger.Info("Autenticação cancelada pelo Windows.");
                            this.DialogResult = DialogResult.Cancel;
                            break;
                        }

                    default:
                        {
                            logger.Info("Não foi possível recuperar o motivo do cancelamento da autenticação.");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("MESSAGE[{0}] \n\n SOURCE[{1}] \n\n STACK[{2}]", ex.Message, ex.Source, ex.StackTrace);
            }
        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtUsuario.Text.Trim().Length > 0)
                    {
                        if (txtSenha.Text.Trim().Length > 0)
                        {
                            this.btnEntrar.PerformClick();
                        }
                        else
                        {
                            MessageBox.Show("Digite sua senha!");
                            txtSenha.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Digite o usuário!");
                        txtUsuario.Focus();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("MESSAGE[{0}] \n\n SOURCE[{1}] \n\n STACK[{2}]", ex.Message, ex.Source, ex.StackTrace);
            }
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtUsuario.Text.Trim().Length > 0)
                    {
                        if (Classes.Aplicacao.Usuarios.ContainsKey(txtUsuario.Text))
                        {
                            txtSenha.Clear();
                            txtSenha.Text = Classes.Aplicacao.Usuarios[txtUsuario.Text];
                            chkManterAutenticacao.Checked = true;
                        }

                        if (txtSenha.Text.Trim().Length > 0)
                        {
                            this.btnEntrar.PerformClick();
                        }
                        else
                        {
                            txtSenha.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Digite o usuário!");
                        txtUsuario.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("MESSAGE[{0}] \n\n SOURCE[{1}] \n\n STACK[{2}]", ex.Message, ex.Source, ex.StackTrace);
            }
        }

        private delegate void UpdateTextDelegate(object value);

        private void UpdateText(object value)
        {
            if (this.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                this.Invoke(new UpdateTextDelegate(this.UpdateText), value);
            }
            else
            {
                // This is the UI thread so perform the task.
                this.lblMensagem.Text = value.ToString();
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Trim().Length > 0)
            {
                if (Classes.Aplicacao.Usuarios.ContainsKey(txtUsuario.Text))
                {
                    txtSenha.UseSystemPasswordChar = true;
                    txtSenha.Clear();
                    txtSenha.Text = Classes.Aplicacao.Usuarios[txtUsuario.Text];
                    chkManterAutenticacao.Checked = true;
                }
                else
                {
                    //txtSenha.Clear();
                }
            }
            else
            {
                txtUsuario.Text = "[usuário]";
                
                //txtSenha.Text = "[senha]";
            }
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Trim().Equals("[usuário]"))
            {
                txtUsuario.Text = String.Empty;
            }
        }

        private void txtSenha_Enter(object sender, EventArgs e)
        {
            if (txtSenha.Text.Trim().Equals("[senha]"))
            {
                txtSenha.UseSystemPasswordChar = true;
                txtSenha.Text = String.Empty;
            }
        }

        private void txtSenha_Leave(object sender, EventArgs e)
        {
            if (txtSenha.Text.Trim().Equals(String.Empty))
            {
                txtSenha.UseSystemPasswordChar = false;
                txtSenha.Text = "[senha]";
            }
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = false;
            txtSenha.Text = "[senha]";
        }
    }
}

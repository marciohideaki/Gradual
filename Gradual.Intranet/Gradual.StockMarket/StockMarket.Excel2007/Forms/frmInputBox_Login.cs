using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Instalador.Controlador;
using System.Diagnostics;
using System.Configuration;
using System.IO;

namespace StockMarket.Excel2007
{
    public partial class frmInputBox_Login : Form
    {
        private AutoCompleteStringCollection _usuarios = null;
        private Dictionary<string, string> _dictUsuarios = null;
        private KeyValuePair<string, string> _usuarioPadrao;
        private Configuration _config;

        #region Globais

        public string CONST_ID_SISTEMA_STOCKMARKET = "E2E91F40-6038-4503-833A-3181CDB573D1";

        #endregion

        #region Propriedades

        public string CodigoEmail
        {
            get
            {
                return txtCodigoEmail.Text;
            }

            set
            {
                txtCodigoEmail.Text = value;
            }
        }

        public string Senha
        {
            get
            {
                return txtSenha.Text;
            }

            set
            {
                txtSenha.Text = value;
            }
        }

        #endregion

        #region Event Handlers

        public frmInputBox_Login()
        {
            InitializeComponent();

            _dictUsuarios = new Dictionary<string, string>();
            _usuarioPadrao = new KeyValuePair<string, string>("", "");
            _usuarios = ConfiguracaoUsuarios();
            txtCodigoEmail.AutoCompleteCustomSource = _usuarios;
            lblMensagem.Text = "";

            if (!_usuarioPadrao.Key.Equals(String.Empty) && !_usuarioPadrao.Value.Equals(String.Empty))
            {
                txtCodigoEmail.Text = _usuarioPadrao.Key;
                txtSenha.Text = _usuarioPadrao.Value;

                chkManterAutenticacao.Checked = true;
                txtSenha.UseSystemPasswordChar = true;
            }

            try
            {
                if (!Debugger.IsAttached)
                {
                    Controlador lControlador = new Controlador();

                    lControlador.LancarVerificacaoDeAtualizacoes(CONST_ID_SISTEMA_STOCKMARKET);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erro ao buscar atualizações:\r\n{0}\r\n\r\n{1}", ex.Message, ex.StackTrace));
            }

            txtCodigoEmail.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void txtSenha_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void txtCodigoEmail_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        public void GravarConfiguracao()
        {
            StringBuilder usuarioPadrao = new StringBuilder();

            if (chkManterAutenticacao.Checked)
                usuarioPadrao.AppendFormat("{0}:{1}",
                    txtCodigoEmail.Text,
                    System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(txtSenha.Text)));

            _config.AppSettings.Settings["UsuarioPadrao"].Value = usuarioPadrao.ToString();

            if (_dictUsuarios.ContainsKey(txtCodigoEmail.Text))
            {
                if (!chkManterAutenticacao.Checked)
                    _dictUsuarios.Remove(txtCodigoEmail.Text);
                else
                    _dictUsuarios[txtCodigoEmail.Text] = txtSenha.Text;
            }
            else
            {
                _dictUsuarios.Add(txtCodigoEmail.Text, txtSenha.Text);
            }

            StringBuilder usuarios = new StringBuilder();
            foreach (var pair in _dictUsuarios)
            {
                if (!chkManterAutenticacao.Checked)
                    if (pair.Key.Equals(txtCodigoEmail.Text))
                        continue;
                usuarios.AppendFormat("{0}:{1},", pair.Key, System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pair.Value)));
            }

            _config.AppSettings.Settings["Usuarios"].Value = usuarios.ToString();

            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private AutoCompleteStringCollection ConfiguracaoUsuarios()
        {
            AutoCompleteStringCollection lista = new AutoCompleteStringCollection();

            _config = LoadLocalConfigurationFile();
            //MessageBox.Show(_config.FilePath);

            string configUsuarioPadrao = _config.AppSettings.Settings["UsuarioPadrao"].Value;
            string[] usuariosPadrao = configUsuarioPadrao.Split(',');
            for (int i = 0; i < usuariosPadrao.Length; i++)
            {
                string[] usuarioPadrao = usuariosPadrao[i].Split(':');
                if (usuarioPadrao[0].Length > 0)
                {
                    string senhaDecriptada = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(usuarioPadrao[1]));
                    _usuarioPadrao = new KeyValuePair<string,string>(usuarioPadrao[0], senhaDecriptada);
                }
            }

            string configUsuarios = _config.AppSettings.Settings["Usuarios"].Value;
            string[] usuarios = configUsuarios.Split(',');
            for (int i = 0; i < usuarios.Length; i++)
            {
                string[] usuario = usuarios[i].Split(':');
                if (usuario[0].Length > 0)
                {
                    string senhaDecriptada = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(usuario[1]));
                    _dictUsuarios.Add(usuario[0], senhaDecriptada);
                }
            }

            foreach (var pair in _dictUsuarios)
                lista.Add(pair.Key);

            return lista;
        }

        private Configuration LoadLocalConfigurationFile()
        {
            if (System.Configuration.ConfigurationManager.AppSettings.Count == 0)
            {
                string arquivoConfig = System.Reflection.Assembly.GetAssembly(typeof(StockMarket.Excel2007.Globals)).Location + ".config";
                var configMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, arquivoConfig)
                };
                return System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            }
            else
            {
                return System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
        }

        #endregion

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtCodigoEmail.Text.Trim().Length > 0)
                {
                    if (txtSenha.Text.Trim().Length > 0)
                    {
                        this.btnLogin.PerformClick();
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
                    txtCodigoEmail.Focus();
                }
            }
        }

        private void txtCodigoEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtCodigoEmail.Text.Trim().Length > 0)
                {
                    if (_dictUsuarios.ContainsKey(txtCodigoEmail.Text))
                    {
                        txtSenha.Clear();
                        txtSenha.Text = _dictUsuarios[txtCodigoEmail.Text];
                        chkManterAutenticacao.Checked = true;
                    }

                    if (txtSenha.Text.Trim().Length > 0)
                        this.btnLogin.PerformClick();
                    else
                        txtSenha.Focus();
                }
                else
                {
                    MessageBox.Show("Digite o usuário!");
                    txtCodigoEmail.Focus();
                }
            }
        }

        private void txtCodigoEmail_Leave(object sender, EventArgs e)
        {
            if (txtCodigoEmail.Text.Trim().Length > 0)
            {
                if (_dictUsuarios.ContainsKey(txtCodigoEmail.Text))
                {
                    txtSenha.UseSystemPasswordChar = true;
                    txtSenha.Clear();
                    txtSenha.Text = _dictUsuarios[txtCodigoEmail.Text];
                    chkManterAutenticacao.Checked = true;
                }
            }
            else
            {
                txtCodigoEmail.Text = "[usuário]";
            }
        }

        private void txtCodigoEmail_Enter(object sender, EventArgs e)
        {
            if (txtCodigoEmail.Text.Trim().Equals("[usuário]"))
            {
                txtCodigoEmail.Text = String.Empty;
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

        private void txtCodigoEmail_TextChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = false;
            txtSenha.Text = "[senha]";
        }
    }
}

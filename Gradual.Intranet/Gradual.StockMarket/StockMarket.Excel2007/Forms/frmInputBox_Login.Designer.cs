namespace StockMarket.Excel2007
{
    partial class frmInputBox_Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInputBox_Login));
            this.label1 = new System.Windows.Forms.Label();
            this.txtCodigoEmail = new System.Windows.Forms.TextBox();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.chkManterAutenticacao = new System.Windows.Forms.CheckBox();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Código ou Email:";
            // 
            // txtCodigoEmail
            // 
            this.txtCodigoEmail.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtCodigoEmail.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtCodigoEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCodigoEmail.Location = new System.Drawing.Point(109, 15);
            this.txtCodigoEmail.Name = "txtCodigoEmail";
            this.txtCodigoEmail.Size = new System.Drawing.Size(175, 20);
            this.txtCodigoEmail.TabIndex = 1;
            this.txtCodigoEmail.Text = "[usuário]";
            this.txtCodigoEmail.TextChanged += new System.EventHandler(this.txtCodigoEmail_TextChanged);
            this.txtCodigoEmail.Enter += new System.EventHandler(this.txtCodigoEmail_Enter);
            this.txtCodigoEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodigoEmail_KeyDown);
            this.txtCodigoEmail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCodigoEmail_KeyUp);
            this.txtCodigoEmail.Leave += new System.EventHandler(this.txtCodigoEmail_Leave);
            // 
            // txtSenha
            // 
            this.txtSenha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSenha.Location = new System.Drawing.Point(109, 44);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(175, 20);
            this.txtSenha.TabIndex = 3;
            this.txtSenha.Text = "[senha]";
            this.txtSenha.Enter += new System.EventHandler(this.txtSenha_Enter);
            this.txtSenha.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSenha_KeyDown);
            this.txtSenha.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSenha_KeyUp);
            this.txtSenha.Leave += new System.EventHandler(this.txtSenha_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Senha:";
            // 
            // btnLogin
            // 
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Location = new System.Drawing.Point(209, 81);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Location = new System.Drawing.Point(34, 81);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // chkManterAutenticacao
            // 
            this.chkManterAutenticacao.AutoSize = true;
            this.chkManterAutenticacao.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkManterAutenticacao.Location = new System.Drawing.Point(177, 122);
            this.chkManterAutenticacao.Name = "chkManterAutenticacao";
            this.chkManterAutenticacao.Size = new System.Drawing.Size(134, 17);
            this.chkManterAutenticacao.TabIndex = 6;
            this.chkManterAutenticacao.Text = "Salvar usuário e senha";
            this.chkManterAutenticacao.UseVisualStyleBackColor = true;
            // 
            // lblMensagem
            // 
            this.lblMensagem.AutoSize = true;
            this.lblMensagem.ForeColor = System.Drawing.Color.Red;
            this.lblMensagem.Location = new System.Drawing.Point(12, 122);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(59, 13);
            this.lblMensagem.TabIndex = 7;
            this.lblMensagem.Text = "Mensagem";
            // 
            // frmInputBox_Login
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(321, 140);
            this.Controls.Add(this.lblMensagem);
            this.Controls.Add(this.chkManterAutenticacao);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCodigoEmail);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInputBox_Login";
            this.Text = "Login Gradual - v. 14-08-18";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCodigoEmail;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.CheckBox chkManterAutenticacao;
        private System.Windows.Forms.Label lblMensagem;
    }
}
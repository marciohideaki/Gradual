namespace Gradual.SerieHistorica
{
    partial class Form1
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
            this.grpOrigem = new System.Windows.Forms.GroupBox();
            this.lblArquivo4 = new System.Windows.Forms.Label();
            this.lblArquivo3 = new System.Windows.Forms.Label();
            this.lblArquivo2 = new System.Windows.Forms.Label();
            this.lblArquivo1 = new System.Windows.Forms.Label();
            this.btnSelecioneArquivo = new System.Windows.Forms.Button();
            this.grpDestino = new System.Windows.Forms.GroupBox();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblSenha = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtBancoDados = new System.Windows.Forms.TextBox();
            this.txtServidorIP = new System.Windows.Forms.TextBox();
            this.lblBancoDados = new System.Windows.Forms.Label();
            this.lblServidorIP = new System.Windows.Forms.Label();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnFinalizar = new System.Windows.Forms.Button();
            this.txtProcessamento = new System.Windows.Forms.TextBox();
            this.lblProcessamento = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.progressBar4 = new System.Windows.Forms.ProgressBar();
            this.grpOrigem.SuspendLayout();
            this.grpDestino.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpOrigem
            // 
            this.grpOrigem.Controls.Add(this.progressBar4);
            this.grpOrigem.Controls.Add(this.progressBar3);
            this.grpOrigem.Controls.Add(this.progressBar2);
            this.grpOrigem.Controls.Add(this.progressBar1);
            this.grpOrigem.Controls.Add(this.lblArquivo4);
            this.grpOrigem.Controls.Add(this.lblArquivo3);
            this.grpOrigem.Controls.Add(this.lblArquivo2);
            this.grpOrigem.Controls.Add(this.lblArquivo1);
            this.grpOrigem.Controls.Add(this.btnSelecioneArquivo);
            this.grpOrigem.Location = new System.Drawing.Point(7, 3);
            this.grpOrigem.Name = "grpOrigem";
            this.grpOrigem.Size = new System.Drawing.Size(315, 183);
            this.grpOrigem.TabIndex = 0;
            this.grpOrigem.TabStop = false;
            this.grpOrigem.Text = "Origem";
            // 
            // lblArquivo4
            // 
            this.lblArquivo4.AutoSize = true;
            this.lblArquivo4.Location = new System.Drawing.Point(13, 142);
            this.lblArquivo4.Name = "lblArquivo4";
            this.lblArquivo4.Size = new System.Drawing.Size(52, 13);
            this.lblArquivo4.TabIndex = 6;
            this.lblArquivo4.Text = "Arquivo 4";
            // 
            // lblArquivo3
            // 
            this.lblArquivo3.AutoSize = true;
            this.lblArquivo3.Location = new System.Drawing.Point(13, 117);
            this.lblArquivo3.Name = "lblArquivo3";
            this.lblArquivo3.Size = new System.Drawing.Size(52, 13);
            this.lblArquivo3.TabIndex = 5;
            this.lblArquivo3.Text = "Arquivo 3";
            // 
            // lblArquivo2
            // 
            this.lblArquivo2.AutoSize = true;
            this.lblArquivo2.Location = new System.Drawing.Point(13, 92);
            this.lblArquivo2.Name = "lblArquivo2";
            this.lblArquivo2.Size = new System.Drawing.Size(52, 13);
            this.lblArquivo2.TabIndex = 4;
            this.lblArquivo2.Text = "Arquivo 2";
            // 
            // lblArquivo1
            // 
            this.lblArquivo1.AutoSize = true;
            this.lblArquivo1.Location = new System.Drawing.Point(13, 67);
            this.lblArquivo1.Name = "lblArquivo1";
            this.lblArquivo1.Size = new System.Drawing.Size(52, 13);
            this.lblArquivo1.TabIndex = 3;
            this.lblArquivo1.Text = "Arquivo 1";
            // 
            // btnSelecioneArquivo
            // 
            this.btnSelecioneArquivo.Location = new System.Drawing.Point(34, 19);
            this.btnSelecioneArquivo.Name = "btnSelecioneArquivo";
            this.btnSelecioneArquivo.Size = new System.Drawing.Size(246, 23);
            this.btnSelecioneArquivo.TabIndex = 2;
            this.btnSelecioneArquivo.Text = "Selecione Arquivos para Importação";
            this.btnSelecioneArquivo.UseVisualStyleBackColor = true;
            this.btnSelecioneArquivo.Click += new System.EventHandler(this.btnSelecioneArquivo_Click);
            // 
            // grpDestino
            // 
            this.grpDestino.Controls.Add(this.txtSenha);
            this.grpDestino.Controls.Add(this.txtUsuario);
            this.grpDestino.Controls.Add(this.lblSenha);
            this.grpDestino.Controls.Add(this.lblUsuario);
            this.grpDestino.Controls.Add(this.txtBancoDados);
            this.grpDestino.Controls.Add(this.txtServidorIP);
            this.grpDestino.Controls.Add(this.lblBancoDados);
            this.grpDestino.Controls.Add(this.lblServidorIP);
            this.grpDestino.Location = new System.Drawing.Point(7, 191);
            this.grpDestino.Name = "grpDestino";
            this.grpDestino.Size = new System.Drawing.Size(315, 150);
            this.grpDestino.TabIndex = 1;
            this.grpDestino.TabStop = false;
            this.grpDestino.Text = "Destino";
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(118, 103);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(100, 20);
            this.txtSenha.TabIndex = 7;
            this.txtSenha.UseSystemPasswordChar = true;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(118, 77);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(100, 20);
            this.txtUsuario.TabIndex = 6;
            // 
            // lblSenha
            // 
            this.lblSenha.AutoSize = true;
            this.lblSenha.Location = new System.Drawing.Point(76, 106);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(41, 13);
            this.lblSenha.TabIndex = 5;
            this.lblSenha.Text = "Senha:";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(71, 80);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 4;
            this.lblUsuario.Text = "Usuário:";
            // 
            // txtBancoDados
            // 
            this.txtBancoDados.Location = new System.Drawing.Point(118, 50);
            this.txtBancoDados.Name = "txtBancoDados";
            this.txtBancoDados.Size = new System.Drawing.Size(100, 20);
            this.txtBancoDados.TabIndex = 3;
            // 
            // txtServidorIP
            // 
            this.txtServidorIP.Location = new System.Drawing.Point(118, 24);
            this.txtServidorIP.Name = "txtServidorIP";
            this.txtServidorIP.Size = new System.Drawing.Size(100, 20);
            this.txtServidorIP.TabIndex = 2;
            // 
            // lblBancoDados
            // 
            this.lblBancoDados.AutoSize = true;
            this.lblBancoDados.Location = new System.Drawing.Point(26, 53);
            this.lblBancoDados.Name = "lblBancoDados";
            this.lblBancoDados.Size = new System.Drawing.Size(90, 13);
            this.lblBancoDados.TabIndex = 1;
            this.lblBancoDados.Text = "Banco de Dados:";
            // 
            // lblServidorIP
            // 
            this.lblServidorIP.AutoSize = true;
            this.lblServidorIP.Location = new System.Drawing.Point(54, 27);
            this.lblServidorIP.Name = "lblServidorIP";
            this.lblServidorIP.Size = new System.Drawing.Size(62, 13);
            this.lblServidorIP.TabIndex = 0;
            this.lblServidorIP.Text = "Servidor IP:";
            // 
            // btnImportar
            // 
            this.btnImportar.Location = new System.Drawing.Point(7, 353);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(120, 32);
            this.btnImportar.TabIndex = 2;
            this.btnImportar.Text = "Importar";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnFinalizar
            // 
            this.btnFinalizar.Location = new System.Drawing.Point(608, 353);
            this.btnFinalizar.Name = "btnFinalizar";
            this.btnFinalizar.Size = new System.Drawing.Size(75, 32);
            this.btnFinalizar.TabIndex = 3;
            this.btnFinalizar.Text = "Finalizar";
            this.btnFinalizar.UseVisualStyleBackColor = true;
            this.btnFinalizar.Click += new System.EventHandler(this.btnFinalizar_Click);
            // 
            // txtProcessamento
            // 
            this.txtProcessamento.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcessamento.Location = new System.Drawing.Point(331, 20);
            this.txtProcessamento.Multiline = true;
            this.txtProcessamento.Name = "txtProcessamento";
            this.txtProcessamento.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProcessamento.Size = new System.Drawing.Size(352, 321);
            this.txtProcessamento.TabIndex = 4;
            this.txtProcessamento.WordWrap = false;
            // 
            // lblProcessamento
            // 
            this.lblProcessamento.AutoSize = true;
            this.lblProcessamento.Location = new System.Drawing.Point(328, 4);
            this.lblProcessamento.Name = "lblProcessamento";
            this.lblProcessamento.Size = new System.Drawing.Size(83, 13);
            this.lblProcessamento.TabIndex = 5;
            this.lblProcessamento.Text = "Processamento:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(191, 64);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(111, 16);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 7;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(191, 88);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(111, 17);
            this.progressBar2.Step = 1;
            this.progressBar2.TabIndex = 8;
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(191, 113);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(111, 17);
            this.progressBar3.Step = 1;
            this.progressBar3.TabIndex = 9;
            // 
            // progressBar4
            // 
            this.progressBar4.Location = new System.Drawing.Point(191, 138);
            this.progressBar4.Name = "progressBar4";
            this.progressBar4.Size = new System.Drawing.Size(111, 17);
            this.progressBar4.Step = 1;
            this.progressBar4.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 395);
            this.Controls.Add(this.lblProcessamento);
            this.Controls.Add(this.txtProcessamento);
            this.Controls.Add(this.btnFinalizar);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.grpDestino);
            this.Controls.Add(this.grpOrigem);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Importação de Dados de Série Histórica";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpOrigem.ResumeLayout(false);
            this.grpOrigem.PerformLayout();
            this.grpDestino.ResumeLayout(false);
            this.grpDestino.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpOrigem;
        private System.Windows.Forms.GroupBox grpDestino;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnFinalizar;
        private System.Windows.Forms.Button btnSelecioneArquivo;
        private System.Windows.Forms.Label lblArquivo1;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtBancoDados;
        private System.Windows.Forms.TextBox txtServidorIP;
        private System.Windows.Forms.Label lblBancoDados;
        private System.Windows.Forms.Label lblServidorIP;
        private System.Windows.Forms.TextBox txtProcessamento;
        private System.Windows.Forms.Label lblProcessamento;
        private System.Windows.Forms.Label lblArquivo4;
        private System.Windows.Forms.Label lblArquivo3;
        private System.Windows.Forms.Label lblArquivo2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar4;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.ProgressBar progressBar2;
    }
}


namespace Gradual.SaldoDevedor.WinApp
{
    partial class frmConfiguracao
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
            this.pnlConfiguracao = new System.Windows.Forms.Panel();
            this.btnSalvarConfiguracoes = new System.Windows.Forms.Button();
            this.btnFecharConfig = new System.Windows.Forms.Button();
            this.pnlParametros = new System.Windows.Forms.Panel();
            this.lblDescrParametros = new System.Windows.Forms.Label();
            this.txtLimiteSaldo = new System.Windows.Forms.TextBox();
            this.lblDescrLimiteSaldo = new System.Windows.Forms.Label();
            this.txtCodArqTesouraria = new System.Windows.Forms.TextBox();
            this.lblDescCodigoArqTesour = new System.Windows.Forms.Label();
            this.txtTaxaJuros = new System.Windows.Forms.TextBox();
            this.lblDescrTaxaJuros = new System.Windows.Forms.Label();
            this.pnlClientesExcecao = new System.Windows.Forms.Panel();
            this.lstClientesIncluidos = new System.Windows.Forms.ListBox();
            this.txtIncluirCliente = new System.Windows.Forms.TextBox();
            this.lblDescrInserirClientes = new System.Windows.Forms.Label();
            this.lblDescrClientesExcecao = new System.Windows.Forms.Label();
            this.pnlAssessoresExcecao = new System.Windows.Forms.Panel();
            this.lstAssessoresIncluidos = new System.Windows.Forms.ListBox();
            this.txtIncluirAssessor = new System.Windows.Forms.TextBox();
            this.lblDescrInserirAssessores = new System.Windows.Forms.Label();
            this.lblDescrExcecaoAssessores = new System.Windows.Forms.Label();
            this.btnExcluirCliente = new System.Windows.Forms.Button();
            this.btnExcluirAssessor = new System.Windows.Forms.Button();
            this.pnlConfiguracao.SuspendLayout();
            this.pnlParametros.SuspendLayout();
            this.pnlClientesExcecao.SuspendLayout();
            this.pnlAssessoresExcecao.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlConfiguracao
            // 
            this.pnlConfiguracao.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConfiguracao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.pnlConfiguracao.Controls.Add(this.btnSalvarConfiguracoes);
            this.pnlConfiguracao.Controls.Add(this.btnFecharConfig);
            this.pnlConfiguracao.Controls.Add(this.pnlParametros);
            this.pnlConfiguracao.Controls.Add(this.pnlClientesExcecao);
            this.pnlConfiguracao.Controls.Add(this.pnlAssessoresExcecao);
            this.pnlConfiguracao.Location = new System.Drawing.Point(3, 36);
            this.pnlConfiguracao.Name = "pnlConfiguracao";
            this.pnlConfiguracao.Size = new System.Drawing.Size(653, 325);
            this.pnlConfiguracao.TabIndex = 0;
            // 
            // btnSalvarConfiguracoes
            // 
            this.btnSalvarConfiguracoes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSalvarConfiguracoes.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSalvarConfiguracoes.FlatAppearance.BorderSize = 2;
            this.btnSalvarConfiguracoes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvarConfiguracoes.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalvarConfiguracoes.ForeColor = System.Drawing.Color.White;
            this.btnSalvarConfiguracoes.Location = new System.Drawing.Point(12, 288);
            this.btnSalvarConfiguracoes.Name = "btnSalvarConfiguracoes";
            this.btnSalvarConfiguracoes.Size = new System.Drawing.Size(207, 32);
            this.btnSalvarConfiguracoes.TabIndex = 18;
            this.btnSalvarConfiguracoes.Tag = "SemRenderizacao";
            this.btnSalvarConfiguracoes.Text = "Salvar Configurações";
            this.btnSalvarConfiguracoes.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSalvarConfiguracoes.UseVisualStyleBackColor = false;
            this.btnSalvarConfiguracoes.Click += new System.EventHandler(this.btnSalvarConfiguracoes_Click);
            // 
            // btnFecharConfig
            // 
            this.btnFecharConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFecharConfig.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnFecharConfig.FlatAppearance.BorderSize = 2;
            this.btnFecharConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFecharConfig.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFecharConfig.ForeColor = System.Drawing.Color.White;
            this.btnFecharConfig.Location = new System.Drawing.Point(534, 285);
            this.btnFecharConfig.Name = "btnFecharConfig";
            this.btnFecharConfig.Size = new System.Drawing.Size(113, 35);
            this.btnFecharConfig.TabIndex = 18;
            this.btnFecharConfig.Tag = "SemRenderizacao";
            this.btnFecharConfig.Text = "Cancelar";
            this.btnFecharConfig.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFecharConfig.UseVisualStyleBackColor = false;
            this.btnFecharConfig.Click += new System.EventHandler(this.btnFecharConfig_Click);
            // 
            // pnlParametros
            // 
            this.pnlParametros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.pnlParametros.Controls.Add(this.lblDescrParametros);
            this.pnlParametros.Controls.Add(this.txtLimiteSaldo);
            this.pnlParametros.Controls.Add(this.lblDescrLimiteSaldo);
            this.pnlParametros.Controls.Add(this.txtCodArqTesouraria);
            this.pnlParametros.Controls.Add(this.lblDescCodigoArqTesour);
            this.pnlParametros.Controls.Add(this.txtTaxaJuros);
            this.pnlParametros.Controls.Add(this.lblDescrTaxaJuros);
            this.pnlParametros.Location = new System.Drawing.Point(12, 22);
            this.pnlParametros.Name = "pnlParametros";
            this.pnlParametros.Size = new System.Drawing.Size(200, 259);
            this.pnlParametros.TabIndex = 0;
            // 
            // lblDescrParametros
            // 
            this.lblDescrParametros.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrParametros.ForeColor = System.Drawing.Color.White;
            this.lblDescrParametros.Location = new System.Drawing.Point(4, 2);
            this.lblDescrParametros.Name = "lblDescrParametros";
            this.lblDescrParametros.Size = new System.Drawing.Size(193, 18);
            this.lblDescrParametros.TabIndex = 12;
            this.lblDescrParametros.Tag = "SemRenderizacao";
            this.lblDescrParametros.Text = "Parâmetros";
            this.lblDescrParametros.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtLimiteSaldo
            // 
            this.txtLimiteSaldo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtLimiteSaldo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLimiteSaldo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtLimiteSaldo.Location = new System.Drawing.Point(21, 145);
            this.txtLimiteSaldo.Name = "txtLimiteSaldo";
            this.txtLimiteSaldo.Size = new System.Drawing.Size(138, 23);
            this.txtLimiteSaldo.TabIndex = 16;
            this.txtLimiteSaldo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIncluirAssessor_KeyDown);
            // 
            // lblDescrLimiteSaldo
            // 
            this.lblDescrLimiteSaldo.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrLimiteSaldo.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrLimiteSaldo.Location = new System.Drawing.Point(18, 112);
            this.lblDescrLimiteSaldo.Name = "lblDescrLimiteSaldo";
            this.lblDescrLimiteSaldo.Size = new System.Drawing.Size(141, 32);
            this.lblDescrLimiteSaldo.TabIndex = 15;
            this.lblDescrLimiteSaldo.Tag = "SemRenderizacao";
            this.lblDescrLimiteSaldo.Text = "Limite do Saldo Disponível ( < 0):";
            this.lblDescrLimiteSaldo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCodArqTesouraria
            // 
            this.txtCodArqTesouraria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtCodArqTesouraria.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCodArqTesouraria.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtCodArqTesouraria.Location = new System.Drawing.Point(21, 222);
            this.txtCodArqTesouraria.Name = "txtCodArqTesouraria";
            this.txtCodArqTesouraria.Size = new System.Drawing.Size(138, 23);
            this.txtCodArqTesouraria.TabIndex = 16;
            this.txtCodArqTesouraria.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIncluirAssessor_KeyDown);
            // 
            // lblDescCodigoArqTesour
            // 
            this.lblDescCodigoArqTesour.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescCodigoArqTesour.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescCodigoArqTesour.Location = new System.Drawing.Point(18, 206);
            this.lblDescCodigoArqTesour.Name = "lblDescCodigoArqTesour";
            this.lblDescCodigoArqTesour.Size = new System.Drawing.Size(141, 13);
            this.lblDescCodigoArqTesour.TabIndex = 15;
            this.lblDescCodigoArqTesour.Tag = "SemRenderizacao";
            this.lblDescCodigoArqTesour.Text = "Código Arq. Tesouraria:";
            this.lblDescCodigoArqTesour.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTaxaJuros
            // 
            this.txtTaxaJuros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtTaxaJuros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTaxaJuros.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtTaxaJuros.Location = new System.Drawing.Point(21, 56);
            this.txtTaxaJuros.Name = "txtTaxaJuros";
            this.txtTaxaJuros.Size = new System.Drawing.Size(138, 23);
            this.txtTaxaJuros.TabIndex = 16;
            this.txtTaxaJuros.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIncluirAssessor_KeyDown);
            // 
            // lblDescrTaxaJuros
            // 
            this.lblDescrTaxaJuros.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrTaxaJuros.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrTaxaJuros.Location = new System.Drawing.Point(18, 40);
            this.lblDescrTaxaJuros.Name = "lblDescrTaxaJuros";
            this.lblDescrTaxaJuros.Size = new System.Drawing.Size(141, 13);
            this.lblDescrTaxaJuros.TabIndex = 15;
            this.lblDescrTaxaJuros.Tag = "SemRenderizacao";
            this.lblDescrTaxaJuros.Text = "Taxa de Juros (%):";
            this.lblDescrTaxaJuros.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlClientesExcecao
            // 
            this.pnlClientesExcecao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.pnlClientesExcecao.Controls.Add(this.btnExcluirCliente);
            this.pnlClientesExcecao.Controls.Add(this.lstClientesIncluidos);
            this.pnlClientesExcecao.Controls.Add(this.txtIncluirCliente);
            this.pnlClientesExcecao.Controls.Add(this.lblDescrInserirClientes);
            this.pnlClientesExcecao.Controls.Add(this.lblDescrClientesExcecao);
            this.pnlClientesExcecao.Location = new System.Drawing.Point(447, 22);
            this.pnlClientesExcecao.Name = "pnlClientesExcecao";
            this.pnlClientesExcecao.Size = new System.Drawing.Size(200, 259);
            this.pnlClientesExcecao.TabIndex = 0;
            // 
            // lstClientesIncluidos
            // 
            this.lstClientesIncluidos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lstClientesIncluidos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstClientesIncluidos.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstClientesIncluidos.FormattingEnabled = true;
            this.lstClientesIncluidos.ItemHeight = 15;
            this.lstClientesIncluidos.Location = new System.Drawing.Point(16, 94);
            this.lstClientesIncluidos.Name = "lstClientesIncluidos";
            this.lstClientesIncluidos.Size = new System.Drawing.Size(138, 152);
            this.lstClientesIncluidos.TabIndex = 1;
            // 
            // txtIncluirCliente
            // 
            this.txtIncluirCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtIncluirCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIncluirCliente.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtIncluirCliente.Location = new System.Drawing.Point(16, 56);
            this.txtIncluirCliente.Name = "txtIncluirCliente";
            this.txtIncluirCliente.Size = new System.Drawing.Size(138, 23);
            this.txtIncluirCliente.TabIndex = 16;
            this.txtIncluirCliente.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIncluirCliente_KeyDown);
            // 
            // lblDescrInserirClientes
            // 
            this.lblDescrInserirClientes.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrInserirClientes.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrInserirClientes.Location = new System.Drawing.Point(13, 40);
            this.lblDescrInserirClientes.Name = "lblDescrInserirClientes";
            this.lblDescrInserirClientes.Size = new System.Drawing.Size(141, 13);
            this.lblDescrInserirClientes.TabIndex = 15;
            this.lblDescrInserirClientes.Tag = "SemRenderizacao";
            this.lblDescrInserirClientes.Text = "Inserir Cliente";
            this.lblDescrInserirClientes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescrClientesExcecao
            // 
            this.lblDescrClientesExcecao.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrClientesExcecao.ForeColor = System.Drawing.Color.White;
            this.lblDescrClientesExcecao.Location = new System.Drawing.Point(4, 2);
            this.lblDescrClientesExcecao.Name = "lblDescrClientesExcecao";
            this.lblDescrClientesExcecao.Size = new System.Drawing.Size(193, 18);
            this.lblDescrClientesExcecao.TabIndex = 12;
            this.lblDescrClientesExcecao.Tag = "SemRenderizacao";
            this.lblDescrClientesExcecao.Text = "Clientes Exceção";
            this.lblDescrClientesExcecao.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlAssessoresExcecao
            // 
            this.pnlAssessoresExcecao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.pnlAssessoresExcecao.Controls.Add(this.btnExcluirAssessor);
            this.pnlAssessoresExcecao.Controls.Add(this.lstAssessoresIncluidos);
            this.pnlAssessoresExcecao.Controls.Add(this.txtIncluirAssessor);
            this.pnlAssessoresExcecao.Controls.Add(this.lblDescrInserirAssessores);
            this.pnlAssessoresExcecao.Controls.Add(this.lblDescrExcecaoAssessores);
            this.pnlAssessoresExcecao.Location = new System.Drawing.Point(229, 22);
            this.pnlAssessoresExcecao.Name = "pnlAssessoresExcecao";
            this.pnlAssessoresExcecao.Size = new System.Drawing.Size(200, 259);
            this.pnlAssessoresExcecao.TabIndex = 0;
            // 
            // lstAssessoresIncluidos
            // 
            this.lstAssessoresIncluidos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lstAssessoresIncluidos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstAssessoresIncluidos.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstAssessoresIncluidos.FormattingEnabled = true;
            this.lstAssessoresIncluidos.ItemHeight = 15;
            this.lstAssessoresIncluidos.Location = new System.Drawing.Point(16, 94);
            this.lstAssessoresIncluidos.Name = "lstAssessoresIncluidos";
            this.lstAssessoresIncluidos.Size = new System.Drawing.Size(138, 152);
            this.lstAssessoresIncluidos.TabIndex = 1;
            // 
            // txtIncluirAssessor
            // 
            this.txtIncluirAssessor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtIncluirAssessor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIncluirAssessor.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtIncluirAssessor.Location = new System.Drawing.Point(16, 56);
            this.txtIncluirAssessor.Name = "txtIncluirAssessor";
            this.txtIncluirAssessor.Size = new System.Drawing.Size(138, 23);
            this.txtIncluirAssessor.TabIndex = 16;
            this.txtIncluirAssessor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIncluirAssessor_KeyDown);
            // 
            // lblDescrInserirAssessores
            // 
            this.lblDescrInserirAssessores.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrInserirAssessores.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrInserirAssessores.Location = new System.Drawing.Point(13, 40);
            this.lblDescrInserirAssessores.Name = "lblDescrInserirAssessores";
            this.lblDescrInserirAssessores.Size = new System.Drawing.Size(141, 13);
            this.lblDescrInserirAssessores.TabIndex = 15;
            this.lblDescrInserirAssessores.Tag = "SemRenderizacao";
            this.lblDescrInserirAssessores.Text = "Inserir Assessor";
            this.lblDescrInserirAssessores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescrExcecaoAssessores
            // 
            this.lblDescrExcecaoAssessores.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrExcecaoAssessores.ForeColor = System.Drawing.Color.White;
            this.lblDescrExcecaoAssessores.Location = new System.Drawing.Point(4, 2);
            this.lblDescrExcecaoAssessores.Name = "lblDescrExcecaoAssessores";
            this.lblDescrExcecaoAssessores.Size = new System.Drawing.Size(193, 18);
            this.lblDescrExcecaoAssessores.TabIndex = 12;
            this.lblDescrExcecaoAssessores.Tag = "SemRenderizacao";
            this.lblDescrExcecaoAssessores.Text = "Assessores Exceção";
            this.lblDescrExcecaoAssessores.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnExcluirCliente
            // 
            this.btnExcluirCliente.BackgroundImage = global::Gradual.SaldoDevedor.WinApp.Properties.Resources.Ico_Excluir_ce;
            this.btnExcluirCliente.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExcluirCliente.FlatAppearance.BorderSize = 0;
            this.btnExcluirCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcluirCliente.Location = new System.Drawing.Point(157, 94);
            this.btnExcluirCliente.Name = "btnExcluirCliente";
            this.btnExcluirCliente.Size = new System.Drawing.Size(30, 30);
            this.btnExcluirCliente.TabIndex = 1;
            this.btnExcluirCliente.Tag = "SemRenderizacao";
            this.btnExcluirCliente.UseVisualStyleBackColor = true;
            this.btnExcluirCliente.Click += new System.EventHandler(this.btnExcluirCliente_Click);
            // 
            // btnExcluirAssessor
            // 
            this.btnExcluirAssessor.BackgroundImage = global::Gradual.SaldoDevedor.WinApp.Properties.Resources.Ico_Excluir_ce;
            this.btnExcluirAssessor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExcluirAssessor.FlatAppearance.BorderSize = 0;
            this.btnExcluirAssessor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcluirAssessor.Location = new System.Drawing.Point(157, 94);
            this.btnExcluirAssessor.Name = "btnExcluirAssessor";
            this.btnExcluirAssessor.Size = new System.Drawing.Size(30, 30);
            this.btnExcluirAssessor.TabIndex = 1;
            this.btnExcluirAssessor.Tag = "SemRenderizacao";
            this.btnExcluirAssessor.UseVisualStyleBackColor = true;
            this.btnExcluirAssessor.Click += new System.EventHandler(this.btnExcluirAssessor_Click);
            // 
            // frmConfiguracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 392);
            this.Controls.Add(this.pnlConfiguracao);
            this.Name = "frmConfiguracao";
            this.TamanhoFixo = true;
            this.Text = "..:: Configuração Saldo Devedor ::..";
            this.pnlConfiguracao.ResumeLayout(false);
            this.pnlParametros.ResumeLayout(false);
            this.pnlParametros.PerformLayout();
            this.pnlClientesExcecao.ResumeLayout(false);
            this.pnlClientesExcecao.PerformLayout();
            this.pnlAssessoresExcecao.ResumeLayout(false);
            this.pnlAssessoresExcecao.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlConfiguracao;
        private System.Windows.Forms.Panel pnlAssessoresExcecao;
        private System.Windows.Forms.Label lblDescrExcecaoAssessores;
        private System.Windows.Forms.ListBox lstAssessoresIncluidos;
        private System.Windows.Forms.TextBox txtIncluirAssessor;
        private System.Windows.Forms.Label lblDescrInserirAssessores;
        private System.Windows.Forms.Button btnExcluirAssessor;
        private System.Windows.Forms.Button btnSalvarConfiguracoes;
        private System.Windows.Forms.Button btnFecharConfig;
        private System.Windows.Forms.Panel pnlClientesExcecao;
        private System.Windows.Forms.Button btnExcluirCliente;
        private System.Windows.Forms.ListBox lstClientesIncluidos;
        private System.Windows.Forms.TextBox txtIncluirCliente;
        private System.Windows.Forms.Label lblDescrInserirClientes;
        private System.Windows.Forms.Label lblDescrClientesExcecao;
        private System.Windows.Forms.Panel pnlParametros;
        private System.Windows.Forms.Label lblDescrParametros;
        private System.Windows.Forms.TextBox txtLimiteSaldo;
        private System.Windows.Forms.Label lblDescrLimiteSaldo;
        private System.Windows.Forms.TextBox txtTaxaJuros;
        private System.Windows.Forms.Label lblDescrTaxaJuros;
        private System.Windows.Forms.TextBox txtCodArqTesouraria;
        private System.Windows.Forms.Label lblDescCodigoArqTesour;
    }
}
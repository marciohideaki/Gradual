namespace Gradual.SaldoDevedor.WinApp
{
    partial class frmSaldoDevedor
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
            this.pnlSaldoDevedor = new System.Windows.Forms.Panel();
            this.rbValorDesenquadrado3 = new System.Windows.Forms.RadioButton();
            this.rbValorDesenquadrado2 = new System.Windows.Forms.RadioButton();
            this.rbValorDesenquadrado1 = new System.Windows.Forms.RadioButton();
            this.rbValorDesenquadrado0 = new System.Windows.Forms.RadioButton();
            this.lblAguardeDados = new System.Windows.Forms.Label();
            this.chkExcecoes = new System.Windows.Forms.CheckBox();
            this.chkDesenquadrado = new System.Windows.Forms.CheckBox();
            this.btnConfigEmail = new System.Windows.Forms.Button();
            this.btnConfiguracao = new System.Windows.Forms.Button();
            this.btnNotificarAtendimento = new System.Windows.Forms.Button();
            this.btnNotificarTesouraria = new System.Windows.Forms.Button();
            this.btnHistorico = new System.Windows.Forms.Button();
            this.btnNotificarAssessores = new System.Windows.Forms.Button();
            this.btnFechar = new System.Windows.Forms.Button();
            this.updwQtdDiasAtraso = new System.Windows.Forms.NumericUpDown();
            this.pnlParametros = new System.Windows.Forms.Panel();
            this.lblDescrLimiteMulta = new System.Windows.Forms.Label();
            this.lblDescrTaxaJuros = new System.Windows.Forms.Label();
            this.lblLimiteMulta = new System.Windows.Forms.Label();
            this.lblTaxaJuros = new System.Windows.Forms.Label();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.chklistAssessores = new System.Windows.Forms.CheckedListBox();
            this.lblDataHoraAtualizacao = new System.Windows.Forms.Label();
            this.lblQtdClientes = new System.Windows.Forms.Label();
            this.lblDescrQtdDiasAtraso = new System.Windows.Forms.Label();
            this.lblDescrCliente = new System.Windows.Forms.Label();
            this.lblListaAssessores = new System.Windows.Forms.Label();
            this.lblDescrTotalClientes = new System.Windows.Forms.Label();
            this.lblUltimaAtualizacao = new System.Windows.Forms.Label();
            this.grdClientesDevedores = new GradualForm.Controls.CustomDataGridView();
            this.pnlSaldoDevedor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updwQtdDiasAtraso)).BeginInit();
            this.pnlParametros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClientesDevedores)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSaldoDevedor
            // 
            this.pnlSaldoDevedor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSaldoDevedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.pnlSaldoDevedor.Controls.Add(this.rbValorDesenquadrado3);
            this.pnlSaldoDevedor.Controls.Add(this.rbValorDesenquadrado1);
            this.pnlSaldoDevedor.Controls.Add(this.rbValorDesenquadrado0);
            this.pnlSaldoDevedor.Controls.Add(this.lblAguardeDados);
            this.pnlSaldoDevedor.Controls.Add(this.chkExcecoes);
            this.pnlSaldoDevedor.Controls.Add(this.chkDesenquadrado);
            this.pnlSaldoDevedor.Controls.Add(this.btnConfigEmail);
            this.pnlSaldoDevedor.Controls.Add(this.btnConfiguracao);
            this.pnlSaldoDevedor.Controls.Add(this.btnNotificarAtendimento);
            this.pnlSaldoDevedor.Controls.Add(this.btnNotificarTesouraria);
            this.pnlSaldoDevedor.Controls.Add(this.btnHistorico);
            this.pnlSaldoDevedor.Controls.Add(this.btnNotificarAssessores);
            this.pnlSaldoDevedor.Controls.Add(this.btnFechar);
            this.pnlSaldoDevedor.Controls.Add(this.updwQtdDiasAtraso);
            this.pnlSaldoDevedor.Controls.Add(this.pnlParametros);
            this.pnlSaldoDevedor.Controls.Add(this.txtCliente);
            this.pnlSaldoDevedor.Controls.Add(this.chklistAssessores);
            this.pnlSaldoDevedor.Controls.Add(this.lblDataHoraAtualizacao);
            this.pnlSaldoDevedor.Controls.Add(this.lblQtdClientes);
            this.pnlSaldoDevedor.Controls.Add(this.lblDescrQtdDiasAtraso);
            this.pnlSaldoDevedor.Controls.Add(this.lblDescrCliente);
            this.pnlSaldoDevedor.Controls.Add(this.lblListaAssessores);
            this.pnlSaldoDevedor.Controls.Add(this.lblDescrTotalClientes);
            this.pnlSaldoDevedor.Controls.Add(this.lblUltimaAtualizacao);
            this.pnlSaldoDevedor.Controls.Add(this.grdClientesDevedores);
            this.pnlSaldoDevedor.Controls.Add(this.rbValorDesenquadrado2);
            this.pnlSaldoDevedor.Location = new System.Drawing.Point(3, 36);
            this.pnlSaldoDevedor.Name = "pnlSaldoDevedor";
            this.pnlSaldoDevedor.Size = new System.Drawing.Size(1148, 520);
            this.pnlSaldoDevedor.TabIndex = 0;
            // 
            // rbValorDesenquadrado3
            // 
            this.rbValorDesenquadrado3.AutoSize = true;
            this.rbValorDesenquadrado3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbValorDesenquadrado3.ForeColor = System.Drawing.Color.DarkGray;
            this.rbValorDesenquadrado3.Location = new System.Drawing.Point(648, 87);
            this.rbValorDesenquadrado3.Name = "rbValorDesenquadrado3";
            this.rbValorDesenquadrado3.Size = new System.Drawing.Size(251, 18);
            this.rbValorDesenquadrado3.TabIndex = 24;
            this.rbValorDesenquadrado3.TabStop = true;
            this.rbValorDesenquadrado3.Text = "Desenquadrado em mais de R$ -15.000,00";
            this.rbValorDesenquadrado3.UseVisualStyleBackColor = true;
            this.rbValorDesenquadrado3.CheckedChanged += new System.EventHandler(this.chkDesenquadrado_CheckedChanged);
            // 
            // rbValorDesenquadrado2
            // 
            this.rbValorDesenquadrado2.AutoSize = true;
            this.rbValorDesenquadrado2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbValorDesenquadrado2.ForeColor = System.Drawing.Color.DarkGray;
            this.rbValorDesenquadrado2.Location = new System.Drawing.Point(648, 65);
            this.rbValorDesenquadrado2.Name = "rbValorDesenquadrado2";
            this.rbValorDesenquadrado2.Size = new System.Drawing.Size(276, 18);
            this.rbValorDesenquadrado2.TabIndex = 23;
            this.rbValorDesenquadrado2.TabStop = true;
            this.rbValorDesenquadrado2.Text = "Desenquadrado de R$ -5.000,00 à R$ -15.000,00";
            this.rbValorDesenquadrado2.UseVisualStyleBackColor = true;
            this.rbValorDesenquadrado2.CheckedChanged += new System.EventHandler(this.rbValorDesenquadrado_CheckedChanged);
            // 
            // rbValorDesenquadrado1
            // 
            this.rbValorDesenquadrado1.AutoSize = true;
            this.rbValorDesenquadrado1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbValorDesenquadrado1.ForeColor = System.Drawing.Color.DarkGray;
            this.rbValorDesenquadrado1.Location = new System.Drawing.Point(648, 43);
            this.rbValorDesenquadrado1.Name = "rbValorDesenquadrado1";
            this.rbValorDesenquadrado1.Size = new System.Drawing.Size(199, 18);
            this.rbValorDesenquadrado1.TabIndex = 22;
            this.rbValorDesenquadrado1.TabStop = true;
            this.rbValorDesenquadrado1.Text = "Desenquadrado até R$ -5.000,00";
            this.rbValorDesenquadrado1.UseVisualStyleBackColor = true;
            this.rbValorDesenquadrado1.CheckedChanged += new System.EventHandler(this.rbValorDesenquadrado_CheckedChanged);
            // 
            // rbValorDesenquadrado0
            // 
            this.rbValorDesenquadrado0.AutoSize = true;
            this.rbValorDesenquadrado0.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbValorDesenquadrado0.ForeColor = System.Drawing.Color.DarkGray;
            this.rbValorDesenquadrado0.Location = new System.Drawing.Point(648, 21);
            this.rbValorDesenquadrado0.Name = "rbValorDesenquadrado0";
            this.rbValorDesenquadrado0.Size = new System.Drawing.Size(57, 18);
            this.rbValorDesenquadrado0.TabIndex = 21;
            this.rbValorDesenquadrado0.TabStop = true;
            this.rbValorDesenquadrado0.Text = "Todos";
            this.rbValorDesenquadrado0.UseVisualStyleBackColor = true;
            this.rbValorDesenquadrado0.CheckedChanged += new System.EventHandler(this.rbValorDesenquadrado_CheckedChanged);
            // 
            // lblAguardeDados
            // 
            this.lblAguardeDados.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAguardeDados.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAguardeDados.ForeColor = System.Drawing.Color.Red;
            this.lblAguardeDados.Location = new System.Drawing.Point(56, 87);
            this.lblAguardeDados.Name = "lblAguardeDados";
            this.lblAguardeDados.Size = new System.Drawing.Size(138, 61);
            this.lblAguardeDados.TabIndex = 20;
            this.lblAguardeDados.Tag = "SemRenderizacao";
            this.lblAguardeDados.Text = "Aguarde... Carregando dados do Sinacor";
            this.lblAguardeDados.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkExcecoes
            // 
            this.chkExcecoes.AutoSize = true;
            this.chkExcecoes.Font = new System.Drawing.Font("Calibri", 9F);
            this.chkExcecoes.ForeColor = System.Drawing.Color.DarkGray;
            this.chkExcecoes.Location = new System.Drawing.Point(14, 72);
            this.chkExcecoes.Name = "chkExcecoes";
            this.chkExcecoes.Size = new System.Drawing.Size(107, 18);
            this.chkExcecoes.TabIndex = 19;
            this.chkExcecoes.Tag = "SemRenderizacao";
            this.chkExcecoes.Text = "Exibir Exceções";
            this.chkExcecoes.UseVisualStyleBackColor = true;
            this.chkExcecoes.CheckedChanged += new System.EventHandler(this.chkExcecoes_CheckedChanged);
            // 
            // chkDesenquadrado
            // 
            this.chkDesenquadrado.AutoSize = true;
            this.chkDesenquadrado.Checked = true;
            this.chkDesenquadrado.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDesenquadrado.Font = new System.Drawing.Font("Calibri", 9F);
            this.chkDesenquadrado.ForeColor = System.Drawing.Color.DarkGray;
            this.chkDesenquadrado.Location = new System.Drawing.Point(14, 49);
            this.chkDesenquadrado.Name = "chkDesenquadrado";
            this.chkDesenquadrado.Size = new System.Drawing.Size(153, 18);
            this.chkDesenquadrado.TabIndex = 19;
            this.chkDesenquadrado.Tag = "SemRenderizacao";
            this.chkDesenquadrado.Text = "Exibir Desenquadrados";
            this.chkDesenquadrado.UseVisualStyleBackColor = true;
            this.chkDesenquadrado.CheckedChanged += new System.EventHandler(this.chkDesenquadrado_CheckedChanged);
            // 
            // btnConfigEmail
            // 
            this.btnConfigEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfigEmail.BackgroundImage = global::Gradual.SaldoDevedor.WinApp.Properties.Resources.Ico_Mensagens_ce;
            this.btnConfigEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnConfigEmail.FlatAppearance.BorderSize = 0;
            this.btnConfigEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfigEmail.Location = new System.Drawing.Point(60, 487);
            this.btnConfigEmail.Name = "btnConfigEmail";
            this.btnConfigEmail.Size = new System.Drawing.Size(30, 30);
            this.btnConfigEmail.TabIndex = 18;
            this.btnConfigEmail.Tag = "SemRenderizacao";
            this.btnConfigEmail.UseVisualStyleBackColor = true;
            this.btnConfigEmail.Click += new System.EventHandler(this.btnConfigEmail_Click);
            // 
            // btnConfiguracao
            // 
            this.btnConfiguracao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfiguracao.BackgroundImage = global::Gradual.SaldoDevedor.WinApp.Properties.Resources.Ico_Configuracoes_ce;
            this.btnConfiguracao.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnConfiguracao.FlatAppearance.BorderSize = 0;
            this.btnConfiguracao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracao.Location = new System.Drawing.Point(6, 487);
            this.btnConfiguracao.Name = "btnConfiguracao";
            this.btnConfiguracao.Size = new System.Drawing.Size(30, 30);
            this.btnConfiguracao.TabIndex = 18;
            this.btnConfiguracao.Tag = "SemRenderizacao";
            this.btnConfiguracao.UseVisualStyleBackColor = true;
            this.btnConfiguracao.Click += new System.EventHandler(this.btnConfiguracao_Click);
            // 
            // btnNotificarAtendimento
            // 
            this.btnNotificarAtendimento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNotificarAtendimento.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNotificarAtendimento.FlatAppearance.BorderSize = 2;
            this.btnNotificarAtendimento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotificarAtendimento.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNotificarAtendimento.ForeColor = System.Drawing.Color.White;
            this.btnNotificarAtendimento.Location = new System.Drawing.Point(328, 484);
            this.btnNotificarAtendimento.Name = "btnNotificarAtendimento";
            this.btnNotificarAtendimento.Size = new System.Drawing.Size(201, 35);
            this.btnNotificarAtendimento.TabIndex = 17;
            this.btnNotificarAtendimento.Tag = "SemRenderizacao";
            this.btnNotificarAtendimento.Text = "Notificar Atendimento";
            this.btnNotificarAtendimento.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNotificarAtendimento.UseVisualStyleBackColor = false;
            this.btnNotificarAtendimento.Click += new System.EventHandler(this.btnNotificarAtendimento_Click);
            // 
            // btnNotificarTesouraria
            // 
            this.btnNotificarTesouraria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNotificarTesouraria.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNotificarTesouraria.FlatAppearance.BorderSize = 2;
            this.btnNotificarTesouraria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotificarTesouraria.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNotificarTesouraria.ForeColor = System.Drawing.Color.White;
            this.btnNotificarTesouraria.Location = new System.Drawing.Point(541, 484);
            this.btnNotificarTesouraria.Name = "btnNotificarTesouraria";
            this.btnNotificarTesouraria.Size = new System.Drawing.Size(175, 35);
            this.btnNotificarTesouraria.TabIndex = 17;
            this.btnNotificarTesouraria.Tag = "SemRenderizacao";
            this.btnNotificarTesouraria.Text = "Notificar Tesouraria";
            this.btnNotificarTesouraria.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNotificarTesouraria.UseVisualStyleBackColor = false;
            this.btnNotificarTesouraria.Click += new System.EventHandler(this.btnNotificarTesouraria_Click);
            // 
            // btnHistorico
            // 
            this.btnHistorico.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHistorico.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnHistorico.FlatAppearance.BorderSize = 2;
            this.btnHistorico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorico.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistorico.ForeColor = System.Drawing.Color.White;
            this.btnHistorico.Location = new System.Drawing.Point(173, 484);
            this.btnHistorico.Name = "btnHistorico";
            this.btnHistorico.Size = new System.Drawing.Size(118, 35);
            this.btnHistorico.TabIndex = 17;
            this.btnHistorico.Tag = "SemRenderizacao";
            this.btnHistorico.Text = "Histórico";
            this.btnHistorico.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnHistorico.UseVisualStyleBackColor = false;
            this.btnHistorico.Click += new System.EventHandler(this.btnHistorico_Click);
            // 
            // btnNotificarAssessores
            // 
            this.btnNotificarAssessores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNotificarAssessores.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNotificarAssessores.FlatAppearance.BorderSize = 2;
            this.btnNotificarAssessores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotificarAssessores.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNotificarAssessores.ForeColor = System.Drawing.Color.White;
            this.btnNotificarAssessores.Location = new System.Drawing.Point(731, 484);
            this.btnNotificarAssessores.Name = "btnNotificarAssessores";
            this.btnNotificarAssessores.Size = new System.Drawing.Size(175, 35);
            this.btnNotificarAssessores.TabIndex = 17;
            this.btnNotificarAssessores.Tag = "SemRenderizacao";
            this.btnNotificarAssessores.Text = "Notificar Assessores";
            this.btnNotificarAssessores.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNotificarAssessores.UseVisualStyleBackColor = false;
            this.btnNotificarAssessores.Click += new System.EventHandler(this.btnNotificarAssessores_Click);
            // 
            // btnFechar
            // 
            this.btnFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFechar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnFechar.FlatAppearance.BorderSize = 2;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFechar.ForeColor = System.Drawing.Color.White;
            this.btnFechar.Location = new System.Drawing.Point(1031, 484);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(113, 35);
            this.btnFechar.TabIndex = 17;
            this.btnFechar.Tag = "SemRenderizacao";
            this.btnFechar.Text = "Cancelar";
            this.btnFechar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFechar.UseVisualStyleBackColor = false;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // updwQtdDiasAtraso
            // 
            this.updwQtdDiasAtraso.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.updwQtdDiasAtraso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.updwQtdDiasAtraso.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.updwQtdDiasAtraso.Location = new System.Drawing.Point(139, 20);
            this.updwQtdDiasAtraso.Name = "updwQtdDiasAtraso";
            this.updwQtdDiasAtraso.Size = new System.Drawing.Size(89, 23);
            this.updwQtdDiasAtraso.TabIndex = 16;
            this.updwQtdDiasAtraso.ValueChanged += new System.EventHandler(this.updwQtdDiasAtraso_ValueChanged);
            // 
            // pnlParametros
            // 
            this.pnlParametros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlParametros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.pnlParametros.Controls.Add(this.lblDescrLimiteMulta);
            this.pnlParametros.Controls.Add(this.lblDescrTaxaJuros);
            this.pnlParametros.Controls.Add(this.lblLimiteMulta);
            this.pnlParametros.Controls.Add(this.lblTaxaJuros);
            this.pnlParametros.Location = new System.Drawing.Point(942, 36);
            this.pnlParametros.Name = "pnlParametros";
            this.pnlParametros.Size = new System.Drawing.Size(200, 61);
            this.pnlParametros.TabIndex = 15;
            // 
            // lblDescrLimiteMulta
            // 
            this.lblDescrLimiteMulta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescrLimiteMulta.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrLimiteMulta.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrLimiteMulta.Location = new System.Drawing.Point(3, 33);
            this.lblDescrLimiteMulta.Name = "lblDescrLimiteMulta";
            this.lblDescrLimiteMulta.Size = new System.Drawing.Size(136, 13);
            this.lblDescrLimiteMulta.TabIndex = 11;
            this.lblDescrLimiteMulta.Tag = "SemRenderizacao";
            this.lblDescrLimiteMulta.Text = "Limite Saldo Devedor:";
            this.lblDescrLimiteMulta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescrTaxaJuros
            // 
            this.lblDescrTaxaJuros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescrTaxaJuros.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrTaxaJuros.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrTaxaJuros.Location = new System.Drawing.Point(43, 8);
            this.lblDescrTaxaJuros.Name = "lblDescrTaxaJuros";
            this.lblDescrTaxaJuros.Size = new System.Drawing.Size(96, 13);
            this.lblDescrTaxaJuros.TabIndex = 11;
            this.lblDescrTaxaJuros.Tag = "SemRenderizacao";
            this.lblDescrTaxaJuros.Text = "Taxa de Juros:";
            this.lblDescrTaxaJuros.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLimiteMulta
            // 
            this.lblLimiteMulta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLimiteMulta.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLimiteMulta.ForeColor = System.Drawing.Color.White;
            this.lblLimiteMulta.Location = new System.Drawing.Point(138, 32);
            this.lblLimiteMulta.Name = "lblLimiteMulta";
            this.lblLimiteMulta.Size = new System.Drawing.Size(59, 13);
            this.lblLimiteMulta.TabIndex = 11;
            this.lblLimiteMulta.Tag = "SemRenderizacao";
            this.lblLimiteMulta.Text = "0,00";
            this.lblLimiteMulta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaxaJuros
            // 
            this.lblTaxaJuros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTaxaJuros.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxaJuros.ForeColor = System.Drawing.Color.White;
            this.lblTaxaJuros.Location = new System.Drawing.Point(159, 7);
            this.lblTaxaJuros.Name = "lblTaxaJuros";
            this.lblTaxaJuros.Size = new System.Drawing.Size(38, 13);
            this.lblTaxaJuros.TabIndex = 11;
            this.lblTaxaJuros.Tag = "SemRenderizacao";
            this.lblTaxaJuros.Text = "0%";
            this.lblTaxaJuros.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCliente
            // 
            this.txtCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCliente.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtCliente.Location = new System.Drawing.Point(14, 20);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.Size = new System.Drawing.Size(89, 23);
            this.txtCliente.TabIndex = 14;
            this.txtCliente.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCliente_KeyUp);
            // 
            // chklistAssessores
            // 
            this.chklistAssessores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.chklistAssessores.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chklistAssessores.CheckOnClick = true;
            this.chklistAssessores.ColumnWidth = 75;
            this.chklistAssessores.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.chklistAssessores.FormattingEnabled = true;
            this.chklistAssessores.Location = new System.Drawing.Point(253, 20);
            this.chklistAssessores.MultiColumn = true;
            this.chklistAssessores.Name = "chklistAssessores";
            this.chklistAssessores.Size = new System.Drawing.Size(389, 128);
            this.chklistAssessores.TabIndex = 13;
            this.chklistAssessores.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chklistAssessores_ItemCheck);
            // 
            // lblDataHoraAtualizacao
            // 
            this.lblDataHoraAtualizacao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataHoraAtualizacao.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataHoraAtualizacao.ForeColor = System.Drawing.Color.White;
            this.lblDataHoraAtualizacao.Location = new System.Drawing.Point(1016, 3);
            this.lblDataHoraAtualizacao.Name = "lblDataHoraAtualizacao";
            this.lblDataHoraAtualizacao.Size = new System.Drawing.Size(126, 13);
            this.lblDataHoraAtualizacao.TabIndex = 12;
            this.lblDataHoraAtualizacao.Tag = "SemRenderizacao";
            this.lblDataHoraAtualizacao.Text = "99/99/9999 99:99:99";
            this.lblDataHoraAtualizacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblQtdClientes
            // 
            this.lblQtdClientes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQtdClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblQtdClientes.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQtdClientes.ForeColor = System.Drawing.Color.White;
            this.lblQtdClientes.Location = new System.Drawing.Point(1031, 133);
            this.lblQtdClientes.Name = "lblQtdClientes";
            this.lblQtdClientes.Size = new System.Drawing.Size(111, 20);
            this.lblQtdClientes.TabIndex = 11;
            this.lblQtdClientes.Tag = "SemRenderizacao";
            this.lblQtdClientes.Text = "0 / 0";
            this.lblQtdClientes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescrQtdDiasAtraso
            // 
            this.lblDescrQtdDiasAtraso.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrQtdDiasAtraso.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrQtdDiasAtraso.Location = new System.Drawing.Point(136, 4);
            this.lblDescrQtdDiasAtraso.Name = "lblDescrQtdDiasAtraso";
            this.lblDescrQtdDiasAtraso.Size = new System.Drawing.Size(101, 13);
            this.lblDescrQtdDiasAtraso.TabIndex = 11;
            this.lblDescrQtdDiasAtraso.Tag = "SemRenderizacao";
            this.lblDescrQtdDiasAtraso.Text = "Qtd Dias Atraso:";
            this.lblDescrQtdDiasAtraso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescrCliente
            // 
            this.lblDescrCliente.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrCliente.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrCliente.Location = new System.Drawing.Point(11, 4);
            this.lblDescrCliente.Name = "lblDescrCliente";
            this.lblDescrCliente.Size = new System.Drawing.Size(69, 13);
            this.lblDescrCliente.TabIndex = 11;
            this.lblDescrCliente.Tag = "SemRenderizacao";
            this.lblDescrCliente.Text = "Cliente:";
            this.lblDescrCliente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblListaAssessores
            // 
            this.lblListaAssessores.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblListaAssessores.ForeColor = System.Drawing.Color.DarkGray;
            this.lblListaAssessores.Location = new System.Drawing.Point(250, 4);
            this.lblListaAssessores.Name = "lblListaAssessores";
            this.lblListaAssessores.Size = new System.Drawing.Size(74, 13);
            this.lblListaAssessores.TabIndex = 11;
            this.lblListaAssessores.Tag = "SemRenderizacao";
            this.lblListaAssessores.Text = "Assessores:";
            this.lblListaAssessores.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescrTotalClientes
            // 
            this.lblDescrTotalClientes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescrTotalClientes.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrTotalClientes.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrTotalClientes.Location = new System.Drawing.Point(1040, 119);
            this.lblDescrTotalClientes.Name = "lblDescrTotalClientes";
            this.lblDescrTotalClientes.Size = new System.Drawing.Size(103, 13);
            this.lblDescrTotalClientes.TabIndex = 11;
            this.lblDescrTotalClientes.Tag = "SemRenderizacao";
            this.lblDescrTotalClientes.Text = "Total de Clientes:";
            this.lblDescrTotalClientes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUltimaAtualizacao
            // 
            this.lblUltimaAtualizacao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUltimaAtualizacao.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUltimaAtualizacao.ForeColor = System.Drawing.Color.DarkGray;
            this.lblUltimaAtualizacao.Location = new System.Drawing.Point(888, 4);
            this.lblUltimaAtualizacao.Name = "lblUltimaAtualizacao";
            this.lblUltimaAtualizacao.Size = new System.Drawing.Size(126, 13);
            this.lblUltimaAtualizacao.TabIndex = 11;
            this.lblUltimaAtualizacao.Tag = "SemRenderizacao";
            this.lblUltimaAtualizacao.Text = "Última atualização:";
            this.lblUltimaAtualizacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grdClientesDevedores
            // 
            this.grdClientesDevedores.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdClientesDevedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdClientesDevedores.Location = new System.Drawing.Point(3, 156);
            this.grdClientesDevedores.Name = "grdClientesDevedores";
            this.grdClientesDevedores.Size = new System.Drawing.Size(1142, 326);
            this.grdClientesDevedores.TabIndex = 0;
            this.grdClientesDevedores.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdClientesDevedores_CellFormatting);
            this.grdClientesDevedores.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.grdClientesDevedores_CellValueNeeded);
            this.grdClientesDevedores.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdClientesDevedores_ColumnHeaderMouseClick);
            this.grdClientesDevedores.Resize += new System.EventHandler(this.grdClientesDevedores_Resize);
            // 
            // frmSaldoDevedor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 586);
            this.Controls.Add(this.pnlSaldoDevedor);
            this.Name = "frmSaldoDevedor";
            this.Text = "..:: Gradual Saldo Devedor ::..";
            this.pnlSaldoDevedor.ResumeLayout(false);
            this.pnlSaldoDevedor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updwQtdDiasAtraso)).EndInit();
            this.pnlParametros.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdClientesDevedores)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSaldoDevedor;
        private System.Windows.Forms.Label lblDataHoraAtualizacao;
        private System.Windows.Forms.Label lblUltimaAtualizacao;
        private GradualForm.Controls.CustomDataGridView grdClientesDevedores;
        private System.Windows.Forms.Label lblDescrTaxaJuros;
        private System.Windows.Forms.Label lblTaxaJuros;
        private System.Windows.Forms.CheckedListBox chklistAssessores;
        private System.Windows.Forms.Label lblQtdClientes;
        private System.Windows.Forms.Label lblDescrTotalClientes;
        private System.Windows.Forms.Label lblListaAssessores;
        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.Label lblDescrCliente;
        private System.Windows.Forms.Panel pnlParametros;
        private System.Windows.Forms.Label lblDescrLimiteMulta;
        private System.Windows.Forms.Label lblLimiteMulta;
        private System.Windows.Forms.NumericUpDown updwQtdDiasAtraso;
        private System.Windows.Forms.Label lblDescrQtdDiasAtraso;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Button btnConfiguracao;
        private System.Windows.Forms.Button btnNotificarAssessores;
        private System.Windows.Forms.CheckBox chkDesenquadrado;
        private System.Windows.Forms.Button btnNotificarTesouraria;
        private System.Windows.Forms.Button btnConfigEmail;
        private System.Windows.Forms.CheckBox chkExcecoes;
        private System.Windows.Forms.Label lblAguardeDados;
        private System.Windows.Forms.Button btnNotificarAtendimento;
        private System.Windows.Forms.Button btnHistorico;
        private System.Windows.Forms.RadioButton rbValorDesenquadrado3;
        private System.Windows.Forms.RadioButton rbValorDesenquadrado2;
        private System.Windows.Forms.RadioButton rbValorDesenquadrado1;
        private System.Windows.Forms.RadioButton rbValorDesenquadrado0;
    }
}
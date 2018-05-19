namespace Gradual.SaldoDevedor.WinApp
{
    partial class frmConfigEmail
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
            this.grdTextoEmail = new GradualForm.Controls.CustomDataGridView();
            this.pnlConfigEmail = new System.Windows.Forms.Panel();
            this.grdAssessorEmail = new GradualForm.Controls.CustomDataGridView();
            this.grdEmail = new GradualForm.Controls.CustomDataGridView();
            this.lblDescrEmail = new System.Windows.Forms.Label();
            this.btnInserirEmail = new System.Windows.Forms.Button();
            this.btnInserirAssessorEmail = new System.Windows.Forms.Button();
            this.btnInserirTextoEmail = new System.Windows.Forms.Button();
            this.btnExcluirAssessorEmail = new System.Windows.Forms.Button();
            this.lblDescrAssessorEmail = new System.Windows.Forms.Label();
            this.lblDescrTextoEmail = new System.Windows.Forms.Label();
            this.btnSalvarConfiguracoes = new System.Windows.Forms.Button();
            this.btnFecharConfig = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdTextoEmail)).BeginInit();
            this.pnlConfigEmail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAssessorEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdEmail)).BeginInit();
            this.SuspendLayout();
            // 
            // grdTextoEmail
            // 
            this.grdTextoEmail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTextoEmail.Location = new System.Drawing.Point(493, 304);
            this.grdTextoEmail.Name = "grdTextoEmail";
            this.grdTextoEmail.Size = new System.Drawing.Size(775, 251);
            this.grdTextoEmail.TabIndex = 0;
            this.grdTextoEmail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdTextoEmail_CellFormatting);
            this.grdTextoEmail.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdTextoEmail_CellValueChanged);
            // 
            // pnlConfigEmail
            // 
            this.pnlConfigEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConfigEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.pnlConfigEmail.Controls.Add(this.grdAssessorEmail);
            this.pnlConfigEmail.Controls.Add(this.grdEmail);
            this.pnlConfigEmail.Controls.Add(this.lblDescrEmail);
            this.pnlConfigEmail.Controls.Add(this.btnInserirEmail);
            this.pnlConfigEmail.Controls.Add(this.btnInserirAssessorEmail);
            this.pnlConfigEmail.Controls.Add(this.btnInserirTextoEmail);
            this.pnlConfigEmail.Controls.Add(this.btnExcluirAssessorEmail);
            this.pnlConfigEmail.Controls.Add(this.lblDescrAssessorEmail);
            this.pnlConfigEmail.Controls.Add(this.lblDescrTextoEmail);
            this.pnlConfigEmail.Controls.Add(this.btnSalvarConfiguracoes);
            this.pnlConfigEmail.Controls.Add(this.btnFecharConfig);
            this.pnlConfigEmail.Controls.Add(this.grdTextoEmail);
            this.pnlConfigEmail.Location = new System.Drawing.Point(4, 35);
            this.pnlConfigEmail.Name = "pnlConfigEmail";
            this.pnlConfigEmail.Size = new System.Drawing.Size(1272, 594);
            this.pnlConfigEmail.TabIndex = 1;
            // 
            // grdAssessorEmail
            // 
            this.grdAssessorEmail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAssessorEmail.Location = new System.Drawing.Point(2, 302);
            this.grdAssessorEmail.Name = "grdAssessorEmail";
            this.grdAssessorEmail.Size = new System.Drawing.Size(427, 253);
            this.grdAssessorEmail.TabIndex = 25;
            this.grdAssessorEmail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdAssessorEmail_CellFormatting);
            this.grdAssessorEmail.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdAssessorEmail_CellValueChanged);
            // 
            // grdEmail
            // 
            this.grdEmail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdEmail.Location = new System.Drawing.Point(2, 35);
            this.grdEmail.Name = "grdEmail";
            this.grdEmail.Size = new System.Drawing.Size(1266, 232);
            this.grdEmail.TabIndex = 24;
            this.grdEmail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdEmail_CellFormatting);
            this.grdEmail.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdEmail_CellValueChanged);
            // 
            // lblDescrEmail
            // 
            this.lblDescrEmail.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrEmail.ForeColor = System.Drawing.Color.White;
            this.lblDescrEmail.Location = new System.Drawing.Point(-1, 14);
            this.lblDescrEmail.Name = "lblDescrEmail";
            this.lblDescrEmail.Size = new System.Drawing.Size(193, 18);
            this.lblDescrEmail.TabIndex = 23;
            this.lblDescrEmail.Tag = "SemRenderizacao";
            this.lblDescrEmail.Text = "E-mail";
            this.lblDescrEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnInserirEmail
            // 
            this.btnInserirEmail.BackgroundImage = global::Gradual.SaldoDevedor.WinApp.Properties.Resources.Ico_Criar_ce;
            this.btnInserirEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnInserirEmail.FlatAppearance.BorderSize = 0;
            this.btnInserirEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInserirEmail.Location = new System.Drawing.Point(1237, 4);
            this.btnInserirEmail.Name = "btnInserirEmail";
            this.btnInserirEmail.Size = new System.Drawing.Size(30, 30);
            this.btnInserirEmail.TabIndex = 22;
            this.btnInserirEmail.Tag = "SemRenderizacao";
            this.btnInserirEmail.UseVisualStyleBackColor = true;
            this.btnInserirEmail.Click += new System.EventHandler(this.btnInserirEmail_Click);
            // 
            // btnInserirAssessorEmail
            // 
            this.btnInserirAssessorEmail.BackgroundImage = global::Gradual.SaldoDevedor.WinApp.Properties.Resources.Ico_Criar_ce;
            this.btnInserirAssessorEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnInserirAssessorEmail.FlatAppearance.BorderSize = 0;
            this.btnInserirAssessorEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInserirAssessorEmail.Location = new System.Drawing.Point(435, 304);
            this.btnInserirAssessorEmail.Name = "btnInserirAssessorEmail";
            this.btnInserirAssessorEmail.Size = new System.Drawing.Size(30, 30);
            this.btnInserirAssessorEmail.TabIndex = 22;
            this.btnInserirAssessorEmail.Tag = "SemRenderizacao";
            this.btnInserirAssessorEmail.UseVisualStyleBackColor = true;
            this.btnInserirAssessorEmail.Click += new System.EventHandler(this.btnInserirAssessorEmail_Click);
            // 
            // btnInserirTextoEmail
            // 
            this.btnInserirTextoEmail.BackgroundImage = global::Gradual.SaldoDevedor.WinApp.Properties.Resources.Ico_Criar_ce;
            this.btnInserirTextoEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnInserirTextoEmail.FlatAppearance.BorderSize = 0;
            this.btnInserirTextoEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInserirTextoEmail.Location = new System.Drawing.Point(1237, 273);
            this.btnInserirTextoEmail.Name = "btnInserirTextoEmail";
            this.btnInserirTextoEmail.Size = new System.Drawing.Size(30, 30);
            this.btnInserirTextoEmail.TabIndex = 22;
            this.btnInserirTextoEmail.Tag = "SemRenderizacao";
            this.btnInserirTextoEmail.UseVisualStyleBackColor = true;
            this.btnInserirTextoEmail.Click += new System.EventHandler(this.btnInserirTextoEmail_Click);
            // 
            // btnExcluirAssessorEmail
            // 
            this.btnExcluirAssessorEmail.BackgroundImage = global::Gradual.SaldoDevedor.WinApp.Properties.Resources.Ico_Excluir_ce;
            this.btnExcluirAssessorEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExcluirAssessorEmail.FlatAppearance.BorderSize = 0;
            this.btnExcluirAssessorEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcluirAssessorEmail.Location = new System.Drawing.Point(435, 355);
            this.btnExcluirAssessorEmail.Name = "btnExcluirAssessorEmail";
            this.btnExcluirAssessorEmail.Size = new System.Drawing.Size(30, 30);
            this.btnExcluirAssessorEmail.TabIndex = 22;
            this.btnExcluirAssessorEmail.Tag = "SemRenderizacao";
            this.btnExcluirAssessorEmail.UseVisualStyleBackColor = true;
            this.btnExcluirAssessorEmail.Click += new System.EventHandler(this.btnExcluirAssessorEmail_Click);
            // 
            // lblDescrAssessorEmail
            // 
            this.lblDescrAssessorEmail.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrAssessorEmail.ForeColor = System.Drawing.Color.White;
            this.lblDescrAssessorEmail.Location = new System.Drawing.Point(2, 283);
            this.lblDescrAssessorEmail.Name = "lblDescrAssessorEmail";
            this.lblDescrAssessorEmail.Size = new System.Drawing.Size(193, 18);
            this.lblDescrAssessorEmail.TabIndex = 21;
            this.lblDescrAssessorEmail.Tag = "SemRenderizacao";
            this.lblDescrAssessorEmail.Text = "Assessores que recebem E-mail";
            this.lblDescrAssessorEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescrTextoEmail
            // 
            this.lblDescrTextoEmail.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrTextoEmail.ForeColor = System.Drawing.Color.White;
            this.lblDescrTextoEmail.Location = new System.Drawing.Point(490, 283);
            this.lblDescrTextoEmail.Name = "lblDescrTextoEmail";
            this.lblDescrTextoEmail.Size = new System.Drawing.Size(193, 18);
            this.lblDescrTextoEmail.TabIndex = 21;
            this.lblDescrTextoEmail.Tag = "SemRenderizacao";
            this.lblDescrTextoEmail.Text = "Corpo do E-mail";
            this.lblDescrTextoEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSalvarConfiguracoes
            // 
            this.btnSalvarConfiguracoes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSalvarConfiguracoes.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSalvarConfiguracoes.FlatAppearance.BorderSize = 2;
            this.btnSalvarConfiguracoes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvarConfiguracoes.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalvarConfiguracoes.ForeColor = System.Drawing.Color.White;
            this.btnSalvarConfiguracoes.Location = new System.Drawing.Point(2, 559);
            this.btnSalvarConfiguracoes.Name = "btnSalvarConfiguracoes";
            this.btnSalvarConfiguracoes.Size = new System.Drawing.Size(207, 32);
            this.btnSalvarConfiguracoes.TabIndex = 20;
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
            this.btnFecharConfig.Location = new System.Drawing.Point(1155, 556);
            this.btnFecharConfig.Name = "btnFecharConfig";
            this.btnFecharConfig.Size = new System.Drawing.Size(113, 35);
            this.btnFecharConfig.TabIndex = 19;
            this.btnFecharConfig.Tag = "SemRenderizacao";
            this.btnFecharConfig.Text = "Cancelar";
            this.btnFecharConfig.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFecharConfig.UseVisualStyleBackColor = false;
            this.btnFecharConfig.Click += new System.EventHandler(this.btnFecharConfig_Click);
            // 
            // frmConfigEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 660);
            this.Controls.Add(this.pnlConfigEmail);
            this.Name = "frmConfigEmail";
            this.Text = "..:: Configuração E-mail ::..";
            ((System.ComponentModel.ISupportInitialize)(this.grdTextoEmail)).EndInit();
            this.pnlConfigEmail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAssessorEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdEmail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GradualForm.Controls.CustomDataGridView grdTextoEmail;
        private System.Windows.Forms.Panel pnlConfigEmail;
        private System.Windows.Forms.Button btnSalvarConfiguracoes;
        private System.Windows.Forms.Button btnFecharConfig;
        private System.Windows.Forms.Label lblDescrTextoEmail;
        private System.Windows.Forms.Button btnInserirTextoEmail;
        private GradualForm.Controls.CustomDataGridView grdEmail;
        private System.Windows.Forms.Label lblDescrEmail;
        private System.Windows.Forms.Button btnInserirEmail;
        private GradualForm.Controls.CustomDataGridView grdAssessorEmail;
        private System.Windows.Forms.Button btnInserirAssessorEmail;
        private System.Windows.Forms.Button btnExcluirAssessorEmail;
        private System.Windows.Forms.Label lblDescrAssessorEmail;
    }
}
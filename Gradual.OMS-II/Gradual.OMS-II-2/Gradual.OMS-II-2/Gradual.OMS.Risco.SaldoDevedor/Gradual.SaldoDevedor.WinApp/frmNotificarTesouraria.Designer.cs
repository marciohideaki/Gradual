namespace Gradual.SaldoDevedor.WinApp
{
    partial class frmNotificarTesouraria
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
            this.pnlNotificarTesouraria = new System.Windows.Forms.Panel();
            this.grdDevedores = new GradualForm.Controls.CustomDataGridView();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnEnviarEmail = new System.Windows.Forms.Button();
            this.lblQtdDevedores = new System.Windows.Forms.Label();
            this.lblDescrTotal = new System.Windows.Forms.Label();
            this.pnlNotificarTesouraria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDevedores)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlNotificarTesouraria
            // 
            this.pnlNotificarTesouraria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlNotificarTesouraria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.pnlNotificarTesouraria.Controls.Add(this.lblQtdDevedores);
            this.pnlNotificarTesouraria.Controls.Add(this.lblDescrTotal);
            this.pnlNotificarTesouraria.Controls.Add(this.btnCancelar);
            this.pnlNotificarTesouraria.Controls.Add(this.btnEnviarEmail);
            this.pnlNotificarTesouraria.Controls.Add(this.grdDevedores);
            this.pnlNotificarTesouraria.Location = new System.Drawing.Point(3, 35);
            this.pnlNotificarTesouraria.Name = "pnlNotificarTesouraria";
            this.pnlNotificarTesouraria.Size = new System.Drawing.Size(697, 438);
            this.pnlNotificarTesouraria.TabIndex = 0;
            // 
            // grdDevedores
            // 
            this.grdDevedores.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDevedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDevedores.Location = new System.Drawing.Point(3, 29);
            this.grdDevedores.Name = "grdDevedores";
            this.grdDevedores.Size = new System.Drawing.Size(691, 364);
            this.grdDevedores.TabIndex = 0;
            this.grdDevedores.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdDevedores_CellContentClick);
            this.grdDevedores.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.grdDevedores_CellValueNeeded);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCancelar.FlatAppearance.BorderSize = 2;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(575, 400);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(119, 35);
            this.btnCancelar.TabIndex = 20;
            this.btnCancelar.Tag = "SemRenderizacao";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnEnviarEmail
            // 
            this.btnEnviarEmail.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnEnviarEmail.FlatAppearance.BorderSize = 2;
            this.btnEnviarEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarEmail.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviarEmail.ForeColor = System.Drawing.Color.White;
            this.btnEnviarEmail.Location = new System.Drawing.Point(3, 400);
            this.btnEnviarEmail.Name = "btnEnviarEmail";
            this.btnEnviarEmail.Size = new System.Drawing.Size(190, 35);
            this.btnEnviarEmail.TabIndex = 19;
            this.btnEnviarEmail.Tag = "SemRenderizacao";
            this.btnEnviarEmail.Text = "Enviar Notificação";
            this.btnEnviarEmail.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEnviarEmail.UseVisualStyleBackColor = false;
            this.btnEnviarEmail.Click += new System.EventHandler(this.btnEnviarEmail_Click);
            // 
            // lblQtdDevedores
            // 
            this.lblQtdDevedores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQtdDevedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblQtdDevedores.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQtdDevedores.ForeColor = System.Drawing.Color.White;
            this.lblQtdDevedores.Location = new System.Drawing.Point(82, 4);
            this.lblQtdDevedores.Name = "lblQtdDevedores";
            this.lblQtdDevedores.Size = new System.Drawing.Size(87, 20);
            this.lblQtdDevedores.TabIndex = 22;
            this.lblQtdDevedores.Tag = "SemRenderizacao";
            this.lblQtdDevedores.Text = "0";
            this.lblQtdDevedores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescrTotal
            // 
            this.lblDescrTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescrTotal.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrTotal.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrTotal.Location = new System.Drawing.Point(6, 8);
            this.lblDescrTotal.Name = "lblDescrTotal";
            this.lblDescrTotal.Size = new System.Drawing.Size(76, 13);
            this.lblDescrTotal.TabIndex = 21;
            this.lblDescrTotal.Tag = "SemRenderizacao";
            this.lblDescrTotal.Text = "Devedores:";
            this.lblDescrTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmNotificarTesouraria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 505);
            this.Controls.Add(this.pnlNotificarTesouraria);
            this.Name = "frmNotificarTesouraria";
            this.TamanhoFixo = true;
            this.Text = "..:: Notificar Tesouraria ::..";
            this.pnlNotificarTesouraria.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDevedores)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNotificarTesouraria;
        private GradualForm.Controls.CustomDataGridView grdDevedores;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnEnviarEmail;
        private System.Windows.Forms.Label lblQtdDevedores;
        private System.Windows.Forms.Label lblDescrTotal;
    }
}
namespace Gradual.SaldoDevedor.WinApp
{
    partial class frmHistoricoTotais
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
            this.pnlHistoricoTotais = new System.Windows.Forms.Panel();
            this.grdHistoricoTotais = new GradualForm.Controls.CustomDataGridView();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtDataInicial = new System.Windows.Forms.TextBox();
            this.lblDescrDataInicial = new System.Windows.Forms.Label();
            this.lblDescrDataFinal = new System.Windows.Forms.Label();
            this.txtDataFinal = new System.Windows.Forms.TextBox();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.pnlHistoricoTotais.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistoricoTotais)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHistoricoTotais
            // 
            this.pnlHistoricoTotais.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHistoricoTotais.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.pnlHistoricoTotais.Controls.Add(this.txtDataFinal);
            this.pnlHistoricoTotais.Controls.Add(this.txtDataInicial);
            this.pnlHistoricoTotais.Controls.Add(this.lblDescrDataFinal);
            this.pnlHistoricoTotais.Controls.Add(this.lblDescrDataInicial);
            this.pnlHistoricoTotais.Controls.Add(this.btnConsultar);
            this.pnlHistoricoTotais.Controls.Add(this.btnCancelar);
            this.pnlHistoricoTotais.Controls.Add(this.grdHistoricoTotais);
            this.pnlHistoricoTotais.Location = new System.Drawing.Point(3, 35);
            this.pnlHistoricoTotais.Name = "pnlHistoricoTotais";
            this.pnlHistoricoTotais.Size = new System.Drawing.Size(325, 451);
            this.pnlHistoricoTotais.TabIndex = 0;
            // 
            // grdHistoricoTotais
            // 
            this.grdHistoricoTotais.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdHistoricoTotais.Location = new System.Drawing.Point(3, 66);
            this.grdHistoricoTotais.Name = "grdHistoricoTotais";
            this.grdHistoricoTotais.Size = new System.Drawing.Size(316, 341);
            this.grdHistoricoTotais.TabIndex = 0;
            this.grdHistoricoTotais.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.grdHistoricoTotais_CellValueNeeded);
            // 
            // btnCancelar
            // 
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCancelar.FlatAppearance.BorderSize = 2;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(199, 411);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(119, 35);
            this.btnCancelar.TabIndex = 21;
            this.btnCancelar.Tag = "SemRenderizacao";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // txtDataInicial
            // 
            this.txtDataInicial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtDataInicial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDataInicial.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtDataInicial.Location = new System.Drawing.Point(86, 7);
            this.txtDataInicial.Name = "txtDataInicial";
            this.txtDataInicial.Size = new System.Drawing.Size(85, 23);
            this.txtDataInicial.TabIndex = 23;
            this.txtDataInicial.Text = "99/99/9999";
            // 
            // lblDescrDataInicial
            // 
            this.lblDescrDataInicial.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrDataInicial.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrDataInicial.Location = new System.Drawing.Point(6, 11);
            this.lblDescrDataInicial.Name = "lblDescrDataInicial";
            this.lblDescrDataInicial.Size = new System.Drawing.Size(79, 13);
            this.lblDescrDataInicial.TabIndex = 22;
            this.lblDescrDataInicial.Tag = "SemRenderizacao";
            this.lblDescrDataInicial.Text = "Data inicial:";
            this.lblDescrDataInicial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescrDataFinal
            // 
            this.lblDescrDataFinal.Font = new System.Drawing.Font("Calibri", 9F);
            this.lblDescrDataFinal.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrDataFinal.Location = new System.Drawing.Point(6, 41);
            this.lblDescrDataFinal.Name = "lblDescrDataFinal";
            this.lblDescrDataFinal.Size = new System.Drawing.Size(79, 13);
            this.lblDescrDataFinal.TabIndex = 22;
            this.lblDescrDataFinal.Tag = "SemRenderizacao";
            this.lblDescrDataFinal.Text = "Data final:";
            this.lblDescrDataFinal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDataFinal
            // 
            this.txtDataFinal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtDataFinal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDataFinal.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtDataFinal.Location = new System.Drawing.Point(86, 37);
            this.txtDataFinal.Name = "txtDataFinal";
            this.txtDataFinal.Size = new System.Drawing.Size(85, 23);
            this.txtDataFinal.TabIndex = 23;
            this.txtDataFinal.Text = "99/99/9999";
            // 
            // btnConsultar
            // 
            this.btnConsultar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnConsultar.FlatAppearance.BorderSize = 2;
            this.btnConsultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsultar.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsultar.ForeColor = System.Drawing.Color.White;
            this.btnConsultar.Location = new System.Drawing.Point(214, 25);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(105, 35);
            this.btnConsultar.TabIndex = 21;
            this.btnConsultar.Tag = "SemRenderizacao";
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConsultar.UseVisualStyleBackColor = false;
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            // 
            // frmHistoricoTotais
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 517);
            this.Controls.Add(this.pnlHistoricoTotais);
            this.Name = "frmHistoricoTotais";
            this.TamanhoFixo = true;
            this.Text = "..:: Histórico de Totais ::..";
            this.pnlHistoricoTotais.ResumeLayout(false);
            this.pnlHistoricoTotais.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistoricoTotais)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHistoricoTotais;
        private GradualForm.Controls.CustomDataGridView grdHistoricoTotais;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtDataFinal;
        private System.Windows.Forms.TextBox txtDataInicial;
        private System.Windows.Forms.Label lblDescrDataFinal;
        private System.Windows.Forms.Label lblDescrDataInicial;
        private System.Windows.Forms.Button btnConsultar;
    }
}
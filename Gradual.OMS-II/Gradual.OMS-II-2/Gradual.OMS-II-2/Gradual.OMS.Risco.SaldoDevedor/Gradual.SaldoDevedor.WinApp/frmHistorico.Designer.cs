namespace Gradual.SaldoDevedor.WinApp
{
    partial class frmHistorico
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
            this.pnlHistorico = new System.Windows.Forms.Panel();
            this.txtTotalSaldo = new System.Windows.Forms.MaskedTextBox();
            this.lblQtdDevedores = new System.Windows.Forms.Label();
            this.lblAssessoresExcl = new System.Windows.Forms.Label();
            this.lblDescrAssessorExcl = new System.Windows.Forms.Label();
            this.lblDescrTotalSaldo = new System.Windows.Forms.Label();
            this.lblDescrTotal = new System.Windows.Forms.Label();
            this.grdHistorico = new GradualForm.Controls.CustomDataGridView();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGerarHistorico = new System.Windows.Forms.Button();
            this.treeDiasHistorico = new System.Windows.Forms.TreeView();
            this.lblDescrClientesExcl = new System.Windows.Forms.Label();
            this.lblClientesExcl = new System.Windows.Forms.Label();
            this.btnTotais = new System.Windows.Forms.Button();
            this.pnlHistorico.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistorico)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHistorico
            // 
            this.pnlHistorico.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHistorico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.pnlHistorico.Controls.Add(this.txtTotalSaldo);
            this.pnlHistorico.Controls.Add(this.lblQtdDevedores);
            this.pnlHistorico.Controls.Add(this.lblClientesExcl);
            this.pnlHistorico.Controls.Add(this.lblAssessoresExcl);
            this.pnlHistorico.Controls.Add(this.lblDescrClientesExcl);
            this.pnlHistorico.Controls.Add(this.lblDescrAssessorExcl);
            this.pnlHistorico.Controls.Add(this.lblDescrTotalSaldo);
            this.pnlHistorico.Controls.Add(this.lblDescrTotal);
            this.pnlHistorico.Controls.Add(this.grdHistorico);
            this.pnlHistorico.Controls.Add(this.btnTotais);
            this.pnlHistorico.Controls.Add(this.btnCancelar);
            this.pnlHistorico.Controls.Add(this.btnGerarHistorico);
            this.pnlHistorico.Controls.Add(this.treeDiasHistorico);
            this.pnlHistorico.Location = new System.Drawing.Point(4, 34);
            this.pnlHistorico.Name = "pnlHistorico";
            this.pnlHistorico.Size = new System.Drawing.Size(1194, 478);
            this.pnlHistorico.TabIndex = 0;
            // 
            // txtTotalSaldo
            // 
            this.txtTotalSaldo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtTotalSaldo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalSaldo.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalSaldo.ForeColor = System.Drawing.Color.White;
            this.txtTotalSaldo.Location = new System.Drawing.Point(1061, 7);
            this.txtTotalSaldo.Name = "txtTotalSaldo";
            this.txtTotalSaldo.ReadOnly = true;
            this.txtTotalSaldo.Size = new System.Drawing.Size(127, 26);
            this.txtTotalSaldo.TabIndex = 25;
            this.txtTotalSaldo.Tag = "SemRenderizacao";
            this.txtTotalSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalSaldo.Click += new System.EventHandler(this.txtTotalSaldo_Click);
            // 
            // lblQtdDevedores
            // 
            this.lblQtdDevedores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQtdDevedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblQtdDevedores.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQtdDevedores.ForeColor = System.Drawing.Color.White;
            this.lblQtdDevedores.Location = new System.Drawing.Point(215, 9);
            this.lblQtdDevedores.Name = "lblQtdDevedores";
            this.lblQtdDevedores.Size = new System.Drawing.Size(87, 20);
            this.lblQtdDevedores.TabIndex = 24;
            this.lblQtdDevedores.Tag = "SemRenderizacao";
            this.lblQtdDevedores.Text = "0";
            this.lblQtdDevedores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAssessoresExcl
            // 
            this.lblAssessoresExcl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAssessoresExcl.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAssessoresExcl.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblAssessoresExcl.Location = new System.Drawing.Point(581, 441);
            this.lblAssessoresExcl.Name = "lblAssessoresExcl";
            this.lblAssessoresExcl.Size = new System.Drawing.Size(479, 13);
            this.lblAssessoresExcl.TabIndex = 23;
            this.lblAssessoresExcl.Tag = "SemRenderizacao";
            this.lblAssessoresExcl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescrAssessorExcl
            // 
            this.lblDescrAssessorExcl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescrAssessorExcl.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrAssessorExcl.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrAssessorExcl.Location = new System.Drawing.Point(306, 441);
            this.lblDescrAssessorExcl.Name = "lblDescrAssessorExcl";
            this.lblDescrAssessorExcl.Size = new System.Drawing.Size(269, 13);
            this.lblDescrAssessorExcl.TabIndex = 23;
            this.lblDescrAssessorExcl.Tag = "SemRenderizacao";
            this.lblDescrAssessorExcl.Text = "Assessores excluídos do Total Saldo Devedor:";
            this.lblDescrAssessorExcl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescrTotalSaldo
            // 
            this.lblDescrTotalSaldo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescrTotalSaldo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrTotalSaldo.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrTotalSaldo.Location = new System.Drawing.Point(908, 13);
            this.lblDescrTotalSaldo.Name = "lblDescrTotalSaldo";
            this.lblDescrTotalSaldo.Size = new System.Drawing.Size(152, 13);
            this.lblDescrTotalSaldo.TabIndex = 23;
            this.lblDescrTotalSaldo.Tag = "SemRenderizacao";
            this.lblDescrTotalSaldo.Text = "Total Saldo Devedor:";
            this.lblDescrTotalSaldo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescrTotal
            // 
            this.lblDescrTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescrTotal.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrTotal.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrTotal.Location = new System.Drawing.Point(154, 12);
            this.lblDescrTotal.Name = "lblDescrTotal";
            this.lblDescrTotal.Size = new System.Drawing.Size(59, 13);
            this.lblDescrTotal.TabIndex = 23;
            this.lblDescrTotal.Tag = "SemRenderizacao";
            this.lblDescrTotal.Text = "Clientes:";
            this.lblDescrTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdHistorico
            // 
            this.grdHistorico.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdHistorico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdHistorico.Location = new System.Drawing.Point(149, 35);
            this.grdHistorico.Name = "grdHistorico";
            this.grdHistorico.Size = new System.Drawing.Size(1040, 402);
            this.grdHistorico.TabIndex = 21;
            this.grdHistorico.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdHistorico_CellContentClick);
            this.grdHistorico.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdHistorico_CellFormatting);
            this.grdHistorico.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.grdHistorico_CellValueNeeded);
            // 
            // btnCancelar
            // 
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCancelar.FlatAppearance.BorderSize = 2;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(1070, 440);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(119, 35);
            this.btnCancelar.TabIndex = 20;
            this.btnCancelar.Tag = "SemRenderizacao";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGerarHistorico
            // 
            this.btnGerarHistorico.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnGerarHistorico.FlatAppearance.BorderSize = 2;
            this.btnGerarHistorico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGerarHistorico.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGerarHistorico.ForeColor = System.Drawing.Color.White;
            this.btnGerarHistorico.Location = new System.Drawing.Point(5, 440);
            this.btnGerarHistorico.Name = "btnGerarHistorico";
            this.btnGerarHistorico.Size = new System.Drawing.Size(142, 35);
            this.btnGerarHistorico.TabIndex = 19;
            this.btnGerarHistorico.Tag = "SemRenderizacao";
            this.btnGerarHistorico.Text = "Gerar Histórico";
            this.btnGerarHistorico.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGerarHistorico.UseVisualStyleBackColor = false;
            this.btnGerarHistorico.Click += new System.EventHandler(this.btnGerarHistorico_Click);
            // 
            // treeDiasHistorico
            // 
            this.treeDiasHistorico.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeDiasHistorico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.treeDiasHistorico.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeDiasHistorico.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.treeDiasHistorico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.treeDiasHistorico.Location = new System.Drawing.Point(5, 5);
            this.treeDiasHistorico.Name = "treeDiasHistorico";
            this.treeDiasHistorico.Size = new System.Drawing.Size(142, 432);
            this.treeDiasHistorico.TabIndex = 0;
            this.treeDiasHistorico.Tag = "SemRenderizacao";
            this.treeDiasHistorico.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeDiasHistorico_AfterSelect);
            // 
            // lblDescrClientesExcl
            // 
            this.lblDescrClientesExcl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescrClientesExcl.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrClientesExcl.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescrClientesExcl.Location = new System.Drawing.Point(306, 457);
            this.lblDescrClientesExcl.Name = "lblDescrClientesExcl";
            this.lblDescrClientesExcl.Size = new System.Drawing.Size(269, 13);
            this.lblDescrClientesExcl.TabIndex = 23;
            this.lblDescrClientesExcl.Tag = "SemRenderizacao";
            this.lblDescrClientesExcl.Text = "Clientes excluídos do Total Saldo Devedor:";
            this.lblDescrClientesExcl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblClientesExcl
            // 
            this.lblClientesExcl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClientesExcl.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientesExcl.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblClientesExcl.Location = new System.Drawing.Point(581, 457);
            this.lblClientesExcl.Name = "lblClientesExcl";
            this.lblClientesExcl.Size = new System.Drawing.Size(479, 13);
            this.lblClientesExcl.TabIndex = 23;
            this.lblClientesExcl.Tag = "SemRenderizacao";
            this.lblClientesExcl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTotais
            // 
            this.btnTotais.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnTotais.FlatAppearance.BorderSize = 2;
            this.btnTotais.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTotais.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTotais.ForeColor = System.Drawing.Color.White;
            this.btnTotais.Location = new System.Drawing.Point(149, 440);
            this.btnTotais.Name = "btnTotais";
            this.btnTotais.Size = new System.Drawing.Size(128, 35);
            this.btnTotais.TabIndex = 20;
            this.btnTotais.Tag = "SemRenderizacao";
            this.btnTotais.Text = "Totais";
            this.btnTotais.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTotais.UseVisualStyleBackColor = false;
            this.btnTotais.Click += new System.EventHandler(this.btnTotais_Click);
            // 
            // frmHistorico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 543);
            this.Controls.Add(this.pnlHistorico);
            this.Name = "frmHistorico";
            this.TamanhoFixo = true;
            this.Text = "..:: Histórico de Movimento ::..";
            this.pnlHistorico.ResumeLayout(false);
            this.pnlHistorico.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistorico)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHistorico;
        private System.Windows.Forms.TreeView treeDiasHistorico;
        private GradualForm.Controls.CustomDataGridView grdHistorico;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGerarHistorico;
        private System.Windows.Forms.Label lblQtdDevedores;
        private System.Windows.Forms.Label lblDescrTotal;
        private System.Windows.Forms.Label lblDescrTotalSaldo;
        private System.Windows.Forms.Label lblAssessoresExcl;
        private System.Windows.Forms.Label lblDescrAssessorExcl;
        private System.Windows.Forms.MaskedTextBox txtTotalSaldo;
        private System.Windows.Forms.Label lblClientesExcl;
        private System.Windows.Forms.Label lblDescrClientesExcl;
        private System.Windows.Forms.Button btnTotais;
    }
}
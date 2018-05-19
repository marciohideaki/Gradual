namespace FormTest
{
    partial class PLD
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
            this.dgResumo = new System.Windows.Forms.DataGridView();
            this.cbo = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lblatualizacao = new System.Windows.Forms.Label();
            this.txtPapel = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgResumo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgResumo
            // 
            this.dgResumo.AllowUserToAddRows = false;
            this.dgResumo.AllowUserToDeleteRows = false;
            this.dgResumo.AllowUserToOrderColumns = true;
            this.dgResumo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgResumo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResumo.Location = new System.Drawing.Point(12, 64);
            this.dgResumo.Name = "dgResumo";
            this.dgResumo.Size = new System.Drawing.Size(1366, 527);
            this.dgResumo.TabIndex = 6;
            // 
            // cbo
            // 
            this.cbo.FormattingEnabled = true;
            this.cbo.Location = new System.Drawing.Point(12, 12);
            this.cbo.Name = "cbo";
            this.cbo.Size = new System.Drawing.Size(184, 21);
            this.cbo.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(309, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Pesquisar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblatualizacao
            // 
            this.lblatualizacao.AutoSize = true;
            this.lblatualizacao.Location = new System.Drawing.Point(322, 15);
            this.lblatualizacao.Name = "lblatualizacao";
            this.lblatualizacao.Size = new System.Drawing.Size(0, 13);
            this.lblatualizacao.TabIndex = 9;
            // 
            // txtPapel
            // 
            this.txtPapel.Location = new System.Drawing.Point(203, 12);
            this.txtPapel.Name = "txtPapel";
            this.txtPapel.Size = new System.Drawing.Size(100, 20);
            this.txtPapel.TabIndex = 10;
            // 
            // PLD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1390, 596);
            this.Controls.Add(this.txtPapel);
            this.Controls.Add(this.lblatualizacao);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbo);
            this.Controls.Add(this.dgResumo);
            this.Name = "PLD";
            this.Text = "PLD";
            this.Load += new System.EventHandler(this.PLD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgResumo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgResumo;
        private System.Windows.Forms.ComboBox cbo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblatualizacao;
        private System.Windows.Forms.TextBox txtPapel;
    }
}
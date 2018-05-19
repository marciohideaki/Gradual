namespace Gradual.OMS.InvXX.Fundos.Teste
{
    partial class frmImportacaoFinancial
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
            this.btnImportacaoFinancial = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImportacaoFinancial
            // 
            this.btnImportacaoFinancial.Location = new System.Drawing.Point(12, 12);
            this.btnImportacaoFinancial.Name = "btnImportacaoFinancial";
            this.btnImportacaoFinancial.Size = new System.Drawing.Size(221, 23);
            this.btnImportacaoFinancial.TabIndex = 0;
            this.btnImportacaoFinancial.Text = "Teste de Importação FINANCIAL";
            this.btnImportacaoFinancial.UseVisualStyleBackColor = true;
            this.btnImportacaoFinancial.Click += new System.EventHandler(this.btnImportacaoFinancial_Click);
            // 
            // frmImportacaoFinancial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 64);
            this.Controls.Add(this.btnImportacaoFinancial);
            this.Name = "frmImportacaoFinancial";
            this.Text = "Importacao Financial";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImportacaoFinancial;
    }
}
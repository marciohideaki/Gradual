namespace Gradual.OMS.InvXX.Fundos.Teste
{
    partial class frmImportacaoITAUUNIBANCO
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnTestarFinancial = new System.Windows.Forms.Button();
            this.btnTestarTD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(228, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Testar Importação ITAU-UNIBANCO";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTestarFinancial
            // 
            this.btnTestarFinancial.Location = new System.Drawing.Point(21, 42);
            this.btnTestarFinancial.Name = "btnTestarFinancial";
            this.btnTestarFinancial.Size = new System.Drawing.Size(228, 23);
            this.btnTestarFinancial.TabIndex = 1;
            this.btnTestarFinancial.Text = "Testar Financial";
            this.btnTestarFinancial.UseVisualStyleBackColor = true;
            this.btnTestarFinancial.Click += new System.EventHandler(this.btnTestarFinancial_Click);
            // 
            // btnTestarTD
            // 
            this.btnTestarTD.Location = new System.Drawing.Point(21, 71);
            this.btnTestarTD.Name = "btnTestarTD";
            this.btnTestarTD.Size = new System.Drawing.Size(228, 23);
            this.btnTestarTD.TabIndex = 2;
            this.btnTestarTD.Text = "Testar TD";
            this.btnTestarTD.UseVisualStyleBackColor = true;
            this.btnTestarTD.Click += new System.EventHandler(this.btnTestarTD_Click);
            // 
            // frmImportacaoITAUUNIBANCO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 112);
            this.Controls.Add(this.btnTestarTD);
            this.Controls.Add(this.btnTestarFinancial);
            this.Controls.Add(this.button1);
            this.Name = "frmImportacaoITAUUNIBANCO";
            this.Text = "Importação ITAU UNBANCO";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnTestarFinancial;
        private System.Windows.Forms.Button btnTestarTD;
    }
}
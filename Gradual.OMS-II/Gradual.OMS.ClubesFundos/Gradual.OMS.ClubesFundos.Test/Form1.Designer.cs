namespace Gradual.OMS.ClubesFundos.Test
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
            this.btnClubes = new System.Windows.Forms.Button();
            this.btnFundos = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCodCliente = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnClubes
            // 
            this.btnClubes.Location = new System.Drawing.Point(198, 12);
            this.btnClubes.Name = "btnClubes";
            this.btnClubes.Size = new System.Drawing.Size(187, 23);
            this.btnClubes.TabIndex = 0;
            this.btnClubes.Text = "Clubes";
            this.btnClubes.UseVisualStyleBackColor = true;
            this.btnClubes.Click += new System.EventHandler(this.btnClubes_Click);
            // 
            // btnFundos
            // 
            this.btnFundos.Location = new System.Drawing.Point(198, 61);
            this.btnFundos.Name = "btnFundos";
            this.btnFundos.Size = new System.Drawing.Size(187, 23);
            this.btnFundos.TabIndex = 1;
            this.btnFundos.Text = "Fundos";
            this.btnFundos.UseVisualStyleBackColor = true;
            this.btnFundos.Click += new System.EventHandler(this.btnFundos_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cliente";
            // 
            // txtCodCliente
            // 
            this.txtCodCliente.Location = new System.Drawing.Point(51, 10);
            this.txtCodCliente.Name = "txtCodCliente";
            this.txtCodCliente.Size = new System.Drawing.Size(100, 20);
            this.txtCodCliente.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 108);
            this.Controls.Add(this.txtCodCliente);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFundos);
            this.Controls.Add(this.btnClubes);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClubes;
        private System.Windows.Forms.Button btnFundos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCodCliente;
    }
}


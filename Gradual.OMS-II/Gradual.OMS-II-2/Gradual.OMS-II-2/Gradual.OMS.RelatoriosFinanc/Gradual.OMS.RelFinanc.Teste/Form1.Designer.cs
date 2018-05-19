namespace Gradual.OMS.RelFinanc.Teste
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
            this.btnCarregaDados = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCarregaDados
            // 
            this.btnCarregaDados.Location = new System.Drawing.Point(12, 12);
            this.btnCarregaDados.Name = "btnCarregaDados";
            this.btnCarregaDados.Size = new System.Drawing.Size(155, 23);
            this.btnCarregaDados.TabIndex = 0;
            this.btnCarregaDados.Text = "Carrega Dados";
            this.btnCarregaDados.UseVisualStyleBackColor = true;
            this.btnCarregaDados.Click += new System.EventHandler(this.btnCarregaDados_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 56);
            this.Controls.Add(this.btnCarregaDados);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCarregaDados;
    }
}


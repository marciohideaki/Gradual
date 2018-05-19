namespace Gradual.Migracao.PendenciasCadastrais
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
            this.ButtonIniciarMigracao = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonIniciarMigracao
            // 
            this.ButtonIniciarMigracao.AutoSize = true;
            this.ButtonIniciarMigracao.Location = new System.Drawing.Point(34, 100);
            this.ButtonIniciarMigracao.Name = "ButtonIniciarMigracao";
            this.ButtonIniciarMigracao.Size = new System.Drawing.Size(223, 23);
            this.ButtonIniciarMigracao.TabIndex = 0;
            this.ButtonIniciarMigracao.Text = "Iniciar Migração das Pendências Cadastrais";
            this.ButtonIniciarMigracao.UseVisualStyleBackColor = true;
            this.ButtonIniciarMigracao.Click += new System.EventHandler(this.ButtonIniciarMigracao_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.ButtonIniciarMigracao);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonIniciarMigracao;
    }
}


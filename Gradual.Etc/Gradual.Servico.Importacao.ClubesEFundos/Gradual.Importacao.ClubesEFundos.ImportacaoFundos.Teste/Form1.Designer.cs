namespace Gradual.MinhaConta.Servico.ImportacaoFundos.Teste
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
            this.buttonIniciarImportacaoFundos = new System.Windows.Forms.Button();
            this.buttonIniciarImportacaoClubes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonIniciarImportacaoFundos
            // 
            this.buttonIniciarImportacaoFundos.Location = new System.Drawing.Point(47, 112);
            this.buttonIniciarImportacaoFundos.Name = "buttonIniciarImportacaoFundos";
            this.buttonIniciarImportacaoFundos.Size = new System.Drawing.Size(172, 23);
            this.buttonIniciarImportacaoFundos.TabIndex = 0;
            this.buttonIniciarImportacaoFundos.Text = "Iniciar Importação Fundos!";
            this.buttonIniciarImportacaoFundos.UseVisualStyleBackColor = true;
            this.buttonIniciarImportacaoFundos.Click += new System.EventHandler(this.buttonIniciarImportacaoFundos_Click);
            // 
            // buttonIniciarImportacaoClubes
            // 
            this.buttonIniciarImportacaoClubes.Location = new System.Drawing.Point(47, 167);
            this.buttonIniciarImportacaoClubes.Name = "buttonIniciarImportacaoClubes";
            this.buttonIniciarImportacaoClubes.Size = new System.Drawing.Size(172, 23);
            this.buttonIniciarImportacaoClubes.TabIndex = 1;
            this.buttonIniciarImportacaoClubes.Text = "Iniciar Importação Clubes!";
            this.buttonIniciarImportacaoClubes.UseVisualStyleBackColor = true;
            this.buttonIniciarImportacaoClubes.Click += new System.EventHandler(this.buttonIniciarImportacaoClubes_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.buttonIniciarImportacaoClubes);
            this.Controls.Add(this.buttonIniciarImportacaoFundos);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonIniciarImportacaoFundos;
        private System.Windows.Forms.Button buttonIniciarImportacaoClubes;
    }
}


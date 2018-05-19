namespace ConversorTesouroDireto
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
            this.btGerarPlanilha = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtFile1 = new System.Windows.Forms.TextBox();
            this.btEscolherArquivo = new System.Windows.Forms.Button();
            this.txtFile2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btGerarPlanilha
            // 
            this.btGerarPlanilha.Location = new System.Drawing.Point(542, 103);
            this.btGerarPlanilha.Name = "btGerarPlanilha";
            this.btGerarPlanilha.Size = new System.Drawing.Size(139, 23);
            this.btGerarPlanilha.TabIndex = 0;
            this.btGerarPlanilha.Text = "Gerar Planilha";
            this.btGerarPlanilha.UseVisualStyleBackColor = true;
            this.btGerarPlanilha.Click += new System.EventHandler(this.btGerarPlanilha_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtFile1
            // 
            this.txtFile1.Location = new System.Drawing.Point(31, 41);
            this.txtFile1.Name = "txtFile1";
            this.txtFile1.Size = new System.Drawing.Size(471, 20);
            this.txtFile1.TabIndex = 1;
            // 
            // btEscolherArquivo
            // 
            this.btEscolherArquivo.Location = new System.Drawing.Point(542, 41);
            this.btEscolherArquivo.Name = "btEscolherArquivo";
            this.btEscolherArquivo.Size = new System.Drawing.Size(134, 23);
            this.btEscolherArquivo.TabIndex = 2;
            this.btEscolherArquivo.Text = "Escolher Arquivo...";
            this.btEscolherArquivo.UseVisualStyleBackColor = true;
            this.btEscolherArquivo.Click += new System.EventHandler(this.btEscolherArquivo_Click);
            // 
            // txtFile2
            // 
            this.txtFile2.Location = new System.Drawing.Point(31, 103);
            this.txtFile2.Name = "txtFile2";
            this.txtFile2.ReadOnly = true;
            this.txtFile2.Size = new System.Drawing.Size(471, 20);
            this.txtFile2.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 188);
            this.Controls.Add(this.txtFile2);
            this.Controls.Add(this.btEscolherArquivo);
            this.Controls.Add(this.txtFile1);
            this.Controls.Add(this.btGerarPlanilha);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btGerarPlanilha;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtFile1;
        private System.Windows.Forms.Button btEscolherArquivo;
        private System.Windows.Forms.TextBox txtFile2;
    }
}


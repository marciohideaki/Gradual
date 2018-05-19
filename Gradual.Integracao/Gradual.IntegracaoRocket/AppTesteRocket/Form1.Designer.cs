namespace AppTesteRocket
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtCpfCnpj = new System.Windows.Forms.TextBox();
            this.txtDtNascFund = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIDCapivara = new System.Windows.Forms.TextBox();
            this.btDetalhe = new System.Windows.Forms.Button();
            this.btObterEvolucao = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(365, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Via WS direto";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(365, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Via Srv Integracao";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtCpfCnpj
            // 
            this.txtCpfCnpj.Location = new System.Drawing.Point(105, 12);
            this.txtCpfCnpj.Name = "txtCpfCnpj";
            this.txtCpfCnpj.Size = new System.Drawing.Size(164, 20);
            this.txtCpfCnpj.TabIndex = 2;
            // 
            // txtDtNascFund
            // 
            this.txtDtNascFund.Location = new System.Drawing.Point(105, 39);
            this.txtDtNascFund.Name = "txtDtNascFund";
            this.txtDtNascFund.Size = new System.Drawing.Size(164, 20);
            this.txtDtNascFund.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "CPF/CNPJ :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "DT Nasc/Fund:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 298);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Capivara:";
            // 
            // txtIDCapivara
            // 
            this.txtIDCapivara.Location = new System.Drawing.Point(99, 298);
            this.txtIDCapivara.Name = "txtIDCapivara";
            this.txtIDCapivara.Size = new System.Drawing.Size(100, 20);
            this.txtIDCapivara.TabIndex = 7;
            // 
            // btDetalhe
            // 
            this.btDetalhe.Location = new System.Drawing.Point(236, 278);
            this.btDetalhe.Name = "btDetalhe";
            this.btDetalhe.Size = new System.Drawing.Size(199, 23);
            this.btDetalhe.TabIndex = 8;
            this.btDetalhe.Text = "Obter Detalhamento Relatorio";
            this.btDetalhe.UseVisualStyleBackColor = true;
            this.btDetalhe.Click += new System.EventHandler(this.btDetalhe_Click);
            // 
            // btObterEvolucao
            // 
            this.btObterEvolucao.Location = new System.Drawing.Point(236, 308);
            this.btObterEvolucao.Name = "btObterEvolucao";
            this.btObterEvolucao.Size = new System.Drawing.Size(199, 23);
            this.btObterEvolucao.TabIndex = 9;
            this.btObterEvolucao.Text = "Obter Evolucao Processo";
            this.btObterEvolucao.UseVisualStyleBackColor = true;
            this.btObterEvolucao.Click += new System.EventHandler(this.btObterEvolucao_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 382);
            this.Controls.Add(this.btObterEvolucao);
            this.Controls.Add(this.btDetalhe);
            this.Controls.Add(this.txtIDCapivara);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDtNascFund);
            this.Controls.Add(this.txtCpfCnpj);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtCpfCnpj;
        private System.Windows.Forms.TextBox txtDtNascFund;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIDCapivara;
        private System.Windows.Forms.Button btDetalhe;
        private System.Windows.Forms.Button btObterEvolucao;
    }
}


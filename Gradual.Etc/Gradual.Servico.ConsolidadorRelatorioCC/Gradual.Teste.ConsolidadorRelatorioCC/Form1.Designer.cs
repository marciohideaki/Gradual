namespace Teste.ConsolidadorRelatorioCC
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
            this.ButtonAtivar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LabelAssuntoRetorno = new System.Windows.Forms.Label();
            this.LabelRetorno = new System.Windows.Forms.Label();
            this.buttonCarregarLista = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonAtivar
            // 
            this.ButtonAtivar.Location = new System.Drawing.Point(103, 145);
            this.ButtonAtivar.Name = "ButtonAtivar";
            this.ButtonAtivar.Size = new System.Drawing.Size(75, 23);
            this.ButtonAtivar.TabIndex = 0;
            this.ButtonAtivar.Text = "Ativar";
            this.ButtonAtivar.UseVisualStyleBackColor = true;
            this.ButtonAtivar.Click += new System.EventHandler(this.ButtonAtivar_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 66);
            this.label1.TabIndex = 1;
            this.label1.Text = "Teste do serviço Consolidador de dados de Conta Corrente";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LabelAssuntoRetorno
            // 
            this.LabelAssuntoRetorno.AutoSize = true;
            this.LabelAssuntoRetorno.Location = new System.Drawing.Point(17, 192);
            this.LabelAssuntoRetorno.Name = "LabelAssuntoRetorno";
            this.LabelAssuntoRetorno.Size = new System.Drawing.Size(48, 13);
            this.LabelAssuntoRetorno.TabIndex = 2;
            this.LabelAssuntoRetorno.Text = "Retorno:";
            // 
            // LabelRetorno
            // 
            this.LabelRetorno.AutoSize = true;
            this.LabelRetorno.Location = new System.Drawing.Point(71, 192);
            this.LabelRetorno.Name = "LabelRetorno";
            this.LabelRetorno.Size = new System.Drawing.Size(0, 13);
            this.LabelRetorno.TabIndex = 3;
            // 
            // buttonCarregarLista
            // 
            this.buttonCarregarLista.Location = new System.Drawing.Point(103, 99);
            this.buttonCarregarLista.Name = "buttonCarregarLista";
            this.buttonCarregarLista.Size = new System.Drawing.Size(75, 23);
            this.buttonCarregarLista.TabIndex = 4;
            this.buttonCarregarLista.Text = "carregar lista";
            this.buttonCarregarLista.UseVisualStyleBackColor = true;
            this.buttonCarregarLista.Click += new System.EventHandler(this.buttonCarregarLista_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.buttonCarregarLista);
            this.Controls.Add(this.LabelRetorno);
            this.Controls.Add(this.LabelAssuntoRetorno);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonAtivar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonAtivar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelAssuntoRetorno;
        private System.Windows.Forms.Label LabelRetorno;
        private System.Windows.Forms.Button buttonCarregarLista;
    }
}


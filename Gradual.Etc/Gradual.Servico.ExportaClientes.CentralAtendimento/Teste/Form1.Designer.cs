namespace Teste
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
            this.btnExportarAssessores = new System.Windows.Forms.Button();
            this.btnExportarClientes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExportarAssessores
            // 
            this.btnExportarAssessores.Location = new System.Drawing.Point(27, 30);
            this.btnExportarAssessores.Name = "btnExportarAssessores";
            this.btnExportarAssessores.Size = new System.Drawing.Size(159, 23);
            this.btnExportarAssessores.TabIndex = 0;
            this.btnExportarAssessores.Text = "Exportar Assessores";
            this.btnExportarAssessores.UseVisualStyleBackColor = true;
            this.btnExportarAssessores.Click += new System.EventHandler(this.btnExportarAssessores_Click);
            // 
            // btnExportarClientes
            // 
            this.btnExportarClientes.Location = new System.Drawing.Point(27, 74);
            this.btnExportarClientes.Name = "btnExportarClientes";
            this.btnExportarClientes.Size = new System.Drawing.Size(159, 23);
            this.btnExportarClientes.TabIndex = 1;
            this.btnExportarClientes.Text = "Exportar Clientes";
            this.btnExportarClientes.UseVisualStyleBackColor = true;
            this.btnExportarClientes.Click += new System.EventHandler(this.btnExportarClientes_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 227);
            this.Controls.Add(this.btnExportarClientes);
            this.Controls.Add(this.btnExportarAssessores);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExportarAssessores;
        private System.Windows.Forms.Button btnExportarClientes;
    }
}


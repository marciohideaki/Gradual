namespace CommServerClientSample
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
            this.btConectar = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtInstrumento = new System.Windows.Forms.TextBox();
            this.btAssinar = new System.Windows.Forms.Button();
            this.btCancelarAssinatura = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btConectar
            // 
            this.btConectar.Location = new System.Drawing.Point(189, 12);
            this.btConectar.Name = "btConectar";
            this.btConectar.Size = new System.Drawing.Size(75, 23);
            this.btConectar.TabIndex = 0;
            this.btConectar.Text = "Conectar";
            this.btConectar.UseVisualStyleBackColor = true;
            this.btConectar.Click += new System.EventHandler(this.btConectar_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(38, 126);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsg.Size = new System.Drawing.Size(432, 123);
            this.txtMsg.TabIndex = 1;
            // 
            // txtInstrumento
            // 
            this.txtInstrumento.Location = new System.Drawing.Point(38, 71);
            this.txtInstrumento.Name = "txtInstrumento";
            this.txtInstrumento.Size = new System.Drawing.Size(100, 20);
            this.txtInstrumento.TabIndex = 2;
            // 
            // btAssinar
            // 
            this.btAssinar.Location = new System.Drawing.Point(189, 71);
            this.btAssinar.Name = "btAssinar";
            this.btAssinar.Size = new System.Drawing.Size(75, 23);
            this.btAssinar.TabIndex = 3;
            this.btAssinar.Text = "Assinar";
            this.btAssinar.UseVisualStyleBackColor = true;
            this.btAssinar.Click += new System.EventHandler(this.btAssinar_Click);
            // 
            // btCancelarAssinatura
            // 
            this.btCancelarAssinatura.Location = new System.Drawing.Point(310, 71);
            this.btCancelarAssinatura.Name = "btCancelarAssinatura";
            this.btCancelarAssinatura.Size = new System.Drawing.Size(123, 23);
            this.btCancelarAssinatura.TabIndex = 4;
            this.btCancelarAssinatura.Text = "Cancelar Assinatura";
            this.btCancelarAssinatura.UseVisualStyleBackColor = true;
            this.btCancelarAssinatura.Click += new System.EventHandler(this.btCancelarAssinatura_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 261);
            this.Controls.Add(this.btCancelarAssinatura);
            this.Controls.Add(this.btAssinar);
            this.Controls.Add(this.txtInstrumento);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btConectar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btConectar;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtInstrumento;
        private System.Windows.Forms.Button btAssinar;
        private System.Windows.Forms.Button btCancelarAssinatura;
    }
}


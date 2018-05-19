namespace AppTesteAutomacao
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
            this.btDesconectar = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtRequestMsg = new System.Windows.Forms.TextBox();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btConectar
            // 
            this.btConectar.Location = new System.Drawing.Point(13, 13);
            this.btConectar.Name = "btConectar";
            this.btConectar.Size = new System.Drawing.Size(75, 23);
            this.btConectar.TabIndex = 0;
            this.btConectar.Text = "Conectar";
            this.btConectar.UseVisualStyleBackColor = true;
            this.btConectar.Click += new System.EventHandler(this.btConectar_Click);
            // 
            // btDesconectar
            // 
            this.btDesconectar.Location = new System.Drawing.Point(113, 12);
            this.btDesconectar.Name = "btDesconectar";
            this.btDesconectar.Size = new System.Drawing.Size(75, 23);
            this.btDesconectar.TabIndex = 1;
            this.btDesconectar.Text = "Disconectar";
            this.btDesconectar.UseVisualStyleBackColor = true;
            this.btDesconectar.Click += new System.EventHandler(this.btDesconectar_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(212, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // txtRequestMsg
            // 
            this.txtRequestMsg.Location = new System.Drawing.Point(13, 64);
            this.txtRequestMsg.Name = "txtRequestMsg";
            this.txtRequestMsg.Size = new System.Drawing.Size(195, 20);
            this.txtRequestMsg.TabIndex = 3;
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(13, 127);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(530, 20);
            this.txtMsg.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 301);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.txtRequestMsg);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btDesconectar);
            this.Controls.Add(this.btConectar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btConectar;
        private System.Windows.Forms.Button btDesconectar;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtRequestMsg;
        private System.Windows.Forms.TextBox txtMsg;
    }
}


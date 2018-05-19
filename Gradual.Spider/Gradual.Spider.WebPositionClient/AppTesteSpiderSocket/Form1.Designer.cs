namespace AppTesteSpiderSocket
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
            this.txtMsgServer = new System.Windows.Forms.TextBox();
            this.btStartServer = new System.Windows.Forms.Button();
            this.btStartClient = new System.Windows.Forms.Button();
            this.txtClient = new System.Windows.Forms.TextBox();
            this.btEnviarServer = new System.Windows.Forms.Button();
            this.btEnviarCliente = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMsgServer
            // 
            this.txtMsgServer.Location = new System.Drawing.Point(37, 75);
            this.txtMsgServer.Multiline = true;
            this.txtMsgServer.Name = "txtMsgServer";
            this.txtMsgServer.Size = new System.Drawing.Size(524, 66);
            this.txtMsgServer.TabIndex = 0;
            // 
            // btStartServer
            // 
            this.btStartServer.Location = new System.Drawing.Point(37, 12);
            this.btStartServer.Name = "btStartServer";
            this.btStartServer.Size = new System.Drawing.Size(75, 23);
            this.btStartServer.TabIndex = 1;
            this.btStartServer.Text = "Start Server";
            this.btStartServer.UseVisualStyleBackColor = true;
            this.btStartServer.Click += new System.EventHandler(this.btStartServer_Click);
            // 
            // btStartClient
            // 
            this.btStartClient.Location = new System.Drawing.Point(134, 12);
            this.btStartClient.Name = "btStartClient";
            this.btStartClient.Size = new System.Drawing.Size(75, 23);
            this.btStartClient.TabIndex = 2;
            this.btStartClient.Text = "Start Client";
            this.btStartClient.UseVisualStyleBackColor = true;
            this.btStartClient.Click += new System.EventHandler(this.btStartClient_Click);
            // 
            // txtClient
            // 
            this.txtClient.Location = new System.Drawing.Point(37, 163);
            this.txtClient.Multiline = true;
            this.txtClient.Name = "txtClient";
            this.txtClient.Size = new System.Drawing.Size(524, 71);
            this.txtClient.TabIndex = 3;
            // 
            // btEnviarServer
            // 
            this.btEnviarServer.Location = new System.Drawing.Point(567, 89);
            this.btEnviarServer.Name = "btEnviarServer";
            this.btEnviarServer.Size = new System.Drawing.Size(94, 23);
            this.btEnviarServer.TabIndex = 4;
            this.btEnviarServer.Text = "Enviar P Cliente";
            this.btEnviarServer.UseVisualStyleBackColor = true;
            this.btEnviarServer.Click += new System.EventHandler(this.btEnviarServer_Click);
            // 
            // btEnviarCliente
            // 
            this.btEnviarCliente.Location = new System.Drawing.Point(568, 163);
            this.btEnviarCliente.Name = "btEnviarCliente";
            this.btEnviarCliente.Size = new System.Drawing.Size(93, 23);
            this.btEnviarCliente.TabIndex = 5;
            this.btEnviarCliente.Text = "Enviar P Server";
            this.btEnviarCliente.UseVisualStyleBackColor = true;
            this.btEnviarCliente.Click += new System.EventHandler(this.btEnviarCliente_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(568, 193);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(568, 222);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 257);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btEnviarCliente);
            this.Controls.Add(this.btEnviarServer);
            this.Controls.Add(this.txtClient);
            this.Controls.Add(this.btStartClient);
            this.Controls.Add(this.btStartServer);
            this.Controls.Add(this.txtMsgServer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMsgServer;
        private System.Windows.Forms.Button btStartServer;
        private System.Windows.Forms.Button btStartClient;
        private System.Windows.Forms.TextBox txtClient;
        private System.Windows.Forms.Button btEnviarServer;
        private System.Windows.Forms.Button btEnviarCliente;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}


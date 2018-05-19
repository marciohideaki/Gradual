namespace AppTesteWSBTG
{
    partial class frmTrades
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
            this.btConsultar = new System.Windows.Forms.Button();
            this.txtTrades = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLastSeq = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbUrl = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btConsultar
            // 
            this.btConsultar.Location = new System.Drawing.Point(754, 23);
            this.btConsultar.Name = "btConsultar";
            this.btConsultar.Size = new System.Drawing.Size(75, 23);
            this.btConsultar.TabIndex = 0;
            this.btConsultar.Text = "Consultar";
            this.btConsultar.UseVisualStyleBackColor = true;
            this.btConsultar.Click += new System.EventHandler(this.btConsultar_Click);
            // 
            // txtTrades
            // 
            this.txtTrades.Location = new System.Drawing.Point(24, 145);
            this.txtTrades.Multiline = true;
            this.txtTrades.Name = "txtTrades";
            this.txtTrades.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTrades.Size = new System.Drawing.Size(805, 260);
            this.txtTrades.TabIndex = 1;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(105, 25);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 2;
            this.txtUsername.Text = "btgpactual";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(105, 52);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Text = "btgpactual";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password:";
            // 
            // txtLastSeq
            // 
            this.txtLastSeq.Location = new System.Drawing.Point(105, 79);
            this.txtLastSeq.Name = "txtLastSeq";
            this.txtLastSeq.Size = new System.Drawing.Size(100, 20);
            this.txtLastSeq.TabIndex = 6;
            this.txtLastSeq.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "LastSequence:";
            // 
            // cmbUrl
            // 
            this.cmbUrl.Location = new System.Drawing.Point(105, 106);
            this.cmbUrl.Name = "cmbUrl";
            this.cmbUrl.Size = new System.Drawing.Size(431, 21);
            this.cmbUrl.TabIndex = 8;
            // 
            // frmTrades
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 420);
            this.Controls.Add(this.cmbUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLastSeq);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtTrades);
            this.Controls.Add(this.btConsultar);
            this.Name = "frmTrades";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmTrades_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btConsultar;
        private System.Windows.Forms.TextBox txtTrades;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLastSeq;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbUrl;
    }
}


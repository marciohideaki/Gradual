namespace MdsManagerApp
{
    partial class frmMdsManagerApp
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
            this.btAtivar = new System.Windows.Forms.Button();
            this.btDesativar = new System.Windows.Forms.Button();
            this.cmbChannelID = new System.Windows.Forms.ComboBox();
            this.btPausarCanal = new System.Windows.Forms.Button();
            this.btRetomar = new System.Windows.Forms.Button();
            this.btD330 = new System.Windows.Forms.Button();
            this.txtSeqNumIni = new System.Windows.Forms.TextBox();
            this.txtSeqNumFim = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btRecInterval = new System.Windows.Forms.Button();
            this.cbProduct = new System.Windows.Forms.ComboBox();
            this.cbSecurityType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtInstrumento = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btResend = new System.Windows.Forms.Button();
            this.btSeqReset = new System.Windows.Forms.Button();
            this.btSubSecList = new System.Windows.Forms.Button();
            this.btUnsubSecList = new System.Windows.Forms.Button();
            this.btSubMarketData = new System.Windows.Forms.Button();
            this.btUnsubMData = new System.Windows.Forms.Button();
            this.cbRequestSent = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCFICode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btAtivarConflated = new System.Windows.Forms.Button();
            this.btDesativConflated = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1059, 712);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btAtivar
            // 
            this.btAtivar.Location = new System.Drawing.Point(195, 12);
            this.btAtivar.Name = "btAtivar";
            this.btAtivar.Size = new System.Drawing.Size(75, 23);
            this.btAtivar.TabIndex = 1;
            this.btAtivar.Text = "Ativar";
            this.btAtivar.UseVisualStyleBackColor = true;
            this.btAtivar.Click += new System.EventHandler(this.btAtivar_Click);
            // 
            // btDesativar
            // 
            this.btDesativar.Location = new System.Drawing.Point(195, 42);
            this.btDesativar.Name = "btDesativar";
            this.btDesativar.Size = new System.Drawing.Size(75, 23);
            this.btDesativar.TabIndex = 2;
            this.btDesativar.Text = "Desativar";
            this.btDesativar.UseVisualStyleBackColor = true;
            this.btDesativar.Click += new System.EventHandler(this.btDesativar_Click);
            // 
            // cmbChannelID
            // 
            this.cmbChannelID.FormattingEnabled = true;
            this.cmbChannelID.Location = new System.Drawing.Point(23, 12);
            this.cmbChannelID.Name = "cmbChannelID";
            this.cmbChannelID.Size = new System.Drawing.Size(121, 21);
            this.cmbChannelID.TabIndex = 3;
            // 
            // btPausarCanal
            // 
            this.btPausarCanal.Location = new System.Drawing.Point(195, 72);
            this.btPausarCanal.Name = "btPausarCanal";
            this.btPausarCanal.Size = new System.Drawing.Size(75, 23);
            this.btPausarCanal.TabIndex = 4;
            this.btPausarCanal.Text = "Pausar";
            this.btPausarCanal.UseVisualStyleBackColor = true;
            this.btPausarCanal.Click += new System.EventHandler(this.btPausarCanal_Click);
            // 
            // btRetomar
            // 
            this.btRetomar.Location = new System.Drawing.Point(195, 102);
            this.btRetomar.Name = "btRetomar";
            this.btRetomar.Size = new System.Drawing.Size(75, 23);
            this.btRetomar.TabIndex = 5;
            this.btRetomar.Text = "Retomar";
            this.btRetomar.UseVisualStyleBackColor = true;
            this.btRetomar.Click += new System.EventHandler(this.btRetomar_Click);
            // 
            // btD330
            // 
            this.btD330.Location = new System.Drawing.Point(195, 132);
            this.btD330.Name = "btD330";
            this.btD330.Size = new System.Drawing.Size(75, 23);
            this.btD330.TabIndex = 6;
            this.btD330.Text = "D3.30";
            this.btD330.UseVisualStyleBackColor = true;
            this.btD330.Click += new System.EventHandler(this.btD330_Click);
            // 
            // txtSeqNumIni
            // 
            this.txtSeqNumIni.Location = new System.Drawing.Point(79, 134);
            this.txtSeqNumIni.Name = "txtSeqNumIni";
            this.txtSeqNumIni.Size = new System.Drawing.Size(78, 20);
            this.txtSeqNumIni.TabIndex = 7;
            // 
            // txtSeqNumFim
            // 
            this.txtSeqNumFim.Location = new System.Drawing.Point(79, 161);
            this.txtSeqNumFim.Name = "txtSeqNumFim";
            this.txtSeqNumFim.Size = new System.Drawing.Size(78, 20);
            this.txtSeqNumFim.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Seqnum Ini:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Seqnum Fim:";
            // 
            // btRecInterval
            // 
            this.btRecInterval.Location = new System.Drawing.Point(195, 164);
            this.btRecInterval.Name = "btRecInterval";
            this.btRecInterval.Size = new System.Drawing.Size(75, 23);
            this.btRecInterval.TabIndex = 11;
            this.btRecInterval.Text = "Recover.Int.";
            this.btRecInterval.UseVisualStyleBackColor = true;
            this.btRecInterval.Click += new System.EventHandler(this.btRecInterval_Click);
            // 
            // cbProduct
            // 
            this.cbProduct.FormattingEnabled = true;
            this.cbProduct.Location = new System.Drawing.Point(86, 226);
            this.cbProduct.Name = "cbProduct";
            this.cbProduct.Size = new System.Drawing.Size(121, 21);
            this.cbProduct.TabIndex = 12;
            // 
            // cbSecurityType
            // 
            this.cbSecurityType.FormattingEnabled = true;
            this.cbSecurityType.Location = new System.Drawing.Point(86, 255);
            this.cbSecurityType.Name = "cbSecurityType";
            this.cbSecurityType.Size = new System.Drawing.Size(121, 21);
            this.cbSecurityType.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Conflated ----------------------------------------";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Product:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 258);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "SecurityType:";
            // 
            // txtInstrumento
            // 
            this.txtInstrumento.Location = new System.Drawing.Point(86, 309);
            this.txtInstrumento.Name = "txtInstrumento";
            this.txtInstrumento.Size = new System.Drawing.Size(100, 20);
            this.txtInstrumento.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 313);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Instrumento:";
            // 
            // btResend
            // 
            this.btResend.Location = new System.Drawing.Point(396, 215);
            this.btResend.Name = "btResend";
            this.btResend.Size = new System.Drawing.Size(75, 23);
            this.btResend.TabIndex = 19;
            this.btResend.Text = "Resend";
            this.btResend.UseVisualStyleBackColor = true;
            this.btResend.Click += new System.EventHandler(this.btResend_Click);
            // 
            // btSeqReset
            // 
            this.btSeqReset.Location = new System.Drawing.Point(396, 243);
            this.btSeqReset.Name = "btSeqReset";
            this.btSeqReset.Size = new System.Drawing.Size(75, 23);
            this.btSeqReset.TabIndex = 20;
            this.btSeqReset.Text = "Seq. Reset";
            this.btSeqReset.UseVisualStyleBackColor = true;
            this.btSeqReset.Click += new System.EventHandler(this.btSeqReset_Click);
            // 
            // btSubSecList
            // 
            this.btSubSecList.Location = new System.Drawing.Point(383, 271);
            this.btSubSecList.Name = "btSubSecList";
            this.btSubSecList.Size = new System.Drawing.Size(126, 23);
            this.btSubSecList.TabIndex = 21;
            this.btSubSecList.Text = "Subscribe SecList";
            this.btSubSecList.UseVisualStyleBackColor = true;
            this.btSubSecList.Click += new System.EventHandler(this.btSubSecList_Click);
            // 
            // btUnsubSecList
            // 
            this.btUnsubSecList.Location = new System.Drawing.Point(383, 299);
            this.btUnsubSecList.Name = "btUnsubSecList";
            this.btUnsubSecList.Size = new System.Drawing.Size(126, 23);
            this.btUnsubSecList.TabIndex = 22;
            this.btUnsubSecList.Text = "Unsubscribe SecList";
            this.btUnsubSecList.UseVisualStyleBackColor = true;
            this.btUnsubSecList.Click += new System.EventHandler(this.btUnsubSecList_Click);
            // 
            // btSubMarketData
            // 
            this.btSubMarketData.Location = new System.Drawing.Point(383, 327);
            this.btSubMarketData.Name = "btSubMarketData";
            this.btSubMarketData.Size = new System.Drawing.Size(126, 23);
            this.btSubMarketData.TabIndex = 23;
            this.btSubMarketData.Text = "Subscribe MarketData";
            this.btSubMarketData.UseVisualStyleBackColor = true;
            this.btSubMarketData.Click += new System.EventHandler(this.btSubMarketData_Click);
            // 
            // btUnsubMData
            // 
            this.btUnsubMData.Location = new System.Drawing.Point(383, 355);
            this.btUnsubMData.Name = "btUnsubMData";
            this.btUnsubMData.Size = new System.Drawing.Size(126, 23);
            this.btUnsubMData.TabIndex = 24;
            this.btUnsubMData.Text = "Unsubscribe MData";
            this.btUnsubMData.UseVisualStyleBackColor = true;
            this.btUnsubMData.Click += new System.EventHandler(this.btUnsubMData_Click);
            // 
            // cbRequestSent
            // 
            this.cbRequestSent.FormattingEnabled = true;
            this.cbRequestSent.Location = new System.Drawing.Point(86, 353);
            this.cbRequestSent.Name = "cbRequestSent";
            this.cbRequestSent.Size = new System.Drawing.Size(291, 21);
            this.cbRequestSent.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 355);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Requests Sent:";
            // 
            // txtCFICode
            // 
            this.txtCFICode.Location = new System.Drawing.Point(86, 283);
            this.txtCFICode.Name = "txtCFICode";
            this.txtCFICode.Size = new System.Drawing.Size(100, 20);
            this.txtCFICode.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 285);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "CFICode:";
            // 
            // btAtivarConflated
            // 
            this.btAtivarConflated.Location = new System.Drawing.Point(396, 153);
            this.btAtivarConflated.Name = "btAtivarConflated";
            this.btAtivarConflated.Size = new System.Drawing.Size(113, 23);
            this.btAtivarConflated.TabIndex = 29;
            this.btAtivarConflated.Text = "Ativar Conflated";
            this.btAtivarConflated.UseVisualStyleBackColor = true;
            this.btAtivarConflated.Click += new System.EventHandler(this.btAtivarConflated_Click);
            // 
            // btDesativConflated
            // 
            this.btDesativConflated.Location = new System.Drawing.Point(396, 183);
            this.btDesativConflated.Name = "btDesativConflated";
            this.btDesativConflated.Size = new System.Drawing.Size(113, 23);
            this.btDesativConflated.TabIndex = 30;
            this.btDesativConflated.Text = "Desativar Conflated";
            this.btDesativConflated.UseVisualStyleBackColor = true;
            this.btDesativConflated.Click += new System.EventHandler(this.btDesativConflated_Click);
            // 
            // frmMdsManagerApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 393);
            this.Controls.Add(this.btDesativConflated);
            this.Controls.Add(this.btAtivarConflated);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtCFICode);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbRequestSent);
            this.Controls.Add(this.btUnsubMData);
            this.Controls.Add(this.btSubMarketData);
            this.Controls.Add(this.btUnsubSecList);
            this.Controls.Add(this.btSubSecList);
            this.Controls.Add(this.btSeqReset);
            this.Controls.Add(this.btResend);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtInstrumento);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSecurityType);
            this.Controls.Add(this.cbProduct);
            this.Controls.Add(this.btRecInterval);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSeqNumFim);
            this.Controls.Add(this.txtSeqNumIni);
            this.Controls.Add(this.btD330);
            this.Controls.Add(this.btRetomar);
            this.Controls.Add(this.btPausarCanal);
            this.Controls.Add(this.cmbChannelID);
            this.Controls.Add(this.btDesativar);
            this.Controls.Add(this.btAtivar);
            this.Controls.Add(this.button1);
            this.Name = "frmMdsManagerApp";
            this.Text = "MdsManagerApp";
            this.Load += new System.EventHandler(this.frmMdsManagerApp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btAtivar;
        private System.Windows.Forms.Button btDesativar;
        private System.Windows.Forms.ComboBox cmbChannelID;
        private System.Windows.Forms.Button btPausarCanal;
        private System.Windows.Forms.Button btRetomar;
        private System.Windows.Forms.Button btD330;
        private System.Windows.Forms.TextBox txtSeqNumIni;
        private System.Windows.Forms.TextBox txtSeqNumFim;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btRecInterval;
        private System.Windows.Forms.ComboBox cbProduct;
        private System.Windows.Forms.ComboBox cbSecurityType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtInstrumento;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btResend;
        private System.Windows.Forms.Button btSeqReset;
        private System.Windows.Forms.Button btSubSecList;
        private System.Windows.Forms.Button btUnsubSecList;
        private System.Windows.Forms.Button btSubMarketData;
        private System.Windows.Forms.Button btUnsubMData;
        private System.Windows.Forms.ComboBox cbRequestSent;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCFICode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btAtivarConflated;
        private System.Windows.Forms.Button btDesativConflated;
    }
}


namespace WindowsFormSmartTraderTest
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
            this.btnDummy = new System.Windows.Forms.Button();
            this.lbAtivo = new System.Windows.Forms.Label();
            this.txtAtivo = new System.Windows.Forms.TextBox();
            this.lbQtd = new System.Windows.Forms.Label();
            this.txtQtd = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIniDisparo = new System.Windows.Forms.TextBox();
            this.txtIniOrdem = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtLValor = new System.Windows.Forms.TextBox();
            this.cbLTipoValor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLOrdem = new System.Windows.Forms.TextBox();
            this.txtLDisparo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtPValor = new System.Windows.Forms.TextBox();
            this.cbPTipoValor = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPOrdem = new System.Windows.Forms.TextBox();
            this.txtPDisparo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnOrderProcessor = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDummy
            // 
            this.btnDummy.Location = new System.Drawing.Point(12, 197);
            this.btnDummy.Name = "btnDummy";
            this.btnDummy.Size = new System.Drawing.Size(75, 23);
            this.btnDummy.TabIndex = 0;
            this.btnDummy.Text = "Dummy";
            this.btnDummy.UseVisualStyleBackColor = true;
            this.btnDummy.Click += new System.EventHandler(this.btnDummy_Click);
            // 
            // lbAtivo
            // 
            this.lbAtivo.AutoSize = true;
            this.lbAtivo.Location = new System.Drawing.Point(24, 27);
            this.lbAtivo.Name = "lbAtivo";
            this.lbAtivo.Size = new System.Drawing.Size(31, 13);
            this.lbAtivo.TabIndex = 1;
            this.lbAtivo.Text = "Ativo";
            // 
            // txtAtivo
            // 
            this.txtAtivo.Location = new System.Drawing.Point(61, 24);
            this.txtAtivo.Name = "txtAtivo";
            this.txtAtivo.Size = new System.Drawing.Size(100, 20);
            this.txtAtivo.TabIndex = 2;
            // 
            // lbQtd
            // 
            this.lbQtd.AutoSize = true;
            this.lbQtd.Location = new System.Drawing.Point(171, 27);
            this.lbQtd.Name = "lbQtd";
            this.lbQtd.Size = new System.Drawing.Size(24, 13);
            this.lbQtd.TabIndex = 3;
            this.lbQtd.Text = "Qtd";
            // 
            // txtQtd
            // 
            this.txtQtd.Location = new System.Drawing.Point(208, 24);
            this.txtQtd.Name = "txtQtd";
            this.txtQtd.Size = new System.Drawing.Size(100, 20);
            this.txtQtd.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbAtivo);
            this.groupBox1.Controls.Add(this.txtQtd);
            this.groupBox1.Controls.Add(this.txtAtivo);
            this.groupBox1.Controls.Add(this.lbQtd);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 56);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtIniDisparo);
            this.groupBox2.Controls.Add(this.txtIniOrdem);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(185, 103);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Inicio";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pç Disparo";
            // 
            // txtIniDisparo
            // 
            this.txtIniDisparo.Location = new System.Drawing.Point(71, 21);
            this.txtIniDisparo.Name = "txtIniDisparo";
            this.txtIniDisparo.Size = new System.Drawing.Size(100, 20);
            this.txtIniDisparo.TabIndex = 4;
            // 
            // txtIniOrdem
            // 
            this.txtIniOrdem.Location = new System.Drawing.Point(71, 51);
            this.txtIniOrdem.Name = "txtIniOrdem";
            this.txtIniOrdem.Size = new System.Drawing.Size(100, 20);
            this.txtIniOrdem.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Pç Ordem";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtLValor);
            this.groupBox3.Controls.Add(this.cbLTipoValor);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtLOrdem);
            this.groupBox3.Controls.Add(this.txtLDisparo);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(203, 74);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 103);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Fim c/ Lucro";
            // 
            // txtLValor
            // 
            this.txtLValor.Location = new System.Drawing.Point(108, 21);
            this.txtLValor.Name = "txtLValor";
            this.txtLValor.Size = new System.Drawing.Size(60, 20);
            this.txtLValor.TabIndex = 6;
            // 
            // cbLTipoValor
            // 
            this.cbLTipoValor.FormattingEnabled = true;
            this.cbLTipoValor.Items.AddRange(new object[] {
            "Spread",
            "Cotação",
            "Financeiro"});
            this.cbLTipoValor.Location = new System.Drawing.Point(10, 21);
            this.cbLTipoValor.Name = "cbLTipoValor";
            this.cbLTipoValor.Size = new System.Drawing.Size(92, 21);
            this.cbLTipoValor.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Pç Disparo";
            // 
            // txtLOrdem
            // 
            this.txtLOrdem.Location = new System.Drawing.Point(68, 71);
            this.txtLOrdem.Name = "txtLOrdem";
            this.txtLOrdem.Size = new System.Drawing.Size(100, 20);
            this.txtLOrdem.TabIndex = 4;
            // 
            // txtLDisparo
            // 
            this.txtLDisparo.Location = new System.Drawing.Point(68, 48);
            this.txtLDisparo.Name = "txtLDisparo";
            this.txtLDisparo.Size = new System.Drawing.Size(100, 20);
            this.txtLDisparo.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Pç Ordem";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtPValor);
            this.groupBox4.Controls.Add(this.cbPTipoValor);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.txtPOrdem);
            this.groupBox4.Controls.Add(this.txtPDisparo);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(409, 74);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 103);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Fim c/ Prejú";
            // 
            // txtPValor
            // 
            this.txtPValor.Location = new System.Drawing.Point(108, 21);
            this.txtPValor.Name = "txtPValor";
            this.txtPValor.Size = new System.Drawing.Size(60, 20);
            this.txtPValor.TabIndex = 6;
            // 
            // cbPTipoValor
            // 
            this.cbPTipoValor.FormattingEnabled = true;
            this.cbPTipoValor.Items.AddRange(new object[] {
            "Spread",
            "Cotação",
            "Financeiro"});
            this.cbPTipoValor.Location = new System.Drawing.Point(10, 21);
            this.cbPTipoValor.Name = "cbPTipoValor";
            this.cbPTipoValor.Size = new System.Drawing.Size(92, 21);
            this.cbPTipoValor.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Pç Disparo";
            // 
            // txtPOrdem
            // 
            this.txtPOrdem.Location = new System.Drawing.Point(68, 71);
            this.txtPOrdem.Name = "txtPOrdem";
            this.txtPOrdem.Size = new System.Drawing.Size(100, 20);
            this.txtPOrdem.TabIndex = 4;
            // 
            // txtPDisparo
            // 
            this.txtPDisparo.Location = new System.Drawing.Point(68, 48);
            this.txtPDisparo.Name = "txtPDisparo";
            this.txtPDisparo.Size = new System.Drawing.Size(100, 20);
            this.txtPDisparo.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Pç Ordem";
            // 
            // btnOrderProcessor
            // 
            this.btnOrderProcessor.BackColor = System.Drawing.Color.LightGray;
            this.btnOrderProcessor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrderProcessor.ForeColor = System.Drawing.Color.DimGray;
            this.btnOrderProcessor.Location = new System.Drawing.Point(93, 197);
            this.btnOrderProcessor.Name = "btnOrderProcessor";
            this.btnOrderProcessor.Size = new System.Drawing.Size(146, 23);
            this.btnOrderProcessor.TabIndex = 9;
            this.btnOrderProcessor.Text = "Handling Order Processor";
            this.btnOrderProcessor.UseVisualStyleBackColor = false;
            this.btnOrderProcessor.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 234);
            this.Controls.Add(this.btnOrderProcessor);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDummy);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDummy;
        private System.Windows.Forms.Label lbAtivo;
        private System.Windows.Forms.TextBox txtAtivo;
        private System.Windows.Forms.Label lbQtd;
        private System.Windows.Forms.TextBox txtQtd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIniDisparo;
        private System.Windows.Forms.TextBox txtIniOrdem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtLValor;
        private System.Windows.Forms.ComboBox cbLTipoValor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLOrdem;
        private System.Windows.Forms.TextBox txtLDisparo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtPValor;
        private System.Windows.Forms.ComboBox cbPTipoValor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPOrdem;
        private System.Windows.Forms.TextBox txtPDisparo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnOrderProcessor;
    }
}


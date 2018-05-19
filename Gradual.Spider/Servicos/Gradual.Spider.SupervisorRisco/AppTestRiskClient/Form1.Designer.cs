namespace AppTestRiskClient
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
            this.btStop = new System.Windows.Forms.Button();
            this.btStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btValidAlteracao = new System.Windows.Forms.Button();
            this.btValidNova = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtAccount = new System.Windows.Forms.TextBox();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.txtOrderQty = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.rdBtCompra = new System.Windows.Forms.RadioButton();
            this.rdBtVenda = new System.Windows.Forms.RadioButton();
            this.rdbtBovespa = new System.Windows.Forms.RadioButton();
            this.rdbtBMF = new System.Windows.Forms.RadioButton();
            this.txtOrderID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOrigClOrdID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btReloadResync = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnConsolidatedRisk = new System.Windows.Forms.Button();
            this.btnPositionClient = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(469, 50);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(144, 23);
            this.btStop.TabIndex = 0;
            this.btStop.Text = "Stop Risk Client";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(469, 12);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(144, 23);
            this.btStart.TabIndex = 1;
            this.btStart.Text = "Start Risk Client";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Account:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Symbol:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "OrderQty:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Price:";
            // 
            // btValidAlteracao
            // 
            this.btValidAlteracao.Location = new System.Drawing.Point(173, 263);
            this.btValidAlteracao.Name = "btValidAlteracao";
            this.btValidAlteracao.Size = new System.Drawing.Size(134, 23);
            this.btValidAlteracao.TabIndex = 6;
            this.btValidAlteracao.Text = "Val. Alteracao";
            this.btValidAlteracao.UseVisualStyleBackColor = true;
            // 
            // btValidNova
            // 
            this.btValidNova.Location = new System.Drawing.Point(22, 263);
            this.btValidNova.Name = "btValidNova";
            this.btValidNova.Size = new System.Drawing.Size(134, 23);
            this.btValidNova.TabIndex = 7;
            this.btValidNova.Text = "Val. Nova Ordem";
            this.btValidNova.UseVisualStyleBackColor = true;
            this.btValidNova.Click += new System.EventHandler(this.btValidNova_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(19, 352);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(594, 63);
            this.txtResult.TabIndex = 8;
            // 
            // txtAccount
            // 
            this.txtAccount.Location = new System.Drawing.Point(70, 94);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(173, 20);
            this.txtAccount.TabIndex = 9;
            // 
            // txtSymbol
            // 
            this.txtSymbol.Location = new System.Drawing.Point(70, 121);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(173, 20);
            this.txtSymbol.TabIndex = 10;
            // 
            // txtOrderQty
            // 
            this.txtOrderQty.Location = new System.Drawing.Point(70, 148);
            this.txtOrderQty.Name = "txtOrderQty";
            this.txtOrderQty.Size = new System.Drawing.Size(173, 20);
            this.txtOrderQty.TabIndex = 11;
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(70, 175);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(173, 20);
            this.txtPrice.TabIndex = 12;
            // 
            // rdBtCompra
            // 
            this.rdBtCompra.AutoSize = true;
            this.rdBtCompra.Location = new System.Drawing.Point(70, 206);
            this.rdBtCompra.Name = "rdBtCompra";
            this.rdBtCompra.Size = new System.Drawing.Size(61, 17);
            this.rdBtCompra.TabIndex = 13;
            this.rdBtCompra.TabStop = true;
            this.rdBtCompra.Text = "Compra";
            this.rdBtCompra.UseVisualStyleBackColor = true;
            this.rdBtCompra.CheckedChanged += new System.EventHandler(this.rdBtCompra_CheckedChanged);
            // 
            // rdBtVenda
            // 
            this.rdBtVenda.AutoSize = true;
            this.rdBtVenda.Location = new System.Drawing.Point(135, 206);
            this.rdBtVenda.Name = "rdBtVenda";
            this.rdBtVenda.Size = new System.Drawing.Size(56, 17);
            this.rdBtVenda.TabIndex = 14;
            this.rdBtVenda.TabStop = true;
            this.rdBtVenda.Text = "Venda";
            this.rdBtVenda.UseVisualStyleBackColor = true;
            this.rdBtVenda.CheckedChanged += new System.EventHandler(this.rdBtVenda_CheckedChanged);
            // 
            // rdbtBovespa
            // 
            this.rdbtBovespa.AutoSize = true;
            this.rdbtBovespa.Location = new System.Drawing.Point(6, 12);
            this.rdbtBovespa.Name = "rdbtBovespa";
            this.rdbtBovespa.Size = new System.Drawing.Size(67, 17);
            this.rdbtBovespa.TabIndex = 15;
            this.rdbtBovespa.TabStop = true;
            this.rdbtBovespa.Text = "Bovespa";
            this.rdbtBovespa.UseVisualStyleBackColor = true;
            this.rdbtBovespa.CheckedChanged += new System.EventHandler(this.rdbtBovespa_CheckedChanged);
            // 
            // rdbtBMF
            // 
            this.rdbtBMF.AutoSize = true;
            this.rdbtBMF.Location = new System.Drawing.Point(79, 12);
            this.rdbtBMF.Name = "rdbtBMF";
            this.rdbtBMF.Size = new System.Drawing.Size(47, 17);
            this.rdbtBMF.TabIndex = 16;
            this.rdbtBMF.TabStop = true;
            this.rdbtBMF.Text = "BMF";
            this.rdbtBMF.UseVisualStyleBackColor = true;
            this.rdbtBMF.CheckedChanged += new System.EventHandler(this.rdbtBMF_CheckedChanged);
            // 
            // txtOrderID
            // 
            this.txtOrderID.Location = new System.Drawing.Point(70, 40);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(173, 20);
            this.txtOrderID.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "ClOrdID:";
            // 
            // txtOrigClOrdID
            // 
            this.txtOrigClOrdID.Location = new System.Drawing.Point(70, 67);
            this.txtOrigClOrdID.Name = "txtOrigClOrdID";
            this.txtOrigClOrdID.Size = new System.Drawing.Size(173, 20);
            this.txtOrigClOrdID.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "OrigClOrdID:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbtBovespa);
            this.groupBox1.Controls.Add(this.rdbtBMF);
            this.groupBox1.Location = new System.Drawing.Point(70, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(173, 32);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // btReloadResync
            // 
            this.btReloadResync.Location = new System.Drawing.Point(469, 263);
            this.btReloadResync.Name = "btReloadResync";
            this.btReloadResync.Size = new System.Drawing.Size(144, 23);
            this.btReloadResync.TabIndex = 22;
            this.btReloadResync.Text = "Reload  Resync";
            this.btReloadResync.UseVisualStyleBackColor = true;
            this.btReloadResync.Click += new System.EventHandler(this.btReloadResync_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(469, 87);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "Start Risk Client Sub";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(506, 196);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 24;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(506, 226);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 25;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(506, 151);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 26;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnConsolidatedRisk
            // 
            this.btnConsolidatedRisk.Location = new System.Drawing.Point(470, 292);
            this.btnConsolidatedRisk.Name = "btnConsolidatedRisk";
            this.btnConsolidatedRisk.Size = new System.Drawing.Size(144, 23);
            this.btnConsolidatedRisk.TabIndex = 27;
            this.btnConsolidatedRisk.Text = "Load Consolidated Risk";
            this.btnConsolidatedRisk.UseVisualStyleBackColor = true;
            this.btnConsolidatedRisk.Click += new System.EventHandler(this.btnConsolidatedRisk_Click);
            // 
            // btnPositionClient
            // 
            this.btnPositionClient.Location = new System.Drawing.Point(469, 321);
            this.btnPositionClient.Name = "btnPositionClient";
            this.btnPositionClient.Size = new System.Drawing.Size(144, 23);
            this.btnPositionClient.TabIndex = 28;
            this.btnPositionClient.Text = "Load Position Client";
            this.btnPositionClient.UseVisualStyleBackColor = true;
            this.btnPositionClient.Click += new System.EventHandler(this.btnPositionClient_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 447);
            this.Controls.Add(this.btnPositionClient);
            this.Controls.Add(this.btnConsolidatedRisk);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btReloadResync);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtOrigClOrdID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtOrderID);
            this.Controls.Add(this.rdBtVenda);
            this.Controls.Add(this.rdBtCompra);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtOrderQty);
            this.Controls.Add(this.txtSymbol);
            this.Controls.Add(this.txtAccount);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btValidNova);
            this.Controls.Add(this.btValidAlteracao);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.btStop);
            this.Name = "Form1";
            this.Text = "App Teste Risk Client";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btValidAlteracao;
        private System.Windows.Forms.Button btValidNova;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtAccount;
        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.TextBox txtOrderQty;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.RadioButton rdBtCompra;
        private System.Windows.Forms.RadioButton rdBtVenda;
        private System.Windows.Forms.RadioButton rdbtBovespa;
        private System.Windows.Forms.RadioButton rdbtBMF;
        private System.Windows.Forms.TextBox txtOrderID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOrigClOrdID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btReloadResync;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnConsolidatedRisk;
        private System.Windows.Forms.Button btnPositionClient;
    }
}


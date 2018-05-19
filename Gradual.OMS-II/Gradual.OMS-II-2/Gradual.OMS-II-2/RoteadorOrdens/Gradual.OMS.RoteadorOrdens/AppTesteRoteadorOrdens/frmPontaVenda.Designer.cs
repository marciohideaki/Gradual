namespace AppTesteRoteadorOrdens
{
    partial class frmPontaVenda
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.txtQtdeVenda = new System.Windows.Forms.TextBox();
            this.txtAccountVenda = new System.Windows.Forms.TextBox();
            this.txtClordIDVenda = new System.Windows.Forms.TextBox();
            this.txtInvestorIDVenda = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtInvestorIDVenda);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btCancel);
            this.groupBox1.Controls.Add(this.btOk);
            this.groupBox1.Controls.Add(this.txtQtdeVenda);
            this.groupBox1.Controls.Add(this.txtAccountVenda);
            this.groupBox1.Controls.Add(this.txtClordIDVenda);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 181);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dados da Ponta de Venda";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Qtde:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Account:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "ClOrdID:";
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(127, 140);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 24);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(17, 140);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 24);
            this.btOk.TabIndex = 3;
            this.btOk.Text = "Enviar";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // txtQtdeVenda
            // 
            this.txtQtdeVenda.Location = new System.Drawing.Point(89, 75);
            this.txtQtdeVenda.Name = "txtQtdeVenda";
            this.txtQtdeVenda.Size = new System.Drawing.Size(135, 20);
            this.txtQtdeVenda.TabIndex = 2;
            // 
            // txtAccountVenda
            // 
            this.txtAccountVenda.Location = new System.Drawing.Point(89, 48);
            this.txtAccountVenda.Name = "txtAccountVenda";
            this.txtAccountVenda.Size = new System.Drawing.Size(135, 20);
            this.txtAccountVenda.TabIndex = 1;
            // 
            // txtClordIDVenda
            // 
            this.txtClordIDVenda.Location = new System.Drawing.Point(89, 20);
            this.txtClordIDVenda.Name = "txtClordIDVenda";
            this.txtClordIDVenda.Size = new System.Drawing.Size(135, 20);
            this.txtClordIDVenda.TabIndex = 0;
            // 
            // txtInvestorIDVenda
            // 
            this.txtInvestorIDVenda.Location = new System.Drawing.Point(89, 102);
            this.txtInvestorIDVenda.Name = "txtInvestorIDVenda";
            this.txtInvestorIDVenda.Size = new System.Drawing.Size(135, 20);
            this.txtInvestorIDVenda.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "InvestorID:";
            // 
            // frmPontaVenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 200);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmPontaVenda";
            this.Text = "Ponta de Venda";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.TextBox txtQtdeVenda;
        private System.Windows.Forms.TextBox txtAccountVenda;
        private System.Windows.Forms.TextBox txtClordIDVenda;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtInvestorIDVenda;
    }
}
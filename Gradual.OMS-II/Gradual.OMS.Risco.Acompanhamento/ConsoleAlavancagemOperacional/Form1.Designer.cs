namespace ConsoleAlavancagemOperacional
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
            this.components = new System.ComponentModel.Container();
            this.stStatusSTM = new System.Windows.Forms.Label();
            this.txtCodigoCliente = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbVendaOpcoes = new System.Windows.Forms.CheckBox();
            this.cbCompraOpcoes = new System.Windows.Forms.CheckBox();
            this.cbVendaAVista = new System.Windows.Forms.CheckBox();
            this.cbCompraAVista = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblDataRecalculo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // stStatusSTM
            // 
            this.stStatusSTM.AutoSize = true;
            this.stStatusSTM.Location = new System.Drawing.Point(12, 138);
            this.stStatusSTM.Name = "stStatusSTM";
            this.stStatusSTM.Size = new System.Drawing.Size(0, 13);
            this.stStatusSTM.TabIndex = 18;
            // 
            // txtCodigoCliente
            // 
            this.txtCodigoCliente.Location = new System.Drawing.Point(15, 25);
            this.txtCodigoCliente.Name = "txtCodigoCliente";
            this.txtCodigoCliente.Size = new System.Drawing.Size(262, 20);
            this.txtCodigoCliente.TabIndex = 17;
            this.txtCodigoCliente.Text = "42089";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Código Cliente";
            // 
            // cbVendaOpcoes
            // 
            this.cbVendaOpcoes.AutoSize = true;
            this.cbVendaOpcoes.Checked = true;
            this.cbVendaOpcoes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbVendaOpcoes.Location = new System.Drawing.Point(181, 78);
            this.cbVendaOpcoes.Name = "cbVendaOpcoes";
            this.cbVendaOpcoes.Size = new System.Drawing.Size(97, 17);
            this.cbVendaOpcoes.TabIndex = 15;
            this.cbVendaOpcoes.Text = "Venda Opções";
            this.cbVendaOpcoes.UseVisualStyleBackColor = true;
            // 
            // cbCompraOpcoes
            // 
            this.cbCompraOpcoes.AutoSize = true;
            this.cbCompraOpcoes.Location = new System.Drawing.Point(181, 55);
            this.cbCompraOpcoes.Name = "cbCompraOpcoes";
            this.cbCompraOpcoes.Size = new System.Drawing.Size(102, 17);
            this.cbCompraOpcoes.TabIndex = 14;
            this.cbCompraOpcoes.Text = "Compra Opções";
            this.cbCompraOpcoes.UseVisualStyleBackColor = true;
            // 
            // cbVendaAVista
            // 
            this.cbVendaAVista.AutoSize = true;
            this.cbVendaAVista.Location = new System.Drawing.Point(16, 78);
            this.cbVendaAVista.Name = "cbVendaAVista";
            this.cbVendaAVista.Size = new System.Drawing.Size(91, 17);
            this.cbVendaAVista.TabIndex = 13;
            this.cbVendaAVista.Text = "Venda a vista";
            this.cbVendaAVista.UseVisualStyleBackColor = true;
            // 
            // cbCompraAVista
            // 
            this.cbCompraAVista.AutoSize = true;
            this.cbCompraAVista.Location = new System.Drawing.Point(16, 55);
            this.cbCompraAVista.Name = "cbCompraAVista";
            this.cbCompraAVista.Size = new System.Drawing.Size(96, 17);
            this.cbCompraAVista.TabIndex = 12;
            this.cbCompraAVista.Text = "Compra a vista";
            this.cbCompraAVista.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(272, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Forçar Re-calculo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 30000;
            // 
            // lblDataRecalculo
            // 
            this.lblDataRecalculo.AutoSize = true;
            this.lblDataRecalculo.Location = new System.Drawing.Point(11, 131);
            this.lblDataRecalculo.Name = "lblDataRecalculo";
            this.lblDataRecalculo.Size = new System.Drawing.Size(0, 13);
            this.lblDataRecalculo.TabIndex = 19;
            this.lblDataRecalculo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 177);
            this.Controls.Add(this.lblDataRecalculo);
            this.Controls.Add(this.stStatusSTM);
            this.Controls.Add(this.txtCodigoCliente);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbVendaOpcoes);
            this.Controls.Add(this.cbCompraOpcoes);
            this.Controls.Add(this.cbVendaAVista);
            this.Controls.Add(this.cbCompraAVista);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label stStatusSTM;
        private System.Windows.Forms.TextBox txtCodigoCliente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbVendaOpcoes;
        private System.Windows.Forms.CheckBox cbCompraOpcoes;
        private System.Windows.Forms.CheckBox cbVendaAVista;
        private System.Windows.Forms.CheckBox cbCompraAVista;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblDataRecalculo;
    }
}


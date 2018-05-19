namespace StopStartForms
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
            this.txtCodigoCliente = new System.Windows.Forms.TextBox();
            this.txtInstrumento = new System.Windows.Forms.TextBox();
            this.txtPreco = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDisparar = new System.Windows.Forms.Button();
            this.txtHistorico = new System.Windows.Forms.TextBox();
            this.btLimpar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIdStopStart = new System.Windows.Forms.TextBox();
            this.btCancelar = new System.Windows.Forms.Button();
            this.cbTipoOrdem = new System.Windows.Forms.ComboBox();
            this.txtPreco2 = new System.Windows.Forms.TextBox();
            this.lblInitialMivelPrice = new System.Windows.Forms.Label();
            this.txtInitialMovelPrice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAdjMovelPrice = new System.Windows.Forms.TextBox();
            this.btnSelecionar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtCodigoCliente
            // 
            this.txtCodigoCliente.Location = new System.Drawing.Point(110, 11);
            this.txtCodigoCliente.Name = "txtCodigoCliente";
            this.txtCodigoCliente.Size = new System.Drawing.Size(142, 20);
            this.txtCodigoCliente.TabIndex = 0;
            // 
            // txtInstrumento
            // 
            this.txtInstrumento.Location = new System.Drawing.Point(110, 37);
            this.txtInstrumento.Name = "txtInstrumento";
            this.txtInstrumento.Size = new System.Drawing.Size(142, 20);
            this.txtInstrumento.TabIndex = 1;
            // 
            // txtPreco
            // 
            this.txtPreco.Location = new System.Drawing.Point(110, 63);
            this.txtPreco.Name = "txtPreco";
            this.txtPreco.Size = new System.Drawing.Size(142, 20);
            this.txtPreco.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Código Cliente";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Instrumento";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(64, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Preço";
            // 
            // btnDisparar
            // 
            this.btnDisparar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisparar.Location = new System.Drawing.Point(401, 179);
            this.btnDisparar.Name = "btnDisparar";
            this.btnDisparar.Size = new System.Drawing.Size(237, 23);
            this.btnDisparar.TabIndex = 6;
            this.btnDisparar.Text = "Disparar Ordem Stop";
            this.btnDisparar.UseVisualStyleBackColor = true;
            this.btnDisparar.Click += new System.EventHandler(this.btnDisparar_Click);
            // 
            // txtHistorico
            // 
            this.txtHistorico.Location = new System.Drawing.Point(272, 11);
            this.txtHistorico.Multiline = true;
            this.txtHistorico.Name = "txtHistorico";
            this.txtHistorico.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHistorico.Size = new System.Drawing.Size(363, 125);
            this.txtHistorico.TabIndex = 7;
            // 
            // btLimpar
            // 
            this.btLimpar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLimpar.Location = new System.Drawing.Point(563, 146);
            this.btLimpar.Name = "btLimpar";
            this.btLimpar.Size = new System.Drawing.Size(75, 23);
            this.btLimpar.TabIndex = 8;
            this.btLimpar.Text = "Limpar";
            this.btLimpar.UseVisualStyleBackColor = true;
            this.btLimpar.Click += new System.EventHandler(this.btLimpar_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Tipo de ordem ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(271, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Id_StopStart";
            // 
            // txtIdStopStart
            // 
            this.txtIdStopStart.Location = new System.Drawing.Point(353, 147);
            this.txtIdStopStart.Name = "txtIdStopStart";
            this.txtIdStopStart.Size = new System.Drawing.Size(51, 20);
            this.txtIdStopStart.TabIndex = 12;
            // 
            // btCancelar
            // 
            this.btCancelar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCancelar.Location = new System.Drawing.Point(411, 146);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(144, 23);
            this.btCancelar.TabIndex = 13;
            this.btCancelar.Text = "Enviar Cancelamento";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // cbTipoOrdem
            // 
            this.cbTipoOrdem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTipoOrdem.FormattingEnabled = true;
            this.cbTipoOrdem.Location = new System.Drawing.Point(110, 115);
            this.cbTipoOrdem.Name = "cbTipoOrdem";
            this.cbTipoOrdem.Size = new System.Drawing.Size(142, 21);
            this.cbTipoOrdem.TabIndex = 14;
            // 
            // txtPreco2
            // 
            this.txtPreco2.Location = new System.Drawing.Point(110, 89);
            this.txtPreco2.Name = "txtPreco2";
            this.txtPreco2.Size = new System.Drawing.Size(142, 20);
            this.txtPreco2.TabIndex = 15;
            // 
            // lblInitialMivelPrice
            // 
            this.lblInitialMivelPrice.AutoSize = true;
            this.lblInitialMivelPrice.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInitialMivelPrice.Location = new System.Drawing.Point(2, 150);
            this.lblInitialMivelPrice.Name = "lblInitialMivelPrice";
            this.lblInitialMivelPrice.Size = new System.Drawing.Size(108, 13);
            this.lblInitialMivelPrice.TabIndex = 16;
            this.lblInitialMivelPrice.Text = "Initial Movel Price";
            // 
            // txtInitialMovelPrice
            // 
            this.txtInitialMovelPrice.Location = new System.Drawing.Point(110, 148);
            this.txtInitialMovelPrice.Name = "txtInitialMovelPrice";
            this.txtInitialMovelPrice.Size = new System.Drawing.Size(142, 20);
            this.txtInitialMovelPrice.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(11, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Adj. Movel Price";
            // 
            // txtAdjMovelPrice
            // 
            this.txtAdjMovelPrice.Location = new System.Drawing.Point(110, 175);
            this.txtAdjMovelPrice.Name = "txtAdjMovelPrice";
            this.txtAdjMovelPrice.Size = new System.Drawing.Size(142, 20);
            this.txtAdjMovelPrice.TabIndex = 19;
            // 
            // btnSelecionar
            // 
            this.btnSelecionar.Location = new System.Drawing.Point(401, 208);
            this.btnSelecionar.Name = "btnSelecionar";
            this.btnSelecionar.Size = new System.Drawing.Size(234, 23);
            this.btnSelecionar.TabIndex = 20;
            this.btnSelecionar.Text = "Selecionar Ordem";
            this.btnSelecionar.UseVisualStyleBackColor = true;
            this.btnSelecionar.Click += new System.EventHandler(this.btnSelecionar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 233);
            this.Controls.Add(this.btnSelecionar);
            this.Controls.Add(this.txtAdjMovelPrice);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtInitialMovelPrice);
            this.Controls.Add(this.lblInitialMivelPrice);
            this.Controls.Add(this.txtPreco2);
            this.Controls.Add(this.cbTipoOrdem);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.txtIdStopStart);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btLimpar);
            this.Controls.Add(this.txtHistorico);
            this.Controls.Add(this.btnDisparar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPreco);
            this.Controls.Add(this.txtInstrumento);
            this.Controls.Add(this.txtCodigoCliente);
            this.Name = "Form1";
            this.Text = "Ordens";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCodigoCliente;
        private System.Windows.Forms.TextBox txtInstrumento;
        private System.Windows.Forms.TextBox txtPreco;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDisparar;
        private System.Windows.Forms.TextBox txtHistorico;
        private System.Windows.Forms.Button btLimpar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtIdStopStart;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.ComboBox cbTipoOrdem;
        private System.Windows.Forms.TextBox txtPreco2;
        private System.Windows.Forms.Label lblInitialMivelPrice;
        private System.Windows.Forms.TextBox txtInitialMovelPrice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAdjMovelPrice;
        private System.Windows.Forms.Button btnSelecionar;
    }
}


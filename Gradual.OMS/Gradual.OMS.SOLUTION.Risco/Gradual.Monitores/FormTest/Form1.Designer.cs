namespace FormTest
{
    partial class frmMonitor
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
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgResumo = new System.Windows.Forms.DataGridView();
            this.dwOperacoes = new System.Windows.Forms.DataGridView();
            this.dwMonitor = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dtatualizacao = new System.Windows.Forms.Label();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.dwBmf = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResumo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwOperacoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwMonitor)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwBmf)).BeginInit();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 22);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Cliente:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dwBmf);
            this.groupBox1.Controls.Add(this.dgResumo);
            this.groupBox1.Controls.Add(this.dwOperacoes);
            this.groupBox1.Controls.Add(this.dwMonitor);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(8, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBox1.Size = new System.Drawing.Size(1575, 834);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // dgResumo
            // 
            this.dgResumo.AllowUserToAddRows = false;
            this.dgResumo.AllowUserToDeleteRows = false;
            this.dgResumo.AllowUserToOrderColumns = true;
            this.dgResumo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgResumo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResumo.Location = new System.Drawing.Point(7, 243);
            this.dgResumo.Name = "dgResumo";
            this.dgResumo.Size = new System.Drawing.Size(1560, 167);
            this.dgResumo.TabIndex = 5;
            // 
            // dwOperacoes
            // 
            this.dwOperacoes.AllowUserToAddRows = false;
            this.dwOperacoes.AllowUserToDeleteRows = false;
            this.dwOperacoes.AllowUserToOrderColumns = true;
            this.dwOperacoes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dwOperacoes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dwOperacoes.Location = new System.Drawing.Point(0, 416);
            this.dwOperacoes.Name = "dwOperacoes";
            this.dwOperacoes.Size = new System.Drawing.Size(1566, 201);
            this.dwOperacoes.TabIndex = 4;
            // 
            // dwMonitor
            // 
            this.dwMonitor.AllowUserToAddRows = false;
            this.dwMonitor.AllowUserToDeleteRows = false;
            this.dwMonitor.AllowUserToOrderColumns = true;
            this.dwMonitor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dwMonitor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dwMonitor.Location = new System.Drawing.Point(7, 76);
            this.dwMonitor.Name = "dwMonitor";
            this.dwMonitor.Size = new System.Drawing.Size(1560, 154);
            this.dwMonitor.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.dtatualizacao);
            this.groupBox2.Controls.Add(this.txtCliente);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox2.Location = new System.Drawing.Point(7, 11);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBox2.Size = new System.Drawing.Size(1560, 58);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(226, 18);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 22);
            this.button1.TabIndex = 27;
            this.button1.Text = "Forçar Recalculo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(119, 18);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 22);
            this.button2.TabIndex = 26;
            this.button2.Text = "Acionar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dtatualizacao
            // 
            this.dtatualizacao.AutoSize = true;
            this.dtatualizacao.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtatualizacao.Location = new System.Drawing.Point(380, 22);
            this.dtatualizacao.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dtatualizacao.Name = "dtatualizacao";
            this.dtatualizacao.Size = new System.Drawing.Size(150, 13);
            this.dtatualizacao.TabIndex = 21;
            this.dtatualizacao.Text = "Data ultima atualização: ";
            // 
            // txtCliente
            // 
            this.txtCliente.Location = new System.Drawing.Point(57, 20);
            this.txtCliente.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.Size = new System.Drawing.Size(58, 20);
            this.txtCliente.TabIndex = 4;
            // 
            // dwBmf
            // 
            this.dwBmf.AllowUserToAddRows = false;
            this.dwBmf.AllowUserToDeleteRows = false;
            this.dwBmf.AllowUserToOrderColumns = true;
            this.dwBmf.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dwBmf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dwBmf.Location = new System.Drawing.Point(0, 623);
            this.dwBmf.Name = "dwBmf";
            this.dwBmf.Size = new System.Drawing.Size(1566, 201);
            this.dwBmf.TabIndex = 6;
            // 
            // frmMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1586, 836);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Name = "frmMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gradual Investimentos -  [ Monitor de exposição de risco operacional ]";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgResumo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwOperacoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwMonitor)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwBmf)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label dtatualizacao;
        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dwMonitor;
        private System.Windows.Forms.DataGridView dwOperacoes;
        private System.Windows.Forms.DataGridView dgResumo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dwBmf;
    }
}


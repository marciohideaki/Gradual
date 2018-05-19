namespace Gradual.Transferencias.Risco.View
{
    partial class frmTransfer
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
            this.tabControlTransfer = new System.Windows.Forms.TabControl();
            this.tabITRA = new System.Windows.Forms.TabPage();
            this.btnCreateITRA = new System.Windows.Forms.Button();
            this.txtSequencialITRA = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpeFileITRA = new System.Windows.Forms.Button();
            this.txtFileNameITRA = new System.Windows.Forms.TextBox();
            this.tabITOP = new System.Windows.Forms.TabPage();
            this.rdoITOPDestino = new System.Windows.Forms.RadioButton();
            this.rdoITOPOrigem = new System.Windows.Forms.RadioButton();
            this.btnCreateITOP = new System.Windows.Forms.Button();
            this.txtSequencialITOP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOpenITOP = new System.Windows.Forms.Button();
            this.txtFileNameITOP = new System.Windows.Forms.TextBox();
            this.tabITRE = new System.Windows.Forms.TabPage();
            this.btnCreateITRE = new System.Windows.Forms.Button();
            this.txtSequencialITRE = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpenITRE = new System.Windows.Forms.Button();
            this.txtFileNameITRE = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControlTransfer.SuspendLayout();
            this.tabITRA.SuspendLayout();
            this.tabITOP.SuspendLayout();
            this.tabITRE.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlTransfer
            // 
            this.tabControlTransfer.Controls.Add(this.tabITRA);
            this.tabControlTransfer.Controls.Add(this.tabITOP);
            this.tabControlTransfer.Controls.Add(this.tabITRE);
            this.tabControlTransfer.Location = new System.Drawing.Point(1, 1);
            this.tabControlTransfer.Name = "tabControlTransfer";
            this.tabControlTransfer.SelectedIndex = 0;
            this.tabControlTransfer.Size = new System.Drawing.Size(407, 167);
            this.tabControlTransfer.TabIndex = 0;
            // 
            // tabITRA
            // 
            this.tabITRA.Controls.Add(this.btnCreateITRA);
            this.tabITRA.Controls.Add(this.txtSequencialITRA);
            this.tabITRA.Controls.Add(this.label1);
            this.tabITRA.Controls.Add(this.btnOpeFileITRA);
            this.tabITRA.Controls.Add(this.txtFileNameITRA);
            this.tabITRA.Location = new System.Drawing.Point(4, 22);
            this.tabITRA.Name = "tabITRA";
            this.tabITRA.Padding = new System.Windows.Forms.Padding(3);
            this.tabITRA.Size = new System.Drawing.Size(399, 141);
            this.tabITRA.TabIndex = 0;
            this.tabITRA.Text = "ITRA";
            this.tabITRA.UseVisualStyleBackColor = true;
            // 
            // btnCreateITRA
            // 
            this.btnCreateITRA.Location = new System.Drawing.Point(304, 112);
            this.btnCreateITRA.Name = "btnCreateITRA";
            this.btnCreateITRA.Size = new System.Drawing.Size(75, 23);
            this.btnCreateITRA.TabIndex = 8;
            this.btnCreateITRA.Text = "Create ITRA";
            this.btnCreateITRA.UseVisualStyleBackColor = true;
            this.btnCreateITRA.Click += new System.EventHandler(this.btnCreateITRA_Click);
            // 
            // txtSequencialITRA
            // 
            this.txtSequencialITRA.Location = new System.Drawing.Point(124, 19);
            this.txtSequencialITRA.Name = "txtSequencialITRA";
            this.txtSequencialITRA.Size = new System.Drawing.Size(100, 20);
            this.txtSequencialITRA.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Número Sequencial:";
            // 
            // btnOpeFileITRA
            // 
            this.btnOpeFileITRA.Location = new System.Drawing.Point(223, 112);
            this.btnOpeFileITRA.Name = "btnOpeFileITRA";
            this.btnOpeFileITRA.Size = new System.Drawing.Size(75, 23);
            this.btnOpeFileITRA.TabIndex = 1;
            this.btnOpeFileITRA.Text = "Open ITRA";
            this.btnOpeFileITRA.UseVisualStyleBackColor = true;
            this.btnOpeFileITRA.Click += new System.EventHandler(this.btnOpeFileITRA_Click);
            // 
            // txtFileNameITRA
            // 
            this.txtFileNameITRA.Location = new System.Drawing.Point(7, 112);
            this.txtFileNameITRA.Name = "txtFileNameITRA";
            this.txtFileNameITRA.Size = new System.Drawing.Size(209, 20);
            this.txtFileNameITRA.TabIndex = 0;
            // 
            // tabITOP
            // 
            this.tabITOP.Controls.Add(this.rdoITOPDestino);
            this.tabITOP.Controls.Add(this.rdoITOPOrigem);
            this.tabITOP.Controls.Add(this.btnCreateITOP);
            this.tabITOP.Controls.Add(this.txtSequencialITOP);
            this.tabITOP.Controls.Add(this.label2);
            this.tabITOP.Controls.Add(this.btnOpenITOP);
            this.tabITOP.Controls.Add(this.txtFileNameITOP);
            this.tabITOP.Location = new System.Drawing.Point(4, 22);
            this.tabITOP.Name = "tabITOP";
            this.tabITOP.Padding = new System.Windows.Forms.Padding(3);
            this.tabITOP.Size = new System.Drawing.Size(399, 141);
            this.tabITOP.TabIndex = 1;
            this.tabITOP.Text = "ITOP";
            this.tabITOP.UseVisualStyleBackColor = true;
            // 
            // rdoITOPDestino
            // 
            this.rdoITOPDestino.AutoSize = true;
            this.rdoITOPDestino.Location = new System.Drawing.Point(304, 42);
            this.rdoITOPDestino.Name = "rdoITOPDestino";
            this.rdoITOPDestino.Size = new System.Drawing.Size(61, 17);
            this.rdoITOPDestino.TabIndex = 10;
            this.rdoITOPDestino.Text = "Destino";
            this.rdoITOPDestino.UseVisualStyleBackColor = true;
            // 
            // rdoITOPOrigem
            // 
            this.rdoITOPOrigem.AutoSize = true;
            this.rdoITOPOrigem.Checked = true;
            this.rdoITOPOrigem.Location = new System.Drawing.Point(304, 19);
            this.rdoITOPOrigem.Name = "rdoITOPOrigem";
            this.rdoITOPOrigem.Size = new System.Drawing.Size(58, 17);
            this.rdoITOPOrigem.TabIndex = 9;
            this.rdoITOPOrigem.TabStop = true;
            this.rdoITOPOrigem.Text = "Origem";
            this.rdoITOPOrigem.UseVisualStyleBackColor = true;
            // 
            // btnCreateITOP
            // 
            this.btnCreateITOP.Location = new System.Drawing.Point(304, 112);
            this.btnCreateITOP.Name = "btnCreateITOP";
            this.btnCreateITOP.Size = new System.Drawing.Size(75, 23);
            this.btnCreateITOP.TabIndex = 8;
            this.btnCreateITOP.Text = "Create ITOP";
            this.btnCreateITOP.UseVisualStyleBackColor = true;
            this.btnCreateITOP.Click += new System.EventHandler(this.btnCreateITOP_Click);
            // 
            // txtSequencialITOP
            // 
            this.txtSequencialITOP.Location = new System.Drawing.Point(124, 19);
            this.txtSequencialITOP.Name = "txtSequencialITOP";
            this.txtSequencialITOP.Size = new System.Drawing.Size(100, 20);
            this.txtSequencialITOP.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Número Sequencial:";
            // 
            // btnOpenITOP
            // 
            this.btnOpenITOP.Location = new System.Drawing.Point(223, 112);
            this.btnOpenITOP.Name = "btnOpenITOP";
            this.btnOpenITOP.Size = new System.Drawing.Size(75, 23);
            this.btnOpenITOP.TabIndex = 3;
            this.btnOpenITOP.Text = "Open ITOP";
            this.btnOpenITOP.UseVisualStyleBackColor = true;
            this.btnOpenITOP.Click += new System.EventHandler(this.btnOpenITOP_Click);
            // 
            // txtFileNameITOP
            // 
            this.txtFileNameITOP.Location = new System.Drawing.Point(7, 112);
            this.txtFileNameITOP.Name = "txtFileNameITOP";
            this.txtFileNameITOP.Size = new System.Drawing.Size(209, 20);
            this.txtFileNameITOP.TabIndex = 2;
            // 
            // tabITRE
            // 
            this.tabITRE.Controls.Add(this.btnCreateITRE);
            this.tabITRE.Controls.Add(this.txtSequencialITRE);
            this.tabITRE.Controls.Add(this.label3);
            this.tabITRE.Controls.Add(this.btnOpenITRE);
            this.tabITRE.Controls.Add(this.txtFileNameITRE);
            this.tabITRE.Location = new System.Drawing.Point(4, 22);
            this.tabITRE.Name = "tabITRE";
            this.tabITRE.Size = new System.Drawing.Size(399, 141);
            this.tabITRE.TabIndex = 2;
            this.tabITRE.Text = "ITRE";
            this.tabITRE.UseVisualStyleBackColor = true;
            // 
            // btnCreateITRE
            // 
            this.btnCreateITRE.Location = new System.Drawing.Point(304, 112);
            this.btnCreateITRE.Name = "btnCreateITRE";
            this.btnCreateITRE.Size = new System.Drawing.Size(75, 23);
            this.btnCreateITRE.TabIndex = 7;
            this.btnCreateITRE.Text = "Create ITRE";
            this.btnCreateITRE.UseVisualStyleBackColor = true;
            this.btnCreateITRE.Click += new System.EventHandler(this.btnCreateITRE_Click);
            // 
            // txtSequencialITRE
            // 
            this.txtSequencialITRE.Location = new System.Drawing.Point(124, 19);
            this.txtSequencialITRE.Name = "txtSequencialITRE";
            this.txtSequencialITRE.Size = new System.Drawing.Size(100, 20);
            this.txtSequencialITRE.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Número Sequencial:";
            // 
            // btnOpenITRE
            // 
            this.btnOpenITRE.Location = new System.Drawing.Point(223, 112);
            this.btnOpenITRE.Name = "btnOpenITRE";
            this.btnOpenITRE.Size = new System.Drawing.Size(75, 23);
            this.btnOpenITRE.TabIndex = 3;
            this.btnOpenITRE.Text = "Open ITRE";
            this.btnOpenITRE.UseVisualStyleBackColor = true;
            this.btnOpenITRE.Click += new System.EventHandler(this.btnOpenITRE_Click);
            // 
            // txtFileNameITRE
            // 
            this.txtFileNameITRE.Location = new System.Drawing.Point(7, 112);
            this.txtFileNameITRE.Name = "txtFileNameITRE";
            this.txtFileNameITRE.Size = new System.Drawing.Size(209, 20);
            this.txtFileNameITRE.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialogTransfer";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 167);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(408, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // frmTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 189);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControlTransfer);
            this.Name = "frmTransfer";
            this.Text = "Montagem de layout de arquivos de transferências";
            this.tabControlTransfer.ResumeLayout(false);
            this.tabITRA.ResumeLayout(false);
            this.tabITRA.PerformLayout();
            this.tabITOP.ResumeLayout(false);
            this.tabITOP.PerformLayout();
            this.tabITRE.ResumeLayout(false);
            this.tabITRE.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlTransfer;
        private System.Windows.Forms.TabPage tabITRA;
        private System.Windows.Forms.TabPage tabITOP;
        private System.Windows.Forms.TabPage tabITRE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpeFileITRA;
        private System.Windows.Forms.TextBox txtFileNameITRA;
        private System.Windows.Forms.Button btnOpenITOP;
        private System.Windows.Forms.TextBox txtFileNameITOP;
        private System.Windows.Forms.Button btnOpenITRE;
        private System.Windows.Forms.TextBox txtFileNameITRE;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSequencialITRA;
        private System.Windows.Forms.TextBox txtSequencialITOP;
        private System.Windows.Forms.Button btnCreateITRE;
        private System.Windows.Forms.TextBox txtSequencialITRE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCreateITRA;
        private System.Windows.Forms.Button btnCreateITOP;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.RadioButton rdoITOPDestino;
        private System.Windows.Forms.RadioButton rdoITOPOrigem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
    }
}


namespace TesteDeSeguranca
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnListarPerfis = new System.Windows.Forms.Button();
            this.lblCodigoSessao = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnListarUsuarios = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gvResultados = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnNovoPerfil = new System.Windows.Forms.Button();
            this.lblPerfil = new System.Windows.Forms.Label();
            this.txtNovoPerfil = new System.Windows.Forms.TextBox();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button6 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvResultados)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(892, 477);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listBox1);
            this.tabPage1.Controls.Add(this.propertyGrid1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.gvResultados);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(884, 451);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(269, 106);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(358, 342);
            this.listBox1.TabIndex = 9;
            this.listBox1.Click += new System.EventHandler(this.listBox1_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(633, 106);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(237, 344);
            this.propertyGrid1.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(878, 102);
            this.panel1.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.btnListarPerfis);
            this.groupBox2.Controls.Add(this.lblCodigoSessao);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnListarUsuarios);
            this.groupBox2.Location = new System.Drawing.Point(386, -1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 97);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ações";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(185, 59);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(146, 23);
            this.button5.TabIndex = 6;
            this.button5.Text = "Assessor compartilhado";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(104, 59);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Listar Perfis";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 59);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Salvar Usuario";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnListarPerfis
            // 
            this.btnListarPerfis.Location = new System.Drawing.Point(104, 34);
            this.btnListarPerfis.Name = "btnListarPerfis";
            this.btnListarPerfis.Size = new System.Drawing.Size(228, 23);
            this.btnListarPerfis.TabIndex = 3;
            this.btnListarPerfis.Text = "Listar Usuarios Assessores";
            this.btnListarPerfis.UseVisualStyleBackColor = true;
            this.btnListarPerfis.Click += new System.EventHandler(this.btnListarPerfis_Click);
            // 
            // lblCodigoSessao
            // 
            this.lblCodigoSessao.AutoSize = true;
            this.lblCodigoSessao.Location = new System.Drawing.Point(107, 16);
            this.lblCodigoSessao.Name = "lblCodigoSessao";
            this.lblCodigoSessao.Size = new System.Drawing.Size(0, 13);
            this.lblCodigoSessao.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Codigo da sessão:";
            // 
            // btnListarUsuarios
            // 
            this.btnListarUsuarios.Location = new System.Drawing.Point(9, 34);
            this.btnListarUsuarios.Name = "btnListarUsuarios";
            this.btnListarUsuarios.Size = new System.Drawing.Size(91, 23);
            this.btnListarUsuarios.TabIndex = 0;
            this.btnListarUsuarios.Text = "Listar Usuários";
            this.btnListarUsuarios.UseVisualStyleBackColor = true;
            this.btnListarUsuarios.Click += new System.EventHandler(this.btnListarUsuarios_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtSenha);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtEmail);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logon";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(297, 49);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Log Out";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(216, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Log In";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(56, 52);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(154, 20);
            this.txtSenha.TabIndex = 3;
            this.txtSenha.Text = "rafa!1985!";
            this.txtSenha.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Senha";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(56, 26);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(316, 20);
            this.txtEmail.TabIndex = 1;
            this.txtEmail.Text = "Admin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Login";
            // 
            // gvResultados
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvResultados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvResultados.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvResultados.Location = new System.Drawing.Point(2, 106);
            this.gvResultados.Name = "gvResultados";
            this.gvResultados.Size = new System.Drawing.Size(261, 341);
            this.gvResultados.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnNovoPerfil);
            this.tabPage2.Controls.Add(this.lblPerfil);
            this.tabPage2.Controls.Add(this.txtNovoPerfil);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(884, 451);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnNovoPerfil
            // 
            this.btnNovoPerfil.Location = new System.Drawing.Point(364, 58);
            this.btnNovoPerfil.Name = "btnNovoPerfil";
            this.btnNovoPerfil.Size = new System.Drawing.Size(75, 23);
            this.btnNovoPerfil.TabIndex = 2;
            this.btnNovoPerfil.Text = "NovoPerfil";
            this.btnNovoPerfil.UseVisualStyleBackColor = true;
            this.btnNovoPerfil.Click += new System.EventHandler(this.btnNovoPerfil_Click);
            // 
            // lblPerfil
            // 
            this.lblPerfil.AutoSize = true;
            this.lblPerfil.Location = new System.Drawing.Point(9, 63);
            this.lblPerfil.Name = "lblPerfil";
            this.lblPerfil.Size = new System.Drawing.Size(30, 13);
            this.lblPerfil.TabIndex = 1;
            this.lblPerfil.Text = "Perfil";
            // 
            // txtNovoPerfil
            // 
            this.txtNovoPerfil.Location = new System.Drawing.Point(45, 60);
            this.txtNovoPerfil.Name = "txtNovoPerfil";
            this.txtNovoPerfil.Size = new System.Drawing.Size(313, 20);
            this.txtNovoPerfil.TabIndex = 0;
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(337, 59);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(81, 23);
            this.button6.TabIndex = 7;
            this.button6.Text = "Desbloquear ";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 477);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvResultados)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnListarPerfis;
        private System.Windows.Forms.Label lblCodigoSessao;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnListarUsuarios;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gvResultados;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnNovoPerfil;
        private System.Windows.Forms.Label lblPerfil;
        private System.Windows.Forms.TextBox txtNovoPerfil;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;

    }
}


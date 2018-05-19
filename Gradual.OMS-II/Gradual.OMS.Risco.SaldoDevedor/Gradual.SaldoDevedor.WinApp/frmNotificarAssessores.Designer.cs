namespace Gradual.SaldoDevedor.WinApp
{
    partial class frmNotificarAssessores
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
            this.treeEmail = new System.Windows.Forms.TreeView();
            this.pnlNotificarAssessores = new System.Windows.Forms.Panel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnEnviarEmail = new System.Windows.Forms.Button();
            this.pnlNotificarAssessores.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeEmail
            // 
            this.treeEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.treeEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeEmail.CheckBoxes = true;
            this.treeEmail.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.treeEmail.ItemHeight = 20;
            this.treeEmail.LabelEdit = true;
            this.treeEmail.Location = new System.Drawing.Point(6, 5);
            this.treeEmail.Name = "treeEmail";
            this.treeEmail.Size = new System.Drawing.Size(465, 409);
            this.treeEmail.TabIndex = 0;
            this.treeEmail.Tag = "SemRenderizacao";
            this.treeEmail.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeEmail_AfterLabelEdit);
            this.treeEmail.Click += new System.EventHandler(this.treeEmail_Click);
            this.treeEmail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeEmail_MouseDown);
            // 
            // pnlNotificarAssessores
            // 
            this.pnlNotificarAssessores.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlNotificarAssessores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.pnlNotificarAssessores.Controls.Add(this.btnCancelar);
            this.pnlNotificarAssessores.Controls.Add(this.btnEnviarEmail);
            this.pnlNotificarAssessores.Controls.Add(this.treeEmail);
            this.pnlNotificarAssessores.Location = new System.Drawing.Point(3, 34);
            this.pnlNotificarAssessores.Name = "pnlNotificarAssessores";
            this.pnlNotificarAssessores.Size = new System.Drawing.Size(476, 460);
            this.pnlNotificarAssessores.TabIndex = 1;
            // 
            // btnCancelar
            // 
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCancelar.FlatAppearance.BorderSize = 2;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(352, 420);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(119, 35);
            this.btnCancelar.TabIndex = 18;
            this.btnCancelar.Tag = "SemRenderizacao";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnEnviarEmail
            // 
            this.btnEnviarEmail.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnEnviarEmail.FlatAppearance.BorderSize = 2;
            this.btnEnviarEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarEmail.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviarEmail.ForeColor = System.Drawing.Color.White;
            this.btnEnviarEmail.Location = new System.Drawing.Point(6, 420);
            this.btnEnviarEmail.Name = "btnEnviarEmail";
            this.btnEnviarEmail.Size = new System.Drawing.Size(190, 35);
            this.btnEnviarEmail.TabIndex = 18;
            this.btnEnviarEmail.Tag = "SemRenderizacao";
            this.btnEnviarEmail.Text = "Enviar Notificações";
            this.btnEnviarEmail.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEnviarEmail.UseVisualStyleBackColor = false;
            this.btnEnviarEmail.Click += new System.EventHandler(this.btnEnviarEmail_Click);
            // 
            // frmNotificarAssessores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 524);
            this.Controls.Add(this.pnlNotificarAssessores);
            this.Name = "frmNotificarAssessores";
            this.Text = "..:: Notificar Assessores ::..";
            this.pnlNotificarAssessores.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeEmail;
        private System.Windows.Forms.Panel pnlNotificarAssessores;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnEnviarEmail;
    }
}
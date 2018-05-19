namespace Gradual.OMS.Host.Windows.Teste
{
    partial class ControleMonitorEventos
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grdMensagens = new System.Windows.Forms.DataGridView();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdMensagens)).BeginInit();
            this.SuspendLayout();
            // 
            // grdMensagens
            // 
            this.grdMensagens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMensagens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMensagens.Location = new System.Drawing.Point(0, 0);
            this.grdMensagens.Name = "grdMensagens";
            this.grdMensagens.ReadOnly = true;
            this.grdMensagens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdMensagens.Size = new System.Drawing.Size(548, 318);
            this.grdMensagens.TabIndex = 1;
            // 
            // ControleMonitorEventos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdMensagens);
            this.Name = "ControleMonitorEventos";
            this.Size = new System.Drawing.Size(548, 318);
            ((System.ComponentModel.ISupportInitialize)(this.grdMensagens)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdMensagens;
        private System.Windows.Forms.Timer timer;
    }
}

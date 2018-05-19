namespace Gradual.OMS.Host.Windows.Teste
{
    partial class frmObjeto
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
            this.ppg = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // ppg
            // 
            this.ppg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppg.Location = new System.Drawing.Point(0, 0);
            this.ppg.Name = "ppg";
            this.ppg.Size = new System.Drawing.Size(694, 388);
            this.ppg.TabIndex = 0;
            // 
            // frmObjeto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 388);
            this.Controls.Add(this.ppg);
            this.Name = "frmObjeto";
            this.Text = "Detalhe de Objeto";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid ppg;
    }
}
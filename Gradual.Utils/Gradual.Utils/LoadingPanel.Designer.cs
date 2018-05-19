namespace Gradual.Utils
{
    partial class LoadingPanel
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
            this.lblText = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.picLoading = new System.Windows.Forms.PictureBox();
            this.lblLoadingSubtitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(3, 25);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(57, 13);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Text Label";
            this.lblText.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 61);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.TabIndex = 2;
            this.progressBar.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.Color.Transparent;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.Color.Black;
            this.lblProgress.Location = new System.Drawing.Point(3, 45);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(91, 13);
            this.lblProgress.TabIndex = 3;
            this.lblProgress.Text = "Progress Label";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgress.Visible = false;
            // 
            // picLoading
            // 
            this.picLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picLoading.BackColor = System.Drawing.Color.Transparent;
            this.picLoading.Image = global::Gradual.Utils.Properties.Resources.loader;
            this.picLoading.Location = new System.Drawing.Point(626, 0);
            this.picLoading.Name = "picLoading";
            this.picLoading.Size = new System.Drawing.Size(32, 32);
            this.picLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLoading.TabIndex = 0;
            this.picLoading.TabStop = false;
            // 
            // lblLoadingSubtitle
            // 
            this.lblLoadingSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoadingSubtitle.AutoSize = true;
            this.lblLoadingSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLoadingSubtitle.ForeColor = System.Drawing.Color.Black;
            this.lblLoadingSubtitle.Location = new System.Drawing.Point(3, 12);
            this.lblLoadingSubtitle.Name = "lblLoadingSubtitle";
            this.lblLoadingSubtitle.Size = new System.Drawing.Size(117, 13);
            this.lblLoadingSubtitle.TabIndex = 4;
            this.lblLoadingSubtitle.Text = "Ordem(ns) carregada(s)";
            this.lblLoadingSubtitle.Visible = false;
            // 
            // LoadingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblLoadingSubtitle);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.picLoading);
            this.Name = "LoadingPanel";
            this.Size = new System.Drawing.Size(661, 124);
            this.Load += new System.EventHandler(this.ModalLoadingUI_Load);
            this.VisibleChanged += new System.EventHandler(this.ModalLoadingUI_VisibleChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmBase_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLoading;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblLoadingSubtitle;
    }
}

namespace Gradual.OMS.Host.Windows.Teste
{
    partial class ControleEnvioMensagens
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
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lstMensagem = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmdEnviarMensagem = new System.Windows.Forms.Button();
            this.ppg = new System.Windows.Forms.PropertyGrid();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(385, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 435);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lstMensagem);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(385, 435);
            this.panel3.TabIndex = 2;
            // 
            // lstMensagem
            // 
            this.lstMensagem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMensagem.FormattingEnabled = true;
            this.lstMensagem.Location = new System.Drawing.Point(0, 0);
            this.lstMensagem.Name = "lstMensagem";
            this.lstMensagem.Size = new System.Drawing.Size(385, 381);
            this.lstMensagem.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cmdEnviarMensagem);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 392);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(385, 43);
            this.panel4.TabIndex = 0;
            // 
            // cmdEnviarMensagem
            // 
            this.cmdEnviarMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdEnviarMensagem.Location = new System.Drawing.Point(5, 3);
            this.cmdEnviarMensagem.Name = "cmdEnviarMensagem";
            this.cmdEnviarMensagem.Size = new System.Drawing.Size(374, 27);
            this.cmdEnviarMensagem.TabIndex = 0;
            this.cmdEnviarMensagem.Text = "Enviar Mensagem";
            this.cmdEnviarMensagem.UseVisualStyleBackColor = true;
            // 
            // ppg
            // 
            this.ppg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppg.Location = new System.Drawing.Point(388, 0);
            this.ppg.Name = "ppg";
            this.ppg.Size = new System.Drawing.Size(518, 435);
            this.ppg.TabIndex = 5;
            // 
            // ControleEnvioMensagens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ppg);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel3);
            this.Name = "ControleEnvioMensagens";
            this.Size = new System.Drawing.Size(906, 435);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListBox lstMensagem;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button cmdEnviarMensagem;
        private System.Windows.Forms.PropertyGrid ppg;
    }
}

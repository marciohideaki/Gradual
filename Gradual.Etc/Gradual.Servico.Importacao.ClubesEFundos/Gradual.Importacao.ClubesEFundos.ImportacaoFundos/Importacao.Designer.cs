namespace Gradual.MinhaConta.Servico.ImportacaoFundos
{
    partial class ImportacaoClubesEFundos
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
            this.Timer1 = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.Timer1)).BeginInit();
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            // 
            // ImportacaoClubesEFundos
            // 
            this.ServiceName = "GradualImportacaoFundos";
            ((System.ComponentModel.ISupportInitialize)(this.Timer1)).EndInit();

        }

        #endregion

        private System.Timers.Timer Timer1;
    }
}

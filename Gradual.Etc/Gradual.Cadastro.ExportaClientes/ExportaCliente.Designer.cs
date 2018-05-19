using System.Timers;
namespace Gradual.Cadastro.ExportaClientes
{
    partial class ExportaCliente
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
            this.dtpHora = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.dtpHora)).BeginInit();
            // 
            // dtpHora
            // 
            this.dtpHora.Enabled = true;
            this.dtpHora.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimedEvent);
            // 
            // ExportaCliente
            // 
            this.ServiceName = "Gradual.Cadastro.ExportaCliente";
            ((System.ComponentModel.ISupportInitialize)(this.dtpHora)).EndInit();

        }

        #endregion

    }
}

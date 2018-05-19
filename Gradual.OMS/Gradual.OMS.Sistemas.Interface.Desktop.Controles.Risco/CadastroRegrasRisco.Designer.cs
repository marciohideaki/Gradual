namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    partial class CadastroRegrasRisco
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.cmdAdicionar = new DevExpress.XtraBars.BarButtonItem();
            this.cmdRemover = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.cadastroRegrasRiscoBase = new Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco.CadastroRegrasRiscoBase();
            this.cmdAtualizar = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.cmdAdicionar,
            this.cmdRemover,
            this.cmdAtualizar});
            this.barManager1.MaxItemId = 3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdAdicionar),
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdRemover),
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdAtualizar)});
            this.bar1.Text = "Tools";
            // 
            // cmdAdicionar
            // 
            this.cmdAdicionar.Caption = "Adicionar";
            this.cmdAdicionar.Id = 0;
            this.cmdAdicionar.Name = "cmdAdicionar";
            // 
            // cmdRemover
            // 
            this.cmdRemover.Caption = "Remover";
            this.cmdRemover.Id = 1;
            this.cmdRemover.Name = "cmdRemover";
            // 
            // cadastroRegrasRiscoBase
            // 
            this.cadastroRegrasRiscoBase.AgrupamentoBase = null;
            this.cadastroRegrasRiscoBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cadastroRegrasRiscoBase.Location = new System.Drawing.Point(0, 24);
            this.cadastroRegrasRiscoBase.Name = "cadastroRegrasRiscoBase";
            this.cadastroRegrasRiscoBase.Size = new System.Drawing.Size(619, 354);
            this.cadastroRegrasRiscoBase.TabIndex = 4;
            // 
            // cmdAtualizar
            // 
            this.cmdAtualizar.Caption = "Atualizar";
            this.cmdAtualizar.Id = 2;
            this.cmdAtualizar.Name = "cmdAtualizar";
            this.cmdAtualizar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.cmdAtualizar_ItemClick);
            // 
            // CadastroRegrasRisco
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cadastroRegrasRiscoBase);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "CadastroRegrasRisco";
            this.Size = new System.Drawing.Size(619, 378);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem cmdAdicionar;
        private DevExpress.XtraBars.BarButtonItem cmdRemover;
        private CadastroRegrasRiscoBase cadastroRegrasRiscoBase;
        private DevExpress.XtraBars.BarButtonItem cmdAtualizar;

    }
}

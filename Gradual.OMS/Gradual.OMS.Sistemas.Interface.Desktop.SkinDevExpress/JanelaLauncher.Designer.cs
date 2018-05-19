namespace Gradual.OMS.Sistemas.Interface.Desktop.SkinDevExpress
{
    partial class JanelaLauncher
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JanelaLauncher));
            this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.menuSalvarConfiguracoes = new DevExpress.XtraBars.BarButtonItem();
            this.menuCarregarConfiguracoes = new DevExpress.XtraBars.BarButtonItem();
            this.menuSair = new DevExpress.XtraBars.BarButtonItem();
            this.menuAplicacoes = new DevExpress.XtraBars.BarSubItem();
            this.menuDesktops = new DevExpress.XtraBars.BarSubItem();
            this.menuAdicionarDesktop = new DevExpress.XtraBars.BarButtonItem();
            this.menuRemoverDesktop = new DevExpress.XtraBars.BarButtonItem();
            this.menuConfigurarDesktop = new DevExpress.XtraBars.BarButtonItem();
            this.menuSkins = new DevExpress.XtraBars.BarSubItem();
            this.toolbarDesktops = new DevExpress.XtraBars.Bar();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.statusDesktopAtivo = new DevExpress.XtraBars.BarStaticItem();
            this.statusMemory = new DevExpress.XtraBars.BarStaticItem();
            this.toolbarAplicacoes = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.images16 = new DevExpress.Utils.ImageCollection(this.components);
            this.menuAparencia = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.images16)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.toolbarDesktops,
            this.bar1,
            this.toolbarAplicacoes});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.DockManager = this.dockManager;
            this.barManager.Form = this;
            this.barManager.Images = this.images16;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.menuAparencia,
            this.menuSkins,
            this.menuDesktops,
            this.menuAplicacoes,
            this.barSubItem3,
            this.barButtonItem1,
            this.menuAdicionarDesktop,
            this.menuSalvarConfiguracoes,
            this.menuCarregarConfiguracoes,
            this.menuRemoverDesktop,
            this.menuConfigurarDesktop,
            this.menuSair,
            this.statusDesktopAtivo,
            this.statusMemory});
            this.barManager.MainMenu = this.bar2;
            this.barManager.MaxItemId = 15;
            this.barManager.StatusBar = this.bar1;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.FloatLocation = new System.Drawing.Point(90, 170);
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.menuAplicacoes),
            new DevExpress.XtraBars.LinkPersistInfo(this.menuDesktops),
            new DevExpress.XtraBars.LinkPersistInfo(this.menuSkins)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "Principal";
            this.barSubItem3.Id = 4;
            this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.menuSalvarConfiguracoes),
            new DevExpress.XtraBars.LinkPersistInfo(this.menuCarregarConfiguracoes),
            new DevExpress.XtraBars.LinkPersistInfo(this.menuSair, true)});
            this.barSubItem3.Name = "barSubItem3";
            // 
            // menuSalvarConfiguracoes
            // 
            this.menuSalvarConfiguracoes.Caption = "Salvar Configurações";
            this.menuSalvarConfiguracoes.Id = 8;
            this.menuSalvarConfiguracoes.ImageIndex = 235;
            this.menuSalvarConfiguracoes.Name = "menuSalvarConfiguracoes";
            this.menuSalvarConfiguracoes.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuSalvarConfiguracoes_ItemClick);
            // 
            // menuCarregarConfiguracoes
            // 
            this.menuCarregarConfiguracoes.Caption = "Carregar Configurações";
            this.menuCarregarConfiguracoes.Id = 9;
            this.menuCarregarConfiguracoes.ImageIndex = 127;
            this.menuCarregarConfiguracoes.Name = "menuCarregarConfiguracoes";
            this.menuCarregarConfiguracoes.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuCarregarConfiguracoes_ItemClick);
            // 
            // menuSair
            // 
            this.menuSair.Caption = "Sair";
            this.menuSair.Id = 12;
            this.menuSair.Name = "menuSair";
            this.menuSair.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuSair_ItemClick);
            // 
            // menuAplicacoes
            // 
            this.menuAplicacoes.Caption = "Aplicações";
            this.menuAplicacoes.Id = 3;
            this.menuAplicacoes.Name = "menuAplicacoes";
            // 
            // menuDesktops
            // 
            this.menuDesktops.Caption = "Desktops";
            this.menuDesktops.Id = 2;
            this.menuDesktops.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.menuAdicionarDesktop),
            new DevExpress.XtraBars.LinkPersistInfo(this.menuRemoverDesktop),
            new DevExpress.XtraBars.LinkPersistInfo(this.menuConfigurarDesktop)});
            this.menuDesktops.Name = "menuDesktops";
            // 
            // menuAdicionarDesktop
            // 
            this.menuAdicionarDesktop.Caption = "Adicionar Desktop";
            this.menuAdicionarDesktop.Id = 6;
            this.menuAdicionarDesktop.ImageIndex = 302;
            this.menuAdicionarDesktop.Name = "menuAdicionarDesktop";
            this.menuAdicionarDesktop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuAdicionarDesktop_ItemClick);
            // 
            // menuRemoverDesktop
            // 
            this.menuRemoverDesktop.Caption = "Remover Desktop";
            this.menuRemoverDesktop.Id = 10;
            this.menuRemoverDesktop.ImageIndex = 73;
            this.menuRemoverDesktop.Name = "menuRemoverDesktop";
            this.menuRemoverDesktop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuRemoverDesktop_ItemClick);
            // 
            // menuConfigurarDesktop
            // 
            this.menuConfigurarDesktop.Caption = "Configurar Desktop";
            this.menuConfigurarDesktop.Id = 11;
            this.menuConfigurarDesktop.ImageIndex = 250;
            this.menuConfigurarDesktop.Name = "menuConfigurarDesktop";
            this.menuConfigurarDesktop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuConfigurarDesktop_ItemClick);
            // 
            // menuSkins
            // 
            this.menuSkins.Caption = "Skins";
            this.menuSkins.Id = 1;
            this.menuSkins.Name = "menuSkins";
            // 
            // toolbarDesktops
            // 
            this.toolbarDesktops.BarName = "Desktops";
            this.toolbarDesktops.DockCol = 1;
            this.toolbarDesktops.DockRow = 1;
            this.toolbarDesktops.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.toolbarDesktops.Offset = 38;
            this.toolbarDesktops.Text = "Custom 3";
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 4";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.statusDesktopAtivo),
            new DevExpress.XtraBars.LinkPersistInfo(this.statusMemory)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 4";
            // 
            // statusDesktopAtivo
            // 
            this.statusDesktopAtivo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
            this.statusDesktopAtivo.Id = 13;
            this.statusDesktopAtivo.Name = "statusDesktopAtivo";
            this.statusDesktopAtivo.TextAlignment = System.Drawing.StringAlignment.Near;
            this.statusDesktopAtivo.Width = 60;
            // 
            // statusMemory
            // 
            this.statusMemory.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
            this.statusMemory.Id = 14;
            this.statusMemory.Name = "statusMemory";
            this.statusMemory.TextAlignment = System.Drawing.StringAlignment.Near;
            this.statusMemory.Width = 60;
            // 
            // toolbarAplicacoes
            // 
            this.toolbarAplicacoes.BarName = "Custom 5";
            this.toolbarAplicacoes.DockCol = 0;
            this.toolbarAplicacoes.DockRow = 1;
            this.toolbarAplicacoes.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.toolbarAplicacoes.Text = "Custom 5";
            // 
            // dockManager
            // 
            this.dockManager.Form = this;
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // images16
            // 
            this.images16.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("images16.ImageStream")));
            // 
            // menuAparencia
            // 
            this.menuAparencia.Caption = "Aparência";
            this.menuAparencia.Id = 0;
            this.menuAparencia.Name = "menuAparencia";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 5;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // JanelaLauncher
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 152);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IsMdiContainer = true;
            this.Name = "JanelaLauncher";
            this.Text = "JanelaLauncher";
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.images16)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem menuAparencia;
        private DevExpress.XtraBars.BarSubItem menuSkins;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraBars.BarSubItem menuAplicacoes;
        private DevExpress.XtraBars.BarSubItem menuDesktops;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem menuAdicionarDesktop;
        private DevExpress.XtraBars.Bar toolbarDesktops;
        private DevExpress.Utils.ImageCollection images16;
        private DevExpress.XtraBars.BarButtonItem menuSalvarConfiguracoes;
        private DevExpress.XtraBars.BarButtonItem menuCarregarConfiguracoes;
        private DevExpress.XtraBars.BarButtonItem menuSair;
        private DevExpress.XtraBars.BarButtonItem menuRemoverDesktop;
        private DevExpress.XtraBars.BarButtonItem menuConfigurarDesktop;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarStaticItem statusDesktopAtivo;
        private System.Windows.Forms.Timer timer;
        private DevExpress.XtraBars.BarStaticItem statusMemory;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private DevExpress.XtraBars.Bar toolbarAplicacoes;
        public DevExpress.XtraBars.Docking.DockManager dockManager;
    }
}
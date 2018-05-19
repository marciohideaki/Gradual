namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    partial class UsuarioGrupoDetalhe
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.grdPerfil = new DevExpress.XtraGrid.GridControl();
            this.grdvPerfil = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdPermissao = new DevExpress.XtraGrid.GridControl();
            this.grdvPermissao = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ppg = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.cmdPermissaoAdicionar = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.cmdPermissaoRemover = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.cmdPerfilAdicionar = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.cmdPerfilRemover = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.repPermissoes = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colCodigoPermissao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPerfil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvPerfil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPermissao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvPermissao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ppg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repPermissoes)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.layoutControl1.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = true;
            this.layoutControl1.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText;
            this.layoutControl1.Appearance.DisabledLayoutItem.Options.UseForeColor = true;
            this.layoutControl1.Controls.Add(this.cmdPerfilRemover);
            this.layoutControl1.Controls.Add(this.cmdPerfilAdicionar);
            this.layoutControl1.Controls.Add(this.cmdPermissaoRemover);
            this.layoutControl1.Controls.Add(this.cmdPermissaoAdicionar);
            this.layoutControl1.Controls.Add(this.grdPerfil);
            this.layoutControl1.Controls.Add(this.grdPermissao);
            this.layoutControl1.Controls.Add(this.ppg);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(586, 372);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // grdPerfil
            // 
            this.grdPerfil.Location = new System.Drawing.Point(16, 38);
            this.grdPerfil.MainView = this.grdvPerfil;
            this.grdPerfil.Name = "grdPerfil";
            this.grdPerfil.Size = new System.Drawing.Size(552, 283);
            this.grdPerfil.TabIndex = 6;
            this.grdPerfil.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvPerfil});
            // 
            // grdvPerfil
            // 
            this.grdvPerfil.GridControl = this.grdPerfil;
            this.grdvPerfil.Name = "grdvPerfil";
            this.grdvPerfil.OptionsBehavior.Editable = false;
            this.grdvPerfil.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdvPerfil.OptionsView.ShowGroupPanel = false;
            // 
            // grdPermissao
            // 
            this.grdPermissao.Location = new System.Drawing.Point(16, 38);
            this.grdPermissao.MainView = this.grdvPermissao;
            this.grdPermissao.Name = "grdPermissao";
            this.grdPermissao.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repPermissoes});
            this.grdPermissao.Size = new System.Drawing.Size(552, 283);
            this.grdPermissao.TabIndex = 5;
            this.grdPermissao.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvPermissao});
            // 
            // grdvPermissao
            // 
            this.grdvPermissao.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCodigoPermissao,
            this.colStatus});
            this.grdvPermissao.GridControl = this.grdPermissao;
            this.grdvPermissao.Name = "grdvPermissao";
            this.grdvPermissao.OptionsBehavior.Editable = false;
            this.grdvPermissao.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdvPermissao.OptionsView.ShowGroupPanel = false;
            // 
            // ppg
            // 
            this.ppg.Location = new System.Drawing.Point(16, 38);
            this.ppg.Name = "ppg";
            this.ppg.Size = new System.Drawing.Size(552, 316);
            this.ppg.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabbedControlGroup1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(586, 372);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // tabbedControlGroup1
            // 
            this.tabbedControlGroup1.CustomizationFormText = "tabbedControlGroup1";
            this.tabbedControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.tabbedControlGroup1.Name = "tabbedControlGroup1";
            this.tabbedControlGroup1.SelectedTabPage = this.layoutControlGroup2;
            this.tabbedControlGroup1.SelectedTabPageIndex = 0;
            this.tabbedControlGroup1.Size = new System.Drawing.Size(584, 370);
            this.tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlGroup3,
            this.layoutControlGroup4});
            this.tabbedControlGroup1.Text = "tabbedControlGroup1";
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "Grupo de Usuário";
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(563, 327);
            this.layoutControlGroup2.Text = "Grupo de Usuário";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ppg;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(563, 327);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.CustomizationFormText = "Permissões";
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(563, 327);
            this.layoutControlGroup3.Text = "Permissões";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.grdPermissao;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(563, 294);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.CustomizationFormText = "Perfis";
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(563, 327);
            this.layoutControlGroup4.Text = "Perfis";
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.grdPerfil;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(563, 294);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // cmdPermissaoAdicionar
            // 
            this.cmdPermissaoAdicionar.Location = new System.Drawing.Point(16, 332);
            this.cmdPermissaoAdicionar.Name = "cmdPermissaoAdicionar";
            this.cmdPermissaoAdicionar.Size = new System.Drawing.Size(270, 22);
            this.cmdPermissaoAdicionar.StyleController = this.layoutControl1;
            this.cmdPermissaoAdicionar.TabIndex = 7;
            this.cmdPermissaoAdicionar.Text = "Adicionar";
            this.cmdPermissaoAdicionar.Click += new System.EventHandler(this.cmdPermissaoAdicionar_Click);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.cmdPermissaoAdicionar;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 294);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(281, 33);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // cmdPermissaoRemover
            // 
            this.cmdPermissaoRemover.Location = new System.Drawing.Point(297, 332);
            this.cmdPermissaoRemover.Name = "cmdPermissaoRemover";
            this.cmdPermissaoRemover.Size = new System.Drawing.Size(271, 22);
            this.cmdPermissaoRemover.StyleController = this.layoutControl1;
            this.cmdPermissaoRemover.TabIndex = 8;
            this.cmdPermissaoRemover.Text = "Remover";
            this.cmdPermissaoRemover.Click += new System.EventHandler(this.cmdPermissaoRemover_Click);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.cmdPermissaoRemover;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(281, 294);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(282, 33);
            this.layoutControlItem5.Text = "layoutControlItem5";
            this.layoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextToControlDistance = 0;
            this.layoutControlItem5.TextVisible = false;
            // 
            // cmdPerfilAdicionar
            // 
            this.cmdPerfilAdicionar.Location = new System.Drawing.Point(16, 332);
            this.cmdPerfilAdicionar.Name = "cmdPerfilAdicionar";
            this.cmdPerfilAdicionar.Size = new System.Drawing.Size(270, 22);
            this.cmdPerfilAdicionar.StyleController = this.layoutControl1;
            this.cmdPerfilAdicionar.TabIndex = 9;
            this.cmdPerfilAdicionar.Text = "Adicionar";
            this.cmdPerfilAdicionar.Click += new System.EventHandler(this.cmdPerfilAdicionar_Click);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.cmdPerfilAdicionar;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 294);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(281, 33);
            this.layoutControlItem6.Text = "layoutControlItem6";
            this.layoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextToControlDistance = 0;
            this.layoutControlItem6.TextVisible = false;
            // 
            // cmdPerfilRemover
            // 
            this.cmdPerfilRemover.Location = new System.Drawing.Point(297, 332);
            this.cmdPerfilRemover.Name = "cmdPerfilRemover";
            this.cmdPerfilRemover.Size = new System.Drawing.Size(271, 22);
            this.cmdPerfilRemover.StyleController = this.layoutControl1;
            this.cmdPerfilRemover.TabIndex = 10;
            this.cmdPerfilRemover.Text = "Remover";
            this.cmdPerfilRemover.Click += new System.EventHandler(this.cmdPerfilRemover_Click);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.cmdPerfilRemover;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem7";
            this.layoutControlItem7.Location = new System.Drawing.Point(281, 294);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(282, 33);
            this.layoutControlItem7.Text = "layoutControlItem7";
            this.layoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextToControlDistance = 0;
            this.layoutControlItem7.TextVisible = false;
            // 
            // repPermissoes
            // 
            this.repPermissoes.AutoHeight = false;
            this.repPermissoes.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repPermissoes.DisplayMember = "NomePermissao";
            this.repPermissoes.Name = "repPermissoes";
            this.repPermissoes.ValueMember = "CodigoPermissao";
            // 
            // colCodigoPermissao
            // 
            this.colCodigoPermissao.Caption = "Permissão";
            this.colCodigoPermissao.ColumnEdit = this.repPermissoes;
            this.colCodigoPermissao.FieldName = "CodigoPermissao";
            this.colCodigoPermissao.Name = "colCodigoPermissao";
            this.colCodigoPermissao.Visible = true;
            this.colCodigoPermissao.VisibleIndex = 0;
            // 
            // colStatus
            // 
            this.colStatus.Caption = "Status";
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 1;
            // 
            // UsuarioGrupoDetalhe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "UsuarioGrupoDetalhe";
            this.Size = new System.Drawing.Size(586, 372);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPerfil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvPerfil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPermissao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvPermissao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ppg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repPermissoes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraVerticalGrid.PropertyGridControl ppg;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
        private DevExpress.XtraGrid.GridControl grdPerfil;
        private DevExpress.XtraGrid.Views.Grid.GridView grdvPerfil;
        private DevExpress.XtraGrid.GridControl grdPermissao;
        private DevExpress.XtraGrid.Views.Grid.GridView grdvPermissao;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton cmdPerfilRemover;
        private DevExpress.XtraEditors.SimpleButton cmdPerfilAdicionar;
        private DevExpress.XtraEditors.SimpleButton cmdPermissaoRemover;
        private DevExpress.XtraEditors.SimpleButton cmdPermissaoAdicionar;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigoPermissao;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repPermissoes;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
    }
}

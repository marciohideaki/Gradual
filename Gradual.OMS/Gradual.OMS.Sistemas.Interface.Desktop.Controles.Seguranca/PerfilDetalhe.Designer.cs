namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    partial class PerfilDetalhe
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
            this.cmdPermissaoRemover = new DevExpress.XtraEditors.SimpleButton();
            this.cmdPermissaoAdicionar = new DevExpress.XtraEditors.SimpleButton();
            this.grdPermissao = new DevExpress.XtraGrid.GridControl();
            this.grdvPermissao = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCodigoPermissao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repPermissoes = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ppg = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tabPerfil = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPermissao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvPermissao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repPermissoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ppg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabPerfil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.layoutControl1.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = true;
            this.layoutControl1.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText;
            this.layoutControl1.Appearance.DisabledLayoutItem.Options.UseForeColor = true;
            this.layoutControl1.Controls.Add(this.cmdPermissaoRemover);
            this.layoutControl1.Controls.Add(this.cmdPermissaoAdicionar);
            this.layoutControl1.Controls.Add(this.grdPermissao);
            this.layoutControl1.Controls.Add(this.ppg);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(642, 386);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // cmdPermissaoRemover
            // 
            this.cmdPermissaoRemover.Location = new System.Drawing.Point(325, 346);
            this.cmdPermissaoRemover.Name = "cmdPermissaoRemover";
            this.cmdPermissaoRemover.Size = new System.Drawing.Size(299, 22);
            this.cmdPermissaoRemover.StyleController = this.layoutControl1;
            this.cmdPermissaoRemover.TabIndex = 7;
            this.cmdPermissaoRemover.Text = "Remover";
            this.cmdPermissaoRemover.Click += new System.EventHandler(this.cmdPermissaoRemover_Click);
            // 
            // cmdPermissaoAdicionar
            // 
            this.cmdPermissaoAdicionar.Location = new System.Drawing.Point(16, 346);
            this.cmdPermissaoAdicionar.Name = "cmdPermissaoAdicionar";
            this.cmdPermissaoAdicionar.Size = new System.Drawing.Size(298, 22);
            this.cmdPermissaoAdicionar.StyleController = this.layoutControl1;
            this.cmdPermissaoAdicionar.TabIndex = 6;
            this.cmdPermissaoAdicionar.Text = "Adicionar";
            this.cmdPermissaoAdicionar.Click += new System.EventHandler(this.cmdPermissaoAdicionar_Click);
            // 
            // grdPermissao
            // 
            this.grdPermissao.Location = new System.Drawing.Point(16, 38);
            this.grdPermissao.MainView = this.grdvPermissao;
            this.grdPermissao.Name = "grdPermissao";
            this.grdPermissao.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repPermissoes});
            this.grdPermissao.Size = new System.Drawing.Size(608, 297);
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
            // colCodigoPermissao
            // 
            this.colCodigoPermissao.Caption = "Permissão";
            this.colCodigoPermissao.ColumnEdit = this.repPermissoes;
            this.colCodigoPermissao.FieldName = "CodigoPermissao";
            this.colCodigoPermissao.Name = "colCodigoPermissao";
            this.colCodigoPermissao.Visible = true;
            this.colCodigoPermissao.VisibleIndex = 0;
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
            // colStatus
            // 
            this.colStatus.Caption = "Status";
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 1;
            // 
            // ppg
            // 
            this.ppg.Location = new System.Drawing.Point(16, 38);
            this.ppg.Name = "ppg";
            this.ppg.Size = new System.Drawing.Size(608, 330);
            this.ppg.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabbedControlGroup1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(642, 386);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // tabbedControlGroup1
            // 
            this.tabbedControlGroup1.CustomizationFormText = "tabbedControlGroup1";
            this.tabbedControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.tabbedControlGroup1.Name = "tabbedControlGroup1";
            this.tabbedControlGroup1.SelectedTabPage = this.tabPerfil;
            this.tabbedControlGroup1.SelectedTabPageIndex = 0;
            this.tabbedControlGroup1.Size = new System.Drawing.Size(640, 384);
            this.tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabPerfil,
            this.layoutControlGroup3});
            this.tabbedControlGroup1.Text = "tabbedControlGroup1";
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.CustomizationFormText = "Permissões";
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(619, 341);
            this.layoutControlGroup3.Text = "Permissões";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.grdPermissao;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(619, 308);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.cmdPermissaoAdicionar;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 308);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(309, 33);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.cmdPermissaoRemover;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(309, 308);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(310, 33);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // tabPerfil
            // 
            this.tabPerfil.CustomizationFormText = "Perfil";
            this.tabPerfil.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.tabPerfil.Location = new System.Drawing.Point(0, 0);
            this.tabPerfil.Name = "tabPerfil";
            this.tabPerfil.Size = new System.Drawing.Size(619, 341);
            this.tabPerfil.Text = "Perfil";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ppg;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(619, 341);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // PerfilDetalhe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "PerfilDetalhe";
            this.Size = new System.Drawing.Size(642, 386);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPermissao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvPermissao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repPermissoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ppg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabPerfil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton cmdPermissaoRemover;
        private DevExpress.XtraEditors.SimpleButton cmdPermissaoAdicionar;
        private DevExpress.XtraGrid.GridControl grdPermissao;
        private DevExpress.XtraGrid.Views.Grid.GridView grdvPermissao;
        private DevExpress.XtraVerticalGrid.PropertyGridControl ppg;
        private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup tabPerfil;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigoPermissao;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repPermissoes;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
    }
}

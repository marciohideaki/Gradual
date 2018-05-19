namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Ordens
{
    partial class OperacaoHistorico
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
            this.colOperacaoStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdOperacoes = new DevExpress.XtraGrid.GridControl();
            this.grdvOperacoes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colOperacaoCodigoOrdem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperacaoDataReferencia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperacaoDataUltimaAlteracao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperacaoDirecao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperacaoInstrumento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperacaoPreco = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperacaoQuantidade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperacaoQuantidadeExecutada = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperacaoValidade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.grdMensagens = new DevExpress.XtraGrid.GridControl();
            this.grdvMensagens = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
            this.popupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.cmdConfigurar = new DevExpress.XtraBars.BarButtonItem();
            this.cmdSalvarLayoutDefault = new DevExpress.XtraBars.BarButtonItem();
            this.cmdTeste = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.grdOperacoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvOperacoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMensagens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvMensagens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // colOperacaoStatus
            // 
            this.colOperacaoStatus.Caption = "Status";
            this.colOperacaoStatus.FieldName = "Status";
            this.colOperacaoStatus.Name = "colOperacaoStatus";
            this.colOperacaoStatus.Visible = true;
            this.colOperacaoStatus.VisibleIndex = 8;
            // 
            // grdOperacoes
            // 
            this.grdOperacoes.Location = new System.Drawing.Point(10, 28);
            this.grdOperacoes.MainView = this.grdvOperacoes;
            this.grdOperacoes.Name = "grdOperacoes";
            this.grdOperacoes.Size = new System.Drawing.Size(627, 122);
            this.grdOperacoes.TabIndex = 0;
            this.grdOperacoes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvOperacoes});
            // 
            // grdvOperacoes
            // 
            this.grdvOperacoes.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colOperacaoCodigoOrdem,
            this.colOperacaoDataReferencia,
            this.colOperacaoDataUltimaAlteracao,
            this.colOperacaoDirecao,
            this.colOperacaoInstrumento,
            this.colOperacaoPreco,
            this.colOperacaoQuantidade,
            this.colOperacaoQuantidadeExecutada,
            this.colOperacaoStatus,
            this.colOperacaoValidade});
            this.grdvOperacoes.GridControl = this.grdOperacoes;
            this.grdvOperacoes.Name = "grdvOperacoes";
            this.grdvOperacoes.OptionsBehavior.Editable = false;
            this.grdvOperacoes.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdvOperacoes.OptionsView.ColumnAutoWidth = false;
            this.grdvOperacoes.OptionsView.ShowGroupPanel = false;
            this.grdvOperacoes.OptionsView.ShowIndicator = false;
            // 
            // colOperacaoCodigoOrdem
            // 
            this.colOperacaoCodigoOrdem.Caption = "Código";
            this.colOperacaoCodigoOrdem.FieldName = "CodigoOrdem";
            this.colOperacaoCodigoOrdem.Name = "colOperacaoCodigoOrdem";
            this.colOperacaoCodigoOrdem.Visible = true;
            this.colOperacaoCodigoOrdem.VisibleIndex = 0;
            // 
            // colOperacaoDataReferencia
            // 
            this.colOperacaoDataReferencia.Caption = "Data";
            this.colOperacaoDataReferencia.FieldName = "DataReferencia";
            this.colOperacaoDataReferencia.Name = "colOperacaoDataReferencia";
            this.colOperacaoDataReferencia.Visible = true;
            this.colOperacaoDataReferencia.VisibleIndex = 1;
            // 
            // colOperacaoDataUltimaAlteracao
            // 
            this.colOperacaoDataUltimaAlteracao.Caption = "Última Alteração";
            this.colOperacaoDataUltimaAlteracao.FieldName = "DataUltimaAlteracao";
            this.colOperacaoDataUltimaAlteracao.Name = "colOperacaoDataUltimaAlteracao";
            this.colOperacaoDataUltimaAlteracao.Visible = true;
            this.colOperacaoDataUltimaAlteracao.VisibleIndex = 2;
            // 
            // colOperacaoDirecao
            // 
            this.colOperacaoDirecao.Caption = "C/V";
            this.colOperacaoDirecao.FieldName = "Direcao";
            this.colOperacaoDirecao.Name = "colOperacaoDirecao";
            this.colOperacaoDirecao.Visible = true;
            this.colOperacaoDirecao.VisibleIndex = 3;
            // 
            // colOperacaoInstrumento
            // 
            this.colOperacaoInstrumento.Caption = "Instrumento";
            this.colOperacaoInstrumento.FieldName = "Instrumento";
            this.colOperacaoInstrumento.Name = "colOperacaoInstrumento";
            this.colOperacaoInstrumento.Visible = true;
            this.colOperacaoInstrumento.VisibleIndex = 4;
            // 
            // colOperacaoPreco
            // 
            this.colOperacaoPreco.Caption = "Preço";
            this.colOperacaoPreco.FieldName = "Preco";
            this.colOperacaoPreco.Name = "colOperacaoPreco";
            this.colOperacaoPreco.Visible = true;
            this.colOperacaoPreco.VisibleIndex = 5;
            // 
            // colOperacaoQuantidade
            // 
            this.colOperacaoQuantidade.Caption = "Qtde.";
            this.colOperacaoQuantidade.FieldName = "Quantidade";
            this.colOperacaoQuantidade.Name = "colOperacaoQuantidade";
            this.colOperacaoQuantidade.Visible = true;
            this.colOperacaoQuantidade.VisibleIndex = 6;
            // 
            // colOperacaoQuantidadeExecutada
            // 
            this.colOperacaoQuantidadeExecutada.Caption = "Qtde. Exec.";
            this.colOperacaoQuantidadeExecutada.FieldName = "QuantidadeExecutada";
            this.colOperacaoQuantidadeExecutada.Name = "colOperacaoQuantidadeExecutada";
            this.colOperacaoQuantidadeExecutada.Visible = true;
            this.colOperacaoQuantidadeExecutada.VisibleIndex = 7;
            // 
            // colOperacaoValidade
            // 
            this.colOperacaoValidade.Caption = "Validade";
            this.colOperacaoValidade.FieldName = "Validade";
            this.colOperacaoValidade.Name = "colOperacaoValidade";
            this.colOperacaoValidade.Visible = true;
            this.colOperacaoValidade.VisibleIndex = 9;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.layoutControl1.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = true;
            this.layoutControl1.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText;
            this.layoutControl1.Appearance.DisabledLayoutItem.Options.UseForeColor = true;
            this.layoutControl1.Controls.Add(this.grdMensagens);
            this.layoutControl1.Controls.Add(this.grdOperacoes);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(646, 368);
            this.layoutControl1.TabIndex = 1;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // grdMensagens
            // 
            this.grdMensagens.Location = new System.Drawing.Point(10, 191);
            this.grdMensagens.MainView = this.grdvMensagens;
            this.grdMensagens.Name = "grdMensagens";
            this.grdMensagens.Size = new System.Drawing.Size(627, 168);
            this.grdMensagens.TabIndex = 4;
            this.grdMensagens.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvMensagens});
            // 
            // grdvMensagens
            // 
            this.grdvMensagens.GridControl = this.grdMensagens;
            this.grdvMensagens.Name = "grdvMensagens";
            this.grdvMensagens.OptionsBehavior.Editable = false;
            this.grdvMensagens.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdvMensagens.OptionsView.ColumnAutoWidth = false;
            this.grdvMensagens.OptionsView.ShowGroupPanel = false;
            this.grdvMensagens.OptionsView.ShowIndicator = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlGroup3,
            this.splitterItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(646, 368);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "Root";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "Eventos";
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 163);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(644, 203);
            this.layoutControlGroup2.Text = "Eventos";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.grdMensagens;
            this.layoutControlItem2.CustomizationFormText = "Eventos";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(638, 179);
            this.layoutControlItem2.Text = "Eventos:";
            this.layoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.CustomizationFormText = "Operações";
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(644, 157);
            this.layoutControlGroup3.Text = "Operações";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.grdOperacoes;
            this.layoutControlItem1.CustomizationFormText = "Operações";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(638, 133);
            this.layoutControlItem1.Text = "Operações:";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // splitterItem1
            // 
            this.splitterItem1.CustomizationFormText = "splitterItem1";
            this.splitterItem1.Location = new System.Drawing.Point(0, 157);
            this.splitterItem1.Name = "splitterItem1";
            this.splitterItem1.Size = new System.Drawing.Size(644, 6);
            // 
            // popupMenu
            // 
            this.popupMenu.Name = "popupMenu";
            this.popupMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.cmdConfigurar,
            this.cmdSalvarLayoutDefault,
            this.cmdTeste});
            this.barManager.MaxItemId = 3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdConfigurar),
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdSalvarLayoutDefault),
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdTeste)});
            this.bar1.Text = "Tools";
            // 
            // cmdConfigurar
            // 
            this.cmdConfigurar.Caption = "Configurar";
            this.cmdConfigurar.Id = 0;
            this.cmdConfigurar.Name = "cmdConfigurar";
            // 
            // cmdSalvarLayoutDefault
            // 
            this.cmdSalvarLayoutDefault.Caption = "Salvar Default";
            this.cmdSalvarLayoutDefault.Id = 1;
            this.cmdSalvarLayoutDefault.Name = "cmdSalvarLayoutDefault";
            this.cmdSalvarLayoutDefault.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.cmdSalvarLayoutDefault_ItemClick);
            // 
            // cmdTeste
            // 
            this.cmdTeste.Caption = "teste";
            this.cmdTeste.Id = 2;
            this.cmdTeste.Name = "cmdTeste";
            this.cmdTeste.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.cmdTeste_ItemClick);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlLeft);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlRight);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlBottom);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlTop);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(650, 403);
            this.splitContainerControl1.SplitterPosition = 25;
            this.splitContainerControl1.TabIndex = 5;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // OperacaoHistorico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "OperacaoHistorico";
            this.Size = new System.Drawing.Size(650, 403);
            ((System.ComponentModel.ISupportInitialize)(this.grdOperacoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvOperacoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMensagens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvMensagens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdOperacoes;
        private DevExpress.XtraGrid.Views.Grid.GridView grdvOperacoes;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.GridControl grdMensagens;
        private DevExpress.XtraGrid.Views.Grid.GridView grdvMensagens;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private System.Windows.Forms.ContextMenuStrip popupMenu;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoCodigoOrdem;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoDataReferencia;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoDataUltimaAlteracao;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoDirecao;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoInstrumento;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoPreco;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoQuantidade;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoQuantidadeExecutada;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoStatus;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem cmdConfigurar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraLayout.SplitterItem splitterItem1;
        private DevExpress.XtraBars.BarButtonItem cmdSalvarLayoutDefault;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraBars.BarButtonItem cmdTeste;
        private DevExpress.XtraGrid.Columns.GridColumn colOperacaoValidade;
    }
}

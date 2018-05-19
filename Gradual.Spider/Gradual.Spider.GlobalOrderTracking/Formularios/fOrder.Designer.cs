namespace Gradual.Spider.GlobalOrderTracking.Formularios
{
    partial class fOrder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlMiddle = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridResumo = new GradualForm.Controls.CustomDataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancelarTudo = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.pnlDetailMain = new System.Windows.Forms.Panel();
            this.gridDetalhe = new GradualForm.Controls.CustomDataGridView();
            this.pnlDetailTop = new System.Windows.Forms.Panel();
            this.lblDetalhe = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.gbFiltro = new System.Windows.Forms.GroupBox();
            this.lblIdSessao = new System.Windows.Forms.Label();
            this.checkedListStatus = new System.Windows.Forms.CheckedListBox();
            this.lblConta = new System.Windows.Forms.Label();
            this.cmbSessao = new System.Windows.Forms.ComboBox();
            this.cComboBox = new CheckComboBox.CheckComboBox();
            this.lblSimbolo = new System.Windows.Forms.Label();
            this.lblClientOrderId = new System.Windows.Forms.Label();
            this.txtConta = new GradualForm.Controls.BorderedTextBox();
            this.cmbCorretora = new System.Windows.Forms.ComboBox();
            this.lblLado = new System.Windows.Forms.Label();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.cmbValidade = new System.Windows.Forms.ComboBox();
            this.txtSimbolo = new GradualForm.Controls.BorderedTextBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.txtNumeroOrdem = new GradualForm.Controls.BorderedTextBox();
            this.cmbLado = new System.Windows.Forms.ComboBox();
            this.lblValidade = new System.Windows.Forms.Label();
            this.lblCorretora = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.btnConfiguracoes = new System.Windows.Forms.Button();
            this.gbAssinatura = new System.Windows.Forms.GroupBox();
            this.btxtSignAccount = new GradualForm.Controls.BorderedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.btxtSignSymbol = new GradualForm.Controls.BorderedTextBox();
            this.cmbSignSession = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.açõesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCancelar = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCancelarTudo = new System.Windows.Forms.ToolStripMenuItem();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmConfiguracoes = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSair = new System.Windows.Forms.ToolStripMenuItem();
            this.loadingPanel1 = new GradualForm.Controls.LoadingPanel();
            this.spiderOrderInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.spiderOrderDetailInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.columnDetailOrderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailOrderDetailID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailOrderQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailCumQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailOrderQtyRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailOrderStatusId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailTransactTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailStopPx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailTransactID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailTradeQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailFixMsgSeqNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailCxlRejResponseTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailCxlRejReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailMsgFixDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetailContraBroker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tmrLoading = new System.Windows.Forms.Timer(this.components);
            this.pnlMain.SuspendLayout();
            this.pnlMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResumo)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlDetailMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalhe)).BeginInit();
            this.pnlDetailTop.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.gbFiltro.SuspendLayout();
            this.gbAssinatura.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spiderOrderInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spiderOrderDetailInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlMiddle);
            this.pnlMain.Controls.Add(this.pnlTop);
            this.pnlMain.Controls.Add(this.pnlBottom);
            this.pnlMain.Controls.Add(this.menuStrip1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(6, 36);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1188, 536);
            this.pnlMain.TabIndex = 5;
            // 
            // pnlMiddle
            // 
            this.pnlMiddle.Controls.Add(this.splitContainer1);
            this.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddle.Location = new System.Drawing.Point(0, 186);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Size = new System.Drawing.Size(1188, 314);
            this.pnlMiddle.TabIndex = 6;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridResumo);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnlDetailMain);
            this.splitContainer1.Panel2.Controls.Add(this.pnlDetailTop);
            this.splitContainer1.Size = new System.Drawing.Size(1188, 314);
            this.splitContainer1.SplitterDistance = 149;
            this.splitContainer1.TabIndex = 1;
            // 
            // gridResumo
            // 
            this.gridResumo.AllowUserToAddRows = false;
            this.gridResumo.AllowUserToDeleteRows = false;
            this.gridResumo.AllowUserToOrderColumns = true;
            this.gridResumo.AllowUserToResizeRows = false;
            this.gridResumo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridResumo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridResumo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridResumo.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridResumo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridResumo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridResumo.Location = new System.Drawing.Point(0, 0);
            this.gridResumo.Name = "gridResumo";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridResumo.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridResumo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResumo.Size = new System.Drawing.Size(1188, 122);
            this.gridResumo.TabIndex = 12;
            this.gridResumo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Resumo_CellClick);
            this.gridResumo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Resumo_CellFormatting);
            this.gridResumo.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gridResumo_CellPainting);
            this.gridResumo.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.Resumo_RowPrePaint);
            this.gridResumo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridResumo_KeyDown);
            this.gridResumo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gridResumo_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancelarTudo);
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 122);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1188, 27);
            this.panel1.TabIndex = 1;
            // 
            // btnCancelarTudo
            // 
            this.btnCancelarTudo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelarTudo.Location = new System.Drawing.Point(105, 4);
            this.btnCancelarTudo.Name = "btnCancelarTudo";
            this.btnCancelarTudo.Size = new System.Drawing.Size(100, 20);
            this.btnCancelarTudo.TabIndex = 53;
            this.btnCancelarTudo.Text = "Cancelar Tudo";
            this.btnCancelarTudo.UseVisualStyleBackColor = true;
            this.btnCancelarTudo.Click += new System.EventHandler(this.btnCancelarTudo_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(4, 4);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 20);
            this.btnCancelar.TabIndex = 52;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // pnlDetailMain
            // 
            this.pnlDetailMain.Controls.Add(this.gridDetalhe);
            this.pnlDetailMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailMain.Location = new System.Drawing.Point(0, 20);
            this.pnlDetailMain.Name = "pnlDetailMain";
            this.pnlDetailMain.Size = new System.Drawing.Size(1188, 141);
            this.pnlDetailMain.TabIndex = 1;
            // 
            // gridDetalhe
            // 
            this.gridDetalhe.AllowUserToAddRows = false;
            this.gridDetalhe.AllowUserToDeleteRows = false;
            this.gridDetalhe.AllowUserToOrderColumns = true;
            this.gridDetalhe.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetalhe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridDetalhe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridDetalhe.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridDetalhe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetalhe.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridDetalhe.Location = new System.Drawing.Point(0, 0);
            this.gridDetalhe.Name = "gridDetalhe";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetalhe.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gridDetalhe.Size = new System.Drawing.Size(1188, 141);
            this.gridDetalhe.TabIndex = 13;
            this.gridDetalhe.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Detalhe_CellFormatting);
            this.gridDetalhe.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.Detalhe_RowPrePaint);
            // 
            // pnlDetailTop
            // 
            this.pnlDetailTop.Controls.Add(this.lblDetalhe);
            this.pnlDetailTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetailTop.Location = new System.Drawing.Point(0, 0);
            this.pnlDetailTop.Name = "pnlDetailTop";
            this.pnlDetailTop.Size = new System.Drawing.Size(1188, 20);
            this.pnlDetailTop.TabIndex = 2;
            this.pnlDetailTop.Click += new System.EventHandler(this.pnlDetailTop_Click);
            // 
            // lblDetalhe
            // 
            this.lblDetalhe.AutoSize = true;
            this.lblDetalhe.Location = new System.Drawing.Point(6, 4);
            this.lblDetalhe.Name = "lblDetalhe";
            this.lblDetalhe.Size = new System.Drawing.Size(53, 13);
            this.lblDetalhe.TabIndex = 0;
            this.lblDetalhe.Text = "+ Detalhe";
            this.lblDetalhe.Click += new System.EventHandler(this.lblDetalhe_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.gbFiltro);
            this.pnlTop.Controls.Add(this.btnConfiguracoes);
            this.pnlTop.Controls.Add(this.gbAssinatura);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 24);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.pnlTop.Size = new System.Drawing.Size(1188, 162);
            this.pnlTop.TabIndex = 9;
            // 
            // gbFiltro
            // 
            this.gbFiltro.Controls.Add(this.lblIdSessao);
            this.gbFiltro.Controls.Add(this.checkedListStatus);
            this.gbFiltro.Controls.Add(this.lblConta);
            this.gbFiltro.Controls.Add(this.cmbSessao);
            this.gbFiltro.Controls.Add(this.cComboBox);
            this.gbFiltro.Controls.Add(this.lblSimbolo);
            this.gbFiltro.Controls.Add(this.lblClientOrderId);
            this.gbFiltro.Controls.Add(this.txtConta);
            this.gbFiltro.Controls.Add(this.cmbCorretora);
            this.gbFiltro.Controls.Add(this.lblLado);
            this.gbFiltro.Controls.Add(this.cmbTipo);
            this.gbFiltro.Controls.Add(this.lblEstado);
            this.gbFiltro.Controls.Add(this.btnFiltrar);
            this.gbFiltro.Controls.Add(this.cmbValidade);
            this.gbFiltro.Controls.Add(this.txtSimbolo);
            this.gbFiltro.Controls.Add(this.lblTipo);
            this.gbFiltro.Controls.Add(this.btnLimpar);
            this.gbFiltro.Controls.Add(this.txtNumeroOrdem);
            this.gbFiltro.Controls.Add(this.cmbLado);
            this.gbFiltro.Controls.Add(this.lblValidade);
            this.gbFiltro.Controls.Add(this.lblCorretora);
            this.gbFiltro.Controls.Add(this.cmbEstado);
            this.gbFiltro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFiltro.ForeColor = System.Drawing.Color.White;
            this.gbFiltro.Location = new System.Drawing.Point(0, 50);
            this.gbFiltro.Name = "gbFiltro";
            this.gbFiltro.Size = new System.Drawing.Size(1188, 109);
            this.gbFiltro.TabIndex = 1;
            this.gbFiltro.TabStop = false;
            this.gbFiltro.Tag = "";
            this.gbFiltro.Text = "Sub-Filtro";
            // 
            // lblIdSessao
            // 
            this.lblIdSessao.BackColor = System.Drawing.Color.Transparent;
            this.lblIdSessao.Location = new System.Drawing.Point(5, 22);
            this.lblIdSessao.Name = "lblIdSessao";
            this.lblIdSessao.Size = new System.Drawing.Size(58, 15);
            this.lblIdSessao.TabIndex = 47;
            this.lblIdSessao.Tag = "";
            this.lblIdSessao.Text = "Sessão";
            // 
            // checkedListStatus
            // 
            this.checkedListStatus.FormattingEnabled = true;
            this.checkedListStatus.Location = new System.Drawing.Point(645, 19);
            this.checkedListStatus.Name = "checkedListStatus";
            this.checkedListStatus.Size = new System.Drawing.Size(120, 79);
            this.checkedListStatus.TabIndex = 51;
            this.checkedListStatus.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListStatus_ItemCheck);
            // 
            // lblConta
            // 
            this.lblConta.BackColor = System.Drawing.Color.Transparent;
            this.lblConta.Location = new System.Drawing.Point(6, 49);
            this.lblConta.Name = "lblConta";
            this.lblConta.Size = new System.Drawing.Size(51, 15);
            this.lblConta.TabIndex = 48;
            this.lblConta.Tag = "";
            this.lblConta.Text = "Conta";
            // 
            // cmbSessao
            // 
            this.cmbSessao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSessao.FormattingEnabled = true;
            this.cmbSessao.Items.AddRange(new object[] {
            "(Todas)"});
            this.cmbSessao.Location = new System.Drawing.Point(69, 19);
            this.cmbSessao.Name = "cmbSessao";
            this.cmbSessao.Size = new System.Drawing.Size(125, 21);
            this.cmbSessao.TabIndex = 4;
            // 
            // cComboBox
            // 
            this.cComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cComboBox.ForeColor = System.Drawing.Color.White;
            this.cComboBox.FormattingEnabled = true;
            this.cComboBox.Location = new System.Drawing.Point(867, 6);
            this.cComboBox.Name = "cComboBox";
            this.cComboBox.Size = new System.Drawing.Size(125, 21);
            this.cComboBox.TabIndex = 50;
            this.cComboBox.Text = "(Todos)";
            this.cComboBox.Visible = false;
            // 
            // lblSimbolo
            // 
            this.lblSimbolo.Location = new System.Drawing.Point(203, 22);
            this.lblSimbolo.Name = "lblSimbolo";
            this.lblSimbolo.Size = new System.Drawing.Size(51, 15);
            this.lblSimbolo.TabIndex = 34;
            this.lblSimbolo.Text = "Ativo";
            // 
            // lblClientOrderId
            // 
            this.lblClientOrderId.BackColor = System.Drawing.Color.Transparent;
            this.lblClientOrderId.Location = new System.Drawing.Point(6, 75);
            this.lblClientOrderId.Name = "lblClientOrderId";
            this.lblClientOrderId.Size = new System.Drawing.Size(57, 15);
            this.lblClientOrderId.TabIndex = 31;
            this.lblClientOrderId.Text = "Nr. Ordem";
            // 
            // txtConta
            // 
            this.txtConta.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.txtConta.ForeColor = System.Drawing.Color.White;
            this.txtConta.Location = new System.Drawing.Point(69, 46);
            this.txtConta.Name = "txtConta";
            this.txtConta.Padding = new System.Windows.Forms.Padding(1);
            this.txtConta.Size = new System.Drawing.Size(125, 20);
            this.txtConta.TabIndex = 5;
            // 
            // cmbCorretora
            // 
            this.cmbCorretora.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCorretora.FormattingEnabled = true;
            this.cmbCorretora.Items.AddRange(new object[] {
            "(Todas)"});
            this.cmbCorretora.Location = new System.Drawing.Point(260, 72);
            this.cmbCorretora.Name = "cmbCorretora";
            this.cmbCorretora.Size = new System.Drawing.Size(125, 21);
            this.cmbCorretora.TabIndex = 10;
            // 
            // lblLado
            // 
            this.lblLado.Location = new System.Drawing.Point(203, 48);
            this.lblLado.Name = "lblLado";
            this.lblLado.Size = new System.Drawing.Size(51, 15);
            this.lblLado.TabIndex = 36;
            this.lblLado.Text = "Lado";
            // 
            // cmbTipo
            // 
            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.FormattingEnabled = true;
            this.cmbTipo.Items.AddRange(new object[] {
            "(Todos)"});
            this.cmbTipo.Location = new System.Drawing.Point(457, 19);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(125, 21);
            this.cmbTipo.TabIndex = 11;
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(588, 22);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(51, 15);
            this.lblEstado.TabIndex = 30;
            this.lblEstado.Text = "Status";
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFiltrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltrar.Location = new System.Drawing.Point(1083, 71);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(97, 20);
            this.btnFiltrar.TabIndex = 14;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // cmbValidade
            // 
            this.cmbValidade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValidade.FormattingEnabled = true;
            this.cmbValidade.Items.AddRange(new object[] {
            "(Todas)"});
            this.cmbValidade.Location = new System.Drawing.Point(457, 44);
            this.cmbValidade.Name = "cmbValidade";
            this.cmbValidade.Size = new System.Drawing.Size(125, 21);
            this.cmbValidade.TabIndex = 12;
            // 
            // txtSimbolo
            // 
            this.txtSimbolo.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.txtSimbolo.Location = new System.Drawing.Point(260, 17);
            this.txtSimbolo.Name = "txtSimbolo";
            this.txtSimbolo.Padding = new System.Windows.Forms.Padding(1);
            this.txtSimbolo.Size = new System.Drawing.Size(125, 20);
            this.txtSimbolo.TabIndex = 7;
            // 
            // lblTipo
            // 
            this.lblTipo.Location = new System.Drawing.Point(391, 22);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(50, 15);
            this.lblTipo.TabIndex = 29;
            this.lblTipo.Text = "Tipo";
            // 
            // btnLimpar
            // 
            this.btnLimpar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpar.Location = new System.Drawing.Point(1083, 45);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(97, 20);
            this.btnLimpar.TabIndex = 13;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // txtNumeroOrdem
            // 
            this.txtNumeroOrdem.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.txtNumeroOrdem.Location = new System.Drawing.Point(69, 72);
            this.txtNumeroOrdem.Name = "txtNumeroOrdem";
            this.txtNumeroOrdem.Padding = new System.Windows.Forms.Padding(1);
            this.txtNumeroOrdem.Size = new System.Drawing.Size(125, 20);
            this.txtNumeroOrdem.TabIndex = 6;
            // 
            // cmbLado
            // 
            this.cmbLado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLado.FormattingEnabled = true;
            this.cmbLado.Items.AddRange(new object[] {
            "(Todos)"});
            this.cmbLado.Location = new System.Drawing.Point(260, 44);
            this.cmbLado.Name = "cmbLado";
            this.cmbLado.Size = new System.Drawing.Size(125, 21);
            this.cmbLado.TabIndex = 9;
            // 
            // lblValidade
            // 
            this.lblValidade.Location = new System.Drawing.Point(391, 48);
            this.lblValidade.Name = "lblValidade";
            this.lblValidade.Size = new System.Drawing.Size(50, 15);
            this.lblValidade.TabIndex = 28;
            this.lblValidade.Text = "Validade";
            // 
            // lblCorretora
            // 
            this.lblCorretora.Location = new System.Drawing.Point(203, 75);
            this.lblCorretora.Name = "lblCorretora";
            this.lblCorretora.Size = new System.Drawing.Size(75, 15);
            this.lblCorretora.TabIndex = 27;
            this.lblCorretora.Text = "Broker";
            // 
            // cmbEstado
            // 
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Items.AddRange(new object[] {
            "(Todos)"});
            this.cmbEstado.Location = new System.Drawing.Point(829, 72);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(125, 21);
            this.cmbEstado.TabIndex = 8;
            this.cmbEstado.Text = "(Todos)";
            this.cmbEstado.Visible = false;
            // 
            // btnConfiguracoes
            // 
            this.btnConfiguracoes.Location = new System.Drawing.Point(688, 4);
            this.btnConfiguracoes.Name = "btnConfiguracoes";
            this.btnConfiguracoes.Size = new System.Drawing.Size(100, 20);
            this.btnConfiguracoes.TabIndex = 9;
            this.btnConfiguracoes.Text = "Configurações";
            this.btnConfiguracoes.UseVisualStyleBackColor = true;
            this.btnConfiguracoes.Visible = false;
            // 
            // gbAssinatura
            // 
            this.gbAssinatura.Controls.Add(this.btxtSignAccount);
            this.gbAssinatura.Controls.Add(this.label2);
            this.gbAssinatura.Controls.Add(this.label3);
            this.gbAssinatura.Controls.Add(this.btnEnviar);
            this.gbAssinatura.Controls.Add(this.btxtSignSymbol);
            this.gbAssinatura.Controls.Add(this.cmbSignSession);
            this.gbAssinatura.Controls.Add(this.label1);
            this.gbAssinatura.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbAssinatura.ForeColor = System.Drawing.Color.White;
            this.gbAssinatura.Location = new System.Drawing.Point(0, 0);
            this.gbAssinatura.Name = "gbAssinatura";
            this.gbAssinatura.Size = new System.Drawing.Size(1188, 50);
            this.gbAssinatura.TabIndex = 0;
            this.gbAssinatura.TabStop = false;
            this.gbAssinatura.Tag = "";
            this.gbAssinatura.Text = "Assinatura";
            // 
            // btxtSignAccount
            // 
            this.btxtSignAccount.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.btxtSignAccount.ForeColor = System.Drawing.Color.White;
            this.btxtSignAccount.Location = new System.Drawing.Point(459, 19);
            this.btxtSignAccount.Name = "btxtSignAccount";
            this.btxtSignAccount.Padding = new System.Windows.Forms.Padding(1);
            this.btxtSignAccount.Size = new System.Drawing.Size(125, 20);
            this.btxtSignAccount.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(202, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 15);
            this.label2.TabIndex = 52;
            this.label2.Text = "Ativo";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(393, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 53;
            this.label3.Text = "Conta";
            // 
            // btnEnviar
            // 
            this.btnEnviar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnviar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviar.Location = new System.Drawing.Point(1085, 18);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(97, 20);
            this.btnEnviar.TabIndex = 3;
            this.btnEnviar.Text = "Pesquisar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // btxtSignSymbol
            // 
            this.btxtSignSymbol.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.btxtSignSymbol.Location = new System.Drawing.Point(262, 19);
            this.btxtSignSymbol.Name = "btxtSignSymbol";
            this.btxtSignSymbol.Padding = new System.Windows.Forms.Padding(1);
            this.btxtSignSymbol.Size = new System.Drawing.Size(125, 20);
            this.btxtSignSymbol.TabIndex = 1;
            // 
            // cmbSignSession
            // 
            this.cmbSignSession.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSignSession.FormattingEnabled = true;
            this.cmbSignSession.Items.AddRange(new object[] {
            "(Todas)"});
            this.cmbSignSession.Location = new System.Drawing.Point(71, 19);
            this.cmbSignSession.Name = "cmbSignSession";
            this.cmbSignSession.Size = new System.Drawing.Size(125, 21);
            this.cmbSignSession.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 49;
            this.label1.Text = "Sessão";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 500);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1188, 36);
            this.pnlBottom.TabIndex = 5;
            this.pnlBottom.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.açõesToolStripMenuItem,
            this.arquivoToolStripMenuItem,
            this.tsmSair});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1188, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // açõesToolStripMenuItem
            // 
            this.açõesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCancelar,
            this.tsmCancelarTudo});
            this.açõesToolStripMenuItem.Name = "açõesToolStripMenuItem";
            this.açõesToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.açõesToolStripMenuItem.Text = "Ações";
            // 
            // tsmCancelar
            // 
            this.tsmCancelar.Name = "tsmCancelar";
            this.tsmCancelar.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.tsmCancelar.Size = new System.Drawing.Size(202, 22);
            this.tsmCancelar.Text = "Cancelar";
            this.tsmCancelar.Click += new System.EventHandler(this.cancelarToolStripMenuItem_Click);
            // 
            // tsmCancelarTudo
            // 
            this.tsmCancelarTudo.Name = "tsmCancelarTudo";
            this.tsmCancelarTudo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F8)));
            this.tsmCancelarTudo.Size = new System.Drawing.Size(202, 22);
            this.tsmCancelarTudo.Text = "Cancelar Tudo";
            this.tsmCancelarTudo.Click += new System.EventHandler(this.cancelarTudoToolStripMenuItem_Click);
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmConfiguracoes});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.arquivoToolStripMenuItem.Text = "Preferências";
            // 
            // tsmConfiguracoes
            // 
            this.tsmConfiguracoes.ForeColor = System.Drawing.Color.White;
            this.tsmConfiguracoes.Name = "tsmConfiguracoes";
            this.tsmConfiguracoes.Size = new System.Drawing.Size(151, 22);
            this.tsmConfiguracoes.Tag = "SemRenderizacao";
            this.tsmConfiguracoes.Text = "Configurações";
            this.tsmConfiguracoes.Click += new System.EventHandler(this.tsmConfiguracoes_Click);
            // 
            // tsmSair
            // 
            this.tsmSair.Name = "tsmSair";
            this.tsmSair.Size = new System.Drawing.Size(38, 20);
            this.tsmSair.Text = "Sair";
            this.tsmSair.Click += new System.EventHandler(this.tsmSair_Click);
            // 
            // loadingPanel1
            // 
            this.loadingPanel1.BackgroundForm = null;
            this.loadingPanel1.Location = new System.Drawing.Point(300, 250);
            this.loadingPanel1.Name = "loadingPanel1";
            this.loadingPanel1.Progress = 0;
            this.loadingPanel1.Size = new System.Drawing.Size(661, 124);
            this.loadingPanel1.TabIndex = 54;
            this.loadingPanel1.Title = "Text Label";
            this.loadingPanel1.UseFadedBackground = false;
            this.loadingPanel1.Visible = false;
            // 
            // spiderOrderInfoBindingSource
            // 
            this.spiderOrderInfoBindingSource.DataSource = typeof(Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo);
            // 
            // spiderOrderDetailInfoBindingSource
            // 
            this.spiderOrderDetailInfoBindingSource.DataSource = typeof(Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderDetailInfo);
            // 
            // columnDetailOrderId
            // 
            this.columnDetailOrderId.Name = "columnDetailOrderId";
            // 
            // columnDetailOrderDetailID
            // 
            this.columnDetailOrderDetailID.Name = "columnDetailOrderDetailID";
            // 
            // columnDetailOrderQty
            // 
            this.columnDetailOrderQty.Name = "columnDetailOrderQty";
            // 
            // columnDetailCumQty
            // 
            this.columnDetailCumQty.Name = "columnDetailCumQty";
            // 
            // columnDetailOrderQtyRemaining
            // 
            this.columnDetailOrderQtyRemaining.Name = "columnDetailOrderQtyRemaining";
            // 
            // columnDetailOrderStatusId
            // 
            this.columnDetailOrderStatusId.Name = "columnDetailOrderStatusId";
            // 
            // columnDetailTransactTime
            // 
            this.columnDetailTransactTime.Name = "columnDetailTransactTime";
            // 
            // columnDetailPrice
            // 
            this.columnDetailPrice.Name = "columnDetailPrice";
            // 
            // columnDetailDescription
            // 
            this.columnDetailDescription.Name = "columnDetailDescription";
            // 
            // columnDetailStopPx
            // 
            this.columnDetailStopPx.Name = "columnDetailStopPx";
            // 
            // columnDetailTransactID
            // 
            this.columnDetailTransactID.Name = "columnDetailTransactID";
            // 
            // columnDetailTradeQty
            // 
            this.columnDetailTradeQty.Name = "columnDetailTradeQty";
            // 
            // columnDetailFixMsgSeqNum
            // 
            this.columnDetailFixMsgSeqNum.Name = "columnDetailFixMsgSeqNum";
            // 
            // columnDetailCxlRejResponseTo
            // 
            this.columnDetailCxlRejResponseTo.Name = "columnDetailCxlRejResponseTo";
            // 
            // columnDetailCxlRejReason
            // 
            this.columnDetailCxlRejReason.Name = "columnDetailCxlRejReason";
            // 
            // columnDetailMsgFixDetail
            // 
            this.columnDetailMsgFixDetail.Name = "columnDetailMsgFixDetail";
            // 
            // columnDetailContraBroker
            // 
            this.columnDetailContraBroker.Name = "columnDetailContraBroker";
            // 
            // tmrLoading
            // 
            this.tmrLoading.Interval = 1000;
            this.tmrLoading.Tick += new System.EventHandler(this.tmrLoading_Tick);
            // 
            // fOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Controls.Add(this.loadingPanel1);
            this.Controls.Add(this.pnlMain);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(800, 300);
            this.Name = "fOrder";
            this.Text = "..:: Mission Control ::..";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fOrder_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fOrder_FormClosed);
            this.Load += new System.EventHandler(this.fOrder_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fOrder_KeyPress);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlMiddle.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridResumo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.pnlDetailMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalhe)).EndInit();
            this.pnlDetailTop.ResumeLayout(false);
            this.pnlDetailTop.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.gbFiltro.ResumeLayout(false);
            this.gbAssinatura.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spiderOrderInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spiderOrderDetailInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlMiddle;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.BindingSource spiderOrderInfoBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private GradualForm.Controls.CustomDataGridView gridDetalhe;
        private System.Windows.Forms.BindingSource spiderOrderDetailInfoBindingSource;
        private System.Windows.Forms.Panel pnlDetailMain;
        private System.Windows.Forms.Panel pnlDetailTop;
        private System.Windows.Forms.Label lblDetalhe;
        private System.Windows.Forms.Panel panel1;
        private GradualForm.Controls.CustomDataGridView gridResumo;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Timer tmrLoading;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.GroupBox gbFiltro;
        private System.Windows.Forms.ComboBox cmbCorretora;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Label lblCorretora;
        private System.Windows.Forms.Label lblValidade;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.Label lblEstado;
        private GradualForm.Controls.BorderedTextBox txtConta;
        private System.Windows.Forms.Label lblClientOrderId;
        private System.Windows.Forms.ComboBox cmbSessao;
        private System.Windows.Forms.Label lblSimbolo;
        private System.Windows.Forms.Label lblConta;
        private System.Windows.Forms.Label lblLado;
        private System.Windows.Forms.Label lblIdSessao;
        private System.Windows.Forms.ComboBox cmbValidade;
        private System.Windows.Forms.ComboBox cmbTipo;
        private GradualForm.Controls.BorderedTextBox txtNumeroOrdem;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.ComboBox cmbLado;
        private GradualForm.Controls.BorderedTextBox txtSimbolo;
        private System.Windows.Forms.Button btnConfiguracoes;
        private System.Windows.Forms.GroupBox gbAssinatura;
        private GradualForm.Controls.BorderedTextBox btxtSignAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnEnviar;
        private GradualForm.Controls.BorderedTextBox btxtSignSymbol;
        private System.Windows.Forms.ComboBox cmbSignSession;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmConfiguracoes;
        private System.Windows.Forms.ToolStripMenuItem tsmSair;
        private GradualForm.Controls.LoadingPanel loadingPanel1;
        private CheckComboBox.CheckComboBox cComboBox;
        private System.Windows.Forms.CheckedListBox checkedListStatus;
        private System.Windows.Forms.Button btnCancelarTudo;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.ToolStripMenuItem açõesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmCancelar;
        private System.Windows.Forms.ToolStripMenuItem tsmCancelarTudo;

    }
}
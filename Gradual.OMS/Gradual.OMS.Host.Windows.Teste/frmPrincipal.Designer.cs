namespace Gradual.OMS.Host.Windows.Teste
{
    partial class frmPrincipal
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
            this.tab = new System.Windows.Forms.TabControl();
            this.tabEnviarMensagem = new System.Windows.Forms.TabPage();
            this.ppg = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lstMensagem = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmdEnviarMensagem = new System.Windows.Forms.Button();
            this.tabMensagens = new System.Windows.Forms.TabPage();
            this.grdMensagens = new System.Windows.Forms.DataGridView();
            this.tabInstrumentos = new System.Windows.Forms.TabPage();
            this.grdInstrumentos = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAtualizaLista = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdListarInstrumentos = new System.Windows.Forms.ToolStripMenuItem();
            this.canaisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdCanaisIniciar = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdCanaisParar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdSair = new System.Windows.Forms.ToolStripMenuItem();
            this.testeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatusCanais = new System.Windows.Forms.ToolStripStatusLabel();
            this.popupMensagens = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.popupMensagensCriarMensagem = new System.Windows.Forms.ToolStripMenuItem();
            this.popupInstrumentos = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.popupInstrumentosEnviarOrdem = new System.Windows.Forms.ToolStripMenuItem();
            this.tab.SuspendLayout();
            this.tabEnviarMensagem.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabMensagens.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMensagens)).BeginInit();
            this.tabInstrumentos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInstrumentos)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.popupMensagens.SuspendLayout();
            this.popupInstrumentos.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabEnviarMensagem);
            this.tab.Controls.Add(this.tabMensagens);
            this.tab.Controls.Add(this.tabInstrumentos);
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 24);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(991, 452);
            this.tab.TabIndex = 2;
            // 
            // tabEnviarMensagem
            // 
            this.tabEnviarMensagem.Controls.Add(this.ppg);
            this.tabEnviarMensagem.Controls.Add(this.splitter1);
            this.tabEnviarMensagem.Controls.Add(this.panel3);
            this.tabEnviarMensagem.Location = new System.Drawing.Point(4, 22);
            this.tabEnviarMensagem.Name = "tabEnviarMensagem";
            this.tabEnviarMensagem.Padding = new System.Windows.Forms.Padding(3);
            this.tabEnviarMensagem.Size = new System.Drawing.Size(983, 426);
            this.tabEnviarMensagem.TabIndex = 1;
            this.tabEnviarMensagem.Text = "Enviar Mensagem";
            this.tabEnviarMensagem.UseVisualStyleBackColor = true;
            // 
            // ppg
            // 
            this.ppg.Location = new System.Drawing.Point(562, 55);
            this.ppg.Name = "ppg";
            this.ppg.Size = new System.Drawing.Size(589, 420);
            this.ppg.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(388, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 420);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lstMensagem);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(385, 420);
            this.panel3.TabIndex = 0;
            // 
            // lstMensagem
            // 
            this.lstMensagem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMensagem.FormattingEnabled = true;
            this.lstMensagem.Location = new System.Drawing.Point(0, 0);
            this.lstMensagem.Name = "lstMensagem";
            this.lstMensagem.Size = new System.Drawing.Size(385, 368);
            this.lstMensagem.TabIndex = 1;
            this.lstMensagem.SelectedIndexChanged += new System.EventHandler(this.lstMensagem_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cmdEnviarMensagem);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 377);
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
            this.cmdEnviarMensagem.Click += new System.EventHandler(this.cmdEnviarMensagem_Click);
            // 
            // tabMensagens
            // 
            this.tabMensagens.Controls.Add(this.grdMensagens);
            this.tabMensagens.Location = new System.Drawing.Point(4, 22);
            this.tabMensagens.Name = "tabMensagens";
            this.tabMensagens.Size = new System.Drawing.Size(983, 426);
            this.tabMensagens.TabIndex = 3;
            this.tabMensagens.Text = "Mensagens";
            this.tabMensagens.UseVisualStyleBackColor = true;
            // 
            // grdMensagens
            // 
            this.grdMensagens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMensagens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMensagens.Location = new System.Drawing.Point(0, 0);
            this.grdMensagens.Name = "grdMensagens";
            this.grdMensagens.ReadOnly = true;
            this.grdMensagens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdMensagens.Size = new System.Drawing.Size(983, 426);
            this.grdMensagens.TabIndex = 0;
            this.grdMensagens.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdMensagensEnviadas_MouseDown);
            this.grdMensagens.DoubleClick += new System.EventHandler(this.grdMensagens_DoubleClick);
            // 
            // tabInstrumentos
            // 
            this.tabInstrumentos.Controls.Add(this.grdInstrumentos);
            this.tabInstrumentos.Location = new System.Drawing.Point(4, 22);
            this.tabInstrumentos.Name = "tabInstrumentos";
            this.tabInstrumentos.Padding = new System.Windows.Forms.Padding(3);
            this.tabInstrumentos.Size = new System.Drawing.Size(983, 426);
            this.tabInstrumentos.TabIndex = 2;
            this.tabInstrumentos.Text = "Lista de Instrumentos";
            this.tabInstrumentos.UseVisualStyleBackColor = true;
            // 
            // grdInstrumentos
            // 
            this.grdInstrumentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInstrumentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdInstrumentos.Location = new System.Drawing.Point(3, 3);
            this.grdInstrumentos.Name = "grdInstrumentos";
            this.grdInstrumentos.ReadOnly = true;
            this.grdInstrumentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdInstrumentos.Size = new System.Drawing.Size(977, 420);
            this.grdInstrumentos.TabIndex = 0;
            this.grdInstrumentos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdInstrumentos_MouseDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(991, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAtualizaLista,
            this.cmdListarInstrumentos,
            this.canaisToolStripMenuItem,
            this.toolStripMenuItem1,
            this.cmdSair,
            this.testeToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.arquivoToolStripMenuItem.Text = "Arquivo";
            // 
            // mnuAtualizaLista
            // 
            this.mnuAtualizaLista.Name = "mnuAtualizaLista";
            this.mnuAtualizaLista.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mnuAtualizaLista.Size = new System.Drawing.Size(197, 22);
            this.mnuAtualizaLista.Text = "Atualiza Lista";
            this.mnuAtualizaLista.Click += new System.EventHandler(this.mnuAtualizaLista_Click);
            // 
            // cmdListarInstrumentos
            // 
            this.cmdListarInstrumentos.Name = "cmdListarInstrumentos";
            this.cmdListarInstrumentos.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.cmdListarInstrumentos.Size = new System.Drawing.Size(197, 22);
            this.cmdListarInstrumentos.Text = "Listar Instrumentos";
            this.cmdListarInstrumentos.Click += new System.EventHandler(this.cmdListarInstrumentos_Click);
            // 
            // canaisToolStripMenuItem
            // 
            this.canaisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdCanaisIniciar,
            this.cmdCanaisParar});
            this.canaisToolStripMenuItem.Name = "canaisToolStripMenuItem";
            this.canaisToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.canaisToolStripMenuItem.Text = "Canais";
            // 
            // cmdCanaisIniciar
            // 
            this.cmdCanaisIniciar.Name = "cmdCanaisIniciar";
            this.cmdCanaisIniciar.Size = new System.Drawing.Size(114, 22);
            this.cmdCanaisIniciar.Text = "Iniciar";
            this.cmdCanaisIniciar.Click += new System.EventHandler(this.cmdCanaisIniciar_Click);
            // 
            // cmdCanaisParar
            // 
            this.cmdCanaisParar.Name = "cmdCanaisParar";
            this.cmdCanaisParar.Size = new System.Drawing.Size(114, 22);
            this.cmdCanaisParar.Text = "Parar";
            this.cmdCanaisParar.Click += new System.EventHandler(this.cmdCanaisParar_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(194, 6);
            // 
            // cmdSair
            // 
            this.cmdSair.Name = "cmdSair";
            this.cmdSair.Size = new System.Drawing.Size(197, 22);
            this.cmdSair.Text = "Sair";
            this.cmdSair.Click += new System.EventHandler(this.cmdSair_Click);
            // 
            // testeToolStripMenuItem
            // 
            this.testeToolStripMenuItem.Name = "testeToolStripMenuItem";
            this.testeToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.testeToolStripMenuItem.Text = "Teste";
            this.testeToolStripMenuItem.Click += new System.EventHandler(this.testeToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusCanais});
            this.statusStrip1.Location = new System.Drawing.Point(0, 476);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(991, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatusCanais
            // 
            this.lblStatusCanais.Name = "lblStatusCanais";
            this.lblStatusCanais.Size = new System.Drawing.Size(19, 17);
            this.lblStatusCanais.Text = "...";
            // 
            // popupMensagens
            // 
            this.popupMensagens.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.popupMensagensCriarMensagem});
            this.popupMensagens.Name = "popupMensagens";
            this.popupMensagens.Size = new System.Drawing.Size(163, 26);
            // 
            // popupMensagensCriarMensagem
            // 
            this.popupMensagensCriarMensagem.Name = "popupMensagensCriarMensagem";
            this.popupMensagensCriarMensagem.Size = new System.Drawing.Size(162, 22);
            this.popupMensagensCriarMensagem.Text = "Criar Mensagem";
            this.popupMensagensCriarMensagem.Click += new System.EventHandler(this.popupMensagensCriarMensagem_Click);
            // 
            // popupInstrumentos
            // 
            this.popupInstrumentos.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.popupInstrumentosEnviarOrdem});
            this.popupInstrumentos.Name = "popupInstrumentos";
            this.popupInstrumentos.Size = new System.Drawing.Size(151, 26);
            // 
            // popupInstrumentosEnviarOrdem
            // 
            this.popupInstrumentosEnviarOrdem.Name = "popupInstrumentosEnviarOrdem";
            this.popupInstrumentosEnviarOrdem.Size = new System.Drawing.Size(150, 22);
            this.popupInstrumentosEnviarOrdem.Text = "Enviar Ordem";
            this.popupInstrumentosEnviarOrdem.Click += new System.EventHandler(this.popupInstrumentosEnviarOrdem_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 498);
            this.Controls.Add(this.tab);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "frmPrincipal";
            this.Text = "OMS - Gradual";
            this.tab.ResumeLayout(false);
            this.tabEnviarMensagem.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabMensagens.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMensagens)).EndInit();
            this.tabInstrumentos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInstrumentos)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.popupMensagens.ResumeLayout(false);
            this.popupInstrumentos.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabEnviarMensagem;
        private System.Windows.Forms.PropertyGrid ppg;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TabPage tabInstrumentos;
        private System.Windows.Forms.ListBox lstMensagem;
        private System.Windows.Forms.Button cmdEnviarMensagem;
        private System.Windows.Forms.DataGridView grdInstrumentos;
        private System.Windows.Forms.TabPage tabMensagens;
        private System.Windows.Forms.DataGridView grdMensagens;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmdSair;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem cmdListarInstrumentos;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem canaisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmdCanaisIniciar;
        private System.Windows.Forms.ToolStripMenuItem cmdCanaisParar;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusCanais;
        private System.Windows.Forms.ContextMenuStrip popupMensagens;
        private System.Windows.Forms.ToolStripMenuItem popupMensagensCriarMensagem;
        private System.Windows.Forms.ContextMenuStrip popupInstrumentos;
        private System.Windows.Forms.ToolStripMenuItem popupInstrumentosEnviarOrdem;
        private System.Windows.Forms.ToolStripMenuItem mnuAtualizaLista;
        private System.Windows.Forms.ToolStripMenuItem testeToolStripMenuItem;
    }
}
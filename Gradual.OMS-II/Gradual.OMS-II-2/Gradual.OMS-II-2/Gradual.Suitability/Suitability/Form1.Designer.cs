namespace Suitability
{
    partial class Form1
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
            this.btnCarregarListaClientesSuitability = new System.Windows.Forms.Button();
            this.btnObterListaExclusao = new System.Windows.Forms.Button();
            this.btnMontarForaPerfil = new System.Windows.Forms.Button();
            this.btnVerificarBovespa = new System.Windows.Forms.Button();
            this.btnVerificarBMF = new System.Windows.Forms.Button();
            this.btnVerificarBTC = new System.Windows.Forms.Button();
            this.btnGerarNotificacoes = new System.Windows.Forms.Button();
            this.btnLimparTemporarios = new System.Windows.Forms.Button();
            this.btnObterPosicaoFundos = new System.Windows.Forms.Button();
            this.btnObterPosicaoFundosItau = new System.Windows.Forms.Button();
            this.grdFinancial = new System.Windows.Forms.DataGridView();
            this.idPosicaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idOperacaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idCotistaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idCarteiraDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorAplicacaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataAplicacaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataConversaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cotaAplicacaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cotaDiaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorBrutoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorLiquidoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidadeInicialDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidadeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidadeBloqueadaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataUltimaCobrancaIRDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorIRDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorIOFDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorPerformanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorIOFVirtualDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidadeAntesCortesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorRendimentoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataUltimoCortePfeeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.posicaoIncorporadaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codigoAnbimaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.posicaoCotistaFinancialBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnVerificarSegunda = new System.Windows.Forms.Button();
            this.btnGerarNotificacoesErro = new System.Windows.Forms.Button();
            this.btnObterListadePara = new System.Windows.Forms.Button();
            this.btnVerificarFundosItau = new System.Windows.Forms.Button();
            this.btnVerificarFundosFinancial = new System.Windows.Forms.Button();
            this.btnObterListaFundos = new System.Windows.Forms.Button();
            this.bsFinancial = new System.Windows.Forms.BindingSource(this.components);
            this.btnExclusao = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdFinancial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posicaoCotistaFinancialBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsFinancial)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCarregarListaClientesSuitability
            // 
            this.btnCarregarListaClientesSuitability.Location = new System.Drawing.Point(3, 3);
            this.btnCarregarListaClientesSuitability.Name = "btnCarregarListaClientesSuitability";
            this.btnCarregarListaClientesSuitability.Size = new System.Drawing.Size(242, 23);
            this.btnCarregarListaClientesSuitability.TabIndex = 0;
            this.btnCarregarListaClientesSuitability.Text = "Carregar Lista Clientes Suitability";
            this.btnCarregarListaClientesSuitability.UseVisualStyleBackColor = true;
            this.btnCarregarListaClientesSuitability.Click += new System.EventHandler(this.btnCarregarListaClientesSuitability_Click);
            // 
            // btnObterListaExclusao
            // 
            this.btnObterListaExclusao.Location = new System.Drawing.Point(3, 60);
            this.btnObterListaExclusao.Name = "btnObterListaExclusao";
            this.btnObterListaExclusao.Size = new System.Drawing.Size(242, 23);
            this.btnObterListaExclusao.TabIndex = 1;
            this.btnObterListaExclusao.Text = "Obter Lista exclusao";
            this.btnObterListaExclusao.UseVisualStyleBackColor = true;
            this.btnObterListaExclusao.Click += new System.EventHandler(this.btnObterListaExclusao_Click);
            // 
            // btnMontarForaPerfil
            // 
            this.btnMontarForaPerfil.Location = new System.Drawing.Point(3, 240);
            this.btnMontarForaPerfil.Name = "btnMontarForaPerfil";
            this.btnMontarForaPerfil.Size = new System.Drawing.Size(242, 23);
            this.btnMontarForaPerfil.TabIndex = 2;
            this.btnMontarForaPerfil.Text = "Montar Fora Perfil";
            this.btnMontarForaPerfil.UseVisualStyleBackColor = true;
            this.btnMontarForaPerfil.Click += new System.EventHandler(this.btnMontarForaPerfil_Click);
            // 
            // btnVerificarBovespa
            // 
            this.btnVerificarBovespa.Location = new System.Drawing.Point(3, 270);
            this.btnVerificarBovespa.Name = "btnVerificarBovespa";
            this.btnVerificarBovespa.Size = new System.Drawing.Size(242, 23);
            this.btnVerificarBovespa.TabIndex = 3;
            this.btnVerificarBovespa.Text = "Verificar Bovespa";
            this.btnVerificarBovespa.UseVisualStyleBackColor = true;
            this.btnVerificarBovespa.Click += new System.EventHandler(this.btnVerificarBovespa_Click);
            // 
            // btnVerificarBMF
            // 
            this.btnVerificarBMF.Location = new System.Drawing.Point(3, 299);
            this.btnVerificarBMF.Name = "btnVerificarBMF";
            this.btnVerificarBMF.Size = new System.Drawing.Size(242, 23);
            this.btnVerificarBMF.TabIndex = 4;
            this.btnVerificarBMF.Text = "Verificar BMF";
            this.btnVerificarBMF.UseVisualStyleBackColor = true;
            this.btnVerificarBMF.Click += new System.EventHandler(this.btnVerificarBMF_Click);
            // 
            // btnVerificarBTC
            // 
            this.btnVerificarBTC.Location = new System.Drawing.Point(3, 328);
            this.btnVerificarBTC.Name = "btnVerificarBTC";
            this.btnVerificarBTC.Size = new System.Drawing.Size(242, 23);
            this.btnVerificarBTC.TabIndex = 5;
            this.btnVerificarBTC.Text = "Verificar BTC";
            this.btnVerificarBTC.UseVisualStyleBackColor = true;
            this.btnVerificarBTC.Click += new System.EventHandler(this.btnVerificarBTC_Click);
            // 
            // btnGerarNotificacoes
            // 
            this.btnGerarNotificacoes.Location = new System.Drawing.Point(3, 411);
            this.btnGerarNotificacoes.Name = "btnGerarNotificacoes";
            this.btnGerarNotificacoes.Size = new System.Drawing.Size(242, 23);
            this.btnGerarNotificacoes.TabIndex = 6;
            this.btnGerarNotificacoes.Text = "Gerar Notificacoes";
            this.btnGerarNotificacoes.UseVisualStyleBackColor = true;
            this.btnGerarNotificacoes.Click += new System.EventHandler(this.btnGerarNotificacoes_Click);
            // 
            // btnLimparTemporarios
            // 
            this.btnLimparTemporarios.Location = new System.Drawing.Point(3, 469);
            this.btnLimparTemporarios.Name = "btnLimparTemporarios";
            this.btnLimparTemporarios.Size = new System.Drawing.Size(242, 23);
            this.btnLimparTemporarios.TabIndex = 7;
            this.btnLimparTemporarios.Text = "Limpar Temporários";
            this.btnLimparTemporarios.UseVisualStyleBackColor = true;
            this.btnLimparTemporarios.Click += new System.EventHandler(this.btnLimparTemporarios_Click);
            // 
            // btnObterPosicaoFundos
            // 
            this.btnObterPosicaoFundos.Location = new System.Drawing.Point(3, 211);
            this.btnObterPosicaoFundos.Name = "btnObterPosicaoFundos";
            this.btnObterPosicaoFundos.Size = new System.Drawing.Size(242, 23);
            this.btnObterPosicaoFundos.TabIndex = 8;
            this.btnObterPosicaoFundos.Text = "Obter Posicao Fundos Financial";
            this.btnObterPosicaoFundos.UseVisualStyleBackColor = true;
            this.btnObterPosicaoFundos.Click += new System.EventHandler(this.btnObterPosicaoFundos_Click);
            // 
            // btnObterPosicaoFundosItau
            // 
            this.btnObterPosicaoFundosItau.Location = new System.Drawing.Point(3, 182);
            this.btnObterPosicaoFundosItau.Name = "btnObterPosicaoFundosItau";
            this.btnObterPosicaoFundosItau.Size = new System.Drawing.Size(242, 23);
            this.btnObterPosicaoFundosItau.TabIndex = 9;
            this.btnObterPosicaoFundosItau.Text = "Obter Posicao Fundos Itau";
            this.btnObterPosicaoFundosItau.UseVisualStyleBackColor = true;
            this.btnObterPosicaoFundosItau.Click += new System.EventHandler(this.btnObterPosicaoFundosItau_Click);
            // 
            // grdFinancial
            // 
            this.grdFinancial.AllowUserToAddRows = false;
            this.grdFinancial.AutoGenerateColumns = false;
            this.grdFinancial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFinancial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idPosicaoDataGridViewTextBoxColumn,
            this.idOperacaoDataGridViewTextBoxColumn,
            this.idCotistaDataGridViewTextBoxColumn,
            this.idCarteiraDataGridViewTextBoxColumn,
            this.valorAplicacaoDataGridViewTextBoxColumn,
            this.dataAplicacaoDataGridViewTextBoxColumn,
            this.dataConversaoDataGridViewTextBoxColumn,
            this.cotaAplicacaoDataGridViewTextBoxColumn,
            this.cotaDiaDataGridViewTextBoxColumn,
            this.valorBrutoDataGridViewTextBoxColumn,
            this.valorLiquidoDataGridViewTextBoxColumn,
            this.quantidadeInicialDataGridViewTextBoxColumn,
            this.quantidadeDataGridViewTextBoxColumn,
            this.quantidadeBloqueadaDataGridViewTextBoxColumn,
            this.dataUltimaCobrancaIRDataGridViewTextBoxColumn,
            this.valorIRDataGridViewTextBoxColumn,
            this.valorIOFDataGridViewTextBoxColumn,
            this.valorPerformanceDataGridViewTextBoxColumn,
            this.valorIOFVirtualDataGridViewTextBoxColumn,
            this.quantidadeAntesCortesDataGridViewTextBoxColumn,
            this.valorRendimentoDataGridViewTextBoxColumn,
            this.dataUltimoCortePfeeDataGridViewTextBoxColumn,
            this.posicaoIncorporadaDataGridViewTextBoxColumn,
            this.codigoAnbimaDataGridViewTextBoxColumn});
            this.grdFinancial.DataSource = this.posicaoCotistaFinancialBindingSource;
            this.grdFinancial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFinancial.Location = new System.Drawing.Point(0, 0);
            this.grdFinancial.Name = "grdFinancial";
            this.grdFinancial.Size = new System.Drawing.Size(774, 534);
            this.grdFinancial.TabIndex = 10;
            // 
            // idPosicaoDataGridViewTextBoxColumn
            // 
            this.idPosicaoDataGridViewTextBoxColumn.DataPropertyName = "IdPosicao";
            this.idPosicaoDataGridViewTextBoxColumn.HeaderText = "IdPosicao";
            this.idPosicaoDataGridViewTextBoxColumn.Name = "idPosicaoDataGridViewTextBoxColumn";
            // 
            // idOperacaoDataGridViewTextBoxColumn
            // 
            this.idOperacaoDataGridViewTextBoxColumn.DataPropertyName = "IdOperacao";
            this.idOperacaoDataGridViewTextBoxColumn.HeaderText = "IdOperacao";
            this.idOperacaoDataGridViewTextBoxColumn.Name = "idOperacaoDataGridViewTextBoxColumn";
            // 
            // idCotistaDataGridViewTextBoxColumn
            // 
            this.idCotistaDataGridViewTextBoxColumn.DataPropertyName = "IdCotista";
            this.idCotistaDataGridViewTextBoxColumn.HeaderText = "IdCotista";
            this.idCotistaDataGridViewTextBoxColumn.Name = "idCotistaDataGridViewTextBoxColumn";
            // 
            // idCarteiraDataGridViewTextBoxColumn
            // 
            this.idCarteiraDataGridViewTextBoxColumn.DataPropertyName = "IdCarteira";
            this.idCarteiraDataGridViewTextBoxColumn.HeaderText = "IdCarteira";
            this.idCarteiraDataGridViewTextBoxColumn.Name = "idCarteiraDataGridViewTextBoxColumn";
            // 
            // valorAplicacaoDataGridViewTextBoxColumn
            // 
            this.valorAplicacaoDataGridViewTextBoxColumn.DataPropertyName = "ValorAplicacao";
            this.valorAplicacaoDataGridViewTextBoxColumn.HeaderText = "ValorAplicacao";
            this.valorAplicacaoDataGridViewTextBoxColumn.Name = "valorAplicacaoDataGridViewTextBoxColumn";
            // 
            // dataAplicacaoDataGridViewTextBoxColumn
            // 
            this.dataAplicacaoDataGridViewTextBoxColumn.DataPropertyName = "DataAplicacao";
            this.dataAplicacaoDataGridViewTextBoxColumn.HeaderText = "DataAplicacao";
            this.dataAplicacaoDataGridViewTextBoxColumn.Name = "dataAplicacaoDataGridViewTextBoxColumn";
            // 
            // dataConversaoDataGridViewTextBoxColumn
            // 
            this.dataConversaoDataGridViewTextBoxColumn.DataPropertyName = "DataConversao";
            this.dataConversaoDataGridViewTextBoxColumn.HeaderText = "DataConversao";
            this.dataConversaoDataGridViewTextBoxColumn.Name = "dataConversaoDataGridViewTextBoxColumn";
            // 
            // cotaAplicacaoDataGridViewTextBoxColumn
            // 
            this.cotaAplicacaoDataGridViewTextBoxColumn.DataPropertyName = "CotaAplicacao";
            this.cotaAplicacaoDataGridViewTextBoxColumn.HeaderText = "CotaAplicacao";
            this.cotaAplicacaoDataGridViewTextBoxColumn.Name = "cotaAplicacaoDataGridViewTextBoxColumn";
            // 
            // cotaDiaDataGridViewTextBoxColumn
            // 
            this.cotaDiaDataGridViewTextBoxColumn.DataPropertyName = "CotaDia";
            this.cotaDiaDataGridViewTextBoxColumn.HeaderText = "CotaDia";
            this.cotaDiaDataGridViewTextBoxColumn.Name = "cotaDiaDataGridViewTextBoxColumn";
            // 
            // valorBrutoDataGridViewTextBoxColumn
            // 
            this.valorBrutoDataGridViewTextBoxColumn.DataPropertyName = "ValorBruto";
            this.valorBrutoDataGridViewTextBoxColumn.HeaderText = "ValorBruto";
            this.valorBrutoDataGridViewTextBoxColumn.Name = "valorBrutoDataGridViewTextBoxColumn";
            // 
            // valorLiquidoDataGridViewTextBoxColumn
            // 
            this.valorLiquidoDataGridViewTextBoxColumn.DataPropertyName = "ValorLiquido";
            this.valorLiquidoDataGridViewTextBoxColumn.HeaderText = "ValorLiquido";
            this.valorLiquidoDataGridViewTextBoxColumn.Name = "valorLiquidoDataGridViewTextBoxColumn";
            // 
            // quantidadeInicialDataGridViewTextBoxColumn
            // 
            this.quantidadeInicialDataGridViewTextBoxColumn.DataPropertyName = "QuantidadeInicial";
            this.quantidadeInicialDataGridViewTextBoxColumn.HeaderText = "QuantidadeInicial";
            this.quantidadeInicialDataGridViewTextBoxColumn.Name = "quantidadeInicialDataGridViewTextBoxColumn";
            // 
            // quantidadeDataGridViewTextBoxColumn
            // 
            this.quantidadeDataGridViewTextBoxColumn.DataPropertyName = "Quantidade";
            this.quantidadeDataGridViewTextBoxColumn.HeaderText = "Quantidade";
            this.quantidadeDataGridViewTextBoxColumn.Name = "quantidadeDataGridViewTextBoxColumn";
            // 
            // quantidadeBloqueadaDataGridViewTextBoxColumn
            // 
            this.quantidadeBloqueadaDataGridViewTextBoxColumn.DataPropertyName = "QuantidadeBloqueada";
            this.quantidadeBloqueadaDataGridViewTextBoxColumn.HeaderText = "QuantidadeBloqueada";
            this.quantidadeBloqueadaDataGridViewTextBoxColumn.Name = "quantidadeBloqueadaDataGridViewTextBoxColumn";
            // 
            // dataUltimaCobrancaIRDataGridViewTextBoxColumn
            // 
            this.dataUltimaCobrancaIRDataGridViewTextBoxColumn.DataPropertyName = "DataUltimaCobrancaIR";
            this.dataUltimaCobrancaIRDataGridViewTextBoxColumn.HeaderText = "DataUltimaCobrancaIR";
            this.dataUltimaCobrancaIRDataGridViewTextBoxColumn.Name = "dataUltimaCobrancaIRDataGridViewTextBoxColumn";
            // 
            // valorIRDataGridViewTextBoxColumn
            // 
            this.valorIRDataGridViewTextBoxColumn.DataPropertyName = "ValorIR";
            this.valorIRDataGridViewTextBoxColumn.HeaderText = "ValorIR";
            this.valorIRDataGridViewTextBoxColumn.Name = "valorIRDataGridViewTextBoxColumn";
            // 
            // valorIOFDataGridViewTextBoxColumn
            // 
            this.valorIOFDataGridViewTextBoxColumn.DataPropertyName = "ValorIOF";
            this.valorIOFDataGridViewTextBoxColumn.HeaderText = "ValorIOF";
            this.valorIOFDataGridViewTextBoxColumn.Name = "valorIOFDataGridViewTextBoxColumn";
            // 
            // valorPerformanceDataGridViewTextBoxColumn
            // 
            this.valorPerformanceDataGridViewTextBoxColumn.DataPropertyName = "ValorPerformance";
            this.valorPerformanceDataGridViewTextBoxColumn.HeaderText = "ValorPerformance";
            this.valorPerformanceDataGridViewTextBoxColumn.Name = "valorPerformanceDataGridViewTextBoxColumn";
            // 
            // valorIOFVirtualDataGridViewTextBoxColumn
            // 
            this.valorIOFVirtualDataGridViewTextBoxColumn.DataPropertyName = "ValorIOFVirtual";
            this.valorIOFVirtualDataGridViewTextBoxColumn.HeaderText = "ValorIOFVirtual";
            this.valorIOFVirtualDataGridViewTextBoxColumn.Name = "valorIOFVirtualDataGridViewTextBoxColumn";
            // 
            // quantidadeAntesCortesDataGridViewTextBoxColumn
            // 
            this.quantidadeAntesCortesDataGridViewTextBoxColumn.DataPropertyName = "QuantidadeAntesCortes";
            this.quantidadeAntesCortesDataGridViewTextBoxColumn.HeaderText = "QuantidadeAntesCortes";
            this.quantidadeAntesCortesDataGridViewTextBoxColumn.Name = "quantidadeAntesCortesDataGridViewTextBoxColumn";
            // 
            // valorRendimentoDataGridViewTextBoxColumn
            // 
            this.valorRendimentoDataGridViewTextBoxColumn.DataPropertyName = "ValorRendimento";
            this.valorRendimentoDataGridViewTextBoxColumn.HeaderText = "ValorRendimento";
            this.valorRendimentoDataGridViewTextBoxColumn.Name = "valorRendimentoDataGridViewTextBoxColumn";
            // 
            // dataUltimoCortePfeeDataGridViewTextBoxColumn
            // 
            this.dataUltimoCortePfeeDataGridViewTextBoxColumn.DataPropertyName = "DataUltimoCortePfee";
            this.dataUltimoCortePfeeDataGridViewTextBoxColumn.HeaderText = "DataUltimoCortePfee";
            this.dataUltimoCortePfeeDataGridViewTextBoxColumn.Name = "dataUltimoCortePfeeDataGridViewTextBoxColumn";
            // 
            // posicaoIncorporadaDataGridViewTextBoxColumn
            // 
            this.posicaoIncorporadaDataGridViewTextBoxColumn.DataPropertyName = "PosicaoIncorporada";
            this.posicaoIncorporadaDataGridViewTextBoxColumn.HeaderText = "PosicaoIncorporada";
            this.posicaoIncorporadaDataGridViewTextBoxColumn.Name = "posicaoIncorporadaDataGridViewTextBoxColumn";
            // 
            // codigoAnbimaDataGridViewTextBoxColumn
            // 
            this.codigoAnbimaDataGridViewTextBoxColumn.DataPropertyName = "CodigoAnbima";
            this.codigoAnbimaDataGridViewTextBoxColumn.HeaderText = "CodigoAnbima";
            this.codigoAnbimaDataGridViewTextBoxColumn.Name = "codigoAnbimaDataGridViewTextBoxColumn";
            // 
            // posicaoCotistaFinancialBindingSource
            // 
            this.posicaoCotistaFinancialBindingSource.DataSource = typeof(Gradual.Suitability.Service.Objetos.PosicaoCotistaFinancial);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnExclusao);
            this.splitContainer1.Panel1.Controls.Add(this.btnVerificarSegunda);
            this.splitContainer1.Panel1.Controls.Add(this.btnGerarNotificacoesErro);
            this.splitContainer1.Panel1.Controls.Add(this.btnObterListadePara);
            this.splitContainer1.Panel1.Controls.Add(this.btnVerificarFundosItau);
            this.splitContainer1.Panel1.Controls.Add(this.btnVerificarFundosFinancial);
            this.splitContainer1.Panel1.Controls.Add(this.btnObterListaFundos);
            this.splitContainer1.Panel1.Controls.Add(this.btnCarregarListaClientesSuitability);
            this.splitContainer1.Panel1.Controls.Add(this.btnObterListaExclusao);
            this.splitContainer1.Panel1.Controls.Add(this.btnObterPosicaoFundosItau);
            this.splitContainer1.Panel1.Controls.Add(this.btnMontarForaPerfil);
            this.splitContainer1.Panel1.Controls.Add(this.btnObterPosicaoFundos);
            this.splitContainer1.Panel1.Controls.Add(this.btnVerificarBovespa);
            this.splitContainer1.Panel1.Controls.Add(this.btnLimparTemporarios);
            this.splitContainer1.Panel1.Controls.Add(this.btnVerificarBMF);
            this.splitContainer1.Panel1.Controls.Add(this.btnGerarNotificacoes);
            this.splitContainer1.Panel1.Controls.Add(this.btnVerificarBTC);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdFinancial);
            this.splitContainer1.Size = new System.Drawing.Size(1031, 534);
            this.splitContainer1.SplitterDistance = 253;
            this.splitContainer1.TabIndex = 11;
            // 
            // btnVerificarSegunda
            // 
            this.btnVerificarSegunda.Location = new System.Drawing.Point(3, 508);
            this.btnVerificarSegunda.Name = "btnVerificarSegunda";
            this.btnVerificarSegunda.Size = new System.Drawing.Size(242, 23);
            this.btnVerificarSegunda.TabIndex = 15;
            this.btnVerificarSegunda.Text = "Verificar Segunda-Feira anterior";
            this.btnVerificarSegunda.UseVisualStyleBackColor = true;
            // 
            // btnGerarNotificacoesErro
            // 
            this.btnGerarNotificacoesErro.Location = new System.Drawing.Point(3, 440);
            this.btnGerarNotificacoesErro.Name = "btnGerarNotificacoesErro";
            this.btnGerarNotificacoesErro.Size = new System.Drawing.Size(242, 23);
            this.btnGerarNotificacoesErro.TabIndex = 14;
            this.btnGerarNotificacoesErro.Text = "Gerar Notificacoes Erros";
            this.btnGerarNotificacoesErro.UseVisualStyleBackColor = true;
            this.btnGerarNotificacoesErro.Click += new System.EventHandler(this.btnGerarNotificacoesErro_Click);
            // 
            // btnObterListadePara
            // 
            this.btnObterListadePara.Location = new System.Drawing.Point(3, 32);
            this.btnObterListadePara.Name = "btnObterListadePara";
            this.btnObterListadePara.Size = new System.Drawing.Size(242, 23);
            this.btnObterListadePara.TabIndex = 13;
            this.btnObterListadePara.Text = "Carregar Lista DePara";
            this.btnObterListadePara.UseVisualStyleBackColor = true;
            this.btnObterListadePara.Click += new System.EventHandler(this.btnObterListadePara_Click);
            // 
            // btnVerificarFundosItau
            // 
            this.btnVerificarFundosItau.Location = new System.Drawing.Point(3, 382);
            this.btnVerificarFundosItau.Name = "btnVerificarFundosItau";
            this.btnVerificarFundosItau.Size = new System.Drawing.Size(242, 23);
            this.btnVerificarFundosItau.TabIndex = 12;
            this.btnVerificarFundosItau.Text = "Verificar Fundos Itau";
            this.btnVerificarFundosItau.UseVisualStyleBackColor = true;
            this.btnVerificarFundosItau.Click += new System.EventHandler(this.btnVerificarFundosItau_Click);
            // 
            // btnVerificarFundosFinancial
            // 
            this.btnVerificarFundosFinancial.Location = new System.Drawing.Point(3, 355);
            this.btnVerificarFundosFinancial.Name = "btnVerificarFundosFinancial";
            this.btnVerificarFundosFinancial.Size = new System.Drawing.Size(242, 23);
            this.btnVerificarFundosFinancial.TabIndex = 11;
            this.btnVerificarFundosFinancial.Text = "Verificar Fundos Financial";
            this.btnVerificarFundosFinancial.UseVisualStyleBackColor = true;
            this.btnVerificarFundosFinancial.Click += new System.EventHandler(this.btnVerificarFundosFinancial_Click);
            // 
            // btnObterListaFundos
            // 
            this.btnObterListaFundos.Location = new System.Drawing.Point(3, 89);
            this.btnObterListaFundos.Name = "btnObterListaFundos";
            this.btnObterListaFundos.Size = new System.Drawing.Size(242, 23);
            this.btnObterListaFundos.TabIndex = 10;
            this.btnObterListaFundos.Text = "Obter Lista Fundos";
            this.btnObterListaFundos.UseVisualStyleBackColor = true;
            this.btnObterListaFundos.Click += new System.EventHandler(this.btnObterListaFundos_Click);
            // 
            // bsFinancial
            // 
            this.bsFinancial.DataSource = typeof(Gradual.Suitability.Service.Objetos.PosicaoCotistaFinancial);
            // 
            // btnExclusao
            // 
            this.btnExclusao.Location = new System.Drawing.Point(0, 118);
            this.btnExclusao.Name = "btnExclusao";
            this.btnExclusao.Size = new System.Drawing.Size(242, 23);
            this.btnExclusao.TabIndex = 16;
            this.btnExclusao.Text = "Obter Exclusao";
            this.btnExclusao.UseVisualStyleBackColor = true;
            this.btnExclusao.Click += new System.EventHandler(this.btnExclusao_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 534);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Teste automatização Suitability";
            ((System.ComponentModel.ISupportInitialize)(this.grdFinancial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posicaoCotistaFinancialBindingSource)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsFinancial)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCarregarListaClientesSuitability;
        private System.Windows.Forms.Button btnObterListaExclusao;
        private System.Windows.Forms.Button btnMontarForaPerfil;
        private System.Windows.Forms.Button btnVerificarBovespa;
        private System.Windows.Forms.Button btnVerificarBMF;
        private System.Windows.Forms.Button btnVerificarBTC;
        private System.Windows.Forms.Button btnGerarNotificacoes;
        private System.Windows.Forms.Button btnLimparTemporarios;
        private System.Windows.Forms.Button btnObterPosicaoFundos;
        private System.Windows.Forms.Button btnObterPosicaoFundosItau;
        private System.Windows.Forms.DataGridView grdFinancial;
        private System.Windows.Forms.BindingSource bsFinancial;
        private System.Windows.Forms.DataGridViewTextBoxColumn idPosicaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idOperacaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idCotistaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idCarteiraDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorAplicacaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataAplicacaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataConversaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cotaAplicacaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cotaDiaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorBrutoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorLiquidoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidadeInicialDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidadeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidadeBloqueadaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataUltimaCobrancaIRDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorIRDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorIOFDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorPerformanceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorIOFVirtualDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidadeAntesCortesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorRendimentoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataUltimoCortePfeeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn posicaoIncorporadaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigoAnbimaDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource posicaoCotistaFinancialBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnObterListaFundos;
        private System.Windows.Forms.Button btnVerificarFundosFinancial;
        private System.Windows.Forms.Button btnVerificarFundosItau;
        private System.Windows.Forms.Button btnObterListadePara;
        private System.Windows.Forms.Button btnGerarNotificacoesErro;
        private System.Windows.Forms.Button btnVerificarSegunda;
        private System.Windows.Forms.Button btnExclusao;
    }
}


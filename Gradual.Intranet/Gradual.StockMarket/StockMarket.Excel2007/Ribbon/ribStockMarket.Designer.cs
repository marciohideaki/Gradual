namespace StockMarket.Excel2007
{
    partial class ribStockMarket : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ribStockMarket()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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
            this.tabStockMarket = this.Factory.CreateRibbonTab();
            this.grpAutenticacao = this.Factory.CreateRibbonGroup();
            this.btnAutenticacao_RealizarLogin = this.Factory.CreateRibbonButton();
            this.mnuOpcoes = this.Factory.CreateRibbonMenu();
            this.mnuOpcoes_FrequenciaDeAtualizacao = this.Factory.CreateRibbonMenu();
            this.mnuOpcoes_FrequenciaDeAtualizacao_Alta = this.Factory.CreateRibbonCheckBox();
            this.mnuOpcoes_FrequenciaDeAtualizacao_Media = this.Factory.CreateRibbonCheckBox();
            this.mnuOpcoes_FrequenciaDeAtualizacao_Baixa = this.Factory.CreateRibbonCheckBox();
            this.mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa = this.Factory.CreateRibbonCheckBox();
            this.grpCotacao = this.Factory.CreateRibbonGroup();
            this.txtCotacao_Ativo = this.Factory.CreateRibbonEditBox();
            this.btngrpCotacao = this.Factory.CreateRibbonButtonGroup();
            this.btnCotacao_AdicionarCotacao = this.Factory.CreateRibbonButton();
            this.btnCotacao_AdicionarTicker = this.Factory.CreateRibbonButton();
            this.btnCotacao_AdicionarLivroDeOfertas = this.Factory.CreateRibbonButton();
            this.grpCarteiras = this.Factory.CreateRibbonGroup();
            this.lblCarteiras_ImportandoLista = this.Factory.CreateRibbonLabel();
            this.cboCarteiras_Carteiras = this.Factory.CreateRibbonDropDown();
            this.btnCarteira_ImportarAtivos = this.Factory.CreateRibbonButton();
            this.grpEstilos = this.Factory.CreateRibbonGroup();
            this.btnEstilos_Aplicar = this.Factory.CreateRibbonButton();
            this.lblMensagensDeAutenticacao = this.Factory.CreateRibbonLabel();
            this.tabStockMarket.SuspendLayout();
            this.grpAutenticacao.SuspendLayout();
            this.grpCotacao.SuspendLayout();
            this.btngrpCotacao.SuspendLayout();
            this.grpCarteiras.SuspendLayout();
            this.grpEstilos.SuspendLayout();
            // 
            // tabStockMarket
            // 
            this.tabStockMarket.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tabStockMarket.Groups.Add(this.grpAutenticacao);
            this.tabStockMarket.Groups.Add(this.grpCotacao);
            this.tabStockMarket.Groups.Add(this.grpCarteiras);
            this.tabStockMarket.Groups.Add(this.grpEstilos);
            this.tabStockMarket.Label = "Stock Market";
            this.tabStockMarket.Name = "tabStockMarket";
            // 
            // grpAutenticacao
            // 
            this.grpAutenticacao.Items.Add(this.btnAutenticacao_RealizarLogin);
            this.grpAutenticacao.Items.Add(this.mnuOpcoes);
            this.grpAutenticacao.Items.Add(this.lblMensagensDeAutenticacao);
            this.grpAutenticacao.Label = "Autenticação";
            this.grpAutenticacao.Name = "grpAutenticacao";
            // 
            // btnAutenticacao_RealizarLogin
            // 
            this.btnAutenticacao_RealizarLogin.Image = global::StockMarket.Excel2007.Properties.Resources.IconeLoginTrans;
            this.btnAutenticacao_RealizarLogin.Label = "Realizar Login";
            this.btnAutenticacao_RealizarLogin.Name = "btnAutenticacao_RealizarLogin";
            this.btnAutenticacao_RealizarLogin.ShowImage = true;
            this.btnAutenticacao_RealizarLogin.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAutenticacao_RealizarLogin_Click);
            // 
            // mnuOpcoes
            // 
            this.mnuOpcoes.Items.Add(this.mnuOpcoes_FrequenciaDeAtualizacao);
            this.mnuOpcoes.Label = "Opções";
            this.mnuOpcoes.Name = "mnuOpcoes";
            this.mnuOpcoes.Visible = false;
            // 
            // mnuOpcoes_FrequenciaDeAtualizacao
            // 
            this.mnuOpcoes_FrequenciaDeAtualizacao.Items.Add(this.mnuOpcoes_FrequenciaDeAtualizacao_Alta);
            this.mnuOpcoes_FrequenciaDeAtualizacao.Items.Add(this.mnuOpcoes_FrequenciaDeAtualizacao_Media);
            this.mnuOpcoes_FrequenciaDeAtualizacao.Items.Add(this.mnuOpcoes_FrequenciaDeAtualizacao_Baixa);
            this.mnuOpcoes_FrequenciaDeAtualizacao.Items.Add(this.mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa);
            this.mnuOpcoes_FrequenciaDeAtualizacao.Label = "Frequência de Atualização";
            this.mnuOpcoes_FrequenciaDeAtualizacao.Name = "mnuOpcoes_FrequenciaDeAtualizacao";
            this.mnuOpcoes_FrequenciaDeAtualizacao.ShowImage = true;
            // 
            // mnuOpcoes_FrequenciaDeAtualizacao_Alta
            // 
            this.mnuOpcoes_FrequenciaDeAtualizacao_Alta.Checked = true;
            this.mnuOpcoes_FrequenciaDeAtualizacao_Alta.Label = "Alta";
            this.mnuOpcoes_FrequenciaDeAtualizacao_Alta.Name = "mnuOpcoes_FrequenciaDeAtualizacao_Alta";
            this.mnuOpcoes_FrequenciaDeAtualizacao_Alta.ScreenTip = "Atualiza todas as cotações uma vez por segundo.";
            this.mnuOpcoes_FrequenciaDeAtualizacao_Alta.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.mnuOpcoes_FrequenciaDeAtualizacao_Alta_Click);
            // 
            // mnuOpcoes_FrequenciaDeAtualizacao_Media
            // 
            this.mnuOpcoes_FrequenciaDeAtualizacao_Media.Label = "Média";
            this.mnuOpcoes_FrequenciaDeAtualizacao_Media.Name = "mnuOpcoes_FrequenciaDeAtualizacao_Media";
            this.mnuOpcoes_FrequenciaDeAtualizacao_Media.ScreenTip = "Atualiza todas as cotações Uma vez a cada dez segundos.";
            this.mnuOpcoes_FrequenciaDeAtualizacao_Media.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.mnuOpcoes_FrequenciaDeAtualizacao_Media_Click);
            // 
            // mnuOpcoes_FrequenciaDeAtualizacao_Baixa
            // 
            this.mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Label = "Baixa";
            this.mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Name = "mnuOpcoes_FrequenciaDeAtualizacao_Baixa";
            this.mnuOpcoes_FrequenciaDeAtualizacao_Baixa.ScreenTip = "Atualiza todas as cotações Uma vez a cada minuto.";
            this.mnuOpcoes_FrequenciaDeAtualizacao_Baixa.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.mnuOpcoes_FrequenciaDeAtualizacao_Baixa_Click);
            // 
            // mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa
            // 
            this.mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Label = "Muito Baixa";
            this.mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Name = "mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa";
            this.mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.ScreenTip = "Atualiza todas as cotações uma vez a cada dez minutos.";
            this.mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa_Click);
            // 
            // grpCotacao
            // 
            this.grpCotacao.Items.Add(this.txtCotacao_Ativo);
            this.grpCotacao.Items.Add(this.btngrpCotacao);
            this.grpCotacao.Label = "Cotação";
            this.grpCotacao.Name = "grpCotacao";
            this.grpCotacao.Visible = false;
            // 
            // txtCotacao_Ativo
            // 
            this.txtCotacao_Ativo.KeyTip = "I";
            this.txtCotacao_Ativo.Label = "Ativo:";
            this.txtCotacao_Ativo.MaxLength = 12;
            this.txtCotacao_Ativo.Name = "txtCotacao_Ativo";
            this.txtCotacao_Ativo.ScreenTip = "Indique o instrumento para buscar cotação ou ticker";
            this.txtCotacao_Ativo.SizeString = "_PETR4_";
            this.txtCotacao_Ativo.Text = null;
            this.txtCotacao_Ativo.TextChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.txtCotacao_Instrumento_TextChanged);
            // 
            // btngrpCotacao
            // 
            this.btngrpCotacao.Items.Add(this.btnCotacao_AdicionarCotacao);
            this.btngrpCotacao.Items.Add(this.btnCotacao_AdicionarTicker);
            this.btngrpCotacao.Items.Add(this.btnCotacao_AdicionarLivroDeOfertas);
            this.btngrpCotacao.Name = "btngrpCotacao";
            // 
            // btnCotacao_AdicionarCotacao
            // 
            this.btnCotacao_AdicionarCotacao.Image = global::StockMarket.Excel2007.Properties.Resources.IconeCotacaoTrans;
            this.btnCotacao_AdicionarCotacao.Label = "Cotação";
            this.btnCotacao_AdicionarCotacao.Name = "btnCotacao_AdicionarCotacao";
            this.btnCotacao_AdicionarCotacao.ScreenTip = "Clique para adicionar à planilha dados de cotação do instrumento indicado";
            this.btnCotacao_AdicionarCotacao.ShowImage = true;
            this.btnCotacao_AdicionarCotacao.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCotacao_AdicionarCotacao_Click);
            // 
            // btnCotacao_AdicionarTicker
            // 
            this.btnCotacao_AdicionarTicker.Image = global::StockMarket.Excel2007.Properties.Resources.IconeTickerCotacaoTrans;
            this.btnCotacao_AdicionarTicker.Label = "Ticker";
            this.btnCotacao_AdicionarTicker.Name = "btnCotacao_AdicionarTicker";
            this.btnCotacao_AdicionarTicker.ScreenTip = "Clique para adicionar à planilha um Ticker de cotação rápida do instrumento indic" +
    "ado";
            this.btnCotacao_AdicionarTicker.ShowImage = true;
            this.btnCotacao_AdicionarTicker.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCotacao_AdicionarTicker_Click);
            // 
            // btnCotacao_AdicionarLivroDeOfertas
            // 
            this.btnCotacao_AdicionarLivroDeOfertas.Image = global::StockMarket.Excel2007.Properties.Resources.IconeLivroDeOfertasTrans;
            this.btnCotacao_AdicionarLivroDeOfertas.Label = "Livro de Ofertas";
            this.btnCotacao_AdicionarLivroDeOfertas.Name = "btnCotacao_AdicionarLivroDeOfertas";
            this.btnCotacao_AdicionarLivroDeOfertas.ScreenTip = "Clique para adicionar à planilha um Livro de Ofertas do instrumento indicado";
            this.btnCotacao_AdicionarLivroDeOfertas.ShowImage = true;
            this.btnCotacao_AdicionarLivroDeOfertas.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCotacao_AdicionarLivroDeOfertas_Click);
            // 
            // grpCarteiras
            // 
            this.grpCarteiras.Items.Add(this.lblCarteiras_ImportandoLista);
            this.grpCarteiras.Items.Add(this.cboCarteiras_Carteiras);
            this.grpCarteiras.Items.Add(this.btnCarteira_ImportarAtivos);
            this.grpCarteiras.Label = "Carteiras";
            this.grpCarteiras.Name = "grpCarteiras";
            this.grpCarteiras.Visible = false;
            // 
            // lblCarteiras_ImportandoLista
            // 
            this.lblCarteiras_ImportandoLista.Label = "Importando lista de carteiras, aguarde...";
            this.lblCarteiras_ImportandoLista.Name = "lblCarteiras_ImportandoLista";
            // 
            // cboCarteiras_Carteiras
            // 
            this.cboCarteiras_Carteiras.Label = "Carteira:";
            this.cboCarteiras_Carteiras.Name = "cboCarteiras_Carteiras";
            this.cboCarteiras_Carteiras.Visible = false;
            // 
            // btnCarteira_ImportarAtivos
            // 
            this.btnCarteira_ImportarAtivos.Image = global::StockMarket.Excel2007.Properties.Resources.IconeCarteirasTrans;
            this.btnCarteira_ImportarAtivos.Label = "Importar Ativos";
            this.btnCarteira_ImportarAtivos.Name = "btnCarteira_ImportarAtivos";
            this.btnCarteira_ImportarAtivos.ShowImage = true;
            this.btnCarteira_ImportarAtivos.Visible = false;
            this.btnCarteira_ImportarAtivos.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCarteira_ImportarAtivos_Click);
            // 
            // grpEstilos
            // 
            this.grpEstilos.Items.Add(this.btnEstilos_Aplicar);
            this.grpEstilos.Label = "Estilos";
            this.grpEstilos.Name = "grpEstilos";
            this.grpEstilos.Visible = false;
            // 
            // btnEstilos_Aplicar
            // 
            this.btnEstilos_Aplicar.Label = "Aplicar Estilos...";
            this.btnEstilos_Aplicar.Name = "btnEstilos_Aplicar";
            this.btnEstilos_Aplicar.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnEstilos_Aplicar_Click);
            // 
            // lblMensagensDeAutenticacao
            // 
            this.lblMensagensDeAutenticacao.Label = " ";
            this.lblMensagensDeAutenticacao.Name = "lblMensagensDeAutenticacao";
            // 
            // ribStockMarket
            // 
            this.Name = "ribStockMarket";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tabStockMarket);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ribGradualStockMarket_Load);
            this.tabStockMarket.ResumeLayout(false);
            this.tabStockMarket.PerformLayout();
            this.grpAutenticacao.ResumeLayout(false);
            this.grpAutenticacao.PerformLayout();
            this.grpCotacao.ResumeLayout(false);
            this.grpCotacao.PerformLayout();
            this.btngrpCotacao.ResumeLayout(false);
            this.btngrpCotacao.PerformLayout();
            this.grpCarteiras.ResumeLayout(false);
            this.grpCarteiras.PerformLayout();
            this.grpEstilos.ResumeLayout(false);
            this.grpEstilos.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabStockMarket;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpAutenticacao;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAutenticacao_RealizarLogin;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpCotacao;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox txtCotacao_Ativo;
        internal Microsoft.Office.Tools.Ribbon.RibbonButtonGroup btngrpCotacao;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCotacao_AdicionarCotacao;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCotacao_AdicionarTicker;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCotacao_AdicionarLivroDeOfertas;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpCarteiras;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown cboCarteiras_Carteiras;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCarteira_ImportarAtivos;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu mnuOpcoes;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu mnuOpcoes_FrequenciaDeAtualizacao;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox mnuOpcoes_FrequenciaDeAtualizacao_Alta;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox mnuOpcoes_FrequenciaDeAtualizacao_Media;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox mnuOpcoes_FrequenciaDeAtualizacao_Baixa;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox mnuOpcoes_FrequenciaDeAtualizacao_MuitoBaixa;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblCarteiras_ImportandoLista;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpEstilos;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnEstilos_Aplicar;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblMensagensDeAutenticacao;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal ribStockMarket ribStockMarket
        {
            get { return this.GetRibbon<ribStockMarket>(); }
        }
    }
}

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Ordens
{
    partial class OperacaoDetalheHistorico
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperacaoDetalheHistorico));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.grd = new DevExpress.XtraGrid.GridControl();
            this.grdv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCodigoBolsa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colClOrdID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrigClOrdID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrderID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSecondaryOrderID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCrossID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExecID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExecType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrdStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBTSFinalTxOrdStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrdType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrdRejReason = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSide = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAvgPx = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLastQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLastPx = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLeavesQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCumQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrigOrdQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTimeInForce = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTradeDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUniqueTradeId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAccount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colText = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExecRefID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExecTransType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrderQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStopPx = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLastShares = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMinQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMaxFloor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTransactTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSymbol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSecurityID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSecurityIDSource = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSecurityExchange = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.cmdConfigurar = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.layoutControl1.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = true;
            this.layoutControl1.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText;
            this.layoutControl1.Appearance.DisabledLayoutItem.Options.UseForeColor = true;
            this.layoutControl1.Controls.Add(this.grd);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(623, 328);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // grd
            // 
            this.grd.Location = new System.Drawing.Point(7, 7);
            this.grd.MainView = this.grdv;
            this.grd.Name = "grd";
            this.grd.Size = new System.Drawing.Size(610, 315);
            this.grd.TabIndex = 4;
            this.grd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdv});
            // 
            // grdv
            // 
            this.grdv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCodigoBolsa,
            this.colClOrdID,
            this.colOrigClOrdID,
            this.colOrderID,
            this.colSecondaryOrderID,
            this.colCrossID,
            this.colExecID,
            this.colExecType,
            this.colOrdStatus,
            this.colBTSFinalTxOrdStatus,
            this.colOrdType,
            this.colOrdRejReason,
            this.colSide,
            this.colAvgPx,
            this.colLastQty,
            this.colLastPx,
            this.colLeavesQty,
            this.colCumQty,
            this.colOrigOrdQty,
            this.colTimeInForce,
            this.colTradeDate,
            this.colUniqueTradeId,
            this.colAccount,
            this.colText,
            this.colExecRefID,
            this.colExecTransType,
            this.colOrderQty,
            this.colPrice,
            this.colStopPx,
            this.colLastShares,
            this.colMinQty,
            this.colMaxFloor,
            this.colTransactTime,
            this.colSymbol,
            this.colSecurityID,
            this.colSecurityIDSource,
            this.colSecurityExchange});
            this.grdv.GridControl = this.grd;
            this.grdv.Name = "grdv";
            this.grdv.OptionsBehavior.Editable = false;
            this.grdv.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdv.OptionsView.ColumnAutoWidth = false;
            this.grdv.OptionsView.ShowGroupPanel = false;
            this.grdv.OptionsView.ShowIndicator = false;
            // 
            // colCodigoBolsa
            // 
            this.colCodigoBolsa.Caption = "Bolsa";
            this.colCodigoBolsa.FieldName = "CodigoBolsa";
            this.colCodigoBolsa.Name = "colCodigoBolsa";
            this.colCodigoBolsa.ToolTip = "Código da bolsa no qual a ordem deve ser enviada.";
            this.colCodigoBolsa.Visible = true;
            this.colCodigoBolsa.VisibleIndex = 0;
            // 
            // colClOrdID
            // 
            this.colClOrdID.Caption = "Código Ordem";
            this.colClOrdID.FieldName = "ClOrdID";
            this.colClOrdID.Name = "colClOrdID";
            this.colClOrdID.ToolTip = "BMF / Bovespa: Identificador da oferta inserida eletronicamente pela instituição";
            this.colClOrdID.Visible = true;
            this.colClOrdID.VisibleIndex = 1;
            // 
            // colOrigClOrdID
            // 
            this.colOrigClOrdID.Caption = "Código Ordem Original";
            this.colOrigClOrdID.FieldName = "OrigClOrdID";
            this.colOrigClOrdID.Name = "colOrigClOrdID";
            this.colOrigClOrdID.ToolTip = resources.GetString("colOrigClOrdID.ToolTip");
            this.colOrigClOrdID.Visible = true;
            this.colOrigClOrdID.VisibleIndex = 2;
            // 
            // colOrderID
            // 
            this.colOrderID.Caption = "Código Ordem Bolsa";
            this.colOrderID.FieldName = "OrderID";
            this.colOrderID.Name = "colOrderID";
            this.colOrderID.ToolTip = "BMF / Bovespa: Identificador único da ordem, conforme atribuído pela Bolsa. A uni" +
                "cidade é garantida para o mesmo dia de pregão / instrumento.";
            this.colOrderID.Visible = true;
            this.colOrderID.VisibleIndex = 3;
            // 
            // colSecondaryOrderID
            // 
            this.colSecondaryOrderID.Caption = "Código Ordem Secundário";
            this.colSecondaryOrderID.FieldName = "SecondaryOrderID";
            this.colSecondaryOrderID.Name = "colSecondaryOrderID";
            this.colSecondaryOrderID.ToolTip = resources.GetString("colSecondaryOrderID.ToolTip");
            this.colSecondaryOrderID.Visible = true;
            this.colSecondaryOrderID.VisibleIndex = 4;
            // 
            // colCrossID
            // 
            this.colCrossID.Caption = "Código Ordem de Direto";
            this.colCrossID.FieldName = "CrossID";
            this.colCrossID.Name = "colCrossID";
            this.colCrossID.ToolTip = "Identificador da oferta direta inserida eletronicamente pela instituição (no caso" +
                " de resposta a uma odem direta)";
            this.colCrossID.Visible = true;
            this.colCrossID.VisibleIndex = 5;
            // 
            // colExecID
            // 
            this.colExecID.Caption = "Código de Execução";
            this.colExecID.FieldName = "ExecID";
            this.colExecID.Name = "colExecID";
            this.colExecID.ToolTip = "BMF / Bovespa: Identificador único de mensagem de execução, atribuído pela Bolsa " +
                "(único por instrumento)";
            this.colExecID.Visible = true;
            this.colExecID.VisibleIndex = 6;
            // 
            // colExecType
            // 
            this.colExecType.Caption = "Tipo de Execução";
            this.colExecType.FieldName = "ExecType";
            this.colExecType.Name = "colExecType";
            this.colExecType.ToolTip = resources.GetString("colExecType.ToolTip");
            this.colExecType.Visible = true;
            this.colExecType.VisibleIndex = 7;
            // 
            // colOrdStatus
            // 
            this.colOrdStatus.Caption = "Status da Ordem";
            this.colOrdStatus.FieldName = "OrdStatus";
            this.colOrdStatus.Name = "colOrdStatus";
            this.colOrdStatus.ToolTip = resources.GetString("colOrdStatus.ToolTip");
            this.colOrdStatus.Visible = true;
            this.colOrdStatus.VisibleIndex = 8;
            // 
            // colBTSFinalTxOrdStatus
            // 
            this.colBTSFinalTxOrdStatus.Caption = "Status Final da Ordem";
            this.colBTSFinalTxOrdStatus.FieldName = "BTSFinalTxOrdStatus";
            this.colBTSFinalTxOrdStatus.Name = "colBTSFinalTxOrdStatus";
            this.colBTSFinalTxOrdStatus.ToolTip = resources.GetString("colBTSFinalTxOrdStatus.ToolTip");
            this.colBTSFinalTxOrdStatus.Visible = true;
            this.colBTSFinalTxOrdStatus.VisibleIndex = 9;
            // 
            // colOrdType
            // 
            this.colOrdType.Caption = "Tipo da Ordem";
            this.colOrdType.FieldName = "OrdType";
            this.colOrdType.Name = "colOrdType";
            this.colOrdType.ToolTip = "Condicionado a ExecType != 8 (rejeição) ou a ExecType != H (negócio cancelado). V" +
                "alores aceitos: 2 = limitada; 4 = stop limitada; K = market with leftover as lim" +
                "it";
            this.colOrdType.Visible = true;
            this.colOrdType.VisibleIndex = 10;
            // 
            // colOrdRejReason
            // 
            this.colOrdRejReason.Caption = "Motivo de Rejeição";
            this.colOrdRejReason.FieldName = "OrdRejReason";
            this.colOrdRejReason.Name = "colOrdRejReason";
            this.colOrdRejReason.ToolTip = resources.GetString("colOrdRejReason.ToolTip");
            this.colOrdRejReason.Visible = true;
            this.colOrdRejReason.VisibleIndex = 11;
            // 
            // colSide
            // 
            this.colSide.Caption = "Direção";
            this.colSide.FieldName = "Side";
            this.colSide.Name = "colSide";
            this.colSide.ToolTip = "BMF / Bovespa: Ponta da oferta. Valores aceitos: 1 = compra; 2 = venda";
            this.colSide.Visible = true;
            this.colSide.VisibleIndex = 12;
            // 
            // colAvgPx
            // 
            this.colAvgPx.Caption = "Preço Médio";
            this.colAvgPx.FieldName = "AvgPx";
            this.colAvgPx.Name = "colAvgPx";
            this.colAvgPx.ToolTip = "BMF: Preço médio calculado para todas as quantidades executadas da oferta. Sempre" +
                " 0. | Bovespa: Sempre 0";
            this.colAvgPx.Visible = true;
            this.colAvgPx.VisibleIndex = 13;
            // 
            // colLastQty
            // 
            this.colLastQty.Caption = "Última Quantidade";
            this.colLastQty.FieldName = "LastQty";
            this.colLastQty.Name = "colLastQty";
            this.colLastQty.ToolTip = "Quantidade de ações ou de contratos comprada / vendida nessa execução (última). C" +
                "ondicionada a ExecType = F (negócio)";
            this.colLastQty.Visible = true;
            this.colLastQty.VisibleIndex = 14;
            // 
            // colLastPx
            // 
            this.colLastPx.Caption = "Último Preço";
            this.colLastPx.FieldName = "LastPx";
            this.colLastPx.Name = "colLastPx";
            this.colLastPx.ToolTip = "BMF: Quantidade de ações ou de contratos comprada / vendida nessa execução (últim" +
                "a). Condicionada a ExecType = F (negócio) | Bovespa: Preço dessa (última) execuç" +
                "ão. Condicionado a ExecType = 1 ou 2.";
            this.colLastPx.Visible = true;
            this.colLastPx.VisibleIndex = 15;
            // 
            // colLeavesQty
            // 
            this.colLeavesQty.Caption = "Saldo Restante";
            this.colLeavesQty.FieldName = "LeavesQty";
            this.colLeavesQty.Name = "colLeavesQty";
            this.colLeavesQty.ToolTip = "BMF / Bovespa: Saldo de ações ou contratos para execução posterior ou não executa" +
                "do. LeavesQty = OrderQty - CumQty.";
            this.colLeavesQty.Visible = true;
            this.colLeavesQty.VisibleIndex = 16;
            // 
            // colCumQty
            // 
            this.colCumQty.Caption = "Quantidade Executada";
            this.colCumQty.FieldName = "CumQty";
            this.colCumQty.Name = "colCumQty";
            this.colCumQty.ToolTip = "BMF / Bovespa: Quantidade executada total de ações ou contratos";
            this.colCumQty.Visible = true;
            this.colCumQty.VisibleIndex = 17;
            // 
            // colOrigOrdQty
            // 
            this.colOrigOrdQty.Caption = "Quantidade Original";
            this.colOrigOrdQty.FieldName = "OrigOrdQty";
            this.colOrigOrdQty.Name = "colOrigOrdQty";
            this.colOrigOrdQty.ToolTip = "Quantidade original da oferta antes de ser modificada. Presente na mensagem somen" +
                "te se ExecType = 5 (substituída).";
            this.colOrigOrdQty.Visible = true;
            this.colOrigOrdQty.VisibleIndex = 18;
            // 
            // colTimeInForce
            // 
            this.colTimeInForce.Caption = "Validade";
            this.colTimeInForce.FieldName = "TimeInForce";
            this.colTimeInForce.Name = "colTimeInForce";
            this.colTimeInForce.ToolTip = resources.GetString("colTimeInForce.ToolTip");
            this.colTimeInForce.Visible = true;
            this.colTimeInForce.VisibleIndex = 19;
            // 
            // colTradeDate
            // 
            this.colTradeDate.Caption = "Data da Operação";
            this.colTradeDate.FieldName = "TradeDate";
            this.colTradeDate.Name = "colTradeDate";
            this.colTradeDate.ToolTip = "BMF / Bovespa: Data da operação referenciada na mensagem, no formato AAAAMMDD (ex" +
                "pressa o horário do local em que a operação foi realizada). A ausência desse cam" +
                "po indica o dia atual.";
            this.colTradeDate.Visible = true;
            this.colTradeDate.VisibleIndex = 20;
            // 
            // colUniqueTradeId
            // 
            this.colUniqueTradeId.Caption = "Identificador Único";
            this.colUniqueTradeId.FieldName = "UniqueTradeId";
            this.colUniqueTradeId.Name = "colUniqueTradeId";
            this.colUniqueTradeId.ToolTip = "Contém o identificador único para essa operação, por instrumento + data de pregão" +
                ", atribuído pela Bolsa. Condicionado a ExecType = F (negócio)";
            this.colUniqueTradeId.Visible = true;
            this.colUniqueTradeId.VisibleIndex = 21;
            // 
            // colAccount
            // 
            this.colAccount.Caption = "Conta";
            this.colAccount.FieldName = "Account";
            this.colAccount.Name = "colAccount";
            this.colAccount.ToolTip = "BMF / Bovespa: Mnemônico de conta";
            this.colAccount.Visible = true;
            this.colAccount.VisibleIndex = 22;
            // 
            // colText
            // 
            this.colText.Caption = "Mensagem Bolsa";
            this.colText.FieldName = "Text";
            this.colText.Name = "colText";
            this.colText.ToolTip = "BMF / Bovespa: Sequência de texto, em formato livre";
            this.colText.Visible = true;
            this.colText.VisibleIndex = 23;
            // 
            // colExecRefID
            // 
            this.colExecRefID.Caption = "Número Referencia Bolsa";
            this.colExecRefID.FieldName = "ExecRefID";
            this.colExecRefID.Name = "colExecRefID";
            this.colExecRefID.ToolTip = "Bovespa: Nos avisos tanto sobre negócios realizados quanto cancelados, contém o n" +
                "úmero da operação conforme emitido pelo núcleo de negociação";
            this.colExecRefID.Visible = true;
            this.colExecRefID.VisibleIndex = 24;
            // 
            // colExecTransType
            // 
            this.colExecTransType.Caption = "Tipo de Transação";
            this.colExecTransType.FieldName = "ExecTransType";
            this.colExecTransType.Name = "colExecTransType";
            this.colExecTransType.ToolTip = "Bovespa: Identifica o tipo de transação. Valores aceitos: 0 = nova; 1 = cancelame" +
                "nto";
            this.colExecTransType.Visible = true;
            this.colExecTransType.VisibleIndex = 25;
            // 
            // colOrderQty
            // 
            this.colOrderQty.Caption = "Quantidade";
            this.colOrderQty.FieldName = "OrderQty";
            this.colOrderQty.Name = "colOrderQty";
            this.colOrderQty.ToolTip = "Bovespa: Número de ações ou contratos da oferta";
            this.colOrderQty.Visible = true;
            this.colOrderQty.VisibleIndex = 26;
            // 
            // colPrice
            // 
            this.colPrice.Caption = "Preço";
            this.colPrice.FieldName = "Price";
            this.colPrice.Name = "colPrice";
            this.colPrice.ToolTip = "Bovespa: Preço por ação ou contrato. Obrigatório se especificado na oferta.";
            this.colPrice.Visible = true;
            this.colPrice.VisibleIndex = 27;
            // 
            // colStopPx
            // 
            this.colStopPx.Caption = "Preço de Stop";
            this.colStopPx.FieldName = "StopPx";
            this.colStopPx.Name = "colStopPx";
            this.colStopPx.ToolTip = "Bovespa: Preço de acionamento de ordem stop limitada (condicionado a OrdType = 4)" +
                "";
            this.colStopPx.Visible = true;
            this.colStopPx.VisibleIndex = 28;
            // 
            // colLastShares
            // 
            this.colLastShares.Caption = "Quantidade Última Execução";
            this.colLastShares.FieldName = "LastShares";
            this.colLastShares.Name = "colLastShares";
            this.colLastShares.ToolTip = "Bovespa: Quantidade de ações ou de contratos comprada / vendida nessa execução (ú" +
                "ltima). Condicionada a ExecType = 1 ou 2.";
            this.colLastShares.Visible = true;
            this.colLastShares.VisibleIndex = 29;
            // 
            // colMinQty
            // 
            this.colMinQty.Caption = "Quantidade Mínima";
            this.colMinQty.FieldName = "MinQty";
            this.colMinQty.Name = "colMinQty";
            this.colMinQty.ToolTip = "Bovespa: Quantidade mínima para execução da oferta. Reflete o campo MinQty da ofe" +
                "rta original.";
            this.colMinQty.Visible = true;
            this.colMinQty.VisibleIndex = 30;
            // 
            // colMaxFloor
            // 
            this.colMaxFloor.Caption = "Máximo Exibido";
            this.colMaxFloor.FieldName = "MaxFloor";
            this.colMaxFloor.Name = "colMaxFloor";
            this.colMaxFloor.ToolTip = "Bovespa: Número máximo de ações ou contratos da oferta a ser exibido no núcleo de" +
                " negociação a qualquer tempo. Reflete o campo MaxFloor da oferta original.";
            this.colMaxFloor.Visible = true;
            this.colMaxFloor.VisibleIndex = 31;
            // 
            // colTransactTime
            // 
            this.colTransactTime.Caption = "Data da Transação";
            this.colTransactTime.FieldName = "TransactTime";
            this.colTransactTime.Name = "colTransactTime";
            this.colTransactTime.ToolTip = "Bovespa: Horário da execução / geração da ordem, expresso em UTC";
            this.colTransactTime.Visible = true;
            this.colTransactTime.VisibleIndex = 32;
            // 
            // colSymbol
            // 
            this.colSymbol.Caption = "Papel";
            this.colSymbol.FieldName = "Symbol";
            this.colSymbol.Name = "colSymbol";
            this.colSymbol.ToolTip = resources.GetString("colSymbol.ToolTip");
            this.colSymbol.Visible = true;
            this.colSymbol.VisibleIndex = 33;
            // 
            // colSecurityID
            // 
            this.colSecurityID.Caption = "Código Papel";
            this.colSecurityID.FieldName = "SecurityID";
            this.colSecurityID.Name = "colSecurityID";
            this.colSecurityID.ToolTip = "Identificador do instrumento, conforme definido pela BMFBOVESPA. Para a lista de " +
                "instrumentos, consulte a mensagem correspondente (Security List).";
            this.colSecurityID.Visible = true;
            this.colSecurityID.VisibleIndex = 34;
            // 
            // colSecurityIDSource
            // 
            this.colSecurityIDSource.Caption = "Origem do Código do Papel";
            this.colSecurityIDSource.FieldName = "SecurityIDSource";
            this.colSecurityIDSource.Name = "colSecurityIDSource";
            this.colSecurityIDSource.ToolTip = "Classe ou fonte de identificação do instrumento. Campo condicionado a SecurityID " +
                "estar especificado. Valor aceito: 8 = símbolo da Bolsa (identificador BMFBOVESPA" +
                " para instrumento)";
            this.colSecurityIDSource.Visible = true;
            this.colSecurityIDSource.VisibleIndex = 35;
            // 
            // colSecurityExchange
            // 
            this.colSecurityExchange.Caption = "Bolsa do Papel";
            this.colSecurityExchange.FieldName = "SecurityExchange";
            this.colSecurityExchange.Name = "colSecurityExchange";
            this.colSecurityExchange.ToolTip = resources.GetString("colSecurityExchange.ToolTip");
            this.colSecurityExchange.Visible = true;
            this.colSecurityExchange.VisibleIndex = 36;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(623, 328);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.grd;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(621, 326);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
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
            this.cmdConfigurar});
            this.barManager1.MaxItemId = 1;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdConfigurar)});
            this.bar1.Text = "Tools";
            // 
            // cmdConfigurar
            // 
            this.cmdConfigurar.Caption = "Configurar";
            this.cmdConfigurar.Id = 0;
            this.cmdConfigurar.Name = "cmdConfigurar";
            // 
            // OperacaoDetalheHistorico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "OperacaoDetalheHistorico";
            this.Size = new System.Drawing.Size(623, 352);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl grd;
        private DevExpress.XtraGrid.Views.Grid.GridView grdv;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigoBolsa;
        private DevExpress.XtraGrid.Columns.GridColumn colClOrdID;
        private DevExpress.XtraGrid.Columns.GridColumn colOrigClOrdID;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderID;
        private DevExpress.XtraGrid.Columns.GridColumn colSecondaryOrderID;
        private DevExpress.XtraGrid.Columns.GridColumn colCrossID;
        private DevExpress.XtraGrid.Columns.GridColumn colExecID;
        private DevExpress.XtraGrid.Columns.GridColumn colExecType;
        private DevExpress.XtraGrid.Columns.GridColumn colOrdStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colBTSFinalTxOrdStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colOrdType;
        private DevExpress.XtraGrid.Columns.GridColumn colOrdRejReason;
        private DevExpress.XtraGrid.Columns.GridColumn colSide;
        private DevExpress.XtraGrid.Columns.GridColumn colAvgPx;
        private DevExpress.XtraGrid.Columns.GridColumn colLastQty;
        private DevExpress.XtraGrid.Columns.GridColumn colLastPx;
        private DevExpress.XtraGrid.Columns.GridColumn colLeavesQty;
        private DevExpress.XtraGrid.Columns.GridColumn colCumQty;
        private DevExpress.XtraGrid.Columns.GridColumn colOrigOrdQty;
        private DevExpress.XtraGrid.Columns.GridColumn colTimeInForce;
        private DevExpress.XtraGrid.Columns.GridColumn colTradeDate;
        private DevExpress.XtraGrid.Columns.GridColumn colUniqueTradeId;
        private DevExpress.XtraGrid.Columns.GridColumn colAccount;
        private DevExpress.XtraGrid.Columns.GridColumn colText;
        private DevExpress.XtraGrid.Columns.GridColumn colExecRefID;
        private DevExpress.XtraGrid.Columns.GridColumn colExecTransType;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderQty;
        private DevExpress.XtraGrid.Columns.GridColumn colPrice;
        private DevExpress.XtraGrid.Columns.GridColumn colStopPx;
        private DevExpress.XtraGrid.Columns.GridColumn colLastShares;
        private DevExpress.XtraGrid.Columns.GridColumn colMinQty;
        private DevExpress.XtraGrid.Columns.GridColumn colMaxFloor;
        private DevExpress.XtraGrid.Columns.GridColumn colTransactTime;
        private DevExpress.XtraGrid.Columns.GridColumn colSymbol;
        private DevExpress.XtraGrid.Columns.GridColumn colSecurityID;
        private DevExpress.XtraGrid.Columns.GridColumn colSecurityIDSource;
        private DevExpress.XtraGrid.Columns.GridColumn colSecurityExchange;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem cmdConfigurar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}

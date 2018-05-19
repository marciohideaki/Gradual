namespace Gradual.Spider.GlobalOrderTracking.Formularios
{
    partial class fCancel
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
            this.spiderOrderCancelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridCancel = new GradualForm.Controls.CustomDataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.orderIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origClOrdIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchangeNumberIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clOrdIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.symbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityExchangeIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stopStartIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrdTypeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrdStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRegisterTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expireDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTimeInForce = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.channelIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.execBrokerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSide = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderQtyMinDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderQtyAparDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderQtyRemainingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stopPxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avgPxWDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.systemIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.memoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cumQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fixMsgSeqNumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sessionIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sessionIDOriginalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idFixDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msgFixDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msg42Base64DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handlInstDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.integrationNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchangeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avgPxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountDvDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.spiderOrderCancelBindingSource)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCancel)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spiderOrderCancelBindingSource
            // 
            this.spiderOrderCancelBindingSource.DataSource = typeof(Gradual.Core.Spider.OrderFixProcessing.Lib.Dados.SpiderOrderInfo);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(6, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(788, 251);
            this.panel2.TabIndex = 1;
            // 
            // gridCancel
            // 
            this.gridCancel.AllowUserToAddRows = false;
            this.gridCancel.AllowUserToDeleteRows = false;
            this.gridCancel.AllowUserToResizeRows = false;
            this.gridCancel.AutoGenerateColumns = false;
            this.gridCancel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridCancel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCancel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.orderIDDataGridViewTextBoxColumn,
            this.origClOrdIDDataGridViewTextBoxColumn,
            this.exchangeNumberIDDataGridViewTextBoxColumn,
            this.clOrdIDDataGridViewTextBoxColumn,
            this.accountDataGridViewTextBoxColumn,
            this.symbolDataGridViewTextBoxColumn,
            this.securityExchangeIDDataGridViewTextBoxColumn,
            this.stopStartIDDataGridViewTextBoxColumn,
            this.columnOrdTypeID,
            this.columnOrdStatus,
            this.columnRegisterTime,
            this.columnHora,
            this.transactTimeDataGridViewTextBoxColumn,
            this.expireDateDataGridViewTextBoxColumn,
            this.columnTimeInForce,
            this.channelIDDataGridViewTextBoxColumn,
            this.execBrokerDataGridViewTextBoxColumn,
            this.columnSide,
            this.orderQtyDataGridViewTextBoxColumn,
            this.orderQtyMinDataGridViewTextBoxColumn,
            this.orderQtyAparDataGridViewTextBoxColumn,
            this.orderQtyRemainingDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.stopPxDataGridViewTextBoxColumn,
            this.avgPxWDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.systemIDDataGridViewTextBoxColumn,
            this.memoDataGridViewTextBoxColumn,
            this.cumQtyDataGridViewTextBoxColumn,
            this.fixMsgSeqNumDataGridViewTextBoxColumn,
            this.sessionIDDataGridViewTextBoxColumn,
            this.sessionIDOriginalDataGridViewTextBoxColumn,
            this.idFixDataGridViewTextBoxColumn,
            this.msgFixDataGridViewTextBoxColumn,
            this.msg42Base64DataGridViewTextBoxColumn,
            this.handlInstDataGridViewTextBoxColumn,
            this.integrationNameDataGridViewTextBoxColumn,
            this.exchangeDataGridViewTextBoxColumn,
            this.avgPxDataGridViewTextBoxColumn,
            this.accountDvDataGridViewTextBoxColumn});
            this.gridCancel.DataSource = this.spiderOrderCancelBindingSource;
            this.gridCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCancel.Location = new System.Drawing.Point(0, 0);
            this.gridCancel.Name = "gridCancel";
            this.gridCancel.RowHeadersVisible = false;
            this.gridCancel.Size = new System.Drawing.Size(788, 251);
            this.gridCancel.TabIndex = 0;
            this.gridCancel.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridCancel_CellFormatting);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnIgnore);
            this.panel1.Controls.Add(this.btnExecute);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(6, 287);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 35);
            this.panel1.TabIndex = 0;
            // 
            // btnIgnore
            // 
            this.btnIgnore.Location = new System.Drawing.Point(579, 6);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(100, 23);
            this.btnIgnore.TabIndex = 1;
            this.btnIgnore.Text = "Ignorar";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(685, 6);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(100, 23);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Cancelar Ordens";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // orderIDDataGridViewTextBoxColumn
            // 
            this.orderIDDataGridViewTextBoxColumn.DataPropertyName = "OrderID";
            this.orderIDDataGridViewTextBoxColumn.HeaderText = "OrderID";
            this.orderIDDataGridViewTextBoxColumn.Name = "orderIDDataGridViewTextBoxColumn";
            this.orderIDDataGridViewTextBoxColumn.Visible = false;
            this.orderIDDataGridViewTextBoxColumn.Width = 50;
            // 
            // origClOrdIDDataGridViewTextBoxColumn
            // 
            this.origClOrdIDDataGridViewTextBoxColumn.DataPropertyName = "OrigClOrdID";
            this.origClOrdIDDataGridViewTextBoxColumn.HeaderText = "OrigClOrdID";
            this.origClOrdIDDataGridViewTextBoxColumn.Name = "origClOrdIDDataGridViewTextBoxColumn";
            this.origClOrdIDDataGridViewTextBoxColumn.Visible = false;
            this.origClOrdIDDataGridViewTextBoxColumn.Width = 69;
            // 
            // exchangeNumberIDDataGridViewTextBoxColumn
            // 
            this.exchangeNumberIDDataGridViewTextBoxColumn.DataPropertyName = "ExchangeNumberID";
            this.exchangeNumberIDDataGridViewTextBoxColumn.HeaderText = "ExchangeNumberID";
            this.exchangeNumberIDDataGridViewTextBoxColumn.Name = "exchangeNumberIDDataGridViewTextBoxColumn";
            this.exchangeNumberIDDataGridViewTextBoxColumn.Visible = false;
            this.exchangeNumberIDDataGridViewTextBoxColumn.Width = 109;
            // 
            // clOrdIDDataGridViewTextBoxColumn
            // 
            this.clOrdIDDataGridViewTextBoxColumn.DataPropertyName = "ClOrdID";
            this.clOrdIDDataGridViewTextBoxColumn.HeaderText = "Nr. Ordem";
            this.clOrdIDDataGridViewTextBoxColumn.Name = "clOrdIDDataGridViewTextBoxColumn";
            this.clOrdIDDataGridViewTextBoxColumn.Width = 80;
            // 
            // accountDataGridViewTextBoxColumn
            // 
            this.accountDataGridViewTextBoxColumn.DataPropertyName = "Account";
            this.accountDataGridViewTextBoxColumn.HeaderText = "Conta";
            this.accountDataGridViewTextBoxColumn.Name = "accountDataGridViewTextBoxColumn";
            this.accountDataGridViewTextBoxColumn.Width = 60;
            // 
            // symbolDataGridViewTextBoxColumn
            // 
            this.symbolDataGridViewTextBoxColumn.DataPropertyName = "Symbol";
            this.symbolDataGridViewTextBoxColumn.HeaderText = "Ativo";
            this.symbolDataGridViewTextBoxColumn.Name = "symbolDataGridViewTextBoxColumn";
            this.symbolDataGridViewTextBoxColumn.Width = 56;
            // 
            // securityExchangeIDDataGridViewTextBoxColumn
            // 
            this.securityExchangeIDDataGridViewTextBoxColumn.DataPropertyName = "SecurityExchangeID";
            this.securityExchangeIDDataGridViewTextBoxColumn.HeaderText = "SecurityExchangeID";
            this.securityExchangeIDDataGridViewTextBoxColumn.Name = "securityExchangeIDDataGridViewTextBoxColumn";
            this.securityExchangeIDDataGridViewTextBoxColumn.Visible = false;
            this.securityExchangeIDDataGridViewTextBoxColumn.Width = 129;
            // 
            // stopStartIDDataGridViewTextBoxColumn
            // 
            this.stopStartIDDataGridViewTextBoxColumn.DataPropertyName = "StopStartID";
            this.stopStartIDDataGridViewTextBoxColumn.HeaderText = "StopStartID";
            this.stopStartIDDataGridViewTextBoxColumn.Name = "stopStartIDDataGridViewTextBoxColumn";
            this.stopStartIDDataGridViewTextBoxColumn.Visible = false;
            this.stopStartIDDataGridViewTextBoxColumn.Width = 87;
            // 
            // columnOrdTypeID
            // 
            this.columnOrdTypeID.DataPropertyName = "OrdTypeID";
            this.columnOrdTypeID.HeaderText = "Tp. Ordem";
            this.columnOrdTypeID.Name = "columnOrdTypeID";
            this.columnOrdTypeID.Visible = false;
            this.columnOrdTypeID.Width = 82;
            // 
            // columnOrdStatus
            // 
            this.columnOrdStatus.DataPropertyName = "OrdStatus";
            this.columnOrdStatus.HeaderText = "Status";
            this.columnOrdStatus.Name = "columnOrdStatus";
            this.columnOrdStatus.Width = 62;
            // 
            // columnRegisterTime
            // 
            this.columnRegisterTime.DataPropertyName = "RegisterTime";
            this.columnRegisterTime.HeaderText = "Dt. Envio";
            this.columnRegisterTime.Name = "columnRegisterTime";
            this.columnRegisterTime.Width = 76;
            // 
            // columnHora
            // 
            this.columnHora.HeaderText = "Hora";
            this.columnHora.Name = "columnHora";
            this.columnHora.Width = 55;
            // 
            // transactTimeDataGridViewTextBoxColumn
            // 
            this.transactTimeDataGridViewTextBoxColumn.DataPropertyName = "TransactTime";
            this.transactTimeDataGridViewTextBoxColumn.HeaderText = "Dt. Atualização";
            this.transactTimeDataGridViewTextBoxColumn.Name = "transactTimeDataGridViewTextBoxColumn";
            this.transactTimeDataGridViewTextBoxColumn.Width = 96;
            // 
            // expireDateDataGridViewTextBoxColumn
            // 
            this.expireDateDataGridViewTextBoxColumn.DataPropertyName = "ExpireDate";
            this.expireDateDataGridViewTextBoxColumn.HeaderText = "ExpireDate";
            this.expireDateDataGridViewTextBoxColumn.Name = "expireDateDataGridViewTextBoxColumn";
            this.expireDateDataGridViewTextBoxColumn.Visible = false;
            this.expireDateDataGridViewTextBoxColumn.Width = 84;
            // 
            // columnTimeInForce
            // 
            this.columnTimeInForce.DataPropertyName = "TimeInForce";
            this.columnTimeInForce.HeaderText = "Validade";
            this.columnTimeInForce.Name = "columnTimeInForce";
            this.columnTimeInForce.Width = 73;
            // 
            // channelIDDataGridViewTextBoxColumn
            // 
            this.channelIDDataGridViewTextBoxColumn.DataPropertyName = "ChannelID";
            this.channelIDDataGridViewTextBoxColumn.HeaderText = "Porta";
            this.channelIDDataGridViewTextBoxColumn.Name = "channelIDDataGridViewTextBoxColumn";
            this.channelIDDataGridViewTextBoxColumn.Visible = false;
            this.channelIDDataGridViewTextBoxColumn.Width = 57;
            // 
            // execBrokerDataGridViewTextBoxColumn
            // 
            this.execBrokerDataGridViewTextBoxColumn.DataPropertyName = "ExecBroker";
            this.execBrokerDataGridViewTextBoxColumn.HeaderText = "ExecBroker";
            this.execBrokerDataGridViewTextBoxColumn.Name = "execBrokerDataGridViewTextBoxColumn";
            this.execBrokerDataGridViewTextBoxColumn.Width = 87;
            // 
            // columnSide
            // 
            this.columnSide.DataPropertyName = "Side";
            this.columnSide.HeaderText = "C/V";
            this.columnSide.Name = "columnSide";
            this.columnSide.Width = 51;
            // 
            // orderQtyDataGridViewTextBoxColumn
            // 
            this.orderQtyDataGridViewTextBoxColumn.DataPropertyName = "OrderQty";
            this.orderQtyDataGridViewTextBoxColumn.HeaderText = "Qtde. Solic.";
            this.orderQtyDataGridViewTextBoxColumn.Name = "orderQtyDataGridViewTextBoxColumn";
            this.orderQtyDataGridViewTextBoxColumn.Width = 80;
            // 
            // orderQtyMinDataGridViewTextBoxColumn
            // 
            this.orderQtyMinDataGridViewTextBoxColumn.DataPropertyName = "OrderQtyMin";
            this.orderQtyMinDataGridViewTextBoxColumn.HeaderText = "Qtde. Min.";
            this.orderQtyMinDataGridViewTextBoxColumn.Name = "orderQtyMinDataGridViewTextBoxColumn";
            this.orderQtyMinDataGridViewTextBoxColumn.Width = 75;
            // 
            // orderQtyAparDataGridViewTextBoxColumn
            // 
            this.orderQtyAparDataGridViewTextBoxColumn.DataPropertyName = "OrderQtyApar";
            this.orderQtyAparDataGridViewTextBoxColumn.HeaderText = "Qtde. Apar.";
            this.orderQtyAparDataGridViewTextBoxColumn.Name = "orderQtyAparDataGridViewTextBoxColumn";
            this.orderQtyAparDataGridViewTextBoxColumn.Width = 79;
            // 
            // orderQtyRemainingDataGridViewTextBoxColumn
            // 
            this.orderQtyRemainingDataGridViewTextBoxColumn.DataPropertyName = "OrderQtyRemaining";
            this.orderQtyRemainingDataGridViewTextBoxColumn.HeaderText = "Saldo";
            this.orderQtyRemainingDataGridViewTextBoxColumn.Name = "orderQtyRemainingDataGridViewTextBoxColumn";
            this.orderQtyRemainingDataGridViewTextBoxColumn.Width = 59;
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            dataGridViewCellStyle1.Format = "N2";
            this.priceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.priceDataGridViewTextBoxColumn.HeaderText = "Preço";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            this.priceDataGridViewTextBoxColumn.Width = 60;
            // 
            // stopPxDataGridViewTextBoxColumn
            // 
            this.stopPxDataGridViewTextBoxColumn.DataPropertyName = "StopPx";
            dataGridViewCellStyle2.Format = "N2";
            this.stopPxDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.stopPxDataGridViewTextBoxColumn.HeaderText = "Pr. Stop/Start";
            this.stopPxDataGridViewTextBoxColumn.Name = "stopPxDataGridViewTextBoxColumn";
            this.stopPxDataGridViewTextBoxColumn.Width = 89;
            // 
            // avgPxWDataGridViewTextBoxColumn
            // 
            this.avgPxWDataGridViewTextBoxColumn.DataPropertyName = "AvgPxW";
            dataGridViewCellStyle3.Format = "N2";
            this.avgPxWDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.avgPxWDataGridViewTextBoxColumn.HeaderText = "Pr. Médio";
            this.avgPxWDataGridViewTextBoxColumn.Name = "avgPxWDataGridViewTextBoxColumn";
            this.avgPxWDataGridViewTextBoxColumn.Width = 71;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Visible = false;
            this.descriptionDataGridViewTextBoxColumn.Width = 85;
            // 
            // systemIDDataGridViewTextBoxColumn
            // 
            this.systemIDDataGridViewTextBoxColumn.DataPropertyName = "SystemID";
            this.systemIDDataGridViewTextBoxColumn.HeaderText = "SystemID";
            this.systemIDDataGridViewTextBoxColumn.Name = "systemIDDataGridViewTextBoxColumn";
            this.systemIDDataGridViewTextBoxColumn.Visible = false;
            this.systemIDDataGridViewTextBoxColumn.Width = 77;
            // 
            // memoDataGridViewTextBoxColumn
            // 
            this.memoDataGridViewTextBoxColumn.DataPropertyName = "Memo";
            this.memoDataGridViewTextBoxColumn.HeaderText = "Memo";
            this.memoDataGridViewTextBoxColumn.Name = "memoDataGridViewTextBoxColumn";
            this.memoDataGridViewTextBoxColumn.Visible = false;
            this.memoDataGridViewTextBoxColumn.Width = 61;
            // 
            // cumQtyDataGridViewTextBoxColumn
            // 
            this.cumQtyDataGridViewTextBoxColumn.DataPropertyName = "CumQty";
            this.cumQtyDataGridViewTextBoxColumn.HeaderText = "CumQty";
            this.cumQtyDataGridViewTextBoxColumn.Name = "cumQtyDataGridViewTextBoxColumn";
            this.cumQtyDataGridViewTextBoxColumn.Visible = false;
            this.cumQtyDataGridViewTextBoxColumn.Width = 69;
            // 
            // fixMsgSeqNumDataGridViewTextBoxColumn
            // 
            this.fixMsgSeqNumDataGridViewTextBoxColumn.DataPropertyName = "FixMsgSeqNum";
            this.fixMsgSeqNumDataGridViewTextBoxColumn.HeaderText = "FixMsgSeqNum";
            this.fixMsgSeqNumDataGridViewTextBoxColumn.Name = "fixMsgSeqNumDataGridViewTextBoxColumn";
            this.fixMsgSeqNumDataGridViewTextBoxColumn.Visible = false;
            this.fixMsgSeqNumDataGridViewTextBoxColumn.Width = 106;
            // 
            // sessionIDDataGridViewTextBoxColumn
            // 
            this.sessionIDDataGridViewTextBoxColumn.DataPropertyName = "SessionID";
            this.sessionIDDataGridViewTextBoxColumn.HeaderText = "SessionID";
            this.sessionIDDataGridViewTextBoxColumn.Name = "sessionIDDataGridViewTextBoxColumn";
            this.sessionIDDataGridViewTextBoxColumn.Visible = false;
            this.sessionIDDataGridViewTextBoxColumn.Width = 80;
            // 
            // sessionIDOriginalDataGridViewTextBoxColumn
            // 
            this.sessionIDOriginalDataGridViewTextBoxColumn.DataPropertyName = "SessionIDOriginal";
            this.sessionIDOriginalDataGridViewTextBoxColumn.HeaderText = "Sessão";
            this.sessionIDOriginalDataGridViewTextBoxColumn.Name = "sessionIDOriginalDataGridViewTextBoxColumn";
            this.sessionIDOriginalDataGridViewTextBoxColumn.Width = 67;
            // 
            // idFixDataGridViewTextBoxColumn
            // 
            this.idFixDataGridViewTextBoxColumn.DataPropertyName = "IdFix";
            this.idFixDataGridViewTextBoxColumn.HeaderText = "IdFix";
            this.idFixDataGridViewTextBoxColumn.Name = "idFixDataGridViewTextBoxColumn";
            this.idFixDataGridViewTextBoxColumn.Visible = false;
            this.idFixDataGridViewTextBoxColumn.Width = 54;
            // 
            // msgFixDataGridViewTextBoxColumn
            // 
            this.msgFixDataGridViewTextBoxColumn.DataPropertyName = "MsgFix";
            this.msgFixDataGridViewTextBoxColumn.HeaderText = "MsgFix";
            this.msgFixDataGridViewTextBoxColumn.Name = "msgFixDataGridViewTextBoxColumn";
            this.msgFixDataGridViewTextBoxColumn.Visible = false;
            this.msgFixDataGridViewTextBoxColumn.Width = 65;
            // 
            // msg42Base64DataGridViewTextBoxColumn
            // 
            this.msg42Base64DataGridViewTextBoxColumn.DataPropertyName = "Msg42Base64";
            this.msg42Base64DataGridViewTextBoxColumn.HeaderText = "Msg42Base64";
            this.msg42Base64DataGridViewTextBoxColumn.Name = "msg42Base64DataGridViewTextBoxColumn";
            this.msg42Base64DataGridViewTextBoxColumn.Visible = false;
            // 
            // handlInstDataGridViewTextBoxColumn
            // 
            this.handlInstDataGridViewTextBoxColumn.DataPropertyName = "HandlInst";
            this.handlInstDataGridViewTextBoxColumn.HeaderText = "HandlInst";
            this.handlInstDataGridViewTextBoxColumn.Name = "handlInstDataGridViewTextBoxColumn";
            this.handlInstDataGridViewTextBoxColumn.Visible = false;
            this.handlInstDataGridViewTextBoxColumn.Width = 77;
            // 
            // integrationNameDataGridViewTextBoxColumn
            // 
            this.integrationNameDataGridViewTextBoxColumn.DataPropertyName = "IntegrationName";
            this.integrationNameDataGridViewTextBoxColumn.HeaderText = "IntegrationName";
            this.integrationNameDataGridViewTextBoxColumn.Name = "integrationNameDataGridViewTextBoxColumn";
            this.integrationNameDataGridViewTextBoxColumn.Visible = false;
            this.integrationNameDataGridViewTextBoxColumn.Width = 110;
            // 
            // exchangeDataGridViewTextBoxColumn
            // 
            this.exchangeDataGridViewTextBoxColumn.DataPropertyName = "Exchange";
            this.exchangeDataGridViewTextBoxColumn.HeaderText = "Exchange";
            this.exchangeDataGridViewTextBoxColumn.Name = "exchangeDataGridViewTextBoxColumn";
            this.exchangeDataGridViewTextBoxColumn.Visible = false;
            this.exchangeDataGridViewTextBoxColumn.Width = 80;
            // 
            // avgPxDataGridViewTextBoxColumn
            // 
            this.avgPxDataGridViewTextBoxColumn.DataPropertyName = "AvgPx";
            dataGridViewCellStyle4.Format = "N2";
            this.avgPxDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.avgPxDataGridViewTextBoxColumn.HeaderText = "AvgPx";
            this.avgPxDataGridViewTextBoxColumn.Name = "avgPxDataGridViewTextBoxColumn";
            this.avgPxDataGridViewTextBoxColumn.Visible = false;
            this.avgPxDataGridViewTextBoxColumn.Width = 63;
            // 
            // accountDvDataGridViewTextBoxColumn
            // 
            this.accountDvDataGridViewTextBoxColumn.DataPropertyName = "AccountDv";
            this.accountDvDataGridViewTextBoxColumn.HeaderText = "AccountDv";
            this.accountDvDataGridViewTextBoxColumn.Name = "accountDvDataGridViewTextBoxColumn";
            this.accountDvDataGridViewTextBoxColumn.Visible = false;
            this.accountDvDataGridViewTextBoxColumn.Width = 86;
            // 
            // fCancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 350);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fCancel";
            this.Text = "..:: Cancelamento Global ::..";
            ((System.ComponentModel.ISupportInitialize)(this.spiderOrderCancelBindingSource)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCancel)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Panel panel2;
        private GradualForm.Controls.CustomDataGridView gridCancel;
        private System.Windows.Forms.BindingSource spiderOrderCancelBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn origClOrdIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangeNumberIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clOrdIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn securityExchangeIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stopStartIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrdTypeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrdStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRegisterTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHora;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expireDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTimeInForce;
        private System.Windows.Forms.DataGridViewTextBoxColumn channelIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn execBrokerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSide;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderQtyMinDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderQtyAparDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderQtyRemainingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stopPxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn avgPxWDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn systemIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn memoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cumQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fixMsgSeqNumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionIDOriginalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idFixDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn msgFixDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn msg42Base64DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handlInstDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn integrationNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn avgPxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountDvDataGridViewTextBoxColumn;

    }
}
namespace Gradual.Spider.MockTest
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
            this.btStartClient = new System.Windows.Forms.Button();
            this.btStartServer = new System.Windows.Forms.Button();
            this.btEnviarCliente = new System.Windows.Forms.Button();
            this.btEnviarServer = new System.Windows.Forms.Button();
            this.txtClient = new System.Windows.Forms.TextBox();
            this.txtMsgServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bAssinar = new System.Windows.Forms.Button();
            this.txtOrders = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bTransmit = new System.Windows.Forms.Button();
            this.lblInicioMds = new System.Windows.Forms.Label();
            this.lblFimMDS = new System.Windows.Forms.Label();
            this.lblFimSocket = new System.Windows.Forms.Label();
            this.lblInicioSocket = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.averagePriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clOrdIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origClOrdIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.symbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registerTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeInForceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expireDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.channelIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchangeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sideDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderQtyRemmainingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cumQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxFloorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stopPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.compIDOMSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transmitidaDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ordemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ordemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btStartClient
            // 
            this.btStartClient.Location = new System.Drawing.Point(93, 12);
            this.btStartClient.Name = "btStartClient";
            this.btStartClient.Size = new System.Drawing.Size(75, 23);
            this.btStartClient.TabIndex = 4;
            this.btStartClient.Text = "Start Client";
            this.btStartClient.UseVisualStyleBackColor = true;
            this.btStartClient.Click += new System.EventHandler(this.btStartClient_Click);
            // 
            // btStartServer
            // 
            this.btStartServer.Location = new System.Drawing.Point(12, 12);
            this.btStartServer.Name = "btStartServer";
            this.btStartServer.Size = new System.Drawing.Size(75, 23);
            this.btStartServer.TabIndex = 3;
            this.btStartServer.Text = "Start Server";
            this.btStartServer.UseVisualStyleBackColor = true;
            this.btStartServer.Click += new System.EventHandler(this.btStartServer_Click);
            // 
            // btEnviarCliente
            // 
            this.btEnviarCliente.Location = new System.Drawing.Point(274, 12);
            this.btEnviarCliente.Name = "btEnviarCliente";
            this.btEnviarCliente.Size = new System.Drawing.Size(93, 23);
            this.btEnviarCliente.TabIndex = 9;
            this.btEnviarCliente.Text = "Enviar P Server";
            this.btEnviarCliente.UseVisualStyleBackColor = true;
            this.btEnviarCliente.Click += new System.EventHandler(this.btEnviarCliente_Click);
            // 
            // btEnviarServer
            // 
            this.btEnviarServer.Location = new System.Drawing.Point(174, 12);
            this.btEnviarServer.Name = "btEnviarServer";
            this.btEnviarServer.Size = new System.Drawing.Size(94, 23);
            this.btEnviarServer.TabIndex = 8;
            this.btEnviarServer.Text = "Enviar P Cliente";
            this.btEnviarServer.UseVisualStyleBackColor = true;
            this.btEnviarServer.Click += new System.EventHandler(this.btEnviarServer_Click);
            // 
            // txtClient
            // 
            this.txtClient.Location = new System.Drawing.Point(12, 67);
            this.txtClient.Multiline = true;
            this.txtClient.Name = "txtClient";
            this.txtClient.Size = new System.Drawing.Size(256, 71);
            this.txtClient.TabIndex = 7;
            // 
            // txtMsgServer
            // 
            this.txtMsgServer.Location = new System.Drawing.Point(12, 161);
            this.txtMsgServer.Multiline = true;
            this.txtMsgServer.Name = "txtMsgServer";
            this.txtMsgServer.Size = new System.Drawing.Size(256, 71);
            this.txtMsgServer.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Client";
            // 
            // bAssinar
            // 
            this.bAssinar.Location = new System.Drawing.Point(374, 13);
            this.bAssinar.Name = "bAssinar";
            this.bAssinar.Size = new System.Drawing.Size(75, 23);
            this.bAssinar.TabIndex = 12;
            this.bAssinar.Text = "Assinar";
            this.bAssinar.UseVisualStyleBackColor = true;
            this.bAssinar.Click += new System.EventHandler(this.bAssinar_Click);
            // 
            // txtOrders
            // 
            this.txtOrders.Location = new System.Drawing.Point(12, 255);
            this.txtOrders.Multiline = true;
            this.txtOrders.Name = "txtOrders";
            this.txtOrders.Size = new System.Drawing.Size(256, 71);
            this.txtOrders.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Orders";
            // 
            // bTransmit
            // 
            this.bTransmit.Location = new System.Drawing.Point(456, 13);
            this.bTransmit.Name = "bTransmit";
            this.bTransmit.Size = new System.Drawing.Size(75, 23);
            this.bTransmit.TabIndex = 15;
            this.bTransmit.Text = "Transmitir";
            this.bTransmit.UseVisualStyleBackColor = true;
            this.bTransmit.Click += new System.EventHandler(this.bTransmit_Click);
            // 
            // lblInicioMds
            // 
            this.lblInicioMds.AutoSize = true;
            this.lblInicioMds.Location = new System.Drawing.Point(12, 333);
            this.lblInicioMds.Name = "lblInicioMds";
            this.lblInicioMds.Size = new System.Drawing.Size(38, 13);
            this.lblInicioMds.TabIndex = 16;
            this.lblInicioMds.Text = "(vazio)";
            // 
            // lblFimMDS
            // 
            this.lblFimMDS.AutoSize = true;
            this.lblFimMDS.Location = new System.Drawing.Point(12, 359);
            this.lblFimMDS.Name = "lblFimMDS";
            this.lblFimMDS.Size = new System.Drawing.Size(38, 13);
            this.lblFimMDS.TabIndex = 17;
            this.lblFimMDS.Text = "(vazio)";
            // 
            // lblFimSocket
            // 
            this.lblFimSocket.AutoSize = true;
            this.lblFimSocket.Location = new System.Drawing.Point(13, 407);
            this.lblFimSocket.Name = "lblFimSocket";
            this.lblFimSocket.Size = new System.Drawing.Size(38, 13);
            this.lblFimSocket.TabIndex = 18;
            this.lblFimSocket.Text = "(vazio)";
            // 
            // lblInicioSocket
            // 
            this.lblInicioSocket.AutoSize = true;
            this.lblInicioSocket.Location = new System.Drawing.Point(13, 381);
            this.lblInicioSocket.Name = "lblInicioSocket";
            this.lblInicioSocket.Size = new System.Drawing.Size(38, 13);
            this.lblInicioSocket.TabIndex = 19;
            this.lblInicioSocket.Text = "(vazio)";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.averagePriceDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.clOrdIDDataGridViewTextBoxColumn,
            this.origClOrdIDDataGridViewTextBoxColumn,
            this.accountDataGridViewTextBoxColumn,
            this.symbolDataGridViewTextBoxColumn,
            this.ordStatusDataGridViewTextBoxColumn,
            this.ordTypeDataGridViewTextBoxColumn,
            this.registerTimeDataGridViewTextBoxColumn,
            this.transactTimeDataGridViewTextBoxColumn,
            this.timeInForceDataGridViewTextBoxColumn,
            this.expireDateDataGridViewTextBoxColumn,
            this.channelIDDataGridViewTextBoxColumn,
            this.exchangeDataGridViewTextBoxColumn,
            this.sideDataGridViewTextBoxColumn,
            this.orderQtyDataGridViewTextBoxColumn,
            this.orderQtyRemmainingDataGridViewTextBoxColumn,
            this.cumQtyDataGridViewTextBoxColumn,
            this.minQtyDataGridViewTextBoxColumn,
            this.maxFloorDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.stopPriceDataGridViewTextBoxColumn,
            this.compIDOMSDataGridViewTextBoxColumn,
            this.transmitidaDataGridViewCheckBoxColumn});
            this.dataGridView1.DataSource = this.ordemBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(274, 63);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(684, 473);
            this.dataGridView1.TabIndex = 20;
            // 
            // averagePriceDataGridViewTextBoxColumn
            // 
            this.averagePriceDataGridViewTextBoxColumn.DataPropertyName = "AveragePrice";
            this.averagePriceDataGridViewTextBoxColumn.HeaderText = "AveragePrice";
            this.averagePriceDataGridViewTextBoxColumn.Name = "averagePriceDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // clOrdIDDataGridViewTextBoxColumn
            // 
            this.clOrdIDDataGridViewTextBoxColumn.DataPropertyName = "ClOrdID";
            this.clOrdIDDataGridViewTextBoxColumn.HeaderText = "ClOrdID";
            this.clOrdIDDataGridViewTextBoxColumn.Name = "clOrdIDDataGridViewTextBoxColumn";
            // 
            // origClOrdIDDataGridViewTextBoxColumn
            // 
            this.origClOrdIDDataGridViewTextBoxColumn.DataPropertyName = "OrigClOrdID";
            this.origClOrdIDDataGridViewTextBoxColumn.HeaderText = "OrigClOrdID";
            this.origClOrdIDDataGridViewTextBoxColumn.Name = "origClOrdIDDataGridViewTextBoxColumn";
            // 
            // accountDataGridViewTextBoxColumn
            // 
            this.accountDataGridViewTextBoxColumn.DataPropertyName = "Account";
            this.accountDataGridViewTextBoxColumn.HeaderText = "Account";
            this.accountDataGridViewTextBoxColumn.Name = "accountDataGridViewTextBoxColumn";
            // 
            // symbolDataGridViewTextBoxColumn
            // 
            this.symbolDataGridViewTextBoxColumn.DataPropertyName = "Symbol";
            this.symbolDataGridViewTextBoxColumn.HeaderText = "Symbol";
            this.symbolDataGridViewTextBoxColumn.Name = "symbolDataGridViewTextBoxColumn";
            // 
            // ordStatusDataGridViewTextBoxColumn
            // 
            this.ordStatusDataGridViewTextBoxColumn.DataPropertyName = "OrdStatus";
            this.ordStatusDataGridViewTextBoxColumn.HeaderText = "OrdStatus";
            this.ordStatusDataGridViewTextBoxColumn.Name = "ordStatusDataGridViewTextBoxColumn";
            // 
            // ordTypeDataGridViewTextBoxColumn
            // 
            this.ordTypeDataGridViewTextBoxColumn.DataPropertyName = "OrdType";
            this.ordTypeDataGridViewTextBoxColumn.HeaderText = "OrdType";
            this.ordTypeDataGridViewTextBoxColumn.Name = "ordTypeDataGridViewTextBoxColumn";
            // 
            // registerTimeDataGridViewTextBoxColumn
            // 
            this.registerTimeDataGridViewTextBoxColumn.DataPropertyName = "RegisterTime";
            this.registerTimeDataGridViewTextBoxColumn.HeaderText = "RegisterTime";
            this.registerTimeDataGridViewTextBoxColumn.Name = "registerTimeDataGridViewTextBoxColumn";
            // 
            // transactTimeDataGridViewTextBoxColumn
            // 
            this.transactTimeDataGridViewTextBoxColumn.DataPropertyName = "TransactTime";
            this.transactTimeDataGridViewTextBoxColumn.HeaderText = "TransactTime";
            this.transactTimeDataGridViewTextBoxColumn.Name = "transactTimeDataGridViewTextBoxColumn";
            // 
            // timeInForceDataGridViewTextBoxColumn
            // 
            this.timeInForceDataGridViewTextBoxColumn.DataPropertyName = "TimeInForce";
            this.timeInForceDataGridViewTextBoxColumn.HeaderText = "TimeInForce";
            this.timeInForceDataGridViewTextBoxColumn.Name = "timeInForceDataGridViewTextBoxColumn";
            // 
            // expireDateDataGridViewTextBoxColumn
            // 
            this.expireDateDataGridViewTextBoxColumn.DataPropertyName = "ExpireDate";
            this.expireDateDataGridViewTextBoxColumn.HeaderText = "ExpireDate";
            this.expireDateDataGridViewTextBoxColumn.Name = "expireDateDataGridViewTextBoxColumn";
            // 
            // channelIDDataGridViewTextBoxColumn
            // 
            this.channelIDDataGridViewTextBoxColumn.DataPropertyName = "ChannelID";
            this.channelIDDataGridViewTextBoxColumn.HeaderText = "ChannelID";
            this.channelIDDataGridViewTextBoxColumn.Name = "channelIDDataGridViewTextBoxColumn";
            // 
            // exchangeDataGridViewTextBoxColumn
            // 
            this.exchangeDataGridViewTextBoxColumn.DataPropertyName = "Exchange";
            this.exchangeDataGridViewTextBoxColumn.HeaderText = "Exchange";
            this.exchangeDataGridViewTextBoxColumn.Name = "exchangeDataGridViewTextBoxColumn";
            // 
            // sideDataGridViewTextBoxColumn
            // 
            this.sideDataGridViewTextBoxColumn.DataPropertyName = "Side";
            this.sideDataGridViewTextBoxColumn.HeaderText = "Side";
            this.sideDataGridViewTextBoxColumn.Name = "sideDataGridViewTextBoxColumn";
            // 
            // orderQtyDataGridViewTextBoxColumn
            // 
            this.orderQtyDataGridViewTextBoxColumn.DataPropertyName = "OrderQty";
            this.orderQtyDataGridViewTextBoxColumn.HeaderText = "OrderQty";
            this.orderQtyDataGridViewTextBoxColumn.Name = "orderQtyDataGridViewTextBoxColumn";
            // 
            // orderQtyRemmainingDataGridViewTextBoxColumn
            // 
            this.orderQtyRemmainingDataGridViewTextBoxColumn.DataPropertyName = "OrderQtyRemmaining";
            this.orderQtyRemmainingDataGridViewTextBoxColumn.HeaderText = "OrderQtyRemmaining";
            this.orderQtyRemmainingDataGridViewTextBoxColumn.Name = "orderQtyRemmainingDataGridViewTextBoxColumn";
            // 
            // cumQtyDataGridViewTextBoxColumn
            // 
            this.cumQtyDataGridViewTextBoxColumn.DataPropertyName = "CumQty";
            this.cumQtyDataGridViewTextBoxColumn.HeaderText = "CumQty";
            this.cumQtyDataGridViewTextBoxColumn.Name = "cumQtyDataGridViewTextBoxColumn";
            // 
            // minQtyDataGridViewTextBoxColumn
            // 
            this.minQtyDataGridViewTextBoxColumn.DataPropertyName = "MinQty";
            this.minQtyDataGridViewTextBoxColumn.HeaderText = "MinQty";
            this.minQtyDataGridViewTextBoxColumn.Name = "minQtyDataGridViewTextBoxColumn";
            // 
            // maxFloorDataGridViewTextBoxColumn
            // 
            this.maxFloorDataGridViewTextBoxColumn.DataPropertyName = "MaxFloor";
            this.maxFloorDataGridViewTextBoxColumn.HeaderText = "MaxFloor";
            this.maxFloorDataGridViewTextBoxColumn.Name = "maxFloorDataGridViewTextBoxColumn";
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn.HeaderText = "Price";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            // 
            // stopPriceDataGridViewTextBoxColumn
            // 
            this.stopPriceDataGridViewTextBoxColumn.DataPropertyName = "StopPrice";
            this.stopPriceDataGridViewTextBoxColumn.HeaderText = "StopPrice";
            this.stopPriceDataGridViewTextBoxColumn.Name = "stopPriceDataGridViewTextBoxColumn";
            // 
            // compIDOMSDataGridViewTextBoxColumn
            // 
            this.compIDOMSDataGridViewTextBoxColumn.DataPropertyName = "CompIDOMS";
            this.compIDOMSDataGridViewTextBoxColumn.HeaderText = "CompIDOMS";
            this.compIDOMSDataGridViewTextBoxColumn.Name = "compIDOMSDataGridViewTextBoxColumn";
            // 
            // transmitidaDataGridViewCheckBoxColumn
            // 
            this.transmitidaDataGridViewCheckBoxColumn.DataPropertyName = "Transmitida";
            this.transmitidaDataGridViewCheckBoxColumn.HeaderText = "Transmitida";
            this.transmitidaDataGridViewCheckBoxColumn.Name = "transmitidaDataGridViewCheckBoxColumn";
            // 
            // ordemBindingSource
            // 
            this.ordemBindingSource.DataSource = typeof(Gradual.Spider.MockTest.Ordem);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 548);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblInicioSocket);
            this.Controls.Add(this.lblFimSocket);
            this.Controls.Add(this.lblFimMDS);
            this.Controls.Add(this.lblInicioMds);
            this.Controls.Add(this.bTransmit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtOrders);
            this.Controls.Add(this.bAssinar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btEnviarCliente);
            this.Controls.Add(this.btEnviarServer);
            this.Controls.Add(this.txtClient);
            this.Controls.Add(this.txtMsgServer);
            this.Controls.Add(this.btStartClient);
            this.Controls.Add(this.btStartServer);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ordemBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btStartClient;
        private System.Windows.Forms.Button btStartServer;
        private System.Windows.Forms.Button btEnviarCliente;
        private System.Windows.Forms.Button btEnviarServer;
        private System.Windows.Forms.TextBox txtClient;
        private System.Windows.Forms.TextBox txtMsgServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bAssinar;
        private System.Windows.Forms.TextBox txtOrders;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bTransmit;
        private System.Windows.Forms.Label lblInicioMds;
        private System.Windows.Forms.Label lblFimMDS;
        private System.Windows.Forms.Label lblFimSocket;
        private System.Windows.Forms.Label lblInicioSocket;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn averagePriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clOrdIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn origClOrdIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn registerTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeInForceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expireDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn channelIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sideDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderQtyRemmainingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cumQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxFloorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stopPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn compIDOMSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn transmitidaDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource ordemBindingSource;
    }
}


namespace WindowsFormsAcompanhamentoTst
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
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.listBox7 = new System.Windows.Forms.ListBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.listBox6 = new System.Windows.Forms.ListBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.orderIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origClOrdIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchangeNumberIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clOrdIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.symbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityExchangeIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stopStartIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordTypeIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordStatusIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registerTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expireDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeInForceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.channelIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.execBrokerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sideDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderQtyRemainingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxFloorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stopPxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cumQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fixMsgSeqNumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.systemIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.memoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sessionIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sessionIDOriginDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idSessionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msgFixDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msg42Base64DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handlInstDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.integrationNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchangeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spiderOrderInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spiderOrderInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(790, 169);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "GetOrders";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(18, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(75, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.listBox7);
            this.groupBox1.Controls.Add(this.textBox9);
            this.groupBox1.Controls.Add(this.listBox6);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.listBox5);
            this.groupBox1.Controls.Add(this.listBox4);
            this.groupBox1.Controls.Add(this.listBox3);
            this.groupBox1.Controls.Add(this.listBox2);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox7);
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(871, 212);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtro";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(787, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "HandlInst";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(692, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "OrderStatus";
            // 
            // listBox7
            // 
            this.listBox7.FormattingEnabled = true;
            this.listBox7.Location = new System.Drawing.Point(790, 68);
            this.listBox7.Name = "listBox7";
            this.listBox7.Size = new System.Drawing.Size(75, 95);
            this.listBox7.TabIndex = 24;
            this.listBox7.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox7_KeyDown);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(790, 42);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(75, 20);
            this.textBox9.TabIndex = 23;
            this.textBox9.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox9_KeyDown);
            // 
            // listBox6
            // 
            this.listBox6.FormattingEnabled = true;
            this.listBox6.Location = new System.Drawing.Point(695, 68);
            this.listBox6.Name = "listBox6";
            this.listBox6.Size = new System.Drawing.Size(75, 95);
            this.listBox6.TabIndex = 22;
            this.listBox6.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox6_KeyDown);
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(695, 42);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(75, 20);
            this.textBox8.TabIndex = 21;
            this.textBox8.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox8_KeyDown);
            // 
            // listBox5
            // 
            this.listBox5.FormattingEnabled = true;
            this.listBox5.Location = new System.Drawing.Point(600, 68);
            this.listBox5.Name = "listBox5";
            this.listBox5.Size = new System.Drawing.Size(75, 95);
            this.listBox5.TabIndex = 20;
            this.listBox5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox5_KeyDown);
            // 
            // listBox4
            // 
            this.listBox4.FormattingEnabled = true;
            this.listBox4.Location = new System.Drawing.Point(503, 68);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(75, 95);
            this.listBox4.TabIndex = 19;
            this.listBox4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox4_KeyDown);
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(406, 68);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(75, 95);
            this.listBox3.TabIndex = 18;
            this.listBox3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox3_KeyDown);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(115, 68);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(75, 95);
            this.listBox2.TabIndex = 17;
            this.listBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox2_KeyDown);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(18, 68);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(75, 95);
            this.listBox1.TabIndex = 16;
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(597, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Bolsa";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(500, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Sentido";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(403, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Sessão";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(306, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Data Final";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(209, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Data Inicial";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Ativo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Conta";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(600, 42);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(75, 20);
            this.textBox7.TabIndex = 8;
            this.textBox7.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox7_KeyDown);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(503, 42);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(75, 20);
            this.textBox6.TabIndex = 7;
            this.textBox6.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox6_KeyDown);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(406, 42);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(75, 20);
            this.textBox5.TabIndex = 6;
            this.textBox5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox5_KeyDown);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(309, 42);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(75, 20);
            this.textBox4.TabIndex = 5;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(212, 42);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(75, 20);
            this.textBox3.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(115, 42);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(75, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.orderIDDataGridViewTextBoxColumn,
            this.origClOrdIDDataGridViewTextBoxColumn,
            this.exchangeNumberIDDataGridViewTextBoxColumn,
            this.clOrdIDDataGridViewTextBoxColumn,
            this.accountDataGridViewTextBoxColumn,
            this.symbolDataGridViewTextBoxColumn,
            this.securityExchangeIDDataGridViewTextBoxColumn,
            this.stopStartIDDataGridViewTextBoxColumn,
            this.ordTypeIDDataGridViewTextBoxColumn,
            this.ordStatusIDDataGridViewTextBoxColumn,
            this.registerTimeDataGridViewTextBoxColumn,
            this.transactTimeDataGridViewTextBoxColumn,
            this.expireDateDataGridViewTextBoxColumn,
            this.timeInForceDataGridViewTextBoxColumn,
            this.channelIDDataGridViewTextBoxColumn,
            this.execBrokerDataGridViewTextBoxColumn,
            this.sideDataGridViewTextBoxColumn,
            this.orderQtyDataGridViewTextBoxColumn,
            this.orderQtyRemainingDataGridViewTextBoxColumn,
            this.minQtyDataGridViewTextBoxColumn,
            this.maxFloorDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.stopPxDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.cumQtyDataGridViewTextBoxColumn,
            this.fixMsgSeqNumDataGridViewTextBoxColumn,
            this.systemIDDataGridViewTextBoxColumn,
            this.memoDataGridViewTextBoxColumn,
            this.sessionIDDataGridViewTextBoxColumn,
            this.sessionIDOriginDataGridViewTextBoxColumn,
            this.idSessionDataGridViewTextBoxColumn,
            this.msgFixDataGridViewTextBoxColumn,
            this.msg42Base64DataGridViewTextBoxColumn,
            this.handlInstDataGridViewTextBoxColumn,
            this.integrationNameDataGridViewTextBoxColumn,
            this.exchangeDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.spiderOrderInfoBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(13, 230);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(864, 154);
            this.dataGridView1.TabIndex = 4;
            // 
            // orderIDDataGridViewTextBoxColumn
            // 
            this.orderIDDataGridViewTextBoxColumn.DataPropertyName = "OrderID";
            this.orderIDDataGridViewTextBoxColumn.HeaderText = "OrderID";
            this.orderIDDataGridViewTextBoxColumn.Name = "orderIDDataGridViewTextBoxColumn";
            // 
            // origClOrdIDDataGridViewTextBoxColumn
            // 
            this.origClOrdIDDataGridViewTextBoxColumn.DataPropertyName = "OrigClOrdID";
            this.origClOrdIDDataGridViewTextBoxColumn.HeaderText = "OrigClOrdID";
            this.origClOrdIDDataGridViewTextBoxColumn.Name = "origClOrdIDDataGridViewTextBoxColumn";
            // 
            // exchangeNumberIDDataGridViewTextBoxColumn
            // 
            this.exchangeNumberIDDataGridViewTextBoxColumn.DataPropertyName = "ExchangeNumberID";
            this.exchangeNumberIDDataGridViewTextBoxColumn.HeaderText = "ExchangeNumberID";
            this.exchangeNumberIDDataGridViewTextBoxColumn.Name = "exchangeNumberIDDataGridViewTextBoxColumn";
            // 
            // clOrdIDDataGridViewTextBoxColumn
            // 
            this.clOrdIDDataGridViewTextBoxColumn.DataPropertyName = "ClOrdID";
            this.clOrdIDDataGridViewTextBoxColumn.HeaderText = "ClOrdID";
            this.clOrdIDDataGridViewTextBoxColumn.Name = "clOrdIDDataGridViewTextBoxColumn";
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
            // securityExchangeIDDataGridViewTextBoxColumn
            // 
            this.securityExchangeIDDataGridViewTextBoxColumn.DataPropertyName = "SecurityExchangeID";
            this.securityExchangeIDDataGridViewTextBoxColumn.HeaderText = "SecurityExchangeID";
            this.securityExchangeIDDataGridViewTextBoxColumn.Name = "securityExchangeIDDataGridViewTextBoxColumn";
            // 
            // stopStartIDDataGridViewTextBoxColumn
            // 
            this.stopStartIDDataGridViewTextBoxColumn.DataPropertyName = "StopStartID";
            this.stopStartIDDataGridViewTextBoxColumn.HeaderText = "StopStartID";
            this.stopStartIDDataGridViewTextBoxColumn.Name = "stopStartIDDataGridViewTextBoxColumn";
            // 
            // ordTypeIDDataGridViewTextBoxColumn
            // 
            this.ordTypeIDDataGridViewTextBoxColumn.DataPropertyName = "OrdTypeID";
            this.ordTypeIDDataGridViewTextBoxColumn.HeaderText = "OrdTypeID";
            this.ordTypeIDDataGridViewTextBoxColumn.Name = "ordTypeIDDataGridViewTextBoxColumn";
            // 
            // ordStatusIDDataGridViewTextBoxColumn
            // 
            this.ordStatusIDDataGridViewTextBoxColumn.DataPropertyName = "OrdStatusID";
            this.ordStatusIDDataGridViewTextBoxColumn.HeaderText = "OrdStatusID";
            this.ordStatusIDDataGridViewTextBoxColumn.Name = "ordStatusIDDataGridViewTextBoxColumn";
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
            // expireDateDataGridViewTextBoxColumn
            // 
            this.expireDateDataGridViewTextBoxColumn.DataPropertyName = "ExpireDate";
            this.expireDateDataGridViewTextBoxColumn.HeaderText = "ExpireDate";
            this.expireDateDataGridViewTextBoxColumn.Name = "expireDateDataGridViewTextBoxColumn";
            // 
            // timeInForceDataGridViewTextBoxColumn
            // 
            this.timeInForceDataGridViewTextBoxColumn.DataPropertyName = "TimeInForce";
            this.timeInForceDataGridViewTextBoxColumn.HeaderText = "TimeInForce";
            this.timeInForceDataGridViewTextBoxColumn.Name = "timeInForceDataGridViewTextBoxColumn";
            // 
            // channelIDDataGridViewTextBoxColumn
            // 
            this.channelIDDataGridViewTextBoxColumn.DataPropertyName = "ChannelID";
            this.channelIDDataGridViewTextBoxColumn.HeaderText = "ChannelID";
            this.channelIDDataGridViewTextBoxColumn.Name = "channelIDDataGridViewTextBoxColumn";
            // 
            // execBrokerDataGridViewTextBoxColumn
            // 
            this.execBrokerDataGridViewTextBoxColumn.DataPropertyName = "ExecBroker";
            this.execBrokerDataGridViewTextBoxColumn.HeaderText = "ExecBroker";
            this.execBrokerDataGridViewTextBoxColumn.Name = "execBrokerDataGridViewTextBoxColumn";
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
            // orderQtyRemainingDataGridViewTextBoxColumn
            // 
            this.orderQtyRemainingDataGridViewTextBoxColumn.DataPropertyName = "OrderQtyRemaining";
            this.orderQtyRemainingDataGridViewTextBoxColumn.HeaderText = "OrderQtyRemaining";
            this.orderQtyRemainingDataGridViewTextBoxColumn.Name = "orderQtyRemainingDataGridViewTextBoxColumn";
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
            // stopPxDataGridViewTextBoxColumn
            // 
            this.stopPxDataGridViewTextBoxColumn.DataPropertyName = "StopPx";
            this.stopPxDataGridViewTextBoxColumn.HeaderText = "StopPx";
            this.stopPxDataGridViewTextBoxColumn.Name = "stopPxDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // cumQtyDataGridViewTextBoxColumn
            // 
            this.cumQtyDataGridViewTextBoxColumn.DataPropertyName = "CumQty";
            this.cumQtyDataGridViewTextBoxColumn.HeaderText = "CumQty";
            this.cumQtyDataGridViewTextBoxColumn.Name = "cumQtyDataGridViewTextBoxColumn";
            // 
            // fixMsgSeqNumDataGridViewTextBoxColumn
            // 
            this.fixMsgSeqNumDataGridViewTextBoxColumn.DataPropertyName = "FixMsgSeqNum";
            this.fixMsgSeqNumDataGridViewTextBoxColumn.HeaderText = "FixMsgSeqNum";
            this.fixMsgSeqNumDataGridViewTextBoxColumn.Name = "fixMsgSeqNumDataGridViewTextBoxColumn";
            // 
            // systemIDDataGridViewTextBoxColumn
            // 
            this.systemIDDataGridViewTextBoxColumn.DataPropertyName = "SystemID";
            this.systemIDDataGridViewTextBoxColumn.HeaderText = "SystemID";
            this.systemIDDataGridViewTextBoxColumn.Name = "systemIDDataGridViewTextBoxColumn";
            // 
            // memoDataGridViewTextBoxColumn
            // 
            this.memoDataGridViewTextBoxColumn.DataPropertyName = "Memo";
            this.memoDataGridViewTextBoxColumn.HeaderText = "Memo";
            this.memoDataGridViewTextBoxColumn.Name = "memoDataGridViewTextBoxColumn";
            // 
            // sessionIDDataGridViewTextBoxColumn
            // 
            this.sessionIDDataGridViewTextBoxColumn.DataPropertyName = "SessionID";
            this.sessionIDDataGridViewTextBoxColumn.HeaderText = "SessionID";
            this.sessionIDDataGridViewTextBoxColumn.Name = "sessionIDDataGridViewTextBoxColumn";
            // 
            // sessionIDOriginDataGridViewTextBoxColumn
            // 
            this.sessionIDOriginDataGridViewTextBoxColumn.DataPropertyName = "SessionIDOrigin";
            this.sessionIDOriginDataGridViewTextBoxColumn.HeaderText = "SessionIDOrigin";
            this.sessionIDOriginDataGridViewTextBoxColumn.Name = "sessionIDOriginDataGridViewTextBoxColumn";
            // 
            // idSessionDataGridViewTextBoxColumn
            // 
            this.idSessionDataGridViewTextBoxColumn.DataPropertyName = "IdSession";
            this.idSessionDataGridViewTextBoxColumn.HeaderText = "IdSession";
            this.idSessionDataGridViewTextBoxColumn.Name = "idSessionDataGridViewTextBoxColumn";
            // 
            // msgFixDataGridViewTextBoxColumn
            // 
            this.msgFixDataGridViewTextBoxColumn.DataPropertyName = "MsgFix";
            this.msgFixDataGridViewTextBoxColumn.HeaderText = "MsgFix";
            this.msgFixDataGridViewTextBoxColumn.Name = "msgFixDataGridViewTextBoxColumn";
            // 
            // msg42Base64DataGridViewTextBoxColumn
            // 
            this.msg42Base64DataGridViewTextBoxColumn.DataPropertyName = "Msg42Base64";
            this.msg42Base64DataGridViewTextBoxColumn.HeaderText = "Msg42Base64";
            this.msg42Base64DataGridViewTextBoxColumn.Name = "msg42Base64DataGridViewTextBoxColumn";
            // 
            // handlInstDataGridViewTextBoxColumn
            // 
            this.handlInstDataGridViewTextBoxColumn.DataPropertyName = "HandlInst";
            this.handlInstDataGridViewTextBoxColumn.HeaderText = "HandlInst";
            this.handlInstDataGridViewTextBoxColumn.Name = "handlInstDataGridViewTextBoxColumn";
            // 
            // integrationNameDataGridViewTextBoxColumn
            // 
            this.integrationNameDataGridViewTextBoxColumn.DataPropertyName = "IntegrationName";
            this.integrationNameDataGridViewTextBoxColumn.HeaderText = "IntegrationName";
            this.integrationNameDataGridViewTextBoxColumn.Name = "integrationNameDataGridViewTextBoxColumn";
            // 
            // exchangeDataGridViewTextBoxColumn
            // 
            this.exchangeDataGridViewTextBoxColumn.DataPropertyName = "Exchange";
            this.exchangeDataGridViewTextBoxColumn.HeaderText = "Exchange";
            this.exchangeDataGridViewTextBoxColumn.Name = "exchangeDataGridViewTextBoxColumn";
            // 
            // spiderOrderInfoBindingSource
            // 
            this.spiderOrderInfoBindingSource.DataSource = typeof(Gradual.Core.Spider.AcompanhamentoOrdens.Lib.Dados.SpiderOrderInfo);
            this.spiderOrderInfoBindingSource.CurrentChanged += new System.EventHandler(this.spiderOrderInfoBindingSource_CurrentChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(544, 429);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Monitor Init";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(625, 429);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Monitor Stop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 464);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spiderOrderInfoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn origClOrdIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangeNumberIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clOrdIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn securityExchangeIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stopStartIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordTypeIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordStatusIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn registerTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expireDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeInForceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn channelIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn execBrokerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sideDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderQtyRemainingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxFloorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stopPxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cumQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fixMsgSeqNumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn systemIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn memoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionIDOriginDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idSessionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn msgFixDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn msg42Base64DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handlInstDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn integrationNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangeDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource spiderOrderInfoBindingSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox5;
        private System.Windows.Forms.ListBox listBox7;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.ListBox listBox6;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
    }
}


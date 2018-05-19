namespace Gradual.OMS.MonitorServicos
{
    partial class frmMonitorServicos
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.clmnStatus = new System.Windows.Forms.DataGridViewImageColumn();
            this.clmServiceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnEndpoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStopStart = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmnStatus,
            this.clmServiceName,
            this.clmnEndpoint,
            this.clmStopStart});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(653, 228);
            this.dataGridView1.TabIndex = 0;
            // 
            // clmnStatus
            // 
            this.clmnStatus.HeaderText = "Status";
            this.clmnStatus.Name = "clmnStatus";
            // 
            // clmServiceName
            // 
            this.clmServiceName.HeaderText = "Service Name";
            this.clmServiceName.Name = "clmServiceName";
            // 
            // clmnEndpoint
            // 
            this.clmnEndpoint.HeaderText = "Endpoint";
            this.clmnEndpoint.Name = "clmnEndpoint";
            // 
            // clmStopStart
            // 
            this.clmStopStart.HeaderText = "Stop/Start";
            this.clmStopStart.Name = "clmStopStart";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 452);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMonitorServicos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn clmnStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmServiceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmnEndpoint;
        private System.Windows.Forms.DataGridViewButtonColumn clmStopStart;
    }
}


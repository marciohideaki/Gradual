namespace Gradual.OMS.InvXX.Fundos.Teste
{
    partial class frmImportacaoANBIMA
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
            this.btnImportacao = new System.Windows.Forms.Button();
            this.btnImportar = new System.Windows.Forms.Button();
            this.txtCodigoAnbima = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.btnImportarTodos = new System.Windows.Forms.Button();
            this.btnImportarAvulsos = new System.Windows.Forms.Button();
            this.btnTestarHistorico = new System.Windows.Forms.Button();
            this.btnImportarTodosFundos = new System.Windows.Forms.Button();
            this.btnImportarTodosFundosANBIMA = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImportacao
            // 
            this.btnImportacao.Location = new System.Drawing.Point(13, 13);
            this.btnImportacao.Name = "btnImportacao";
            this.btnImportacao.Size = new System.Drawing.Size(223, 23);
            this.btnImportacao.TabIndex = 0;
            this.btnImportacao.Text = "Testar Importação ANBIMA";
            this.btnImportacao.UseVisualStyleBackColor = true;
            this.btnImportacao.Click += new System.EventHandler(this.btnImportacao_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.Location = new System.Drawing.Point(169, 99);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(75, 23);
            this.btnImportar.TabIndex = 1;
            this.btnImportar.Text = "Importar";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // txtCodigoAnbima
            // 
            this.txtCodigoAnbima.Location = new System.Drawing.Point(91, 75);
            this.txtCodigoAnbima.Name = "txtCodigoAnbima";
            this.txtCodigoAnbima.Size = new System.Drawing.Size(75, 20);
            this.txtCodigoAnbima.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Codigo Anbima:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Data Inicial:";
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(91, 101);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(75, 20);
            this.txtDate.TabIndex = 4;
            // 
            // btnImportarTodos
            // 
            this.btnImportarTodos.Location = new System.Drawing.Point(13, 127);
            this.btnImportarTodos.Name = "btnImportarTodos";
            this.btnImportarTodos.Size = new System.Drawing.Size(153, 38);
            this.btnImportarTodos.TabIndex = 6;
            this.btnImportarTodos.Text = "Importar Fundo com Código Anbima";
            this.btnImportarTodos.UseVisualStyleBackColor = true;
            this.btnImportarTodos.Click += new System.EventHandler(this.btnImportarTodos_Click);
            // 
            // btnImportarAvulsos
            // 
            this.btnImportarAvulsos.Location = new System.Drawing.Point(13, 190);
            this.btnImportarAvulsos.Name = "btnImportarAvulsos";
            this.btnImportarAvulsos.Size = new System.Drawing.Size(153, 37);
            this.btnImportarAvulsos.TabIndex = 7;
            this.btnImportarAvulsos.Text = "Importar arquivos avulsos";
            this.btnImportarAvulsos.UseVisualStyleBackColor = true;
            this.btnImportarAvulsos.Click += new System.EventHandler(this.btnImportarAvulsos_Click);
            // 
            // btnTestarHistorico
            // 
            this.btnTestarHistorico.Location = new System.Drawing.Point(13, 268);
            this.btnTestarHistorico.Name = "btnTestarHistorico";
            this.btnTestarHistorico.Size = new System.Drawing.Size(153, 23);
            this.btnTestarHistorico.TabIndex = 8;
            this.btnTestarHistorico.Text = "Historico de Cotas";
            this.btnTestarHistorico.UseVisualStyleBackColor = true;
            this.btnTestarHistorico.Click += new System.EventHandler(this.btnTestarHistorico_Click);
            // 
            // btnImportarTodosFundos
            // 
            this.btnImportarTodosFundos.Location = new System.Drawing.Point(13, 318);
            this.btnImportarTodosFundos.Name = "btnImportarTodosFundos";
            this.btnImportarTodosFundos.Size = new System.Drawing.Size(153, 38);
            this.btnImportarTodosFundos.TabIndex = 9;
            this.btnImportarTodosFundos.Text = "Importar Todos os Fundos DB";
            this.btnImportarTodosFundos.UseVisualStyleBackColor = true;
            this.btnImportarTodosFundos.Click += new System.EventHandler(this.btnImportarTodosFundos_Click);
            // 
            // btnImportarTodosFundosANBIMA
            // 
            this.btnImportarTodosFundosANBIMA.Location = new System.Drawing.Point(13, 370);
            this.btnImportarTodosFundosANBIMA.Name = "btnImportarTodosFundosANBIMA";
            this.btnImportarTodosFundosANBIMA.Size = new System.Drawing.Size(153, 38);
            this.btnImportarTodosFundosANBIMA.TabIndex = 10;
            this.btnImportarTodosFundosANBIMA.Text = "Importar Todos os Fundos ANBIMA";
            this.btnImportarTodosFundosANBIMA.UseVisualStyleBackColor = true;
            this.btnImportarTodosFundosANBIMA.Click += new System.EventHandler(this.btnImportarTodosFundosANBIMA_Click);
            // 
            // frmImportacaoANBIMA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 420);
            this.Controls.Add(this.btnImportarTodosFundosANBIMA);
            this.Controls.Add(this.btnImportarTodosFundos);
            this.Controls.Add(this.btnTestarHistorico);
            this.Controls.Add(this.btnImportarAvulsos);
            this.Controls.Add(this.btnImportarTodos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCodigoAnbima);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnImportacao);
            this.Name = "frmImportacaoANBIMA";
            this.Text = "Importação ANBIMA";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImportacao;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.TextBox txtCodigoAnbima;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Button btnImportarTodos;
        private System.Windows.Forms.Button btnImportarAvulsos;
        private System.Windows.Forms.Button btnTestarHistorico;
        private System.Windows.Forms.Button btnImportarTodosFundos;
        private System.Windows.Forms.Button btnImportarTodosFundosANBIMA;
    }
}


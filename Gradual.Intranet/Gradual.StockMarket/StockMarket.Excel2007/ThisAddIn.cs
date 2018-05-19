using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Microsoft.Office.Tools.Excel.Extensions;
using System.Windows.Forms;
using System.Diagnostics;
using MdsBayeuxClient;
using StockMarket.Excel2007.Classes;
using System.IO;
using System.Threading;

namespace StockMarket.Excel2007
{
    public partial class ThisAddIn
    {
        private class RangeCoordinates
        {
            public int qtdRegistros { get; set; }
            public int qtdRegistrosAnterior { get; set; }
            public int colStart { get; set; }
            public int rowStart { get; set; }
            public int colEnd { get; set; }
            public int rowEnd { get; set; }

            public RangeCoordinates(int qtdRegistros, int qtdRegistrosAnterior, int colStart, int rowStart, int colEnd, int rowEnd)
            {
                this.qtdRegistros = qtdRegistros;
                this.qtdRegistrosAnterior = qtdRegistrosAnterior;
                this.colStart = colStart;
                this.rowStart = rowStart;
                this.colEnd = colEnd;
                this.rowEnd = rowEnd;
            }
        }

        public RTD.ComRTDInfo comRTDInfo = null;

        protected override object RequestComAddInAutomationService()
        {
            if (comRTDInfo == null)
                comRTDInfo = new RTD.ComRTDInfo();

            return comRTDInfo;
        }

        #region Globais

        #endregion

        #region Métodos Private

        private System.Windows.Forms.Timer gTimerDeRecalculo;
        private string gUsuario = "";
        private int gQtdRegistrosPosicaoNet = 0;

        private void GravaLogDebug(string mensagem)
        {
            using (StreamWriter streamWriter = new StreamWriter("c:\\Temp\\StockMarket.log", true))
            {
                streamWriter.Write(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss,fff") + " [ThisAddIn]: ");
                streamWriter.WriteLine(mensagem);
            }
        }

        private void IniciarTimer()
        {
            try
            {
                gTimerDeRecalculo = new System.Windows.Forms.Timer();
                gTimerDeRecalculo.Interval = 1000;
                gTimerDeRecalculo.Tick += new EventHandler(gTimerDeRecalculo_Tick);
                gTimerDeRecalculo.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em IniciarTimer: {0}", ex.Message));
            }
        }

        #endregion

        #region Métodos Private - Interface

        private void Interface_MontarMonitorDeCotacoes(string pInstrumento)
        {
            try
            {
                int lRow, lCol;

                lRow = Application.ActiveCell.Row;
                lCol = Application.ActiveCell.Column;

                if (Application.ActiveSheet != null)
                {
                    object v2, f, fa;

                    if (lRow > 1)
                    {
                        v2 = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Value2;
                        f  = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Formula;
                        fa = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).FormulaArray;

                        // Monta o cabecalho das cotacoes, se a célula de cima estiver vazia
                        if (v2 == null && f.ToString() == "" && fa.ToString() == "")
                        {
                            lRow--;
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  0]).Value2 = "Papel";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  1]).Value2 = "Última";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  2]).Value2 = "Var (%)";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  3]).Value2 = "Média";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  4]).Value2 = "Corr Compr";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  5]).Value2 = "Qtd Compr";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  6]).Value2 = "Vl Compr";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  7]).Value2 = "Vl Venda";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  8]).Value2 = "Qtd Venda";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  9]).Value2 = "Corr Venda";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 10]).Value2 = "Abertura";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 11]).Value2 = "Mínima";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 12]).Value2 = "Máxima";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).Value2 = "Fech. Ant.";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 14]).Value2 = "Qtd Total";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 15]).Value2 = "N. Neg";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 16]).Value2 = "Volume";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 17]).Value2 = "Qtd Teor";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 18]).Value2 = "Preço Teor";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 19]).Value2 = "Preço Exer";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 20]).Value2 = "Data Vencto";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 21]).Value2 = "Hora";
                            lRow++;
                        }

                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  0]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_INSTRUMENTO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_ULTIMO_PRECO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  2]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_VARIACAO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_MEDIO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  4]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_CORRETORA_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  5]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  6]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  7]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  8]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  9]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_CORRETORA_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 10]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_ABERTURA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 11]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_MINIMO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 12]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_MAXIMO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_FECHAMENTO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 14]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_TOTAL_PAPEIS);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 15]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_NEGOCIOS);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 16]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_VOLUME);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 17]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_TEORICA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 18]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_TEORICO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 19]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_EXERCICIO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 20]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_DATA_VENCIMENTO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 21]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, pInstrumento, Funcoes.PROPRIEDADE_HORA);
                    }
                    
                    Excel.Range lRangeMonitor;

                    lRangeMonitor = ((Excel.Worksheet)Application.ActiveSheet).Range[((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 0],
                                                                                     ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 21]];

                    //formatação qtdtotal e volume
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 14]).NumberFormatLocal = "[>=1000000]0..\"M\";[>=1000]0.\"K\";0";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 16]).NumberFormatLocal = "[>=1000000]0..\"M\";[>=1000]0.\"K\";0";

                    // Coloca a última célula com formatação de data
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 20]).NumberFormat = "HH:mm:ss";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 21]).NumberFormat = "HH:mm:ss";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em : {0}", ex.Message));
            }
        }

        private void Interface_MontarMonitorDePosicoes(string cliente)
        {
            try
            {
                int lRow = Application.ActiveCell.Row;
                int lCol = Application.ActiveCell.Column;

                if (Application.ActiveSheet != null)
                {
                    if (lRow > 1)
                    {
                        object v2 = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Value2;
                        object f = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).Formula;
                        object fa = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow - 1, lCol + 0]).FormulaArray;

                        // Monta o cabecalho das cotacoes, se a célula de cima estiver vazia
                        if (v2 == null && f.ToString() == "" && fa.ToString() == "")
                        {
                            lRow--;
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Value2 = "Cliente";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 1]).Value2 = "Papel";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 2]).Value2 = "Qtd.Exec.Cpa";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 3]).Value2 = "Qtd.Exec.Vda";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 4]).Value2 = "Net Exec.";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 5]).Value2 = "Pr.Med.Exec.Cpa";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 6]).Value2 = "Pr.Med.Exec.Vda";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 7]).Value2 = "Qtd.Ab.Cpa";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 8]).Value2 = "Qtd.Ab.Vda";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 9]).Value2 = "Net Ab.";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 10]).Value2 = "Pr.Med.Ab.Cpa";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 11]).Value2 = "Pr.Med.Ab.Vda";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 12]).Value2 = "Vl.Fin.Net Ab.";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).Value2 = "Vl.Fin.Net Exec.";
                            ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 14]).Value2 = "Total Regs";
                            lRow++;
                        }

                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_CLIENTE);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_ATIVO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 2]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_QTD_EXEC_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_QTD_EXEC_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 4]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_NET_EXEC);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 5]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_PRECO_MEDIO_EXEC_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 6]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_PRECO_MEDIO_EXEC_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 7]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_QTD_ABERTA_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 8]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_QTD_ABERTA_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 9]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_NET_ABERTA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 10]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_PRECO_MEDIO_ABERTA_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 11]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_PRECO_MEDIO_ABERTA_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 12]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_VOLUMEFIN_NET_ABERTA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_VOLUMEFIN_NET_EXEC);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 14]).Value2 = string.Format("={0}(\"{1}.{2}.1\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_TOTAL_REGISTROS);
                    }

                    /*
                    Excel.Range lRangeMonitor = ((Excel.Worksheet)Application.ActiveSheet).Range[
                        ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol],
                        ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 14]];

                    string lFormula = string.Format("={0}(\"{1}\")", Funcoes.FUNCAO_POSICAO_NET, cliente);

                    lRangeMonitor.FormulaArray = lFormula;

                    lRangeMonitor.Dirty();
                    lRangeMonitor.Calculate();
                    */
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em : {0}", ex.Message));
            }
        }

        private void Interface_MontarCarteira(string[] pAtivos)
        {
            try
            {
                int lRow = Application.ActiveCell.Row;
                int lCol = Application.ActiveCell.Column;

                if (Application.ActiveSheet != null)
                {
                    // monta os cabecalho da tabela:
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Value2 = "Papel";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 1]).Value2 = "Última";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 2]).Value2 = "Var (%)";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 3]).Value2 = "Média";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 4]).Value2 = "Corr Compr";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 5]).Value2 = "Qtd Compr";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 6]).Value2 = "Vl Compr";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 7]).Value2 = "Vl Venda";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 8]).Value2 = "Qtd Venda";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 9]).Value2 = "Corr Venda";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 10]).Value2 = "Abertura";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 11]).Value2 = "Mínima";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 12]).Value2 = "Máxima";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).Value2 = "Fech. Ant.";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 14]).Value2 = "Qtd Total";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 15]).Value2 = "N. Neg";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 16]).Value2 = "Volume";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 17]).Value2 = "Qtd Teor";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 18]).Value2 = "Preço Teor";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 19]).Value2 = "Preço Exer";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 20]).Value2 = "Data Vencto";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 21]).Value2 = "Hora";

                    // seleciona o range do "corpo" da tabela e coloca a fórmula:

                    foreach (string lAtivo in pAtivos)
                    {
                        lRow++;
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  0]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_INSTRUMENTO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_ULTIMO_PRECO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  2]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_VARIACAO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_PRECO_MEDIO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  4]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_CORRETORA_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  5]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_QUANTIDADE_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  6]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_PRECO_COMPRA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  7]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_PRECO_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  8]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_QUANTIDADE_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol +  9]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_CORRETORA_VENDA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 10]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_PRECO_ABERTURA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 11]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_PRECO_MINIMO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 12]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_PRECO_MAXIMO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 13]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_PRECO_FECHAMENTO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 14]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_QUANTIDADE_TOTAL_PAPEIS);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 15]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_QUANTIDADE_NEGOCIOS);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 16]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_VOLUME);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 17]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_QUANTIDADE_TEORICA);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 18]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_PRECO_TEORICO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 19]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_PRECO_EXERCICIO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 20]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_DATA_VENCIMENTO);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 21]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_LINHA, lAtivo, Funcoes.PROPRIEDADE_HORA);

                        Excel.Range lRangeMonitor = ((Excel.Worksheet)Application.ActiveSheet).Range[
                            ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 0],
                            ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 21]];

                        // Formatação pro qtdtotal e volume
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 14]).NumberFormatLocal = "[>=1000000]0..\"M\";[>=1000]0.\"K\";0";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 16]).NumberFormatLocal = "[>=1000000]0..\"M\";[>=1000]0.\"K\";0";

                        // Coloca a última célula com formatação de data
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 20]).NumberFormat = "HH:mm:ss";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 21]).NumberFormat = "HH:mm:ss";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em Interface_MontarCarteira: {0}", ex.Message));
            }
        }

        private void Interface_MontarTickerDeCotacaoRapida(string pInstrumento)
        {
            try
            {
                int lRow = Application.ActiveCell.Row;
                int lCol = Application.ActiveCell.Column;

                if (Application.ActiveSheet != null)
                {
                    // Monta o cabecalho se a célula de cima estiver vazia
                    object v2 = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Value2;
                    object f = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Formula;
                    object fa = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).FormulaArray;

                    if (v2 == null && f.ToString() == "" && fa.ToString() == "")
                    {
                        // Título "<PAPEL>"
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Value2 = 
                            string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_INSTRUMENTO);

                        Excel.Range lRangeTitulo = ((Excel.Worksheet)Application.ActiveSheet).Range[
                            ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0], 
                            ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 3]];
                        lRangeTitulo.Merge(false);
                        lRangeTitulo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    }

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 0]).Value2 = "Última";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_ULTIMO_PRECO);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 2]).Value2 = "Var(%)";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_VARIACAO);

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 2, lCol + 0]).Value2 = "Compra";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 2, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_COMPRA);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 2, lCol + 2]).Value2 = "Venda";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 2, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_VENDA);

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 3, lCol + 0]).Value2 = "Qtd Compra";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 3, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_COMPRA);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 3, lCol + 2]).Value2 = "Qtd Venda";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 3, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_VENDA);

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 4, lCol + 0]).Value2 = "Corr Compra";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 4, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_CORRETORA_COMPRA);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 4, lCol + 2]).Value2 = "Corr Venda";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 4, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_CORRETORA_VENDA);

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 5, lCol + 0]).Value2 = "Max";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 5, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_MAXIMO);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 5, lCol + 2]).Value2 = "Min";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 5, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_MINIMO);

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 6, lCol + 0]).Value2 = "Aber";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 6, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_ABERTURA);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 6, lCol + 2]).Value2 = "Fech";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 6, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_FECHAMENTO);

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 7, lCol + 0]).Value2 = "Media";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 7, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_MEDIO);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 7, lCol + 2]).Value2 = "Qtd Total";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 7, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_TOTAL_PAPEIS);

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 8, lCol + 0]).Value2 = "Nr. Neg";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 8, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_NEGOCIOS);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 8, lCol + 2]).Value2 = "Vol";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 8, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_VOLUME);

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 9, lCol + 0]).Value2 = "Qtd Teor";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 9, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_QUANTIDADE_TEORICA);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 9, lCol + 2]).Value2 = "Prc Teor";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 9, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_TEORICO);

                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 10, lCol + 0]).Value2 = "Prc Exer";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 10, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_PRECO_EXERCICIO);
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 10, lCol + 2]).Value2 = "Data Vencto";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 10, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_COTACAO_RAPIDA, pInstrumento, Funcoes.PROPRIEDADE_DATA_VENCIMENTO);

                    // Seleciona o range do "corpo" da tabela e coloca a fórmula
                    //Excel.Range lRangeTicker;
                    //lRangeTicker = ((Excel.Worksheet)Application.ActiveSheet).Range[((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 0],
                    //                                                                ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 9, lCol + 3]];

                    // Formatação pro qtdtotal e volume
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 7, lCol + 3]).NumberFormatLocal = 
                        "[>=1000000]0..\"M\";[>=1000]0.\"K\";0";
                    ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 8, lCol + 3]).NumberFormatLocal = 
                        "[>=1000000]0..\"M\";[>=1000]0.\"K\";0";

                    //lRangeTicker.FormulaArray = "=SM_COTACAORAPIDA(\"" + pInstrumento + "\")";

                    //lRangeTicker.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(204, 67, 193));

                    //lRangeTicker.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(204, 67, 193));

                    //lRangeTicker.Calculate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em Interface_MontarTickerDeCotacao: {0}", ex.Message));
            }
        }

        private void Interface_MontarLivroDeOfertas(string pInstrumento)
        {
            try
            {
                int lRow = Application.ActiveCell.Row;
                int lCol = Application.ActiveCell.Column;

                if (Application.ActiveSheet != null)
                {
                    object v2 = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Value2;
                    object f = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Formula;
                    object fa = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).FormulaArray;

                    if (v2 == null && f.ToString() == "" && fa.ToString() == "")
                    {
                        // Título "Livro de Ofertas - <PAPEL>"
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0]).Value2 = 
                            string.Format("={0}(\"{1}.{2}\")", Funcoes.FUNCAO_LIVRO_OFERTAS, pInstrumento, Funcoes.PROPRIEDADE_OFERTA_ATIVO);

                        Excel.Range lRangeTitulo = ((Excel.Worksheet)Application.ActiveSheet).Range[
                            ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 0], 
                            ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow, lCol + 5]];
                        lRangeTitulo.Merge(false);
                        lRangeTitulo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        // Cabeçalho da tabela
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 0]).Value2 = "Corr.";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 1]).Value2 = "Qtd.";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 2]).Value2 = "OFC";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 3]).Value2 = "OFV";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 4]).Value2 = "Qtd";
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1, lCol + 5]).Value2 = "Corr.";
                    }

                    // seleciona o range do "corpo" da tabela e coloca as fórmulas
                    for (int ocorrencia = 1; ocorrencia <= 10; ocorrencia++)
                    {
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1 + ocorrencia, lCol + 0]).Value2 = string.Format("={0}(\"{1}.{2}.{3}.{4}\")", Funcoes.FUNCAO_LIVRO_OFERTAS, pInstrumento, Funcoes.PROPRIEDADE_OFERTA_CORRETORA, Funcoes.PROPRIEDADE_OFERTA_SENTIDO_COMPRA, ocorrencia);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1 + ocorrencia, lCol + 1]).Value2 = string.Format("={0}(\"{1}.{2}.{3}.{4}\")", Funcoes.FUNCAO_LIVRO_OFERTAS, pInstrumento, Funcoes.PROPRIEDADE_OFERTA_QUANTIDADE, Funcoes.PROPRIEDADE_OFERTA_SENTIDO_COMPRA, ocorrencia);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1 + ocorrencia, lCol + 2]).Value2 = string.Format("={0}(\"{1}.{2}.{3}.{4}\")", Funcoes.FUNCAO_LIVRO_OFERTAS, pInstrumento, Funcoes.PROPRIEDADE_OFERTA_PRECO, Funcoes.PROPRIEDADE_OFERTA_SENTIDO_COMPRA, ocorrencia);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1 + ocorrencia, lCol + 3]).Value2 = string.Format("={0}(\"{1}.{2}.{3}.{4}\")", Funcoes.FUNCAO_LIVRO_OFERTAS, pInstrumento, Funcoes.PROPRIEDADE_OFERTA_PRECO, Funcoes.PROPRIEDADE_OFERTA_SENTIDO_VENDA, ocorrencia);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1 + ocorrencia, lCol + 4]).Value2 = string.Format("={0}(\"{1}.{2}.{3}.{4}\")", Funcoes.FUNCAO_LIVRO_OFERTAS, pInstrumento, Funcoes.PROPRIEDADE_OFERTA_QUANTIDADE, Funcoes.PROPRIEDADE_OFERTA_SENTIDO_VENDA, ocorrencia);
                        ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 1 + ocorrencia, lCol + 5]).Value2 = string.Format("={0}(\"{1}.{2}.{3}.{4}\")", Funcoes.FUNCAO_LIVRO_OFERTAS, pInstrumento, Funcoes.PROPRIEDADE_OFERTA_CORRETORA, Funcoes.PROPRIEDADE_OFERTA_SENTIDO_VENDA, ocorrencia);
                    }

                    /*
                    Excel.Range lRangeLivro;
                    lRangeLivro = ((Excel.Worksheet)Application.ActiveSheet).Range[ ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 0, lCol + 0],
                                                                                    ((Excel.Worksheet)Application.ActiveSheet).Cells[lRow + 9, lCol + 5]];

                    lRangeLivro.FormulaArray = "=SM_LIVROOFERTAS(\"" + pInstrumento + "\")";

                    lRangeLivro.Calculate();
                    */
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em Interface_MontarLivroDeOfertas: {0}", ex.Message));
            }
        }

        #endregion

        #region Event Handlers

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Globals.Ribbons.ribStockMarket.UsuarioLogado += new UsuarioLogadoEventHandler(ribGradualStockMarket_UsuarioLogado);

            Globals.Ribbons.ribStockMarket.UsuarioDeslogado += new UsuarioDeslogadoEventHandler(ribGradualStockMarket_UsuarioDeslogado);

            Globals.Ribbons.ribStockMarket.RealizarAcao += new RealizarAcaoEventHandler(ribGradualStockMarket_RealizarAcao);
        }

        void AtualizarPosicaoNet(string cliente)
        {
            try
            {
                Excel.Range celulasEmUso = ((Excel.Worksheet)Application.ActiveSheet).UsedRange;

                // Se existir SM_POSICAO_NET, atualiza todos os ranges existentes com a PosicaoNet atual
                Excel.Range celulaComFormula = celulasEmUso.Find(Funcoes.FUNCAO_POSICAO_NET, missing, Excel.XlFindLookIn.xlFormulas, missing, missing, Excel.XlSearchDirection.xlNext, missing, missing, missing);
                if (celulaComFormula != null)
                {
                    List<RangeCoordinates> listaRanges = new List<RangeCoordinates>();

                    int colStart = celulaComFormula.Column;
                    int rowStart = celulaComFormula.Row;
                    do
                    {
                        object celula = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).
                            Cells[celulaComFormula.Row, celulaComFormula.Column]).Value2;
                        if (celula.GetType() == typeof(string))
                        {
                            if (((string)celula).StartsWith(" "))
                            {
                                int qtdRegistros = (int)((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).
                                    Cells[celulaComFormula.Row, celulaComFormula.Column + 14]).Value2;
                                
                                int qtdRegistrosAnterior;
                                for (qtdRegistrosAnterior = 1; qtdRegistrosAnterior < 1000; qtdRegistrosAnterior++)
                                {
                                    object formula = ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).
                                        Cells[celulaComFormula.Row + qtdRegistrosAnterior, celulaComFormula.Column]).Formula;
                                    if (formula == null || formula.GetType() != typeof(string) || !formula.ToString().Contains(Funcoes.FUNCAO_POSICAO_NET))
                                        break;
                                }

                                listaRanges.Add(new RangeCoordinates(
                                    qtdRegistros, 
                                    qtdRegistrosAnterior,
                                    celulaComFormula.Column, 
                                    celulaComFormula.Row, 
                                    celulaComFormula.Column + 14, 
                                    celulaComFormula.Row + qtdRegistrosAnterior - 1));
                            }
                        }
                        celulaComFormula = celulasEmUso.FindNext(celulaComFormula);
                    }
                    while (colStart != celulaComFormula.Column || rowStart != celulaComFormula.Row);

                    foreach (RangeCoordinates item in listaRanges)
                    {
                        if (item.qtdRegistros > 0 && item.qtdRegistros != item.qtdRegistrosAnterior)
                        {
                            //EventLogger.WriteFormat(EventLogEntryType.Information,
                            //    string.Format("Qtd registros mudou de {0} para {1}, atualizando a partir de x[{2}] y[{3}]",
                            //    item.qtdRegistrosAnterior, item.qtdRegistros, item.colStart, item.rowStart));

                            // Remove range anterior
                            Excel.Range rangePosicaoNet =
                                ((Excel.Worksheet)Application.ActiveSheet).Range[
                                ((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart, item.colStart],
                                ((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowEnd, item.colEnd]];
                            rangePosicaoNet.Clear();

                            // Aplicar nova range
                            rangePosicaoNet = ((Excel.Worksheet)Application.ActiveSheet).Range[
                                ((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart, item.colStart],
                                ((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + item.qtdRegistros - 1, item.colEnd]];

                            for (int linha = 0; linha < item.qtdRegistros; linha++)
                            {
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 2]).NumberFormat = "0";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 3]).NumberFormat = "0";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 4]).NumberFormat = "0";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 5]).NumberFormat = "0.000";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 6]).NumberFormat = "0.000";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 7]).NumberFormat = "0";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 8]).NumberFormat = "0";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 9]).NumberFormat = "0";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 10]).NumberFormat = "0.000";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 11]).NumberFormat = "0.000";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 12]).NumberFormat = "0.00";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 13]).NumberFormat = "0.00";
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 14]).NumberFormat = "0";

                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 0]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_CLIENTE, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 1]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_ATIVO, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 2]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_QTD_EXEC_COMPRA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 3]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_QTD_EXEC_VENDA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 4]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_NET_EXEC, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 5]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_PRECO_MEDIO_EXEC_COMPRA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 6]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_PRECO_MEDIO_EXEC_VENDA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 7]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_QTD_ABERTA_COMPRA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 8]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_QTD_ABERTA_VENDA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 9]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_NET_ABERTA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 10]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_PRECO_MEDIO_ABERTA_COMPRA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 11]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_PRECO_MEDIO_ABERTA_VENDA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 12]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_VOLUMEFIN_NET_ABERTA, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 13]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_VOLUMEFIN_NET_EXEC, linha + 1);
                                ((Excel.Range)((Excel.Worksheet)Application.ActiveSheet).Cells[item.rowStart + linha, item.colStart + 14]).Value2 = string.Format("={0}(\"{1}.{2}.{3}\")", Funcoes.FUNCAO_POSICAO_NET, cliente, Funcoes.PROPRIEDADE_POSICAONET_TOTAL_REGISTROS, linha + 1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em AtualizarPosicaoNet: {0}", ex.Message));
                //EventLogger.WriteFormat(EventLogEntryType.Error, string.Format("Erro em AtualizarPosicaoNet: {0}", ex.Message));
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void gTimerDeRecalculo_Tick(object sender, EventArgs e)
        {
            try
            {
                // Apenas atualiza Posicao Net se o usuario estiver logado e mudar a quantidade de papeis listados no acompanhamento de ordens
                if (!gUsuario.Equals("") && comRTDInfo.qtdRegistrosPosicaoNet != gQtdRegistrosPosicaoNet)
                {
                    gQtdRegistrosPosicaoNet = comRTDInfo.qtdRegistrosPosicaoNet;
                    AtualizarPosicaoNet(gUsuario);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em gTimerDeRecalculo_Tick: {0}", ex.Message));
            }
        }

        private void ribGradualStockMarket_UsuarioLogado(string cliente)
        {
            gUsuario = cliente;

            if (comRTDInfo == null)
                comRTDInfo = new RTD.ComRTDInfo();

            comRTDInfo.ultimaAtualizacao = DateTime.Now;
            comRTDInfo.acao = Funcoes.ACAO_LOGIN;
            comRTDInfo.usuarioLogado = true;
            comRTDInfo.usuario = gUsuario;
            comRTDInfo.qtdRegistrosPosicaoNet = 0;

            //GravaLogDebug("ribGradualStockMarket_UsuarioLogado() usuario[" + comRTDInfo.usuario + "]");
            IniciarTimer();
        }

        private void ribGradualStockMarket_UsuarioDeslogado()
        {
            gUsuario = "";

            if (MdsHttpClient.Conectado)
            {
                Debug.WriteLine("Vou desconectar!");

                //MdsHttpClient.Instance.Terminate();
            }

            gTimerDeRecalculo.Stop();
        }

        private void ribGradualStockMarket_RealizarAcao(string pAcao, string pParametro)
        {
            switch (pAcao)
            {
                case Funcoes.ACAO_AJUSTAR_FREQUENCIA:

                    gTimerDeRecalculo.Interval = Convert.ToInt32(pParametro);

                    if (comRTDInfo == null)
                        comRTDInfo = new RTD.ComRTDInfo();

                    comRTDInfo.ultimaAtualizacao = DateTime.Now;
                    comRTDInfo.acao = Funcoes.ACAO_AJUSTAR_FREQUENCIA;
                    comRTDInfo.parametro = pParametro;

                    //GravaLogDebug("Ajustar Frequencia [" + comRTDInfo.parametro + "]");

                    break;

                case Funcoes.ACAO_MONTAR_COTACAO:

                    Interface_MontarMonitorDeCotacoes(pParametro.ToUpper());

                    break;

                case Funcoes.ACAO_MONTAR_POSICAO_NET:

                    Interface_MontarMonitorDePosicoes(pParametro);

                    break;

                case Funcoes.ACAO_MONTAR_TICKERCOTACAO:

                    Interface_MontarTickerDeCotacaoRapida(pParametro.ToUpper());

                    break;

                case Funcoes.ACAO_MONTAR_LIVROOFERTAS:

                    Interface_MontarLivroDeOfertas(pParametro.ToUpper());

                    break;

                case Funcoes.ACAO_MONTAR_CARTEIRA:

                    Interface_MontarCarteira(pParametro.ToUpper().Split(','));

                    break;
            }
        }

        #endregion

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}

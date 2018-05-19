using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConversorTesouroDireto.Registros;
using System.IO;
using System.Runtime.InteropServices;
using Gradual.OMS.Library;
using log4net;
using System.Reflection;
using Gradual.GeracaoBasesDB.Lib;

namespace ConversorTesouroDireto
{
    public partial class Form1 : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string arquivo = @"c:\temp\MDTD00227020117020117180445261_DOWNLOAD.txt";
        private string excelFile = @"c:\temp\MDTD00227020117020117180445261_DOWNLOAD.xlsx";
        private string rede = String.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void btGerarPlanilha_Click(object sender, EventArgs e)
        {

            btGerarPlanilha.Enabled = false;

            DataTable deta = new DataTable("MDTD");

            deta.Columns.Add("CodigoAgenteCustodia", typeof(System.String));
            deta.Columns.Add("CodigoCliente", typeof(System.String));
            deta.Columns.Add("DigitoCodigoCliente", typeof(System.String));
            deta.Columns.Add("CpfCpnjCliente", typeof(System.String));
            deta.Columns.Add("TipoTitulo", typeof(System.String));
            deta.Columns.Add("DataVctoTitulo", typeof(System.String));
            deta.Columns.Add("CodigoSelic", typeof(System.String));
            deta.Columns.Add("CodigoISIN", typeof(System.String));
            deta.Columns.Add("TipoTransacao", typeof(System.String));
            deta.Columns.Add("IDContabilTransacao", typeof(System.String));
            deta.Columns.Add("QuantitadeTitulosTransacao", typeof(System.String));
            deta.Columns.Add("NumeroProtocolo", typeof(System.String));
            deta.Columns.Add("Usuario", typeof(System.String));
            deta.Columns.Add("AgenteCustodiaContraparte", typeof(System.String));
            deta.Columns.Add("PrecoUnitarioTransacao", typeof(System.String));
            deta.Columns.Add("ValorTransacao", typeof(System.String));
            deta.Columns.Add("ValorTaxaAgente", typeof(System.String));
            deta.Columns.Add("ValorTotal", typeof(System.String));
            deta.Columns.Add("PrecoUnitarioOriginal", typeof(System.String));
            deta.Columns.Add("ValorOriginal", typeof(System.String));
            deta.Columns.Add("DataPrecoOriginal", typeof(System.String));
            deta.Columns.Add("OrigemSaldo", typeof(System.String));
            deta.Columns.Add("DataOperacao", typeof(System.String));
            deta.Columns.Add("DataMovimento", typeof(System.String));
            deta.Columns.Add("DataFinalDoacaoCupom", typeof(System.String));
            deta.Columns.Add("ValorTxSemBMFBovespa", typeof(System.String));
            deta.Columns.Add("ValorTxSemAgenteCustodia", typeof(System.String));
            deta.Columns.Add("PercentualReinvestimento", typeof(System.String));
            deta.Columns.Add("ProtocoloAgendamento", typeof(System.String));
            deta.Columns.Add("DataEmissaoTitulo", typeof(System.String));
            deta.Columns.Add("DataPagtoCupomAnterior", typeof(System.String));
            deta.Columns.Add("DataPagtoPrimeiroCupom", typeof(System.String));
            deta.Columns.Add("PUPrimeiroCupomJurosPago", typeof(System.String));
            deta.Columns.Add("DataPgtoCupomAnteriorCompra", typeof(System.String));
            deta.Columns.Add("DataLiquidacaoCompra", typeof(System.String));
            deta.Columns.Add("Filler", typeof(System.String));
            
            deta.AcceptChanges();



            //try
            //{
                logger.Info("Inicio processamento arquivo MDTD_01_IDENTIFICACAO_X");

                string[] lines = File.ReadAllLines(arquivo);

                long tamstrut = Marshal.SizeOf(typeof(MDTD_01_IDENTIFICACAO_X));

                foreach(string line in lines)
                {
                    string tipo = line.Substring(0, 2);
                    string registro = line.Substring(2);

                    if ( tipo.Equals("01") )
                    {
                        DataRow row = deta.NewRow();

                        MDTD_01_IDENTIFICACAO_X strut = Utilities.MarshalFromStringBlock<MDTD_01_IDENTIFICACAO_X>(line);

                        row["CodigoAgenteCustodia"] = Convert.ToInt32(strut.CodigoAgenteCustodia.ByteArrayToString()).ToString();
                        row["CodigoCliente"] = Convert.ToInt32(strut.CodigoCliente.ByteArrayToString()).ToString();
                        row["DigitoCodigoCliente"] = Convert.ToInt32(strut.DigitoCodigoCliente.ByteArrayToString()).ToString();
                        row["CpfCpnjCliente"] = strut.CpfCpnjCliente.ByteArrayToString();
                        row["TipoTitulo"] = strut.TipoTitulo.ByteArrayToString();
                        row["DataVctoTitulo"] = strut.DataVctoTitulo.ByteArrayToString();
                        row["CodigoSelic"] = strut.CodigoSelic.ByteArrayToString();
                        row["CodigoISIN"] = strut.CodigoISIN.ByteArrayToString();
                        row["TipoTransacao"] = strut.TipoTransacao.ByteArrayToString();
                        row["IDContabilTransacao"] = strut.IDContabilTransacao.ByteArrayToString();
                        row["QuantitadeTitulosTransacao"] = strut.QuantitadeTitulosTransacao.ByteArrayToDecimal(2).ToString();
                        row["NumeroProtocolo"] = strut.NumeroProtocolo.ByteArrayToString();
                        row["Usuario"] = strut.Usuario.ByteArrayToString();
                        row["AgenteCustodiaContraparte"] = strut.CodigoAgenteCustodia.ByteArrayToString().ToString();
                        row["PrecoUnitarioTransacao"] = strut.PrecoUnitarioTransacao.ByteArrayToDecimal(2).ToString();
                        row["ValorTransacao"] = strut.ValorTransacao.ByteArrayToDecimal(2).ToString();
                        row["ValorTaxaAgente"] = strut.ValorTaxaAgente.ByteArrayToDecimal(2).ToString();
                        row["ValorTotal"] = strut.ValorTotal.ByteArrayToDecimal(2).ToString();
                        row["PrecoUnitarioOriginal"] = Convert.ToInt32(strut.PrecoUnitarioOriginal.ByteArrayToDecimal(2)).ToString();
                        row["ValorOriginal"] = strut.ValorOriginal.ByteArrayToDecimal(2).ToString();
                        row["DataPrecoOriginal"] = strut.DataPrecoOriginal.ByteArrayToString();
                        row["OrigemSaldo"] = strut.OrigemSaldo.ByteArrayToString();
                        row["DataOperacao"] = strut.DataOperacao.ByteArrayToString();
                        row["DataMovimento"] = strut.DataMovimento.ByteArrayToString();
                        row["DataFinalDoacaoCupom"] = strut.DataFinalDoacaoCupom.ByteArrayToString();
                        row["ValorTxSemBMFBovespa"] = strut.ValorTxSemBMFBovespa.ByteArrayToDecimal(2).ToString();
                        row["ValorTxSemAgenteCustodia"] = strut.ValorTxSemAgenteCustodia.ByteArrayToDecimal(2).ToString();
                        row["PercentualReinvestimento"] = strut.PercentualReinvestimento.ByteArrayToString();
                        row["ProtocoloAgendamento"] = strut.ProtocoloAgendamento.ByteArrayToString();
                        row["DataEmissaoTitulo"] = strut.DataEmissaoTitulo.ByteArrayToString();
                        row["DataPagtoCupomAnterior"] = strut.DataPagtoCupomAnterior.ByteArrayToString();
                        row["DataPagtoPrimeiroCupom"] = strut.DataPagtoPrimeiroCupom.ByteArrayToString();
                        row["PUPrimeiroCupomJurosPago"] = strut.PUPrimeiroCupomJurosPago.ByteArrayToDecimal(2).ToString();
                        row["DataPgtoCupomAnteriorCompra"] = strut.DataPgtoCupomAnteriorCompra.ByteArrayToString();
                        row["DataLiquidacaoCompra"] = strut.DataLiquidacaoCompra.ByteArrayToString();
                        row["Filler"] = strut.Filler.ByteArrayToString();

                        deta.Rows.Add(row);
                    }
                }

                DataSet ds1 = new DataSet("DS1");
                ds1.Tables.Add(deta);
                ds1.AcceptChanges();

                if (ExcelCreator.CreateExcel(ds1, excelFile, "Sheet1"))
                {
                    logger.Info("Movimento Diario Tesouro Direto planilha gerada com sucesso");


                    string subject = " Movimento Diario Tesouro Direto - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                    string message = "Planilha Movimento Diario Tesouro Direto gerada as " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    message += "\n\n";
                    message += "Gravado em [" + excelFile + "]";

                    if (!String.IsNullOrEmpty(rede))
                    {
                        message += "\n\n";

                        message += "Disponivel na pasta de rede: [" + rede + "]";
                    }


                    //if (!String.IsNullOrEmpty(parametros.Message))
                    //{
                    //    message += "\n\n" + parametros.Message;
                    //}

                    message += "\n\n";

                    string[] anexos = new string[1];
                    anexos[0] = excelFile;

                    MailUtil.EnviarPlanilhaPorEmail("suporte_sistemas@gradualinvestimentos.com.br",
                        "apiza@gradualinvestimentos.com.br",
                        null,
                        null,
                        subject,
                        message,
                        anexos);

                }

                //try
                //{
                //    if (!String.IsNullOrEmpty(rede))
                //    {
                //        FileInfo excelInfo = new FileInfo(excelFile);
                //        rede += Path.DirectorySeparatorChar;
                //        rede += excelInfo.Name;

                //        logger.Info("GenerateVctosRendaFixa(" + functionName + ") Copiando arquivo [" + excelFile + "] para [" + rede + "]");
                //        File.Copy(excelFile, rede);
                //    }
                //    else
                //        logger.Info("GenerateVctosRendaFixa(" + functionName + ") Chave appsettings 'DiretorioRede' nao existe para copia do arquivo!");
                //}
                //catch (Exception ex)
                //{
                //    logger.Error("GenerateVctosRendaFixa(" + functionName + "): Erro ao copiar para pasta de rede");
                //    logger.Error("GenerateVctosRendaFixa(" + functionName + "): " + ex.Message, ex);
                //}
            //}
            //catch (Exception ex)
            //{
            //    logger.Error("_processa_ItauFJ_03_Cotacoes(): " + ex.Message, ex);
            //}

                btGerarPlanilha.Enabled = true;
        }

        private void btEscolherArquivo_Click(object sender, EventArgs e)
        {
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = false;

            openFileDialog1.Filter = "Text Files|*.txt";

            openFileDialog1.ShowDialog();

            if (!String.IsNullOrEmpty(openFileDialog1.FileName))
            {
                txtFile1.Text = openFileDialog1.FileName;

                FileInfo info = new FileInfo(openFileDialog1.FileName);

                txtFile2.Text = info.FullName.Replace(".txt", ".xlsx");

                arquivo = txtFile1.Text;
                excelFile = txtFile2.Text;
            }
        }
    }
}

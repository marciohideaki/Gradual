using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using excel = Microsoft.Office.Interop.Excel;
using Gradual.Transferencias.Risco.Lib.Dados;
using System.IO;
using Gradual.Transferencias.Risco.DbLib;
using System.Configuration;

namespace Gradual.Transferencias.Risco.View
{
    public partial class frmTransfer : Form
    {
        #region Atributos
        /// <summary>
        /// Atributo do arquivo ITRA
        /// </summary>
        private ITRAInfo ITRAFile = new ITRAInfo();

        /// <summary>
        /// Atributo do arquivo ITOP
        /// </summary>
        private ITOPInfo ITOPFile = new ITOPInfo();

        /// <summary>
        /// Atributo do arquivo ITRE
        /// </summary>
        private ITREInfo ITREFile = new ITREInfo();

        
        /// <summary>
        /// Atributo que armazena os valores das colunas que foram lidos da planilha excel
        /// </summary>
        private List<AuxITRADetailInfo> ListDetailITRA = new List<AuxITRADetailInfo>();

        /// <summary>
        /// Atributo que armazena os valores das colunas que foram lidos da planilha excel
        /// </summary>
        private List<AuxITOPDetailInfo> ListDetailITOP = new List<AuxITOPDetailInfo>();

        /// <summary>
        /// Atributo que armazena os valores das colunas que foram lidos da planilha excel
        /// </summary>
        private List<AuxITREDetailInfo> ListDetailITRE = new List<AuxITREDetailInfo>();

        /// <summary>
        /// Atributo que armazena os dados de cliente gradual e plural com seus devidos digitos verificadores
        /// </summary>
        private List<ClientData> ListClienteData       = new List<ClientData>();

        /// <summary>
        /// Path de MTA para gravação do arquivo
        /// </summary>
        private  string PathMTA = ConfigurationManager.AppSettings["PathMTA"].ToString();
        #endregion

        #region Struct
        /// <summary>
        /// Struct auxiliar para carregamento e leitura do arquivo excel 
        /// com as informações de garantias preenchidas pelo setor do risco
        /// </summary>
        private class AuxColumnsITRA
        {
            public int    IndexColumn { get; set; }
            public string NameColumn { get; set; }
            public Type   TypeColumn { get; set; }

            public AuxColumnsITRA(int pIndexColumn, string pNameColumn, Type pType)
            {
                this.IndexColumn = pIndexColumn;
                this.NameColumn  = pNameColumn;
                this.TypeColumn  = pType;
            }
        }
        /// <summary>
        /// Classe auxiliar para armazenar os valores de garantias lidos no excel
        /// </summary>
        private class AuxITRADetailInfo
        {
            public int CodigoClienteGradual { get; set; }
            public int CodigoClientePlural  { get; set; }
            public int CodigoCarteira       { get; set; }
            public string DescricaoCarteira { get; set; }
            public int Quantidade           { get; set; }
            public string Ativo             { get; set; }
        }

        /// <summary>
        /// Classe auxiliar para armazenar os valores de opções lidos no excel
        /// </summary>
        private class AuxITOPDetailInfo
        {
            public int CodigoClienteGradual { get; set; }
            public int CodigoClientePlural  { get; set; }
            public string Mercado           { get; set; }
            public int CodigoCarteira       { get; set; }
            public string DescricaoCarteira { get; set; }
            public int Quantidade           { get; set; }
            public string Ativo             { get; set; }
        }

        /// <summary>
        /// Classe auxiliar para armazenar os valores de proventos lidos no excel
        /// </summary>
        private class AuxITREDetailInfo
        {
            public int CodigoClienteGradual { get; set; }
            public int CodigoClientePlural  { get; set; }
            public int CodigoCarteira       { get; set; }
            public int Quantidade           { get; set; }
            public string Ativo             { get; set; }
            public string NumeroProcesso    { get; set; }
            public string CodigoISIN        { get; set; }
            public int NumeroDitribuicao   { get; set; }
        }
        #endregion

        #region Construtores
        public frmTransfer()
        {
            InitializeComponent();
            
        }
        #endregion
        
        #region Métodos para geração de arquivos ITRA
        /// <summary>
        /// Evento de disparo para 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpeFileITRA_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.txtFileNameITRA.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Não pode abrir o arquivo. Erro-> " + ex.Message);
            }

        }

        private void btnCreateITRA_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.ValidaNumeroSequencialITRA())
                {
                    return;
                }
                this.DisableComponents();
                this.ReadExcelGarantiasFile();
                this.LoadITRAObject();
                this.CreateITRAFile();
                this.ReleaseComponents();

                MessageBox.Show("Arquivo ITRA gerado com sucesso", "ITRA", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Não pode criar o arquivo de transferência. Erro-> " + ex.Message);;
            }
            finally
            {
                this.ReleaseComponents();
            }
        }

        /// <summary>
        /// Lê o arquivo excel as garantias e carrega na memória os dados de garantia
        /// </summary>
        private void ReadExcelGarantiasFile()
        {
            txtFileNameITRA.Text = openFileDialog1.FileName;

            excel.Application lApp;
            excel.Workbook lWorkBook;
            excel.Worksheet lWorkSheet;
            excel.Range lRange;

            lApp = new excel.Application();

            lWorkBook = lApp.Workbooks.Open(txtFileNameITRA.Text, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            lWorkSheet = (excel.Worksheet)lWorkBook.Worksheets.get_Item(1);

            lRange = lWorkSheet.UsedRange;

            dynamic lValor;

            var lDetail = new AuxITRADetailInfo();

            this.ListDetailITRA.Clear();

            for (int count = 2; count <= lRange.Rows.Count; count++)
            {
                lDetail = new AuxITRADetailInfo();

                for (int countr = 1; countr <= lRange.Columns.Count; countr++)
                {

                    lValor = (lRange.Cells[count, countr] as excel.Range).Value2;

                    switch (countr)
                    {
                        case 1:
                            lDetail.CodigoClienteGradual = ((int)lValor);
                            break;
                        case 2:
                            lDetail.CodigoClientePlural = ((int)lValor);
                            break;
                        case 3:
                            break;
                        case 4:
                            lDetail.CodigoCarteira = ((int)lValor);
                            break;
                        case 5:
                            lDetail.DescricaoCarteira = ((string)lValor);
                            break;
                        case 6:
                            lDetail.Quantidade = ((int)lValor);
                            break;
                        case 7:
                            lDetail.Ativo = ((string)lValor);
                            break;
                    }
                }

                ListDetailITRA.Add(lDetail);
            }

            lWorkBook.Close(true, null, null);
            lApp.Quit();

            this.ReleaseObject(lWorkSheet);
            this.ReleaseObject(lWorkBook);
            this.ReleaseObject(lApp);
        }

        /// <summary>
        /// Carrega o objeto do ITRA para futuro uso dos 
        /// </summary>
        private void LoadITRAObject()
        {
            try
            {
                toolStripProgressBar1.Value = 70;
                statusStrip1.Refresh();

                //Lista de clientes
                var ListaCliente = new List<int>();

                string lClientes = string.Empty;

                //tratando os clientes
                foreach (var posicao in ListDetailITRA)
                {
                    if (!ListaCliente.Contains(posicao.CodigoClienteGradual))
                    {
                        ListaCliente.Add(posicao.CodigoClienteGradual);
                        lClientes += string.Concat(posicao.CodigoClienteGradual, ",");
                    }
                }

                lClientes = lClientes.Remove(lClientes.LastIndexOf(','), 1);

                var lDb = new TransferenciasRiscoDbLib();

                this.ListClienteData = lDb.ConsultarDadosCliente(lClientes);

                int lNumeroSequencial = int.Parse(txtSequencialITRA.Text);
                //Header
                ITRAFile.ITRAHeader.DataMovimento = DateTime.Now.ToString();

                //Detail
                ITRAFile.ITRADetail.Clear();
                foreach (var lExcel in ListDetailITRA)
                {
                    var lDetail = new ITRADetailInfo();

                    var lClienteData = this.ListClienteData.Find(cliente =>
                    { return cliente.CodigoClienteGradual == lExcel.CodigoClienteGradual; });

                    if (lClienteData == null)
                    {
                        continue;
                    }

                    lNumeroSequencial++;

                    lDetail.NumeroSequencial = lNumeroSequencial.ToString();

                    lDetail.CodigoClienteDVOrigem = string.Concat(lClienteData.CodigoClienteGradual, lClienteData.DigitoClienteGradual);
                    lDetail.CodigoClienteDVDestino = string.Concat(lClienteData.CodigoClientePlural, lClienteData.DigitoClientePlural);
                    lDetail.CodigoCarteiraOrigem = lExcel.CodigoCarteira.ToString();
                    lDetail.CodigoCarteiraDestino = lExcel.CodigoCarteira.ToString();

                    var lTrade = lDb.ConsultarCodigoISIN(lExcel.Ativo, lExcel.CodigoClienteGradual);

                    lDetail.CodigoISIN = lTrade.CodigoISIN;
                    lDetail.DistribuicaoCodigoISIN = lTrade.CodigoDistribuicao;

                    ITRAFile.ITRADetail.Add(lDetail);
                }

                //Trailer
                ITRAFile.ITRATrailer.TotalRegistros = (ITRAFile.ITRADetail.Count + 2).ToString();

                toolStripProgressBar1.Value = 90;
                statusStrip1.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: erro encontrado no LoadITRAObject-->> " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Cria o arquivo de transferencia com os garantias para o 
        /// </summary>
        private void CreateITRAFile()
        {
            StringBuilder lContext = new StringBuilder();

            var lHeader = ITRAFile.ITRAHeader;

            var lTrailer = ITRAFile.ITRATrailer;

            lContext.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}\r\n",
                lHeader.TipoRegistro,
                lHeader.CodigoArquivo,
                lHeader.CodigoUsuario,
                lHeader.CodigoOrigem,
                lHeader.CodigoDestino,
                lHeader.DataGeracaoArquivo,
                lHeader.DataMovimento,
                lHeader.Reserva);

            foreach (var lRegister in ITRAFile.ITRADetail)
            {
                lContext.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}\r\n",
                    lRegister.TipoRegistro,
                    lRegister.NumeroSequencial,
                    lRegister.CodigoClienteDVOrigem,
                    lRegister.CodigoCarteiraOrigem,
                    lRegister.CodigoUsuarioDigito,
                    lRegister.CodigoClienteDVDestino,
                    lRegister.CodigoCarteiraDestino,
                    lRegister.CodigoISIN,
                    lRegister.DistribuicaoCodigoISIN,
                    lRegister.QuantidadeTotal,
                    lRegister.MotivoTransferencia,
                    lRegister.TipoTransferencia,
                    lRegister.TipoAtivo,
                    lRegister.Reserva);
            }

            lContext.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}\r\n",
                lTrailer.TipoRegistro,
                lTrailer.CodigoArquivo,
                lTrailer.CodigoUsuario,
                lTrailer.CodigoOrigem,
                lTrailer.CodigoDestino,
                lTrailer.DataGeracaoArquivo,
                lTrailer.TotalRegistros,
                lTrailer.Reserva);

            string lFilePath = PathMTA + @"itra_gradual_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".dat";

            if (!File.Exists(lFilePath))
            {
                using (StreamWriter lsw = File.CreateText(lFilePath))
                {
                    lsw.Write(lContext.ToString());
                }
            }

            this.toolStripProgressBar1.Value = 100;
            this.statusStrip1.Refresh();

        }

        /// <summary>
        /// Valida se o código do sequencial do arquivo ITRA é valido
        /// </summary>
        /// <returns></returns>
        private bool ValidaNumeroSequencialITRA()
        {
            int lSequencial;
            if (!int.TryParse(txtSequencialITRA.Text, out lSequencial))
            {
                MessageBox.Show("É necessário inserir um código sequencial para o layout do arquivo ITRA");
                return false;
            }

            return true;
        }
        #endregion

        #region Metodos para geração de arquivos ITOP
        /// <summary>
        /// Valida se o código do sequencial do arquivo ITOP é valido
        /// </summary>
        /// <returns></returns>
        private bool ValidaNumeroSequencialITOP()
        {
            int lSequencial;
            if (!int.TryParse(txtSequencialITOP.Text, out lSequencial))
            {
                MessageBox.Show("É necessário inserir um código sequencial para o layout do arquivo ITOP");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Lê o arquivo excel com a lista de opções e carrega os dados na memória
        /// </summary>
        private void ReadExcelOpcoesFile()
        {
            excel.Application lApp;
            excel.Workbook lWorkBook;
            excel.Worksheet lWorkSheet;
            excel.Range lRange;

            lApp = new excel.Application();

            lWorkBook = lApp.Workbooks.Open(txtFileNameITOP.Text, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            lWorkSheet = (excel.Worksheet)lWorkBook.Worksheets.get_Item(1);

            lRange = lWorkSheet.UsedRange;

            dynamic lValor;

            var lDetail = new AuxITOPDetailInfo();

            this.ListDetailITRA.Clear();

            for (int count = 2; count <= lRange.Rows.Count; count++)
            {
                lDetail = new AuxITOPDetailInfo();

                for (int countr = 1; countr <= lRange.Columns.Count; countr++)
                {

                    lValor = (lRange.Cells[count, countr] as excel.Range).Value2;

                    switch (countr)
                    {
                        case 1:
                            lDetail.CodigoClienteGradual = ((int)lValor);
                            break;
                        case 2:
                            lDetail.CodigoClientePlural = ((int)lValor);
                            break;
                        case 3:
                            break;
                        case 4:
                            lDetail.CodigoCarteira = ((int)lValor);
                            break;
                        case 5:
                            lDetail.DescricaoCarteira = ((string)lValor);
                            break;
                        case 6:
                            lDetail.Quantidade = ((int)lValor);
                            break;
                        case 7:
                            lDetail.Ativo = ((string)lValor);
                            break;
                    }
                }

                ListDetailITOP.Add(lDetail);
            }

            lWorkBook.Close(true, null, null);
            lApp.Quit();

            this.ReleaseObject(lWorkSheet);
            this.ReleaseObject(lWorkBook);
            this.ReleaseObject(lApp);
        }

        /// <summary>
        /// Carrega o objeto do ITOP para criar o arquivo dat posteriormente
        /// </summary>
        private void LoadITOPObject()
        {
            try
            {
                //Lista de clientes
                var ListaCliente = new List<int>();

                string lClientes = string.Empty;

                //tratando os clientes
                foreach (var posicao in ListDetailITOP)
                {
                    if (!ListaCliente.Contains(posicao.CodigoClienteGradual))
                    {
                        ListaCliente.Add(posicao.CodigoClienteGradual);
                        lClientes += string.Concat(posicao.CodigoClienteGradual, ",");
                    }
                }

                lClientes = lClientes.Remove(lClientes.LastIndexOf(','), 1);

                var lDb = new TransferenciasRiscoDbLib();

                this.ListClienteData = lDb.ConsultarDadosCliente(lClientes);

                int lNumeroSequencial = int.Parse(txtSequencialITOP.Text);
                
                bool lEhOrigem = this.rdoITOPOrigem.Checked;

                //Header
                ITOPFile.ITOPHeader.DataMovimento = DateTime.Now.ToString();
                ITOPFile.ITOPHeader.EhOrigem = lEhOrigem;
                ITOPFile.ITOPHeader.NumeroSequencial = lNumeroSequencial.ToString();

                //Detail
                ITOPFile.ITOPDetail.Clear();
                foreach (var lExcel in ListDetailITOP)
                {
                    var lDetail = new ITOPDetailInfo();

                    var lClienteData = this.ListClienteData.Find(cliente =>
                    { return cliente.CodigoClienteGradual == lExcel.CodigoClienteGradual; });

                    if (lClienteData == null)
                    {
                        continue;
                    }
                    
                    lDetail.CodigoInvestidor      = lEhOrigem ? string.Concat(lClienteData.CodigoClienteGradual, lClienteData.DigitoClienteGradual) : string.Concat(lClienteData.CodigoClientePlural, lClienteData.DigitoClientePlural);
                    lDetail.CodigoNegociacao      = lExcel.Ativo;
                    lDetail.NaturezaPosicao       = (lExcel.Quantidade <= 0) ? "L": "T";
                    lDetail.NaturezaTransferencia = (lEhOrigem)? "O" : "D";
                    lDetail.QuantidadeCoberta     = (lExcel.Quantidade > 0) ? Math.Abs(lExcel.Quantidade).ToString() : "0";
                    lDetail.QuantidadeDescoberta  = (lExcel.Quantidade < 0) ? Math.Abs(lExcel.Quantidade).ToString() : "0";
                    lDetail.QuantidadePop         = "0";
                    //lDetail.CodigoClienteDVDestino = string.Concat(lClienteData.CodigoClientePlural, lClienteData.DigitoClientePlural);
                    //lDetail.CodigoCarteiraOrigem = lExcel.CodigoCarteira.ToString();
                    //lDetail.CodigoCarteiraDestino = lExcel.CodigoCarteira.ToString();

                    ITOPFile.ITOPDetail.Add(lDetail);
                }

                //Trailer
                ITOPFile.ITOPTrailer.EhOrigem = lEhOrigem;
                ITOPFile.ITOPTrailer.TotalRegistrosGerado = (ITOPFile.ITOPDetail.Count + 2).ToString();
                ITOPFile.ITOPTrailer.NumeroSequencial = lNumeroSequencial.ToString();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: erro encontrado no LoadITRAObject-->> " + ex.StackTrace);
            }
        }

        private void btnOpenITOP_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtFileNameITOP.Text = openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// Cria o arquivo de transferencia com os Opções
        /// </summary>
        private void CreateITOPFile()
        {
            StringBuilder lContext = new StringBuilder();

            var lHeader = ITOPFile.ITOPHeader;

            var lTrailer = ITOPFile.ITOPTrailer;

            lContext.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}\r\n",
                lHeader.TipoRegistro,
                lHeader.CodigoArquivo,
                lHeader.CodigoUsuario,
                lHeader.CodigoOrigem,
                lHeader.CodigoDestino,
                lHeader.NumeroSequencial,
                lHeader.DataGeracaoArquivo,
                lHeader.DataMovimento,
                lHeader.Reserva);

            foreach (var lRegister in ITOPFile.ITOPDetail)
            {
                lContext.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}\r\n",
                    lRegister.CodigoRegistro,
                    lRegister.CodigoInvestidor,
                    lRegister.CodigoDaBolsa,
                    lRegister.CodigoNegociacao,
                    lRegister.NaturezaPosicao,
                    lRegister.NaturezaTransferencia,
                    lRegister.QuantidadeCoberta,
                    lRegister.QuantidadeDescoberta,
                    lRegister.QuantidadePop,
                    lRegister.Operacao,
                    lRegister.CodigoGuia,
                    lRegister.Reserva);
            }

            lContext.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}\r\n",
                lTrailer.TipoRegistro,
                lTrailer.CodigoArquivo,
                lTrailer.CodigoUsuario,
                lTrailer.CodigoOrigem,
                lTrailer.CodigoDestino,
                lTrailer.NumeroSequencial,
                lTrailer.DataGeracaoArquivo,
                lTrailer.TotalRegistrosGerado,
                lTrailer.DataMovimento,
                lTrailer.Reserva);

            string lFilePath = PathMTA + "itop_gradual_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".dat";

            if (!File.Exists(lFilePath))
            {
                using (StreamWriter lsw = File.CreateText(lFilePath))
                {
                    lsw.Write(lContext.ToString());
                }
            }

        }

        private void btnCreateITOP_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.ValidaNumeroSequencialITOP())
                {
                    return;
                }
                this.DisableComponents();
                this.ReadExcelOpcoesFile();
                this.LoadITOPObject();
                this.CreateITOPFile();
                this.ReleaseComponents();

                MessageBox.Show("Arquivo ITOP gerado com sucesso", "ITOP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Erro encontrado no método btnCreateITOP_Click-> " + ex.StackTrace);
            }
            finally
            {
                this.ReleaseComponents();
            }
        }
        #endregion

        #region Métodos para geração do arquivo ITRE
        /// <summary>
        /// Valida se o código do sequencial do arquivo ITRE é valido
        /// </summary>
        /// <returns></returns>
        private bool ValidaNumeroSequencialITRE()
        {
            int lSequencial;
            if (!int.TryParse(txtSequencialITRE.Text, out lSequencial))
            {
                MessageBox.Show("É necessário inserir um código sequencial para o layout do arquivo ITRE");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateITRE_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.ValidaNumeroSequencialITRE())
                {
                    return;
                }

                this.DisableComponents();
                this.ReadExcelProventosFile();
                this.LoadITREObject();
                this.CreateITREFile();
                this.ReleaseComponents();

                MessageBox.Show("Arquivo ITRE gerado com sucesso", "ITRE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Erro encontrado no método btnCreateITRE_Click-> " + ex.StackTrace);
            }
            finally
            {
                this.ReleaseComponents();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenITRE_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFileNameITRE.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Erro encontrado no Evento btnOpenITRE_Click"  + ex.StackTrace );
            }
        }

        /// <summary>
        /// Lê o arquivo excel as garantias e carrega na memória os dados de garantia
        /// </summary>
        private void ReadExcelProventosFile()
        {
            txtFileNameITRE.Text = openFileDialog1.FileName;
            
            toolStripProgressBar1.Value = 15;
            statusStrip1.Refresh();

            excel.Application lApp;
            excel.Workbook lWorkBook;
            excel.Worksheet lWorkSheet;
            excel.Range lRange;

            lApp = new excel.Application();

            lWorkBook = lApp.Workbooks.Open(txtFileNameITRE.Text, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            lWorkSheet = (excel.Worksheet)lWorkBook.Worksheets.get_Item(1);

            lRange = lWorkSheet.UsedRange;

            dynamic lValor;

            var lDetail = new AuxITREDetailInfo();

            this.ListDetailITRE.Clear();

            for (int count = 2; count <= lRange.Rows.Count; count++)
            {
                lDetail = new AuxITREDetailInfo();

                for (int countr = 1; countr <= lRange.Columns.Count; countr++)
                {

                    lValor = (lRange.Cells[count, countr] as excel.Range).Value2;

                    switch (countr)
                    {
                        case 1:
                            lDetail.CodigoClienteGradual = ((int)lValor);
                            break;
                        case 2:
                            lDetail.CodigoClientePlural = ((int)lValor);
                            break;
                        case 3:
                            lDetail.CodigoISIN = ((string)lValor);
                            break;
                        case 4:
                            lDetail.NumeroDitribuicao = ((int)lValor);
                            break;
                        case 5:
                            lDetail.CodigoCarteira = ((int)lValor);
                            break;
                        case 6:
                            lDetail.Quantidade = ((int)lValor);
                            break;
                        case 7:
                            {
                                int lNumeroProcesso = ((int)lValor);
                                lDetail.NumeroProcesso = lNumeroProcesso.ToString();
                            }
                            break;
                    }
                }

                ListDetailITRE.Add(lDetail);
            }

            lWorkBook.Close(true, null, null);
            lApp.Quit();

            this.ReleaseObject(lWorkSheet);
            this.ReleaseObject(lWorkBook);
            this.ReleaseObject(lApp);

            toolStripProgressBar1.Value = 30;
            statusStrip1.Refresh();
        }

        /// <summary>
        /// Carrega o objeto do ITRE para criar o arquivo dat posteriormente
        /// </summary>
        private void LoadITREObject()
        {
            toolStripProgressBar1.Value = 50;
            statusStrip1.Refresh();

            try
            {
                //Lista de clientes
                var ListaCliente = new List<int>();

                string lClientes = string.Empty;

                //tratando os clientes
                foreach (var posicao in ListDetailITRE)
                {
                    if (!ListaCliente.Contains(posicao.CodigoClienteGradual))
                    {
                        ListaCliente.Add(posicao.CodigoClienteGradual);
                        lClientes += string.Concat(posicao.CodigoClienteGradual, ",");
                    }
                }

                lClientes = lClientes.Remove(lClientes.LastIndexOf(','), 1);

                var lDb = new TransferenciasRiscoDbLib();

                this.ListClienteData = lDb.ConsultarDadosCliente(lClientes);

                int lNumeroSequencial = int.Parse(txtSequencialITRE.Text);

                bool lEhOrigem = this.rdoITOPOrigem.Checked;

                //Header
                ITREFile.ITREHeader.DataMovimento = DateTime.Now.ToString();
                ITREFile.ITREHeader.NumeroMovimentoArquivo = lNumeroSequencial.ToString();

                //Detail
                ITREFile.ITREDetail.Clear();
                foreach (var lExcel in ListDetailITRE)
                {
                    var lDetail = new ITREDetailInfo();

                    var lClienteData = this.ListClienteData.Find(cliente =>
                    { return cliente.CodigoClienteGradual == lExcel.CodigoClienteGradual; });

                    if (lClienteData == null)
                    {
                        continue;
                    }

                    lDetail.NumeroPedidoAtualizacao   = lExcel.NumeroProcesso;
                    lDetail.CodigoClienteDVOrigem     = string.Concat(lClienteData.CodigoClienteGradual, lClienteData.DigitoClienteGradual);
                    lDetail.CodigoCarteiraOrigem      = lExcel.CodigoCarteira.ToString();
                    lDetail.CodigoInvestidorDestinoDV = string.Concat(lClienteData.CodigoClientePlural, lClienteData.DigitoClientePlural);
                    lDetail.CodigoCarteiraDestinoDV   = lExcel.CodigoCarteira.ToString();
                    lDetail.CodigoISIN                = lExcel.CodigoISIN;
                    lDetail.DistribuicaoISIN          = lExcel.NumeroDitribuicao.ToString();
                    lDetail.Quantidade                = Math.Abs(lExcel.Quantidade).ToString();

                    ITREFile.ITREDetail.Add(lDetail);
                }

                //Trailer
                ITREFile.ITRETrailer.TotalRegistros= (ITREFile.ITREDetail.Count + 2).ToString();
                ITREFile.ITRETrailer.NumeroMovimentoArquivo = lNumeroSequencial.ToString();

                toolStripProgressBar1.Value = 70;
                statusStrip1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: erro encontrado no LoadITRAObject-->> " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Cria o arquivo de transferencia com os Opções
        /// </summary>
        private void CreateITREFile()
        {
            toolStripProgressBar1.Value = 90;
            statusStrip1.Refresh();

            StringBuilder lContext = new StringBuilder();

            var lHeader = ITREFile.ITREHeader;

            var lTrailer = ITREFile.ITRETrailer;

            lContext.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}\r\n",
                lHeader.TipoRegistro,
                lHeader.CodigoArquivo,
                lHeader.CodigoUsuario,
                lHeader.CodigoOrigem,
                lHeader.CodigoDestino,
                lHeader.NumeroMovimentoArquivo,
                lHeader.DataGeracaoArquivo,
                lHeader.DataMovimento,
                lHeader.HoraMovimento,
                lHeader.Reserva);

            foreach (var lRegister in ITREFile.ITREDetail)
            {
                lContext.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\r\n",
                    lRegister.TipoRegistro,
                    lRegister.NumeroPedidoAtualizacao,
                    lRegister.CodigoClienteDVOrigem,
                    lRegister.CodigoCarteiraOrigem,
                    lRegister.CodigoAgenteCustodiaDestino,
                    lRegister.CodigoInvestidorDestinoDV,
                    lRegister.CodigoCarteiraDestinoDV,
                    lRegister.CodigoISIN,
                    lRegister.DistribuicaoISIN,
                    lRegister.Quantidade,
                    lRegister.Reserva);
            }

            lContext.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}\r\n",
                lTrailer.TipoRegistro,
                lTrailer.CodigoArquivo,
                lTrailer.CodigoUsuario,
                lTrailer.CodigoOrigem,
                lTrailer.CodigoDestino,
                lTrailer.NumeroMovimentoArquivo,
                lTrailer.DataGeracaoArquivo,
                lTrailer.TotalRegistros,
                lTrailer.Reserva);

            string lFilePath = PathMTA  + @"itre_gradual_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".dat";

            if (!File.Exists(lFilePath))
            {
                using (StreamWriter lsw = File.CreateText(lFilePath))
                {
                    lsw.Write(lContext.ToString());
                }
            }

            toolStripProgressBar1.Value = 100;
            statusStrip1.Refresh();
        }


        #endregion

        #region Métodos
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object" + ex.ToString());
            }
            finally
            {
                GC.Collect();

            }
        }

        private void DisableComponents()
        {
            this.toolStripStatusLabel1.Text =  "Gerando o arquivo, aguade...";
            this.toolStripProgressBar1.Visible = true;
            this.toolStripProgressBar1.Value = 10;
            statusStrip1.Refresh();

            foreach (Control lControl in this.Controls)
            {
                if (lControl.Name == "statusStrip1")
                {
                    continue;
                }

                lControl.Enabled = false;
            }

        }

        private void ReleaseComponents()
        {
            toolStripStatusLabel1.Text = "";
            statusStrip1.Refresh();

            foreach (Control lControl in this.Controls)
            {
                lControl.Enabled = true;
            }

            this.toolStripProgressBar1.Value = 0;
            this.toolStripProgressBar1.Visible = false;
        }
        #endregion
    }
}

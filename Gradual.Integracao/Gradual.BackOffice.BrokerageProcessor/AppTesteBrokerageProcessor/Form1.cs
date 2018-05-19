using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using PdfSharp.Pdf;
//using PdfSharp.Pdf.IO;
//using PdfSharp.Pdf.Content.Objects;
//using PdfSharp.Pdf.Content;
using System.Diagnostics;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using iTextSharp.text;
using Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher;
using Gradual.BackOffice.BrokerageProcessor.Account;
using System.Globalization;
using log4net;
using Gradual.BackOffice.BrokerageProcessor.Lib.Cold;
using Gradual.BackOffice.BrokerageProcessor.Db;
using System.Collections.Concurrent;
using Gradual.BackOffice.BrokerageProcessor.Processor;

namespace AppTesteBrokerageProcessor
{


    public partial class Form1 : Form
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private Dictionary<int, string> dctNome = new Dictionary<int, string>();
        private Dictionary<int, List<STRelatMergeCustodia>> dctRelatorioMerge = new Dictionary<int, List<STRelatMergeCustodia>>();

        private SortedDictionary<int, List<STCustodiaCliente>> dctCustodia = new SortedDictionary<int, List<STCustodiaCliente>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(TypeWatcher.POSICAO_BMF.GetType(). Name);
        }


        //private IEnumerable<string> ExtractText(CObject cObject)
        //{
        //    var textList = new List<string>();
        //    if (cObject is COperator)
        //    {
        //        var cOperator = cObject as COperator;
        //        if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
        //            cOperator.OpCode.Name == OpCodeName.TJ.ToString())
        //        {
        //            foreach (var cOperand in cOperator.Operands)
        //            {
        //                textList.AddRange(ExtractText(cOperand));
        //            }
        //        }
        //    }
        //    else if (cObject is CSequence)
        //    {
        //        var cSequence = cObject as CSequence;
        //        foreach (var element in cSequence)
        //        {
        //            textList.AddRange(ExtractText(element));
        //        }
        //    }
        //    else if (cObject is CString)
        //    {
        //        var cString = cObject as CString;
        //        textList.Add(cString.Value);
        //    }
        //    return textList;
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            int aa = 123;
            MessageBox.Show(aa.ToString("D8"));

        }

        public int ExtractID(string content)
        {
            string cod = content.Substring(content.IndexOf("Código do cliente") + "Código do cliente".Length, 7);

            return 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string pdf = @"C:\temp\brokerage\posiçao bmf.pdf";
            //ExtractPages(pdf, @"C:\temp\brokerage\aa.pdf", 1, 10);
            ReadPdfFile(pdf);
        }

        public string ReadPdfFile(string fileName)
        {
            StringBuilder text = new StringBuilder();

            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);

                for (int page = 1; page <= 10; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    Console.WriteLine(string.Format("[{0}] [{1}]", page, currentText)); 

                    text.Append(currentText);
                }
                pdfReader.Close();
            }
            return text.ToString();
        }

        public void ExtractPages(string sourcePdfPath, string outputPdfPath,
    int startPage, int endPage)
        {
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;

            try
            {
                // Intialize a new PdfReader instance with the contents of the source Pdf file:
                reader = new PdfReader(sourcePdfPath);

                // For simplicity, I am assuming all the pages share the same size
                // and rotation as the first page:
                sourceDocument = new Document(reader.GetPageSizeWithRotation(startPage));

                // Initialize an instance of the PdfCopyClass with the source 
                // document and an output file stream:
                pdfCopyProvider = new PdfCopy(sourceDocument,
                    new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

                sourceDocument.Open();

                // Walk the specified range and add the page copies to the output file:
                for (int i = startPage; i <= endPage; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }
                sourceDocument.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*

        public static IEnumerable<string> ExtractText2(this CObject cObject)
        {
            if (cObject is COperator)
            {
                var cOperator = cObject as COperator;
                if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
                    cOperator.OpCode.Name == OpCodeName.TJ.ToString())
                {
                    foreach (var cOperand in cOperator.Operands)
                        foreach (var txt in ExtractText2(cOperand))
                            yield return txt;
                }
            }
            else if (cObject is CSequence)
            {
                var cSequence = cObject as CSequence;
                foreach (var element in cSequence)
                    foreach (var txt in ExtractText2(element))
                        yield return txt;
            }
            else if (cObject is CString)
            {
                var cString = cObject as CString;
                yield return cString.Value;
            }
        }
         */

        /// <summary>
        /// processa o arquivo da brasilplural
        /// o layout eh semelhante ao gradual, com menores diferencas no inicio das linhas
        /// </summary>
        public bool ParseCustodia(string arqentrada, bool converteconta = false)
        {
            try
            {
                ColdFilesSplitter splitter = new ColdFilesSplitter();

                logger.Info("Parsing [" + arqentrada + "]");

                string[] allLines = File.ReadAllLines(arqentrada);
                bool cabecalholido = false;

                string cabecalho = "";
                string clienteBRP = "";
                string clienteGradual = "";
                string cliente = "";
                string clienteOrig = "";
                string oldcliente = "";
                string oldpapel = "";
                string carteira = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();
                STCustodiaCliente custodia = null;

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    int xxx = line.IndexOf("***  C B L C  -  COMPANHIA BRASILEIRA DE LIQUIDACAO E CUSTODIA  ***");
                    if (xxx > 0 && !cabecalholido)
                    {
                        string data = line.Substring(3, 8).Trim();

                        //TODO: bater as datas
                    }

                    // Ignorar todas as linhas referentes a cabecalho
                    if (cabecalholido)
                    {
                        if (line.IndexOf("C B L C") > 0 ||
                            line.IndexOf("S I S T E M A") > 0 ||
                            line.IndexOf("POSICAO CONSOLIDADA") > 0 ||
                            line.IndexOf("SOCIEDADE CORRETORA") > 0 ||
                            line.IndexOf("SALDOS  NA  CUSTODIA") > 0 ||
                            line.IndexOf("BLOQUEIOS P/") > 0 ||
                            line.IndexOf("CUSTODIA  ----II----    DEPOSITO") > 0 ||
                            line.IndexOf("DIREITOS  DE") > 0 ||
                            line.IndexOf("SALDOS REGISTRADO") > 0 ||
                            line.IndexOf("QUANTIDADE  DE  CLIENTES") > 0 ||
                            line.IndexOf("T O T A L") > 0)
                        {
                            continue;
                        }
                    }


                    // Do the fuc*** job
                    if (!String.IsNullOrEmpty(line))
                    {
                        int idxClient = line.IndexOf("CLIENTE:");
                        if (idxClient >= 0)
                        {
                            cabecalholido = true;
                            //int idxFimCliente = line.IndexOf("XXXXXXXXXXXXXXXXX");

                            clienteOrig = line.Substring(idxClient + 9, 11);
                            cliente = splitter.StripDigitAndThousand(clienteOrig.Trim());

                            if (converteconta)
                                logger.Info("Processando cliente Gradual [" + cliente + "]");
                            else
                                logger.Info("Processando cliente BRP [" + cliente + "]");

                            if (!String.IsNullOrEmpty(oldcliente) && !cliente.Equals(oldcliente))
                            {
                                oldcliente = cliente;

                                if (converteconta)
                                {
                                    clienteBRP = splitter.BuscarClienteBRP(cliente);
                                    clienteGradual = cliente;
                                }
                                else
                                {
                                    clienteBRP = cliente;
                                    clienteGradual = splitter.BuscarClienteGradual(cliente);
                                }
                            }

                            if (String.IsNullOrEmpty(oldcliente))
                            {
                                oldcliente = cliente;

                                if (converteconta)
                                {
                                    clienteBRP = splitter.BuscarClienteBRP(cliente);
                                    clienteGradual = cliente;
                                }
                                else
                                {
                                    clienteBRP = cliente;
                                    clienteGradual = splitter.BuscarClienteGradual(cliente);
                                }
                            }

                            continue;
                        }

                        if (!cabecalholido)
                        {
                            cabecalho += line + "\r\n";
                            continue;
                        }


                        int icarteira = line.IndexOf("CARTEIRA...:");
                        if (icarteira > 0)
                        {
                            carteira = line.Substring(icarteira + 13).Trim();
                            continue;
                        }


                        // Aqui é a linha do papel/ativo
                        line = line.TrimEnd().PadRight(150);

                        // Se a linha comecar com espaco, remove
                        if (line[0] == ' ')
                            line.Remove(0, 1);

                        string papel = line.Substring(0, 12).Trim();
                        string isin = line.Substring(13, 12).Trim();
                        string dis = line.Substring(26, 3).Trim();
                        string esp1 = line.Substring(30, 3).Trim();
                        string esp2 = line.Substring(34, 3).Trim();
                        string esp3 = line.Substring(38, 3).Trim();
                        string sit = line.Substring(41, 11).Trim();

                        string cust = line.Substring(54, 23).Trim().Replace(".", "");
                        string depst = line.Substring(79, 23).Trim().Replace(".", "");
                        string credito = line.Substring(105, 21).Trim().Replace(".", "");
                        string debito = line.Substring(128, 21).Trim().Replace(".", "");

                        if (!String.IsNullOrEmpty(papel))
                        {
                            if (!String.IsNullOrEmpty(oldpapel) && !papel.Equals(oldpapel))
                            {
                                // Busca as entradas ja existentes
                                // para esse cliente, para fazer a soma
                                STCustodiaCliente itemEncontrado = null;
                                foreach (STCustodiaCliente item in dctCustodia[custodia.CodBolsa])
                                {
                                    if (item.Carteira.Equals(custodia.Carteira) &&
                                        item.Papel.Equals(custodia.Papel))
                                    {
                                        int iContaGradual = Convert.ToInt32(clienteGradual);
                                        if (!dctRelatorioMerge.ContainsKey(iContaGradual))
                                        {
                                            dctRelatorioMerge.Add(iContaGradual, new List<STRelatMergeCustodia>());
                                        }

                                        List<STRelatMergeCustodia> relatorio = dctRelatorioMerge[iContaGradual];
                                        STRelatMergeCustodia itemRelat = relatorio.Find(x => (x.Carteira.Equals(custodia.Carteira) && x.Papel.Equals(custodia.Papel)));

                                        if (itemRelat == null)
                                        {
                                            itemRelat = new STRelatMergeCustodia();
                                            itemRelat.Papel = custodia.Papel;
                                            itemRelat.Carteira = custodia.Carteira;
                                            itemRelat.CodConta = iContaGradual;
                                            itemRelat.CodContaBRP = Convert.ToInt32(clienteBRP);

                                            relatorio.Add(itemRelat);
                                        }

                                        item.SaldoCustodia += custodia.SaldoCustodia;
                                        item.BloqueioDeposito += custodia.BloqueioDeposito;

                                        foreach (KeyValuePair<string, STLancamentoPrevisto> custoPrev in custodia.Lancamentos)
                                        {
                                            if (item.Lancamentos.ContainsKey(custoPrev.Key))
                                            {
                                                item.Lancamentos[custoPrev.Key].LctoPrevCredito += custoPrev.Value.LctoPrevCredito;
                                                item.Lancamentos[custoPrev.Key].LctoPrevDebito += custoPrev.Value.LctoPrevDebito;
                                            }
                                            else
                                            {
                                                item.Lancamentos.Add(custoPrev.Key, custoPrev.Value);
                                            }
                                        }


                                        //if (converteconta)
                                        //{
                                        //    itemRelat.BloqueioDepositoGRD = custodia.BloqueioDeposito;
                                        //    itemRelat.LctoPrevCreditoGRD = custodia.LctoPrevCredito;
                                        //    itemRelat.LctoPrevDebitoGRD = custodia.LctoPrevDebito;
                                        //    itemRelat.SaldoCustodiaGRD = custodia.SaldoCustodia;
                                        //}
                                        //else
                                        //{
                                        //    itemRelat.BloqueioDepositoBRP = custodia.BloqueioDeposito;
                                        //    itemRelat.LctoPrevCreditoBRP = custodia.LctoPrevCredito;
                                        //    itemRelat.LctoPrevDebitoBRP = custodia.LctoPrevDebito;
                                        //    itemRelat.SaldoCustodiaBRP = custodia.SaldoCustodia;
                                        //}


                                        itemEncontrado = item;

                                        break;
                                    }

                                    if (itemEncontrado == null)
                                    {
                                        // Se nao achou carteira + papel, acrescenta
                                        dctCustodia[custodia.CodBolsa].Add(custodia);
                                    }
                                }

                            }

                            // Inicializa nova entrada de custodia
                            custodia = new STCustodiaCliente();
                            custodia.Papel = papel;
                            custodia.Carteira = carteira;
                            custodia.CodBolsa = Convert.ToInt32(clienteBRP);
                            custodia.Situacao = sit;
                            custodia.ISIN = string.Format("{0} {1} {2} {3} {4}",
                                                        isin.PadRight(12),
                                                        dis.PadRight(3),
                                                        esp1.PadRight(3),
                                                        esp2.PadRight(3),
                                                        esp3.PadRight(3));

                            if (!String.IsNullOrEmpty(cust))
                                custodia.SaldoCustodia = Convert.ToInt64(cust);
                            if (!String.IsNullOrEmpty(depst))
                                custodia.BloqueioDeposito = Convert.ToInt64(depst);

                            if (!dctCustodia.ContainsKey(custodia.CodBolsa))
                            {
                                dctCustodia.Add(custodia.CodBolsa, new List<STCustodiaCliente>());
                            }

                            oldpapel = papel;
                        }
                        else
                        {
                            string dataprevisao = sit;

                            logger.DebugFormat("Parsing Account [{0}] Carteira [{1}] Papel [{2}] Previsao [{3}] Cred [{4}] Deb [{5}]",
                                clienteBRP,
                                carteira,
                                papel,
                                dataprevisao,
                                credito,
                                debito);

                            STLancamentoPrevisto lcto = new STLancamentoPrevisto();
                            lcto.DataPrevisao = dataprevisao;

                            if (!String.IsNullOrEmpty(credito))
                                lcto.LctoPrevCredito = Convert.ToInt64(credito);
                            if (!String.IsNullOrEmpty(debito))
                                lcto.LctoPrevDebito = Convert.ToInt64(debito);

                            custodia.Lancamentos.Add(dataprevisao, lcto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("ParseCustodia: " + ex.Message, ex);
                return false;
            }
            return true;
        }


        private string StripDigitAndThousand(string account)
        {
            string strippedAccount = account;

            int idxMinus = account.IndexOf('-');

            if (idxMinus > 0)
            {
                strippedAccount = account.Remove(idxMinus);
            }

            idxMinus = strippedAccount.IndexOf('.');

            if (idxMinus > 0)
            {
                strippedAccount = strippedAccount.Remove(idxMinus, 1);
            }

            return strippedAccount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        private string BuscarClienteGradual(string cliente)
        {
            AccountDigit acDig = new AccountDigit();

            int accountGradual = AccountParser.Instance.GetAccountParsed(Convert.ToInt32(cliente));

            return accountGradual.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        private string BuscarClienteBRP(string cliente)
        {
            AccountDigit acDig = new AccountDigit();

            int accountBRP = AccountParser.Instance.GetAccountBRP(Convert.ToInt32(cliente));

            return accountBRP.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="retornaComDigito"></param>
        /// <param name="retornaComSepMilhar"></param>
        /// <param name="padLeft"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        private string BuscarClienteGradualFormatado(string cliente, bool retornaComDigito = false, bool retornaComSepMilhar = false, int padLeft = 0, char paddingChar = ' ')
        {
            AccountDigit acDig = new AccountDigit();
            string account = "";

            int accountGradual = AccountParser.Instance.GetAccountParsed(Convert.ToInt32(cliente));

            if (!retornaComSepMilhar)
            {
                if (!retornaComDigito)
                    account = accountGradual.ToString();
                else
                {
                    account = acDig.GetAccountWithDigit(accountGradual, AccountDigit.BOVESPA).ToString();
                    account = account.Insert(account.Length - 1, "-");
                }
            }
            else
            {
                if (!retornaComDigito)
                    account = accountGradual.ToString("N0", CultureInfo.CreateSpecificCulture("pt-BR"));
                else
                {
                    account = acDig.GetAccountWithDigit(accountGradual, AccountDigit.BOVESPA).ToString("N0", CultureInfo.CreateSpecificCulture("pt-BR"));
                    account = account.Insert(account.Length - 1, "-");
                }
            }

            if (padLeft > 0)
            {
                account = account.PadLeft(padLeft, paddingChar);
            }

            return account;
        }

        private void _CarregarListaNomes()
        {
            try
            {
                DbAccOracle db = new DbAccOracle();

                ConcurrentDictionary<int, string> dct = db.CarregarNomeClientes();

                foreach (KeyValuePair<int, string> item in dct)
                {
                    if (!dctNome.ContainsKey(item.Key))
                        dctNome.Add(item.Key, item.Value);
                }

                logger.Info("Carregou lista de nomes com " + dctNome.Count + " itens.");
            }
            catch (Exception ex)
            {
                logger.Error("_CarregarListaNomes: " + ex.Message, ex);
            }
        }


        private string BuscarNomeCliente(string clienteGradual)
        {
            string ret = String.Empty;

            if (dctNome.ContainsKey(Convert.ToInt32(clienteGradual)))
                ret = dctNome[Convert.ToInt32(clienteGradual)];

            return ret;
        }

        private void btCarrCust_Click(object sender, EventArgs e)
        {
            btCarrCust.Enabled = false;

            string arq1 = @"C:\Temp\TesteCustodiaCold\CUSTODIA_120_Gradual.txt";
            this.ParseCustodia(arq1);

            string arq2 = @"C:\Temp\TesteCustodiaCold\CustodiaGRADUAL227.txt";

            this.ParseCustodia(arq2, true);

            btCarrCust.Enabled = true;
        }

        private void btGerarArqCust_Click(object sender, EventArgs e)
        {
            _CarregarListaNomes();

            btGerarArqCust.Enabled = false;
            string arqMerge = @"C:\Temp\TesteCustodiaCold\Mergeado.txt";

            this.GeraCustodiaMergeCold(arqMerge);
            btGerarArqCust.Enabled = true;
        }

        public void GeraCustodiaMergeCold(string arqMerge)
        {
            AccountDigit acDig = new AccountDigit();
            string account = "";

            string tplcabec = File.ReadAllText(@"C:\Temp\TesteCustodiaCold\TemplateCabecalho.txt");

            string cabecalho = string.Format(tplcabec, DateTime.Now.ToString("dd/MM/yyyy"));

            File.WriteAllText(arqMerge, cabecalho);

            foreach (KeyValuePair<int, List<STCustodiaCliente>> item in dctCustodia)
            {
                int codBolsa = item.Key;

                List<STCustodiaCliente> custodias = item.Value;

                //string nomeCliente = BuscarNomeCliente(codBolsa.ToString());

                account = acDig.CalculaDV(120, codBolsa).ToString();

                account = account.Insert(account.Length - 1, "-");

                //string linha = string.Format("CLIENTE: {0} - {1}\n", account.PadLeft(11), nomeCliente);
                string linha = string.Format("CLIENTE: {0} - {1}\n", account.PadLeft(11), "GRADUAL CCTVM S/A");
                File.AppendAllText(arqMerge, linha);

                // custodias.Sort((x, y) => x.Carteira.CompareTo(y.Carteira));
                custodias = item.Value.OrderBy(i => i.Carteira).ThenBy(i => i.Papel).ToList();

                string carteira = "";
                foreach (STCustodiaCliente custodia in custodias)
                {
                    if (!custodia.Carteira.Equals(carteira) )
                    {
                        linha = string.Format("      CARTEIRA...:  {0}", custodia.Carteira);
                        File.AppendAllText(arqMerge, linha.PadLeft(21));
                        File.AppendAllText(arqMerge, "\n");
                    }
                    carteira = custodia.Carteira;

                    linha = string.Format("{0} {1}",
                        custodia.Papel.PadRight(12),
                        custodia.ISIN);

                    File.AppendAllText(arqMerge, linha);

                    if ( !String.IsNullOrEmpty(custodia.Situacao) )
                        linha = string.Format("{0}", custodia.Situacao.PadRight(14));
                    else
                        linha = string.Format("{0}", " ".PadRight(12));

                    File.AppendAllText(arqMerge, linha);

                    linha = String.Format(CultureInfo.CreateSpecificCulture("pt-Br"), "{0:#,0}", custodia.SaldoCustodia);
                    File.AppendAllText(arqMerge, linha.PadLeft(23));

                    File.AppendAllText(arqMerge, " ".PadRight(2));

                    linha = String.Format(CultureInfo.CreateSpecificCulture("pt-Br"), "{0:#,0}", custodia.BloqueioDeposito);
                    File.AppendAllText(arqMerge, linha.PadLeft(23));

                    File.AppendAllText(arqMerge, "\n");

                    foreach (STLancamentoPrevisto lcto in custodia.Lancamentos.Values)
                    {
                        linha = string.Format("{0} {1} {2}",
                            " ".PadRight(42),
                            lcto.DataPrevisao,
                            " ".PadRight(50));

                        File.AppendAllText(arqMerge, linha);

                        linha = String.Format(CultureInfo.CreateSpecificCulture("pt-Br"), "{0:#,0}", lcto.LctoPrevCredito);
                        File.AppendAllText(arqMerge, linha.PadLeft(20));

                        File.AppendAllText(arqMerge, " ".PadRight(3));

                        linha = String.Format(CultureInfo.CreateSpecificCulture("pt-Br"), "{0:#,0}", lcto.LctoPrevDebito);
                        File.AppendAllText(arqMerge, linha.PadLeft(20));

                        File.AppendAllText(arqMerge, "\r\n");
                    }
                }

                File.AppendAllText(arqMerge,"\n");
            }
        }
    }
}

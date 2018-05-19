using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections.Concurrent;
using System.Threading;
//using PdfSharp.Pdf.Content.Objects;
//using PdfSharp.Pdf;
//using PdfSharp.Pdf.IO;
//using PdfSharp.Pdf.Content;
using Gradual.BackOffice.BrokerageProcessor.Lib.Pdf;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using System.IO;
using Gradual.BackOffice.BrokerageProcessor.Lib.Email;
using Gradual.BackOffice.BrokerageProcessor.Email;
using System.Configuration;
using Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher;
using Gradual.BackOffice.BrokerageProcessor.Db;

namespace Gradual.BackOffice.BrokerageProcessor.FileManager
{
    public class PdfManager
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region Static Objects
        private static PdfManager _me = null;
        public static PdfManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new PdfManager();
                }

                return _me;
            }
        }
        #endregion


        #region private variables
        bool _isRunning;
        ConcurrentQueue<TOArqPdf> _cqFilesPdf;
        Thread _thPdf;
        object _syncPdf = new object();


        //string _emailAlert;
        //string _emailFrom;
        //string _emailTo;
        //string _emailCc;
        //string _emailCco;
        #endregion

        public void Start()
        {
            try
            {
                logger.Info("Iniciando Pdf Manager...");

                _cqFilesPdf = new ConcurrentQueue<TOArqPdf>();
                _isRunning = true;
                _thPdf = new Thread(new ThreadStart(this._queuePdf));
                _thPdf.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao iniciar Pdf Manager: " + ex.Message, ex);
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Parando PdfManager...");
                _isRunning = false;

                if (_thPdf != null && _thPdf.IsAlive)
                {
                    _thPdf.Join(200);
                    if (_thPdf.IsAlive)
                    {
                        try
                        {
                            _thPdf.Abort();
                        }
                        catch { }
                    }
                    _thPdf = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao parar o Pdf Manager: " + ex.Message, ex);
            }
        }

        public void AddPdfFile(TOArqPdf file)
        {
            _cqFilesPdf.Enqueue(file);
            lock (_syncPdf)
                Monitor.Pulse(_syncPdf);

        }

        private void _queuePdf()
        {
            while (_isRunning)
            {
                try
                {
                    TOArqPdf fileName = null;
                    if (_cqFilesPdf.TryDequeue(out fileName))
                    {
                        this._processPdf(fileName);
                    }
                    else
                    {
                        lock (_syncPdf)
                            Monitor.Wait(_syncPdf, 300);
                    }
                }
                catch { }
            }
        }

        // Efetua o split do arquivo pdf
        private void _processPdf(TOArqPdf to)
        {
            
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopy = null;
            PdfImportedPage page = null;
            int currentCod = 0;
            string arqname;
            string completePath;
            try
            {
                logger.Info("============================================================");
                logger.Info("Processando arquivo: " + to.FileName);
                reader = new PdfReader(to.FileName);
                sourceDocument = new Document(reader.GetPageSizeWithRotation(1));
                string pathTimeStamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff");
                completePath = to.Config.PathProcessed + "\\" + pathTimeStamp;
                if (!Directory.Exists(completePath))
                    Directory.CreateDirectory(completePath);
                
                string onlyFile = to.FileName.Substring(to.FileName.LastIndexOf("\\") + 1);
                int pageCount = reader.NumberOfPages;
                for (int i = 1; i <= pageCount; i++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    int idCliente = 0;
                    switch (to.Config.Type)
                    {
                        case TypeWatcher.BMF:
                            idCliente = PdfTools.ExtractBMFClientID(currentText);
                            break;
                        case TypeWatcher.BOVESPA:
                            idCliente = PdfTools.ExtractBovespaClientID(currentText);
                            break;
                        case TypeWatcher.POSICAO_BMF:
                            idCliente = PdfTools.ExtractPosicaoBmfClientID(currentText);
                            break;
                    }
                    logger.InfoFormat("Arquivo: [{0}] Pagina: [{1}] IdCliente [{2}]", onlyFile, i.ToString("D5"), idCliente.ToString("D8"));
                    if (idCliente >= 0)
                    {
                        if (currentCod != idCliente)
                        {
                            // Salvar o anterior
                            if (pdfCopy != null)
                            {
                                pdfCopy.Close();
                                sourceDocument.Close();
                                sourceDocument = new Document(reader.GetPageSizeWithRotation(i));
                            }
                            arqname = string.Format("{0}\\{1}\\{2}.pdf", to.Config.PathProcessed, pathTimeStamp, idCliente.ToString("D8"));
                            if (File.Exists(arqname))
                                arqname = string.Format("{0}\\{1}\\{2}-{3}.pdf", to.Config.PathProcessed, pathTimeStamp, idCliente.ToString("D8"), new Random().Next(1, 100000).ToString("D6"));

                            pdfCopy = new PdfCopy(sourceDocument, new FileStream(arqname, FileMode.Create));
                            sourceDocument.Open();
                            currentCod = idCliente;
                        }
                        page = pdfCopy.GetImportedPage(reader, i);
                        pdfCopy.AddPage(page);

                        // Verifica se pagina corrente eh igual ao numero de paginas, para fechar o arquivo
                        if (i == pageCount)
                        {
                            pdfCopy.Close();
                            pdfCopy = null;
                        }
                    }
                    else
                    {
                        logger.Error("Não foi possivel extrair o codigo de cliente da pagina: " + i);
                        continue;
                    }
                }
                EmailManager.Instance.ProcessEmailPath(completePath, to.Config);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento do Pdf: " + ex.Message, ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (sourceDocument != null)
                {
                    sourceDocument.Close();
                    sourceDocument = null;
                }

                if (pdfCopy != null)
                {
                    pdfCopy.Close();
                    pdfCopy = null;
                }
                page = null;
            }   
        }
    }
}

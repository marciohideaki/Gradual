using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EvoPdf.HtmlToPdf;
using System.Text;
using System.IO;
using System.Net.Mime;
using System.Net.Mail;

namespace InviXX.Www
{
    /// <summary>
    /// Summary description for GerarDocumetosPDF
    /// </summary>
    public class GerarDocumetosPDF : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StringBuilder url = new StringBuilder();
            if (context != null)
            {
                bool lEnviarEmail = false;
                string lfileName = "{0}_{1}.pdf";
                
                lEnviarEmail = (!String.IsNullOrWhiteSpace(context.Request["EnviarEmail"]) && context.Request["EnviarEmail"].Equals("S"));
                switch (context.Request["TipoRelatorio"])
                {
                    case "ExtratoMensal":
                        url.Append(context.Request.Url.AbsoluteUri.Replace(this.GetType().Name + ".ashx", "Financeiro/RelExtratoMensal.aspx"));
                        lfileName = string.Format(lfileName, "ExtratoMensal", DateTime.Now.Ticks.ToString());
                        break;
                    case "ExtratoConsolidado":
                            url.Append(context.Request.Url.AbsoluteUri.Replace(this.GetType().Name + ".ashx", "Financeiro/RelExtratoConsolidado.aspx"));
                            if (lEnviarEmail)
                            {
                                url.Append("?BuscarPor=");
                                url.Append(context.Request["BuscarPor"]);
                                url.Append("&TermoDeBusca=");
                                url.Append(context.Request["TermoDeBusca"]);
                                url.Append("&TipoCliente=");
                                url.Append(context.Request["TipoCliente"]);
                                url.Append("&CodigoProduto=");
                                url.Append(context.Request["CodigoProduto"]);
                            }
                            lfileName = string.Format(lfileName, "ExtratoConsolidado", DateTime.Now.Ticks.ToString());
                        break;
                }
                this.ConvertURLToPDF(context, url.ToString(), lfileName, lEnviarEmail);
            }
        }

        /// <summary>
        /// Convert the HTML code from the specified URL to a PDF document
        /// and send the document to the browser
        /// </summary>
        private void ConvertURLToPDF(HttpContext pContext, string pUrl, string pfileName, bool pEnviarEmail)
        {
            string urlToConvert = pUrl;// textBoxWebPageURL.Text.Trim();

            // Create the PDF converter. Optionally the HTML viewer width can be specified as parameter
            // The default HTML viewer width is 1024 pixels.
            PdfConverter pdfConverter = new PdfConverter();

            // set the license key - required
            pdfConverter.LicenseKey = "ORIJGQoKGQkZCxcJGQoIFwgLFwAAAAA=";

            // set the converter options - optional
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.NoCompression;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Landscape;

            // set if header and footer are shown in the PDF - optional - default is false 
            pdfConverter.PdfDocumentOptions.ShowHeader = false;////cbAddHeader.Checked;
            pdfConverter.PdfDocumentOptions.ShowFooter = false;// cbAddFooter.Checked;
            // set if the HTML content is resized if necessary to fit the PDF page width - default is true
            //pdfConverter.PdfDocumentOptions.FitWidth = true;// cbFitWidth.Checked;
            //pdfConverter.PdfDocumentOptions.StretchToFit = true;
            pdfConverter.PdfDocumentOptions.AutoSizePdfPage = false;
            //pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.

            pdfConverter.PdfDocumentOptions.RightMargin = 5;

            // set the embedded fonts option - optional - default is false
            pdfConverter.PdfDocumentOptions.EmbedFonts = false;// cbEmbedFonts.Checked;
            // set the live HTTP links option - optional - default is true
            pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = false;// cbLiveLinks.Checked;

            // set if the JavaScript is enabled during conversion to a PDF - default is true
            pdfConverter.JavaScriptEnabled = true;// cbClientScripts.Checked;

            // set if the images in PDF are compressed with JPEG to reduce the PDF document size - default is true
            pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;// cbJpegCompression.Checked;

            pdfConverter.ConversionDelay = 5;

            // be saved to a file or sent as a browser response
            byte[] pdfBytes = pdfConverter.GetPdfBytesFromUrl(urlToConvert);

            // send the PDF document as a response to the browser for download
            if (pEnviarEmail)
            {
                MemoryStream ms = new MemoryStream(pdfBytes);
                ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                Attachment attach = new System.Net.Mail.Attachment(ms, ct);
                attach.ContentDisposition.FileName = pfileName;
                attach.ContentDisposition.DispositionType = String.Format("attachment; filename={0}; size={1}", pfileName, pdfBytes.Length.ToString());
                attach.ContentDisposition.CreationDate = DateTime.Now; 
                
                MailMessage pMail = new MailMessage();
                pMail.From = new MailAddress("amiguel@gradualinvestimentos.com.br");
                pMail.To.Add("brocha@gradualinvestimentos.com.br");
                //pMail.To.Add(req.EmailCliente);
                pMail.To.Add("amiguel@gradualinvestimentos.com.br");
                pMail.Subject = "Invixx - Extrato Consolidado";
                pMail.IsBodyHtml = false;

                pMail.Body =
@"Prezado Cliente.

Segue anexo o seu extrato consolidado.

Atenciosamente

Invixx";
                pMail.Attachments.Add(attach);
                SmtpClient pSmtpClient = new SmtpClient("ironport.gradual.intra");
                System.Net.NetworkCredential credential = new System.Net.NetworkCredential("amiguel", "Gradual456", "GRADUAL");
                pSmtpClient.Credentials = credential;
                pSmtpClient.Send(pMail);
                pContext.Response.Clear();
                pContext.Response.Write("sucesso");
                pContext.Response.End();
            }
            else
            {
                pContext.Response.Clear();
                pContext.Response.ContentType = "application/pdf";
                pContext.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}; size={1}",
                                        pfileName, pdfBytes.Length.ToString()));
                pContext.Response.BinaryWrite(pdfBytes);
                // Note: it is important to end the response, otherwise the ASP.NET
                // web page will render its content to PDF document stream
                pContext.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
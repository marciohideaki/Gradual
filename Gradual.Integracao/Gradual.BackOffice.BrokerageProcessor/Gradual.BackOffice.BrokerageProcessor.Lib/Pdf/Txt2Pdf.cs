using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Pdf
{
    public class Txt2Pdf
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public static bool ConvertTxt2Pdf(string inTxtFile, string outPdfFile, bool flagAppend=false)
        {
            try
            {
                string textBody = File.ReadAllText(inTxtFile);

                //StreamReader rdr = new StreamReader(textBody);
                StreamReader rdr = new StreamReader(inTxtFile);

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);
                iTextSharp.text.Font fonte = new iTextSharp.text.Font(bfTimes, 8, iTextSharp.text.Font.NORMAL);
                //Create a New instance on Document Class

                Document doc = new Document();
                doc.SetPageSize(PageSize.A4.Rotate());

                PdfWriter writer;
                PdfContentByte cb;

                //Create a New instance of PDFWriter Class for Output File
                writer = PdfWriter.GetInstance(doc, new FileStream(outPdfFile, FileMode.Create));

                //Open the Document
                doc.Open();
                
                //Add the content of Text File to PDF File
                doc.Add(new Paragraph(rdr.ReadToEnd(), fonte));

                //Close the Document
                doc.Close();
                rdr.Close();
                writer.Close();
            }
            catch (Exception ex)
            {
                logger.Error("ConvertTxt2Pdf: " + ex.Message, ex);

                return false;
            }

            return true;
        }
    }
}

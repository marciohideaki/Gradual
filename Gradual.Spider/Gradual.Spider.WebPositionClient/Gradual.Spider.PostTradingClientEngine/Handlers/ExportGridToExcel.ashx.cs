using ExportToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using MS.Internal.WindowsBase;
using DocumentFormat.OpenXml;

namespace ExportToExcel
{
    /// <summary>
    /// Summary description for ExportGridToExcel
    /// </summary>
    public class ExportGridToExcel : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string tabData = context.Request["excelData"]; 

            DataTable dt = ConvertCsvData(tabData);
            if (dt == null)
            {
                //  Add some error-catching here...
                return;
            }

            string excelFilename = context.Request["filename"];  

            if (File.Exists(excelFilename))
                File.Delete(excelFilename);

            CreateExcelFile.CreateExcelDocument(dt, excelFilename, context.Response);
        }

        private DataTable ConvertCsvData(string CSVdata)
        {
            //  Convert a tab-separated set of data into a DataTable, ready for our C# CreateExcelFile libraries
            //  to turn into an Excel file.
            //
            DataTable dt = new DataTable();
            try
            {
                System.Diagnostics.Trace.WriteLine(CSVdata);

                string[] Lines = CSVdata.Split(new char[] { '\r', '\n' });
                if (Lines == null)
                    return dt;
                if (Lines.GetLength(0) == 0)
                    return dt;

                string[] HeaderText = Lines[0].Split('\t');

                int numOfColumns = HeaderText.Count();

                
                foreach (string header in HeaderText)
                    dt.Columns.Add(header, typeof(string));

                DataRow Row;
                for (int i = 1; i < Lines.GetLength(0); i++)
                {
                    string[] Fields = Lines[i].Split('\t');
                    if (Fields.GetLength(0) == numOfColumns)
                    {
                        Row = dt.NewRow();
                        for (int f = 0; f < numOfColumns; f++)
                            Row[f] = Fields[f];
                        dt.Rows.Add(Row);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("An exception occurred: " + ex.Message);
                return null;
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
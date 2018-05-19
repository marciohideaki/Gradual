using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using log4net;
using System.Configuration;

namespace Gradual.Spider.JaneStreet.Report
{
    public class ExcelCreator
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public static string CreateJaneStreetExcel(DataSet ds, string exchange)
        {
            try
            {
                
                string currentDirectorypath = string.Empty;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("PathTmp"))
                    currentDirectorypath = ConfigurationManager.AppSettings["PathTmp"].ToString() + Path.DirectorySeparatorChar;
                else
                    currentDirectorypath = Environment.CurrentDirectory+ Path.DirectorySeparatorChar + "tmp";
                
                bool exists = System.IO.Directory.Exists(currentDirectorypath);
                if (!exists)
                    System.IO.Directory.CreateDirectory(currentDirectorypath);
                string fileName = string.Format("{0}{1}{2}-{3}-{4}.xlsx", currentDirectorypath, Path.DirectorySeparatorChar, "JaneStreetDP", exchange, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                FileInfo newFile = new FileInfo(fileName);
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Executadas");
                    if (ds != null)
                    {
                        ws.Cells["A1"].LoadFromDataTable(ds.Tables[0], true, TableStyles.None);
                    }
                    package.Save();
                }
                return fileName;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na criacao do excel...: " + ex.Message, ex);
                return string.Empty;
            }

        }

    }
}

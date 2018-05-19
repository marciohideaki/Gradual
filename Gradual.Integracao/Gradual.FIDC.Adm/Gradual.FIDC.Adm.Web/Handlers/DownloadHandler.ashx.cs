using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.Handlers
{
    /// <summary>
    /// Summary description for DownloadHandler1
    /// </summary>
    public class DownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string lFile = "";

            string lTypeFile = "";

            string lFileName = "";

            if (context.Request["FileName"] != null)
            {
                lFile = context.Request["FileName"].ToString();
            }

            if (context.Request["TypeFile"] != null)
            {
                lTypeFile = context.Request["TypeFile"].ToString();
            }

            

            if (lFile.Contains("c:"))
            {
                lFile = lFile.Replace("c:","").Trim();
            }

            //switch (lTypeFile.ToLower())
            //{
            //    case "fidc":

            //        lFileName = lFile;

            //        break;
            //    case "carteira":

            //        lFileName = lFile;

            //        break;
            //    case "extrato":

            //        lFileName =  lFile;

            //        break;
            //    case "mec":

            //        lFileName = lFile;

            //        break;
            //}

            lFileName = lFile;

            try
            {
                System.IO.FileInfo lFileInfo = new System.IO.FileInfo( HttpUtility.UrlDecode( lFileName));

                if (lFileInfo.Exists)
                {
                    context.Response.Clear();
                    context.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + lFileInfo.Name + "\"");
                    context.Response.AddHeader("Content-Length", lFileInfo.Length.ToString());
                    context.Response.ContentType = "ApplicationException/octet-stream";
                    context.Response.TransmitFile(lFileInfo.FullName);
                    context.Response.Flush();
                }
                else
                {
                    throw new Exception("File not found");
                }

            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";

                context.Response.Write(ex.Message);
            }
            finally
            {
                context.Response.End();
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
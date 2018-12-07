﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class FileLoader : PaginaBaseAutenticada
    {

        //FICHA_CADASTRAL  -> 95a7f5db23b383b98e1ca6b61cf2c6c1
        //FICHA_CAMBIO     -> 1dac8b6c1d8adbaa1a0af55f91bf4e31
        //TERMO            -> b5f831311230fa38b177d9e768a35cbf

        System.String lArquivoFicha     = String.Empty;
        System.String lArquivoCambio    = String.Empty;
        System.String lArquivoTermo     = String.Empty;
        System.String lPathArquivo      = System.Configuration.ConfigurationManager.AppSettings["pathVirtualIntranet"];

        System.String lFilePath         = String.Empty;


        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            //if ((TransporteSessaoClienteLogado)this.Session["ClienteLogado"] != null)
            //{
            //    TransporteSessaoClienteLogado lSessaoClienteLogado = (TransporteSessaoClienteLogado)this.Session["ClienteLogado"];

            //    // Verifica se não é uma solicitação de Termo de Adesão, ou seja, se é Ficha Cadastral ou Ficha de Cambio
            //    if (!Request["Tipo"].Equals("b5f831311230fa38b177d9e768a35cbf"))
            //    {
                    
            //        // Se for Ficha Cadatral
            //        if (Request["Tipo"].Equals("95a7f5db23b383b98e1ca6b61cf2c6c1"))
            //        {
            //            // Ficha existe na sessão?
            //            if (!String.IsNullOrEmpty(Session["ArquivoFicha"].ToString()))
            //            {
            //                lArquivoFicha = String.Concat(lPathArquivo, Session["ArquivoFicha"].ToString());

            //                // Verifica se a ficha da sessão ainda existe
            //                if (!System.IO.File.Exists(lArquivoFicha))
            //                {
            //                    // Caso a ficha não exista, cria uma nova
            //                    GerarFichas();
            //                }
            //            }
            //        }

            //        // Se for Ficha Cadastral de Cambio
            //        if (Request["Tipo"].Equals("1dac8b6c1d8adbaa1a0af55f91bf4e31"))
            //        {
            //            // Ficha existe na Sessão?
            //            if (!String.IsNullOrEmpty(Session["ArquivoFichaCambio"].ToString()))
            //            {
            //                lArquivoCambio = String.Concat(lPathArquivo, Session["ArquivoFichaCambio"].ToString());
                            
            //                // Verifica se a ficha da sessão ainda existe
            //                if (!System.IO.File.Exists(lArquivoCambio))
            //                {
            //                    // Caso a ficha não exista, cria uma nova
            //                    GerarFichas();
            //                }
            //            }
            //        }

            //        if (Request["Tipo"].Equals("95a7f5db23b383b98e1ca6b61cf2c6c1"))
            //        {
            //            if (!String.IsNullOrEmpty(Session["ArquivoFicha"].ToString()))
            //            {
            //                lFilePath = String.Concat(lPathArquivo, Session["ArquivoFicha"].ToString());
            //            }

            //        }

            //        if (Request["Tipo"].Equals("1dac8b6c1d8adbaa1a0af55f91bf4e31"))
            //        {
            //            if (!String.IsNullOrEmpty(Session["ArquivoFichaCambio"].ToString()))
            //            {
            //                lFilePath = String.Concat(lPathArquivo, Session["ArquivoFichaCambio"].ToString());
            //            }
            //        }
            //    }

            //    if (Request["Tipo"].Equals("b5f831311230fa38b177d9e768a35cbf"))
            //    {
            //        if (!String.IsNullOrEmpty(Session["ArquivoTermo"].ToString()))
            //        {
            //            lFilePath = String.Concat(lPathArquivo, Session["ArquivoTermo"].ToString());

            //            if (!System.IO.File.Exists(lFilePath))
            //            {
            //                Gradual.Servico.FichaCadastral.Lib.IServicoFichaCadastral lServico = InstanciarServicoDoAtivador<Gradual.Servico.FichaCadastral.Lib.IServicoFichaCadastral>();

            //                Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request.ReceberEntidadeRequest<Gradual.Servico.FichaCadastral.Lib.TermoAdesaoGradualInfo> lRequestTermo = new Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request.ReceberEntidadeRequest<Gradual.Servico.FichaCadastral.Lib.TermoAdesaoGradualInfo>();
            //                OMS.Persistencia.ReceberObjetoResponse<Gradual.Servico.FichaCadastral.Lib.TermoAdesaoGradualInfo> lResponseTermo;

            //                lRequestTermo.Objeto = new Gradual.Servico.FichaCadastral.Lib.TermoAdesaoGradualInfo();

            //                lRequestTermo.Objeto.IdCliente = lSessaoClienteLogado.IdCliente.Value;
            //                lRequestTermo.Objeto.SitemaOrigem = Gradual.Servico.FichaCadastral.Lib.SistemaOrigem.Portal;

            //                lResponseTermo = lServico.GerarTermoDeAdesao(lRequestTermo);

            //                if (lResponseTermo.Objeto != null)
            //                {
            //                    lArquivoTermo = lResponseTermo.Objeto.PathDownloadPdf;
            //                    Session["ArquivoTermo"] = lArquivoTermo;
            //                    lFilePath = String.Concat(lPathArquivo, Session["ArquivoTermo"].ToString());
            //                }
            //            }
            //            else
            //            {
            //                lArquivoTermo = Session["ArquivoTermo"].ToString();
            //                lFilePath = String.Concat(lPathArquivo, lArquivoTermo);
            //            }
            //        }
            //    }

            //    FileLoadToBrowser();
            //}
            //else
            //{
            //    Response.Redirect("~/MinhaConta/Login.aspx");
            //}

            if (!String.IsNullOrEmpty(Request["Param"].ToString()))
            {
                lFilePath = System.String.Concat(System.Configuration.ConfigurationManager.AppSettings["pathVirtualIntranet"], Request["Param"].ToString());
            }

            FileLoadToBrowser();
        }
        //protected void FileLoadToDownload(System.String pFile)
        //{
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=FileName.pdf");
        //    Response.TransmitFile(Server.MapPath("~/Files/FichaCadastral/FichaCadastral-61125.pdf"));
        //    Response.End(); 
        //}

        protected void FileLoadToBrowser()
        {
            

            System.IO.FileStream fs = new System.IO.FileStream(lFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] ar = new byte[(int)fs.Length];
            fs.Read(ar, 0, (int)fs.Length);
            fs.Close();

            if (System.IO.File.Exists(lFilePath))
            {
                System.IO.File.Delete(lFilePath);
            }

            Response.ContentType = "Application/pdf";
            Response.BinaryWrite(ar);

            Response.End();

        }
    }
}
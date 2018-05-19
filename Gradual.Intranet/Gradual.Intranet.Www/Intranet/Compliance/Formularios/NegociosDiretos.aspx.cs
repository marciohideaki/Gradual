using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Monitores.Compliance.Lib;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library.Servicos;
using Newtonsoft.Json;
using System.Text;

namespace Gradual.Intranet.Www.Intranet.Compliance.Formularios
{
    public partial class NegociosDiretos : PaginaBase
    {
        #region Propriedades
        private DateTime? GetData
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request["Data"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        //private DateTime? GetDataAte
        //{
        //    get
        //    {
        //        var lRetorno = default(DateTime);

        //        if (!DateTime.TryParse(this.Request["DataAte"], out lRetorno))
        //            return null;

        //        return lRetorno;
        //    }
        //}

        //private string GetSentido
        //{ 
        //    get
        //    {
        //        return this.Request["Sentido"].ToString();
        //    }
        //}

        //private int? GetCodigoCliente
        //{
        //    get
        //    {
        //        var lRetorno = default(int);

        //        if (!int.TryParse(this.Request["Codigocliente"], out lRetorno))
        //            return null;

        //        return lRetorno;
        //    }

        //}

        private static List<TransporteCompliance.NegociosDiretosInfo> gComplianceNegociosDiretos;

        private List<TransporteCompliance.NegociosDiretosInfo> SessaoUltimaConsulta
        {
            get { return gComplianceNegociosDiretos != null ? gComplianceNegociosDiretos : new List<TransporteCompliance.NegociosDiretosInfo>(); }
            set { gComplianceNegociosDiretos = value; }
        }
        #endregion

        #region Métodos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                      ,"BuscarNegociosDiretos"
                                                      ,"CarregarComoCSV"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderArquivoCSV
                                                     });
        }

        private string ResponderBuscarItensParaListagemSimples()
        {
            NegociosDiretosRequest lRequest = new NegociosDiretosRequest();

            IServicoMonitorCompliance lServico = Ativador.Get<IServicoMonitorCompliance>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            string lRetorno = string.Empty;

            if (this.GetData != null)
            {
                lRequest.Data = this.GetData.Value;
            }

            NegociosDiretosResponse lResponse = new NegociosDiretosResponse();

            lResponse = lServico.ObterNegociosDiretos(lRequest);

            if (lResponse != null && lResponse.lstNegociosDiretos != null)
            {
                this.SessaoUltimaConsulta = new TransporteCompliance().TraduzirLista(lResponse.lstNegociosDiretos);

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsulta);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsulta.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;

                return lRetorno;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private string ResponderArquivoCSV()
        {
            StringBuilder lContent= new StringBuilder();

            if (this.SessaoUltimaConsulta != null)
            {
                lContent.Append("Numero Negócio"        + "\t");
                lContent.Append("Cod. Cliente Vendedor" + "\t");
                lContent.Append("Nome Cliente Vendedor" + "\t");
                lContent.Append("Sentido"               + "\t");
                lContent.Append("Instrumento"           + "\t");
                lContent.Append("Pessoa Vinculada"      + "\t");
                lContent.Append("Data Negocio"          + "\t");
                lContent.Append("Cod. Cliente Comprador"+ "\t");
                lContent.Append("Nome Cliente Comprador"+ "\t");
                lContent.Append("Pessoa Vinculada\r");

                this.SessaoUltimaConsulta.ForEach(negocio => 
                {
                    lContent.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\r", 
                        negocio.NumeroNegocio,
                        negocio.CodigoClienteVendedor,
                        negocio.NomeClienteVendedor,
                        negocio.Sentido,
                        negocio.Instrumento,
                        negocio.PessoaVinculadaVendedor,
                        negocio.DataNegocio,
                        negocio.CodigoClienteComprador,
                        negocio.NomeClienteComprador,
                        negocio.PessoaVinculadaComprador);
                });

                this.Response.Clear();

                this.Response.ContentType = "text/xls";

                this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                this.Response.Charset = "iso-8859-1";

                this.Response.AddHeader("content-disposition", "attachment;filename=NegociosDiretos.xls");

                this.Response.Write(lContent.ToString());

                this.Response.End();
            }

            return string.Empty;
        }

        #endregion
    }
}
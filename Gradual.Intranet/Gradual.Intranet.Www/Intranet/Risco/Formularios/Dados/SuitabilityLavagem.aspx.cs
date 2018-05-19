using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco;
using Newtonsoft.Json;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class SuitabilityLavagem : PaginaBase
    {
        #region Propriedades
        private string GetFiltrarPor
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["sidx"]))
                    return this.Request["sidx"];

                return null;
            }
        }

        private string GetOrdenacao
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["sord"]))
                    return this.Request["sord"];

                return null;
            }
        }

        private int? GetCdAssessor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodAssessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }
        public enumVOLxSFP GetenumVOLxSFP
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["PercentualVOLxSFP"]))
                    return enumVOLxSFP.TODOS;

                return (enumVOLxSFP)Enum.Parse(typeof(enumVOLxSFP), this.Request["PercentualVOLxSFP"]);
            }
        }

        public enumVolume GetVOLUME
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["Volume"]))
                    return enumVolume.TODOS;

                return (enumVolume)Enum.Parse(typeof(enumVolume), this.Request["Volume"]);
            }
        }

        public enumEnquadrado GetEnquadrado
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["Enquadrado"]))
                    return enumEnquadrado.Enquadrado;

                return (enumEnquadrado)Enum.Parse(typeof(enumEnquadrado), this.Request["Enquadrado"]);
            }
        }

        private int? GetCdCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }
        public DateTime GetDataDe
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["DataDe"]))
                    return DateTime.Now.AddDays(-30);

                return DateTime.Parse(this.Request["DataDe"]);
            }
        }

        public DateTime GetDataAte
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["DataAte"]))
                    return DateTime.Now.AddDays(1);

                return DateTime.Parse(this.Request["DataAte"] + " 23:59:59");
            }
        }
        private static List<TransporteSuitabilityLavagem> gListSuitabilityLavagem;

        private List<TransporteSuitabilityLavagem> SessaoUltimaConsulta
        {
            get { return gListSuitabilityLavagem != null ? gListSuitabilityLavagem : new List<TransporteSuitabilityLavagem>(); }
            set { gListSuitabilityLavagem = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarSuitabilityLavagem",
                                                        "ExportarParaExcel"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples,
                                                        this.ResponderExportarParaExcel
                                                     });
        }

        private string ResponderExportarParaExcel()
        {
            string lRetorno = string.Empty;

            var lConteudoArquivo = new StringBuilder();

            SuitabilityLavagemDbLib lServico = new SuitabilityLavagemDbLib();

            lConteudoArquivo.Append("Codigo Cliente\tNome Cliente\tAssessor\tNome Assessor\tVolume\tSFP\t% Vol./SFP\tSuitability\tCodigo Bmf\n");

            SuitabilityLavagemInfo lRetornoConsulta = new SuitabilityLavagemInfo();

            SuitabilityLavagemInfo lRequest = new SuitabilityLavagemInfo();

            if (null != this.GetCdCliente)
            {
                lRequest.CodigoCliente = this.GetCdCliente.Value;
            }

            if (null != this.GetCdAssessor)
            {
                lRequest.CodigoAssessor = this.GetCdAssessor.Value;
            }

            if (base.CodigoAssessor != null)
            {
                lRequest.CodigoAssessor = base.CodigoAssessor.Value;
                lRequest.CodigoLogin = this.UsuarioLogado.Id;
            }

            lRequest.enumVolume = this.GetVOLUME;

            lRequest.enumVOLxSFP = this.GetenumVOLxSFP;

            lRequest.enumEnquadrado = this.GetEnquadrado;

            lRequest.DataDe = this.GetDataDe;

            lRequest.DataAte = this.GetDataAte;

            lRetornoConsulta = lServico.ObterSuitabilityLavagem(lRequest);

            if (lRetornoConsulta != null && lRetornoConsulta.Resultado != null)
            {
                var lTransporte = new TransporteSuitabilityLavagem().TraduzirLista(lRetornoConsulta.Resultado);

                lTransporte.ForEach
                    (sui => 
                    {
                        lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\n", sui.CodigoCliente, sui.NomeCliente, sui.CodigoAssessor, sui.NomeAssessor, sui.Volume, sui.SFP, sui.PercentualVOLxSFP, sui.Suitability, sui.CodigoClienteBmf);
                    });
            }

            this.Response.Clear();

            this.Response.ContentType = "text/xls";

            this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            this.Response.Charset = "iso-8859-1";

            this.Response.AddHeader("content-disposition", string.Format("attachment;filename=SuitabilityLavagem_{0}.xls", DateTime.Now.ToString("dd-MM-yyyy_HH-mm")));

            this.Response.Write(lConteudoArquivo.ToString());

            this.Response.End();

            return base.RetornarSucessoAjax("Sucesso");
            
        }

        private string ResponderBuscarItensParaListagemSimples()
        {
            SuitabilityLavagemDbLib lServico = new SuitabilityLavagemDbLib();

            string lRetorno = string.Empty;

            string lColunas = string.Empty;

            SuitabilityLavagemInfo lRequest = new SuitabilityLavagemInfo();

            TransporteDeListaPaginada lRetornoLista = null;

            if (Session["Usuario"] == null) { return string.Empty; }

            if (null != this.GetCdCliente)
            {
                lRequest.CodigoCliente = this.GetCdCliente.Value;
            }

            if (null != this.GetCdAssessor)
            {
                lRequest.CodigoAssessor = this.GetCdAssessor.Value;
            }

            if (base.CodigoAssessor != null)
            {
                lRequest.CodigoAssessor = base.CodigoAssessor.Value;
                lRequest.CodigoLogin = this.UsuarioLogado.Id;
            }

            lRequest.enumVolume = this.GetVOLUME;

            lRequest.enumVOLxSFP = this.GetenumVOLxSFP;

            lRequest.enumEnquadrado = this.GetEnquadrado;

            lRequest.DataDe = this.GetDataDe;

            lRequest.DataAte = this.GetDataAte;

            SuitabilityLavagemInfo lRetornoConsulta = new SuitabilityLavagemInfo();

            lRetornoConsulta = lServico.ObterSuitabilityLavagem(lRequest);

            if (lRetornoConsulta != null && lRetornoConsulta.Resultado != null)
            {
                List<TransporteSuitabilityLavagem> lListaTransporte = new TransporteSuitabilityLavagem().TraduzirLista(lRetornoConsulta.Resultado);

                this.SessaoUltimaConsulta = lListaTransporte;

                this.ResponderFiltrarPorColuna();

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsulta);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = lRetornoConsulta.Resultado.Count;// this.SessaoUltimaConsulta.Count;

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

        private string ResponderFiltrarPorColuna()
        {
            switch (this.GetFiltrarPor)
            {
                
                case "CodigoCliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.CodigoCliente), int.Parse(lp2.CodigoCliente)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.CodigoCliente), int.Parse(lp1.CodigoCliente)));
                    }
                    break;
                
                case "CodigoAssessor":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.CodigoAssessor), int.Parse(lp2.CodigoAssessor)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.CodigoAssessor), int.Parse(lp1.CodigoAssessor)));
                    }
                    break;
                
                case "NomeCliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.NomeCliente, lp2.NomeCliente));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.NomeCliente, lp1.NomeCliente));
                    }
                    break;
                
                case "NomeAssessor":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.NomeAssessor, lp2.NomeAssessor));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.NomeAssessor, lp1.NomeAssessor));
                    }
                    break;
                
                case "SFP":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SFP), decimal.Parse(lp2.SFP)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SFP), decimal.Parse(lp1.SFP)));
                    }
                    break;
                
                case "Volume":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Volume), decimal.Parse(lp2.Volume)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Volume), decimal.Parse(lp1.Volume)));
                    }
                    break;

                case "PercentualVOLxSFP":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PercentualVOLxSFP), decimal.Parse(lp2.PercentualVOLxSFP)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PercentualVOLxSFP), decimal.Parse(lp1.PercentualVOLxSFP)));
                    }
                    break;
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsulta, "sucesso");
        }
    }
}
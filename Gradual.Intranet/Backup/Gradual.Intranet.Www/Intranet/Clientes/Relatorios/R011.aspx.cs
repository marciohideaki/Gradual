using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R011 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 50;
        public int gTotalClientes = 0;

        #endregion

        #region | Propriedades

        private IEnumerable<TransporteRelatorio_011> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_011>)Session["ListaDeResultados_Relatorio_011"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_011"] = value;
            }
        }

        private int? GetCodCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetClienteCompleto
        {
            get 
            {
                var lRetorno = default(int);
                
                if (!int.TryParse(this.Request["ClientePasso"], out lRetorno))
                    return null;
                
                return lRetorno;
            }
        }

        private int? GetProduto
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["Produtos"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime GetDataDe
        {
            get
            {
                DateTime lRetorno = DateTime.Now.AddDays(-1);

                if (!string.IsNullOrWhiteSpace(this.Request["DataInicial"]))
                    lRetorno = Convert.ToDateTime(this.Request["DataInicial"]);

                return lRetorno;
            }
        }

        private DateTime GetDataAte
        {
            get 
            {
                DateTime lRetorno = DateTime.Today.AddDays(2).AddSeconds(-1);

                if (!string.IsNullOrWhiteSpace(this.Request["DataFinal"]))
                    lRetorno = Convert.ToDateTime(this.Request["DataFinal"]).AddDays(1).AddSeconds(-1);

                return lRetorno;
            }
        }
        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            try
            {
                var lResponse = new ConsultarEntidadeCadastroResponse<ClienteDirectInfo>() ;

                var lRequest = new ConsultarEntidadeCadastroRequest<ClienteDirectInfo>()
                {
                    EntidadeCadastro = new ClienteDirectInfo() 
                    {
                        De             = GetDataDe,
                        Ate            = GetDataAte,
                        CdCblc         = GetCodCliente,
                        StClienteCompleto = GetClienteCompleto.Value,
                        IdProdutoPlano = GetProduto
                        //IdProdutoPlano = GetProduto
                    }
                };

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteDirectInfo>(lRequest);

                //lServico.ConsultarProdutosClienteFiltro(
                //    new ListarProdutosClienteRequest()
                //    {
                //        CdCblc = GetCodCliente,
                //        IdProduto = GetProduto,
                //        De = GetDataDe,
                //        Ate = GetDataAte
                //    });

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {

                    IEnumerable<TransporteRelatorio_011> lLista = from ClienteDirectInfo i in lResponse.Resultado select new TransporteRelatorio_011(i);

                    gTotalClientes = lLista.Count();

                    if (lResponse.Resultado.Count > 0)
                    {
                        if (lLista.Count() >= gTamanhoDaParte)
                        {
                            this.ListaDeResultados = lLista;

                            this.rptRelatorio.DataSource = BuscarParte(1);

                            rowLinhaCarregandoMais.Visible = true;
                        }
                        else
                        {
                            this.rptRelatorio.DataSource = lLista;

                        }

                        this.rptRelatorio.DataBind();

                        rowLinhaDeNenhumItem.Visible = false;
                    }
                    else
                    {
                        rowLinhaDeNenhumItem.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<TransporteRelatorio_011> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_011> lRetorno = new List<TransporteRelatorio_011>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = (pParte - 1) * gTamanhoDaParte;
            lIndiceFinal = lIndiceInicial + gTamanhoDaParte;

            if (null != this.ListaDeResultados)
            {
                for (int a = lIndiceInicial; a < this.ListaDeResultados.Count(); a++)
                {
                    lRetorno.Add(this.ListaDeResultados.ElementAt(a));

                    if (a == lIndiceFinal) break;
                }
            }

            return lRetorno;
        }

        private string ResponderBuscarMaisDados()
        {
            string lRetorno;

            int lParte;

            if (int.TryParse(Request.Form["Parte"], out lParte))
            {
                string lMensagemFim;

                if (null == this.ListaDeResultados || (lParte * gTamanhoDaParte) > this.ListaDeResultados.Count())
                {
                    lMensagemFim = "Fim";
                }
                else
                {
                    lMensagemFim = string.Format("TemMais:Parte {0} de {1}", lParte, Math.Ceiling((double)(this.ListaDeResultados.Count() / gTamanhoDaParte)));
                }


                lRetorno = RetornarSucessoAjax(BuscarParte(lParte), lMensagemFim);
            }
            else
            {
                lRetorno = RetornarSucessoAjax("Fim");
            }

            return lRetorno;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                ResponderBuscarItensParaListagemSimples();
            }
            else if (this.Acao == "BuscarParte")
            {
                Response.Clear();

                string lResponse = ResponderBuscarMaisDados();

                Response.Write(lResponse);

                Response.End();
            }
            else if (this.Acao == "CarregarComoCSV")
            {
                this.ResponderArquivoCSV();
            }
        }

        private void ResponderArquivoCSV()
        {
            StringBuilder lBuilder = new StringBuilder();

            
             var lResponse = new ConsultarEntidadeCadastroResponse<ClienteDirectInfo>() ;

            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteDirectInfo>()
            {
                EntidadeCadastro = new ClienteDirectInfo() 
                {
                    De             = GetDataDe,
                    Ate            = GetDataAte,
                    CdCblc         = GetCodCliente,
                    StClienteCompleto = GetClienteCompleto.Value,
                    IdProdutoPlano = GetProduto
                    //IdProdutoPlano = GetProduto
                }
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteDirectInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {

                IEnumerable<TransporteRelatorio_011> lLista = from ClienteDirectInfo i in lResponse.Resultado select new TransporteRelatorio_011(i);

                gTotalClientes = lLista.Count();

                //Excel.Application lExcelApp = new Excel.Application();

                //Excel.Range lCelulas;

                //lExcelApp.Workbooks.Add(Type.Missing);

                //lExcelApp.Visible = true;

                //lExcelApp.Cells[1, 1] = string.Format("Clientes Encontrados:{0}\r\n", gTotalClientes);

                //lExcelApp.ActiveWorkbook.SaveCopyAs("RelatorioClientesDirect.xls");

                lBuilder.AppendLine("Cod. Bolsa\tNome Cliente\tAssessor\tCpf/Cnpj\tProduto\tData de Adesão\t\r\n");

                foreach (TransporteRelatorio_011 info in lLista)
                {
                    lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\r\n", info.CodBolsa, info.NomeCliente, info.CpfCnpj, info.CodAssessor, info.Produto, info.DataAdesao);
                }

                Response.Clear();

                Response.ContentType = "text/xls";

                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                Response.Charset = "iso-8859-1";

                Response.AddHeader("content-disposition", "attachment;filename=RelatorioClientesDirect.xls");

                Response.Write(lBuilder.ToString());

                //Response.Write(lExcelApp.ActiveWorkbook);

                Response.End();

                //lExcelApp.ActiveWorkbook.Saved = true;

                //lExcelApp.Quit();
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.OMS.Library;
using System.Text;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R015 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 50;
        public int gTotalClientes = 0;

        #endregion

        #region | Propriedades

        private IEnumerable<TransporteRelatorio_015> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_015>)Session["ListaDeResultados_Relatorio_015"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_015"] = value;
            }
        }

        public int? GetCodigoAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0)
                    return base.CodigoAssessor.Value;

                return null;
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
                    return 4;

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
                DateTime lRetorno = new DateTime();

                if (!string.IsNullOrWhiteSpace(this.Request["DataInicial"]))
                    lRetorno = Convert.ToDateTime(this.Request["DataInicial"]);

                return lRetorno;
            }
        }

        private DateTime GetDataAte
        {
            get
            {
                DateTime lRetorno = new DateTime();

                if (!string.IsNullOrWhiteSpace(this.Request["DataFinal"]))
                    lRetorno = Convert.ToDateTime(this.Request["DataFinal"]);

                lRetorno = new DateTime(lRetorno.Year, lRetorno.Month, lRetorno.Day, 13, 59, 59);

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

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

        #endregion

        #region | Metodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            try
            {

                var lResponse = new ConsultarEntidadeCadastroResponse<ClienteProdutoInfo>();

                var lRequest = new ConsultarEntidadeCadastroRequest<ClienteProdutoInfo>()
                {
                    EntidadeCadastro = new ClienteProdutoInfo()
                    {
                        De = this.GetDataDe,
                        Ate = this.GetDataAte,
                        CdCblc = this.GetCodCliente,
                        IdProdutoPlano = this.GetProduto,
                        CodigoAssessor = this.GetCodigoAssessor
                        //StClienteCompleto = GetClienteCompleto.Value,
                        //IdProdutoPlano = GetProduto
                    }
                };

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteProdutoInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (null != lResponse.Resultado)
                        lResponse.Resultado.Sort((cp1, cp2) => { return string.Compare(cp1.NomeCliente.Trim(), cp2.NomeCliente.Trim()); });

                    IEnumerable<TransporteRelatorio_015> lLista = from ClienteProdutoInfo i in lResponse.Resultado select new TransporteRelatorio_015(i);

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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<TransporteRelatorio_015> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_015> lRetorno = new List<TransporteRelatorio_015>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = (pParte - 1) * gTamanhoDaParte;
            lIndiceFinal = lIndiceInicial + gTamanhoDaParte;

            if (null != this.ListaDeResultados)
            {
                for (int a = lIndiceInicial; a < this.ListaDeResultados.Count(); a++)
                {
                    if (a == lIndiceFinal) break;

                    lRetorno.Add(this.ListaDeResultados.ElementAt(a));
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

        private void ResponderArquivoCSV()
        {
            StringBuilder lBuilder = new StringBuilder();


            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteProdutoInfo>();

            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteProdutoInfo>()
            {
                EntidadeCadastro = new ClienteProdutoInfo()
                {
                    De = GetDataDe,
                    Ate = GetDataAte,
                    CdCblc = GetCodCliente,
                    StClienteCompleto = GetClienteCompleto.Value,
                    IdProdutoPlano = GetProduto
                    
                }
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteProdutoInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {

                IEnumerable<TransporteRelatorio_015> lLista = from ClienteProdutoInfo i in lResponse.Resultado select new TransporteRelatorio_015(i);

                gTotalClientes = lLista.Count();

                lBuilder.AppendLine("Cod. Bolsa\tNome Cliente\tCpf/Cnpj\tDescrição do Produto\tData Início do Plano\tData Fim do Plano\tData Ultima Cobrança\tValor Cobrado\t\r\n");

                foreach (TransporteRelatorio_015 info in lLista)
                {
                    lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t\r\n", info.CodBolsa, info.NomeCliente, info.CpfCnpj, info.Produto, info.DataAdesao, info.DataFimAdesao, info.DataCobranca, info.ValorCobranca);
                }

                Response.Clear();

                Response.ContentType = "text/xls";

                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                Response.Charset = "iso-8859-1";

                Response.AddHeader("content-disposition", "attachment;filename=RelatorioClientesIR.xls");

                Response.Write(lBuilder.ToString());

                Response.End();

                
            }
        }

        #endregion
    }
}
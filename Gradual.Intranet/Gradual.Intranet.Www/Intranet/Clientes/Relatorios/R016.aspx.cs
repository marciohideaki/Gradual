using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R016 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 50;

        #endregion

        #region | Propriedades

        private int? GetCdCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CodCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetIdProduto
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["ProdutosPoupe"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno))
                    return DateTime.Today.AddDays(1D);

                return lRetorno.AddDays(1D);
            }
        }

        private List<TransporteRelatorio_016> ListaDeResultados
        {
            get
            {
                return (List<TransporteRelatorio_016>)Session["ListaDeResultados_Relatorio_016"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_016"] = value;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                this.ResponderBuscarItensParaListagemSimples();
            }
            else if (this.Acao == "BuscarParte")
            {
                this.Response.Clear();

                string lResponse = this.ResponderBuscarMaisDados();

                this.Response.Write(lResponse);

                this.Response.End();
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePlanoPoupeInfo>(
                new ConsultarEntidadeCadastroRequest<ClientePlanoPoupeInfo>()
                {
                    EntidadeCadastro = new ClientePlanoPoupeInfo()
                    {
                        ConsultaCdCliente = this.GetCdCliente,
                        ConsultaIdProduto = this.GetIdProduto,
                        ConsultaDtFim = this.GetDataFinal,
                        ConsultaDtInicio = this.GetDataInicial,
                    }
                });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count > 0)
                {
                    var lLista = new TransporteRelatorio_016().TraduzirLista(lResponse.Resultado);

                    if (lLista.Count >= this.gTamanhoDaParte)
                    {
                        this.ListaDeResultados = lLista;

                        this.rptClientePlanoPoupe.DataSource = this.BuscarParte(1);

                        this.rowLinhaCarregandoMais.Visible = true;
                    }
                    else
                    {
                        this.rptClientePlanoPoupe.DataSource = lLista;
                    }

                    this.rptClientePlanoPoupe.DataBind();

                    this.rowLinhaDeNenhumItem.Visible = false;
                }
                else
                {
                    this.rowLinhaDeNenhumItem.Visible = true;
                }
            }
        }

        private List<TransporteRelatorio_016> BuscarParte(int pParte)
        {
            var lRetorno = new List<TransporteRelatorio_016>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = (pParte - 1) * gTamanhoDaParte;
            lIndiceFinal = lIndiceInicial + gTamanhoDaParte;

            if (null != this.ListaDeResultados)
            {
                var lCount = lIndiceInicial;

                while (lCount != lIndiceFinal && lCount < this.ListaDeResultados.Count)
                {
                    lRetorno.Add(this.ListaDeResultados.ElementAt(lCount));

                    lCount++;
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


                lRetorno = base.RetornarSucessoAjax(BuscarParte(lParte), lMensagemFim);
            }
            else
            {
                lRetorno = base.RetornarSucessoAjax("Fim");
            }

            return lRetorno;
        }

        #endregion
    }
}
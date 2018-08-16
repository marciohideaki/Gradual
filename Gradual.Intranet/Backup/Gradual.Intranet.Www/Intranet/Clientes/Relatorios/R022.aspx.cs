using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R022 : PaginaBaseAutenticada
    {
        #region Properties
        public DateTime DataInicial;
        public DateTime DataFinal;
        private int? GetAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                return null;
            }
        }

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

        public DateTime GetDataInicial
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno);

                DataInicial = lRetorno;

                return lRetorno;
            }
        }

        public DateTime GetDataFinal
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

                DataFinal = lRetorno;

                return lRetorno;
            }
        }

        private string GetPapel
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["papel"]))
                    lRetorno = this.Request["papel"];

                return lRetorno;
            }
        }
        #endregion

        #region Eventos
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

                ResponderBuscarItensParaListagemSimples();
                //Response.Write(lResponse);

                Response.End();
            }
        }
        #endregion

        #region Métodos
        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new PapelPorClienteInfo()
            {
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DataInicial     = this.GetDataInicial,
                DataFinal       = this.GetDataFinal,
                CodigoAssessor  = this.GetAssessor,
                CodigoCliente   = this.GetCdCliente,
            };

            try
            {
                PapelPorClienteDbLib lServico = new PapelPorClienteDbLib();

                var lResponse = lServico.ConsultarPapeisPorCliente(lRequest);

                if (lResponse.Resultado != null && lResponse.Resultado.Count > 0)
                {
                    //List<PapelPorClienteInfo> lListaTemp = lResponse.Resultado;//.OrderBy(lp => lp.CodigoAssessor).ToList();

                    //List<PapelPorClienteInfo> lListaBusca = new List<PapelPorClienteInfo>();

                    //lListaTemp.ForEach(result =>
                    //{
                    //    PapelPorClienteInfo lCad = new PapelPorClienteInfo();

                    //    lCad = result;

                    //    PapelPorClienteInfo lCadBusca = lListaBusca.Find(busca => busca.CodigoAssessor == lCad.CodigoAssessor);

                    //    if (lCadBusca != null)
                    //    {
                    //        lListaBusca.Remove(lCadBusca);
                    //        lListaBusca.Add(lCadBusca);
                    //    }
                    //    else
                    //    {
                    //        lListaBusca.Add(lCad);
                    //    }
                    //});

                    string lPapel = GetPapel;

                    List<TransporteRelatorio_022> lListaTemp = new TransporteRelatorio_022().TraduzirLista(lResponse.Resultado, lPapel.ToUpper());

                    this.rptRelatorio.DataSource = lListaTemp;

                    this.rptRelatorio.DataBind();

                    rowLinhaDeNenhumItem.Visible = false;
                }
                else
                {
                    rowLinhaDeNenhumItem.Visible = true;
                }

            }
            catch (Exception exBusca)
            {
                throw exBusca;
            }
        }
        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R005 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 50;

        #endregion

        #region | Propriedades

        private IEnumerable<TransporteRelatorio_005> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_005>)Session["ListaDeResultados_Relatorio_005"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_005"] = value;
            }
        }

        private string GetTipoPessoa
        {
            get
            {
                string lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request.Form["TipoPessoa"]))
                    lRetorno = this.Request.Form["TipoPessoa"];

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

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

                return lRetorno.AddDays(1D);
            }
        }

        private int? GetAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                var lRetorno = default(int);

                if (int.TryParse(this.Request.Form["Assessor"], out lRetorno))
                    return lRetorno;

                return null;
            }
        }

        private string GetCpfCnpj
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["CpfCnpj"]))
                    return null;

                return this.Request.Form["CpfCnpj"];
            }
        }

        private string GetBolsa
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Bolsa"]))
                    return null;

                return this.Request.Form["Bolsa"];
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<ClientesExportadosSinacorInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClientesExportadosSinacorInfo>();

            try
            {
                string lDataInicial = Request.Form["DataInicial"], lDataFinal = Request.Form["DataFinal"],
                    lAssessor = Request.Form["Assessor"], lBolsa = Request.Form["Bolsa"], lCpfCnpj = Request.Form["CpfCnpj"];

                ClientesExportadosSinacorInfo lInfo = new ClientesExportadosSinacorInfo()
                {
                    DtDe           = this.GetDataInicial,
                    DtAte          = this.GetDataFinal,
                    CodigoAssessor = this.GetAssessor,
                    DsCpfCnpj      = this.GetCpfCnpj,
                    CdBolsa        = this.GetBolsa,
                    TipoPessoa     = this.GetTipoPessoa
                };

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientesExportadosSinacorInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<TransporteRelatorio_005> lLista = from ClientesExportadosSinacorInfo i in lResponse.Resultado select new TransporteRelatorio_005(i);

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
            catch (Exception exBusca)
            {

                throw exBusca;
            }
        }

        private List<TransporteRelatorio_005> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_005> lRetorno = new List<TransporteRelatorio_005>();

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
        }

        #endregion
    }
}

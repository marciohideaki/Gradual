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
    public partial class R027 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 50;

        private Gradual.Intranet.Contratos.Dados.Cadastro.ListaAssessoresVinculadosInfo gListaAssessoresVinculados;

        #endregion
        
        #region | Propriedades

        private int? GetCodigoGradual
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CodigoGradual"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCodigoExterno
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CodigoExterno"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private IEnumerable<Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027> ListaDeResultados
        {
            get
            {
                return (IEnumerable<Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027>)Session["ListaDeResultados_Relatorio_027"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_027"] = value;
            }
        }

        private int? GetCodigoAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                return null;
            }
        }

        private Gradual.Intranet.Contratos.Dados.Cadastro.ListaAssessoresVinculadosInfo GetListaAssessoresVinculados
        {
            get
            {
                if (null == this.gListaAssessoresVinculados)
                    this.gListaAssessoresVinculados = base.ConsultarCodigoAssessoresVinculadosLista(this.GetCodigoAssessor);

                return this.gListaAssessoresVinculados;
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
            else if (this.Acao == "CarregarComoCSV")
            {
                this.ResponderArquivoCSV();
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo>();

            string lPrefixo = this.PrefixoDaRaiz;

            try
            {

                string lListaAssessores = String.Empty;

                if (!base.EhAdministrador && this.GetListaAssessoresVinculados != null)
                {
                    foreach (int assessor in this.GetListaAssessoresVinculados.ListaCodigoAssessoresVinculados)
                    {
                        if (String.IsNullOrEmpty(lListaAssessores))
                        {
                            lListaAssessores += String.Format("{0}", assessor);
                        }
                        else
                        {
                            lListaAssessores += String.Format(",{0}", assessor);
                        }
                    }
                }

                var lInfo = new Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo()
                {
                    CodigoGradual = this.GetCodigoGradual,
                    CodigoExterno = this.GetCodigoExterno,
                    CodigoAssessor = lListaAssessores
                };

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027> lLista = from Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo i in lResponse.Resultado select new Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027(i);

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

        private List<Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027> BuscarParte(int pParte)
        {
            List<Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027> lRetorno = new List<Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027>();

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


                lRetorno = base.RetornarSucessoAjax(BuscarParte(lParte), lMensagemFim);
            }
            else
            {
                lRetorno = base.RetornarSucessoAjax("Fim");
            }

            return lRetorno;
        }

        private Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo GerarRequest()
        {
            int lCodigoGradual = default(int);
            int lCodigoExterno = default(int);

            if (!int.TryParse(this.Request["CodigoGradual"], out lCodigoGradual))
            {
                lCodigoGradual = default(int);
            }

            if(!int.TryParse(this.Request["CodigoExterno"], out lCodigoExterno))
            {
                lCodigoExterno = default(int);
            }

            string lListaAssessores = String.Empty;

            if (!base.EhAdministrador && this.GetListaAssessoresVinculados != null)
            {
                foreach (int assessor in this.GetListaAssessoresVinculados.ListaCodigoAssessoresVinculados)
                {
                    if (String.IsNullOrEmpty(lListaAssessores))
                    {
                        lListaAssessores += String.Format("{0}", assessor);

                    }
                    else
                    {
                        lListaAssessores += String.Format(",{0}", assessor);
                    }
                }
            }

            return new Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo()
            {
                CodigoGradual = lCodigoGradual.Equals(0) ? (int?)null : lCodigoGradual,
                CodigoExterno = lCodigoExterno.Equals(0) ? (int?)null : lCodigoExterno ,
                CodigoAssessor = lListaAssessores
            };
        }


        private void ResponderArquivoCSV()
        {

            System.Text.StringBuilder lBuilder = new System.Text.StringBuilder();

            var lRequest = new ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo>();

            string lPrefixo = this.PrefixoDaRaiz;

            try
            {

                

                string lListaAssessores = String.Empty;

                if (!base.EhAdministrador && this.GetListaAssessoresVinculados != null)
                {
                    foreach (int assessor in this.GetListaAssessoresVinculados.ListaCodigoAssessoresVinculados)
                    {
                        if (String.IsNullOrEmpty(lListaAssessores))
                        {
                            lListaAssessores += String.Format("{0}", assessor);

                        }
                        else
                        {
                            lListaAssessores += String.Format(",{0}", assessor);
                        }
                    }
                }

                //var lInfo = new Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo()
                //{
                //    CodigoGradual = this.GetCodigoGradual,
                //    CodigoExterno = this.GetCodigoExterno,
                //    CodigoAssessor = lListaAssessores
                //};

                var lInfo = GerarRequest();

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027> lLista = from Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo i in lResponse.Resultado select new Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027(i);

                        if (lLista.Count() > 0)
                        {
                            lBuilder.AppendLine("Conversão de contas Plural x Gradual");
                            lBuilder.AppendLine("CodigoGradual\tCodigoAssessor\tCodigoExterno\tDigitoExterno\tNome\n");

                            foreach (Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente.TransporteRelatorio_027 lOcorrencia in lLista)
                            {
                                lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n"
                                , lOcorrencia.CodigoGradual
                                , lOcorrencia.CodigoAssessor
                                , lOcorrencia.CodigoExterno
                                , lOcorrencia.DigitoExterno
                                , lOcorrencia.Nome);
                            }

                            Response.Clear();

                            Response.ContentType = "text/xls";

                            Response.ContentEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");

                            Response.Charset = "iso-8859-1";

                            Response.AddHeader("content-disposition", "attachment;filename=ClientesDePara.xls");

                            Response.Write(lBuilder.ToString());

                            Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

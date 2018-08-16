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
    public partial class R006 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 50;

        #endregion
        
        #region | Propriedades

        private Nullable<bool> GetRealizado
        {
            get
            {
                if (null == this.Request.Form["Realizado"] || "0".Equals(this.Request.Form["Realizado"]))
                    return false;

                else if (null != this.Request.Form["Realizado"] && "1".Equals(this.Request.Form["Realizado"]))
                    return true;

                return null;
            }
        }

        private DateTime? GetDataInicial
        {
            get 
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(Request.Form["DataInicial"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime? GetDataFinal
        {
            get 
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetIdAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Assessor"], out lRetorno))
                    return null;

                return lRetorno;
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

        private int? GetCodCliente    
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CodCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private IEnumerable<TransporteRelatorio_006> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_006>)Session["ListaDeResultados_Relatorio_006"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_006"] = value;
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
            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteSuitabilityEfetuadoInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteSuitabilityEfetuadoInfo>();

            string lPrefixo = this.PrefixoDaRaiz;

            try
            {
                var lInfo = new ClienteSuitabilityEfetuadoInfo()
                {
                    IdCliente      = this.GetCodCliente,
                    DtDe           = this.GetDataInicial,
                    DtAte          = this.GetDataFinal.Value.AddDays(1D),
                    DsCpfCnpj      = this.GetCpfCnpj,
                    CodigoAssessor = this.GetIdAssessor,
                    StRealizado    = this.GetRealizado,
                    TipoPessoa     = this.GetTipoPessoa
                };

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSuitabilityEfetuadoInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<TransporteRelatorio_006> lLista = from ClienteSuitabilityEfetuadoInfo i in lResponse.Resultado select new TransporteRelatorio_006(i, lPrefixo);

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

        private List<TransporteRelatorio_006> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_006> lRetorno = new List<TransporteRelatorio_006>();

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

        private ClienteSuitabilityEfetuadoInfo GerarRequest()
        {
            DateTime lDataInicial           = default(DateTime);
            DateTime lDataFinal             = default(DateTime);
            String lTipoPessoa              = String.Empty;
            String lCpfCnpj                 = String.Empty;

            DateTime.TryParse(this.Request["DataInicial"], out lDataInicial);
            DateTime.TryParse(this.Request["DataFinal"], out lDataFinal);

            var lCodAssessor = default(int);

            if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                lCodAssessor = base.CodigoAssessor.Value;

            if (!int.TryParse(this.Request["CodAssessor"], out lCodAssessor))
                lCodAssessor = 0;

            if (!string.IsNullOrWhiteSpace(this.Request["TipoPessoa"]))
            {
                lTipoPessoa = this.Request["TipoPessoa"];
            }

            if (!string.IsNullOrWhiteSpace(this.Request["CpfCnpj"]))
            {
                lCpfCnpj = this.Request["CpfCnpj"];
            }

            var lCodCliente = default(int);

            if (!int.TryParse(this.Request["CodCliente"], out lCodCliente))
                lCodCliente = 0;

            return new ClienteSuitabilityEfetuadoInfo()
            {
                DtDe = lDataInicial,
                DtAte = lDataFinal,
                DsCpfCnpj = lCpfCnpj,
                CodigoAssessor = lCodAssessor,
                TipoPessoa = this.GetTipoPessoa,
                IdCliente = lCodCliente
            };
        }


        private void ResponderArquivoCSV()
        {
            System.Text.StringBuilder lBuilder = new System.Text.StringBuilder();

            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteSuitabilityEfetuadoInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteSuitabilityEfetuadoInfo>();

            string lPrefixo = this.PrefixoDaRaiz;

            try
            {
                var lInfo = GerarRequest();

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSuitabilityEfetuadoInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<TransporteRelatorio_006> lLista = from ClienteSuitabilityEfetuadoInfo i in lResponse.Resultado select new TransporteRelatorio_006(i, lPrefixo);

                        if (lLista.Count() > 0)
                        {
                            lBuilder.AppendLine("idcliente\tbovespa\tnome\tcpfcnpj\tassessor\tdatarealizacao\tstatus\tresultado\tlocalrealizacao\tpeso\trespostas\trealizadopelocliente\tciencia\t");

                            foreach (TransporteRelatorio_006 lOcorrencia in lLista)
                            {
                                lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\r\n"
                                , lOcorrencia.Id
                                , lOcorrencia.CodigoBovespa
                                , lOcorrencia.Nome
                                , lOcorrencia.CpfCnpj
                                , lOcorrencia.Assessor
                                , lOcorrencia.UltimaAlteracaoSuitability
                                , lOcorrencia.Status
                                , lOcorrencia.ResultadoDaAnalise
                                , lOcorrencia.Local
                                , lOcorrencia.Peso
                                , lOcorrencia.Respostas
                                , lOcorrencia.RealizadoPeloCliente
                                , lOcorrencia.ArquivoCienciaLink);
                            }

                            Response.Clear();

                            Response.ContentType = "text/xls";

                            Response.ContentEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");

                            Response.Charset = "iso-8859-1";

                            Response.AddHeader("content-disposition", "attachment;filename=ClientesEfetuaramSuitability.xls");

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

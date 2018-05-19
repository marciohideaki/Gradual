using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R008 : PaginaBaseAutenticada
    {
        
        #region Globais

        private int gTamanhoDaParte = 50;

        #endregion
         
        #region | Propriedades

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

                return lRetorno;
            }
        }

        private eTipoEmailDisparo GetTipoEmail
        {
            get 
            {
                var lTipoEmail = this.Request.Form["TipoEmail"];
                if ("1".Equals(lTipoEmail))
                    return eTipoEmailDisparo.Assessor;
                else if ("2".Equals(lTipoEmail))
                    return eTipoEmailDisparo.Compliance;
                else
                    return eTipoEmailDisparo.Todos;
            }
        }

        private string GetEmailDestinatario
        {
            get 
            {
                if(string.IsNullOrWhiteSpace(this.Request.Form["EmailDisparado"]))
                    return string.Empty;

                return this.Request.Form["EmailDisparado"].Trim();
            }
        }

        private int GetCodCliente
        {
            get
            {
                var lRetorno = 0;

                Int32.TryParse(this.Request.Form["CodCliente_R008"], out lRetorno);
                
                return lRetorno;
            }
        }

        private string GetCpfCnpj
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["CpfCnpj"]))
                    return string.Empty;

                return this.Request.Form["CpfCnpj"].Trim();
            }
        }

        private IEnumerable<TransporteRelatorio_008> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_008>)Session["ListaDeResultados_Relatorio_008"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_008"] = value;
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

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<EmailDisparadoPeriodoInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome  };
            var lResponse = new ConsultarEntidadeCadastroResponse<EmailDisparadoPeriodoInfo>();

            try
            {
                EmailDisparadoPeriodoInfo lInfo = new EmailDisparadoPeriodoInfo()
                {
                    DtDe                = this.GetDataInicial,
                    DtAte               = this.GetDataFinal.AddDays(1D),
                    ETipoEmailDisparo   = this.GetTipoEmail,
                    DsEmailDestinatario = this.GetEmailDestinatario,
                    TipoPessoa          = this.GetTipoPessoa,
                    IdCliente           = this.GetCodCliente,
                    DsCpfCnpj           = this.GetCpfCnpj
                };

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<EmailDisparadoPeriodoInfo>(lRequest);

                if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<TransporteRelatorio_008> lLista = from EmailDisparadoPeriodoInfo i in lResponse.Resultado select new TransporteRelatorio_008(i);

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

        private List<TransporteRelatorio_008> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_008> lRetorno = new List<TransporteRelatorio_008>();

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

        private EmailDisparadoPeriodoInfo GerarRequest()
        {
            DateTime lDataInicial           = default(DateTime);
            DateTime lDataFinal             = default(DateTime);
            eTipoEmailDisparo lTipoEmail    = eTipoEmailDisparo.Todos;
            String lEmailDestinatario       = String.Empty;
            String lTipoPessoa              = String.Empty;
            Int32 lCodCliente               = 0;
            String lCpfCnpj                 = String.Empty;

            DateTime.TryParse(this.Request["DataInicial"], out lDataInicial);
            DateTime.TryParse(this.Request["DataFinal"], out lDataFinal);


            var lTipo = this.Request["TipoEmail"];

            if (lTipo.Equals("1"))
            {
                lTipoEmail = eTipoEmailDisparo.Assessor;
            }
            
            if (lTipo.Equals("2"))
            {
                lTipoEmail = eTipoEmailDisparo.Compliance;
            }


            if (!string.IsNullOrWhiteSpace(this.Request["EmailDisparado"]))
            {
                lEmailDestinatario = this.Request["EmailDisparado"];
            }


            if (!string.IsNullOrWhiteSpace(this.Request["TipoPessoa"]))
            {
                lTipoPessoa = this.Request["TipoPessoa"];
            }

            Int32.TryParse(this.Request["CodCliente"], out lCodCliente);

            if (!string.IsNullOrWhiteSpace(this.Request["CpfCnpj"]))
            {
                lCpfCnpj = this.Request["CpfCnpj"];
            }

            return new EmailDisparadoPeriodoInfo()
            {
                DtDe = lDataInicial,
                DtAte = lDataFinal,
                ETipoEmailDisparo = lTipoEmail,
                DsEmailDestinatario = lEmailDestinatario,
                TipoPessoa = lTipoPessoa,
                IdCliente = lCodCliente,
                DsCpfCnpj = lCpfCnpj
            };
        }

        private void ResponderArquivoCSV()
        {
            System.Text.StringBuilder lBuilder = new System.Text.StringBuilder();
            
            var lRequest = new ConsultarEntidadeCadastroRequest<EmailDisparadoPeriodoInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome  };
            var lResponse = new ConsultarEntidadeCadastroResponse<EmailDisparadoPeriodoInfo>();

            try
            {

                var lInfo = GerarRequest();

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<EmailDisparadoPeriodoInfo>(lRequest);

                if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<TransporteRelatorio_008> lLista = from EmailDisparadoPeriodoInfo i in lResponse.Resultado select new TransporteRelatorio_008(i);

                        if (lLista.Count() > 0)
                        {
                            lBuilder.AppendLine("nome\tcodbovespa\tcpfcnpj\temail\tdataenvio\temailremetente\temaildestinatario\tassunto\tperfil\t");

                            foreach (TransporteRelatorio_008 lEmail in lLista)
                            {
                                lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n"
                                    , lEmail.Nome
                                    , lEmail.Bovespa
                                    , lEmail.CpfCnpj
                                    , lEmail.Email  
                                    , lEmail.DataDeEnvio
                                    , lEmail.DsEmailRemetente
                                    , lEmail.DsEmailDestinatario
                                    , lEmail.Assunto
                                    , lEmail.Perfil);
                            }

                            Response.Clear();

                            Response.ContentType = "text/xls";

                            Response.ContentEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");

                            Response.Charset = "iso-8859-1";

                            Response.AddHeader("content-disposition", "attachment;filename=RelatorioEmailsDisparados.xls");

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
    }
}

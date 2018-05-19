using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Library.Servicos;
using Gradual.IntegracaoCMRocket.Lib;
using Gradual.IntegracaoCMRocket.Lib.Mensagens;
using Gradual.IntegracaoCMRocket.Lib.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.IntegracaoRocket
{
    public partial class Sumario : PaginaBaseAutenticada
    {
        public String GetEmail
        {
            get
            {
                return Request["Email"];
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

        private int? GetCodigo
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Codigo"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetTransacao
        {
            get
            {
                string lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request.Form["Transacao"]))
                {
                    if (this.Request.Form["Transacao"].Equals("TODOS"))
                    {
                        lRetorno = null;
                    }
                    else
                    {
                        lRetorno = this.Request.Form["Transacao"];
                    }
                }

                if (String.IsNullOrEmpty(lRetorno))
                {
                    return null;
                }

                return lRetorno;
            }
        }

        private string GetStatus
        {
            get
            {
                string lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request.Form["Status"]))
                {
                    if (this.Request.Form["Status"].Equals("TODOS"))
                    {
                        lRetorno = null;
                    }
                    else
                    {
                        lRetorno = this.Request.Form["Status"];
                    }
                }

                if (String.IsNullOrEmpty(lRetorno))
                {
                    return null;
                }

                return lRetorno;
            }
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] 
            { 
                "CarregarHtmlComDados"
                , "Pesquisar"
                , "SolicitarDocumentos"
                , "SolicitarDescricoes"
            },
            new ResponderAcaoAjaxDelegate[] 
            { 
                CarregarHtmlComDados
                , Pesquisar
                , SolicitarDocumentos
                , SolicitarDescricoes
            });
        }

        private string CarregarHtmlComDados()
        {

            IServicoIntegracaoCMRocket lServico = Ativador.Get<IServicoIntegracaoCMRocket>();

            ObterRelatorioDetalhadoRequest lParametros = new ObterRelatorioDetalhadoRequest();
            lParametros.DataInicial     = System.DateTime.Now.AddDays(-1);
            lParametros.DataFinal       = System.DateTime.Now;
            ObterRelatorioDetalhadoResponse lResponse = lServico.ObterRelatorios(lParametros);

            //this.rptProcessos.DataSource = lResponse.Capivaras;
            //this.rptProcessos.DataBind();

            //return String.Empty;

            //return JsonConvert.SerializeObject(lResponse);

            return RetornarSucessoAjax(lResponse, "Sucesso");
        }

        private String Pesquisar()
        {
            IServicoIntegracaoCMRocket lServico = Ativador.Get<IServicoIntegracaoCMRocket>();

            ObterRelatorioDetalhadoRequest lParametros = new ObterRelatorioDetalhadoRequest();

            lParametros.Codigo      = this.GetCodigo;
            lParametros.Transacao   = this.GetTransacao;
            lParametros.Status      = this.GetStatus;
            lParametros.DataInicial = this.GetDataInicial;
            lParametros.DataFinal   = this.GetDataFinal;

            ObterRelatorioDetalhadoResponse lResponse = lServico.ObterRelatorios(lParametros);

            return RetornarSucessoAjax(lResponse, "Sucesso");
        }

        private String SolicitarDescricoes()
        {
            IServicoIntegracaoCMRocket lServico = Ativador.Get<IServicoIntegracaoCMRocket>();

            ObterDescricoesRequest lParametros = new ObterDescricoesRequest();

            lParametros.Codigo  = null; //this.GetCodigo;
            lParametros.Nome    = null; //this.GetTransacao;

            ObterDescricoesResponse  lResponse = lServico.ObterDescricoes(lParametros);

            return RetornarSucessoAjax(lResponse, "Sucesso");
        }

        private string SolicitarDocumentos()
        {
            String lRetorno = String.Empty;

            //EnviarEmail(new List<String>() { pEmail }, "Gradual Investimentos - Pendência Cadastral", "CadastroPendenciaCadastral.htm", null, eTipoEmailDisparo.Todos);

            Gradual.OMS.Library.MensagemResponseStatusEnum lRespostaEnvioEmail =  EnviarEmail(new List<String>() { GetEmail }, "Gradual Investimentos - Pendência Cadastral", "CadastroPendenciaCadastral.htm", new Dictionary<String, String>(), eTipoEmailDisparo.Todos);

            if (lRespostaEnvioEmail.Equals(Gradual.OMS.Library.MensagemResponseStatusEnum.OK))
            {
                IServicoIntegracaoCMRocket lServico = Ativador.Get<IServicoIntegracaoCMRocket>();

                InserirSolicitacaoDocumentacaoRequest lParametros = new InserirSolicitacaoDocumentacaoRequest();

                if (null != this.GetCodigo)
                {
                    int lCodigo = this.GetCodigo ?? default(int);

                    lParametros.Codigo  = lCodigo;
                    lParametros.Email   = this.GetEmail;

                    InserirSolicitacaoDocumentacaoResponse lRespostaInsercao = lServico.InserirSolicitacaoDocumentos(lParametros);

                    if (lRespostaInsercao.StatusResposta.Equals(Gradual.OMS.Library.MensagemResponseStatusEnum.OK))
                    {
                        lRetorno = RetornarSucessoAjax("Email enviado com sucesso!");
                    }
                }
                else
                {
                    lRetorno = RetornarErroAjax("Email enviado com sucesso porém não foi possível registrar o envio!");
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Erro no envio do email!");
            }

            return lRetorno;
        }


        /// <summary>
        /// Formatar uma string CNPJ
        /// </summary>
        /// <param name="CNPJ">string CNPJ sem formatacao</param>
        /// <returns>string CNPJ formatada</returns>
        /// <example>Recebe '99999999999999' Devolve '99.999.999/9999-99'</example>

        public static string FormatCNPJ(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="CPF">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>

        public static string FormatCPF(string CPF)
        {
            return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
        }
        /// <summary>
        /// Retira a Formatacao de uma string CNPJ/CPF
        /// </summary>
        /// <param name="Codigo">string Codigo Formatada</param>
        /// <returns>string sem formatacao</returns>
        /// <example>Recebe '99.999.999/9999-99' Devolve '99999999999999'</example>

        public static string SemFormatacao(string Codigo)
        {
            return Codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        }

        public static string FormatarDocumento(string Codigo)
        {
            if (Codigo.Trim().Length.Equals(11))
            {
                return FormatCPF(Codigo);
            }
            else if (Codigo.Trim().Length.Equals(14))
            {
                return FormatCNPJ(Codigo);
            }
            else
            {
                return Codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            }
        }

        public string ValidarStatus(String pStatus)
        {
            switch (pStatus)
            {
                case "APROVADO":
                    return "aprovado";
                case "REPROVADO":
                    return "reprovado";
            }

            return "neutro";

        }
    }
}
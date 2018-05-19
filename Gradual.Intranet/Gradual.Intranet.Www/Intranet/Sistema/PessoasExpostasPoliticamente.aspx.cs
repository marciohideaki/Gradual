using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Sistema
{
    public partial class PessoasExpostasPoliticamente : PaginaBaseAutenticada
    {
        #region Propriedades

        private List<PessoaExpostaPoliticamenteInfo> SessionUltimoResultadoDeBusca
        {
            get
            {
                return (List<PessoaExpostaPoliticamenteInfo>)Session["UltimoResultadoDeBuscaDePEP"];
            }

            set
            {
                Session["UltimoResultadoDeBuscaDePEP"] = value;
            }
        }
        
        private string UltimoCampoDeOrdenacao
        {
            get
            {
                return (string)Session["UltimoCampoDeOrdenacaoPEP"];
            }

            set
            {
                Session["UltimoCampoDeOrdenacaoPEP"] = value;
            }
        }
        
        private string UltimaDirecaoDeOrdenacao
        {
            get
            {
                return (string)Session["UltimaDirecaoDeOrdenacaoPEP"];
            }

            set
            {
                Session["UltimaDirecaoDeOrdenacaoPEP"] = value;
            }
        }

        private Dictionary<string, TransporteImportacaoPEP> ApplicationImportacoesPEPSendoEfetuadas 
        {
            get
            {
                if (Application["ImportacoesPEPSendoEfetuadas"] == null)
                {
                    Application["ImportacoesPEPSendoEfetuadas"] = new Dictionary<string, TransporteImportacaoPEP>();
                }

                return (Dictionary<string, TransporteImportacaoPEP>)Application["ImportacoesPEPSendoEfetuadas"];
            }
        }

        public string DsDocumento { get; set; }

        public string DsNome { get; set; }

        #endregion
        
        #region Métodos Private

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina, string pCampoDeOrdenacao, string pDirecaoDeOrdenacao)
        {
            TransporteDeListaPaginada lRetorno = new TransporteDeListaPaginada();

            List<TransportePessoaExpostaPoliticamente> lLista = new List<TransportePessoaExpostaPoliticamente>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            if (!string.IsNullOrEmpty(pCampoDeOrdenacao) 
                && (this.UltimoCampoDeOrdenacao != pCampoDeOrdenacao || this.UltimaDirecaoDeOrdenacao != pDirecaoDeOrdenacao))
            {
                this.UltimoCampoDeOrdenacao = pCampoDeOrdenacao;
                this.UltimaDirecaoDeOrdenacao = pDirecaoDeOrdenacao;

                switch (pCampoDeOrdenacao.ToLower())
                {
                    case "id" :

                        if(pDirecaoDeOrdenacao == "asc")
                            this.SessionUltimoResultadoDeBusca.Sort((a,b) => (a.IdPEP.Value - b.IdPEP.Value));
                        else
                            this.SessionUltimoResultadoDeBusca.Sort((a,b) => (b.IdPEP.Value - a.IdPEP.Value));

                        break;
                            
                    case "documento" :
                        
                        if(pDirecaoDeOrdenacao == "asc")
                            this.SessionUltimoResultadoDeBusca.Sort((a,b) => string.Compare(a.DsDocumento, b.DsDocumento));
                        else
                            this.SessionUltimoResultadoDeBusca.Sort((a,b) => string.Compare(b.DsDocumento, a.DsDocumento));

                        break;
                            
                    case "nome" :
                        
                        if(pDirecaoDeOrdenacao == "asc")
                            this.SessionUltimoResultadoDeBusca.Sort((a,b) => string.Compare(a.DsNome, b.DsNome));
                        else
                            this.SessionUltimoResultadoDeBusca.Sort((a,b) => string.Compare(b.DsNome, a.DsNome));

                        break;
                }
            }

            for (int a = lIndiceInicial; a < lIndiceFinal; a++)
            {
                if (a < this.SessionUltimoResultadoDeBusca.Count)
                {
                    lLista.Add( new TransportePessoaExpostaPoliticamente(this.SessionUltimoResultadoDeBusca[a]));
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        private void BuscarItens()
        {
            //string lRetorno = "Erro...";

            ConsultarEntidadeCadastroRequest<PessoaExpostaPoliticamenteInfo>  lRequest  =  new ConsultarEntidadeCadastroRequest<PessoaExpostaPoliticamenteInfo>();
            ConsultarEntidadeCadastroResponse<PessoaExpostaPoliticamenteInfo> lResponse = new ConsultarEntidadeCadastroResponse<PessoaExpostaPoliticamenteInfo>();

            PessoaExpostaPoliticamenteInfo lDadosDeBusca = new PessoaExpostaPoliticamenteInfo();

            lRequest.EntidadeCadastro = lDadosDeBusca;

            lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

            lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

            lRequest.EntidadeCadastro = new PessoaExpostaPoliticamenteInfo();

            if (DsDocumento != null && DsDocumento != string.Empty)
                lRequest.EntidadeCadastro.DsDocumento = this.DsDocumento;

            if (DsNome != null && DsNome != string.Empty)
                lRequest.EntidadeCadastro.DsNome = this.DsNome;

            this.SessionUltimoResultadoDeBusca = null;

            try
            {
                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<PessoaExpostaPoliticamenteInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    this.SessionUltimoResultadoDeBusca = lResponse.Resultado;
                }
            }
            catch
            {
                //RetornarErroAjax("Erro durante a busca", exBusca);
            }

            //return lRetorno;
        }

        private string ResponderPaginar()
        {
            string lRetorno = string.Empty;
            
            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

            // Acao=Paginar&_search=false&nd=1277378181807&rows=50&page=1&sidx=invid&sord=desc

            if (this.SessionUltimoResultadoDeBusca == null)
            {
                BuscarItens();
            }

            int lPagina;

            if (int.TryParse(Request["page"], out lPagina) && this.SessionUltimoResultadoDeBusca != null)
            {
                lLista = BuscarPaginaDeResultados(lPagina, Request["sidx"], Request["sord"]);
            }

            lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }
        
        private string ResponderBuscarPEP()
        {
            string lRetorno = string.Empty;

            Int64 cpf = 0;

            string lCPF1 = RemoverPontoEVazio(Request.Form["CPF"]);

            if (!Int64.TryParse(lCPF1, out cpf))
            {
                DsNome = Request.Form["CPF"].ToUpper();
                DsDocumento = null;
            }
            else
            {
                DsDocumento = cpf.ToString();
                DsNome = null;
            }

            BuscarItens();
    
            lRetorno = RetornarSucessoAjax(0, "Nenhuma PEP encontrada com o documento [{0}]", Request.Form["CPF"]);

            int numCount = this.SessionUltimoResultadoDeBusca.Count;
            
            TransporteDeListaPaginada lListaPaginada = new TransporteDeListaPaginada();
            lListaPaginada = BuscarPaginaDeResultados(1, "nome", "asc");

            if (numCount > 0)
            {
                //PessoaExpostaPoliticamenteInfo lPessoa = pep.ElementAt<PessoaExpostaPoliticamenteInfo>(0);
                lRetorno = RetornarSucessoAjax(lListaPaginada, "{0} PEP(s) encontrado(s)", numCount);
                base.RegistrarLogConsulta(string.Concat("Consulta realizada pela chave: ", Request.Form["CPF"]));
            }

            return lRetorno;
        }
        
        private string ResponderEnviarEmail()
        {
            string lRetorno = string.Empty;

            Dictionary<string, string> lDados= new Dictionary<string,string>();
            
            lDados.Add("@Documento",   this.Request.Form["Documento"]);
            lDados.Add("@NomeCliente", this.Request.Form["Nome"]);
            lDados.Add("@Id",          this.Request.Form["Id"]);

            MensagemResponseStatusEnum lResposta = base.EnviarEmail( ConfiguracoesValidadas.EmailCompliance
                                                                     , "Alerta de cliente RELACIONADO - Cadastro de cliente"
                                                                     , "EmailAlertaPEPCompliance.htm"
                                                                     , lDados
                                                                     , Contratos.Dados.Enumeradores.eTipoEmailDisparo.Compliance);
            if (lResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = RetornarSucessoAjax("Email enviado com sucesso.");
            }
            else
            {
                lRetorno = RetornarErroAjax("Erro ao enviar email: {0}", lResposta);
            }

            return lRetorno;
        }

        private string ResponderReceberArquivo()
        {
            string lIdDaImportacao;
            string lResposta = string.Empty;

            TransporteImportacaoPEP lObjetoDeResposta;
            
            ParameterizedThreadStart lThreadStart;
            Thread lNovoThread;

            lIdDaImportacao = string.Format("{0}_{1}", this.UsuarioLogado.Id, Guid.NewGuid().ToString());

            foreach (string lKey in this.ApplicationImportacoesPEPSendoEfetuadas.Keys)
            {
                if(lKey.StartsWith(string.Format("{0}_", this.UsuarioLogado.Id)))
                {
                    //já existia uma requisição de sincronização para esse usuário 
                    //(alguém fez bypass do javascript pra re-enviar)

                    lObjetoDeResposta = this.ApplicationImportacoesPEPSendoEfetuadas[lKey];

                    lResposta = RetornarSucessoAjax(lObjetoDeResposta, "Já havia uma requisição sendo executada.");

                    return lResposta;
                }
            }

            HttpPostedFile lFile;
            byte[] lFileBytes;
            string lConteudo;

            lFile = Request.Files[0];
            lFileBytes = new byte[lFile.InputStream.Length];

            lFile.InputStream.Read(lFileBytes, 0, lFileBytes.Length);

            lConteudo = System.Text.Encoding.UTF8.GetString(lFileBytes);

            lObjetoDeResposta = new TransporteImportacaoPEP(lIdDaImportacao);

            if (!this.ApplicationImportacoesPEPSendoEfetuadas.ContainsKey(lIdDaImportacao))
            {
                this.ApplicationImportacoesPEPSendoEfetuadas.Add(lIdDaImportacao, lObjetoDeResposta);
            }

            lThreadStart = new ParameterizedThreadStart(ResponderImportarPEP_Async);

            lNovoThread = new Thread(lThreadStart);

            lNovoThread.Start( new string[] { lIdDaImportacao, lConteudo } );

            lResposta = RetornarSucessoAjax(lObjetoDeResposta, "Arquivo recebido com sucesso, processo de importação iniciado.");

            return lResposta;
        }
        
        private string ResponderVerificarImportacao()
        {
            string lIdDaImportacao = Request.Form["IdDaImportacao"];

            string lRetorno;

            if (this.ApplicationImportacoesPEPSendoEfetuadas.ContainsKey(lIdDaImportacao))
            {
                TransporteImportacaoPEP lTransporte = this.ApplicationImportacoesPEPSendoEfetuadas[lIdDaImportacao];

                lRetorno = RetornarSucessoAjax(lTransporte, lTransporte.StatusDaImportacao);

                if (lTransporte.StatusDaImportacao == "Finalizada")
                    this.ApplicationImportacoesPEPSendoEfetuadas.Remove(lTransporte.IdDaImportacao);
            }
            else
            {
                lRetorno = RetornarSucessoAjax("INEXISTENTE");
            }

            return lRetorno;
        }

        private string RemoverPontoEVazio(string item)
        {
            return item.Replace(" ", "").Replace(".", "");
        }

        private void ResponderImportarPEP_Async(object pIdDaImportacaoEConteudo)
        {
            string lIdDaImportacao    = ((string[])pIdDaImportacaoEConteudo)[0];
            string lConteudoDoArquivo = ((string[])pIdDaImportacaoEConteudo)[1];

            TransporteImportacaoPEP lTransporte = this.ApplicationImportacoesPEPSendoEfetuadas[lIdDaImportacao];

            List<PessoaExpostaPoliticamenteInfo> lListaParaImportar = new List<PessoaExpostaPoliticamenteInfo>();

            if (lTransporte != null)
            {
                SalvarEntidadeRequest<PessoaExpostaPoliticamenteImportacaoInfo> lRequest = new SalvarEntidadeRequest<PessoaExpostaPoliticamenteImportacaoInfo>();
                SalvarObjetoResponse<PessoaExpostaPoliticamenteImportacaoInfo> lResponse;

                string[] lLinhas = lConteudoDoArquivo.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                Dictionary<string, string> lJaImportados = new Dictionary<string, string>();

                PessoaExpostaPoliticamenteInfo lNovaPessoa;

                foreach (PessoaExpostaPoliticamenteInfo lPessoaImportada in this.SessionUltimoResultadoDeBusca)
                {
                    if(!lJaImportados.ContainsKey(lPessoaImportada.DsDocumento))
                        lJaImportados.Add(lPessoaImportada.DsDocumento, lPessoaImportada.DsNome);
                }

                foreach (string lLinha in lLinhas)
                {
                    if (!string.IsNullOrEmpty(lLinha) && lLinha.StartsWith("110"))
                    {
                        lNovaPessoa = new PessoaExpostaPoliticamenteInfo(lLinha);

                        if(!lJaImportados.ContainsKey(lNovaPessoa.DsDocumento))     //TODO: método melhor pra não-repetidos? direto na tabela?
                            lListaParaImportar.Add(lNovaPessoa);
                    }
                }

                lRequest.Objeto = new PessoaExpostaPoliticamenteImportacaoInfo() { ListaParaImportar = lListaParaImportar };

                lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                lResponse = ((ServicoPersistenciaCadastro)this.ServicoPersistenciaCadastro).ImportarPessoasExpostasPoliticamente(lRequest);

                lTransporte.MensagemDeFinalizacao = "Registros importados com sucesso";

                lTransporte.RegistrosImportadosComSucesso = lResponse.Objeto.RegistrosImportadosComSucesso;
                lTransporte.RegistrosComErro = lResponse.Objeto.RegistrosImportadosComErro;

                lTransporte.StatusDaImportacao = "Finalizada";

                this.SessionUltimoResultadoDeBusca = null;
            }
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            
            RegistrarRespostasAjax(new string[] { "Paginar"
                                                , "BuscarPEP"
                                                , "EnviarEmail"
                                                , "ReceberArquivo"
                                                , "VerificarImportacao"
                                                },
                new ResponderAcaoAjaxDelegate[] { this.ResponderPaginar
                                                , this.ResponderBuscarPEP
                                                , this.ResponderEnviarEmail
                                                , this.ResponderReceberArquivo
                                                , this.ResponderVerificarImportacao
                                                });
        }

        #endregion
    }
}

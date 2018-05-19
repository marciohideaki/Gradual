using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.FIDC.Adm.Web.CadastroFundos
{
    public partial class FluxoAprovacaoFundos : PaginaBase
    {
        #region Propriedades
        public int GetIdFundoCadastro
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdFundoCadastro"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int GetIdFundoFluxoGrupoEtapa
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdFundoFluxoGrupoEtapa"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int GetIdFundoFluxoStatus
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdFundoFluxoStatus"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public DateTime? GetDtInicio
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(Request["DtInicio"]);
                }
                catch
                {
                    return null;
                }
            }
        }
        public DateTime? GetDtConclusao
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(Request["DtConclusao"]);
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion

        #region Eventos
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    base.Page_Load(sender, e);

                    RegistrarRespostasAjax(new[] { "CarregarEtapasFluxoAprovacao",
                        "CarregarListasStatus",
                        "GravarEtapaFluxoAprovacaoFundo",
                        "CarregarFluxoAprovacaoPorFundo",
                        "CarregarSelectFundos"
                                                     },
                         new ResponderAcaoAjaxDelegate[] { 
                                                        ResponderCarregarEtapasFluxoAprovacao,
                                                        ResponderCarregarListaStatus,
                                                        ResponderGravarEtapaFluxoAprovacaoFundo,
                                                        ResponderCarregarFluxoAprovacaoPorFundo,
                                                        ResponderCarregarSelectFundos
                                                     });

                    CarregarDadosIniciais();
                }
                catch (Exception ex)
                {
                    Logger.Error("Erro ao carregar os dados do fluxo de fundos na tela", ex);
                }
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carregar dados iniciais da página de carteiras
        /// </summary>
        private void CarregarDadosIniciais()
        {
            if (Page.IsPostBack) return;
            TituloDaPagina = "Cadastro de Fundos";
            LinkPreSelecionado = "lnkTL_CadastroFundos";
        }

        /// <summary>
        /// Seleciona etapas de um grupo
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarEtapasFluxoAprovacao()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new FundoFluxoGrupoEtapaRequest();

                var lResponse = BuscarEtapasFluxoAprovacaoPorGrupo(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaEtapas);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar as etapas na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarEtapasFluxoAprovacao ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega lista de status parametrizados na base
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarListaStatus()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new FundoFluxoStatusRequest();

                var lResponse = BuscarStatusFluxoAprovacao(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaStatus);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar as etapas na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarListasStatus ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Grava uma etapa de aprovação de um fundo
        /// </summary>
        /// <returns></returns>
        public string ResponderGravarEtapaFluxoAprovacaoFundo()
        {
            string lRetorno;

            try
            {
                var lRequest = new FundoFluxoAprovacaoRequest
                {
                    IdFundoFluxoGrupoEtapa = GetIdFundoFluxoGrupoEtapa,
                    IdFundoCadastro = GetIdFundoCadastro,
                    IdFundoFluxoStatus = GetIdFundoFluxoStatus,
                    DtInicio = GetDtInicio,
                    DtConclusao = GetDtConclusao,
                    UsuarioResponsavel = UsuarioLogado.Nome
                };
                
                var lResponse = InserirEtapaFluxoAprovacaoFundo(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    AnexarArquivos(lRequest.IdFundoFluxoAprovacao);

                    lRetorno = RetornarSucessoAjax(lResponse.StatusResposta.ToString());
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro ao gravar etapa de fluxo de aprovação");
                }
            }
            catch (HttpException ex)
            {
                lRetorno = RetornarErroAjax("Erro ao anexar arquivo: " + ex.Message);
            }
            catch (DirectoryNotFoundException)
            {
                lRetorno = RetornarErroAjax("Caminho de destino do arquivo não encontrado");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao gravar a etapa", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderGravarEtapaFluxoAprovacaoFundo: ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Busca etapas do fluxo de aprovação de um fundo
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarFluxoAprovacaoPorFundo()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new FundoFluxoAprovacaoRequest {IdFundoCadastro = GetIdFundoCadastro};

                var lResponse = BuscarEtapasAprovacaoFundo(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaFluxoAprovacao);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar as etapas na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarFluxoAprovacaoPorFundo ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega a lista de fundos na tela do fluxo de aprovação
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarSelectFundos()
        {
            var lRetorno = string.Empty;

            try
            {
                //var lRequest = new CadastroFundoRequest {IdFundoCadastro = 2}; //apenas fundos em constituição
                
                var lResponse = BuscarFundosCadastradosPorCategoria(new CadastroFundoRequest());

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaFundos);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os lista de fundos na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarSelectFundos ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Anexa arquivo selecionado pelo cliente
        /// </summary>
        /// <returns></returns>
        private void AnexarArquivos(int idFundoFluxoAprovacao)
        {
            if (!HttpContext.Current.Request.Files.AllKeys.Any()) return; //só prossegue se houver arquivo

            var httpPostedFile = HttpContext.Current.Request.Files["ArquivoAnexo"];

            if (httpPostedFile == null) return; //só prossegue se houver arquivo anexo

            //busca o caminho de destino dos arquivos no config
            var caminhoDestino = ConfigurationManager.AppSettings["RaizUploadFluxoAprovacaoFundo"];

            //cria o nome do arquivo com data e hora atual para evitar de sobrescrever arquivos com mesmo nome
            var nomeFinalArquivo = Path.GetFileNameWithoutExtension(httpPostedFile.FileName) + "_" +
                                   idFundoFluxoAprovacao + "_" +
                                   DateTime.Now.ToString("ddMMyyyhhmmss") +
                                   Path.GetExtension(httpPostedFile.FileName);

            //obtém caminho destino
            var fileSavePath = Path.Combine(caminhoDestino, nomeFinalArquivo);

            //salva arquivo no diretório
            httpPostedFile.SaveAs(fileSavePath);

            GravarAnexoEtapaFluxoAprovacaoFundo(idFundoFluxoAprovacao, fileSavePath);
        }

        /// <summary>
        /// Grava um anexo de uma etapa de aprovação de um fundo
        /// </summary>
        /// <returns></returns>
        private void GravarAnexoEtapaFluxoAprovacaoFundo(int idFundoFluxoAprovacao, string caminhoAnexo)
        {
            try
            {
                var lRequest = new FundoFluxoAprovacaoAnexoRequest
                {
                    IdFundoFluxoAprovacao = idFundoFluxoAprovacao,
                    CaminhoAnexo = caminhoAnexo
                };

                InserirAnexoEtapaFluxoAprovacao(lRequest);
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao gravar registro do anexo na base de dados", ex);

                throw;
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.Web.App_Codigo.Transporte;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.FIDC.Adm.Web.CadastroFundos
{
    public partial class ManutencaoFundos : PaginaBase
    {
        #region Propriedades
        public int GetIdFundoSubCategoria
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdFundoSubCategoria"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int GetIdFundoCategoria
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdFundoCategoria"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public string GetParamAtualizarRelacionamentoFundos
        {
            get
            {
                try
                {
                    return Request["ParamAtualizarRelacionamentoFundos"];
                }
                catch
                {
                    return "";
                }
            }
        }
        #endregion

        #region Eventos
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            try
            {

                base.Page_Load(sender, e);

                RegistrarRespostasAjax(new[] { 
                        "CarregarGridFundosAdministrados",
                        "CarregarGridFundosPreOperacionais",
                        "CarregarGridFundosPrateleira",
                        "ResponderCarregarFundosAdministradosPorCategoriaSubCategoria",
                        "ResponderCarregarFundosPreOperacionaisPorCategoriaSubCategoria",
                        "ResponderCarregarFundosPrateleiraPorCategoriaSubCategoria",
                        "CarregarFundosConstituicaoPorCategoriaSubCategoria",
                        "CarregarGridModalFundos",
                        "AtualizarRelacionamentoFundos",
                        "CarregarDadosCategoria"
                    },
                    new ResponderAcaoAjaxDelegate[] { 
                        ResponderCarregarFundosAdministradosSubCategorias,
                        ResponderCarregarFundosPreOperacionaisSubCategorias,
                        ResponderCarregarFundosPrateleiraSubCategorias,
                        ResponderCarregarFundosAdministradosPorCategoriaSubCategoria,
                        ResponderCarregarFundosPreOperacionaisPorCategoriaSubCategoria,
                        ResponderCarregarFundosPrateleiraPorCategoriaSubCategoria,
                        ResponderCarregarFundosConstituicaoPorCategoriaSubCategoria,
                        ResponderCarregarGridModalFundos,
                        ResponderAtualizarRelacionamentoFundos,
                        ResponderCarregarDadosCategoria
                    });

                CarregarDadosIniciais();
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de fundos na tela", ex);
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Carregar dados iniciais da página de carteiras
        /// </summary>
        private void CarregarDadosIniciais()
        {
            try
            {
                if (Page.IsPostBack) return;
                TituloDaPagina = "Manutenção de Fundos";
                LinkPreSelecionado = "lnkTL_CadastroFundos";
            }
            catch (Exception ex)
            {
                RetornarErroAjax(ex.Message, ex);
            }
        }        
        /// <summary>
        /// Busca sub categorias para carregamento de grid
        /// </summary>
        /// <returns></returns>
        public string BuscarSubCategorias()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new FundoSubCategoriaRequest();

                var lResponse = BuscarFundoSubCategorias(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteFundoSubCategoria().TraduzirLista(lResponse.ListaSubCategorias);

                    var lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaSubCategorias.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };




                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de sub-categorias", ex);

                lRetorno = RetornarErroAjax("Erro no método BuscarSubCategorias ", ex);
            }

            return lRetorno;
        }
        /// <summary>
        /// Busca fundos para carregamento de grid
        /// </summary>
        /// <returns></returns>
        public string BuscarFundosPorCategoriaSubCategoria(int idFundoCategoria, int idFundoSubCategoria)
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new FundoCategoriaSubCategoriaRequest
                {
                    IdFundoCategoria = idFundoCategoria,
                    IdFundoSubCategoria = idFundoSubCategoria
                };


                var lResponse = base.BuscarFundosPorCategoriaSubCategoria(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteCadastroFundos().TraduzirLista(lResponse.ListaFundos);

                    var lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaFundos.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };
                    
                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de fundos na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método BuscarFundosPorCategoriaSubCategoria ", ex);
            }
            
            return lRetorno;
        }
        /// <summary>
        /// Carrega os fundos da categoria Fundos Administrados a partir da sub categoria selecionada
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarFundosAdministradosPorCategoriaSubCategoria()
        {
            return BuscarFundosPorCategoriaSubCategoria(GetIdFundoCategoria, GetIdFundoSubCategoria); 
        }
        /// <summary>
        /// Carrega os fundos da categoria Fundos Pré operacionais a partir da sub categoria selecionada
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarFundosPreOperacionaisPorCategoriaSubCategoria()
        {
            return BuscarFundosPorCategoriaSubCategoria(GetIdFundoCategoria, GetIdFundoSubCategoria);
        }

        /// <summary>
        /// Carrega os fundos da categoria Fundos de Prateleira a partir da sub categoria selecionada
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarFundosPrateleiraPorCategoriaSubCategoria()
        {
            return BuscarFundosPorCategoriaSubCategoria(GetIdFundoCategoria, GetIdFundoSubCategoria);
        }

        /// <summary>
        /// Carrega os fundos da categoria Fundos de Prateleira a partir da sub categoria selecionada
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarFundosConstituicaoPorCategoriaSubCategoria()
        {
            return BuscarFundosPorCategoriaSubCategoria(GetIdFundoCategoria, GetIdFundoSubCategoria);
        }
        /// <summary>
        /// Carrega sub categorias no grid de fundos administrados
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarFundosAdministradosSubCategorias()
        {
            return BuscarSubCategorias();
        }
        /// <summary>
        /// Carrega sub categorias no grid de fundos pre operacionais
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarFundosPreOperacionaisSubCategorias()
        {
            return BuscarSubCategorias();
        }
        /// <summary>
        /// Carrega sub categorias no grid de fundos de prateleira
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarFundosPrateleiraSubCategorias()
        {
            return BuscarSubCategorias();
        }
        /// <summary>
        /// Método de carregamento do grid de fundos do modal
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarGridModalFundos()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new FundoCategoriaSubCategoriaRequest
                {
                    IdFundoCategoria = GetIdFundoCategoria,
                    IdFundoSubCategoria = GetIdFundoSubCategoria
                };

                //chama método da classe base
                var lResponse = BuscarFundosCarregarGridModalFundos(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteCadastroFundos().TraduzirLista(lResponse.ListaFundos);

                    var lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaFundos.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };
                    
                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de fundos na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarGridModalFundos ", ex);
            }

            return lRetorno;
        }
        /// <summary>
        /// Atualizar os relacionamentos Fundos x Categorias x SubCategorias
        /// </summary>
        /// <returns></returns>
        public string ResponderAtualizarRelacionamentoFundos()
        {
            try
            {
                var parametros = GetParamAtualizarRelacionamentoFundos;

                var lReqJson = JsonConvert.DeserializeObject<List<FundoCategoriaSubCategoriaJsonRequest>>(parametros);

                var listaRelacionamentoRemover = new List<FundoCategoriaSubCategoriaRequest>();
                var listaFundosJaRelacionados = new List<FundoCategoriaSubCategoriaRequest>();

                //preenche lista de relacionamentos a adicionar
                var listaRelacionamentoAdicionar = lReqJson.Select(item => new FundoCategoriaSubCategoriaRequest
                {
                    IdFundoCadastro = item.IdFundoCadastro, IdFundoCategoria = GetIdFundoCategoria, IdFundoSubCategoria = GetIdFundoSubCategoria
                }).ToList();

                #region Seleciona fundos já relacionados

                var lRequest = new FundoCategoriaSubCategoriaRequest
                {
                    IdFundoCategoria = GetIdFundoCategoria,
                    IdFundoSubCategoria = GetIdFundoSubCategoria
                };


                var lResponse = base.BuscarFundosPorCategoriaSubCategoria(lRequest);

                var lTempJaRelacionados = new TransporteCadastroFundos().TraduzirLista(lResponse.ListaFundos);

                //preenche lista de relacionamentos a adicionar
                foreach (var item in lTempJaRelacionados)
                {
                    var req = new FundoCategoriaSubCategoriaRequest
                    {
                        IdFundoCadastro = item.IdFundoCadastro,
                        IdFundoCategoria = GetIdFundoCategoria,
                        IdFundoSubCategoria = GetIdFundoSubCategoria
                    };

                    listaFundosJaRelacionados.Add(req);
                }
                #endregion

                //preenche lista de relacionamentos a remover
                foreach (var item in listaFundosJaRelacionados)
                {
                    //caso o elemento não conste na lista de adicionar, o mesmo deve ser removido
                    if (!listaRelacionamentoAdicionar.Any(p => p.IdFundoCadastro == item.IdFundoCadastro))
                    {
                        listaRelacionamentoRemover.Add(new FundoCategoriaSubCategoriaRequest
                        {
                            IdFundoCategoria = item.IdFundoCategoria,
                            IdFundoSubCategoria = item.IdFundoSubCategoria,
                            IdFundoCadastro = item.IdFundoCadastro
                        });
                    }
                }

                //adiciona todos os relacionamentos que constam na lista
                foreach (var item in listaRelacionamentoAdicionar)
                {
                    AdicionarRelacionamentosFundosCategoriasSubCategorias(item);

                    //antes de gravar o log, verifica se o registro inserido é novo
                    //caso contrário, não deve ser logado
                    if (item.IdFundoCategoriaSubCategoria > 0)
                    {
                        //grava log na base e no log4net
                        InserirLog(new FundoCategoriaSubCategoriaLogRequest
                        {
                            IdFundoCadastro = item.IdFundoCadastro,
                            IdFundoCategoria = item.IdFundoCategoria,
                            IdFundoSubCategoria = item.IdFundoSubCategoria,
                            UsuarioLogado = UsuarioLogado.Nome,
                            DtAlteracao = DateTime.Now,
                            TipoTransacao = "INSERT"
                        });
                    }
                }
                //remove todos os relacionamentos que constam na lista de remoção
                foreach (var item in listaRelacionamentoRemover)
                {
                    RemoverRelacionamentosFundosCategoriasSubCategorias(item);

                    //grava log na base e no log4net
                    InserirLog(new FundoCategoriaSubCategoriaLogRequest
                    {
                        IdFundoCadastro = item.IdFundoCadastro,
                        IdFundoCategoria = item.IdFundoCategoria,
                        IdFundoSubCategoria = item.IdFundoSubCategoria,
                        UsuarioLogado = UsuarioLogado.Nome,
                        DtAlteracao = DateTime.Now,
                        TipoTransacao = "DELETE"
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de fundos na tela", ex);

                return RetornarErroAjax("Erro no método ResponderAtualizarRelacionamentoFundos ", ex);
            }

            var response = new FundoCategoriaSubCategoriaResponse
            {
                StatusResposta = MensagemResponseStatusEnum.OK
            };

            return RetornarSucessoAjax(response.StatusResposta.ToString());
        }
        /// <summary>
        /// Carregar os dados das categorias parametrizadas no banco de dados
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarDadosCategoria()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new FundoCategoriaRequest();

                var lResponse = BuscarFundoCategorias(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteFundoCategoria().TraduzirLista(lResponse.ListaCategorias);

                    lRetorno = JsonConvert.SerializeObject(lListaTransporte);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de categorias", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarDadosCategoria ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Inserção do log de transações
        /// </summary>
        /// <param name="request"></param>
        private void InserirLog(FundoCategoriaSubCategoriaLogRequest request)
        {
            var response = InserirLogFundoCategoriaSubCategoria(request);

            #region Gravação Log4Net
            var mensagemLog = string.Empty;

            mensagemLog += "Associação Fundo x Categoria x SubCategoria: ";
            mensagemLog += "Fundo:" + response.NomeFundo + ";";
            mensagemLog += "Categoria:" + response.DsFundoCategoria + ";";
            mensagemLog += "SubCategoria:" + response.DsFundoSubCategoria + ";";
            mensagemLog += "DtAlteracao:" + request.DtAlteracao + ";";
            mensagemLog += "TipoTransacao:" + request.TipoTransacao + ";";
            mensagemLog += "UsuarioTransacao:" + request.UsuarioLogado + ";";

            Logger.Info(mensagemLog);
            #endregion
        }
        #endregion

        /// <summary>
        /// Classe de request utilizada para desserializar um JSON contido na propriedade GetParamAtualizarRelacionamentoFundos
        /// </summary>
        private class FundoCategoriaSubCategoriaJsonRequest
        {
            public FundoCategoriaSubCategoriaJsonRequest(int idFundoCadastro)
            {
                IdFundoCadastro = idFundoCadastro;
            }

            public int IdFundoCadastro { get; private set; }
        }
    }
}
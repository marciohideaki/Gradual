using System;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Persistencias.Pagina;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Mensagens;
using log4net;
using Gradual.Site.DbLib.Persistencias.MinhaConta.Suitability;
using Gradual.Site.DbLib.Persistencias.MinhaConta;
using Gradual.Site.DbLib.Dados.MinhaConta;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using System.Text;
using Gradual.Site.DbLib.Widgets;
using System.Collections.Generic;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.DbLib.Dados;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Gradual.Site.DbLib.Persistencias.MinhaConta.Cadastro;

namespace Gradual.Site.DbLib.Persistencias
{
    public class ServicoPersistenciaSite : IServicoPersistenciaSite
    {
        #region Globais

        private readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Propriedades

        public static string ConexaoCadastro { get { return "Cadastro"; } }

        public static string ConexaoEducacional { get { return "EducacionalSQL"; } }

        public static string ConexaoPortal { get { return "ConexaoPortal"; } }

        public static string ConexaoSinacor { get { return "SINACOR"; } }

        public static string ConexaoSinacorConsulta { get { return "SinacorConsulta"; } }

        public static string ConexaoTrade { get { return "Trade"; } }

        public static string ConexaoSisfinance { get { return "Sisfinance"; } }

        public static string ConexaoFinancial { get { return "Financial"; } }

        public static string ConexaoOMS { get { return "OMS"; } }

        public static string ConexaoContaMargem { get { return "ContaMargem"; } }

        public static string ConexaoRisco { get { return "Risco"; } }

        public static string ConexaoDrive { get { return "drive"; } }

        public static string ConexaoRendaFixa { get { return "RendaFixa"; } }

        public static string ConexaoMyCapital { get { return "MyCapital"; } }

        public static string ConexaoCadastroOracle { get { return "CadastroOracle"; } }

        public static string ConexaoSeguranca { get { return "Seguranca"; } }

        #endregion

        #region Metodos Private

        private void VerificarIntegracao(IntegracaoIRInfo IntegracaoIR)
        {
            if (IntegracaoIR.dataInicio == null)
                throw new Exception("A data da transação não pode ser nula.");

            if (IntegracaoIR.IdBovespa < 1)
                throw new Exception("Código Bovespa precisa ser maior que 0.");

            if (IntegracaoIR.Email == string.Empty)
                throw new Exception("Email não pode ser vazio.");

            if (IntegracaoIR.Cidade == string.Empty)
                throw new Exception("Cidade não pode ser vazia.");

            if (IntegracaoIR.Estado == string.Empty)
                throw new Exception("Estado não pode ser vazia.");
        }

        protected WidgetBase InstanciarWidget(WidgetInfo pWidgetInfo)
        {
            Regex lRegex = new Regex("\"Tipo\".*?.*\"");

            Match lMatch = lRegex.Match(pWidgetInfo.WidgetJson);

            if (!string.IsNullOrEmpty(lMatch.Value))
            {
                string lTipo = lMatch.Value;

                lTipo = lTipo.Substring(lTipo.IndexOf(":"));
                lTipo = lTipo.Replace("\"", "").Replace(",", "").Replace(":", "");

                WidgetBase.TipoWidget tipo = (WidgetBase.TipoWidget)System.Enum.Parse(typeof(WidgetBase.TipoWidget), lTipo);

                switch (tipo)
                {
                    case WidgetBase.TipoWidget.Imagem:

                        widImagem lWidgetImagem = JsonConvert.DeserializeObject<widImagem>(pWidgetInfo.WidgetJson);

                        lWidgetImagem.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetImagem.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetImagem;

                    case WidgetBase.TipoWidget.Titulo:

                        widTitulo lWidgetTitulo = JsonConvert.DeserializeObject<widTitulo>(pWidgetInfo.WidgetJson);

                        lWidgetTitulo.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetTitulo.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetTitulo;

                    case WidgetBase.TipoWidget.Lista:

                        widLista lWidgetLista = JsonConvert.DeserializeObject<widLista>(pWidgetInfo.WidgetJson);

                        lWidgetLista.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetLista.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetLista.IdDaLista = pWidgetInfo.CodigoListaConteudo;

                        return lWidgetLista;

                    case WidgetBase.TipoWidget.Texto:

                        widTexto lWidgetTexto = JsonConvert.DeserializeObject<widTexto>(pWidgetInfo.WidgetJson);

                        lWidgetTexto.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetTexto.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetTexto;

                    case WidgetBase.TipoWidget.Tabela:

                        widTabela lWidgetTabela = JsonConvert.DeserializeObject<widTabela>(pWidgetInfo.WidgetJson);

                        lWidgetTabela.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetTabela.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetTabela.IdDaLista = pWidgetInfo.CodigoListaConteudo;

                        return lWidgetTabela;

                    case WidgetBase.TipoWidget.Repetidor:

                        widRepetidor lWidgetRepetidor = JsonConvert.DeserializeObject<widRepetidor>(pWidgetInfo.WidgetJson);

                        lWidgetRepetidor.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetRepetidor.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetRepetidor.IdDaLista = pWidgetInfo.CodigoListaConteudo;

                        return lWidgetRepetidor;

                    case WidgetBase.TipoWidget.ListaDeDefinicao:

                        widListaDeDefinicao lWidgetListaDeDefinicao = JsonConvert.DeserializeObject<widListaDeDefinicao>(pWidgetInfo.WidgetJson);

                        lWidgetListaDeDefinicao.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetListaDeDefinicao.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetListaDeDefinicao.IdDaLista = pWidgetInfo.CodigoListaConteudo;

                        return lWidgetListaDeDefinicao;

                    case WidgetBase.TipoWidget.Abas:

                        widAbas lWidgetAbas = JsonConvert.DeserializeObject<widAbas>(pWidgetInfo.WidgetJson);

                        lWidgetAbas.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetAbas.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetAbas;

                    case WidgetBase.TipoWidget.Acordeon:

                        widAcordeon lWidgetAcordeon = JsonConvert.DeserializeObject<widAcordeon>(pWidgetInfo.WidgetJson);

                        lWidgetAcordeon.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetAcordeon.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetAcordeon;

                    case WidgetBase.TipoWidget.TextoHTML:

                        widTextoHTML lWidgetTextoHTML = JsonConvert.DeserializeObject<widTextoHTML>(pWidgetInfo.WidgetJson);

                        lWidgetTextoHTML.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetTextoHTML.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetTextoHTML;

                    default:
                        break;
                }
            }
            return null;
        }

        #endregion

        #region Metodos Públicos

        public string ToCodigoClienteFormatado(object pObject)
        {
            int lDigito = 0;

            int lCodigoCorretora = 227;

            lDigito = (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            lDigito = lDigito % 11;

            if (lDigito == 0 || lDigito == 1)
            {
                lDigito = 0;
            }

            else
            {
                lDigito = 11 - lDigito;
            }

            return string.Format("{0}{1}", pObject.ToString(), lDigito);
        }

        public void LimparCache(Nullable<int> pIdPagina)
        {
            if (pIdPagina.HasValue && CMSCache.Estruturas.ContainsKey(pIdPagina.Value))
            {
                string lId;

                foreach (EstruturaInfo lEstrutura in CMSCache.Estruturas[pIdPagina.Value])
                {
                    lId = CMSCache.GerarIdCache(lEstrutura.CodigoPagina, lEstrutura.CodigoEstrutura);

                    CMSCache.Paginas.Remove(lId);
                }

                CMSCache.Estruturas.Remove(pIdPagina.Value);
            }
            else
            {
                //limpa tudo

                CMSCache.Paginas = new Dictionary<string, CMSCachePagina>();
                CMSCache.Estruturas = new Dictionary<int, List<EstruturaInfo>>();
            }
        }

        #endregion

        #region IServicoSite - Seleção

        public ConteudoResponse SelecionarConteudo(ConteudoRequest pRequest)
        {
            ConteudoResponse lRetorno = new ConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarConteudo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarConteudo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public WidgetResponse SelecionarWdiget(WidgetRequest pRequest)
        {
            WidgetResponse lRetorno = new WidgetResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarWdiget(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarWdiget - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;

        }

        public EstruturaResponse SelecionarEstrutura(EstruturaRequest pRequest)
        {
            EstruturaResponse lRetorno = new EstruturaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarEstrutura(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarEstrutura - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public PaginaResponse SelecionarPagina(PaginaRequest pRequest)
        {
            PaginaResponse lRetorno = new PaginaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarPagina(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarPagina - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }
        
        public PaginaResponse SelecionarPaginaCompleta(PaginaRequest pRequest)
        {
            PaginaResponse lRetorno = new PaginaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarPaginaCompleta(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarPaginaCompleta - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public PaginaResponse SelecionarPaginas(PaginaRequest pRequest)
        {
            PaginaResponse lRetorno = new PaginaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarPaginas(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarPaginas - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public PaginaResponse BuscarPaginasEVersoes(PaginaRequest pRequest)
        {
            PaginaResponse lRetorno = new PaginaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.BuscarPaginasEVersoes(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscarPaginasEVersoes - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public PaginaConteudoResponse SelecionarPaginaConteudo(PaginaConteudoRequest pRequest)
        {
            PaginaConteudoResponse lRetorno = new PaginaConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarPaginaConteudo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;


            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarPaginaWidget - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ConteudoResponse SelecionarConteudoPorPropriedade(ConteudoRequest pRequest)
        {
            ConteudoResponse lRetorno = new ConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarConteudoPorPropriedade(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarConteudoPorPropriedade - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ConteudoResponse SelecionarConteudoEntreDatas(ConteudoRequest pRequest)
        {
            ConteudoResponse lRetorno = new ConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarConteudoEntreDatas(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarConteudoEntreDatas - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public TipoConteudoResponse SelecionarTipoConteudo(TipoConteudoRequest pRequest)
        {
            TipoConteudoResponse lRetorno = new TipoConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarTipoConteudo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarTipoConteudo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ListaConteudoResponse SelecionarListaConteudo(ListaConteudoRequest pRequest)
        {
            ListaConteudoResponse lRetorno = new ListaConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.SelecionarListaConteudo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarListaConteudo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public BuscarItensDaListaResponse BuscarItensDaLista(BuscarItensDaListaRequest pRequest)
        {
            BuscarItensDaListaResponse lRetorno = new BuscarItensDaListaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.BuscarItensDaLista(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscarItensDaLista - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public BuscarItensDaListaResponse BuscarBannersLaterais(BuscarItensDaListaRequest pRequest)
        {
            BuscarItensDaListaResponse lRetorno = new BuscarItensDaListaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.BuscarBannersLaterais(pRequest);

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscarBannersLaterais - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public DateTime SelecionaUltimoDiaUtil()
        {
            DateTime lRetorno = new DateTime();

            PersistenciaClubes lClube = new PersistenciaClubes();

            try
            {
                lRetorno = lClube.SelecionaUltimoDiaUtil();
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em SelecionaUltimoDiaUtil - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ClubeResponse SelecionarClube(ClubeRequest pRequest)
        {
            ClubeResponse lRetorno = new ClubeResponse();

            try
            {
                PersistenciaClubes lDb = new PersistenciaClubes();

                lRetorno = lDb.SelecionarClube(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarClube - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ClubeResponse SelecionarExtratoClube(ClubeRequest pRequest)
        {
            ClubeResponse lRetorno = new ClubeResponse();

            try
            {
                PersistenciaClubes lDb = new PersistenciaClubes();

                lRetorno = lDb.SelecionarExtratoClube(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarExtratoClube - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ClubeResponse SelecionarPosicaoClube(ClubeRequest pRequest)
        {
            ClubeResponse lRetorno = new ClubeResponse();

            try
            {
                PersistenciaClubes lDb = new PersistenciaClubes();

                lRetorno = lDb.SelecionarPosicaoClube(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarPosicaoClube - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public FundoResponse SelecionarFundo(FundoRequest pRequest)
        {
            FundoResponse lRetorno = new FundoResponse();

            try
            {
                PersistenciaClubes lDb = new PersistenciaClubes();

                lRetorno = lDb.SelecionarFundo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarFundo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }
        
        public FundoResponse SelecionarFundoItau(FundoRequest pRequest)
        {
            FundoResponse lRetorno = new FundoResponse();

            try
            {
                PersistenciaClubes lDb = new PersistenciaClubes();

                lRetorno = lDb.SelecionarFundoItau(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarFundoItau - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Busca Informações de Cliente no sinacor  
        /// </summary>
        /// <param name="pRequest">Dados do cliente para efetuar o filtro na procedure do banco de dados do sinacor</param>
        /// <returns>Retorna uma lista dos clientes encontrados...</returns>
        public ClienteSinacorResponse BuscaInformacoesClienteSinacor(ClienteSinacorRequest pRequest)
        {
            var lRetorno = new ClienteSinacorResponse();

            try
            {
                PersistenciaCadastro lDb = new PersistenciaCadastro();

                lRetorno = lDb.BuscaInformacoesClienteSinacor(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscaInformacoesClienteSinacor - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }


        public ContaBancariaResponse BuscarContasBancariasDoCliente(ContaBancariaRequest pRequest)
        {
            ContaBancariaResponse lRetorno = new ContaBancariaResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.BuscarContasBancariasDoCliente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscarContasBancariasDoCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ContaCorrenteResponse ObterSaldoContaCorrente(ContaCorrenteRequest pRequest)
        {
            ContaCorrenteResponse lRetorno = new ContaCorrenteResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();



                lRetorno = lDb.ObterSaldoContaCorrente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ObterSaldoContaCorrente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }


        public decimal ObterSaldoAbertura(Int32 pRequest)
        {
            decimal lRetorno = 0;

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterSaldoAbertura(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterSaldoContaCorrente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public DateTime ObterDataPregao(Int32 pDias)
        {
            DateTime lRetorno = DateTime.Now;

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterDataPregao(pDias);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterDataPregao - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public List<CustodiaBTC> ObterBTC(Int32 pRequest)
        {
            List<CustodiaBTC> lRetorno = new List<CustodiaBTC>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterBTC(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterBTC - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public List<Garantia> ObterGarantias(Int32 pRequest)
        {
            List<Garantia> lRetorno = new List<Garantia>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterGarantiaDinheiro(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterGarantias - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public List<Provento> ObterGarantiasDividendos(Int32 pRequest)
        {
            List<Provento> lRetorno = new List<Provento>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterGarantiaDividendo(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterGarantiasDividendos - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }


        public List<GarantiaBMF> ObterGarantiasBMF(Int32 pRequest)
        {
            List<GarantiaBMF> lRetorno = new List<GarantiaBMF>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterGarantiaDinheiroBMF(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterGarantiasBMF - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public List<CustodiaTermo> ObterTermo(Int32 pRequest)
        {
            List<CustodiaTermo> lRetorno = new List<CustodiaTermo>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterTermo(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterTermo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public List<CustodiaTermo> ObterTermoALiquidar(Int32 pRequest)
        {
            List<CustodiaTermo> lRetorno = new List<CustodiaTermo>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterTermoALiquidar(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterTermoALiquidar - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public List<CustodiaTesouro> ObterTesouroDireto(Int32 pRequest)
        {
            List<CustodiaTesouro> lRetorno = new List<CustodiaTesouro>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterTesouroDireto(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterTermoALiquidar - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public DateTime? ObterDataPosicaoFundo(String pCodigoAnbima, decimal pCota)
        {
            DateTime? lRetorno = null;

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterDataPosicaoFundo(pCodigoAnbima, pCota);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterDataPosicaoFundo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public List<ChamadaMargem> ObterChamadaMargem(Int32 pRequest)
        {
            List<ChamadaMargem> lRetorno = new List<ChamadaMargem>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterChamadaMargem(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterChamadaMargem - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public List<Provento> ObterProventos(Int32 pRequest)
        {
            List<Provento> lRetorno = new List<Provento>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterProventos(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterProventos - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public List<ResgateFundo> ObterResgateFundo(Int32 pRequest)
        {
            List<ResgateFundo> lRetorno = new List<ResgateFundo>();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterResgateFundo(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterResgateFundo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public DateTime? ObterDataProcessamentoFundo(Int32 pRequest)
        {
            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                return lDb.ObterDataProcessamentoFundo(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterDataProcessamentoFundo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public List<PosicaoFundo> ObterPosicaoFundo(Int32 pRequest)
        {
            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                return lDb.ObterPosicaoFundo(pRequest);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ObterPosicaoFundo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public CustodiaResponse ObterPosicaoAtual(int CBLC)
        {
            CustodiaResponse lRetorno = new CustodiaResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterPosicaoAtual(CBLC);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ObterPosicaoAtual - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public CustodiaResponse ObterPosicaoAtualBMF(int CBLC)
        {
            CustodiaResponse lRetorno = new CustodiaResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ObterPosicaoAtualBMF(CBLC);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ObterPosicaoAtualBMF - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public UltimasNegociacoesResponse ConsultarUltimasNegociacoesCliente(UltimasNegociacoesRequest pRequest)
        {
            UltimasNegociacoesResponse lRetorno = new UltimasNegociacoesResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ConsultarUltimasNegociacoesCliente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ConsultarUltimasNegociacoesCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public FundoResponse SelecionaFundoPorCliente(FundoRequest pRequest)
        {
            FundoResponse lRetorno = new FundoResponse();

            try
            {
                PersistenciaClubes lDb = new PersistenciaClubes();

                lRetorno = lDb.SelecionaFundoPorCliente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionaFundoPorCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public FundoResponse SelecionaFundoPorCotasClientes(FundoRequest pRequest)
        {
            FundoResponse lRetorno = new FundoResponse();

            try
            {
                PersistenciaClubes lDb = new PersistenciaClubes();

                lRetorno = lDb.SelecionaFundoPorCotasClientes(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionaFundoPorCotasClientes - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public AtivoResponse ListarAtivo(AtivoRequest pRequest)
        {
            AtivoResponse lRetorno = new AtivoResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.ListarAtivo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ListarAtivo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public IntegracaoIRResponse SelecionarIntegracaoIR(int pCodigoCliente)
        {
            IntegracaoIRResponse lRetorno = new IntegracaoIRResponse();

            try
            {
                PersistenciaIR lDb = new PersistenciaIR();

                lRetorno = lDb.SelecionarIntegracaoIR(pCodigoCliente);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarIntegracaoIR - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public InformeRendimentosTesouroResponse GetRendimentoTesouroDireto(InformeRendimentosTesouroRequest pRequest)
        {
            InformeRendimentosTesouroResponse lRetorno = new InformeRendimentosTesouroResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.GetRendimentoTesouroDireto(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em GetRendimentoTesouroDireto - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public InformeRendimentosResponse GetRendimento(InformeRendimentosRequest pRequest)
        {
            InformeRendimentosResponse lRetorno = new InformeRendimentosResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.GetRendimento(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em GetRendimento - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public SinacorEnderecoResponse GetEnderecoSinacorCustodia(SinacorEnderecoRequest pRequest)
        {
            SinacorEnderecoResponse lRetorno = new SinacorEnderecoResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.GetEnderecoSinacorCustodia(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em GetEnderecoSinacorCustodia - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public SinacorEnderecoResponse GetEnderecoSinacorCorrespondencia(SinacorEnderecoRequest pRequest)
        {
            SinacorEnderecoResponse lRetorno = new SinacorEnderecoResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.GetEnderecoSinacorCorrespondencia(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em GetEnderecoSinacorCorrespondencia - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public BuscarHtmlDaPaginaResponse BuscarHtmlPagina(BuscarHtmlDaPaginaRequest pRequest)
        {
            BuscarHtmlDaPaginaResponse lRetorno = new BuscarHtmlDaPaginaResponse();

            try
            {
                string lHtmlDaPagina = "<p>n/d</p>";

                List<WidgetBase> lLista = new List<WidgetBase>();

                PaginaRequest lRequest = new PaginaRequest();
                PaginaResponse lResponse;

                //string lMensagem;

                lRequest.Pagina = new PaginaInfo();

                lRequest.Pagina.CodigoPagina = pRequest.IdDaPagina; //this.PaginaMaster.IdDaPagina;
                lRequest.VersaoDaEstrutura = pRequest.Versao;

                lResponse = SelecionarPaginaCompleta(lRequest);

                if (lResponse.Pagina != null)
                {
                    //this.PaginaMaster.SubSecao = new SecaoDoSite(lResponse.ListaPagina[0].NomePagina, lResponse.ListaPagina[0].DescURL);

                    VersaoInfo lVersaoDaPagina = lResponse.Pagina.BuscarVersao(pRequest.Versao, false);

                    EstruturaInfo lEstruturaDaPagina = null;

                    if (lVersaoDaPagina != null)
                    {

                        for (int i = 0; i < lVersaoDaPagina.ListaEstrutura.Count; i++)
                        {
                            if (lVersaoDaPagina.ListaEstrutura[i].TipoUsuario == 1 || (lVersaoDaPagina.ListaEstrutura[i].TipoUsuario == 3 && pRequest.ExisteClienteLogado))
                            {
                                //só interessa o tipo da estrutura que seja "todos" ou "cliente" se tiver também um usuário logado
                                //this.EstruturaDaPaginaParaTodosTipos = true;

                                lEstruturaDaPagina = lVersaoDaPagina.ListaEstrutura[i];

                                lRetorno.URL = lResponse.Pagina.DescURL;

                                break;
                            }
                            else if (lVersaoDaPagina.ListaEstrutura[i].TipoUsuario == 2 && !pRequest.ExisteClienteLogado)
                            {
                                lEstruturaDaPagina = lVersaoDaPagina.ListaEstrutura[i];

                                lRetorno.URL = lResponse.Pagina.DescURL;

                                break;
                            }
                        }

                        if (lEstruturaDaPagina != null)
                        {
                            StringBuilder lBuilder = new StringBuilder();

                            foreach (WidgetInfo lWidget in lEstruturaDaPagina.ListaWidget)
                            {
                                lLista.Add((WidgetBase)this.InstanciarWidget(lWidget));
                            }

                            foreach (WidgetBase lWidget in lLista)
                            {
                                if (lWidget != null)
                                {
                                    lWidget.RenderizarHabilitandoCMS = false;

                                    lWidget.ExisteUsuarioLogado = pRequest.ExisteClienteLogado;

                                    lWidget.HostERaiz = pRequest.HostERaiz;

                                    lWidget.VersaoDaEstrutura = pRequest.Versao;

                                    lBuilder.AppendLine(lWidget.Renderizar(pRequest.RenderizandoNaAba)); //iniciando o "seed" do abinception 
                                }
                            }

                            lHtmlDaPagina = lBuilder.ToString();
                        }
                        else
                        {
                            lHtmlDaPagina = string.Format("<p>Estrutura não encontrada para a página [{0}]</p>", pRequest.IdDaPagina);
                        }
                    }
                    else
                    {
                        lHtmlDaPagina = string.Format("<p>Versão [{0}] não encontrada para a página [{1}]</p>", pRequest.Versao, pRequest.IdDaPagina);
                    }
                }
                else
                {
                    lHtmlDaPagina = string.Format("<p>Pagína [{0}] não encontrada!</p>", pRequest.IdDaPagina);
                }

                lRetorno.HTML = lHtmlDaPagina;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = string.Format("Erro em BuscarHtmlDaPagina: [{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public BuscarNasPaginasResponse BuscarNasPaginas(BuscarNasPaginasRequest pRequest)
        {
            BuscarNasPaginasResponse lResponse = new BuscarNasPaginasResponse();

            try
            {
                lResponse.Resultados = new List<PaginaBuscaInfo>();

                foreach (string lChave in CMSCache.Paginas.Keys)
                {
                    if (CMSCache.Paginas[lChave].HTML.ToLower().Contains(pRequest.Termo.ToLower()))
                    {
                        lResponse.Resultados.Add(new PaginaBuscaInfo(pRequest.Termo, lChave, CMSCache.Paginas[lChave].HTML));
                    }

                    if (lResponse.Resultados.Count >= 10)
                        break;
                }

                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                string lMensagem = string.Format("Erro em IServicoSite.BuscarNasPaginas({0}) [{1}]\r\n{2}", pRequest.Termo, ex.Message, ex.StackTrace);

                gLogger.Error(lMensagem);

                lResponse.DescricaoResposta = lMensagem;

                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lResponse;
        }

        public BuscarVersoesResponse BuscarVersoes(BuscarVersoesRequest pRequest)
        {
            BuscarVersoesResponse lRetorno = new BuscarVersoesResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.BuscarVersoes(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscarVersoes - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public VersaoResponse IncluirVersao(VersaoRequest pRequest)
        {
            VersaoResponse lRetorno = new VersaoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.IncluirVersao(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em IncluirVersao - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public VersaoResponse PublicarVersao(VersaoRequest pRequest)
        {
            VersaoResponse lRetorno = new VersaoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.PublicarVersao(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em PublicarVersao - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        #endregion

        #region IServicoSite - Inserir/Atualizar

        public PaginaResponse InserirPagina(PaginaRequest pRequest)
        {
            PaginaResponse lRetorno = new PaginaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.InserirPagina(pRequest);

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirPagina - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public EstruturaResponse InserirEstrutura(EstruturaRequest pRequest)
        {
            EstruturaResponse lRetorno = new EstruturaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.InserirEstrutura(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirEstrutura - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public WidgetResponse InserirWidget(WidgetRequest pRequest)
        {
            WidgetResponse lRetorno = new WidgetResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.InserirWidget(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirWidget - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ConteudoResponse InserirConteudo(ConteudoRequest pRequest)
        {
            ConteudoResponse lRetorno = new ConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.InserirConteudo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;


            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirConteudo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public FichaPerfilResponse SelecionarFichaPerfil(FichaPerfilRequest pParametro)
        {
            var lRetorno = new FichaPerfilResponse();

            try
            {
                lRetorno = new FichaPerfilDbLib().SelecionarFicha(pParametro);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirConteudo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public AtualizarOrdemDoWidgetNaPaginaResponse AtualizarOrdemDoWidgetNaPagina(AtualizarOrdemDoWidgetNaPaginaRequest pRequest)
        {
            AtualizarOrdemDoWidgetNaPaginaResponse lRetorno = new AtualizarOrdemDoWidgetNaPaginaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.AtualizarOrdemDoWidgetNaPagina(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em AualizarOrdemWidget - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ListaConteudoResponse InserirListaConteudo(ListaConteudoRequest pRequest)
        {
            ListaConteudoResponse lRetorno = new ListaConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.InserirListaConteudo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;


            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirListaConteudo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public EstruturaResponse CopiarEstrutra(EstruturaRequest pRequest)
        {
            EstruturaResponse lRetorno = new EstruturaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.CopiarEstrutra(pRequest);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public InserirLogRocketResponse InserirLogRocket(InserirLogRocketRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            InserirLogRocketResponse lResponse = new InserirLogRocketResponse();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoCadastro;

                using (DbCommand _DbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "log_rocket_ins_sp"))
                {
                    lAcessaDados.AddInParameter(_DbCommand, "@dados_enviados", DbType.String, pRequest.DadosEnviados);
                    lAcessaDados.AddInParameter(_DbCommand, "@retorno", DbType.String, pRequest.Resposta);
                
                    lAcessaDados.AddOutParameter(_DbCommand, "@id_log_rocket", DbType.Int32, 0);

                    lAcessaDados.ExecuteNonQuery(_DbCommand);

                    lResponse.IdLogRocket = _DbCommand.Parameters["@id_log_rocket"].Value.DBToInt32();

                    lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        #region Integração IR

        /// <summary>
        /// Inseri um cliente a partir da data inicio, que deve ser maior ou igual a data corrente.
        /// Inseri somente para BOVESPA
        /// </summary>
        /// <param name="IntegracaoIR"></param>
        /// <returns></returns>
        public IntegracaoIRResponse IncluirIRBovespaSimples(IntegracaoIRRequest pRequest)
        {
            IntegracaoIRResponse lRetorno = new IntegracaoIRResponse();

            try
            {
                this.VerificarIntegracao(pRequest.IntegracaoIR);

                pRequest.IntegracaoIR.CdEvento = IntegracaoIRInfo.CodigoEvento.CADASTRO_NOVO;

                pRequest.IntegracaoIR.TPProduto = IntegracaoIRInfo.TipoProduto.BOVESPA;

                pRequest.IntegracaoIR.IdBovespa = this.ToCodigoClienteFormatado(pRequest.IntegracaoIR.IdBovespa).DBToInt32();

                pRequest.IntegracaoIR.EstadoBloqueado = "S";

                PersistenciaIR lb = new PersistenciaIR();

                lb.InserirIntegracaoIR(pRequest);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirIntegracaoIR - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Inseri um cliente a partir da data inicio, que deve ser maior ou igual a data corrente.
        /// Inserir para BMF e BOVESPA
        /// </summary>
        /// <param name="IntegracaoIR"></param>
        /// <returns></returns>
        public IntegracaoIRResponse IncluirIRBovespaBMFSimples(IntegracaoIRRequest pRequest)
        {
            IntegracaoIRResponse lRetorno = new IntegracaoIRResponse();

            try
            {
                PersistenciaIR lb = new PersistenciaIR();

                this.VerificarIntegracao(pRequest.IntegracaoIR);

                if (pRequest.IntegracaoIR.dataInicio > DateTime.Now)
                    throw new Exception("A data da transação deve ser menor que a data de hoje.");

                pRequest.IntegracaoIR.EstadoBloqueado = "S";

                if (pRequest.IntegracaoIR.IdBMF < 1)
                    throw new Exception("Código BMF precisa ser maior que 0.");

                pRequest.IntegracaoIR.IdBovespa = this.ToCodigoClienteFormatado(pRequest.IntegracaoIR.IdBovespa).DBToInt32();

                pRequest.IntegracaoIR.IdBMF = pRequest.IntegracaoIR.IdBMF.DBToInt32();

                lb.InserirIntegracaoIRBMF(pRequest);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirIntegracaoIRBMF - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Inseri um cliente num determinado intervalo de datas.
        /// Este intervalo de datas deve ser menor que a data corrente.
        /// Método valido para BOVESPA
        /// </summary>
        /// <param name="IntegracaoIR"></param>
        /// <returns></returns>
        public IntegracaoIRResponse IncluirIRRetrocedente(IntegracaoIRRequest pRequest)
        {
             IntegracaoIRResponse lRetorno = new IntegracaoIRResponse();

             try
             {
                 PersistenciaIR lb = new PersistenciaIR();

                 this.VerificarIntegracao(pRequest.IntegracaoIR);

                 if (pRequest.IntegracaoIR.dataInicio > DateTime.Now)
                     throw new Exception("As datas de inicio e fim devem ser menores que a data de hoje.");

                 pRequest.IntegracaoIR.EstadoBloqueado = "S";

                 pRequest.IntegracaoIR.IdBovespa = Convert.ToInt32(this.ToCodigoClienteFormatado(pRequest.IntegracaoIR.IdBovespa));

                 lb.InserirRetrocedente(pRequest);
             }
             catch (Exception ex)
             {
                 lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                 lRetorno.DescricaoResposta = ex.ToString();

                 gLogger.ErrorFormat("Erro em InserirRetrocedente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
             }

             return lRetorno;
            
        }

        /// <summary>
        /// Inseri um cliente num determinado intervalo de datas.
        /// Este intervalo de datas deve ser menor que a data corrente.
        /// Método valido para BOVESPA E BMF.
        /// </summary>
        /// <param name="IntegracaoIR"></param>
        /// <returns></returns>
        public IntegracaoIRResponse IncluirIRRetrocedenteBMF(IntegracaoIRRequest pRequest)
        {
             IntegracaoIRResponse lRetorno = new IntegracaoIRResponse();

             try
             {
                 PersistenciaIR lb = new PersistenciaIR();

                 this.VerificarIntegracao(pRequest.IntegracaoIR);

                 if (pRequest.IntegracaoIR.dataInicio > DateTime.Now)
                     throw new Exception("data da transação deve ser menor que hoje.");

                 pRequest.IntegracaoIR.EstadoBloqueado = "S";

                 if (pRequest.IntegracaoIR.IdBMF < 1)
                     throw new Exception("Código BMF precisa ser maior que 0.");

                 pRequest.IntegracaoIR.IdBovespa    = Convert.ToInt32(this.ToCodigoClienteFormatado(pRequest.IntegracaoIR.IdBovespa))   ;

                 pRequest.IntegracaoIR.IdBMF        = Convert.ToInt32(pRequest.IntegracaoIR.IdBMF)                                      ;

                 lb.InserirIRRetrocedenteBMF(pRequest);
             }
             catch (Exception ex)
             {
                 lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                 lRetorno.DescricaoResposta = ex.ToString();

                 gLogger.ErrorFormat("Erro em InserirIRRetrocedenteBMF - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
             }

             return lRetorno;
            
        }


        #endregion

        #endregion

        #region IServicoSite - Delete

        public ListaConteudoResponse ApagarListaConteudo(ListaConteudoRequest pRequest)
        {
            ListaConteudoResponse lRetorno = new ListaConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.ApagarListaConteudo(pRequest);
                
                lRetorno.DataResposta = DateTime.Now;


            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ApagarListaConteudo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public WidgetResponse ApagarWidget(WidgetRequest pRequest)
        {
            WidgetResponse lRetorno = new WidgetResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.ApagarWidget(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;


            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ApagarWidget - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ConteudoResponse ApagarConteudo(ConteudoRequest pRequest)
        {
            ConteudoResponse lRetorno = new ConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.ApagarConteudo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;


            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ApagarConteudo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public TipoConteudoResponse ApagarTipoConteudo(TipoConteudoRequest pRequest)
        {
            TipoConteudoResponse lRetorno = new TipoConteudoResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.ApagarTipoConteudo(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;


            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ApagarTipoConteudo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public PaginaResponse ExcluirPagina(PaginaRequest pRequest)
        {
            PaginaResponse lRetorno = new PaginaResponse();

            try
            {
                PersistenciaPagina lDb = new PersistenciaPagina();

                lRetorno = lDb.ExcluirPagina(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em ExcluirPagina - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        #endregion

        #region IServicoSite - Comercial

        public BuscarDadosDosProdutosResponse BuscarDadosDosProdutos(BuscarDadosDosProdutosRequest pRequest)
        {
            BuscarDadosDosProdutosResponse lResponse = new BuscarDadosDosProdutosResponse();

            try
            {
                PersistenciaComercial lPersistencia = new PersistenciaComercial();

                lResponse = lPersistencia.BuscarDadosDosProdutos(pRequest);
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscarDadosDosProdutos - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        public InserirLogDePagamentoResponse InserirLogDePagamento(InserirLogDePagamentoRequest pRequest)
        {
            InserirLogDePagamentoResponse lResponse = new InserirLogDePagamentoResponse();

            try
            {
                PersistenciaComercial lPersistencia = new PersistenciaComercial();

                lResponse = lPersistencia.InserirLogDePagamento(pRequest);
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirLogDePagamento - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }
        
        public InserirPagamentoResponse InserirPagamento(InserirPagamentoRequest pRequest)
        {
            InserirPagamentoResponse lResponse = new InserirPagamentoResponse();

            try
            {
                PersistenciaComercial lPersistencia = new PersistenciaComercial();

                lResponse = lPersistencia.InserirPagamento(pRequest);
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirPagamento - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        public InserirTransacaoResponse InserirTransacao(InserirTransacaoRequest pRequest)
        {
            InserirTransacaoResponse lResponse = new InserirTransacaoResponse();

            try
            {
                PersistenciaComercial lPersistencia = new PersistenciaComercial();

                lResponse = lPersistencia.InserirTransacao(pRequest);
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirTransacao - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }
        
        public InserirProdutoPorTransacaoResponse InserirProdutoPorTransacao(InserirProdutoPorTransacaoRequest pRequest)
        {
            InserirProdutoPorTransacaoResponse lResponse = new InserirProdutoPorTransacaoResponse();

            try
            {
                PersistenciaComercial lPersistencia = new PersistenciaComercial();

                lResponse = lPersistencia.InserirProdutoPorTransacao(pRequest);
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirProdutoPorTransacao - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }
        

        public InserirVendaResponse InserirVenda(InserirVendaRequest pRequest)
        {
            InserirVendaResponse lResponse = new InserirVendaResponse();

            try
            {
                PersistenciaComercial lPersistencia = new PersistenciaComercial();

                lResponse = lPersistencia.InserirVenda(pRequest);
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirVenda - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        public BuscarComprasDoClienteResponse BuscarComprasDoCliente(BuscarComprasDoClienteRequest pRequest)
        {
            BuscarComprasDoClienteResponse lResponse = new BuscarComprasDoClienteResponse();

            try
            {
                PersistenciaComercial lPersistencia = new PersistenciaComercial();

                lResponse = lPersistencia.BuscarComprasDoCliente(pRequest);
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscarComprasDoCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        public BuscarProdutosDoClienteResponse BuscarProdutosDoCliente(BuscarProdutosDoClienteRequest pRequest)
        {
            BuscarProdutosDoClienteResponse lResponse = new BuscarProdutosDoClienteResponse();

            try
            {
                PersistenciaComercial lPersistencia = new PersistenciaComercial();

                lResponse = lPersistencia.BuscarProdutosDoCliente(pRequest);
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscarComprasDoCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        #endregion

        public LojaResponse BuscarLojas(LojaRequest pParametro)
        {
            var lRetorno = new LojaResponse();

            try
            {
                lRetorno = new Gradual.Site.DbLib.Persistencias.Invetimentos.PersistenciaLojas().BuscarLojas(pParametro);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro ao buscar lojas - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public TipoTecladoResponse BuscarTipoTeclado(TipoTecladoRequest pRequest)
        {
            TipoTecladoResponse lRetorno = new TipoTecladoResponse();

            try
            {
                PersistenciaMinhaConta lDb = new PersistenciaMinhaConta();

                lRetorno = lDb.GetTipoTeclado(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em BuscarTipoTeclado - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Geral;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using System.Configuration;
using log4net;
using Gradual.Site.Lib.Mensagens;
using Gradual.Site.Lib;
using Gradual.Site.Lib.Dados;
using Gradual.Site.Lib.Widgets;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text;

namespace Gradual.OMS.WsIntegracao
{
    /// <summary>
    /// Summary description for Plataforma
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Plataforma : System.Web.Services.WebService
    {
        #region Propriedades

        private static ILog _Logger = null;

        private static ILog Logger
        {
            get
            {
                if (_Logger == null)
                {
                    log4net.Config.XmlConfigurator.Configure();

                    _Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }

                return _Logger;
            }
        }

        #endregion

        #region Métodos Private

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

                    default:
                        break;
                }
            }
            return null;
        }

        private string BuscarHtmlDePaginaDoSite(int pIdDaPagina)
        {
            StringBuilder lBuilder = new StringBuilder();

            List<WidgetBase> lLista = new List<WidgetBase>();

            PaginaRequest lRequest = new PaginaRequest();
            PaginaResponse lResponse;

            ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

            IServicoPersistenciaSite lServico = Ativador.Get<IServicoPersistenciaSite>();

            //string lMensagem;

            lRequest.Pagina = new PaginaInfo();

            lRequest.Pagina.CodigoPagina = pIdDaPagina; //this.PaginaMaster.IdDaPagina;

            lResponse = lServico.SelecionarPagina(lRequest);

            if (lResponse.ListaPagina != null && lResponse.ListaPagina.Count > 0)
            {
                //this.PaginaMaster.SubSecao = new SecaoDoSite(lResponse.ListaPagina[0].NomePagina, lResponse.ListaPagina[0].DescURL);
                
                if (lResponse.ListaPagina[0].ListaEstrutura.Count > 0)
                {
                    EstruturaInfo lEstruturaDaPagina = null;

                    //this.TiposDeUsuarioLogadoDisponiveis.Clear();
                    //this.EstruturaDaPaginaParaTodosTipos = false;

                    for (int i = 0; i < lResponse.ListaPagina[0].ListaEstrutura.Count; i++)
                    {
                        //this.TiposDeUsuarioLogadoDisponiveis.Add(lResponse.ListaPagina[0].ListaEstrutura[i].TipoUsuario);

                        if (lResponse.ListaPagina[0].ListaEstrutura[i].TipoUsuario == 1 || lResponse.ListaPagina[0].ListaEstrutura[i].TipoUsuario == 5)
                        {
                            //só interessa o tipo da estrutura que seja "todos" ou "cliente"
                            //this.EstruturaDaPaginaParaTodosTipos = true;

                            lEstruturaDaPagina = lResponse.ListaPagina[0].ListaEstrutura[i];

                            break;
                        }
                    }

                    if (lEstruturaDaPagina != null)
                    {
                        //this.PaginaMaster.IdDaEstrutura = lEstruturaDaPagina.CodigoEstrutura;

                        //this.IDEstruturaCorrente = lEstruturaDaPagina.CodigoEstrutura; //Guarda em sessão o ID da Estrutura

                        //this.QuantidadeWidgetMaiorQueZero = lEstruturaDaPagina.ListaWidget.Count > 0;

                        foreach (WidgetInfo lWidget in lEstruturaDaPagina.ListaWidget)
                        {
                            lLista.Add((WidgetBase)this.InstanciarWidget(lWidget));
                        }

                        foreach (WidgetBase lWidget in lLista)
                        {
                            if (lWidget != null)
                            {
                                lWidget.RenderizarHabilitandoCMS = false;

                                lBuilder.AppendLine(lWidget.Renderizar());
                            }
                        }
                    }
                    else
                    {
                        Logger.InfoFormat("Estrutura não encontrada para a página [{0}]", pIdDaPagina);
                    }
                }
            }

            return lBuilder.ToString();
        }

        private void ExtrairLinksDaPagina(ref List<LinkAnaliseRelatorios> pLinks, string pTextoDaPagina)
        {
            string[] pLinhas = pTextoDaPagina.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            string lCategoria = "";

            foreach (string lLinha in pLinhas)
            {
                if (lLinha.Contains("<h2"))
                {
                    lCategoria = lLinha.Substring(lLinha.IndexOf('>') + 1);

                    if(lCategoria.Contains("span"))
                        lCategoria = lCategoria.Substring(lCategoria.IndexOf('>') + 1);

                    lCategoria = lCategoria.Substring(0, lCategoria.IndexOf('<')).TrimEnd().TrimStart();
                }
                else if (lLinha.Contains(".pdf") || lLinha.Contains(".PDF"))
                {
                    pLinks.Add(new LinkAnaliseRelatorios(lCategoria, lLinha));
                }
            }
        }

        #endregion

        #region Métodos Públicos

        [WebMethod]
        public BuscarCarteirasComAtivosResponse BuscarCarteirasComAtivos(BuscarCarteirasComAtivosRequest pRequest)
        {
            BuscarCarteirasComAtivosResponse lResponse = new BuscarCarteirasComAtivosResponse();

            AcessaDados lAcessaDados = null;
            DataTable lTable = null;
            DbCommand lCommand = null;

            Logger.InfoFormat(" >> BuscarCarteirasComAtivos({0})", pRequest.CodigoDoUsuario);

            try
            {
                int lCodCblc;
                List<int> lCodigosCarteiras;
                string lAtivos;

                if (int.TryParse(pRequest.CodigoDoUsuario, out lCodCblc))
                {
                    lAcessaDados = new AcessaDados();
                    lAcessaDados.ConnectionStringName = "GRADUAL_TRADE";

                    lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_lst");
                    lAcessaDados.AddInParameter(lCommand, "@CD_CBLC", DbType.Int32, lCodCblc);

                    lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                    lCodigosCarteiras = new List<int>();
                    lResponse.Carteiras = new List<string>();
                    lResponse.Ativos = new List<string>();

                    Logger.InfoFormat(" >> BuscarCarteirasComAtivos({0}): [{1}] Carteiras encontradas", pRequest.CodigoDoUsuario, lTable.Rows.Count);

                    foreach (DataRow dr in lTable.Rows)
                    {
                        lResponse.Carteiras.Add(Conversao.ToString(dr["ds_carteira"]));
                        lCodigosCarteiras.Add(Conversao.ToInt32(dr["id_carteira"]));
                    }

                    foreach (int lIdCarteira in lCodigosCarteiras)
                    {
                        Logger.InfoFormat(" >> BuscarCarteirasComAtivos({0}): Buscando ativos para carteira [{1}]", pRequest.CodigoDoUsuario, lIdCarteira);

                        lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_ATIVO_lst");
                        lAcessaDados.AddInParameter(lCommand, "@id_carteira", DbType.Int32, lIdCarteira);

                        lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                        Logger.InfoFormat(" >> BuscarCarteirasComAtivos({0}): [{1}] Ativos encontrados para carteira [{2}]", pRequest.CodigoDoUsuario, lTable.Rows.Count, lIdCarteira);

                        lAtivos = "";

                        foreach (DataRow dr in lTable.Rows)
                        {
                            lAtivos += string.Format("{0},", Conversao.ToString(dr["cd_Negocio"]));
                        }

                        lAtivos = lAtivos.TrimEnd(',');

                        lResponse.Ativos.Add(lAtivos);
                    }

                    Logger.InfoFormat(" >> BuscarCarteirasComAtivos({0}): Retornando OK", pRequest.CodigoDoUsuario);

                    lResponse.StatusResposta = "OK";
                }
                else
                {
                    lResponse.DescricaoResposta = "O código do usuário deve ser numérico. O tipo string está sendo mantido por compatibilidade.";
                    lResponse.StatusResposta = "Erro Validação";

                    Logger.InfoFormat(" >> BuscarCarteirasComAtivos({0}): Retornando Erro [{1}]", pRequest.CodigoDoUsuario, lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lResponse.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
                lResponse.StatusResposta = "Exception";

                Logger.InfoFormat(" >> BuscarCarteirasComAtivos({0}): Retornando Exception [{1}]", pRequest.CodigoDoUsuario, lResponse.DescricaoResposta);
            }
            finally
            {
                lAcessaDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lResponse;
        }

        [WebMethod]
        public BuscarAvisosParaUsuariosDoHomebrokerResponse BuscarAvisosParaUsuariosDoHomeBroker(BuscarAvisosParaUsuariosDoHomebrokerRequest pRequest)
        {
            BuscarAvisosParaUsuariosDoHomebrokerResponse lResponse = new BuscarAvisosParaUsuariosDoHomebrokerResponse();

            try
            {
                ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

                ConsultarEntidadeRequest<AvisoHomeBrokerInfo> lRequestAviso = new ConsultarEntidadeRequest<AvisoHomeBrokerInfo>();
                ConsultarObjetosResponse<AvisoHomeBrokerInfo> lResponseAviso;

                lRequestAviso.Objeto = new AvisoHomeBrokerInfo();

                lRequestAviso.Objeto.IdSistema = pRequest.IdSistema;

                lResponseAviso = Gradual.Intranet.Servicos.BancoDeDados.Persistencias.ClienteDbLib.ConsultarAvisosHomeBroker(lRequestAviso);

                if (pRequest.BuscarSomenteAvisosAtivos  )
                {
                    lResponse.Avisos = new List<AvisoHomeBrokerInfo>();

                    foreach (AvisoHomeBrokerInfo lAviso in lResponseAviso.Resultado)
                    {
                        if ((lAviso.DtEntrada <= DateTime.Now && DateTime.Now <= lAviso.DtSaida) || lAviso.StAtivacaoManual == "S")
                        {
                            if(string.IsNullOrEmpty(lAviso.DsCBLCs) || lAviso.ContemCBLC(pRequest.CBLC))
                                lResponse.Avisos.Add(lAviso);
                        }
                    }
                }
                else
                {
                    lResponse.Avisos = lResponseAviso.Resultado;
                }

                //lResponse.TextoDoContrato = System.Text.UTF8Encoding.UTF8.GetString(lResponseArquivo.Objeto.Arquivo);

                lResponse.StatusResposta = "OK";
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = "ERRO";

                lResponse.DescricaoResposta = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        [WebMethod]
        public BuscarLinksAnaliseRelatoriosResposta BuscarLinksAnaliseRelatorios(BuscarLinksAnaliseRelatoriosRequest pRequest)
        {
            BuscarLinksAnaliseRelatoriosResposta lResponse = new BuscarLinksAnaliseRelatoriosResposta();

            List<LinkAnaliseRelatorios> lListaDeLinks = new List<LinkAnaliseRelatorios>();

            try
            {
                string lIDsParaCarregar = ConfigurationManager.AppSettings["IDsDeCategoriasParaAnaliseRelatorio"];

                Logger.InfoFormat("Carregando Links de Análise e Relatórios para os IDs [{0}]", lIDsParaCarregar);

                if(!string.IsNullOrEmpty(lIDsParaCarregar))
                {
                    string lTextoDaPagina;

                    string[] lIDs = lIDsParaCarregar.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (string lID in lIDs)
                    {
                        lTextoDaPagina = BuscarHtmlDePaginaDoSite(Convert.ToInt32(lID));

                        ExtrairLinksDaPagina(ref lListaDeLinks, lTextoDaPagina);
                    }
                }

                lResponse.StatusResposta = "OK";
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = "ERRO";

                lResponse.DescricaoResposta = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
            }

            lResponse.Links = lListaDeLinks;

            return lResponse;
        }

        [WebMethod]
        public string TestarBuscarLinksAnaliseRelatorios()
        {
            BuscarLinksAnaliseRelatoriosResposta lResposta;
            BuscarLinksAnaliseRelatoriosRequest lRequest = new BuscarLinksAnaliseRelatoriosRequest();

            lResposta = BuscarLinksAnaliseRelatorios(lRequest);

            return "OK";
        }

        [WebMethod]
        public string TestarBuscarAvisosParaUsuariosDoHomeBroker(string pCBLC)
        {
            BuscarAvisosParaUsuariosDoHomebrokerRequest lRequest = new BuscarAvisosParaUsuariosDoHomebrokerRequest();
            BuscarAvisosParaUsuariosDoHomebrokerResponse lResponse;

            lRequest.CBLC = pCBLC;

            lRequest.BuscarSomenteAvisosAtivos = true;

            lResponse = BuscarAvisosParaUsuariosDoHomeBroker(lRequest);

            return string.Format("[{0}] avisos encontrados para o CBLC [{1}]", lResponse.Avisos.Count, pCBLC);
        }

        #endregion
    }
}

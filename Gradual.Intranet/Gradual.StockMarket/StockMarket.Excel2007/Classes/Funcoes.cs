using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Globalization;
using Microsoft.Win32;

using MdsBayeuxClient;
using System.Diagnostics;
using log4net;
using System.Diagnostics.Eventing.Reader;
using StockMarket.Excel2007.Classes;
using Gradual.OMS.Library;
using Gradual.OMS.CadastroCliente.Lib;
using Gradual.OMS.CadastroCliente.Lib.Dados;
using Gradual.OMS.CadastroPapeis.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System.Threading;
using System.Configuration;
using System.IO;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using StockMarket.Excel2007.RTD;
using Extensibility;

namespace StockMarket.Excel2007
{
    public delegate void AtualizarPosicaoNetEventHandler(int totalLinhas, string cliente);

    [ComDefaultInterface(typeof(IFuncoes))]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class Funcoes : IFuncoes, RTD.IRTDServer, IDTExtensibility2
    {
        #region Globais

        public const string ACAO_LOGIN = "LOGIN";
        public const string ACAO_AJUSTAR_FREQUENCIA = "AJUSTAR_FREQUENCIA";
        public const string ACAO_MONTAR_COTACAO = "MONTAR_COTACAO";
        public const string ACAO_MONTAR_POSICAO_NET = "MONTAR_POSICAO_NET";
        public const string ACAO_MONTAR_TICKERCOTACAO = "MONTAR_TICKERCOTACAO";
        public const string ACAO_MONTAR_LIVROOFERTAS = "MONTAR_LIVROOFERTAS";
        public const string ACAO_MONTAR_CARTEIRA = "MONTAR_CARTEIRA";

        public const string FUNCAO_COTACAO = "SM_COTACAO";
        public const string FUNCAO_COTACAO_LINHA = "SM_COTACAO_LINHA";
        public const string FUNCAO_COTACAO_RAPIDA = "SM_COTACAORAPIDA";
        public const string FUNCAO_LIVRO_OFERTAS = "SM_LIVROOFERTAS";
        public const string FUNCAO_POSICAO_NET = "SM_POSICAO_NET";
        public const string FUNCAO_POSICAO = "SM_POSICAO";

        public const string PROPRIEDADE_ULTIMO_PRECO = "ULT";
        public const string PROPRIEDADE_VARIACAO = "VAR";
        public const string PROPRIEDADE_PRECO_MEDIO = "MEDIA";
        public const string PROPRIEDADE_CORRETORA_COMPRA = "CORRCPA";
        public const string PROPRIEDADE_QUANTIDADE_COMPRA = "QTDCPA";
        public const string PROPRIEDADE_PRECO_COMPRA = "PRCCPA";
        public const string PROPRIEDADE_PRECO_VENDA = "PRCVDA";
        public const string PROPRIEDADE_QUANTIDADE_VENDA = "QTDVDA";
        public const string PROPRIEDADE_CORRETORA_VENDA = "CORRVDA";
        public const string PROPRIEDADE_PRECO_ABERTURA = "ABERT";
        public const string PROPRIEDADE_PRECO_MAXIMO = "MAX";
        public const string PROPRIEDADE_PRECO_MINIMO = "MIN";
        public const string PROPRIEDADE_PRECO_FECHAMENTO = "FECH";
        public const string PROPRIEDADE_QUANTIDADE_TOTAL_PAPEIS = "QTDTOTAL";
        public const string PROPRIEDADE_QUANTIDADE_NEGOCIOS = "QTDNEG";
        public const string PROPRIEDADE_INSTRUMENTO = "PAPNEG";
        public const string PROPRIEDADE_VOLUME = "VOL";
        public const string PROPRIEDADE_QUANTIDADE_TEORICA = "QTDTEOR";
        public const string PROPRIEDADE_PRECO_TEORICO = "PRCTEOR";
        public const string PROPRIEDADE_PRECO_EXERCICIO = "PRCEXER";
        public const string PROPRIEDADE_DATA_VENCIMENTO = "DATAVENC";
        public const string PROPRIEDADE_ESTADO = "STATUS";
        public const string PROPRIEDADE_HORA = "HORA";

        public const string PROPRIEDADE_OFERTA_ATIVO = "ATIVO";
        public const string PROPRIEDADE_OFERTA_PRECO = "PRC";
        public const string PROPRIEDADE_OFERTA_CORRETORA = "CORR";
        public const string PROPRIEDADE_OFERTA_QUANTIDADE = "QTD";
        public const string PROPRIEDADE_OFERTA_SENTIDO_COMPRA = "CPA";
        public const string PROPRIEDADE_OFERTA_SENTIDO_VENDA = "VDA";

        public const string PROPRIEDADE_POSICAONET_CLIENTE = "CLIENTE";
        public const string PROPRIEDADE_POSICAONET_ATIVO = "PAPEL";
        public const string PROPRIEDADE_POSICAONET_QTD_EXEC_COMPRA = "QTDEXECCPA";
        public const string PROPRIEDADE_POSICAONET_QTD_EXEC_VENDA = "QTDEXECVDA";
        public const string PROPRIEDADE_POSICAONET_NET_EXEC = "NETEXEC";
        public const string PROPRIEDADE_POSICAONET_PRECO_MEDIO_EXEC_COMPRA = "PRMEDEXECCPA";
        public const string PROPRIEDADE_POSICAONET_PRECO_MEDIO_EXEC_VENDA = "PRMEDEXECVDA";
        public const string PROPRIEDADE_POSICAONET_QTD_ABERTA_COMPRA = "QTDABCPA";
        public const string PROPRIEDADE_POSICAONET_QTD_ABERTA_VENDA = "QTDABVDA";
        public const string PROPRIEDADE_POSICAONET_NET_ABERTA = "NETAB";
        public const string PROPRIEDADE_POSICAONET_PRECO_MEDIO_ABERTA_COMPRA = "PRMEDABCPA";
        public const string PROPRIEDADE_POSICAONET_PRECO_MEDIO_ABERTA_VENDA = "PRMEDABVDA";
        public const string PROPRIEDADE_POSICAONET_VOLUMEFIN_NET_ABERTA = "VLFINNETEXEC";
        public const string PROPRIEDADE_POSICAONET_VOLUMEFIN_NET_EXEC = "VLFINNETAB";
        public const string PROPRIEDADE_POSICAONET_TOTAL_REGISTROS = "TOTALREGS";

        private static IComRTDInfo comRTDInfo = null;
        private static DataTable _ListaAtivos = null;

        private IRTDUpdateEvent gCallback;
        private DateTime gUltimaAtualizacao = DateTime.MinValue;
        private ComRTDInfo gRtdInfo = new ComRTDInfo();

        private System.Windows.Forms.Timer gTimer;

        private Dictionary<int, TopicExcelInfo> gTopicsExcel = new Dictionary<int, TopicExcelInfo>();

        private Excel.Application gInstanciaExcel = null;

        private string gUrlStreamerDeCotacao;
        private string gUrlServicoCadastroPapeis;
        private string gUrlServicoCadastroCliente;
        private ClienteResumidoInfo gClienteInfo = null;
        private string gClienteBovespaPadrao = "";
        private bool gMdsClientConectado = false;

        private bool gAssinaturaAcompanhamentoOrdens = false;

        private Dictionary<string, MdsNegocios> gCotacao = new Dictionary<string, MdsNegocios>();
        private List<string> gAtivosInexistentes = new List<string>();
        private Dictionary<string, CadastroPapelMdsInfo> gCadastroBasico = new Dictionary<string, CadastroPapelMdsInfo>();

        private Dictionary<string, List<MdsOfertaRegistro>> gLivroCompra = new Dictionary<string, List<MdsOfertaRegistro>>();
        private Dictionary<string, List<MdsOfertaRegistro>> gLivroVenda = new Dictionary<string, List<MdsOfertaRegistro>>();

        private Dictionary<string, PosicaoNetInfo> gPosicaoOrdens = new Dictionary<string, PosicaoNetInfo>();
        private Dictionary<string, Dictionary<System.String, Acompanhamento>> ListaPosicoes = new Dictionary<string, Dictionary<string, Acompanhamento>>();

        private CultureInfo gCulture = new CultureInfo("pt-BR");

        #endregion

        #region Métodos Private

        private void GravaLogDebug(string mensagem)
        {
            using (StreamWriter streamWriter = new StreamWriter("c:\\Temp\\StockMarket.log", true))
            {
                streamWriter.Write(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss,fff") + " [Funcoes]: ");
                streamWriter.WriteLine(mensagem);
            }
        }

        private int ConverterParaInt(object pValor)
        {
            if (pValor == null) return 0;

            string lValor = pValor.ToString().Trim().Replace(".", null);

            if (string.IsNullOrEmpty(lValor)) return 0;

            if (lValor.Contains(","))
                lValor = lValor.Substring(0, lValor.IndexOf(','));

            return Convert.ToInt32(lValor, gCulture);
        }

        private Int64 ConverterParaInt64(object pValor)
        {
            if (pValor == null) return 0;

            string lValor = pValor.ToString();

            if (string.IsNullOrEmpty(lValor.Trim())) return 0;

            if (lValor.Contains(","))
                lValor = lValor.Substring(0, lValor.IndexOf(','));

            return Convert.ToInt64(lValor, gCulture);
        }

        private decimal ConverterParaDecimal(object pValor)
        {
            if (pValor == null) return 0;

            string lValor = pValor.ToString().Trim();

            if (string.IsNullOrEmpty(lValor)) return 0;
            
            if (lValor.ToUpper() == "ABERTURA") 
                return 0;

            return Convert.ToDecimal(lValor, gCulture);
        }

        private object ConverterPrecoParaDecimal(object pValor)
        {
            if (pValor == null) return 0;

            string lValor = pValor.ToString().Trim();

            if (string.IsNullOrEmpty(lValor)) return 0;

            if (lValor.ToUpper() == "ABERTURA")
                return lValor.ToUpper();

            return Convert.ToDecimal(lValor, gCulture);
        }

        private void IniciarTimer()
        {
            try
            {
                gTimer = new System.Windows.Forms.Timer();
                gTimer.Tick += new EventHandler(TimerEventHandler);
                gTimer.Interval = 1000;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em IniciarTimer: {0}", ex.Message));
            }
        }

        private void ConectarMdsClient()
        {
            if (gMdsClientConectado)
                return;

            if (System.Configuration.ConfigurationManager.AppSettings.Count == 0)
            {
                Configuration config = LoadLocalConfigurationFile();
                gUrlStreamerDeCotacao = config.AppSettings.Settings["UrlStreamerDeCotacao"].Value;
                gUrlServicoCadastroPapeis = config.AppSettings.Settings["UrlServicoCadastroPapeis"].Value;
                gUrlServicoCadastroCliente = config.AppSettings.Settings["UrlServicoCadastroCliente"].Value;
            }
            else
            {
                gUrlStreamerDeCotacao = System.Configuration.ConfigurationManager.AppSettings["UrlStreamerDeCotacao"];
                gUrlServicoCadastroPapeis = System.Configuration.ConfigurationManager.AppSettings["UrlServicoCadastroPapeis"];
                gUrlServicoCadastroCliente = System.Configuration.ConfigurationManager.AppSettings["UrlServicoCadastroCliente"];
            }

            try
            {
                Debug.WriteLine("Conectando HttpClient em ribStockMarket");
                //EventLogger.WriteFormat(EventLogEntryType.Information, "Conectando HttpClient em ribStockMarket");

                MdsHttpClient.Instance.Conecta(gUrlStreamerDeCotacao);

                DateTime horaAtual = DateTime.Now;
                while (!MdsHttpClient.Conectado)
                {
                    Thread.Sleep(300);
                    TimeSpan timeout = DateTime.Now.Subtract(horaAtual);
                    if (timeout.TotalSeconds > 20)
                    {
                        System.Windows.Forms.MessageBox.Show("Falha na conexao com o servidor após 20 segundos");
                        //EventLogger.WriteFormat(EventLogEntryType.Error, "Falha na conexao HttpClient em ribStockMarket após 20 segundos");
                        return;
                    }
                }
                gMdsClientConectado = true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Erro ao conectar em [{0}]: {1}\r\n\r\n{2}", gUrlStreamerDeCotacao, ex.Message, ex.StackTrace));
                return;
            }
            //EventLogger.WriteFormat(EventLogEntryType.Information, "MdsHttpClient conectado!");
        }

        private Configuration LoadLocalConfigurationFile()
        {
            string arquivoConfig = System.Reflection.Assembly.GetAssembly(typeof(StockMarket.Excel2007.Globals)).Location + ".config";
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, arquivoConfig)
            };
            return System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        private ClienteResumidoInfo ObterDadosCliente(string cliente)
        {
            ClienteResumidoInfo clienteInfo = null;

            try
            {
                IServicoCadastroCliente servico = Ativador.GetByAddr<IServicoCadastroCliente>(gUrlServicoCadastroCliente);

                BuscarClienteResumidoRequest request = new BuscarClienteResumidoRequest();
                request.DadosDoClienteParaBusca = new ClienteResumidoInfo();
                request.DadosDoClienteParaBusca.TipoDeConsulta = TipoDeConsultaClienteResumidoInfo.Clientes;
                request.DadosDoClienteParaBusca.OpcaoBuscarPor = OpcoesBuscarPor.CodBovespa;
                request.DadosDoClienteParaBusca.TermoDeBusca = cliente;
                request.DadosDoClienteParaBusca.OpcaoPendencia = 0;

                BuscarClienteResumidoResponse response = servico.BuscarClienteResumido(request);

                if (response.StatusResposta == MensagemResponseStatusEnum.OK && response.Resultados.Count > 0)
                {
                    clienteInfo = response.Resultados[0];
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Erro em ObterDadosCliente: {0}", ex.Message));
                //EventLogger.WriteFormat(EventLogEntryType.Error, string.Format("Erro em ObterDadosCliente: {0}", ex.Message));
            }

            return clienteInfo;
        }

        private CadastroPapelMdsInfo ObterCadastroPapel(string instrumento)
        {
            try
            {
                IServicoCadastroPapeis servico = Ativador.GetByAddr<IServicoCadastroPapeis>(gUrlServicoCadastroPapeis);

                BuscarPapelCadastradoRequest request = new BuscarPapelCadastradoRequest();
                request.Instrumento = instrumento;

                BuscarPapelCadastradoResponse response = servico.BuscarPapelCadastradoMds(request);
                if (response.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    if (response.PapelCadastradoMDS != null && response.PapelCadastradoMDS.Instrumento.Equals(instrumento.ToUpper()))
                    {
                        if ((DateTime)response.PapelCadastradoMDS.DataVencimento == DateTime.MinValue || 
                            (DateTime)response.PapelCadastradoMDS.DataVencimento >= DateTime.Now)
                            return response.PapelCadastradoMDS;
                        else
                            return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Exception em ObterCadastroPapel: {0}", ex.Message));
            }
            return null;
        }

        private DataTable ListaAtivos
        {
            get
            {
                if (_ListaAtivos == null)
                {
                    //Aplicacao.GravarLog("Aplicacao.ListaAtivos.get()", "Iniciando carga da lista de ativos.");

                    _ListaAtivos = new DataTable();

                    lock (_ListaAtivos)
                    {
                        try
                        {
                            _ListaAtivos.Columns.Add("Instrumento", typeof(string));
                            _ListaAtivos.Columns.Add("RazaoSocial", typeof(string));
                            _ListaAtivos.Columns.Add("CodigoISIN", typeof(string));
                            _ListaAtivos.Columns.Add("LotePadrao", typeof(string));
                            _ListaAtivos.Columns.Add("CodigoPapelObjeto", typeof(string));
                            _ListaAtivos.Columns.Add("SegmentoMercado", typeof(string));
                            _ListaAtivos.Columns.Add("ComposicaoIndice", typeof(string));
                            _ListaAtivos.Columns.Add("PrecoExercicio", typeof(string));
                            _ListaAtivos.Columns.Add("DataVencimento", typeof(string));
                            _ListaAtivos.Columns.Add("IndicadorOpcao", typeof(string));
                            _ListaAtivos.Columns.Add("TipoOpcao", typeof(string));

                            for (int bloco = 1; ; bloco++)
                            {
                                IServicoCadastroPapeis lServico = Ativador.GetByAddr<IServicoCadastroPapeis>(gUrlServicoCadastroPapeis);

                                ListarCadastroPapeisMdsRequest lRequest = new ListarCadastroPapeisMdsRequest();
                                lRequest.NumBlocoDados = bloco;

                                ListarCadastroPapeisMdsResponse lResponse = lServico.ConsultarCadastroPapeisMds(lRequest);

                                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                                {
                                    foreach (string item in lResponse.ListaCadastroPapeisMds)
                                    {
                                        string tipoOpcao = "";
                                        string[] campos = Regex.Split(item, @"\|\|");

                                        _ListaAtivos.Rows.Add(
                                            campos[0], // Instrumento
                                            campos[1], // RazaoSocial
                                            campos[6], // CodigoISIN
                                            campos[2], // LotePadrao
                                            campos[4], // CodigoPapelObjeto
                                            campos[5], // SegmentoMercado
                                            campos[7], // ComposicaoIndice
                                            Convert.ToDouble(campos[8].Replace('.', ',')).ToString("N2", gCulture), // PrecoExercicio //HACK: o preço Exercício do cadastro do MDS está vindo com '.' no separador invés da ','
                                            campos[9].Substring(6, 2) + "/" + campos[9].Substring(4, 2) + "/" + campos[9].Substring(0, 4), // DataVencimento
                                            campos[3], // IndicadorOpcao
                                            tipoOpcao
                                            );
                                    }
                                }
                                if (!lResponse.ContinuaDados)
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(string.Format("Aplicacao.ListaAtivos.get(): {0}", ex.Message));
                        }
                    }

                    //Aplicacao.GravarLog("Aplicacao.ListaAtivos.get()", "Lista de ativos carregada com sucesso.");
                }
                else
                {
                    lock (_ListaAtivos)
                    { }
                }
                return _ListaAtivos;
            }
        }

        private System.String RetornarCodigoPapel(System.String Instrumento)
        {
            var lListaAtivos = ListaAtivos;

            DataRow[] lRowPapel = lListaAtivos.Select(String.Format("instrumento='{0}'", Instrumento));

            if (lRowPapel.Length > 0)
            {
                switch (lRowPapel[0]["SegmentoMercado"].ToString())
                {
                    case "FUT": //SEGMENTOMERCADO_BMF_FUTURO
                        return lRowPapel[0]["Instrumento"].ToString().Trim();
                    case "SPOT": //SEGMENTOMERCADO_BMF_VISTA
                        return lRowPapel[0]["Instrumento"].ToString().Trim();
                    case "SOPT": //SEGMENTOMERCADO_BMF_OPCAO_VISTA
                        return lRowPapel[0]["Instrumento"].ToString().Trim();
                    case "FOPT": //SEGMENTOMERCADO_BMF_OPCAO_FUTURO
                        return lRowPapel[0]["Instrumento"].ToString().Trim();
                    case "DTERM": //SEGMENTOMERCADO_BMF_TERMO
                        return lRowPapel[0]["Instrumento"].ToString().Trim();
                    case "01": //SEGMENTOMERCADO_BOVESPA_VISTA
                        return lRowPapel[0]["CodigoPapelObjeto"].ToString().Trim();
                    case "91": //SEGMENTOMERCADO_BOVESPA_VISTA_OUTROS
                        return lRowPapel[0]["CodigoPapelObjeto"].ToString().Trim();
                    case "02": //SEGMENTOMERCADO_BOVESPA_TERMO
                        return lRowPapel[0]["CodigoPapelObjeto"].ToString().Trim();
                    case "03": //SEGMENTOMERCADO_BOVESPA_FRACIONARIO
                        return lRowPapel[0]["CodigoPapelObjeto"].ToString().Trim();
                    case "04": //SEGMENTOMERCADO_BOVESPA_OPCOES
                        return lRowPapel[0]["Instrumento"].ToString().Trim();
                    case "09": //SEGMENTOMERCADO_BOVESPA_EXERCICIO_OPCOES
                        return lRowPapel[0]["Instrumento"].ToString().Trim();
                    case "90": //SEGMENTOMERCADO_BOVESPA_INDICES
                        return lRowPapel[0]["CodigoPapelObjeto"].ToString().Trim();
                }
            }

            return System.String.Empty;
        }

        private void EfetuarAssinaturaLivroOfertas(string ativo)
        {
            if (gLivroCompra.Count == 0)
                MdsHttpClient.Instance.OnOfertasEvent += new MdsHttpClient.OnOfertasHandler(Instance_OnOfertasEvent);

            ConectarMdsClient();

            try
            {
                if (!gCadastroBasico.ContainsKey(ativo) && !gAtivosInexistentes.Contains(ativo))
                {
                    CadastroPapelMdsInfo dadosCadastroPapel = ObterCadastroPapel(ativo);
                    if (dadosCadastroPapel != null)
                        gCadastroBasico.Add(ativo, dadosCadastroPapel);
                    else
                        gAtivosInexistentes.Add(ativo);
                }

                if (!gLivroCompra.ContainsKey(ativo) && !gAtivosInexistentes.Contains(ativo))
                {
                    gLivroCompra.Add(ativo, new List<MdsOfertaRegistro>());
                    gLivroVenda.Add(ativo, new List<MdsOfertaRegistro>());

                    // Efetua assinatura de livro de ofertas do Instrumento
                    MdsAssinatura assinatura = new MdsAssinatura();
                    assinatura.Instrumento = ativo;
                    assinatura.Sinal = TipoSinal.LivroOfertas;
                    MdsHttpClient.Instance.Assina(assinatura);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Erro assinatura livro ofertas ativo[{0}]: {1}\r\n\r\n{2}",
                    ativo, ex.Message, ex.StackTrace));
            }
        }

        private void EfetuarAssinaturaNegocio(string ativo)
        {
            if (gCotacao.Count == 0)
                MdsHttpClient.Instance.OnNegociosEvent += new MdsHttpClient.OnNegociosHandler(Instance_OnNegociosEvent);

            ConectarMdsClient();

            try
            {
                if (!gCadastroBasico.ContainsKey(ativo) && !gAtivosInexistentes.Contains(ativo))
                {
                    CadastroPapelMdsInfo dadosCadastroPapel = ObterCadastroPapel(ativo);
                    if (dadosCadastroPapel != null)
                        gCadastroBasico.Add(ativo, dadosCadastroPapel);
                    else
                        gAtivosInexistentes.Add(ativo);
                }

                if (!gCotacao.ContainsKey(ativo) && !gAtivosInexistentes.Contains(ativo))
                {
                    gCotacao.Add(ativo, new MdsNegocios());

                    // Efetua assinatura de cotação do Instrumento
                    MdsAssinatura assinatura = new MdsAssinatura();
                    assinatura.Instrumento = ativo;
                    assinatura.Sinal = TipoSinal.CotacaoRapida;
                    MdsHttpClient.Instance.Assina(assinatura);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Erro assinatura negocio ativo[{0}]: {1}\r\n\r\n{2}",
                    ativo, ex.Message, ex.StackTrace));
            }
        }

        private void EfetuarAssinaturaAcompanhamentoOrdens(string cliente)
        {
            ConectarMdsClient();

            if (!gAssinaturaAcompanhamentoOrdens)
            {
                try
                {
                    gClienteInfo = ObterDadosCliente(cliente);
                    gClienteBovespaPadrao = cliente;

                    MdsAssinatura lAssinatura = new MdsAssinatura();

                    lAssinatura.CodigoCliente = new List<string>();
                    if (gClienteInfo == null)
                    {
                        lAssinatura.CodigoCliente.Add(cliente);
                    }
                    else
                    {
                        lAssinatura.CodigoCliente.Add(gClienteInfo.CodBovespa);
                        lAssinatura.CodigoCliente.Add(gClienteInfo.CodBMF);
                    }

                    lAssinatura.Acompanhamento = TipoAcompanhamento.AcompanhamentoOrdens;

                    lAssinatura.Sinal = TipoSinal.Acompanhamento;

                    MdsHttpClient.Instance.AssinarEventoAcompanhamento(string.Concat("Acompanhamento", 1), Instance_OnAcompanhamentoEvent);

                    MdsHttpClient.Instance.Assina(lAssinatura);

                    gAssinaturaAcompanhamentoOrdens = true;
                    gPosicaoOrdens.Clear();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Erro assinatura acompanhamento ordens cliente[{0}]: {1}\r\n\r\n{2}",
                        cliente, ex.Message, ex.StackTrace));
                }
            }
        }

        #endregion

        #region IDTExtensibility2 Interface

        public void OnAddInsUpdate(ref Array custom)
        {
            //GravaLogDebug("OnAddInsUpdate()");
        }

        public void OnBeginShutdown(ref Array custom)
        {
        }

        public void OnConnection(object Application, ext_ConnectMode ConnectMode, object AddInInst, ref Array custom)
        {
            //GravaLogDebug("OnConnection()");
            gInstanciaExcel = Application as Excel.Application;
            comRTDInfo = gInstanciaExcel.COMAddIns.Item("StockMarket.Excel2007").Object;
        }

        public void OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom)
        {
        }

        public void OnStartupComplete(ref Array custom)
        {
            //GravaLogDebug("OnStartupComplete()");
        }

        #endregion

        #region IFuncoes Interface

        public object SM_COTACAO(string ativoPropriedade)
        {
            object lRetorno = null;

            if (!string.IsNullOrEmpty(ativoPropriedade))
            {
                // Assume "ultimo preco" como propriedade padrão, se nao for informado nada junto ao instrumento
                if (!ativoPropriedade.Contains(".")) 
                    ativoPropriedade = ativoPropriedade + "." + PROPRIEDADE_ULTIMO_PRECO;

                string ativo = ativoPropriedade.Substring(0, ativoPropriedade.IndexOf('.') ).ToUpper();
                string propriedade = ativoPropriedade.Substring(ativoPropriedade.IndexOf('.') + 1).ToUpper();

                lock (gInstanciaExcel)
                {
                    lRetorno = gInstanciaExcel.WorksheetFunction.RTD("StockMarket.RTD", string.Empty, FUNCAO_COTACAO, "", ativo, propriedade);
                }
            }
            return lRetorno;
        }

        public object SM_COTACAO_LINHA(string ativoPropriedade)
        {
            object lRetorno = null;

            if (!string.IsNullOrEmpty(ativoPropriedade))
            {
                // Assume "ultimo preco" como propriedade padrão, se nao for informado nada junto ao instrumento
                if (!ativoPropriedade.Contains("."))
                    ativoPropriedade = ativoPropriedade + "." + PROPRIEDADE_ULTIMO_PRECO;

                string ativo = ativoPropriedade.Substring(0, ativoPropriedade.IndexOf('.')).ToUpper();
                string propriedade = ativoPropriedade.Substring(ativoPropriedade.IndexOf('.') + 1).ToUpper();

                lock (gInstanciaExcel)
                {
                    lRetorno = gInstanciaExcel.WorksheetFunction.RTD("StockMarket.RTD", string.Empty, FUNCAO_COTACAO_LINHA, "", ativo, propriedade);
                }
            }
            return lRetorno;


            /*
            object[] lRetorno = new object[22];     // o Excel mapeia essa array para colunas / linhas
            try
            {
                if (!string.IsNullOrEmpty(Instrumento))
                {
                    Instrumento = Instrumento.ToUpper();

                    //pela documentação, temos que o resultado dessa função deve ser:
                    // 
                    //     Papel           >> Nome do Instrumento
                    //     última          >> Ultima cotação enviada pelo MDS
                    //     Var (%)         >> Variação do Instrumento em relação a abertura.
                    //     Cor.Comp        >> Código da melhor corretora compradora
                    //     Vl.Comp         >> Valor da melhor oferta de compra
                    //     Cor.Venda       >> Código da melhor corretora vendedora
                    //     Vl.Venda        >> Valor da melhor oferta de venda
                    //     //  Abertura        >> Valor da abertura do dia
                    //     Mínima          >> Negócio mais barato do dia
                    //     Máxima          >> Negócio mais caro do dia.
                    //     //  Fech.Anterior   >> Valor do fechamento anterior.
                    //     N.Neg           >> Numero de negócios do dia para o instrumento
                    //     Volume          >> Volume gerado pelo instrumento
                    //     Qtd Teorica     >> Qtd Teorica durante leilao
                    //     Preco Teorico   >> Preco Teorico durante leilao
                    //     Data/Hora       >> Data e hora do último negócio realizado para o instrumento
                    // 
                    // 
                    // lRetorno[0] = Instrumento;
                    // lRetorno[1] = 24.77 + gRand.Next(-5, 5);
                    // lRetorno[2] = 0.78;
                    // lRetorno[3] = gRand.Next(27, 300).ToString();
                    // lRetorno[4] = 24.78;
                    // lRetorno[5] = gRand.Next(27, 300).ToString();
                    // lRetorno[6] = 24.79;
                    // lRetorno[7] = 24.6;
                    // lRetorno[8] = 24.79;
                    // lRetorno[9] = 1500 + gRand.Next(1, 20);
                    // lRetorno[10] = 44560 + gRand.Next(10, 500);
                    // lRetorno[11] = DateTime.Now;
                    // 

                    if (gCotacao.Count == 0)
                        MdsHttpClient.Instance.OnNegociosEvent += new MdsHttpClient.OnNegociosHandler(Instance_OnNegociosEvent);

                    try
                    {
                        ConectarMdsClient();

                        try
                        {
                            if (!gCotacao.ContainsKey(Instrumento))
                            {
                                // Efetua assinatura de cotação do Instrumento
                                MdsAssinatura lAssinatura = new MdsAssinatura();
                                lAssinatura.Instrumento = Instrumento;
                                lAssinatura.Sinal = TipoSinal.CotacaoRapida;
                                MdsHttpClient.Instance.Assina(lAssinatura);

                                gCotacao.Add(Instrumento, new MdsNegocios());

                                CadastroPapelMdsInfo dadosCadastroPapel = ObterCadastroPapel(Instrumento);
                                if (dadosCadastroPapel != null)
                                    gCadastroBasico.Add(Instrumento, dadosCadastroPapel);
                            }
                        }
                        catch (Exception ex2)
                        {
                            System.Windows.Forms.MessageBox.Show(string.Format("Erro ao assinar [{0}]: {1}\r\n\r\n{2}", Instrumento, ex2.Message, ex2.StackTrace));

                            return lRetorno;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Erro ao conectar: {1}\r\n\r\n{2}", ex.Message, ex.StackTrace));

                        return lRetorno;
                    }

                    lRetorno[0] = Instrumento;                                                                              // Papel
                    lRetorno[1] = ConverterParaDecimal(gCotacao[Instrumento].ng.pr);                                     // última
                    lRetorno[2] = ConverterParaDecimal(gCotacao[Instrumento].ng.sv + gCotacao[Instrumento].ng.v);     // Var (%)
                    lRetorno[3] = ConverterParaDecimal(gCotacao[Instrumento].ng.md);                                     // Média
                    lRetorno[4] = ConverterParaInt(gCotacao[Instrumento].ng.cp);                                         // Corr Compr
                    lRetorno[5] = ConverterParaInt(gCotacao[Instrumento].ng.mqc);                                        // Qtd Compr
                    lRetorno[6] = ConverterParaDecimal(gCotacao[Instrumento].ng.mc);                                     // Vl Compr
                    lRetorno[7] = ConverterParaDecimal(gCotacao[Instrumento].ng.mv);                                     // Vl Venda
                    lRetorno[8] = ConverterParaInt(gCotacao[Instrumento].ng.mqv);                                        // Qtd Venda
                    lRetorno[9] = ConverterParaInt(gCotacao[Instrumento].ng.vd);                                         // Corr Venda
                    lRetorno[10] = ConverterParaDecimal(gCotacao[Instrumento].ng.ab);                                    // Abertura
                    lRetorno[11] = ConverterParaDecimal(gCotacao[Instrumento].ng.mi);                                    // Mínima
                    lRetorno[12] = ConverterParaDecimal(gCotacao[Instrumento].ng.mx);                                    // Máxima
                    lRetorno[13] = ConverterParaDecimal(gCotacao[Instrumento].ng.fe);                                    // Fech.Anterior
                    lRetorno[14] = ConverterParaInt(gCotacao[Instrumento].ng.qa);                                        // Qtd Total
                    lRetorno[15] = ConverterParaInt(gCotacao[Instrumento].ng.n);                                         // N.Neg
                    lRetorno[16] = ConverterParaInt64(gCotacao[Instrumento].ng.vl);                                      // Volume
                    if (gCadastroBasico.ContainsKey(Instrumento) && gCotacao[Instrumento].ng.st.Equals("1"))
                    {
                        lRetorno[17] = ConverterParaInt(gCotacao[Instrumento].ng.qt);                                    // Qtd Teorica
                        lRetorno[18] = ConverterParaDecimal(gCotacao[Instrumento].ng.pt);                                // Preço Teorico
                    }
                    else
                    {
                        lRetorno[17] = 0;
                        lRetorno[18] = 0;
                    }

                    if (gCadastroBasico.ContainsKey(Instrumento) && (
                        gCadastroBasico[Instrumento].SegmentoMercado.Equals("04") ||
                        gCadastroBasico[Instrumento].SegmentoMercado.Equals("SOPT") ||
                        gCadastroBasico[Instrumento].SegmentoMercado.Equals("FOPT")))
                    {
                        lRetorno[19] = gCadastroBasico[Instrumento].PrecoExercicio;                                     // Preco Exercicio
                        lRetorno[20] = ((DateTime)gCadastroBasico[Instrumento].DataVencimento).ToString("dd/MM/yyyy");  // Data Venc
                    }
                    else
                    {
                        lRetorno[19] = 0;
                        lRetorno[20] = "";
                    }

                    if (gCotacao[Instrumento].ng.d != null)
                    {
                        lRetorno[21] = DateTime.ParseExact(gCotacao[Instrumento].ng.d, "yyyyMMdd", gCulture);            // Data/Hora

                        if (gCadastroBasico.ContainsKey(Instrumento) && gCotacao[Instrumento].ng.st.Equals("1"))
                        {
                            if (gCotacao[Instrumento].ng.ht != null)
                            {
                                lRetorno[21] = ((DateTime)lRetorno[21]).AddHours(Convert.ToDouble(gCotacao[Instrumento].ng.ht.ToString().Substring(8, 2)));
                                lRetorno[21] = ((DateTime)lRetorno[21]).AddMinutes(Convert.ToDouble(gCotacao[Instrumento].ng.ht.ToString().Substring(10, 2)));
                                lRetorno[21] = ((DateTime)lRetorno[21]).AddSeconds(Convert.ToDouble(gCotacao[Instrumento].ng.ht.ToString().Substring(12, 2)));
                            }
                        }
                        else
                        {
                            if (gCotacao[Instrumento].ng.h != null)
                            {
                                lRetorno[21] = ((DateTime)lRetorno[21]).AddHours(Convert.ToDouble(gCotacao[Instrumento].ng.h.ToString().Substring(0, 2)));
                                lRetorno[21] = ((DateTime)lRetorno[21]).AddMinutes(Convert.ToDouble(gCotacao[Instrumento].ng.h.ToString().Substring(2, 2)));
                                lRetorno[21] = ((DateTime)lRetorno[21]).AddSeconds(Convert.ToDouble(gCotacao[Instrumento].ng.h.ToString().Substring(4, 2)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string lMensagem = string.Format("Erro em SM_COTACAO_LINHA: {0}", ex.Message);
                Debug.WriteLine(lMensagem);
                //EventLogger.WriteFormat(EventLogEntryType.Error, lMensagem);
            }

            return lRetorno;
            */
        }

        public object SM_COTACAORAPIDA(string ativoPropriedade)
        {
            object lRetorno = null;

            if (!string.IsNullOrEmpty(ativoPropriedade))
            {
                // Assume "ultimo preco" como propriedade padrão, se nao for informado nada junto ao instrumento
                if (!ativoPropriedade.Contains("."))
                    ativoPropriedade = ativoPropriedade + "." + PROPRIEDADE_ULTIMO_PRECO;

                string ativo = ativoPropriedade.Substring(0, ativoPropriedade.IndexOf('.')).ToUpper();
                string propriedade = ativoPropriedade.Substring(ativoPropriedade.IndexOf('.') + 1).ToUpper();

                lock (gInstanciaExcel)
                {
                    lRetorno = gInstanciaExcel.WorksheetFunction.RTD("StockMarket.RTD", string.Empty, FUNCAO_COTACAO_RAPIDA, "", ativo, propriedade);
                }
            }
            return lRetorno;

            /*
            object[,] lRetorno = new object[10, 4];

            try
            {
                if (!string.IsNullOrEmpty(Instrumento))
                {
                    Instrumento = Instrumento.ToUpper();

                    //pela documentação, temos que o resultado dessa função deve ser:
                    //
                    //  Ultima       <valor>    Var(%)      <valor>
                    //  Compra       <valor>    Venda       <valor>
                    //  Qtd Compra   <valor>    Qtd Venda   <valor>
                    //  Corr Compra  <valor>    Corr Venda  <valor>
                    //  Max          <valor>    Min         <valor>
                    //  Aber         <valor>    Fech        <valor>
                    //  Media        <valor>    Qtd Total   <valor>
                    //  Nr. Neg      <valor>    Vol         <valor>
                    //  Qtd Teor     <valor>    Prc Teor    <valor>
                    //  Prc Exer     <valor>    Data Vencto <valor>
                    //

                    if (gCotacao.Count == 0)
                        MdsHttpClient.Instance.OnNegociosEvent += new MdsHttpClient.OnNegociosHandler(Instance_OnNegociosEvent);

                    try
                    {
                        ConectarMdsClient();

                        try
                        {
                            if (!gCotacao.ContainsKey(Instrumento))
                            {
                                // Efetua assinatura de cotação do Instrumento
                                MdsAssinatura lAssinatura = new MdsAssinatura();
                                lAssinatura.Instrumento = Instrumento;
                                lAssinatura.Sinal = TipoSinal.CotacaoRapida;
                                MdsHttpClient.Instance.Assina(lAssinatura);

                                gCotacao.Add(Instrumento, new MdsNegocios());

                                CadastroPapelMdsInfo dadosCadastroPapel = ObterCadastroPapel(Instrumento);
                                if (dadosCadastroPapel != null)
                                    gCadastroBasico.Add(Instrumento, dadosCadastroPapel);
                            }
                        }
                        catch (Exception ex2)
                        {
                            System.Windows.Forms.MessageBox.Show(string.Format("Erro ao assinar [{0}]: {1}\r\n\r\n{2}", Instrumento, ex2.Message, ex2.StackTrace));

                            return lRetorno;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Erro ao conectar: {1}\r\n\r\n{2}", ex.Message, ex.StackTrace));

                        return lRetorno;
                    }


                    // primeira linha:

                    lRetorno[0, 0] = "Última";
                    lRetorno[0, 1] = ConverterParaDecimal(gCotacao[Instrumento].ng.pr);
                    lRetorno[0, 2] = "Var(%)";
                    lRetorno[0, 3] = ConverterParaDecimal(gCotacao[Instrumento].ng.sv + gCotacao[Instrumento].ng.v);

                    // segunda linha:

                    lRetorno[1, 0] = "Compra";
                    lRetorno[1, 1] = ConverterParaDecimal(gCotacao[Instrumento].ng.mc);
                    lRetorno[1, 2] = "Venda";
                    lRetorno[1, 3] = ConverterParaDecimal(gCotacao[Instrumento].ng.mv);

                    // terceira linha:

                    lRetorno[2, 0] = "Qtd Compra";
                    lRetorno[2, 1] = ConverterParaInt(gCotacao[Instrumento].ng.mqc);
                    lRetorno[2, 2] = "Qtd Venda";
                    lRetorno[2, 3] = ConverterParaInt(gCotacao[Instrumento].ng.mqv);

                    // quarta linha:

                    lRetorno[3, 0] = "Corr Compra";
                    lRetorno[3, 1] = ConverterParaInt(gCotacao[Instrumento].ng.cp);
                    lRetorno[3, 2] = "Corr Venda";
                    lRetorno[3, 3] = ConverterParaInt(gCotacao[Instrumento].ng.vd);

                    // quinta linha:

                    lRetorno[4, 0] = "Max";
                    lRetorno[4, 1] = ConverterParaDecimal(gCotacao[Instrumento].ng.mx);
                    lRetorno[4, 2] = "Min";
                    lRetorno[4, 3] = ConverterParaDecimal(gCotacao[Instrumento].ng.mi);

                    // sexta linha:

                    lRetorno[5, 0] = "Aber";
                    lRetorno[5, 1] = ConverterParaDecimal(gCotacao[Instrumento].ng.ab);
                    lRetorno[5, 2] = "Fech";
                    lRetorno[5, 3] = ConverterParaDecimal(gCotacao[Instrumento].ng.fe);

                    // setima linha:

                    lRetorno[6, 0] = "Media";
                    lRetorno[6, 1] = ConverterParaDecimal(gCotacao[Instrumento].ng.md);
                    lRetorno[6, 2] = "Qtd Total";
                    lRetorno[6, 3] = ConverterParaInt(gCotacao[Instrumento].ng.qa);

                    // oitava linha:

                    lRetorno[7, 0] = "Nr. Neg";
                    lRetorno[7, 1] = ConverterParaInt(gCotacao[Instrumento].ng.n);
                    lRetorno[7, 2] = "Vol";
                    lRetorno[7, 3] = ConverterParaInt64(gCotacao[Instrumento].ng.vl);

                    // nona linha:

                    lRetorno[8, 0] = "Qtd Teor";
                    lRetorno[8, 2] = "Prc Teor";
                    if (gCadastroBasico.ContainsKey(Instrumento) && gCotacao[Instrumento].ng.st.Equals("1"))
                    {
                        lRetorno[8, 1] = ConverterParaInt(gCotacao[Instrumento].ng.qt);
                        lRetorno[8, 3] = ConverterParaDecimal(gCotacao[Instrumento].ng.pt);
                    }
                    else
                    {
                        lRetorno[8, 1] = 0;
                        lRetorno[8, 3] = 0;
                    }

                    // decima linha:

                    lRetorno[9, 0] = "Prc Exer";
                    lRetorno[9, 2] = "Data Vencto";
                    if (gCadastroBasico.ContainsKey(Instrumento) && (
                        gCadastroBasico[Instrumento].SegmentoMercado.Equals("04") ||
                        gCadastroBasico[Instrumento].SegmentoMercado.Equals("SOPT") ||
                        gCadastroBasico[Instrumento].SegmentoMercado.Equals("FOPT")))
                    {
                        lRetorno[9, 1] = gCadastroBasico[Instrumento].PrecoExercicio;
                        lRetorno[9, 3] = ((DateTime)gCadastroBasico[Instrumento].DataVencimento).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lRetorno[9, 1] = 0;
                        lRetorno[9, 3] = "";
                    }
                }
            }
            catch (Exception ex)
            {
                string lMensagem = string.Format("Erro em SM_COTACAORAPIDA: {0}", ex.Message);
                Debug.WriteLine(lMensagem);
                //EventLogger.WriteFormat(EventLogEntryType.Error, lMensagem);
            }

            return lRetorno;
            */
        }


        public object SM_LIVROOFERTAS(string ativoPropriedadeSentidoOcorrencia)
        {
            object lRetorno = null;

            if (!string.IsNullOrEmpty(ativoPropriedadeSentidoOcorrencia))
            {
                // Assume "preco topo de compra" como propriedade padrão, se nao for informado nada junto ao instrumento
                if (!ativoPropriedadeSentidoOcorrencia.Contains("."))
                    ativoPropriedadeSentidoOcorrencia = ativoPropriedadeSentidoOcorrencia + "." + PROPRIEDADE_OFERTA_PRECO + "." + PROPRIEDADE_OFERTA_SENTIDO_COMPRA + ".1";

                string[] itens = ativoPropriedadeSentidoOcorrencia.Split('.');
                string ativo = itens[0].ToUpper();
                string propriedade = itens[1].ToUpper();
                string sentido = (itens.Length < 3 ? "" : itens[2].ToUpper());
                string ocorrencia = (itens.Length < 3 ? "" : itens[3].ToUpper());

                lock (gInstanciaExcel)
                {
                    lRetorno = gInstanciaExcel.WorksheetFunction.RTD("StockMarket.RTD", string.Empty, FUNCAO_LIVRO_OFERTAS, "", ativo, propriedade, sentido, ocorrencia);
                }
            }
            return lRetorno;

            /*
            object[,] lRetorno = new object[10, 6];

            try
            {
                if (!string.IsNullOrEmpty(Instrumento))
                {
                    Instrumento = Instrumento.ToUpper();

                    if (gLivroDeOfertasCompras.Count == 0)
                        MdsHttpClient.Instance.OnTopoLivroOfertasEvent += new MdsHttpClient.OnTopoLivroOfertasHandler(Instance_OnTopoLivroOfertasEvent);

                    try
                    {
                        ConectarMdsClient();

                        try
                        {
                            if (!gLivroDeOfertasVendas.ContainsKey(Instrumento))
                                gLivroDeOfertasVendas.Add(Instrumento, new List<MdsOfertaRegistro>());

                            if (!gLivroDeOfertasCompras.ContainsKey(Instrumento))
                            {
                                gLivroDeOfertasCompras.Add(Instrumento, new List<MdsOfertaRegistro>());

                                MdsAssinatura lAssinatura = new MdsAssinatura();

                                lAssinatura.Instrumento = Instrumento;

                                lAssinatura.Sinal = TipoSinal.TopoLivroOfertas;

                                MdsHttpClient.Instance.Assina(lAssinatura);
                            }
                        }
                        catch (Exception ex2)
                        {
                            System.Windows.Forms.MessageBox.Show(string.Format("Erro ao assinar [{0}]: {1}\r\n\r\n{2}", Instrumento, ex2.Message, ex2.StackTrace));

                            return lRetorno;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Erro ao conectar: {1}\r\n\r\n{2}", ex.Message, ex.StackTrace));

                        return lRetorno;
                    }

                    object lPreco;

                    for (int a = 0; a < 10; a++)
                    {
                        if (gLivroDeOfertasCompras[Instrumento].Count > a)
                        {
                            lPreco = string.Format("{0}", gLivroDeOfertasCompras[Instrumento][a].preco).Trim();

                            if (lPreco.ToString().ToUpper() != "ABERTURA") lPreco = ConverterParaDecimal(lPreco);

                            lRetorno[a, 0] = ConverterParaInt(gLivroDeOfertasCompras[Instrumento][a].corretora);
                            lRetorno[a, 1] = ConverterParaInt(gLivroDeOfertasCompras[Instrumento][a].quantidade);
                            lRetorno[a, 2] = lPreco;

                            if (gLivroDeOfertasVendas[Instrumento].Count > a)
                            {
                                lPreco = string.Format("{0}", gLivroDeOfertasVendas[Instrumento][a].preco).Trim();

                                if (lPreco.ToString().ToUpper() != "ABERTURA") lPreco = ConverterParaDecimal(lPreco);

                                lRetorno[a, 3] = lPreco;
                                lRetorno[a, 4] = ConverterParaInt(gLivroDeOfertasVendas[Instrumento][a].quantidade);
                                lRetorno[a, 5] = ConverterParaInt(gLivroDeOfertasVendas[Instrumento][a].corretora);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string lMensagem = string.Format("Erro em SM_LIVROOFERTAS: {0}", ex.Message);
                Debug.WriteLine(lMensagem);
                //EventLogger.WriteFormat(EventLogEntryType.Error, lMensagem);
            }

            return lRetorno;
            */
        }

        public object SM_POSICAO(string clienteAtivoPropriedade)
        {
            object lRetorno = null;

            if (!string.IsNullOrEmpty(clienteAtivoPropriedade))
            {
                string[] parametros = clienteAtivoPropriedade.Split('.');
                string cliente = parametros[0];
                string ativo = parametros[1];
                string propriedade = parametros[2];

                if (cliente == null || string.IsNullOrEmpty(cliente) ||
                    ativo == null || string.IsNullOrEmpty(ativo) ||
                    propriedade == null || string.IsNullOrEmpty(propriedade))
                {
                    return lRetorno;
                }

                lock (gInstanciaExcel)
                {
                    lRetorno = gInstanciaExcel.WorksheetFunction.RTD("StockMarket.RTD", string.Empty, FUNCAO_POSICAO, cliente, ativo, propriedade);
                }
            }
            return lRetorno;

            /*
            object lRetorno = null;

            try
            {
                string[] parametros = clienteInstrumentoPropriedade.Split('.');
                string cliente = parametros[0];
                string instrumento = parametros[1];
                string propriedade = parametros[2];

                if (cliente == null || string.IsNullOrEmpty(cliente) ||
                    instrumento == null || string.IsNullOrEmpty(instrumento) ||
                    propriedade == null || string.IsNullOrEmpty(propriedade))
                {
                    return lRetorno;
                }

                ConectarMdsClient();

                if (!gAssinaturaAcompanhamentoOrdens)
                {
                    try
                    {
                        gClienteInfo = ObterDadosCliente(cliente);
                        gClienteBovespaPadrao = cliente;

                        MdsAssinatura lAssinatura = new MdsAssinatura();

                        lAssinatura.CodigoCliente = new List<string>();
                        if (gClienteInfo == null)
                        {
                            lAssinatura.CodigoCliente.Add(cliente);
                        }
                        else
                        {
                            lAssinatura.CodigoCliente.Add(gClienteInfo.CodBovespa);
                            lAssinatura.CodigoCliente.Add(gClienteInfo.CodBMF);
                        }

                        lAssinatura.Acompanhamento = TipoAcompanhamento.AcompanhamentoOrdens;

                        lAssinatura.Sinal = TipoSinal.Acompanhamento;

                        MdsHttpClient.Instance.AssinarEventoAcompanhamento(string.Concat("Acompanhamento", 1), Instance_OnAcompanhamentoEvent);

                        MdsHttpClient.Instance.Assina(lAssinatura);

                        gAssinaturaAcompanhamentoOrdens = true;
                        gPosicaoOrdens.Clear();

                        //EventLogger.WriteFormat(EventLogEntryType.Information, "Cliente[" + cliente + "] assinado!");
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Erro ao assinar [{0}]: {1}\r\n\r\n{2}", cliente, ex.Message, ex.StackTrace));
                        return lRetorno;
                    }
                }

                //ObterPosicaoNet(cliente);

                string chave = cliente + "." + instrumento;
                if (!gPosicaoOrdens.ContainsKey(chave))
                    return lRetorno;

                propriedade = propriedade.ToUpper();
                switch (propriedade)
                {
                    case "CLIENTE":
                        lRetorno = cliente;
                        break;
                    case "PAPEL":
                        lRetorno = instrumento;
                        break;
                    case "QTDEXECCPA":
                        lRetorno = gPosicaoOrdens[chave].qtdeExecCompra;
                        break;
                    case "QTDEXECVDA":
                        lRetorno = gPosicaoOrdens[chave].qtdeExecVenda;
                        break;
                    case "NETEXEC":
                        lRetorno = gPosicaoOrdens[chave].NetExec;
                        break;
                    case "PRMEDEXECCPA":
                        lRetorno = gPosicaoOrdens[chave].precoMedioExecCompra;
                        break;
                    case "PRMEDEXECVDA":
                        lRetorno = gPosicaoOrdens[chave].precoMedioExecVenda;
                        break;
                    case "QTDABCPA":
                        lRetorno = gPosicaoOrdens[chave].qtdeAbertCompra;
                        break;
                    case "QTDABVDA":
                        lRetorno = gPosicaoOrdens[chave].qtdeAbertVenda;
                        break;
                    case "NETAB":
                        lRetorno = gPosicaoOrdens[chave].NetAbert;
                        break;
                    case "PRMEDABCPA":
                        lRetorno = gPosicaoOrdens[chave].precoMedioAbertCompra;
                        break;
                    case "PRMEDABVDA":
                        lRetorno = gPosicaoOrdens[chave].precoMedioAbertVenda;
                        break;
                    case "VLFINNETEXEC":
                        lRetorno = gPosicaoOrdens[chave].volumeFinNetExec;
                        break;
                    case "VLFINNETAB":
                        lRetorno = gPosicaoOrdens[chave].volumeFinNetAbert;
                        break;
                    default:
                        lRetorno = instrumento;
                        break;
                }
            }
            catch (Exception ex)
            {
                string lMensagem = string.Format("Erro em SM_POSICAO: {0}", ex.Message);
                Debug.WriteLine(lMensagem);
                //EventLogger.WriteFormat(EventLogEntryType.Error, lMensagem);
            }

            return lRetorno;
            */
        }

        public object SM_POSICAO_NET(string clientePropriedadeOcorrencia)
        {
            object lRetorno = null;

            if (!string.IsNullOrEmpty(clientePropriedadeOcorrencia))
            {
                // Assume "cliente da 1a.ocorrencia" como propriedade padrão, se nao for informado nada junto ao cliente
                if (!clientePropriedadeOcorrencia.Contains("."))
                    clientePropriedadeOcorrencia = clientePropriedadeOcorrencia + "." + PROPRIEDADE_POSICAONET_CLIENTE + ".1";

                string[] itens = clientePropriedadeOcorrencia.Split('.');
                string cliente = itens[0].ToUpper();
                string propriedade = itens[1].ToUpper();
                string ocorrencia = itens[2];

                lock (gInstanciaExcel)
                {
                    lRetorno = gInstanciaExcel.WorksheetFunction.RTD("StockMarket.RTD", string.Empty, FUNCAO_POSICAO_NET, cliente, "", propriedade, "", ocorrencia);
                }
            }
            return lRetorno;


            /*
            int totalLinhas = 0;

            if (gPosicaoOrdens == null || gPosicaoOrdens.Count == 0)
                totalLinhas = 1;
            else
                totalLinhas = gPosicaoOrdens.Count;

            object[,] lRetorno = new object[totalLinhas, 15];

            try
            {
                ConectarMdsClient();

                if (!gAssinaturaAcompanhamentoOrdens)
                {
                    try
                    {
                        gClienteInfo = ObterDadosCliente(cliente);
                        gClienteBovespaPadrao = cliente;

                        MdsAssinatura lAssinatura = new MdsAssinatura();

                        lAssinatura.CodigoCliente = new List<string>();
                        if (gClienteInfo == null)
                        {
                            lAssinatura.CodigoCliente.Add(cliente);
                        }
                        else
                        {
                            lAssinatura.CodigoCliente.Add(gClienteInfo.CodBovespa);
                            lAssinatura.CodigoCliente.Add(gClienteInfo.CodBMF);
                        }

                        lAssinatura.Acompanhamento = TipoAcompanhamento.AcompanhamentoOrdens;

                        lAssinatura.Sinal = TipoSinal.Acompanhamento;

                        MdsHttpClient.Instance.AssinarEventoAcompanhamento(string.Concat("Acompanhamento", 1), Instance_OnAcompanhamentoEvent);

                        MdsHttpClient.Instance.Assina(lAssinatura);

                        gAssinaturaAcompanhamentoOrdens = true;
                        gPosicaoOrdens.Clear();

                        //EventLogger.WriteFormat(EventLogEntryType.Information, "Cliente[" + cliente + "] assinado!");

                        //ObterPosicaoNet(cliente);
                        //OnAtualizarPosicaoNet(gPosicaoOrdens.Count, cliente);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Erro ao assinar [{0}]: {1}\r\n\r\n{2}", cliente, ex.Message, ex.StackTrace));
                        return lRetorno;
                    }
                }


                //EventLogger.WriteFormat(EventLogEntryType.Information, "Populando dados: " + gPosicaoOrdens.Count + " ocorrencias");

                if (gPosicaoOrdens.Count == 0)
                {
                    lRetorno[0, 0] = " ";
                    lRetorno[0, 1] = "";
                    lRetorno[0, 2] = "";
                    lRetorno[0, 3] = "";
                    lRetorno[0, 4] = "";
                    lRetorno[0, 5] = "";
                    lRetorno[0, 6] = "";
                    lRetorno[0, 7] = "";
                    lRetorno[0, 8] = "";
                    lRetorno[0, 9] = "";
                    lRetorno[0, 10] = "";
                    lRetorno[0, 11] = "";
                    lRetorno[0, 12] = "";
                    lRetorno[0, 13] = "";
                    lRetorno[0, 14] = 1;
                }
                else
                {
                    int linha = 0;
                    foreach (KeyValuePair<string, PosicaoNetInfo> posicao in gPosicaoOrdens)
                    {
                        string[] clienteAtivo = posicao.Key.Split('.');
                        lRetorno[linha, 0] = (linha == 0 ? " " : "") + clienteAtivo[0];
                        lRetorno[linha, 1] = clienteAtivo[1];
                        lRetorno[linha, 2] = posicao.Value.qtdeExecCompra;
                        lRetorno[linha, 3] = posicao.Value.qtdeExecVenda;
                        lRetorno[linha, 4] = posicao.Value.NetExec;
                        lRetorno[linha, 5] = posicao.Value.precoMedioExecCompra;
                        lRetorno[linha, 6] = posicao.Value.precoMedioExecVenda;
                        lRetorno[linha, 7] = posicao.Value.qtdeAbertCompra;
                        lRetorno[linha, 8] = posicao.Value.qtdeAbertVenda;
                        lRetorno[linha, 9] = posicao.Value.NetAbert;
                        lRetorno[linha, 10] = posicao.Value.precoMedioAbertCompra;
                        lRetorno[linha, 11] = posicao.Value.precoMedioAbertVenda;
                        lRetorno[linha, 12] = posicao.Value.volumeFinNetAbert;
                        lRetorno[linha, 13] = posicao.Value.volumeFinNetExec;
                        lRetorno[linha, 14] = totalLinhas;
                        linha++;
                    }
                }
            }
            catch (Exception ex)
            {
                string lMensagem = string.Format("Erro em SM_POSICAO_NET: {0}", ex.Message);
                Debug.WriteLine(lMensagem);
                //EventLogger.WriteFormat(EventLogEntryType.Error, lMensagem);
            }

            return lRetorno;
            */
        }

        #endregion

        #region IRtdServer Interface

        public int ServerStart(IRTDUpdateEvent callback)
        {
            gCallback = callback;

            IniciarTimer();

            return 1;
        }

        public void ServerTerminate()
        {
            if (null != gTimer)
            {
                gTimer.Dispose();
                gTimer = null;
            }
        }

        public object ConnectData(int topicId, ref object[] topics, ref bool newValue)
        {
            newValue = true;

            string funcao = (string)topics[0];
            string cliente = (string)topics[1];
            string ativo = (string)topics[2];
            string propriedade = (topics.Length == 3 ? "" : (string)topics[3]);
            string sentido = (topics.Length <= 4 ? "" : (string)topics[4]);
            string ocorrencia = (topics.Length <= 4 ? "" : (string)topics[5]);
            int numOcorr;
            bool isNumero = int.TryParse(ocorrencia, out numOcorr);

            if (!gTopicsExcel.ContainsKey(topicId) && propriedade != null)
            {
                TopicExcelInfo topic = new TopicExcelInfo();
                topic.funcao = funcao;
                topic.cliente = cliente;
                topic.ativo = ativo;
                topic.propriedade = propriedade;
                topic.sentido = sentido;
                topic.ocorrencia = (isNumero ? numOcorr : 1);
                topic.valorAnterior = null;
                gTopicsExcel.Add(topicId, topic);
                //GravaLogDebug(string.Format("ConnectData() funcao[{0}] ativo[{1}] propriedade[{2}]", 
                //    topic.funcao, topic.ativo, topic.propriedade));
            }

            AtualizaRtdInfo();

            gTimer.Start();

            return "0";
        }

        public void DisconnectData(int topicId)
        {
            gTimer.Stop();

            if (gTopicsExcel.ContainsKey(topicId))
                gTopicsExcel.Remove(topicId);

            gTimer.Start();
        }

        public Array RefreshData(ref int topicCount)
        {
            object[,] data = new object[2, gTopicsExcel.Count]; 

            object valor;
            string funcao;
            string cliente;
            string ativo;
            string propriedade;
            string sentido;
            int ocorrencia;
            object valorNovo = null;
            object valorAnterior;

            AtualizaRtdInfo();

            if (!gRtdInfo.usuarioLogado)
                return null;

            int index = 0;
            foreach (int topicId in gTopicsExcel.Keys)
            {
                valor = null;
                funcao = gTopicsExcel[topicId].funcao;
                cliente = gTopicsExcel[topicId].cliente;
                ativo = gTopicsExcel[topicId].ativo;
                propriedade = gTopicsExcel[topicId].propriedade;
                sentido = gTopicsExcel[topicId].sentido;
                ocorrencia = (gTopicsExcel[topicId].ocorrencia < 1 ? 1 : gTopicsExcel[topicId].ocorrencia);
                valorAnterior = gTopicsExcel[topicId].valorAnterior;

                //GravaLogDebug(string.Format("RefreshData({0}, {1}, {2}, {3}, {4})", funcao, cliente, ativo, propriedade, valorAnterior));

                switch (funcao)
                {
                    case FUNCAO_POSICAO:
                    case FUNCAO_POSICAO_NET:
                        if (!gRtdInfo.usuario.Equals(cliente))
                        {
                            valorNovo = null;
                            break;
                        }

                        EfetuarAssinaturaAcompanhamentoOrdens(cliente);

                        string clienteAtivo = null;
                        PosicaoNetInfo posicaoNet = null;

                        if (funcao.Equals(FUNCAO_POSICAO_NET))
                        {
                            if (gPosicaoOrdens.Count == 0)
                            {
                                valorNovo = null;
                                break;
                            }

                            int numItem = 1;
                            Dictionary<string, PosicaoNetInfo>.Enumerator itens = gPosicaoOrdens.GetEnumerator();
                            while (itens.MoveNext())
                            {
                                if (numItem == ocorrencia)
                                {
                                    clienteAtivo = itens.Current.Key;
                                    posicaoNet = itens.Current.Value;
                                    break;
                                }
                                numItem++;
                            }
                        }
                        else
                        {
                            if (gPosicaoOrdens.ContainsKey(cliente + "." + ativo))
                            {
                                clienteAtivo = cliente + "." + ativo;
                                posicaoNet = gPosicaoOrdens[clienteAtivo];
                            }
                        }

                        if (clienteAtivo == null)
                        {
                            valorNovo = null;
                            break;
                        }

                        switch (propriedade)
                        {
                            case PROPRIEDADE_POSICAONET_CLIENTE:
                                valorNovo = (ocorrencia == 1 ? " " : "") + clienteAtivo.Substring(0, clienteAtivo.IndexOf('.'));
                                break;
                            case PROPRIEDADE_POSICAONET_ATIVO:
                                valorNovo = clienteAtivo.Substring(clienteAtivo.IndexOf('.') + 1).ToUpper();
                                break;
                            case PROPRIEDADE_POSICAONET_QTD_EXEC_COMPRA:
                                valorNovo = posicaoNet.qtdeExecCompra;
                                break;
                            case PROPRIEDADE_POSICAONET_QTD_EXEC_VENDA:
                                valorNovo = posicaoNet.qtdeExecVenda;
                                break;
                            case PROPRIEDADE_POSICAONET_NET_EXEC:
                                valorNovo = posicaoNet.NetExec;
                                break;
                            case PROPRIEDADE_POSICAONET_PRECO_MEDIO_EXEC_COMPRA:
                                valorNovo = posicaoNet.precoMedioExecCompra;
                                break;
                            case PROPRIEDADE_POSICAONET_PRECO_MEDIO_EXEC_VENDA:
                                valorNovo = posicaoNet.precoMedioExecVenda;
                                break;
                            case PROPRIEDADE_POSICAONET_QTD_ABERTA_COMPRA:
                                valorNovo = posicaoNet.qtdeAbertCompra;
                                break;
                            case PROPRIEDADE_POSICAONET_QTD_ABERTA_VENDA:
                                valorNovo = posicaoNet.qtdeAbertVenda;
                                break;
                            case PROPRIEDADE_POSICAONET_NET_ABERTA:
                                valorNovo = posicaoNet.NetAbert;
                                break;
                            case PROPRIEDADE_POSICAONET_PRECO_MEDIO_ABERTA_COMPRA:
                                valorNovo = posicaoNet.precoMedioAbertCompra;
                                break;
                            case PROPRIEDADE_POSICAONET_PRECO_MEDIO_ABERTA_VENDA:
                                valorNovo = posicaoNet.precoMedioAbertVenda;
                                break;
                            case PROPRIEDADE_POSICAONET_VOLUMEFIN_NET_ABERTA:
                                valorNovo = posicaoNet.volumeFinNetAbert;
                                break;
                            case PROPRIEDADE_POSICAONET_VOLUMEFIN_NET_EXEC:
                                valorNovo = posicaoNet.volumeFinNetExec;
                                break;
                            case PROPRIEDADE_POSICAONET_TOTAL_REGISTROS:
                                valorNovo = gPosicaoOrdens.Count;
                                comRTDInfo.qtdRegistrosPosicaoNet = gPosicaoOrdens.Count;
                                break;
                            default:
                                valorNovo = clienteAtivo.Substring(0, clienteAtivo.IndexOf('.'));
                                break;
                        }
                        break;

                    case FUNCAO_COTACAO:
                    case FUNCAO_COTACAO_LINHA:
                    case FUNCAO_COTACAO_RAPIDA:

                        EfetuarAssinaturaNegocio(ativo);

                        if (gCotacao.ContainsKey(ativo) && !gAtivosInexistentes.Contains(ativo) && gCotacao[ativo].ng != null)
                        {
                            lock (gCotacao[ativo])
                            {
                                switch (propriedade)
                                {
                                    case PROPRIEDADE_ULTIMO_PRECO:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.pr);
                                        break;
                                    case PROPRIEDADE_VARIACAO:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.sv + gCotacao[ativo].ng.v);
                                        break;
                                    case PROPRIEDADE_PRECO_MEDIO:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.md);
                                        break;
                                    case PROPRIEDADE_CORRETORA_COMPRA:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.cp);
                                        break;
                                    case PROPRIEDADE_QUANTIDADE_COMPRA:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.mqc);
                                        break;
                                    case PROPRIEDADE_PRECO_COMPRA:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.mc);
                                        break;
                                    case PROPRIEDADE_PRECO_VENDA:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.mv);
                                        break;
                                    case PROPRIEDADE_QUANTIDADE_VENDA:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.mqv);
                                        break;
                                    case PROPRIEDADE_CORRETORA_VENDA:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.vd);
                                        break;
                                    case PROPRIEDADE_PRECO_ABERTURA:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.ab);
                                        break;
                                    case PROPRIEDADE_PRECO_MAXIMO:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.mx);
                                        break;
                                    case PROPRIEDADE_PRECO_MINIMO:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.mi);
                                        break;
                                    case PROPRIEDADE_PRECO_FECHAMENTO:
                                        valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.fe);
                                        break;
                                    case PROPRIEDADE_QUANTIDADE_TOTAL_PAPEIS:
                                        valorNovo = ConverterParaInt64(gCotacao[ativo].ng.qa);
                                        break;
                                    case PROPRIEDADE_QUANTIDADE_NEGOCIOS:
                                        valorNovo = ConverterParaInt(gCotacao[ativo].ng.n);
                                        break;
                                    case PROPRIEDADE_INSTRUMENTO:
                                        valorNovo = ativo;
                                        break;
                                    case PROPRIEDADE_VOLUME:
                                        valorNovo = ConverterParaInt64(gCotacao[ativo].ng.vl);
                                        break;
                                    case PROPRIEDADE_ESTADO:
                                        valorNovo = ConverterParaInt(gCotacao[ativo].ng.st);
                                        break;
                                    case PROPRIEDADE_QUANTIDADE_TEORICA:
                                        if (gCadastroBasico.ContainsKey(ativo) && gCotacao[ativo].ng.st != null && gCotacao[ativo].ng.st.Equals("1"))
                                            valorNovo = ConverterParaInt(gCotacao[ativo].ng.qt);
                                        else
                                            valorNovo = 0;
                                        break;
                                    case PROPRIEDADE_PRECO_TEORICO:
                                        if (gCadastroBasico.ContainsKey(ativo) && gCotacao[ativo].ng.st != null && gCotacao[ativo].ng.st.Equals("1"))
                                            valorNovo = ConverterParaDecimal(gCotacao[ativo].ng.pt);
                                        else
                                            valorNovo = 0;
                                        break;
                                    case PROPRIEDADE_PRECO_EXERCICIO:
                                        if (gCadastroBasico.ContainsKey(ativo) && (
                                                gCadastroBasico[ativo].SegmentoMercado.Equals("04") ||
                                                gCadastroBasico[ativo].SegmentoMercado.Equals("SOPT") ||
                                                gCadastroBasico[ativo].SegmentoMercado.Equals("FOPT")))
                                            valorNovo = gCadastroBasico[ativo].PrecoExercicio;
                                        else
                                            valorNovo = 0;
                                        break;
                                    case PROPRIEDADE_DATA_VENCIMENTO:
                                        if (gCadastroBasico.ContainsKey(ativo) && (
                                                gCadastroBasico[ativo].SegmentoMercado.Equals("04") ||
                                                gCadastroBasico[ativo].SegmentoMercado.Equals("SOPT") ||
                                                gCadastroBasico[ativo].SegmentoMercado.Equals("FOPT")))
                                            valorNovo = ((DateTime)gCadastroBasico[ativo].DataVencimento).ToString("dd/MM/yyyy");
                                        else
                                            valorNovo = "";
                                        break;
                                    case PROPRIEDADE_HORA:
                                        if (gCotacao[ativo].ng.d == null || gCotacao[ativo].ng.d.Equals("00000000"))
                                        {
                                            valorNovo = DateTime.MinValue.AddHours(0).AddMinutes(0).AddSeconds(0);
                                        }
                                        else
                                        {
                                            valorNovo = DateTime.ParseExact(gCotacao[ativo].ng.d, "yyyyMMdd", gCulture);

                                            if (gCadastroBasico.ContainsKey(ativo) && gCotacao[ativo].ng.st != null && gCotacao[ativo].ng.st.Equals("1"))
                                            {
                                                if (gCotacao[ativo].ng.ht != null && !gCotacao[ativo].ng.ht.Equals("000000000"))
                                                {
                                                    valorNovo = ((DateTime)valorNovo).AddHours(Convert.ToDouble(gCotacao[ativo].ng.ht.ToString().Substring(8, 2)));
                                                    valorNovo = ((DateTime)valorNovo).AddMinutes(Convert.ToDouble(gCotacao[ativo].ng.ht.ToString().Substring(10, 2)));
                                                    valorNovo = ((DateTime)valorNovo).AddSeconds(Convert.ToDouble(gCotacao[ativo].ng.ht.ToString().Substring(12, 2)));
                                                }
                                            }
                                            else
                                            {
                                                if (gCotacao[ativo].ng.h != null && !gCotacao[ativo].ng.h.Equals("000000000"))
                                                {
                                                    valorNovo = ((DateTime)valorNovo).AddHours(Convert.ToDouble(gCotacao[ativo].ng.h.ToString().Substring(0, 2)));
                                                    valorNovo = ((DateTime)valorNovo).AddMinutes(Convert.ToDouble(gCotacao[ativo].ng.h.ToString().Substring(2, 2)));
                                                    valorNovo = ((DateTime)valorNovo).AddSeconds(Convert.ToDouble(gCotacao[ativo].ng.h.ToString().Substring(4, 2)));
                                                }
                                            }
                                        }
                                        break;

                                    default:
                                        valorNovo = ativo;
                                        break;
                                }
                            }
                        }
                        else if (gAtivosInexistentes.Contains(ativo))
                        {
                            valorNovo = null;
                        }
                        break;

                    case FUNCAO_LIVRO_OFERTAS:
                        EfetuarAssinaturaLivroOfertas(ativo);
                        if (!gAtivosInexistentes.Contains(ativo))
                        {
                            switch (sentido)
                            {
                                case PROPRIEDADE_OFERTA_SENTIDO_COMPRA:
                                    List<MdsOfertaRegistro> ofertasCompra = gLivroCompra[ativo];
                                    lock (ofertasCompra)
                                    {
                                        switch (propriedade)
                                        {
                                            case PROPRIEDADE_OFERTA_PRECO:
                                                valorNovo = (ofertasCompra.Count < ocorrencia ? 0 : ConverterPrecoParaDecimal(ofertasCompra[ocorrencia-1].preco));
                                                break;
                                            case PROPRIEDADE_OFERTA_CORRETORA:
                                                valorNovo = (ofertasCompra.Count < ocorrencia ? 0 : ConverterParaDecimal(ofertasCompra[ocorrencia-1].corretora));
                                                break;
                                            case PROPRIEDADE_OFERTA_QUANTIDADE:
                                                valorNovo = (ofertasCompra.Count < ocorrencia ? 0 : ConverterParaDecimal(ofertasCompra[ocorrencia-1].quantidade));
                                                break;
                                            default:
                                                valorNovo = 0;
                                                break;
                                        }
                                    }
                                    break;
                                case PROPRIEDADE_OFERTA_SENTIDO_VENDA:
                                    List<MdsOfertaRegistro> ofertasVenda = gLivroVenda[ativo];
                                    lock (ofertasVenda)
                                    {
                                        switch (propriedade)
                                        {
                                            case PROPRIEDADE_OFERTA_PRECO:
                                                valorNovo = (ofertasVenda.Count < ocorrencia ? 0 : ConverterPrecoParaDecimal(ofertasVenda[ocorrencia-1].preco));
                                                break;
                                            case PROPRIEDADE_OFERTA_CORRETORA:
                                                valorNovo = (ofertasVenda.Count < ocorrencia ? 0 : ConverterParaDecimal(ofertasVenda[ocorrencia-1].corretora));
                                                break;
                                            case PROPRIEDADE_OFERTA_QUANTIDADE:
                                                valorNovo = (ofertasVenda.Count < ocorrencia ? 0 : ConverterParaDecimal(ofertasVenda[ocorrencia-1].quantidade));
                                                break;
                                            default:
                                                valorNovo = 0;
                                                break;
                                        }
                                    }
                                    break;
                                default:
                                    if (propriedade.Equals(PROPRIEDADE_OFERTA_ATIVO))
                                        valorNovo = "Livro de Ofertas - " + ativo;
                                    break;
                            }
                        }
                        break;
                }


                // Apenas atualiza as celulas do Excel que sofreram atualizacao de valores
                if (valorAnterior == null || !valorNovo.Equals(valorAnterior))
                {
                    //GravaLogDebug(string.Format("RefreshData ATUALIZA ({0}, {1}, {2}, {3}, {4}, {5})", ativo, propriedade, sentido, ocorrencia, valorAnterior, valorNovo));
                    gTopicsExcel[topicId].valorAnterior = valorNovo;
                    valor = valorNovo;

                    data[0, index] = topicId;
                    data[1, index++] = valor;
                }
            }
            topicCount = index;

            gTimer.Start();

            return data;
        }

        // Retorna 1 no Heartbeat() para indicar sucesso da conexão RTD no Excel 
        public int Heartbeat()
        {
            return 1;
        }

        private void TimerEventHandler(object sender, EventArgs e)
        {
            //gTimer.Stop();
            gCallback.UpdateNotify();
        }

        private void AtualizaRtdInfo()
        {
            if (comRTDInfo != null && comRTDInfo.ultimaAtualizacao != null && comRTDInfo.ultimaAtualizacao > gUltimaAtualizacao)
            {
                gUltimaAtualizacao = comRTDInfo.ultimaAtualizacao;

                gRtdInfo.ultimaAtualizacao = comRTDInfo.ultimaAtualizacao;
                gRtdInfo.usuarioLogado = comRTDInfo.usuarioLogado;
                gRtdInfo.usuario = comRTDInfo.usuario;
                gRtdInfo.acao = comRTDInfo.acao;
                gRtdInfo.parametro = comRTDInfo.parametro;

                if (gRtdInfo.acao == null)
                    return;

                //GravaLogDebug(string.Format("UsuarioLogado[{0}] usuario[{1}] acao[{2}] parametro[{3}] ultimaAtualizacao[{4}]",
                //    gRtdInfo.usuarioLogado, gRtdInfo.usuario, gRtdInfo.acao, gRtdInfo.parametro, gRtdInfo.ultimaAtualizacao));

                if (gRtdInfo.acao.Equals(ACAO_AJUSTAR_FREQUENCIA))
                {
                    gTimer.Interval = Convert.ToInt32(gRtdInfo.parametro);
                    //GravaLogDebug(string.Format("Intervalo atualizado [{0}]", gTimer.Interval));
                }
            }
        }

        #endregion

        #region Event Handlers

        private void Instance_OnNegociosEvent(object sender, MdsNegociosEventArgs e)
        {
            try
            {
                string instrumento = e.negocios.cb.i.ToUpper();

                if (gCotacao.ContainsKey(instrumento))
                {
                    lock (gCotacao[instrumento])
                    {
                        gCotacao[instrumento] = e.negocios;
                    }
                }
            }
            catch (Exception ex)
            {
                string lMensagem = string.Format("Erro em Instance_OnNegociosEvent: {0}", ex.Message);
                Debug.WriteLine(lMensagem);
                //EventLogger.WriteFormat(EventLogEntryType.Error, lMensagem);
            }
        }

        private void Instance_OnOfertasEvent(object sender, MdsOfertasEventArgs e)
        {
            string ativo = e.ofertas.cb.i;

            List<MdsOfertaRegistro> livroCompra = gLivroCompra[ativo];
            lock (livroCompra)
            {
                foreach (MdsOfertasOcorrencia ocorrencia in e.ofertas.oc)
                {
                    if (int.Parse(ocorrencia.ac) == 2 || int.Parse(ocorrencia.ac) == 3)
                    {
                        livroCompra.RemoveAt(int.Parse(ocorrencia.ps));
                    }
                    if (int.Parse(ocorrencia.ac) == 1 || int.Parse(ocorrencia.ac) == 2)
                    {
                        MdsOfertaRegistro registro = new MdsOfertaRegistro(ocorrencia.ct, ocorrencia.pr, ocorrencia.qt);
                        livroCompra.Insert(int.Parse(ocorrencia.ps), registro);
                    }
                }
            }

            List<MdsOfertaRegistro> livroVenda = gLivroVenda[ativo];
            lock (livroVenda)
            {
                foreach (MdsOfertasOcorrencia ocorrencia in e.ofertas.ov)
                {
                    if (int.Parse(ocorrencia.ac) == 2 || int.Parse(ocorrencia.ac) == 3)
                    {
                        livroVenda.RemoveAt(int.Parse(ocorrencia.ps));
                    }
                    if (int.Parse(ocorrencia.ac) == 1 || int.Parse(ocorrencia.ac) == 2)
                    {
                        MdsOfertaRegistro registro = new MdsOfertaRegistro(ocorrencia.ct, ocorrencia.pr, ocorrencia.qt);
                        livroVenda.Insert(int.Parse(ocorrencia.ps), registro);
                    }
                }
            }
        }


        private void Instance_OnAcompanhamentoEvent(object sender, MdsAcompanhamentoOrdensEventArgs e)
        {
            try
            {
                if (!e.ordem.OrdStatus.Equals(OrdemStatusEnum.REJEITADA))
                {
                    //GravaLogDebug(string.Format("TransactionTime[{0}] Ativo[{1}] OrdStatus[{2}] ClOrdId[{3}] Cliente[{4}]",
                    //    e.ordem.TransactTime.ToString(), e.ordem.Symbol, e.ordem.OrdStatus, e.ordem.ClOrdID, e.ordem.Account.ToString()));

                    Posicao pos = null;
                    if (e.ordem.Acompanhamentos.Count != 0)
                    {
                        String Instrumento = RetornarCodigoPapel(e.ordem.Symbol.ToString()).ToString().Trim();

                        if (ListaPosicoes.ContainsKey(e.ordem.Account.ToString()))
                        {
                            if (ListaPosicoes[e.ordem.Account.ToString()].ContainsKey(Instrumento))
                            {
                                pos = ListaPosicoes[e.ordem.Account.ToString()][Instrumento].Add((OrdemInfo)e.ordem);
                            }
                            else
                            {
                                Acompanhamento acompanhamento = new Acompanhamento();
                                acompanhamento.Instrumento = Instrumento;
                                pos = acompanhamento.Add(e.ordem);
                                ListaPosicoes[e.ordem.Account.ToString()].Add(Instrumento, acompanhamento);
                            }
                        }
                        else
                        {
                            Acompanhamento acompanhamento = new Acompanhamento();

                            acompanhamento.Instrumento = Instrumento;
                            pos = acompanhamento.Add(e.ordem);

                            Dictionary<System.String, Acompanhamento> ocorrencias = new Dictionary<string, Acompanhamento>();

                            ocorrencias.Add(Instrumento, acompanhamento);

                            ListaPosicoes.Add(e.ordem.Account.ToString(), ocorrencias);
                        }
                    }

                    string clienteAtivo = gClienteBovespaPadrao + "." + pos.Instrumento;

                    if (!gPosicaoOrdens.ContainsKey(clienteAtivo))
                    {
                        PosicaoNetInfo posicaoNet = new PosicaoNetInfo();
                        gPosicaoOrdens.Add(clienteAtivo, posicaoNet);
                    }

                    gPosicaoOrdens[clienteAtivo].qtdeAbertCompra = pos.QuantidadeAbertaCompra;
                    gPosicaoOrdens[clienteAtivo].qtdeAbertVenda = pos.QuantidadeAbertaVenda;
                    gPosicaoOrdens[clienteAtivo].qtdeExecCompra = pos.QuantidadeExecutadaCompra;
                    gPosicaoOrdens[clienteAtivo].qtdeExecVenda = pos.QuantidadeExecutadaVenda;
                    gPosicaoOrdens[clienteAtivo].NetAbert = pos.QuantidadeNetAberta;
                    gPosicaoOrdens[clienteAtivo].NetExec = pos.QuantidadeNetExecutada;
                    gPosicaoOrdens[clienteAtivo].precoMedioAbertCompra = decimal.ToDouble(pos.PrecoMedioCompra);
                    gPosicaoOrdens[clienteAtivo].precoMedioExecCompra = decimal.ToDouble(pos.PrecoMedioCompra);
                    gPosicaoOrdens[clienteAtivo].precoMedioAbertVenda = decimal.ToDouble(pos.PrecoMedioVenda);
                    gPosicaoOrdens[clienteAtivo].precoMedioExecVenda = decimal.ToDouble(pos.PrecoMedioVenda);
                    gPosicaoOrdens[clienteAtivo].volumeFinNetAbert = decimal.ToDouble(pos.FinanceiroNetAbertas);
                    gPosicaoOrdens[clienteAtivo].volumeFinNetExec = decimal.ToDouble(pos.FinanceiroNetExecutadas);
                }
            }
            catch (Exception ex)
            {
                string lMensagem = string.Format("Erro em Instance_OnAcompanhamentoEvent: {0}", ex.Message);

                //EventLogger.WriteFormat(EventLogEntryType.Error, lMensagem);

                Debug.WriteLine(lMensagem);
            }
        }

        #endregion

        #region Funções para registro COM automático

        [ComRegisterFunctionAttribute]
        public static void RegisterFunction(Type type)
        {
            Registry.ClassesRoot.CreateSubKey(GetSubKeyName(type));

            RegistryKey key = Registry.ClassesRoot.CreateSubKey("CLSID\\{" + type.GUID.ToString().ToUpper() + "}\\InprocServer32");

            //key.SetValue("", @"C:\Windows\System32\mscoree.dll"); 
            key.SetValue("", System.Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\mscoree.dll"); 
        }

        [ComUnregisterFunctionAttribute]
        public static void UnregisterFunction(Type type)
        {
            Registry.ClassesRoot.DeleteSubKey(GetSubKeyName(type), false);
        }

        private static string GetSubKeyName(Type type)
        {
            string s = @"CLSID\{" + type.GUID.ToString().ToUpper() + @"}\Programmable";
            return s;
        }

        #endregion
    }
}

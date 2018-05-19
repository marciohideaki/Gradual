using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.CadastroPapeis.Entidades;
using System.Collections;
using System.Threading;
using System.Configuration;
using System.Runtime.CompilerServices;
using Gradual.OMS.Library;
using Gradual.OMS.CadastroPapeis.Lib;
using log4net;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;
using System.Runtime.Serialization;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace Gradual.OMS.CadastroPapeis
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]       
    public class ServicoCadastroPapeis : IServicoCadastroPapeis, IServicoControlavel
    {
        #region Atributes

        private const int LIMITE_BLOCO_DADOS = 10000;

        private static int _temporizadorListagemBovespaBmf;

        private static string _ConnectionStringMDS;

        private static List<string> _papeisExistentesServicoCotacao = null;
        
        private static AutoResetEvent autoEvent = new AutoResetEvent(false);

        private static Timer stateTimer;

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");

        private ServicoStatus _ServicoStatus = ServicoStatus.Indefinido;
        #endregion

        #region Constructors
        public ServicoCadastroPapeis()
        {
            
        }
        #endregion

        #region Properties
        
        private static Hashtable ListaPapeisNegociados { get; set; }

        private static Dictionary<string, CadastroPapelMdsInfo> ListaCadastroPapeisMDS { get; set; }

        private static int TemporizadorListagemBovespaBmf
        {
            get
            {
                if (_temporizadorListagemBovespaBmf == 0)
                {
                    _temporizadorListagemBovespaBmf = Convert.ToInt32(ConfigurationManager.AppSettings["TemporizadorListagemBovespaBmf"].ToString());
                    //_temporizadorListagemBovespaBmf = 50;
                }
                //transformando em horas
                return (_temporizadorListagemBovespaBmf * 1000) * 3600;

                //return 10000;
            }
        }

        private static string ConnectionStringMDS
        {
            get
            {
                if (_ConnectionStringMDS == null)
                {
                    _ConnectionStringMDS = ConfigurationManager.ConnectionStrings["MDS"].ToString();
                }
                return _ConnectionStringMDS;
            }
        }

        private static List<string> PapeisExistentesServicoCotacao
        {
            get
            {
                if (_papeisExistentesServicoCotacao == null)
                {
                    _papeisExistentesServicoCotacao = new List<string>();

                    string valor = ConfigurationManager.AppSettings["PapeisExistentesServicoCotacao"].ToString();

                    if (string.IsNullOrEmpty(valor))
                        valor = "";

                    string[] lista = valor.Trim().ToUpper().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string papel in lista)
                        _papeisExistentesServicoCotacao.Add(papel);
                }
                return _papeisExistentesServicoCotacao;
            }
        }

        #endregion

        #region Métodos de apoio

        /// <summary>
        /// Métod para listar os papeis que estão no banco e inserir na hashtable estática
        /// </summary>
        /// <param name="pStateTransaction"></param>
        [MethodImpl(MethodImplOptions.PreserveSig)]        
        private void ListarPapeisNegociadosBovespaBmf(object pStateTransaction)
        {
            try
            {
                PapelNegociadoBmfDbLib lPapelBmfDb = new PapelNegociadoBmfDbLib();
                List<PapelNegociadoBmfInfo> listPapelBmf = lPapelBmfDb.ListarPapelNegociadoBmf();

                PapelNegociadoBovespaDbLib lPapelBovespaDb = new PapelNegociadoBovespaDbLib();
                List<PapelNegociadoBovespaInfo> listPapelBovespa = lPapelBovespaDb.ListarPapelNegociadoBovespa();

                ListaPapeisNegociados = new Hashtable();
                ListaPapeisNegociados.Clear();

                lock (ListaPapeisNegociados)
                {
                    foreach (PapelNegociadoBovespaInfo item in listPapelBovespa)
                    {
                        if (!ListaPapeisNegociados.Contains(item.CodNegociacao.Trim()))
                            ListaPapeisNegociados.Add(item.CodNegociacao.Trim(), item);
                    }

                    foreach (PapelNegociadoBmfInfo item in listPapelBmf)
                    {
                        if (!ListaPapeisNegociados.Contains(string.Concat(item.CodMercadoria.Trim(), item.SerieVencimento.Trim())))
                            ListaPapeisNegociados.Add(string.Concat(item.CodMercadoria.Trim(),item.SerieVencimento.Trim()), item);
                    }

                    logger.Info(string.Concat("Entrou no ListarPapeisNegociadosBovespaBmf e listou ", ListaPapeisNegociados.Count, " papeis"));
                }

                ListaCadastroPapeisMDS = new Dictionary<string, CadastroPapelMdsInfo>();
                ListaCadastroPapeisMDS.Clear();

                lock (ListaCadastroPapeisMDS)
                {
                    CadastroPapeisMdsDbLib lCadastroPapeisMdsDb = new CadastroPapeisMdsDbLib();
                    lCadastroPapeisMdsDb._ConnectionStringName = ConnectionStringMDS;
                    ListaCadastroPapeisMDS = lCadastroPapeisMdsDb.ListarCadastroPapeisMDS();
                    logger.Info(string.Concat("Entrou no ListarCadastroPapeisMDS e listou ", ListaCadastroPapeisMDS.Count, " papeis"));

                    List<string> ListaComposicaoIndicesMDS = lCadastroPapeisMdsDb.ListarComposicaoIndicesMDS();
                    logger.Info(string.Concat("Entrou no ListarComposicaoIndicesMDS e listou ", ListaComposicaoIndicesMDS.Count, " papeis"));

                    foreach (string item in ListaComposicaoIndicesMDS)
                    {
                        string[] composicaoIndice = item.Split('@');
                        string indice = composicaoIndice[0];
                        string instrumento = composicaoIndice[1];

                        CadastroPapelMdsInfo dadosInstrumento = ListaCadastroPapeisMDS[instrumento];
                        dadosInstrumento.ComposicaoIndice += indice + ",";
                        ListaCadastroPapeisMDS[instrumento] = dadosInstrumento;
                    }
                    logger.Info("Composição dos Indices atualizado");
                }
            }
            catch (Exception ex)
            {
                logger.Error(pStateTransaction, ex);
            }
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private string Compress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        private string Decompress(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        #endregion

        #region Métodos Públicos

        public ListarCadastroPapeisMdsResponse ConsultarCadastroPapeisMds(ListarCadastroPapeisMdsRequest pRequest)
        {
            ListarCadastroPapeisMdsResponse lResposta = new ListarCadastroPapeisMdsResponse(pRequest.CodigoMensagem);

            /*
             * Para manter compatibilidade com as versões antigas do GTI, será mantida os blocos de dados;
             * As novas versões do GTI deverão pedir apenas dados compactados, que não necessita quebrar em blocos separados para envio
             */
            if (pRequest.NumBlocoDados == 0)
                logger.Info("> ConsultarCadastroPapeisMds: solicitando dados compactados");
            else
                logger.InfoFormat("> ConsultarCadastroPapeisMds: solicitando Bloco[{0}] de dados", pRequest.NumBlocoDados);

            try
            {
                //Se ainda não estiver carregado, pedimos para carregar novamente os dados na 
                //List chamando o método ListarPapeisNegociadosBovespaBmf
                if (ListaCadastroPapeisMDS == null)
                {
                    ListarPapeisNegociadosBovespaBmf(null);
                    logger.InfoFormat("> ConsultarCadastroPapeisMds: Cadastro de Papéis MDS preenchido com [{0}] entradas", ListaCadastroPapeisMDS.Count);
                }

                if (ListaCadastroPapeisMDS.Count > 0)
                {
                    if (pRequest.NumBlocoDados == 0)
                    {
                        StringBuilder lista = new StringBuilder();
                        foreach (KeyValuePair<string, CadastroPapelMdsInfo> dadosInstrumento in ListaCadastroPapeisMDS)
                        {
                            lista.Append(
                                dadosInstrumento.Value.Instrumento + "||" +
                                dadosInstrumento.Value.RazaoSocial + "||" +
                                dadosInstrumento.Value.LotePadrao.ToString() + "||" +
                                dadosInstrumento.Value.IndicadorOpcao + "||" +
                                dadosInstrumento.Value.CodigoPapelObjeto + "||" +
                                dadosInstrumento.Value.SegmentoMercado + "||" +
                                dadosInstrumento.Value.CodigoISIN + "||" +
                                dadosInstrumento.Value.ComposicaoIndice + "||" +
                                Convert.ToDouble(dadosInstrumento.Value.PrecoExercicio, ciBR) + "||" +
                                ((DateTime)dadosInstrumento.Value.DataVencimento).ToString("yyyyMMddHHmmss") + "!!!!");
                        }

                        List<string> listaPapeis = new List<string>();
                        string listaCompactada = Compress(lista.ToString());
                        listaPapeis.Add(listaCompactada);
                        lResposta.ListaCadastroPapeisMds = listaPapeis;

                        logger.InfoFormat("> ConsultarCadastroPapeisMds: Itens[{0}] descompactada[{1} bytes] compactada[{2} bytes]",
                            ListaCadastroPapeisMDS.Count, lista.Length, listaCompactada.Length);
                    }
                    else
                    {
                        int posicao = (pRequest.NumBlocoDados - 1) * LIMITE_BLOCO_DADOS;

                        List<string> lista = new List<string>();
                        foreach (KeyValuePair<string, CadastroPapelMdsInfo> dadosInstrumento in
                            ListaCadastroPapeisMDS.Skip(posicao).Take(System.Math.Min(LIMITE_BLOCO_DADOS, ListaCadastroPapeisMDS.Count - posicao)))
                        {
                            string retInstrumento =
                                dadosInstrumento.Value.Instrumento + "||" +
                                dadosInstrumento.Value.RazaoSocial + "||" +
                                dadosInstrumento.Value.LotePadrao.ToString() + "||" +
                                dadosInstrumento.Value.IndicadorOpcao + "||" +
                                dadosInstrumento.Value.CodigoPapelObjeto + "||" +
                                dadosInstrumento.Value.SegmentoMercado + "||" +
                                dadosInstrumento.Value.CodigoISIN + "||" +
                                dadosInstrumento.Value.ComposicaoIndice + "||" +
                                Convert.ToDouble(dadosInstrumento.Value.PrecoExercicio, ciBR) + "||" +
                                ((DateTime)dadosInstrumento.Value.DataVencimento).ToString("yyyyMMddHHmmss");
                            lista.Add(retInstrumento);
                        }

                        lResposta.ListaCadastroPapeisMds = lista;

                        if (lista.Count < LIMITE_BLOCO_DADOS)
                            lResposta.ContinuaDados = false;
                        else
                            lResposta.ContinuaDados = true;
                    }

                    lResposta.DescricaoResposta = "Cadastro de Papeis preenchido com sucesso.";
                    lResposta.StatusResposta = MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lResposta.DescricaoResposta = ex.Message;
                lResposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                logger.Error(ex.StackTrace);
            }

            return lResposta;
        }

        /// <summary>
        /// Consulta na memória Efetuando um query de Linq dentro de uma hashtable que está na memória
        /// </summary>
        /// <param name="pRequest">Request do papel negociado</param>
        /// <returns>Retorna uma lista dos papeis que estão em na memória</returns>
        public ConsultarPapelNegociadoResponse ConsultarPapelNegociado(ConsultarPapelNegociadoRequest pRequest)
        {
            ConsultarPapelNegociadoResponse lResposta = new ConsultarPapelNegociadoResponse(pRequest.CodigoMensagem);

            PapelNegociadoBmfDbLib lPapelBmfDb = new PapelNegociadoBmfDbLib();

            PapelNegociadoBovespaDbLib lPapelBovespaDb = new PapelNegociadoBovespaDbLib();

            bool lPapelEncontrado = false;

            string lAtivosParaLog = "";

            List<string> lstAtivosTemp = new List<string>();

            foreach (string lAtivo in pRequest.LstAtivos)
            {
                lAtivosParaLog += string.Format("{0}, ", lAtivo);

                if (lAtivo.ToLower().Substring(lAtivo.Length - 1).Equals("f"))
                    lstAtivosTemp.Add(lAtivo.Remove(lAtivo.Length - 1));
                else
                    lstAtivosTemp.Add(lAtivo);
            }

            lAtivosParaLog = lAtivosParaLog.TrimEnd(", ".ToCharArray());

            logger.InfoFormat("> Chamada ConsultarPapelNegociado [{0}]", lAtivosParaLog);

            try
            {
                //Se ainda não estiver carregado, pedimos para carregar novamente os dados na 
                //hashtable chamando o método ListarPapeisNegociadosBovespaBmf
                if (ListaPapeisNegociados == null)
                {
                    ListarPapeisNegociadosBovespaBmf(null);

                    logger.InfoFormat("HashTable de Papéis negociados preenchida com [{0}] entradas", ListaPapeisNegociados.Count);
                }

                //lock (ListaPapeisNegociados)
                //{
                    var lPapeis = from a in ListaPapeisNegociados.Cast<DictionaryEntry>()
                                  select a;

                    logger.InfoFormat("[{0}] Papéis selecionados depois do cast", lPapeis.Count());

                    if (lPapeis.Count() > 0)
                    {

                        ///Filtro de ativos
                        if (pRequest.LstAtivos.Count > 0)
                        {
                            if (pRequest.LstAtivos.Count == 1)
                            {
                                string lBusca = pRequest.LstAtivos[0];

                                if (lBusca.ToLower().Substring(lBusca.Length - 1).Equals("f"))
                                    lBusca = lBusca.Remove(lBusca.Length - 1);

                                lPapeis = from a in ListaPapeisNegociados.Cast<DictionaryEntry>()
                                          where a.Key.ToString().Contains(lBusca)
                                          select a;
                            }
                            else
                            {
                                
                                lPapeis = from a in ListaPapeisNegociados.Cast<DictionaryEntry>()
                                          where lstAtivosTemp.Contains(a.Key.ToString())
                                          select a;
                            }
                        }

                        //filtro com a data de vencimento no resultado
                        if (pRequest.DataVencimento != null)
                        {
                            lPapeis = lPapeis.Where(delegate(DictionaryEntry dic)
                            {
                                bool lReturn = false;

                                string lNameType = dic.Value.GetType().Name;

                                if (lNameType.Equals("PapelNegociadoBovespaInfo"))

                                    lReturn = (((PapelNegociadoBovespaInfo)dic.Value).DataVencimento != null && ((PapelNegociadoBovespaInfo)dic.Value).DataVencimento.Value.ToString("dd/MM/yyyy").Equals(pRequest.DataVencimento.Value.ToString("dd/MM/yyyy")));

                                else if (lNameType.Equals("PapelNegociadoBmfInfo"))

                                    lReturn = (((PapelNegociadoBmfInfo)dic.Value).DataVencimento != null && ((PapelNegociadoBmfInfo)dic.Value).DataVencimento.Value.ToString("dd/MM/yyyy").Equals(pRequest.DataVencimento.Value.ToString("dd/MM/yyyy")));

                                else
                                    return lReturn;

                                return lReturn;
                            });
                        }

                        ///Filtro com o tipo de mercado no resultado
                        if (!pRequest.TipoMercado.Equals(0))
                        {
                            lPapeis = lPapeis.Where(delegate(DictionaryEntry dic)
                            {
                                string lNameType = dic.Value.GetType().Name;

                                if (lNameType.Equals("PapelNegociadoBovespaInfo"))
                                {
                                    return (((PapelNegociadoBovespaInfo)dic.Value).TipoMercado == pRequest.TipoMercado);
                                }
                                else
                                {
                                    return false;
                                }
                            });
                        }

                        ///Filtro de Tipo Segmento de mercado: Se for BMF efetua o filtro 
                        if (pRequest.DescTipoMercado != null && pRequest.DescTipoMercado.Equals("BMF"))
                        {
                            lPapeis = lPapeis.Where(delegate(DictionaryEntry dic)
                            {
                                string lNameType = dic.Value.GetType().Name;

                                if (lNameType.Equals("PapelNegociadoBmfInfo"))
                                {
                                    return (pRequest.DescTipoMercado.Equals("BMF"));
                                }
                                else
                                {
                                    return false;
                                }
                            });
                        }

                        if (pRequest.DescTipoMercado != null && pRequest.DescTipoMercado.Equals("BOV"))
                        {
                            lPapeis = lPapeis.Where(delegate(DictionaryEntry dic)
                            {
                                string lNameType = dic.Value.GetType().Name;

                                if (lNameType.Equals("PapelNegociadoBovespaInfo"))
                                {
                                    return (pRequest.DescTipoMercado.Equals("BOV"));
                                }
                                else
                                {
                                    return false;
                                }
                            });
                        }

                        foreach (var item in lPapeis)
                        {
                            var lItem = item.Value;

                            string lNameType = item.Value.GetType().Name;

                            if (lNameType.Equals("PapelNegociadoBmfInfo"))
                                lResposta.LstPapelBmfInfo.Add((PapelNegociadoBmfInfo)item.Value);

                            if (lNameType.Equals("PapelNegociadoBovespaInfo"))
                                lResposta.LstPapelBovespaInfo.Add((PapelNegociadoBovespaInfo)item.Value);

                            if (!lResposta.LstPapelInfo.Contains(item.Key))
                                lResposta.LstPapelInfo.Add(item.Key, item.Value);
                        }

                        lPapelEncontrado = true;

                        string lAtivo = "n/e";

                        if (pRequest.LstAtivos.Count > 0)
                        {
                            lAtivo = pRequest.LstAtivos[0];
                        }

                        logger.InfoFormat("< Papel [{0}] encontrado com sucesso. LstPapelBmfInfo.Count: [{1}], LstPapelBovespaInfo.Count: [{2}], LstPapelInfo.Count: [{3}]"
                                          , lAtivo
                                          , lResposta.LstPapelBmfInfo.Count
                                          , lResposta.LstPapelBovespaInfo.Count
                                          , lResposta.LstPapelInfo.Count);
                    }

                    if (lPapelEncontrado)
                    {
                        lResposta.DescricaoResposta = "Ativo(s) encontrado(s) com sucesso.";
                        lResposta.StatusResposta = MensagemResponseStatusEnum.OK;
                    }
                    else
                    {
                        lResposta.DescricaoResposta = "Ativo(s) não encontrado(s)";
                        lResposta.StatusResposta = MensagemResponseStatusEnum.ErroValidacao;
                    }
                //}
            }
            catch (Exception ex)
            {
                lResposta.DescricaoResposta = ex.Message;
                lResposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                logger.Error(ex.StackTrace);
            }

            return lResposta;
        }

        public ListarOpcoesDoPapelResponse ListarOpcoesDoPapel(ListarOpcoesDoPapelRequest pRequest)
        {
            ListarOpcoesDoPapelResponse lResponse = new ListarOpcoesDoPapelResponse();

            logger.InfoFormat("> Chamada ListarOpcoesDoPapel [{0}]", pRequest.Ativo);
            
            try
            {
                AcessaDados lAcesso = new AcessaDados("Retorno");

                lAcesso.ConnectionStringName = "Trade";

                DbCommand lCommand = lAcesso.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_OPCOES_INSTRUMENTO");

                if (string.IsNullOrWhiteSpace(pRequest.TipoMercado))
                    logger.Info("o Tipo mercado está em branco...");

                lAcesso.AddInParameter(lCommand, "pInstrumentoBase", System.Data.DbType.String, pRequest.Ativo);

                lAcesso.AddInParameter(lCommand, "pTipoMercadoBase", System.Data.DbType.String, pRequest.TipoMercado);

                logger.InfoFormat("< Tipo de mercado [{0}] >", pRequest.TipoMercado);

                DataTable lTable = lAcesso.ExecuteOracleDataTable(lCommand);

                foreach (DataRow lRow in lTable.Rows)
                {
                    lResponse.Opcoes.Add(new OpcaoDePapelInfo(lRow));
                }

                lResponse.DescricaoResposta = string.Format("[{0}] opções encontradas.", lResponse.Opcoes.Count);

                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;

                logger.InfoFormat("< [{0}] opções encontradas para [{1}]: [{2}]", lResponse.Opcoes.Count, pRequest.Ativo, lResponse.ListaDeOpcoes);
            }
            catch (Exception ex)
            {
                logger.InfoFormat("< Erro ao buscar opções: [{0}]", ex.Message);

                logger.Error(ex.Message, ex);

                lResponse.DescricaoResposta = string.Format("Erro ao buscar opções: [{0}] [{1}]", ex.Message, ex.StackTrace);

                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lResponse;
        }

        public BuscarPapelCadastradoResponse BuscarPapelCadastradoMds(BuscarPapelCadastradoRequest pRequest)
        {
            BuscarPapelCadastradoResponse lResposta = new BuscarPapelCadastradoResponse();

            try
            {
                string lAtivo = pRequest.Instrumento.ToUpper();

                CadastroPapeisMdsDbLib lCadastroPapeisMdsDb = new CadastroPapeisMdsDbLib();
                lCadastroPapeisMdsDb._ConnectionStringName = ConnectionStringMDS;

                lResposta.PapelCadastradoMDS = lCadastroPapeisMdsDb.BuscarPapelCadastradoMds(pRequest.Instrumento.ToUpper());
                lResposta.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResposta.DescricaoResposta = ex.Message;
                lResposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                logger.Error(ex.StackTrace);
            }

            return lResposta;
        }

        public ConsultarSePapelExisteResponse ConsultarSePapelExiste(ConsultarSePapelExisteRequest pRequest)
        {
            ConsultarSePapelExisteResponse lResposta = new ConsultarSePapelExisteResponse();

            try
            {
                //Se ainda não estiver carregado, pedimos para carregar novamente os dados na 
                //hashtable chamando o método ListarPapeisNegociadosBovespaBmf
                if (ListaPapeisNegociados == null)
                {
                    ListarPapeisNegociadosBovespaBmf(null);

                    logger.InfoFormat("HashTable de Papéis negociados preenchida com [{0}] entradas", ListaPapeisNegociados.Count);
                }

                string lAtivo = pRequest.Ativo.ToUpper();

                if (lAtivo.Substring(lAtivo.Length - 1) == "F")
                    lAtivo = lAtivo.Remove(lAtivo.Length - 1);

                var lPapeis = from a in ListaPapeisNegociados.Cast<DictionaryEntry>()
                              where a.Key.ToString().ToUpper().Contains(lAtivo)
                              select a;

                lResposta.PapelExiste = (lPapeis.Count<DictionaryEntry>() > 0);

                if (lResposta.PapelExiste == false)
                    if (PapeisExistentesServicoCotacao.Contains(lAtivo))
                        lResposta.PapelExiste = true;
            }
            catch (Exception ex)
            {
                lResposta.DescricaoResposta = ex.Message;
                lResposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                logger.Error(ex.StackTrace);
            }

            return lResposta;
        }

        #endregion

        #region IServicoControlavel Members

        /// <summary>
        /// Método chamado ao iniciar o serviço
        /// </summary>
        public void IniciarServico()
        {
            try
            {
                logger.Info("Servico Inicilializado com sucesso");

                TimerCallback CallBack = ListarPapeisNegociadosBovespaBmf;

                if (stateTimer == null)
                {
                    stateTimer = new Timer(CallBack, autoEvent, 0, TemporizadorListagemBovespaBmf);

                    logger.Info("Ao inicializar o serviço, entrou no ticker do timer == null para chamar com o callback");
                }

                _ServicoStatus = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Erro ao inicializar servico de cadastro de papeis", ex.Message));
            }
        }

        /// <summary>
        /// Método chamado ao parar o serviço
        /// </summary>
        public void PararServico()
        {
            _ServicoStatus = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion
    }
 
}

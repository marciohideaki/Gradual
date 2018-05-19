using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.Collections;
using System.Threading;
using Gradual.OMS.Risco.Lib;
using Gradual.OMS.CadastroPapeis.Lib.Mensageria;
using Gradual.OMS.CadastroPapeis.Lib.Info;
using Gradual.OMS.Risco.Lib.Info;
using Gradual.OMS.Risco.Lib.Mensageria;
using Gradual.OMS.CadastroPapeis;
using Gradual.OMS.Risco.Lib.Enum;
using Gradual.OMS.Custodia.Lib.Mensageria;
using Gradual.OMS.Custodia.Lib.Info;
using Gradual.OMS.Risco.Custodia;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.ContaCorrente;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Ordens.Persistencia.Lib;
using Gradual.OMS.Risco.Persistencia.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.Cotacao.Lib.Mensageria;
using Gradual.OMS.Cotacao;


namespace Gradual.OMS.Risco
{
    /// <summary>
    /// Classe responsável pela implementação e validação de todas regras de risco do OMS da Gradual     
    /// </summary>    
    /// <respomsavel>Rafael Sanches Garcia</respomsavel>
    [Category("Gradual.OMS.Sistemas.Risco")]
    [Description("Classe responsável pela implementação e validação de todas regras de risco do OMS da Gradual")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ServicoRisco : IServicoRisco
    {
        #region Construtor

        public ServicoRisco()
        {
            log4net.Config.XmlConfigurator.Configure();

            // Vencimento de opções
            if (VencimentoInstrumentoInfo.htInstrumentos == null)
            {
                this.ObterVencimentoOpcoes();
            }

            // Lista de instrumentos.
            if (SecurityListInfo.htSecurityList == null)
            {
                this.ObterSecurityList();
            }

            // Lista de papeis para alavancagem
            if (ListaInstrumentoAlavancagem == null)
            {
                this.ListaInstrumentoAlavancagem = ObterListaInstrumentosAlavancagem();
            }
        }

        #endregion

        #region Variaveis Locais

        // Numero da porta repassadora.
        private const int NumeroPortaAssessor = 316;

        // Esse tipo de declaracao é preferivel sobre a outra
        // classes derivadas de MyClass automaticamente gravarao no log
        // com o nome da classe corrigido
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Hashtable responsável por armazenar todos os instrumentos
        /// </summary>
        private CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis { set; get; }

        private Hashtable ObtemFatorCotacaoInstrumento = new Hashtable();

        /// <summary>
        /// Lista de papeis permitidos para alavancagem
        /// </summary>
        private ListaInstrumentosAlavancagemResponse<ListaInstrumentoAlavancagemInfo> ListaInstrumentoAlavancagem { set; get; }

        #endregion

        #region  constantes

        private const int CodigoCorretora = 227;
        private const int CodigoPermissaoInstitucional = 62;

        #endregion

        #region [ Oscilacao Opcoes ]


        private const int OscilacaoMaximaPermitidaMercadoAVista = 3;

        private const int OscilacaoMaximaITM = 30;
        private const int OscilacaoMaximaATM = 10;
        private const int OscilacaoMaximaOTM = 90;



        #endregion

        #region ValidarPipeLineRisco

        /// <summary>
        /// Método responsável por validar todos os parametros e permissões de uma ordem
        /// Efetua o pipeline de risco de acordo com o tipo de mercado e tipo de ordem
        /// </summary>
        /// <param name="pParametros">Atibutos da ordem do cliente</param>
        /// <returns>Objeto de risco validado com criticas de validação ou sucesso</returns>
        public ValidacaoRiscoResponse ValidarPipeLineRisco(ValidacaoRiscoRequest pParametros)
        {

            try
            {

                bool BLClienteInstitucional = false;

                logger.Info("Solicitação de validação de risco iniciada");


                if (pParametros.EnviarOrdemRequest.OrdemInfo == null)
                {
                    logger.Info("TimeInForce nulo");
                }
                else
                {
                    logger.Info("TimeInForce não é nulo");
                }

                logger.Info("TipoOrdem :" + pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce.ToString());

                //OBJETO RESPONSAVEL POR GUARDAR TODAS AS CRITICAS FEITAS DURANTE O ENVIO DE UMA ORDEM
                List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
                PipeLineCriticaInfo CriticaInfo = null;

                // OBJETO RESPONSAVEL POR ENCAPSULAR OS RETORNOS DA VALIDAÇÃO DA ORDEM DO CLIENTE
                ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();

                //Caso fracionário, transforma em integral para ober o lote padrao

                logger.Info("Valida fracionario / Integral");
                if (pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Substring(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length - 1, 1) == "F")
                {
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol = pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length - 1);
                }


                logger.Info("Inicia consulta no cadastro de papeis");

                CadastroPapeis = new ServicoCadastroPapeis().ObterInformacoesIntrumento(
                                                                                        new CadastroPapeisRequest()
                                                                                        {
                                                                                            Instrumento = pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                                                        });

                if (CadastroPapeis.Objeto == null)
                {
                    logger.Info("Papel não encontrado no cadastro de papeis");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "Instrumento nao encontrado no cadastro de papeis";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);
                }


                logger.Info("Inicia a consulta de parametros e permissões. Cliente:  " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

                //Permissoes de Risco do cliente            
                ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam =
                    new ServicoRisco().CarregarParametosPermissoesCliente(
                                                                            new ClienteParametrosPermissoesRequest()
                                                                            {
                                                                                IdCliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                                                                            });

                if (_responseClienteRiscoParam.ColecaoObjeto == null)
                {
                    logger.Info("Cliente não possui nenhuma permissão vinculada a seu código de bolsa");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "Cliente não possui nenhuma permissão vinculada a seu código de bolsa";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);
                }


                //Verifica se as informações do cadastro de papeis / permissoes de risco do cliente estão preenchiddas
                if ((CadastroPapeis.Objeto != null) && (_responseClienteRiscoParam.ColecaoObjeto != null))
                {
                    logger.Info("Parametros e permissões retornados com sucesso");

                    #region [ Permissao para abrir boleta no OMS ]

                    var PermissaoBloqueioBoletaOMS = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                     where p.IdParametroPermissao == (int)(ParametroPermissaoRiscoEnum.PERMISSAO_BLOQUEAR_ENVIO_ORDEM_OMS)
                                                     && p.IdCliente == pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()

                                                     select p;

                    if (PermissaoBloqueioBoletaOMS.Count() > 0)
                    {
                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = "Permissão insuficiente para enviar ordens pelo OMS";
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                        PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                        PipeLineResponse.StatusResposta = CriticaRiscoEnum.Sucesso;
                        PipeLineResponse.DataResposta = DateTime.Now;
                        return TratamentoRetorno(PipeLineResponse);
                    }


                    #endregion

                    #region [ Lista de Instrumentos Bloqueados ]

                    logger.Info("Verifica se o papel requisitado esta bloqueado para operações");
                    logger.Info("Carrega a lista de papeis bloqueados para o cliente");

                    InstrumentoBloqueadoResponse<InstrumentoBloqueioInfo> ListaBloqueioCliente =
                        new PersistenciaRisco().CarregarInstrumentosBloqueados(
                                                                                new InstrumentoBloqueadoRequest()
                                                                                {
                                                                                    Id_Cliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account
                                                                                }
                                                                              );

                    if (ListaBloqueioCliente.StatusResposta == CriticaMensagemEnum.OK)
                    {
                        logger.Info("Bloqueio carregado com sucesso");
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao carregar os instrumentos bloqueados do cliente.");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = "Ocorreu um erro ao carregar os instrumentos bloqueados do cliente";
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);
                    }

                    #endregion

                    #region Cliente Institucional

                    var PermissaoInstitucional = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                 where p.IdParametroPermissao == CodigoPermissaoInstitucional
                                                 && p.IdCliente == pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()

                                                 select p;

                    if (PermissaoInstitucional.Count() > 0)
                    {
                        BLClienteInstitucional = true;
                    }
                    else
                    {
                        BLClienteInstitucional = false;
                    }

                    #endregion

                    // Valida de a Ordem enviada é uma ordem de compra ou venda
                    if (pParametros.EnviarOrdemRequest.OrdemInfo.Side == RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra)
                    {
                        #region [ OPERACAO DE COMPRA ]

                        logger.Info("Verifica se o papel que o cliente esta tentando operar esta bloqueado para operacao de compra");

                        #region [ Bloqueios na Compra ]

                        if (ListaBloqueioCliente.ColecaoObjeto != null)
                        {

                            var InstrumentoBloqueioCompra = from p in ListaBloqueioCliente.ColecaoObjeto
                                                            where p.Instrumento == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                            && (p.Sentido == Lib.Dados.Enum.SentidoBloqueioEnum.Compra
                                                            || p.Sentido == Lib.Dados.Enum.SentidoBloqueioEnum.Ambos)

                                                            select p;

                            if ((InstrumentoBloqueioCompra != null) && (InstrumentoBloqueioCompra.Count() > 0))
                            {

                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = "O papel <" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " > foi bloqueado para compra";
                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                                PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                PipeLineResponse.StatusResposta = CriticaRiscoEnum.Sucesso;
                                PipeLineResponse.DataResposta = DateTime.Now;
                                return TratamentoRetorno(PipeLineResponse);
                            }
                        }

                        #endregion

                        logger.Info("Inicia busca de permissões. Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

                        if (CadastroPapeis.Objeto.TipoBolsa == OMS.CadastroPapeis.Lib.Enum.TipoBolsaEnum.BOVESPA)
                        {

                            #region [ OPERACAO DE COMPRA NO MERCADO DE BOVESPA ]

                            // ****************************************************************************************
                            // 2º PASSO: VALIDACAO PERMISSAO NO MERCADO DE BOVESPA
                            // ****************************************************************************************

                            // CHECAR PERMISSAO PARA OPERAR NO MERCADO DE BOVESPA

                            logger.Info("Solicitação de operação no mercado de BOVESPA");

                            // Faz query nas permissões do cliente para verificar se o cliente possui permissão para operar
                            // no mercado de Bovespa
                            var PermissaoMercadoBovespa = (from p in _responseClienteRiscoParam.ColecaoObjeto
                                                           where p.IdBolsa == (int)(ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_BOVESPA)
                                                           && p.IdCliente == pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                                                           && p.DtValidade <= DateTime.Now

                                                           select p).Count();

                            // Caso a variável 'PermissaoMercadoBovespa'  para o mercado de bovespa retorne > 0
                            // significa que o cliente possui permissão para operar o Instrumento no mercado de Bovespa
                            if (PermissaoMercadoBovespa > 0)
                            {
                                logger.Info("Permissão para operar no mercado de bovespa concedida. Cliente:  " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

                                // **********************************************************************************
                                // 3º PASSO: VALIDA PERMISSAO PARA OPERAR NO MERCADO QUE O ATIVO PERTENCE
                                // **********************************************************************************

                                switch (CadastroPapeis.Objeto.TipoMercado)
                                {
                                    // VALIDA DE O CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO A VISTA
                                    case OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.AVISTA:

                                        logger.Info("Operação no mercado a vista");

                                        //VALIDACAO DE COMPRA DE ACOES NO MERCADO A VISTA //ValidarCompraAcoesInstitucional

                                        //Verifica se o cliente é um cliente institucional
                                        if (BLClienteInstitucional == true)
                                        {
                                            PipeLineResponse = ValidarCompraAcoesInstitucional(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                        }
                                        else
                                        {
                                            PipeLineResponse = ValidarCompraAcoes(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                        }

                                        return PipeLineResponse;

                                    // VALIDA DE O CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO A FRACIONARIO
                                    case OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.FRACIONARIO:

                                        logger.Info("Operação no mercado a fracionário");

                                        //Verifica se o cliente é um cliente institucional
                                        if (BLClienteInstitucional == true)
                                        {
                                            PipeLineResponse = ValidarCompraOpcoesInstitucional(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                        }
                                        else
                                        {
                                            PipeLineResponse = ValidarCompraOpcoes(pParametros, _responseClienteRiscoParam);
                                        }

                                        return PipeLineResponse;

                                    // VALIDA SE O CLIENTE POSSUI PERMISSÃO PARA OPERAR NO MERCADO DE OPCOES
                                    case OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.OPCAO:

                                        logger.Info("Operação no mercado de opções");

                          
                                        //Verifica se o cliente é um cliente institucional
                                        if (BLClienteInstitucional == true)
                                        {
                                            PipeLineResponse = ValidarCompraOpcoesInstitucional(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                        }
                                        else
                                        {
                                            PipeLineResponse = ValidarCompraOpcoes(pParametros, _responseClienteRiscoParam);
                                        }

                                        return PipeLineResponse;
                                }
                            }
                            else

                                // ---------  PERMISSAO INSUFICIENTE --------------------------------------------------------
                                // --------------------------------- VALIDACAO PERMISSAO NO MERCADO DE BOVESPA  --------------

                                // Caso a variavel 'PermissaoMercadoBovespa' para o mercado de bovespa retorna = 0
                                // significa que o cliente não possui permissão para operar no mercado de Bovespa
                                if (PermissaoMercadoBovespa == 0)
                                {

                                    logger.Info("Cliente não possui permissão para operar no mercado de bovespa");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Cliente não possui permissão para operar no mercado de bovespa";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                }

                            #endregion
                        }
                        else
                            // CHECAR PERMISSAO PARA OPERAR NO MERCADO DE BMF
                            if (CadastroPapeis.Objeto.TipoBolsa == OMS.CadastroPapeis.Lib.Enum.TipoBolsaEnum.BMF)
                            {
                                #region [ OPERACAO DE COMPRA NO MERCADO DE BMF ]

                                logger.Info("Solicitação de operação no mercado de BMF");
                                logger.Info("Verifica permissão para operar no mercado de BMF");

                                var PermissaoMercadoBMF = (from p in _responseClienteRiscoParam.ColecaoObjeto
                                                           where p.IdParametroPermissao == (int)(ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_BMF)
                                                           && p.IdCliente == pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                                                           && p.DtValidade <= DateTime.Now

                                                           select p).Count();

                                if (PermissaoMercadoBMF > 0)
                                {
                                    logger.Info("Permissão para operar BMF concedida");

                                    // COMPRA DE BMF
                                    PipeLineResponse = ValidarCompraBMF(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                    return PipeLineResponse;


                                }
                                else
                                {

                                    logger.Info("Cliente não possui permissão para operar BMF");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Cliente não possui permissão suficiente para operar no mercado de BMF.";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);
                                }


                                #endregion
                            }

                        #endregion
                    }
                    else if (pParametros.EnviarOrdemRequest.OrdemInfo.Side == RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda)
                    {
                        #region [ OPERACAO DE VENDA ]

                        #region [ Bloqueios na venda ]

                        if (ListaBloqueioCliente.ColecaoObjeto != null)
                        {

                            var InstrumentoBloqueioCompra = from p in ListaBloqueioCliente.ColecaoObjeto
                                                            where p.Instrumento == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                            && (p.Sentido == Lib.Dados.Enum.SentidoBloqueioEnum.Venda
                                                            || p.Sentido == Lib.Dados.Enum.SentidoBloqueioEnum.Ambos)

                                                            select p;

                            if ((InstrumentoBloqueioCompra != null) && (InstrumentoBloqueioCompra.Count() > 0))
                            {

                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = "O papel <" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " > foi bloqueado para venda";
                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                                PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                PipeLineResponse.DataResposta = DateTime.Now;
                                PipeLineResponse.StatusResposta = CriticaRiscoEnum.Sucesso;
                                return TratamentoRetorno(PipeLineResponse);
                            }
                        }

                        #endregion

                        // CHECAR PERMISSAO PARA OPERAR NO MERCADO DE BOVESPA
                        if (CadastroPapeis.Objeto.TipoBolsa == OMS.CadastroPapeis.Lib.Enum.TipoBolsaEnum.BOVESPA)
                        {
                            #region [ OPERACAO DE VENDA NO MERCADO DE BOVESPA ]

                            // Faz query nas permissões do cliente para verificar se o cliente possui permissão para operar
                            // no mercado de Bovespa
                            var PermissaoMercadoBovespa = (from p in _responseClienteRiscoParam.ColecaoObjeto
                                                           where p.IdBolsa == (int)(ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_BOVESPA)
                                                           && p.IdCliente == pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()

                                                           select p).Count();

                            // Caso a variável 'PermissaoMercadoBovespa'  para o mercado de bovespa retorne > 0
                            // significa que o cliente possui permissão para operar o Instrumento no mercado de Bovespa
                            if (PermissaoMercadoBovespa > 0)
                            {
                                switch (CadastroPapeis.Objeto.TipoMercado)
                                {
                                    // VALIDA DE O CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO A VISTA
                                    case OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.AVISTA:

                                        //VALIDACAO DE COMPRA NO MERCADO DE OPCOES
                                        if (BLClienteInstitucional == true)
                                        {
                                            PipeLineResponse = ValidarVendaAcoesInstitucional(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                        }
                                        else
                                        {
                                            PipeLineResponse = ValidarVendaAcoes(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                        }

                                        return PipeLineResponse;

                                    // VALIDA DE O CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO A VISTA
                                    case OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.FRACIONARIO:

                                        //VALIDACAO DE VENDA DE ACOES NO MERCADO FRACIONARIO
                                        if (BLClienteInstitucional == true)
                                        {
                                            PipeLineResponse = ValidarVendaAcoesInstitucional(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                        }
                                        else
                                        {
                                            PipeLineResponse = ValidarVendaAcoes(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                        }

                                        return PipeLineResponse;

                                    // VALIDA SE O CLIENTE POSSUI PERMISSÃO PARA OPERAR NO MERCADO DE OPCOES
                                    case OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.OPCAO:

                                        //VALIDACAO DE VENDA DE ACOES NO MERCADO FRACIONARIO
                                        if (BLClienteInstitucional == true)
                                        {
                                            PipeLineResponse = ValidarVendaOpcoesInstitucional(pParametros, _responseClienteRiscoParam);
                                        }
                                        else
                                        {
                                            PipeLineResponse = ValidarVendaOpcoes(pParametros, _responseClienteRiscoParam);
                                        }
                                        return PipeLineResponse;
                                }

                            }

                            #endregion
                        }
                        else
                        {
                            //VENDA DE BMF
                            if (pParametros.EnviarOrdemRequest.OrdemInfo.Side == RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda)
                            {
                                #region [ OPERACAO DE VENDA NO MERCADO DE BMF ]

                                // CHECAR PERMISSAO PARA OPERAR NO MERCADO DE BMF
                                if (CadastroPapeis.Objeto.TipoBolsa == OMS.CadastroPapeis.Lib.Enum.TipoBolsaEnum.BMF)
                                {

                                    var PermissaoMercadoBMF = (from p in _responseClienteRiscoParam.ColecaoObjeto
                                                               where p.IdParametroPermissao == (int)(ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_BMF)
                                                               && p.IdCliente == pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                                                               && p.DtValidade <= DateTime.Now

                                                               select p).Count();

                                    if (PermissaoMercadoBMF > 0)
                                    {
                                        // COMPRA DE BMF
                                        PipeLineResponse = ValidarVendaBMF(pParametros, _responseClienteRiscoParam, CadastroPapeis);
                                        return PipeLineResponse;

                                    }
                                    else
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Cliente não possui permissão suficiente para operar no mercado de BMF.";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);
                                    }

                                }

                                #endregion
                            }

                        }

                        #endregion
                    }
                }

                PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                PipeLineResponse.DataResposta = DateTime.Now;
                return TratamentoRetorno(PipeLineResponse);

            }
            catch (Exception ex)
            {
                logger.Info("Descricao do erro: " + ex.Message);
                logger.Info("StackTrace: " + ex.StackTrace);

                return null;
            }

        }

        #endregion

        #region Envio de Ordens Canal Institucional

        #region  Inclusão de ordem

        /// <summary>
        /// Valida Compra de ações para clientes institucionais.
        /// </summary>
        /// <param name="pParametros"></param>
        /// <param name="_responseClienteRiscoParam"></param>
        /// <param name="CadastroPapeis"></param>
        /// <returns></returns>
        private ValidacaoRiscoResponse ValidarCompraAcoesInstitucional(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo CriticaInfo = null;

            logger.Info("Inicia Rotina de validação de compra de ações, cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

            logger.Info("Caracteristicas da ordem");
            logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Operação            : Compra");
            logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Preco               :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
            logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());
            logger.Info("ClasseCliente       : Institucional");


            int FatorCotacao = int.Parse(CadastroPapeis.Objeto.FatorCotacao);

            // TRATAMENTO [ FRACIONARIO / INTEGRAL ]
            int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
            int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

            #region [ Alteração de Ordem ]

            // Verifica se o ID da ordem já existe no banco de dados.
            OrdemInfo OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
            {
                logger.Info("Alteração de ordem solicitada");
                logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                logger.Info("Inicia rotina de validação de alteração de ordens.");

                return this.AlterarOrdemCompraInstitucional(pParametros, OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
            }

            #endregion

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
            {

                // ENVIAR INTEGRAL
                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;
                logger.Info("Inicia operação de compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                // Inseri a Ordem solicitada no banco de dados    
                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                    logger.Info("Envia a ordem para o roteador de ordens");

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem institucional enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }
                }

                if (ModuloLoteNegociacao > 0)
                {
                    // ENVIAR FRACIONARIO
                    logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                    // ENVIAR FRACIONARIO
                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                    pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                        logger.Info("Envia a ordem para o roteador de ordens");


                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);


                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                            PipeLineResponse.DataResposta = DateTime.Now;
                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }
                    }
                }
            }
            else
            {
                //FRACIONARIO
                pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                // Inseri a Ordem solicitada no banco de dados    
                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                    //ENVIA ORDEM PARA O ROTEADOR                                    
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    logger.Info("Envia a ordem para o roteador de ordens");


                    var RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }

                }
            }

            return PipeLineResponse;
        }

        /// <summary>
        /// Valida Venda de ações para clientes institucionais
        /// </summary>
        /// <param name="pParametros"></param>
        /// <param name="_responseClienteRiscoParam"></param>
        /// <param name="CadastroPapeis"></param>
        /// <returns>Objeto com criticas</returns>
        private ValidacaoRiscoResponse ValidarVendaAcoesInstitucional(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo CriticaInfo = null;

            logger.Info("Inicia Rotina de validação de Venda de ações, cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

            logger.Info("Caracteristicas da ordem");
            logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Operação            : Venda");
            logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Preco               :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
            logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());
            logger.Info("ClasseCliente       : Institucional");

            int FatorCotacao = int.Parse(CadastroPapeis.Objeto.FatorCotacao);

            // TRATAMENTO [ FRACIONARIO / INTEGRAL ]
            int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
            int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

            #region [ Alteração de Ordem ]

            // Verifica se o ID da ordem já existe no banco de dados.
            OrdemInfo OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
            {
                logger.Info("Alteração de ordem solicitada");
                logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                logger.Info("Inicia rotina de validação de alteração de ordens.");

                return this.AlterarOrdemVendaInstitucional(pParametros, OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
            }

            #endregion

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
            {
                // ENVIAR INTEGRAL
                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;
                logger.Info("Inicia operação de venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                // Inseri a Ordem solicitada no banco de dados    
                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                    logger.Info("Envia a ordem para o roteador de ordens");

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem institucional enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem institucional para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }
                }

                if (ModuloLoteNegociacao > 0)
                {
                    // ENVIAR FRACIONARIO
                    logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                    // ENVIAR FRACIONARIO
                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                    pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                    logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                        logger.Info("Envia a ordem para o roteador de ordens");


                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);


                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem institucional enviada com sucesso para o Roteador de Ordens");

                            PipeLineResponse.DataResposta = DateTime.Now;
                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem institucional para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }
                    }
                }
            }
            else
            {
                //FRACIONARIO
                pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                // Inseri a Ordem solicitada no banco de dados    
                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                    //ENVIA ORDEM PARA O ROTEADOR                                    
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    logger.Info("Envia a ordem para o roteador de ordens");


                    var RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }

                }
            }

            return PipeLineResponse;
        }

        /// <summary>
        /// Valida Venda de opções para clientes institucionais.
        /// </summary>
        /// <param name="pParametros"></param>
        /// <param name="_responseClienteRiscoParam"></param>
        /// <returns></returns>
        private ValidacaoRiscoResponse ValidarVendaOpcoesInstitucional(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam)
        {
            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();

            try
            {
                logger.Info("Cliente Institucional");
                logger.Info("Inicia Rotina de validação de venda de opções, cliente institucional: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
                logger.Info("Caracteristicas da ordem");
                logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
                logger.Info("Operação            :'Venda");
                logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
                logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
                logger.Info("Preco               :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
                logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());

                //OBJETO RESPONSAVEL POR GUARDAR TODAS AS CRITICAS FEITAS DURANTE O ENVIO DE UMA ORDEM
                List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
                PipeLineCriticaInfo CriticaInfo = null;

                #region [ Alteração de Ordem ]

                // Verifica se o ID da ordem já existe no banco de dados.
                OrdemInfo OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
                {
                    logger.Info("Alteração de ordem solicitada");
                    logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                    logger.Info("Inicia rotina de validação de alteração de ordens.");

                    return this.AlterarOrdemVendaOpcoesInstitucional(pParametros, OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
                }

                #endregion

                // Verifica a quantidade 

                int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
                int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

                if (ModuloLoteNegociacao != 0)
                {
                    logger.Info("A quantidade digitada deve ser multiplo de 100");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A quantidade digitada deve ser multiplo de 100";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    PipeLineResponse.CriticaInfo = CriticaInfoCollection;

                    return TratamentoRetorno(PipeLineResponse);
                }

                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                    //ENVIA ORDEM PARA O ROTEADOR

                    logger.Info("Envia a ordem para o roteador de ordens");

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem de venda de opção para o cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
                logger.Info("Descrição do erro: " + ex.Message);
                logger.Info("StackTrace: " + ex.StackTrace.ToString());
            }

            return PipeLineResponse;

        }

        /// <summary>
        /// Valida Compra de opções para clientes institucionais.
        /// </summary>
        /// <param name="pParametros"></param>
        /// <param name="_responseClienteRiscoParam"></param>
        /// <returns></returns>
        private ValidacaoRiscoResponse ValidarCompraOpcoesInstitucional(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam , CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();

            try
            {
                logger.Info("Cliente Institucional");
                logger.Info("Inicia Rotina de validação de compra de opções, cliente institucional: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
                logger.Info("Caracteristicas da ordem");
                logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
                logger.Info("Operação            : Compra");
                logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
                logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
                logger.Info("Preco               :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
                logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());

                //OBJETO RESPONSAVEL POR GUARDAR TODAS AS CRITICAS FEITAS DURANTE O ENVIO DE UMA ORDEM
                List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
                PipeLineCriticaInfo CriticaInfo = null;

                #region [ Alteração de Ordem ]

                // Verifica se o ID da ordem já existe no banco de dados.
                OrdemInfo OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
                {
                    logger.Info("Alteração de ordem solicitada");
                    logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                    logger.Info("Inicia rotina de validação de alteração de ordens.");

                    return this.AlterarOrdemCompraOpcoesInstitucional(pParametros, OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
                }

                #endregion

                int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
                int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;
    
                if (ModuloLoteNegociacao != 0)
                {               
                    logger.Info("A quantidade digitada deve ser multiplo de 100");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A quantidade digitada deve ser multiplo de 100";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    PipeLineResponse.CriticaInfo = CriticaInfoCollection;

                    return TratamentoRetorno(PipeLineResponse);
                }

                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                    //ENVIA ORDEM PARA O ROTEADOR

                    logger.Info("Envia a ordem para o roteador de ordens");

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem de compra de opção para o cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
                logger.Info("Descrição do erro: " + ex.Message);
                logger.Info("StackTrace: " + ex.StackTrace.ToString());
            }

            return PipeLineResponse;

        }


        #endregion

        #region  Alteracao de Ordens Institucional

        private ValidacaoRiscoResponse AlterarOrdemCompraInstitucional(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse _RespostaOrdem = new ValidacaoRiscoResponse();

            decimal ValorOrdemEnviada = 0;
            decimal VolumeOperacao = 0;
            decimal VolumeOrdemOriginal = 0;
            decimal DiferencialVolumeOrdemAlterada = 0;
            bool BlMercadoIntegral = false;
            bool BlMercadoFracionario = false;

            int Quantidade = 0;
            int QuantidadeOriginal = 0;

            decimal Preco = 0;
            decimal PrecoOriginal = 0;

            int QtdeAux = 0;
            int _LoteNegociacao = 0;

            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
            ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

            Preco = Math.Round(Preco, 2);
            PrecoOriginal = Math.Round(PrecoOriginal, 2);

            // TRATAMENTO [ FRACIONARIO / INTEGRAL ]
            int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
            int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

            DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);

            #region Valida Quantidade da ordem Original

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQtyRemmaining;

            int QuantidadeExecutada = (OrdemInfoAlteracao.OrderQty - OrdemInfoAlteracao.OrderQtyRemmaining);
            pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty + QuantidadeExecutada);

            if ((Preco == PrecoOriginal) && (Quantidade == QuantidadeOriginal))
            {
                logger.Info("A ordem enviada esta identica a original");

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "Os dados enviados estão idênticos aos originais";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(_RespostaOrdem);
            }

            var LotePadrao = CadastroPapeis.Objeto.LoteNegociacao;

            QtdeAux = Math.Abs(Quantidade);

            _LoteNegociacao = (QuantidadeOriginal % int.Parse(LotePadrao));

            if ((_LoteNegociacao) != 0)
            {
                BlMercadoFracionario = true;
            }
            else
            {
                BlMercadoIntegral = true;
            }

            if (BlMercadoIntegral)
            {
                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % int.Parse(LotePadrao) != 0)
                {
                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A Quantidade da ordem deve ser multiplo de: " + LotePadrao;
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                    return TratamentoRetorno(_RespostaOrdem);
                }
            }
            else if (BlMercadoFracionario)
            {
                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= int.Parse(LotePadrao))
                {
                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A Quantidade da ordem deve estar entre 1 e 99 ( Mercado fracionário ).";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                    return TratamentoRetorno(_RespostaOrdem);

                }
            }

            #endregion

            DiferencialVolumeOrdemAlterada = Math.Round(DiferencialVolumeOrdemAlterada, 2);

            // DiferencialVolumeOrdemAlterada = Math.Abs(DiferencialVolumeOrdemAlterada);
            if (DiferencialVolumeOrdemAlterada > 0)
            {

                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                {
                    // ENVIAR INTEGRAL
                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;
                    logger.Info("Inicia operação de compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                        logger.Info("Envia a ordem para o roteador de ordens");

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem institucional enviada com sucesso para o Roteador de Ordens");

                            _RespostaOrdem.DataResposta = DateTime.Now;
                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }
                    }

                    if (ModuloLoteNegociacao > 0)
                    {
                        // ENVIAR FRACIONARIO
                        logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                        // ENVIAR FRACIONARIO
                        pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                        pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                        logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                        // Inseri a Ordem solicitada no banco de dados    
                        if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                        {
                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                            logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                            logger.Info("Envia a ordem para o roteador de ordens");


                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                            ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                _RespostaOrdem.DataResposta = DateTime.Now;
                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                            }
                            else
                            {
                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                            }
                        }
                    }
                }
                else
                {
                    //FRACIONARIO
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                        //ENVIA ORDEM PARA O ROTEADOR                                    
                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        logger.Info("Envia a ordem para o roteador de ordens");


                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                            _RespostaOrdem.DataResposta = DateTime.Now;
                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }

                    }
                }

            }
            else
            {
                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                {
                    // ENVIAR INTEGRAL
                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;
                    logger.Info("Inicia operação de compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                        logger.Info("Envia a ordem para o roteador de ordens");

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem institucional enviada com sucesso para o Roteador de Ordens");

                            _RespostaOrdem.DataResposta = DateTime.Now;
                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }
                    }

                    if (ModuloLoteNegociacao > 0)
                    {
                        // ENVIAR FRACIONARIO
                        logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                        // ENVIAR FRACIONARIO
                        pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                        pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                        logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                        // Inseri a Ordem solicitada no banco de dados    
                        if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                        {
                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                            logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                            logger.Info("Envia a ordem para o roteador de ordens");


                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                            ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                _RespostaOrdem.DataResposta = DateTime.Now;
                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                            }
                            else
                            {
                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                            }
                        }
                    }
                }
                else
                {
                    //FRACIONARIO
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                        //ENVIA ORDEM PARA O ROTEADOR                                    
                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        logger.Info("Envia a ordem para o roteador de ordens");


                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                            _RespostaOrdem.DataResposta = DateTime.Now;
                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }

                    }
                }

            }

            return _RespostaOrdem;
        }

        private ValidacaoRiscoResponse AlterarOrdemVendaInstitucional(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse _RespostaOrdem = new ValidacaoRiscoResponse();

            decimal ValorOrdemEnviada = 0;
            decimal VolumeOperacao = 0;
            decimal VolumeOrdemOriginal = 0;
            decimal DiferencialVolumeOrdemAlterada = 0;
            bool BlMercadoIntegral = false;
            bool BlMercadoFracionario = false;

            int Quantidade = 0;
            int QuantidadeOriginal = 0;

            decimal Preco = 0;
            decimal PrecoOriginal = 0;

            int QtdeAux = 0;
            int _LoteNegociacao = 0;

            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
            ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

            Preco = Math.Round(Preco, 2);
            PrecoOriginal = Math.Round(PrecoOriginal, 2);

            // TRATAMENTO [ FRACIONARIO / INTEGRAL ]
            int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
            int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

            DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);
            DiferencialVolumeOrdemAlterada = Math.Abs(DiferencialVolumeOrdemAlterada);

            #region Valida Quantidade da ordem Original

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

            int QuantidadeExecutada = (OrdemInfoAlteracao.OrderQty - OrdemInfoAlteracao.OrderQtyRemmaining);
            pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty + QuantidadeExecutada);
    
            if ((Preco == PrecoOriginal) && (Quantidade == QuantidadeOriginal))
            {
                logger.Info("A ordem enviada esta identica a original");

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "Os dados enviados estão idênticos aos originais";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(_RespostaOrdem);
            }

        
            var LotePadrao = CadastroPapeis.Objeto.LoteNegociacao;

            QtdeAux = Math.Abs(Quantidade);

            _LoteNegociacao = (QuantidadeOriginal % int.Parse(LotePadrao));

            if ((_LoteNegociacao) != 0)
            {
                BlMercadoFracionario = true;
            }
            else
            {
                BlMercadoIntegral = true;
            }

            if (BlMercadoIntegral)
            {
                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % int.Parse(LotePadrao) != 0)
                {
                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A Quantidade da ordem deve ser multiplo de: " + LotePadrao;
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                    return TratamentoRetorno(_RespostaOrdem);
                }
            }
            else if (BlMercadoFracionario)
            {
                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                {
                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A Quantidade da ordem deve estar contida no mercado fracionário.";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                    return TratamentoRetorno(_RespostaOrdem);

                }
            }

            #endregion

           // DiferencialVolumeOrdemAlterada = Math.Abs(DiferencialVolumeOrdemAlterada);
            if (DiferencialVolumeOrdemAlterada != 0)
            {

                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                {
                    // ENVIAR INTEGRAL
                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;
                    logger.Info("Inicia operação de compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                        logger.Info("Envia a ordem para o roteador de ordens");

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem institucional enviada com sucesso para o Roteador de Ordens");

                            _RespostaOrdem.DataResposta = DateTime.Now;
                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }
                    }

                    if (ModuloLoteNegociacao > 0)
                    {
                        // ENVIAR FRACIONARIO
                        logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                        // ENVIAR FRACIONARIO
                        pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                        pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                        logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                        // Inseri a Ordem solicitada no banco de dados    
                        if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                        {
                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                            logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                            logger.Info("Envia a ordem para o roteador de ordens");


                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                            ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                _RespostaOrdem.DataResposta = DateTime.Now;
                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                            }
                            else
                            {
                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                            }
                        }
                    }
                }
                else
                {
                    //FRACIONARIO
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                        //ENVIA ORDEM PARA O ROTEADOR                                    
                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        logger.Info("Envia a ordem para o roteador de ordens");


                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                            _RespostaOrdem.DataResposta = DateTime.Now;
                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }

                    }
                }

            }

            return _RespostaOrdem;
        }

        private ValidacaoRiscoResponse AlterarOrdemCompraOpcoesInstitucional(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();

            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse _RespostaOrdem = new ValidacaoRiscoResponse();

            decimal ValorOrdemEnviada = 0;
            decimal VolumeOperacao = 0;
            decimal VolumeOrdemOriginal = 0;
            decimal DiferencialVolumeOrdemAlterada = 0;

            int Quantidade = 0;
            int QuantidadeOriginal = 0;

            decimal Preco = 0;
            decimal PrecoOriginal = 0;

            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

            Preco = Math.Round(Preco, 2);
            PrecoOriginal = Math.Round(PrecoOriginal, 2);

            if ((Preco == PrecoOriginal) && (Quantidade == QuantidadeOriginal))
            {
                logger.Info("A ordem enviada esta identica a original");
                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "Os dados enviados estão idênticos aos originais";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(_RespostaOrdem);
            }

            // Verifica a quantidade 
            var LotePadrao = CadastroPapeis.Objeto.LoteNegociacao;

            if ((pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % int.Parse(LotePadrao.ToString()) != 0))
            {
                logger.Info("A quantidade digitada deve ser multiplo de:" + LotePadrao.ToString());

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "A quantidade digitada deve ser multiplo de:" + LotePadrao.ToString();
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                return TratamentoRetorno(_RespostaOrdem);
            }


            VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
            ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

            DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);

            //DiferencialVolumeOrdemAlterada = Math.Abs(DiferencialVolumeOrdemAlterada);
            if (DiferencialVolumeOrdemAlterada > 0)
            {

                int QuantidadeExecutada = (OrdemInfoAlteracao.OrderQty - OrdemInfoAlteracao.OrderQtyRemmaining);
                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty + QuantidadeExecutada);

                string PapelBase = CadastroPapeis.Objeto.PapelObjeto;

                try
                {
                    VolumeOperacao = DiferencialVolumeOrdemAlterada;

                    logger.Info("Verifica se a ordem enviada é uma alteração ou nova ordem");

                    if (pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID != null)
                    {
                        logger.Info("Resposta: Alteração de Ordem");
                    }
                    else
                    {
                        logger.Info("Resposta: Inclusão de Oferta");
                    }

                    logger.Info("Inicia validação de permissão para operar no mercado de opções.");



                    // Verifica a quantidade 
                    if ((pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % int.Parse(LotePadrao.ToString()) != 0))
                    {

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = CriticaInfo.Critica = "A quantidade digitada deve ser multiplo de:" + LotePadrao.ToString();
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                        _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                        return TratamentoRetorno(_RespostaOrdem);
                    }

                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                        //ENVIA ORDEM PARA O ROTEADOR

                        logger.Info("Envia a ordem para o roteador de ordens");

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                            _RespostaOrdem.DataResposta = DateTime.Now;
                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Info("Ocorreu um erro ao tentar modificar a oferta. " + ex.Message);
                }
            }
            else
            {

                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                    //ENVIA ORDEM PARA O ROTEADOR
                    logger.Info("Envia a ordem para o roteador de ordens");

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                        _RespostaOrdem.DataResposta = DateTime.Now;
                        _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);
                    }
                }

            }

            return _RespostaOrdem;
        }

        private ValidacaoRiscoResponse AlterarOrdemVendaOpcoesInstitucional(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();

            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse _RespostaOrdem = new ValidacaoRiscoResponse();

            decimal ValorOrdemEnviada = 0;
            decimal VolumeOperacao = 0;
            decimal VolumeOrdemOriginal = 0;
            decimal DiferencialVolumeOrdemAlterada = 0;

            int Quantidade = 0;
            int QuantidadeOriginal = 0;

            decimal Preco = 0;
            decimal PrecoOriginal = 0;

            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
            ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

            Preco = Math.Round(Preco, 2);
            PrecoOriginal = Math.Round(PrecoOriginal, 2);         

            if ((Preco == PrecoOriginal) && (Quantidade == QuantidadeOriginal))
            {
                logger.Info("A ordem enviada esta identica a original");
                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "Os dados enviados estão idênticos aos originais";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(_RespostaOrdem);
            }

            // Verifica a quantidade 
            var LotePadrao = CadastroPapeis.Objeto.LoteNegociacao;

            if ((pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % int.Parse(LotePadrao.ToString()) != 0))
            {
                logger.Info("A quantidade digitada deve ser multiplo de :" + LotePadrao.ToString());

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "A quantidade digitada deve ser multiplo de:" + LotePadrao.ToString();
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                return TratamentoRetorno(_RespostaOrdem);
            }


            DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);

            // DiferencialVolumeOrdemAlterada = Math.Abs(DiferencialVolumeOrdemAlterada);

            if (DiferencialVolumeOrdemAlterada > 0)
            {
                // CLIENTE NÃO POSSUI CUSTODIA SUFICIENTE PARA ZERAR O PAPEL.
                // VERIFICAR SE ESTA FAZENDO UM LANÇAMENTO DESCOBERTO COM GARANTIA NA CARTEIRA 27
                string PapelBase = CadastroPapeis.Objeto.PapelObjeto;

                int QuantidadeExecutada = (OrdemInfoAlteracao.OrderQty - OrdemInfoAlteracao.OrderQtyRemmaining);
                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty + QuantidadeExecutada);

                try
                {
                    VolumeOperacao = DiferencialVolumeOrdemAlterada;

                    logger.Info("Verifica se a ordem enviada é uma alteração ou nova ordem");

                    if (pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID != null)
                    {
                        logger.Info("Resposta: Alteração de Ordem");
                    }
                    else
                    {
                        logger.Info("Resposta: Inclusão de Oferta");
                    }

                    logger.Info("Inicia validação de permissão para operar no mercado de opções.");

             

                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                        //ENVIA ORDEM PARA O ROTEADOR
                        logger.Info("Envia a ordem para o roteador de ordens");

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                            _RespostaOrdem.DataResposta = DateTime.Now;
                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Info("Ocorreu um erro ao tentar modificar a oferta. " + ex.Message);
                }
            }
            else
            {
                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                    //ENVIA ORDEM PARA O ROTEADOR
                    logger.Info("Envia a ordem para o roteador de ordens");

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    ExecutarModificacaoOrdensResponse RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                        _RespostaOrdem.DataResposta = DateTime.Now;
                        _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);
                    }
                }

            }

            return _RespostaOrdem;
        }

                                      
        #endregion

        #endregion

        /// <summary>
        /// Método responsável por retornar todos os parametros e permissões do cliente no sistema
        /// </summary>
        /// <param name="pParametros">Código do cliente</param>
        /// <returns>Lista de parametros e permissões</returns>
        public ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> VerificarPermissaoCliente(ClienteParametrosPermissoesRequest pParametros)
        {
            // OBJETO DE RESPOSTA
            ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _ResponseParametrosPermissoes =
                new ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo>();

            try
            {

                // PERMISSOES E PARAMETROS         
                ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> ParametrosPermissoesColecao =
                 new ServicoRisco().CarregarParametosPermissoesCliente(
                                                                        new ClienteParametrosPermissoesRequest()
                                                                        {
                                                                            IdCliente = pParametros.IdCliente
                                                                        });

                // EFETUA A PESQUISA PARA O PARAMETRO / PERMISSAO SELECIONADO.
                var ColecaoParametrosPermissoes = from p in ParametrosPermissoesColecao.ColecaoObjeto
                                                  where p.IdParametroPermissao == (int)pParametros.ParametroPermissaoEnum

                                                  select p;

                _ResponseParametrosPermissoes.ColecaoObjeto = new List<ClienteParametroPermissaoInfo>();
                _ResponseParametrosPermissoes.DataResposta = DateTime.Now;
                _ResponseParametrosPermissoes.StatusResposta = CriticaMensagemEnum.Sucesso;
                _ResponseParametrosPermissoes.DescricaoResposta = "Parametros e Permissões carregados";

                // Preenche resultado.
                foreach (ClienteParametroPermissaoInfo Permissao in ColecaoParametrosPermissoes)
                {
                    _ResponseParametrosPermissoes.ColecaoObjeto.Add(Permissao);
                }

            }
            catch (Exception ex)
            {
                PipeLineCriticaInfo Info = new PipeLineCriticaInfo();

                _ResponseParametrosPermissoes.DescricaoResposta = "Ocorreu um erro ao carregar as permissões do cliente" + ex.Message;
                _ResponseParametrosPermissoes.DataResposta = DateTime.Now;
                _ResponseParametrosPermissoes.StatusResposta = CriticaMensagemEnum.Sucesso;

            }

            return _ResponseParametrosPermissoes;
        }

        /// <summary>
        /// Método responsável por retornar o código do cliente na atividade BMF.
        /// </summary>
        /// <param name="pParametros">Código do cliente na atividade de bolsa</param>
        /// <returns>Código do cliente na atividade BMF.</returns>
        public ClienteAtividadeBmfResponse ObterCodigoClienteAtividadeBmf(ClienteAtividadeBmfRequest lParametro)
        {
            return new PersistenciaRisco().ObterCodigoBmfCliente(lParametro);
        }
     
        #region Métodos de apoio

        /// <summary>
        /// Obtem os bloqueios do cliente
        /// </summary>
        /// <param name="CodigoCliente">código do cliente</param>
        /// <returns>Bloqueios de custodia do cliente</returns>
        private ClienteCustodiaBloqueioResponse<ClienteCustodiaBloqueioInfo> ObterCustodiaBloqueadaCliente(int CodigoCliente, int CodigoCarteira, string InstrumentoBase)
        {

            // CARREGA BLOQUEIOS DO CLIENTE
            ClienteCustodiaBloqueioRequest<ClienteCustodiaBloqueioInfo> requestQtdeBloqueada = new ClienteCustodiaBloqueioRequest<ClienteCustodiaBloqueioInfo>();

            requestQtdeBloqueada.Objeto = new ClienteCustodiaBloqueioInfo();
            requestQtdeBloqueada.Objeto.IdCliente = CodigoCliente;
            requestQtdeBloqueada.Objeto.IdCarteira = CodigoCarteira;
            requestQtdeBloqueada.Objeto.InstrumentoBase = InstrumentoBase;

            return new ServicoCustodia().ListarBloqueioCliente(requestQtdeBloqueada);

        }

        /// <summary>
        /// Gera control numbber 
        /// </summary>
        private string CtrlNumber
        {
            get
            {
                return string.Format("{0}{1}{2}",
                        DateTime.Now.ToString("ddMMyyyyhhmmss").Replace("/", string.Empty).Replace(" ", string.Empty).Replace(":", string.Empty),
                        "-", new Random().Next(0, 99999999).ToString());
            }
        }


        /// <summary>
        /// Gera o digito do cliente nas atividades Bovespa e BMF e concatena com a conta informada
        /// </summary>
        /// <param name="CodigoCorretora">Código da Corretora</param>
        /// <param name="CodigoCliente">Código do cliente na corretora ( BOVESPA/BMF)</param>
        /// <returns></returns>
        private int RetornaCodigoCliente(int CodigoCorretora, int CodigoCliente)
        {

            int valor = 0;

            valor = (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5) +

            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            valor = valor % 11;

            if (valor == 0 || valor == 1)
            {
                valor = 0;
            }
            else
            {
                valor = 11 - valor;
            }

            return int.Parse(string.Format("{0}{1}", CodigoCliente, valor));

        }

        /// <summary>
        /// Formata a resposta da requisição feita para a validação do risco.
        /// </summary>
        /// <param name="PipeLineResponse">Objeto de validação de risco preenchido</param>
        /// <returns>Objeto de validação de risco formatado</returns>
        private ValidacaoRiscoResponse TratamentoRetorno(ValidacaoRiscoResponse PipeLineResponse)
        {
            PipeLineResponse.DataResposta = DateTime.Now;

            if ((PipeLineResponse.CriticaInfo != null) && (PipeLineResponse.CriticaInfo.Count > 0))
            {
                PipeLineResponse.DescricaoResposta = string.Format("{0}{1}{2}", "O controle de risco encontrou <", PipeLineResponse.CriticaInfo.Count.ToString(), "> item(s) a ser(em) verificado(s).");
                PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
            }
            else
            {
                // PipeLineResponse.DescricaoResposta = "Solicitação efetuada com sucesso";
                // PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
            }

            return PipeLineResponse;

        }


        private ListaInstrumentosAlavancagemResponse<ListaInstrumentoAlavancagemInfo> ObterListaInstrumentosAlavancagem()
        {
            ListaInstrumentosAlavancagemResponse<ListaInstrumentoAlavancagemInfo> Resposta = null;
            try
            {
                Resposta = new PersistenciaRisco().CarregarInstrumentosAlavancagem();

                Resposta.StatusResposta = CriticaMensagemEnum.Sucesso;
                Resposta.DescricaoResposta = "Instrumentos de alavancagem carregados com sucesso";

                return Resposta;
            }
            catch (Exception ex)
            {
                Resposta.StatusResposta = CriticaMensagemEnum.ErroNegocio;
                Resposta.DescricaoResposta = ex.Message;

            }
            return Resposta;

        }

        /// <summary>
        /// Obtem lista completa das opcoes da serie corrente e seus reespectivos vencimentos
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns>Lista de opcoes corrente e seu vencimento</returns>
        public VencimentoInstrumentoResponse ObterVencimentoInstrumentos(VencimentoInstrumentoRequest pRequest)
        {
            return new PersistenciaRisco().CarregarVencimentoOpcoes(pRequest);
        }


        #region  Persistencia Inclusão / Alteracao de ordens ( Banco de Dados )
        /// <summary>
        /// Inseri um novo cliente na tabela de ordens do sistema sinacor
        /// </summary>
        /// <param name="pParametros"> informações da ordem do cliente </param>
        /// <returns> True(sucesso) / False(Fracasso) </returns>
        private bool InserirOrdemCliente(EnviarOrdemRoteadorRequest pParametros)
        {
            pParametros.OrdemInfo.OrderQtyRemmaining = pParametros.OrdemInfo.OrderQty;

            // Inseri a Ordem solicitada no banco de dados
            return new PersistenciaOrdens().InserirOrdemCliente(pParametros.OrdemInfo);

        }

        /// <summary>
        /// Metodo responsavel por atualizar a ordem no banco de dados
        /// </summary>
        /// <param name="pParametros">informações da ordem</param>
        /// <returns></returns>
        private bool AtualizaOrdemCliente(EnviarOrdemRoteadorRequest pParametros)
        {
            // Inseri a Ordem solicitada no banco de dados
            return new PersistenciaOrdens().InserirOrdemCliente(pParametros.OrdemInfo);

        }

        #endregion

        #endregion

        #region  Alteração de Ordem cliente normal

        #region [ Alteração de ordens no mercado A vista ]

        /// <summary>
        /// Método responsavel por alterar uma oferta de compra no mercado a vista
        /// </summary>
        /// <returns></returns>
        private ValidacaoRiscoResponse AlterarOrdemCompra(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();

            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse _RespostaOrdem = new ValidacaoRiscoResponse();

            //OBJETO RESPONSAVEL POR ARMAZENAR O SALDO E CONTA CORRENTE DO CLIENTE
            SaldoContaCorrenteResponse<ContaCorrenteInfo> SaldoContaCorrenteResponse = null;

            decimal ValorOrdemEnviada = 0;
            decimal VolumeOperacao = 0;
            decimal VolumeOrdemOriginal = 0;
            decimal DiferencialVolumeOrdemAlterada = 0;
            bool BlMercadoIntegral = false;
            bool BlMercadoFracionario = false;

            int Quantidade = 0;
            int QuantidadeOriginal = 0;

            decimal Preco = 0;
            decimal PrecoOriginal = 0;

            int QtdeAux = 0;
            int _LoteNegociacao = 0;


            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
            ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Preco = Math.Round(Preco, 2);
            PrecoOriginal = Math.Round(PrecoOriginal, 2);

            int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);            

            DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);

            #region Valida Quantidade da ordem Original

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

            int QuantidadeExecutada = (OrdemInfoAlteracao.OrderQty - OrdemInfoAlteracao.OrderQtyRemmaining);
            pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty + QuantidadeExecutada);

            if ((Preco == PrecoOriginal) && (Quantidade == QuantidadeOriginal))
            {
                logger.Info("A ordem enviada esta identica a original");
                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "Os dados enviados estão idênticos aos originais";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(_RespostaOrdem);
            }


            var LotePadrao = CadastroPapeis.Objeto.LoteNegociacao;

            QtdeAux = Math.Abs(Quantidade);

            _LoteNegociacao = (QuantidadeOriginal % int.Parse(LotePadrao));

            if ((_LoteNegociacao) != 0)
            {
                BlMercadoFracionario = true;
                //pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
            }
            else
            {
                BlMercadoIntegral = true;
            }

            if (BlMercadoIntegral)
            {
                if (Quantidade % int.Parse(LotePadrao) != 0)
                {
                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A Quantidade da ordem deve ser multiplo de: " + LotePadrao;
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                    return TratamentoRetorno(_RespostaOrdem);
                }
            }
            else if (BlMercadoFracionario)
            {
                if (Quantidade >= int.Parse(LotePadrao))
                {
                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A Quantidade da ordem deve estar entre 1 e 99 ( Mercado fracionário ).";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                    return TratamentoRetorno(_RespostaOrdem);

                }
            }

            #endregion

           // DiferencialVolumeOrdemAlterada = Math.Abs(DiferencialVolumeOrdemAlterada);

            if (DiferencialVolumeOrdemAlterada > 0)
            {
                logger.Info("Diferencial Volume da ordem alterada > 0 ");

                logger.Info("Volume de ordem enviado maior que o volume original");

                VolumeOperacao = DiferencialVolumeOrdemAlterada;

                #region "PipeLine Alteração de ordens"

                try
                {
                    // VERIFICA PERMISSAO NO MERCADO A VISTA
                    logger.Info("Valida Permissão do cliente para operar no mercado a vista");
                    var PermissaoOperarMercadoAVista = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                       where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_NO_MERCADO_AVISTA

                                                       select p;

                    if (PermissaoOperarMercadoAVista.Count() == 0)
                    {
                        logger.Info("Permissão de operar no mercado a vista negada.");
                    }

                    // CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO A VISTA
                    if (PermissaoOperarMercadoAVista.Count() > 0)
                    {
                        logger.Info("Permissão de operar no mercado a vista concedida com sucesso.");

                        //**************************************************************************
                        // 3º VALIDA SALDO EM CONTA CORRENTE DO CLIENTE
                        //**************************************************************************

                        VolumeOperacao = DiferencialVolumeOrdemAlterada;

                        logger.Info("Volume solicitado para a operação: " + string.Format("{0:c}", VolumeOperacao));
                        logger.Info("Inicia consulta de saldo em conta corrente");

                        //OBTEM O SALDO TOTAL DE CONTA CORRENTE DO CLIENTE
                        SaldoContaCorrenteResponse = new ServicoContaCorrente().ObterSaldoContaCorrente(
                          new SaldoContaCorrenteRequest()
                          {
                              IdCliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                          });


                        // Obtem total de bloqueios ( Ofertas em aberto ) do cliente
                        Nullable<Decimal> SaldoBloqueado = SaldoContaCorrenteResponse.Objeto.SaldoBloqueado;

                        if (SaldoContaCorrenteResponse.StatusResposta == ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                        {
                            // Forma o saldo para em ações
                            SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes = (SaldoContaCorrenteResponse.Objeto.SaldoD0 + SaldoContaCorrenteResponse.Objeto.SaldoD1 + SaldoContaCorrenteResponse.Objeto.SaldoD2 + SaldoContaCorrenteResponse.Objeto.SaldoD3 + decimal.Parse(SaldoContaCorrenteResponse.Objeto.SaldoContaMargem.ToString()));
                            SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes = SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes + decimal.Parse(SaldoContaCorrenteResponse.Objeto.SaldoBloqueado.ToString());

                            logger.Info("Saldo em conta corrente carregado com sucesso. Saldo em compra de ações: " + string.Format("{0:c}", SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes.ToString()));

                            // TRATAMENTO [ FRACIONARIO / INTEGRAL ]                      
                            int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

                            logger.Info("Verifica se o saldo em conta corrente é suficiente para cobrir a operação");

                            // VERIFICA SE O SALDO EM CONTA CORRENTE CONSEGUE COBRIR A OPERACAO
                            // SEM UTILIZACAO DE LIMITE OPERACIONAL
                            if (VolumeOperacao <= SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes)
                            {
                                logger.Info("Saldo em CC suficiente para efetuar a compra sem a utilização de limite");

                                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                                {
                                    // ENVIAR INTEGRAL
                                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;
                                    logger.Info("Inicia operação de compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");


                                    // Inseri a Ordem solicitada no banco de dados    
                                    if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                    {
                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                                        logger.Info("Envia a ordem para o roteador de ordens");

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }
                                    }


                                    if (ModuloLoteNegociacao > 0)
                                    {

                                        logger.Info("Calcula restante a ser enviado para o mercado fracionário");


                                        // ENVIAR FRACIONARIO
                                        pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                        pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                        logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                                        // Inseri a Ordem solicitada no banco de dados    
                                        if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                        {
                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                            logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                            logger.Info("Envia a ordem para o roteador de ordens");


                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                            var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                            {
                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                _RespostaOrdem.DataResposta = DateTime.Now;
                                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                            }
                                            else
                                            {
                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // ENVIAR FRACIONARIO                       
                                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                                    logger.Info("Codigo do papel: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " Ctrl Number original " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString() + " Ctrl Number Novo: " + pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID.ToString());

                                    // Inseri a Ordem solicitada no banco de dados    
                                    if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                    {
                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                                        //ENVIA ORDEM PARA O ROTEADOR                                    
                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                        logger.Info("Envia a ordem para o roteador de ordens");

                                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }

                                    }
                                }
                            }
                            else
                            {
                                // VOLUME DE COMPRA E MAIOR QUE O SALDO EM CONTA CORRENTE
                                if (VolumeOperacao > SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes)
                                {
                                    logger.Info("Volume da operação de compra  maior que o saldo disponivel em ações");

                                    /**************************************************************************
                                    // 4º VALIDA PERMISSAO PARA UTILIZAÇÃO DE LIMITE NO MERCADO A VISTA
                                    //**************************************************************************/

                                    logger.Info("Verifica os Limites cadastrados para o cliente");

                                    logger.Info("Inicia consulta para utilizar limite no mercado a vista");

                                    var PermissaoUtilizarLimiteAVista = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                        where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_MERCADO_AVISTA
                                                                        orderby p.DtMovimento descending

                                                                        select p;

                                    if (PermissaoUtilizarLimiteAVista.Count() == 0)
                                    {
                                        logger.Info("Cliente não possui permissão para utilizar limite operacional");
                                    }

                                    //VERIFICA SE O CLIENTE POSSUI PERMISAO PARA UTILIZACAO DE LIMITE OPERACIONAL
                                    if (PermissaoUtilizarLimiteAVista.Count() > 0)
                                    {
                                        //CLIENTE POSSUI PERMISSAO PARA UTILIZAR LIMITE OPERACIONAL     
                                        logger.Info("Cliente possui permissao para utilizar limite operacional");

                                        #region Ofertas 30 dias

                                        if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                        {
                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Não é possível abrir oferta de ações com validade para 30 dias utilizando Limite Operacional";
                                            CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                            _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                            _RespostaOrdem.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                            _RespostaOrdem.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                            return _RespostaOrdem;
                                        }

                                        #endregion

                                        //OBTEM SALDO EM CONTA CORRENTE + CONTA MARGEM DO CLIENTE                                                        
                                        decimal SaldoDisponivelAcoes = SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes.DBToDecimal();

                                        /***********************************************************************************
                                         * 4º OBTEM VALOR DO LIMITE PARA O MERCADO A VISTA
                                         * *********************************************************************************/

                                        var ValorLimiteMercadoAVista = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                       where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_MERCADO_AVISTA
                                                                       && p.DtValidade >= DateTime.Now
                                                                       orderby p.DtMovimento descending

                                                                       select p;

                                        decimal SaldoLimiteOperacional = 0;
                                        decimal ValorAlocado = 0;
                                        int idClienteParametroPermissao = 0;

                                        // SOMA O SALDO DE CONTA CORRENTE COM O LIMITE OPERACIONAL DO CLIENTE.
                                        foreach (var ClientePosicao in ValorLimiteMercadoAVista)
                                        {
                                            SaldoLimiteOperacional = ClientePosicao.ValorParametro;
                                            ValorAlocado = ClientePosicao.ValorAlocado;
                                            idClienteParametroPermissao = ClientePosicao.IdClienteParametroPermissao;
                                            break;
                                        }

                                        logger.Info("Limite Operacional carregado com sucesso, valor disponivel em limite operacional :" + string.Format("{0:c}", SaldoLimiteOperacional));
                                        logger.Info("Saldo em conta corrente + Limite Operacional: " + string.Format("{0:c}", (SaldoDisponivelAcoes + SaldoLimiteOperacional)));

                                        // VERIFICA SE O SALDO EM CONTA CORRENTE + LIMITE OPERACIONAL 
                                        // É CAPAZ DE COBRIR A COMPRA A SER REALIZADA.

                                        decimal? SaldoRestante = (SaldoDisponivelAcoes - ValorAlocado);
                                        decimal? LimiteTotal = (SaldoLimiteOperacional + ValorAlocado);

                                        decimal? valor = ((SaldoDisponivelAcoes) + LimiteTotal);

                                        if (((SaldoDisponivelAcoes) + LimiteTotal) >= VolumeOperacao)
                                        {
                                            //TODO: INVOCAR SERVICO ENVIO ORDENS
                                            decimal ValorEmprestimo = 0;

                                            if ((SaldoDisponivelAcoes - ValorAlocado) > 0)
                                            {
                                                //Subtrai do saldo de ações
                                                ValorEmprestimo = (SaldoDisponivelAcoes - VolumeOperacao);
                                            }
                                            else
                                            {
                                                //Caso o saldo em ações seja menor ou igual a zero atribui o valor do volume da operacao
                                                ValorEmprestimo = VolumeOperacao;
                                            }

                                            string Historico = string.Empty;

                                            Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                            Historico = " Alteracao de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                            Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();
                                            Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();


                                            logger.Info("Atualiza o limite operacional do cliente");
                                            // Atualiza e bloqueia limite do cliente antes de enviar a ordem 
                                            bool BLAtualizacaoLimite = new PersistenciaRisco().AtualizaLimiteCliente(
                                                idClienteParametroPermissao,
                                                Math.Abs(ValorEmprestimo),
                                                Historico,
                                                 pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID
                                                );

                                            if (BLAtualizacaoLimite == true)
                                            {

                                                //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                                                           
                                                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                                                {
                                                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                                    // ENVIAR INTEGRAL
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;

                                                    // Inseri a Ordem solicitada no banco de dados    
                                                    if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                                    {
                                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                        logger.Info("Alteracao de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                                        // ENVIA ORDEM PARA O ROTEADOR                                                   
                                                        logger.Info("Envia a ordem para o roteador de ordens");

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);

                                                        }

                                                    }

                                                    // ENVIAR FRACIONARIO
                                                    if (ModuloLoteNegociacao > 0)
                                                    {

                                                        logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                                        pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                                        logger.Info("Inicia aletracao de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                                                        // Inseri a Ordem solicitada no banco de dados    
                                                        if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                                        {
                                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                            logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                            // ENVIA ORDEM PARA O ROTEADOR                                                    
                                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                            logger.Info("Envia a ordem para o roteador de ordens");

                                                            var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                            {
                                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                                _RespostaOrdem.DataResposta = DateTime.Now;
                                                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                            }
                                                            else
                                                            {
                                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                                CriticaInfo = new PipeLineCriticaInfo();
                                                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                                // Adiciona as criticas no pipeline de risco.
                                                                CriticaInfoCollection.Add(CriticaInfo);

                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // ENVIAR FRACIONARIO                                               
                                                    logger.Info("Inicia alteracao de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                                    // Inseri a Ordem solicitada no banco de dados    
                                                    if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                                    {
                                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                        logger.Info("Alteracao de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                        logger.Info("Envia a ordem para o roteador de ordens");

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);

                                                        }
                                                    }
                                                }
                                            }

                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS
                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                            _RespostaOrdem.DescricaoResposta = "Solicitação encaminhada para o serviço de ordens";
                                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;

                                        }
                                        else
                                            if (VolumeOperacao > (SaldoDisponivelAcoes + SaldoLimiteOperacional))
                                            {

                                                logger.Info("Saldo em conta corrente insuficiente para realizar a operação.");

                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = "Saldo em conta corrente insuficiente para realizar a operação";
                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoContaCorrenteInsuficiente;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;


                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);
                                            }
                                    }
                                    else
                                    {
                                        // CLIENTE NÃO POSSUI PERMISSAO PARA UTILIZAR LIMITE
                                        // SALDO EM CONTA CORRENTE INSUFICIENTE                                   

                                        logger.Info("Saldo em conta corrente insuficiente para realizar a operação");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Saldo em conta corrente insuficiente para realizar a operação";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoContaCorrenteInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        logger.Info("Cliente não possui permissão para utlizar limite operacional");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Cliente não possui permissão para utlizar limite operacional";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                    }
                                }
                            }
                        }
                        else
                        {

                            if (SaldoContaCorrenteResponse.StatusResposta == ContaCorrente.Lib.Enum.CriticaMensagemEnum.Exception)
                            {
                                logger.Info("Ocorreu um erro ao carregar o saldo em conta corrente do cliente");
                            }
                        }
                    }
                    else
                    { // CLIENTE NAO POSSUI PERMISSAO PARA OPERAR NO MERCADO A VISTA                   

                        logger.Info("Cliente não possui permissão insuficiente para operar no mercado vista");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = "Cliente não possui permissão suficiente para operar no mercado a vista";
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);
                    }
                }
                catch (Exception ex)
                {

                    logger.Info("Ocorreu um erro ao invocar o método ValidarCompraAcoes");
                    logger.Info("Descrição do erro    :" + ex.Message);

                    _RespostaOrdem.DataResposta = DateTime.Now;
                    _RespostaOrdem.StackTrace = ex.StackTrace;
                    _RespostaOrdem.DescricaoResposta = ex.Message;
                    _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Exception;

                }

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;


                #endregion

            }
            else
            {

                VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
                ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

                DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);

                Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
                QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

                var LotePadraoNegociacao = CadastroPapeis.Objeto.LoteNegociacao;

                QtdeAux = Math.Abs(Quantidade);

                _LoteNegociacao = (Quantidade % int.Parse(LotePadrao));

                if ((_LoteNegociacao) != 0)
                {
                    BlMercadoFracionario = true;
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                }
                else
                {
                    BlMercadoIntegral = true;
                }

                logger.Info("Diferencial Volume da ordem alterada <= 0 ");

                VolumeOperacao = (pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.DBToDecimal());

                #region ["Envia Ordem direto para o roteador"]

                pParametros.EnviarOrdemRequest.OrdemInfo.OrdStatus = OrdemStatusEnum.SUBSTITUICAOSOLICITADA;

                // Atualiza o status da ordem do cliente.
                if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));


                    //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Alteracao de ordem enviada com sucesso para o Roteador de Ordens");

                        _RespostaOrdem.DataResposta = DateTime.Now;
                        _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a alteracao de ordens para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }

                }

                #endregion
            }

            return TratamentoRetorno(_RespostaOrdem);
        }

        /// <summary>
        /// Método responsavel por alterar uma oferta de venda no mercado a vista
        /// </summary>
        /// <returns></returns>
        private ValidacaoRiscoResponse AlterarOrdemVenda(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();

            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse _RespostaOrdem = new ValidacaoRiscoResponse();

            //OBJETO RESPONSAVEL POR ARMAZENAR O SALDO EM CUSTODIA DO CLIENTE
            SaldoCustodiaResponse<CustodiaClienteInfo> SaldoCustodiaCliente = null;

            decimal ValorOrdemEnviada = 0;
            decimal VolumeOperacao = 0;
            decimal VolumeOrdemOriginal = 0;
            decimal DiferencialVolumeOrdemAlterada = 0;
            decimal QuantidadePosicao = 0;
            bool BlMercadoIntegral = false;
            bool BlMercadoFracionario = false;

            int Quantidade = 0;
            int QuantidadeOriginal = 0;
            int QuantidadeOfertaEmAberto = 0;

            decimal Preco = 0;
            decimal PrecoOriginal = 0;

            int QtdeAux = 0;
            int _LoteNegociacao = 0;


            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
            ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Preco = Math.Round(Preco, 2);
            PrecoOriginal = Math.Round(PrecoOriginal, 2);

            DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);

            #region Valida Quantidade da ordem Original

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

            int QuantidadeExecutada = (OrdemInfoAlteracao.OrderQty - OrdemInfoAlteracao.OrderQtyRemmaining);
            pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty + QuantidadeExecutada);

            if ((Preco == PrecoOriginal) && (Quantidade == QuantidadeOriginal))
            {
                logger.Info("A ordem enviada esta identica a original");

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "Os dados enviados estão idênticos aos originais";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(_RespostaOrdem);
            }


            var LotePadrao = CadastroPapeis.Objeto.LoteNegociacao;

            QtdeAux = Math.Abs(Quantidade);

            _LoteNegociacao = (QuantidadeOriginal % int.Parse(LotePadrao));

            if ((_LoteNegociacao) != 0)
            {
                BlMercadoFracionario = true;
                //pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
            }
            else
            {
                BlMercadoIntegral = true;
            }

            if (BlMercadoIntegral)
            {
                if (Quantidade % int.Parse(LotePadrao) != 0)
                {
                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A Quantidade da ordem deve ser multiplo de: " + LotePadrao;
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                    return TratamentoRetorno(_RespostaOrdem);
                }
            }
            else if (BlMercadoFracionario)
            {
                if (Quantidade >= int.Parse(LotePadrao))
                {
                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A Quantidade da ordem deve estar contida no mercado fracionário.";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                    return TratamentoRetorno(_RespostaOrdem);

                }
            }


            #endregion

           // DiferencialVolumeOrdemAlterada = Math.Abs(DiferencialVolumeOrdemAlterada);
            if (DiferencialVolumeOrdemAlterada >= 0)
            {
                try
                {
                    logger.Info("Consulta permissão para vender no mercado a vista");

                    var PermissaoOperarMercadoBovespa = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                        where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_NO_MERCADO_AVISTA

                                                        select p;

                    if (PermissaoOperarMercadoBovespa.Count() == 0)
                    {
                        logger.Info("Cliente não possui permissão para operar no mercado a vista");
                    }

                    // CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO A VISTA
                    if (PermissaoOperarMercadoBovespa.Count() > 0)
                    {
                        //**************************************************************************
                        // 3º VALIDA SALDO EM CUSTODIA DO CLIENTE
                        //**************************************************************************

                        // TRATAMENTO [ FRACIONARIO / INTEGRAL ]
                        int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
                        int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

                        logger.Info("Permissão para operar no mercado a vista concedida com sucesso.");

                        decimal QuantidadeSolicitadaVenda = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.DBToInt32();

                        logger.Info("Quantidade solicitada para venda: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
                        logger.Info("Inicia consulta de saldo em custódia do cliente");

                        //OBTEM A POSICAO EM CUSTODIA DO CLIENTE
                        SaldoCustodiaCliente = new ServicoCustodia().ObterCustodiaCliente(
                          new SaldoCustodiaRequest()
                          {
                              IdCliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                          });


                        if ((SaldoCustodiaCliente.StatusResposta == OMS.Custodia.Lib.Enum.CriticaMensagemEnum.Sucesso))
                        {

                            #region [ Ofertas em Aberto ]

                            string Instrumento = pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;

                            if (ModuloLoteNegociacao != 0)
                            {
                                Instrumento += "F";
                            }

                            string InstrumentoBase;

                            if (Instrumento.Substring(Instrumento.Length - 1, 1) == "F")
                            {
                                InstrumentoBase = Instrumento.Remove(Instrumento.Length - 1);
                            }
                            else
                            {
                                InstrumentoBase = Instrumento;
                            }

                            QuantidadeOfertaEmAberto = new PersistenciaCustodia().ListarBloqueioOpcoes(pParametros.EnviarOrdemRequest.OrdemInfo.Account, Instrumento);
                            QuantidadePosicao = new PersistenciaCustodia().ObterCustodiaExecutadaInstrumento(pParametros.EnviarOrdemRequest.OrdemInfo.Account, Instrumento);


                            #endregion

                            logger.Info("Posição em custódia carregada com sucesso");

                            logger.Info("Consulta se o cliente possui o papel solicitado para venda em custódia.");

                            if (SaldoCustodiaCliente.ColecaoObjeto != null)
                            {

                                //CARREGA A POSICAO EM CUSTODIA DO PAPEL QUE O CLIENTE DESEJA VENDER
                                var PosicaoCustodia = from p in SaldoCustodiaCliente.ColecaoObjeto
                                                      where p.CodigoInstrumento == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                      && p.CodigoCarteira == 21016 // Cliente só pode vender ações na carteira a vista.

                                                      select p;

                                int QuantidadeDisponivelCustodia = 0;


                                // OBTEM QUANTIDADE NEGOCIAVEL E CUSTODIA.
                                foreach (var ClienteCustodia in PosicaoCustodia)
                                {
                                    //QuantidadeDisponivelCustodia = ClienteCustodia.QtdeAtual;
                                    QuantidadeDisponivelCustodia = int.Parse(QuantidadePosicao.ToString());
                                }

                                logger.Info("Papel solicitado encontrado em custódia, quantidade disponivel para venda coberta: " + QuantidadeDisponivelCustodia.ToString());


                                // VERIFICA SE A QUANTIDADE SOLICITADA PARA VENDA ESTA BLOQUEADA DEVIDO A LANCAMENTO DE OPCÃO.
                                logger.Info("Verifica o total bloqueado ( qtde bloqueada ) para o papel solicitado");


                                if ((QuantidadeDisponivelCustodia + QuantidadeOfertaEmAberto) >= QuantidadeSolicitadaVenda)
                                {
                                    logger.Info("Quantidade Solicitada para venda é suficiente para cobrir a solicitação");

                                    #region BLOCO VENDA COBERTA

                                    if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                                    {
                                        // ENVIAR INTEGRAL
                                        pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;

                                        logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                        // Inseri a Ordem solicitada no banco de dados    
                                        if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                        {
                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                            logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral salva com sucesso.");

                                            logger.Info("Envia a ordem para o roteador de ordens");

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                            var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                            {
                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                _RespostaOrdem.DataResposta = DateTime.Now;
                                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                            }
                                            else
                                            {
                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                                logger.Info("Descrição do ero   :" + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);

                                            }
                                        }
                                        else
                                        {
                                            // ENVIAR FRACIONARIO
                                            pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                            logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario");

                                            // Inseri a Ordem solicitada no banco de dados    
                                            if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                            {
                                                //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                                                //ENVIA ORDEM PARA O ROTEADOR

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account += 5;

                                                logger.Info("Envia a ordem de venda para o roteador de ordens");

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                {
                                                    logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                    _RespostaOrdem.DataResposta = DateTime.Now;
                                                    _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                    _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                }
                                                else
                                                {
                                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                                    logger.Info("Descrição erro     :" + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                                    CriticaInfo = new PipeLineCriticaInfo();
                                                    CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                                    // Adiciona as criticas no pipeline de risco.
                                                    CriticaInfoCollection.Add(CriticaInfo);
                                                }

                                            }
                                        }


                                        if (ModuloLoteNegociacao > 0)
                                        {
                                            logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                                            // ENVIAR FRACIONARIO
                                            pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                            pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                            pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                            logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");
                                            // Inseri a Ordem solicitada no banco de dados    
                                            if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                            {
                                                //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                logger.Info("Envia a ordem para o roteador de ordens");

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                {
                                                    logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                    _RespostaOrdem.DataResposta = DateTime.Now;
                                                    _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                    _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                }
                                                else
                                                {
                                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                                    logger.Info("Descrição erro     :" + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                                    CriticaInfo = new PipeLineCriticaInfo();
                                                    CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                                    // Adiciona as criticas no pipeline de risco.
                                                    CriticaInfoCollection.Add(CriticaInfo);

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                        // Inseri a Ordem solicitada no banco de dados    
                                        if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                        {
                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                            logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                                            logger.Info("Envia a ordem para o roteador de ordens");

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                            var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                            {
                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                _RespostaOrdem.DataResposta = DateTime.Now;
                                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                            }
                                            else
                                            {
                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                                logger.Info("Descrição erro     :" + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);

                                            }
                                        }

                                    }


                                    #endregion

                                }
                                else

                                    // CLIENTE POSSUI O PAPEL , PORÉM, NAO POSSUI A QUANTIDADE SOLICITADA PARA VENDA.
                                    if ((QuantidadeDisponivelCustodia + QuantidadeOfertaEmAberto) < QuantidadeSolicitadaVenda)
                                    {
                                        //*****************************************************************
                                        // 4º VALIDA PERMISSAO PARA VENDER DESCOBERTO
                                        //*****************************************************************

                                        logger.Info("Cliente não possui custódia suficiente para vender o instrumento");
                                        logger.Info("Verifica permissão para vender o papel a descoberto");

                                        var PosicaoCustodiaDescoberto = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                        where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_VENDA_DESCOBERTO_AVISTA
                                                                        && p.DtValidade >= DateTime.Now
                                                                        orderby p.DtMovimento descending

                                                                        select p;

                                        if (PosicaoCustodiaDescoberto.Count() == 0)
                                        {
                                            logger.Info("Cliente não possui permissão para vender descoberto.");
                                        }

                                        // Verifica se o cliente possui permissao / saldo para vender descoberto
                                        if (PosicaoCustodiaDescoberto.Count() > 0)
                                        {

                                            int ValorFinanceiroDescoberto = 0;
                                            int idClienteParametroPermissao = 0;

                                            #region Ofertas 30 dias

                                            if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                            {
                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = "Não é possível abrir oferta de ações com validade para 30 dias utilizando Limite Operacional";
                                                CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);

                                                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                                                _RespostaOrdem.DataResposta = DateTime.Now;
                                                _RespostaOrdem.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                                _RespostaOrdem.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                                return _RespostaOrdem;
                                            }

                                            #endregion

                                            #region [Calcula percentual Máximo de oscilação mercado a vista]

                                            EnviarCotacaoResponse ResponseCotacao = this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());

                                            decimal OscilacaoMaxima = 5;

                                            decimal CotacaoAtual = ResponseCotacao.Objeto.Ultima;
                                            decimal PrecoEnviado = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();

                                            if (PrecoEnviado < CotacaoAtual)
                                            {
                                                decimal ValorOscilacao = Math.Abs((((PrecoEnviado / CotacaoAtual) - 1) * 100));

                                                if (ValorOscilacao > OscilacaoMaxima)
                                                {
                                                    CriticaInfo = new PipeLineCriticaInfo();
                                                    CriticaInfo.Critica = "Oscilação Máxima de 5% atingida, oferta recusada , preço fora do tunel";
                                                    CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                                    // Adiciona as criticas no pipeline de risco.
                                                    CriticaInfoCollection.Add(CriticaInfo);

                                                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                                                    _RespostaOrdem.DataResposta = DateTime.Now;
                                                    _RespostaOrdem.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                                    _RespostaOrdem.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                                    return _RespostaOrdem;

                                                }
                                            }

                                            #endregion

                                            foreach (var ClienteCustodia in PosicaoCustodiaDescoberto)
                                            {
                                                idClienteParametroPermissao = ClienteCustodia.IdClienteParametroPermissao;
                                                ValorFinanceiroDescoberto = (int)ClienteCustodia.ValorParametro;
                                                break;
                                            }

                                            logger.Info("Permissão para operar descoberto concedida com sucesso. Total disponivel: " + string.Format("{0:c}", ValorFinanceiroDescoberto.ToString()));

                                            decimal QuantidadeDescoberto = 0;

                                            if (QuantidadeDisponivelCustodia > 0)
                                            {
                                                QuantidadeDescoberto = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty - (QuantidadeDisponivelCustodia));
                                            }
                                            else
                                            {
                                                QuantidadeDescoberto = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty);
                                            }

                                            decimal ValorFinanceiroSolicitadoParaVenda = DiferencialVolumeOrdemAlterada;

                                            logger.Info("Total em custódia Coberta: " + QuantidadeDisponivelCustodia.ToString());
                                            logger.Info("Quantidade solicitada descoberto: " + QuantidadeDescoberto.ToString());

                                            decimal VolumeAjustadoLimite = 0;

                                            if (ValorFinanceiroSolicitadoParaVenda <= ValorFinanceiroDescoberto)
                                            {
                                                EnviarCotacaoResponse Response =
                                                    this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());

                                                #region [Calcula percentual Máximo de oscilação]

                                                decimal PrecoOrdem = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();
                                                decimal UltimaCotacao = Response.Objeto.Ultima;

                                                bool LimiteOscilacaoAtingido = false;

                                                decimal Oscilacao = (((UltimaCotacao / PrecoOrdem) - 1) * 100);

                                                if (Oscilacao > OscilacaoMaximaPermitidaMercadoAVista)
                                                {
                                                    VolumeAjustadoLimite = (UltimaCotacao * QuantidadeDescoberto);
                                                    LimiteOscilacaoAtingido = true;
                                                }

                                                #endregion


                                                string Historico = string.Empty;

                                                Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                                Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                                Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();

                                                if (LimiteOscilacaoAtingido == true)
                                                {
                                                    Historico += " Preco: " + UltimaCotacao.ToString();
                                                }
                                                else
                                                {
                                                    Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();
                                                }


                                                logger.Info("Atualiza os Limites do cliente");

                                                bool BLAtualizaSaldo;
                                                if (LimiteOscilacaoAtingido == true)
                                                {
                                                    BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                                       idClienteParametroPermissao,
                                                       VolumeAjustadoLimite,
                                                       Historico,
                                                       pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                                       'V'
                                                       );
                                                }
                                                else
                                                {
                                                    BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                                      idClienteParametroPermissao,
                                                      ValorFinanceiroSolicitadoParaVenda,
                                                      Historico,
                                                      pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                                      'V'
                                                      );

                                                }

                                                if (BLAtualizaSaldo)
                                                {
                                                    // Inseri a ordem no banco 

                                                    logger.Info("Inseri a ordem no banco de dados.");

                                                    if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                                                    {
                                                        // ENVIAR INTEGRAL
                                                        pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;

                                                        logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                                        // Inseri a Ordem solicitada no banco de dados    
                                                        if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                                        {
                                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                            logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                            logger.Info("Envia a ordem para o roteador de ordens");

                                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                            var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                            {
                                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                                _RespostaOrdem.DataResposta = DateTime.Now;
                                                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                            }
                                                            else
                                                            {
                                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                                CriticaInfo = new PipeLineCriticaInfo();
                                                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                                // Adiciona as criticas no pipeline de risco.
                                                                CriticaInfoCollection.Add(CriticaInfo);

                                                            }
                                                        }

                                                        logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                                                        if (ModuloLoteNegociacao > 0)
                                                        {
                                                            // ENVIAR FRACIONARIO
                                                            pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                                            pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                                            pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                                            logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");
                                                            // Inseri a Ordem solicitada no banco de dados    
                                                            if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                                            {
                                                                //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                                logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account); ;

                                                                logger.Info("Envia a ordem para o roteador de ordens");

                                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                                var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                                // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                                {
                                                                    logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                                    _RespostaOrdem.DataResposta = DateTime.Now;
                                                                    _RespostaOrdem.DescricaoResposta = "Ordem enviada com sucesso para o Roteador de Ordens";
                                                                    _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                                }
                                                                else
                                                                {
                                                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                                    CriticaInfo = new PipeLineCriticaInfo();
                                                                    CriticaInfo.Critica = "Ocorreu um erro ao enviar a ordem para o roteador de Ordens.";
                                                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                                    CriticaInfo.StackTrace = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                                                    // Adiciona as criticas no pipeline de risco.
                                                                    CriticaInfoCollection.Add(CriticaInfo);

                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // ENVIAR FRACIONARIO
                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                                        logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario");

                                                        // Inseri a Ordem solicitada no banco de dados    
                                                        if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                                        {
                                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                            logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                            //ENVIA ORDEM PARA O ROTEADOR                                                            

                                                            logger.Info("Envia a ordem para o roteador de ordens");

                                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                            var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));


                                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                            {
                                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                                _RespostaOrdem.DataResposta = DateTime.Now;
                                                                _RespostaOrdem.DescricaoResposta = "Ordem enviada com sucesso para o Roteador de Ordens";
                                                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                            }
                                                            else
                                                            {
                                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                                CriticaInfo = new PipeLineCriticaInfo();
                                                                CriticaInfo.Critica = "Ocorreu um erro ao enviar a ordem para o roteador de Ordens.";
                                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                                CriticaInfo.StackTrace = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                                // Adiciona as criticas no pipeline de risco.
                                                                CriticaInfoCollection.Add(CriticaInfo);
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // CLIENTE NÃO TEM SALDO SUFICIENTE PARA VENDER OPÇÃO A DESCOBERTO
                                                logger.Info("Cliente não possui permissão ou valor suficiente para operar o valor solicitado a descoberto no mercado a vista.");

                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = "Cliente não possui permissão ou valor suficiente para operar o valor solicitado a descoberto no mercado a vista.";
                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoCustodiaInsuficiente;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);

                                            }
                                        }
                                        else
                                        {
                                            // CLIENTE NÃO POSSUI PERMISSAO PARA VENDER CUSTODIA A DESCOBERTO.

                                            logger.Info("Cliente não possui permissão suficiente para operar descoberto no mercado a vista.");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Cliente não possui quantidade suficiente em custódia para efetuar a operação.";
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Cliente não possui permissão suficiente para operar descoberto no mercado a vista";
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }
                                    }
                            }
                            else
                            {
                                //CLIENTE NÃO POSSUI O PAPEL EM CUSTODIA ( TENTATIVA DE VENDA A DESCOBERTO )
                                if (SaldoCustodiaCliente.ColecaoObjeto == null)
                                {
                                    logger.Info(string.Format("{0}{1}{2}{3}", "RISCO: ", "Cliente: ", pParametros.EnviarOrdemRequest.OrdemInfo.Account, " [Cliente não possui o papel em custódia.]"));

                                    //*****************************************************************
                                    // 4º VALIDA PERMISSAO PARA VENDER DESCOBERTO
                                    //*****************************************************************

                                    logger.Info(string.Format("{0}{1}{2}{3}", "RISCO: ", "Cliente: ", pParametros.EnviarOrdemRequest.OrdemInfo.Account, " [Inicia validação de permissão para vender a descoberto no mercado de ações ]."));

                                    var PosicaoCustodiaDescoberto = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                    where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_VENDA_DESCOBERTO_AVISTA
                                                                    && p.DtValidade >= DateTime.Now
                                                                    orderby p.DtMovimento descending

                                                                    select p;

                                    // Verifica se o cliente possui permissao / saldo para vender descoberto
                                    if (PosicaoCustodiaDescoberto.Count() > 0)
                                    {

                                        #region Ofertas 30 dias

                                        if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                        {
                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Não é possível abrir oferta de ações com validade para 30 dias utilizando Limite Operacional";
                                            CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                            _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                            _RespostaOrdem.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                            _RespostaOrdem.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                            return _RespostaOrdem;
                                        }

                                        #endregion

                                        int ValorFinanceiroDescoberto = 0;
                                        int idClienteParametroPermissao = 0;

                                        foreach (var ClienteCustodia in PosicaoCustodiaDescoberto)
                                        {
                                            idClienteParametroPermissao = ClienteCustodia.IdClienteParametroPermissao;
                                            ValorFinanceiroDescoberto = (int)ClienteCustodia.ValorParametro;
                                            break;
                                        }

                                        logger.Info(string.Format("{0}{1}{2}{3}", "RISCO: ", "Cliente: ", pParametros.EnviarOrdemRequest.OrdemInfo.Account, " [Permissão para operar descoberto no mercado de ações concedida. Saldo em Limite: " + string.Format("{0:c}", ValorFinanceiroDescoberto).ToString() + " ]."));
                                        decimal ValorFinanceiroSolicitadoParaVenda = DiferencialVolumeOrdemAlterada;

                                        decimal VolumeAjustadoLimite = 0;
                                        if (ValorFinanceiroSolicitadoParaVenda <= ValorFinanceiroDescoberto)
                                        {
                                            //TODO ENVIA ORDEM DE VENDA

                                            #region [Calcula percentual Máximo de oscilação]

                                            EnviarCotacaoResponse Response =
                                                    this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());

                                            decimal PrecoOrdem = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();
                                            decimal UltimaCotacao = Response.Objeto.Ultima;

                                            bool LimiteOscilacaoAtingido = false;

                                            decimal Oscilacao = (((UltimaCotacao / PrecoOrdem) - 1) * 100);

                                            if (Oscilacao > OscilacaoMaximaPermitidaMercadoAVista)
                                            {
                                                VolumeAjustadoLimite = (UltimaCotacao * QuantidadeSolicitadaVenda);
                                                LimiteOscilacaoAtingido = true;
                                            }

                                            #endregion

                                            string Historico = string.Empty;

                                            if (LimiteOscilacaoAtingido == true)
                                            {
                                                Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                                Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                                Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();
                                                Historico += " Preco: " + Response.Objeto.Ultima.ToString();
                                            }
                                            else
                                            {
                                                Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                                Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                                Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();
                                                Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();

                                            }


                                            logger.Info(string.Format("{0}{1}{2}{3}", "RISCO: ", "Cliente: ", pParametros.EnviarOrdemRequest.OrdemInfo.Account, " [Ordem de venda enviada com sucesso.]"));

                                            bool BLAtualizaSaldo;
                                            // Atualiza os limites
                                            if (LimiteOscilacaoAtingido == true)
                                            {
                                                BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                                    idClienteParametroPermissao,
                                                    VolumeAjustadoLimite,
                                                    Historico,
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                                    'V'
                                                );
                                            }
                                            else
                                            {
                                                BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                                    idClienteParametroPermissao,
                                                    ValorFinanceiroSolicitadoParaVenda,
                                                    Historico,
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                                    'V'
                                                );
                                            }

                                            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                                            {
                                                // ENVIAR INTEGRAL
                                                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;

                                                logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                                // Inseri a Ordem solicitada no banco de dados    
                                                if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                                {
                                                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                    logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                    logger.Info("Envia a ordem para o roteador de ordens");

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                    var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                    {
                                                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                        _RespostaOrdem.DataResposta = DateTime.Now;
                                                        _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                    }
                                                    else
                                                    {
                                                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                        CriticaInfo = new PipeLineCriticaInfo();
                                                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                                        // Adiciona as criticas no pipeline de risco.
                                                        CriticaInfoCollection.Add(CriticaInfo);

                                                    }
                                                }

                                                logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                                                if (ModuloLoteNegociacao > 0)
                                                {

                                                    // ENVIAR FRACIONARIO
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                                    logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");
                                                    // Inseri a Ordem solicitada no banco de dados    
                                                    if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                                    {
                                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = 219775;

                                                        logger.Info("Envia a ordem para o roteador de ordens");

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));


                                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // ENVIAR FRACIONARIO
                                                pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                                logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                                // Inseri a Ordem solicitada no banco de dados    
                                                if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                                {
                                                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                                    //ENVIA ORDEM PARA O ROTEADOR

                                                    logger.Info("Envia a ordem para o roteador de ordens");

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                    var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                    {
                                                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                        _RespostaOrdem.DataResposta = DateTime.Now;
                                                        _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                    }
                                                    else
                                                    {
                                                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                        CriticaInfo = new PipeLineCriticaInfo();
                                                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                                        // Adiciona as criticas no pipeline de risco.
                                                        CriticaInfoCollection.Add(CriticaInfo);
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {

                                            // CLIENTE NÃO TEM SALDO SUFICIENTE PARA VENDER O INSTRUMENTO DESCOBERTO     
                                            logger.Info("Cliente não possui saldo suficiente para vender a quantidade solicitada a descoberto: Quantidade solicitada: " + ValorFinanceiroSolicitadoParaVenda.ToString());

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Cliente não possui saldo suficiente para vender a quantidade solicitada a descoberto: Quantidade solicitada: " + ValorFinanceiroSolicitadoParaVenda.ToString();
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoCustodiaInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }
                                    }
                                    else
                                    {
                                        // Cliente não possui papel em custódia
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Cliente não possui o papel em custódia";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoCustodiaInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);


                                        // Cliente não possui permissão para operar descoberto.
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Cliente não possui permissão para operar descoberto";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Não possui permissão para operar no mercado a vista
                        logger.Info("Cliente não possui permissão suficiente para operar no mercado a vista");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = "Cliente não possui permissão insuficiente para operar no mercado vista";
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);
                    }
                }
                catch (Exception ex)
                {
                    _RespostaOrdem.DataResposta = DateTime.Now;
                    _RespostaOrdem.StackTrace = ex.StackTrace;
                    _RespostaOrdem.DescricaoResposta = ex.Message;
                    _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Exception;

                }

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(_RespostaOrdem);

            }
            else
            {

                VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
                ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

                DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);

                Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
                QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

                var LotePadraoNegociacao = CadastroPapeis.Objeto.LoteNegociacao;

                QtdeAux = Math.Abs(Quantidade);

                _LoteNegociacao = (Quantidade % int.Parse(LotePadrao));

                if ((_LoteNegociacao) != 0)
                {
                    BlMercadoFracionario = true;
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                }
                else
                {
                    BlMercadoIntegral = true;
                }

                logger.Info("Diferencial Volume da ordem alterada <= 0 ");

                VolumeOperacao = (pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.DBToDecimal());

                #region ["Envia Ordem direto para o roteador"]

                pParametros.EnviarOrdemRequest.OrdemInfo.OrdStatus = OrdemStatusEnum.SUBSTITUICAOSOLICITADA;

                // Atualiza o status da ordem do cliente.
                if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Alteracao de ordem enviada com sucesso para o Roteador de Ordens");

                        _RespostaOrdem.DataResposta = DateTime.Now;
                        _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a alteracao de ordens para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }

                }

                #endregion
            }

            return _RespostaOrdem;

        }


        #endregion

        #region [ Alteração de ordens no mercado de opções ]

        /// <summary>
        /// Método responsavel por alterar uma oferta de compra no mercado de opções
        /// </summary>
        private ValidacaoRiscoResponse AlterarOrdemCompraOpcoes(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();

            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse _RespostaOrdem = new ValidacaoRiscoResponse();

            //OBJETO RESPONSAVEL POR ARMAZENAR O SALDO E CONTA CORRENTE DO CLIENTE
            SaldoContaCorrenteResponse<ContaCorrenteInfo> SaldoContaCorrenteResponse = null;

            decimal ValorOrdemEnviada = 0;
            decimal VolumeOperacao = 0;
            decimal VolumeOrdemOriginal = 0;
            decimal DiferencialVolumeOrdemAlterada = 0;

            decimal Preco= 0;
            decimal PrecoOriginal= 0;

            decimal Quantidade = 0;
            decimal QuantidadeOriginal = 0;

            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
            ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Preco = Math.Round(Preco, 2);
            PrecoOriginal = Math.Round(PrecoOriginal, 2);

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

            if ((Preco == PrecoOriginal) && (Quantidade == QuantidadeOriginal))
            {
                logger.Info("A ordem enviada esta identica a original");

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "Os dados enviados estão idênticos aos originais";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(_RespostaOrdem);
            }

            // Verifica a quantidade 
            if ((pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % 100) != 0)
            {
                logger.Info("A quantidade digitada deve ser multiplo de 100");

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "A quantidade digitada deve ser multiplo de 100";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                return TratamentoRetorno(_RespostaOrdem);
            }

            DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);

           // DiferencialVolumeOrdemAlterada = Math.Abs(DiferencialVolumeOrdemAlterada);
            if (DiferencialVolumeOrdemAlterada > 0)
            {
                // CLIENTE NÃO POSSUI CUSTODIA SUFICIENTE PARA ZERAR O PAPEL.
                // VERIFICAR SE ESTA FAZENDO UM LANÇAMENTO DESCOBERTO COM GARANTIA NA CARTEIRA 27
                string PapelBase = CadastroPapeis.Objeto.PapelObjeto;

                try
                {
                    VolumeOperacao = DiferencialVolumeOrdemAlterada;

                    logger.Info("Verifica se a ordem enviada é uma alteração ou nova ordem");

                    if (pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID != null)
                    {
                        logger.Info("Resposta: Alteração de Ordem");
                    }
                    else
                    {
                        logger.Info("Resposta: Inclusão de Oferta");
                    }

                    logger.Info("Inicia validação de permissão para operar no mercado de opções.");
                 

                    var PermissaoOperarMercadoOpcoes = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                       where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_NO_MERCADO_OPCOES

                                                       select p;


                    if (PermissaoOperarMercadoOpcoes.Count() == 0)
                    {
                        logger.Info("Cliente não possui permissão para operar no mercado de opções");
                    }

                    if (PermissaoOperarMercadoOpcoes.Count() > 0)
                    {

                        // VERIFICA SE A OPÇÃO QUE O CLIENTE ESTA QUERENDO NEGOCIAR ESTA VENCIDA OU SE HOJE É DIA DE VENCIMENTO DE OPÇÕES
                        #region [ Vencimento de opções ]

                        logger.Info("Permissão concedida para operar no mercado de opções com sucesso.");

                        logger.Info("Verifica se o cliente esta tentando abrir posição em dia de vncimento de opção ou negociando uma opção já vencida.");

                        lock (VencimentoInstrumentoInfo.htInstrumentos)
                        {
                            if (VencimentoInstrumentoInfo.htInstrumentos.Contains(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol))
                            {
                                DateTime dataVencimento = DateTime.Parse(VencimentoInstrumentoInfo.htInstrumentos[pParametros.EnviarOrdemRequest.OrdemInfo.Symbol].ToString());

                                //Vencimento de opção
                                if (dataVencimento.ToString("{dd/MM/yyyy}") == DateTime.Now.ToString("{dd/MM/yyyy}"))
                                {
                                    logger.Info("Não é possível abrir posição em dia de vencimento de opções");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Não é possível abrir posição em dia de vencimento de opção. Caso queria zerar sua posição, entre com contato com a mesa de operações.";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                                    return TratamentoRetorno(_RespostaOrdem);
                                }
                                else
                                {
                                    logger.Info("Verifica se a série de opção esta ativa");
                                    // Serie Vencida.
                                    if (dataVencimento < DateTime.Now)
                                    {
                                        logger.Info("A serie de opção referente ao instrumento " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " já esta vencida");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Esta serie de opção já esta vencida.";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                                        return TratamentoRetorno(_RespostaOrdem);
                                    }
                                    else
                                    {
                                        logger.Info("Série de opção valida para operação");
                                    }
                                }
                            }
                        }

                        #endregion

                        //**************************************************************************
                        // 3º VALIDA SALDO EM CONTA CORRENTE DO CLIENTE
                        //**************************************************************************
                        VolumeOperacao = DiferencialVolumeOrdemAlterada;

                        logger.Info("Volume solicitado para operação: " + string.Format("{0:c}", VolumeOperacao));

                        logger.Info("Inicia consulta de saldo em conta corrente");

                        //OBTEM O SALDO TOTAL DE CONTA CORRENTE DO CLIENTE
                        SaldoContaCorrenteResponse = new ServicoContaCorrente().ObterSaldoContaCorrente(
                          new SaldoContaCorrenteRequest()
                          {
                              IdCliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                          });

                        if (SaldoContaCorrenteResponse.StatusResposta == ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                        {
                            logger.Info("Saldo em conta corrente carregado com sucesso.");

                            // Saldo em opções.
                            SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes = (SaldoContaCorrenteResponse.Objeto.SaldoD0 + SaldoContaCorrenteResponse.Objeto.SaldoD1);
                            SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes = SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes + decimal.Parse(SaldoContaCorrenteResponse.Objeto.SaldoBloqueado.ToString());

                            logger.Info("Saldo disponível em opções sem utlização de limite operacional " + string.Format("{0:c}", SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes));

                            // VERIFICA SE O SALDO EM CONTA CORRENTE CONSEGUE COBRIR A OPERACAO
                            // SEM UTILIZACAO DE LIMITE OPERACIONAL
                            if (VolumeOperacao <= SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes)
                            {
                                // Encaminha solicitação de compra de opções para o banco de dados
                                if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                {
                                    logger.Info("Envia a ordem para o roteador de ordens");

                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                    var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                    {
                                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                        _RespostaOrdem.DataResposta = DateTime.Now;
                                        _RespostaOrdem.DescricaoResposta = "Ordem enviada com sucesso para o Roteador de Ordens";
                                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                    }
                                    else
                                    {
                                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                        logger.Info("Descricao do erro:  " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                    }

                                }

                                _RespostaOrdem.DataResposta = DateTime.Now;
                                _RespostaOrdem.DescricaoResposta = "Ordem enviada para o Serviço de ordens";
                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                            }
                            else
                            {
                                // VOLUME DE COMPRA E MAIOR QUE O SALDO EM CONTA CORRENTE
                                if (VolumeOperacao > SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes)
                                {
                                    //CLIENTE POSSUI PERMISSAO PARA UTILIZAR LIMITE OPERACIONAL

                                    logger.Info("Volume da operação maior do que o saldo disponivel em conta corrente");
                                    logger.Info("Verifica permissão para utilizar limite no mercado de opções");

                                    //OBTEM SALDO EM CONTA CORRENTE PROJETADO ATE D+1                                                    
                                    decimal SaldoDisponivelOpcoes = SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes.DBToDecimal();

                                    /***********************************************************************************
                                     * 4º OBTEM VALOR DO LIMITE PARA O MERCADO DE OPCOES
                                     * *********************************************************************************/

                                    var ValorLimiteMercadoOpcoes = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                   where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_MERCADO_OPCOES
                                                                   && p.DtValidade >= DateTime.Now
                                                                   orderby p.DtMovimento descending

                                                                   select p;

                                    decimal SaldoLimiteOperacional = 0;
                                    decimal ValorAlocado = 0;
                                    int idClienteParametroPermissao = 0;

                                    #region Opcoes 30 dias

                                    if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Não é possível abrir oferta de opções com validade para 30 dias utilizando Limite Operacional";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                                        _RespostaOrdem.DataResposta = DateTime.Now;
                                        _RespostaOrdem.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        _RespostaOrdem.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return _RespostaOrdem;
                                    }

                                    #endregion

                                    // SOMA O SALDO DE CONTA CORRENTE COM O LIMITE OPERACIONAL DO CLIENTE.
                                    foreach (var ClientePosicao in ValorLimiteMercadoOpcoes)
                                    {
                                        SaldoLimiteOperacional = ClientePosicao.ValorParametro;
                                        ValorAlocado = ClientePosicao.ValorAlocado;
                                        idClienteParametroPermissao = ClientePosicao.IdClienteParametroPermissao;
                                        break;
                                    }

                                    logger.Info("Limite operacional para o mercado de opções carregados com sucesso. Valor disponivel: " + string.Format("{0:c}", SaldoLimiteOperacional));
                                    logger.Info("Saldo em conta corrente + Limite operacional em opções:  " + string.Format("{0:c}", (SaldoDisponivelOpcoes + SaldoLimiteOperacional)));

                                    decimal? LimiteTotal = (SaldoLimiteOperacional + ValorAlocado);
                                    // VERIFICA SE O SALDO EM CONTA CORRENTE + LIMITE OPERACIONAL 
                                    // É CAPAZ DE COBRIR A COMPRA A SER REALIZADA.                         
                                    if ((SaldoDisponivelOpcoes + LimiteTotal) >= VolumeOperacao)
                                    {
                                        //TODO: INVOCAR SERVICO ENVIO ORDENS
                                        decimal ValorEmprestimo = 0;

                                        if (SaldoDisponivelOpcoes > 0)
                                        {
                                            //Subtrai do saldo de ações
                                            ValorEmprestimo = (SaldoDisponivelOpcoes - VolumeOperacao);
                                        }
                                        else
                                        {
                                            //Caso o saldo em ações seja menor ou igual a zero atribui o valor do volume da operacao
                                            ValorEmprestimo = VolumeOperacao;
                                        }

                                        logger.Info("Valor do emprestimo a ser utilizado para realizar esta operação é de: " + string.Format("{0:c}", ValorEmprestimo));

                                        string Historico = string.Empty;

                                        Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                        Historico = " Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                        Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();
                                        Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();

                                        logger.Info("Atualiza saldo do cliente");

                                        bool BLAtualizacaoLimite = new PersistenciaRisco().AtualizaLimiteCliente(
                                            idClienteParametroPermissao,
                                            Math.Abs(ValorEmprestimo),
                                            Historico,
                                              pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID
                                            );

                                        if (BLAtualizacaoLimite == true)
                                        {

                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                            logger.Info("Limite atualizado com sucesso.");

                                            logger.Info("Inseri bloqueio para o cliente.");

                                            if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                            {
                                                //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                logger.Info("Ordem inserida no banco de dados com sucesso.");

                                                logger.Info("Envia a ordem para o roteador de ordens");

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));


                                                // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                {
                                                    logger.Info("Solicitação de alteração enviada com sucesso para o Roteador de Ordens");

                                                    _RespostaOrdem.DataResposta = DateTime.Now;
                                                    _RespostaOrdem.DescricaoResposta = "Ordem enviada com sucesso para o Roteador de Ordens";
                                                    _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                }
                                            }
                                        }

                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS
                                        _RespostaOrdem.DataResposta = DateTime.Now;
                                        _RespostaOrdem.DescricaoResposta = "Ordem enviada para o Serviço de ordens";
                                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;

                                    }
                                    else
                                        if (VolumeOperacao > (SaldoDisponivelOpcoes + SaldoLimiteOperacional))
                                        {
                                            logger.Info("Saldo em conta corrente insuficiente para envia a ordem.");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Saldo em conta corrente insuficiente para envia a ordem.";
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoContaCorrenteInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                            _RespostaOrdem.DescricaoResposta = "Saldo em conta corrente insuficiente para envia a ordem.";
                                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);
                                        }
                                }
                                else
                                { // CLIENTE NÃO POSSUI PERMISSAO PARA UTILIZAR LIMITE
                                    // SALDO EM CONTA CORRENTE INSUFICIENTE                      


                                    logger.Info("Saldo em conta corrente insuficiente para realizar a operação.");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Saldo em conta corrente insuficiente para envia a ordem.";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoContaCorrenteInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                }
                            }
                        }
                        else
                        {
                            if (SaldoContaCorrenteResponse.StatusResposta == ContaCorrente.Lib.Enum.CriticaMensagemEnum.Exception)
                            {
                                logger.Info("Ocorreu um erro ao carregar o saldo em conta corrente do corrente.");
                            }
                        }
                    }
                    else
                    { // CLIENTE NAO POSSUI PERMISSAO PARA OPERAR NO MERCADO DE OPCOES

                        logger.Info("Cliente não possui permissão para operar no mercado de opções.");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = "Cliente não possui permissão para operar no mercado de opções.";
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);
                    }


                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                }
                catch (Exception ex)
                {
                    _RespostaOrdem.DataResposta = DateTime.Now;
                    _RespostaOrdem.StackTrace = ex.StackTrace;
                    _RespostaOrdem.DescricaoResposta = ex.Message;
                    _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Exception;

                }
            }
            else
            {

                VolumeOperacao = (pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.DBToDecimal());

                #region ["Envia Ordem direto para o roteador"]

                pParametros.EnviarOrdemRequest.OrdemInfo.OrdStatus = OrdemStatusEnum.SUBSTITUICAOSOLICITADA;

                // Atualiza o status da ordem do cliente.
                if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));


                    //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Alteracao de ordem enviada com sucesso para o Roteador de Ordens");

                        _RespostaOrdem.DataResposta = DateTime.Now;
                        _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a alteracao de ordens para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }

                }

                #endregion

            }

            if ((pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % 100) != 0)
            {

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "A Quantidade enviada deve ser multiplo de 100";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return _RespostaOrdem;
            }

            _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
            return _RespostaOrdem;

        }

        /// <summary>
        /// Método responsavel por alterar uma oferta de venda no mercado de opções
        /// </summary>
        private ValidacaoRiscoResponse AlterarOrdemVendaOpcoes(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {

            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();

            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse _RespostaOrdem = new ValidacaoRiscoResponse();

            //OBJETO RESPONSAVEL POR ARMAZENAR O SALDO EM CUSTODIA DO CLIENTE
            SaldoCustodiaResponse<CustodiaClienteInfo> SaldoCustodiaCliente = null;

            //OBJETO RESPONSAVEL POR ARMAZENAR O SALDO E CONTA CORRENTE DO CLIENTE
            SaldoContaCorrenteResponse<ContaCorrenteInfo> SaldoContaCorrenteResponse = null;

            decimal ValorOrdemEnviada = 0;
            decimal VolumeOperacao = 0;
            decimal VolumeOrdemOriginal = 0;
            decimal DiferencialVolumeOrdemAlterada = 0;
            decimal DiferencialQuantidade = 0;


            int Quantidade = 0;
            int QuantidadeOriginal = 0;    

            decimal Preco = 0;
            decimal PrecoOriginal = 0;

            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            VolumeOrdemOriginal = (OrdemInfoAlteracao.OrderQty * decimal.Parse(OrdemInfoAlteracao.Price.ToString()));
            ValorOrdemEnviada = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty * decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString()));

            DiferencialVolumeOrdemAlterada = (ValorOrdemEnviada - VolumeOrdemOriginal);
            DiferencialQuantidade = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty - OrdemInfoAlteracao.OrderQty);

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Quantidade = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            QuantidadeOriginal = OrdemInfoAlteracao.OrderQty;

            logger.Info("Preco Enviado: " + PrecoOriginal.ToString());
            logger.Info("Preco Antigo : " + Preco.ToString());

            Preco = Math.Round(Preco, 2);
            PrecoOriginal = Math.Round(PrecoOriginal, 2);

            logger.Info("Preco Enviado 2 decimais : " + PrecoOriginal.ToString());
            logger.Info("Preco Antigo 2 decimais : " + Preco.ToString());

            logger.Info("Quantidade Enviada: " + QuantidadeOriginal.ToString());
            logger.Info("Quantidade Antiga : " + Quantidade.ToString());

            if ((Preco == PrecoOriginal) && (Quantidade == QuantidadeOriginal))
            {
                logger.Info("A ordem enviada esta identica a original");

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "Os dados enviados estão idênticos aos originais";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(_RespostaOrdem);
            }

            // Verifica a quantidade 
            if ((pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % 100) != 0)
            {
                logger.Info("A quantidade digitada deve ser multiplo de 100");

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "A quantidade digitada deve ser multiplo de 100";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                return TratamentoRetorno(_RespostaOrdem);
            }

            //DiferencialVolumeOrdemAlterada = Math.Abs(DiferencialVolumeOrdemAlterada);
            if (DiferencialVolumeOrdemAlterada >= 0)
            {
                // CLIENTE NÃO POSSUI CUSTODIA SUFICIENTE PARA ZERAR O PAPEL.
                // VERIFICAR SE ESTA FAZENDO UM LANÇAMENTO DESCOBERTO COM GARANTIA NA CARTEIRA 27

                string PapelBase = CadastroPapeis.Objeto.PapelObjeto;

                int QuantidadeSolicitada = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;

                int QuantidadeOfertaEmAberto = 0;

                try
                {                   

                    logger.Info("Inicializa validação de permissão para operar no mercado de opções");

                    var PermissaoOperarMercadoOpcoes = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                       where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_NO_MERCADO_OPCOES
                                                       select p;

                    if (PermissaoOperarMercadoOpcoes.Count() == 0)
                    {

                        logger.Info("Cliente não possui permissão para operar no mercado de opções");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = "Cliente não possui permissão para operar no mercado de opções";
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);
                    }

                    // CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO DE OPÇÕES
                    if (PermissaoOperarMercadoOpcoes.Count() > 0)
                    {
                        logger.Info("Permissão concedida para operar no mercado de opções");

                        // VERIFICA SE A OPÇÃO QUE O CLIENTE ESTA QUERENDO NEGOCIAR ESTA VENCIDA OU SE HOJE É DIA DE VENCIMENTO DE OPÇÕES
                        #region [ Vencimento de opções ]

                        logger.Info("Verifica se o cliente esta tentando abrir posição em dia de vncimento de opção ou negociando uma opção já vencida.");

                        lock (VencimentoInstrumentoInfo.htInstrumentos)
                        {
                            if (VencimentoInstrumentoInfo.htInstrumentos.Contains(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol))
                            {
                                DateTime dataVencimento = DateTime.Parse(VencimentoInstrumentoInfo.htInstrumentos[pParametros.EnviarOrdemRequest.OrdemInfo.Symbol].ToString());

                                //Vencimento de opção
                                if (dataVencimento.ToString("{dd/MM/yyyy}") == DateTime.Now.ToString("{dd/MM/yyyy}"))
                                {
                                    logger.Info("Não é possível abrir posição em dia de vencimento de opções");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Não é possível abrir posição em dia de vencimento de opção. Caso queria zerar sua posição, entre com contato com a mesa de operações.";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                    _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                                    return TratamentoRetorno(_RespostaOrdem);
                                }
                                else
                                {
                                    logger.Info("Verifica se a série de opção esta ativa");
                                    // Serie Vencida.
                                    if (dataVencimento < DateTime.Now)
                                    {
                                        logger.Info("A serie de opção referente ao instrumento " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " já esta vencida");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Esta serie de opção já esta vencida.";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                                        return TratamentoRetorno(_RespostaOrdem);
                                    }
                                    else
                                    {
                                        logger.Info("Série de opção valida para operação");
                                    }
                                }
                            }
                        }

                        #endregion

                        #region [Calcula percentual Máximo de oscilação mercado a vista sem Limites 80%]

                        EnviarCotacaoResponse _ResponseCotacao = this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());

                        decimal OscilacaoMaxima = 80;

                        decimal CotacaoAtual = _ResponseCotacao.Objeto.Ultima;
                        decimal PrecoEnviado = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();

                        if (PrecoEnviado < CotacaoAtual)
                        {
                            decimal ValorOscilacao = Math.Abs((((PrecoEnviado / CotacaoAtual) - 1) * 100));

                            if (ValorOscilacao > OscilacaoMaxima)
                            {
                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = "Oscilação Máxima de 80% atingida, oferta recusada , preço fora do tunel, Por favor alterar o preço da oferta.";
                                CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                                _RespostaOrdem.DataResposta = DateTime.Now;
                                _RespostaOrdem.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                _RespostaOrdem.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                return _RespostaOrdem;

                            }
                        }

                        #endregion

                        SaldoCustodiaCliente = new ServicoCustodia().ObterCustodiaCliente(
                        new SaldoCustodiaRequest()
                        {
                            IdCliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                        });


                        VolumeOperacao = DiferencialVolumeOrdemAlterada;

                        #region [ Verifica Custodia para zerar posição ]

                        int QuantidadeOpcaoCustodiaCarteira = 0;

                        // Existe custódia para realizar a operacao 
                        if ((SaldoCustodiaCliente.ColecaoObjeto != null) && (SaldoCustodiaCliente.ColecaoObjeto.Count > 0))
                        {

                            //TODO: VERIFICA SE CLIENTE ESTA ZERANDO POSICAO      
                            var CustodiaClienteZerandoPosicao = from p in SaldoCustodiaCliente.ColecaoObjeto
                                                                where p.CodigoInstrumento == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                                && p.QtdeAtual > 0

                                                                select p;

                            if (CustodiaClienteZerandoPosicao.Count() > 0)
                            {

                                // CLIENTE ZERANDO PARTE DA POSICAO/ POSICAO COMPLETA OPCOES
                                foreach (var Custodia in CustodiaClienteZerandoPosicao)
                                {
                                    QuantidadeOpcaoCustodiaCarteira = Custodia.QtdeAtual;
                                }

                            }

                        #endregion


                            #region [  Valida a quantidade nas carteiras ]
                            //VERIFICA A QUANTIDADE COBERTA EM CUSTODIA DO CLIENTE NA CARTEIRA DE ACOES 21016

                            int QuantidadeAcaoCustodiaCobertaCarteira = 0;


                            // Verifica se o cliente possui a quantidade solicitada para zerar
                            var CustodiaClientePapelBase = from p in SaldoCustodiaCliente.ColecaoObjeto
                                                           where
                                                               // p.QtdeAtual >= QuantidadeSolicitada &&
                                                               p.CodigoInstrumento == PapelBase

                                                           select p;


                            // Cliente possui papel base suficiente para efetuar o lancamento coberto
                            if ((CustodiaClientePapelBase != null) && (CustodiaClientePapelBase.Count() > 0))
                            {
                                logger.Info("Cliente possui papel base suficiente para efetuar o lancamento coberto de opcoes na carteira 21");

                                foreach (var Custodia in CustodiaClientePapelBase)
                                {
                                    QuantidadeAcaoCustodiaCobertaCarteira += Custodia.QtdeAtual;
                                }
                            }

                            #endregion

                            #region [ Custodia Bloqueada Venda em aberto]

                            int QuantidadeBloqueada = 0;

                            QuantidadeBloqueada = new PersistenciaCustodia().ListarBloqueioOpcoes(pParametros.EnviarOrdemRequest.OrdemInfo.Account, PapelBase);


                            logger.Info("Bloqueio em custódia carregado com sucesso.");
                            logger.Info("Quantidade Bloqueada: " + QuantidadeBloqueada.ToString());

                            if (QuantidadeBloqueada < 0)
                            {
                                // Existem bloqueios
//                                QuantidadeAcaoCustodiaCobertaCarteira = (QuantidadeAcaoCustodiaCobertaCarteira + QuantidadeBloqueada);
                                QuantidadeAcaoCustodiaCobertaCarteira = (QuantidadeAcaoCustodiaCobertaCarteira + 0);
                            }

                            #endregion

                            //TODO: VERIFICA SALDO PARA ENVIAR ORDEM.

                            // VERIFICA SE A QUANTIDADE COBERTA DO PAPEL BASE + QUANTIDADE EM OPCAO NA CUSTODIA E SUFICIENTE PARA COBRIR A OPERAÇÃO
                            if ((QuantidadeOpcaoCustodiaCarteira + QuantidadeAcaoCustodiaCobertaCarteira) >= QuantidadeSolicitada)
                            {
                                // PRIMEIRA TENTA ZERAR A TODA A POSICAO DO CLIENTE
                                if ((QuantidadeOpcaoCustodiaCarteira - QuantidadeSolicitada) >= 0)
                                {
                                    #region [ Zera posição parcial / total de opções e envia a ordem. ]


                                    // Inseri a Ordem solicitada no banco de dados    
                                    if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                    {
                                        //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                        //ENVIA ORDEM PARA O ROTEADOR

                                        logger.Info("Envia a ordem para o roteador de ordens");

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            _RespostaOrdem.DataResposta = DateTime.Now;
                                            _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);
                                        }

                                    }
                                }

                                else
                                {
                                    // Zerando posição total / Efetuando lancamento Coberto de Opções
                                    if ((QuantidadeOpcaoCustodiaCarteira - QuantidadeSolicitada) < 0)
                                    {

                                        // Inseri a Ordem solicitada no banco de dados    
                                        if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                        {
                                            //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                                            logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                            //ENVIA ORDEM PARA O ROTEADOR
                                            logger.Info("Envia a ordem para o roteador de ordens");

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                            var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                            {
                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                _RespostaOrdem.DataResposta = DateTime.Now;
                                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                            }
                                            else
                                            {
                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);
                                            }

                                        }
                                        else
                                        {

                                            logger.Info("Ocorreu um erro ao efetuar o bloqueio do cliente");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Ocorreu um erro ao efetuar o bloqueio do cliente";
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }

                                    }

                                    #endregion

                                }
                            }
                            else
                            {

                                // Cliente não possui custódia suficiente para efetuar o lançamento coberto de opções.

                                //*****************************************************************
                                // 4º VALIDA PERMISSAO PARA VENDER DESCOBERTO
                                //*****************************************************************

                                logger.Info("Cliente não possui custódia suficiente para vender o instrumento");
                                logger.Info("Verifica permissão para vender o papel a descoberto");

                                var PosicaoCustodiaDescoberto = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_DESCOBERTO_OPCOES
                                                                && p.DtValidade >= DateTime.Now
                                                                orderby p.DtMovimento descending

                                                                select p;

                                if (PosicaoCustodiaDescoberto.Count() == 0)
                                {
                                    logger.Info("Cliente não possui permissão para vender descoberto.");
                                }

                                // Verifica se o cliente possui permissao / saldo para vender descoberto
                                if (PosicaoCustodiaDescoberto.Count() > 0)
                                {

                                    #region Opcoes 30 dias

                                    if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Não é possível abrir oferta de opções com validade para 30 dias utilizando Limite Operacional";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        _RespostaOrdem.CriticaInfo = CriticaInfoCollection;
                                        _RespostaOrdem.DataResposta = DateTime.Now;
                                        _RespostaOrdem.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        _RespostaOrdem.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return _RespostaOrdem;
                                    }

                                    #endregion

                                    int ValorFinanceiroDescoberto = 0;
                                    int idClienteParametroPermissao = 0;

                                    foreach (var ClienteCustodia in PosicaoCustodiaDescoberto)
                                    {
                                        idClienteParametroPermissao = ClienteCustodia.IdClienteParametroPermissao;
                                        ValorFinanceiroDescoberto = (int)ClienteCustodia.ValorParametro;
                                        break;
                                    }

                                    logger.Info("Permissão para operar descoberto concedida com sucesso. Total disponivel: " + string.Format("{0:c}", ValorFinanceiroDescoberto.ToString()));

                                    if (QuantidadeAcaoCustodiaCobertaCarteira < 0)
                                    {
                                        QuantidadeAcaoCustodiaCobertaCarteira = 0;
                                    }

                                    if (QuantidadeOpcaoCustodiaCarteira < 0)
                                    {
                                        QuantidadeOpcaoCustodiaCarteira = 0;
                                    }

                                    decimal QuantidadeDescoberto = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty - (QuantidadeOpcaoCustodiaCarteira + QuantidadeAcaoCustodiaCobertaCarteira));

                                    decimal ValorFinanceiroSolicitadoParaVenda = DiferencialVolumeOrdemAlterada;

                                    logger.Info("Total em custódia Coberta: " + QuantidadeOpcaoCustodiaCarteira.ToString());
                                    logger.Info("Quantidade solicitada descoberto: " + QuantidadeDescoberto.ToString());

                                    decimal VolumeAjustadoLimite = 0;

                                    if (ValorFinanceiroSolicitadoParaVenda <= ValorFinanceiroDescoberto)
                                    {
                                        logger.Info("Inicia processo de calculo de validação de oscilação");
                                        decimal UltimaCotacao;
                                        decimal UltimaCotacaoPapelBase;
                                        bool LimiteOscilacaoAtingido = false;
                                        decimal Oscilacao;

                                        logger.Info("Obtem cotação do papel base: " + PapelBase);
                                        EnviarCotacaoResponse ResponseCotacaoBase = this.ObterCotacao(PapelBase);
                                        UltimaCotacaoPapelBase = ResponseCotacaoBase.Objeto.Ultima;
                                        logger.Info("Valor obtido: " + PapelBase + " : " + UltimaCotacaoPapelBase.ToString());

                                        logger.Info("Obtem Cotação da opção: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
                                        EnviarCotacaoResponse ResponseCotacao = this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());
                                        UltimaCotacao = ResponseCotacao.Objeto.Ultima;
                                        logger.Info("Valor obtido: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim() + " : " + UltimaCotacao.ToString());

                                        decimal PrecoOrdem = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();

                                        Oscilacao = (((UltimaCotacao / PrecoOrdem) - 1) * 100);

                                        #region [Verificar se a opção esta ITM ,OTM,ATM]

                                        logger.Info("Obtem Strike da Opcao");

                                        int StrikeOpcao = 0;

                                        if (pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length == 7)
                                        {
                                            StrikeOpcao = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Substring(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length - 2, 2));
                                        }
                                        else if (pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length == 6)
                                        {
                                            StrikeOpcao = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Substring(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length - 1, 1));
                                        }

                                        logger.Info("Verifica se a opção esta ITM,ATM ou OTM.");

                                        if (StrikeOpcao == UltimaCotacaoPapelBase)
                                        {
                                            logger.Info("Opcao ATM");
                                            if (Oscilacao > OscilacaoMaximaATM)
                                            {
                                                VolumeAjustadoLimite = (UltimaCotacao * QuantidadeDescoberto);
                                                LimiteOscilacaoAtingido = true;
                                            }
                                        }
                                        else if (StrikeOpcao < UltimaCotacaoPapelBase)
                                        {
                                            logger.Info("Opcao ITM");
                                            if (Oscilacao > OscilacaoMaximaITM)
                                            {
                                                VolumeAjustadoLimite = (UltimaCotacao * QuantidadeDescoberto);
                                                LimiteOscilacaoAtingido = true;
                                            }
                                        }
                                        else if (StrikeOpcao > UltimaCotacaoPapelBase)
                                        {
                                            logger.Info("Opcao OTM");
                                            if (Oscilacao > OscilacaoMaximaOTM)
                                            {
                                                VolumeAjustadoLimite = (UltimaCotacao * QuantidadeDescoberto);
                                                LimiteOscilacaoAtingido = true;
                                            }
                                        }

                                        #endregion

                                        string Historico = string.Empty;

                                        if (LimiteOscilacaoAtingido == true)
                                        {
                                            logger.Info("O Limte de Oscilação Atingido");

                                            Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                            Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                            Historico += " Quantidade: " + QuantidadeDescoberto.ToString();
                                            Historico += " Preco: " + UltimaCotacao.ToString();
                                        }
                                        else
                                        {
                                            logger.Info("O Limte de Oscilação não foi Atingido");

                                            Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                            Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                            Historico += " Quantidade: " + QuantidadeDescoberto.ToString();
                                            Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();

                                        }

                                        logger.Info("Atualiza os Limites do cliente");

                                        bool BLAtualizaSaldo;

                                        if (LimiteOscilacaoAtingido == true)
                                        {

                                            logger.Info("Saldo do Volume ajustado: " + VolumeAjustadoLimite.ToString());

                                            BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                                idClienteParametroPermissao,
                                                VolumeAjustadoLimite,
                                                Historico,
                                                pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                                'V'
                                            );
                                        }
                                        else
                                        {
                                            BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                               idClienteParametroPermissao,
                                               ValorFinanceiroSolicitadoParaVenda,
                                               Historico,
                                               pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                               'V'
                                           );
                                        }

                                        if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                                        {
                                            //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                                            logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                            //ENVIA ORDEM PARA O ROTEADOR

                                            logger.Info("Envia a ordem para o roteador de ordens");

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                            var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                            {
                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                _RespostaOrdem.DataResposta = DateTime.Now;
                                                _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                            }
                                            else
                                            {
                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        logger.Info("Cliente não possui custódia suficiente para efetuar o lançamento coberto de opções.");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Cliente não possui custódia suficiente para efetuar o lançamento coberto de opções.";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);
                                    }
                                }
                                else
                                {
                                    logger.Info("Cliente não possui custódia suficiente.");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Cliente não possui custódia suficiente.";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                }

                            }

                        }
                        else
                        {
                            logger.Info("Cliente não possui custódia suficiente.");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = "Cliente não possui custódia suficiente.";
                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }
                    }
                }

                catch (Exception ex)
                {
                    _RespostaOrdem.DataResposta = DateTime.Now;
                    _RespostaOrdem.StackTrace = ex.StackTrace;
                    _RespostaOrdem.DescricaoResposta = ex.Message;
                    _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Exception;

                }

                _RespostaOrdem.CriticaInfo = CriticaInfoCollection;

                return TratamentoRetorno(_RespostaOrdem);

            }
            else
            {
                VolumeOperacao = (pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.DBToDecimal());

                #region ["Envia Ordem direto para o roteador"]

                pParametros.EnviarOrdemRequest.OrdemInfo.OrdStatus = OrdemStatusEnum.SUBSTITUICAOSOLICITADA;

                // Atualiza o status da ordem do cliente.
                if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Alteracao de ordem enviada com sucesso para o Roteador de Ordens");

                        _RespostaOrdem.DataResposta = DateTime.Now;
                        _RespostaOrdem.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        _RespostaOrdem.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a alteracao de ordens para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }

                }

                #endregion

            }

            return _RespostaOrdem;

        }


        #endregion

        #region [ Alteracao de Ordem BMF ]

        private ValidacaoRiscoResponse AlterarOrdemCompraBMF(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {

            decimal Preco, PrecoOriginal;
            int Qtde, QtdeOriginal;

            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();

            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse ResponstaRisco = new ValidacaoRiscoResponse();

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());

            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Qtde = (OrdemInfoAlteracao.OrderQty);
            QtdeOriginal = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            int QuantidadeExecutada = (OrdemInfoAlteracao.OrderQty - OrdemInfoAlteracao.OrderQtyRemmaining);
            pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty + QuantidadeExecutada);

            if ((Qtde == QtdeOriginal) && (Preco == PrecoOriginal))
            {

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "É preciso alterar o preço ou quantidade da ordem.";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                ResponstaRisco.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(ResponstaRisco);
            }


            logger.Info("Obtem código do cliente BMF.");

            ClienteAtividadeBmfRequest lRequest = new ClienteAtividadeBmfRequest();

            lRequest.CodigoBase = pParametros.EnviarOrdemRequest.OrdemInfo.Account;

            ClienteAtividadeBmfResponse Resposta = new PersistenciaRisco().ObterCodigoBmfCliente(lRequest);

            pParametros.EnviarOrdemRequest.OrdemInfo.Account = Resposta.CodigoBmf;

            logger.Info("Código BMF gerado com sucesso. " + Resposta.CodigoBmf.ToString());

            string[] SecurityList = SecurityListInfo.htSecurityList[pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim()].ToString().Split('|');

            string SecurityID = SecurityList[0].ToString();
            string SecurityIDSource = SecurityList[1].ToString();

            logger.Info("Obtem SecurityID do instrumento. " + SecurityID);

            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Qtde = (OrdemInfoAlteracao.OrderQty);
            QtdeOriginal = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            if ((Qtde == QtdeOriginal) && (Preco == PrecoOriginal))
            {

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "É preciso alterar o preço ou quantidade da ordem.";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                ResponstaRisco.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(ResponstaRisco);
            }

            pParametros.EnviarOrdemRequest.OrdemInfo.Exchange = "BMF";
            pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID = 0;
            pParametros.EnviarOrdemRequest.OrdemInfo.SecurityID = SecurityID;
            pParametros.EnviarOrdemRequest.OrdemInfo.SecurityIDSource = SecurityIDSource;

            logger.Info("Grava a ordem antiga na tabela de alteracao de ordens bmf");
            new PersistenciaOrdens().InserirOrdemAtualizada(OrdemInfoAlteracao);

            logger.Info("Envia solicitação para gravar ordem no banco de dados");
            if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
            {
                logger.Info("Ordem inserida no banco de dados com sucesso");

                //pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);
                pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce = OrdemValidadeEnum.ValidaParaODia;

                logger.Info("Encaminha solicitação de compra para o roteamento de ordem");
                var ResponseOrdens = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                //Verifica Retorno do roteador
                if (ResponseOrdens.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                {
                    logger.Info("Ordem enviada para a BMF com sucesso");

                    ResponstaRisco.DataResposta = DateTime.Now;
                    ResponstaRisco.DescricaoResposta = ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    ResponstaRisco.StatusResposta = CriticaRiscoEnum.Sucesso;

                }
                else
                {
                    logger.Info("Ocorreu um erro ao enviar a ordem para a BMF");
                    logger.Info("Descrição do erro:    " + ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia);

                    PipeLineCriticaInfo info = new PipeLineCriticaInfo();

                    info.Critica = ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                    info.DataHoraCritica = DateTime.Now;

                    ResponstaRisco.CriticaInfo.Add(info);
                    ResponstaRisco.DataResposta = DateTime.Now;
                    ResponstaRisco.DescricaoResposta = "Ocorreu um erro ao enviar a ordem de BMF";
                    ResponstaRisco.StatusResposta = CriticaRiscoEnum.Exception;

                }
            }
            else
            {
                logger.Info("Ocorreu um erro ao gravar a ordem no banco de dados.");
            }


            return TratamentoRetorno(ResponstaRisco);


        }

        private ValidacaoRiscoResponse AlterarOrdemVendaBMF(ValidacaoRiscoRequest pParametros, OrdemInfo OrdemInfoAlteracao, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            decimal Preco, PrecoOriginal;
            int Qtde, QtdeOriginal;

            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();

            PipeLineCriticaInfo CriticaInfo = null;

            ValidacaoRiscoResponse ResponstaRisco = new ValidacaoRiscoResponse();

            logger.Info("Obtem código do cliente BMF.");

            ClienteAtividadeBmfRequest lRequest = new ClienteAtividadeBmfRequest();

            lRequest.CodigoBase = pParametros.EnviarOrdemRequest.OrdemInfo.Account;
            Preco = decimal.Parse(OrdemInfoAlteracao.Price.ToString());
            PrecoOriginal = decimal.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());

            Qtde = (OrdemInfoAlteracao.OrderQty);
            QtdeOriginal = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;
            pParametros.EnviarOrdemRequest.OrdemInfo.IdOrdem = OrdemInfoAlteracao.IdOrdem;

            int QuantidadeExecutada = (OrdemInfoAlteracao.OrderQty - OrdemInfoAlteracao.OrderQtyRemmaining);
            pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty + QuantidadeExecutada);

            if ((Qtde == QtdeOriginal) && (Preco == PrecoOriginal))
            {

                CriticaInfo = new PipeLineCriticaInfo();
                CriticaInfo.Critica = "É preciso alterar o preço ou quantidade da ordem.";
                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                CriticaInfo.DataHoraCritica = DateTime.Now;

                // Adiciona as criticas no pipeline de risco.
                CriticaInfoCollection.Add(CriticaInfo);

                ResponstaRisco.CriticaInfo = CriticaInfoCollection;
                return TratamentoRetorno(ResponstaRisco);
            }

            ClienteAtividadeBmfResponse Resposta = new PersistenciaRisco().ObterCodigoBmfCliente(lRequest);

            pParametros.EnviarOrdemRequest.OrdemInfo.Account = Resposta.CodigoBmf;

            logger.Info("Código BMF gerado com sucesso. " + Resposta.CodigoBmf.ToString());

            string[] SecurityList = SecurityListInfo.htSecurityList[pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim()].ToString().Split('|');

            string SecurityID = SecurityList[0].ToString();
            string SecurityIDSource = SecurityList[1].ToString();

            logger.Info("Obtem SecurityID do instrumento. " + SecurityID);

            pParametros.EnviarOrdemRequest.OrdemInfo.Exchange = "BMF";
            pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID = 0;
            pParametros.EnviarOrdemRequest.OrdemInfo.SecurityID = SecurityID;
            pParametros.EnviarOrdemRequest.OrdemInfo.SecurityIDSource = SecurityIDSource;

            logger.Info("Grava a ordem antiga na tabela de alteracao de ordens bmf");
            new PersistenciaOrdens().InserirOrdemAtualizada(OrdemInfoAlteracao);

            logger.Info("Envia solicitação para gravar ordem no banco de dados");
            if (AtualizaOrdemCliente(pParametros.EnviarOrdemRequest))
            {
                logger.Info("Ordem inserida no banco de dados com sucesso");

                //pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);
                pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce = OrdemValidadeEnum.ValidaParaODia;

                logger.Info("Encaminha solicitação de compra para o roteamento de ordem");
                var ResponseOrdens = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                //Verifica Retorno do roteador
                if (ResponseOrdens.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                {
                    logger.Info("Ordem enviada para a BMF com sucesso");

                    ResponstaRisco.DataResposta = DateTime.Now;
                    ResponstaRisco.DescricaoResposta = ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    ResponstaRisco.StatusResposta = CriticaRiscoEnum.Sucesso;

                }
                else
                {
                    logger.Info("Ocorreu um erro ao enviar a ordem para a BMF");
                    logger.Info("Descrição do erro:    " + ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia);

                    PipeLineCriticaInfo info = new PipeLineCriticaInfo();

                    info.Critica = ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                    info.DataHoraCritica = DateTime.Now;

                    ResponstaRisco.CriticaInfo.Add(info);
                    ResponstaRisco.DataResposta = DateTime.Now;
                    ResponstaRisco.DescricaoResposta = "Ocorreu um erro ao enviar a ordem de BMF";
                    ResponstaRisco.StatusResposta = CriticaRiscoEnum.Exception;

                }
            }
            else
            {
                logger.Info("Ocorreu um erro ao gravar a ordem no banco de dados.");
            }


            return TratamentoRetorno(ResponstaRisco);


        }

        #endregion

        #endregion

        #region Envio de Ordem Bovespa cliente normal

        #region Regras e Validações [ Compra e Venda Mercado A Vista e Opções ]

        /// <summary>
        /// Efetua a validação de risco de compra de ações no mercado a vista
        /// </summary>
        /// <param name="pParametros"> Objeto contemplando a ordem do cliente</param>
        /// <param name="_responseClienteRiscoParam">objeto contendo um conjunto de criticas pertinentes a validação de risco da ordem/param>
        /// <returns></returns>
        private ValidacaoRiscoResponse ValidarCompraAcoes(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {
            // Armazena as propriedades em caso de alteração de ordem;
            //OrdemInfo OrdemInfoAlteracao;
            bool FlgAlteracaoOrdem = false;

            decimal VolumeOperacao = 0;

            int FatorCotacao = int.Parse(CadastroPapeis.Objeto.FatorCotacao);

            logger.Info("Inicia Rotina de validação de compra de ações, cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

            logger.Info("Caracteristicas da ordem");
            logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Operação            :Compra");
            logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Preco               :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
            logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());

            #region [ Alteração de Ordem ]

            // Verifica se o ID da ordem já existe no banco de dados.
            OrdemInfo OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
            {
                logger.Info("Alteração de ordem solicitada");
                logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                logger.Info("Inicia rotina de validação de alteração de ordens.");

                return this.AlterarOrdemCompra(pParametros, OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
            }

            #endregion

            #region Fator de Cotacao

            //CadastroPapelInfo InstrumentoInfo = (CadastroPapelInfo) (ObtemFatorCotacaoInstrumento[pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim()]);
            //var FatorCotacao = InstrumentoInfo.FatorCotacao;

            #endregion

            //OBJETO RESPONSAVEL POR GUARDAR TODAS AS CRITICAS FEITAS DURANTE O ENVIO DE UMA ORDEM
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo CriticaInfo = null;

            //OBJETO RESPONSAVEL POR ARMAZENAR O SALDO E CONTA CORRENTE DO CLIENTE
            SaldoContaCorrenteResponse<ContaCorrenteInfo> SaldoContaCorrenteResponse = null;

            // OBJETO RESPONSAVEL POR ENCAPSULAR OS RETORNOS DA VALIDAÇÃO DA ORDEM DO CLIENTE
            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();

            try
            {

                // VERIFICA PERMISSAO NO MERCADO A VISTA

                logger.Info("Valida Permissão do cliente para operar no mercado a vista");
                var PermissaoOperarMercadoAVista = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                   where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_NO_MERCADO_AVISTA

                                                   select p;

                if (PermissaoOperarMercadoAVista.Count() == 0)
                {
                    logger.Info("Permissão de operar no mercado a vista negada.");
                }

                // CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO A VISTA
                if (PermissaoOperarMercadoAVista.Count() > 0)
                {
                    logger.Info("Permissão de operar no mercado a vista concedida com sucesso.");

                    //**************************************************************************
                    // 3º VALIDA SALDO EM CONTA CORRENTE DO CLIENTEb 
                    //**************************************************************************

                    VolumeOperacao = ((pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.DBToDecimal()));

                    logger.Info("Volume solicitado para a operação: " + string.Format("{0:c}", VolumeOperacao));
                    logger.Info("Inicia consulta de saldo em conta corrente");

                    //OBTEM O SALDO TOTAL DE CONTA CORRENTE DO CLIENTE
                    SaldoContaCorrenteResponse = new ServicoContaCorrente().ObterSaldoContaCorrente(
                      new SaldoContaCorrenteRequest()
                      {
                          IdCliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                      });

                    #region Papeis permitidos conta margem

                    var InstrumentoAlavancagem = from p in ListaInstrumentoAlavancagem.ListaPapeisAlavancagem
                                                 where p.Ativo == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                 select p;

                    if (InstrumentoAlavancagem.Count() == 0)
                    {
                        SaldoContaCorrenteResponse.Objeto.SaldoContaMargem = 0;
                    }

                    #endregion

                    // Obtem total de bloqueios ( Ofertas em aberto ) do cliente
                    Nullable<Decimal> SaldoBloqueado = SaldoContaCorrenteResponse.Objeto.SaldoBloqueado;

                    if (SaldoContaCorrenteResponse.StatusResposta == ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                    {
                        // Forma o saldo para em ações
                        SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes = (SaldoContaCorrenteResponse.Objeto.SaldoD0 + SaldoContaCorrenteResponse.Objeto.SaldoD1 + SaldoContaCorrenteResponse.Objeto.SaldoD2 + SaldoContaCorrenteResponse.Objeto.SaldoD3 + decimal.Parse(SaldoContaCorrenteResponse.Objeto.SaldoContaMargem.ToString()));
                        SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes = SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes + decimal.Parse(SaldoContaCorrenteResponse.Objeto.SaldoBloqueado.ToString());

                        logger.Info("Saldo em conta corrente carregado com sucesso. Saldo em compra de ações: " + string.Format("{0:c}", SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes.ToString()));

                        // TRATAMENTO [ FRACIONARIO / INTEGRAL ]
                        int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
                        int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

                        logger.Info("Verifica se o saldo em conta corrente é suficiente para cobrir a operação");

                        // VERIFICA SE O SALDO EM CONTA CORRENTE CONSEGUE COBRIR A OPERACAO
                        // SEM UTILIZACAO DE LIMITE OPERACIONAL
                        if (VolumeOperacao <= SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes)
                        {
                            logger.Info("Saldo em CC suficiente para efetuar a compra sem a utilização de limite");

                            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                            {
                                // ENVIAR INTEGRAL
                                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;
                                logger.Info("Inicia operação de compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");


                                // Inseri a Ordem solicitada no banco de dados    
                                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                {
                                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                                    logger.Info("Envia a ordem para o roteador de ordens");

                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                    {
                                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                    }
                                    else
                                    {
                                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                    }
                                }


                                if (ModuloLoteNegociacao > 0)
                                {

                                    logger.Info("Calcula restante a ser enviado para o mercado fracionário");


                                    // ENVIAR FRACIONARIO
                                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                    pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                                    // Inseri a Ordem solicitada no banco de dados    
                                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                    {
                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                        logger.Info("Envia a ordem para o roteador de ordens");


                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);


                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }
                                    }
                                }
                            }
                            else
                            {
                                // ENVIAR FRACIONARIO                       
                                pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                                // Inseri a Ordem solicitada no banco de dados    
                                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                {
                                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                                    //ENVIA ORDEM PARA O ROTEADOR                                    
                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                    logger.Info("Envia a ordem para o roteador de ordens");

                                    if (FlgAlteracaoOrdem)
                                    {
                                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }
                                    }
                                    else
                                    {
                                        var RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            // VOLUME DE COMPRA E MAIOR QUE O SALDO EM CONTA CORRENTE
                            if (VolumeOperacao > SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes)
                            {
                                logger.Info("Volume da operação de compra  maior que o saldo disponivel em ações");

                                /**************************************************************************
                                // 4º VALIDA PERMISSAO PARA UTILIZAÇÃO DE LIMITE NO MERCADO A VISTA
                                //**************************************************************************/

                                logger.Info("Verifica os Limites cadastrados para o cliente");

                                logger.Info("Inicia consulta para utilizar limite no mercado a vista");

                                var PermissaoUtilizarLimiteAVista = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                    where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_MERCADO_AVISTA
                                                                    orderby p.DtMovimento descending

                                                                    select p;

                                if (PermissaoUtilizarLimiteAVista.Count() == 0)
                                {
                                    logger.Info("Cliente não possui permissão para utilizar limite operacional");
                                }

                                //VERIFICA SE O CLIENTE POSSUI PERMISAO PARA UTILIZACAO DE LIMITE OPERACIONAL
                                if (PermissaoUtilizarLimiteAVista.Count() > 0)
                                {
                                    #region Estado do pregão


                                    if (pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID != NumeroPortaAssessor)
                                    {
                                        int EstadoPregao = new PersistenciaRisco().ObterEstadoPregao();

                                        if (EstadoPregao != 0)
                                        {
                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Não é permitido utilizar limite operacional fora do horário regular do pregão.";
                                            CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                            PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                            PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                            return PipeLineResponse;

                                        }
                                    }

                                    #endregion

                                    #region Lista Permitida Alavancagem

                                    if (pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID != NumeroPortaAssessor)
                                    {
                                        var _InstrumentoAlavancagem = from p in ListaInstrumentoAlavancagem.ListaPapeisAlavancagem
                                                                      where p.Ativo == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                                      select p;

                                        if (_InstrumentoAlavancagem.Count() == 0)
                                        {
                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Este ativo não esta liberado para suportar alavancagem operacional.";
                                            CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                            PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                            PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                            return PipeLineResponse;

                                        }

                                    }
                                    #endregion

                                    #region Ofertas 30 dias

                                    if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Não é possível abrir oferta de ações com validade para 30 dias utilizando Limite Operacional";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return PipeLineResponse;
                                    }

                                    #endregion

                                    //CLIENTE POSSUI PERMISSAO PARA UTILIZAR LIMITE OPERACIONAL     
                                    logger.Info("Cliente possui permissao para utilizar limite operacional");

                                    //OBTEM SALDO EM CONTA CORRENTE + CONTA MARGEM DO CLIENTE                                                        
                                    decimal SaldoDisponivelAcoes = SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes.DBToDecimal();

                                    /***********************************************************************************
                                     * 4º OBTEM VALOR DO LIMITE PARA O MERCADO A VISTA
                                     * *********************************************************************************/

                                    var ValorLimiteMercadoAVista = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                   where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_MERCADO_AVISTA
                                                                   && p.DtValidade >= DateTime.Now
                                                                   orderby p.DtMovimento descending

                                                                   select p;

                                    decimal SaldoLimiteOperacional = 0;
                                    decimal ValorAlocado = 0;
                                    int idClienteParametroPermissao = 0;

                                    // SOMA O SALDO DE CONTA CORRENTE COM O LIMITE OPERACIONAL DO CLIENTE.
                                    foreach (var ClientePosicao in ValorLimiteMercadoAVista)
                                    {
                                        SaldoLimiteOperacional = ClientePosicao.ValorParametro;
                                        ValorAlocado = ClientePosicao.ValorAlocado;
                                        idClienteParametroPermissao = ClientePosicao.IdClienteParametroPermissao;
                                        break;
                                    }

                                    logger.Info("Limite Operacional carregado com sucesso, valor disponivel em limite operacional :" + string.Format("{0:c}", SaldoLimiteOperacional));
                                    logger.Info("Saldo em conta corrente + Limite Operacional: " + string.Format("{0:c}", (SaldoDisponivelAcoes + SaldoLimiteOperacional)));

                                    // VERIFICA SE O SALDO EM CONTA CORRENTE + LIMITE OPERACIONAL 
                                    // É CAPAZ DE COBRIR A COMPRA A SER REALIZADA.

                                    decimal? SaldoRestante = (SaldoDisponivelAcoes - ValorAlocado);
                                    decimal? LimiteTotal = (SaldoLimiteOperacional + ValorAlocado);

                                    decimal? valor = ((SaldoDisponivelAcoes) + LimiteTotal);

                                    if (((SaldoDisponivelAcoes) + LimiteTotal) >= VolumeOperacao)
                                    {
                                        //TODO: INVOCAR SERVICO ENVIO ORDENS
                                        decimal ValorEmprestimo = 0;

                                        if ((SaldoDisponivelAcoes - ValorAlocado) > 0)
                                        {
                                            //Subtrai do saldo de ações
                                            ValorEmprestimo = (SaldoDisponivelAcoes - VolumeOperacao);
                                        }
                                        else
                                        {
                                            //Caso o saldo em ações seja menor ou igual a zero atribui o valor do volume da operacao
                                            ValorEmprestimo = VolumeOperacao;
                                        }

                                        string Historico = string.Empty;

                                        Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                        Historico = " Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                        Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();
                                        Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();


                                        logger.Info("Atualiza o limite operacional do cliente");
                                        // Atualiza e bloqueia limite do cliente antes de enviar a ordem 
                                        bool BLAtualizacaoLimite = new PersistenciaRisco().AtualizaLimiteCliente(
                                            idClienteParametroPermissao,
                                            Math.Abs(ValorEmprestimo),
                                            Historico,
                                             pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID
                                            );

                                        if (BLAtualizacaoLimite == true)
                                        {

                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                                                           
                                            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                                            {
                                                logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                                // ENVIAR INTEGRAL
                                                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;

                                                // Inseri a Ordem solicitada no banco de dados    
                                                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                                {
                                                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                                    // ENVIA ORDEM PARA O ROTEADOR                                                   
                                                    logger.Info("Envia a ordem para o roteador de ordens");

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                    if (FlgAlteracaoOrdem)
                                                    {

                                                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            PipeLineResponse.DataResposta = DateTime.Now;
                                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        var RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            PipeLineResponse.DataResposta = DateTime.Now;
                                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);

                                                        }
                                                    }

                                                }

                                                // ENVIAR FRACIONARIO
                                                if (ModuloLoteNegociacao > 0)
                                                {

                                                    logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                                                    // Inseri a Ordem solicitada no banco de dados    
                                                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                                    {
                                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                        // ENVIA ORDEM PARA O ROTEADOR                                                    

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                        logger.Info("Envia a ordem para o roteador de ordens");
                                                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);


                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            PipeLineResponse.DataResposta = DateTime.Now;
                                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // ENVIAR FRACIONARIO                                               
                                                logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                                // Inseri a Ordem solicitada no banco de dados    
                                                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                                {
                                                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                    logger.Info("Envia a ordem para o roteador de ordens");

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                    // ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                    if (FlgAlteracaoOrdem)
                                                    {
                                                        var RespostaRoteador = this.ModificarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            PipeLineResponse.DataResposta = DateTime.Now;
                                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        var RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                        //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            PipeLineResponse.DataResposta = DateTime.Now;
                                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);

                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS
                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = "Solicitação encaminhada para o serviço de ordens";
                                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;

                                    }
                                    else
                                        if (VolumeOperacao > (SaldoDisponivelAcoes + SaldoLimiteOperacional))
                                        {

                                            logger.Info("Saldo em conta corrente insuficiente para realizar a operação.");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Saldo em conta corrente insuficiente para realizar a operação";
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoContaCorrenteInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;


                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);
                                        }
                                }
                                else
                                {
                                    // CLIENTE NÃO POSSUI PERMISSAO PARA UTILIZAR LIMITE
                                    // SALDO EM CONTA CORRENTE INSUFICIENTE                                   

                                    logger.Info("Saldo em conta corrente insuficiente para realizar a operação");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Saldo em conta corrente insuficiente para realizar a operação";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoContaCorrenteInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                    logger.Info("Cliente não possui permissão para utlizar limite operacional");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Cliente não possui permissão para utlizar limite operacional";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                }
                            }
                        }
                    }
                    else
                    {

                        if (SaldoContaCorrenteResponse.StatusResposta == ContaCorrente.Lib.Enum.CriticaMensagemEnum.Exception)
                        {
                            logger.Info("Ocorreu um erro ao carregar o saldo em conta corrente do cliente");
                        }
                    }
                }
                else
                { // CLIENTE NAO POSSUI PERMISSAO PARA OPERAR NO MERCADO A VISTA                   

                    logger.Info("Cliente não possui permissão insuficiente para operar no mercado vista");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "Cliente não possui permissão suficiente para operar no mercado a vista";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);
                }
            }
            catch (Exception ex)
            {

                logger.Info("Ocorreu um erro ao invocar o método ValidarCompraAcoes");
                logger.Info("Descrição do erro    :" + ex.Message);

                PipeLineResponse.DataResposta = DateTime.Now;
                PipeLineResponse.StackTrace = ex.StackTrace;
                PipeLineResponse.DescricaoResposta = ex.Message;
                PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Exception;

            }

            PipeLineResponse.CriticaInfo = CriticaInfoCollection;

            return TratamentoRetorno(PipeLineResponse);
        }

        /// <summary>
        /// Efetua a validação de risco para venda de ações no mercado a vista
        /// </summary>
        /// <param name="pParametros"> Objeto contendo a ordem de venda do cliente</param>
        /// <param name="_responseClienteRiscoParam">objeto contendo um conjunto de criticas pertinentes a validação de risco da ordem</param>
        /// <returns></returns>
        private ValidacaoRiscoResponse ValidarVendaAcoes(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {

            logger.Info("Inicia Rotina de validação de venda de ações, cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

            logger.Info("Caracteristicas da ordem");
            logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Operação            :VENDA");
            logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Preco               :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
            logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());

            //OBJETO RESPONSAVEL POR GUARDAR TODAS AS CRITICAS FEITAS DURANTE O ENVIO DE UMA ORDEM
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo CriticaInfo = null;

            int QuantidadeOfertaEmAberto = 0;

            //OBJETO RESPONSAVEL POR ARMAZENAR O SALDO EM CUSTODIA DO CLIENTE
            SaldoCustodiaResponse<CustodiaClienteInfo> SaldoCustodiaCliente = null;

            // OBJETO RESPONSAVEL POR ENCAPSULAR OS RETORNOS DA VALIDAÇÃO DA ORDEM DO CLIENTE
            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();

            #region [ Alteração de Ordem ]

            // Verifica se o ID da ordem já existe no banco de dados.
            OrdemInfo OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
            {
                logger.Info("Alteração de ordem solicitada");
                logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                logger.Info("Inicia rotina de validação de alteração de ordens.");

                return this.AlterarOrdemVenda(pParametros, OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
            }

            #endregion

            try
            {
                logger.Info("Consulta permissão para vender no mercado a vista");

                var PermissaoOperarMercadoBovespa = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                    where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_NO_MERCADO_AVISTA

                                                    select p;

                if (PermissaoOperarMercadoBovespa.Count() == 0)
                {
                    logger.Info("Cliente não possui permissão para operar no mercado a vista");
                }

                // CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO A VISTA
                if (PermissaoOperarMercadoBovespa.Count() > 0)
                {
                    //**************************************************************************
                    // 3º VALIDA SALDO EM CUSTODIA DO CLIENTE
                    //**************************************************************************

                    #region [Calcula percentual Máximo de oscilação mercado a vista sem Limites 80%]

                    EnviarCotacaoResponse _ResponseCotacao = this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());

                    decimal _OscilacaoMaxima = 80;

                    decimal _CotacaoAtual = _ResponseCotacao.Objeto.Ultima;
                    decimal _PrecoEnviado = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();

                    if (_PrecoEnviado < _CotacaoAtual)
                    {
                        decimal ValorOscilacao = Math.Abs((((_PrecoEnviado / _CotacaoAtual) - 1) * 100));

                        if (ValorOscilacao > _OscilacaoMaxima)
                        {
                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = "Oscilação Máxima de 80% atingida, oferta recusada , preço fora do tunel, Por favor alterar o preço da oferta.";
                            CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                            PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                            PipeLineResponse.DataResposta = DateTime.Now;
                            PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                            PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                            return PipeLineResponse;

                        }
                    }

                    #endregion

                    // TRATAMENTO [ FRACIONARIO / INTEGRAL ]
                    int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
                    int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

                    logger.Info("Permissão para operar no mercado a vista concedida com sucesso.");

                    decimal QuantidadeSolicitadaVenda = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.DBToInt32();

                    logger.Info("Quantidade solicitada para venda: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
                    logger.Info("Inicia consulta de saldo em custódia do cliente");

                    //OBTEM A POSICAO EM CUSTODIA DO CLIENTE
                    SaldoCustodiaCliente = new ServicoCustodia().ObterCustodiaCliente(
                      new SaldoCustodiaRequest()
                      {
                          IdCliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                      });


                    if ((SaldoCustodiaCliente.StatusResposta == OMS.Custodia.Lib.Enum.CriticaMensagemEnum.Sucesso))
                    {

                        #region [ Ofertas em Aberto ]

                        string Instrumento = pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;

                        if (ModuloLoteNegociacao != 0)
                        {
                            Instrumento += "F";
                        }

                        string InstrumentoBase;

                        if (Instrumento.Substring(Instrumento.Length - 1, 1) == "F")
                        {
                            InstrumentoBase = Instrumento.Remove(Instrumento.Length - 1);
                        }
                        else
                        {
                            InstrumentoBase = Instrumento;
                        }

                        QuantidadeOfertaEmAberto = new PersistenciaCustodia().ListarBloqueioOpcoes(pParametros.EnviarOrdemRequest.OrdemInfo.Account, Instrumento);


                        #endregion

                        logger.Info("Posição em custódia carregada com sucesso");

                        logger.Info("Consulta se o cliente possui o papel solicitado para venda em custódia.");

                        if (SaldoCustodiaCliente.ColecaoObjeto != null)
                        {

                            //CARREGA A POSICAO EM CUSTODIA DO PAPEL QUE O CLIENTE DESEJA VENDER
                            var PosicaoCustodia = from p in SaldoCustodiaCliente.ColecaoObjeto
                                                  where p.CodigoInstrumento == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                  && p.CodigoCarteira == 21016 // Cliente só pode vender ações na carteira a vista.

                                                  select p;

                            int QuantidadeDisponivelCustodia = 0;


                            // OBTEM QUANTIDADE NEGOCIAVEL E CUSTODIA.
                            foreach (var ClienteCustodia in PosicaoCustodia)
                            {
                                QuantidadeDisponivelCustodia = ClienteCustodia.QtdeAtual;
                            }

                            logger.Info("Papel solicitado encontrado em custódia, quantidade disponivel para venda coberta: " + QuantidadeDisponivelCustodia.ToString());


                            // VERIFICA SE A QUANTIDADE SOLICITADA PARA VENDA ESTA BLOQUEADA DEVIDO A LANCAMENTO DE OPCÃO.
                            logger.Info("Verifica o total bloqueado ( qtde bloqueada ) para o papel solicitado");


                            if ((QuantidadeDisponivelCustodia + QuantidadeOfertaEmAberto) >= QuantidadeSolicitadaVenda)
                            {
                                logger.Info("Quantidade Solicitada para venda é suficiente para cobrir a solicitação");

                                #region BLOCO VENDA COBERTA

                                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                                {
                                    // ENVIAR INTEGRAL
                                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;

                                    logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                    // Inseri a Ordem solicitada no banco de dados    
                                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                    {
                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral salva com sucesso.");

                                        logger.Info("Envia a ordem para o roteador de ordens");

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                            logger.Info("Descrição do ero   :" + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }
                                    }
                                    else
                                    {
                                        // ENVIAR FRACIONARIO
                                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                        logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario");

                                        // Inseri a Ordem solicitada no banco de dados    
                                        if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                        {
                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                            logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                                            //ENVIA ORDEM PARA O ROTEADOR

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account += 5;

                                            logger.Info("Envia a ordem de venda para o roteador de ordens");

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                            {
                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                PipeLineResponse.DataResposta = DateTime.Now;
                                                PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                            }
                                            else
                                            {
                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                                logger.Info("Descrição erro     :" + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);
                                            }

                                        }
                                    }


                                    if (ModuloLoteNegociacao > 0)
                                    {
                                        logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                                        // ENVIAR FRACIONARIO
                                        pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                        pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                        logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");
                                        // Inseri a Ordem solicitada no banco de dados    
                                        if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                        {
                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                            logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                            logger.Info("Envia a ordem para o roteador de ordens");

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                            {
                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                PipeLineResponse.DataResposta = DateTime.Now;
                                                PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                            }
                                            else
                                            {
                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                                logger.Info("Descrição erro     :" + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                    // Inseri a Ordem solicitada no banco de dados    
                                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                    {
                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                                        logger.Info("Envia a ordem para o roteador de ordens");

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                            logger.Info("Descrição erro     :" + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }
                                    }

                                }


                                #endregion

                            }
                            else

                                // CLIENTE POSSUI O PAPEL , PORÉM, NAO POSSUI A QUANTIDADE SOLICITADA PARA VENDA.
                                if ((QuantidadeDisponivelCustodia + QuantidadeOfertaEmAberto) < QuantidadeSolicitadaVenda)
                                {
                                    //*****************************************************************
                                    // 4º VALIDA PERMISSAO PARA VENDER DESCOBERTO
                                    //*****************************************************************

                                    logger.Info("Cliente não possui custódia suficiente para vender o instrumento");
                                    logger.Info("Verifica permissão para vender o papel a descoberto");

                                    var PosicaoCustodiaDescoberto = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                    where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_VENDA_DESCOBERTO_AVISTA
                                                                    && p.DtValidade >= DateTime.Now
                                                                    orderby p.DtMovimento descending

                                                                    select p;

                                    if (PosicaoCustodiaDescoberto.Count() == 0)
                                    {
                                        logger.Info("Cliente não possui permissão para vender descoberto.");
                                    }

                                    // Verifica se o cliente possui permissao / saldo para vender descoberto
                                    if (PosicaoCustodiaDescoberto.Count() > 0)
                                    {

                                        #region [Calcula percentual Máximo de oscilação mercado a vista]

                                        EnviarCotacaoResponse ResponseCotacao = this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());

                                        decimal OscilacaoMaxima = 5;

                                        decimal CotacaoAtual = ResponseCotacao.Objeto.Ultima;
                                        decimal PrecoEnviado = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();

                                        if (PrecoEnviado < CotacaoAtual)
                                        {
                                            decimal ValorOscilacao = Math.Abs((((PrecoEnviado / CotacaoAtual) - 1) * 100));

                                            if (ValorOscilacao > OscilacaoMaxima)
                                            {
                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = "Oscilação Máxima de 5% atingida, oferta recusada , preço fora do tunel";
                                                CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);

                                                PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                                PipeLineResponse.DataResposta = DateTime.Now;
                                                PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                                PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                                return PipeLineResponse;

                                            }
                                        }

                                        #endregion

                                        #region Estado do pregão

                                        if (pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID != NumeroPortaAssessor)
                                        {
                                            int EstadoPregao = new PersistenciaRisco().ObterEstadoPregao();

                                            if (EstadoPregao != 0)
                                            {
                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = "Não é permitido utilizar limite operacional fora do horário regular do pregão.";
                                                CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);

                                                PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                                PipeLineResponse.DataResposta = DateTime.Now;
                                                PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                                PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                                return PipeLineResponse;

                                            }
                                        }

                                        #endregion

                                        #region Lista Permitida Alavancagem

                                        if (pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID != NumeroPortaAssessor)
                                        {
                                            var _InstrumentoAlavancagem = from p in ListaInstrumentoAlavancagem.ListaPapeisAlavancagem
                                                                          where p.Ativo == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                                          select p;

                                            if (_InstrumentoAlavancagem.Count() == 0)
                                            {
                                                CriticaInfo = new PipeLineCriticaInfo();
                                                CriticaInfo.Critica = "Este ativo não esta liberado para suportar alavancagem operacional.";
                                                CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                // Adiciona as criticas no pipeline de risco.
                                                CriticaInfoCollection.Add(CriticaInfo);

                                                PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                                PipeLineResponse.DataResposta = DateTime.Now;
                                                PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                                PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                                return PipeLineResponse;

                                            }

                                        }
                                        #endregion

                                        #region Ofertas 30 dias

                                        if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                        {
                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Não é possível abrir oferta de ações com validade para 30 dias utilizando Limite Operacional";
                                            CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                            PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                            PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                            return PipeLineResponse;
                                        }

                                        #endregion

                                        int ValorFinanceiroDescoberto = 0;
                                        int idClienteParametroPermissao = 0;

                                        foreach (var ClienteCustodia in PosicaoCustodiaDescoberto)
                                        {
                                            idClienteParametroPermissao = ClienteCustodia.IdClienteParametroPermissao;
                                            ValorFinanceiroDescoberto = (int)ClienteCustodia.ValorParametro;
                                            break;
                                        }

                                        logger.Info("Permissão para operar descoberto concedida com sucesso. Total disponivel: " + string.Format("{0:c}", ValorFinanceiroDescoberto.ToString()));

                                        decimal QuantidadeDescoberto = 0;

                                        if (QuantidadeDisponivelCustodia > 0)
                                        {
                                            QuantidadeDescoberto = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty - (QuantidadeDisponivelCustodia));
                                        }
                                        else
                                        {
                                            QuantidadeDescoberto = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty);
                                        }
                                        decimal ValorFinanceiroSolicitadoParaVenda = (pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * QuantidadeDescoberto);

                                        logger.Info("Total em custódia Coberta: " + QuantidadeDisponivelCustodia.ToString());
                                        logger.Info("Quantidade solicitada descoberto: " + QuantidadeDescoberto.ToString());

                                        decimal VolumeAjustadoLimite = 0;

                                        if (ValorFinanceiroSolicitadoParaVenda <= ValorFinanceiroDescoberto)
                                        {
                                            EnviarCotacaoResponse Response =
                                                this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());

                                            #region [Calcula percentual Máximo de oscilação]

                                            decimal PrecoOrdem = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();
                                            decimal UltimaCotacao = Response.Objeto.Ultima;

                                            bool LimiteOscilacaoAtingido = false;

                                            decimal Oscilacao = (((UltimaCotacao / PrecoOrdem) - 1) * 100);

                                            if (Oscilacao > OscilacaoMaximaPermitidaMercadoAVista)
                                            {
                                                VolumeAjustadoLimite = (UltimaCotacao * QuantidadeDescoberto);
                                                LimiteOscilacaoAtingido = true;
                                            }

                                            #endregion


                                            string Historico = string.Empty;

                                            Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                            Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                            Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();

                                            if (LimiteOscilacaoAtingido == true)
                                            {
                                                Historico += " Preco: " + UltimaCotacao.ToString();
                                            }
                                            else
                                            {
                                                Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();
                                            }


                                            logger.Info("Atualiza os Limites do cliente");

                                            bool BLAtualizaSaldo;
                                            if (LimiteOscilacaoAtingido == true)
                                            {
                                                BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                                   idClienteParametroPermissao,
                                                   VolumeAjustadoLimite,
                                                   Historico,
                                                   pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                                   'V'
                                                   );
                                            }
                                            else
                                            {
                                                BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                                  idClienteParametroPermissao,
                                                  ValorFinanceiroSolicitadoParaVenda,
                                                  Historico,
                                                  pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                                  'V'
                                                  );

                                            }

                                            if (BLAtualizaSaldo)
                                            {
                                                // Inseri a ordem no banco 

                                                logger.Info("Inseri a ordem no banco de dados.");

                                                if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                                                {
                                                    // ENVIAR INTEGRAL
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;

                                                    logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                                    // Inseri a Ordem solicitada no banco de dados    
                                                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                                    {
                                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                        logger.Info("Envia a ordem para o roteador de ordens");

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            PipeLineResponse.DataResposta = DateTime.Now;
                                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);

                                                        }
                                                    }

                                                    logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                                                    if (ModuloLoteNegociacao > 0)
                                                    {
                                                        // ENVIAR FRACIONARIO
                                                        pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                                        pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                                        logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");
                                                        // Inseri a Ordem solicitada no banco de dados    
                                                        if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                                        {
                                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                            logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                            logger.Info("Envia a ordem para o roteador de ordens");

                                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                            {
                                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                                PipeLineResponse.DataResposta = DateTime.Now;
                                                                PipeLineResponse.DescricaoResposta = "Ordem enviada com sucesso para o Roteador de Ordens";
                                                                PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                            }
                                                            else
                                                            {
                                                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                                CriticaInfo = new PipeLineCriticaInfo();
                                                                CriticaInfo.Critica = "Ocorreu um erro ao enviar a ordem para o roteador de Ordens.";
                                                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                                CriticaInfo.StackTrace = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                                                // Adiciona as criticas no pipeline de risco.
                                                                CriticaInfoCollection.Add(CriticaInfo);

                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // ENVIAR FRACIONARIO
                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario");

                                                    // Inseri a Ordem solicitada no banco de dados    
                                                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                                    {
                                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                        //ENVIA ORDEM PARA O ROTEADOR                                                            

                                                        logger.Info("Envia a ordem para o roteador de ordens");

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));


                                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                        {
                                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                            PipeLineResponse.DataResposta = DateTime.Now;
                                                            PipeLineResponse.DescricaoResposta = "Ordem enviada com sucesso para o Roteador de Ordens";
                                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                        }
                                                        else
                                                        {
                                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                            CriticaInfo = new PipeLineCriticaInfo();
                                                            CriticaInfo.Critica = "Ocorreu um erro ao enviar a ordem para o roteador de Ordens.";
                                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                            CriticaInfo.StackTrace = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                                            // Adiciona as criticas no pipeline de risco.
                                                            CriticaInfoCollection.Add(CriticaInfo);
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // CLIENTE NÃO TEM SALDO SUFICIENTE PARA VENDER OPÇÃO A DESCOBERTO
                                            logger.Info("Cliente não possui permissão ou valor suficiente para operar o valor solicitado a descoberto no mercado a vista.");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = "Cliente não possui permissão ou valor suficiente para operar o valor solicitado a descoberto no mercado a vista.";
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoCustodiaInsuficiente;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);

                                        }
                                    }
                                    else
                                    {
                                        // CLIENTE NÃO POSSUI PERMISSAO PARA VENDER CUSTODIA A DESCOBERTO.

                                        logger.Info("Cliente não possui permissão suficiente para operar descoberto no mercado a vista.");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Cliente não possui quantidade suficiente em custódia para efetuar a operação.";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Cliente não possui permissão suficiente para operar descoberto no mercado a vista";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                    }
                                }
                        }
                        else
                        {
                            //CLIENTE NÃO POSSUI O PAPEL EM CUSTODIA ( TENTATIVA DE VENDA A DESCOBERTO )
                            if (SaldoCustodiaCliente.ColecaoObjeto == null)
                            {
                                logger.Info(string.Format("{0}{1}{2}{3}", "RISCO: ", "Cliente: ", pParametros.EnviarOrdemRequest.OrdemInfo.Account, " [Cliente não possui o papel em custódia.]"));

                                //*****************************************************************
                                // 4º VALIDA PERMISSAO PARA VENDER DESCOBERTO
                                //*****************************************************************

                                logger.Info(string.Format("{0}{1}{2}{3}", "RISCO: ", "Cliente: ", pParametros.EnviarOrdemRequest.OrdemInfo.Account, " [Inicia validação de permissão para vender a descoberto no mercado de ações ]."));

                                var PosicaoCustodiaDescoberto = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                                where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_VENDA_DESCOBERTO_AVISTA
                                                                && p.DtValidade >= DateTime.Now
                                                                orderby p.DtMovimento descending

                                                                select p;

                                // Verifica se o cliente possui permissao / saldo para vender descoberto
                                if (PosicaoCustodiaDescoberto.Count() > 0)
                                {
                                    #region Estado do pregão

                                    int EstadoPregao = new PersistenciaRisco().ObterEstadoPregao();

                                    if (EstadoPregao != 0)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Não é permitido utilizar limite operacional fora do horário regular do pregão.";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return PipeLineResponse;

                                    }

                                    #endregion

                                    #region Lista Permitida Alavancagem

                                    var InstrumentoAlavancagem = from p in ListaInstrumentoAlavancagem.ListaPapeisAlavancagem
                                                                 where p.Ativo == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                                 select p;

                                    if (InstrumentoAlavancagem.Count() == 0)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Este ativo não esta liberado para suportar alavancagem operacional.";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return PipeLineResponse;

                                    }

                                    #endregion

                                    #region Ofertas 30 dias

                                    if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Não é possível abrir oferta de ações com validade para 30 dias utilizando Limite Operacional";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return PipeLineResponse;
                                    }

                                    #endregion

                                    int ValorFinanceiroDescoberto = 0;
                                    int idClienteParametroPermissao = 0;

                                    foreach (var ClienteCustodia in PosicaoCustodiaDescoberto)
                                    {
                                        idClienteParametroPermissao = ClienteCustodia.IdClienteParametroPermissao;
                                        ValorFinanceiroDescoberto = (int)ClienteCustodia.ValorParametro;
                                        break;
                                    }

                                    logger.Info(string.Format("{0}{1}{2}{3}", "RISCO: ", "Cliente: ", pParametros.EnviarOrdemRequest.OrdemInfo.Account, " [Permissão para operar descoberto no mercado de ações concedida. Saldo em Limite: " + string.Format("{0:c}", ValorFinanceiroDescoberto).ToString() + " ]."));
                                    decimal ValorFinanceiroSolicitadoParaVenda = (pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * QuantidadeSolicitadaVenda);

                                    decimal VolumeAjustadoLimite = 0;
                                    if (ValorFinanceiroSolicitadoParaVenda <= ValorFinanceiroDescoberto)
                                    {
                                        //TODO ENVIA ORDEM DE VENDA

                                        #region [Calcula percentual Máximo de oscilação]

                                        EnviarCotacaoResponse Response =
                                                this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());

                                        decimal PrecoOrdem = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();
                                        decimal UltimaCotacao = Response.Objeto.Ultima;

                                        bool LimiteOscilacaoAtingido = false;

                                        decimal Oscilacao = (((UltimaCotacao / PrecoOrdem) - 1) * 100);

                                        if (Oscilacao > OscilacaoMaximaPermitidaMercadoAVista)
                                        {
                                            VolumeAjustadoLimite = (UltimaCotacao * QuantidadeSolicitadaVenda);
                                            LimiteOscilacaoAtingido = true;
                                        }

                                        #endregion

                                        string Historico = string.Empty;

                                        if (LimiteOscilacaoAtingido == true)
                                        {
                                            Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                            Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                            Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();
                                            Historico += " Preco: " + Response.Objeto.Ultima.ToString();
                                        }
                                        else
                                        {
                                            Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                            Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                            Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();
                                            Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();

                                        }


                                        logger.Info(string.Format("{0}{1}{2}{3}", "RISCO: ", "Cliente: ", pParametros.EnviarOrdemRequest.OrdemInfo.Account, " [Ordem de venda enviada com sucesso.]"));

                                        bool BLAtualizaSaldo;
                                        // Atualiza os limites
                                        if (LimiteOscilacaoAtingido == true)
                                        {
                                            BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                                idClienteParametroPermissao,
                                                VolumeAjustadoLimite,
                                                Historico,
                                                pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                                'V'
                                            );
                                        }
                                        else
                                        {
                                            BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                                idClienteParametroPermissao,
                                                ValorFinanceiroSolicitadoParaVenda,
                                                Historico,
                                                pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                                'V'
                                            );
                                        }

                                        if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
                                        {
                                            // ENVIAR INTEGRAL
                                            pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;

                                            logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                            // Inseri a Ordem solicitada no banco de dados    
                                            if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                            {
                                                //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                logger.Info("Envia a ordem para o roteador de ordens");

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                {
                                                    logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                    PipeLineResponse.DataResposta = DateTime.Now;
                                                    PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                    PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                }
                                                else
                                                {
                                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                    CriticaInfo = new PipeLineCriticaInfo();
                                                    CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                                    // Adiciona as criticas no pipeline de risco.
                                                    CriticaInfoCollection.Add(CriticaInfo);

                                                }
                                            }

                                            logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                                            if (ModuloLoteNegociacao > 0)
                                            {

                                                // ENVIAR FRACIONARIO
                                                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                                                pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                                                pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber;

                                                logger.Info("Inicia venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");
                                                // Inseri a Ordem solicitada no banco de dados    
                                                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                                {
                                                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                    logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = 219775;

                                                    logger.Info("Envia a ordem para o roteador de ordens");

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));


                                                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                    {
                                                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                        PipeLineResponse.DataResposta = DateTime.Now;
                                                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                    }
                                                    else
                                                    {
                                                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                        CriticaInfo = new PipeLineCriticaInfo();
                                                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                                        // Adiciona as criticas no pipeline de risco.
                                                        CriticaInfoCollection.Add(CriticaInfo);

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // ENVIAR FRACIONARIO
                                            pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                                            logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                                            // Inseri a Ordem solicitada no banco de dados    
                                            if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                            {
                                                //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                                logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                                //ENVIA ORDEM PARA O ROTEADOR

                                                logger.Info("Envia a ordem para o roteador de ordens");

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                                ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                                // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                                {
                                                    logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                    PipeLineResponse.DataResposta = DateTime.Now;
                                                    PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                    PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                                }
                                                else
                                                {
                                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                                    CriticaInfo = new PipeLineCriticaInfo();
                                                    CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                                    // Adiciona as criticas no pipeline de risco.
                                                    CriticaInfoCollection.Add(CriticaInfo);
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {

                                        // CLIENTE NÃO TEM SALDO SUFICIENTE PARA VENDER O INSTRUMENTO DESCOBERTO     
                                        logger.Info("Cliente não possui saldo suficiente para vender a quantidade solicitada a descoberto: Quantidade solicitada: " + ValorFinanceiroSolicitadoParaVenda.ToString());

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Cliente não possui saldo suficiente para vender a quantidade solicitada a descoberto: Quantidade solicitada: " + ValorFinanceiroSolicitadoParaVenda.ToString();
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoCustodiaInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                    }
                                }
                                else
                                {
                                    // Cliente não possui papel em custódia
                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Cliente não possui o papel em custódia";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoCustodiaInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);


                                    // Cliente não possui permissão para operar descoberto.
                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Cliente não possui permissão para operar descoberto";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                }
                            }
                        }
                    }
                }
                else
                {
                    // Não possui permissão para operar no mercado a vista
                    logger.Info("Cliente não possui permissão suficiente para operar no mercado a vista");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "Cliente não possui permissão insuficiente para operar no mercado vista";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);
                }
            }
            catch (Exception ex)
            {
                PipeLineResponse.DataResposta = DateTime.Now;
                PipeLineResponse.StackTrace = ex.StackTrace;
                PipeLineResponse.DescricaoResposta = ex.Message;
                PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Exception;

            }

            PipeLineResponse.CriticaInfo = CriticaInfoCollection;
            return TratamentoRetorno(PipeLineResponse);
        }

        /// <summary>
        /// Efetua a validação de risco para a compra de opções
        /// </summary>
        /// <param name="pParametros"> Objeto contendo a ordem de venda do cliente</param>
        /// <param name="_responseClienteRiscoParam">objeto contendo um conjunto de criticas pertinentes a validação de risco da ordem</param>
        /// <returns></returns>
        private ValidacaoRiscoResponse ValidarCompraOpcoes(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam)
        {

            logger.Info("Inicia Rotina de validação de compra de opções, cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

            logger.Info("Caracteristicas da ordem");
            logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Operação            :'Compra");
            logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Preco               :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
            logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());

            //OBJETO RESPONSAVEL POR GUARDAR TODAS AS CRITICAS FEITAS DURANTE O ENVIO DE UMA ORDEM
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo CriticaInfo = null;

            #region [ Alteração de Ordem ]

            // Verifica se o ID da ordem já existe no banco de dados.
            OrdemInfo OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
            {
                logger.Info("Alteração de ordem solicitada");
                logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                logger.Info("Inicia rotina de validação de alteração de ordens.");

                return this.AlterarOrdemCompraOpcoes(pParametros, OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
            }

            #endregion

            //OBJETO RESPONSAVEL POR ARMAZENAR O SALDO E CONTA CORRENTE DO CLIENTE
            SaldoContaCorrenteResponse<ContaCorrenteInfo> SaldoContaCorrenteResponse = null;

            // OBJETO RESPONSAVEL POR ENCAPSULAR OS RETORNOS DA VALIDAÇÃO DA ORDEM DO CLIENTE
            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();

            // CLIENTE NÃO POSSUI CUSTODIA SUFICIENTE PARA ZERAR O PAPEL.
            // VERIFICAR SE ESTA FAZENDO UM LANÇAMENTO DESCOBERTO COM GARANTIA NA CARTEIRA 27
            string PapelBase = CadastroPapeis.Objeto.PapelObjeto;

            try
            {

                logger.Info("Verifica se a ordem enviada é uma alteração ou nova ordem");

                if (pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID != null)
                {
                    logger.Info("Resposta: Alteração de Ordem");
                }
                else
                {
                    logger.Info("Resposta: Inclusão de Oferta");
                }

                logger.Info("Inicia validação de permissão para operar no mercado de opções.");

                // Verifica a quantidade 
                if ((pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % 100) != 0)
                {
                    logger.Info("A quantidade digitada deve ser multiplo de 100");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A quantidade digitada deve ser multiplo de 100";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    PipeLineResponse.CriticaInfo = CriticaInfoCollection;

                    return TratamentoRetorno(PipeLineResponse);
                }

                var PermissaoOperarMercadoOpcoes = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                   where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_NO_MERCADO_OPCOES

                                                   select p;


                if (PermissaoOperarMercadoOpcoes.Count() == 0)
                {
                    logger.Info("Cliente não possui permissão para operar no mercado de opções");
                }

                if (PermissaoOperarMercadoOpcoes.Count() > 0)
                {

                    // VERIFICA SE A OPÇÃO QUE O CLIENTE ESTA QUERENDO NEGOCIAR ESTA VENCIDA OU SE HOJE É DIA DE VENCIMENTO DE OPÇÕES
                    #region [ Vencimento de opções ]

                    logger.Info("Permissão concedida para operar no mercado de opções com sucesso.");

                    logger.Info("Verifica se o cliente esta tentando abrir posição em dia de vncimento de opção ou negociando uma opção já vencida.");

                    lock (VencimentoInstrumentoInfo.htInstrumentos)
                    {
                        if (VencimentoInstrumentoInfo.htInstrumentos.Contains(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol))
                        {
                            DateTime dataVencimento = DateTime.Parse(VencimentoInstrumentoInfo.htInstrumentos[pParametros.EnviarOrdemRequest.OrdemInfo.Symbol].ToString());

                            //Vencimento de opção
                            if (dataVencimento.ToString("{dd/MM/yyyy}") == DateTime.Now.ToString("{dd/MM/yyyy}"))
                            {
                                logger.Info("Não é possível abrir posição em dia de vencimento de opções");

                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = "Não é possível abrir posição em dia de vencimento de opção. Caso queria zerar sua posição, entre com contato com a mesa de operações.";
                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                                PipeLineResponse.CriticaInfo = CriticaInfoCollection;

                                return TratamentoRetorno(PipeLineResponse);
                            }
                            else
                            {
                                logger.Info("Verifica se a série de opção esta ativa");
                                // Serie Vencida.
                                if (dataVencimento < DateTime.Now)
                                {
                                    logger.Info("A serie de opção referente ao instrumento " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " já esta vencida");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Esta serie de opção já esta vencida.";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                    PipeLineResponse.CriticaInfo = CriticaInfoCollection;

                                    return TratamentoRetorno(PipeLineResponse);
                                }
                                else
                                {
                                    logger.Info("Série de opção valida para operação");
                                }
                            }
                        }
                    }

                    #endregion

                    //**************************************************************************
                    // 3º VALIDA SALDO EM CONTA CORRENTE DO CLIENTE
                    //**************************************************************************
                    decimal VolumeOperacao = (pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.DBToDecimal());

                    logger.Info("Volume solicitado para operação: " + string.Format("{0:c}", VolumeOperacao));

                    logger.Info("Inicia consulta de saldo em conta corrente");

                    //OBTEM O SALDO TOTAL DE CONTA CORRENTE DO CLIENTE
                    SaldoContaCorrenteResponse = new ServicoContaCorrente().ObterSaldoContaCorrente(
                      new SaldoContaCorrenteRequest()
                      {
                          IdCliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                      });


                    #region Papeis permitidos conta margem

                    var InstrumentoAlavancagem = from p in ListaInstrumentoAlavancagem.ListaPapeisAlavancagem
                                                 where p.Ativo == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                 select p;

                    if (InstrumentoAlavancagem.Count() == 0)
                    {
                        SaldoContaCorrenteResponse.Objeto.SaldoContaMargem = 0;
                    }

                    #endregion

                    if (SaldoContaCorrenteResponse.StatusResposta == ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                    {
                        logger.Info("Saldo em conta corrente carregado com sucesso.");

                        // Saldo em opções.
                        SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes = (SaldoContaCorrenteResponse.Objeto.SaldoD0 + SaldoContaCorrenteResponse.Objeto.SaldoD1);
                        SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes = SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes + decimal.Parse(SaldoContaCorrenteResponse.Objeto.SaldoBloqueado.ToString());

                        logger.Info("Saldo disponível em opções sem utlização de limite operacional " + string.Format("{0:c}", SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes));

                        // VERIFICA SE O SALDO EM CONTA CORRENTE CONSEGUE COBRIR A OPERACAO
                        // SEM UTILIZACAO DE LIMITE OPERACIONAL
                        if (VolumeOperacao <= SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes)
                        {
                            // Encaminha solicitação de compra de opções para o banco de dados
                            if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                            {
                                logger.Info("Envia a ordem para o roteador de ordens");

                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                {
                                    logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                    PipeLineResponse.DataResposta = DateTime.Now;
                                    PipeLineResponse.DescricaoResposta = "Ordem enviada com sucesso para o Roteador de Ordens";
                                    PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                }
                                else
                                {
                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");
                                    logger.Info("Descricao do erro:  " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                }

                            }

                            PipeLineResponse.DataResposta = DateTime.Now;
                            PipeLineResponse.DescricaoResposta = "Ordem enviada para o Serviço de ordens";
                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            // VOLUME DE COMPRA E MAIOR QUE O SALDO EM CONTA CORRENTE
                            if (VolumeOperacao > SaldoContaCorrenteResponse.Objeto.SaldoCompraAcoes)
                            {
                                //CLIENTE POSSUI PERMISSAO PARA UTILIZAR LIMITE OPERACIONAL

                                logger.Info("Volume da operação maior do que o saldo disponivel em conta corrente");
                                logger.Info("Verifica permissão para utilizar limite no mercado de opções");

                                //OBTEM SALDO EM CONTA CORRENTE PROJETADO ATE D+1                                                    
                                decimal SaldoDisponivelOpcoes = SaldoContaCorrenteResponse.Objeto.SaldoCompraOpcoes.DBToDecimal();

                                /***********************************************************************************
                                 * 4º OBTEM VALOR DO LIMITE PARA O MERCADO DE OPCOES
                                 * *********************************************************************************/

                                var ValorLimiteMercadoOpcoes = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                               where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_MERCADO_OPCOES
                                                               && p.DtValidade >= DateTime.Now
                                                               orderby p.DtMovimento descending

                                                               select p;

                                decimal SaldoLimiteOperacional = 0;
                                decimal ValorAlocado = 0;
                                int idClienteParametroPermissao = 0;

                                #region Lista Permitida Alavancagem

                                if (pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID != NumeroPortaAssessor)
                                {

                                    var lInstrumentoAlavancagem = from p in ListaInstrumentoAlavancagem.ListaPapeisAlavancagem
                                                                  where p.Ativo == PapelBase
                                                                  select p;

                                    if (lInstrumentoAlavancagem.Count() == 0)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Este ativo não esta liberado para suportar alavancagem operacional.";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return PipeLineResponse;

                                    }
                                }

                                #endregion

                                #region Estado do pregão

                                if (pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID != NumeroPortaAssessor)
                                {
                                    int EstadoPregao = new PersistenciaRisco().ObterEstadoPregao();

                                    if (EstadoPregao != 0)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Não é permitido utilizar limite operacional fora do horário regular do pregão.";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return PipeLineResponse;

                                    }
                                }

                                #endregion

                                #region Opcoes 30 dias

                                if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                {
                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Não é possível abrir oferta de opções com validade para 30 dias utilizando Limite Operacional";
                                    CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                    PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                    PipeLineResponse.DataResposta = DateTime.Now;
                                    PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                    PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                    return PipeLineResponse;
                                }

                                #endregion

                                // SOMA O SALDO DE CONTA CORRENTE COM O LIMITE OPERACIONAL DO CLIENTE.
                                foreach (var ClientePosicao in ValorLimiteMercadoOpcoes)
                                {
                                    SaldoLimiteOperacional = ClientePosicao.ValorParametro;
                                    ValorAlocado = ClientePosicao.ValorAlocado;
                                    idClienteParametroPermissao = ClientePosicao.IdClienteParametroPermissao;
                                    break;
                                }

                                logger.Info("Limite operacional para o mercado de opções carregados com sucesso. Valor disponivel: " + string.Format("{0:c}", SaldoLimiteOperacional));
                                logger.Info("Saldo em conta corrente + Limite operacional em opções:  " + string.Format("{0:c}", (SaldoDisponivelOpcoes + SaldoLimiteOperacional)));

                                decimal? LimiteTotal = (SaldoLimiteOperacional + ValorAlocado);
                                // VERIFICA SE O SALDO EM CONTA CORRENTE + LIMITE OPERACIONAL 
                                // É CAPAZ DE COBRIR A COMPRA A SER REALIZADA.                         
                                if ((SaldoDisponivelOpcoes + LimiteTotal) >= VolumeOperacao)
                                {
                                    //TODO: INVOCAR SERVICO ENVIO ORDENS
                                    decimal ValorEmprestimo = 0;

                                    if (SaldoDisponivelOpcoes > 0)
                                    {
                                        //Subtrai do saldo de ações
                                        ValorEmprestimo = (SaldoDisponivelOpcoes - VolumeOperacao);
                                    }
                                    else
                                    {
                                        //Caso o saldo em ações seja menor ou igual a zero atribui o valor do volume da operacao
                                        ValorEmprestimo = VolumeOperacao;
                                    }

                                    logger.Info("Valor do emprestimo a ser utilizado para realizar esta operação é de: " + string.Format("{0:c}", ValorEmprestimo));

                                    string Historico = string.Empty;

                                    Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                    Historico = " Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                    Historico += " Quantidade: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString();
                                    Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();

                                    logger.Info("Atualiza saldo do cliente");

                                    bool BLAtualizacaoLimite = new PersistenciaRisco().AtualizaLimiteCliente(
                                        idClienteParametroPermissao,
                                        Math.Abs(ValorEmprestimo),
                                        Historico,
                                          pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID
                                        );

                                    if (BLAtualizacaoLimite == true)
                                    {

                                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                        logger.Info("Limite atualizado com sucesso.");

                                        logger.Info("Inseri bloqueio para o cliente.");

                                        if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                        {
                                            //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                                            logger.Info("Ordem inserida no banco de dados com sucesso.");

                                            logger.Info("Envia a ordem para o roteador de ordens");

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                            pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));


                                            // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                            {
                                                logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                                PipeLineResponse.DataResposta = DateTime.Now;
                                                PipeLineResponse.DescricaoResposta = "Ordem enviada com sucesso para o Roteador de Ordens";
                                                PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                            }
                                        }
                                    }

                                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS
                                    PipeLineResponse.DataResposta = DateTime.Now;
                                    PipeLineResponse.DescricaoResposta = "Ordem enviada para o Serviço de ordens";
                                    PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;

                                }
                                else
                                    if (VolumeOperacao > (SaldoDisponivelOpcoes + SaldoLimiteOperacional))
                                    {
                                        logger.Info("Saldo em conta corrente insuficiente para envia a ordem.");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Saldo em conta corrente insuficiente para envia a ordem.";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoContaCorrenteInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;


                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);
                                    }
                            }
                            else
                            { // CLIENTE NÃO POSSUI PERMISSAO PARA UTILIZAR LIMITE
                                // SALDO EM CONTA CORRENTE INSUFICIENTE                      


                                logger.Info("Saldo em conta corrente insuficiente para realizar a operação.");

                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = "Saldo em conta corrente insuficiente para envia a ordem.";
                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.SaldoContaCorrenteInsuficiente;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                            }
                        }
                    }
                    else
                    {
                        if (SaldoContaCorrenteResponse.StatusResposta == ContaCorrente.Lib.Enum.CriticaMensagemEnum.Exception)
                        {
                            logger.Info("Ocorreu um erro ao carregar o saldo em conta corrente do corrente.");
                        }
                    }
                }
                else
                { // CLIENTE NAO POSSUI PERMISSAO PARA OPERAR NO MERCADO DE OPCOES

                    logger.Info("Cliente não possui permissão para operar no mercado de opções.");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "Cliente não possui permissão para operar no mercado de opções.";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);
                }


                PipeLineResponse.CriticaInfo = CriticaInfoCollection;

            }
            catch (Exception ex)
            {
                PipeLineResponse.DataResposta = DateTime.Now;
                PipeLineResponse.StackTrace = ex.StackTrace;
                PipeLineResponse.DescricaoResposta = ex.Message;
                PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Exception;

            }

            // Formata o retorno da mensagem.
            return TratamentoRetorno(PipeLineResponse);

        }

        /// <summary>
        /// Efetua a validação de risco para venda no mercado de opções
        /// </summary>
        /// <param name="pParametros"> Objeto contendo a ordem de venda do cliente</param>
        /// <param name="_responseClienteRiscoParam">objeto contendo um conjunto de criticas pertinentes a validação de risco da ordem</param>
        /// <returns></returns>
        private ValidacaoRiscoResponse ValidarVendaOpcoes(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam)
        {
            logger.Info("Inicia Rotina de validação de venda de opções, cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

            logger.Info("Caracteristicas da ordem");
            logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Operação            :VENDA");
            logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Preco               :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
            logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());


            //OBJETO RESPONSAVEL POR GUARDAR TODAS AS CRITICAS FEITAS DURANTE O ENVIO DE UMA ORDEM
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo CriticaInfo = null;

            //OBJETO RESPONSAVEL POR ARMAZENAR O SALDO EM CUSTODIA DO CLIENTE
            SaldoCustodiaResponse<CustodiaClienteInfo> SaldoCustodiaCliente = null;

            // OBJETO RESPONSAVEL POR ENCAPSULAR OS RETORNOS DA VALIDAÇÃO DA ORDEM DO CLIENTE

            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();

            #region [ Alteração de Ordem ]

            // Verifica se o ID da ordem já existe no banco de dados.
            OrdemInfo OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
            {
                logger.Info("Alteração de ordem solicitada");
                logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                logger.Info("Inicia rotina de validação de alteração de ordens.");

                return this.AlterarOrdemVendaOpcoes(pParametros, OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
            }

            #endregion

            // CLIENTE NÃO POSSUI CUSTODIA SUFICIENTE PARA ZERAR O PAPEL.
            // VERIFICAR SE ESTA FAZENDO UM LANÇAMENTO DESCOBERTO COM GARANTIA NA CARTEIRA 27

            string PapelBase = CadastroPapeis.Objeto.PapelObjeto;

            int QuantidadeSolicitada = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty;

            int QuantidadeOfertaEmAberto = 0;

            try
            {

                // Verifica a quantidade 
                if ((pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % 100) != 0)
                {
                    logger.Info("A quantidade digitada deve ser multiplo de 100");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "A quantidade digitada deve ser multiplo de 100";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);

                    PipeLineResponse.CriticaInfo = CriticaInfoCollection;

                    return TratamentoRetorno(PipeLineResponse);
                }

                logger.Info("Inicializa validação de permissão para operar no mercado de opções");

                var PermissaoOperarMercadoOpcoes = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                   where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_NO_MERCADO_OPCOES
                                                   select p;

                if (PermissaoOperarMercadoOpcoes.Count() == 0)
                {

                    logger.Info("Cliente não possui permissão para operar no mercado de opções");

                    CriticaInfo = new PipeLineCriticaInfo();
                    CriticaInfo.Critica = "Cliente não possui permissão para operar no mercado de opções";
                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                    CriticaInfo.DataHoraCritica = DateTime.Now;

                    // Adiciona as criticas no pipeline de risco.
                    CriticaInfoCollection.Add(CriticaInfo);
                }

                // CLIENTE POSSUI PERMISSAO PARA OPERAR NO MERCADO DE OPÇÕES
                if (PermissaoOperarMercadoOpcoes.Count() > 0)
                {
                    logger.Info("Permissão concedida para operar no mercado de opções");


                    #region [Calcula percentual Máximo de oscilação mercado a vista sem Limites 80%]

                    EnviarCotacaoResponse _ResponseCotacao = this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());

                    decimal OscilacaoMaxima = 80;

                    decimal CotacaoAtual = _ResponseCotacao.Objeto.Ultima;
                    decimal PrecoEnviado = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();

                    if (PrecoEnviado < CotacaoAtual)
                    {
                        decimal ValorOscilacao = Math.Abs((((PrecoEnviado / CotacaoAtual) - 1) * 100));

                        if (ValorOscilacao > OscilacaoMaxima)
                        {
                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = "Oscilação Máxima de 80% atingida, oferta recusada , preço fora do tunel, Por favor alterar o preço da oferta.";
                            CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                            PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                            PipeLineResponse.DataResposta = DateTime.Now;
                            PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                            PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                            return PipeLineResponse;

                        }
                    }

                    #endregion


                    // VERIFICA SE A OPÇÃO QUE O CLIENTE ESTA QUERENDO NEGOCIAR ESTA VENCIDA OU SE HOJE É DIA DE VENCIMENTO DE OPÇÕES
                    #region [ Vencimento de opções ]

                    logger.Info("Verifica se o cliente esta tentando abrir posição em dia de vncimento de opção ou negociando uma opção já vencida.");

                    lock (VencimentoInstrumentoInfo.htInstrumentos)
                    {
                        if (VencimentoInstrumentoInfo.htInstrumentos.Contains(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol))
                        {
                            DateTime dataVencimento = DateTime.Parse(VencimentoInstrumentoInfo.htInstrumentos[pParametros.EnviarOrdemRequest.OrdemInfo.Symbol].ToString());

                            //Vencimento de opção
                            if (dataVencimento.ToString("{dd/MM/yyyy}") == DateTime.Now.ToString("{dd/MM/yyyy}"))
                            {
                                logger.Info("Não é possível abrir posição em dia de vencimento de opções");

                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = "Não é possível abrir posição em dia de vencimento de opção. Caso queria zerar sua posição, entre com contato com a mesa de operações.";
                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                                PipeLineResponse.CriticaInfo = CriticaInfoCollection;

                                return TratamentoRetorno(PipeLineResponse);
                            }
                            else
                            {
                                logger.Info("Verifica se a série de opção esta ativa");
                                // Serie Vencida.
                                if (dataVencimento < DateTime.Now)
                                {
                                    logger.Info("A serie de opção referente ao instrumento " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " já esta vencida");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Esta serie de opção já esta vencida.";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                    PipeLineResponse.CriticaInfo = CriticaInfoCollection;

                                    return TratamentoRetorno(PipeLineResponse);
                                }
                                else
                                {
                                    logger.Info("Série de opção valida para operação");
                                }
                            }
                        }
                    }

                    #endregion

                    SaldoCustodiaCliente = new ServicoCustodia().ObterCustodiaCliente(
                    new SaldoCustodiaRequest()
                    {
                        IdCliente = pParametros.EnviarOrdemRequest.OrdemInfo.Account.DBToInt32()
                    });


                    decimal VolumeOperacao = (pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.DBToDecimal());

                    #region [ Verifica Custodia para zerar posição ]

                    int QuantidadeOpcaoCustodiaCarteira = 0;

                    // Existe custódia para realizar a operacao 
                    if ((SaldoCustodiaCliente.ColecaoObjeto != null) && (SaldoCustodiaCliente.ColecaoObjeto.Count > 0))
                    {

                        //TODO: VERIFICA SE CLIENTE ESTA ZERANDO POSICAO      
                        var CustodiaClienteZerandoPosicao = from p in SaldoCustodiaCliente.ColecaoObjeto
                                                            where p.CodigoInstrumento == pParametros.EnviarOrdemRequest.OrdemInfo.Symbol
                                                            && p.QtdeAtual > 0

                                                            select p;

                        if (CustodiaClienteZerandoPosicao.Count() > 0)
                        {

                            // CLIENTE ZERANDO PARTE DA POSICAO/ POSICAO COMPLETA OPCOES
                            foreach (var Custodia in CustodiaClienteZerandoPosicao)
                            {

                                QuantidadeOpcaoCustodiaCarteira = Custodia.QtdeAtual;
                            }

                            logger.Info("Custodia atual: " + QuantidadeOpcaoCustodiaCarteira.ToString());
                        }

                    #endregion


                        #region [  Valida a quantidade nas carteiras ]
                        //VERIFICA A QUANTIDADE COBERTA EM CUSTODIA DO CLIENTE NA CARTEIRA DE ACOES 21016

                        int QuantidadeAcaoCustodiaCobertaCarteira = 0;


                        // Verifica se o cliente possui a quantidade solicitada para zerar
                        var CustodiaClientePapelBase = from p in SaldoCustodiaCliente.ColecaoObjeto
                                                       where
                                                           // p.QtdeAtual >= QuantidadeSolicitada &&
                                                           p.CodigoInstrumento == PapelBase

                                                       select p;


                        // Cliente possui papel base suficiente para efetuar o lancamento coberto
                        if ((CustodiaClientePapelBase != null) && (CustodiaClientePapelBase.Count() > 0))
                        {
                            logger.Info("Cliente possui papel base suficiente para efetuar o lancamento coberto de opcoes na carteira 21");

                            foreach (var Custodia in CustodiaClientePapelBase)
                            {
                                QuantidadeAcaoCustodiaCobertaCarteira += Custodia.QtdeAtual;
                            }
                        }

                        #endregion

                        #region [ Custodia Bloqueada Venda em aberto]

                        int QuantidadeBloqueada = 0;

                        QuantidadeBloqueada = new PersistenciaCustodia().ListarBloqueioOpcoes(pParametros.EnviarOrdemRequest.OrdemInfo.Account, PapelBase);


                        logger.Info("Bloqueio em custódia carregado com sucesso.");
                        logger.Info("Quantidade Bloqueada: " + QuantidadeBloqueada.ToString());

                        if (QuantidadeBloqueada < 0)
                        {
                            // Existem bloqueios
                            QuantidadeAcaoCustodiaCobertaCarteira = (QuantidadeAcaoCustodiaCobertaCarteira + QuantidadeBloqueada);
                        }

                        #endregion

                        //TODO: VERIFICA SALDO PARA ENVIAR ORDEM.

                        // VERIFICA SE A QUANTIDADE COBERTA DO PAPEL BASE + QUANTIDADE EM OPCAO NA CUSTODIA E SUFICIENTE PARA COBRIR A OPERAÇÃO
                        if ((QuantidadeOpcaoCustodiaCarteira + QuantidadeAcaoCustodiaCobertaCarteira) >= QuantidadeSolicitada)
                        {
                            // PRIMEIRA TENTA ZERAR A TODA A POSICAO DO CLIENTE
                            if ((QuantidadeOpcaoCustodiaCarteira - QuantidadeSolicitada) >= 0)
                            {
                                #region [ Zera posição parcial / total de opções e envia a ordem. ]


                                // Inseri a Ordem solicitada no banco de dados    
                                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                {
                                    //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                                    logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                    //ENVIA ORDEM PARA O ROTEADOR

                                    logger.Info("Envia a ordem para o roteador de ordens");

                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                    {
                                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                    }
                                    else
                                    {
                                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);
                                    }

                                }
                            }

                            else
                            {
                                // Zerando posição total / Efetuando lancamento Coberto de Opções
                                if ((QuantidadeOpcaoCustodiaCarteira - QuantidadeSolicitada) < 0)
                                {

                                    // Inseri a Ordem solicitada no banco de dados    
                                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                    {
                                        //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                        //ENVIA ORDEM PARA O ROTEADOR
                                        logger.Info("Envia a ordem para o roteador de ordens");

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);
                                        }

                                    }
                                    else
                                    {

                                        logger.Info("Ocorreu um erro ao efetuar o bloqueio do cliente");

                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Ocorreu um erro ao efetuar o bloqueio do cliente";
                                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                    }

                                }

                                #endregion

                            }
                        }
                        else
                        {

                            // Cliente não possui custódia suficiente para efetuar o lançamento coberto de opções.

                            //*****************************************************************
                            // 4º VALIDA PERMISSAO PARA VENDER DESCOBERTO
                            //*****************************************************************

                            logger.Info("Cliente não possui custódia suficiente para vender o instrumento");
                            logger.Info("Verifica permissão para vender o papel a descoberto");

                            var PosicaoCustodiaDescoberto = from p in _responseClienteRiscoParam.ColecaoObjeto
                                                            where p.IdParametroPermissao == (int)ParametroPermissaoRiscoEnum.PARAMETRO_LIMITE_DESCOBERTO_OPCOES
                                                            && p.DtValidade >= DateTime.Now
                                                            orderby p.DtMovimento descending

                                                            select p;

                            if (PosicaoCustodiaDescoberto.Count() == 0)
                            {
                                logger.Info("Cliente não possui permissão para vender descoberto.");
                            }

                            // Verifica se o cliente possui permissao / saldo para vender descoberto
                            if (PosicaoCustodiaDescoberto.Count() > 0)
                            {
                                #region Estado do pregão

                                if (pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID != NumeroPortaAssessor)
                                {
                                    int EstadoPregao = new PersistenciaRisco().ObterEstadoPregao();

                                    if (EstadoPregao != 0)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Não é permitido utilizar limite operacional fora do horário regular do pregão.";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return PipeLineResponse;

                                    }
                                }

                                #endregion

                                #region Lista Permitida Alavancagem

                                if (pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID != NumeroPortaAssessor)
                                {
                                    var InstrumentoAlavancagem = from p in ListaInstrumentoAlavancagem.ListaPapeisAlavancagem
                                                                 where p.Ativo == PapelBase
                                                                 select p;

                                    if (InstrumentoAlavancagem.Count() == 0)
                                    {
                                        CriticaInfo = new PipeLineCriticaInfo();
                                        CriticaInfo.Critica = "Este ativo não esta liberado para suportar alavancagem operacional.";
                                        CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                        CriticaInfo.DataHoraCritica = DateTime.Now;

                                        // Adiciona as criticas no pipeline de risco.
                                        CriticaInfoCollection.Add(CriticaInfo);

                                        PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                        PipeLineResponse.DataResposta = DateTime.Now;
                                        PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                        PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                        return PipeLineResponse;

                                    }
                                }
                                #endregion

                                #region Opcoes 30 dias

                                if (pParametros.EnviarOrdemRequest.OrdemInfo.TimeInForce != OrdemValidadeEnum.ValidaParaODia)
                                {
                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Não é possível abrir oferta de opções com validade para 30 dias utilizando Limite Operacional";
                                    CriticaInfo.CriticaTipo = CriticaRiscoEnum.PermissaoInsuficiente;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);

                                    PipeLineResponse.CriticaInfo = CriticaInfoCollection;
                                    PipeLineResponse.DataResposta = DateTime.Now;
                                    PipeLineResponse.DescricaoResposta = "O sistema de risco encontrou <1> item a ser vefificado";
                                    PipeLineResponse.StatusResposta = CriticaRiscoEnum.PermissaoInsuficiente;

                                    return PipeLineResponse;
                                }

                                #endregion

                                int ValorFinanceiroDescoberto = 0;
                                int idClienteParametroPermissao = 0;

                                foreach (var ClienteCustodia in PosicaoCustodiaDescoberto)
                                {
                                    idClienteParametroPermissao = ClienteCustodia.IdClienteParametroPermissao;
                                    ValorFinanceiroDescoberto = (int)ClienteCustodia.ValorParametro;
                                    break;
                                }

                                logger.Info("Permissão para operar descoberto concedida com sucesso. Total disponivel: " + string.Format("{0:c}", ValorFinanceiroDescoberto.ToString()));

                                if (QuantidadeAcaoCustodiaCobertaCarteira < 0)
                                {
                                    QuantidadeAcaoCustodiaCobertaCarteira = 0;
                                }

                                if (QuantidadeOpcaoCustodiaCarteira < 0)
                                {
                                    QuantidadeOpcaoCustodiaCarteira = 0;
                                }

                                decimal QuantidadeDescoberto = (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty - (QuantidadeOpcaoCustodiaCarteira + QuantidadeAcaoCustodiaCobertaCarteira));

                                decimal ValorFinanceiroSolicitadoParaVenda = (pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal() * (QuantidadeDescoberto));

                                logger.Info("Total em custódia Coberta: " + QuantidadeOpcaoCustodiaCarteira.ToString());
                                logger.Info("Quantidade solicitada descoberto: " + QuantidadeDescoberto.ToString());

                                decimal VolumeAjustadoLimite = 0;

                                if (ValorFinanceiroSolicitadoParaVenda <= ValorFinanceiroDescoberto)
                                {
                                    logger.Info("Inicia processo de calculo de validação de oscilação");
                                    decimal UltimaCotacao;
                                    decimal UltimaCotacaoPapelBase;
                                    bool LimiteOscilacaoAtingido = false;
                                    decimal Oscilacao;

                                    logger.Info("Obtem cotação do papel base: " + PapelBase);
                                    EnviarCotacaoResponse ResponseCotacaoBase = this.ObterCotacao(PapelBase);
                                    UltimaCotacaoPapelBase = ResponseCotacaoBase.Objeto.Ultima;
                                    logger.Info("Valor obtido: " + PapelBase + " : " + UltimaCotacaoPapelBase.ToString());

                                    logger.Info("Obtem Cotação da opção: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
                                    EnviarCotacaoResponse ResponseCotacao = this.ObterCotacao(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim());
                                    UltimaCotacao = ResponseCotacao.Objeto.Ultima;
                                    logger.Info("Valor obtido: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim() + " : " + UltimaCotacao.ToString());

                                    decimal PrecoOrdem = pParametros.EnviarOrdemRequest.OrdemInfo.Price.DBToDecimal();

                                    Oscilacao = (((UltimaCotacao / PrecoOrdem) - 1) * 100);

                                    #region [Verificar se a opção esta ITM ,OTM,ATM]

                                    logger.Info("Obtem Strike da Opcao");

                                    int StrikeOpcao = 0;

                                    if (pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length == 7)
                                    {
                                        StrikeOpcao = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Substring(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length - 2, 2));
                                    }
                                    else if (pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length == 6)
                                    {
                                        StrikeOpcao = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Substring(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Length - 1, 1));
                                    }

                                    logger.Info("Verifica se a opção esta ITM,ATM ou OTM.");

                                    if (StrikeOpcao == UltimaCotacaoPapelBase)
                                    {
                                        logger.Info("Opcao ATM");
                                        if (Oscilacao > OscilacaoMaximaATM)
                                        {
                                            VolumeAjustadoLimite = (UltimaCotacao * QuantidadeDescoberto);
                                            LimiteOscilacaoAtingido = true;
                                        }
                                    }
                                    else if (StrikeOpcao < UltimaCotacaoPapelBase)
                                    {
                                        logger.Info("Opcao ITM");
                                        if (Oscilacao > OscilacaoMaximaITM)
                                        {
                                            VolumeAjustadoLimite = (UltimaCotacao * QuantidadeDescoberto);
                                            LimiteOscilacaoAtingido = true;
                                        }
                                    }
                                    else if (StrikeOpcao > UltimaCotacaoPapelBase)
                                    {
                                        logger.Info("Opcao OTM");
                                        if (Oscilacao > OscilacaoMaximaOTM)
                                        {
                                            VolumeAjustadoLimite = (UltimaCotacao * QuantidadeDescoberto);
                                            LimiteOscilacaoAtingido = true;
                                        }
                                    }

                                    #endregion

                                    string Historico = string.Empty;

                                    if (LimiteOscilacaoAtingido == true)
                                    {
                                        logger.Info("O Limte de Oscilação Atingido");

                                        Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                        Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                        Historico += " Quantidade: " + QuantidadeDescoberto.ToString();
                                        Historico += " Preco: " + UltimaCotacao.ToString();
                                    }
                                    else
                                    {
                                        logger.Info("O Limte de Oscilação não foi Atingido");

                                        Historico += "Cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString();
                                        Historico = " Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                                        Historico += " Quantidade: " + QuantidadeDescoberto.ToString();
                                        Historico += " Preco: " + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString();

                                    }

                                    logger.Info("Atualiza os Limites do cliente");

                                    bool BLAtualizaSaldo;

                                    if (LimiteOscilacaoAtingido == true)
                                    {

                                        logger.Info("Saldo do Volume ajustado: " + VolumeAjustadoLimite.ToString());

                                        BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                            idClienteParametroPermissao,
                                            VolumeAjustadoLimite,
                                            Historico,
                                            pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                            'V'
                                        );
                                    }
                                    else
                                    {
                                        BLAtualizaSaldo = new PersistenciaRisco().AtualizaLimiteClienteOMS(
                                           idClienteParametroPermissao,
                                           ValorFinanceiroSolicitadoParaVenda,
                                           Historico,
                                           pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID,
                                           'V'
                                       );
                                    }

                                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                                    {
                                        //TODO: VENDE O PAPEL COM O SERVICO DE ORDENS                                 
                                        logger.Info("Venda de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral efetuada com sucesso.");

                                        //ENVIA ORDEM PARA O ROTEADOR

                                        logger.Info("Envia a ordem para o roteador de ordens");

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                        {
                                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                                            PipeLineResponse.DataResposta = DateTime.Now;
                                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                                        }
                                        else
                                        {
                                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                                            CriticaInfo = new PipeLineCriticaInfo();
                                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                                            CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                                            CriticaInfo.DataHoraCritica = DateTime.Now;

                                            // Adiciona as criticas no pipeline de risco.
                                            CriticaInfoCollection.Add(CriticaInfo);
                                        }

                                    }
                                }
                                else
                                {
                                    logger.Info("Cliente não possui custódia suficiente para efetuar o lançamento coberto de opções.");

                                    CriticaInfo = new PipeLineCriticaInfo();
                                    CriticaInfo.Critica = "Cliente não possui custódia suficiente para efetuar o lançamento coberto de opções.";
                                    CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                                    CriticaInfo.DataHoraCritica = DateTime.Now;

                                    // Adiciona as criticas no pipeline de risco.
                                    CriticaInfoCollection.Add(CriticaInfo);
                                }
                            }
                            else
                            {
                                logger.Info("Cliente não possui custódia suficiente.");

                                CriticaInfo = new PipeLineCriticaInfo();
                                CriticaInfo.Critica = "Cliente não possui custódia suficiente.";
                                CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                                CriticaInfo.DataHoraCritica = DateTime.Now;

                                // Adiciona as criticas no pipeline de risco.
                                CriticaInfoCollection.Add(CriticaInfo);

                            }

                        }

                    }
                    else
                    {
                        logger.Info("Cliente não possui custódia suficiente.");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = "Cliente não possui custódia suficiente.";
                        CriticaInfo.CriticaTipo = OMS.Risco.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }
                }
            }

            catch (Exception ex)
            {
                PipeLineResponse.DataResposta = DateTime.Now;
                PipeLineResponse.StackTrace = ex.StackTrace;
                PipeLineResponse.DescricaoResposta = ex.Message;
                PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Exception;

            }

            PipeLineResponse.CriticaInfo = CriticaInfoCollection;

            return TratamentoRetorno(PipeLineResponse);

        }


        #endregion

        #endregion

        #region Envio de Ordem BMF [ Cliente Normal e Institucional ]

        private ValidacaoRiscoResponse ValidarCompraBMF(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {

            logger.Info("Inicia Validação de Compra no mercado de BMF");
            ValidacaoRiscoResponse ResponstaRisco = new ValidacaoRiscoResponse();

            logger.Info("Dados da Operação de compra ");
            logger.Info("Cliente            :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Instrumento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Preço              :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
            logger.Info("Quantidade         :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Validade           :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());


            if (!SecurityListInfo.htSecurityList.Contains(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim()))
            {
                PipeLineCriticaInfo info = new PipeLineCriticaInfo();

                info.Critica = "Não é possível negociar o instrumento: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                info.DataHoraCritica = DateTime.Now;

                ResponstaRisco.CriticaInfo.Add(info);
                ResponstaRisco.DataResposta = DateTime.Now;
                ResponstaRisco.DescricaoResposta = "Validação de Instrumentos BMF";
                ResponstaRisco.StatusResposta = CriticaRiscoEnum.Exception;
                return ResponstaRisco;
            }

            string[] SecurityList = SecurityListInfo.htSecurityList[pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim()].ToString().Split('|');

            string SecurityID = SecurityList[0].ToString();
            string SecurityIDSource = SecurityList[1].ToString();

            //ObterCodigoBmfCliente

            #region [ Alteração de Ordem ]

            // Verifica se o ID da ordem já existe no banco de dados.
            OrdemInfo _OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
            {
                logger.Info("Alteração de ordem solicitada");
                logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                logger.Info("Inicia rotina de validação de alteração de ordens.");

                return this.AlterarOrdemCompraBMF(pParametros, _OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
            }

            #endregion


            logger.Info("Obtem código do cliente BMF.");

            ClienteAtividadeBmfRequest lRequest = new ClienteAtividadeBmfRequest();

            lRequest.CodigoBase = pParametros.EnviarOrdemRequest.OrdemInfo.Account;

            ClienteAtividadeBmfResponse Resposta = new PersistenciaRisco().ObterCodigoBmfCliente(lRequest);

            pParametros.EnviarOrdemRequest.OrdemInfo.Account = Resposta.CodigoBmf;

            logger.Info("Código BMF gerado com sucesso. " + Resposta.CodigoBmf.ToString());

            logger.Info("Obtem SecurityID do instrumento. " + SecurityID);

            pParametros.EnviarOrdemRequest.OrdemInfo.Exchange = "BMF";
            pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID = 0;
            pParametros.EnviarOrdemRequest.OrdemInfo.SecurityID = SecurityID;
            pParametros.EnviarOrdemRequest.OrdemInfo.SecurityIDSource = SecurityIDSource;

            logger.Info("Envia solicitação para gravar ordem no banco de dados");
            if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
            {
                logger.Info("Ordem inserida no banco de dados com sucesso");

                //pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);


                logger.Info("Encaminha solicitação de compra para o roteamento de ordem");
                ExecutarOrdemResponse ResponseOrdens = EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                //Verifica Retorno do roteador
                if (ResponseOrdens.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                {
                    logger.Info("Ordem enviada para a BMF com sucesso");

                    ResponstaRisco.DataResposta = DateTime.Now;
                    ResponstaRisco.DescricaoResposta = ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    ResponstaRisco.StatusResposta = CriticaRiscoEnum.Sucesso;

                }
                else
                {
                    logger.Info("Ocorreu um erro ao enviar a ordem para a BMF");
                    logger.Info("Descrição do erro:    " + ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia);

                    PipeLineCriticaInfo info = new PipeLineCriticaInfo();

                    info.Critica = ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                    info.DataHoraCritica = DateTime.Now;

                    ResponstaRisco.CriticaInfo.Add(info);
                    ResponstaRisco.DataResposta = DateTime.Now;
                    ResponstaRisco.DescricaoResposta = "Ocorreu um erro ao enviar a ordem de BMF";
                    ResponstaRisco.StatusResposta = CriticaRiscoEnum.Exception;

                }
            }
            else
            {
                logger.Info("Ocorreu um erro ao gravar a ordem no banco de dados.");
            }


            return TratamentoRetorno(ResponstaRisco);

        }

        private ValidacaoRiscoResponse ValidarVendaBMF(ValidacaoRiscoRequest pParametros, ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> _responseClienteRiscoParam, CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis)
        {

            logger.Info("Inicia validação de venda no mercado de BMF");
            logger.Info("Dados da Operação de compra ");
            logger.Info("Cliente            :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Instrumento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Preço              :" + pParametros.EnviarOrdemRequest.OrdemInfo.Price.ToString());
            logger.Info("Quantidade         :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Validade           :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());

            ValidacaoRiscoResponse ResponstaRisco = new ValidacaoRiscoResponse();

            if (!SecurityListInfo.htSecurityList.Contains(pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim()))
            {
                PipeLineCriticaInfo info = new PipeLineCriticaInfo();

                info.Critica = "Não é possível negociar o instrumento: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;
                info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                info.DataHoraCritica = DateTime.Now;

                ResponstaRisco.CriticaInfo.Add(info);
                ResponstaRisco.DataResposta = DateTime.Now;
                ResponstaRisco.DescricaoResposta = "Validação de Instrumentos BMF";
                ResponstaRisco.StatusResposta = CriticaRiscoEnum.Exception;
                return ResponstaRisco;
            }

            string[] SecurityList = SecurityListInfo.htSecurityList[pParametros.EnviarOrdemRequest.OrdemInfo.Symbol.Trim()].ToString().Split('|');

            string SecurityID = SecurityList[0].ToString();
            string SecurityIDSource = SecurityList[1].ToString();


            #region [ Alteração de Ordem ]

            // Verifica se o ID da ordem já existe no banco de dados.
            OrdemInfo _OrdemInfoAlteracao = (OrdemInfo)(new PersistenciaOrdens().SelecionarOrdemCliente(pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID));

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID != null)
            {
                logger.Info("Alteração de ordem solicitada");
                logger.Info("Ordem original: " + pParametros.EnviarOrdemRequest.OrdemInfo.OrigClOrdID.ToString());
                logger.Info("Inicia rotina de validação de alteração de ordens.");

                return this.AlterarOrdemVendaBMF(pParametros, _OrdemInfoAlteracao, _responseClienteRiscoParam, CadastroPapeis);
            }

            #endregion

            logger.Info("Obtem SecurityID do instrumento. " + SecurityID);

            pParametros.EnviarOrdemRequest.OrdemInfo.Exchange = "BMF";
            pParametros.EnviarOrdemRequest.OrdemInfo.ChannelID = 0;
            pParametros.EnviarOrdemRequest.OrdemInfo.SecurityID = SecurityID;
            pParametros.EnviarOrdemRequest.OrdemInfo.SecurityIDSource = SecurityIDSource;

            logger.Info("Obtem código do cliente BMF.");
            ClienteAtividadeBmfRequest lRequest = new ClienteAtividadeBmfRequest();

            lRequest.CodigoBase = pParametros.EnviarOrdemRequest.OrdemInfo.Account;

            ClienteAtividadeBmfResponse Resposta = new PersistenciaRisco().ObterCodigoBmfCliente(lRequest);

            pParametros.EnviarOrdemRequest.OrdemInfo.Account = Resposta.CodigoBmf;

            logger.Info("Código BMF gerado com sucesso. " + Resposta.CodigoBmf.ToString());


            logger.Info("Envia solicitação para gravar ordem no banco de dados");
            if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
            {

                logger.Info("Ordem inserida no banco de dados com sucesso");

                // pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(CodigoCorretora, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                logger.Info("Encaminha solicitação de compra para o roteamento de ordem");
                ExecutarOrdemResponse ResponseOrdens = EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                //Verifica Retorno do roteador
                if (ResponseOrdens.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                {
                    logger.Info("Ordem enviada para a BMF com sucesso");

                    ResponstaRisco.DataResposta = DateTime.Now;
                    ResponstaRisco.DescricaoResposta = ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    ResponstaRisco.StatusResposta = CriticaRiscoEnum.Sucesso;

                }
                else
                {
                    PipeLineCriticaInfo info = new PipeLineCriticaInfo();

                    logger.Info("Ocorreu um erro ao enviar a ordem para a BMF");
                    logger.Info("Descrição do erro:    " + ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia);

                    info.Critica = ResponseOrdens.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                    info.DataHoraCritica = DateTime.Now;

                    ResponstaRisco.CriticaInfo.Add(info);
                    ResponstaRisco.DataResposta = DateTime.Now;
                    ResponstaRisco.DescricaoResposta = "Ocorreu um erro ao enviar a ordem de BMF";
                    ResponstaRisco.StatusResposta = CriticaRiscoEnum.Exception;

                }
            }


            return TratamentoRetorno(ResponstaRisco);

        }

        #endregion

        #region Vencimento Opções
        /// <summary>
        /// Obtem todas as series vencidas no mercado de opcoes
        /// </summary>
        private void ObterVencimentoOpcoes()
        {
            VencimentoInstrumentoResponse response = this.ObterVencimentoInstrumentos(
                new VencimentoInstrumentoRequest()
                );

            VencimentoInstrumentoInfo.htInstrumentos = response.Objeto;

        }

        #endregion

        #region Security List
        /// <summary>
        /// Obtem a lista de securitys da BMF
        /// </summary>
        private void ObterSecurityList()
        {
            SecurityListResponse ResponseSecurity = new ServicoCadastroPapeis().ObterSecurityList();

            if (ResponseSecurity.StatusResposta == OMS.CadastroPapeis.Lib.Enum.CriticaMensagemEnum.OK)
            {
                SecurityListInfo.htSecurityList = ResponseSecurity.HtSecurityList;
            }
        }

        #endregion

        #region Cotacao OMS

        /// <summary>
        /// Método responsável por obter a cotação do instrumento selecionado.
        /// </summary>
        /// <param name="Instrumento">Nome do Instrumento</param>
        /// <returns></returns>
        private EnviarCotacaoResponse ObterCotacao(string Instrumento)
        {
            try
            {
                EnviarCotacaoRequest _request = new EnviarCotacaoRequest();
                _request.CotacaoInfo.Ativo = Instrumento.Trim();

                ServicoCotacaoOMS _ServicoCotacaoOMS = new ServicoCotacaoOMS();
                return _ServicoCotacaoOMS.ObterCotacaoInstrumento(_request);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region Envio de Ordem Roteador

        #region [ Inclusão de Ordem ]
        /// <summary>
        /// Método responsável por enviar a ordem para o roteador Bovespa/BMF
        /// </summary>
        /// <param name="pParametros">Informações da ordem ( NewOrderSingle ) </param>
        /// <returns>Objeto de resposta com o status da execução da ordem </returns>
        private ExecutarOrdemResponse EnviarOrdemRoteador(OrdemInfo OrdemInfo)
        {
            try
            {
                // Instancia servico de ordens
                logger.Info("Invoca servico de roteamento de ordens");
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();

                OrdemInfo.TransactTime = DateTime.Now;

                // Envia a ordem para o reteador e aguarda o retorno
                logger.Info("Envia a ordem para o roteador");
                ExecutarOrdemResponse RespostaOrdem =
                ServicoRoteador.ExecutarOrdem(new RoteadorOrdens.Lib.Mensagens.ExecutarOrdemRequest()
                {
                    info = OrdemInfo
                });

                return RespostaOrdem;

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                logger.Info("Descrição     :" + ex.Message);

                throw (ex);
            }

        }

        #endregion

        #region [ Alteração de Ordem ]
        /// <summary>
        /// Método responsável por alterar uma ordem já existente na Bovespa/BMF
        /// </summary>
        /// <param name="pParametros">Informações da ordem  </param>
        /// <returns>Objeto de resposta com o status da execução da ordem </returns>
        private ExecutarModificacaoOrdensResponse ModificarOrdemRoteador(OrdemInfo OrdemInfo)
        {
            try
            {
                // Instancia servico de ordens
                logger.Info("Invoca servico de roteamento de ordens");
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();

                OrdemInfo.TransactTime = DateTime.Now;

                // Envia a ordem para o reteador e aguarda o retorno
                logger.Info("Alteração de ordem enviada para o roteador");
                ExecutarModificacaoOrdensResponse RespostaOrdem =
                ServicoRoteador.ModificarOrdem(new RoteadorOrdens.Lib.Mensagens.ExecutarModificacaoOrdensRequest()
                {
                    info = OrdemInfo
                });

                return RespostaOrdem;

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a alteração de ordens para o roteador");
                logger.Info("Descrição     :" + ex.Message);

                throw (ex);
            }

        }

        #endregion

        #endregion

        #region Configurações Globais de Risco de um determinado cliente
        /// <summary>
        /// Metodo responsável por carregar todas as informações de permissão/parametro do cliente
        /// </summary>
        /// <param name="pParametros"> Código do cliente </param>
        /// <returns>Conjunto de parametros e permissões do cliente </returns>
        /// 
        private ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> CarregarParametosPermissoesCliente(ClienteParametrosPermissoesRequest pParametros)
        {
            return new PersistenciaRisco().CarregarParametrosPermissoesCliente(pParametros);
        }

        #endregion

    }
}
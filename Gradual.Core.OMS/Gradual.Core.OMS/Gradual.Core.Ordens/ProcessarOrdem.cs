using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Core.Ordens.Lib;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.Core.Ordens.Persistencia;
using Gradual.Core.Ordens.Lib.Dados.Enum;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.RoteadorOrdens.Lib;
using System.Collections;
using Gradual.Core.Ordens.Lib.Mensageria;
using Gradual.Core.OMS.LimiteBMF.Lib;
using Gradual.Core.OMS.LimiteBMF;
using System.Text.RegularExpressions;
using System.Configuration;



namespace Gradual.Core.Ordens
{
    public class ProcessarOrdem
    {
        #region Declaracoes

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<int> lstRepassador = new List<int>();

        #endregion

        #region Constantes

        private const string CORRETORA = "227";
        private const string EXCHANGEBOVESPA = "BOVESPA";
        private const string EXCHANGEBMF = "BMF";
        private const int LoteCem = 100;
        private const string HomeBroker = "HB";     

        #endregion

        #region Propriedades

        public bool isContaBroker { set; get; }
        public int CodigoContaMae { set; get; }        

        #endregion

        IRoteadorOrdens ServicoRoteador;
        private bool bRemoveDigito = true;

        public ProcessarOrdem()
        {

            if (ConfigurationManager.AppSettings["AccountStripDigit"] != null)
            {
                bRemoveDigito = ConfigurationManager.AppSettings["AccountStripDigit"].ToString().ToLower().Equals("true");

                logger.WarnFormat("REMOCAO DO DIGITO BOVESPA ESTA {0}", bRemoveDigito ? "HABILITADO" : "DESABILITADO");
            }
            
           // ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
            this.ObterSessoesRepassador();
            //log4net.Config.XmlConfigurator.Configure();
        }



        private void ObterSessoesRepassador()
        {            
            //TODO: CAMADA DE BANCO DE DADOS
            lstRepassador.Add(317);

            //ATP 2015-01-15
            if (ConfigurationManager.AppSettings["ListaPortasRepasse"] != null)
            {
                char[] separators = { ',', ';' };

                string[] portasRepasse = ConfigurationManager.AppSettings["ListaPortasRepasse"].ToString().Split(separators);

                foreach (string portaRepasse in portasRepasse)
                {
                    int numPorta = Convert.ToInt32(portaRepasse);
                    if (!lstRepassador.Contains(numPorta))
                    {
                        lstRepassador.Add(numPorta);
                    }
                }
            }

            foreach (int numPorta in lstRepassador)
            {
                logger.Info("Porta de repasse: [" + numPorta + "]");
            }
       
        }

        private bool ClienteOperandoAlavancado(EnviarOrdemRequest pParametroOrdemResquest)
        {
            try{

                logger.Info("INICIA ROTINA DE VALIDACAO DE POSICAO E CUSTODIA DO CLIENTE");

                string CodigoInstrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;

                int    CodigoCliente           = pParametroOrdemResquest.ClienteOrdemInfo.Account;                
                int    QuantidadeOrdem         = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;
                decimal PrecoOrdem             = pParametroOrdemResquest.ClienteOrdemInfo.Price.DBToDecimal();
                decimal VolumeOrdem            = (QuantidadeOrdem * PrecoOrdem);

               
                if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Venda)
                {
                    logger.Info("CONSULTA SALDO PROJETADO EM CUSTODIA.");
                    decimal TotalCustodiaProjetada = new PersistenciaOrdens().ObterSaldoProjetadoCustodia(CodigoCliente, CodigoInstrumento);
                    logger.Info("SALDO EM CUSTODIA ENCONTRADO: " + TotalCustodiaProjetada.ToString());


                    logger.Info("TOTAL SOLICITADO PARA VENDA: " + QuantidadeOrdem.ToString());
                    logger.Info("TOTAL DISPONIVEL PARA VENDA: " + TotalCustodiaProjetada.ToString());
                    // Validacao de custodia
                    if (TotalCustodiaProjetada <= QuantidadeOrdem){
                        logger.Info("SALDO INDISPONIVEL");
                        return false;
                    }
                }

                if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Compra)
                {

                    logger.Info("CONSULTA SALDO PROJETADO EM CC.");
                    decimal TotalSaldoCCProjetado = new PersistenciaOrdens().ObterSaldoProjetadoContaCorrente(CodigoCliente);
                    logger.Info("SALDO EM CC ENCONTRADO: " + TotalSaldoCCProjetado.ToString());


                    logger.Info("TOTAL SOLICITADO PARA COMPRA: " + VolumeOrdem.ToString());
                    logger.Info("TOTAL PROJETADO(R$) PARA COMPRA: " + TotalSaldoCCProjetado.ToString());
                    // Validacao de custodia

                    if (TotalSaldoCCProjetado <= VolumeOrdem)
                    {
                        logger.Info("SALDO INDISPONIVEL");
                        return false;
                    }
                }

                return true;
                

            }
            catch (Exception ex){
                logger.Info("OCORREU UM ERRO AO VERIFICAR SE O CLIENTE ESTA OPERANDO ALAVANCADO. DESCRICAO DO ERRO: " + ex.Message);
            }

            return false;            
        }

        private bool ValidaExercicioOpcoes(string symbol)
        {
            DateTime dtVencimentoOpcao = new PersistenciaOrdens().CarregarVencimentoOpcoes(symbol);

            #region [ Vencimento de opções ]

            logger.Info("Verifica se o cliente esta tentando abrir posição em dia de vncimento de opção ou negociando uma opção já vencida.");


            //Vencimento de opção
            if (dtVencimentoOpcao.ToString("{dd/MM/yyyy}") == DateTime.Now.ToString("{dd/MM/yyyy}"))
            {
                logger.Info("Não é possível abrir posição em dia de vencimento de opções");

                return false;
            } 
            else
            {
                logger.Info("Verifica se a série de opção esta ativa");
                // Serie Vencida.
                if (dtVencimentoOpcao < DateTime.Now)
                {
                    logger.Info("A serie de opção referente ao instrumento " + symbol + " já esta vencida");

                    return false;
                }
                else
                {
                    logger.Info("Série de opção valida para operação");
                }
            }



            #endregion

            return true;
        }

        /// <summary>
        /// Metodo responsável por efetuar o controle de risco e tratamento da solicitação de envio de ordens.
        /// </summary>
        /// <param name="pParametroOrdemResquest">Informações da Ordem</param>
        /// <returns>EnviarOrdemResponse</returns>
        public EnviarOrdemResponse EnviarOrdem(EnviarOrdemRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();

            #region [ Trace da ordem ]

            logger.Info("INICIA O PARSEAMENTO DA OFERTA");
            pParametroOrdemResquest = this.PARSEARORDEM(pParametroOrdemResquest);

            logger.Info("TRACE DA SOLICITACAO DE ENVIO DE ORDENS::..........: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
            logger.Info("*******************************************************************");
            logger.Info("DATAHORASOLICITACAO:...........................:" + pParametroOrdemResquest.ClienteOrdemInfo.RegisterTime.ToString());
            logger.Info("CODIGOCLIENTE:.................................:" + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
            logger.Info("NUMEROCONTROLEORDEM:...........................:" + pParametroOrdemResquest.ClienteOrdemInfo.ClOrdID.ToString());
            logger.Info("INSTRUMENTO:...................................:" + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
            logger.Info("DATAVALIDADE:..................................:" + pParametroOrdemResquest.ClienteOrdemInfo.ExpireDate.ToString());
            logger.Info("PRECO:.........................................:" + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());
            logger.Info("STOPSTART: ....................................:" + pParametroOrdemResquest.ClienteOrdemInfo.StopPrice.ToString());
            logger.Info("CHANNELID: ....................................:" + pParametroOrdemResquest.ClienteOrdemInfo.ChannelID.ToString());


            if (pParametroOrdemResquest.ClienteOrdemInfo.TimeInForce == Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.BoaParaLeilao)
            {
                logger.Info("VALIDADE: ....................................: VALIDADE BOA PARA LEILAO");
            }

            if (pParametroOrdemResquest.ClienteOrdemInfo.TimeInForce == Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia)
            {
                logger.Info("VALIDADE: ....................................: VALIDADE VALIDA PARA O DIA");
            }

            if (pParametroOrdemResquest.ClienteOrdemInfo.TimeInForce == Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidoAteDeterminadaData)
            {
                logger.Info("VALIDADE: ....................................: VALIDA ATE DETERMINADA DATA");
            }

            if (pParametroOrdemResquest.ClienteOrdemInfo.TimeInForce == Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada)
            {
                logger.Info("VALIDADE: ....................................: VALIDA ATE SER CANCELADA");
            }

            #endregion

            #region [ Ordem tipo StopStart ]

            if ((pParametroOrdemResquest.ClienteOrdemInfo.OrdType == OrdemTipoEnum.StopLimitada) || (pParametroOrdemResquest.ClienteOrdemInfo.OrdType == OrdemTipoEnum.StopStart))
            {
                if (pParametroOrdemResquest.ClienteOrdemInfo.StopPrice <= 0)
                {
                    string mensagem = "STOP/START PRICE MUST BE > 0 ";
                    logger.Info(mensagem);
                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, mensagem, CriticaRiscoEnum.ErroNegocio);
                    return ObjetoOrdem;
                }

            }

            #endregion

            #region [ Validacao do estado do pregão para ordens alavancadas no HomeBroker ]

            if (pParametroOrdemResquest.ClienteOrdemInfo.CompIDOMS.Trim() == HomeBroker)
            {
                logger.Info("VERIFICA SE O HORARIO LIMITE PARA UTILIZAR ALAVANCAGEM NO INTRADAY FOI ATINGIDO");
                bool EstadoPregaoAtivo = new PersistenciaOrdens().ObterEstadoPregaoBovespa();

      


                if (EstadoPregaoAtivo == false)
                {

                    #region cadastro de papeis

                    logger.Info("OBTEM O SEGMENTO DE MERCADO DO PAPEL");

                    EnviarCadastroPapelRequest request = new EnviarCadastroPapelRequest();
                    request.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                    EnviarCadastroPapelResponse response = ObjetoPersistenciaOrdens.ObterCadastroPapel(request);

                    #endregion

                    if (response.CadastroPapelInfo.SegmentoMercado != SegmentoMercadoEnum.FUTURO)
                    {
                        //VALIDA SE O CLIENTE ESTA OPERANDO ALAVANCADO
                        if (this.ClienteOperandoAlavancado(pParametroOrdemResquest) == false)
                        {
                            string mensagem = "Não é permitido utilizar alavancagem financeira no final do pregão.";
                            logger.Info(mensagem);
                            ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, mensagem, CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }
                    }
                
                }

            }


            #endregion

            #region [Obter a porta para os clientes que estao cadastrados como excecao]

            logger.Info("Obtem a Porta de roteamento para o cliente caso ele esteja cadastrado como excecao");

            int Porta = new PersistenciaOrdens().ObterPortaRoteamentoClienteExcecao(pParametroOrdemResquest.ClienteOrdemInfo.Account);

            if (Porta > 0)
            {
                logger.Info("CLIENTE ENCONTRADO:...... " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                logger.Info("CHANNELID:............... " + Porta.ToString());
                pParametroOrdemResquest.ClienteOrdemInfo.ChannelID = Porta;
            }
            else
            {
                logger.Info("Cliente não cadastrado como <ClienteExcecao>");
            }

            #endregion

            #region [VALIDADE]

            if (pParametroOrdemResquest.ClienteOrdemInfo.ExpireDate < DateTime.Now)
            {
                string mensagem = "A DATA DE VALIDADE NÃO PODE SER MENOR QUE A DATA ATUAL.";
                logger.Info(mensagem);
                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, mensagem, CriticaRiscoEnum.ErroNegocio);
                return ObjetoOrdem;
            }

            #endregion

            #region VALIDACAO DE REPASSADOR DE ORDENS

            logger.Info("Verifica sessão de repasse para [" + pParametroOrdemResquest.ClienteOrdemInfo.ChannelID + "]");

            if (lstRepassador.Contains(pParametroOrdemResquest.ClienteOrdemInfo.ChannelID))
            {

                logger.Info("OBTEM A SIGLA DO REPASSADOR DE ORDENS");

                string SiglaRepassador = new PersistenciaOrdens().ObterSiglaRepassador(pParametroOrdemResquest.ClienteOrdemInfo.Account);

                if (SiglaRepassador == string.Empty)
                {
                    logger.Info("NÃO EXISTE SENDER LOCATION (SIGLA) CADASTRADA PARA O ASSESSOR.");
                }
                else
                {
                    logger.Info("SENDER LOCATION OBTIDO COM SUCESSO. CODIGO DA SIGLA: " + SiglaRepassador.ToString());
                    pParametroOrdemResquest.ClienteOrdemInfo.ExecBroker = SiglaRepassador;
                }
               
            }

            #endregion

            if (pParametroOrdemResquest.ClienteOrdemInfo.Price == 0)
            {
                logger.Info("ORDEM A MERCADO :.........................................: A MERCADO");
                pParametroOrdemResquest.ClienteOrdemInfo.OrdType = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemTipoEnum.Mercado;
            }

            if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Compra)
            {
                logger.Info("SENTIDO:.......................................: COMPRA");
            }
            else
            {
                logger.Info("SENTIDO:.......................................: VENDA");
            }

            logger.Info("QUANTIDADE:....................................:" + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
            logger.Info("QUANTIDADEMINIMA:..............................:" + pParametroOrdemResquest.ClienteOrdemInfo.MinQty.ToString());
            logger.Info("QUANTIDADEAPARENTE:............................:" + pParametroOrdemResquest.ClienteOrdemInfo.MaxFloor.ToString());
            logger.Info("VALIDADEORDEM:.................................:" + pParametroOrdemResquest.ClienteOrdemInfo.ExpireDate.ToString());
            logger.Info("*******************************************************************");

            logger.Info("******* INICIA O PROCESSAMENTO ********");

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            #region Validacao de Cliente Conta Master

            logger.Info("VERIFICA SE O CLIENTE POSSUI CONTA MASTER ATRELADA AO CODIGO DE BOLSA");
            Dictionary<int, int> RelacaoContaMaster = new PersistenciaOrdens().ObterDadosContaMaster(CodigoCliente);

            if (RelacaoContaMaster.Count > 0)
            {
                isContaBroker = true;                
                CodigoContaMae = RelacaoContaMaster[CodigoCliente];
                EnviarOrdemRiscoRequest.IsContaMaster = true;
                EnviarOrdemRiscoRequest.ContaMaster = CodigoContaMae;

                logger.Info("CLIENTE POSSUI CONTA MASTER VINCULADA AO CODIGO DE BOLSA. CODIGO DA CONTA MASTER: " + CodigoContaMae.ToString());
            }
            else
            {
                EnviarOrdemRiscoRequest.IsContaMaster = false;
                logger.Info("CLIENTE NÃO POSSUI CONTA MASTER VINCULADA AO CODIGO DE BOLSA.");
            }


            #endregion

            try
            {
                /*FIRST STEP, THE SYSTEM WILL CHECKS OUT GENERAL PERMISSIONS AND INSTRUMENT'S RULES */
                /*PRIMEIRO PASSO, VERIFICARA AS REGRAS GERAIS DE PERMISSAO E BLOQUEIO DE ATIVOS DO SISTEMA*/


                #region PARAMETROS GLOBAIS DE RISCO

                logger.Info("PARAMETROS E PERMISSOES CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                logger.Info("CARREGA OS PARAMETROS GLOBAIS DE RISCO DO CLIENTE.");

                ParametrosPermissoesClienteRequest ParametrosPermissoesClienteRequest = new ParametrosPermissoesClienteRequest();

                if (isContaBroker){
                    ParametrosPermissoesClienteRequest.CodigoCliente = CodigoContaMae;
                }
                else{
                    ParametrosPermissoesClienteRequest.CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }

                ParametrosPermissoesClienteResponse ParametrosGlobalRisco = ObjetoPersistenciaOrdens.ObterParametrosGlobalRisco(ParametrosPermissoesClienteRequest);

                EnviarOrdemRiscoRequest.PermissoesGlobal = ContextoOMS.ObterConfiguracoesRisco(ParametrosGlobalRisco, AcaoValidacaoRiscoEnum.PERMISSAO);
                EnviarOrdemRiscoRequest.ParametrosGlobal = ContextoOMS.ObterConfiguracoesRisco(ParametrosGlobalRisco, AcaoValidacaoRiscoEnum.PARAMETRO);
                EnviarOrdemRiscoRequest.ClienteOrdemInfo = pParametroOrdemResquest.ClienteOrdemInfo;

                
                #region Bloqueio de Envio de ordens

                var pGlobal = from p in EnviarOrdemRiscoRequest.PermissoesGlobal.lstParametrosPermissoesClienteInfo
                              where p.Permissao == RiscoPermissoesEnum.BloquearEnvioOrdemOMS
                              select p;

                if (pGlobal.Count() > 0)
                {
                    logger.Info("CLIENTE SEM PERMISSAO PARA ENVIAR ORDENS NO OMS. ENTRAR EM CONTATO COM O ATENDIMENTO: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE SEM PERMISSAO PARA ENVIAR ORDENS NO OMS. ENTRAR EM CONTATO COM O ATENDIMENTO.", CriticaRiscoEnum.Permissao);
                    return ObjetoOrdem;
                }

                #endregion

                logger.Info("PARAMETROS E PERMISSOES CARREGADOS COM SUCESSO.");

                #endregion

                #region CADASTRO DE PAPEIS

                logger.Info("CADASTRO DE PAPEIS CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                logger.Info("INICIA VALIDACAO DO INSTRUMENTO NO CADASTRO DE PAPEIS");


                int QUANTIDADEINTEGRAL = 0;
                int QUANTIDADEFRACIONARIO = 0;

                //Obtem informacoes referentes ao cadastro de papel do instrumento selecionado
                EnviarCadastroPapelRequest ObjetoCadastroPapelRequest = new EnviarCadastroPapelRequest();

                string SYMBOL = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;

                ObjetoCadastroPapelRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                EnviarOrdemRiscoRequest.CadastroPapel = ObjetoPersistenciaOrdens.ObterCadastroPapel(ObjetoCadastroPapelRequest);

                int QUANTIDADEORDEM = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;
                int LOTEPADRAO = EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                int MODULOLOTENEGOCIACAO = (QUANTIDADEORDEM % LOTEPADRAO);

                if (EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado != SegmentoMercadoEnum.FUTURO)
                {

                    if (QUANTIDADEORDEM >= LOTEPADRAO)
                    {
                        EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.INTEGRAL;
                       // EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado = SegmentoMercadoEnum.INTEGRALFRACIONARIO;
                        QUANTIDADEINTEGRAL = (QUANTIDADEORDEM - MODULOLOTENEGOCIACAO);

                        if (MODULOLOTENEGOCIACAO > 0)
                        {
                            EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.INTEGRALFRACIONARIO;
                         //   EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado = SegmentoMercadoEnum.INTEGRALFRACIONARIO;
                            QUANTIDADEFRACIONARIO = MODULOLOTENEGOCIACAO;
                        }
                    }
                    else
                    {
                        EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.FRACIONARIO;
                       // EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado = SegmentoMercadoEnum.FRACIONARIO;
                        QUANTIDADEFRACIONARIO = QUANTIDADEORDEM;
                        QUANTIDADEINTEGRAL = 0;
                    }

                    logger.Info("CADASTRO DE PAPEIS CARREGADOS COM SUCESSO");

                    if (SYMBOL.Substring(SYMBOL.Length - 1, 1) == "F")
                    {
                        pParametroOrdemResquest.ClienteOrdemInfo.Symbol = SYMBOL.Remove(SYMBOL.Length - 1);
                    }
                }

                #endregion

                

                #region EXPOSICAO PATRIMONIAL E PERMISSOES POR SEGMENTO DE MERCADO.

                /*
                  ********************************************************************************************************
                  * TRECHO RESPONSAVEL POR VALIDAR AS PERMISSOES PARA OPERAR NO MERCADO DO REESPECTIVO ATIVO SOLICITADO.
                  *******************************************************************************************************/

                ParametrosPermissoesClienteResponse PermissoesGlobal = EnviarOrdemRiscoRequest.PermissoesGlobal;

                
                #region REGRA DE EXPOSICAO PATRIMONIAL

                logger.Info("VALIDANDO REGRA DE EXPOSICAO PATRIMONIAL MAXIMA");

                var Exposicao = from p in EnviarOrdemRiscoRequest.PermissoesGlobal.lstParametrosPermissoesClienteInfo
                                where p.Permissao == RiscoPermissoesEnum.ValidarExposicaoPatrimonial
                                select p;

                if (Exposicao.Count() > 0)
                {
                    logger.Info("CLIENTE POSSUI RESTRICAO DE EXPOSICAO MAXIMA");
                    logger.Info("OBTENDO CONFIGURACAO GLOBAL DE EXPOSICAO");

                    CalcularExposicaoMaximaRequest request = new CalcularExposicaoMaximaRequest();
                    request.IdCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                    request.dtMovimento = DateTime.Now;

                    CalcularExposicaoMaximaResponse response = new PersistenciaOrdens().ObterExposicaoRiscoCliente(request);

                    if (response.PosicaoEncontrada == true)
                    {
                        logger.Info("POSICAO ENCONTRADA");

                        decimal OscilacaoMaxima = 0;
                        decimal PrejuizoMaximo = 0;

                        decimal PatrimonioLiquido = 0;
                        decimal LucroPrejuizo = 0;

                        logger.Info("OBTER PARAMETROS DE OSCILACAO E PREJUIZO MAXIMO");
                        ParametroExposicaoResponse CalcularExposicaoMaximaResponse = new PersistenciaOrdens().ObterParametrosGlobaisExposicaoRisco(new ParametroExposicaoRequest());

                        logger.Info("PARAMETOS DE PREJUIZO E OSCILACAO MAXIMO CARREGADOS COM SUCESSO.");

                        logger.Info("PATRIMONIO LIQUIDO.............:" + response.ClienteExposicaoInfo.PatrimonioLiquido.ToString());
                        logger.Info("LUCRO PREJUISO.................:" + response.ClienteExposicaoInfo.LucroPrejuizo.ToString());
                        logger.Info("DATA ATUALIZACAO...............:" + response.ClienteExposicaoInfo.DataAtualizacao.ToString());

                        OscilacaoMaxima = CalcularExposicaoMaximaResponse.ParametroExposicaoInfo.OscilacaoMaxima;
                        OscilacaoMaxima = (OscilacaoMaxima / 100);
                        PrejuizoMaximo = CalcularExposicaoMaximaResponse.ParametroExposicaoInfo.PrejuizoMaximo;

                        PatrimonioLiquido = response.ClienteExposicaoInfo.PatrimonioLiquido;
                        LucroPrejuizo = response.ClienteExposicaoInfo.LucroPrejuizo;

                        if (LucroPrejuizo != 0)
                        {

                            decimal PercPerdaPatrimonial = ((LucroPrejuizo / PatrimonioLiquido) * 100);

                            if ((OscilacaoMaxima > 0) && (PrejuizoMaximo != 0))
                            {
                                if (PercPerdaPatrimonial >= OscilacaoMaxima)
                                {
                                    string mensagem = "PREJUIZO PATRIMONIAL MAXIMO ATINGIDO. [" + PercPerdaPatrimonial.ToString() + "] DO PATRIMONIO LIQUIDO TOTAL ATINGIDO. ENTRAR EM CONTATO COM O DEPARTAMENTO DE RISCO.";
                                    logger.Info(mensagem);
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                                if (LucroPrejuizo > PrejuizoMaximo)
                                {
                                    string mensagem = "PREJUIZO PATRIMONIAL MAXIMO ATINGIDO. [" + LucroPrejuizo.ToString() + "] DE PREJUIZO. ENTRAR EM CONTATO COM O DEPARTAMENTO DE RISCO.";
                                    logger.Info(mensagem);
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                            }

                            if ((OscilacaoMaxima > 0) && (PrejuizoMaximo != 0))
                            {
                                if (PercPerdaPatrimonial >= OscilacaoMaxima)
                                {
                                    string mensagem = "PREJUIZO PATRIMONIAL MAXIMO ATINGIDO. [" + PercPerdaPatrimonial.ToString() + "] DO PATRIMONIO LIQUIDO TOTAL ATINGIDO. ENTRAR EM CONTATO COM O DEPARTAMENTO DE RISCO.";
                                    logger.Info(mensagem);
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }
                            }

                            if ((OscilacaoMaxima == 0) && (PrejuizoMaximo != 0))
                            {
                                if (LucroPrejuizo > PrejuizoMaximo)
                                {
                                    string mensagem = "PREJUIZO PATRIMONIAL MAXIMO ATINGIDO. [" + LucroPrejuizo.ToString() + "] DE PREJUIZO. ENTRAR EM CONTATO COM O DEPARTAMENTO DE RISCO.";
                                    logger.Info(mensagem);
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }
                            }
                        }

                        logger.Info("CLIENTE ENCONTRA-SE FORA DA ZONA DE RISCO");
                        logger.Info("CONTINUAR VALIDACAO....");
                    }
                    else
                    {
                        logger.Info("CLIENTE AINDA NÃO POSSUI ORDENS ENVIADAS PARA O DIA");
                        logger.Info("CONTINUAR VALIDACAO....");
                    }
                }

                #endregion
             

                logger.Info("VALIDACAO DE CONTROLE DE PERMISSOES POR MERCADO CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                switch (EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado)
                {
                    case SegmentoMercadoEnum.AVISTA:

                     

                        var pAvista = from p in EnviarOrdemRiscoRequest.PermissoesGlobal.lstParametrosPermissoesClienteInfo
                                      where p.Permissao == RiscoPermissoesEnum.OperarMercadoAVista
                                      select p;

                        if (pAvista.Count() == 0)
                        {
                            logger.Info("CLIENTE SEM PERMISSAO PARA OPERAR NO MERCADO A VISTA. CODIGO DO CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "Usuario sem permissão para operar no mercado a vista.", CriticaRiscoEnum.Permissao);
                            return ObjetoOrdem;
                        }

                        break;
                    case SegmentoMercadoEnum.OPCAO:

                        var pOpcoes = from p in EnviarOrdemRiscoRequest.PermissoesGlobal.lstParametrosPermissoesClienteInfo
                                      where p.Permissao == RiscoPermissoesEnum.OperarMercadoOpoes
                                      select p;

                        if (pOpcoes.Count() == 0)
                        {
                            logger.Info("USUARIO SEM PERMISSAO PARA OPERAR NO MERCADO DE OPCOES. CODIGO DO CLIENTE " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "Usuario sem permissão para operar no mercado de opções", CriticaRiscoEnum.Permissao);
                            return ObjetoOrdem;
                        }

                        break;

                    case SegmentoMercadoEnum.FUTURO:

                        var pFuturos = from p in EnviarOrdemRiscoRequest.PermissoesGlobal.lstParametrosPermissoesClienteInfo
                                       where p.Permissao == RiscoPermissoesEnum.OperarMercadoFuturo
                                       select p;

                        if (pFuturos.Count() == 0)
                        {
                            logger.Info("USUARIO SEM PERMISSAO PARA OPERAR NO MERCADO DE FUTUROS. CODIGO DO CLIENTE " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "Usuario sem permissão para operar no mercado de BMF", CriticaRiscoEnum.Permissao);
                            return ObjetoOrdem;
                        }

                        break;
                }

                logger.Info("CONTROLE DE PERMISSOES CARREGADOS COM SUCESSO. CLIENTE APTO A OPERAR NO MERCADO SELECIONADO.");


                #endregion

                #region ESTADO DOS INSTRUMENTOS

                #region PERMISSAO GLOBAL DO INSTRUMENTO
                
                logger.Info("VALIDACAO DE CONTROLE DE ESTADO DO INSTRUMENTO GLOBAL : " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());

                InstrumentoBloqueadoRequest InstrumentoBloqueadoGlobalRequest = new InstrumentoBloqueadoRequest();
                InstrumentoBloqueadoGlobalRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;

                InstrumentoBloqueadoResponse InstrumentoBloqueadoGlobalResponse = ObjetoPersistenciaOrdens.CarregarInstrumentosBloqueadosGlobal(InstrumentoBloqueadoGlobalRequest);

                List<string> listaInstrumentosCliente = new List<string>();


                var GBloqueios = from p in InstrumentoBloqueadoGlobalResponse.ListaInstrumentoBloqueio
                                 where p.Instrumento == pParametroOrdemResquest.ClienteOrdemInfo.Symbol
                                 select p;

                foreach (var item in GBloqueios)
                {
                    if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Compra)
                    {
                        if ((item.Sentido == SentidoBloqueioEnum.Compra) || (item.Sentido == SentidoBloqueioEnum.Ambos))
                        {
                            listaInstrumentosCliente.Add(item.Instrumento);
                        }
                    }
                    else
                    {
                        if ((item.Sentido == SentidoBloqueioEnum.Venda) || (item.Sentido == SentidoBloqueioEnum.Ambos))
                        {
                            listaInstrumentosCliente.Add(item.Instrumento);
                        }
                    }
                }

                if (listaInstrumentosCliente.Contains(pParametroOrdemResquest.ClienteOrdemInfo.Symbol))
                {
                    logger.Info("O INSTRUMENTO " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol + " ESTA BLOQUEADO.");

                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "INSTRUMENTO BLOQUEADO PELO RISCO. CLIENTE NÃO POSSUI PERMISSÃO PARA OPERAR ESTE INSTRUMENTO", CriticaRiscoEnum.ErroNegocio);
                    return ObjetoOrdem;
                }

                #endregion

                #region PERMISSAO INSTRUMENTO X GRUPO

                logger.Info("VALIDACAO DE CONTROLE DE ESTADO DE GRUPO DE INSTRUMENTO POR CLIENTE : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                InstrumentoBloqueadoRequest GrupoInstrumentoBloqueadoRequest = new InstrumentoBloqueadoRequest();

                if (isContaBroker){                    
                    GrupoInstrumentoBloqueadoRequest.IdCliente = CodigoContaMae;
                }
                else{
                    GrupoInstrumentoBloqueadoRequest.IdCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }

                
                GrupoInstrumentoBloqueadoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;

                InstrumentoBloqueadoResponse InstrumentoBloqueadoResponse = ObjetoPersistenciaOrdens.CarregarInstrumentosBloqueadosGrupoCliente(GrupoInstrumentoBloqueadoRequest);

                var Bloqueios = from p in InstrumentoBloqueadoResponse.ListaInstrumentoBloqueio
                                where p.Instrumento == pParametroOrdemResquest.ClienteOrdemInfo.Symbol
                                select p;

                foreach (var item in Bloqueios)
                {
                    if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Compra)
                    {
                        if ((item.Sentido == SentidoBloqueioEnum.Compra) || (item.Sentido == SentidoBloqueioEnum.Ambos))
                        {
                            listaInstrumentosCliente.Add(item.Instrumento);
                        }
                    }
                    else
                    {
                        if ((item.Sentido == SentidoBloqueioEnum.Venda) || (item.Sentido == SentidoBloqueioEnum.Ambos))
                        {
                            listaInstrumentosCliente.Add(item.Instrumento);
                        }
                    }
                }


                if (listaInstrumentosCliente.Contains(pParametroOrdemResquest.ClienteOrdemInfo.Symbol))
                {
                    logger.Info("O INSTRUMENTO " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol + " ESTA BLOQUEADO PARA O CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NÃO POSSUI PERMISSÃO PARA OPERAR ESTE INSTRUMENTO", CriticaRiscoEnum.ErroNegocio);
                    logger.Info("GRUPO DE ATIVO BLOQUEADO.");
                    return ObjetoOrdem;
                }


                #endregion

                #region PERMISSAO INSTRUMENTO CLIENTE

                logger.Info("VALIDACAO DE CONTROLE DE ESTADO DO INSTRUMENTO CLIENTE : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                InstrumentoBloqueadoRequest InstrumentoBloqueadoRequest = new InstrumentoBloqueadoRequest();
            
                if (isContaBroker){                    
                    InstrumentoBloqueadoRequest.IdCliente = CodigoContaMae;
                }
                else{
                    InstrumentoBloqueadoRequest.IdCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }

                InstrumentoBloqueadoResponse GrupoInstrumentoBloqueadoResponse = ObjetoPersistenciaOrdens.CarregarInstrumentosBloqueadosCliente(GrupoInstrumentoBloqueadoRequest);

                List<string> listaGrupoInstrumentosCliente = new List<string>();


                var EBloqueios = from p in GrupoInstrumentoBloqueadoResponse.ListaInstrumentoBloqueio
                                 where p.Instrumento == pParametroOrdemResquest.ClienteOrdemInfo.Symbol
                                 select p;

                foreach (var item in EBloqueios)
                {
                    if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Compra)
                    {
                        if ((item.Sentido == SentidoBloqueioEnum.Compra) || (item.Sentido == SentidoBloqueioEnum.Ambos))
                        {
                            listaInstrumentosCliente.Add(item.Instrumento);
                        }
                    }
                    else
                    {
                        if ((item.Sentido == SentidoBloqueioEnum.Venda) || (item.Sentido == SentidoBloqueioEnum.Ambos))
                        {
                            listaInstrumentosCliente.Add(item.Instrumento);
                        }
                    }
                }

                if (listaInstrumentosCliente.Contains(pParametroOrdemResquest.ClienteOrdemInfo.Symbol))
                {
                    logger.Info("O INSTRUMENTO " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol + " ESTA BLOQUEADO PARA O CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NÃO POSSUI PERMISSÃO PARA OPERAR ESTE INSTRUMENTO", CriticaRiscoEnum.ErroNegocio);
                    return ObjetoOrdem;
                }


                #endregion

                #endregion
               

                
                #region FATFINGER

                EnviarClienteFatFingerRequest EnviarClienteFatFingerRequest = new EnviarClienteFatFingerRequest();

                if (isContaBroker){
                    EnviarClienteFatFingerRequest.CodigoCliente = CodigoContaMae;
                }
                else{
                    EnviarClienteFatFingerRequest.CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }
               

                EnviarClienteFatFingerResponse EnviarClienteFatFingerResponse = new EnviarClienteFatFingerResponse();
                EnviarClienteFatFingerResponse = new PersistenciaOrdens().ObterRegrasFatFingerCliente(EnviarClienteFatFingerRequest);

                if (EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado == SegmentoMercadoEnum.FUTURO)
                {
                    var LimiteBoletaBovespa = from p in EnviarClienteFatFingerResponse.lsConfiguracaoFatFinger
                                              where p.Mercado == "BMF"
                                              select p;

                    if (LimiteBoletaBovespa.Count() > 0)
                    {
                        decimal VALORMAXIMO, QUANTIDADE, PRECO, VOLUME = 0;

                        foreach (var item in LimiteBoletaBovespa)
                        {
                            VALORMAXIMO = item.valorRegra;
                            QUANTIDADE = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                            #region COTACAO

                            logger.Info("INICIALIZA O SERVICO DE COTACOES");

                            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                            CotacaoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                            PRECO = CotacaoResponse.CotacaoInfo.Ultima;

                            logger.Info("SERVICO DE COTACOES FINALIZADO COM SUCESSO.");

                            #endregion

                            VOLUME = QUANTIDADE;

                            if (VOLUME > VALORMAXIMO)
                            {
                                string mensagem = "A ORDEM ENVIADO EXCEDEU O LIMITE MAXIMO CONFIGURADO POR BOLETA.";
                                logger.Info(mensagem);
                                ObjetoOrdem = this.ObterRespostaOMS(EnviarClienteFatFingerRequest.CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                        }

                    }
                }
                else
                {
                    var LimiteBoletaBovespa = from p in EnviarClienteFatFingerResponse.lsConfiguracaoFatFinger
                                              where p.Mercado == "BOVESPA"
                                              select p;

                    if (LimiteBoletaBovespa.Count() > 0)
                    {
                        decimal VALORMAXIMO, QUANTIDADE, PRECO, VOLUME = 0;

                        foreach (var item in LimiteBoletaBovespa)
                        {
                            VALORMAXIMO = item.valorRegra;
                            QUANTIDADE = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                            #region COTACAO

                            logger.Info("INICIALIZA O SERVICO DE COTACOES");

                            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                            CotacaoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                            PRECO = CotacaoResponse.CotacaoInfo.Ultima;

                            #endregion

                            if (LOTEPADRAO > LoteCem)
                            {
                                VOLUME = (PRECO * (QUANTIDADE / EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.LotePadrao));
                            }else{
                                VOLUME = (PRECO  * QUANTIDADE);
                            }
                           
                            if (VOLUME > VALORMAXIMO)
                            {
                                string mensagem = "A ORDEM ENVIADO EXCEDEU O LIMITE MAXIMO CONFIGURADO POR BOLETA.";
                                logger.Info(mensagem);
                                ObjetoOrdem = this.ObterRespostaOMS(EnviarClienteFatFingerRequest.CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;

                            }
                        }
                    }
                }

                #endregion
            

                if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Compra)
                {
                    logger.Info("INICIA THREAD DE ENVIO DE ORDEM DE COMPRA");

                    switch (EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado)
                    {
                        case SegmentoMercadoEnum.OPCAO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO DE OPCOES");
                            ObjetoOrdem = this.EnviarOrdemCompraOpcao(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.AVISTA:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO A VISTA");
                            ObjetoOrdem = this.EnviarOrdemCompraAVista(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.INTEGRALFRACIONARIO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO A VISTA");
                            ObjetoOrdem = this.EnviarOrdemCompraAVista(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.FRACIONARIO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO FRACIONARIO");
                            ObjetoOrdem = this.EnviarOrdemCompraFracionario(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.FUTURO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO FUTURO");
                            ObjetoOrdem = this.EnviarOrdemCompraBMF(EnviarOrdemRiscoRequest);
                            break;

                    }

                }
                else if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Venda)
                {
                    logger.Info("INICIA THREAD DE ENVIO DE ORDEM DE VENDA");

                    switch (EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado)
                    {
                        case SegmentoMercadoEnum.OPCAO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO DE OPCOES");
                            ObjetoOrdem = this.EnviarOrdemVendaOpcao(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.AVISTA:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO A VISTA");
                            ObjetoOrdem = this.EnviarOrdemVendaAVista(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.FRACIONARIO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO FRACIONARIO");
                            ObjetoOrdem = this.EnviarOrdemVendaFracionario(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.INTEGRALFRACIONARIO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO A VISTA");
                            ObjetoOrdem = this.EnviarOrdemVendaAVista(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.FUTURO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO FUTURO");
                            ObjetoOrdem = this.EnviarOrdemVendaBMF(EnviarOrdemRiscoRequest);
                            break;
                    }
                }
            }
            catch (DivideByZeroException ex)
            {
                logger.Info("Não é possível dividir por zero. Verifique as divisões !!! ", ex);
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao invocar o método EnviarOrdem. Cliente: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString(), ex);
            }

            return ObjetoOrdem;

        }        

        /// <summary>
        /// Metodo responsável por efetuar o controle de risco e tratamento da solicitação responsavel por zerar a posicao do cliente.
        /// </summary>
        /// <param name="pParametroOrdemResquest">Informações da Ordem</param>
        /// <returns>EnviarOrdemResponse</returns>
        public EnviarOrdemResponse ZerarPosicao(EnviarOrdemRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
            ZerarPosicaoRequest ZerarPosicaoRequest = new ZerarPosicaoRequest();

            int QuantidadeVenda = 0;
            int QuantidadeCompra = 0;

            logger.Info("ROTINA DE ZERAR POSICAO");
            logger.Info("CLIENTE.........." + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
            logger.Info("INSTRUMENTO......" + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());

            #region VALIDACAO DE REPASSADOR DE ORDENS
            logger.Info("Verifica sessão de repasse para [" + pParametroOrdemResquest.ClienteOrdemInfo.ChannelID + "]");
            if (lstRepassador.Contains(pParametroOrdemResquest.ClienteOrdemInfo.ChannelID))
            {

                logger.Info("OBTEM A SIGLA DO REPASSADOR DE ORDENS");

                string SiglaRepassador = new PersistenciaOrdens().ObterSiglaRepassador(pParametroOrdemResquest.ClienteOrdemInfo.Account);

                if (SiglaRepassador == string.Empty)
                {
                    logger.Info("NÃO EXISTE SENDER LOCATION (SIGLA) CADASTRADA PARA O ASSESSOR.");
                }
                else
                {
                    logger.Info("SENDER LOCATION OBTIDO COM SUCESSO. CODIGO DA SIGLA: " + SiglaRepassador.ToString());
                    pParametroOrdemResquest.ClienteOrdemInfo.ExecBroker = SiglaRepassador;
                }

            
            }

            #endregion

            #region Cotacao

            EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
            CotacaoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
            EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

            #endregion

            pParametroOrdemResquest.ClienteOrdemInfo.Price = double.Parse(CotacaoResponse.CotacaoInfo.Ultima.ToString());

            ZerarPosicaoRequest.ZerarPosicaoInfo.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;
            ZerarPosicaoRequest.ZerarPosicaoInfo.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;

            ZerarPosicaoResponse ZerarPosicaoClientePapel = new PersistenciaOrdens().ZerarPosicaoClientePapel(ZerarPosicaoRequest);

            #region Quantidade na compra

            var QtdeCompra = from p in ZerarPosicaoClientePapel.lstZerarPosicaoInfo
                             where p.Sentido == "C"
                             select p;
            if (QtdeCompra.Count() > 0)
            {
                foreach (var item in QtdeCompra)
                {
                    QuantidadeCompra += item.Quantidade;
                }
            }

            #endregion

            #region Quantidade na venda


            var QtdeVenda = from p in ZerarPosicaoClientePapel.lstZerarPosicaoInfo
                            where p.Sentido == "V"
                            select p;
            if (QtdeVenda.Count() > 0)
            {
                foreach (var item in QtdeVenda)
                {
                    QuantidadeVenda += item.Quantidade;
                }
            }

            #endregion


            int QuantidadeResultante = (QuantidadeCompra - QuantidadeVenda);

            if (QuantidadeResultante < 0)
            {
                pParametroOrdemResquest.ClienteOrdemInfo.Side = OrdemDirecaoEnum.Compra;
            }
            else if (QuantidadeResultante > 0)
            {
                pParametroOrdemResquest.ClienteOrdemInfo.Side = OrdemDirecaoEnum.Venda;
            }
            else if (QuantidadeResultante == 0)
            {
                string mensagem = "NÃO EXISTE POSICAO A SER ZERADA. QUANTIDADE ATUAL 0";
                logger.Info(mensagem);
                ObjetoOrdem = this.ObterRespostaOMS(ZerarPosicaoRequest.ZerarPosicaoInfo.Account, mensagem, CriticaRiscoEnum.ErroNegocio);
                return ObjetoOrdem;
            }


            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QuantidadeResultante;

            logger.Info("QUANTIDADE NA COMPRA............." + QuantidadeCompra.ToString());
            logger.Info("QUANTIDADE NA VENDA.............." + QuantidadeVenda.ToString());
            logger.Info("RESULTANTE ......................" + QuantidadeResultante.ToString());

            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = Math.Abs(pParametroOrdemResquest.ClienteOrdemInfo.OrderQty); ;

            logger.Info("INICIA O PARSEAMENTO DA OFERTA");
            pParametroOrdemResquest = this.PARSEARORDEM(pParametroOrdemResquest);

            pParametroOrdemResquest.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;
            pParametroOrdemResquest.ClienteOrdemInfo.OrdType = OrdemTipoEnum.Mercado;

            logger.Info("TRACE DA SOLICITACAO DE ENVIO DE ORDENS::..........: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
            logger.Info("*******************************************************************");
            logger.Info("DATAHORASOLICITACAO:...........................:" + pParametroOrdemResquest.ClienteOrdemInfo.RegisterTime.ToString());
            logger.Info("CODIGOCLIENTE:.................................:" + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
            logger.Info("NUMEROCONTROLEORDEM:...........................:" + pParametroOrdemResquest.ClienteOrdemInfo.ClOrdID.ToString());
            logger.Info("INSTRUMENTO:...................................:" + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
            logger.Info("DATAVALIDADE:..................................:" + pParametroOrdemResquest.ClienteOrdemInfo.ExpireDate.ToString());
            logger.Info("PRECO:.........................................:" + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());

            if (pParametroOrdemResquest.ClienteOrdemInfo.Price == 0)
            {
                logger.Info("ORDEM A MERCADO :.........................................: A MERCADO");
                pParametroOrdemResquest.ClienteOrdemInfo.OrdType = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemTipoEnum.MarketWithLeftOverLimit;
            }

            if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Compra)
            {
                logger.Info("SENTIDO:.......................................: COMPRA");
            }
            else
            {
                logger.Info("SENTIDO:.......................................: VENDA");
            }

            logger.Info("QUANTIDADE:....................................:" + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
            logger.Info("QUANTIDADEMINIMA:..............................:" + pParametroOrdemResquest.ClienteOrdemInfo.MinQty.ToString());
            logger.Info("QUANTIDADEAPARENTE:............................:" + pParametroOrdemResquest.ClienteOrdemInfo.MaxFloor.ToString());
            logger.Info("VALIDADEORDEM:.................................:" + pParametroOrdemResquest.ClienteOrdemInfo.ExpireDate.ToString());
            logger.Info("*******************************************************************");

            logger.Info("******* INICIA O PROCESSAMENTO ********");

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {
                /*FIRST STEP, THE SYSTEM WILL CHECKS OUT GENERAL PERMISSIONS AND INSTRUMENT RULES */
                /*PRIMEIRO PASSO, VERIFICARA AS REGRAS GERAIS DE PERMISSAO E BLOQUEIO DE ATIVOS DO SISTEMA*/


                #region PARAMETROS GLOBAIS DE RISCO

                logger.Info("PARAMETROS E PERMISSOES CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                logger.Info("CARREGA OS PARAMETROS GLOBAIS DE RISCO DO CLIENTE.");

                ParametrosPermissoesClienteRequest ParametrosPermissoesClienteRequest = new ParametrosPermissoesClienteRequest();
                ParametrosPermissoesClienteRequest.CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

                ParametrosPermissoesClienteResponse ParametrosGlobalRisco = ObjetoPersistenciaOrdens.ObterParametrosGlobalRisco(ParametrosPermissoesClienteRequest);

                EnviarOrdemRiscoRequest.PermissoesGlobal = ContextoOMS.ObterConfiguracoesRisco(ParametrosGlobalRisco, AcaoValidacaoRiscoEnum.PERMISSAO);
                EnviarOrdemRiscoRequest.ParametrosGlobal = ContextoOMS.ObterConfiguracoesRisco(ParametrosGlobalRisco, AcaoValidacaoRiscoEnum.PARAMETRO);
                EnviarOrdemRiscoRequest.ClienteOrdemInfo = pParametroOrdemResquest.ClienteOrdemInfo;


                logger.Info("PARAMETROS E PERMISSOES CARREGADOS COM SUCESSO.");

                #endregion

                #region CADASTRO DE PAPEIS

                logger.Info("CADASTRO DE PAPEIS CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                logger.Info("INICIA VALIDACAO DO INSTRUMENTO NO CADASTRO DE PAPEIS");

                //Obtem informacoes referentes ao cadastro de papel do instrumento selecionado
                EnviarCadastroPapelRequest ObjetoCadastroPapelRequest = new EnviarCadastroPapelRequest();

                string SYMBOL = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;

                ObjetoCadastroPapelRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                EnviarOrdemRiscoRequest.CadastroPapel = ObjetoPersistenciaOrdens.ObterCadastroPapel(ObjetoCadastroPapelRequest);

                logger.Info("CADASTRO DE PAPEIS CARREGADOS COM SUCESSO");

                if (SYMBOL.Substring(SYMBOL.Length - 1, 1) == "F")
                {
                    pParametroOrdemResquest.ClienteOrdemInfo.Symbol = SYMBOL.Remove(SYMBOL.Length - 1);
                }

                #region VALIDACAO DE MERCADO { FRACIONARIO / INTEGRAL }

                int QUANTIDADEORDEM = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;
                int LOTEPADRAO = EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                int MODULOLOTENEGOCIACAO = (QUANTIDADEORDEM % LOTEPADRAO);

                if (QUANTIDADEORDEM < LOTEPADRAO)
                {
                    EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado = SegmentoMercadoEnum.FRACIONARIO;
                }


                #endregion

                #endregion

                #region EXPOSICAO PATRIMONIAL E PERMISSOES POR SEGMENTO DE MERCADO.

                /*
                  ********************************************************************************************************
                  * TRECHO RESPONSAVEL POR VALIDAR AS PERMISSOES PARA OPERAR NO MERCADO DO REESPECTIVO ATIVO SOLICITADO.
                  *******************************************************************************************************/

                ParametrosPermissoesClienteResponse PermissoesGlobal = EnviarOrdemRiscoRequest.PermissoesGlobal;

                #region REGRA DE EXPOSICAO PATRIMONIAL

                logger.Info("VALIDANDO REGRA DE EXPOSICAO PATRIMONIAL MAXIMA");

                var Exposicao = from p in EnviarOrdemRiscoRequest.PermissoesGlobal.lstParametrosPermissoesClienteInfo
                                where p.Permissao == RiscoPermissoesEnum.ValidarExposicaoPatrimonial
                                select p;

                if (Exposicao.Count() > 0)
                {
                    logger.Info("CLIENTE POSSUI RESTRICAO DE EXPOSICAO MAXIMA");
                    logger.Info("OBTENDO CONFIGURACAO GLOBAL DE EXPOSICAO");

                    CalcularExposicaoMaximaRequest request = new CalcularExposicaoMaximaRequest();
                    request.IdCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                    request.dtMovimento = DateTime.Now;

                    CalcularExposicaoMaximaResponse response = new PersistenciaOrdens().ObterExposicaoRiscoCliente(request);

                    if (response.PosicaoEncontrada == true)
                    {
                        logger.Info("POSICAO ENCONTRADA");

                        decimal OscilacaoMaxima = 0;
                        decimal PrejuizoMaximo = 0;

                        decimal PatrimonioLiquido = 0;
                        decimal LucroPrejuizo = 0;

                        logger.Info("OBTER PARAMETROS DE OSCILACAO E PREJUIZO MAXIMO");
                        ParametroExposicaoResponse CalcularExposicaoMaximaResponse = new PersistenciaOrdens().ObterParametrosGlobaisExposicaoRisco(new ParametroExposicaoRequest());

                        logger.Info("PARAMETOS DE PREJUIZO E OSCILACAO MAXIMO CARREGADOS COM SUCESSO.");

                        logger.Info("PATRIMONIO LIQUIDO.............:" + response.ClienteExposicaoInfo.PatrimonioLiquido.ToString());
                        logger.Info("LUCRO PREJUISO.................:" + response.ClienteExposicaoInfo.LucroPrejuizo.ToString());
                        logger.Info("DATA ATUALIZACAO...............:" + response.ClienteExposicaoInfo.DataAtualizacao.ToString());

                        OscilacaoMaxima = CalcularExposicaoMaximaResponse.ParametroExposicaoInfo.OscilacaoMaxima;
                        OscilacaoMaxima = (OscilacaoMaxima / 100);
                        PrejuizoMaximo = CalcularExposicaoMaximaResponse.ParametroExposicaoInfo.PrejuizoMaximo;

                        PatrimonioLiquido = response.ClienteExposicaoInfo.PatrimonioLiquido;
                        LucroPrejuizo = response.ClienteExposicaoInfo.LucroPrejuizo;

                        if (LucroPrejuizo != 0)
                        {

                            decimal PercPerdaPatrimonial = ((LucroPrejuizo / PatrimonioLiquido) * 100);

                            if ((OscilacaoMaxima > 0) && (PrejuizoMaximo != 0))
                            {
                                if (PercPerdaPatrimonial >= OscilacaoMaxima)
                                {
                                    string mensagem = "PREJUIZO PATRIMONIAL MAXIMO ATINGIDO. [" + PercPerdaPatrimonial.ToString() + "] DO PATRIMONIO LIQUIDO TOTAL ATINGIDO. ENTRAR EM CONTATO COM O DEPARTAMENTO DE RISCO.";
                                    logger.Info(mensagem);
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                                if (LucroPrejuizo > PrejuizoMaximo)
                                {
                                    string mensagem = "PREJUIZO PATRIMONIAL MAXIMO ATINGIDO. [" + LucroPrejuizo.ToString() + "] DE PREJUIZO. ENTRAR EM CONTATO COM O DEPARTAMENTO DE RISCO.";
                                    logger.Info(mensagem);
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                            }

                            if ((OscilacaoMaxima > 0) && (PrejuizoMaximo != 0))
                            {
                                if (PercPerdaPatrimonial >= OscilacaoMaxima)
                                {
                                    string mensagem = "PREJUIZO PATRIMONIAL MAXIMO ATINGIDO. [" + PercPerdaPatrimonial.ToString() + "] DO PATRIMONIO LIQUIDO TOTAL ATINGIDO. ENTRAR EM CONTATO COM O DEPARTAMENTO DE RISCO.";
                                    logger.Info(mensagem);
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }
                            }

                            if ((OscilacaoMaxima == 0) && (PrejuizoMaximo != 0))
                            {
                                if (LucroPrejuizo > PrejuizoMaximo)
                                {
                                    string mensagem = "PREJUIZO PATRIMONIAL MAXIMO ATINGIDO. [" + LucroPrejuizo.ToString() + "] DE PREJUIZO. ENTRAR EM CONTATO COM O DEPARTAMENTO DE RISCO.";
                                    logger.Info(mensagem);
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, mensagem, CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }
                            }
                        }

                        logger.Info("CLIENTE ENCONTRA-SE FORA DA ZONA DE RISCO");
                        logger.Info("CONTINUAR VALIDACAO....");
                    }
                    else
                    {
                        logger.Info("CLIENTE AINDA NÃO POSSUI ORDENS ENVIADAS PARA O DIA");
                        logger.Info("CONTINUAR VALIDACAO....");
                    }
                }

                #endregion


                logger.Info("VALIDACAO DE CONTROLE DE PERMISSOES POR MERCADO CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                switch (EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado)
                {
                    case SegmentoMercadoEnum.AVISTA:

                        var pAvista = from p in EnviarOrdemRiscoRequest.PermissoesGlobal.lstParametrosPermissoesClienteInfo
                                      where p.Permissao == RiscoPermissoesEnum.OperarMercadoAVista
                                      select p;

                        if (pAvista.Count() == 0)
                        {
                            logger.Info("CLIENTE SEM PERMISSAO PARA OPERAR NO MERCADO A VISTA. CODIGO DO CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "Usuario sem permissão para operar no mercado a vista.", CriticaRiscoEnum.Permissao);
                            return ObjetoOrdem;
                        }

                        break;
                    case SegmentoMercadoEnum.OPCAO:

                        var pOpcoes = from p in EnviarOrdemRiscoRequest.PermissoesGlobal.lstParametrosPermissoesClienteInfo
                                      where p.Permissao == RiscoPermissoesEnum.OperarMercadoOpoes
                                      select p;

                        if (pOpcoes.Count() == 0)
                        {
                            logger.Info("USUARIO SEM PERMISSAO PARA OPERAR NO MERCADO DE OPCOES. CODIGO DO CLIENTE " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "Usuario sem permissão para operar no mercado de opções", CriticaRiscoEnum.Permissao);
                            return ObjetoOrdem;
                        }

                        break;

                    case SegmentoMercadoEnum.FUTURO:

                        var pFuturos = from p in EnviarOrdemRiscoRequest.PermissoesGlobal.lstParametrosPermissoesClienteInfo
                                       where p.Permissao == RiscoPermissoesEnum.OperarMercadoFuturo
                                       select p;

                        if (pFuturos.Count() == 0)
                        {
                            logger.Info("USUARIO SEM PERMISSAO PARA OPERAR NO MERCADO DE FUTUROS. CODIGO DO CLIENTE " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "Usuario sem permissão para operar no mercado de BMF", CriticaRiscoEnum.Permissao);
                            return ObjetoOrdem;
                        }

                        break;
                }

                logger.Info("CONTROLE DE PERMISSOES CARREGADOS COM SUCESSO. CLIENTE APTO A OPERAR NO MERCADO SELECIONADO.");


                #endregion

                #region ESTADO DOS INSTRUMENTOS

                #region PERMISSAO GLOBAL DO INSTRUMENTO

                logger.Info("VALIDACAO DE CONTROLE DE ESTADO DO INSTRUMENTO GLOBAL : " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());

                InstrumentoBloqueadoRequest InstrumentoBloqueadoGlobalRequest = new InstrumentoBloqueadoRequest();
                InstrumentoBloqueadoGlobalRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;

                InstrumentoBloqueadoResponse InstrumentoBloqueadoGlobalResponse = ObjetoPersistenciaOrdens.CarregarInstrumentosBloqueadosGlobal(InstrumentoBloqueadoGlobalRequest);

                List<string> listaInstrumentos = new List<string>();
                foreach (var Instrumento in InstrumentoBloqueadoGlobalResponse.ListaInstrumentoBloqueio)
                {
                    listaInstrumentos.Add(Instrumento.Instrumento);
                }

                if (listaInstrumentos.Contains(pParametroOrdemResquest.ClienteOrdemInfo.Symbol))
                {
                    logger.Info("O INSTRUMENTO " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol + " ESTA BLOQUEADO.");

                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "INSTRUMENTO BLOQUEADO PELO RISCO. CLIENTE NÃO POSSUI PERMISSÃO PARA OPERAR ESTE INSTRUMENTO", CriticaRiscoEnum.ErroNegocio);
                    return ObjetoOrdem;
                }

                #endregion

                #region PERMISSAO INSTRUMENTO CLIENTE

                logger.Info("VALIDACAO DE CONTROLE DE ESTADO DO INSTRUMENTO CLIENTE : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                InstrumentoBloqueadoRequest InstrumentoBloqueadoRequest = new InstrumentoBloqueadoRequest();
                InstrumentoBloqueadoRequest.IdCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

                InstrumentoBloqueadoResponse InstrumentoBloqueadoResponse = ObjetoPersistenciaOrdens.CarregarInstrumentosBloqueadosCliente(InstrumentoBloqueadoRequest);

                List<string> listaInstrumentosCliente = new List<string>();
                foreach (var Instrumento in InstrumentoBloqueadoResponse.ListaInstrumentoBloqueio)
                {
                    listaInstrumentosCliente.Add(Instrumento.Instrumento);
                }

                if (listaInstrumentosCliente.Contains(pParametroOrdemResquest.ClienteOrdemInfo.Symbol))
                {
                    logger.Info("O INSTRUMENTO " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol + " ESTA BLOQUEADO PARA O CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NÃO POSSUI PERMISSÃO PARA OPERAR ESTE INSTRUMENTO", CriticaRiscoEnum.ErroNegocio);
                    return ObjetoOrdem;
                }


                #endregion

                #endregion

                if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Compra)
                {
                    logger.Info("INICIA THREAD DE ENVIO DE ORDEM DE COMPRA");

                    switch (EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado)
                    {
                        case SegmentoMercadoEnum.OPCAO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO DE OPCOES");
                            ObjetoOrdem = this.EnviarOrdemCompraOpcao(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.AVISTA:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO A VISTA");
                            ObjetoOrdem = this.EnviarOrdemCompraAVista(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.FRACIONARIO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO FRACIONARIO");
                            ObjetoOrdem = this.EnviarOrdemCompraFracionario(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.FUTURO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO FUTURO");
                            ObjetoOrdem = this.EnviarOrdemCompraBMF(EnviarOrdemRiscoRequest);
                            break;

                    }

                }
                else if (pParametroOrdemResquest.ClienteOrdemInfo.Side == OrdemDirecaoEnum.Venda)
                {
                    logger.Info("INICIA THREAD DE ENVIO DE ORDEM DE VENDA");

                    switch (EnviarOrdemRiscoRequest.CadastroPapel.CadastroPapelInfo.SegmentoMercado)
                    {
                        case SegmentoMercadoEnum.OPCAO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO DE OPCOES");
                            ObjetoOrdem = this.EnviarOrdemVendaOpcao(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.AVISTA:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO A VISTA");
                            ObjetoOrdem = this.EnviarOrdemVendaAVista(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.FRACIONARIO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO FRACIONARIO");
                            ObjetoOrdem = this.EnviarOrdemVendaFracionario(EnviarOrdemRiscoRequest);
                            break;
                        case SegmentoMercadoEnum.FUTURO:
                            logger.Info("SEGMENTO DE MERCADO...... MERCADO FUTURO");
                            ObjetoOrdem = this.EnviarOrdemVendaBMF(EnviarOrdemRiscoRequest);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao invocar o método EnviarOrdem. Cliente: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString(), ex);
            }

            return ObjetoOrdem;

        }

        #region CANCELAMENTO DE ORDENS

        /// <summary>
        ///  Metodo responsável por efetuar o controle de risco e tratamento da solicitação de cancelamento de oferta.
        /// </summary>
        /// <param name="pParametroCancelamentoRequest">Dados do cancelamento da oferta</param>
        /// <returns>EnviarCancelamentoOrdemResponse</returns>
        public ExecutarCancelamentoOrdemResponse CancelarOrdem(EnviarCancelamentoOrdemRequest pParametroCancelamentoRequest)
        {

            logger.Info("PREPARA CANCELAMENTO DE ORDENS");
            logger.Info("CLIENTE..........................: " + pParametroCancelamentoRequest.ClienteCancelamentoInfo.Account.ToString());
            logger.Info("OrigClOrdID......................: " + pParametroCancelamentoRequest.ClienteCancelamentoInfo.OrigClOrdID.ToString());

            ExecutarCancelamentoOrdemResponse _EnviarCancelamentoOrdemResponse = new ExecutarCancelamentoOrdemResponse();
            PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();

            LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();
            LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();


            int CODIGOPARAMETROCLIENTECOMPRA = 0;
            int CODIGOPARAMETROCLIENTEVENDA = 0;

            decimal LIMITEALOCADOCOMPRA = 0;
            decimal LIMITEALOCADOVENDA = 0;

            try
            {
                EnviarInformacoesOrdemRequest EnviarInformacoesOrdemRequest = new EnviarInformacoesOrdemRequest();
                EnviarInformacoesOrdemResponse EnviarInformacoesOrdemResponse = new EnviarInformacoesOrdemResponse();

                int CODIGOCLIENTE = pParametroCancelamentoRequest.ClienteCancelamentoInfo.Account;

                pParametroCancelamentoRequest.ClienteCancelamentoInfo.OrigClOrdID = pParametroCancelamentoRequest.ClienteCancelamentoInfo.OrigClOrdID;

                // ****************** ENVIO DO CANCELAMENTO DA OFERTA PARA O ROTEADOR *************************/
                pParametroCancelamentoRequest = this.PARSEARORDEMCANCELAMENTO(pParametroCancelamentoRequest);

                #region VALIDACAO DE REPASSADOR DE ORDENS

                if (lstRepassador.Contains(pParametroCancelamentoRequest.ClienteCancelamentoInfo.ChannelID))
                {

                    logger.Info("OBTEM A SIGLA DO REPASSADOR DE ORDENS");

                    string SiglaRepassador = new PersistenciaOrdens().ObterSiglaRepassador(pParametroCancelamentoRequest.ClienteCancelamentoInfo.Account);

                    if (SiglaRepassador == string.Empty)
                    {
                        logger.Info("NÃO EXISTE SENDER LOCATION (SIGLA) CADASTRADA PARA O ASSESSOR.");
                    }
                    else
                    {
                        logger.Info("SENDER LOCATION OBTIDO COM SUCESSO. CODIGO DA SIGLA: " + SiglaRepassador.ToString());
                        pParametroCancelamentoRequest.ClienteCancelamentoInfo.ExecBroker = SiglaRepassador;
                    }
                  
                }

                #endregion

                logger.Info("INVOCA O ROTEADOR DE ORDENS PARA EFETUAR O CANCELAMENTO DA OFERTA");

                #region CADASTRO DE PAPEIS

                //Obtem informacoes referentes ao cadastro de papel do instrumento selecionado
                EnviarCadastroPapelRequest ObjetoCadastroPapelRequest = new EnviarCadastroPapelRequest();
                EnviarCadastroPapelResponse ObjetoCadastroPapelResponse = new EnviarCadastroPapelResponse();

                string SYMBOL = pParametroCancelamentoRequest.ClienteCancelamentoInfo.Symbol;


                ObjetoCadastroPapelRequest.Instrumento = pParametroCancelamentoRequest.ClienteCancelamentoInfo.Symbol.Trim();
                ObjetoCadastroPapelResponse = PersistenciaOrdens.ObterCadastroPapel(ObjetoCadastroPapelRequest);

                #endregion

                switch (ObjetoCadastroPapelResponse.CadastroPapelInfo.SegmentoMercado)
                {                   
                    case SegmentoMercadoEnum.FUTURO:
                        logger.Info("SOLICITACAO DE CANCELAMENTO DE ORDEM BMF");
                        _EnviarCancelamentoOrdemResponse = this.EnviarCancelamentoOrdemBMFRoteador(pParametroCancelamentoRequest);
                        break;
                    default:
                        logger.Info("SOLICITACAO DE CANCELAMENTO DE ORDEM BPVESPA");
                        _EnviarCancelamentoOrdemResponse = this.EnviarCancelamentoOrdemRoteador(pParametroCancelamentoRequest);
                        break;
                }

             

                if (_EnviarCancelamentoOrdemResponse.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                {

                    logger.Info("ORDEM CANCELADA COM SUCESSO.");
                    logger.Info("PREPARA O RECALCULO DOS LIMITES DO CLIENTE");

                   //***************** RECALCULA O LIMITE ******************/                

                    #region VOLUME DO CANCELAMENTO DA ORDEM

                    decimal PRICE = 0;

                    EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                    CotacaoRequest.Instrumento = pParametroCancelamentoRequest.ClienteCancelamentoInfo.Symbol.Trim();
                    EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                    PRICE = CotacaoResponse.CotacaoInfo.Ultima + (CotacaoResponse.CotacaoInfo.Ultima * (5 / 100));


                    decimal VOLUMEORDEM = (pParametroCancelamentoRequest.ClienteCancelamentoInfo.OrderQty * PRICE);

                    #endregion

                    //switch (ObjetoCadastroPapelResponse.CadastroPapelInfo.SegmentoMercado)
                    //{
                    //    case SegmentoMercadoEnum.AVISTA:

                    //        #region RECALCULO DE LIMITE NO MERCADO A VISTA

                    //        LimiteOperacionalRequest.CodigoCliente = CODIGOCLIENTE;
                    //        ObjetoLimitesOperacionais = PersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                    //        #region LIMITE DE COMPRA

                    //        var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                    //                                 where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                    //                                 select p;

                    //        //SALDO DE COMPRA NO MERCADO A VISTA.
                    //        if (LIMITECOMPRAAVISTA.Count() > 0)
                    //        {

                    //            foreach (var ITEM in LIMITECOMPRAAVISTA)
                    //            {
                    //                // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                            
                    //                CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                    //                LIMITEALOCADOCOMPRA = ITEM.ValorAlocado;
                    //            }
                    //        }

                    //        #endregion

                    //        #region LIMITE VENDA


                    //        var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                    //                                where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                    //                                select p;

                    //        //SALDO DE COMPRA NO MERCADO A VISTA.
                    //        if (LIMITEVENDAAVISTA.Count() > 0)
                    //        {
                    //            foreach (var ITEM in LIMITEVENDAAVISTA)
                    //            {
                    //                // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                    //                CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                    //                LIMITEALOCADOVENDA = ITEM.ValorAlocado;
                    //            }
                    //        }

                    //        #endregion

                    //        if (pParametroCancelamentoRequest.ClienteCancelamentoInfo.Side == OrdemDirecaoEnum.Compra)
                    //        {
                    //            if (LIMITEALOCADOCOMPRA >= VOLUMEORDEM)
                    //            {

                    //                //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                    //                AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                    //                AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de compra a vista");

                    //                //******************** CREDITA O LIMITE DE VENDA ************************************************/
                    //                AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                    //                AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de venda a vista");
                    //            }

                    //        }
                    //        else if (pParametroCancelamentoRequest.ClienteCancelamentoInfo.Side == OrdemDirecaoEnum.Venda)
                    //        {
                    //            if (LIMITEALOCADOVENDA >= VOLUMEORDEM)
                    //            {
                    //                //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                    //                AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                    //                AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de compra a vista");

                    //                //******************** CREDITA O LIMITE DE VENDA ************************************************/
                    //                AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                    //                AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);

                    //                logger.Info("Atualiza os limites de venda a vista");
                    //            }
                    //        }

                    //        #endregion

                    //        break;
                    //    case SegmentoMercadoEnum.FRACIONARIO:

                    //        #region  RECALCULO DE LIMITE NO MERCADO FRACIONARIO

                    //        LimiteOperacionalRequest.CodigoCliente = CODIGOCLIENTE;
                    //        ObjetoLimitesOperacionais = PersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                    //        #region LIMITE DE COMPRA

                    //        var LIMITECOMPRAFRACIONARIO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                    //                                      where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                    //                                      select p;

                    //        //SALDO DE COMPRA NO MERCADO A VISTA.
                    //        if (LIMITECOMPRAFRACIONARIO.Count() > 0)
                    //        {

                    //            foreach (var ITEM in LIMITECOMPRAFRACIONARIO)
                    //            {
                    //                // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                            
                    //                CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                    //                LIMITEALOCADOCOMPRA = ITEM.ValorAlocado;
                    //            }
                    //        }

                    //        #endregion

                    //        #region LIMITE VENDA


                    //        var LIMITEVENDAFRACIONARIO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                    //                                     where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                    //                                     select p;

                    //        //SALDO DE COMPRA NO MERCADO A VISTA.
                    //        if (LIMITEVENDAFRACIONARIO.Count() > 0)
                    //        {

                    //            foreach (var ITEM in LIMITEVENDAFRACIONARIO)
                    //            {
                    //                // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                    //                CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                    //                LIMITEALOCADOVENDA = ITEM.ValorAlocado;
                    //            }
                    //        }

                    //        #endregion

                    //        if (pParametroCancelamentoRequest.ClienteCancelamentoInfo.Side == OrdemDirecaoEnum.Compra)
                    //        {
                    //            if (LIMITEALOCADOCOMPRA >= VOLUMEORDEM)
                    //            {
                    //                //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                    //                AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                    //                AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de compra a vista");

                    //                //******************** CREDITA O LIMITE DE VENDA ************************************************/
                    //                AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                    //                AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de venda a vista");
                    //            }

                    //        }
                    //        else if (pParametroCancelamentoRequest.ClienteCancelamentoInfo.Side == OrdemDirecaoEnum.Venda)
                    //        {
                    //            if (LIMITEALOCADOVENDA >= VOLUMEORDEM)
                    //            {
                    //                //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                    //                AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                    //                AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de compra a vista");

                    //                //******************** CREDITA O LIMITE DE VENDA ************************************************/
                    //                AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                    //                AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de venda a vista");

                    //            }
                    //        }

                    //        #endregion

                    //        break;
                    //    case SegmentoMercadoEnum.OPCAO:

                    //        #region RECALCULO DE LIMITE NO MERCAO DE OPCOES

                    //        LimiteOperacionalRequest.CodigoCliente = CODIGOCLIENTE;
                    //        ObjetoLimitesOperacionais = PersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                    //        #region LIMITE DE COMPRA

                    //        var LIMITECOMPRAOPCAO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                    //                                where p.TipoLimite == TipoLimiteEnum.COMPRAOPCOES
                    //                                select p;

                    //        //SALDO DE COMPRA NO MERCADO A VISTA.
                    //        if (LIMITECOMPRAOPCAO.Count() > 0)
                    //        {

                    //            foreach (var ITEM in LIMITECOMPRAOPCAO)
                    //            {
                    //                // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                            
                    //                CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                    //                LIMITEALOCADOCOMPRA = ITEM.ValorAlocado;
                    //            }
                    //        }

                    //        #endregion

                    //        #region LIMITE VENDA


                    //        var LIMITEVENDAOPCAO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                    //                               where p.TipoLimite == TipoLimiteEnum.VENDAOPCOES
                    //                               select p;

                    //        //SALDO DE COMPRA NO MERCADO A VISTA.
                    //        if (LIMITEVENDAOPCAO.Count() > 0)
                    //        {

                    //            foreach (var ITEM in LIMITEVENDAOPCAO)
                    //            {
                    //                // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                    //                CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                    //                LIMITEALOCADOVENDA = ITEM.ValorAlocado;
                    //            }
                    //        }

                    //        #endregion

                    //        if (pParametroCancelamentoRequest.ClienteCancelamentoInfo.Side == OrdemDirecaoEnum.Compra)
                    //        {
                    //            if (LIMITEALOCADOCOMPRA >= VOLUMEORDEM)
                    //            {

                    //                //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                    //                AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                    //                AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de compra a vista");

                    //                //******************** CREDITA O LIMITE DE VENDA ************************************************/
                    //                AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                    //                AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de venda a vista");
                    //            }

                    //        }
                    //        else if (pParametroCancelamentoRequest.ClienteCancelamentoInfo.Side == OrdemDirecaoEnum.Venda)
                    //        {
                    //            if (LIMITEALOCADOVENDA >= VOLUMEORDEM)
                    //            {
                    //                //****************************** DEBITA O LIMITE DE COMPRA **************************************/
                    //                AtualizarLimitesRequest AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTECOMPRA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.COMPRAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM;

                    //                AtualizarLimitesResponse AtualizarLimitesResponse = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de compra a vista");

                    //                //******************** CREDITA O LIMITE DE VENDA ************************************************/
                    //                AtualizarLimitesRequest = new AtualizarLimitesRequest();

                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoCliente = CODIGOCLIENTE;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.CodigoParametroCliente = CODIGOPARAMETROCLIENTEVENDA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.TipoLimite = TipoLimiteEnum.VENDAAVISTA;
                    //                AtualizarLimitesRequest.LimiteOperacionalInfo.ValorAlocado = VOLUMEORDEM * -1;

                    //                AtualizarLimitesResponse AtualizarLimitesResponseVenda = this.AtualizarLimiteOperacional(AtualizarLimitesRequest);
                    //                logger.Info("Atualiza os limites de venda a vista");
                    //            }

                    //        }

                    //        #endregion

                    //        break;
                    //}

                    logger.Info("LIMITE RECALCULADO COM SUCESSO");
                }

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO CANCELAR A ORDEM DO CLIENTE: " + pParametroCancelamentoRequest.ClienteCancelamentoInfo.Account.ToString());
                logger.Error("DESCRICAO DO ERRO: " + ex.Message, ex);

                _EnviarCancelamentoOrdemResponse.DadosRetorno.Ocorrencias[0].Ocorrencia = "OCORREU UM PROBLEMA AO CANCELAR A ORDEM.";
                _EnviarCancelamentoOrdemResponse.DadosRetorno.StatusResposta = StatusRoteamentoEnum.Erro;
                _EnviarCancelamentoOrdemResponse.DadosRetorno.StackTrace = ex.StackTrace;
                _EnviarCancelamentoOrdemResponse.DadosRetorno.DataResposta = DateTime.Now;

            }

            _EnviarCancelamentoOrdemResponse.DadosRetorno.Ocorrencias[0].Ocorrencia = "ORDEM CANCELADA COM SUCESSO.";
            _EnviarCancelamentoOrdemResponse.DadosRetorno.StatusResposta = StatusRoteamentoEnum.Sucesso;
            _EnviarCancelamentoOrdemResponse.DadosRetorno.DataResposta = DateTime.Now;

            return _EnviarCancelamentoOrdemResponse;


        }

        #endregion

        #region ENVIO DE ORDEM BMF

        /// <summary>
        /// METODO RESPONSAVEL POR ENVIAR UMA ORDEM DE COMPRA PARA O MERCADO DE BMF
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns></returns>
        private EnviarOrdemResponse EnviarOrdemCompraBMFxx(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            SecurityIDRequest SecurityIDRequest = new Lib.SecurityIDRequest();


            logger.Info("OBTEM O CODIGO DE BMF DO CLIENTE ATRAVES DO CODIGO BOVESPA CADASTRADO.");

            string CodigoBMF = new PersistenciaOrdens().ObterCodigoBMF(pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

            logger.Info("CODIGO BOVESPA : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
            logger.Info("CODIGO BMF : " + CodigoBMF.ToString());

            pParametroOrdemResquest.ClienteOrdemInfo.Account = int.Parse(CodigoBMF);

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {
                /**************************************************************************************************/
                // CONFIGURA AS INFORMACOES BASICAS PARA O ENVIO DE UMA OFERTA DE BMF (Exchange,ChannelID)
                /**************************************************************************************************/

                logger.Info("INICIA ENVIO DE ORDEM DE COMPRA NO MERCADO DE BMF");
                logger.Info("CLIENTE........: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                SecurityIDRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                SecurityIDResponse SecurityIDResponse = ObjetoPersistenciaOrdens.ObterSecurityID(SecurityIDRequest);

                pParametroOrdemResquest.ClienteOrdemInfo.Exchange = "BMF";
                pParametroOrdemResquest.ClienteOrdemInfo.ChannelID = 0;

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    logger.Info("INSTRUMENTO..............: " + SecurityIDResponse.Instrumento);
                    logger.Info("SECURITYID ..............: " + SecurityIDResponse.SecurityID);
                    logger.Info("QUANTIDADE ..............: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());

                    logger.Info("TIPO DE OPERACAO .................: ALTERACAO DE ORDENS");

                    #region [ALTERACAO DE ORDEM]

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;

                    logger.Info("OBTEM INFORMACOES SOBRE A ORDEM ORIGINAL. ORIGCLORDID: " + pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID);

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    logger.Info("INFORMACOES OBTIDAS COM SUCESSO.");

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);
                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                    logger.Info("VOLUME ORIGINAL ......................: " + VOLUMEORIGINAL.ToString());
                    logger.Info("VOLUME SOLICITADO ....................: " + VOLUMEMODIFICACAO.ToString());
                    logger.Info("DIFERENCIAL ..........................: " + DIFERENCIAL.ToString());

                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);
                    ExecutarModificacaoOrdensRequest.info.Exchange = "BMF";
                    ExecutarModificacaoOrdensRequest.info.ChannelID = 0;

                    // VERIFICA SE A ORDEM E DIFERENTE DA ORIGINAL
                    if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    {
                        logger.Info("ORDEM IDENTICA A ORIGINAL");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM IDENTICA A ORIGINAL", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }
                    else
                    {
                        #region Controle de Pre-Risco BMF

                        AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                        AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                        int QuantidadeContrato = 0;
                        int QuantidadeInstrumento = 0;
                        int idClienteParametroBMF = 0;
                        char stUtilizacaoInstrumento = 'N';

                        logger.Info("EFETUA O CONTROLE DE PRE RISCO NO MERCADO DE BMF");

                        logger.Info("OBTEM OS LIMITES PARA O MERCADO DE BMF");
                        ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();
                        requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;

                        if (pParametroOrdemResquest.IsContaMaster)
                        {
                            requestListaLimite.Account = pParametroOrdemResquest.ContaMaster;
                        }
                        else
                        {
                            requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                        }

                        ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                        logger.Info("LIMITES OBTIDOS COM SUCESSO.");

                        string Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                        string Contrato = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Substring(0, 3);

                        //CONTRATO BASE
                        var PosicaoContrato = from p in response.ListaLimites
                                              where p.Contrato == Contrato
                                              && p.Sentido == "C"
                                              select p;

                        if (PosicaoContrato.Count() > 0)
                        {
                            foreach (var item in PosicaoContrato)
                            {
                                idClienteParametroBMF = item.idClienteParametroBMF;
                                QuantidadeContrato = item.QuantidadeDisponivel;

                                //CONTRATO PAI
                                if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > item.QuantidadeMaximaOferta)
                                {
                                    logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + item.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + item.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;

                                }

                                if (response.ListaLimitesInstrumentos.Count > 0)
                                {
                                    // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                    foreach (var vencimento in response.ListaLimitesInstrumentos)
                                    {                                       

                                        if (vencimento.Instrumento == Instrumento)
                                        {
                                            if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > vencimento.QuantidadeMaximaOferta)
                                            {
                                                logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + vencimento.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + vencimento.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                                return ObjetoOrdem;

                                            }

                                            if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                            {

                                                QuantidadeInstrumento += vencimento.QtDisponivel;
                                                Instrumento = vencimento.Instrumento;
                                                stUtilizacaoInstrumento = 'S';
                                            }
                                        }
                                    }
                                }
                            }
                          

                            int QuantidadeSolicitada = 0;

                            int QuantidadeResultanteOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.OrderQty;

                            // QUANTIDADE ORIGINAL  > QUANTIDADE SOLICITADA
                            if (InformacoesOrdem.OrdemInfo.OrderQty > pParametroOrdemResquest.ClienteOrdemInfo.OrderQty)
                            {
                                #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                                logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL NO BANCO DE DADOS.");
                                this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                                logger.Info("BACKUP REALIZADO COM SUCESSO.");

                                EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                                EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                                logger.Info("GRAVA A ORDEM NO BANCO DE DADOS.");
                                this.InserirOrdemCliente(EnviarOrdemRiscoRequest);
                                logger.Info("ORDEM SALVA COM SUCESSO.");

                                #endregion

                                #region ENVIA A ORDEM PARA A BOLSA

                                logger.Info("ENVIA A ORDEM PARA A BOLSA");
                                ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteadorBMF(ExecutarModificacaoOrdensRequest.info);

                                if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                {
                                    logger.Info("ORDEM ENVIADA COM SUCESSO.");
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                    return ObjetoOrdem;

                                }
                                else
                                {
                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA A BOLSA.");
                                    logger.Info("DESCRICAO: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                }

                                #endregion

                            }   // QUANTIDADE ORIGINAL < QUANTIDADE SOLICITADA
                            else if (InformacoesOrdem.OrdemInfo.OrderQty < pParametroOrdemResquest.ClienteOrdemInfo.OrderQty)
                            {
                                QuantidadeSolicitada = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.OrderQty;
                            }
                            
                            
                            // LIMITE SOMENTE PARA O CONTRATO
                            if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                            {
                                logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                                logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                                if (QuantidadeSolicitada > QuantidadeInstrumento)
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                            }

                            // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                            if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                            {
                                logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                                logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                                if (stUtilizacaoInstrumento == 'S')
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    //QUANTIDADE INSUFICIENTE
                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                                if (QuantidadeSolicitada > QuantidadeContrato)
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    //QUANTIDADE INSUFICIENTE
                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }
                            }
                        }
                        else
                        {
                            logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                            ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Cliente não possui limite para operar este contrato.Por favor, entre em contato com o nosso atendimento.", CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }


                        #endregion

                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL NO BANCO DE DADOS.");
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                        logger.Info("BACKUP REALIZADO COM SUCESSO.");

                        EnviarOrdemRiscoRequest _EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        _EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        logger.Info("GRAVA A ORDEM NO BANCO DE DADOS.");
                        this.InserirOrdemCliente(_EnviarOrdemRiscoRequest);
                        logger.Info("ORDEM SALVA COM SUCESSO.");

                        #endregion

                        #region ENVIA A ORDEM PARA A BOLSA

                        logger.Info("ENVIA A ORDEM PARA A BOLSA");
                        ExecutarModificacaoOrdensResponse _respostaAutenticacao = this.ModificarOrdemRoteadorBMF(ExecutarModificacaoOrdensRequest.info);

                        if (_respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("ORDEM ENVIADA COM SUCESSO.");
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;

                        }
                        else
                        {
                            logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA A BOLSA.");
                            logger.Info("DESCRICAO: " + _respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {

                    #region Controle de Pre-Risco BMF

                    AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                    AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                    int QuantidadeContrato = 0;
                    int QuantidadeInstrumento = 0;
                    int idClienteParametroBMF = 0;
                    char stUtilizacaoInstrumento = 'N';

                    logger.Info("EFETUA O CONTROLE DE PRE RISCO NO MERCADO DE BMF");

                    logger.Info("OBTEM OS LIMITES PARA O MERCADO DE BMF");
                    ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();

                    if (pParametroOrdemResquest.IsContaMaster)
                    {
                        requestListaLimite.Account = pParametroOrdemResquest.ContaMaster;
                    }
                    else
                    {
                        requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                    }

                    ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                    logger.Info("LIMITES OBTIDOS COM SUCESSO.");

                    string Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                    string Contrato = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Substring(0, 3);

                    //CONTRATO BASE
                    var PosicaoContrato = from p in response.ListaLimites
                                          where p.Contrato == Contrato
                                          && p.Sentido == "C"
                                          select p;

                    if (PosicaoContrato.Count() > 0)
                    {
                        foreach (var item in PosicaoContrato)
                        {
                            idClienteParametroBMF = item.idClienteParametroBMF;
                            QuantidadeContrato = item.QuantidadeDisponivel;

                            //CONTRATO PAI
                            if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > item.QuantidadeMaximaOferta)
                            {
                                logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + item.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + item.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;

                            }

                            if (response.ListaLimitesInstrumentos.Count > 0)
                            {                             

                                // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                foreach (var vencimento in response.ListaLimitesInstrumentos)
                                {
                                    if (vencimento.Instrumento == Instrumento)
                                    {
                                        if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > vencimento.QuantidadeMaximaOferta)
                                        {
                                            logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + vencimento.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + vencimento.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                            return ObjetoOrdem;

                                        }


                                        if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                        {

                                            QuantidadeInstrumento += vencimento.QtDisponivel;
                                            Instrumento = vencimento.Instrumento;
                                            stUtilizacaoInstrumento = 'S';
                                        }
                                    }
                                }
                            }
                        }

                        if ((QuantidadeContrato == 0) && (QuantidadeInstrumento == 0))
                        {
                            logger.Info("LIMITE CONTRATO ..........:" + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........:" + QuantidadeInstrumento.ToString());

                            logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                            ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }

                        int QuantidadeSolicitada = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                        // LIMITE SOMENTE PARA O CONTRATO
                        if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                        {
                            logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                            logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                            if (QuantidadeSolicitada > QuantidadeInstrumento)
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                        }

                        // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                        if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                        {
                            logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                            logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                            if (stUtilizacaoInstrumento == 'S')
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                //QUANTIDADE INSUFICIENTE
                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                            if (QuantidadeSolicitada > QuantidadeContrato)
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                //QUANTIDADE INSUFICIENTE
                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }
                        }
                    }
                    else
                    {
                        logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                        ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Cliente não possui limite para operar este contrato.Por favor, entre em contato com o nosso atendimento.", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }


                    #endregion


                    logger.Info("SALVAR ORDEM NO BANCO DE DADOS.");
                    this.InserirOrdemCliente(pParametroOrdemResquest);
                    logger.Info("ORDEM SALVA COM SUCESSO.");

                    #region ENTRY POINT

                    //  pParametroOrdemResquest.ClienteOrdemInfo.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString().Remove(pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString().Length - 1, 1).DBToInt32();

                    logger.Info("ENVIA ORDEM PARA O ROTEADOR.");
                    //*************************** SESSÃO RESPONSAVEL POR ENVIAR UMA NOVA ORDEM PARA O MERCADO DE BMF
                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteadorBMF(pParametroOrdemResquest.ClienteOrdemInfo);

                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("ORDEM ENVIADA COM SUCESSO.");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                        return ObjetoOrdem;
                    }
                    else
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                        {

                            logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR.");
                            logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                            return ObjetoOrdem;
                        }

                    #endregion
                }

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "ORDEM ENVIADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                return ObjetoOrdem;

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ENVIAR A OFERTA DE COMPRA NO MERCADO DE BMF.", ex);

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "OCORREU UM ERRO AO ENVIAR A OFERTA DE COMPRA NO MERCADO DE BMF", CriticaRiscoEnum.Exception);
                return ObjetoOrdem;
            }


            return ObjetoOrdem;
        }

        /// <summary>
        /// METODO RESPONSAVEL POR ENVIAR UMA ORDEM DE VENDA PARA O MERCADO DE BMF
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns></returns>
        private EnviarOrdemResponse EnviarOrdemVendaBMFxx(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            SecurityIDRequest SecurityIDRequest = new Lib.SecurityIDRequest();


            logger.Info("OBTEM O CODIGO DE BMF DO CLIENTE ATRAVES DO CODIGO BOVESPA CADASTRADO.");

            string CodigoBMF = new PersistenciaOrdens().ObterCodigoBMF(pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

            logger.Info("CODIGO BOVESPA : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
            logger.Info("CODIGO BMF : " + CodigoBMF.ToString());

            pParametroOrdemResquest.ClienteOrdemInfo.Account = int.Parse(CodigoBMF);

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {
                /**************************************************************************************************/
                // CONFIGURA AS INFORMACOES BASICAS PARA O ENVIO DE UMA OFERTA DE BMF (Exchange,ChannelID)
                /**************************************************************************************************/

                logger.Info("INICIA ENVIO DE ORDEM DE COMPRA NO MERCADO DE BMF");
                logger.Info("CLIENTE........: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                SecurityIDRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                SecurityIDResponse SecurityIDResponse = ObjetoPersistenciaOrdens.ObterSecurityID(SecurityIDRequest);

                pParametroOrdemResquest.ClienteOrdemInfo.Exchange = "BMF";
                pParametroOrdemResquest.ClienteOrdemInfo.ChannelID = 0;

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    logger.Info("INSTRUMENTO..............: " + SecurityIDResponse.Instrumento);
                    logger.Info("SECURITYID ..............: " + SecurityIDResponse.SecurityID);
                    logger.Info("QUANTIDADE ..............: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());

                    logger.Info("TIPO DE OPERACAO .................: ALTERACAO DE ORDENS");

                    #region [ALTERACAO DE ORDEM]

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;

                    logger.Info("OBTEM INFORMACOES SOBRE A ORDEM ORIGINAL. ORIGCLORDID: " + pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID);

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    logger.Info("INFORMACOES OBTIDAS COM SUCESSO.");

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);
                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                    logger.Info("VOLUME ORIGINAL ......................: " + VOLUMEORIGINAL.ToString());
                    logger.Info("VOLUME SOLICITADO ....................: " + VOLUMEMODIFICACAO.ToString());
                    logger.Info("DIFERENCIAL ..........................: " + DIFERENCIAL.ToString());

                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);
                    ExecutarModificacaoOrdensRequest.info.Exchange = "BMF";
                    ExecutarModificacaoOrdensRequest.info.ChannelID = 0;

                    // VERIFICA SE A ORDEM E DIFERENTE DA ORIGINAL
                    if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    {
                        logger.Info("ORDEM IDENTICA A ORIGINAL");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM IDENTICA A ORIGINAL", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }
                    else
                    {
                        #region Controle de Pre-Risco BMF

                        AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                        AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                        int QuantidadeContrato = 0;
                        int QuantidadeInstrumento = 0;
                        int idClienteParametroBMF = 0;
                        char stUtilizacaoInstrumento = 'N';

                        logger.Info("EFETUA O CONTROLE DE PRE RISCO NO MERCADO DE BMF");

                        logger.Info("OBTEM OS LIMITES PARA O MERCADO DE BMF");
                        ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();

                        if (pParametroOrdemResquest.IsContaMaster)
                        {
                            requestListaLimite.Account = pParametroOrdemResquest.ContaMaster;
                        }
                        else
                        {
                            requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                        }

                        ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                        logger.Info("LIMITES OBTIDOS COM SUCESSO.");

                        string Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                        string Contrato = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Substring(0, 3);

                        //CONTRATO BASE
                        var PosicaoContrato = from p in response.ListaLimites
                                              where p.Contrato == Contrato
                                              && p.Sentido == "V"
                                              select p;

                        if (PosicaoContrato.Count() > 0)
                        {
                            foreach (var item in PosicaoContrato)
                            {
                                idClienteParametroBMF = item.idClienteParametroBMF;
                                QuantidadeContrato = item.QuantidadeDisponivel;

                                //CONTRATO PAI
                                if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > item.QuantidadeMaximaOferta)
                                {
                                    logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + item.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + item.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;

                                }

                                if (response.ListaLimitesInstrumentos.Count > 0)
                                {
                                    // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                    foreach (var vencimento in response.ListaLimitesInstrumentos)
                                    {
                                        if (vencimento.Instrumento == Instrumento)
                                        {
                                            if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > vencimento.QuantidadeMaximaOferta)
                                            {
                                                logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + vencimento.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + vencimento.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                                return ObjetoOrdem;

                                            }


                                            if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                            {

                                                QuantidadeInstrumento += vencimento.QtDisponivel;
                                                Instrumento = vencimento.Instrumento;
                                                stUtilizacaoInstrumento = 'S';
                                            }
                                        }
                                    }
                                }
                            }

                            if ((QuantidadeContrato == 0) && (QuantidadeInstrumento == 0))
                            {
                                logger.Info("LIMITE CONTRATO ..........:" + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........:" + QuantidadeInstrumento.ToString());

                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                            int QuantidadeSolicitada = 0;

                            int QuantidadeResultanteOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.OrderQty;

                            // QUANTIDADE ORIGINAL  > QUANTIDADE SOLICITADA
                            if (InformacoesOrdem.OrdemInfo.OrderQty > pParametroOrdemResquest.ClienteOrdemInfo.OrderQty)
                            {
                                #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                                logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL NO BANCO DE DADOS.");
                                this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                                logger.Info("BACKUP REALIZADO COM SUCESSO.");

                                EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                                EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                                logger.Info("GRAVA A ORDEM NO BANCO DE DADOS.");
                                this.InserirOrdemCliente(EnviarOrdemRiscoRequest);
                                logger.Info("ORDEM SALVA COM SUCESSO.");

                                #endregion

                                #region ENVIA A ORDEM PARA A BOLSA

                                logger.Info("ENVIA A ORDEM PARA A BOLSA");
                                ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteadorBMF(ExecutarModificacaoOrdensRequest.info);

                                if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                {
                                    logger.Info("ORDEM ENVIADA COM SUCESSO.");
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                    return ObjetoOrdem;

                                }
                                else
                                {
                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA A BOLSA.");
                                    logger.Info("DESCRICAO: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                }

                                #endregion

                            }   // QUANTIDADE ORIGINAL < QUANTIDADE SOLICITADA
                            else if (InformacoesOrdem.OrdemInfo.OrderQty < pParametroOrdemResquest.ClienteOrdemInfo.OrderQty)
                            {
                                QuantidadeSolicitada = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.OrderQty;
                            }


                            // LIMITE SOMENTE PARA O CONTRATO
                            if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                            {
                                logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                                logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                                if (QuantidadeSolicitada > QuantidadeInstrumento)
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                            }

                            // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                            if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                            {
                                logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                                logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                                if (stUtilizacaoInstrumento == 'S')
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    //QUANTIDADE INSUFICIENTE
                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                                if (QuantidadeSolicitada > QuantidadeContrato)
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    //QUANTIDADE INSUFICIENTE
                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }
                            }
                        }
                        else
                        {
                            logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                            ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Cliente não possui limite para operar este contrato.Por favor, entre em contato com o nosso atendimento.", CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }


                        #endregion

                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL NO BANCO DE DADOS.");
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                        logger.Info("BACKUP REALIZADO COM SUCESSO.");

                        EnviarOrdemRiscoRequest _EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        _EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        logger.Info("GRAVA A ORDEM NO BANCO DE DADOS.");
                        this.InserirOrdemCliente(_EnviarOrdemRiscoRequest);
                        logger.Info("ORDEM SALVA COM SUCESSO.");

                        #endregion

                        #region ENVIA A ORDEM PARA A BOLSA

                        logger.Info("ENVIA A ORDEM PARA A BOLSA");
                        ExecutarModificacaoOrdensResponse _respostaAutenticacao = this.ModificarOrdemRoteadorBMF(ExecutarModificacaoOrdensRequest.info);

                        if (_respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("ORDEM ENVIADA COM SUCESSO.");
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;

                        }
                        else
                        {
                            logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA A BOLSA.");
                            logger.Info("DESCRICAO: " + _respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {

                    #region Controle de Pre-Risco BMF

                    AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                    AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                    int QuantidadeContrato = 0;
                    int QuantidadeInstrumento = 0;
                    int idClienteParametroBMF = 0;
                    char stUtilizacaoInstrumento = 'N';

                    logger.Info("EFETUA O CONTROLE DE PRE RISCO NO MERCADO DE BMF");

                    logger.Info("OBTEM OS LIMITES PARA O MERCADO DE BMF");
                    ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();

                    if (pParametroOrdemResquest.IsContaMaster)
                    {
                        requestListaLimite.Account = pParametroOrdemResquest.ContaMaster;
                    }
                    else
                    {
                        requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                    }

                    ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                    logger.Info("LIMITES OBTIDOS COM SUCESSO.");

                    string Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                    string Contrato = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Substring(0, 3);

                    //CONTRATO BASE
                    var PosicaoContrato = from p in response.ListaLimites
                                          where p.Contrato == Contrato
                                          && p.Sentido == "V"
                                          select p;

                    if (PosicaoContrato.Count() > 0)
                    {
                        foreach (var item in PosicaoContrato)
                        {
                            //CONTRATO PAI
                            if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > item.QuantidadeMaximaOferta)
                            {
                                logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + item.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + item.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;

                            }

                            idClienteParametroBMF = item.idClienteParametroBMF;
                            QuantidadeContrato = item.QuantidadeDisponivel;

                            if (response.ListaLimitesInstrumentos.Count > 0)
                            {
                                // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                foreach (var vencimento in response.ListaLimitesInstrumentos)
                                {
                                    if (vencimento.Instrumento == Instrumento)
                                    {
                                        if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > vencimento.QuantidadeMaximaOferta)
                                        {
                                            logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + vencimento.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + vencimento.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                            return ObjetoOrdem;

                                        }


                                        if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                        {

                                            QuantidadeInstrumento += vencimento.QtDisponivel;
                                            Instrumento = vencimento.Instrumento;
                                            stUtilizacaoInstrumento = 'S';
                                        }
                                    }
                                }
                            }
                        }

                        if ((QuantidadeContrato == 0) && (QuantidadeInstrumento == 0))
                        {
                            logger.Info("LIMITE CONTRATO ..........:" + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........:" + QuantidadeInstrumento.ToString());

                            logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                            ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }

                        int QuantidadeSolicitada = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                        // LIMITE SOMENTE PARA O CONTRATO
                        if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                        {
                            logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                            logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                            if (QuantidadeSolicitada > QuantidadeInstrumento)
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                        }

                        // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                        if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                        {
                            logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                            logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                            if (stUtilizacaoInstrumento == 'S')
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                //QUANTIDADE INSUFICIENTE
                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                            if (QuantidadeSolicitada > QuantidadeContrato)
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                //QUANTIDADE INSUFICIENTE
                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }
                        }
                    }
                    else
                    {
                        logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                        ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Cliente não possui limite para operar este contrato.Por favor, entre em contato com o nosso atendimento.", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }


                    #endregion


                    logger.Info("SALVAR ORDEM NO BANCO DE DADOS.");
                    this.InserirOrdemCliente(pParametroOrdemResquest);
                    logger.Info("ORDEM SALVA COM SUCESSO.");

                    #region ENTRY POINT

                    //  pParametroOrdemResquest.ClienteOrdemInfo.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString().Remove(pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString().Length - 1, 1).DBToInt32();

                    logger.Info("ENVIA ORDEM PARA O ROTEADOR.");
                    //*************************** SESSÃO RESPONSAVEL POR ENVIAR UMA NOVA ORDEM PARA O MERCADO DE BMF
                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteadorBMF(pParametroOrdemResquest.ClienteOrdemInfo);

                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("ORDEM ENVIADA COM SUCESSO.");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                        return ObjetoOrdem;
                    }
                    else
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                        {

                            logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR.");
                            logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                            return ObjetoOrdem;
                        }

                    #endregion
                }

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "ORDEM ENVIADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                return ObjetoOrdem;

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ENVIAR A OFERTA DE COMPRA NO MERCADO DE BMF.", ex);

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "OCORREU UM ERRO AO ENVIAR A OFERTA DE COMPRA NO MERCADO DE BMF", CriticaRiscoEnum.Exception);
                return ObjetoOrdem;
            }


            return ObjetoOrdem;
        }

        #region ENVIO DE ORDEM BMF

        /// <summary>
        /// METODO RESPONSAVEL POR ENVIAR UMA ORDEM DE COMPRA PARA O MERCADO DE BMF
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns></returns>
        private EnviarOrdemResponse EnviarOrdemCompraBMF(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            SecurityIDRequest SecurityIDRequest = new Lib.SecurityIDRequest();


            logger.Info("OBTEM O CODIGO DE BMF DO CLIENTE ATRAVES DO CODIGO BOVESPA CADASTRADO.");

            string CodigoBMF = new PersistenciaOrdens().ObterCodigoBMF(pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

            logger.Info("CODIGO BOVESPA : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
            logger.Info("CODIGO BMF : " + CodigoBMF.ToString());

            pParametroOrdemResquest.ClienteOrdemInfo.Account = int.Parse(CodigoBMF);

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {
                /**************************************************************************************************/
                // CONFIGURA AS INFORMACOES BASICAS PARA O ENVIO DE UMA OFERTA DE BMF (Exchange,ChannelID)
                /**************************************************************************************************/

                logger.Info("INICIA ENVIO DE ORDEM DE COMPRA NO MERCADO DE BMF");
                logger.Info("CLIENTE........: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                SecurityIDRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                SecurityIDResponse SecurityIDResponse = ObjetoPersistenciaOrdens.ObterSecurityID(SecurityIDRequest);

                pParametroOrdemResquest.ClienteOrdemInfo.Exchange = "BMF";
                pParametroOrdemResquest.ClienteOrdemInfo.ChannelID = 0;

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    logger.Info("INSTRUMENTO..............: " + SecurityIDResponse.Instrumento);
                    logger.Info("SECURITYID ..............: " + SecurityIDResponse.SecurityID);
                    logger.Info("QUANTIDADE ..............: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());

                    logger.Info("TIPO DE OPERACAO .................: ALTERACAO DE ORDENS");

                    #region [ALTERACAO DE ORDEM]

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;

                    logger.Info("OBTEM INFORMACOES SOBRE A ORDEM ORIGINAL. ORIGCLORDID: " + pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID);

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    logger.Info("INFORMACOES OBTIDAS COM SUCESSO.");

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);
                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                    logger.Info("VOLUME ORIGINAL ......................: " + VOLUMEORIGINAL.ToString());
                    logger.Info("VOLUME SOLICITADO ....................: " + VOLUMEMODIFICACAO.ToString());
                    logger.Info("DIFERENCIAL ..........................: " + DIFERENCIAL.ToString());

                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);
                    ExecutarModificacaoOrdensRequest.info.Exchange = "BMF";
                    ExecutarModificacaoOrdensRequest.info.ChannelID = 0;

                    // VERIFICA SE A ORDEM E DIFERENTE DA ORIGINAL
                    if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    {
                        logger.Info("ORDEM IDENTICA A ORIGINAL");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM IDENTICA A ORIGINAL", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }
                    else
                    {
                        #region Controle de Pre-Risco BMF

                        AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                        AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                        int QuantidadeContrato = 0;
                        int QuantidadeInstrumento = 0;
                        int idClienteParametroBMF = 0;
                        char stUtilizacaoInstrumento = 'N';

                        logger.Info("EFETUA O CONTROLE DE PRE RISCO NO MERCADO DE BMF");

                        logger.Info("OBTEM OS LIMITES PARA O MERCADO DE BMF");
                        ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();
                        requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;

                        if (pParametroOrdemResquest.IsContaMaster)
                        {
                            requestListaLimite.Account = pParametroOrdemResquest.ContaMaster;
                        }
                        else
                        {
                            requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                        }

                        ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                        logger.Info("LIMITES OBTIDOS COM SUCESSO.");

                        string Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                        string Contrato = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Substring(0, 3);

                        //CONTRATO BASE
                        var PosicaoContrato = from p in response.ListaLimites
                                              where p.Contrato == Contrato
                                              && p.Sentido == "C"
                                              select p;

                        if (PosicaoContrato.Count() > 0)
                        {
                            foreach (var item in PosicaoContrato)
                            {
                                idClienteParametroBMF = item.idClienteParametroBMF;
                                QuantidadeContrato = item.QuantidadeDisponivel;

                                //CONTRATO PAI
                                if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > item.QuantidadeMaximaOferta)
                                {
                                    logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + item.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + item.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;

                                }

                                if (response.ListaLimitesInstrumentos.Count > 0)
                                {
                                    // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                    foreach (var vencimento in response.ListaLimitesInstrumentos)
                                    {

                                        if (vencimento.Instrumento == Instrumento)
                                        {
                                            if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > vencimento.QuantidadeMaximaOferta)
                                            {
                                                logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + vencimento.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + vencimento.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                                return ObjetoOrdem;

                                            }

                                            if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                            {

                                                QuantidadeInstrumento += vencimento.QtDisponivel;
                                                Instrumento = vencimento.Instrumento;
                                                stUtilizacaoInstrumento = 'S';
                                            }
                                        }
                                    }
                                }
                            }


                            int QuantidadeSolicitada = 0;

                            int QuantidadeResultanteOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.OrderQty;

                            // QUANTIDADE ORIGINAL  > QUANTIDADE SOLICITADA
                            if (InformacoesOrdem.OrdemInfo.OrderQty > pParametroOrdemResquest.ClienteOrdemInfo.OrderQty)
                            {
                                #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                                logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL NO BANCO DE DADOS.");
                                this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                                logger.Info("BACKUP REALIZADO COM SUCESSO.");

                                EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                                EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                                logger.Info("GRAVA A ORDEM NO BANCO DE DADOS.");
                                this.InserirOrdemCliente(EnviarOrdemRiscoRequest);
                                logger.Info("ORDEM SALVA COM SUCESSO.");

                                #endregion

                                #region ENVIA A ORDEM PARA A BOLSA

                                logger.Info("ENVIA A ORDEM PARA A BOLSA");
                                ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteadorBMF(ExecutarModificacaoOrdensRequest.info);

                                if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                {
                                    logger.Info("ORDEM ENVIADA COM SUCESSO.");
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                    return ObjetoOrdem;

                                }
                                else
                                {
                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA A BOLSA.");
                                    logger.Info("DESCRICAO: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                }

                                #endregion

                            }   // QUANTIDADE ORIGINAL < QUANTIDADE SOLICITADA
                            else if (InformacoesOrdem.OrdemInfo.OrderQty < pParametroOrdemResquest.ClienteOrdemInfo.OrderQty)
                            {
                                QuantidadeSolicitada = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.OrderQty;
                            }


                            // LIMITE SOMENTE PARA O CONTRATO
                            if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                            {
                                logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                                logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                                if (QuantidadeSolicitada > QuantidadeInstrumento)
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                            }

                            // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                            if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                            {
                                logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                                logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                                if (stUtilizacaoInstrumento == 'S')
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    //QUANTIDADE INSUFICIENTE
                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                                if (QuantidadeSolicitada > QuantidadeContrato)
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    //QUANTIDADE INSUFICIENTE
                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }
                            }
                        }
                        else
                        {
                            logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                            ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Cliente não possui limite para operar este contrato.Por favor, entre em contato com o nosso atendimento.", CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }


                        #endregion

                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL NO BANCO DE DADOS.");
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                        logger.Info("BACKUP REALIZADO COM SUCESSO.");

                        EnviarOrdemRiscoRequest _EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        _EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        logger.Info("GRAVA A ORDEM NO BANCO DE DADOS.");
                        this.InserirOrdemCliente(_EnviarOrdemRiscoRequest);
                        logger.Info("ORDEM SALVA COM SUCESSO.");

                        #endregion

                        #region ENVIA A ORDEM PARA A BOLSA

                        logger.Info("ENVIA A ORDEM PARA A BOLSA");
                        ExecutarModificacaoOrdensResponse _respostaAutenticacao = this.ModificarOrdemRoteadorBMF(ExecutarModificacaoOrdensRequest.info);

                        if (_respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("ORDEM ENVIADA COM SUCESSO.");
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;

                        }
                        else
                        {
                            logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA A BOLSA.");
                            logger.Info("DESCRICAO: " + _respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {

                    #region Controle de Pre-Risco BMF

                    AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                    AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                    int QuantidadeContrato = 0;
                    int QuantidadeInstrumento = 0;
                    int idClienteParametroBMF = 0;
                    char stUtilizacaoInstrumento = 'N';

                    logger.Info("EFETUA O CONTROLE DE PRE RISCO NO MERCADO DE BMF");

                    logger.Info("OBTEM OS LIMITES PARA O MERCADO DE BMF");
                    ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();

                    if (pParametroOrdemResquest.IsContaMaster)
                    {
                        requestListaLimite.Account = pParametroOrdemResquest.ContaMaster;
                    }
                    else
                    {
                        requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                    }

                    ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                    logger.Info("LIMITES OBTIDOS COM SUCESSO.");

                    string Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                    string Contrato = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Substring(0, 3);

                    //CONTRATO BASE
                    var PosicaoContrato = from p in response.ListaLimites
                                          where p.Contrato == Contrato
                                          && p.Sentido == "C"
                                          select p;

                    if (PosicaoContrato.Count() > 0)
                    {
                        foreach (var item in PosicaoContrato)
                        {
                            idClienteParametroBMF = item.idClienteParametroBMF;
                            QuantidadeContrato = item.QuantidadeDisponivel;

                            //CONTRATO PAI
                            if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > item.QuantidadeMaximaOferta)
                            {
                                logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + item.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + item.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;

                            }

                            if (response.ListaLimitesInstrumentos.Count > 0)
                            {

                                // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                foreach (var vencimento in response.ListaLimitesInstrumentos)
                                {
                                    if (vencimento.Instrumento == Instrumento)
                                    {
                                        if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > vencimento.QuantidadeMaximaOferta)
                                        {
                                            logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + vencimento.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + vencimento.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                            return ObjetoOrdem;

                                        }


                                        if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                        {

                                            QuantidadeInstrumento += vencimento.QtDisponivel;
                                            Instrumento = vencimento.Instrumento;
                                            stUtilizacaoInstrumento = 'S';
                                        }
                                    }
                                }
                            }
                        }

                        if ((QuantidadeContrato == 0) && (QuantidadeInstrumento == 0))
                        {
                            logger.Info("LIMITE CONTRATO ..........:" + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........:" + QuantidadeInstrumento.ToString());

                            logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                            ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }

                        int QuantidadeSolicitada = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                        // LIMITE SOMENTE PARA O CONTRATO
                        if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                        {
                            logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                            logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                            if (QuantidadeSolicitada > QuantidadeInstrumento)
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                        }

                        // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                        if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                        {
                            logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                            logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                            if (stUtilizacaoInstrumento == 'S')
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                //QUANTIDADE INSUFICIENTE
                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                            if (QuantidadeSolicitada > QuantidadeContrato)
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                //QUANTIDADE INSUFICIENTE
                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }
                        }
                    }
                    else
                    {
                        logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                        ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Cliente não possui limite para operar este contrato.Por favor, entre em contato com o nosso atendimento.", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }


                    #endregion


                    logger.Info("SALVAR ORDEM NO BANCO DE DADOS.");
                    this.InserirOrdemCliente(pParametroOrdemResquest);
                    logger.Info("ORDEM SALVA COM SUCESSO.");

                    #region ENTRY POINT

                    //  pParametroOrdemResquest.ClienteOrdemInfo.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString().Remove(pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString().Length - 1, 1).DBToInt32();

                    logger.Info("ENVIA ORDEM PARA O ROTEADOR.");
                    //*************************** SESSÃO RESPONSAVEL POR ENVIAR UMA NOVA ORDEM PARA O MERCADO DE BMF
                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteadorBMF(pParametroOrdemResquest.ClienteOrdemInfo);

                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("ORDEM ENVIADA COM SUCESSO.");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                        return ObjetoOrdem;
                    }
                    else
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                        {

                            logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR.");
                            logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                            return ObjetoOrdem;
                        }

                    #endregion
                }

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "ORDEM ENVIADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                return ObjetoOrdem;

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ENVIAR A OFERTA DE COMPRA NO MERCADO DE BMF.", ex);

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "OCORREU UM ERRO AO ENVIAR A OFERTA DE COMPRA NO MERCADO DE BMF", CriticaRiscoEnum.Exception);
                return ObjetoOrdem;
            }


            return ObjetoOrdem;
        }

        /// <summary>
        /// METODO RESPONSAVEL POR ENVIAR UMA ORDEM DE VENDA PARA O MERCADO DE BMF
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns></returns>
        private EnviarOrdemResponse EnviarOrdemVendaBMF(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            SecurityIDRequest SecurityIDRequest = new Lib.SecurityIDRequest();


            logger.Info("OBTEM O CODIGO DE BMF DO CLIENTE ATRAVES DO CODIGO BOVESPA CADASTRADO.");

            string CodigoBMF = new PersistenciaOrdens().ObterCodigoBMF(pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

            logger.Info("CODIGO BOVESPA : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
            logger.Info("CODIGO BMF : " + CodigoBMF.ToString());

            pParametroOrdemResquest.ClienteOrdemInfo.Account = int.Parse(CodigoBMF);

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {
                /**************************************************************************************************/
                // CONFIGURA AS INFORMACOES BASICAS PARA O ENVIO DE UMA OFERTA DE BMF (Exchange,ChannelID)
                /**************************************************************************************************/

                logger.Info("INICIA ENVIO DE ORDEM DE COMPRA NO MERCADO DE BMF");
                logger.Info("CLIENTE........: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                SecurityIDRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                SecurityIDResponse SecurityIDResponse = ObjetoPersistenciaOrdens.ObterSecurityID(SecurityIDRequest);

                pParametroOrdemResquest.ClienteOrdemInfo.Exchange = "BMF";
                pParametroOrdemResquest.ClienteOrdemInfo.ChannelID = 0;

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    logger.Info("INSTRUMENTO..............: " + SecurityIDResponse.Instrumento);
                    logger.Info("SECURITYID ..............: " + SecurityIDResponse.SecurityID);
                    logger.Info("QUANTIDADE ..............: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());

                    logger.Info("TIPO DE OPERACAO .................: ALTERACAO DE ORDENS");

                    #region [ALTERACAO DE ORDEM]

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;

                    logger.Info("OBTEM INFORMACOES SOBRE A ORDEM ORIGINAL. ORIGCLORDID: " + pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID);

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    logger.Info("INFORMACOES OBTIDAS COM SUCESSO.");

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);
                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                    logger.Info("VOLUME ORIGINAL ......................: " + VOLUMEORIGINAL.ToString());
                    logger.Info("VOLUME SOLICITADO ....................: " + VOLUMEMODIFICACAO.ToString());
                    logger.Info("DIFERENCIAL ..........................: " + DIFERENCIAL.ToString());

                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);
                    ExecutarModificacaoOrdensRequest.info.Exchange = "BMF";
                    ExecutarModificacaoOrdensRequest.info.ChannelID = 0;

                    // VERIFICA SE A ORDEM E DIFERENTE DA ORIGINAL
                    if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    {
                        logger.Info("ORDEM IDENTICA A ORIGINAL");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM IDENTICA A ORIGINAL", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }
                    else
                    {
                        #region Controle de Pre-Risco BMF

                        AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                        AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                        int QuantidadeContrato = 0;
                        int QuantidadeInstrumento = 0;
                        int idClienteParametroBMF = 0;
                        char stUtilizacaoInstrumento = 'N';

                        logger.Info("EFETUA O CONTROLE DE PRE RISCO NO MERCADO DE BMF");

                        logger.Info("OBTEM OS LIMITES PARA O MERCADO DE BMF");
                        ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();

                        if (pParametroOrdemResquest.IsContaMaster)
                        {
                            requestListaLimite.Account = pParametroOrdemResquest.ContaMaster;
                        }
                        else
                        {
                            requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                        }

                        ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                        logger.Info("LIMITES OBTIDOS COM SUCESSO.");

                        string Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                        string Contrato = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Substring(0, 3);

                        //CONTRATO BASE
                        var PosicaoContrato = from p in response.ListaLimites
                                              where p.Contrato == Contrato
                                              && p.Sentido == "V"
                                              select p;

                        if (PosicaoContrato.Count() > 0)
                        {
                            foreach (var item in PosicaoContrato)
                            {
                                idClienteParametroBMF = item.idClienteParametroBMF;
                                QuantidadeContrato = item.QuantidadeDisponivel;

                                //CONTRATO PAI
                                if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > item.QuantidadeMaximaOferta)
                                {
                                    logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + item.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + item.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;

                                }

                                if (response.ListaLimitesInstrumentos.Count > 0)
                                {
                                    // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                    foreach (var vencimento in response.ListaLimitesInstrumentos)
                                    {
                                        if (vencimento.Instrumento == Instrumento)
                                        {
                                            if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > vencimento.QuantidadeMaximaOferta)
                                            {
                                                logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + vencimento.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + vencimento.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                                return ObjetoOrdem;

                                            }


                                            if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                            {

                                                QuantidadeInstrumento += vencimento.QtDisponivel;
                                                Instrumento = vencimento.Instrumento;
                                                stUtilizacaoInstrumento = 'S';
                                            }
                                        }
                                    }
                                }
                            }

                            if ((QuantidadeContrato == 0) && (QuantidadeInstrumento == 0))
                            {
                                logger.Info("LIMITE CONTRATO ..........:" + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........:" + QuantidadeInstrumento.ToString());

                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                            int QuantidadeSolicitada = 0;

                            int QuantidadeResultanteOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.OrderQty;

                            // QUANTIDADE ORIGINAL  > QUANTIDADE SOLICITADA
                            if (InformacoesOrdem.OrdemInfo.OrderQty > pParametroOrdemResquest.ClienteOrdemInfo.OrderQty)
                            {
                                #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                                logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL NO BANCO DE DADOS.");
                                this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                                logger.Info("BACKUP REALIZADO COM SUCESSO.");

                                EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                                EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                                logger.Info("GRAVA A ORDEM NO BANCO DE DADOS.");
                                this.InserirOrdemCliente(EnviarOrdemRiscoRequest);
                                logger.Info("ORDEM SALVA COM SUCESSO.");

                                #endregion

                                #region ENVIA A ORDEM PARA A BOLSA

                                logger.Info("ENVIA A ORDEM PARA A BOLSA");
                                ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteadorBMF(ExecutarModificacaoOrdensRequest.info);

                                if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                                {
                                    logger.Info("ORDEM ENVIADA COM SUCESSO.");
                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                    return ObjetoOrdem;

                                }
                                else
                                {
                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA A BOLSA.");
                                    logger.Info("DESCRICAO: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                }

                                #endregion

                            }   // QUANTIDADE ORIGINAL < QUANTIDADE SOLICITADA
                            else if (InformacoesOrdem.OrdemInfo.OrderQty < pParametroOrdemResquest.ClienteOrdemInfo.OrderQty)
                            {
                                QuantidadeSolicitada = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty - InformacoesOrdem.OrdemInfo.OrderQty;
                            }


                            // LIMITE SOMENTE PARA O CONTRATO
                            if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                            {
                                logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                                logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                                if (QuantidadeSolicitada > QuantidadeInstrumento)
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                            }

                            // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                            if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                            {
                                logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                                logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                                logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                                if (stUtilizacaoInstrumento == 'S')
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    //QUANTIDADE INSUFICIENTE
                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }

                                if (QuantidadeSolicitada > QuantidadeContrato)
                                {
                                    logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                    //QUANTIDADE INSUFICIENTE
                                    ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;
                                }
                            }
                        }
                        else
                        {
                            logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                            ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Cliente não possui limite para operar este contrato.Por favor, entre em contato com o nosso atendimento.", CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }


                        #endregion

                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL NO BANCO DE DADOS.");
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                        logger.Info("BACKUP REALIZADO COM SUCESSO.");

                        EnviarOrdemRiscoRequest _EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        _EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        logger.Info("GRAVA A ORDEM NO BANCO DE DADOS.");
                        this.InserirOrdemCliente(_EnviarOrdemRiscoRequest);
                        logger.Info("ORDEM SALVA COM SUCESSO.");

                        #endregion

                        #region ENVIA A ORDEM PARA A BOLSA

                        logger.Info("ENVIA A ORDEM PARA A BOLSA");
                        ExecutarModificacaoOrdensResponse _respostaAutenticacao = this.ModificarOrdemRoteadorBMF(ExecutarModificacaoOrdensRequest.info);

                        if (_respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("ORDEM ENVIADA COM SUCESSO.");
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;

                        }
                        else
                        {
                            logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA A BOLSA.");
                            logger.Info("DESCRICAO: " + _respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {

                    #region Controle de Pre-Risco BMF

                    AtualizarLimitesBMFRequest requestBMF = new AtualizarLimitesBMFRequest();
                    AtualizarLimitesBMFResponse responseBMF = new AtualizarLimitesBMFResponse();

                    int QuantidadeContrato = 0;
                    int QuantidadeInstrumento = 0;
                    int idClienteParametroBMF = 0;
                    char stUtilizacaoInstrumento = 'N';

                    logger.Info("EFETUA O CONTROLE DE PRE RISCO NO MERCADO DE BMF");

                    logger.Info("OBTEM OS LIMITES PARA O MERCADO DE BMF");
                    ListarLimiteBMFRequest requestListaLimite = new ListarLimiteBMFRequest();

                    if (pParametroOrdemResquest.IsContaMaster)
                    {
                        requestListaLimite.Account = pParametroOrdemResquest.ContaMaster;
                    }
                    else
                    {
                        requestListaLimite.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                    }

                    ListarLimiteBMFResponse response = new ServicoLimiteBMF().ObterLimiteBMFCliente(requestListaLimite);

                    logger.Info("LIMITES OBTIDOS COM SUCESSO.");

                    string Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol;
                    string Contrato = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Substring(0, 3);

                    //CONTRATO BASE
                    var PosicaoContrato = from p in response.ListaLimites
                                          where p.Contrato == Contrato
                                          && p.Sentido == "V"
                                          select p;

                    if (PosicaoContrato.Count() > 0)
                    {
                        foreach (var item in PosicaoContrato)
                        {
                            //CONTRATO PAI
                            if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > item.QuantidadeMaximaOferta)
                            {
                                logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + item.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + item.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;

                            }

                            idClienteParametroBMF = item.idClienteParametroBMF;
                            QuantidadeContrato = item.QuantidadeDisponivel;

                            if (response.ListaLimitesInstrumentos.Count > 0)
                            {
                                // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                                foreach (var vencimento in response.ListaLimitesInstrumentos)
                                {
                                    if (vencimento.Instrumento == Instrumento)
                                    {
                                        if (pParametroOrdemResquest.ClienteOrdemInfo.OrderQty > vencimento.QuantidadeMaximaOferta)
                                        {
                                            logger.Info("A QUANTIDADE MAXIMA CONFIGURADA POR OFERTA PARA ESTE INSTRUMENTO É DE : " + vencimento.QuantidadeMaximaOferta.ToString() + ". NÃO FOI POSSIVEL CONCLUIR O ENVIO DE OFERTA.");
                                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A QUANTIDADE DA OFERTA ENVIADA ULTRAPASSA O LIMITE MAXIMO POR BOLETA CONFIGURADO. TOTAL CONFIGURADO: " + vencimento.QuantidadeMaximaOferta.ToString() + " .", CriticaRiscoEnum.ErroNegocio);
                                            return ObjetoOrdem;

                                        }


                                        if (vencimento.IdClienteParametroBMF == item.idClienteParametroBMF)
                                        {

                                            QuantidadeInstrumento += vencimento.QtDisponivel;
                                            Instrumento = vencimento.Instrumento;
                                            stUtilizacaoInstrumento = 'S';
                                        }
                                    }
                                }
                            }
                        }

                        if ((QuantidadeContrato == 0) && (QuantidadeInstrumento == 0))
                        {
                            logger.Info("LIMITE CONTRATO ..........:" + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........:" + QuantidadeInstrumento.ToString());

                            logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                            ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }

                        int QuantidadeSolicitada = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                        // LIMITE SOMENTE PARA O CONTRATO
                        if ((QuantidadeContrato > 0) && (QuantidadeInstrumento > 0))
                        {
                            logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                            logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                            if (QuantidadeSolicitada > QuantidadeInstrumento)
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                        }

                        // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                        if ((QuantidadeContrato > 0) && (QuantidadeInstrumento == 0))
                        {
                            logger.Info("LIMITE CONTRATO .............: " + QuantidadeContrato.ToString());
                            logger.Info("LIMITE INSTRUMENTO ..........: " + QuantidadeInstrumento.ToString());
                            logger.Info("QUANTIDADE SOLICITADA .......: " + QuantidadeSolicitada.ToString());

                            if (stUtilizacaoInstrumento == 'S')
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                //QUANTIDADE INSUFICIENTE
                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                            if (QuantidadeSolicitada > QuantidadeContrato)
                            {
                                logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                                //QUANTIDADE INSUFICIENTE
                                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Limite operacional insuficiente para operar o contrato.", CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }
                        }
                    }
                    else
                    {
                        logger.Info("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");

                        ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "Cliente não possui limite para operar este contrato.Por favor, entre em contato com o nosso atendimento.", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }


                    #endregion


                    logger.Info("SALVAR ORDEM NO BANCO DE DADOS.");
                    this.InserirOrdemCliente(pParametroOrdemResquest);
                    logger.Info("ORDEM SALVA COM SUCESSO.");

                    #region ENTRY POINT

                    //  pParametroOrdemResquest.ClienteOrdemInfo.Account = pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString().Remove(pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString().Length - 1, 1).DBToInt32();

                    logger.Info("ENVIA ORDEM PARA O ROTEADOR.");
                    //*************************** SESSÃO RESPONSAVEL POR ENVIAR UMA NOVA ORDEM PARA O MERCADO DE BMF
                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteadorBMF(pParametroOrdemResquest.ClienteOrdemInfo);

                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("ORDEM ENVIADA COM SUCESSO.");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                        return ObjetoOrdem;
                    }
                    else
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                        {

                            logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR.");
                            logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                            return ObjetoOrdem;
                        }

                    #endregion
                }

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "ORDEM ENVIADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                return ObjetoOrdem;

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ENVIAR A OFERTA DE COMPRA NO MERCADO DE BMF.", ex);

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "OCORREU UM ERRO AO ENVIAR A OFERTA DE COMPRA NO MERCADO DE BMF", CriticaRiscoEnum.Exception);
                return ObjetoOrdem;
            }


            return ObjetoOrdem;
        }

        #endregion

        #endregion

        #region ENVIO DE ORDEM BOVESPA

        /// <summary>
        /// Método responsável por inserir uma nova oferta de compra no mercado a vista.
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns>EnviarOrdemResponse</returns>
        private EnviarOrdemResponse EnviarOrdemCompraAVista(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();

            decimal VOLUMEORDEM = 0;
            decimal LIMITEOPERACIONAL = 0;

            int QUANTIDADEINTEGRAL = 0;
            int QUANTIDADEFRACIONARIO = 0;

            int CODIGOPARAMETROCLIENTECOMPRA = 0;
            int CODIGOPARAMETROCLIENTEVENDA = 0;

            #region Validacao Ordem a MERCADO

            logger.Info("VERIFICA SE A ORDEM ENVIADA E LIMITADA OU A MERCADO.");
            if (pParametroOrdemResquest.ClienteOrdemInfo.OrdType == OrdemTipoEnum.Mercado)
            {
                logger.Info("ORDEM A MERCADO.");
            }

            #endregion

            /* METODO RESPONSAVEL POR PROCESSAR A SOLICITACAO DE ENVIO DE ORDENS NO MERCADO A VISTA
             * PRIMEIRAMENTE O METODO PASSA PELA VALIDACAO DE RISCO E POSTERIOR ENVIA A ORDEM PARA A BOLSA.
             * */
            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {

                #region LIMITES OPERACIONAIS

                logger.Info("INICIALIZA ROTINA RESPONSAVEL POR CARREGAR OS LIMITES OPERACIONAIS. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                //**************************************************************************************************************
                //     TRECHO RESPONSAVEL POR CARREGAR OS LIMITES OPERACIOANIS DO CLIENTE.
                //**************************************************************************************************************
                LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();

                if (pParametroOrdemResquest.IsContaMaster)
                {
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ContaMaster;
                }
                else
                {
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }

                ObjetoLimitesOperacionais = ObjetoPersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);     

                logger.Info("LIMITES CARREGADOS COM SUCESSO.");

                #endregion

                #region TIPO MERCADO ( INTEGRAL / FRACIONARIO / AMBOS)

                logger.Info("EFETUA VALIDACAO O TIPO DE MERCADO");

                int QUANTIDADEORDEM = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;
                int LOTEPADRAO = pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                int MODULOLOTENEGOCIACAO = (QUANTIDADEORDEM % LOTEPADRAO);

                if (QUANTIDADEORDEM >= LOTEPADRAO)
                {
                    pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.INTEGRAL;
                    QUANTIDADEINTEGRAL = (QUANTIDADEORDEM - MODULOLOTENEGOCIACAO);

                    if (MODULOLOTENEGOCIACAO > 0)
                    {
                        pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.INTEGRALFRACIONARIO;
                        QUANTIDADEFRACIONARIO = MODULOLOTENEGOCIACAO;
                    }
                }
                else
                {
                    pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.FRACIONARIO;
                    QUANTIDADEFRACIONARIO = QUANTIDADEORDEM;
                    QUANTIDADEINTEGRAL = 0;
                }

                logger.Info("VALIDACAO DE TIPO DE MERCADO EFETUADA COM SUCESSO");

                #endregion

                #region INCLUSAO / ALTERACAO DE OFERTA

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    //**************************************************************************************************************
                    //    ALTERACAO DE UMA NOVA ORDEM. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************     

                    logger.Info("SOLICITAÇÃO DE ORDEM TIPO................ ALTERACAO");

                    logger.Info("CLIENTE.......................: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                    logger.Info("INSTRUMENTO...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
                    logger.Info("QUANTIDADE ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ........................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());
                    logger.Info("STOPSTART PRICE ..............: " + pParametroOrdemResquest.ClienteOrdemInfo.StopPrice.ToString());


                    #region [ALTERACAO DE ORDENS]

                    #region OBTER INFORMACOES SOBRE A ORDEM ORIGINAL

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;

                    logger.Info("OBTEM INFORMACOES REFERENTES A ORDEM ORIGINAL. CLORDID ORDEM ORIGINAL: " + pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID);

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    logger.Info("INFORMACOES CARREGADAS COM SUCESSO.");

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);
                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                    logger.Info("VOLUME ORIGINAL ..................... " + VOLUMEORIGINAL.ToString());
                    logger.Info("VOLUME SOLICITADO ................... " + VOLUMEMODIFICACAO.ToString());
                    logger.Info("DIFERENCIAL ......................... " + DIFERENCIAL.ToString());

                    logger.Info("INICIALIZA O PARSEAMENTO DA ORDEM.");
                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);
                    ExecutarModificacaoOrdensRequest.info.Symbol = InformacoesOrdem.OrdemInfo.Symbol;

                    // VERIFICA SE A ORDEM E DIFERENTE DA ORIGINAL
                    //if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    //{
                    //    logger.Info("NÃO EXISTE ALTERAÇÕES A SEREM EFETUADAS - ORDEM IDENTICA A ORIGINAL.");
                    //    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "NÃO EXISTE ALTERAÇÕES A SEREM EFETUADAS - ORDEM IDENTICA A ORIGINAL.", CriticaRiscoEnum.ErroNegocio);
                    //    return ObjetoOrdem;
                    //}

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************               

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    logger.Info("INICIALIZA O SALDO DISPONIVEL DO CLIENTE NO PARA COMPRA NO MERCADO A VISTA. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {
                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;

                            logger.Info("******** LIMITE DISPONIVEL ********");
                            logger.Info("CLIENTE: ............ " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("LIMITE DISPONIVEL ... " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        }
                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA");

                        logger.Info("******** LIMITE DISPONIVEL ********");
                        logger.Info("CLIENTE: ............ " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        logger.Info("LIMITE DISPONIVEL ... 0,00");

                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #endregion

                    #region VALIDACAO DE LOTE DE NEGOCIACAO

                    TipoMercadoEnum TipoMercadoEnumOrdemOriginal;

                    logger.Info("VALIDAO O LOTE DE NEGOCIACAO");

                    int QUANTIDADEORDEMORIGINAL = InformacoesOrdem.OrdemInfo.OrderQty;
                    int LOTEPADRAOORIGINAL = pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                    int MODULOLOTENEGOCIACAOORIGINAL = (QUANTIDADEORDEMORIGINAL % LOTEPADRAOORIGINAL);

                    if (QUANTIDADEORDEMORIGINAL >= LOTEPADRAOORIGINAL)
                    {
                        TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.INTEGRAL;
                        QUANTIDADEINTEGRAL = (QUANTIDADEORDEMORIGINAL - MODULOLOTENEGOCIACAOORIGINAL);

                        if (MODULOLOTENEGOCIACAOORIGINAL > 0)
                        {
                            TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.INTEGRALFRACIONARIO;
                            QUANTIDADEFRACIONARIO = MODULOLOTENEGOCIACAOORIGINAL;
                        }
                    }
                    else
                    {
                        TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.FRACIONARIO;
                        QUANTIDADEFRACIONARIO = QUANTIDADEORDEMORIGINAL;
                        QUANTIDADEINTEGRAL = 0;
                    }



                    #endregion

                    if (TipoMercadoEnumOrdemOriginal != pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado)
                    {
                        logger.Info("LOTE PADRAO INVALIDO PARA REALIZAR A ALTERACAO DA ORDEM");

                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "LOTE PADRÃO INVALIDO PARA REALIZAR A ALTERACAO DA ORDEM", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }

                    // CLIENTE UTILIZANDO MAIS LIMITE DO QUE A ORDEM ORIGINAL
                    if (DIFERENCIAL > 0)
                    {
                        //**************************************************************************************************************
                        // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                        //**************************************************************************************************************
                        double LIMITEOPERACIONALDISPONIVEL = double.Parse(LIMITEOPERACIONAL.ToString());

                        logger.Info("VALIDA SE O SALDO DISPONIVEL É SUFICIENTE PARA ENVIAR A OFERTA");
                        logger.Info("CLIENTE                :.........." + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        logger.Info("DIFERENCIAL SOLICITADO :.........." + DIFERENCIAL.ToString());
                        logger.Info("SALDO DISPONIVEL       :.........." + LIMITEOPERACIONALDISPONIVEL.ToString());

                        if (DIFERENCIAL <= LIMITEOPERACIONALDISPONIVEL)
                        {
                            #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                            logger.Info("REALIZA O BACKUP DA ORDEM ORINAL PARA CASOS DE REJEICAO E/OU INTERVALO DE BANDA ");

                            //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                            this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                            logger.Info("BACKUP EFETUADO COM SUCESSO.");

                            EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                            EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                            logger.Info("INSERI UMA NOVA ORDEM NO BANCO DE DADOS");

                            this.InserirOrdemCliente(EnviarOrdemRiscoRequest);

                            logger.Info("OFERTA INSERIDA COM SUCESSO.");

                            #endregion

                            logger.Info("ENVIA OFERTA PARA O ROTEADOR DE ORDENS. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                            if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ALTERADA COM SUCESSO.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                            {
                                logger.Info("OCORREU UM PROBLEMA AO ALTERARA ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }
                        }
                        else
                        {
                            logger.Info("SALDO INSUFICIENTE PARA ENVIAR A OFERTA");
                        }

                    }
                    else
                    {
                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                        EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        this.InserirOrdemCliente(EnviarOrdemRiscoRequest);

                        #endregion

                        // NÃO É NECESSARIO A CHECAGEM DE LIMITE.
                        ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                        if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;
                        }
                        else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                        {
                            logger.Info("OCORREU UM PROBLEMA AO ALTERARA ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }

                    }

                    #endregion

                }
                else
                {
                    //**************************************************************************************************************
                    //    INCLUSAO DE UMA NOVA OFERTA. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************

                    logger.Info("SOLICITAÇÃO DE ORDEM TIPO................ INCLUSAO");

                    logger.Info("CLIENTE.......................: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                    logger.Info("INSTRUMENTO...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
                    logger.Info("QUANTIDADE ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ........................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());
                    logger.Info("STOPSTART PRICE ..............: " + pParametroOrdemResquest.ClienteOrdemInfo.StopPrice.ToString());

                    #region INCLUSAO DE ORDEM

                    decimal PRECO = decimal.Parse(pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());
                    decimal QUANTIDADE = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                    #region VALIDACAO DE PRECO POR ORDEM ENVIADA

                    logger.Info("INICIALIZA O SERVICO DE COTACOES");

                    EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                    CotacaoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                    EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                    if (PRECO <= CotacaoResponse.CotacaoInfo.Ultima)
                    {
                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;
                    }

                    logger.Info("SERVICO DE COTACOES INICIALIZADO COM SUCESSO");

                    #endregion

                    VOLUMEORDEM = (PRECO * QUANTIDADE);

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************

                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    logger.Info("CARREGA O LIMITE DISPONIVEL PARA COMPRA DE ACOES. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;

                            logger.Info("SALDO DISPONIVEL ............: " + LIMITEOPERACIONAL.ToString());

                        }
                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    //**************************************************************************************************************
                    // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                    //**************************************************************************************************************

                    logger.Info("VALIDA SE EXISTE LIMITE DISPONIVEL PARA ENVIAR A OFERTA.");
                    if (VOLUMEORDEM <= LIMITEOPERACIONAL)
                    {
                        logger.Info("LIMITE DISPONIVEL PARA REALIZAR A OPERACAO");
                        //*********************** VALIDA SE EXISTE DIVISAO DE MERCADO FRACIONARIO/INTEGRAL ***************************/
                        if ((QUANTIDADEFRACIONARIO > 0) && (QUANTIDADEINTEGRAL > 0))
                        {
                            //********* EXECUCAO NOS DOIS MERCADOS INTEGRAL/FRACIONARIO ***********/

                            logger.Info("EXECUCAO EM DOIS MERCADOS. INTEGREAL E FRACIONARIO");
                            //PARTE INTEGRAL
                            logger.Info("QUANTIDADE FRACIONARIO ....................: " + QUANTIDADEFRACIONARIO.ToString());
                            logger.Info("QUANTIDADE INTEGRAL  ......................: " + QUANTIDADEINTEGRAL.ToString());

                            logger.Info("ENVIA A ORDEM NO MERCADO INTEGRAL. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                            //ALOCACAO DE LIMITES
                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEINTEGRAL;

                            logger.Info("GRAVANDO INFORMACOES NO BANCO DE DADOS [INTEGRAL]");
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("INFORMACOES SALVAS COM SUCESSO [INTEGRAL].");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O 
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM NO MERCADO INTEGRAL ENVIADA COM SUCESSO.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                // return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM NO MERCADO INTEGRAL PARA O ROTEADOR");
                                    logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion



                            logger.Info("GRAVANDO INFORMACOES NO BANCO DE DADOS [FRACIONARIO]");

                            //PARTE FRACIONARIO
                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEFRACIONARIO;
                            pParametroOrdemResquest.ClienteOrdemInfo.Symbol = ContextoOMS.ObterInstrumentoFracionario(pParametroOrdemResquest.ClienteOrdemInfo.Symbol);
                            pParametroOrdemResquest.ClienteOrdemInfo.ClOrdID = ContextoOMS.CtrlNumber;
                            pParametroOrdemResquest.ClienteOrdemInfo.Account = CodigoCliente;

                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("INFORMACOES SALVAS COM SUCESSO [FRACIONARIO].");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR *************/
                            RespostaRoteador = new ExecutarOrdemResponse();
                            RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                logger.Info("ORDEM NO MERCADO FRACIONARIO ENVIADA COM SUCESSO.");
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {
                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR DE ORDENS");
                                    logger.Info("DESCRICAO DO ERRO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                        }
                        else if ((QUANTIDADEFRACIONARIO > 0) && (QUANTIDADEINTEGRAL == 0))
                        {
                            // SOMENTE MERCADO FRACIONARIO

                            logger.Info("EXECUCAO NO MERCADO FRACIONARIO");
                            //PARTE INTEGRAL
                            logger.Info("QUANTIDADE FRACIONARIO ....................: " + QUANTIDADEFRACIONARIO.ToString());

                            logger.Info("GRAVANDO ORDEM NO BANCO DE DADOS");
                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEFRACIONARIO;
                            pParametroOrdemResquest.ClienteOrdemInfo.Symbol = ContextoOMS.ObterInstrumentoFracionario(pParametroOrdemResquest.ClienteOrdemInfo.Symbol);
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("ORDEM SALVA NO BANCO DE DADOS COM SUCESSO [ FRACIONARIO]");

                            #region ENTRY POINT

                            logger.Info("ENVIADO ORDEM PARA O ROTEADOR. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR *************/                  
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ENVIADA PARA O ROTEADOR COM SUCESSO");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR.");
                                    logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                        }
                        else if ((QUANTIDADEFRACIONARIO == 0) && (QUANTIDADEINTEGRAL > 0))
                        {
                            // SOMENTE MERCADO INTEGRAL

                            logger.Info("EXECUCAO NO MERCADO INTEGRAL");
                            //PARTE INTEGRAL
                            logger.Info("QUANTIDADE INTEGRAL ....................: " + QUANTIDADEINTEGRAL.ToString());

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEINTEGRAL;
                            logger.Info("GRAVANDO A ORDEM NO BANCO DE DADOS.");
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("ORDEM SALVA COM SUCESSO.");

                            #region ENTRY POINT

                            logger.Info("ENVIANDO A OFERTA PARA O ROTEADOR DE ORDENS.");

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O 
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ENVIADA COM SUCESSO PARA O ROTEADOR DE ORDENS.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;

                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {
                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR.");
                                    logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                }

                            #endregion

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                            return ObjetoOrdem;
                        }

                    }
                    else
                    {
                        logger.Info("CLIENTE NÃO POSSUI LIMITE OPERACIONAL PARA ENVIAR A OFERTA");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL SUFICIENTE PARA ENVIAR ESTA OFERTA.", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion
                }

                #endregion


            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ENVIAR A ORDEM DE COMPRA NO MERCADO A VISTA PARA O CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                logger.Error("DESCRICAO DO ERRO: " + ex.Message, ex);
            }

            return ObjetoOrdem;
        }        

        /// <summary>
        /// Método responsável por inserir uma nova oferta de compra no mercado a vista.
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns>EnviarOrdemResponse</returns>
        private EnviarOrdemResponse EnviarOrdemCompraFracionario(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();

            decimal VOLUMEORDEM = 0;
            decimal LIMITEOPERACIONAL = 0;

            int QUANTIDADEINTEGRAL = 0;
            int QUANTIDADEFRACIONARIO = 0;

            int CODIGOPARAMETROCLIENTECOMPRA = 0;
            int CODIGOPARAMETROCLIENTEVENDA = 0;

            /* METODO RESPONSAVEL POR PROCESSAR A SOLICITACAO DE ENVIO DE ORDENS NO MERCADO A VISTA
             * PRIMEIRAMENTE O METODO PASSA PELA VALIDACAO DE RISCO E POSTERIOR ENVIA A ORDEM PARA A BOLSA.
             * */

            logger.Info("PREPARA ENVIO DE ORDEM DE COMPRAS NO MERCADO FRACIONARIO.");
            logger.Info("CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {

                #region LIMITES OPERACIONAIS

                logger.Info("INICIALIZA LIMITES OPERACIONAIS PARA OPERAR NO MERCADO FRACIONARIO. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                //**************************************************************************************************************
                //     TRECHO RESPONSAVEL POR CARREGAR OS LIMITES OPERACIOANIS DO CLIENTE.
                //**************************************************************************************************************
                LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();
        

                if (pParametroOrdemResquest.IsContaMaster)
                {
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ContaMaster;
                }
                else
                {
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }

                ObjetoLimitesOperacionais = ObjetoPersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);


                logger.Info("LIMITES CARREGADOS COM SUCESSO.");
                #endregion

                #region TIPO MERCADO ( INTEGRAL / FRACIONARIO / AMBOS)

                int QUANTIDADEORDEM = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;
                int LOTEPADRAO = pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                int MODULOLOTENEGOCIACAO = (QUANTIDADEORDEM % LOTEPADRAO);

                pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.FRACIONARIO;
                QUANTIDADEFRACIONARIO = QUANTIDADEORDEM;
                QUANTIDADEINTEGRAL = 0;

                #endregion

                #region INCLUSAO / ALTERACAO DE OFERTA

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    //**************************************************************************************************************
                    //    ALTERACAO DE UMA NOVA ORDEM. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************     

                    logger.Info("SOLICITAÇÃO DE ORDEM TIPO................ ALTERACAO");

                    logger.Info("CLIENTE.......................: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                    logger.Info("INSTRUMENTO...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
                    logger.Info("QUANTIDADE ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ........................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());

                    #region [ALTERACAO DE ORDENS]

                    #region OBTER INFORMACOES SOBRE A ORDEM ORIGINAL

                    logger.Info("OBTEM INFORMACOES SOBRE A ORDEM ORGINAL OrigClOrdID: " + pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID.ToString());

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    logger.Info("INFORMACOES CARREGADAS COM SUCESSO.");

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);
                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                    logger.Info("VOLUME ORIGINAL: " + VOLUMEORIGINAL.ToString());
                    logger.Info("VOLUME SOLICITADO: " + VOLUMEMODIFICACAO.ToString());
                    logger.Info("DIFERENCIAL: " + DIFERENCIAL.ToString());


                    logger.Info("REALIZA O PARSEAMENTO DA OFERTA");
                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);

                    //// VERIFICA SE A ORDEM E DIFERENTE DA ORIGINAL
                    //if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    //{
                    //    logger.Info("OS DADOS DA ORDEM ORIGINAL SÃO IDENTICOS AOS DADOS DA ORDEM ALTERADA.");
                    //    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM IDENTICA A ORIGINAL", CriticaRiscoEnum.ErroNegocio);
                    //    return ObjetoOrdem;
                    //}

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************               

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    logger.Info("OBTEM LIMITES PARA O MERCADO A VISTA");

                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {
                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;

                            logger.Info("LIMITE OPERACIONAL");
                            logger.Info("CLIENTE.................................:" + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("LIMITE DISPONIVEL COMPRA A VISTA .......:" + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #endregion

                    logger.Info("VALIDA O LOTE DE NEGOCIAÇÃO");

                    #region VALIDACAO DE LOTE DE NEGOCIACAO

                    TipoMercadoEnum TipoMercadoEnumOrdemOriginal;

                    int QUANTIDADEORDEMORIGINAL = InformacoesOrdem.OrdemInfo.OrderQty;
                    int LOTEPADRAOORIGINAL = pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                    int MODULOLOTENEGOCIACAOORIGINAL = (QUANTIDADEORDEMORIGINAL % LOTEPADRAOORIGINAL);

                    if (QUANTIDADEORDEMORIGINAL >= LOTEPADRAOORIGINAL)
                    {
                        TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.INTEGRAL;
                        QUANTIDADEINTEGRAL = (QUANTIDADEORDEMORIGINAL - MODULOLOTENEGOCIACAOORIGINAL);

                        if (MODULOLOTENEGOCIACAOORIGINAL > 0)
                        {
                            TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.INTEGRALFRACIONARIO;
                            QUANTIDADEFRACIONARIO = MODULOLOTENEGOCIACAOORIGINAL;
                        }
                    }
                    else
                    {
                        TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.FRACIONARIO;
                        QUANTIDADEFRACIONARIO = QUANTIDADEORDEMORIGINAL;
                        QUANTIDADEINTEGRAL = 0;
                    }

                    #endregion


                    if (TipoMercadoEnumOrdemOriginal != pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado)
                    {
                        logger.Info("LOTE PADRÃO INVALIDO PARA REALIZAR A ALTERACAO DA ORDEM");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "LOTE PADRÃO INVALIDO PARA REALIZAR A ALTERACAO DA ORDEM", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }

                    logger.Info("LOTE DE NEGOCIACAO CARREGADO COM SUCESSO.");

                    ExecutarModificacaoOrdensRequest.info.Symbol += "F";

                    // CLIENTE UTILIZANDO MAIS LIMITE DO QUE A ORDEM ORIGINAL
                    if (DIFERENCIAL > 0)
                    {
                        //**************************************************************************************************************
                        // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                        //**************************************************************************************************************
                        double LIMITEOPERACIONALDISPONIVEL = double.Parse(LIMITEOPERACIONAL.ToString());


                        logger.Info("LIMITE OPERACIONAL DISPONIVEL : " + LIMITEOPERACIONALDISPONIVEL.ToString());
                        logger.Info("DIFERENCIAL: " + DIFERENCIAL.ToString());

                        if (DIFERENCIAL <= LIMITEOPERACIONALDISPONIVEL)
                        {

                            logger.Info("LIMITE SUFICIENTE PARA ENVIAR A ALTERAÇÃO DA OFERTA");


                            #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                            logger.Info("EFETUA BACKUP DA ORDEM ORIGINAL NO BANCO DE DADOS.");

                            //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                            this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                            logger.Info("BACKUP EFETUADO COM SUCESSO");

                            EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                            EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                            logger.Info("GRAVA A ORDEM NO BANCO DE DADOS");

                            this.InserirOrdemCliente(EnviarOrdemRiscoRequest);

                            logger.Info("ORDEM SALVA COM SUCESSO");

                            #endregion

                            logger.Info("INVOCA O SERVICO DE ENVIO DE ORDENS. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                            if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ALTERADA COM SUCESSO");
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                            {
                                logger.Info("OCORREU UM PROBLEMA AO ALTERAR A ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                        }

                    }
                    else
                    {
                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                        EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        this.InserirOrdemCliente(EnviarOrdemRiscoRequest);

                        #endregion

                        // NÃO É NECESSARIO A CHECAGEM DE LIMITE.
                        ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                        if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;
                        }

                    }

                    #endregion

                }
                else
                {
                    //**************************************************************************************************************
                    //    INCLUSAO DE UMA NOVA OFERTA. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************


                    logger.Info("SOLICITAÇÃO DE ORDEM TIPO................ INCLUSAO");

                    logger.Info("CLIENTE.......................: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                    logger.Info("INSTRUMENTO...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
                    logger.Info("QUANTIDADE ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ........................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());

                    #region INCLUSAO DE ORDEM

                    decimal PRECO = decimal.Parse(pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());
                    decimal QUANTIDADE = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                    #region VALIDACAO DE PRECO POR ORDEM ENVIADA

                    logger.Info("OBTEM INFORMACOES NO SERVICO DE COTAÇÃO SOBRE A O INSTRUMENTO A SER ENVIADO");

                    EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                    CotacaoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                    EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                    if (PRECO <= CotacaoResponse.CotacaoInfo.Ultima)
                    {
                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;
                        logger.Info("ALTERA O PRECO PARA A ULTIMA COTACAO DO MERCADO PRECO <= ULTIMA");
                    }

                    #endregion

                    VOLUMEORDEM = (PRECO * QUANTIDADE);

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************

                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    logger.Info("OBTEM LIMITES PARA O MERCADO A VISTA");

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;

                            logger.Info("LIMITES CARREGADOS COM SUCESSO.");
                            logger.Info("CLIENTE.............: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("LIMITE DISPONIVEL ..: " + LIMITEOPERACIONAL.ToString());
                        }
                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    //**************************************************************************************************************
                    // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                    //**************************************************************************************************************

                    logger.Info("VOLUME SOLICITADO ......: " + VOLUMEORDEM.ToString());
                    logger.Info("LIMITE OPERACIONAL......: " + LIMITEOPERACIONAL.ToString());

                    if (VOLUMEORDEM <= LIMITEOPERACIONAL)
                    {

                        //*********************** VALIDA SE EXISTE DIVISAO DE MERCADO FRACIONARIO/INTEGRAL ***************************/
                        if ((QUANTIDADEFRACIONARIO > 0) && (QUANTIDADEINTEGRAL > 0))
                        {
                            //********* EXECUCAO NOS DOIS MERCADOS INTEGRAL/FRACIONARIO ***********/

                            //PARTE INTEGRAL
                            logger.Info("Envia a ordem para o banco de dados");

                            //ALOCACAO DE LIMITES
                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEINTEGRAL;
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("Ordem enviada com sucesso");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O 
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ordem enviada para o roteador de ordens com sucesso.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                                    logger.Info("Descricao do erro: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                            //PARTE FRACIONARIO
                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEFRACIONARIO;
                            pParametroOrdemResquest.ClienteOrdemInfo.Symbol = ContextoOMS.ObterInstrumentoFracionario(pParametroOrdemResquest.ClienteOrdemInfo.Symbol);
                            pParametroOrdemResquest.ClienteOrdemInfo.ClOrdID = ContextoOMS.CtrlNumber;
                            pParametroOrdemResquest.ClienteOrdemInfo.Account = CodigoCliente;

                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("Ordem enviada com sucesso");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR *************/
                            RespostaRoteador = new ExecutarOrdemResponse();
                            RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ordem enviada para o roteador de ordens com sucesso.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {
                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                                    logger.Info("Descricao do erro: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                        }
                        else if ((QUANTIDADEFRACIONARIO > 0) && (QUANTIDADEINTEGRAL == 0))
                        {
                            // SOMENTE MERCADO FRACIONARIO

                            logger.Info("QUANTIDADE INTEGRAL ...........: 0");
                            logger.Info("QUANTIDADE FRACIONARIO ........" + QUANTIDADEFRACIONARIO.ToString());

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEFRACIONARIO;
                            pParametroOrdemResquest.ClienteOrdemInfo.Symbol = ContextoOMS.ObterInstrumentoFracionario(pParametroOrdemResquest.ClienteOrdemInfo.Symbol);

                            logger.Info("GRAVA A ORDEM NO BANCO DE DADOS.");

                            this.InserirOrdemCliente(pParametroOrdemResquest);

                            logger.Info("ORDEM SALVA NO BANCO DE DADOS COM SUCESSO.");

                            #region ENTRY POINT

                            logger.Info("INVOCA O SERVICO DE ROTEAMENTO DE ORDENS");

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR *************/                  
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ENVIADA COM SUCESSO.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR.");
                                    logger.Info("DESCRICAO DO ERRO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                        }
                        else if ((QUANTIDADEFRACIONARIO == 0) && (QUANTIDADEINTEGRAL > 0))
                        {
                            // SOMENTE MERCADO INTEGRAL
                            logger.Info("Envia a ordem para o banco de dados");

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEINTEGRAL;
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("Ordem enviada com sucesso");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O 
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ordem enviada para o roteador de ordens com sucesso.");
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;

                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                                    logger.Info("Descricao do erro: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                                    return ObjetoOrdem;

                                }

                            #endregion

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                            return ObjetoOrdem;
                        }

                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL SUFICIENTE PARA EFETUAR ESTA OPERACAO");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL SUFICIENTE PARA EFETUAR ESTA OPERACAO.", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion
                }

                #endregion


            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO INVOCAR O METODO ENVIARORDEMCOMPRAFRACIONARIO. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                logger.Error("DESCRICAO DO ERRO: " + ex.Message, ex);
            }

            return ObjetoOrdem;
        }

        /// <summary>
        /// Método responsável por inserir uma nova oferta de venda no mercado a vista.
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns>EnviarOrdemResponse</returns>
        private EnviarOrdemResponse EnviarOrdemVendaAVista(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();

            decimal VOLUMEORDEM = 0;
            decimal LIMITEOPERACIONAL = 0;

            int QUANTIDADEINTEGRAL = 0;
            int QUANTIDADEFRACIONARIO = 0;

            int CODIGOPARAMETROCLIENTECOMPRA = 0;
            int CODIGOPARAMETROCLIENTEVENDA = 0;

            /* METODO RESPONSAVEL POR PROCESSAR A SOLICITACAO DE ENVIO DE ORDENS NO MERCADO A VISTA
             * PRIMEIRAMENTE O METODO PASSA PELA VALIDACAO DE RISCO E POSTERIOR ENVIA A ORDEM PARA A BOLSA.
             * */

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {

                #region LIMITES OPERACIONAIS

                //**************************************************************************************************************
                //     TRECHO RESPONSAVEL POR CARREGAR OS LIMITES OPERACIOANIS DO CLIENTE.
                //**************************************************************************************************************

                logger.Info("INICIALIZA ROTINA RESPONSAVEL POR CARREGAR OS LIMITES OPERACIONAIS. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();
                
                if (pParametroOrdemResquest.IsContaMaster){
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ContaMaster;
                }
                else{
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }

                ObjetoLimitesOperacionais = ObjetoPersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                logger.Info("LIMITES CARREGADOS COM SUCESSO.");

                #endregion

                #region TIPO MERCADO ( INTEGRAL / FRACIONARIO / AMBOS)

                logger.Info("EFETUA VALIDACAO O TIPO DE MERCADO");

                int QUANTIDADEORDEM = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;
                int LOTEPADRAO = pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                int MODULOLOTENEGOCIACAO = QUANTIDADEORDEM % LOTEPADRAO;

                if (QUANTIDADEORDEM >= LOTEPADRAO)
                {
                    pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.INTEGRAL;
                    QUANTIDADEINTEGRAL = (QUANTIDADEORDEM - MODULOLOTENEGOCIACAO);

                    if (MODULOLOTENEGOCIACAO > 0)
                    {
                        pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.INTEGRALFRACIONARIO;
                        QUANTIDADEFRACIONARIO = MODULOLOTENEGOCIACAO;
                    }
                }
                else
                {
                    pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.FRACIONARIO;
                    QUANTIDADEFRACIONARIO = QUANTIDADEORDEM;
                    QUANTIDADEINTEGRAL = 0;
                }

                logger.Info("VALIDACAO DE TIPO DE MERCADO EFETUADA COM SUCESSO");

                #endregion

                #region INCLUSAO / ALTERACAO DE OFERTA

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    //**************************************************************************************************************
                    //    ALTERACAO DE UMA NOVA ORDEM. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************     

                    logger.Info("SOLICITAÇÃO DE ORDEM TIPO................ ALTERACAO");

                    logger.Info("CLIENTE.......................: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                    logger.Info("INSTRUMENTO...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
                    logger.Info("QUANTIDADE ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ........................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());

                    #region [ALTERACAO DE ORDENS]

                    #region OBTER INFORMACOES SOBRE A ORDEM ORIGINAL

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;

                    logger.Info("OBTEM INFORMACOES REFERENTES A ORDEM ORIGINAL. CLORDID ORDEM ORIGINAL: " + pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID);

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    logger.Info("INFORMACOES CARREGADAS COM SUCESSO.");

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);
                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);


                    logger.Info("VOLUME ORIGINAL ..................... " + VOLUMEORIGINAL.ToString());
                    logger.Info("VOLUME SOLICITADO ................... " + VOLUMEMODIFICACAO.ToString());
                    logger.Info("DIFERENCIAL ......................... " + DIFERENCIAL.ToString());

                    logger.Info("INICIALIZA O PARSEAMENTO DA ORDEM.");

                    #endregion

                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);
                    ExecutarModificacaoOrdensRequest.info.Symbol = InformacoesOrdem.OrdemInfo.Symbol;

                    //// VERIFICA SE A ORDEM E DIFERENTE DA ORIGINAL
                    //if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    //{
                    //    logger.Info("NÃO EXISTE ALTERAÇÕES A SEREM EFETUADAS - ORDEM IDENTICA A ORIGINAL.");

                    //    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM IDENTICA A ORIGINAL", CriticaRiscoEnum.ErroNegocio);
                    //    return ObjetoOrdem;
                    //}


                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************

                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    logger.Info("INICIALIZA O SALDO DISPONIVEL DO CLIENTE NO PARA COMPRA NO MERCADO A VISTA. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                            logger.Info("******** LIMITE DISPONIVEL ********");
                            logger.Info("CLIENTE: ............ " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("LIMITE DISPONIVEL ... " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        }
                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA");

                        logger.Info("******** LIMITE DISPONIVEL ********");
                        logger.Info("CLIENTE: ............ " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        logger.Info("LIMITE DISPONIVEL ... 0,00");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region VALIDACAO DE LOTE DE NEGOCIACAO

                    TipoMercadoEnum TipoMercadoEnumOrdemOriginal;

                    logger.Info("VALIDAO O LOTE DE NEGOCIACAO");

                    int QUANTIDADEORDEMORIGINAL = InformacoesOrdem.OrdemInfo.OrderQty;
                    int LOTEPADRAOORIGINAL = pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                    int MODULOLOTENEGOCIACAOORIGINAL = (QUANTIDADEORDEMORIGINAL % LOTEPADRAOORIGINAL);

                    if (QUANTIDADEORDEMORIGINAL >= LOTEPADRAOORIGINAL)
                    {
                        TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.INTEGRAL;
                        QUANTIDADEINTEGRAL = (QUANTIDADEORDEMORIGINAL - MODULOLOTENEGOCIACAOORIGINAL);

                        if (MODULOLOTENEGOCIACAOORIGINAL > 0)
                        {
                            TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.INTEGRALFRACIONARIO;
                            QUANTIDADEFRACIONARIO = MODULOLOTENEGOCIACAOORIGINAL;
                        }
                    }
                    else
                    {
                        TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.FRACIONARIO;
                        QUANTIDADEFRACIONARIO = QUANTIDADEORDEMORIGINAL;
                        QUANTIDADEINTEGRAL = 0;
                    }

                    #endregion

                    if (TipoMercadoEnumOrdemOriginal != pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado)
                    {
                        logger.Info("LOTE PADRAO INVALIDO PARA REALIZAR A ALTERACAO DA ORDEM");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "LOTE PADRÃO INVALIDO PARA REALIZAR A ALTERACAO DA ORDEM", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }

                    // CLIENTE UTILIZANDO MAIS LIMITE DO QUE A ORDEM ORIGINAL
                    if (DIFERENCIAL > 0)
                    {
                        //**************************************************************************************************************
                        // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                        //**************************************************************************************************************
                        double LIMITEOPERACIONALDISPONIVEL = double.Parse(LIMITEOPERACIONAL.ToString());


                        logger.Info("VALIDA SE O SALDO DISPONIVEL É SUFICIENTE PARA ENVIAR A OFERTA");
                        logger.Info("CLIENTE                :.........." + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        logger.Info("DIFERENCIAL SOLICITADO :.........." + DIFERENCIAL.ToString());
                        logger.Info("SALDO DISPONIVEL       :.........." + LIMITEOPERACIONALDISPONIVEL.ToString());

                        if (VOLUMEMODIFICACAO <= LIMITEOPERACIONALDISPONIVEL)
                        {

                            #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS


                            logger.Info("REALIZA O BACKUP DA ORDEM ORIGINAL PARA CASOS DE REJEICAO E/OU INTERVALO DE BANDA ");
                            this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                            logger.Info("BACKUP EFETUADO COM SUCESSO.");

                            EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                            EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                            logger.Info("INSERI A ORDEM NO BANCO DE DADOS.");
                            this.InserirOrdemCliente(EnviarOrdemRiscoRequest);
                            logger.Info("ORDEM INSERIDA COM SUCESSO.");

                            #endregion

                            logger.Info("ENVIA A ORDEM PARA O ROTEADOR DE ORDENS. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                            ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                            if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ALTERADA COM SUCESSO.");
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                            {
                                logger.Info("OCORREU UM PROBLEMA AO ALTERAR A ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }
                        }

                    }
                    else
                    {
                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        logger.Info("REALIZA O BACKUP DA ORDEM ORIGINAL PARA CASOS DE REJEICAO E/OU INTERVALO DE BANDA ");
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                        logger.Info("BACKUP EFETUADO COM SUCESSO.");

                        logger.Info("INSERI A ORDEM NO BANCO DE DADOS.");
                        EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        this.InserirOrdemCliente(EnviarOrdemRiscoRequest);
                        logger.Info("ORDEM INSERIDA COM SUCESSO.");

                        #endregion

                        logger.Info("ENVIA A ORDEM PARA O ROTEADOR DE ORDENS. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                        ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                        if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("ORDEM ALTERADA COM SUCESSO.");
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;
                        }
                        else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                        {
                            logger.Info("OCORREU UM PROBLEMA AO ALTERAR A ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }

                    }

                    #endregion

                }
                else
                {

                    //**************************************************************************************************************
                    //    INCLUSAO DE UMA NOVA OFERTA. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************
                    #region INCLUSAO DE NOVA OFERTA

                    logger.Info("SOLICITAÇÃO DE ORDEM TIPO................ INCLUSAO");

                    logger.Info("CLIENTE.......................: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                    logger.Info("INSTRUMENTO...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
                    logger.Info("QUANTIDADE ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ........................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());

                    decimal PRECO = decimal.Parse(pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());
                    decimal QUANTIDADE = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                    #region COTACAO

                    logger.Info("INICIALIZA O SERVICO DE COTACOES");

                    EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                    CotacaoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                    EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                    if (PRECO < CotacaoResponse.CotacaoInfo.Ultima)
                    {
                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;
                    }

                    logger.Info("SERVICO DE COTACOES INICIALIZADO COM SUCESSO.");

                    #endregion

                    VOLUMEORDEM = (PRECO * QUANTIDADE);

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************

                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                      
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    logger.Info("OBTEM LIMITE DE VENDA NO MERCADO AVISTA");

                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.    
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                            logger.Info("******** LIMITE DISPONIVEL ********");
                            logger.Info("CLIENTE: ............ " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("LIMITE DISPONIVEL ... " + LIMITEOPERACIONAL.ToString());
                        }
                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA");

                        logger.Info("******** LIMITE DISPONIVEL ********");
                        logger.Info("CLIENTE: ............ " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        logger.Info("LIMITE DISPONIVEL ... 0,00");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    //**************************************************************************************************************
                    // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                    //**************************************************************************************************************

                    logger.Info("VALIDA SE EXISTE LIMITE DISPONIVEL PARA ENVIAR A OFERTA.");
                    if (VOLUMEORDEM <= LIMITEOPERACIONAL)
                    {
                        logger.Info("LIMITE DISPONIVEL PARA REALIZAR A OPERACAO");

                        //*********************** VALIDA SE EXISTE DIVISAO DE MERCADO FRACIONARIO/INTEGRAL ***************************/
                        if ((QUANTIDADEFRACIONARIO > 0) && (QUANTIDADEINTEGRAL > 0))
                        {
                            //********* EXECUCAO NOS DOIS MERCADOS INTEGRAL/FRACIONARIO ***********/

                            logger.Info("EXECUCAO EM DOIS MERCADOS FRACIONARIO E INTEGRAL.");
                            logger.Info("TOTAL INTEGRAL     ............: " + QUANTIDADEINTEGRAL.ToString());
                            logger.Info("TOTAL FRACIONARIO  ............: " + QUANTIDADEFRACIONARIO.ToString());


                            //PARTE INTEGRAL
                            logger.Info("ENVIA ORDEM PARA O BANCO DE DADOS [ INTEGRAL ]");

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEINTEGRAL;
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("ORDEM ENVIADA PARA O BANCO DE DADOS COM SUCESSO.[INTEGRAL]");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR
                            logger.Info("ENVIA ORDEM PARA O ROTEADOR DE ORDENS [ INTEGRAL]");
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ENVIADA PARA O ROTEADOR COM SUCESSO [INTEGRAL]");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                //return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR");
                                    logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                            logger.Info("INICIA PARTE FRACIONARIO");

                            logger.Info("ENVIA ORDEM PARA O BANCO DE DADOS [ FRACIONARIO ]");

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEFRACIONARIO;
                            pParametroOrdemResquest.ClienteOrdemInfo.Symbol = ContextoOMS.ObterInstrumentoFracionario(pParametroOrdemResquest.ClienteOrdemInfo.Symbol);
                            pParametroOrdemResquest.ClienteOrdemInfo.ClOrdID = ContextoOMS.CtrlNumber;
                            pParametroOrdemResquest.ClienteOrdemInfo.Account = CodigoCliente;

                            this.InserirOrdemCliente(pParametroOrdemResquest);

                            logger.Info("ORDEM SALVA COM SUCESSO.[ FRACIONARIO ]");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR *************/
                            RespostaRoteador = new ExecutarOrdemResponse();
                            RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            logger.Info("ENVIA UMA NOVA ORDEM PARA O ROTEADOR");
                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ENVIADA COM SUCESSO.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("OCORREU UM ERRO AO ENVIAR ORDEM PARA O ROTEADOR.");
                                    logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                        }
                        else if ((QUANTIDADEFRACIONARIO > 0) && (QUANTIDADEINTEGRAL == 0))
                        {
                            // SOMENTE MERCADO FRACIONARIO
                            logger.Info("MERCADO FRACIONARIO");
                            logger.Info("QUANTIDADE FRACIONARIO.......: " + QUANTIDADEFRACIONARIO.ToString());

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEFRACIONARIO;
                            pParametroOrdemResquest.ClienteOrdemInfo.Symbol = ContextoOMS.ObterInstrumentoFracionario(pParametroOrdemResquest.ClienteOrdemInfo.Symbol);

                            logger.Info("SALVA A ORDEM NO BANCO DE DADOS.");
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("ORDEM SALVA COM SUCESSO.");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR *************/                  

                            logger.Info("ENVIA A ORDEM PARA O ROTEADOR");

                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM EXECUTADA COM SUCESSO.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O ROTEADOR.");
                                    logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                        }
                        else if ((QUANTIDADEFRACIONARIO == 0) && (QUANTIDADEINTEGRAL > 0))
                        {

                            logger.Info("MERCADO INTEGRAL.");
                            logger.Info("QUANTIDADE INTEGRAL .............:" + QUANTIDADEINTEGRAL.ToString());

                            logger.Info("ENVIA OFERTA PARA O BANCO DE DADOS");

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEINTEGRAL;
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("ORDEM SALVA COM SUCESSO.");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O 

                            logger.Info("ENVIA ORDEM PARA O ROTEADOR.");
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM EXECUTADA COM SUCESSO.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("OCORREU UM ERRO AO ENVIAR A OFERTA PARA O ROTEADOR.");
                                    logger.Info("DESCRICAO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                }

                            #endregion

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                            return ObjetoOrdem;

                        }

                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL SUFICIENTE PARA EFETUAR ESTA OPERACAO.");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL SUFICIENTE PARA EFETUAR ESTA OPERACAO.", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                }

                #endregion

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "ORDEM ENVIADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                return ObjetoOrdem;

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ENVIAR A ORDEM DE VENDA NO MERCADO A VISTA PARA O CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                logger.Error("DESCRICAO DO ERRO: " + ex.Message, ex);
            }

            return ObjetoOrdem;
        }

        /// <summary>
        /// Método responsável por inserir uma nova oferta de venda no mercado a vista.
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns>EnviarOrdemResponse</returns>
        private EnviarOrdemResponse EnviarOrdemVendaFracionario(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();

            decimal VOLUMEORDEM = 0;
            decimal LIMITEOPERACIONAL = 0;

            int QUANTIDADEINTEGRAL = 0;
            int QUANTIDADEFRACIONARIO = 0;

            int CODIGOPARAMETROCLIENTECOMPRA = 0;
            int CODIGOPARAMETROCLIENTEVENDA = 0;

            /* METODO RESPONSAVEL POR PROCESSAR A SOLICITACAO DE ENVIO DE ORDENS NO MERCADO A VISTA
             * PRIMEIRAMENTE O METODO PASSA PELA VALIDACAO DE RISCO E POSTERIOR ENVIA A ORDEM PARA A BOLSA.
             * */

            logger.Info("PREPARA ENVIO DE ORDEM DE VENDA NO MERCADO FRACIONARIO.");
            logger.Info("CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {

                #region LIMITES OPERACIONAIS

                //**************************************************************************************************************
                //     TRECHO RESPONSAVEL POR CARREGAR OS LIMITES OPERACIOANIS DO CLIENTE.
                //**************************************************************************************************************
                LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();

                logger.Info("CARREGA OS LIMITES OPERACIONAIS DO CLIENTE. :" + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                if (pParametroOrdemResquest.IsContaMaster) {
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ContaMaster;
                }
                else{
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }

                ObjetoLimitesOperacionais = ObjetoPersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                logger.Info("LIMITES CARREGADOS COM SUCESSO.");

                #endregion

                #region TIPO MERCADO ( INTEGRAL / FRACIONARIO / AMBOS)

                int QUANTIDADEORDEM = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;
                int LOTEPADRAO = pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                int MODULOLOTENEGOCIACAO = QUANTIDADEORDEM % LOTEPADRAO;

                pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado = TipoMercadoEnum.FRACIONARIO;
                QUANTIDADEFRACIONARIO = QUANTIDADEORDEM;
                QUANTIDADEINTEGRAL = 0;

                #endregion

                #region INCLUSAO / ALTERACAO DE OFERTA

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    //**************************************************************************************************************
                    //    ALTERACAO DE UMA NOVA ORDEM. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************     

                    logger.Info("SOLICITAÇÃO DE ORDEM TIPO................ ALTERACAO");

                    logger.Info("CLIENTE.......................: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                    logger.Info("INSTRUMENTO...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
                    logger.Info("QUANTIDADE ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ........................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());


                    #region [ALTERACAO DE ORDENS]

                    #region OBTER INFORMACOES SOBRE A ORDEM ORIGINAL

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;

                    logger.Info("OBTEM INFORMACOES REFERENTES A ORDEM ORIGINAL. OrigCLOrdID: " + pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID);

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    logger.Info("INFORMAÇÕES OBTIDAS COM SUCESSO.");

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);
                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                    logger.Info("VOLUME ORIGINAL....... : " + VOLUMEORIGINAL.ToString());
                    logger.Info("VOLUME SOLICITADO..... : " + VOLUMEMODIFICACAO.ToString());
                    logger.Info("DIFERENCIAL........... : " + DIFERENCIAL.ToString());

                    #endregion

                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);

                    //// VERIFICA SE A ORDEM E DIFERENTE DA ORIGINAL
                    //if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    //{
                    //    logger.Info("ORDEM ENVIADA IDENTICA A ORIGINAL.");
                    //    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM IDENTICA A ORIGINAL", CriticaRiscoEnum.ErroNegocio);
                    //    return ObjetoOrdem;
                    //}


                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************

                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;

                            logger.Info("LIMITE PARA COMPRA CARREGADO COM SUCESSO.");
                            logger.Info("CLIENTE ..................: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("LIMITE DISPONIVEL ........: " + LIMITEOPERACIONAL.ToString());
                        }
                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region VALIDACAO DE LOTE DE NEGOCIACAO

                    TipoMercadoEnum TipoMercadoEnumOrdemOriginal;

                    int QUANTIDADEORDEMORIGINAL = InformacoesOrdem.OrdemInfo.OrderQty;
                    int LOTEPADRAOORIGINAL = pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.LotePadrao;
                    int MODULOLOTENEGOCIACAOORIGINAL = (QUANTIDADEORDEMORIGINAL % LOTEPADRAOORIGINAL);

                    if (QUANTIDADEORDEMORIGINAL >= LOTEPADRAOORIGINAL)
                    {
                        TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.INTEGRAL;
                        QUANTIDADEINTEGRAL = (QUANTIDADEORDEMORIGINAL - MODULOLOTENEGOCIACAOORIGINAL);

                        if (MODULOLOTENEGOCIACAOORIGINAL > 0)
                        {
                            TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.INTEGRALFRACIONARIO;
                            QUANTIDADEFRACIONARIO = MODULOLOTENEGOCIACAOORIGINAL;
                        }
                    }
                    else
                    {
                        TipoMercadoEnumOrdemOriginal = TipoMercadoEnum.FRACIONARIO;
                        QUANTIDADEFRACIONARIO = QUANTIDADEORDEMORIGINAL;
                        QUANTIDADEINTEGRAL = 0;
                    }

                    #endregion

                    logger.Info("VALIDA O LOTE DE NEGOCIACAO");
                    if (TipoMercadoEnumOrdemOriginal != pParametroOrdemResquest.CadastroPapel.CadastroPapelInfo.TipoMercado)
                    {
                        logger.Info("LOTE PADRÃO INVALIDO PARA REALIZAR A ALTERACAO DE ORDEM");

                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "LOTE PADRÃO INVALIDO PARA REALIZAR A ALTERACAO DA ORDEM", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }
                    logger.Info("LOTE VALIDADO COM SUCESSO");

                    ExecutarModificacaoOrdensRequest.info.Symbol += "F";

                    // CLIENTE UTILIZANDO MAIS LIMITE DO QUE A ORDEM ORIGINAL
                    if (DIFERENCIAL > 0)
                    {
                        //**************************************************************************************************************
                        // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                        //**************************************************************************************************************
                        double LIMITEOPERACIONALDISPONIVEL = double.Parse(LIMITEOPERACIONAL.ToString());

                        logger.Info("VOLUME SOLICITADO ......................: " + VOLUMEMODIFICACAO.ToString());
                        logger.Info("LIMITE OPERACIONAL DISPONIVEL ..........: " + LIMITEOPERACIONALDISPONIVEL.ToString());

                        if (VOLUMEMODIFICACAO <= LIMITEOPERACIONALDISPONIVEL)
                        {

                            #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                            logger.Info("EFETUA BKP E SALVA A OFERTA NO BANCO DE DADOS.");
                            //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                            this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                            logger.Info("BACKUP SALVA COM SUCESSO");

                            EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                            EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                            logger.Info("SALVA A ORDEM NO BANCO DE DADOS");

                            this.InserirOrdemCliente(EnviarOrdemRiscoRequest);

                            logger.Info("ORDEM SALVA COM SUCESSO");

                            #endregion

                            logger.Info("ENVIA ORDEM PARA O ROTEADOR DE ORDENS.");
                            ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                            if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ALTERADA COM SUCESSO.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                        }

                    }
                    else
                    {
                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        //BACKUP DA ORDEM ORIGINAL DO CLIENTE.

                        logger.Info("EFETUA BACKUP DA ORDEM A SER ALTERADA");
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);
                        logger.Info("BACKUP EFETUADO COM SUCESSO.");

                        EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        logger.Info("GRAVA A ORDEM NO BANCO DE DADOS");
                        this.InserirOrdemCliente(EnviarOrdemRiscoRequest);
                        logger.Info("ORDEM SALVA COM SUCESSO.");

                        #endregion

                        // NÃO É NECESSARIO A CHECAGEM DE LIMITE.

                        logger.Info("ENVIA A OFERTA PARA O ROTEADOR DE ORDENS. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                        if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("ORDEM ALTERADA COM SUCESSO.");
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;
                        }
                        else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                        {
                            logger.Info("OCORREU UM PROBLEMA AO ALTERAR A ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }

                    }

                    #endregion

                }
                else
                {

                    //**************************************************************************************************************
                    //    INCLUSAO DE UMA NOVA OFERTA. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************

                    logger.Info("SOLICITAÇÃO DE ORDEM TIPO................ INCLUSAO");

                    logger.Info("CLIENTE.......................: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                    logger.Info("INSTRUMENTO...................: " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.ToString());
                    logger.Info("QUANTIDADE ...................: " + pParametroOrdemResquest.ClienteOrdemInfo.OrderQty.ToString());
                    logger.Info("PRECO ........................: " + pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());


                    #region ENVIO DE ORDEM

                    decimal PRECO = decimal.Parse(pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());
                    decimal QUANTIDADE = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                    #region COTACAO


                    logger.Info("OBTEM INFORMACOES NO SERVICO DE COTAÇÃO SOBRE A O INSTRUMENTO A SER ENVIADO");

                    EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                    CotacaoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                    EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                    if (PRECO < CotacaoResponse.CotacaoInfo.Ultima)
                    {
                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;

                        logger.Info("ALTERA O PRECO PARA A ULTIMA COTACAO DO MERCADO.");

                    }

                    #endregion

                    VOLUMEORDEM = (PRECO * QUANTIDADE);

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************

                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAAVISTA
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                      
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAAVISTA
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.    
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;

                            logger.Info("LIMITE DE VENDA NO MERCADO FRACIONARIO CARREGADO COM SUCESSO. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("CLIENTE: ...............: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("LIMITE DISPONIVEL ......: " + LIMITEOPERACIONAL.ToString());
                        }
                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    //**************************************************************************************************************
                    // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                    //**************************************************************************************************************

                    logger.Info("VOLUME ORDEM .......................: " + VOLUMEORDEM.ToString());
                    logger.Info("LIMITE OPERACIONAL .................: " + LIMITEOPERACIONAL.ToString());

                    if (VOLUMEORDEM <= LIMITEOPERACIONAL)
                    {
                        //*********************** VALIDA SE EXISTE DIVISAO DE MERCADO FRACIONARIO/INTEGRAL ***************************/
                        if ((QUANTIDADEFRACIONARIO > 0) && (QUANTIDADEINTEGRAL > 0))
                        {
                            //********* EXECUCAO NOS DOIS MERCADOS INTEGRAL/FRACIONARIO ***********/

                            //PARTE INTEGRAL
                            logger.Info("Envia a ordem para o banco de dados");

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEINTEGRAL;
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("Ordem enviada com sucesso");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O 
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ordem enviada para o roteador de ordens com sucesso.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                                    logger.Info("Descricao do erro: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                            //PARTE FRACIONARIO
                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEFRACIONARIO;
                            pParametroOrdemResquest.ClienteOrdemInfo.Symbol = ContextoOMS.ObterInstrumentoFracionario(pParametroOrdemResquest.ClienteOrdemInfo.Symbol);
                            pParametroOrdemResquest.ClienteOrdemInfo.ClOrdID = ContextoOMS.CtrlNumber;
                            pParametroOrdemResquest.ClienteOrdemInfo.Account = CodigoCliente;

                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("Ordem enviada com sucesso");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR *************/
                            RespostaRoteador = new ExecutarOrdemResponse();
                            RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ordem enviada para o roteador de ordens com sucesso.");
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                                    logger.Info("Descricao do erro: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                        }
                        else if ((QUANTIDADEFRACIONARIO > 0) && (QUANTIDADEINTEGRAL == 0))
                        {
                            // SOMENTE MERCADO FRACIONARIO
                            logger.Info("QUANTIDADE INTEGRAL .....;.....: 0 ");
                            logger.Info("QUANTIDADE FRACIONARIO ........: " + QUANTIDADEFRACIONARIO.ToString());

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEFRACIONARIO;
                            pParametroOrdemResquest.ClienteOrdemInfo.Symbol = ContextoOMS.ObterInstrumentoFracionario(pParametroOrdemResquest.ClienteOrdemInfo.Symbol);

                            logger.Info("INSERI A ORDEM NO BANCO DE DADOS");

                            this.InserirOrdemCliente(pParametroOrdemResquest);

                            logger.Info("ORDEM SALVA COM SUCESSO.");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR *************/                  
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ordem enviada para o roteador de ordens com sucesso.");

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else
                                if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                                {

                                    logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                                    logger.Info("Descricao do erro: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                    return ObjetoOrdem;
                                }

                            #endregion

                        }
                        else if ((QUANTIDADEFRACIONARIO == 0) && (QUANTIDADEINTEGRAL > 0))
                        {
                            // SOMENTE MERCADO INTEGRAL
                            logger.Info("Envia a ordem para o banco de dados");

                            pParametroOrdemResquest.ClienteOrdemInfo.OrderQty = QUANTIDADEINTEGRAL;
                            this.InserirOrdemCliente(pParametroOrdemResquest);
                            logger.Info("Ordem enviada com sucesso");

                            #region ENTRY POINT

                            //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O 
                            ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ordem enviada para o roteador de ordens com sucesso.");
                            }
                            else if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                            {
                                logger.Info("OCORREU UM PROBLEMA AO ALTERAR A ORDEM: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }

                            #endregion

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                            return ObjetoOrdem;

                        }

                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL SUFICIENTE PARA EFETUAR ESTA OPERACAO.", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                }

                #endregion

                ObjetoOrdem = this.ObterRespostaOMS(pParametroOrdemResquest.ClienteOrdemInfo.Account, "ORDEM ENVIADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                return ObjetoOrdem;

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao processar o método EnviarOrdemVendaAVista. Cliente: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString(), ex);
            }

            return ObjetoOrdem;
        }

        /// <summary>
        /// Método responsável por inserir uma nova oferta de compra no mercado de opções
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns>EnviarOrdemResponse</returns>
        private EnviarOrdemResponse EnviarOrdemCompraOpcao(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {

            logger.Info("INICIALIZANDO OS OBJETOS DO CLIENTE :" + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();

            decimal VOLUMEORDEM = 0;
            decimal LIMITEOPERACIONAL = 0;

            int CODIGOPARAMETROCLIENTECOMPRAOPCAO = 0;
            int CODIGOPARAMETROCLIENTEVENDAOPCAO = 0;

            /* METODO RESPONSAVEL POR PROCESSAR A SOLICITACAO DE ENVIO DE ORDENS NO MERCADO DE OPCOES
             * PRIMEIRAMENTE O METODO PASSA PELA VALIDACAO DE RISCO E POSTERIOR ENVIA A ORDEM PARA A BOLSA.
             * */

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            logger.Info("OBJETOS INICIALIZADOS COM SUCESSO.");

            try
            {

                #region [ Vencimento Opcao HB ]

                if (pParametroOrdemResquest.ClienteOrdemInfo.CompIDOMS.Trim() == HomeBroker)
                {
                    bool VencimentoOpcao = this.ValidaExercicioOpcoes(pParametroOrdemResquest.ClienteOrdemInfo.Symbol);

                    if (!VencimentoOpcao)
                    {
                        logger.Info("NÃO E POSSIVEL OPERAR ESTA OPCAO NO EXERCICIO. POR GENTILEZA, ENTRAR EM CONTATO COM A EQUIPE DE ATENDIMENTO.");
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "NÃO EÉ POSSIVEL OPERAR ESTA OPÇÃO, VERIFIQUE SE A MESMA JÁ FOI EXPIRADA OU ESTA NO VENCIMENTO.", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }
                }

                #endregion

                #region VALIDA SE A SERIE DE OPCAO ESTA BLOQUEADA

                logger.Info("VALIDANDO SE A SERIE DE OPCAO DA ORDEM ESTA BLOQUEADA PARA OPERACAO");

                string SerieOpcao = Regex.Replace(pParametroOrdemResquest.ClienteOrdemInfo.Symbol, "[^A-Za-z _]", string.Empty);
                SerieOpcao = SerieOpcao.Substring(SerieOpcao.Length - 1, 1);

                ValidarSerieOpcaoRequest ValidarSerieOpcaoRequest = new ValidarSerieOpcaoRequest();
                ValidarSerieOpcaoRequest.SerieOpcao = SerieOpcao;

                ValidarSerieOpcaoResponse ValidarSerieOpcaoResponse = new PersistenciaOrdens().ValidarSerieOpcao(ValidarSerieOpcaoRequest);

                if (ValidarSerieOpcaoResponse.PermissaoOpcaoEnum == PermissaoOpcaoEnum.NAOPERMITIDA)
                {

                    logger.Info("A SERIE [" + SerieOpcao.ToString() + "] ESTA BLOQUEADA PARA OPERACOES");
                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A SERIE:[" + SerieOpcao.ToString() + "] ESTA BLOQUEADA PARA OPERACOES", CriticaRiscoEnum.LimiteOperacional);
                    return ObjetoOrdem;
                }

                #endregion

                #region LIMITES OPERACIONAIS

                //**************************************************************************************************************
                //     TRECHO RESPONSAVEL POR CARREGAR OS LIMITES OPERACIOANIS DO CLIENTE.
                //**************************************************************************************************************

                logger.Info("LIMITES OPERACIONAIS CLIENTE:" + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                logger.Info("INICIALIZA THREAD DE CONTROLE DE LIMITES OPERACIONAIS");

                LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();

                if (pParametroOrdemResquest.IsContaMaster){
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ContaMaster;
                }
                else{
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }

                ObjetoLimitesOperacionais = ObjetoPersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                logger.Info("THREAD DE CONTROLE DE LIMITES OPERACIONAIS INICIALIZADAS COM SUCESSO.");

                #endregion

                #region INCLUSAO / ALTERACAO DE OFERTA

                logger.Info("VERIFICA O TIPO DE SOLICITAÇÃO DE ORDEM ACATADA. CLIENTE :" + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    logger.Info("TIPO SOLICITACAO.......... ALTERACAO DE ORDEM");

                    //**************************************************************************************************************
                    //    ALTERACAO DE UMA NOVA ORDEM. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************     
                    #region [ALTERACAO DE ORDENS]

                    #region LOTE DE NEGOCIACAO

                    if ((pParametroOrdemResquest.ClienteOrdemInfo.OrderQty % 2) != 0)
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "O LOTE PADRAO INFORMADO NÃO É VALIDO PARA ESTE MERCADO", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region OBTER INFORMACOES SOBRE A ORDEM ORIGINAL

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;
                    double PRECOORIGINAL = 0;

                    logger.Info("OBTENDO INFORMACOES SOBRE A ORDEM ORIGINAL");

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    logger.Info("INFORMACOES SOBRE A ORDEM ORIGINAL OBTIDA COM SUCESSO.");

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    PRECOORIGINAL = InformacoesOrdem.OrdemInfo.Price;
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);

                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);
                    logger.Info("TOTAL DO DIFERENCIAL ........" + DIFERENCIAL.ToString());

                    #endregion

                    //if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    //{
                    //    logger.Info("A ORDEM ALTERADA É IDENTICA A ORIGINAL");
                    //    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM IDENTICA A ORIGINAL", CriticaRiscoEnum.ErroNegocio);
                    //    return ObjetoOrdem;

                    //}

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************
                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    logger.Info("CARREGA O SALDO DISPONIVEL PARA COMPRA DE OPÇÕES CLIENTE " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                    var LIMITECOMPRAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAOPCOES
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;

                            logger.Info("DADOS LIMITE CLIENTE : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("TIPO LIMITE.................... OPCOES");
                            logger.Info("SALDO DISPONIVEL .............. " + LIMITEOPERACIONAL.ToString());

                            CODIGOPARAMETROCLIENTECOMPRAOPCAO = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        logger.Info("DADOS LIMITE CLIENTE : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        logger.Info("TIPO LIMITE.................... OPCOES");
                        logger.Info("SALDO DISPONIVEL .............. CLIENTE NAO POSSUI SALDO PARA OPERAR NO MERCADO DE OPCOES");

                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    var LIMITEVENDAAVISTA = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAOPCOES
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAAVISTA.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAAVISTA)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDAOPCAO = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);

                    // CLIENTE UTILIZANDO MAIS LIMITE DO QUE A ORDEM ORIGINAL
                    if (DIFERENCIAL > 0)
                    {
                        //**************************************************************************************************************
                        // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                        //**************************************************************************************************************
                        double LIMITEOPERACIONALDISPONIVEL = double.Parse(LIMITEOPERACIONAL.ToString());

                        logger.Info("VALIDA SALDO PARA ALTERAR A ORDEM");
                        logger.Info("TOTAL SOLICITADO ................." + VOLUMEMODIFICACAO.ToString());
                        logger.Info("TOTAL DISPONIVEL ................." + LIMITEOPERACIONALDISPONIVEL.ToString());

                        if (VOLUMEMODIFICACAO <= LIMITEOPERACIONALDISPONIVEL)
                        {

                            #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                            logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                            //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                            this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                            logger.Info("BACKUP EFETUADO COM SUCESSO");

                            EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                            EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                            logger.Info("GRAVA A ORDEM NO BANCO DE DADOS CLIENTE " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                            this.InserirOrdemCliente(EnviarOrdemRiscoRequest);

                            logger.Info("ORDEM SALVA COM SUCESSO");

                            #endregion

                            logger.Info("EFETUA CHAMADA NO ROTREADOR DE ORDENS");
                            ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                            if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                logger.Info("ORDEM ALTERADA COM SUCESSO.");
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;
                            }
                            else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                            {
                                logger.Info("OCORREU UM PROBLEMA AO ALTERAR A ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }
                        }
                        else
                        {
                            logger.Info("SALDO INSUFICIENTE PARA EFETUAR A ALTERACAO DA ORDEM SOLICITADA.");
                        }

                    }
                    else
                    {
                        // NÃO É NECESSARIO A CHECAGEM DE LIMITE.

                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        logger.Info("EFETUA O BACKUP DA ORDEM ORIGINAL. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                        //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                        logger.Info("ORDEM SALVA COM SUCESSO");

                        EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        logger.Info("GRAVA A ORDEM NO BANCO DE DADOS CLIENTE " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());

                        this.InserirOrdemCliente(EnviarOrdemRiscoRequest);

                        logger.Info("ORDEM SALVA COM SUCESSO");

                        #endregion

                        logger.Info("EFETUA CHAMADA NO ROTREADOR DE ORDENS");

                        ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                        if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("ORDEM ALTERADA COM SUCESSO.");

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;
                        }
                        else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                        {
                            logger.Info("OCORREU UM PROBLEMA AO ALTERAR A ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }

                    }


                    #endregion

                }
                else
                {
                    logger.Info("TIPO SOLICITACAO.......... NOVA ORDEM");

                    //**************************************************************************************************************
                    //    INCLUSAO DE UMA NOVA OFERTA. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************
                    #region INCLUSAO DE ORDEM

                    decimal PRECO = decimal.Parse(pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());
                    decimal QUANTIDADE = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                    #region COTACAO

                    logger.Info("OBTEM A COTACAO DO INSTRUMENTO: " + pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim());

                    EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                    CotacaoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                    EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                    if (PRECO > CotacaoResponse.CotacaoInfo.Ultima)
                    {
                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;
                    }

                    logger.Info("COTACAO OBTIDA COM SUCESSO");

                    #endregion

                    logger.Info("VOLUME SOLICITADO : " + VOLUMEORDEM.ToString());
                    VOLUMEORDEM = (PRECO * QUANTIDADE);

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO DE OPCOES
                    //**************************************************************************************************************

                    #region LIMITE OPERACIONAL PARA COMPRA DE OPCOES

                    logger.Info("CARREGA O SALDO DISPONIVEL PARA COMPRA DE OPÇÕES CLIENTE " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());


                    var LIMITECOMPRAOPCAO = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.COMPRAOPCOES
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAOPCAO.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAOPCAO)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;

                            logger.Info("DADOS LIMITE CLIENTE : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                            logger.Info("TIPO LIMITE.................... OPCOES");
                            logger.Info("SALDO DISPONIVEL .............. " + LIMITEOPERACIONAL.ToString());


                            CODIGOPARAMETROCLIENTECOMPRAOPCAO = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        logger.Info("DADOS LIMITE CLIENTE : " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        logger.Info("TIPO LIMITE.................... OPCOES");
                        logger.Info("SALDO DISPONIVEL .............. CLIENTE NAO POSSUI SALDO PARA OPERAR NO MERCADO DE OPCOES");


                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO DE OPÇÕES ", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA DE OPCOES

                    var LIMITEVENDAOPCOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAOPCOES
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAOPCOES.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAOPCOES)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDAOPCAO = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO DE OPCOES", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    //**************************************************************************************************************
                    // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                    //**************************************************************************************************************

                    logger.Info("SALDO SOLICITADO :..........." + VOLUMEORDEM.ToString());
                    logger.Info("SALDO DISPONIVEL :..........." + LIMITEOPERACIONAL.ToString());

                    if (VOLUMEORDEM <= LIMITEOPERACIONAL)
                    {
                        //*************************** TRECHO RESPONSAVEL POR SALVAR A OFERTA ******************************/                        

                        logger.Info("ENVIA ORDEM PARA O BANCO DE DADOS");
                        this.InserirOrdemCliente(pParametroOrdemResquest);
                        logger.Info("ORDEM SALVA COM SUCESSO");

                        #region ENTRY POINT

                        //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O ROTEADOR DE ORDENS

                        logger.Info("ENVIA ORDEM PARA O ROTEADOR");
                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("ORDEM ENVIADA COM SUCESSO");

                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;
                        }
                        else
                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                            {

                                logger.Info("OCORREU UM ERRO PARA ENVIAR A ORDEM PARA O ROTEADOR");
                                logger.Info("DESCRICAO DO ERRO: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                return ObjetoOrdem;
                            }

                        #endregion

                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ENVIADA COM SUCESSO.", CriticaRiscoEnum.Sucesso);
                        return ObjetoOrdem;
                    }
                    else
                    {
                        logger.Info("CLIENTE NAO POSSUI LIMITE OPERACIONAL DISPONIVEL PARA ENVIAR A OFERTA. CLIENTE: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString());
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL SUFICIENTE PARA EFETUAR ESTA OPERACAO.", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                }

                #endregion

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao processar o método EnviarOrdemCompraOpcoes. Cliente: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString(), ex);
            }

            return ObjetoOrdem;
        }

        /// <summary>
        /// Método responsável por inserir uma nova oferta de venda no mercado de opções
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns>EnviarOrdemResponse</returns>
        private EnviarOrdemResponse EnviarOrdemVendaOpcao(EnviarOrdemRiscoRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();
            PersistenciaOrdens ObjetoPersistenciaOrdens = new PersistenciaOrdens();
            LimiteOperacionalClienteResponse ObjetoLimitesOperacionais = new LimiteOperacionalClienteResponse();

            decimal VOLUMEORDEM = 0;
            decimal LIMITEOPERACIONAL = 0;

            int CODIGOPARAMETROCLIENTECOMPRA = 0;
            int CODIGOPARAMETROCLIENTEVENDA = 0;

            /* METODO RESPONSAVEL POR PROCESSAR A SOLICITACAO DE ENVIO DE ORDENS NO MERCADO DE OPCOES
             * PRIMEIRAMENTE O METODO PASSA PELA VALIDACAO DE RISCO E POSTERIOR ENVIA A ORDEM PARA A BOLSA.
             * */

            int CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;

            try
            {

                #region VALIDA SE A SERIE DE OPCAO ESTA BLOQUEADA

                logger.Info("VALIDANDO SE A SERIE DE OPCAO DA ORDEM ESTA BLOQUEADA PARA OPERACAO");

                string SerieOpcao = Regex.Replace(pParametroOrdemResquest.ClienteOrdemInfo.Symbol, "[^A-Za-z _]", string.Empty);
                SerieOpcao = SerieOpcao.Substring(SerieOpcao.Length - 1, 1);

                ValidarSerieOpcaoRequest ValidarSerieOpcaoRequest = new ValidarSerieOpcaoRequest();
                ValidarSerieOpcaoRequest.SerieOpcao = SerieOpcao;

                ValidarSerieOpcaoResponse ValidarSerieOpcaoResponse = new PersistenciaOrdens().ValidarSerieOpcao(ValidarSerieOpcaoRequest);

                if (ValidarSerieOpcaoResponse.PermissaoOpcaoEnum == PermissaoOpcaoEnum.NAOPERMITIDA)
                {

                    logger.Info("A SERIE [" + SerieOpcao.ToString() + "] ESTA BLOQUEADA PARA OPERACOES");
                    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "A SERIE:[" + SerieOpcao.ToString() + "] ESTA BLOQUEADA PARA OPERACOES", CriticaRiscoEnum.LimiteOperacional);
                    return ObjetoOrdem;
                }

                #endregion

                #region LIMITES OPERACIONAIS

                //**************************************************************************************************************
                //     TRECHO RESPONSAVEL POR CARREGAR OS LIMITES OPERACIOANIS DO CLIENTE.
                //**************************************************************************************************************
                LimiteOperacionalClienteRequest LimiteOperacionalRequest = new LimiteOperacionalClienteRequest();

                if (pParametroOrdemResquest.IsContaMaster)
                {
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ContaMaster;
                }
                else
                {
                    LimiteOperacionalRequest.CodigoCliente = pParametroOrdemResquest.ClienteOrdemInfo.Account;
                }

                ObjetoLimitesOperacionais = ObjetoPersistenciaOrdens.ObterLimiteCliente(LimiteOperacionalRequest);

                #endregion

                #region INCLUSAO / ALTERACAO DE OFERTA

                if ((pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != null) && (pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID != string.Empty))
                {
                    //**************************************************************************************************************
                    //    ALTERACAO DE UMA NOVA ORDEM. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************     
                    #region [ALTERACAO DE ORDENS]

                    #region LOTE DE NEGOCIACAO

                    if ((pParametroOrdemResquest.ClienteOrdemInfo.OrderQty % 2) != 0)
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "O LOTE PADRAO INFORMADO NÃO É VALIDO PARA ESTE MERCADO", CriticaRiscoEnum.ErroNegocio);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region OBTER INFORMACOES SOBRE A ORDEM ORIGINAL

                    double VOLUMEORIGINAL = 0;
                    double VOLUMEMODIFICACAO = 0;
                    double DIFERENCIAL = 0;

                    EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                    InformacoesRequest.NumeroControleOrdem = pParametroOrdemResquest.ClienteOrdemInfo.OrigClOrdID;

                    EnviarInformacoesOrdemResponse InformacoesOrdem =
                        this.ObterInformacoesOrdem(InformacoesRequest);

                    pParametroOrdemResquest.ClienteOrdemInfo.IdOrdem = InformacoesOrdem.OrdemInfo.IdOrdem;

                    VOLUMEORIGINAL = (InformacoesOrdem.OrdemInfo.Price * InformacoesOrdem.OrdemInfo.OrderQty);
                    VOLUMEMODIFICACAO = (pParametroOrdemResquest.ClienteOrdemInfo.Price * pParametroOrdemResquest.ClienteOrdemInfo.OrderQty);
                    DIFERENCIAL = (VOLUMEMODIFICACAO - VOLUMEORIGINAL);

                    ExecutarModificacaoOrdensRequest ExecutarModificacaoOrdensRequest = new ExecutarModificacaoOrdensRequest();
                    ExecutarModificacaoOrdensRequest = this.PARSEARALTERACAOORDEM(pParametroOrdemResquest.ClienteOrdemInfo);

                    //// VERIFICA SE A ORDEM E DIFERENTE DA ORIGINAL
                    //if ((InformacoesOrdem.OrdemInfo.Price == pParametroOrdemResquest.ClienteOrdemInfo.Price) && (InformacoesOrdem.OrdemInfo.OrderQty == pParametroOrdemResquest.ClienteOrdemInfo.OrderQty))
                    //{
                    //    ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM IDENTICA A ORIGINAL", CriticaRiscoEnum.ErroNegocio);
                    //    return ObjetoOrdem;

                    //}

                    #endregion

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************

                    #region LIMITE OPERACIONAL PARA COMPRA A VISTA

                    var LIMITECOMPRAOPCOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAOPCOES
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAOPCOES.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAOPCOES)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    var LIMITEVENDAOPCOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAOPCOES
                                            select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITEVENDAOPCOES.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAOPCOES)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                 
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO A VISTA", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    // CLIENTE UTILIZANDO MAIS LIMITE DO QUE A ORDEM ORIGINAL
                    if (DIFERENCIAL > 0)
                    {
                        //**************************************************************************************************************
                        // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                        //**************************************************************************************************************
                        double LIMITEOPERACIONALDISPONIVEL = double.Parse(LIMITEOPERACIONAL.ToString());

                        if (VOLUMEMODIFICACAO <= LIMITEOPERACIONALDISPONIVEL)
                        {
                            #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                            //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                            this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                            EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                            EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                            this.InserirOrdemCliente(EnviarOrdemRiscoRequest);

                            #endregion

                            ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                            if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                            {
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                                return ObjetoOrdem;

                            }
                            else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                            {
                                logger.Info("OCORREU UM PROBLEMA AO ALTERAR A ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                                return ObjetoOrdem;
                            }
                        }

                    }
                    else
                    {

                        #region EFETUA O BACKUP E GRAVA A ORDEM NO BANCO DE DADOS

                        //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                        this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                        EnviarOrdemRiscoRequest EnviarOrdemRiscoRequest = new EnviarOrdemRiscoRequest();
                        EnviarOrdemRiscoRequest.ClienteOrdemInfo = ExecutarModificacaoOrdensRequest.info;

                        this.InserirOrdemCliente(EnviarOrdemRiscoRequest);

                        #endregion

                        // NÃO É NECESSARIO A CHECAGEM DE LIMITE.
                        ExecutarModificacaoOrdensResponse respostaAutenticacao = this.ModificarOrdemRoteador(ExecutarModificacaoOrdensRequest.info);

                        if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ALTERADA COM SUCESSO", CriticaRiscoEnum.Sucesso);
                            return ObjetoOrdem;
                        }
                        else if (respostaAutenticacao.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                        {
                            logger.Info("OCORREU UM PROBLEMA AO ALTERAR A ORDEM: " + respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia);
                            ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, respostaAutenticacao.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.ErroNegocio);
                            return ObjetoOrdem;
                        }
                    }

                    #endregion
                }
                else
                {
                    //**************************************************************************************************************
                    //    INCLUSAO DE UMA NOVA OFERTA. 
                    //    PREPARA O PIPELINE DAS VALIDACOES DOS CONTROLES DE RISCO.                  
                    //**************************************************************************************************************
                    #region INCLUSAO DE OFERTA

                    decimal PRECO = decimal.Parse(pParametroOrdemResquest.ClienteOrdemInfo.Price.ToString());
                    decimal QUANTIDADE = pParametroOrdemResquest.ClienteOrdemInfo.OrderQty;

                    #region COTACAO

                    EnviarCotacaoRequest CotacaoRequest = new EnviarCotacaoRequest();
                    CotacaoRequest.Instrumento = pParametroOrdemResquest.ClienteOrdemInfo.Symbol.Trim();
                    EnviarCotacaoResponse CotacaoResponse = this.ObterCotacao(CotacaoRequest);

                    if (PRECO < CotacaoResponse.CotacaoInfo.Ultima)
                    {
                        PRECO = CotacaoResponse.CotacaoInfo.Ultima;
                    }

                    #endregion

                    VOLUMEORDEM = (PRECO * QUANTIDADE);

                    //**************************************************************************************************************
                    // OBTEM O LIMITE DO SALDO DE COMPRA NO MERCADO A VISTA 
                    //**************************************************************************************************************

                    #region LIMITE OPERACIONAL PARA COMPRA DE OPCOES

                    var LIMITECOMPRAOPCOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                             where p.TipoLimite == TipoLimiteEnum.COMPRAOPCOES
                                             select p;

                    //SALDO DE COMPRA NO MERCADO A VISTA.
                    if (LIMITECOMPRAOPCOES.Count() > 0)
                    {

                        foreach (var ITEM in LIMITECOMPRAOPCOES)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.                      
                            CODIGOPARAMETROCLIENTECOMPRA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO DE OPCOES", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    #region LIMITE OPERACIONAL PARA VENDA A VISTA

                    var LIMITEVENDAOPCOES = from p in ObjetoLimitesOperacionais.LimitesOperacionais
                                            where p.TipoLimite == TipoLimiteEnum.VENDAOPCOES
                                            select p;

                    //SALDO DE COMPRA NO MERCADO DE OPCOES.
                    if (LIMITEVENDAOPCOES.Count() > 0)
                    {

                        foreach (var ITEM in LIMITEVENDAOPCOES)
                        {
                            // LIMITE OPERACIONAL DISPONIVEL PARA EFETUAR A COMPRA NO MERCADO A VISTA.    
                            LIMITEOPERACIONAL = ITEM.ValorDisponivel;
                            CODIGOPARAMETROCLIENTEVENDA = ITEM.CodigoParametroCliente;
                        }
                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL PARA OPERAR NO MERCADO DE OPCOES", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion

                    //**************************************************************************************************************
                    // VALIDA SE O SALDO DISPONIVEL EM LIMITE E SUFICIENTE PARA COBRIR A OPERACAO.
                    //**************************************************************************************************************
                    if (VOLUMEORDEM <= LIMITEOPERACIONAL)
                    {
                        #region INCLUSAO OFERTA BANCO DE DADOS

                        //*************************** TRECHO RESPONSAVEL POR SALVAR A OFERTA ******************************/
                        logger.Info("Envia a ordem para o banco de dados");
                        this.InserirOrdemCliente(pParametroOrdemResquest);
                        logger.Info("Ordem enviada com sucesso");

                        #endregion

                        #region ENTRY POINT

                        //*************************** SESSÃO RESPONSAVEL POR ENVIAR UA NOVA ORDEM PARA O 
                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametroOrdemResquest.ClienteOrdemInfo);

                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("ordem enviada para o roteador de ordens com sucesso.");
                        }
                        else
                            if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Erro)
                            {

                                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                                logger.Info("Descricao do erro: " + RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia);

                                ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia, CriticaRiscoEnum.Exception);
                                return ObjetoOrdem;
                            }

                        #endregion

                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "ORDEM ENVIADA COM SUCESSO.", CriticaRiscoEnum.Sucesso);
                        return ObjetoOrdem;

                    }
                    else
                    {
                        ObjetoOrdem = this.ObterRespostaOMS(CodigoCliente, "CLIENTE NAO POSSUI LIMITE OPERACIONAL SUFICIENTE PARA EFETUAR ESTA OPERACAO.", CriticaRiscoEnum.LimiteOperacional);
                        return ObjetoOrdem;
                    }

                    #endregion
                }

                #endregion
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao processar o método EnviarOrdemVendaAVista. Cliente: " + pParametroOrdemResquest.ClienteOrdemInfo.Account.ToString(), ex);
            }

            return ObjetoOrdem;
        }

        #endregion

        #region ROTEADOR DE ORDENS

        /// <summary>
        /// METODO RESPONSAVEL POR ENVIAR UMA NOVA OFERTA PARA O RETEADOR DE ORDEM
        /// </summary>
        /// <param name="OrdemInfo"></param>
        /// <returns></returns>
        //private ExecutarOrdemResponse EnviarOrdemRoteador(OrdemInfo OrdemInfo)
        //{
        //    try
        //    {
        //        // Instancia servico de ordens
        //        logger.Info("Invoca servico de roteamento de ordens");

        //        OrdemInfo.TransactTime = DateTime.Now;
        //        OrdemInfo.Account = ContextoOMS.CalcularCodigoCliente(227, OrdemInfo.Account);

        //        logger.Info("Instanciando Ativador.Get<IRoteadorOrdens>()");
        //        IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
        //        logger.Info("tivador.Get<IRoteadorOrdens>() instanciado com sucesso.");

        //        // Envia a ordem para o reteador e aguarda o retorno
        //        logger.Info("Envia a ordem para o roteador");
        //        ExecutarOrdemResponse RespostaOrdem =
        //        ServicoRoteador.ExecutarOrdem(new ExecutarOrdemRequest()
        //        {
        //            info = OrdemInfo
        //        });

        //        logger.Info("Roteador invocado com sucesso.");

        //        return RespostaOrdem;

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
        //        logger.Info("Descrição     :" + ex.Message);

        //        throw (ex);
        //    }

        //}

        private ExecutarOrdemResponse EnviarOrdemRoteador(OrdemInfo OrdemInfo)
        {
            try
            {
                // Instancia servico de ordens
                logger.Info("Invoca servico de roteamento de ordens");

                OrdemInfo.TransactTime = DateTime.Now;

                if (bRemoveDigito)
                    OrdemInfo.Account = ContextoOMS.CalcularCodigoCliente(227, OrdemInfo.Account);


                //logger.Info("Instanciando Ativador.Get<IRoteadorOrdens>()");
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
                //logger.Info("tivador.Get<IRoteadorOrdens>() instanciado com sucesso.");

                // Envia a ordem para o reteador e aguarda o retorno
                logger.Info("Envia a ordem para o roteador");
                ExecutarOrdemResponse RespostaOrdem =
                ServicoRoteador.ExecutarOrdem(new ExecutarOrdemRequest()
                {
                    info = OrdemInfo
                });

                //ExecutarOrdemResponse RespostaOrdem = new ExecutarOrdemResponse();
                //DadosRetornoExecutarOrdem dados = new DadosRetornoExecutarOrdem();
                //OcorrenciaRoteamentoOrdem ocorrencia = new OcorrenciaRoteamentoOrdem();

                //dados.DataResposta = DateTime.Now.AddDays(30);

                //dados.StatusResposta = StatusRoteamentoEnum.Sucesso;
                //dados.Ocorrencias.Add(ocorrencia);
                //RespostaOrdem.DadosRetorno = dados;
                
                    

                logger.Info("Roteador invocado com sucesso.");

                return RespostaOrdem;

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                logger.Info("Descrição     :" + ex.Message);

                throw (ex);
            }

        }
        /// <summary>
        /// METODO RESPONSAVEL POR ENVIAR UMA NOVA OFERTA PARA O RETEADOR DE ORDEM
        /// </summary>
        /// <param name="OrdemInfo"></param>
        /// <returns></returns>
        private ExecutarOrdemResponse EnviarOrdemRoteadorBMF(OrdemInfo OrdemInfo)
        {
            try
            {
                // Instancia servico de ordens
                logger.Info("Invoca servico de roteamento de ordens");

                OrdemInfo.TransactTime = DateTime.Now;

                logger.Info("Instanciando Ativador.Get<IRoteadorOrdens>() ");
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();

                // Envia a ordem para o reteador e aguarda o retorno
                logger.Info("Envia a ordem para o roteador");
                ExecutarOrdemResponse RespostaOrdem =
                ServicoRoteador.ExecutarOrdem(new ExecutarOrdemRequest()
                {
                    info = OrdemInfo
                });

                logger.Info("Roteador invocado com sucesso.");
              

                return RespostaOrdem;

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                logger.Info("Descrição     :" + ex.Message);

                throw (ex);
            }

        }
        /// <summary>
        /// METODO RESPONSAVEL POR MODIFICAR UMA ORDEM NO MERCADO.
        /// </summary>
        /// <param name="OrdemInfo"></param>
        /// <returns></returns>
        private ExecutarModificacaoOrdensResponse ModificarOrdemRoteador(OrdemInfo OrdemInfo)
        {
            try
            {
                // Instancia servico de ordens
                logger.Info("Invoca servico de roteamento de ordens");

                OrdemInfo.TransactTime = DateTime.Now;
                if (bRemoveDigito)
                    OrdemInfo.Account = ContextoOMS.CalcularCodigoCliente(227, OrdemInfo.Account);

                // Envia a ordem para o reteador e aguarda o retorno
                logger.Info("Alteração de ordem enviada para o roteador");

                logger.Info("Instanciando Ativador.Get<IRoteadorOrdens>() - Modificação");
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
                logger.Info("tivador.Get<IRoteadorOrdens>() instanciado com sucesso.");

                ExecutarModificacaoOrdensResponse RespostaOrdem =
                ServicoRoteador.ModificarOrdem(new ExecutarModificacaoOrdensRequest()
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
        /// <summary>
        /// METODO RESPONSAVEL POR MODIFICAR UMA ORDEM NO MERCADO.
        /// </summary>
        /// <param name="OrdemInfo"></param>
        /// <returns></returns>
        private ExecutarModificacaoOrdensResponse ModificarOrdemRoteadorBMF(OrdemInfo OrdemInfo)
        {
            try
            {
                // Instancia servico de ordens
                logger.Info("Invoca servico de roteamento de ordens");


                OrdemInfo.TransactTime = DateTime.Now;

                logger.Info("Instanciando Ativador.Get<IRoteadorOrdens>() - Modificação");
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
                logger.Info("Ativador.Get<IRoteadorOrdens>() instanciado com sucesso.");

                // Envia a ordem para o reteador e aguarda o retorno
                logger.Info("Alteração de ordem enviada para o roteador");
                ExecutarModificacaoOrdensResponse RespostaOrdem =
                ServicoRoteador.ModificarOrdem(new ExecutarModificacaoOrdensRequest()
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
        /// <summary>
        /// METODO RESPONSAVEL POR CANCELAR UMA OFERTA BOVESPA
        /// </summary>
        /// <param name="OrdemInfo"></param>
        /// <returns></returns>
        private ExecutarCancelamentoOrdemResponse EnviarCancelamentoOrdemRoteador(EnviarCancelamentoOrdemRequest pParametroCancelamentoRequest)
        {
            try
            {
                // Instancia servico de ordens
                logger.Info("Invoca servico de roteamento de ordens");


                #region OBTER INFORMACOES SOBRE A ORDEM ORIGINAL
       

                logger.Info("GRAVA A ORDEM ORIGINAL NO BANCO DE DADOS / REJEICAO");
                logger.Info("OBTEM INFORMACOES REFERENTES A ORDEM ORIGINAL. CLORDID ORDEM ORIGINAL: " + pParametroCancelamentoRequest.ClienteCancelamentoInfo.OrigClOrdID);

                EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                InformacoesRequest.NumeroControleOrdem = pParametroCancelamentoRequest.ClienteCancelamentoInfo.OrigClOrdID;

                EnviarInformacoesOrdemResponse InformacoesOrdem =
                    this.ObterInformacoesOrdem(InformacoesRequest);

                logger.Info("INFORMACOES CARREGADAS COM SUCESSO.");  

                logger.Info("REALIZA O BACKUP DA ORDEM ORIGINAL PARA CASOS DE REJEICAO E/OU INTERVALO DE BANDA ");

                //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                logger.Info("ORDEM BACKUP SALVA COM SUCESSO.");     

                #endregion

                if (bRemoveDigito)
                    pParametroCancelamentoRequest.ClienteCancelamentoInfo.Account = ContextoOMS.CalcularCodigoCliente(227, pParametroCancelamentoRequest.ClienteCancelamentoInfo.Account);

                // Envia a ordem para o reteador e aguarda o retorno
                logger.Info("Envia a ordem para o roteador");


                logger.Info("Instanciando Ativador.Get<IRoteadorOrdens>() - Cancelamento");
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
                logger.Info("tivador.Get<IRoteadorOrdens>() instanciado com sucesso.");

                ExecutarCancelamentoOrdemResponse RespostaOrdem = ServicoRoteador.CancelarOrdem(
                new ExecutarCancelamentoOrdemRequest()
                {
                    info = pParametroCancelamentoRequest.ClienteCancelamentoInfo
                });

                logger.Info("Roteador invocado com sucesso.");
             

                return RespostaOrdem;

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                logger.Info("Descrição     :" + ex.Message);

                throw (ex);
            }

        }

        /// <summary>
        /// METODO RESPONSAVEL POR CANCELAR UMA OFERTA DE BMF
        /// </summary>
        /// <param name="OrdemInfo"></param>
        /// <returns></returns>
        private ExecutarCancelamentoOrdemResponse EnviarCancelamentoOrdemBMFRoteador(EnviarCancelamentoOrdemRequest pParametroCancelamentoRequest)
        {
            try
            {
                // Instancia servico de ordens
                logger.Info("Invoca servico de roteamento de ordens");
       
                // Envia a ordem para o reteador e aguarda o retorno
                logger.Info("Envia a ordem para o roteador");

                #region OBTER INFORMACOES SOBRE A ORDEM ORIGINAL


                logger.Info("GRAVA A ORDEM ORIGINAL NO BANCO DE DADOS / REJEICAO");
                logger.Info("OBTEM INFORMACOES REFERENTES A ORDEM ORIGINAL. CLORDID ORDEM ORIGINAL: " + pParametroCancelamentoRequest.ClienteCancelamentoInfo.OrigClOrdID);

                EnviarInformacoesOrdemRequest InformacoesRequest = new EnviarInformacoesOrdemRequest();
                InformacoesRequest.NumeroControleOrdem = pParametroCancelamentoRequest.ClienteCancelamentoInfo.OrigClOrdID;

                EnviarInformacoesOrdemResponse InformacoesOrdem =
                    this.ObterInformacoesOrdem(InformacoesRequest);

                logger.Info("INFORMACOES CARREGADAS COM SUCESSO.");

                logger.Info("REALIZA O BACKUP DA ORDEM ORIGINAL PARA CASOS DE REJEICAO E/OU INTERVALO DE BANDA ");

                //BACKUP DA ORDEM ORIGINAL DO CLIENTE.
                this.InserirOrdemCliente(InformacoesOrdem.OrdemInfo, true);

                logger.Info("ORDEM BACKUP SALVA COM SUCESSO.");

                #endregion

                logger.Info("Instanciando Ativador.Get<IRoteadorOrdens>() - Cancelamento");
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
                logger.Info("tivador.Get<IRoteadorOrdens>() instanciado com sucesso.");

                ExecutarCancelamentoOrdemResponse RespostaOrdem = ServicoRoteador.CancelarOrdem(
                new ExecutarCancelamentoOrdemRequest()
                {
                    info = pParametroCancelamentoRequest.ClienteCancelamentoInfo
                });

                logger.Info("Roteador invocado com sucesso.");
              

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

        #region METODOS DE APOIO

        /// <summary>
        /// Metodo responsavel por obter as informações referentes a uma ordem especifica
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private EnviarInformacoesOrdemResponse ObterInformacoesOrdem(EnviarInformacoesOrdemRequest request)
        {
            EnviarInformacoesOrdemResponse _ObterInformacoesOrdem = new EnviarInformacoesOrdemResponse();

            try
            {
                _ObterInformacoesOrdem = new PersistenciaOrdens().ObterInformacoesOrdem(request);

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao obter informações da oferta", ex);
            }

            return _ObterInformacoesOrdem;
        }

        /// <summary>
        /// METODO RESPONSAVEL POR PARSEAR UMA ORDEM DO CLIENTE
        /// </summary>
        /// <param name="OrdemRequest"></param>
        /// <returns></returns>
        private EnviarOrdemRequest PARSEARORDEM(EnviarOrdemRequest OrdemRequest)
        {
            PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();

            DateTime DataAux = DateTime.Now;

            try
            {
                logger.Info("Inicia a formatação da classe OrdemInfo");

                //Validacao para sessão mesa
                if ((OrdemRequest.ClienteOrdemInfo.ExecBroker == null) || (OrdemRequest.ClienteOrdemInfo.ExecBroker == string.Empty))
                {
                    OrdemRequest.ClienteOrdemInfo.ExecBroker = CORRETORA;
                }


                OrdemRequest.ClienteOrdemInfo.Exchange = EXCHANGEBOVESPA;
                OrdemRequest.ClienteOrdemInfo.OrdStatus = OrdemStatusEnum.ENVIADAPARAOROTEADORDEORDENS;
                OrdemRequest.ClienteOrdemInfo.RegisterTime = DateTime.Now;

                #region VALIDADE DA ORDEM

                switch (OrdemRequest.ClienteOrdemInfo.TimeInForce)
                {

                    case Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia:

                        OrdemRequest.ClienteOrdemInfo.ExpireDate = new DateTime(DataAux.Year, DataAux.Month, DataAux.Day, 23, 59, 59);
                        break;

                    case Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada:

                        OrdemRequest.ClienteOrdemInfo.ExpireDate = new DateTime(9999,12,31,23,59,59);
                        break;

                    case Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidoAteDeterminadaData:

                        OrdemRequest.ClienteOrdemInfo.ExpireDate = OrdemRequest.ClienteOrdemInfo.ExpireDate.Value;
                        break;
                }

                #endregion

                #region NUMERO DE CONTROLE DA ORDEM

                if (string.IsNullOrEmpty(OrdemRequest.ClienteOrdemInfo.ClOrdID))
                {
                    OrdemRequest.ClienteOrdemInfo.ClOrdID = ContextoOMS.CtrlNumber;
                }
                else
                {
                    OrdemRequest.ClienteOrdemInfo.OrigClOrdID = OrdemRequest.ClienteOrdemInfo.OrigClOrdID;
                    OrdemRequest.ClienteOrdemInfo.ClOrdID = ContextoOMS.CtrlNumber;
                }

                #endregion

                #region SECURITYID

                SecurityIDRequest SecurityIDRequest = new SecurityIDRequest();
                SecurityIDResponse SecurityIDResponse = new SecurityIDResponse();

                SecurityIDRequest.Instrumento = OrdemRequest.ClienteOrdemInfo.Symbol;
                SecurityIDResponse = PersistenciaOrdens.ObterSecurityID(SecurityIDRequest);

                OrdemRequest.ClienteOrdemInfo.SecurityID = OrdemRequest.ClienteOrdemInfo.Symbol;

                #endregion

                return OrdemRequest;

            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO PARSEAR A ORDEM SOLICITADA. NUMERO DE CONTROLE DA ORDEM: " + OrdemRequest.ClienteOrdemInfo.ClOrdID);
                throw (ex);
            }

        }

        /// <summary>
        /// METODO RESPONSAVEL POR PARSEAR UMA ALTERACAO DE ORDENS
        /// </summary>
        /// <param name="OrdemRequest"></param>
        /// <returns></returns>
        private ExecutarModificacaoOrdensRequest PARSEARALTERACAOORDEM(OrdemInfo OrdemRequest)
        {
            PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
            ExecutarModificacaoOrdensRequest ModificaoOrdemRequest = new ExecutarModificacaoOrdensRequest();
            ModificaoOrdemRequest.info = new OrdemInfo();

            DateTime DataAux = DateTime.Now;

            try
            {
                logger.Info("Inicia a formatação da classe OrdemInfo");
                ModificaoOrdemRequest.info.ExecBroker = CORRETORA;

                ModificaoOrdemRequest.info.Exchange = EXCHANGEBOVESPA;
                ModificaoOrdemRequest.info.IdOrdem = OrdemRequest.IdOrdem;
                ModificaoOrdemRequest.info.OrdStatus = OrdemStatusEnum.SUBSTITUICAOSOLICITADA;
                ModificaoOrdemRequest.info.RegisterTime = DateTime.Now;
                ModificaoOrdemRequest.info.Side = OrdemRequest.Side;
                ModificaoOrdemRequest.info.Symbol = OrdemRequest.Symbol;
                ModificaoOrdemRequest.info.TimeInForce = OrdemRequest.TimeInForce;
                ModificaoOrdemRequest.info.Account = OrdemRequest.Account;
                ModificaoOrdemRequest.info.ChannelID = OrdemRequest.ChannelID;
                ModificaoOrdemRequest.info.ExecBroker = OrdemRequest.ExecBroker;
                ModificaoOrdemRequest.info.ExpireDate = OrdemRequest.ExpireDate;
                ModificaoOrdemRequest.info.MinQty = OrdemRequest.MinQty;
                ModificaoOrdemRequest.info.OrderQty = (OrdemRequest.CumQty + OrdemRequest.OrderQty);
                ModificaoOrdemRequest.info.SecurityID = OrdemRequest.SecurityID;
                ModificaoOrdemRequest.info.TransactTime = DateTime.Now;
                ModificaoOrdemRequest.info.OrdType = OrdemRequest.OrdType;
                ModificaoOrdemRequest.info.OrigClOrdID = OrdemRequest.OrigClOrdID;
                ModificaoOrdemRequest.info.ClOrdID = ContextoOMS.CtrlNumber;
                ModificaoOrdemRequest.info.Price = OrdemRequest.Price;
                ModificaoOrdemRequest.info.StopPrice = OrdemRequest.StopPrice;


                #region VALIDADE DA ORDEM

                switch (OrdemRequest.TimeInForce)
                {

                    case Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia:

                        ModificaoOrdemRequest.info.ExpireDate = new DateTime(DataAux.Year, DataAux.Month, DataAux.Day, 23, 59, 59);
                        break;

                    case Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada:

                        ModificaoOrdemRequest.info.ExpireDate = new DateTime(9999, 12, 31, 23, 59, 59);
                        break;

                    case Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidoAteDeterminadaData:

                        ModificaoOrdemRequest.info.ExpireDate = OrdemRequest.ExpireDate.Value;
                        break;
                }

                #endregion


                #region SECURITYID

                SecurityIDRequest SecurityIDRequest = new SecurityIDRequest();
                SecurityIDResponse SecurityIDResponse = new SecurityIDResponse();

                SecurityIDRequest.Instrumento = OrdemRequest.Symbol;
                SecurityIDResponse = PersistenciaOrdens.ObterSecurityID(SecurityIDRequest);

                ModificaoOrdemRequest.info.SecurityID = OrdemRequest.Symbol;

                #endregion

                return ModificaoOrdemRequest;

            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO PARSEAR A ORDEM SOLICITADA. NUMERO DE CONTROLE DA ORDEM: " + OrdemRequest.ClOrdID);
                throw (ex);
            }

        }

        /// <summary>
        /// METODO RESPONSAVEL POR PARSEAR UM CANCELAMENTO DE ORDENS
        /// </summary>
        /// <param name="CancelamentoOrdemRequest"></param>
        /// <returns></returns>
        private EnviarCancelamentoOrdemRequest PARSEARORDEMCANCELAMENTO(EnviarCancelamentoOrdemRequest CancelamentoOrdemRequest)
        {
            try
            {
                PersistenciaOrdens PersistenciaOrdens = new Persistencia.PersistenciaOrdens();
                EnviarInformacoesOrdemRequest EnviarInformacoesOrdemRequest = new EnviarInformacoesOrdemRequest();
                EnviarInformacoesOrdemResponse EnviarInformacoesOrdemResponse = new EnviarInformacoesOrdemResponse();

                EnviarInformacoesOrdemRequest.NumeroControleOrdem = CancelamentoOrdemRequest.ClienteCancelamentoInfo.OrigClOrdID;
                EnviarInformacoesOrdemResponse = PersistenciaOrdens.ObterInformacoesOrdem(EnviarInformacoesOrdemRequest);

                #region CADASTRO DE PAPEIS

                //Obtem informacoes referentes ao cadastro de papel do instrumento selecionado
                EnviarCadastroPapelRequest ObjetoCadastroPapelRequest = new EnviarCadastroPapelRequest();

                string SYMBOL = EnviarInformacoesOrdemResponse.OrdemInfo.Symbol;

                //if (SYMBOL.Substring(SYMBOL.Length - 1, 1) == "F")
                //{
                //    EnviarInformacoesOrdemResponse.OrdemInfo.Symbol = SYMBOL.Remove(SYMBOL.Length - 1);
                //}

                ObjetoCadastroPapelRequest.Instrumento = EnviarInformacoesOrdemResponse.OrdemInfo.Symbol.Trim();
                EnviarCadastroPapelResponse CadastroPapeis = PersistenciaOrdens.ObterCadastroPapel(ObjetoCadastroPapelRequest);

                #endregion

                if (EnviarInformacoesOrdemResponse.OrdemInfo.Account != 0)
                {

                    switch (CadastroPapeis.CadastroPapelInfo.SegmentoMercado)
                    {
                        case SegmentoMercadoEnum.FUTURO:

                            SecurityIDRequest SecurityIDRequest = new SecurityIDRequest();

                            SecurityIDRequest.Instrumento = EnviarInformacoesOrdemResponse.OrdemInfo.Symbol;
                            SecurityIDResponse SecurityIDResponse = PersistenciaOrdens.ObterSecurityID(SecurityIDRequest);

                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Exchange = "BMF";
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.ChannelID = 0;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.SecurityID = EnviarInformacoesOrdemResponse.OrdemInfo.Symbol;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Account = EnviarInformacoesOrdemResponse.OrdemInfo.Account;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.ChannelID = EnviarInformacoesOrdemResponse.OrdemInfo.ChannelID;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.ClOrdID = ContextoOMS.CtrlNumber;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.ExecBroker = EnviarInformacoesOrdemResponse.OrdemInfo.ExecBroker;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.OrderQty = EnviarInformacoesOrdemResponse.OrdemInfo.OrderQtyRemmaining;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Side = EnviarInformacoesOrdemResponse.OrdemInfo.Side;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Symbol = EnviarInformacoesOrdemResponse.OrdemInfo.Symbol;
                            //CancelamentoOrdemRequest.ClienteCancelamentoInfo.SecurityID = SecurityIDResponse.SecurityID;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Memo5149 = EnviarInformacoesOrdemResponse.OrdemInfo.Price.ToString();

                            break;

                        default:
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Account = EnviarInformacoesOrdemResponse.OrdemInfo.Account;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.ChannelID = EnviarInformacoesOrdemResponse.OrdemInfo.ChannelID;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.ClOrdID = ContextoOMS.CtrlNumber;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Exchange = "BOVESPA";
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.ExecBroker = EnviarInformacoesOrdemResponse.OrdemInfo.ExecBroker;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.OrderQty = EnviarInformacoesOrdemResponse.OrdemInfo.OrderQtyRemmaining;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Side = EnviarInformacoesOrdemResponse.OrdemInfo.Side;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Symbol = EnviarInformacoesOrdemResponse.OrdemInfo.Symbol;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.SecurityID = CancelamentoOrdemRequest.ClienteCancelamentoInfo.Symbol;
                            CancelamentoOrdemRequest.ClienteCancelamentoInfo.Memo5149 = EnviarInformacoesOrdemResponse.OrdemInfo.Price.ToString();

                            break;
                    }


                }

                return CancelamentoOrdemRequest;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        /// <summary>
        /// METODO RESPONSAVEL POR FORMATAR A SAIDA DE UMA ORDEM
        /// </summary>
        /// <param name="CodigoCliente"></param>
        /// <param name="Descricao"></param>
        /// <param name="tipoCritica"></param>
        /// <returns></returns>
        private EnviarOrdemResponse ObterRespostaOMS(int CodigoCliente, string Descricao, CriticaRiscoEnum tipoCritica)
        {
            EnviarOrdemResponse ObjetoOrdem = new EnviarOrdemResponse();

            try
            {
                List<PipeLineCriticaInfo> CriticaInfo = new List<PipeLineCriticaInfo>();
                PipeLineCriticaInfo PipeLineCriticaInfo = new PipeLineCriticaInfo();

                PipeLineCriticaInfo.Critica = Descricao;
                PipeLineCriticaInfo.CriticaTipo = tipoCritica;
                PipeLineCriticaInfo.DataHoraCritica = DateTime.Now;

                ObjetoOrdem.CriticaInfo.Add(PipeLineCriticaInfo);
                ObjetoOrdem.StatusResposta = ObjetoOrdem.CriticaInfo[0].CriticaTipo;
                ObjetoOrdem.DescricaoResposta = ObjetoOrdem.CriticaInfo[0].Critica;
                ObjetoOrdem.DataResposta = DateTime.Now;

                logger.Info("PERMISSAO.........................: " + Descricao + " CodigoCliente: " + CodigoCliente.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao Obter a resposta do OMS.", ex);
            }

            return ObjetoOrdem;

        }

        /// <summary>
        /// METODO RESPONSAVEL POR FORMATAR A SAIDA DE UM CANCELAMENTO DE ORDENS
        /// </summary>
        /// <param name="CodigoCliente"></param>
        /// <param name="Descricao"></param>
        /// <param name="tipoCritica"></param>
        /// <returns></returns>
        private ExecutarCancelamentoOrdemResponse ObterRespostaCancelamentoOMS(int CodigoCliente, string Descricao, CriticaRiscoEnum tipoCritica)
        {
            ExecutarCancelamentoOrdemResponse ObjetoOrdem = new ExecutarCancelamentoOrdemResponse();


            try
            {

                ExecutarCancelamentoOrdemResponse ExecutarCancelamentoOrdemResponse = new ExecutarCancelamentoOrdemResponse();

                OcorrenciaRoteamentoOrdem OcorrenciaRoteamentoOrdem = new Gradual.OMS.RoteadorOrdens.Lib.Dados.OcorrenciaRoteamentoOrdem();

                OcorrenciaRoteamentoOrdem.DataHoraOcorrencia = DateTime.Now;
                OcorrenciaRoteamentoOrdem.Ocorrencia = Descricao;

                ObjetoOrdem.DadosRetorno.Ocorrencias.Add(OcorrenciaRoteamentoOrdem);

                logger.Info("PERMISSAO.........................: " + Descricao + " CodigoCliente: " + CodigoCliente.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao Obter a resposta do OMS.", ex);
            }

            return ObjetoOrdem;

        }

        /// <summary>
        /// MÉTODO RESPONSAVEL POR GRAVAR UMA ORDEM NO BANCO DE DADOS
        /// </summary>
        /// <param name="pParametros">INFORMAÇÕES DA ORDEM</param>
        private void InserirOrdemCliente(EnviarOrdemRiscoRequest pParametros)
        {
            try
            {
                PersistenciaOrdens persistenciaOrdem = new PersistenciaOrdens();

                pParametros.ClienteOrdemInfo.OrderQtyRemmaining = pParametros.ClienteOrdemInfo.OrderQty;

                persistenciaOrdem.InserirOrdem(pParametros.ClienteOrdemInfo);


            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O BANCO DE DADOS. NUMERO DE CONTROLE: " + pParametros.ClienteOrdemInfo.ClOrdID, ex);
                throw (ex);
            }

        }

        /// <summary>
        /// MÉTODO RESPONSAVEL POR GRAVAR O BACKUP ORDEM NO BANCO DE DADOS
        /// </summary>
        /// <param name="pParametros">INFORMAÇÕES DA ORDEM</param>
        private void InserirOrdemCliente(OrdemInfo pParametros, bool backup)
        {
            try
            {
                PersistenciaOrdens persistenciaOrdem = new PersistenciaOrdens();

                persistenciaOrdem.InserirOrdemBackup(pParametros);

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O BANCO DE DADOS. NUMERO DE CONTROLE: " + pParametros.ClOrdID, ex);
                throw (ex);
            }

        }

        /// <summary>
        /// MÉTODO RESPONSAVEL POR ALTERAR UMA ORDEM NO BANCO DE DADOS
        /// </summary>
        /// <param name="pParametros">INFORMAÇÕES DA ORDEM</param>
        private void AlterarOrdemCliente(EnviarOrdemRiscoRequest pParametros)
        {
            try
            {
                PersistenciaOrdens persistenciaOrdem = new PersistenciaOrdens();

                pParametros.ClienteOrdemInfo.OrderQtyRemmaining = pParametros.ClienteOrdemInfo.OrderQty;

                persistenciaOrdem.InserirOrdem(pParametros.ClienteOrdemInfo);

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O BANCO DE DADOS. NUMERO DE CONTROLE: " + pParametros.ClienteOrdemInfo.ClOrdID, ex);
                throw (ex);
            }

        }

        /// <summary>
        /// Metodo responsavel por atualizar o limite operacional no banco de dados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        private AtualizarLimitesResponse AtualizarLimiteOperacional(AtualizarLimitesRequest pRequest)
        {
            AtualizarLimitesResponse LimiteResponse = new AtualizarLimitesResponse();

            try
            {
                logger.Info("********************** LIMITE OPERACIONAL ************************************");

                logger.Info("CODIGO CLIENTE :.......................:" + pRequest.LimiteOperacionalInfo.CodigoCliente.ToString());
                logger.Info("CODIGO DO PARAMETRO :..................:" + pRequest.LimiteOperacionalInfo.CodigoParametroCliente.ToString());
                logger.Info("VALOR(R$) :............................:" + pRequest.LimiteOperacionalInfo.ValorAlocado.ToString());


                switch (pRequest.LimiteOperacionalInfo.TipoLimite)
                {
                    case TipoLimiteEnum.COMPRAAVISTA:
                        logger.Info("TIPO LIMITE :..........................:" + "COMPRA A VISTA");
                        break;

                    case TipoLimiteEnum.VENDAAVISTA:
                        logger.Info("TIPO LIMITE :..........................:" + "VENDA A VISTA");
                        break;

                    case TipoLimiteEnum.COMPRAOPCOES:
                        logger.Info("TIPO LIMITE :..........................:" + "COMPRA OPCOES");
                        break;

                    case TipoLimiteEnum.VENDAOPCOES:
                        logger.Info("TIPO LIMITE :..........................:" + "VENDA OPCOES");
                        break;
                }

                LimiteResponse = new PersistenciaOrdens().AtualizaLimiteCliente(pRequest);
                if (LimiteResponse.LimiteAtualizado)
                {
                    logger.Info("LIMITE ATUALIZADO COM SUCESSO.");
                }

                logger.Info("****************************************************************************");

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao atualizar o Limite operacional do cliente.", ex);
            }

            return LimiteResponse;
        }

        /// <summary>
        /// Obtem a cotação de um determinado instrumento
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private EnviarCotacaoResponse ObterCotacao(EnviarCotacaoRequest request)
        {
            try
            {
                return new PersistenciaOrdens().ObterCotacao(request);
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao obter a cotação do instrumento: " + request.Instrumento, ex);
                throw (ex);
            }

        }

        #endregion

    }
}


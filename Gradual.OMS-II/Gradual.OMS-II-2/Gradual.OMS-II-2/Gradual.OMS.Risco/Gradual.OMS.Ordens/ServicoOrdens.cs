using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.Risco.Lib.Mensageria;
using Gradual.OMS.Risco;
using Gradual.OMS.Ordens.Lib.Enum;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Ordens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.CadastroPapeis;
using Gradual.OMS.CadastroPapeis.Lib.Mensageria;
using log4net;
using System.Threading;
using System.Configuration;
using Gradual.OMS.Ordens.Persistencia.Lib;
using Gradual.OMS.Risco.Persistencia.Lib;


namespace Gradual.OMS.Ordens
{
    public partial class ServicoOrdens : IServicoOrdens, IServicoControlavel
    {

        // Esse tipo de declaracao é preferivel sobre a outra
        // classes derivadas de MyClass automaticamente gravarao no log
        // com o nome da classe corrigido
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int CodigoCorretora = 227;    

        private int CodigoPortaRepassadorOrdem {
            get {
                return int.Parse(ConfigurationManager.AppSettings["CodigoPortaRepassador"].ToString());
            }
        }

        private int CodigoPortaClienteHomeBroker
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CodigoPortaClienteHomeBroker"].ToString());
            }
        }

        private int CodigoPortaClienteAssessor
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CodigoPortaClienteAssessor"].ToString());
            }
        }

        private int CodigoGrupoAlavancagemDesktop{
            get{
                return int.Parse(ConfigurationManager.AppSettings["GrupoAlavancagemDesktop"].ToString());
            }
        }

        private int CodigoGrupoAlavancagemHomeBroker{
            get{
                return int.Parse(ConfigurationManager.AppSettings["GrupoAlavancagemHomeBroker"].ToString());
            }
        }


        private ServicoStatus _status = ServicoStatus.Parado;
        private bool _bKeepRunning = false;
        private IRoteadorOrdens ServicoRoteador = null;
        private Thread _thrMonitorRoteador = null;

        #region [ Implementação da Interface - Membtos públicos ]

        /// <summary>
        /// Metodo responsável por invocar o pipeline de risco a verificar todos os parametros e permissões
        /// da ordem a ser enviada para o roteador de ordem. [ Efetua todo o controle pré-trading da ordem ]
        /// </summary>
        /// <param name="pParametros">Objeto de entidade da ordem do cliente</param>
        /// <returns> Resposta da requisição </returns>
        public EnviarOrdemResponse EnviarOrdem(EnviarOrdemRequest pParametros)
        {
            try
            {
                logger.Info("Solicitacao en envio de ordem acatada. Cliente: " + pParametros.ClienteOrdemInfo.CodigoCliente.ToString());

                logger.Info("Cliente.....             " + pParametros.ClienteOrdemInfo.CodigoCliente.ToString());
                logger.Info("CodigoStopStart.....     " + pParametros.ClienteOrdemInfo.CodigoStopStart.ToString());
                logger.Info("DataHoraSolicitacao..... " + pParametros.ClienteOrdemInfo.DataHoraSolicitacao.ToString());
                logger.Info("DataValidade.....        " + pParametros.ClienteOrdemInfo.DataValidade.ToString());
                logger.Info("DirecaoOrdem.....        " + pParametros.ClienteOrdemInfo.DirecaoOrdem.ToString());
                logger.Info("Instrumento.....         " + pParametros.ClienteOrdemInfo.Instrumento.ToString());
                logger.Info("Preco.....               " + pParametros.ClienteOrdemInfo.Preco.ToString());
                logger.Info("Quantidade.....          " + pParametros.ClienteOrdemInfo.Quantidade.ToString());
                logger.Info("TipoDeOrdem.....         " + pParametros.ClienteOrdemInfo.TipoDeOrdem.ToString());
                logger.Info("ValidadeOrdem.....       " + pParametros.ClienteOrdemInfo.ValidadeOrdem.ToString());
                logger.Info("QuantidadeMinima...      " + pParametros.ClienteOrdemInfo.QuantidadeMinima.ToString());
                logger.Info("QuantidadeAparente...    " + pParametros.ClienteOrdemInfo.QuantidadeAparente.ToString());

                EnviarOrdemResponse OrdemResposta = new EnviarOrdemResponse();

                #region  [ Validação do formato da mensagem enviada ]

                logger.Info("Verifica se todos os campos da solicitação foram preenchidos corretamente");

                // VALIDA SE OS ATRIBUTOS INFORMADOS PELO CLIENTE SÃO VALIDOS
                ValidarFormatoOrdemResponse ValidarFormatoOrdemResponse =
                    this.ValidarFormatoOrdemCliente(pParametros.ClienteOrdemInfo);

                //Obtem o numero da porta de controle a ser roteada pelo OMS

                #region Obter a porta de roteamento pelo perfil do cliente

                //logger.Info("Verifica a porta que a ordem sera roteada");
                //if (pParametros.ClienteOrdemInfo.PortaControleOrdem != CodigoPortaRepassadorOrdem.ToString())
                //{                    
                //    int GrupoAlavancagemCliente = new PersistenciaOrdens().ObterCodigoGrupoAlavancagem(pParametros.ClienteOrdemInfo.CodigoCliente);
                //    logger.Info("Codigo do grupo de alavancagem do cliente : " + GrupoAlavancagemCliente.ToString());

                //    if(GrupoAlavancagemCliente == CodigoGrupoAlavancagemHomeBroker)
                //    {
                //        logger.Info("Cliente HomeBroker");
                //        pParametros.ClienteOrdemInfo.PortaControleOrdem = CodigoPortaClienteHomeBroker.ToString();
                //    }

                //    if (GrupoAlavancagemCliente == CodigoGrupoAlavancagemDesktop)
                //    {
                //        logger.Info("Cliente Assessor");
                //        pParametros.ClienteOrdemInfo.PortaControleOrdem = CodigoPortaClienteAssessor.ToString();
                //    }

                //    if ((GrupoAlavancagemCliente != CodigoGrupoAlavancagemDesktop) && (GrupoAlavancagemCliente != CodigoGrupoAlavancagemHomeBroker))
                //    {
                //        // caso não encontre cobrar tabela mesa.
                //        pParametros.ClienteOrdemInfo.PortaControleOrdem = CodigoPortaClienteAssessor.ToString();
                //    }

                //    logger.Info("Numero da porta a ser roteado : " + pParametros.ClienteOrdemInfo.PortaControleOrdem);
                //}
                //else
                //{
                //    pParametros.ClienteOrdemInfo.PortaControleOrdem = CodigoPortaRepassadorOrdem.ToString();
                //    logger.Info("Numero da porta a ser roteado : " + pParametros.ClienteOrdemInfo.PortaControleOrdem);
                //}

                #endregion

                // VERIFICA SE A MENSAGEM ENVIADA PELO CLIENTE É VALIDA.
                if (ValidarFormatoOrdemResponse.StatusResposta == CriticaRiscoEnum.Sucesso)
                {
                    #region  [ Parseador de Mensagem FIX ]

                    logger.Info("Inicia composição da mensagem FIX");

                    // INICIA PARSEADOR DE MENSAGEM PARA O FORMATO FIX.
                    OrdemFIXResponse<OrdemInfo> OrdemFixResponse = this.ParsearOrdemCliente(pParametros.ClienteOrdemInfo);

                    if (OrdemFixResponse.StatusResposta == CriticaRiscoEnum.ErroNegocio)
                    {
                        logger.Info("Mensagem FIX contem com sucesso");
                        logger.Info("Descrição da critica    : Instrumento não encontrado");

                        //VERIFICA SE EXISTEM CRITICAS A SEREM OBSERVADAS
                        if (OrdemFixResponse.CriticaInfo.Count >= 0)
                        {
                            OrdemResposta.CriticaInfo =
                                new List<PipeLineCriticaInfo>();

                            // EFETUA O DE / PARA DOS OBJETOS PARA NÃO CRIAR VINCULO ALGUM ENTRE
                            // AS BIBLIOTECAS DE RISCO E ORDEM
                            foreach (var RiscoItem in OrdemFixResponse.CriticaInfo)
                            {

                                PipeLineCriticaInfo _PipeLineCriticaInfo
                                    = new PipeLineCriticaInfo();

                                OrdemResposta.CriticaInfo.Add(
                                    new PipeLineCriticaInfo()
                                    {
                                        Critica = RiscoItem.Critica,
                                        CriticaTipo = (CriticaRiscoEnum)RiscoItem.CriticaTipo,
                                        DataHoraCritica = RiscoItem.DataHoraCritica
                                    });
                            }

                            // FORMATA A MENSAGEM DE SAIDA
                            OrdemResposta.DataResposta = OrdemFixResponse.DataResposta;
                            OrdemResposta.DescricaoResposta = OrdemFixResponse.DescricaoResposta;
                            OrdemResposta.StackTrace = OrdemFixResponse.StackTrace;
                            OrdemResposta.StatusResposta = (CriticaRiscoEnum)OrdemFixResponse.StatusResposta;

                            return OrdemResposta;
                        }
                    }

                    #endregion

                    logger.Info("Mensagem FIX composta com sucesso");

                    // OBTEM A ORDEM JÁ PARSEADA NO FORMATO FIX.
                    EnviarOrdemRoteadorRequest OrdemRoteadorRequest = new EnviarOrdemRoteadorRequest()
                    {
                        OrdemInfo = OrdemFixResponse.Objeto
                    };

                #endregion

                #region [ Efetua chamada para as validações do sistema de risco ]

                ValidacaoRiscoRequest RiscoRequisicao = new ValidacaoRiscoRequest()
                {
                    EnviarOrdemRequest = OrdemRoteadorRequest
                };

                logger.Info("Inicia rotina de validação de risco: PipeLine Risco");

                DateTime DataInicioExecucao = DateTime.Now;
                logger.Info("Data inicial da solicitação:     " + DataInicioExecucao.ToString());

                // EFETUA CHAMADA DO METODO VALIDAPIPEPINE RISCO PARA VALIDAR A ORDEM ENVIADA PELO CLIENTE.
                ValidacaoRiscoResponse RiscoResposta =
                    new ServicoRisco().ValidarPipeLineRisco(RiscoRequisicao);

                TimeSpan datafinal = (DateTime.Now - DataInicioExecucao);

                #endregion

                #region [ Tratamento da resposta da validação de risco ]

                logger.Info("Prepara a resposta da solicitação");

                //VERIFICA SE EXISTEM CRITICAS A SEREM OBSERVADAS
                if (RiscoResposta.CriticaInfo.Count >= 0)
                {
                    OrdemResposta.CriticaInfo =
                        new List<PipeLineCriticaInfo>();

                    // EFETUA O DE / PARA DOS OBJETOS PARA NÃO CRIAR VINCULO ALGUM ENTRE
                    // AS BIBLIOTECAS DE RISCO E ORDEM
                    foreach (var RiscoItem in RiscoResposta.CriticaInfo)
                    {

                        PipeLineCriticaInfo _PipeLineCriticaInfo
                            = new PipeLineCriticaInfo();

                        OrdemResposta.CriticaInfo.Add(
                            new PipeLineCriticaInfo()
                            {
                                Critica = RiscoItem.Critica,
                                CriticaTipo = (CriticaRiscoEnum)RiscoItem.CriticaTipo,
                                DataHoraCritica = RiscoItem.DataHoraCritica
                            });
                    }

                    // FORMATA A MENSAGEM DE SAIDA
                    OrdemResposta.DataResposta = RiscoResposta.DataResposta;
                    OrdemResposta.DescricaoResposta = RiscoResposta.DescricaoResposta;
                    OrdemResposta.StackTrace = RiscoResposta.StackTrace;
                    OrdemResposta.StatusResposta = (CriticaRiscoEnum)RiscoResposta.StatusResposta;

                    logger.Info("Solicitação de resposta enviado ao solicitante com sucesso.");
                    logger.Info("Tempo Total de execução   :" + datafinal.ToString()); 
                }

                #endregion

                }
                else
                    // ERRO NA FORMATACAO DA MENSAGEM
                    if (ValidarFormatoOrdemResponse.StatusResposta == CriticaRiscoEnum.ErroFormatoMensagem)
                    {
                        logger.Info("Ocorreu um erro ao formatar a mensagem de retorno.");

                        #region [ Efetua tratamento da resposta da validação da mensamgem ]

                        OrdemResposta.CriticaInfo =
                           new List<PipeLineCriticaInfo>();

                        // EFETUA O DE / PARA DOS OBJETOS PARA NÃO CRIAR VINCULO ALGUM ENTRE
                        // AS BIBLIOTECAS DE RISCO E ORDEM
                        foreach (var RiscoItem in ValidarFormatoOrdemResponse.CriticaInfo)
                        {
                            PipeLineCriticaInfo _PipeLineCriticaInfo
                                = new PipeLineCriticaInfo();

                            OrdemResposta.CriticaInfo.Add(
                                new PipeLineCriticaInfo()
                                {
                                    Critica = RiscoItem.Critica,
                                    CriticaTipo = (CriticaRiscoEnum)RiscoItem.CriticaTipo,
                                    DataHoraCritica = RiscoItem.DataHoraCritica
                                });
                        }

                        // FORMATA A MENSAGEM DE SAIDA
                        OrdemResposta.DataResposta = ValidarFormatoOrdemResponse.DataResposta;
                        OrdemResposta.DescricaoResposta = ValidarFormatoOrdemResponse.DescricaoResposta;
                        OrdemResposta.StackTrace = ValidarFormatoOrdemResponse.StackTrace;
                        OrdemResposta.StatusResposta = (CriticaRiscoEnum)ValidarFormatoOrdemResponse.StatusResposta;

                        logger.Info("Envia mensagem de retorno com o erro.");

                        #endregion
                    }

                return OrdemResposta;
            }
            catch (Exception ex)
            {
                logger.Error("Erro :" + ex.Message,ex);
                return null;
            }                   
        }
     
        /// <summary>
        /// Método responsável por enviar um cancelamento de ordens para o roteador cancelar.
        /// </summary>
        /// <param name="pParametroCancelamentoRequest"></param>
        /// <returns></returns>
        public EnviarCancelamentoOrdemResponse CancelarOrdem(EnviarCancelamentoOrdemRequest pParametroCancelamentoRequest)
        {

            logger.Info("Iniciar rotina de cancelamento de ordens");
             

            DateTime DataInicioExecucao = DateTime.Now;
            logger.Info("Data de inicio de execução:    " + DataInicioExecucao.ToString());     

            EnviarCancelamentoOrdemResponse CancelamentoOrderResponse = new EnviarCancelamentoOrdemResponse();

            OrdemFIXResponse<OrdemCancelamentoInfo> OrdemCancelamentoInfo = this.ParsearOrdemCancelamentoCliente(pParametroCancelamentoRequest.ClienteCancelamentoInfo);

            if (OrdemCancelamentoInfo.StatusResposta == CriticaRiscoEnum.Sucesso)
            {

                logger.Info("Solicitação de cancelamento de ordem enviado ");
                logger.Info("Bovespa       : " + OrdemCancelamentoInfo.Objeto.Account.ToString());
                logger.Info("ChannelID     : " + OrdemCancelamentoInfo.Objeto.ChannelID.ToString());
                logger.Info("ClOrdID       : " + OrdemCancelamentoInfo.Objeto.ClOrdID.ToString());
                logger.Info("Exchange      : " + OrdemCancelamentoInfo.Objeto.Exchange.ToString());
                logger.Info("OrderID       : " + OrdemCancelamentoInfo.Objeto.OrderID.ToString());
                logger.Info("OrigClOrdID   : " + OrdemCancelamentoInfo.Objeto.OrigClOrdID.ToString());
                logger.Info("Side          : " + OrdemCancelamentoInfo.Objeto.Side.ToString());
                logger.Info("Symbol        : " + OrdemCancelamentoInfo.Objeto.Symbol.ToString());


                logger.Info("Mensagem parseado com sucesso!");

                logger.Info("Inicializa serviço de roteamento de ordem");
                //Invoca o serviço de roteamento de ordem
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
                logger.Info("Serviço do ativador inicializado com sucesso");

                logger.Info("Calculando digito do cliente");
                OrdemCancelamentoInfo.Objeto.Account = RetornaCodigoCliente(CodigoCorretora, OrdemCancelamentoInfo.Objeto.Account);


                logger.Info("Envia ordem para o roteador");
                // Enviar a ordem para o roteador de ordens e aguarda o retorno.

                ExecutarCancelamentoOrdemResponse RespostaOrdem = ServicoRoteador.CancelarOrdem(
                new ExecutarCancelamentoOrdemRequest()
                {
                    info = OrdemCancelamentoInfo.Objeto
                });

                if (RespostaOrdem.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                {
                    logger.Info("Cancelamento enviado com sucesso.");

                    CancelamentoOrderResponse.DescricaoResposta = RespostaOrdem.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    CancelamentoOrderResponse.DataResposta = DateTime.Now;
                    CancelamentoOrderResponse.StatusResposta = CriticaRiscoEnum.Sucesso;
                }
                else
                {
                    logger.Info("Erro ao enviar o cancelamento para o Roteador.");

                    CancelamentoOrderResponse.DescricaoResposta = RespostaOrdem.DadosRetorno.Ocorrencias[0].Ocorrencia;
                    CancelamentoOrderResponse.DataResposta = DateTime.Now;
                    CancelamentoOrderResponse.StatusResposta = CriticaRiscoEnum.ErroNegocio;
                }

            }
            else
            {
                CancelamentoOrderResponse.DescricaoResposta = OrdemCancelamentoInfo.CriticaInfo[0].Critica.ToString();
                CancelamentoOrderResponse.CriticaInfo = OrdemCancelamentoInfo.CriticaInfo;
                CancelamentoOrderResponse.DataResposta = DateTime.Now;
                CancelamentoOrderResponse.StatusResposta = CriticaRiscoEnum.Sucesso;
            }

            TimeSpan datafinal = (DateTime.Now - DataInicioExecucao);
            logger.Info("Tempo total de execução     :" + datafinal.ToString());
            
            return CancelamentoOrderResponse;
   
        }

        /// <summary>
        /// Método responsável por enviar um cancelamento de ordens para o roteador cancelar.
        /// </summary>
        /// <param name="List<EnviarCancelamentoOrdemRequest>">Lista de objetos de cancelamento</param>
        /// <returns>Resposta da solicitação</returns>

        #endregion

        #region Membros Privados

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
        /// Valida se todas as informãções da solicitação de envio de ordem foram preenchidas corretamente.
        /// </summary>
        /// <param name="pParametro">Objeto Ordem</param>
        /// <returns></returns>
        private ValidarFormatoOrdemResponse ValidarFormatoOrdemCliente(ClienteOrdemInfo pParametro)
        {

            try
            {
                ValidarFormatoOrdemResponse ValidarOrdemResponse
                    = new ValidarFormatoOrdemResponse();

                List<PipeLineCriticaInfo> ListaCriticaInfo
                    = new List<PipeLineCriticaInfo>();

                // CÓDIGO DO CLIENTE
                if (pParametro.CodigoCliente == 0)
                {
                    ListaCriticaInfo.Add(
                                 new PipeLineCriticaInfo()
                                 {
                                     CriticaTipo = Lib.Enum.CriticaRiscoEnum.ErroFormatoMensagem,
                                     Critica = "O atributo <CodigoCliente> é obrigatório ",
                                     DataHoraCritica = DateTime.Now
                                 }
                          );
                }

                // DIRECAO DA ORDEM
                if (pParametro.DirecaoOrdem.Equals(null))
                {
                    ListaCriticaInfo.Add(
                                 new PipeLineCriticaInfo()
                                 {
                                     CriticaTipo = Lib.Enum.CriticaRiscoEnum.ErroFormatoMensagem,
                                     Critica = "O atributo <DirecaoOrdem> é obrigatório",
                                     DataHoraCritica = DateTime.Now
                                 }
                          );
                }

                // INSTRUMENTO
                if (string.IsNullOrEmpty(pParametro.Instrumento))
                {
                    ListaCriticaInfo.Add(
                                 new PipeLineCriticaInfo()
                                 {
                                     CriticaTipo = Lib.Enum.CriticaRiscoEnum.ErroFormatoMensagem,
                                     Critica = "O atributo <Instrumento> é obrigatório ",
                                     DataHoraCritica = DateTime.Now
                                 }
                          );

                }

                // PRECO
                if (pParametro.Preco == 0)
                {
                    ListaCriticaInfo.Add(
                                 new PipeLineCriticaInfo()
                                 {
                                     CriticaTipo = Lib.Enum.CriticaRiscoEnum.ErroFormatoMensagem,
                                     Critica = "O atributo <Preco> é obrigatório ",
                                     DataHoraCritica = DateTime.Now
                                 }
                          );
                }

                // QUANTIDADE
                if (pParametro.Quantidade == 0)
                {
                    ListaCriticaInfo.Add(
                                 new PipeLineCriticaInfo()
                                 {
                                     CriticaTipo = Lib.Enum.CriticaRiscoEnum.ErroFormatoMensagem,
                                     Critica = "O atributo <Quantidade> é obrigatório ",
                                     DataHoraCritica = DateTime.Now
                                 }
                          );
                }

                // DATAVALIDADE
                if ((pParametro.ValidadeOrdem != RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada) || (pParametro.TipoDeOrdem != OrdemTipoEnum.OnClose))
                {
                    if ((pParametro.DataValidade == null) || (pParametro.DataValidade == DateTime.MinValue))
                    {
                        ListaCriticaInfo.Add(
                                     new PipeLineCriticaInfo()
                                     {
                                         CriticaTipo = Lib.Enum.CriticaRiscoEnum.ErroFormatoMensagem,
                                         Critica = "O atributo <DataValidade> é obrigatório ",
                                         DataHoraCritica = DateTime.Now
                                     }
                              );
                    }
                }             


                if ((pParametro.PortaControleOrdem == null))
                {
                    ListaCriticaInfo.Add(
                                 new PipeLineCriticaInfo()
                                 {
                                     CriticaTipo = Lib.Enum.CriticaRiscoEnum.ErroFormatoMensagem,
                                     Critica = "É preciso informar o numero da porta que a ordem será roteada",
                                     DataHoraCritica = DateTime.Now
                                 }
                          );
                }



                // TIPO ORDEM
                if (pParametro.TipoDeOrdem.Equals(null))
                {
                    ListaCriticaInfo.Add(
                                 new PipeLineCriticaInfo()
                                 {
                                     CriticaTipo = Lib.Enum.CriticaRiscoEnum.ErroFormatoMensagem,
                                     Critica = "O atributo <TipoDeOrdem> é obrigatório ",
                                     DataHoraCritica = DateTime.Now
                                 }
                          );
                }

                // VERIFICA SE EXISTE CRITICAS
                if (ListaCriticaInfo.Count > 0)
                {
                    ValidarOrdemResponse.CriticaRiscoEnum = CriticaRiscoEnum.ErroFormatoMensagem;
                    ValidarOrdemResponse.StatusResposta = CriticaRiscoEnum.ErroFormatoMensagem;
                    ValidarOrdemResponse.CriticaInfo = ListaCriticaInfo;
                    ValidarOrdemResponse.DataResposta = DateTime.Now;
                    ValidarOrdemResponse.DescricaoResposta = string.Format("{0}{1}{2}", "Existem <", ListaCriticaInfo.Count.ToString(), "> items a serem verificados.");
                }
                else
                {
                    ValidarOrdemResponse.StatusResposta = CriticaRiscoEnum.Sucesso;

                }

                logger.Info("Mensagem validada com sucesso.");
                return ValidarOrdemResponse;
            }
            catch (Exception ex)
            {
                logger.Error("Erro :" + ex.Message, ex);

                return null ;
            }

           

        }

        /// <summary>
        /// Gera um novo numero de controle para a ordem
        /// </summary>
        private string CtrlNumber
        {
            get
            {
                return  string.Format("{0}{1}{2}",
                        DateTime.Now.ToString("ddMMyyyyhhmmss").Replace("/", string.Empty).Replace(" ", string.Empty).Replace(":", string.Empty),
                        "-", new Random().Next(0, 99999999).ToString());
            }
        }


        #endregion

        #region IServicoControlavel Members
        public void IniciarServico()
        {
            _thrMonitorRoteador = new Thread(new ThreadStart(RunMonitor));
            _thrMonitorRoteador.Start();

            _bKeepRunning = true;
            _status = ServicoStatus.EmExecucao;
        }

        public void PararServico()
        {
            _bKeepRunning = false;
            if (_thrMonitorRoteador != null)
            {
                while (_thrMonitorRoteador.IsAlive)
                {
                    logger.Info("Aguardando finalizacao da thread de monitoracao do roteador");
                    Thread.Sleep(250);
                }
            }
            _status = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }


        /// <summary>
        /// Monitor de conexoes ao roteador de ordens
        /// </summary>
        private void RunMonitor()
        {
            try
            {
                logger.Info("Iniciando thread de monitoracao do roteador de ordens");
                int _iMonitorConexoes = 0;

                if (ServicoRoteador == null)
                    ServicoRoteador = Ativador.Get<IRoteadorOrdens>();

                while (_bKeepRunning)
                {
                    // 4 * 250 = 1 segundo
                    if (_iMonitorConexoes == 30 * 4)
                    {
                        lock (ServicoRoteador)
                        {
                            try
                            {
                                if (ServicoRoteador == null)
                                    ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
                                ServicoRoteador.Ping(new PingRequest());
                            }
                            catch (Exception ex)
                            {
                                Ativador.AbortChannel(ServicoRoteador);
                                ServicoRoteador = null;
                            }
                        }
                        _iMonitorConexoes = 0;
                    }
                    else
                    {
                        _iMonitorConexoes++;
                    }

                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("RunMonitor(): " + ex.Message, ex);
            }

            logger.Info("Thread de monitoracao do roteador finalizada");
        }

        #endregion // IServicoControlavel Members
    }
}

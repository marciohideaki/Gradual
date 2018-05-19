using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

using Gradual.OMS.Contratos.CanaisNegociacao;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Ordens
{
    /// <summary>
    /// Implementação do serviço de ordens.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoOrdens : IServicoOrdens
    {
        #region Variaveis Locais

        /// <summary>
        /// Configurações do servico de ordens
        /// </summary>
        private ServicoOrdensConfig _config = GerenciadorConfig.ReceberConfig<ServicoOrdensConfig>();

        /// <summary>
        /// Referencia ao serviço de persistencia de ordens.
        /// A rotina receberPersistenciaOrdens pega a referencia para a instancia na primeira chamada.
        /// </summary>
        private IServicoOrdensPersistencia _servicoOrdensPersistencia = null;

        /// <summary>
        /// Referencia ao serviço de persistencia de mensagens.
        /// A rotina receberPersistenciaMensagens pega a referencia para a instancia na primeira chamada.
        /// </summary>
        private IServicoPersistenciaMensagens _servicoPersistenciaMensagens = null;

        /// <summary>
        /// Referencia para o serviço de segurança
        /// </summary>
        private IServicoSeguranca _servicoSeguranca = Ativador.Get<IServicoSeguranca>();

        /// <summary>
        /// Referencia para o servico de risco
        /// </summary>
        private IServicoRisco _servicoRisco = Ativador.Get<IServicoRisco>();
        
        /// <summary>
        /// Repositório de instrumentos por bolsa
        /// </summary>
        private Dictionary<string, RepositorioInstrumentos> _repositorioInstrumentos = 
            new Dictionary<string, RepositorioInstrumentos>();

        /// <summary>
        /// Controle de codigos para as mensagem. A chave é o grupo, e o número é o último código gerado
        /// </summary>
        private List<string> _codigosMensagens = new List<string>();

        /// <summary>
        /// Mantem a data da última mensagem processada.
        /// Utilizado para gerar códigos únicos de mensagens pela rotina GerarCodigoMensagem.
        /// </summary>
        private DateTime _dataUltimaMensagem = DateTime.MinValue;

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default.
        /// </summary>
        public ServicoOrdens()
        {
            // Efetua log
            Log.EfetuarLog("ServicoOrdens - Construtor", LogTipoEnum.Passagem, ModulosOMS.ModuloOrdens);
        }

        #endregion

        #region IServicoOrdens Members

        #region Eventos

        public event EventHandler<SinalizarEventArgs> EventoSinalizacao;
        public event EventHandler<SinalizarMensagemInvalidaEventArgs> EventoSinalizarMensagemInvalida;
        public event EventHandler<SinalizarRejeicaoCancelamentoOrdemEventArgs> EventoSinalizarRejeicaoCancelamentoOrdem;
        public event EventHandler<SinalizarListaInstrumentosEventArgs> EventoSinalizarListaInstrumentos;
        public event EventHandler<SinalizarExecucaoOrdemEventArgs> EventoSinalizarExecucaoOrdem;

        #endregion

        #region Métodos

        #region Execução de Ordem

        public ExecutarOrdemResponse ExecutarOrdem(ExecutarOrdemRequest parametros)
        {
            // Prepara resposta
            ExecutarOrdemResponse resposta =
                new ExecutarOrdemResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    DataReferencia = DateTime.Now,
                    StatusSolicitacao = ExecutarOrdemStatusSolicitacaoEnum.EmExecucao
                };
            
            // Bloco de controle
            try
            {
                // Acha a sessao para complementar o sistema cliente
                ReceberSessaoResponse sessaoCliente = acharSessao(parametros.CodigoSessao, true);
                parametros.CodigoSistemaCliente = sessaoCliente.Sessao.CodigoSistemaCliente;

                // Se nao informou a conta, informa
                if (parametros.Account == null)
                    parametros.Account = sessaoCliente.Usuario.Complementos.ReceberItem<ContextoOMSInfo>().CodigoCBLC;

                // Faz a validação da mensagem
                bool mensagemValida = true;

                // Apenas se estiver válida
                if (mensagemValida)
                {
                    // Verifica para qual canal repassar 
                    // **** Por enquanto não utiliza o parametro tipo de cliente

                    // ATP: Revisar toda essa parte, para envio de ordens em operadores diferentes.
                    parametros.CodigoCanal = traduzCanal(parametros.CodigoSistemaCliente, parametros.CodigoBolsa, null);

                    // Salva mensagem na persistencia
                    salvarMensagem(parametros);

                    // Repassa a mensagem para o canal
                    IServicoCanaisNegociacao servicoCanais = Ativador.Get<IServicoCanaisNegociacao>();
                    servicoCanais.EnviarMensagem(parametros.CodigoCanal, parametros);

                    // Sinaliza conta corrente
                    _servicoRisco.ProcessarOperacao(
                        new ProcessarOperacaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            Usuario = sessaoCliente.Usuario,
                            CodigoOrigemOperacao = "Ordens",
                            CodigoReferenciaOperacao = parametros.ClOrdID,
                            TipoEvento = ProcessarOperacaoTipoEventoEnum.Provisionar,
                            ValorOperacao = parametros.OrderQty * parametros.Price
                        });

                    // Sinaliza na mensagem
                }
                else
                {
                    // Sinaliza na mensagem o erro de negócio
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    resposta.DescricaoResposta = "Erro na validação da mensagem... ...";
                }
            }
            catch (Exception ex)
            {
                // Sinaliza na mensagem
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna resposta
            return resposta;
        }

        public CancelarOrdemResponse CancelarOrdem(CancelarOrdemRequest parametros)
        {
            // Prepara resposta
            CancelarOrdemResponse resposta =
                new CancelarOrdemResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Bloco de controle
            try
            {
                // Acha a sessao para complementar o sistema cliente
                ReceberSessaoResponse sessaoCliente = acharSessao(parametros.CodigoSessao, true);
                parametros.CodigoSistemaCliente = sessaoCliente.Sessao.CodigoSistemaCliente;

                // Se nao informou a conta, informa
                if (parametros.Account == null)
                    parametros.Account = sessaoCliente.Usuario.Complementos.ReceberItem<ContextoOMSInfo>().CodigoCBLC;

                // Verifica se necessita completar algo da ordem
                if (parametros.Symbol == null || parametros.Side == OrdemDirecaoEnum.NaoInformado || parametros.CodigoBolsa == null)
                {
                    // Tenta localizar a ordem
                    OrdemInfo ordem = receberOrdemPersistencia(parametros.OrigClOrdID);

                    // Se não achou a ordem, dispara erro
                    if (ordem == null)
                        throw new OrdemReferenciaNaoEncontradaException()
                        {
                            ClOrdID = parametros.OrigClOrdID,
                            MensagemOriginal = parametros,
                            Source = "ServicoOrdens"
                        };

                    // Completa com as informações
                    parametros.Symbol = ordem.Instrumento;
                    parametros.Side = ordem.Direcao;
                    parametros.CodigoBolsa = ordem.CodigoBolsa;
                }

                // Salva mensagem na persistencia
                salvarMensagem(parametros);

                // Verifica para qual canal repassar 
                // **** Por enquanto não utiliza o parametro tipo de cliente
                string codigoCanal = traduzCanal(parametros.CodigoSistemaCliente, parametros.CodigoBolsa, null);

                // Repassa a mensagem para o canal
                IServicoCanaisNegociacao servicoCanais = Ativador.Get<IServicoCanaisNegociacao>();
                servicoCanais.EnviarMensagem(codigoCanal, parametros);

                // Sinaliza conta corrente
                _servicoRisco.ProcessarOperacao(
                    new ProcessarOperacaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        Usuario = sessaoCliente.Usuario,
                        CodigoOrigemOperacao = "Ordens",
                        CodigoReferenciaOperacao = parametros.ClOrdID,
                        TipoEvento = ProcessarOperacaoTipoEventoEnum.Remover
                    });

                // Sinaliza na mensagem
                resposta.DescricaoStatusSolicitacao = "Solicitação enviada";

            }
            catch (Exception ex)
            {
                // Sinaliza na mensagem
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna resposa
            return resposta;
        }

        public AlterarOrdemResponse AlterarOrdem(AlterarOrdemRequest parametros)
        {
            // Prepara resposta
            AlterarOrdemResponse resposta =
                new AlterarOrdemResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Bloco de controle
            try
            {
                // Complementa a mensagem com as informações da ordem original

                // Verifica para qual canal repassar 
                // **** Por enquanto não utiliza o parametro tipo de cliente
                string codigoCanal = traduzCanal(parametros.CodigoSistemaCliente, parametros.CodigoBolsa, null);

                // Salva mensagem na persistencia
                salvarMensagem(parametros);

                // Repassa a mensagem para o canal
                IServicoCanaisNegociacao servicoCanais = Ativador.Get<IServicoCanaisNegociacao>();
                servicoCanais.EnviarMensagem(codigoCanal, parametros);

                // Sinaliza na mensagem
            }
            catch (Exception ex)
            {
                // Sinaliza na mensagem
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna resposta
            return resposta;
        }

        public SinalizarExecucaoOrdemResponse SinalizarExecucaoOrdem(SinalizarExecucaoOrdemRequest parametros)
        {
            // Bloco de controle
            try
            {
                // Indica a qual mensagem esta se refere
                // ** O codigoMensagemReferencia do SinalizarExecucaoOrdemRequest
                // já é o ClOrdID, ou seja, a linha abaixo não tem sentido.
                //parametros.CodigoMensagemReferencia = parametros.ClOrdID;

                // Salva mensagem na persistencia
                salvarMensagem(parametros);

                // Muitos tipos de execução podem ser tratados da mesma maneira, apenas
                // atualizando as informações, salvando histórico e atualizando a mensagem
                // original
                if (parametros.ExecType == OrdemTipoExecucaoEnum.NovaPendente ||
                    parametros.ExecType == OrdemTipoExecucaoEnum.Nova ||
                    parametros.ExecType == OrdemTipoExecucaoEnum.CancelamentoOferta ||
                    parametros.ExecType == OrdemTipoExecucaoEnum.Negocio ||
                    parametros.ExecType == OrdemTipoExecucaoEnum.Rejeicao ||
                    parametros.ExecType == OrdemTipoExecucaoEnum.Substituicao ||
                    parametros.ExecType == OrdemTipoExecucaoEnum.TerminoValidade ||
                    parametros.ExecType == OrdemTipoExecucaoEnum.Preenchimento || 
                    parametros.ExecType == OrdemTipoExecucaoEnum.Suspenso)
                {
                    // Salva a ordem
                    salvarOrdem(parametros);
                }
                else
                {
                    // Informa sinalização de ordem não tratada
                    Log.EfetuarLog(
                        string.Format(
                            "Gradual.OMS.Sistemas.Ordens.SinalizarExecucaoOrdem - ExecType não tratado: \n{0}", 
                            Serializador.TransformarEmString(parametros)),
                        LogTipoEnum.Aviso, ModulosOMS.ModuloOrdens);
                }

                // Dispara o evento
                if (this.EventoSinalizarExecucaoOrdem != null)
                    this.EventoSinalizarExecucaoOrdem(
                        this,
                        new SinalizarExecucaoOrdemEventArgs()
                        {
                            SinalizarExecucaoOrdemRequest = parametros
                        });

                // Dispara evento generico
                this.sinalizar(parametros);
            }
            catch (Exception ex)
            {
                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna 
            return new SinalizarExecucaoOrdemResponse();
        }

        public SinalizarRejeicaoCancelamentoOrdemResponse SinalizarRejeicaoCancelamentoOrdem(SinalizarRejeicaoCancelamentoOrdemRequest parametros)
        {
            // Bloco de controle
            try
            {
                // Atualiza o estado interno da ordem

                // Dispara o evento
                if (this.EventoSinalizarRejeicaoCancelamentoOrdem != null)
                    this.EventoSinalizarRejeicaoCancelamentoOrdem(
                        this,
                        new SinalizarRejeicaoCancelamentoOrdemEventArgs()
                        {
                            SinalizarRejeicaoCancelamentoOrdemRequest = parametros
                        });

                // Dispara evento generico
                this.sinalizar(parametros);
            }
            catch (Exception ex)
            {
                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna
            return new SinalizarRejeicaoCancelamentoOrdemResponse();
        }

        public ReceberOrdemResponse ReceberOrdem(ReceberOrdemRequest parametros)
        {
            // Prepara resposta
            ReceberOrdemResponse resposta = new ReceberOrdemResponse();

            // Bloco de controle
            try
            {
                // Recebe a resposta
                resposta = receberPersistenciaOrdens().ReceberOrdem(parametros);
            }
            catch (Exception ex)
            {
                // Sinaliza na mensagem
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Repassa para o servico de persistencia
            return resposta;
        }

        public ListarOrdensResponse ListarOrdens(ListarOrdensRequest parametros)
        {
            // Prepara resposta
            ListarOrdensResponse resposta = new ListarOrdensResponse();

            // Bloco de controle
            try
            {
                // Recebe a resposta
                resposta = receberPersistenciaOrdens().ListarOrdens(parametros);
            }
            catch (Exception ex)
            {
                // Sinaliza na mensagem
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }
            
            // Repassa para o servico de persistencia
            return resposta;
        }

        public ListarMensagensResponse ListarMensagens(ListarMensagensRequest parametros)
        {
            // Prepara resposta
            ListarMensagensResponse resposta = new ListarMensagensResponse();

            // Bloco de controle
            try
            {
                // Recebe a resposta
                resposta = receberPersistenciaMensagens().ListarMensagens(parametros);
            }
            catch (Exception ex)
            {
                // Sinaliza na mensagem
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Repassa para o servico de persistencia
            return resposta;
        }

        #endregion

        #region Genérico

        public string GerarCodigoMensagem(string grupo)
        {
            string codigo = null;
            DateTime hora = DateTime.Now;

            // Se passou o segundo, limpa a lista
            if ((hora - _dataUltimaMensagem).Seconds > 0)
                _codigosMensagens.Clear();

            // Gera a mensagem
            int i = 1;
            while (true)
            {
                codigo = grupo + hora.ToString("hhmmss") + i.ToString("0000");
                if (!_codigosMensagens.Contains(codigo))
                    break;
                i++;
            }

            // Adiciona na lista e corrige a hora
            _dataUltimaMensagem = hora;
            _codigosMensagens.Add(codigo);

            // Retorna
            return codigo;
        }

        public SinalizarMensagemInvalidaResponse SinalizarMensagemInvalida(SinalizarMensagemInvalidaRequest parametros)
        {
            // Bloco de controle
            try
            {
                // Faz o log
                Log.EfetuarLog("SinalizarMensagemInvalida: " + Serializador.TransformarEmString(parametros), LogTipoEnum.Aviso, ModulosOMS.ModuloOrdens);

                // TODO: ServicoOrdens - IServicoOrdens.SinalizaMensagemInvalida: Retornar erro para o cliente indicando mensagem inválida

                // Descobre o cliente que enviou a mensagem

                // Caso tenha numero de ordem, informa ao risco

                // Dispara o evento
                if (this.EventoSinalizarMensagemInvalida != null)
                    this.EventoSinalizarMensagemInvalida(
                        this,
                        new SinalizarMensagemInvalidaEventArgs()
                        {
                            SinalizarMensagemInvalidaRequest = parametros
                        });

                // Dispara evento generico
                this.sinalizar(parametros);

                // Reenvia
            }
            catch (Exception ex)
            {
                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna
            return new SinalizarMensagemInvalidaResponse();
        }

        public MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros)
        {
            // Inicializa
            Type tipo = parametros.GetType();
            MensagemResponseBase resposta = null;

            // Bloco de controle
            try
            {
                // Faz o log da mensagem
                Log.EfetuarLog(Serializador.TransformarEmString(parametros), LogTipoEnum.Passagem, ModulosOMS.ModuloOrdens);

                // Repassa a chamada de acordo com o tipo da mensagem de parametros
                if (tipo == typeof(ExecutarOrdemRequest))
                    resposta = ExecutarOrdem((ExecutarOrdemRequest)parametros);
                else if (tipo == typeof(ListarInstrumentosRequest))
                    resposta = ListarInstrumentos((ListarInstrumentosRequest)parametros);
                else if (tipo == typeof(CancelarOrdemRequest))
                    resposta = CancelarOrdem((CancelarOrdemRequest)parametros);
                else if (tipo == typeof(SincronizarCanalRequest))
                    resposta = SincronizarCanal((SincronizarCanalRequest)parametros);
                else if (tipo == typeof(AlterarOrdemRequest))
                    resposta = AlterarOrdem((AlterarOrdemRequest)parametros);
                else if (tipo == typeof(ListarOrdensRequest))
                    resposta = ListarOrdens((ListarOrdensRequest)parametros);
                else if (tipo == typeof(ReceberOrdemRequest))
                    resposta = ReceberOrdem((ReceberOrdemRequest)parametros);
                else if (tipo == typeof(ListarMensagensRequest))
                    resposta = ListarMensagens((ListarMensagensRequest)parametros);
                else if (tipo == typeof(SinalizarExecucaoOrdemRequest))
                    resposta = SinalizarExecucaoOrdem((SinalizarExecucaoOrdemRequest)parametros);
                else if (tipo == typeof(SinalizarListaInstrumentosRequest))
                    resposta = ListarInstrumentos((ListarInstrumentosRequest)parametros);
                else if (tipo == typeof(SinalizarMensagemInvalidaRequest))
                    resposta = SinalizarMensagemInvalida((SinalizarMensagemInvalidaRequest)parametros);
                else if (tipo == typeof(SinalizarRejeicaoCancelamentoOrdemRequest))
                    resposta = SinalizarRejeicaoCancelamentoOrdem((SinalizarRejeicaoCancelamentoOrdemRequest)parametros);
            }
            catch (Exception ex)
            {
                // Se tem mensagem, informa
                if (resposta != null)
                {
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                    resposta.DescricaoResposta = ex.ToString();
                }

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna
            return resposta;
        }

        public SincronizarCanalResponse SincronizarCanal(SincronizarCanalRequest parametros)
        {
            // Prepara resposta
            SincronizarCanalResponse resposta = new SincronizarCanalResponse();

            // Bloco de controle
            try
            {
                // Repassa a mensagem para o canal
                IServicoCanaisNegociacao servicoCanais = Ativador.Get<IServicoCanaisNegociacao>();
                servicoCanais.EnviarMensagem(parametros.CodigoCanal, parametros);

                // Sinaliza na mensagem
                resposta.DescricaoStatusSolicitacao = "Solicitação OK";
            }
            catch (Exception ex)
            {
                // Sinaliza na mensagem
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna resposta
            return resposta;
        }

        #endregion

        #region Lista de Instrumentos

        public ListarInstrumentosResponse ListarInstrumentos(ListarInstrumentosRequest parametros)
        {
            // Prepara resposta
            ListarInstrumentosResponse resposta =
                new ListarInstrumentosResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    DataReferencia = DateTime.Now,
                };

            // Bloco de controle
            try
            {
                // Acha o repositorio referente à bolsa
                RepositorioInstrumentos repositorioInstrumentos =
                    receberRepositorioInstrumentos(parametros.CodigoBolsa);

                // Verifica se deve pedir novamente a lista de instrumentos
                if (repositorioInstrumentos.DataCarregamento < DateTime.Now.AddHours(-8))
                    listarInstrumentos(parametros);

                // Sinaliza na mensagem
                resposta.Instrumentos = (from i in repositorioInstrumentos.Instrumentos
                                         select i.Symbol).ToArray();
            }
            catch (Exception ex)
            {
                // Sinaliza na mensagem
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna resposta
            return resposta;
        }

        public SinalizarListaInstrumentosResponse SinalizarListaInstrumentos(SinalizarListaInstrumentosRequest parametros)
        {
            // Bloco de controle
            try
            {
                // Dispara o evento
                if (EventoSinalizarListaInstrumentos != null)
                    EventoSinalizarListaInstrumentos(
                        this,
                        new SinalizarListaInstrumentosEventArgs()
                        {
                            SinalizarListaInstrumentosRequest = parametros
                        });

                // Dispara evento generico
                this.sinalizar(parametros);
            }
            catch (Exception ex)
            {
                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloOrdens);
            }

            // Retorna
            return new SinalizarListaInstrumentosResponse();
        }

        public ConsultarInstrumentosResponse ConsultarInstrumentos(ConsultarInstrumentosRequest parametros)
        {
            // Prepara o retorno
            ConsultarInstrumentosResponse resposta = new ConsultarInstrumentosResponse();

            // Verifica se é por security ID ou por symbol
            if (parametros.SecurityID != null)
                resposta.Instrumentos.Add(receberRepositorioInstrumentos(parametros.CodigoBolsa).ConsultarPorSecurityID(parametros.SecurityID));
            else 
                resposta.Instrumentos.Add(receberRepositorioInstrumentos(parametros.CodigoBolsa).ConsultarPorSymbol(parametros.Symbol));

            // Retorna
            return resposta;
        }

        #endregion

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Acha o código do sistema cliente através do código da sessão
        /// </summary>
        /// <param name="codigoSessao"></param>
        /// <returns></returns>
        private ReceberSessaoResponse acharSessao(string codigoSessao, bool retornarUsuario)
        {
            return 
                ((ReceberSessaoResponse)
                    _servicoSeguranca.ReceberSessao(
                        new ReceberSessaoRequest()
                        {
                            CodigoSessao = codigoSessao,
                            CodigoSessaoARetornar = codigoSessao,
                            RetornarUsuario = retornarUsuario
                        }));
        }

        /// <summary>
        /// Acha o canal correspondente à combinação sistema + bolsa + clienteTipo
        /// </summary>
        /// <param name="codigoSistema">Código do sistema cliente (hb, plataforma mesa, etc)</param>
        /// <param name="codigoBolsa">Código da bolsa para negociação (ex: bmf, bovespa)</param>
        /// <param name="clienteTipo">Tipo do cliente (ex: financeiro, não financeiro, etc)</param>
        /// <returns>Código do canal a ser utilizado</returns>
        private string traduzCanal(string codigoSistema, string codigoBolsa, string clienteTipo)
        {
            string codigoCanal = null;
            foreach (RelacaoBolsaCanalInfo info in _config.RelacaoCanais)
            {
                if ((info.ClienteTipo == clienteTipo || info.ClienteTipo == null) &&
                    (info.CodigoBolsa == codigoBolsa || info.CodigoBolsa == null) &&
                    (info.CodigoSistema == codigoSistema || info.CodigoSistema == null))
                {
                    codigoCanal = info.CodigoCanal;
                    break;
                }
            }
            return codigoCanal;
        }

        /// <summary>
        /// Dispara o evento de sinalização passando a mensagem como parâmetro
        /// </summary>
        /// <param name="mensagem">Mensagem a ser passada no evento</param>
        private void sinalizar(MensagemSinalizacaoBase mensagem)
        {
            // Faz o log da mensagem
            Log.EfetuarLog(Serializador.TransformarEmString(mensagem), LogTipoEnum.Passagem, ModulosOMS.ModuloOrdens);
            
            // Se a mensagem não tem o código da sessão mas tem o apontamento para a mensagem original,
            // seta o codigo da sessao o mesmo da mensagem original
            if (mensagem.CodigoSessao == null && mensagem.CodigoMensagemReferencia != null)
            {
                // Recupera a mensagem original
                MensagemBase mensagemOriginal = receberMensagem(mensagem.CodigoMensagemReferencia);

                // Recupera a sessao
                if (mensagemOriginal != null)
                    mensagem.CodigoSessao = mensagemOriginal.CodigoSessao;
            }

            // Dispara o evento de sinalização
            if (this.EventoSinalizacao != null)
                this.EventoSinalizacao(
                    this,
                    new SinalizarEventArgs()
                    {
                        Mensagem = mensagem
                    });
        }

        /// <summary>
        /// Contém a regra para verificar se deve solicitar a lista de instrumentos para
        /// o canal ou se deve utilizar o repositório salvo.
        /// </summary>
        /// <param name="codigoBolsa">Código da bolsa que se deseja a lista de instrumentos</param>
        /// <returns>Classe de repositório de instrumentos</returns>
        private RepositorioInstrumentos receberRepositorioInstrumentos(string codigoBolsa)
        {
            // Verifica se deve carregar instrumentos
            if (!_repositorioInstrumentos.ContainsKey(codigoBolsa))
            {
                // Tem arquivo salvo?
                string fileName = 
                    Path.Combine(
                    _config.DiretorioRepositorioInstrumentos, 
                    _config.PrefixoRepositorioInstrumentos + codigoBolsa + ".xml");
                if (_config.DiretorioRepositorioInstrumentos != "" && File.Exists(fileName))
                {
                    RepositorioInstrumentos repositorio = new RepositorioInstrumentos();
                    repositorio.Carregar(fileName);
                    _repositorioInstrumentos.Add(codigoBolsa, repositorio);
                }
                else
                {
                    // Pede a lista de instrumentos para o canal
                    listarInstrumentos(
                        new ListarInstrumentosRequest()
                        {
                            CodigoBolsa = codigoBolsa,
                            CodigoMensagem = Guid.NewGuid().ToString()
                        });

                    // Deve salvar o arquivo?
                    if (_config.DiretorioRepositorioInstrumentos != "")
                        _repositorioInstrumentos[codigoBolsa].Salvar(fileName);
                }
            }

            // Retorna
            return _repositorioInstrumentos[codigoBolsa];
        }

        /// <summary>
        /// Rotina para listar os instrumentos de determinada bolsa. Faz a solicitação e 
        /// fica aguardando a resposta com a lista de instrumentos. Tem um timeout 
        /// (por enquanto fixo em 30 segundos) para esperar a resposta de retorno com a 
        /// lista de instrumentos.
        /// </summary>
        /// <param name="parametros">Mensagem de parametros para requisição da lista de instrumentos</param>
        /// <returns>Mensagem com a lista de instrumentos</returns>
        private List<InstrumentoInfo> listarInstrumentos(ListarInstrumentosRequest parametros)
        {
            // Acha o repositorio referente à bolsa
            RepositorioInstrumentos repositorioInstrumentos = null;
            if (_repositorioInstrumentos.ContainsKey(parametros.CodigoBolsa))
            {
                repositorioInstrumentos = _repositorioInstrumentos[parametros.CodigoBolsa];
            }
            else
            {
                repositorioInstrumentos = new RepositorioInstrumentos();
                _repositorioInstrumentos.Add(parametros.CodigoBolsa, repositorioInstrumentos);
            }

            // Acha a sessao para complementar o sistema cliente
            ReceberSessaoResponse sessaoCliente = acharSessao(parametros.CodigoSessao, false);
            parametros.CodigoSistemaCliente = sessaoCliente.Sessao.CodigoSistemaCliente;

            // Verifica para qual canal repassar 
            // **** Por enquanto não utiliza o parametro tipo de cliente
            string codigoCanal = traduzCanal(parametros.CodigoSistemaCliente, parametros.CodigoBolsa, null);

            // Espera a chegada do evento de retorno
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            SinalizarListaInstrumentosEventArgs sinalizarEventArgs = null;
            this.EventoSinalizarListaInstrumentos +=
                delegate(object sender, SinalizarListaInstrumentosEventArgs eventArgs)
                {
                    // Verifica se a resposta é referente à solicitação
                    if (eventArgs.SinalizarListaInstrumentosRequest.SecurityReqID == parametros.CodigoMensagem)
                    {
                        sinalizarEventArgs = eventArgs;
                        manualResetEvent.Set();
                    }
                };

            // Espera chegada de evento de mensagem inválida
            // TODO: ServicoOrdens - IServidoOrdens.ListarInstrumentos: Tratar evento de mensagem inválida

            // Repassa a mensagem para o canal
            IServicoCanaisNegociacao servicoCanais = Ativador.Get<IServicoCanaisNegociacao>();
            servicoCanais.EnviarMensagem(codigoCanal, parametros);

            // Aguarda retorno com um timeout de 30 segundos
            manualResetEvent.WaitOne(new TimeSpan(0, 0, 30));

            // Verifica se retornou a lista
            if (sinalizarEventArgs == null)
                throw new Exception("Lista de instrumentos indisponível.");
            
            // Carrega repositorio
            repositorioInstrumentos.CarregarLista(
                sinalizarEventArgs.SinalizarListaInstrumentosRequest.SecurityListMessages);

            // Deve salvar o arquivo?
            if (_config.DiretorioRepositorioInstrumentos != "")
                _repositorioInstrumentos[parametros.CodigoBolsa].Salvar(
                    Path.Combine(
                        _config.DiretorioRepositorioInstrumentos,
                        _config.PrefixoRepositorioInstrumentos + parametros.CodigoBolsa + ".xml"));

            // Retorna resposta
            return repositorioInstrumentos.Instrumentos;
        }

        #region Operações de Persistencia de Mensagens e Ordens

        /// <summary>
        /// Salva ordem de acordo com a mensagem de sinalização
        /// </summary>
        /// <param name="parametros"></param>
        private void salvarOrdem(SinalizarExecucaoOrdemRequest parametros)
        {
            // Pega o codigo da ordem de acordo com o tipo de sinalizacao
            string codigoOrdem = null;
            if (parametros.ExecType == OrdemTipoExecucaoEnum.Nova ||
                parametros.ExecType == OrdemTipoExecucaoEnum.NovaPendente ||
                parametros.ExecType == OrdemTipoExecucaoEnum.Negocio ||
                parametros.ExecType == OrdemTipoExecucaoEnum.Preenchimento ||
                parametros.ExecType == OrdemTipoExecucaoEnum.Rejeicao ||
                parametros.ExecType == OrdemTipoExecucaoEnum.Suspenso)
                codigoOrdem = parametros.ClOrdID;
            else
                codigoOrdem = parametros.OrigClOrdID;
            
            // Pega mensagem original
            MensagemOrdemRequestBase mensagemOriginal =
                (MensagemOrdemRequestBase)
                    receberMensagem(codigoOrdem);

            // Recupera sessao
            if (mensagemOriginal != null)
                parametros.CodigoSessao = mensagemOriginal.CodigoSessao;

            // Pega ordem existente ou cria nova
            OrdemInfo ordemInfo = receberOrdemPersistencia(codigoOrdem);
            if (ordemInfo == null)
            {
                // Cria ordem
                ordemInfo = new OrdemInfo();
                ordemInfo.CodigoBolsa = parametros.CodigoBolsa;
                ordemInfo.CodigoOrdem = codigoOrdem;
                ordemInfo.CodigoOrdemBolsa = parametros.OrderID;
                ordemInfo.CodigoSessao = parametros.CodigoSessao;
                ordemInfo.CodigoCliente = parametros.Account;
                ordemInfo.DataReferencia = parametros.TradeDate;
                ordemInfo.Direcao = parametros.Side;
                ordemInfo.Instrumento = parametros.Symbol;
                ordemInfo.Validade = parametros.TimeInForce;
                ordemInfo.CodigoSistemaCliente = parametros.CodigoSistemaCliente;
                
                // Se a mensagem original for uma execução, pega o código externo
                ExecutarOrdemRequest mensagemOriginal2 = mensagemOriginal as ExecutarOrdemRequest;
                if (mensagemOriginal2 != null)
                {
                    ordemInfo.CodigoExterno = mensagemOriginal2.CodigoExterno;
                    ordemInfo.CodigoCanal = mensagemOriginal2.CodigoCanal;
                    ordemInfo.DataValidade = mensagemOriginal2.ExpireDate;
                }
            }

            // Coloca informações que servem tanto para inserção quanto alteração
            ordemInfo.DataUltimaAlteracao = DateTime.Now;
            ordemInfo.Preco = parametros.Price;
            ordemInfo.Quantidade = parametros.OrderQty;
            ordemInfo.QuantidadeExecutada = parametros.CumQty;
            ordemInfo.Status = parametros.OrdStatus;

            // Adiciona historico
            ordemInfo.Historico.Add(parametros);

            // Salva a ordem
            salvarOrdemPersistencia(ordemInfo, parametros);

            // Atualiza risco de acordo com o evento ocorrido, cria a mensagem
            ProcessarOperacaoRequest processarRequest =
                new ProcessarOperacaoRequest()
                {
                    CodigoSessao = parametros.CodigoSessao,
                    CodigoOrigemOperacao = "Ordens",
                    CodigoReferenciaOperacao = codigoOrdem,
                    CodigoUsuario = mensagemOriginal.CodigoUsuario,
                };
            
            // Preenche valor e tipo do evento de acordo com o tipo do evento de sinalizacao
            switch (parametros.ExecType)
            {
                case OrdemTipoExecucaoEnum.CancelamentoNegocio:
                case OrdemTipoExecucaoEnum.CancelamentoOferta:
                case OrdemTipoExecucaoEnum.Rejeicao:
                    processarRequest.TipoEvento = ProcessarOperacaoTipoEventoEnum.Remover;
                    break;
                case OrdemTipoExecucaoEnum.Nova:
                case OrdemTipoExecucaoEnum.NovaPendente:
                    processarRequest.TipoEvento = ProcessarOperacaoTipoEventoEnum.Reservar;
                    processarRequest.ValorOperacao = parametros.Price * parametros.OrderQty;
                    break;
                case OrdemTipoExecucaoEnum.Negocio:
                case OrdemTipoExecucaoEnum.Preenchimento:
                    processarRequest.TipoEvento = ProcessarOperacaoTipoEventoEnum.Alterar;
                    processarRequest.ValorOperacao = parametros.LeavesQty * parametros.Price;
                    break;
                case OrdemTipoExecucaoEnum.Substituicao:
                    break;
            }
            
            // Executa o processamento no risco
            _servicoRisco.ProcessarOperacao(processarRequest);

            // Informa a execução na mensagem correspondente
            if (mensagemOriginal != null)
            {
                mensagemOriginal.Status = mensagemOriginal.Status ==
                    MensagemStatusEnum.Respondida ? MensagemStatusEnum.RespondidaMaisDeUmaVez : MensagemStatusEnum.Respondida;
                salvarMensagem(mensagemOriginal);
            }
        }

        /// <summary>
        /// Acha a ordem solicitada no repositório local, ou tenta carregar através da persistência
        /// </summary>
        /// <param name="codigoOrdem"></param>
        /// <returns></returns>
        private OrdemInfo receberOrdemPersistencia(string clOrdID)
        {
            // Retorna
            return 
                receberPersistenciaOrdens().ReceberOrdem(
                    new ReceberOrdemRequest()
                    {
                        ClOrdID = clOrdID
                    }).OrdemInfo;
        }

        /// <summary>
        /// Salva informações da ordem no repositório interno e na persistência
        /// </summary>
        /// <param name="ordemInfo"></param>
        /// <param name="mensagemSinalizacao">Mensagem para ser salva na persistencia como histórico de execução da ordem</param>
        private void salvarOrdemPersistencia(OrdemInfo ordemInfo, SinalizarExecucaoOrdemRequest mensagemSinalizacao)
        {
            // Salva na persistencia
            receberPersistenciaOrdens().SalvarOrdem(
                new SalvarOrdemRequest() 
                { 
                    OrdemInfo = ordemInfo,
                    MensagemSinalizacao = mensagemSinalizacao
                });
        }

        /// <summary>
        /// Recupera a mensagem da lista interna ou do repositorio
        /// </summary>
        /// <returns></returns>
        private MensagemBase receberMensagem(string codigoMensagem)
        {
            // Retorna
            return 
                receberPersistenciaMensagens().ReceberMensagem(
                    new ReceberMensagemRequest() 
                    { 
                        CodigoMensagem = codigoMensagem
                    }).Mensagem;
        }

        /// <summary>
        /// Salva a mensagem na lista interna e na persistencia
        /// </summary>
        /// <returns></returns>
        private void salvarMensagem(MensagemBase mensagem)
        {
            // Pede para a persistencia salvar a mensagem
            receberPersistenciaMensagens().SalvarMensagem(
                new SalvarMensagemRequest() 
                { 
                    Mensagem = mensagem
                });
        }

        /// <summary>
        /// Retorna referencia para o servico de persistencia de mensagens.
        /// Age como se fosse um singleton, na primeira chamada mantem a referencia internamente.
        /// </summary>
        /// <returns></returns>
        private IServicoPersistenciaMensagens receberPersistenciaMensagens()
        {
            if (_servicoPersistenciaMensagens == null)
                _servicoPersistenciaMensagens = Ativador.Get<IServicoPersistenciaMensagens>();
            return _servicoPersistenciaMensagens;
        }

        /// <summary>
        /// Retorna referencia para o servico de persistencia de ordens.
        /// Age como se fosse um singleton, na primeira chamada mantem a referencia internamente.
        /// </summary>
        /// <returns></returns>
        private IServicoOrdensPersistencia receberPersistenciaOrdens()
        {
            if (_servicoOrdensPersistencia == null)
                _servicoOrdensPersistencia = Ativador.Get<IServicoOrdensPersistencia>();
            return _servicoOrdensPersistencia;
        }

        #endregion

        #endregion
    }
}

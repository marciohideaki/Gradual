using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.ContaCorrente.Mensagens;
using Gradual.OMS.Contratos.Custodia;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Custodia.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor;
using Gradual.OMS.Contratos.Integracao.Sinacor.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Comum;

//using Orbite.RV.Contratos.MarketData.Bovespa;
//using Orbite.RV.Contratos.MarketData.Bovespa.Dados;
//using Orbite.RV.Contratos.MarketData.Bovespa.Mensagens;

namespace Gradual.OMS.Sistemas.Integracao.Sinacor.OMS
{
    /// <summary>
    /// Implementação do serviço de integração entre o sinacor e a segurança
    /// </summary>
    public class ServicoIntegracaoSinacorOMS : IServicoIntegracaoSinacorOMS
    {
        #region IServicoIntegracaoSinacorOMS Members

        /// <summary>
        /// Solicita a sincronização da custodia de determinado cliente com o sinacor
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SincronizarCustodiaResponse SincronizarCustodia(SincronizarCustodiaRequest parametros)
        {
            // Prepara resposta
            SincronizarCustodiaResponse resposta =
                new SincronizarCustodiaResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            
            // Bloco de controle
            try
            {
                // Inicializacao
                IServicoCustodia servicoCustodia = Ativador.Get<IServicoCustodia>();
                IServicoIntegracaoSinacor servicoIntegracaoSinacor = Ativador.Get<IServicoIntegracaoSinacor>();
                IServicoSeguranca servicoSeguranca = null;
                ContextoOMSInfo contextoOMS = null;
                CustodiaInfo custodiaInfo = parametros.Custodia;
                UsuarioInfo usuarioInfo = parametros.Usuario;
                string codigoCustodia = parametros.CodigoCustodia;
                string codigoClienteCBLC = parametros.CodigoClienteCBLC;
                bool salvarUsuario = false;

                // Verifica se informou codigo custodia e cblc, se nao, precisa consultar o usuario
                if (codigoCustodia == null || codigoClienteCBLC == null)
                {
                    // Referencia ao servico de seguranca
                    servicoSeguranca = Ativador.Get<IServicoSeguranca>();

                    // Pede o usuario
                    if (usuarioInfo == null)
                        usuarioInfo = 
                            servicoSeguranca.ReceberUsuario(
                                new ReceberUsuarioRequest() 
                                { 
                                    CodigoSessao = parametros.CodigoSessao,
                                    CodigoUsuario = parametros.CodigoUsuario
                                }).Usuario;

                    // Pega o contexto do OMS
                    contextoOMS = usuarioInfo.Complementos.ReceberItem<ContextoOMSInfo>();

                    // Se nao tem contexto é erro
                    if (contextoOMS == null)
                        throw new Exception("ContextoOMS não encontrado para este usuário");

                    // Pega os valores desejados
                    if (codigoClienteCBLC == null) codigoClienteCBLC = contextoOMS.CodigoCBLC;
                    if (codigoCustodia == null) codigoCustodia = contextoOMS.CodigoCustodia;
                }
                
                // Recebe ou cria a custodia
                if (custodiaInfo == null && codigoCustodia != null)
                {
                    // Recebe a custodia
                    custodiaInfo =
                        servicoCustodia.ReceberCustodia(
                            new ReceberCustodiaRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoCustodia = codigoCustodia
                            }).CustodiaInfo;
                }
                else if (custodiaInfo == null)
                {
                    // Cria a custodia
                    custodiaInfo = new CustodiaInfo();

                    // Se ainda não carregou..
                    if (contextoOMS == null)
                    {
                        // Pede o usuario
                        if (usuarioInfo == null)
                            usuarioInfo =
                                servicoSeguranca.ReceberUsuario(
                                    new ReceberUsuarioRequest()
                                    {
                                        CodigoSessao = parametros.CodigoSessao,
                                        CodigoUsuario = parametros.CodigoUsuario
                                    }).Usuario;

                        // Pega o contexto do OMS
                        contextoOMS = usuarioInfo.Complementos.ReceberItem<ContextoOMSInfo>();

                        // Se nao tem contexto é erro
                        if (contextoOMS == null)
                            throw new Exception("ContextoOMS não encontrado para este usuário");
                    }

                    // Salva no contexto
                    contextoOMS.CodigoCustodia = custodiaInfo.CodigoCustodia;
                    contextoOMS.CodigoCBLC = codigoClienteCBLC;

                    // Sinaliza salvar usuario
                    salvarUsuario = true;
                }

                // Prepara
                custodiaInfo.Posicoes.Clear();

                // Recebe a lista de posicoes do sinacor
                ReceberCustodiaSinacorResponse custodiaSinacorResponse =
                    servicoIntegracaoSinacor.ReceberCustodiaSinacor(
                        new ReceberCustodiaSinacorRequest()
                        {
                            CodigoClienteCBLC = codigoClienteCBLC
                        });

                // Preenche a custodia
                foreach (CustodiaSinacorPosicaoInfo posicaoSinacorInfo in custodiaSinacorResponse.Resultado)
                    custodiaInfo.Posicoes.Add(
                        new CustodiaPosicaoInfo()
                        {
                            Carteira = posicaoSinacorInfo.Carteira,
                            CodigoAtivo = posicaoSinacorInfo.CodigoAtivo,
                            CodigoBolsa = posicaoSinacorInfo.CodigoBolsa,
                            QuantidadeAbertura = posicaoSinacorInfo.QuantidadeAbertura,
                            QuantidadeAtual = posicaoSinacorInfo.QuantidadeAtual,
                            QuantidadeCompra = posicaoSinacorInfo.QuantidadeCompra,
                            QuantidadeVenda = posicaoSinacorInfo.QuantidadeVenda
                        });

                // Sinaliza datas na custodia
                custodiaInfo.DataIntegracao = DateTime.Now;

                // Salva a custodia
                if (parametros.SalvarEntidades)
                    servicoCustodia.SalvarCustodia(
                        new SalvarCustodiaRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CustodiaInfo = custodiaInfo
                        });

                // Salva usuario se necessário
                if (parametros.SalvarEntidades && salvarUsuario)
                    servicoSeguranca.SalvarUsuario(
                        new SalvarUsuarioRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            Usuario = usuarioInfo
                        });

                // Informa na resposta
                resposta.Custodia = custodiaInfo;
            }
            catch (Exception ex)
            {
                // Faz o log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloIntegracao);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita a sincronização da conta corrente de determinado cliente com o sinacor
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SincronizarContaCorrenteResponse SincronizarContaCorrente(SincronizarContaCorrenteRequest parametros)
        {
            // Prepara resposta
            SincronizarContaCorrenteResponse resposta =
                new SincronizarContaCorrenteResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Bloco de controle
            try
            {
                // Inicializacao
                IServicoIntegracaoSinacor servicoIntegracaoSinacor = Ativador.Get<IServicoIntegracaoSinacor>();
                IServicoContaCorrente servicoContaCorrente = Ativador.Get<IServicoContaCorrente>();
                IServicoSeguranca servicoSeguranca = Ativador.Get<IServicoSeguranca>();
                ContextoOMSInfo contextoOMS = null;
                UsuarioInfo usuarioInfo = parametros.Usuario;
                bool salvarUsuario = false;

                // Parametros iniciais
                ContaCorrenteInfo contaCorrente = parametros.ContaCorrente;
                string codigoContaCorrente = parametros.CodigoContaCorrente;
                string codigoClienteCBLC = parametros.CodigoClienteCBLC;
                string codigoClienteCBLCInvestimento = parametros.CodigoClienteCBLCInvestimento;

                // Tem tudo o que precisa?
                if ((parametros.SincronizarContaCorrente && codigoClienteCBLC == null) && 
                    (parametros.SincronizarContaInvestimento && codigoClienteCBLCInvestimento == null))
                {
                    // Pede o usuario
                    if (usuarioInfo == null)
                        usuarioInfo =
                            servicoSeguranca.ReceberUsuario(
                                new ReceberUsuarioRequest()
                                {
                                    CodigoSessao = parametros.CodigoSessao,
                                    CodigoUsuario = parametros.CodigoUsuario
                                }).Usuario;

                    // Pega o contexto do OMS
                    contextoOMS = usuarioInfo.Complementos.ReceberItem<ContextoOMSInfo>();

                    // Se nao tem contexto é erro
                    if (contextoOMS == null)
                        throw new Exception("ContextoOMS não encontrado para este usuário");

                    // Pega os valores desejados
                    if (codigoClienteCBLC == null) codigoClienteCBLC = contextoOMS.CodigoCBLC;
                    if (codigoClienteCBLCInvestimento == null) codigoClienteCBLCInvestimento = contextoOMS.CodigoCBLCInvestimento;
                    if (codigoContaCorrente == null) codigoContaCorrente = contextoOMS.CodigoContaCorrente;
                }

                // Recebe ou cria a conta corrente
                if (contaCorrente == null && codigoContaCorrente != null)
                {
                    // Recebe a conta corrente
                    contaCorrente =
                        servicoContaCorrente.ReceberContaCorrente(
                            new ReceberContaCorrenteRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoContaCorrente = codigoContaCorrente
                            }).ContaCorrenteInfo;
                }
                else if (contaCorrente == null)
                {
                    // Cria a conta corrente
                    contaCorrente = new ContaCorrenteInfo();

                    // Se ainda não carregou..
                    if (contextoOMS == null)
                    {
                        // Pede o usuario
                        if (usuarioInfo != null)
                            usuarioInfo =
                                servicoSeguranca.ReceberUsuario(
                                    new ReceberUsuarioRequest()
                                    {
                                        CodigoSessao = parametros.CodigoSessao,
                                        CodigoUsuario = parametros.CodigoUsuario
                                    }).Usuario;

                        // Pega o contexto do OMS
                        contextoOMS = usuarioInfo.Complementos.ReceberItem<ContextoOMSInfo>();

                        // Se nao tem contexto é erro
                        if (contextoOMS == null)
                            throw new Exception("ContextoOMS não encontrado para este usuário");
                    }

                    // Salva no contexto
                    contextoOMS.CodigoContaCorrente = contaCorrente.CodigoContaCorrente;
                    contextoOMS.CodigoCBLC = codigoClienteCBLC;
                    contextoOMS.CodigoCBLCInvestimento = codigoClienteCBLCInvestimento;

                    // Sinaliza salvar usuário
                    salvarUsuario = true;
                }

                // Sincroniza conta corrente?
                if (parametros.SincronizarContaCorrente && codigoClienteCBLC != null)
                {
                    // Recebe a conta corrente do sinacor
                    ReceberSaldoContaCorrenteSinacorResponse contaCorrenteSinacorResponse =
                        servicoIntegracaoSinacor.ReceberSaldoContaCorrenteSinacor(
                            new ReceberSaldoContaCorrenteSinacorRequest()
                            {
                                CodigoClienteCBLC = codigoClienteCBLC
                            });

                    // Prepara
                    contaCorrente.SaldoRegularProjetado.Clear();
                    for (int i = 0; i < 4; i++)
                        contaCorrente.SaldoRegularProjetado.Add(0);

                    // Preenche a conta corrente
                    contaCorrente.SaldoRegularAtual = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD0;
                    contaCorrente.SaldoRegularProjetado[0] = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD0;
                    contaCorrente.SaldoRegularProjetado[1] = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD1;
                    contaCorrente.SaldoRegularProjetado[2] = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD2;
                    contaCorrente.SaldoRegularProjetado[3] = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD3;

                    // Sinaliza datas na conta corrente
                    contaCorrente.SaldoRegularDataUltimaMovimentacao = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.DataReferencia;
                    contaCorrente.SaldoRegularDataIntegracao = DateTime.Now;
                }

                // Sincroniza conta investimento?
                if (parametros.SincronizarContaInvestimento && codigoClienteCBLCInvestimento != null)
                {
                    // Recebe a conta corrente do sinacor
                    ReceberSaldoContaCorrenteSinacorResponse contaCorrenteSinacorResponse =
                        servicoIntegracaoSinacor.ReceberSaldoContaCorrenteSinacor(
                            new ReceberSaldoContaCorrenteSinacorRequest()
                            {
                                CodigoClienteCBLC = codigoClienteCBLCInvestimento
                            });

                    // Prepara
                    contaCorrente.SaldoInvestimentoProjetado.Clear();
                    for (int i = 0; i < 4; i++)
                        contaCorrente.SaldoInvestimentoProjetado.Add(0);

                    // Preenche a conta corrente
                    contaCorrente.SaldoInvestimentoAtual = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD0;
                    contaCorrente.SaldoInvestimentoProjetado[0] = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD0;
                    contaCorrente.SaldoInvestimentoProjetado[1] = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD1;
                    contaCorrente.SaldoInvestimentoProjetado[2] = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD2;
                    contaCorrente.SaldoInvestimentoProjetado[3] = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.SaldoD3;

                    // Sinaliza datas na conta corrente
                    contaCorrente.SaldoInvestimentoDataUltimaMovimentacao = contaCorrenteSinacorResponse.SaldoContaCorrenteSinacor.DataReferencia;
                    contaCorrente.SaldoInvestimentoDataIntegracao = DateTime.Now;
                }

                // Deve salvar a conta corrente?
                if (parametros.SalvarEntidades)
                    servicoContaCorrente.SalvarContaCorrente(
                        new SalvarContaCorrenteRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            ContaCorrenteInfo = contaCorrente
                        });

                // Salva usuario
                if (parametros.SalvarEntidades && salvarUsuario)
                    servicoSeguranca.SalvarUsuario(
                        new SalvarUsuarioRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            Usuario = usuarioInfo
                        });

                // Informa conta corrente na resposta
                resposta.ContaCorrente = contaCorrente;
            }
            catch (Exception ex)
            {
                // Faz o log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloIntegracao);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Faz a tradução do código CBLC para o código de usuário.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public TraduzirCodigoCBLCResponse TraduzirCodigoCBLC(TraduzirCodigoCBLCRequest parametros)
        {
            // Inicializa
            IServicoSeguranca servicoSeguranca = Ativador.Get<IServicoSeguranca>();
            
            // Prepara retorno
            TraduzirCodigoCBLCResponse resposta = 
                new TraduzirCodigoCBLCResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Com persistencia de objetos, o jeito para se conseguir traduzir o código CBLC é varrendo a 
            // lista de usuários para ver quem tem esse CBLC.
            // Quando a persistencia for no banco de dados, pode-se fazer apenas uma query
            List<UsuarioInfo> usuarios =
                ((ListarUsuariosResponse)
                    servicoSeguranca.ProcessarMensagem(
                        new ListarUsuariosRequest() 
                        { 
                            CodigoSessao = parametros.CodigoSessao
                        })).Usuarios;
            foreach (UsuarioInfo usuario in usuarios)
            {
                // Pega o contexto do usuario
                ContextoOMSInfo contexto = usuario.Complementos.ReceberItem<ContextoOMSInfo>();

                // Verifica se é o código solicitado
                if (contexto.CodigoCBLC == parametros.CodigoCBLC)
                    resposta.Usuarios.Add(usuario);
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita a inicialização do usuário.
        /// Carrega os códigos CBLC através do CPF/CNPJ, cria custodia, conta corrente e pode sincronizar.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public InicializarUsuarioResponse InicializarUsuario(InicializarUsuarioRequest parametros)
        {
            // Prepara resposta
            InicializarUsuarioResponse resposta = 
                new InicializarUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Inicializa
            IServicoSeguranca servicoSeguranca = Ativador.Get<IServicoSeguranca>();
            IServicoContaCorrente servicoContaCorrente = Ativador.Get<IServicoContaCorrente>();
            IServicoCustodia servicoCustodia = Ativador.Get<IServicoCustodia>();
            IServicoIntegracaoSinacor servicoIntegracaoSinacor = Ativador.Get<IServicoIntegracaoSinacor>();

            // Consegue o usuario
            UsuarioInfo usuario = parametros.Usuario;
            if (usuario == null)
                usuario = 
                    servicoSeguranca.ReceberUsuario(
                        new ReceberUsuarioRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoUsuario = parametros.CodigoUsuario
                        }).Usuario;

            // Pega, ou cria o contexto
            ContextoOMSInfo contextoOMS = usuario.Complementos.ReceberItem<ContextoOMSInfo>();
            if (contextoOMS == null)
            {
                // Cria e associa com o usuario
                contextoOMS = new ContextoOMSInfo();
                contextoOMS.CodigoCBLC = parametros.CodigoCBLC;
                usuario.Complementos.AdicionarItem<ContextoOMSInfo>(contextoOMS);
            }

            // Se nao tem codigo cblc, utiliza o informado
            if (contextoOMS.CodigoCBLC == null)
                contextoOMS.CodigoCBLC = parametros.CodigoCBLC;

            // Pega, ou cria conta corrente
            ContaCorrenteInfo contaCorrente = null;
            string codigoContaCorrente = contextoOMS.CodigoContaCorrente;
            if (codigoContaCorrente == null)
            {
                // Cria a conta corrente
                contaCorrente = new ContaCorrenteInfo();

                // Salva no contexto do usuario
                contextoOMS.CodigoContaCorrente = contaCorrente.CodigoContaCorrente;
            }
            else
            {
                // Lê da persistencia
                contaCorrente =
                    servicoContaCorrente.ReceberContaCorrente(
                        new ReceberContaCorrenteRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoContaCorrente = codigoContaCorrente
                        }).ContaCorrenteInfo;
            }

            // Pega, ou cria custódia
            CustodiaInfo custodia = null;
            string codigoCustodia = contextoOMS.CodigoCustodia;
            if (codigoCustodia == null)
            {
                // Cria a custodia
                custodia = new CustodiaInfo();

                // Salva no contexto do usuario
                contextoOMS.CodigoCustodia = custodia.CodigoCustodia;
            }
            else
            {
                // Lê da persistencia
                custodia =
                    servicoCustodia.ReceberCustodia(
                        new ReceberCustodiaRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoCustodia = codigoCustodia
                        }).CustodiaInfo;
            }

            // Deve inferir conta investimento?
            if (parametros.InferirCBLCInvestimento)
            {
                // Faz o pedido
                List<ClienteCBLCInfo> clientesCBLC =
                    servicoIntegracaoSinacor.ListarCBLCsClienteSinacor(
                        new ListarCBLCsClienteSinacorRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoCBLC = contextoOMS.CodigoCBLC 
                        }).ClientesCBLC;

                // Tenta achar a conta investimento
                if (clientesCBLC.Count > 0)
                {
                    // Consulta
                    ClienteCBLCInfo clienteInvestimento = clientesCBLC.Find(c => c.TipoConta == ClienteCBLCTipoContaEnum.ContaInvestimento);

                    // Se achou salva no contexto
                    if (clienteInvestimento != null)
                        contextoOMS.CodigoCBLCInvestimento = clienteInvestimento.CodigoCBLC;
                }
            }

            // Deve sincronizar conta corrente ou conta investimento?
            if (parametros.SincronizarContaCorrente || parametros.SincronizarContaInvestimento)
                this.SincronizarContaCorrente(
                    new SincronizarContaCorrenteRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        ContaCorrente = contaCorrente,
                        Usuario = usuario,
                        SalvarEntidades = false,
                        SincronizarContaCorrente = parametros.SincronizarContaCorrente,
                        SincronizarContaInvestimento = parametros.SincronizarContaInvestimento
                    });

            // Deve sincronizar custodia?
            if (parametros.SincronizarCustodia)
                this.SincronizarCustodia(
                    new SincronizarCustodiaRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        Custodia = custodia,
                        Usuario = usuario,
                        SalvarEntidades = false
                    });

            // Salva conta corrente
            servicoContaCorrente.SalvarContaCorrente(
                new SalvarContaCorrenteRequest()
                {
                    CodigoSessao = parametros.CodigoSessao,
                    ContaCorrenteInfo = contaCorrente
                });

            // Salva custodia
            servicoCustodia.SalvarCustodia(
                new SalvarCustodiaRequest()
                {
                    CodigoSessao = parametros.CodigoSessao,
                    CustodiaInfo = custodia
                });

            // Salva usuario
            servicoSeguranca.SalvarUsuario(
                new SalvarUsuarioRequest()
                {
                    CodigoSessao = parametros.CodigoSessao,
                    Usuario = usuario
                });

            // Atualiza resposta
            resposta.Usuario = usuario;

            // Retorna
            return resposta;
        }

        #endregion
    }
}

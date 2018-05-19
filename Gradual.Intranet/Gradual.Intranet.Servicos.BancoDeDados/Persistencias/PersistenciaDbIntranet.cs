using System;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Dados.ControleDeOrdens;
using Gradual.Intranet.Contratos.Dados.Portal;
using Gradual.Intranet.Contratos.Dados.Relatorios;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Contratos.Dados.Relatorios.Sistema;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Contratos.Dados.Views;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Cliente;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Monitoramento;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Sistema;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.Intranet.Servicos.RegrasDeNegocio;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados.Fundos;
using Gradual.Intranet.Contratos.Dados.Vendas;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Vendas;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public class PersistenciaDbIntranet : IPersistencia
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<T> ConsultarObjetos<T>(ConsultarObjetosRequest<T> parametros) where T : ICodigoEntidade
        {
            ConsultarObjetosResponse<T> resposta = null;

            Type tipoObjeto = typeof(T);

            if (tipoObjeto == typeof(TipoDePendenciaCadastralInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarTipoPendencia(parametros as ConsultarEntidadeRequest<TipoDePendenciaCadastralInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(AvisoHomeBrokerInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarAvisosHomeBroker(parametros as ConsultarEntidadeRequest<AvisoHomeBrokerInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePendenciaCadastralInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClientePendenciaCadastral(parametros as ConsultarEntidadeRequest<ClientePendenciaCadastralInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteAutorizacaoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarAutorizacoesCadastrais(parametros as ConsultarEntidadeRequest<ClienteAutorizacaoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(TipoTelefoneInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarTipoTelefone(parametros as ConsultarEntidadeRequest<TipoTelefoneInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteBancoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteBanco(parametros as ConsultarEntidadeRequest<ClienteBancoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteProcuradorRepresentanteInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteProcuradorRepresentante(parametros as ConsultarEntidadeRequest<ClienteProcuradorRepresentanteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEmitenteInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteEmitente(parametros as ConsultarEntidadeRequest<ClienteEmitenteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDiretorInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteDiretor(parametros as ConsultarEntidadeRequest<ClienteDiretorInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteControladoraInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteControladora(parametros as ConsultarEntidadeRequest<ClienteControladoraInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(FrasesInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarFrase(parametros as ConsultarEntidadeRequest<FrasesInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(PaisesBlackListInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarPaisesBlackList(parametros as ConsultarEntidadeRequest<PaisesBlackListInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(PessoaExpostaPoliticamenteInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarPessoaExpostaPoliticamente(parametros as ConsultarEntidadeRequest<PessoaExpostaPoliticamenteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(AtividadeIlicitaInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarAtividadesIlicitas(parametros as ConsultarEntidadeRequest<AtividadeIlicitaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ContratoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarContrato(parametros as ConsultarEntidadeRequest<ContratoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteContratoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteContrato(parametros as ConsultarEntidadeRequest<ClienteContratoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSituacaoFinanceiraPatrimonialInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteSituacaoFinanceiraPatrimonial(parametros as ConsultarEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarCliente(parametros as ConsultarEntidadeRequest<ClienteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePassoContaInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarPassoEContasDoCliente(parametros as ConsultarEntidadeRequest<ClientePassoContaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ConfiguracaoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarConfiguracao(parametros as ConsultarEntidadeRequest<ConfiguracaoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePessoaVinculadaInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarPessoaVinculada(parametros as ConsultarEntidadeRequest<ClientePessoaVinculadaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePessoaVinculadaPorClienteInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarPessoaVinculadaPorCliente(parametros as ConsultarEntidadeRequest<ClientePessoaVinculadaPorClienteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteResumidoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteResumido(parametros as ConsultarEntidadeRequest<ClienteResumidoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteResumidoMigracaoInfo))
            {
                resposta = ClienteDbLib.ConsultarClienteResumidoMigracao_ViaIdDoAssessor(parametros as ConsultarEntidadeRequest<ClienteResumidoMigracaoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(SinacorListaInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarListaSinacor(parametros as ConsultarEntidadeRequest<SinacorListaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEnderecoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteEndereco(parametros as ConsultarEntidadeRequest<ClienteEnderecoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(LoginInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarLogin(parametros as ConsultarEntidadeRequest<LoginInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteTelefoneInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteTelefone(parametros as ConsultarEntidadeRequest<ClienteTelefoneInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSuitabilityInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteSuitability(parametros as ConsultarEntidadeRequest<ClienteSuitabilityInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteRenovacaoCadastralInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteRenovacaoCadastral(parametros as ConsultarEntidadeRequest<ClienteRenovacaoCadastralInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(SinacorChaveClienteInfo))
            {
                resposta =
                    new SinacorImportarDbLib().SinacorListarTodos(parametros as ConsultarEntidadeRequest<SinacorChaveClienteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteCadastradoPeriodoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteCadastradoPeriodo(parametros as ConsultarEntidadeRequest<ClienteCadastradoPeriodoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePendenciaCadastralRelInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClientePendenciaCadastralRel(parametros as ConsultarEntidadeRequest<ClientePendenciaCadastralRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSolicitacaoAlteracaoCadastralInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteSolicitacaoAlteracaoCadastralRel(parametros as ConsultarEntidadeRequest<ClienteSolicitacaoAlteracaoCadastralInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteMigradoCorretagemAnualInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteMigradoCorretagemAnual(parametros as ConsultarEntidadeRequest<ClienteMigradoCorretagemAnualInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDistribuicaoRegionalInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteDistribuicaoRegional(parametros as ConsultarEntidadeRequest<ClienteDistribuicaoRegionalInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientesExportadosSinacorInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteExportadoSinacor(parametros as ConsultarEntidadeRequest<ClientesExportadosSinacorInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSemLoginInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteSemLogin(parametros as ConsultarEntidadeRequest<ClienteSemLoginInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSuspeitoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteSuspeito(parametros as ConsultarEntidadeRequest<ClienteSuspeitoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(EmailDisparadoPeriodoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarEmailDisparadoPeriodo(parametros as ConsultarEntidadeRequest<EmailDisparadoPeriodoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(TipoEnderecoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarTipoEndereco(parametros as ConsultarEntidadeRequest<TipoEnderecoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteContaConsultaInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarCodigoCliente(parametros as ConsultarEntidadeRequest<ClienteContaConsultaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteInvestidorNaoResidenteInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteNaoResidente(parametros as ConsultarEntidadeRequest<ClienteInvestidorNaoResidenteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(PendenciaClienteAssessorInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarPendenciaClienteAssessor(parametros as ConsultarEntidadeRequest<PendenciaClienteAssessorInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(SinacorListaComboInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarListaComboSinacor(parametros as ConsultarEntidadeRequest<SinacorListaComboInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ArquivoContratoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarArquivosContratos(parametros as ConsultarEntidadeRequest<ArquivoContratoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSuitabilityEfetuadoInfo))
            {
                resposta =
                    ClienteDbLib.ConsultarClienteSuitabilityEfetuado(parametros as ConsultarEntidadeRequest<ClienteSuitabilityEfetuadoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteClubesInfo))
            {
                resposta = new ClienteClubesDbLib().ConsultarClienteClubes(parametros as ConsultarEntidadeRequest<ClienteClubesInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteFundosInfo))
            {

                resposta = new ClienteRiscoRendaDbLib().ConsultarClientesFundosItau(parametros as ConsultarEntidadeRequest<ClienteFundosInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(FundosInfo))
            {
                resposta = ClienteDbLib.ConsultarFundoTermo(parametros as ConsultarEntidadeRequest<FundosInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteAlteracaoInfo))
            {
                resposta = ClienteDbLib.ConsultarClienteAlteracao(parametros as ConsultarEntidadeRequest<ClienteAlteracaoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEducacionalInfo))
            {
                resposta = ClienteDbLib.ConsultarClienteEducacional(parametros as ConsultarEntidadeRequest<ClienteEducacionalInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteContaInfo))
            {
                resposta = ClienteDbLib.ConsultarClienteConta(parametros as ConsultarEntidadeRequest<ClienteContaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoClientePermissaoRelInfo))
            {
                resposta = new Relatorios.Risco.RiscoRelDbLib().ConsultarRiscoClientePermissao(parametros as ConsultarEntidadeRequest<RiscoClientePermissaoRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoGrupoInfo))
            {
                resposta = new Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco.RiscoDbLib().ConsultarRiscoGrupos(parametros as ConsultarEntidadeRequest<RiscoGrupoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoPermissaoInfo))
            {
                resposta = new RiscoDbLib().ConsultarRiscoPermissoes(parametros as ConsultarEntidadeRequest<RiscoPermissaoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoParametrosInfo))
            {
                resposta = new RiscoDbLib().ConsultarRiscoParametros(parametros as ConsultarEntidadeRequest<RiscoParametrosInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoParametrosRelInfo))
            {
                resposta = new RiscoRelDbLib().ConsultarRiscoParametros(parametros as ConsultarEntidadeRequest<RiscoParametrosRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoClienteParametroRelInfo))
            {
                resposta = new RiscoRelDbLib().ConsultarRiscoClienteParametros(parametros as ConsultarEntidadeRequest<RiscoClienteParametroRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoClienteLimiteRelInfo))
            {
                resposta = new RiscoRelDbLib().ConsultarRiscoClienteLimite(parametros as ConsultarEntidadeRequest<RiscoClienteLimiteRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoPermissaoRelInfo))
            {
                resposta = new RiscoRelDbLib().ConsultarRiscoPermissao(parametros as ConsultarEntidadeRequest<RiscoPermissaoRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoClienteLimiteMovimentoRelInfo))
            {
                resposta = new RiscoRelDbLib().ConsultarRiscoClienteLimiteMovimentacao(parametros as ConsultarEntidadeRequest<RiscoClienteLimiteMovimentoRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoClienteBloqueioInstrumentoRelInfo))
            {
                resposta = new RiscoClienteBloqueioInstrumentoRelDbLib().ConsultarClienteBloqueioInstrumento(parametros as ConsultarEntidadeRequest<RiscoClienteBloqueioInstrumentoRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoClienteBloqueioGrupoRelInfo))
            {
                resposta = new RiscoClienteBloqueioGrupoRelDbLib().ConsultarClienteBloqueioGrupo(parametros as ConsultarEntidadeRequest<RiscoClienteBloqueioGrupoRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteInativoInfo))
            {
                resposta = ClienteDbLib.ConsultarClienteInativo(parametros as ConsultarEntidadeRequest<ClienteInativoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoClienteSaldoBloqueadoRelInfo))
            {
                resposta = new RiscoRelDbLib().ConsultarRiscoClienteSaldoBloqueado(parametros as ConsultarEntidadeRequest<RiscoClienteSaldoBloqueadoRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoClienteMercadoVistaOpcaoRelInfo))
            {
                resposta = new RiscoRelDbLib().ConsultarRiscoMercadoVistaOpcao(parametros as ConsultarEntidadeRequest<RiscoClienteMercadoVistaOpcaoRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoLimiteAlocadoInfo))
            {
                ConsultarEntidadeRequest<RiscoLimiteAlocadoInfo> parametro = parametros as ConsultarEntidadeRequest<RiscoLimiteAlocadoInfo>;

                if (parametro.Objeto.NovoOMS)
                {
                    resposta = new RiscoDbLib().ConsultarRiscoLimiteAlocadoPorClienteNovoOMS(parametros as ConsultarEntidadeRequest<RiscoLimiteAlocadoInfo>) as ConsultarObjetosResponse<T>;
                }
                else if (parametro.Objeto.Spider)
                {
                    resposta = new RiscoDbLib().ConsultarRiscoLimiteAlocadoPorClienteSpider(parametros as ConsultarEntidadeRequest<RiscoLimiteAlocadoInfo>) as ConsultarObjetosResponse<T>;
                }
                else
                {
                    resposta = new RiscoDbLib().ConsultarRiscoLimiteAlocadoPorCliente(parametros as ConsultarEntidadeRequest<RiscoLimiteAlocadoInfo>) as ConsultarObjetosResponse<T>;
                }
            }
            else if (tipoObjeto == typeof(ClientePosicaoDeOpcaoRelInfo))
            {
                resposta = new MonitoramentoClientePosicaoDeOpcaoRelDbLib().ConsultarClientePosiçãoDeOpcao(parametros as ConsultarEntidadeRequest<ClientePosicaoDeOpcaoRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePosicaoOpcaoExercidaRelInfo))
            {
                resposta = new MonitoramentoClientePosicaoOpcoesExercidasRelDbLib().ConsultarClientePosicaoOpcoesExercidas(parametros as ConsultarEntidadeRequest<ClientePosicaoOpcaoExercidaRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(UltimasNegociacoesInfo))
            {
                resposta = ClienteDbLib.ConsultarUltimasNegociacoesCliente(parametros as ConsultarEntidadeRequest<UltimasNegociacoesInfo>) as ConsultarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(SistemaControleLogIntranetRelInfo))
            {
                resposta = SistemaRelDbLib.ConsultarControleLogIntranet(parametros as ConsultarEntidadeRequest<SistemaControleLogIntranetRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDirectInfo))
            {
                resposta = ClienteDbLib.ConsultarPlanoClientesFiltrado(parametros as ConsultarEntidadeRequest<ClienteDirectInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteProdutoInfo))
            {
                resposta = ClienteIRLib.ConsultarPlanoClientesFiltrado(parametros as ConsultarEntidadeRequest<ClienteProdutoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(InformeRendimentosInfo))
            {
                resposta = ClienteDbLib.ConsultarInformeRendimentos(parametros as ConsultarEntidadeRequest<InformeRendimentosInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(InformeRendimentosTesouroDiretoInfo))
            {
                resposta = ClienteDbLib.ConsultarInformeRendimentosTesouroDireto(parametros as ConsultarEntidadeRequest<InformeRendimentosTesouroDiretoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(PosicaoConsolidadaPorPapelRelInfo))
            {
                resposta = ClienteDbLib.ConsultarPosicaoConsolidadaPorPapel(parametros as ConsultarEntidadeRequest<PosicaoConsolidadaPorPapelRelInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDocumentacaoEntregueInfo))
            {
                resposta = ClienteDbLib.ConsultarClienteDocumentacaoEntregue(parametros as ConsultarEntidadeRequest<ClienteDocumentacaoEntregueInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ParametroAlavancagemConsultaInfo))
            {
                resposta = new RiscoDbLib().ConsultarClienteParametroAlavancagem(parametros as ConsultarEntidadeRequest<ParametroAlavancagemConsultaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(LTVDoClienteInfo))
            {
                resposta = new LTVDoClienteLTVDoClienteDbLib().ConsultarBDM(parametros as ConsultarEntidadeRequest<LTVDoClienteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoGerenteinfo))
            {
                resposta = new RessumoGerenteDbLib().ConsultarAssessor(parametros as ConsultarEntidadeRequest<ResumoGerenteinfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(DBMClienteInfo))
            {
                resposta = new RessumoGerenteDbLib().ConsultarCliente(parametros as ConsultarEntidadeRequest<DBMClienteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoDoClienteCarteiraInfo))
            {
                resposta = new ResumoDoClienteDbLib().ConsultarCarteira(parametros as ConsultarEntidadeRequest<ResumoDoClienteCarteiraInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoDoAssessorTop10Info))
            {
                resposta = new ResumoDoAssessorDbLib().ConsultarClientesTop10(parametros as ConsultarEntidadeRequest<ResumoDoAssessorTop10Info>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(FilialInfo))
            {
                resposta = new FilialDbLib().ConsultarFilial(parametros as ConsultarEntidadeRequest<FilialInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(FilialAssessorInfo))
            {
                resposta = new FilialDbLib().ConsultarFilial(parametros as ConsultarEntidadeRequest<FilialAssessorInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(TotalClientePorAssessorInfo))
            {
                resposta = new TotalClientePorAssessorDbLib().ConsultarTotalDeAssessorPorCliente(parametros as ConsultarEntidadeRequest<TotalClientePorAssessorInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(SaldoProjecoesEmContaCorrenteInfo))
            {
                resposta = new SaldoProjecoesEmContaCorrenteDbLib().ConsultarSaldoProjecoesEmContaCorrente(parametros as ConsultarEntidadeRequest<SaldoProjecoesEmContaCorrenteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(MovimentoDeContaCorrenteInfo))
            {
                resposta = new MovimentoDeContaCorrenteDbLib().ConsultarMovimentoDeContaCorrente(parametros as ConsultarEntidadeRequest<MovimentoDeContaCorrenteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePlanoPoupeInfo))
            {
                resposta = ClienteDbLib.ConsultarClienteProdutoPoupeRel(parametros as ConsultarEntidadeRequest<ClientePlanoPoupeInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(PoupeDirectProdutoInfo))
            {
                resposta = new PoupeDirectProdutoDbLib().ConsultarPoupeDirectProduto(parametros as ConsultarEntidadeRequest<PoupeDirectProdutoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePorAssessorInfo))
            {
                resposta = new ClientePorAssessorDbLib().ConsultarListaClientePorAssessor(parametros as ConsultarEntidadeRequest<ClientePorAssessorInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(HistoricoSenhaInfo))
            {
                resposta = ClienteDbLib.ConsultarHistoricoSenhaPorCliente(parametros as ConsultarEntidadeRequest<HistoricoSenhaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(VendaDeFerramentaInfo))
            {
                resposta = VendasDbLib.ConsultarVendaDeFerramenta(parametros as ConsultarEntidadeRequest<VendaDeFerramentaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(PagamentoLogInfo))
            {
                resposta = VendasDbLib.ConsultarPagamentoLogDaVenda(parametros as ConsultarEntidadeRequest<PagamentoLogInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(AlteracaoDeVendaInfo))
            {
                resposta = VendasDbLib.ConsultarAlteracoesDaVenda(parametros as ConsultarEntidadeRequest<AlteracaoDeVendaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ProdutoInfo))
            {
                resposta = ProdutosDbLib.ConsultarProdutos(parametros as ConsultarEntidadeRequest<ProdutoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitoramentoRiscoLucroPrejuizoParametrosInfo))
            {
                resposta = MonitoramentoRiscoLucroPrejuizoParametrosDbLib.ConsultarMonitoramentoRiscoLucroPrejuizoJanelas(parametros as ConsultarEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitorRiscoInfo))
            {
                resposta = RiscoMonitorPrejuizoGeralDbLib.ConsultarDadosMonitorRisco(parametros as ConsultarEntidadeRequest<MonitorRiscoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(TotalClienteCadastradoAssessorPeriodoInfo))
            {
                resposta = ClienteDbLib.ConsultarTotalClienteCadastradoAssessorPeriodo(parametros as ConsultarEntidadeRequest<TotalClienteCadastradoAssessorPeriodoInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(RendaFixaInfo))
            {
                resposta = new ClienteRiscoRendaDbLib().ConsultarRendaFixa(parametros as ConsultarEntidadeRequest<RendaFixaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ContratoBmfInfo))
            {
                resposta = new RiscoDbLib().ListarContratosBMF(parametros as ConsultarEntidadeRequest<ContratoBmfInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(IPOClienteInfo))
            {
                resposta = ProdutoIPOClienteDbLib.ConsultarProdutosIPOCliente(parametros as ConsultarEntidadeRequest<IPOClienteInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(IPOInfo))
            {
                resposta = ProdutoIPODbLib.ConsultarProdutosIPO(parametros as  ConsultarEntidadeRequest<IPOInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDeParaInfo))
            {
                resposta = ClienteDeParaDbLib.ConsultarClienteDePara(parametros as ConsultarEntidadeRequest<ClienteDeParaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(CustodiaInfo))
            {
                resposta = CustodiaDbLib.ConsultarCustodia(parametros as ConsultarEntidadeRequest<CustodiaInfo>) as ConsultarObjetosResponse<T>;
            }
            else if (tipoObjeto == typeof(LancamentoTEDInfo))
            {
                resposta = Financeiro.LancamentosTEDDbLib.ConsultarLancamentos(parametros as ConsultarEntidadeRequest<LancamentoTEDInfo>) as ConsultarObjetosResponse<T>;
            }

            return resposta;
        }

        public ReceberObjetoResponse<T> ReceberObjeto<T>(ReceberObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            ReceberObjetoResponse<T> resposta = null;

            Type tipoObjeto = typeof(T);

            if (tipoObjeto == typeof(TipoDePendenciaCadastralInfo))
            {
                resposta =
                    ClienteDbLib.ReceberTipoPendencia(parametros as ReceberEntidadeRequest<TipoDePendenciaCadastralInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(AvisoHomeBrokerInfo))
            {
                resposta =
                    ClienteDbLib.ReceberAvisoHomebroker(parametros as ReceberEntidadeRequest<AvisoHomeBrokerInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePendenciaCadastralInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClientePendenciaCadastral(parametros as ReceberEntidadeRequest<ClientePendenciaCadastralInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(TipoTelefoneInfo))
            {
                resposta =
                    ClienteDbLib.ReceberTipoTelefone(parametros as ReceberEntidadeRequest<TipoTelefoneInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteBancoInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteBanco(parametros as ReceberEntidadeRequest<ClienteBancoInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteContaInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteConta(parametros as ReceberEntidadeRequest<ClienteContaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteProcuradorRepresentanteInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteProcuradorRepresentante(parametros as ReceberEntidadeRequest<ClienteProcuradorRepresentanteInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEmitenteInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteEmitente(parametros as ReceberEntidadeRequest<ClienteEmitenteInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDiretorInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteDiretor(parametros as ReceberEntidadeRequest<ClienteDiretorInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteControladoraInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteControladora(parametros as ReceberEntidadeRequest<ClienteControladoraInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(FrasesInfo))
            {
                resposta =
                    ClienteDbLib.ReceberFrase(parametros as ReceberEntidadeRequest<FrasesInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(PaisesBlackListInfo))
            {
                resposta =
                    ClienteDbLib.ReceberPaisesBlackList(parametros as ReceberEntidadeRequest<PaisesBlackListInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(PessoaExpostaPoliticamenteInfo))
            {
                resposta =
                    ClienteDbLib.ReceberPessoaExpostaPoliticamente(parametros as ReceberEntidadeRequest<PessoaExpostaPoliticamenteInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(AtividadeIlicitaInfo))
            {
                resposta =
                    ClienteDbLib.ReceberAtividadesIlicitas(parametros as ReceberEntidadeRequest<AtividadeIlicitaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ContratoInfo))
            {
                resposta =
                    ClienteDbLib.ReceberContrato(parametros as ReceberEntidadeRequest<ContratoInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteContratoInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteContrato(parametros as ReceberEntidadeRequest<ClienteContratoInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSituacaoFinanceiraPatrimonialInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteSituacaoFinanceiraPatrimonial(parametros as ReceberEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteInfo))
            {
                resposta =
                    ClienteDbLib.ReceberCliente(parametros as ReceberEntidadeRequest<ClienteInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ConfiguracaoInfo))
            {
                resposta =
                    ClienteDbLib.ReceberConfiguracaoPorDescricao(parametros as ReceberEntidadeRequest<ConfiguracaoInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteInvestidorNaoResidenteInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteNaoResidente(parametros as ReceberEntidadeRequest<ClienteInvestidorNaoResidenteInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePessoaVinculadaInfo))
            {
                resposta =
                    ClienteDbLib.ReceberPessoaVinculada(parametros as ReceberEntidadeRequest<ClientePessoaVinculadaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEnderecoInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteEndereco(parametros as ReceberEntidadeRequest<ClienteEnderecoInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(LoginInfo))
            {
                resposta =
                    ClienteDbLib.ReceberLogin(parametros as ReceberEntidadeRequest<LoginInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteTelefoneInfo))
            {
                resposta =
                    ClienteDbLib.ReceberClienteTelefone(parametros as ReceberEntidadeRequest<ClienteTelefoneInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(AssessorDoClienteInfo))
            {
                resposta =
                    ClienteDbLib.ReceberAssessorDoCliente(parametros as ReceberEntidadeRequest<AssessorDoClienteInfo>) as ReceberObjetoResponse<T>;
            }
            //else if (tipoObjeto == typeof(FichaPessoaFisicaInfo))
            //{
            //    resposta =
            //        ClienteDbLib.ReceberFichaPessoaFisica(parametros as ReceberEntidadeRequest<FichaPessoaFisicaInfo>) as ReceberObjetoResponse<T>;
            //}
            //else if (tipoObjeto == typeof(FichaPessoaJuridicaInfo))
            //{
            //    resposta =
            //        ClienteDbLib.ReceberFichaPessoaJuridica(parametros as ReceberEntidadeRequest<FichaPessoaJuridicaInfo>) as ReceberObjetoResponse<T>;
            //}
            else if (tipoObjeto == typeof(SinacorClienteInfo))
            {
                ReceberEntidadeRequest<SinacorClienteInfo> p = parametros as ReceberEntidadeRequest<SinacorClienteInfo>;
                SinacorImportarDbLib lSinacor = new SinacorImportarDbLib(p.Objeto.ChaveCliente);
                resposta =
                    lSinacor.SinacorBuscarCliente(parametros as ReceberEntidadeRequest<SinacorClienteInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ListarOrdensInfo))
            {
                throw new Exception("Retirado temporariamente enquando o sistema de ordens é re-desenvolvido");
                //resposta = //--> Apontando para camada de Regra de Negócio(Rn)
                //    ClienteRnLib.ListarOrdens(parametros as ReceberEntidadeRequest<ListarOrdensInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ListarOrdemStartStopInfo))
            {
                throw new Exception("Retirado temporariamente enquando o sistema de ordens é re-desenvolvido");
                //resposta = //--> Apontando para camada de Regra de Negócio(Rn)
                //    ClienteRnLib.ReceberOrdensStartStop(parametros as ReceberEntidadeRequest<ListarOrdemStartStopInfo>) as ReceberObjetoResponse<T>;
            }
            //else if (tipoObjeto == typeof(PendenciaClienteAssessorInfo))
            //{
            //    resposta = //--> Apontando para camada de Regra de Negócio(Rn)
            //        new ClientePendenciaAssessorRnLib().PendenciaClienteAssessorEnviarEmail(parametros as ReceberEntidadeRequest<PendenciaClienteAssessorInfo>) as ReceberObjetoResponse<T>;
            //}
            else if (tipoObjeto == typeof(ArquivoContratoInfo))
            {
                resposta =
                    ClienteDbLib.ReceberArquivosContratos
                    (parametros as ReceberEntidadeRequest<ArquivoContratoInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ViewFichaCadastralCompletaInfo))
            {
                resposta = ClienteDbLib.ReceberViewFichaCadastralCompleta(parametros as ReceberEntidadeRequest<ViewFichaCadastralCompletaInfo>) as ReceberObjetoResponse<T>;
            }
            //else if (tipoObjeto == typeof(ClienteFichaDucPFInfo))
            //{
            //    resposta = //--> Apontando para camada de Regra de Negócio(Rn)
            //        new ReportFichaDuc_PF().GerarFichaDuc(parametros as ReceberEntidadeRequest<ClienteFichaDucPFInfo>) as ReceberObjetoResponse<T>;
            //}
            //else if (tipoObjeto == typeof(ClienteFichaDucPJInfo))
            //{
            //    resposta = //--> Apontando para camada de Regra de Negócio(Rn)
            //        new ReportFichaDuc_PJ().GerarFichaDuc(parametros as ReceberEntidadeRequest<ClienteFichaDucPJInfo>) as ReceberObjetoResponse<T>;
            //}
            else if (tipoObjeto == typeof(FichaCadastralInfo))
            {
                resposta = ClienteDbLib.ReceberFichaCadastral(parametros as ReceberEntidadeRequest<FichaCadastralInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteAtivoInativoInfo))
            {
                resposta = ClienteDbLib.ReceberClienteAtivoInativo(parametros as ReceberEntidadeRequest<ClienteAtivoInativoInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(PrimeiroAcessoValidaInfo))
            {
                resposta = ClienteDbLib.ReceberValidacaoPrimeiroAcesso(parametros as ReceberEntidadeRequest<PrimeiroAcessoValidaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(PrimeiroAcessoValidaCpfInfo))
            {
                resposta = ClienteDbLib.ReceberValidacaoCpfCnpjPrimeiroAcesso(parametros as ReceberEntidadeRequest<PrimeiroAcessoValidaCpfInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(VerificaNomeInfo))
            {
                resposta = ClienteDbLib.ReceberNome(parametros as ReceberEntidadeRequest<VerificaNomeInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(EsqueciSenhaInfo))
            {
                resposta = ClienteDbLib.ReceberEsqueciSenha(parametros as ReceberEntidadeRequest<EsqueciSenhaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(EsqueciAssinaturaEletronicaInfo))
            {
                resposta = ClienteDbLib.ReceberEsqueciAssinaturaEletronica(parametros as ReceberEntidadeRequest<EsqueciAssinaturaEletronicaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(SolicitacaoNovaSenhaInfo))
            {
                resposta = ClienteDbLib.ReceberSolicitacaoNovaSenha(parametros as ReceberEntidadeRequest<SolicitacaoNovaSenhaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(EfetuarLoginInfo))
            {
                resposta = ClienteDbLib.ReceberLogin(parametros as ReceberEntidadeRequest<EfetuarLoginInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(SessaoPortalInfo))
            {
                resposta = ClienteDbLib.ReceberSessaoPortal(parametros as ReceberEntidadeRequest<SessaoPortalInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MensagemAjudaInfo))
            {
                resposta = ClienteDbLib.ReceberMensagemPortal(parametros as ReceberEntidadeRequest<MensagemAjudaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteAlteracaoInfo))
            {
                resposta = ClienteDbLib.ReceberClienteAlteracao(parametros as ReceberEntidadeRequest<ClienteAlteracaoInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteAtivarInativarInfo))
            {
                resposta = ClienteDbLib.ReceberClienteAtivarInativar(parametros as ReceberEntidadeRequest<ClienteAtivarInativarInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(LoginAssinaturaEletronicaInfo))
            {
                resposta = ClienteDbLib.ReceberLoginAssinaturaEletronica(parametros as ReceberEntidadeRequest<LoginAssinaturaEletronicaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(AssessorSinacorInfo))
            {
                resposta = ClienteDbLib.ReceberAssessorSinacor(parametros as ReceberEntidadeRequest<AssessorSinacorInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ListaEmailAssessorInfo))
            {
                resposta = ClienteDbLib.ConsultarListaEmailAssessor(parametros as ReceberEntidadeRequest<ListaEmailAssessorInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteNaoOperaPorContaPropriaInfo))
            {
                resposta = ClienteDbLib.ConsultarClienteNaoOperaPorContaPropria(parametros as ReceberEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteRenovacaoCadastralInfo))
            {
                resposta = ClienteDbLib.ReceberClienteRenovacaoCadastral(parametros as ReceberEntidadeRequest<ClienteRenovacaoCadastralInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDocumentacaoEntregueInfo))
            {
                resposta = ClienteDbLib.ReceberClienteDocumentacaoEntregue(parametros as ReceberEntidadeRequest<ClienteDocumentacaoEntregueInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoDoClienteDadosCadastraisInfo))
            {
                resposta = new ResumoDoClienteDbLib().ReceberDadosCadastrais(parametros as ReceberEntidadeRequest<ResumoDoClienteDadosCadastraisInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoGerenteinfo))
            {
                resposta = new RessumoGerenteDbLib().ReceberDadosMesAtual(parametros as ReceberEntidadeRequest<ResumoGerenteinfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoGerenteMesAnteriorInfo))
            {
                resposta = new RessumoGerenteDbLib().ReceberDadosMesAnterior(parametros as ReceberEntidadeRequest<ResumoGerenteMesAnteriorInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoGerentePeriodoinfo))
            {
                resposta = new RessumoGerenteDbLib().ReceberDadosPorPeriodo(parametros as ReceberEntidadeRequest<ResumoGerentePeriodoinfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoGerenteClienteInfo))
            {
                resposta = new RessumoGerenteDbLib().ReceberDadosClientes(parametros as ReceberEntidadeRequest<ResumoGerenteClienteInfo>) as ReceberObjetoResponse<T>;
            }

            else if (tipoObjeto == typeof(ResumoDoClienteCorretagemInfo))
            {
                resposta = new ResumoDoClienteDbLib().ReceberCorretagem(parametros as ReceberEntidadeRequest<ResumoDoClienteCorretagemInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoDoAssessorCadastroInfo))
            {
                resposta = new ResumoDoAssessorDbLib().ReceberCadastro(parametros as ReceberEntidadeRequest<ResumoDoAssessorCadastroInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoDoAssessorReceitaInfo))
            {
                resposta = new ResumoDoAssessorDbLib().ReceberReceita(parametros as ReceberEntidadeRequest<ResumoDoAssessorReceitaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoDoAssessorCanalInfo))
            {
                resposta = new ResumoDoAssessorDbLib().ReceberCanal(parametros as ReceberEntidadeRequest<ResumoDoAssessorCanalInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ResumoDoAssessorMetricasInfo))
            {
                resposta = new ResumoDoAssessorDbLib().ReceberMetricas(parametros as ReceberEntidadeRequest<ResumoDoAssessorMetricasInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(AssessorFilialInfo))
            {
                resposta = ClienteDbLib.ReceberIdFilialDeAssessor(parametros as ReceberEntidadeRequest<AssessorFilialInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ListaAssessoresVinculadosInfo))
            {
                resposta = ClienteDbLib.ReceberListaCodigoAssessoresVinculados(parametros as ReceberEntidadeRequest<ListaAssessoresVinculadosInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEnderecoDeCustodiaInfo))
            {
                resposta = ClienteDbLib.ReceberClienteEnderecoDeCustodia(parametros as ReceberEntidadeRequest<ClienteEnderecoDeCustodiaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(LoginReceberQuantidadeTentativasErradasInfo))
            {
                resposta = ClienteDbLib.ReceberQuantidadeTentativasLoginErradas(parametros as ReceberEntidadeRequest<LoginReceberQuantidadeTentativasErradasInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitoramentoRiscoLucroPrejuizoParametrosInfo))
            {
                resposta = MonitoramentoRiscoLucroPrejuizoParametrosDbLib.ReceberMonitoramentoRiscoLucroPrejuizoJanelas(parametros as ReceberEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitoramentoRiscoLucroCustodiaInfo))
            {
                resposta = MonitoramentoRiscoLucroCustodiaDbLib.ConsultarCustodiaNormal(parametros as ReceberEntidadeRequest<MonitoramentoRiscoLucroCustodiaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitorRiscoGarantiaBMFInfo))
            {
                resposta = MonitoramentoRiscoLucroCustodiaDbLib.ConsultarFinanceiroGarantiaBMF(parametros as ReceberEntidadeRequest<MonitorRiscoGarantiaBMFInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitorRiscoGarantiaBovespaInfo))
            {
                resposta = MonitoramentoRiscoLucroCustodiaDbLib.ConsultarFinanceiroGarantiaBovespa(parametros as ReceberEntidadeRequest<MonitorRiscoGarantiaBovespaInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitorRiscoFeriadosInfo))
            {
                resposta = RiscoMonitorPrejuizoGeralDbLib.ReceberFeriados(parametros as ReceberEntidadeRequest<MonitorRiscoFeriadosInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo))
            {
                resposta = MonitoramentoRiscoLucroCustodiaDbLib.ConsultarCustodiaPosicao(parametros as ReceberEntidadeRequest<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitoramentoRiscoLucroTaxaPTAXInfo))
            {
                resposta = MonitoramentoRiscoLucroCustodiaDbLib.ObteCotacaoPtax() as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitoramentoRiscoLucroVencimentosDI))
            {
                resposta = MonitoramentoRiscoLucroCustodiaDbLib.ObterVencimentosDI() as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitorRiscoGarantiaBMFOuroInfo))
            {
                resposta = MonitoramentoRiscoLucroCustodiaDbLib.ReceberFinanceiroGarantiaBMFOuro(parametros as ReceberEntidadeRequest<MonitorRiscoGarantiaBMFOuroInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteCustodiaFinanceiroInfo))
            {
                resposta = ClienteDbLib.ReceberClienteCustodiaFinanceiro(parametros as ReceberEntidadeRequest<ClienteCustodiaFinanceiroInfo>) as ReceberObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteAutorizacaoInfo))
            {
                resposta = ClienteDbLib.ReceberAutorizacoesCadastrais(parametros as ReceberEntidadeRequest<ClienteAutorizacaoInfo>) as ReceberObjetoResponse<T>;
            }
            return resposta;
        }

        public RemoverObjetoResponse<T> RemoverObjeto<T>(RemoverObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            RemoverObjetoResponse<T> resposta = null;

            Type tipoObjeto = typeof(T);

            if (tipoObjeto == typeof(TipoDePendenciaCadastralInfo))
            {
                resposta =
                    ClienteDbLib.RemoverTipoPendencia(parametros as RemoverEntidadeRequest<TipoDePendenciaCadastralInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePendenciaCadastralInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClientePendenciaCadastral(parametros as RemoverEntidadeRequest<ClientePendenciaCadastralInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(TipoTelefoneInfo))
            {
                resposta =
                    ClienteDbLib.RemoverTipoTelefone(parametros as RemoverEntidadeRequest<TipoTelefoneInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteBancoInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteBanco(parametros as RemoverEntidadeRequest<ClienteBancoInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteProcuradorRepresentanteInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteProcuradorRepresentante(parametros as RemoverEntidadeRequest<ClienteProcuradorRepresentanteInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEmitenteInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteEmitente(parametros as RemoverEntidadeRequest<ClienteEmitenteInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDiretorInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteDiretor
                    (parametros as RemoverEntidadeRequest<ClienteDiretorInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteControladoraInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteControladora
                    (parametros as RemoverEntidadeRequest<ClienteControladoraInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(FrasesInfo))
            {
                resposta =
                    ClienteDbLib.RemoverFrase
                    (parametros as RemoverEntidadeRequest<FrasesInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(PaisesBlackListInfo))
            {
                resposta =
                    ClienteDbLib.RemoverPaisesBlackList
                    (parametros as RemoverEntidadeRequest<PaisesBlackListInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(AtividadeIlicitaInfo))
            {
                resposta =
                    ClienteDbLib.RemoverAtividadesIlicitas
                    (parametros as RemoverEntidadeRequest<AtividadeIlicitaInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ContratoInfo))
            {
                resposta =
                    ClienteDbLib.RemoverContrato(parametros as RemoverEntidadeRequest<ContratoInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteContratoInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteContrato(parametros as RemoverEntidadeRequest<ClienteContratoInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSituacaoFinanceiraPatrimonialInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteSituacaoFinanceiraPatrimonial(parametros as RemoverEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteInfo))
            {
                resposta =
                    ClienteDbLib.RemoverCliente
                    (parametros as RemoverEntidadeRequest<ClienteInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ConfiguracaoInfo))
            {
                resposta =
                    ClienteDbLib.RemoverConfiguracao
                    (parametros as RemoverEntidadeRequest<ConfiguracaoInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteInvestidorNaoResidenteInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteInvestidorNaoResidente
                    (parametros as RemoverEntidadeRequest<ClienteInvestidorNaoResidenteInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePessoaVinculadaInfo))
            {
                resposta =
                    ClienteDbLib.RemoverPessoaVinculada
                    (parametros as RemoverEntidadeRequest<ClientePessoaVinculadaInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEnderecoInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteEndereco
                    (parametros as RemoverEntidadeRequest<ClienteEnderecoInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(LoginInfo))
            {
                resposta =
                    ClienteDbLib.RemoverLogin
                    (parametros as RemoverEntidadeRequest<LoginInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteTelefoneInfo))
            {
                resposta =
                    ClienteDbLib.RemoverClienteTelefone
                    (parametros as RemoverEntidadeRequest<ClienteTelefoneInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(CancelarOrdemInfo))
            {
                throw new Exception("Retirado temporariamente enquando o sistema de ordens é re-desenvolvido");
                //resposta =
                //    ClienteRnLib.CancelarOrdem
                //    (parametros as RemoverEntidadeRequest<CancelarOrdemInfo>) as RemoverObjetoResponse<T>;
            } 
            else if (tipoObjeto == typeof(ArquivoContratoInfo))
            {
                resposta =
                    ClienteDbLib.RemoverArquivosContratos
                    (parametros as RemoverEntidadeRequest<ArquivoContratoInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteNaoOperaPorContaPropriaInfo))
            {
                resposta =
                    ClienteDbLib.ExcluirClienteNaoOperaPorContaPropria
                    (parametros as RemoverEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDocumentacaoEntregueInfo))
            {
                resposta = ClienteDbLib.RemoverClienteDocumentacaoEntregue(parametros as RemoverEntidadeRequest<ClienteDocumentacaoEntregueInfo>) as RemoverObjetoResponse<T>;
            } 
            else if (tipoObjeto == typeof(ParametroAlavancagemConsultaInfo))
            {
                resposta = new RiscoDbLib().ExcluirClienteParametro(parametros as RemoverEntidadeRequest<ParametroAlavancagemConsultaInfo>) as RemoverObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitoramentoRiscoLucroPrejuizoParametrosInfo))
            {
                resposta = MonitoramentoRiscoLucroPrejuizoParametrosDbLib.RemoverMonitoramentoRiscoLucroPrejuizoJanelas(parametros as RemoverEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>) as RemoverObjetoResponse<T>;
            }

            return resposta;
        }

        public SalvarObjetoResponse<T> SalvarObjeto<T>(SalvarObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            SalvarObjetoResponse<T> resposta = null;

            Type tipoObjeto = typeof(T);

            if (tipoObjeto == typeof(TipoDePendenciaCadastralInfo))
            {
                resposta =
                    ClienteDbLib.SalvarTipoPendencia(parametros as SalvarObjetoRequest<TipoDePendenciaCadastralInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(AvisoHomeBrokerInfo))
            {
                resposta =
                    ClienteDbLib.SalvarAvisoHomebroker(parametros as SalvarObjetoRequest<AvisoHomeBrokerInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDesejaAplicarInfo))
            {
                resposta =
                    ClienteDbLib.AtualizarDesejaAplicar(parametros as SalvarObjetoRequest<ClienteDesejaAplicarInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteAutorizacaoInfo))
            {
                resposta =
                    ClienteDbLib.SalvarAutorizacaoCadastral(parametros as SalvarObjetoRequest<ClienteAutorizacaoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePendenciaCadastralInfo))
            {
                resposta =
                    ClienteDbLib.SalvarClientePendenciaCadastral(parametros as SalvarObjetoRequest<ClientePendenciaCadastralInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(TipoTelefoneInfo))
            {
                resposta =
                    ClienteDbLib.SalvarTipoTelefone(parametros as SalvarObjetoRequest<TipoTelefoneInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteBancoInfo))
            {
                resposta =
                    ClienteDbLib.SalvarClienteBanco(parametros as SalvarObjetoRequest<ClienteBancoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteProcuradorRepresentanteInfo))
            {
                resposta =
                    ClienteDbLib.SalvarClienteProcuradorRepresentante(parametros as SalvarObjetoRequest<ClienteProcuradorRepresentanteInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEmitenteInfo))
            {
                resposta =
                    ClienteDbLib.SalvarClienteEmitente(parametros as SalvarObjetoRequest<ClienteEmitenteInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDiretorInfo))
            {
                resposta =
                    ClienteDbLib.SalvarClienteDiretor(parametros as SalvarObjetoRequest<ClienteDiretorInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteControladoraInfo))
            {
                resposta =
                    ClienteDbLib.SalvarClienteControladora(parametros as SalvarObjetoRequest<ClienteControladoraInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(FrasesInfo))
            {
                resposta =
                    ClienteDbLib.SalvarFrase(parametros as SalvarObjetoRequest<FrasesInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(PaisesBlackListInfo))
            {
                resposta =
                    ClienteDbLib.SalvarPaisesBlackList(parametros as SalvarObjetoRequest<PaisesBlackListInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(PessoaExpostaPoliticamenteImportacaoInfo))
            {
                resposta =
                    ClienteDbLib.SalvarPessoaExpostaPoliticamente(parametros as SalvarObjetoRequest<PessoaExpostaPoliticamenteImportacaoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(AtividadeIlicitaInfo))
            {
                resposta =
                    ClienteDbLib.SalvarAtividadesIlicitas(parametros as SalvarObjetoRequest<AtividadeIlicitaInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ContratoInfo))
            {
                resposta =
                    ClienteDbLib.SalvarContrato(parametros as SalvarObjetoRequest<ContratoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteContratoInfo))
            {
                resposta =
                    ClienteDbLib.SalvarClienteContrato(parametros as SalvarObjetoRequest<ClienteContratoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSituacaoFinanceiraPatrimonialInfo))
            {
                resposta =
                    ClienteDbLib.SalvarClienteSituacaoFinanceiraPatrimonial(parametros as SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteInfo))
            {
                resposta =
                    ClienteDbLib.SalvarCliente(parametros as SalvarObjetoRequest<ClienteInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ConfiguracaoInfo))
            {
                resposta =
                    ClienteDbLib.SalvarConfiguracao(parametros as SalvarObjetoRequest<ConfiguracaoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteInvestidorNaoResidenteInfo))
            {
                resposta =
                   ClienteDbLib.SalvarClienteInvestidorNaoResidente(parametros as SalvarObjetoRequest<ClienteInvestidorNaoResidenteInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClientePessoaVinculadaInfo))
            {
                resposta =
                   ClienteDbLib.SalvarPessoaVinculada(parametros as SalvarObjetoRequest<ClientePessoaVinculadaInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(LoginInfo))
            {
                resposta =
                   ClienteDbLib.SalvarLogin(parametros as SalvarObjetoRequest<LoginInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteEnderecoInfo))
            {
                resposta =
                   ClienteDbLib.SalvarClienteEndereco(parametros as SalvarObjetoRequest<ClienteEnderecoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteTelefoneInfo))
            {
                resposta =
               ClienteDbLib.SalvarClienteTelefone(parametros as SalvarObjetoRequest<ClienteTelefoneInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteSuitabilityInfo))
            {
                resposta =
               ClienteDbLib.SalvarClienteSuitability(parametros as SalvarObjetoRequest<ClienteSuitabilityInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ProdutoInfo))
            {
                resposta = ProdutosDbLib.Salvar(parametros as SalvarObjetoRequest<ProdutoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(MigracaoClienteAssessorInfo))
            {
                resposta = 
                    ClienteDbLib.MigracaoClienteAssessor(parametros as SalvarObjetoRequest<MigracaoClienteAssessorInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteRenovacaoCadastralInfo))
            {
                resposta =
                   ClienteDbLib.AtualizarDataValidadeClienteRenovacaoCadastral(parametros as SalvarObjetoRequest<ClienteRenovacaoCadastralInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(SinacorClienteInfo))
            {
                var lSinacor = new SinacorImportarDbLib();
                resposta =
                    lSinacor.SinacorSalvarCliente(parametros as SalvarObjetoRequest<SinacorClienteInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(CancelarOrdemStartStopInfo))
            {
                throw new Exception("Retirado temporariamente enquando o sistema de ordens é re-desenvolvido");
                //resposta =
                //   ClienteRnLib.CancelarOrdemStopStart(parametros as SalvarObjetoRequest<CancelarOrdemStartStopInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(SinacorExportarInfo))
            {
                SinacorExportarDbLib lSinacor = new SinacorExportarDbLib();
                
                resposta =
                    lSinacor.SinacorExportarCliente(parametros as SalvarObjetoRequest<SinacorExportarInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ArquivoContratoInfo))
            {
                ArquivoContratoInfo lArquivoContratoInfo = new ArquivoContratoInfo();
                resposta =
                    ClienteDbLib.SalvarArquivosContratos(parametros as SalvarObjetoRequest<ArquivoContratoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(EmailDisparadoPeriodoInfo))
            {
                resposta =
                    ClienteDbLib.SalvarEmailDisparadoPeriodoInfo(parametros as SalvarObjetoRequest<EmailDisparadoPeriodoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(LogEmailInfo))
            {
                resposta =
                    new GravacaoLog().GravarLogEmail(parametros as SalvarObjetoRequest<LogEmailInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteAtivoInativoInfo))
            {
                resposta = ClienteDbLib.SalvarClienteAtivoInativo(parametros as SalvarObjetoRequest<ClienteAtivoInativoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(PrimeiroAcessoAtualizaInfo))
            {
                resposta = ClienteDbLib.AtualizarPrimeiroAcesso(parametros as SalvarObjetoRequest<PrimeiroAcessoAtualizaInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(AlterarSenhaInfo))
            {
                resposta = ClienteDbLib.AtualizarSenha(parametros as SalvarObjetoRequest<AlterarSenhaInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(AlterarSenhaDinamicaInfo))
            {
                resposta = ClienteDbLib.AtualizarSenhaDinamica(parametros as SalvarObjetoRequest<AlterarSenhaDinamicaInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(AlterarAssinaturaEletronicaInfo))
            {
                resposta = ClienteDbLib.AtualizarAssinaturaEletronica(parametros as SalvarObjetoRequest<AlterarAssinaturaEletronicaInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(AlterarAssinaturaEletronicaDinamicaInfo))
            {
                resposta = ClienteDbLib.AtualizarAssinaturaEletronicaDinamica(parametros as SalvarObjetoRequest<AlterarAssinaturaEletronicaDinamicaInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(Passo1Info))
            {
                resposta = ClienteDbLib.SalvarPasso1(parametros as SalvarObjetoRequest<Passo1Info>) as SalvarEntidadeResponse<T>;
            } // 
            else if (tipoObjeto == typeof(ClienteAlteracaoInfo))
            {
                resposta = ClienteDbLib.SalvarClienteAlteracao(parametros as SalvarObjetoRequest<ClienteAlteracaoInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(RiscoClienteLimiteRelInfo))
            {
                resposta = new RiscoRelDbLib().AtualizarDataValidadeParametro(parametros as SalvarObjetoRequest<RiscoClienteLimiteRelInfo>) as SalvarObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteAtivarInativarInfo))
            {
                resposta = ClienteDbLib.SalvarClienteAtivarInativar(parametros as SalvarObjetoRequest<ClienteAtivarInativarInfo>) as SalvarObjetoResponse<T>;
            } 
            else if (tipoObjeto == typeof(ReservaIPOInfo))
            {
                resposta = ClienteDbLib.EfetuarLogDeReservaDeIPO(parametros as SalvarObjetoRequest<ReservaIPOInfo>) as SalvarObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(LogIntranetInfo))
            {
                resposta = ClienteDbLib.RegistrarLog(parametros as SalvarObjetoRequest<LogIntranetInfo>) as SalvarObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteNaoOperaPorContaPropriaInfo))
            {
                resposta = ClienteDbLib.SalvarClienteNaoOperaPorContaPropria(parametros as SalvarObjetoRequest<ClienteNaoOperaPorContaPropriaInfo>) as SalvarObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(ClienteDocumentacaoEntregueInfo))
            {
                resposta = ClienteDbLib.SalvarClienteDocumentacaoEntregue(parametros as SalvarObjetoRequest<ClienteDocumentacaoEntregueInfo>) as SalvarEntidadeResponse<T>;
            } 
            else if (tipoObjeto == typeof(RiscoClienteGrupoInfo))
            {
                resposta = RiscoDbLib.SalvarClienteGrupo(parametros as SalvarObjetoRequest<RiscoClienteGrupoInfo>) as SalvarEntidadeResponse<T>;
            } 
            else if (tipoObjeto == typeof(LoginIncrementarTentativasErradasInfo))
            {
                resposta = ClienteDbLib.IncrementarTentativasErradasLogin(parametros as SalvarObjetoRequest<LoginIncrementarTentativasErradasInfo>) as SalvarEntidadeResponse<T>;
            } 
            else if (tipoObjeto == typeof(LoginLiberarAcesoTentativasErradasInfo))
            {
                resposta = ClienteDbLib.LiberarAcesoTentativasErradasLogin(parametros as SalvarObjetoRequest<LoginLiberarAcesoTentativasErradasInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(HistoricoSenhaInfo))
            {
                resposta = ClienteDbLib.SalvarHistoricoSenha(parametros as SalvarObjetoRequest<HistoricoSenhaInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(AlteracaoDeVendaInfo))
            {
                resposta = VendasDbLib.SalvarAlteracaoDeVenda(parametros as SalvarObjetoRequest<AlteracaoDeVendaInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(MonitoramentoRiscoLucroPrejuizoParametrosInfo))
            {
                resposta = MonitoramentoRiscoLucroPrejuizoParametrosDbLib.SalvarMonitoramentoRiscoLucroPrejuizoJanelas(parametros as SalvarObjetoRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>) as SalvarEntidadeResponse<T>;
            }
            else if (tipoObjeto == typeof(FundosInfo))
            {
                resposta = ClienteDbLib.SalvarFundoTermo(parametros as SalvarObjetoRequest<FundosInfo>) as SalvarObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(IPOClienteInfo))
            {
                resposta = ProdutoIPOClienteDbLib.Salvar(parametros as SalvarObjetoRequest<IPOClienteInfo>) as SalvarObjetoResponse<T>;
            }
            else if (tipoObjeto == typeof(IPOInfo))
            {
                resposta = ProdutoIPODbLib.Salvar(parametros as SalvarObjetoRequest<IPOInfo>) as SalvarObjetoResponse<T>;
            }

            return resposta;
        }

        #endregion

        #region | Métodos Apoio

        ListarTiposResponse IPersistencia.ListarTipos(ListarTiposRequest parametros)
        {
            throw new NotImplementedException();
        }

        void Gradual.OMS.Library.Servicos.IServicoControlavel.IniciarServico()
        {

        }

        void Gradual.OMS.Library.Servicos.IServicoControlavel.PararServico()
        {

        }

        Gradual.OMS.Library.Servicos.ServicoStatus Gradual.OMS.Library.Servicos.IServicoControlavel.ReceberStatusServico()
        {
            return Gradual.OMS.Library.Servicos.ServicoStatus.Indefinido;
        }

        #endregion

        #region | IPersistencia Members

        public AtualizarMetadadosResponse AtualizarMetadados(AtualizarMetadadosRequest parametros)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

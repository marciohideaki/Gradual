using System.ServiceModel;
using Gradual.Site.DbLib.Mensagens;
using System;
using Gradual.Site.DbLib.Dados;
using System.Collections.Generic;

namespace Gradual.Site.DbLib
{
    
    public interface IServicoPersistenciaSite
    {
        [OperationContract]
        ConteudoResponse SelecionarConteudo(ConteudoRequest pRequest);

        [OperationContract]
        ConteudoResponse SelecionarConteudoPorPropriedade(ConteudoRequest pRequest);

        [OperationContract]
        WidgetResponse SelecionarWdiget(WidgetRequest pRequest);

        [OperationContract]
        EstruturaResponse SelecionarEstrutura(EstruturaRequest pRequest);

        [OperationContract]
        PaginaResponse SelecionarPagina(PaginaRequest pRequest);

        [OperationContract]
        PaginaResponse BuscarPaginasEVersoes(PaginaRequest pRequest);

        [OperationContract]
        PaginaResponse InserirPagina(PaginaRequest pRequest);

        [OperationContract]
        WidgetResponse InserirWidget(WidgetRequest pRequest);

        [OperationContract]
        PaginaResponse ExcluirPagina(PaginaRequest pRequest);

        [OperationContract]
        PaginaResponse SelecionarPaginas(PaginaRequest pRequest);

        [OperationContract]
        PaginaConteudoResponse SelecionarPaginaConteudo(PaginaConteudoRequest pRequest);

        [OperationContract]
        FichaPerfilResponse SelecionarFichaPerfil(FichaPerfilRequest pParametro);

        [OperationContract]
        TipoConteudoResponse SelecionarTipoConteudo(TipoConteudoRequest pRequest);

        [OperationContract]
        ConteudoResponse ApagarConteudo(ConteudoRequest pRequest);

        [OperationContract]
        TipoConteudoResponse ApagarTipoConteudo(TipoConteudoRequest pRequest);

        [OperationContract]
        ListaConteudoResponse SelecionarListaConteudo(ListaConteudoRequest pRequest);
        
        [OperationContract]
        ConteudoResponse InserirConteudo(ConteudoRequest pRequest);

        [OperationContract]
        ListaConteudoResponse InserirListaConteudo(ListaConteudoRequest pRequest);

        [OperationContract]
        ListaConteudoResponse ApagarListaConteudo(ListaConteudoRequest pRequest);

        [OperationContract]
        BuscarItensDaListaResponse BuscarItensDaLista(BuscarItensDaListaRequest pRequest);

        [OperationContract]
        BuscarItensDaListaResponse BuscarBannersLaterais(BuscarItensDaListaRequest pRequest);

        [OperationContract]
        WidgetResponse ApagarWidget(WidgetRequest pRequest);
        
        [OperationContract]
        AtualizarOrdemDoWidgetNaPaginaResponse AtualizarOrdemDoWidgetNaPagina(AtualizarOrdemDoWidgetNaPaginaRequest pRequest);

        [OperationContract]
        DateTime SelecionaUltimoDiaUtil();
        
        [OperationContract]
        ClubeResponse SelecionarClube(ClubeRequest pRequest);

        [OperationContract]
        ClubeResponse SelecionarExtratoClube(ClubeRequest pRequest);

        [OperationContract]
        ClubeResponse SelecionarPosicaoClube(ClubeRequest pRequest);

        [OperationContract]
        FundoResponse SelecionarFundo(FundoRequest pRequest);

        [OperationContract]
        FundoResponse SelecionarFundoItau(FundoRequest pRequest);

        [OperationContract]
        ClienteSinacorResponse BuscaInformacoesClienteSinacor(ClienteSinacorRequest pRequest);

        [OperationContract]
        ContaBancariaResponse BuscarContasBancariasDoCliente(ContaBancariaRequest pRequest);

        [OperationContract]
        ContaCorrenteResponse ObterSaldoContaCorrente(ContaCorrenteRequest pRequest);

        [OperationContract]
        decimal ObterSaldoAbertura(Int32 pRequest);

        [OperationContract]
        DateTime ObterDataPregao(Int32 pDias);

        [OperationContract]
        System.Collections.Generic.List<Gradual.Site.DbLib.Dados.CustodiaBTC> ObterBTC(Int32 pRequest);

        [OperationContract]
        System.Collections.Generic.List<Gradual.Site.DbLib.Dados.Garantia> ObterGarantias(Int32 pRequest);

        [OperationContract]
        System.Collections.Generic.List<Provento> ObterGarantiasDividendos(Int32 pRequest);

        [OperationContract]
        System.Collections.Generic.List<GarantiaBMF> ObterGarantiasBMF(Int32 pRequest);

        [OperationContract]
        System.Collections.Generic.List<Gradual.Site.DbLib.Dados.CustodiaTermo> ObterTermo(Int32 pRequest);

        [OperationContract]
        System.Collections.Generic.List<Gradual.Site.DbLib.Dados.CustodiaTermo> ObterTermoALiquidar(Int32 pRequest);

        [OperationContract]
        System.Collections.Generic.List<Gradual.Site.DbLib.Dados.CustodiaTesouro> ObterTesouroDireto(Int32 pRequest);

        [OperationContract]
        DateTime? ObterDataPosicaoFundo(String pCodigoAnbima, decimal pCota);

        [OperationContract]
        System.Collections.Generic.List<ChamadaMargem> ObterChamadaMargem(Int32 pRequest);

        [OperationContract]
        System.Collections.Generic.List<Provento> ObterProventos(Int32 pRequest);

        [OperationContract]
        System.Collections.Generic.List<ResgateFundo> ObterResgateFundo(Int32 pRequest);

        [OperationContract]
        DateTime? ObterDataProcessamentoFundo(Int32 pRequest);

        [OperationContract]
        List<PosicaoFundo> ObterPosicaoFundo(Int32 pRequest);

        [OperationContract]
        CustodiaResponse ObterPosicaoAtual(int CBLC);

        [OperationContract]
        CustodiaResponse ObterPosicaoAtualBMF(int CBLC);

        [OperationContract]
        UltimasNegociacoesResponse ConsultarUltimasNegociacoesCliente(UltimasNegociacoesRequest pRequest);
        
        [OperationContract]
        FundoResponse SelecionaFundoPorCliente(FundoRequest pRequest);

        [OperationContract]
        FundoResponse SelecionaFundoPorCotasClientes(FundoRequest pRequest);

        [OperationContract]
        AtivoResponse ListarAtivo(AtivoRequest pRequest);

        [OperationContract]
        IntegracaoIRResponse IncluirIRBovespaSimples(IntegracaoIRRequest pRequest);

        [OperationContract]
        IntegracaoIRResponse IncluirIRBovespaBMFSimples(IntegracaoIRRequest pRequest);

        [OperationContract]
        IntegracaoIRResponse IncluirIRRetrocedente(IntegracaoIRRequest pRequest);

        [OperationContract]
        IntegracaoIRResponse IncluirIRRetrocedenteBMF(IntegracaoIRRequest pRequest);

        [OperationContract]
        IntegracaoIRResponse SelecionarIntegracaoIR(int pCodigoCliente);

        [OperationContract]
        InformeRendimentosResponse GetRendimento(InformeRendimentosRequest pRequest);

        [OperationContract]
        InformeRendimentosTesouroResponse GetRendimentoTesouroDireto(InformeRendimentosTesouroRequest pRequest);

        [OperationContract]
        SinacorEnderecoResponse GetEnderecoSinacorCustodia(SinacorEnderecoRequest pRequest);

        [OperationContract]
        SinacorEnderecoResponse GetEnderecoSinacorCorrespondencia(SinacorEnderecoRequest pRequest);

        [OperationContract]
        EstruturaResponse CopiarEstrutra(EstruturaRequest pRequest);



        [OperationContract]
        BuscarDadosDosProdutosResponse BuscarDadosDosProdutos(BuscarDadosDosProdutosRequest pRequest);

        [OperationContract]
        InserirLogDePagamentoResponse InserirLogDePagamento(InserirLogDePagamentoRequest pRequest);

        [OperationContract]
        InserirPagamentoResponse InserirPagamento(InserirPagamentoRequest pRequest);

        [OperationContract]
        InserirTransacaoResponse InserirTransacao(InserirTransacaoRequest pRequest);

        [OperationContract]
        InserirProdutoPorTransacaoResponse InserirProdutoPorTransacao(InserirProdutoPorTransacaoRequest pRequest);

        [OperationContract]
        InserirVendaResponse InserirVenda(InserirVendaRequest pRequest);

        [OperationContract]
        BuscarComprasDoClienteResponse BuscarComprasDoCliente(BuscarComprasDoClienteRequest pRequest);

        [OperationContract]
        BuscarProdutosDoClienteResponse BuscarProdutosDoCliente(BuscarProdutosDoClienteRequest pRequest);

        [OperationContract]
        InserirLogRocketResponse InserirLogRocket(InserirLogRocketRequest pRequest);


        [OperationContract]
        BuscarHtmlDaPaginaResponse BuscarHtmlPagina(BuscarHtmlDaPaginaRequest pRequest);

        [OperationContract]
        BuscarNasPaginasResponse BuscarNasPaginas(BuscarNasPaginasRequest pRequest);

        [OperationContract]
        BuscarVersoesResponse BuscarVersoes(BuscarVersoesRequest pRequest);

        [OperationContract]
        VersaoResponse IncluirVersao(VersaoRequest pRequest);

        [OperationContract]
        VersaoResponse PublicarVersao(VersaoRequest pRequest);

        [OperationContract]
        PaginaResponse SelecionarPaginaCompleta(PaginaRequest pRequest);

        [OperationContract]
        void LimparCache(Nullable<int> pIdPagina);

        [OperationContract]
        LojaResponse BuscarLojas(LojaRequest pParametro);
    }
}

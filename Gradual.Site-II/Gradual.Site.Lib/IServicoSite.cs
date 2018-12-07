using System.ServiceModel;
using Gradual.Site.Lib.Mensagens;
using System;

namespace Gradual.Site.Lib
{
    
    public interface IServicoPersistenciaSite
    {
        [OperationContract]
        ConteudoResponse SelecionarConteudo(ConteudoRequest pRequest);

        [OperationContract]
        WidgetResponse SelecionarWdiget(WidgetRequest pRequest);

        [OperationContract]
        EstruturaResponse SelecionarEstrutura(EstruturaRequest pRequest);

        [OperationContract]
        PaginaResponse SelecionarPagina(PaginaRequest pRequest);
        
        [OperationContract]
        PaginaResponse InserirPagina(PaginaRequest pRequest);

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
        ContaBancariaResponse BuscarContasBancariasDoCliente(ContaBancariaRequest pRequest);

        [OperationContract]
        ContaCorrenteResponse ObterSaldoContaCorrente(ContaCorrenteRequest pRequest);

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
        EstruturaResponse CopiarEstrutra(int pIDPagina, int pTipoUsuarioAtual, int pTipoUsuarioNovo);



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
    }
}

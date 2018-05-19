using System.ServiceModel;
using Gradual.OMS.TesouroDireto.AcessoWS;
using Gradual.OMS.TesouroDireto.Lib;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Compra;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Consultas;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Venda;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.CadastroInvestidor;

namespace Gradual.OMS.TesouroDireto
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoTesouroDireto : IServicoTesouroDireto
    {
        public ServicoTesouroDireto()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region | Consulta Tesouro

        public ConsultasConsultaMercadoResponse ConsultarMercado(ConsultasConsultaMercadoRequest pParametro)
        {
            return new TesouroDiretoConsulta().ConsultarMercado(pParametro);
        }

        public ConsultasConsultaTipoTituloResponse ConsultarTipoTitulo(ConsultasConsultaTipoTituloRequest pParametro)
        {
            return new TesouroDiretoConsulta().ConsultarTipoTitulo(pParametro);
        }

        public ConsultasConsultaTipoIndexadorResponse ConsultarTipoIndexador(ConsultasConsultaTipoIndexadorRequest pParametro)
        {
            return new TesouroDiretoConsulta().ConsultarTipoIndexador(pParametro);
        }

        public ConsultasConsultaExtratoMensalResponse ConsultarExtratoMensal(ConsultasConsultaExtratoMensalRequest pParametro)
        {
            return new TesouroDiretoConsulta().ConsultarExtratoMensal(pParametro);
        }

        public ConsultasConsultaCestaResponse ConsultarCesta(ConsultasConsultaCestaRequest pParametro)
        {
            return new TesouroDiretoConsulta().ConsultarCesta(pParametro);
        }

        public ConsultasConsultaTaxaProtocoloResponse ConsultarProtocolo(ConsultasConsultaTaxaProtocoloRequest pParametro)
        {
            return new TesouroDiretoConsulta().ConsultarProtocolo(pParametro);
        }

        #endregion

        #region | Compra Tesouro

        public CompraVerificacaoDeCondicaoDeCompraResponse CompraVerificarCondicaoDeCompra(CompraVerificacaoDeCondicaoDeCompraRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraVerificarCondicaoDeCompra(pParametro);
        }

        public CompraConsultaTituloMercadoResponse CompraConsultarTituloMercado(CompraConsultaTituloMercadoRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraConsultarTituloMercado(pParametro);
        }

        public CompraConsultaCestaResponse CompraConsultarCesta(CompraConsultaCestaRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraConsultarCesta(pParametro);
        }

        public CompraInsereNovaCestaResponse CompraInserirNovaCesta(CompraInsereNovaCestaRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraInserirNovaCesta(pParametro);
        }

        public CompraInsereItemNaCestaResponse CompraInserirItemNaCesta(CompraInsereItemNaCestaRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraInserirItemNaCesta(pParametro);
        }

        public CompraConsultaCestaItensResponse CompraConsultarCestaItens(CompraConsultaCestaItensRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraConsultarCestaItens(pParametro);
        }

        public CompraCalculaTaxaWSResponse CompraCalcularTaxaWs(CompraCalculaTaxaWSRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraCalcularTaxaWs(pParametro);
        }

        public CompraVerificaTituloNoMercadoResponse CompraVerificarTituloNoMercado(CompraVerificaTituloNoMercadoRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraVerificarTituloNoMercado(pParametro);
        }

        public CompraFecharCestaResponse CompraFecharCesta(CompraFecharCestaRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraFecharCesta(pParametro);
        }

        public CompraExcluirCestaResponse CompraExcluirCesta(CompraExcluirCestaRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraExcluirCesta(pParametro);
        }

        public CompraExcluirItemDaCestaResponse CompraExcluirItemCesta(CompraExcluirItemDaCestaRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraExcluirItemCesta(pParametro);
        }

        public CompraAlteraItemDaCestaResponse CompraAlterarItemCesta(CompraAlteraItemDaCestaRequest pParametro)
        {
            return new TesouroDiretoCompra().CompraAlterarItemCesta(pParametro);
        }

        #endregion

        #region | Compra Venda

        public VendaVerificaCondicaoDeVendaResponse VendaVerificarCondicao(VendaVerificaCondicaoDeVendaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaVerificarCondicao(pParametro);
        }

        public VendaVerificaTituloMercadoResponse VendaVerificarTituloMercado(VendaVerificaTituloMercadoRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaVerificarTituloMercado(pParametro);
        }

        public VendaConsultaTituloDeVendaResponse VendaConsultarTitulo(VendaConsultaTituloDeVendaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaConsultarTitulo(pParametro);
        }

        public VendaConsultaValidadeDeTaxaProvisoriaResponse VendaConsultarValidadeTaxaProvisoria(VendaConsultaValidadeDeTaxaProvisoriaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaConsultarValidadeTaxaProvisoria(pParametro);
        }

        public VendaConsultaCestaResponse VendaConsultarCesta(VendaConsultaCestaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaConsultarCesta(pParametro);
        }

        public VendaConsultaCestaItensResponse VendaConsultarItensCesta(VendaConsultaCestaItensRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaConsultarItensCesta(pParametro);
        }

        public VendaInsereNovaCestaResponse VendaInserirCesta(VendaInsereNovaCestaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaInserirCesta(pParametro);
        }

        public VendaInsereItemNaCestaResponse VendaInserirItensCesta(VendaInscereItemNaCestaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaInserirItensCesta(pParametro);
        }

        public VendaAlteraItemDaCestaResponse VendaAlterarItensCesta(VendaAlteraItemDaCestaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaAlterarItensCesta(pParametro);
        }

        public VendaFechaCestaResponse VendaFecharCesta(VendaFechaCestaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaFecharCesta(pParametro);
        }

        public VendaExcluiCestaResponse VendaExcluirCesta(VendaExcluiItemCestaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaExcluirCesta(pParametro);
        }

        public VendaExcluiItemCestaResponse VendaExcluirItemCesta(VendaExcluiItemCestaRequest pParametro)
        {
            return new TesouroDiretoVenda().VendaExcluirItemCesta(pParametro);
        }

        #endregion

        #region | Cadastro Investidor

        public HabilitarInvestidorResponse HabilitarInvestidor(HabilitarInvestidorRequest pParametros)
        {
            return new CadastroInvestidor().HabilitarInvestidor(pParametros);
        }

        public IncluirInvestidorResponse IncluirInvestidor(IncluirInvestidorRequest pParametro)
        {
            return new CadastroInvestidor().IncluirInvestidor(pParametro);
        }

        #endregion
    }
}

using System.Collections.Generic;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using System;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente
{
    public class TransporteRelatorio_019
    {
        #region | Propriedades

        public CabecalhoRelatorio Cabecalho { get; set; }

        public List<DetalhesRelatorio> ListaConteudoRelatorio { get; set; }

        #endregion

        #region | Estruturas

        public struct CabecalhoRelatorio
        {
            public string DadosCliente { get; set; }
        }

        public struct DetalhesRelatorio
        {
            public string Mercado { get; set; }

            public string ISIN { get; set; }

            public string Codigo { get; set; }

            public string Empresa { get; set; }

            public string Carteira { get; set; }

            public string Origem { get; set; }

            public string Abertura { get; set; }

            public string Vencimento { get; set; }

            public string QtAbertura { get; set; }

            public string QtCompra { get; set; }

            public string QtVenda { get; set; }

            public string QtAtual { get; set; }

            public string Medio { get; set; }

            public string Remuneracao { get; set; }

            public string FechamentoAnterior { get; set; }

            public string Cotacao { get; set; }

            public string Variacao { get; set; }

            public string Liquido { get; set; }

            public string Tomador { get; set; }

            public string ValorAtual { get; set; }

            public string QtdD1 { get; set; }

            public string QtdD2 { get; set; }

            public string QtdD3 { get; set; }
        }

        #endregion

        #region | Construtores

        public TransporteRelatorio_019()
        {
            this.Cabecalho = new CabecalhoRelatorio();
            this.ListaConteudoRelatorio = new List<DetalhesRelatorio>();
        }

        #endregion

        #region | Métodos

        public List<TransporteRelatorio_019> TraduzirLista(List<PosicaoCustodiaInfo> pParametros, string pTipoMercado)
        {
            var lRetorno = new List<TransporteRelatorio_019>();
            var lRelatorio = new TransporteRelatorio_019();

            if (null != pParametros && pParametros.Count > 0)
            {
                TransporteMensagemDeNegocio lCotacao = null;

                var lServico = Ativador.Get<IServicoCotacao>();

                foreach (PosicaoCustodiaInfo lZaz in pParametros)
                {
                    lRelatorio = new TransporteRelatorio_019();

                    lZaz.ListaMovimento.ForEach(lPosicao =>
                    {
                        if (string.IsNullOrWhiteSpace(pTipoMercado) || pTipoMercado.DBToString().ToUpper().Equals(lPosicao.TipoMercado.ToUpper()))
                        {
                            if (!"TEDI".Equals(lPosicao.TipoGrupo))
                            {
                                lCotacao = new TransporteMensagemDeNegocio(lServico.ReceberTickerCotacao(lPosicao.CodigoInstrumento));
                            }

                            lRelatorio.ListaConteudoRelatorio.Add(new DetalhesRelatorio()
                            {
                                Abertura = lPosicao.DtAbertura == null ? "n/d" : lPosicao.DtAbertura.Value.ToString("dd/MM/yyyy"),
                                Carteira = lPosicao.DescricaoCarteira.DBToString(),
                                Codigo = lPosicao.CodigoInstrumento,
                                Empresa = lPosicao.NomeEmpresa,
                                ISIN = lPosicao.CdISIN,
                                Liquido = lPosicao.VlLiquido == null ? "n/d" : lPosicao.VlLiquido.Value.ToString("N2"),
                                Medio = lPosicao.VlPrecoMedio == null ? "n/d" : lPosicao.VlPrecoMedio.Value.ToString("N2"),
                                Mercado = lPosicao.TipoMercado == null ? "n/d" : lPosicao.TipoMercado,
                                Origem = lPosicao.DtOrigem == null ? "n/d" : lPosicao.DtOrigem.Value.ToString("dd/MM/yyyy"),
                                QtAbertura = lPosicao.TipoMercado == "BTC" ? "n/d": lPosicao.QtdeDisponivel.ToString("N0"),
                                QtAtual = lPosicao.QtdeAtual.ToString("N0"),
                                QtCompra = lPosicao.TipoMercado == "BTC" ? "n/d" : lPosicao.QtdeAExecCompra.ToString("N0"),
                                QtVenda = lPosicao.TipoMercado == "BTC" ? "n/d" : lPosicao.QtdeAExecVenda.ToString("N0"),
                                Vencimento = lPosicao.DtVencimento == null || DateTime.MinValue.Equals(lPosicao.DtVencimento) ? "n/d" : lPosicao.DtVencimento.Value.ToString("dd/MM/yyyy"),
                                Cotacao = lCotacao == null || lPosicao.TipoMercado == "BTC" ? "n/d" : lCotacao.Preco.DBToDecimal().ToString("N2"),
                                Variacao = lCotacao == null || lPosicao.TipoMercado == "BTC" ? "n/d" : lCotacao.Variacao.DBToDecimal().ToString("N2"),
                                FechamentoAnterior = lCotacao == null || lPosicao.TipoMercado == "BTC" ? "n/d" : lCotacao.ValorFechamento.DBToDecimal().ToString("N2"),
                                Remuneracao = lPosicao.VlTaxaRemuneracao == null ? "n/d" : lPosicao.VlTaxaRemuneracao.Value.ToString("N2"),
                                Tomador = string.IsNullOrWhiteSpace(lPosicao.DsTomador) ? string.Empty : lPosicao.DsTomador.ToUpper().Substring(0, 1),
                                ValorAtual = (lPosicao.QtdeAtual * (lCotacao == null ? 0M : lCotacao.Preco.DBToDecimal())).ToString("N2"),
                                QtdD1 = lPosicao.QtdeD1.ToString("N0"),
                                QtdD2 = lPosicao.QtdeD2.ToString("N0"),
                                QtdD3 = lPosicao.QtdeD3.ToString("N0"),
                            });
                        }
                    });

                    lRelatorio.Cabecalho = new CabecalhoRelatorio() { DadosCliente = string.Format("{0} - {1}", lZaz.CdCodigo, lZaz.DsNomeCliente.ToStringFormatoNome()) };

                    lRetorno.Add(lRelatorio);
                };
            }

            return lRetorno;
        }

        #endregion
    }
}
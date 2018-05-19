using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [DataContract]
    public class MonitorCustodiaInfo
    {
        #region Propriedades
        [DataMember]
        public int? CodigoClienteBov { get; set; }
        [DataMember]
        public int? CodigoClienteBmf { get; set; }
        [DataMember]
        public List<CustodiaPosicao> ListaCustodia { get; set; }
        [DataMember]
        public List<CustodiaPosicaoDiaBMF> ListaPosicaoDiaBMF { get; set; }
        [DataMember]
        public List<CustodiaGarantiaBMF> ListaGarantias { get; set; }
        [DataMember]
        public List<CustodiaGarantiaBMFOuro> ListaGarantiasBMFOuro { get; set; }
        [DataMember]
        public List<CustodiaGarantiaBovespa> ListaGarantiasBovespa { get; set; }
        [DataMember]
        public decimal ValorMargemRequerida { get; set; }
        [DataMember]
        public decimal ValorGarantiaDeposito { get; set; }
        [DataMember]
        public DateTime DataMovimentoGarantia { get; set; }
        [DataMember]
        public decimal ValorMargemRequeridaBovespa { get; set; }
        #endregion

        #region Construtores
        public MonitorCustodiaInfo()
        {
            ListaCustodia = new List<CustodiaPosicao>();
            ListaPosicaoDiaBMF = new List<CustodiaPosicaoDiaBMF>();
            ListaGarantias = new List<CustodiaGarantiaBMF>();
            ListaGarantiasBMFOuro = new List<CustodiaGarantiaBMFOuro>();
            ListaGarantiasBovespa = new List<CustodiaGarantiaBovespa>();
        }
        #endregion

        #region struct Custodia Posicao
        [DataContract]
        public struct CustodiaPosicao
        {
            [DataMember]
            public int IdCliente { get; set; }

            [DataMember]
            public string DsTomador { get; set; }

            [DataMember]
            public string CdISIN { get; set; }

            [DataMember]
            public DateTime? DtOrigem { get; set; }

            [DataMember]
            public DateTime? DtAbertura { get; set; }

            [DataMember]
            public DateTime? DtVencimento { get; set; }

            [DataMember]
            public decimal? VlPrecoMedio { get; set; }

            [DataMember]
            public decimal? VlTaxaRemuneracao { get; set; }

            [DataMember]
            public decimal? VlBruto { get; set; }

            [DataMember]
            public decimal? VlLiquido { get; set; }

            [DataMember]
            public int BloqueadoPorLancamentoOpcao { get; set; }

            [DataMember]
            public int CodigoCarteira { get; set; }

            [DataMember]
            public string CodigoInstrumento { get; set; }

            [DataMember]
            public string DescricaoCarteira { get; set; }

            [DataMember]
            public string NomeEmpresa { get; set; }

            [DataMember]
            public decimal QtdeAExecCompra { get; set; }

            [DataMember]
            public decimal QtdeAExecVenda { get; set; }

            [DataMember]
            public decimal QtdeAtual { get; set; }

            [DataMember]
            public decimal QtdeDisponivel { get; set; }

            [DataMember]
            public decimal QtdeLiquidar { get; set; }

            [DataMember]
            public string TipoGrupo { get; set; }

            [DataMember]
            public string TipoMercado { get; set; }

            [DataMember]
            public decimal ValorPosicao { get; set; }

            [DataMember]
            public decimal QtdeD1 { get; set; }

            [DataMember]
            public decimal QtdeD2 { get; set; }

            [DataMember]
            public decimal QtdeD3 { get; set; }

            [DataMember]
            public decimal QtdeDATotal { get; set; }

            [DataMember]
            public string CodigoSerie { get; set; }

            [DataMember]
            public decimal FatorCotacao { get; set; }

            [DataMember]
            public decimal QtdLiquidar { get; set; }

            [DataMember]
            public decimal Cotacao { get; set; }
        }
        #endregion

        #region  struct Custodia garantia BMF
        [DataContract]
        public struct CustodiaGarantiaBMF
        {
            [DataMember]
            public int? CodigoClienteBmf { get; set; }

            [DataMember]
            public decimal ValorGarantiaDeposito { get; set; }

            [DataMember]
            public string DescricaoGarantia { get; set; }
        }
        #endregion

        #region struct Custodia posicao dia BMF
        [DataContract]
        public struct CustodiaPosicaoDiaBMF
        {
            [DataMember]
            public string DsTomador { get; set; }

            [DataMember]
            public string CdISIN { get; set; }

            [DataMember]
            public DateTime? DtOrigem { get; set; }

            [DataMember]
            public DateTime? DtAbertura { get; set; }

            [DataMember]
            public DateTime? DtVencimento { get; set; }

            [DataMember]
            public decimal? VlPrecoMedio { get; set; }

            [DataMember]
            public decimal? VlTaxaRemuneracao { get; set; }

            [DataMember]
            public decimal? VlBruto { get; set; }

            [DataMember]
            public decimal? VlLiquido { get; set; }

            [DataMember]
            public int BloqueadoPorLancamentoOpcao { get; set; }

            [DataMember]
            public int CodigoCarteira { get; set; }

            [DataMember]
            public string CodigoInstrumento { get; set; }

            [DataMember]
            public string DescricaoCarteira { get; set; }

            [DataMember]
            public int IdCliente { get; set; }

            [DataMember]
            public string NomeEmpresa { get; set; }

            [DataMember]
            public decimal QtdeAExecCompra { get; set; }

            [DataMember]
            public decimal QtdeAExecVenda { get; set; }

            [DataMember]
            public decimal QtdeAtual { get; set; }

            [DataMember]
            public decimal QtdeDisponivel { get; set; }

            [DataMember]
            public string TipoGrupo { get; set; }

            [DataMember]
            public string TipoMercado { get; set; }

            [DataMember]
            public double ValorPosicao { get; set; }

            [DataMember]
            public decimal QtdeDATotal { get; set; }

            [DataMember]
            public string CodigoSerie { get; set; }

            [DataMember]
            public decimal PrecoNegocioCompra { get; set; }

            [DataMember]
            public decimal PrecoNegocioVenda { get; set; }

            [DataMember]
            public string Sentido { get; set; }

            [DataMember]
            public decimal Diferenca { get; set; }

        }
        #endregion

        #region struct Custodia garantia BMF Ouro
        [DataContract]
        public struct CustodiaGarantiaBMFOuro
        {
            [DataMember]
            public int? CodigoClienteBmf { get; set; }

            [DataMember]
            public decimal ValorGarantiaDeposito { get; set; }

            [DataMember]
            public string DescricaoGarantia { get; set; }
        }
        #endregion

        #region struct Custodia Garantia Bovespa
        [DataContract]
        public struct CustodiaGarantiaBovespa
        {
            [DataMember]
            public int? CodigoClienteBov { get; set; }
            [DataMember]
            public decimal ValorGarantiaDeposito { get; set; }
            [DataMember]
            public string DescricaoGarantia { get; set; }
            [DataMember]
            public DateTime DtDeposito { get; set; }
            [DataMember]
            public string FinalidadeGarantia { get; set; }
            [DataMember]
            public string CodigoAtividade { get; set; }
            [DataMember]
            public string CodigoIsin { get; set; }
            [DataMember]
            public int CodigoDistribuicao { get; set; }
            [DataMember]
            public string NomeEmpresa { get; set; }
            [DataMember]
            public int Quantidade { get; set; }
        }
        #endregion
    }
}
